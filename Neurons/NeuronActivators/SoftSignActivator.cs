using System;

namespace FarCo.GeneticNeurons.Neurons.NeuronActivators
{
    public class SoftSignActivator : INeuronActivator
    {
        public float ActivateNeuron(float neuronValue)
        {
            return neuronValue / (1 + Math.Abs(neuronValue));
        }
    }
}