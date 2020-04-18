using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NEAT.NEAT.Phenotype
{
    public class NeuralNetwork
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Connection> Connections { get; set; } = new List<Connection>();
        public List<Node> OutputNodes { get; set; } = new List<Node>();
        public NeuralNetwork(Genome genome)
        {
            CreateNodes(genome);
            CreateConnections(genome);
            ReferenceConnectionsToNodes();
            OutputNodes = Nodes.Where(node => node.NodeType == NeuronType.Output).ToList();
            Nodes.Sort();
        }
        private void CreateConnections(Genome genome)
        {
            genome.ConnectionGenes.ForEach(conn =>
            {
                if (conn.IsEnabled)
                    Connections.Add(new Connection(conn, this));
            });
        }
        private void ReferenceConnectionsToNodes()
        {
            Nodes.ForEach(node =>
            {
                node.ReferenceConnections(Connections.Where(conn => conn.FromNode == node).ToList());
            });
        }
        private void CreateNodes(Genome genome)
        {
            genome.NeuronGenes.ForEach(neuron =>
            {
                Nodes.Add(new Node(neuron));
            });
        }
        public int Compute(double[] inputs)
        {
            FeedInputs(inputs);
            FeedForward();
            NormaliseOutput();
            return GetOutputIndex();
        }
        private void FeedInputs(double[] inputs)
        {
            var inputNodes = Nodes.Where(node => node.NodeType == NeuronType.Input).ToList();
            for (int i = 0; i < inputNodes.Count; i++)
            {
                inputNodes[i].FeedInput(inputs[i]);
            }
        }
        private void FeedForward()
        {
            foreach (var node in Nodes)
            {
                node.FeedForward();
            }
        }
        private void NormaliseOutput()
        {
            var sum = OutputNodes.Select(node => node.Value).Sum();
            OutputNodes.ForEach(node =>
            {
                node.Value = Math.Round(node.Value / sum, 2);
            });
        }
        public int GetOutputIndex()
        {
            return OutputNodes.IndexOf(OutputNodes.OrderByDescending(node => node.Value).First());
        }
        public override string ToString()
        {
            string str = null;
            str += String.Format("Node count: {0}\n", Nodes.Count);
            str += String.Format("Connections count: {0}\n", Connections.Count);
            str += String.Format("Input nodes: {0}", Nodes.Where(node => node.NodeType == NeuronType.Input).Count());
            str += String.Format("hidden nodes: {0}", Nodes.Where(node => node.NodeType == NeuronType.Hidden).Count());
            str += String.Format("output nodes: {0}", Nodes.Where(node => node.NodeType == NeuronType.Output).Count());
            return str;
        }
    }
}
