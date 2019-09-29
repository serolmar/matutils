namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass()]
    public class EdgeListGraphTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void GetAlgorithmsProcessorTest_GetMinimumSpanningTree()
        {
            var graph = this.GetTestGraph();
            var processor = graph.GetLabeledAlgorithmsProcessor();
            var result = processor.GetMinimumSpanningTree<double>(
                0,
                e => e.Value,
                Comparer<double>.Default,
                new DoubleField());

            var root = result.RootNode;
            Assert.AreEqual(0, root.NodeObject.Value);
            Assert.AreEqual(1, root.Count);

            var childNode = root.ChildAt(0);
            Assert.AreEqual(1, childNode.NodeObject.Vertex);

            Assert.AreEqual(2, childNode.Count);
            var firstChildNode = childNode.ChildAt(0);
            var secondChildNode = childNode.ChildAt(1);
            if (firstChildNode.NodeObject.Vertex == 2)
            {
                Assert.AreEqual(3, secondChildNode.NodeObject.Vertex);
                Assert.AreEqual(1, firstChildNode.Count);
                Assert.AreEqual(0, secondChildNode.Count);
                childNode = firstChildNode.ChildAt(0);
                Assert.AreEqual(0, childNode.Count);
                Assert.AreEqual(4, childNode.NodeObject.Vertex);
            }
            else if (firstChildNode.NodeObject.Value == 3)
            {
                Assert.AreEqual(2, secondChildNode.NodeObject.Vertex); 
                Assert.AreEqual(0, firstChildNode.Count);
                Assert.AreEqual(1, secondChildNode.Count);
                childNode = secondChildNode.ChildAt(0);
                Assert.AreEqual(0, childNode.Count);
                Assert.AreEqual(4, childNode.NodeObject.Vertex);
            }
            else
            {
                Assert.Fail("Nó não esperado.");
            }
        }

        [TestMethod()]
        public void GetAlgorithmsProcessorTest_GetCycles()
        {
            var graph = this.GetTestGraph();
            var processor = graph.GetAlgorithmsProcessor();
            var cycles = processor.GetCycles();

            // O número de conjuntos de ciclos tem de igualar o número de componentes conexas.
            Assert.AreEqual(3, cycles.Count);
        }

        [TestMethod()]
        public void GetAlgorithmsProcessorTest_GetConnectedComponents()
        {
            var graph = this.GetTestGraph();
            var processor = graph.GetAlgorithmsProcessor();
            var cycles = processor.GetConnectedComponents();

            // Vaerifica o número de compoentes conexas.
            Assert.AreEqual(3, cycles.Count);
        }

        /// <summary>
        /// Obtém o grafo que irá servir de teste.
        /// </summary>
        /// <returns>O grafo.</returns>
        private LabeledEdgeListGraphMathExtensions<int, int> GetTestGraph()
        {
            var graph = new LabeledEdgeListGraphMathExtensions<int, int>();
            graph.AddEdge(0, 1, 1);
            graph.AddEdge(0, 2, 3);
            graph.AddEdge(0, 3, 5);
            graph.AddEdge(1, 2, 1);
            graph.AddEdge(1, 3, 3);
            graph.AddEdge(2, 3, 7);
            graph.AddEdge(2, 4, 6);
            graph.AddEdge(3, 4, 7);

            graph.AddVertex(10);
            graph.AddVertex(11);

            return graph;
        }
    }
}
