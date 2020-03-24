using System;
using System.Collections.Generic;
using System.Text;

namespace NEAT.NEAT
{
    public class Innovation
    {
        public int InnovationID { get; set; }
        public InnovationType InnovationType { get; set; }
        public int? FromNeuron { get; set; }
        public int? ToNeuron { get; set; }
        public Innovation(InnovationType type, int? from = null, int? to = null, int innovationID = -1)
        {
            InnovationID = innovationID;
            InnovationType = type;
            FromNeuron = from;
            ToNeuron = to;
        }
    }
}
