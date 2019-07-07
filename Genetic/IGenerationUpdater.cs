using System.Collections.Generic;

namespace FarCo.GeneticNeurons.Genetic
{
    public interface IGenerationUpdater
    {
        int CurrentGeneration { get; }
        void UpdateGeneration(List<Genotype> population, int genotypeGenesAmount, int populationSize);
    }
}