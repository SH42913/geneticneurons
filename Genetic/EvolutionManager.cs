using System;
using System.Collections.Generic;
using FarCo.GeneticNeurons.Utils;

namespace FarCo.GeneticNeurons.Genetic
{
    public class EvolutionManager
    {
        public float InitMinValue = -1f;
        public float InitMaxValue = 1f;

        public readonly int PopulationSize;
        public readonly List<Genotype> CurrentPopulation;
        public Genotype BestGenotype { get; private set; }

        public int GenerationCount
        {
            get { return _generationUpdater.CurrentGeneration; }
        }

        private readonly IGenerationUpdater _generationUpdater;
        private readonly int _genotypeGenesAmount;

        public EvolutionManager(IGenerationUpdater generationUpdater, int genotypeGenesAmount, int populationSize)
        {
            PopulationSize = populationSize;
            _generationUpdater = generationUpdater;
            _genotypeGenesAmount = genotypeGenesAmount;

            CurrentPopulation = new List<Genotype>(PopulationSize);
            for (int i = 0; i < PopulationSize; i++)
            {
                CurrentPopulation.Add(new Genotype(new float[genotypeGenesAmount]));
            }
        }

        public void FillPopulationWithRandom(Random random)
        {
            foreach (Genotype genotype in CurrentPopulation)
            {
                random.FillArrayWithRandom(genotype.Genes, InitMinValue, InitMaxValue);
            }
        }

        public void UpdateGeneration()
        {
            CurrentPopulation.Sort();
            BestGenotype = CurrentPopulation[0];
            _generationUpdater.UpdateGeneration(CurrentPopulation, _genotypeGenesAmount, PopulationSize);
        }
    }
}