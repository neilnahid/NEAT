using System;
using System.Linq;

namespace NEAT.NEAT
{
    [Serializable]
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
            Weight = Static.Random.NextDouble() * 2 - 1;
            InnovationID = innovationID;
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
            InnovationID = conn.InnovationID;
        }
        public override string ToString()
        {
            string str = "";
            str += String.Format("___________________");
            str += String.Format("\n| InnovationID: {0}            |", this.InnovationID);
            str += String.Format("\n| In: {0}            |", this.From.InnovationID);
            str += String.Format("\n| Out: {0}           |", this.To.InnovationID);
            str += String.Format("\n| Weight: {0}     |", Math.Round(this.Weight, 2));
            str += String.Format("\n| Weight: {0}     |", Math.Round(this.Weight, 2));
            str += String.Format("\n|__________________|\n", this.Weight);
            return str;
        }
        public bool Equals(ConnectionGene c)
        {
            return From.Equals(c.From) && To.Equals(c.To) && Weight == c.Weight && IsEnabled == c.IsEnabled && IsRecurrent == c.IsRecurrent && InnovationID == c.InnovationID;
        }

    }
}