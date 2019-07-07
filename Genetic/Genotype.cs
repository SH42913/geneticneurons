using System;

namespace FarCo.GeneticNeurons.Genetic
{
    public class Genotype : IComparable<Genotype>
    {
        public float Fitness;
        public int Generation;
        public readonly float[] Genes;

        public Genotype(float[] genes, int generation = 0)
        {
            if (genes == null)
            {
                throw new Exception("Genes must be not null!");
            }

            Genes = genes;
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