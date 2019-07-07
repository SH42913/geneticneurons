using System;

namespace FarCo.GeneticNeurons.Neurons.NeuronActivators
{
    public class SigmoidActivator : INeuronActivator
    {
        public float ActivateNeuron(float neuronValue)
        {
            if (neuronValue > 10) return 1f;
            else if (neuronValue < -10) return 0f;
            else return (float) (1.0 / (1.0 + Math.Exp(-neuronValue)));
        }
    }
}