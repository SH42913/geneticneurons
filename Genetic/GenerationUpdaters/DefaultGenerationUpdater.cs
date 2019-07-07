using System;
using System.Collections.Generic;
using FarCo.GeneticNeurons.Utils;

namespace FarCo.GeneticNeurons.Genetic.GenerationUpdaters
{
    public class DefaultGenerationUpdater : IGenerationUpdater
    {
        public double CrossoverSwapChance = 0.6;
        public double MutatePopulationPercent = 1.0;
        public double MutationChance = 0.3;
        public double MutationRange = 2.0;

        public int CurrentGeneration { get; private set; }

        private const int MaxBestGenotypes = 3;
        public readonly Genotype[] BestGenotypes = new Genotype[MaxBestGenotypes];
        private readonly Random _random;

        private int _genotypeGenesAmount;
        private int _populationSize;

        public DefaultGenerationUpdater(Random random)
        {
            _random = random;
            CurrentGeneration = 0;
        }

        public void UpdateGeneration(List<Genotype> population, int genotypeGenesAmount, int populationSize)
        {
            _genotypeGenesAmount = genotypeGenesAmount;
            _populationSize = populationSize;

            SelectBestGenotypes(population);
            RecombinateBestGenotypes(population);
            MutatePopulation(population);
        }

        private void SelectBestGenotypes(List<Genotype> population)
        {
            for (int i = 0; i < MaxBestGenotypes; i++)
            {
                BestGenotypes[i] = population[i];
            }
        }

        private void GetTwoGenotypesFromBest(out Genotype genotype1, out Genotype genotype2)
        {
            int index1 = _random.Next(0, BestGenotypes.Length);
            int index2;
            do
            {
                index2 = _random.Next(0, BestGenotypes.Length);
            } while (index1 == index2);

            genotype1 = BestGenotypes[index1];
            genotype2 = BestGenotypes[index2];
        }

        private void RecombinateBestGenotypes(List<Genotype> population)
        {
            population.Clear();
            CurrentGeneration++;
            population.AddRange(BestGenotypes);
            while (population.Count < _populationSize)
            {
                GetTwoGenotypesFromBest(out Genotype parent1, out Genotype parent2);
                CompleteCrossover(parent1, parent2, out Genotype child1, out Genotype child2);

                population.Add(child1);
                if (population.Count < _populationSize)
                {
                    population.Add(child2);
                }
            }
        }

        private void CompleteCrossover(Genotype parent1, Genotype parent2, out Genotype child1, out Genotype child2)
        {
            var off1Parameters = new float[_genotypeGenesAmount];
            var off2Parameters = new float[_genotypeGenesAmount];

            for (int i = 0; i < _genotypeGenesAmount; i++)
            {
                if (_random.Next() < CrossoverSwapChance)
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

            child1 = new Genotype(off1Parameters, CurrentGeneration);
            child2 = new Genotype(off2Parameters, CurrentGeneration);
        }

        private void MutatePopulation(List<Genotype> population)
        {
            int currentGenotype = 1;
            foreach (Genotype genotype in population)
            {
                var populationPercent = (double) currentGenotype++ / population.Count;
                if (populationPercent <= MutatePopulationPercent)
                {
                    for (int i = 0; i < genotype.Genes.Length; i++)
                    {
                        if (_random.NextDouble() < MutationChance)
                        {
                            genotype.Genes[i] += (float) (_random.NextDouble() * (MutationRange * 2) - MutationRange);
                        }
                    }
                }
            }
        }
    }
}