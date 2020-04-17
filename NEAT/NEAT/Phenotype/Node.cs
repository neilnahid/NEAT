using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using NEAT.NEAT;
namespace NEAT.NEAT.Phenotype
{
    class Node : IComparable<Node>
    {
        public int Id { get; set; }
        public double X { get; set; }//its X coordinate value in the genotype
        public double Value { get; set; }
        public NeuronType NodeType { get; set; }

        public Node(NeuronGene neuron)
        {
            Id = neuron.InnovationID;
            X = neuron.X;
            Value = 0;
            NodeType = neuron.NeuronType;
        }
        public List<Connection> Connections { get; set; } //connections originated from this node

        public int CompareTo([AllowNull] Node other)
        {
            if (X > other.X)
                return 1;
            if (X < other.X)
                return -1;
            return 0;
        }
        /// <summary>
        /// forwards values of this node's connection to the destination node
        /// </summary>
        public void FeedForward()
        {
            ApplySigmoid();
            Connections.ForEach(conn =>
            {
                conn.ToNode.Value += conn.FromNode.Value * conn.Weight;
            });
        }
        public void FeedInput(double val)
        {
            Value = val;
        }
        private double Sigmoid(double val)
        {
            // formula: e^x/e^x+1

            return Math.Pow(Math.E, val) / (Math.Pow(Math.E, val) + 1);
        }
        private void ApplySigmoid()
        {
            Value = Sigmoid(Value);
        }
    }
}
