using System;
using System.Collections.Generic;

namespace FarCo.GeneticNeurons.Genetic.GenerationUpdaters
{
    public sealed class GenotypeMutator : IGenerationUpdater
    {
        public int GenotypesToSkip = 2;
        public float MutatePopulationPercent = 1f;
        public float MutationChance = 0.3f;
        public float MutationRange = 2f;

        private readonly Random _random;

        public GenotypeMutator(Random random)
        {
            _random = random;
        }

        public void UpdateGeneration(List<Genotype> population, int generation)
        {
            int currentGenotype = 1;
            foreach (Genotype genotype in population)
            {
                double populationPercent = (double) currentGenotype / population.Count;
                if (currentGenotype++ <= GenotypesToSkip || populationPercent > MutatePopulationPercent) continue;

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