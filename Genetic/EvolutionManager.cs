using System;
using System.Collections.Generic;
using FarCo.GeneticNeurons.Utils;

namespace FarCo.GeneticNeurons.Genetic
{
    public sealed class EvolutionManager
    {
        public readonly int PopulationSize;
        public readonly List<Genotype> CurrentPopulation;
        public int GenerationCount { get; private set; }
        public Genotype BestGenotype { get; private set; }

        private readonly IGenerationUpdater[] _updaters;

        public EvolutionManager(int genotypeGenesAmount, int populationSize, params IGenerationUpdater[] updaters)
        {
            PopulationSize = populationSize;
            _updaters = updaters;

            CurrentPopulation = new List<Genotype>(PopulationSize);
            for (int i = 0; i < PopulationSize; i++)
            {
                CurrentPopulation.Add(new Genotype(new float[genotypeGenesAmount], 0));
            }
        }

        public void FillPopulationWithRandom(Random random, float minValue, float maxValue)
        {
            foreach (Genotype genotype in CurrentPopulation)
            {
                random.FillArrayWithRandom(genotype.Genes, minValue, maxValue);
            }
        }

        public void UpdateGeneration()
        {
            CurrentPopulation.Sort();
            BestGenotype = CurrentPopulation[0];

            GenerationCount++;
            foreach (IGenerationUpdater updater in _updaters)
            {
                updater.UpdateGeneration(CurrentPopulation, GenerationCount);
            }
        }
    }
}