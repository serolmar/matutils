namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um conjunto de algoritmos sobre os grafos definidos como listas de arestas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos associados às arestas.</typeparam>
    internal class EdgeListGraphAlgorithms<VertexType, EdgeValueType> : IGraphAlgorithms<VertexType, EdgeValueType>
    {
        /// <summary>
        /// Mantém o grafo sobre o qual actuam os algoritmos.
        /// </summary>
        private EdgeListGraph<VertexType, EdgeValueType> graph;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="EdgeListGraphAlgorithms{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="graph">O grafo..</param>
        public EdgeListGraphAlgorithms(EdgeListGraph<VertexType, EdgeValueType> graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// Obtém os ciclos simples do grafo.
        /// </summary>
        /// <returns>Uma lista com os ciclos por componente conexa.</returns>
        public List<List<IGraph<VertexType, EdgeValueType>>> GetCycles()
        {
            var result = new List<List<IGraph<VertexType, EdgeValueType>>>();
            var verticesEnumerator = this.graph.VerticesDictionary.GetEnumerator();
            var linkNodesQueue = new Queue<LinkNode<VertexType, EdgeValueType>>();
            var processedEdges = new Dictionary<IEdge<VertexType, EdgeValueType>, bool>();
            var visitedNodes = default(Dictionary<VertexType, LinkNode<VertexType, EdgeValueType>>);
            if (graph.VertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeValueType>>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeValueType>>(graph.VertexEqualityComparer);
            }

            while (verticesEnumerator.MoveNext())
            {
                if (!visitedNodes.ContainsKey(verticesEnumerator.Current.Key))
                {
                    linkNodesQueue.Clear();
                    var nodeToEnqueue = new LinkNode<VertexType, EdgeValueType>()
                    {
                        Node = verticesEnumerator.Current.Key
                    };

                    nodeToEnqueue.ConnectedEdges.AddRange(verticesEnumerator.Current.Value);
                    linkNodesQueue.Enqueue(nodeToEnqueue);
                    var cyclesList = new List<IGraph<VertexType, EdgeValueType>>();
                    visitedNodes.Add(
                        verticesEnumerator.Current.Key,
                        new LinkNode<VertexType, EdgeValueType> { Node = verticesEnumerator.Current.Key });

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
                                if (this.graph.VertexEqualityComparer == null)
                                {
                                    if (currentLinkNode.Node.Equals(edge.InitialVertex))
                                    {
                                        otherNode = edge.FinalVertex;
                                        inverseEdge = false;
                                    }
                                }
                                else
                                {
                                    if (this.graph.VertexEqualityComparer.Equals(
                                        currentLinkNode.Node,
                                        edge.InitialVertex))
                                    {
                                        otherNode = edge.FinalVertex;
                                        inverseEdge = false;
                                    }
                                }

                                if (!inverseEdge || !this.graph.Directed)
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeValueType>()
                                    {
                                        Node = otherNode,
                                        Link = new LinkNodeTie<VertexType, EdgeValueType>()
                                        {
                                            OtherNode = currentLinkNode,
                                            TieEdge = edge
                                        }
                                    };

                                    var visitedLinkNode = default(LinkNode<VertexType, EdgeValueType>);
                                    if (visitedNodes.TryGetValue(otherNode, out visitedLinkNode))
                                    {
                                        var cycle = this.GetCycleFromLink(
                                            newLinkedNode,
                                            visitedLinkNode,
                                            this.graph.Directed);
                                        cyclesList.Add(cycle);
                                    }
                                    else
                                    {
                                        visitedNodes.Add(otherNode, newLinkedNode);
                                        linkNodesQueue.Enqueue(newLinkedNode);
                                        newLinkedNode.ConnectedEdges.AddRange(this.graph.VerticesDictionary[otherNode]);
                                    }
                                }
                                else
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeValueType>()
                                    {
                                        Node = otherNode
                                    };

                                    linkNodesQueue.Enqueue(newLinkedNode);
                                    newLinkedNode.ConnectedEdges.AddRange(this.graph.VerticesDictionary[otherNode]);
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
        public List<IGraph<VertexType, EdgeValueType>> GetConnectedComponents()
        {
            var result = new List<IGraph<VertexType, EdgeValueType>>();
            var nodesQueue = new Queue<VertexType>();
            var processedEdges = new Dictionary<IEdge<VertexType, EdgeValueType>, bool>();
            Dictionary<VertexType, bool> visitedNodes = null;
            if (graph.VertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, bool>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, bool>(graph.VertexEqualityComparer);
            }

            var vertexEnumerator = graph.Vertices.GetEnumerator();
            while (vertexEnumerator.MoveNext())
            {
                var currentGraph = new EdgeListGraph<VertexType, EdgeValueType>(graph.Directed);
                var currentVertex = vertexEnumerator.Current;
                currentGraph.AddVertex(currentVertex);
                if (!visitedNodes.ContainsKey(currentVertex))
                {
                    var component = new EdgeListGraph<VertexType, EdgeValueType>(this.graph.Directed);
                    nodesQueue.Clear();
                    nodesQueue.Enqueue(currentVertex);
                    while (nodesQueue.Count > 0)
                    {
                        currentVertex = nodesQueue.Dequeue();
                        var edges = this.graph.VerticesDictionary[currentVertex];
                        foreach (var edgeWithVertex in edges)
                        {
                            if (!processedEdges.ContainsKey(edgeWithVertex))
                            {
                                processedEdges.Add(edgeWithVertex, true);
                                if (this.graph.VertexEqualityComparer != null)
                                {
                                    if (this.graph.VertexEqualityComparer.Equals(
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
        public GraphCyclesComponentsPair<VertexType, EdgeValueType> GetCyclesAndConnectedComponents()
        {
            var resultCycles = new List<List<IGraph<VertexType, EdgeValueType>>>();
            var resultComponents = new List<IGraph<VertexType, EdgeValueType>>();
            var verticesEnumerator = this.graph.VerticesDictionary.GetEnumerator();
            var linkNodesQueue = new Queue<LinkNode<VertexType, EdgeValueType>>();
            var processedEdges = new Dictionary<IEdge<VertexType, EdgeValueType>, bool>();
            var visitedNodes = default(Dictionary<VertexType, LinkNode<VertexType, EdgeValueType>>);
            if (graph.VertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeValueType>>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType, EdgeValueType>>(graph.VertexEqualityComparer);
            }

            var innerVisitedNodes = default(Dictionary<VertexType, bool>);
            if (graph.VertexEqualityComparer == null)
            {
                innerVisitedNodes = new Dictionary<VertexType, bool>();
            }
            else
            {
                innerVisitedNodes = new Dictionary<VertexType, bool>(graph.VertexEqualityComparer);
            }

            while (verticesEnumerator.MoveNext())
            {
                if (!visitedNodes.ContainsKey(verticesEnumerator.Current.Key))
                {
                    linkNodesQueue.Clear();
                    var nodeToEnqueue = new LinkNode<VertexType, EdgeValueType>()
                    {
                        Node = verticesEnumerator.Current.Key
                    };

                    nodeToEnqueue.ConnectedEdges.AddRange(verticesEnumerator.Current.Value);
                    linkNodesQueue.Enqueue(nodeToEnqueue);
                    var cyclesList = new List<IGraph<VertexType, EdgeValueType>>();

                    // A componente conexa actual
                    var currentGraph = new EdgeListGraph<VertexType, EdgeValueType>(graph.Directed);

                    visitedNodes.Add(
                        verticesEnumerator.Current.Key,
                        new LinkNode<VertexType, EdgeValueType> { Node = verticesEnumerator.Current.Key });
                    while (linkNodesQueue.Count > 0)
                    {
                        var currentLinkNode = linkNodesQueue.Dequeue();
                        var edges = this.graph.VerticesDictionary[currentLinkNode.Node];
                        foreach (var edge in edges)
                        {
                            if (!processedEdges.ContainsKey(edge))
                            {
                                processedEdges.Add(edge, true);
                                var otherNode = edge.InitialVertex;
                                var inverseEdge = true;
                                if (this.graph.VertexEqualityComparer == null)
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
                                    if (this.graph.VertexEqualityComparer.Equals(currentLinkNode.Node, edge.InitialVertex))
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

                                if (!inverseEdge || !this.graph.Directed)
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeValueType>()
                                    {
                                        Node = otherNode,
                                        Link = new LinkNodeTie<VertexType, EdgeValueType>()
                                        {
                                            OtherNode = currentLinkNode,
                                            TieEdge = edge
                                        }
                                    };

                                    var visitedLinkNode = default(LinkNode<VertexType, EdgeValueType>);
                                    if (visitedNodes.TryGetValue(otherNode, out visitedLinkNode))
                                    {
                                        var cycle = this.GetCycleFromLink(
                                            newLinkedNode,
                                            visitedLinkNode,
                                            this.graph.Directed);
                                        cyclesList.Add(cycle);
                                    }
                                    else
                                    {
                                        visitedNodes.Add(otherNode, newLinkedNode);
                                        linkNodesQueue.Enqueue(newLinkedNode);
                                        newLinkedNode.ConnectedEdges.AddRange(this.graph.VerticesDictionary[otherNode]);
                                    }
                                }
                                else
                                {
                                    var newLinkedNode = new LinkNode<VertexType, EdgeValueType>()
                                    {
                                        Node = otherNode
                                    };

                                    linkNodesQueue.Enqueue(newLinkedNode);
                                    newLinkedNode.ConnectedEdges.AddRange(this.graph.VerticesDictionary[otherNode]);
                                }
                            }
                        }
                    }

                    resultCycles.Add(cyclesList);
                    resultComponents.Add(currentGraph);
                }
            }

            return new GraphCyclesComponentsPair<VertexType, EdgeValueType>(resultComponents, resultCycles);
        }

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
            Func<IEdge<VertexType, EdgeValueType>, OuterType> edgeValueFunction,
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
                if (this.graph.VertexEqualityComparer == null)
                {
                    visitedVertices = new Dictionary<VertexType, TreeNode<VertexValuePair<VertexType, OuterType>>>();
                }
                else
                {
                    visitedVertices = new Dictionary<VertexType, TreeNode<VertexValuePair<VertexType, OuterType>>>(
                        graph.VertexEqualityComparer);
                }

                verticesQueue.Enqueue(result.InternalRootNode);
                var processedEdges = new Dictionary<Edge<VertexType, EdgeValueType>, bool>();
                while (verticesQueue.Count > 0)
                {
                    var topTreeNode = verticesQueue.Dequeue();
                    var edges = graph.VerticesDictionary[topTreeNode.NodeObject.Vertex];
                    foreach (var edge in edges)
                    {
                        if (!processedEdges.ContainsKey(edge))
                        {
                            processedEdges.Add(edge, true);
                            var otherNode = edge.InitialVertex;
                            var inverseEdge = true;
                            if (this.graph.VertexEqualityComparer == null)
                            {
                                if (topTreeNode.NodeObject.Vertex.Equals(edge.InitialVertex))
                                {
                                    otherNode = edge.FinalVertex;
                                    inverseEdge = false;
                                }
                            }
                            else
                            {
                                if (this.graph.VertexEqualityComparer.Equals(
                                    topTreeNode.NodeObject.Vertex,
                                    edge.InitialVertex))
                                {
                                    otherNode = edge.FinalVertex;
                                    inverseEdge = false;
                                }
                            }

                            if (!inverseEdge || this.graph.Directed)
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

        /// <summary>
        /// Obtém o ciclo definido por uma lista ligada.
        /// </summary>
        /// <param name="newLinkNode">Um nó da lista.</param>
        /// <param name="directed">Indica se o grafo em questão é dirigido.</param>
        /// <returns>O ciclo.</returns>
        private EdgeListGraph<VertexType, EdgeValueType> GetCycleFromLink(
            LinkNode<VertexType, EdgeValueType> newLinkNode,
            LinkNode<VertexType, EdgeValueType> currentLinkNode,
            bool directed)
        {
            var result = new EdgeListGraph<VertexType, EdgeValueType>(directed);
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
}
