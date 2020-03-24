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
        public static double M_ADD_CONNECTION_RATE = 0.50;
        public static double M_PERTURB_WEIGHT_RATE = 0.03;
        public static double M_RANDOMISE_WEIGHT_RATE = 0.03;
        public static double M_ADD_NODE_RATE = 0.03;
        public static double WEIGHT_RANDOM_STRENGTH = 0.03;
        public static double WEIGHT_SHIFT_RANDOM = 0.03;

        public ConnectionGene GetConnection(NeuronGene from, NeuronGene to)
        {
            var inno = Innovations.Where(inno => inno.FromNeuron.Equals(from) && inno.ToNeuron.Equals(to)).FirstOrDefault();
            if (inno == null)
            {
                inno = new Innovation(InnovationType.NEW_CONNECTION, from.InnovationID, to.InnovationID, Innovations.Count + 1);
                Innovations.Add(inno);
            }
            return new ConnectionGene(from, to, innovationID: inno.InnovationID);

        }
        public ConnectionGene GetConnection()
        {
            return new ConnectionGene(null, null, true, this.Innovations.Count + 1);
        }
        public NeuronGene GetNode(NeuronGene from, NeuronGene to)
        {
            var inno = Innovations.Where(inno => inno.FromNeuron.Equals(from) && inno.ToNeuron.Equals(to)).FirstOrDefault();
            if (inno == null)
            {
                inno = new Innovation(InnovationType.NEW_CONNECTION, from.InnovationID, to.InnovationID, Innovations.Count + 1);
                Innovations.Add(inno);
            }
            return new NeuronGene(NeuronType.Hidden, (from.X + to.X) / 2, (from.Y + to.Y) / 2, innovationID: inno.InnovationID);
        }
        public NeuronGene GetNode(NeuronType type, double x, double y)
        {
            Innovations.Add(new Innovation(InnovationType.NEW_NODE, innovationID: Innovations.Count + 1));
            return new NeuronGene(type, x, y, innovationID: Innovations.Count);
        }
        public NeuronGene GetNode()
        {
            return new NeuronGene(Innovations.Count + 1);
        }
    }
}
