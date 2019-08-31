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
        private readonly int _weightCount;

        private readonly GeneticNeuron[] _currentPopulation;

        public GeneticNeuronGenerator(
            INeuronActivator neuronActivator,
            IGenerationUpdater generationUpdater,
            int populationSize,
            params int[] topology)
        {
            _neuralNetworkTopology = topology;
            _weightCount = NeuralNetwork.CalculateWeightCount(topology);

            _currentPopulation = new GeneticNeuron[populationSize];
            EvolutionManager = new EvolutionManager(generationUpdater, _weightCount, populationSize);

            _neuronActivator = neuronActivator;
        }

        public void InitRandomPopulation(Random random)
        {
            EvolutionManager.FillPopulationWithRandom(random);
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
                if (_weightCount != genotype.Genes.Length)
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