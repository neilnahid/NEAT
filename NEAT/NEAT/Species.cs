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
        public List<Genome> Population { get; set; }


        public void AddToSpecies(Genome genome)
        {
            Population.Add(genome);
        }

    }
}
