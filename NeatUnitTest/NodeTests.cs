using Microsoft.VisualStudio.TestTools.UnitTesting;
using NEAT.Phenotype;
using NEAT.Genotype;
namespace NeatUnitTest
{
    [TestClass]
    public class NodeTests
    {
        NeuronGene neuronGene = new NeuronGene(NeuronType.Input, 0, 0, false, 1);
        [TestMethod]
        public void TestConstructor()
        {
            Node node = new Node(neuronGene);
            Assert.AreEqual(1, node.Id, "innovation id is incorrect");
            Assert.AreEqual(0, node.X, "innovation id is incorrect");
            Assert.AreEqual(NeuronType.Input, node.NodeType, "innovation id is incorrect");
        }
        [TestMethod]
        public void TestSigmoid()
        {
            Node node = new Node(neuronGene);
            node.Value = 1;
            node.FeedForward();
            Assert.AreEqual(0.73106, node.Value, 0.0001, "sigmoid function incorrect");
        }
        [TestMethod]
        public void FeedInput()
        {
            Node node = new Node(neuronGene);
            node.FeedInput(1);
            Assert.AreEqual(1, node.Value, "fed input not equal to actual value");
        }
    }
}
