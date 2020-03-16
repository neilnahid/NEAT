namespace NEAT.NEAT
{
    public class NeuronGene : Gene
    {

        public NeuronType NeuronType { get; set; }
        public bool IsRecurrent { get; set; }
        public double ActivationResponse { get; set; }
        public double Y { get; set; }
        public double X { get; set; }
        /// <summary>
        /// Construct a new neuron gene with specified type, and position
        /// </summary>
        /// <param name="nodeType">the type of neuron gene(input, hidden, output)</param>
        /// <param name="x">its X position in phenotype representation</param>
        /// <param name="y">its y position in phenotype representation</param>/
        /// <param name="isRecurrent">specifies if neuron gene is recurrent</param>
        /// <param name="innovationID">specify the innovationID</param>
        public NeuronGene(NeuronType nodeType, double x, double y, bool isRecurrent = false, int innovationID = -1)
        {
            NeuronType = nodeType;
            InnovationID = innovationID == 0 ? Neat.LATEST_NODE_NUMBER++ : innovationID;
            Y = y;
            X = x;
            IsRecurrent = IsRecurrent;
        }
        public NeuronGene(NeuronGene gene)
        {
            NeuronType = gene.NeuronType;
            IsRecurrent = gene.IsRecurrent;
            ActivationResponse = gene.ActivationResponse;
            X = gene.X;
            Y = gene.Y;
            InnovationID = gene.InnovationID;
        }
        public bool Equals(NeuronGene neuron)
        {
            return InnovationID == neuron.InnovationID;
        }
    }
}