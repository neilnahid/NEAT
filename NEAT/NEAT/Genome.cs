using System;
using System.Collections.Generic;
using System.Linq;
using NEAT.ExtensionMethods;
namespace NEAT.NEAT
{
    public class Genome
    {
        public int GenomeID { get; set; }
        public List<NeuronGene> NeuronGenes { get; set; }
        public List<ConnectionGene> ConnectionGenes { get; set; }

        public Neat Neat { get; set; }
        public NeuralNetwork PhenoType { get; set; }
        public double Fitness { get; set; }
        public double AdjustedFitness { get; set; }
        public double AmountToSpawn { get; set; }
        public int Species { get; set; }
        public Genome(int genomeID, int numberOfInputs, int numberOfOutputs, Neat neat)
        {
            GenomeID = genomeID;
            Neat = neat;
        }
        public void Mutate()
        {
            if (new Random().NextDouble() <= Neat.MUTATION_RATE)
            {

            }
        }
        private void MutateAddConnection()
        {
            for (int i = 0; i < 100; i++)
            {
                //select 2 random nodes
                var fromNode = NeuronGenes.Where(neuron => neuron.NeuronType != NeuronType.Output).PickRandomElement();
                var toNode = NeuronGenes.Where(neuron => fromNode.X > neuron.X).PickRandomElement();

                //break if there's no possible NodeB to pick
                if (toNode == null)
                    continue;
                if (ConnectionGenes.Any(conn => conn.From.Equals(fromNode) && conn.To.Equals(toNode)))
                    continue;
                var conn = Neat.GetConnection(fromNode, toNode);
                //add connection between these nodes
                conn.Weight = ((Static.Random.NextDouble() * 2 - 1) * Neat.WEIGHT_RANDOM_STRENGTH);
                ConnectionGenes.Add(conn);
            }
        }
        private void MutateAddNode()
        {
            //select a random connection
            var conn = ConnectionGenes.PickRandomElement();
            var fromNode = NeuronGenes.Where(neuron => neuron.InnovationID == conn.From.InnovationID).First();
            var toNode = NeuronGenes.Where(neuron => neuron.InnovationID == conn.To.InnovationID).First();

            var middle = new NeuronGene(NeuronType.Hidden, (fromNode.X + toNode.X) / 2, (fromNode.Y + toNode.Y) / 2);
        }
        public void MutateShiftWeight()
        {
            var conn = ConnectionGenes[Static.Random.Next(0, ConnectionGenes.Count)];
            conn.Weight += (Static.Random.NextDouble() * 2 - 1) * Neat.WEIGHT_SHIFT_RANDOM;
        }

        /// <summary>
        /// assigns a new random weight to a random connection
        /// 
        /// </summary>
        public void MutateRandomConnectionWeight()
        {
            //randomly selects a connection gene from the list
            var conn = ConnectionGenes[new Random().Next(0, ConnectionGenes.Count)];
            conn.Weight = new Random().NextDouble();
        }
    }
}