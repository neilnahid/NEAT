using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NEAT.NEAT.Phenotype
{
    class NeuralNetwork
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Connection> Connections { get; set; } = new List<Connection>();
        public NeuralNetwork(Genome genome)
        {
            CreateNodes(genome);
            CreateConnections(genome);
            Nodes.Sort();
        }
        private void CreateConnections(Genome genome)
        {
            genome.ConnectionGenes.ForEach(conn =>
            {
                Connections.Add(new Connection(conn, this));
            });
        }
        private void CreateNodes(Genome genome)
        {
            genome.NeuronGenes.ForEach(neuron =>
            {
                Nodes.Add(new Node(neuron));
            });
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
