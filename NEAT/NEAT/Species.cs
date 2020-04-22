using NEAT.Genotype;
using System;
using System.Collections.Generic;
using System.Text;

namespace NEAT.NEAT
{
    public class Species
    {
        public int SpeciesId { get; set; }
        public Genome Representative { get; set; }
        public List<Genome> Population { get; set; } = new List<Genome>();

        public Species(Genome representative, int speciesId)
        {
            SpeciesId = speciesId;
            Representative = representative;
            Population.Add(representative);
        }
        public void AddToSpecies(Genome genome)
        {
            Population.Add(genome);
        }
        public void CalculateAdjustedFitness()
        {
            foreach (var g in Population)
            {
                g.CalculateAdjustedFitness(this);
            }
        }
    }
}
