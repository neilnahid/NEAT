using System;
using System.Linq;

namespace NEAT.NEAT
{
    public class ConnectionGene
    {
        public NeuronGene From { get; private set; }
        public NeuronGene To { get; private set; }
        public double Weight { get; set; }

        public bool IsEnabled { get; set; }
        public int IsRecurrent { get; set; }
        public int InnovationID { get; private set; }
        public ConnectionGene(NeuronGene from, NeuronGene to, bool isEnabled = true, int innovationID = 0)
        {
            IsEnabled = isEnabled;
            From = from;
            To = to;
            InnovationID = innovationID == 0 ? Neat.LATEST_INNOVATION_NUMBER : innovationID;
        }
        /// <summary>
        /// Copies the connectionGene and updates the values of references type variables to genome object
        /// </summary>
        /// <param name="conn">the connection gene to copy</param>
        /// <param name="genome">the genome object that will be referenced by the copy</param>
        public ConnectionGene(ConnectionGene conn, Genome genome)
        {
            From = genome.NeuronGenes.Where(neuron => neuron.InnovationID == conn.From.InnovationID).First();
            To = genome.NeuronGenes.Where(neuron => neuron.InnovationID == conn.To.InnovationID).First();
            Weight = conn.Weight;
            IsEnabled = conn.IsEnabled;
            IsRecurrent = conn.IsRecurrent;
            InnovationID = InnovationID;
        }
    }
}