using System;
using NEAT;
using NEAT.Genotype;
namespace NEAT
{
    class Program
    {
        static void Main(string[] args)
        {
            Neat neat = new Neat();
            neat.Genomes.Add(new Genome(0, 3, 4, neat));
            for (int i = 0; i < 100; i++)
            {
                neat.Genomes.Add(new Genome(neat.Genomes[0]));
            }
            for (int i = 0; i < 100; i++)
            {
                foreach (var g in neat.Genomes)
                {
                    g.Mutate();
                }
            }
            var compDistance = Neat.CompatibilityDistance(neat.Genomes[0], neat.Genomes[1]);
            neat.AssignGenomesToSpecies();
            Console.WriteLine(compDistance);
        }
    }
}
