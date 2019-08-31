using System;
using System.Linq;
using FarCo.GeneticNeurons.Genetic;
using FarCo.GeneticNeurons.Neurons;

namespace FarCo.GeneticNeurons
{
    public class GeneticNeuronGenerator
    {
        public readonly EvolutionManager EvolutionManager;
        public GeneticNeuron BestGeneticNeuron { get; private set; }

        private readonly INeuronActivator _neuronActivator;

        private readonly int[] _neuralNetworkTopology;
        private readonly int _genotypeGenesAmount;

        private readonly GeneticNeuron[] _currentPopulation;

        public GeneticNeuronGenerator(
            INeuronActivator neuronActivator,
            int populationSize,
            int[] topology,
            params IGenerationUpdater[] generationUpdaters)
        {
            _neuralNetworkTopology = topology;
            _genotypeGenesAmount = NeuralNetwork.CalculateWeightCount(topology);

            _currentPopulation = new GeneticNeuron[populationSize];
            EvolutionManager = new EvolutionManager(_genotypeGenesAmount, populationSize, generationUpdaters);

            _neuronActivator = neuronActivator;
        }

        public void InitRandomPopulation(Random random, float minValue, float maxValue)
        {
            EvolutionManager.FillPopulationWithRandom(random, minValue, maxValue);
            GeneratePopulation();
        }

        public GeneticNeuron[] GetPopulation()
        {
            var populationCopy = new GeneticNeuron[_currentPopulation.Length];
            _currentPopulation.CopyTo(populationCopy, 0);
            return populationCopy;
        }

        public void UpdateGeneration()
        {
            BestGeneticNeuron = _currentPopulation.Max(neuron => neuron);
            EvolutionManager.UpdateGeneration();
            GeneratePopulation();
        }

        private void GeneratePopulation()
        {
            for (int i = 0; i < _currentPopulation.Length; i++)
            {
                Genotype genotype = EvolutionManager.CurrentPopulation[i];
                var network = new NeuralNetwork(_neuronActivator, _neuralNetworkTopology);
#if DEBUG
                if (_genotypeGenesAmount != genotype.Genes.Length)
                {
                    throw new Exception("The given genotype's parameter count must match " +
                                        "the neural network topology's weight count.");
                }
#endif
                _currentPopulation[i] = new GeneticNeuron(genotype, network);
            }
        }
    }
}