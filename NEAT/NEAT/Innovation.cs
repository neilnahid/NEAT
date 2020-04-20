using System;
using System.Collections.Generic;
using System.Text;

namespace NEAT.NEAT
{
    public class Innovation
    {
        public int InnovationID { get; set; }
        public InnovationType InnovationType { get; set; }
        public NeuronType NeuronType { get; set; }
        public int? FromNeuron { get; set; }
        public int? ToNeuron { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Innovation(InnovationType type, int? from = null, int? to = null, int innovationID = -1, NeuronType neuronType = NeuronType.None,double x = 0,double y = 0)
        {
            InnovationID = innovationID;
            InnovationType = type;
            FromNeuron = from;
            ToNeuron = to;
            NeuronType = neuronType;
        }
    }
}
