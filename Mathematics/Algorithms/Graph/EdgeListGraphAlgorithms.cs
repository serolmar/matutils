using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    internal class EdgeListGraphAlgorithms<VertexType> : IGraphAlgorithms<VertexType>
    {
        /// <summary>
        /// Mantém o grafo sobre o qual actuam os algoritmos.
        /// </summary>
        private EdgeListGraph<VertexType> graph;

        public EdgeListGraphAlgorithms(EdgeListGraph<VertexType> graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// Obtém os ciclos simples do grafo.
        /// </summary>
        /// <returns>Uma lista com os ciclos por componente conexa.</returns>
        public List<List<IGraph<VertexType>>> GetCycles()
        {
            var result = new List<List<IGraph<VertexType>>>();
            var verticesEnumerator = this.graph.VerticesDictionary.GetEnumerator();
            var linkNodesQueue = new Queue<LinkNode<VertexType>>();
            var processedEdges = new Dictionary<IEdge<VertexType>, bool>();
            var visitedNodes = default(Dictionary<VertexType, LinkNode<VertexType>>);
            if (graph.VertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType>>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType>>(graph.VertexEqualityComparer);
            }

            while (verticesEnumerator.MoveNext())
            {
                if (!visitedNodes.ContainsKey(verticesEnumerator.Current.Key))
                {
                    linkNodesQueue.Clear();
                    var nodeToEnqueue = new LinkNode<VertexType>()
                    {
                        Node = verticesEnumerator.Current.Key
                    };

                    nodeToEnqueue.ConnectedEdges.AddRange(verticesEnumerator.Current.Value);
                    linkNodesQueue.Enqueue(nodeToEnqueue);
                    var cyclesList = new List<IGraph<VertexType>>();
                    visitedNodes.Add(
                        verticesEnumerator.Current.Key,
                        new LinkNode<VertexType> { Node = verticesEnumerator.Current.Key });

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
                                    var newLinkedNode = new LinkNode<VertexType>()
                                    {
                                        Node = otherNode,
                                        Link = new LinkNodeTie<VertexType>()
                                        {
                                            OtherNode = currentLinkNode,
                                            TieEdge = edge
                                        }
                                    };

                                    var visitedLinkNode = default(LinkNode<VertexType>);
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
                                    var newLinkedNode = new LinkNode<VertexType>()
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
        public List<IGraph<VertexType>> GetConnectedComponents()
        {
            var result = new List<IGraph<VertexType>>();
            var nodesQueue = new Queue<VertexType>();
            var processedEdges = new Dictionary<IEdge<VertexType>, bool>();
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
                var currentGraph = new EdgeListGraph<VertexType>(graph.Directed);
                var currentVertex = vertexEnumerator.Current;
                currentGraph.AddVertex(currentVertex);
                if (!visitedNodes.ContainsKey(currentVertex))
                {
                    var component = new EdgeListGraph<VertexType>(this.graph.Directed);
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
                                        currentGraph.AddEdge(currentVertex, edgeWithVertex.FinalVertex);
                                        if (!visitedNodes.ContainsKey(edgeWithVertex.FinalVertex))
                                        {
                                            nodesQueue.Enqueue(edgeWithVertex.FinalVertex);
                                            visitedNodes.Add(edgeWithVertex.FinalVertex, true);
                                        }
                                    }
                                    else
                                    {
                                        currentGraph.AddEdge(edgeWithVertex.InitialVertex, currentVertex);
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
        public GraphCyclesComponentsPair<VertexType> GetCyclesAndConnectedComponents()
        {
            var resultCycles = new List<List<IGraph<VertexType>>>();
            var resultComponents = new List<IGraph<VertexType>>();
            var verticesEnumerator = this.graph.VerticesDictionary.GetEnumerator();
            var linkNodesQueue = new Queue<LinkNode<VertexType>>();
            var processedEdges = new Dictionary<IEdge<VertexType>, bool>();
            var visitedNodes = default(Dictionary<VertexType, LinkNode<VertexType>>);
            if (graph.VertexEqualityComparer == null)
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType>>();
            }
            else
            {
                visitedNodes = new Dictionary<VertexType, LinkNode<VertexType>>(graph.VertexEqualityComparer);
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
                    var nodeToEnqueue = new LinkNode<VertexType>()
                    {
                        Node = verticesEnumerator.Current.Key
                    };

                    nodeToEnqueue.ConnectedEdges.AddRange(verticesEnumerator.Current.Value);
                    linkNodesQueue.Enqueue(nodeToEnqueue);
                    var cyclesList = new List<IGraph<VertexType>>();

                    // A componente conexa actual
                    var currentGraph = new EdgeListGraph<VertexType>(graph.Directed);

                    visitedNodes.Add(
                        verticesEnumerator.Current.Key,
                        new LinkNode<VertexType> { Node = verticesEnumerator.Current.Key });
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
                                        currentGraph.AddEdge(edge.InitialVertex, edge.FinalVertex);
                                        otherNode = edge.FinalVertex;
                                        inverseEdge = false;
                                    }
                                    else
                                    {
                                        currentGraph.AddEdge(edge.InitialVertex, currentLinkNode.Node);
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
                                    var newLinkedNode = new LinkNode<VertexType>()
                                    {
                                        Node = otherNode,
                                        Link = new LinkNodeTie<VertexType>()
                                        {
                                            OtherNode = currentLinkNode,
                                            TieEdge = edge
                                        }
                                    };

                                    var visitedLinkNode = default(LinkNode<VertexType>);
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
                                    var newLinkedNode = new LinkNode<VertexType>()
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

            return new GraphCyclesComponentsPair<VertexType>(resultComponents, resultCycles);
        }

        /// <summary>
        /// Obtém uma árvore de cobertura mínima que se inicia num vértice.
        /// </summary>
        /// <param name="startVertex">O vértice inicial.</param>
        /// <param name="edgeValueFunction">A função que permite obter o valor do peso da aresta.</param>
        /// <param name="valueComparer">O comparador de valores.</param>
        /// <returns>A árvore.</returns>
        public ITree<VertexType> GetMinimumSpanningTree<EdgeValueType>(
            VertexType startVertex,
            Func<IEdge<VertexType>, EdgeValueType> edgeValueFunction,
            IComparer<EdgeValueType> valueComparer)
        {
            IComparer<EdgeValueType> innerValueComparer = Comparer<EdgeValueType>.Default;
            if (valueComparer != null)
            {
                innerValueComparer = valueComparer;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o ciclo definido por uma lista ligada.
        /// </summary>
        /// <param name="newLinkNode">Um nó da lista.</param>
        /// <param name="directed">Indica se o grafo em questão é dirigido.</param>
        /// <returns>O ciclo.</returns>
        private EdgeListGraph<VertexType> GetCycleFromLink(
            LinkNode<VertexType> newLinkNode, 
            LinkNode<VertexType> currentLinkNode,
            bool directed)
        {
            var result = new EdgeListGraph<VertexType>(directed);
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
