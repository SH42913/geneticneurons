using System.Collections.Generic;

namespace FarCo.GeneticNeurons.Genetic
{
    public interface IGenerationUpdater
    {
        void UpdateGeneration(List<Genotype> population, int generation);
    }
}