using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace FarCo.GeneticNeurons.Neurons.SaveLoad
{
    public static class NeuralNetworkXmlSaveLoader
    {
        private const string LayerId = "Layer";
        private const string WeightId = "Weight";
        private const string BiasId = "Bias";
        private const string InputCountId = "InputCount";
        private const string OutputCountId = "OutputCount";

        public static void SaveNetworkToFile(this NeuralNetwork network, string path)
        {
            XElement root = new XElement("NeuralNetwork");
            foreach (NeuralLayer layer in network.Layers)
            {
                XElement layerElement = new XElement(
                    LayerId,
                    new XAttribute(BiasId, layer.Bias.ToString(CultureInfo.InvariantCulture)),
                    new XAttribute(InputCountId, layer.InputCount.ToString()),
                    new XAttribute(OutputCountId, layer.OutputCount.ToString()));

                for (int outputIndex = 0; outputIndex < layer.OutputCount; outputIndex++)
                {
                    for (int inputIndex = 0; inputIndex < layer.InputCount; inputIndex++)
                    {
                        layerElement.Add(new XElement(WeightId)
                        {
                            Value = layer.Weights[inputIndex][outputIndex].ToString("G")
                        });
                    }
                }

                root.Add(layerElement);
            }

            new XDocument(root).Save(path);
        }

        public static void LoadWeightsFromFile(this NeuralNetwork network, string path)
        {
            XDocument doc = XDocument.Load(path);
            if (doc.Root == null) return;

            XElement root = doc.Root;
            var weightList = new List<string>(8);
            foreach (XElement layerElement in root.Elements(LayerId))
            {
                foreach (XElement element in layerElement.Elements())
                {
                    weightList.Add(element.Value);
                }
            }

            float[] weights = weightList.Select(float.Parse).ToArray();
            network.FillLayers(weights);
        }

        public static NeuralNetwork CreateNeuralNetworkFromFile(string path, INeuronActivator neuronActivator)
        {
            XDocument doc = XDocument.Load(path);
            if (doc.Root == null) throw new Exception("Document is empty");

            XElement root = doc.Root;
            var biasList = new List<string>(8);
            var weightList = new List<string>(8);
            var inputCountList = new List<string>(8);
            var outputCountList = new List<string>(8);
            foreach (XElement layerElement in root.Elements(LayerId))
            {
                biasList.Add(layerElement.Attribute(BiasId)?.Value);
                inputCountList.Add(layerElement.Attribute(InputCountId)?.Value);
                outputCountList.Add(layerElement.Attribute(OutputCountId)?.Value);
                foreach (XElement element in layerElement.Elements())
                {
                    weightList.Add(element.Value);
                }
            }

            int[] topology = new int[inputCountList.Count + 1];
            for (int i = 0; i < inputCountList.Count; i++)
            {
                topology[i] = int.Parse(inputCountList[i]);
            }

            topology[topology.Length - 1] = int.Parse(outputCountList[outputCountList.Count - 1]);

            float[] weights = weightList.Select(float.Parse).ToArray();
            var network = new NeuralNetwork(neuronActivator, topology);
            network.FillLayers(weights);

            for (int i = 0; i < biasList.Count; i++)
            {
                string biasString = biasList[i];
                if (!string.IsNullOrEmpty(biasString) && float.TryParse(biasString, out float bias))
                {
                    network.Layers[i].Bias = bias;
                }
            }

            return network;
        }
    }
}