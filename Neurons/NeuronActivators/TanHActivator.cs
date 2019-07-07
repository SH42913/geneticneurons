using System;

namespace FarCo.GeneticNeurons.Neurons.NeuronActivators
{
    public class TanHActivator : INeuronActivator
    {
        public float ActivateNeuron(float neuronValue)
        {
            if (neuronValue > 10) return 1f;
            else if (neuronValue < -10) return -1f;
            else return (float) Math.Tanh(neuronValue);
        }
    }
}