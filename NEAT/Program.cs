using System;
using NEAT;
using NEAT.NEAT.Phenotype;
using NEAT.NEAT;
namespace NEAT
{
    class Program
    {
        static void Main(string[] args)
        {
            Neat neat = new Neat();
            neat.Genomes.Add(new Genome(0, 3, 4, neat));
            neat.Genomes.Add(new Genome(neat.Genomes[0]));
            var genome = neat.Genomes[0];
            var genome2 = neat.Genomes[1];
            for (int i = 0; i < 100; i++)
            {
                genome.Mutate();
                genome2.Mutate();
            }
            var compDistance = neat.CompatibilityDistance(neat.Genomes[0], neat.Genomes[1]);
            Console.WriteLine(compDistance);
        }
    }
}
