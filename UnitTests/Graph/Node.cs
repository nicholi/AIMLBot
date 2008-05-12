using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AimlBot.UnitTests.Graph
{
    /// <summary>
    /// Encapsulates a node in the graphmaster tree structure
    /// </summary>
    [TestFixture]
    public class Node : BaseTestClass
    {
        private string[] path = { "This", "is", "a", "test", "path", "<that>", "*", "<topic>", "*" };
        private string template = "This is a test template";
        private string source = "test source";

        [Test]
        public void TestLearn()
        {
            AimlBot.Graph.Node n = new AimlBot.Graph.Node("test");
            AimlBot.Graph.Node result = n.Learn(this.path, this.template, this.source);
            // check the result node is correct
            Assert.AreEqual(0, result.Children.Count);
            Assert.AreEqual(this.source, result.Source);
            Assert.AreEqual(this.template, result.Template);
            Assert.AreEqual("*", result.Word);
            // ok, make sure all the nodes up to and including the leaf are correct if
            // accessed via the tree
            AimlBot.Graph.Node[] testNodes = new AimlBot.Graph.Node[this.path.Length + 1];
            testNodes[0] = n;
            for (int i = 1; i < this.path.Length + 1; i++)
            {
                testNodes[i] = testNodes[i - 1].Children[this.path[i - 1].ToUpper()];
            }
            Assert.AreEqual(this.path.Length + 1, testNodes.Length);
            for (int j = 0; j < testNodes.Length - 1; j++)
            {
                Assert.AreEqual(1, testNodes[j].Children.Count);
                Assert.AreEqual(string.Empty, testNodes[j].Source);
                Assert.AreEqual(null, testNodes[j].Template);
                if (j > 0)
                {
                    Assert.AreEqual(this.path[j - 1].ToUpper(), testNodes[j].Word);
                }
            }
            // leaf node
            AimlBot.Graph.Node leaf = testNodes[testNodes.Length-1];
            Assert.AreEqual(0, leaf.Children.Count);
            Assert.AreEqual(this.source, leaf.Source);
            Assert.AreEqual(this.template, leaf.Template);
            Assert.AreEqual("*", leaf.Word);
        }

        [Test]
        [ExpectedException(typeof(AimlBot.Graph.LearnException))]
        public void TestLearnWithEmptyPath()
        {
            AimlBot.Graph.Node n = new AimlBot.Graph.Node("test");
            n.Learn(new string[0], string.Empty, this.source);
        }

        [Test]
        [ExpectedException(typeof(AimlBot.Graph.LearnException))]
        public void TestLearnWithNoTemplate()
        {
            AimlBot.Graph.Node n = new AimlBot.Graph.Node("test");
            n.Learn(path, string.Empty, this.source);
        }

        [Test]
        [ExpectedException(typeof(AimlBot.Graph.LearnException))]
        public void TestLearnWithNoSourceURI()
        {
            AimlBot.Graph.Node n = new AimlBot.Graph.Node("test");
            n.Learn(path, this.template, string.Empty);
        }
    }
}

