using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace NEAT.NEAT
{
    public class Neat
    {
        public List<Genome> Genomes { get; set; } = new List<Genome>();
        public List<Innovation> Innovations { get; set; } = new List<Innovation>();
        public static int LATEST_INNOVATION_NUMBER = 0;
        public static int LATEST_NODE_NUMBER = 0;
        public static double MUTATION_RATE = 0.03;
        public static double WEIGHT_RANDOM_STRENGTH = 0.03;
        public static double WEIGHT_SHIFT_RANDOM = 0.03;

        public bool AlreadyExists(int inNodeID, int outNodeID)
        {
            return Innovations.Where(innovation => innovation.FromNeuron == inNodeID && innovation.ToNeuron == outNodeID).Count() > 0;
        }
        public ConnectionGene GetConnection(NeuronGene from, NeuronGene to)
        {
            var inno = Innovations.Where(inno => inno.FromNeuron.Equals(from) && inno.ToNeuron.Equals(to)).FirstOrDefault();
            if (inno == null)
            {
                inno = new Innovation(InnovationType.NEW_CONNECTION, from.InnovationID, to.InnovationID, LATEST_INNOVATION_NUMBER++);
                Innovations.Add(inno);
            }
            return new ConnectionGene(from, to, innovationID: inno.InnovationID);

        }
        public NeuronGene GetNode(NeuronGene from, NeuronGene to)
        {
            var inno = Innovations.Where(inno => inno.FromNeuron.Equals(from) && inno.ToNeuron.Equals(to)).FirstOrDefault();
            if (inno == null)
            {
                inno = new Innovation(InnovationType.NEW_CONNECTION, from.InnovationID, to.InnovationID, LATEST_INNOVATION_NUMBER++);
                Innovations.Add(inno);
            }
            return new NeuronGene(NeuronType.Hidden, (from.X + to.X) / 2, (from.Y + to.Y) / 2, innovationID: inno.InnovationID);
        }
    }
}
