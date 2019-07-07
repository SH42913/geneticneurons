using System;

namespace FarCo.GeneticNeurons.Neurons
{
    public class NeuralLayer
    {
        public readonly float[][] Neurons;

        public float BiasInput = 1f;
        public readonly float[] BiasNode;

        public readonly int InputCount;
        public readonly int OutputCount;

        public readonly INeuronActivator NeuronActivator;

        public NeuralLayer(int inputCount, int outputCount, INeuronActivator neuronActivator = null)
        {
            InputCount = inputCount;
            OutputCount = outputCount;
            NeuronActivator = neuronActivator;

            Neurons = new float[InputCount][];
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i] = new float[OutputCount];
            }

            BiasNode = new float[OutputCount];
        }

        public void Process(float[] inputs, float[] outputs)
        {
#if DEBUG
            if (inputs.Length != InputCount)
            {
                throw new Exception("Given inputs don't match layer input count!");
            }

            if (outputs.Length != OutputCount)
            {
                throw new Exception("Given outputs don't match layer output count!");
            }
#endif

            for (int outputIndex = 0; outputIndex < OutputCount; outputIndex++)
            {
                for (int inputIndex = 0; inputIndex < InputCount; inputIndex++)
                {
                    outputs[outputIndex] += inputs[inputIndex] * Neurons[inputIndex][outputIndex];
                }

                outputs[outputIndex] += BiasInput * BiasNode[outputIndex];
                if (NeuronActivator != null)
                {
                    outputs[outputIndex] = NeuronActivator.ActivateNeuron(outputs[outputIndex]);
                }
            }
        }
    }
}