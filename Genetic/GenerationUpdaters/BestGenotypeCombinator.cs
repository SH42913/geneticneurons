using System;
using System.Collections.Generic;

namespace FarCo.GeneticNeurons.Genetic.GenerationUpdaters
{
    public sealed class BestGenotypeCombinator : IGenerationUpdater
    {
        public bool CopyBestToNewGeneration = true;
        public bool CombineOnlyDifferent = false;
        public float CombineSwapChance = 0.6f;

        private readonly Random _random;
        private readonly Genotype[] _bestGenotypes;

        public BestGenotypeCombinator(Random random, int bestGenotypesCount = 2)
        {
#if DEBUG
            if (bestGenotypesCount < 2)
            {
                throw new Exception("Minimal BestGenotypesCount - 2");
            }
#endif

            _random = random;
            _bestGenotypes = new Genotype[bestGenotypesCount];
        }

        public void UpdateGeneration(List<Genotype> population, int generation)
        {
#if DEBUG
            if (population.Count < _bestGenotypes.Length)
            {
                throw new Exception($"Population must has more than {_bestGenotypes.Length} genotypes");
            }
#endif

            SelectBestGenotypes(population);
            CombineBestGenotypes(population, generation);

            Array.Clear(_bestGenotypes, 0, _bestGenotypes.Length);
        }

        private void SelectBestGenotypes(List<Genotype> population)
        {
            for (int i = 0; i < _bestGenotypes.Length; i++)
            {
                _bestGenotypes[i] = population[i];
            }
        }

        private void GetTwoGenotypesFromBest(out Genotype genotype1, out Genotype genotype2)
        {
            int index1 = _random.Next(0, _bestGenotypes.Length);
            int index2;
            do
            {
                index2 = _random.Next(0, _bestGenotypes.Length);
            } while (CombineOnlyDifferent && index1 == index2);

            genotype1 = _bestGenotypes[index1];
            genotype2 = _bestGenotypes[index2];
        }

        private void CombineBestGenotypes(List<Genotype> population, int generation)
        {
            int populationSize = population.Count;

            population.Clear();
            if (CopyBestToNewGeneration)
            {
                population.AddRange(_bestGenotypes);
            }

            while (population.Count < populationSize)
            {
                GetTwoGenotypesFromBest(out Genotype parent1, out Genotype parent2);
                Combine(parent1, parent2, out Genotype child1, out Genotype child2, generation);

                population.Add(child1);
                if (population.Count < populationSize)
                {
                    population.Add(child2);
                }
            }
        }

        private void Combine(Genotype parent1, Genotype parent2, out Genotype child1, out Genotype child2, int newGen)
        {
            int parentGeneCount = parent1.Genes.Length;
            var off1Parameters = new float[parentGeneCount];
            var off2Parameters = new float[parentGeneCount];

            for (int i = 0; i < parentGeneCount; i++)
            {
                if (_random.Next() < CombineSwapChance)
                {
                    off1Parameters[i] = parent2.Genes[i];
                    off2Parameters[i] = parent1.Genes[i];
                }
                else
                {
                    off1Parameters[i] = parent1.Genes[i];
                    off2Parameters[i] = parent2.Genes[i];
                }
            }

            child1 = new Genotype(off1Parameters, newGen);
            child2 = new Genotype(off2Parameters, newGen);
        }
    }
}