using System;
using NEAT;
namespace NEAT
{
    class Program
    {
        static void Main(string[] args)
        {
            NEAT.Neat neat = new NEAT.Neat();
            neat.Genomes.Add(new NEAT.Genome(0, 3, 4, neat));
            var genome = neat.Genomes[0];
            genome.Display();
            for (int i = 0; i < 100; i++)
            {
                genome.Mutate();
            }
            Console.Clear();
            genome.Display();
            neat.Genomes.Add(new NEAT.Genome(genome));
            Console.WriteLine(neat.Genomes[0].Equals(neat.Genomes[1]));
        }
    }
}
