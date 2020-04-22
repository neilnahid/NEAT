using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NEAT.Genotype;
using NEAT.Phenotype;
using NEAT.NEAT;

namespace NEAT
{
    public class Neat
    {
        public List<Genome> Genomes { get; set; } = new List<Genome>();
        public List<Innovation> Innovations { get; set; } = new List<Innovation>();
        public List<Species> Species { get; set; } = new List<Species>();
        public static double M_ADD_CONNECTION_RATE = 0.50;
        public static double M_PERTURB_WEIGHT_RATE = 0.03;
        public static double M_RANDOMISE_WEIGHT_RATE = 0.03;
        public static double M_ADD_NODE_RATE = 0.03;
        public static double WEIGHT_RANDOM_STRENGTH = 0.03;
        public static double WEIGHT_SHIFT_RANDOM = 0.03;
        public static double COMPATIBLITY_THRESHOLD = 0;
        public static double C1 = 1;
        public static double C2 = 1;
        public static double C3 = 1;

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
                inno = new Innovation(InnovationType.NEW_NODE, from.InnovationID, to.InnovationID, Innovations.Count + 1, NeuronType.Hidden);
                Innovations.Add(inno);
            }
            return new NeuronGene(NeuronType.Hidden, (from.X + to.X) / 2, (from.Y + to.Y) / 2, innovationID: inno.InnovationID);
        }
        public NeuronGene GetNode(NeuronType type, double x, double y)
        {
            if (Innovations.Any(inno => inno.X == x && inno.Y == y && inno.NeuronType == type))
            {
                var inno = Innovations.Where(inno => inno.X == x && inno.Y == y && inno.NeuronType == type).First();
                return new NeuronGene(type, x, y, innovationID: inno.InnovationID);
            }
            Innovations.Add(new Innovation(InnovationType.NEW_NODE, null, null, Innovations.Count + 1, type, x, y));
            return new NeuronGene(type, x, y, innovationID: Innovations.Count);
        }
        public static double CompatibilityDistance(Genome genome1, Genome genome2)
        {
            List<ConnectionGene> disjointGenes = DisjointGenes(genome1, genome2);
            List<ConnectionGene> excessGenes = ExcessGenes(genome1, genome2);
            int N = genome1.GeneCount > genome2.GeneCount ? genome1.GeneCount : genome2.GeneCount;
            N = N < 20 ? 1 : N;
            double excessFactor = (C1 * excessGenes.Count) / N;
            double disjointFactor = (C2 * disjointGenes.Count) / N;
            double weightFactor = C3 * GetAverageWeightDifference(genome1, genome2);
            return excessFactor + disjointFactor + weightFactor;
        }
        private static List<ConnectionGene> DisjointGenes(Genome genome1, Genome genome2)
        {
            var lesserInnovationNumber = genome1.HighestInnovationNumber() < genome2.HighestInnovationNumber() ?
                                            genome1.HighestInnovationNumber() : genome2.HighestInnovationNumber();
            return genome1.ConnectionGenes
                .Where(conn => conn.InnovationID <= lesserInnovationNumber)
                .Except(genome2.ConnectionGenes
                            .Where(conn => conn.InnovationID <= lesserInnovationNumber))
                            .ToList();
        }
        private static List<ConnectionGene> ExcessGenes(Genome genome1, Genome genome2)
        {
            var lesserInnovationNumber = genome1.HighestInnovationNumber() < genome2.HighestInnovationNumber() ?
                                            genome1.HighestInnovationNumber() : genome2.HighestInnovationNumber();
            return lesserInnovationNumber == genome1.HighestInnovationNumber() ?
                genome2.ConnectionGenes.Where(conn => conn.InnovationID > lesserInnovationNumber).ToList() :
                genome1.ConnectionGenes.Where(conn => conn.InnovationID > lesserInnovationNumber).ToList();

        }
        private static List<int> MatchingGenesInnovationID(Genome genome1, Genome genome2)
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
        private static double GetAverageWeightDifference(Genome genome1, Genome genome2)
        {
            var matchingGenesID = MatchingGenesInnovationID(genome1, genome2);
            double averageWeightDiff = 0;
            foreach (var innoID in matchingGenesID)
            {
                double conn1Weight = genome1.ConnectionGenes.Find(conn => conn.InnovationID == innoID).Weight;
                double conn2Weight = genome2.ConnectionGenes.Find(conn => conn.InnovationID == innoID).Weight;
                averageWeightDiff += Math.Abs(conn1Weight - conn2Weight);
            }
            return averageWeightDiff / matchingGenesID.Count;
        }
        public void AssignGenomesToSpecies()
        {
            foreach (var currentGenome in Genomes)
            {
                bool wasAssigned = false;
                foreach (var currentSpecies in Species)
                {
                    if (CompatibilityDistance(currentGenome, currentSpecies.Representative) < COMPATIBLITY_THRESHOLD)
                    {
                        currentSpecies.Population.Add(currentGenome);
                        wasAssigned = true;
                        break;
                    }
                }
                if (!wasAssigned)
                {
                    var newSpecies = new Species(currentGenome, Species.Count + 1);
                    Species.Add(newSpecies);
                    continue;
                }
            }
        }
    }
}
