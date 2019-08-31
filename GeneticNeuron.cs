using System;
using FarCo.GeneticNeurons.Genetic;
using FarCo.GeneticNeurons.Neurons;

namespace FarCo.GeneticNeurons
{
    public class GeneticNeuron : IComparable<GeneticNeuron>
    {
        public readonly Genotype Genotype;

        public float Fitness
        {
            set { Genotype.Fitness = value; }
            get { return Genotype.Fitness; }
        }

        public float Generation
        {
            get { return Genotype.Generation; }
        }

        public readonly NeuralNetwork NeuralNetwork;

        public GeneticNeuron(Genotype genotype, NeuralNetwork neuralNetwork)
        {
            Genotype = genotype;
            NeuralNetwork = neuralNetwork;
            neuralNetwork.FillLayers(genotype.Genes);
        }

        public void Process(float[] inputs, float[] outputs)
        {
            NeuralNetwork.Process(inputs, outputs);
        }

        public int CompareTo(GeneticNeuron other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return -Fitness.CompareTo(other.Fitness);
        }
    }
}