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
            Console.WriteLine(new NeuralNetwork(neat.Genomes[0]).ToString());
        }
    }
}
