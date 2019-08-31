using System;

namespace FarCo.GeneticNeurons.Genetic
{
    public sealed class Genotype : IComparable<Genotype>
    {
        public float Fitness;
        public int Generation;
        public readonly float[] Genes;

        public Genotype(float[] genes, int generation)
        {
            Genes = genes ?? throw new Exception("Genes must be not null!");
            Fitness = 0;
            Generation = generation;
        }

        public int CompareTo(Genotype other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return -Fitness.CompareTo(other.Fitness);
        }
    }
}