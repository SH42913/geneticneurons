using System;
using FarCo.GeneticNeurons.Utils;

namespace FarCo.GeneticNeurons.Neurons
{
    public class NeuralNetwork
    {
        public readonly NeuralLayer[] Layers;
        public readonly int TotalWeightCount;

        private readonly float[][] _layersOutput;

        public float[] LastOutput
        {
            get { return _layersOutput[Layers.Length - 1]; }
        }

        public NeuralNetwork(INeuronActivator neuronActivator, params int[] topology)
        {
#if DEBUG
            if (topology.Length <= 1)
            {
                throw new Exception("Network should have at least 1 layer");
            }
#endif

            TotalWeightCount = CalculateWeightCount(topology);
            Layers = new NeuralLayer[topology.Length - 1];
            _layersOutput = new float[Layers.Length][];
            for (int i = 0; i < Layers.Length; i++)
            {
                int outputCount = topology[i + 1];
                Layers[i] = new NeuralLayer(topology[i], outputCount, neuronActivator);
                _layersOutput[i] = new float[outputCount];
            }
        }

        public void Process(params float[] inputs)
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
            int targetOutputCount = Layers[Layers.Length - 1].OutputCount;
            if (outputs.Length != targetOutputCount)
            {
                throw new Exception("Given output array length doesn't match last layer output count, " +
                                    $"it should contain {targetOutputCount} outputs!");
            }
#endif

            Process(inputs);
            for (int i = 0; i < LastOutput.Length; i++)
            {
                outputs[i] = LastOutput[i];
            }
        }

        public void SetBiasForAllLayers(float bias)
        {
            foreach (NeuralLayer layer in Layers)
            {
                layer.Bias = bias;
            }
        }

        public void FillLayers(params float[] weights)
        {
#if DEBUG
            if (weights.Length != TotalWeightCount)
            {
                throw new Exception($"Weight Array has {weights.Length} weights, " +
                                    $"but Network WeightCount is {TotalWeightCount}");
            }
#endif

            int curWeight = 0;
            foreach (NeuralLayer layer in Layers)
            {
                for (int outputIndex = 0; outputIndex < layer.OutputCount; outputIndex++)
                {
                    for (int inputIndex = 0; inputIndex < layer.InputCount; inputIndex++)
                    {
                        layer.Weights[inputIndex][outputIndex] = weights[curWeight++];
                    }
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