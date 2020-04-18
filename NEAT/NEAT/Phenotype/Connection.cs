using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NEAT.NEAT.Phenotype
{
    public class Connection
    {
        public double Weight { get; set; }
        public Node FromNode { get; set; }
        public Node ToNode { get; set; }
        public Connection(ConnectionGene connection, NeuralNetwork neuralNetwork)
        {
            Weight = connection.Weight;
            FromNode = neuralNetwork.Nodes.Where(node => node.Id == connection.From.InnovationID).First();
            ToNode = neuralNetwork.Nodes.Where(node => node.Id == connection.To.InnovationID).First();
        }
        public override string ToString()
        {
            string str = null;
            str += "FromNodeID: " + FromNode.Id;
            str += "\nToNodeID: " + ToNode.Id;
            str += "\nWeight" + Weight;
            return str;
        }
    }
}
