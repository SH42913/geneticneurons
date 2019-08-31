using System;

namespace FarCo.GeneticNeurons.Neurons
{
    public class NeuralLayer
    {
        public readonly float[][] Weights;

        public float Bias = 0f;
        public readonly int InputCount;
        public readonly int OutputCount;

        private readonly INeuronActivator _neuronActivator;
        private readonly float[] _biasedInputs;

        public NeuralLayer(int inputCount, int outputCount, INeuronActivator neuronActivator = null)
        {
            _neuronActivator = neuronActivator;

            InputCount = inputCount;
            OutputCount = outputCount;

            Weights = new float[InputCount + 1][];
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = new float[OutputCount];
            }

            _biasedInputs = new float[InputCount + 1];
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

            inputs.CopyTo(_biasedInputs, 0);
            _biasedInputs[InputCount] = Bias;

            for (int outputIndex = 0; outputIndex < OutputCount; outputIndex++)
            {
                for (int inputIndex = 0; inputIndex < InputCount; inputIndex++)
                {
                    float result = _biasedInputs[inputIndex] * Weights[inputIndex][outputIndex];
                    if (inputIndex > 0)
                    {
                        result += outputs[outputIndex];
                    }

                    outputs[outputIndex] = result;
                }
            }

            if (_neuronActivator == null) return;
            for (int outputIndex = 0; outputIndex < OutputCount; outputIndex++)
            {
                float nonActive = outputs[outputIndex];
                outputs[outputIndex] = _neuronActivator.ActivateNeuron(nonActive);
            }
        }
    }
}