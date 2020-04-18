using System;
using System.Collections.Generic;
using System.Linq;
using NEAT.ExtensionMethods;
using NEAT.NEAT.Phenotype;

namespace NEAT.NEAT
{
    [Serializable]
    public class Genome
    {
        public int GenomeID { get; set; }
        public List<NeuronGene> NeuronGenes { get; set; } = new List<NeuronGene>();
        public List<ConnectionGene> ConnectionGenes { get; set; } = new List<ConnectionGene>();

        public NeuralNetwork NeuralNetwork { get; set; }
        public Neat Neat { get; set; }
        public double Fitness { get; set; }
        public double AdjustedFitness { get; set; }
        public double AmountToSpawn { get; set; }
        public int Species { get; set; }
        public Genome(Genome genome)
        {
            Genome newGenome = (Genome)genome.MemberwiseClone();
            foreach (var node in genome.NeuronGenes)
            {
                NeuronGenes.Add(new NeuronGene(node));
            }
            foreach (var conn in genome.ConnectionGenes)
            {
                ConnectionGenes.Add(new ConnectionGene(conn, this));
            }
        }
        public Genome(int genomeID, int numberOfInputs, int numberOfOutputs, Neat neat)
        {
            Neat = neat;
            GenomeID = genomeID;
            //handle input
            for (int i = 0; i < numberOfInputs; i++)
            {
                this.NeuronGenes.Add(Neat.GetNode(NeuronType.Input, 0, i));
            }
            //handle output
            for (int i = 0; i < numberOfOutputs; i++)
            {
                this.NeuronGenes.Add(Neat.GetNode(NeuronType.Output, 1, i));
            }
            //handle connections
            foreach (var input in NeuronGenes.Where(neuron => neuron.NeuronType == NeuronType.Input))
            {
                foreach (var output in NeuronGenes.Where(neuron => neuron.NeuronType == NeuronType.Output))
                {
                    ConnectionGenes.Add(Neat.GetConnection(input, output));
                }
            }
        }
        public void Mutate()
        {
            if (Static.Random.NextDouble() < Neat.M_ADD_NODE_RATE)
                MutateAddNode();
            if (Static.Random.NextDouble() < Neat.M_ADD_CONNECTION_RATE)
                MutateAddConnection();
            if (Static.Random.NextDouble() < Neat.M_PERTURB_WEIGHT_RATE)
                MutateShiftWeight();
            if (Static.Random.NextDouble() < Neat.M_RANDOMISE_WEIGHT_RATE)
                MutateRandomConnectionWeight();
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

            var middle = Neat.GetNode(fromNode, toNode);
            NeuronGenes.Add(middle);
        }
        private void MutateShiftWeight()
        {
            var conn = ConnectionGenes[Static.Random.Next(0, ConnectionGenes.Count)];
            conn.Weight += (Static.Random.NextDouble() * 2 - 1) * Neat.WEIGHT_SHIFT_RANDOM;
        }

        /// <summary>
        /// assigns a new random weight to a random connection
        /// 
        /// </summary>
        private void MutateRandomConnectionWeight()
        {
            //randomly selects a connection gene from the list
            var conn = ConnectionGenes[new Random().Next(0, ConnectionGenes.Count)];
            conn.Weight = new Random().NextDouble();
        }
        public void Display()
        {
            foreach (var neuron in NeuronGenes)
            {
                Console.WriteLine(neuron.ToString());
            }
            foreach (var conn in ConnectionGenes)
            {
                Console.WriteLine(conn.ToString());
            }
        }
        public bool Equals(Genome genome)
        {
            bool isEquals = false;
            if (genome.ConnectionGenes.Count == ConnectionGenes.Count)
            {
                isEquals = true;
                for (int i = 0; i < ConnectionGenes.Count; i++)
                {
                    if (!ConnectionGenes[i].Equals(genome.ConnectionGenes[i]))
                        return false;
                }
            }
            if (genome.NeuronGenes.Count == NeuronGenes.Count)
            {
                isEquals = true;
                for (int i = 0; i < NeuronGenes.Count; i++)
                {
                    if (!NeuronGenes[i].Equals(genome.NeuronGenes[i]))
                        return false;
                }
            }
            isEquals = Fitness == genome.Fitness && AdjustedFitness == genome.AdjustedFitness && AmountToSpawn == genome.AmountToSpawn && Species == genome.Species && GenomeID == genome.GenomeID;
            return isEquals;
        }
        public void InstantiateNeuralNetwork()
        {
            NeuralNetwork = new NeuralNetwork(this);
        }
    }
}