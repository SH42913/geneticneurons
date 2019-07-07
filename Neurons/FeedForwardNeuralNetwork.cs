using System;
using FarCo.GeneticNeurons.Utils;

namespace FarCo.GeneticNeurons.Neurons
{
    public class FeedForwardNeuralNetwork
    {
        public readonly NeuralLayer[] Layers;
        public readonly int[] Topology;
        public readonly int WeightCount;

        private readonly float[][] _layersOutput;

        public float[] LastOutput
        {
            get { return _layersOutput[Layers.Length - 1]; }
        }

        public FeedForwardNeuralNetwork(INeuronActivator neuronActivator, params int[] topology)
        {
#if DEBUG
            if (topology.Length <= 1)
            {
                throw new Exception("Network should have at least 1 layer");
            }
#endif

            Topology = topology;
            WeightCount = CalculateWeightCount(Topology);
            Layers = new NeuralLayer[Topology.Length - 1];
            _layersOutput = new float[Layers.Length][];
            for (int i = 0; i < Layers.Length; i++)
            {
                int outputCount = Topology[i + 1];
                Layers[i] = new NeuralLayer(Topology[i], outputCount, neuronActivator);
                _layersOutput[i] = new float[outputCount];
            }
        }

        public void Process(float[] inputs)
        {
            Layers[0].Process(inputs, _layersOutput[0]);
            for (int i = 1; i < Layers.Length; i++)
            {
                Layers[i].Process(_layersOutput[i - 1], _layersOutput[i]);
            }
        }

        public void Process(float[] inputs, float[] outputs)
        {
#if DEBUG
            if (outputs.Length != Layers[Layers.Length - 1].OutputCount)
            {
                throw new Exception("Given outputs don't match last layer output count!");
            }
#endif

            Process(inputs);
            for (int i = 0; i < LastOutput.Length; i++)
            {
                outputs[i] = LastOutput[i];
            }
        }

        public void FillLayersWithRandom(Random random, float minValue, float maxValue, bool withBias = true)
        {
            foreach (NeuralLayer layer in Layers)
            {
                for (int inputIndex = 0; inputIndex < layer.InputCount; inputIndex++)
                {
                    random.FillArrayWithRandom(layer.Neurons[inputIndex], minValue, maxValue);
                    if (withBias)
                    {
                        random.FillArrayWithRandom(layer.BiasNode, minValue, maxValue);
                    }
                }
            }
        }

        public void FillLayers(params float[] weights)
        {
            int current = 0;

#if DEBUG
            if (weights.Length != WeightCount)
            {
                throw new Exception($"Weight Array has {weights.Length} weights, " +
                                    $"but Network WeightCount is {WeightCount}");
            }
#endif
            
            foreach (NeuralLayer layer in Layers)
            {
                for (int outputIndex = 0; outputIndex < layer.OutputCount; outputIndex++)
                {
                    for (int inputIndex = 0; inputIndex < layer.InputCount; inputIndex++)
                    {
                        layer.Neurons[inputIndex][outputIndex] = weights[current++];
                    }

                    layer.BiasNode[outputIndex] = weights[current++];
                }
            }
        }

        public static int CalculateWeightCount(params int[] topology)
        {
            int weightCount = 0;
            for (int i = 0; i < topology.Length - 1; i++)
            {
                weightCount += (topology[i] + 1) * topology[i + 1];
            }
            
            return weightCount;
        }
    }
}