namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Efectua testes aos grafos.
    /// </summary>
    [TestClass]
    public class GraphTest
    {
        /// <summary>
        /// Testa a função de pesquisa por extensão num grafo constituído
        /// apenas por um nó.
        /// </summary>
        [TestMethod]
        [Description("Tests the breath search in a single node graph.")]
        public void EdgeListGraph_BreathFirstSearchTest_SingleNode()
        {
            var syncSource = new SynchGen();
            var target = this.CreateTestGraph_SingleNode(syncSource);

            for (var i = 0; i < 5; ++i)
            {
                var startVertex = target.Vertices.First();
                var endVertex = target.Vertices.Last();

                target.BreathFirstSearch(
                    new List<OperationVertex> { startVertex },
                    v => v.Process(),
                    (c, p, b, e) =>
                    {
                        var prevChannel = e.Value.SourceChannel;
                        var currChannel = e.Value.TargetChannel;
                        c.Arguments[currChannel] = p.Results[prevChannel];

                        // Processa o nó se ainda não foi visitado.
                        if (!b)
                        {
                            c.Process();
                        }
                    });

                Assert.AreEqual(i, endVertex.Results[0]);

                syncSource.Increment();
            }
        }

        /// <summary>
        /// Testa a função de pesquisa por extensão num grafo dirigido
        /// com dois nós e um ciclo.
        /// </summary>
        [TestMethod]
        [Description("Tests the breath first search in a directed graph with two nodes and a cycle.")]
        public void EdgeListGraph_BreathFirstSearch_SympleCycle()
        {
            var syncSource = new SynchGen();
            var target = this.CreateTestGraph_SimpleCycle(syncSource);

            var startVertex = target.Vertices.First();

            for (var i = 0; i < 5; ++i)
            {
                target.BreathFirstSearch(
                    new List<OperationVertex> { startVertex },
                    v => v.Process(),
                    (c, p, b, e) =>
                    {
                        var prevChannel = e.Value.SourceChannel;
                        var currChannel = e.Value.TargetChannel;
                        c.Arguments[currChannel] = p.Results[prevChannel];
                        if (!b)
                        {
                            c.Process();
                        }
                    });

                Assert.AreEqual(i * (i + 1) / 2, startVertex.Results[0]);
                syncSource.Increment();
            }
        }

        /// <summary>
        /// Testa a pesquisa por extensão num grafo ligeiramente mais complexo.
        /// </summary>
        [TestMethod]
        [Description("Tests the breath first search in a slight complex graph.")]
        public void EdgeListGraph_BreathFirstSearch_Complex()
        {
            var syncSource = new SynchGen();
            var target = this.CreateTestGraph_Complex(syncSource);

            var startVertex = target.Vertices.First();
            var endVertex = target.Vertices.ElementAt(1);

            var expected = new int[] { 0, -13, -6, 13, 36 };
            for (var i = 0; i < 5; ++i)
            {

                target.BreathFirstSearch(
                    new List<OperationVertex> { startVertex },
                    v => v.Process(),
                    (c, p, b, e) =>
                    {
                        var prevChannel = e.Value.SourceChannel;
                        var currChannel = e.Value.TargetChannel;
                        c.Arguments[currChannel] = p.Results[prevChannel];
                        if (!b)
                        {
                            c.Process();
                        }
                    });

                Assert.AreEqual(expected[i], endVertex.Results[0]);

                syncSource.Increment();
            }
        }

        /// <summary>
        /// Testa a pesquisa por extensão num grafo não direccionado complexo.
        /// </summary>
        [TestMethod]
        [Description("Tests the breath first search in a undirected complex graph.")]
        public void EdgeListGraph_BreahFirstSearch_UndirectedComplex()
        {
            var target = this.CreateTestGraph_Undirected();
            var expected = new int[] { 1, 3, 2, 4, 5, 6, 7, 8, 9, 10 };
            var actual = new int[10];
            var i = 0;
            target.BreathFirstSearch(
                new int[] { 1, 2, 3, 4 },
                v => actual[i++] = v,
                (c, p, b, e) =>
                {
                    if (!b)
                    {
                        actual[i++] = c;
                    }
                });

            for (i = 0; i < 10; ++i)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        /// Testa a pesquisa em profundidade num grafo não direccionado complexo.
        /// </summary>
        [TestMethod]
        [Description("Tests the depth first search in an undirected complex graph.")]
        public void EdgeListGraph_DepthFirstSearch_UndirectedComplex()
        {
            var target = this.CreateTestGraph_Undirected();
            var expected = new int[] { 4, 10, 9, 7, 8, 6, 3, 5, 2, 1 };
            var actual = new int[10];
            var i = 0;
            target.DepthFirstSearch(
                new int[] { 1, 2, 3, 4 },
                v => actual[i++] = v,
                (c, p, b, e) =>
                {
                    if (!b)
                    {
                        actual[i++] = c;
                    }
                });

            for (i = 0; i < 10; ++i)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        /// Testa a pesquisa em profundidade para resolver a eqaução às diferenças:
        /// y[n+3] = n - y[n+1] - 2y[n+2] - 3y[n+3]
        /// </summary>
        [TestMethod]
        [Description("Test the depth first search in solving a difference equation.")]
        public void EdgeListGraph_DepthFirstSearch_DirectedDiffEquation()
        {
            var syncSource = new SynchGen();
            var target = this.CreateTestGraph_DiffEquation(syncSource);
            var expected = new int[] { 1, 2, 3, -14, 35, -78 };
            var outputVertex = target.Vertices.First();
            var sumVertex = target.Vertices.ElementAt(1);
            for (var i = 0; i < 6; ++i)
            {
                target.DepthFirstSearch(
                    new[] { outputVertex },
                    v => v.Process(),
                    (c, p, b, e) =>
                    {
                        if (!b)
                        {
                            c.Process();
                        }

                        var prevChannel = e.Value.SourceChannel;
                        var currChannel = e.Value.TargetChannel;
                        p.Arguments[prevChannel] = c.Results[currChannel];
                    });

                Assert.AreEqual(expected[i], outputVertex.Results[0]);
                syncSource.Increment();
            }
        }

        /// <summary>
        /// Testa a pesquisa em profundidade, processando uma rede neuronal simples.
        /// </summary>
        [TestMethod]
        [Description("Tests the depth first search in processing simple trained neural network.")]
        public void EdgeListGraph_DepthFirstSearch_SimpleNeuralNetwork()
        {
            var target = this.CreateTestGraph_SimpleNeuralNework();
            var input0 = target.Vertices.ElementAt(0);
            var input1 = target.Vertices.ElementAt(1);
            var input2 = target.Vertices.ElementAt(2);
            var input3 = target.Vertices.ElementAt(3);
            var input4 = target.Vertices.ElementAt(4);
            var input5 = target.Vertices.ElementAt(5);

            var output0 = target.Vertices.ElementAt(6);
            var output1 = target.Vertices.ElementAt(7);

            input0.Output = 1.0;
            input3.Output = 1.0;

            target.DepthFirstSearch(
                new List<NeuSource>() { output0, output1 },
                v =>
                {
                    v.Process();
                },
                (c, p, b, e) =>
                {
                    if (!b)
                    {
                        c.Process();
                    }

                    ((NeuTest)p).Input += e.Value * c.Output;
                });

            Assert.AreEqual(1.0, output0.Output);
            Assert.AreEqual(0.0, output1.Output);
        }

        /// <summary>
        /// Cria um grafo de testes.
        /// </summary>
        /// <param name="syncSource">
        /// Uma fonte de valores.
        /// </param>
        /// <returns>O grafo de testes.</returns>
        private LabeledEdgeListGraph<OperationVertex, OperationEdge> CreateTestGraph_SingleNode(
            SynchGen syncSource)
        {
            var result = new LabeledEdgeListGraph<OperationVertex, OperationEdge>(
                true);
            result.AddVertex(new OperationVertex(() => syncSource.Current));

            return result;
        }

        /// <summary>
        /// Cria um grafo de testes.
        /// </summary>
        /// <param name="syncSource">Uma fonte de valores.</param>
        /// <returns>O grafo de testes.</returns>
        private LabeledEdgeListGraph<OperationVertex, OperationEdge> CreateTestGraph_SimpleCycle(
            SynchGen syncSource)
        {
            var result = new LabeledEdgeListGraph<OperationVertex, OperationEdge>(true);
            var sourceVertex = new OperationVertex(
                1,
                1,
                (arg, res) => res[0] = arg[0] + syncSource.Current);
            Array.Clear(sourceVertex.Arguments, 0, sourceVertex.Arguments.Length);
            Array.Clear(sourceVertex.Results, 0, sourceVertex.Results.Length);

            result.AddVertex(sourceVertex);
            result.AddEdge(
                sourceVertex,
                sourceVertex,
                new OperationEdge(0, 0));
            return result;
        }

        /// <summary>
        /// Cria um grafo de testes um pouco mais complexo.
        /// </summary>
        /// <param name="syncSource">Uma fonte de valores.</param>
        /// <returns>O grafo de testes.</returns>
        private LabeledEdgeListGraph<OperationVertex, OperationEdge> CreateTestGraph_Complex(
            SynchGen syncSource)
        {
            var result = new LabeledEdgeListGraph<OperationVertex, OperationEdge>(
                true);
            var delayLine = new int[] { 1, 2, 3 };
            var sourceVertex = new OperationVertex(
                () => syncSource.Current);
            var prodSumVertex = new OperationVertex(
                4,
                1,
                (i, o) =>
                {
                    o[0] = i[0] - i[1] - 2 * i[2] - 3 * i[3];
                });

            // Poderia ser realizado com apenas um vértice e vários canais de saída
            // sem a necessidade de uma linha de atraso.
            var delay1Vertex = new OperationVertex(
                1,
                1,
                (i, o) =>
                {
                    o[0] = delayLine[0];
                    delayLine[0] = i[0];
                });

            var delay2Vertex = new OperationVertex(
                1,
                1,
                (i, o) =>
                {
                    o[0] = delayLine[1];
                    delayLine[1] = i[0];
                });

            var delay3Vertex = new OperationVertex(
                1,
                1,
                (i, o) =>
                {
                    o[0] = delayLine[2];
                    delayLine[2] = i[0];
                });

            // O grafo tem de ser realizado da saída para a entrada
            result.AddEdge(
                sourceVertex,
                prodSumVertex,
                new OperationEdge(0, 0));
            result.AddEdge(
                prodSumVertex,
                delay1Vertex,
                new OperationEdge(0, 0));
            result.AddEdge(
                delay1Vertex,
                delay2Vertex,
                new OperationEdge(0, 0));
            result.AddEdge(
                delay1Vertex,
                prodSumVertex,
                new OperationEdge(0, 1));
            result.AddEdge(
                delay2Vertex,
                delay3Vertex,
                new OperationEdge(0, 0));
            result.AddEdge(
                delay2Vertex,
                prodSumVertex,
                new OperationEdge(0, 2));
            result.AddEdge(
                delay3Vertex,
                prodSumVertex,
                new OperationEdge(0, 3));

            return result;
        }

        /// <summary>
        /// Cria um grafo não direccionado complexo para teste.
        /// </summary>
        /// <returns>O grafo.</returns>
        private EdgeListGraph<int> CreateTestGraph_Undirected()
        {
            var result = new EdgeListGraph<int>();
            for (var i = 1; i <= 10; ++i)
            {
                result.AddVertex(i);
            }

            result.AddEdge(1, 2);
            result.AddEdge(1, 4);
            result.AddEdge(4, 7);
            result.AddEdge(7, 10);
            result.AddEdge(7, 9);
            result.AddEdge(2, 5);
            result.AddEdge(3, 5);
            result.AddEdge(3, 6);
            result.AddEdge(5, 8);
            result.AddEdge(6, 7);
            result.AddEdge(6, 8);
            result.AddEdge(6, 9);

            return result;
        }

        /// <summary>
        /// Cria um grafo de testes que, quando devidamente estruturado com a pesquisa
        /// de profundidade, pode ser utlizado na resolução da equação às diferenças da
        /// forma y[n+3]= n - y[n] - 2y[n+1] - 3y[n+3]
        /// </summary>
        /// <param name="syncSource">Uma fonte de valores.</param>
        /// <returns>O grafo de testes.</returns>
        /// <remarks>
        /// Note-se que o grafo deverá ser dirigido dos nós de saída para os nós de entrada.
        /// </remarks>
        private LabeledEdgeListGraph<OperationVertex, OperationEdge> CreateTestGraph_DiffEquation(
            SynchGen syncSource)
        {
            var result = new LabeledEdgeListGraph<OperationVertex, OperationEdge>(
                true);
            var sourceVertex = new OperationVertex(
                () => syncSource.Current);
            var prodSumVertex = new OperationVertex(
                4,
                1,
                (i, o) =>
                {
                    o[0] = i[0] - i[1] - 2 * i[2] - 3 * i[3];
                });

            // Poderia ser realizado com apenas um vértice e vários canais de saída
            // sem a necessidade de uma linha de atraso.
            var delayVertex = new OperationVertex(
                1,
                4,
                (i, o) =>
                {
                    o[0] = o[1];
                    o[1] = o[2];
                    o[2] = o[3];
                    o[3] = i[0];
                });
            delayVertex.Results[1] = 1;
            delayVertex.Results[2] = 2;
            delayVertex.Results[3] = 3;

            result.AddEdge(
                delayVertex,
                prodSumVertex,
                new OperationEdge(0, 0));
            result.AddEdge(
                prodSumVertex,
                sourceVertex,
                new OperationEdge(0, 0));
            result.AddEdge(
                prodSumVertex,
                delayVertex,
                new OperationEdge(1, 1));
            result.AddEdge(
                prodSumVertex,
                delayVertex,
                new OperationEdge(2, 2));
            result.AddEdge(
                prodSumVertex,
                delayVertex,
                new OperationEdge(3, 3));

            return result;
        }

        /// <summary>
        /// Cria um grafo de teste que define uma espécie de rede neural muito simples.
        /// </summary>
        /// <returns>O grafo de teste.</returns>
        private LabeledEdgeListGraph<NeuSource, double> CreateTestGraph_SimpleNeuralNework()
        {
            var result = new LabeledEdgeListGraph<NeuSource, double>(true);
            var h1 = new NeuTest("h1");
            var h2 = new NeuTest("h2");
            var g1 = new NeuTest(d => d > 1.5 ? 1.0 : 0.0, "g1");
            var g2 = new NeuTest(d => d > -1.5 ? 1.0 : 0.0, "g2");

            var vertices = new NeuSource[6];
            for (var i = 0; i < 6; ++i)
            {
                var vertex = new NeuSource(string.Format("input{0}", i));
                result.AddVertex(vertex);
                vertices[i] = vertex;
            }

            result.AddVertex(g1);
            result.AddVertex(g2);

            var j = 0;
            for (; j < 3; ++j)
            {
                result.AddEdge(h1, vertices[j], 1.0);
            }

            for (; j < 6; ++j)
            {
                result.AddEdge(h2, vertices[j], 1.0);
            }

            result.AddEdge(g1, h1, 1.0);
            result.AddEdge(g1, h2, 1.0);
            result.AddEdge(g2, h1, -1.0);
            result.AddEdge(g2, h2, -1.0);

            return result;
        }

        #region Auxiliary Classes

        /// <summary>
        /// Define um tipo de vértice que representa uma operação.
        /// </summary>
        private class OperationVertex
        {
            /// <summary>
            /// Mantém o valor dos argumentos.
            /// </summary>
            private int[] arguments;

            /// <summary>
            /// Mantém o valor dos resultados.
            /// </summary>
            private int[] results;

            /// <summary>
            /// Mantém a função responsável pela transformação.
            /// </summary>
            private Action<int[], int[]> processAction;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="OperationVertex"/>.
            /// </summary>
            /// <remarks>
            /// Função que permite apenas gerar a identidade.
            /// Não necessita de argumentos.
            /// </remarks>
            public OperationVertex()
            {
                this.arguments = null;
                this.results = new int[1];
                this.processAction = (i, r) => r[0] = 0;
            }

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="OperationVertex"/>.
            /// </summary>
            /// <param name="sourceFunc">
            /// Função que permite gerar valores.
            /// </param>
            /// <remarks>
            /// Não necessita de argumentos.
            /// </remarks>
            public OperationVertex(
                Func<int> sourceFunc)
            {
                this.arguments = null;
                this.results = new int[1];
                this.processAction = (i, r) => r[0] = sourceFunc.Invoke();
            }

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="OperationVertex"/>.
            /// </summary>
            /// <param name="inputLength">O tamanho do vector dos argumentos.</param>
            /// <param name="outputLength">O tamanho do vector de saída.</param>
            /// <param name="process">A função responsável pela transformação.</param>
            public OperationVertex(
                int inputLength,
                int outputLength,
                Action<int[], int[]> process)
            {
                this.arguments = new int[inputLength];
                this.results = new int[outputLength];
                this.processAction = process;
            }

            /// <summary>
            /// Obtém o vector de argumentos.
            /// </summary>
            public int[] Arguments
            {
                get
                {
                    return this.arguments;
                }
            }

            /// <summary>
            /// Obtém o vector de resultados.
            /// </summary>
            public int[] Results
            {
                get
                {
                    return this.results;
                }
            }

            /// <summary>
            /// Obtém a função responsável pela transformação.
            /// </summary>
            public Action<int[], int[]> ProcessAction
            {
                get
                {
                    return this.processAction;
                }
            }

            /// <summary>
            /// Processa o nó.
            /// </summary>
            public void Process()
            {
                this.processAction.Invoke(
                    this.arguments,
                    this.results);
            }
        }

        /// <summary>
        /// Define um tipo para a aresta.
        /// </summary>
        private class OperationEdge
        {
            /// <summary>
            /// Canal de saída do vértice fonte.
            /// </summary>
            private int sourceChannel;

            /// <summary>
            /// Canal de entrada do vértice alvo.
            /// </summary>
            private int targetChannel;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="OperationEdge"/>.
            /// </summary>
            /// <param name="sourceChannel">O canal de saída do vértice fonte.</param>
            /// <param name="targetChannel">O canal de entrada do vértice alvo.</param>
            public OperationEdge(
                int sourceChannel,
                int targetChannel)
            {
                this.sourceChannel = sourceChannel;
                this.targetChannel = targetChannel;
            }

            /// <summary>
            /// Obtém o canal de saída do vértice fonte.
            /// </summary>
            public int SourceChannel
            {
                get
                {
                    return this.sourceChannel;
                }
            }

            /// <summary>
            /// Obtém o canal de entrada do vértice alvo.
            /// </summary>
            public int TargetChannel
            {
                get
                {
                    return this.targetChannel;
                }
            }
        }

        /// <summary>
        /// Define uma sequência de números.
        /// </summary>
        private class SynchGen
        {
            /// <summary>
            /// O número actual.
            /// </summary>
            private int current = 0;

            /// <summary>
            /// Obtém o número actual.
            /// </summary>
            public int Current
            {
                get
                {
                    return this.current;
                }
            }

            /// <summary>
            /// Incrementa o número.
            /// </summary>
            public void Increment()
            {
                ++this.current;
            }
        }

        /// <summary>
        /// Define um nó com valor de saída.
        /// </summary>
        private class NeuSource
        {
            /// <summary>
            /// Mantém o valor de saída.
            /// </summary>
            protected double output;

            /// <summary>
            /// O nome do nó.
            /// </summary>
            protected string name;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="NeuSource"/>.
            /// </summary>
            /// <param name="name">O nome do nó.</param>
            public NeuSource(string name)
            {
                this.name = name;
            }

            /// <summary>
            /// Obtém ou atribui o valor de saída.
            /// </summary>
            public double Output
            {
                get
                {
                    return this.output;
                }
                set
                {
                    this.output = value;
                }
            }

            /// <summary>
            /// Processa nó.
            /// </summary>
            public virtual void Process()
            {
            }

            /// <summary>
            /// Obtém uma representação do nó.
            /// </summary>
            /// <returns>A representação do nó.</returns>
            public override string ToString()
            {
                return this.name;
            }
        }

        /// <summary>
        /// Define um nó com valor de entrada e valor de saída.
        /// </summary>
        private class NeuTest : NeuSource
        {
            /// <summary>
            /// Mantém o valor de entrada.
            /// </summary>
            private double input;

            /// <summary>
            /// Mantém a função de processamento.
            /// </summary>
            private Func<double, double> processFunc = v => v > 0.5 ? 1.0 : 0.0;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="NeuTest"/>.
            /// </summary>
            /// <param name="name">O nome do nó.</param>
            public NeuTest(string name) : base(name) { }

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="NeuTest"/>.
            /// </summary>
            /// <param name="func">A função de processamento.</param>
            /// <param name="name">O nome do nó.</param>
            public NeuTest(Func<double, double> func, string name)
                : base(name)
            {
                this.processFunc = func;
            }

            /// <summary>
            /// Obtém ou atribui o valor de entrada.
            /// </summary>
            public double Input
            {
                get
                {
                    return this.input;
                }
                set
                {
                    this.input = value;
                }
            }

            /// <summary>
            /// Processa o nó.
            /// </summary>
            public override void Process()
            {
                this.output = this.processFunc.Invoke(
                    this.input);
            }
        }

        #endregion Auxiliary Classes
    }
}
