namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa o processador de algoritmos sobre grafos cujas arestas não
    /// necessitem de etiquetas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeType">O tipo de objectos que constituem as arestas.</typeparam>
    /// <typeparam name="GraphType">O tipo de objectos que constituem o grafo.</typeparam>
    internal class EdgeListGraphAlgorithmsProcessor<VertexType, EdgeType, GraphType>
        : IEdgeListGraphAlgorithmsProcessor<VertexType, EdgeType, GraphType>
        where EdgeType : IEdge<VertexType>
        where GraphType : IEdgeGraph<VertexType, EdgeType>
    {
        /// <summary>
        /// Indica se o grafo é direccionado.
        /// </summary>
        protected bool directed = false;

        /// <summary>
        /// O comparador de vértices.
        /// </summary>
        protected IEqualityComparer<VertexType> vertexEqualityComparer;

        /// <summary>
        /// Mapeia cada vértice às arestas adjacentes.
        /// </summary>
        protected Dictionary<VertexType, List<EdgeType>> vertices;

        /// <summary>
        /// O contentor das arestas.
        /// </summary>
        protected List<EdgeType> edges;

        /// <summary>
        /// Objecto responsável pela criação dos grafos.
        /// </summary>
        protected Func<bool, IEqualityComparer<VertexType>, GraphType> graphFactory;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo 
        /// <see cref="EdgeListGraphAlgorithmsProcessor{VertexType, EdgeType, GraphType}"/>
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="vertices">Os vértices.</param>
        /// <param name="edges">As arestas.</param>
        /// <param name="graphFactory">O objecto responsável pela criação de grafos.</param>
        public EdgeListGraphAlgorithmsProcessor(
            bool directed,
            IEqualityComparer<VertexType> vertexEqualityComparer,
            Dictionary<VertexType, List<EdgeType>> vertices,
            List<EdgeType> edges,
            Func<bool, IEqualityComparer<VertexType>, GraphType> graphFactory)
        {
            this.directed = directed;
            this.vertexEqualityComparer = vertexEqualityComparer;
            this.vertices = vertices;
            this.edges = edges;
            this.graphFactory = graphFactory;
        }

        /// <summary>
        /// Obtém os ciclos simples do grafo.
        /// </summary>
        /// <returns>Uma lista com os ciclos por componente conexa.</returns>
        public List<List<GraphType>> GetCycles()
        {
            var result = new List<List<GraphType>>();
            var verticesEnumerator = this.vertices.GetEnumerator();
            var linkNodesQueue = new Queue<LinkNode<VertexType, EdgeType>>();
            var processedEdges = new Dictionary<EdgeType, bool>();
            var visitedNodes = default(Dictionary<VertexType, LinkNode<VertexType, EdgeType>>);
            if (this.vertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeType>>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeType>>(this.vertexEqualityComparer);
            }

            while (verticesEnumerator.MoveNext())
            {
                if (!visitedNodes.ContainsKey(verticesEnumerator.Current.Key))
                {
                    linkNodesQueue.Clear();
                    var nodeToEnqueue = new LinkNode<VertexType, EdgeType>()
                    {
                        Node = verticesEnumerator.Current.Key
                    };

                    nodeToEnqueue.ConnectedEdges.AddRange(
                        verticesEnumerator.Current.Value);
                    linkNodesQueue.Enqueue(nodeToEnqueue);
                    var cyclesList = new List<GraphType>();
                    visitedNodes.Add(
                        verticesEnumerator.Current.Key,
                        new LinkNode<VertexType, EdgeType> { Node = verticesEnumerator.Current.Key });

                    while (linkNodesQueue.Count > 0)
                    {
                        var currentLinkNode = linkNodesQueue.Dequeue();
                        foreach (var edge in currentLinkNode.ConnectedEdges)
                        {
                            if (!processedEdges.ContainsKey(edge))
                            {
                                processedEdges.Add(edge, true);
                                var otherNode = edge.InitialVertex;
                                var inverseEdge = true;
                                if (this.vertexEqualityComparer == null)
                                {
                                    if (currentLinkNode.Node.Equals(edge.InitialVertex))
                                    {
                                        otherNode = edge.FinalVertex;
                                        inverseEdge = false;
                                    }
                                }
                                else
                                {
                                    if (this.vertexEqualityComparer.Equals(
                                        currentLinkNode.Node,
                                        edge.InitialVertex))
                                    {
                                        otherNode = edge.FinalVertex;
                                        inverseEdge = false;
                                    }
                                }

                                if (!inverseEdge || !this.directed)
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeType>()
                                    {
                                        Node = otherNode,
                                        Link = new LinkNodeTie<VertexType, EdgeType>()
                                        {
                                            OtherNode = currentLinkNode,
                                            TieEdge = edge
                                        }
                                    };

                                    var visitedLinkNode = default(LinkNode<VertexType, EdgeType>);
                                    if (visitedNodes.TryGetValue(otherNode, out visitedLinkNode))
                                    {
                                        var cycle = this.GetCycleFromLink(
                                            newLinkedNode,
                                            visitedLinkNode);
                                        cyclesList.Add(cycle);
                                    }
                                    else
                                    {
                                        visitedNodes.Add(otherNode, newLinkedNode);
                                        linkNodesQueue.Enqueue(newLinkedNode);
                                        newLinkedNode.ConnectedEdges.AddRange(this.vertices[otherNode]);
                                    }
                                }
                                else
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeType>()
                                    {
                                        Node = otherNode
                                    };

                                    linkNodesQueue.Enqueue(newLinkedNode);
                                    newLinkedNode.ConnectedEdges.AddRange(this.vertices[otherNode]);
                                }
                            }
                        }
                    }

                    result.Add(cyclesList);
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém todos os sub-grafos que são componentes conexas.
        /// </summary>
        /// <returns>As componentes conexas.</returns>
        public List<GraphType> GetConnectedComponents()
        {
            var result = new List<GraphType>();
            var nodesQueue = new Queue<VertexType>();
            var processedEdges = new Dictionary<EdgeType, bool>();
            Dictionary<VertexType, bool> visitedNodes = null;
            if (this.vertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, bool>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, bool>(this.vertexEqualityComparer);
            }

            var vertexEnumerator = this.vertices.Keys.GetEnumerator();
            while (vertexEnumerator.MoveNext())
            {
                var currentGraph = this.graphFactory.Invoke(this.directed, this.vertexEqualityComparer);
                var currentVertex = vertexEnumerator.Current;
                currentGraph.AddVertex(currentVertex);
                if (!visitedNodes.ContainsKey(currentVertex))
                {
                    var component = this.graphFactory.Invoke(this.directed, this.vertexEqualityComparer);
                    nodesQueue.Clear();
                    nodesQueue.Enqueue(currentVertex);
                    while (nodesQueue.Count > 0)
                    {
                        currentVertex = nodesQueue.Dequeue();
                        var edges = this.vertices[currentVertex];
                        foreach (var edgeWithVertex in edges)
                        {
                            if (!processedEdges.ContainsKey(edgeWithVertex))
                            {
                                processedEdges.Add(edgeWithVertex, true);
                                if (this.vertexEqualityComparer != null)
                                {
                                    if (this.vertexEqualityComparer.Equals(
                                        currentVertex,
                                        edgeWithVertex.InitialVertex))
                                    {
                                        currentGraph.AddEdge(edgeWithVertex);
                                        if (!visitedNodes.ContainsKey(edgeWithVertex.FinalVertex))
                                        {
                                            nodesQueue.Enqueue(edgeWithVertex.FinalVertex);
                                            visitedNodes.Add(edgeWithVertex.FinalVertex, true);
                                        }
                                    }
                                    else
                                    {
                                        currentGraph.AddEdge(edgeWithVertex);
                                        if (!visitedNodes.ContainsKey(edgeWithVertex.InitialVertex))
                                        {
                                            nodesQueue.Enqueue(edgeWithVertex.InitialVertex);
                                            visitedNodes.Add(edgeWithVertex.InitialVertex, true);
                                        }
                                    }
                                }
                                else
                                {
                                    if (currentVertex.Equals(edgeWithVertex.InitialVertex))
                                    {
                                        currentGraph.AddEdge(edgeWithVertex);
                                        if (!visitedNodes.ContainsKey(edgeWithVertex.FinalVertex))
                                        {
                                            nodesQueue.Enqueue(edgeWithVertex.FinalVertex);
                                            visitedNodes.Add(edgeWithVertex.FinalVertex, true);
                                        }
                                    }
                                    else
                                    {
                                        currentGraph.AddEdge(edgeWithVertex);
                                        if (!visitedNodes.ContainsKey(edgeWithVertex.InitialVertex))
                                        {
                                            nodesQueue.Enqueue(edgeWithVertex.InitialVertex);
                                            visitedNodes.Add(edgeWithVertex.InitialVertex, true);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    result.Add(currentGraph);
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a lista de ciclos simples de um grafo classificados por componentes conexas bem
        /// como as respectivas componentes.
        /// </summary>
        /// <returns>A lista de ciclos simples e as componentes conexas.</returns>
        public GraphCyclesComponentsPair<VertexType, EdgeType, GraphType> GetCyclesAndConnectedComponents()
        {
            var resultCycles = new List<List<GraphType>>();
            var resultComponents = new List<GraphType>();
            var verticesEnumerator = this.vertices.GetEnumerator();
            var linkNodesQueue = new Queue<LinkNode<VertexType, EdgeType>>();
            var processedEdges = new Dictionary<EdgeType, bool>();
            var visitedNodes = default(Dictionary<VertexType, LinkNode<VertexType, EdgeType>>);
            if (this.vertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeType>>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeType>>(this.vertexEqualityComparer);
            }

            var innerVisitedNodes = default(Dictionary<VertexType, bool>);
            if (this.vertexEqualityComparer == null)
            {
                innerVisitedNodes = new Dictionary<VertexType, bool>();
            }
            else
            {
                innerVisitedNodes = new Dictionary<VertexType, bool>(this.vertexEqualityComparer);
            }

            while (verticesEnumerator.MoveNext())
            {
                if (!visitedNodes.ContainsKey(verticesEnumerator.Current.Key))
                {
                    linkNodesQueue.Clear();
                    var nodeToEnqueue = new LinkNode<VertexType, EdgeType>()
                    {
                        Node = verticesEnumerator.Current.Key
                    };

                    nodeToEnqueue.ConnectedEdges.AddRange(verticesEnumerator.Current.Value);
                    linkNodesQueue.Enqueue(nodeToEnqueue);
                    var cyclesList = new List<GraphType>();

                    // A componente conexa actual
                    var currentGraph = this.graphFactory.Invoke(this.directed, this.vertexEqualityComparer);

                    visitedNodes.Add(
                        verticesEnumerator.Current.Key,
                        new LinkNode<VertexType, EdgeType> { Node = verticesEnumerator.Current.Key });
                    while (linkNodesQueue.Count > 0)
                    {
                        var currentLinkNode = linkNodesQueue.Dequeue();
                        var edges = this.vertices[currentLinkNode.Node];
                        foreach (var edge in edges)
                        {
                            if (!processedEdges.ContainsKey(edge))
                            {
                                processedEdges.Add(edge, true);
                                var otherNode = edge.InitialVertex;
                                var inverseEdge = true;
                                if (this.vertexEqualityComparer == null)
                                {
                                    if (currentLinkNode.Node.Equals(edge.InitialVertex))
                                    {
                                        currentGraph.AddEdge(edge);
                                        otherNode = edge.FinalVertex;
                                        inverseEdge = false;
                                    }
                                    else
                                    {
                                        currentGraph.AddEdge(edge);
                                    }
                                }
                                else
                                {
                                    if (this.vertexEqualityComparer.Equals(currentLinkNode.Node, edge.InitialVertex))
                                    {
                                        currentGraph.AddEdge(edge);
                                        otherNode = edge.FinalVertex;
                                        inverseEdge = false;
                                    }
                                    else
                                    {
                                        currentGraph.AddEdge(edge);
                                    }
                                }

                                if (!inverseEdge || !this.directed)
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeType>()
                                    {
                                        Node = otherNode,
                                        Link = new LinkNodeTie<VertexType, EdgeType>()
                                        {
                                            OtherNode = currentLinkNode,
                                            TieEdge = edge
                                        }
                                    };

                                    var visitedLinkNode = default(LinkNode<VertexType, EdgeType>);
                                    if (visitedNodes.TryGetValue(otherNode, out visitedLinkNode))
                                    {
                                        var cycle = this.GetCycleFromLink(
                                            newLinkedNode,
                                            visitedLinkNode);
                                        cyclesList.Add(cycle);
                                    }
                                    else
                                    {
                                        visitedNodes.Add(otherNode, newLinkedNode);
                                        linkNodesQueue.Enqueue(newLinkedNode);
                                        newLinkedNode.ConnectedEdges.AddRange(this.vertices[otherNode]);
                                    }
                                }
                                else
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeType>()
                                    {
                                        Node = otherNode
                                    };

                                    linkNodesQueue.Enqueue(newLinkedNode);
                                    newLinkedNode.ConnectedEdges.AddRange(this.vertices[otherNode]);
                                }
                            }
                        }
                    }

                    resultCycles.Add(cyclesList);
                    resultComponents.Add(currentGraph);
                }
            }

            return new GraphCyclesComponentsPair<VertexType, EdgeType, GraphType>(
                resultComponents,
                resultCycles);
        }

        /// <summary>
        /// Obtém o ciclo definido por uma lista ligada.
        /// </summary>
        /// <param name="newLinkNode">O próximo nó da lista.</param>
        /// <param name="currentLinkNode">O nó actual.</param>
        /// <returns>O ciclo.</returns>
        private GraphType GetCycleFromLink(
            LinkNode<VertexType, EdgeType> newLinkNode,
            LinkNode<VertexType, EdgeType> currentLinkNode)
        {
            var result = this.graphFactory.Invoke(
                this.directed,
                this.vertexEqualityComparer);
            var startLinkNode = newLinkNode;
            var nextLinkNode = newLinkNode.Link;

            while (nextLinkNode != null)
            {
                result.AddEdge(nextLinkNode.TieEdge);
                nextLinkNode = nextLinkNode.OtherNode.Link;
            }

            startLinkNode = currentLinkNode;
            nextLinkNode = currentLinkNode.Link;
            while (nextLinkNode != null)
            {
                result.AddEdge(nextLinkNode.TieEdge);
                nextLinkNode = nextLinkNode.OtherNode.Link;
            }

            return result;
        }
    }

    /// <summary>
    /// Implementa o processador de algoritmos sobre grafos cujas arestas não
    /// necessitem de etiquetas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos que constituem as arestas.</typeparam>
    internal class LabeledEdgeListGraphAlgorithmsProcessor<VertexType, EdgeValueType>
        : EdgeListGraphAlgorithmsProcessor<VertexType, ILabeledEdge<VertexType, EdgeValueType>, LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>>,
        ILabeledEdgeListGraphAlgorithmsProcessor<VertexType, EdgeValueType>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo 
        /// <see cref="LabeledEdgeListGraphAlgorithmsProcessor{VertexType, EdgeValueType}"/>
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="vertices">Os vértices.</param>
        /// <param name="edges">As arestas.</param>
        /// <param name="graphFactory">O objecto responsável pela criação de grafos.</param>
        public LabeledEdgeListGraphAlgorithmsProcessor(
            bool directed,
            IEqualityComparer<VertexType> vertexEqualityComparer,
            Dictionary<VertexType, List<ILabeledEdge<VertexType, EdgeValueType>>> vertices,
            List<ILabeledEdge<VertexType, EdgeValueType>> edges,
            Func<bool, IEqualityComparer<VertexType>, LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>> graphFactory)
            : base(
            directed,
            vertexEqualityComparer,
            vertices,
            edges,
            graphFactory) { }

        /// <summary>
        /// Obtém uma árvore de cobertura mínima que se inicia num vértice.
        /// </summary>
        /// <param name="startVertex">O vértice inicial.</param>
        /// <param name="edgeValueFunction">A função que permite obter o valor do peso da aresta.</param>
        /// <param name="valueComparer">O comparador de valores.</param>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>A árvore.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se o monóide, a função ou o vértice inicial forem nulos.
        /// </exception>
        public ITree<VertexValuePair<VertexType, OuterType>> GetMinimumSpanningTree<OuterType>(
            VertexType startVertex,
            Func<ILabeledEdge<VertexType, EdgeValueType>, OuterType> edgeValueFunction,
            IComparer<OuterType> valueComparer,
            IMonoid<OuterType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (edgeValueFunction == null)
            {
                throw new ArgumentNullException("edgeValueFunction");
            }
            else if (startVertex == null)
            {
                throw new ArgumentNullException("startVertex");
            }
            else
            {
                IComparer<OuterType> innerValueComparer = Comparer<OuterType>.Default;
                if (valueComparer != null)
                {
                    innerValueComparer = valueComparer;
                }

                var result = new Tree<VertexValuePair<VertexType, OuterType>>(
                    new VertexValuePair<VertexType, OuterType>(startVertex, monoid.AdditiveUnity));
                var verticesQueue = new Queue<TreeNode<VertexValuePair<VertexType, OuterType>>>();
                var visitedVertices = default(
                    Dictionary<VertexType, TreeNode<VertexValuePair<VertexType, OuterType>>>);
                if (this.vertexEqualityComparer == null)
                {
                    visitedVertices = new Dictionary<VertexType, TreeNode<VertexValuePair<VertexType, OuterType>>>();
                }
                else
                {
                    visitedVertices = new Dictionary<VertexType, TreeNode<VertexValuePair<VertexType, OuterType>>>(
                        this.vertexEqualityComparer);
                }

                verticesQueue.Enqueue(result.InternalRootNode);
                var processedEdges = new Dictionary<ILabeledEdge<VertexType, EdgeValueType>, bool>();
                while (verticesQueue.Count > 0)
                {
                    var topTreeNode = verticesQueue.Dequeue();
                    var edges = this.vertices[topTreeNode.NodeObject.Vertex];
                    foreach (var edge in edges)
                    {
                        if (!processedEdges.ContainsKey(edge))
                        {
                            processedEdges.Add(edge, true);
                            var otherNode = edge.InitialVertex;
                            var inverseEdge = true;
                            if (this.vertexEqualityComparer == null)
                            {
                                if (topTreeNode.NodeObject.Vertex.Equals(edge.InitialVertex))
                                {
                                    otherNode = edge.FinalVertex;
                                    inverseEdge = false;
                                }
                            }
                            else
                            {
                                if (this.vertexEqualityComparer.Equals(
                                    topTreeNode.NodeObject.Vertex,
                                    edge.InitialVertex))
                                {
                                    otherNode = edge.FinalVertex;
                                    inverseEdge = false;
                                }
                            }

                            if (!inverseEdge || this.directed)
                            {
                                var visitedNodeValue = default(TreeNode<VertexValuePair<VertexType, OuterType>>);
                                var sum = monoid.Add(topTreeNode.NodeObject.Value, edgeValueFunction.Invoke(edge));
                                if (visitedVertices.TryGetValue(otherNode, out visitedNodeValue))
                                {
                                    var comparisionValue = innerValueComparer.Compare(
                                        sum,
                                        visitedNodeValue.NodeObject.Value);
                                    if (comparisionValue < 0)
                                    {
                                        var parent = visitedNodeValue.Parent;
                                        parent.Remove(visitedNodeValue);
                                        visitedNodeValue.NodeObject.Value = sum;
                                        topTreeNode.Add(visitedNodeValue);
                                    }
                                }
                                else
                                {
                                    var newNode = new TreeNode<VertexValuePair<VertexType, OuterType>>(
                                        new VertexValuePair<VertexType, OuterType>(otherNode, sum),
                                        result,
                                        topTreeNode);
                                    topTreeNode.Add(newNode);
                                    verticesQueue.Enqueue(newNode);
                                    visitedVertices.Add(newNode.NodeObject.Vertex, newNode);
                                }
                            }
                        }
                    }
                }

                return result;
            }
        }
    }


    /// <summary>
    /// Implementa uma extensão ao grafo baseado em listas de arestas de modo
    /// a incluir alguns algoritmos.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    public class EdgeListGraphMathExtensions<VertexType>
        : EdgeListGraph<VertexType>,
        IEdgeListGraphMathExtension<
        VertexType,
        IEdge<VertexType>,
        EdgeListGraphMathExtensions<VertexType>
        >
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraphMathExtensions{VertexType}"/>.
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraphMathExtensions(bool directed = false) : base(directed) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraphMathExtensions{VertexType}"/>.
        /// </summary>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraphMathExtensions(
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(vertexEqualityComparer, directed) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraphMathExtensions{VertexType}"/>.
        /// </summary>
        /// <param name="edgeFactory">A fábrica responsável pela criação das arestas.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraphMathExtensions(
            Func<VertexType, VertexType, IEdge<VertexType>> edgeFactory,
            bool directed = false)
            : base(edgeFactory, directed) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EdgeListGraphMathExtensions{VertexType}"/>.
        /// </summary>
        /// <param name="edgeFactory">A fábrica responsável pela criação das arestas.</param>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public EdgeListGraphMathExtensions(
            Func<VertexType, VertexType, IEdge<VertexType>> edgeFactory,
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(edgeFactory, vertexEqualityComparer, directed) { }

        /// <summary>
        /// Obtém o processador de algoritmos sobre o grafo cujas arestas
        /// não necessitam de etiqueta.
        /// </summary>
        /// <returns>O processador de algoritmos.</returns>
        public IEdgeListGraphAlgorithmsProcessor<VertexType, IEdge<VertexType>, EdgeListGraphMathExtensions<VertexType>> GetAlgorithmsProcessor()
        {
            return new EdgeListGraphAlgorithmsProcessor<VertexType, IEdge<VertexType>, EdgeListGraphMathExtensions<VertexType>>(
                this.directed,
                this.vertexEqualityComparer,
                this.vertices,
                this.edges,
                (d, c) => new EdgeListGraphMathExtensions<VertexType>(c, d));
        }
    }

    /// <summary>
    /// Implementa uma extensão ao grafo baseado em listas de arestas com etiqueta de modo
    /// a incluir alguns algoritmos.
    /// </summary>
    /// <typeparam name="VertexType">O tipo dos objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo dos objectos que constituem as arestas.</typeparam>
    public class LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>
        : LabeledEdgeListGraph<VertexType, EdgeValueType>,
        ILabeledEdgeListGraphMathExtension<
        VertexType,
        EdgeValueType,
        LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>
        >
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo 
        /// <see cref="LabeledEdgeListGraphMathExtensions{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public LabeledEdgeListGraphMathExtensions(bool directed = false) : base(directed) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo 
        /// <see cref="LabeledEdgeListGraphMathExtensions{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public LabeledEdgeListGraphMathExtensions(
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(vertexEqualityComparer, directed) { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LabeledEdgeListGraphMathExtensions{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="edgeFactory">O objecto responsável pela criação de arestas.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja directo e falso caso contrário.</param>
        public LabeledEdgeListGraphMathExtensions(
            Func<VertexType, VertexType, EdgeValueType, ILabeledEdge<VertexType, EdgeValueType>> edgeFactory,
            bool directed = false)
            : base(edgeFactory, directed) { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LabeledEdgeListGraphMathExtensions{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="edgeFactory">O objecto responsável pela criação de arestas.</param>.
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public LabeledEdgeListGraphMathExtensions(
            Func<VertexType, VertexType, EdgeValueType, ILabeledEdge<VertexType, EdgeValueType>> edgeFactory,
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(edgeFactory, vertexEqualityComparer, directed) { }

        /// <summary>
        /// Obtém o processador de algoritmos sobre o grafo cujas arestas
        /// não necessitam de etiqueta.
        /// </summary>
        /// <returns>O processador de algoritmos.</returns>
        public IEdgeListGraphAlgorithmsProcessor<VertexType, ILabeledEdge<VertexType, EdgeValueType>, LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>> GetAlgorithmsProcessor()
        {
            return new EdgeListGraphAlgorithmsProcessor<VertexType, ILabeledEdge<VertexType, EdgeValueType>, LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>>(
                this.directed,
                this.vertexEqualityComparer,
                this.vertices,
                this.edges,
                (d, c) => new LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>(c, d));
        }

        /// <summary>
        /// Obtém o processador de algoritmos sobre o grafo cujas arestas
        /// necessitam de etiqueta.
        /// </summary>
        /// <returns>O processador de algoritmos.</returns>
        public ILabeledEdgeListGraphAlgorithmsProcessor<VertexType, EdgeValueType> GetLabeledAlgorithmsProcessor()
        {
            return new LabeledEdgeListGraphAlgorithmsProcessor<VertexType, EdgeValueType>(
                this.directed,
                this.vertexEqualityComparer,
                this.vertices,
                this.edges,
                (d, c) => new LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>(c, d));
        }
    }
}
