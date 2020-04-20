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
        public static double C1 = 0.05;
        public static double C2 = 0.05;
        public static double C3 = 0.05;

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
        public double CompatibilityDistance(Genome genome1, Genome genome2)
        {
            List<ConnectionGene> disjointGenes = DisjointGenes(genome1, genome2);
            List<ConnectionGene> excessGenes = ExcessGenes(genome1, genome2);
            int N = genome1.GeneCount > genome2.GeneCount ? genome1.GeneCount : genome2.GeneCount;
            N = N < 20 ? 1 : N;
            double excessFactor = C1 * excessGenes.Count / N;
            double disjointFactor = C2 * disjointGenes.Count / N;
            double weightFactor = C3 * GetAverageWeightDifference(genome1, genome2);
            return excessFactor + disjointFactor + weightFactor;
        }
        private List<ConnectionGene> DisjointGenes(Genome genome1, Genome genome2)
        {
            var lesserInnovationNumber = genome1.HighestInnovationNumber() < genome2.HighestInnovationNumber() ?
                                            genome1.HighestInnovationNumber() : genome2.HighestInnovationNumber();
            return genome1.ConnectionGenes
                .Where(conn => conn.InnovationID <= lesserInnovationNumber)
                .Except(genome2.ConnectionGenes
                            .Where(conn => conn.InnovationID <= lesserInnovationNumber))
                            .ToList();
        }
        private List<ConnectionGene> ExcessGenes(Genome genome1, Genome genome2)
        {
            var lesserInnovationNumber = genome1.HighestInnovationNumber() < genome2.HighestInnovationNumber() ?
                                            genome1.HighestInnovationNumber() : genome2.HighestInnovationNumber();
            return lesserInnovationNumber == genome1.HighestInnovationNumber() ?
                genome1.ConnectionGenes.Where(conn => conn.InnovationID > lesserInnovationNumber).ToList() :
                genome2.ConnectionGenes.Where(conn => conn.InnovationID > lesserInnovationNumber).ToList();

        }
        private List<int> MatchingGenesInnovationID(Genome genome1, Genome genome2)
        {
            List<int> matchingGenesID = new List<int>();
            foreach (var connection in genome1.ConnectionGenes)
            {
                if (genome2.ConnectionGenes.Any(conn => conn.InnovationID == connection.InnovationID))
                {
                    matchingGenesID.Add(connection.InnovationID);
                }
            }
            return matchingGenesID;
        }
        private double GetAverageWeightDifference(Genome genome1, Genome genome2)
        {
            var matchingGenesID = MatchingGenesInnovationID(genome1, genome2);
            double averageWeightDiff = 0;
            foreach (var innoID in matchingGenesID)
            {
                double conn1Weight = genome1.ConnectionGenes.Find(conn => conn.InnovationID == innoID).Weight;
                double conn2Weight = genome2.ConnectionGenes.Find(conn => conn.InnovationID == innoID).Weight;
                averageWeightDiff = Math.Abs(conn1Weight - conn2Weight);
            }
            return averageWeightDiff / matchingGenesID.Count;
        }
    }
}
