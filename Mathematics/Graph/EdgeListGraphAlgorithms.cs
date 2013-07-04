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
            List<List<IGraph<VertexType>>> result = new List<List<IGraph<VertexType>>>();
            var verticesEnumerator = this.graph.VerticesDictionary.GetEnumerator();
            var linkNodesQueue = new Queue<LinkNode<VertexType>>();
            while (verticesEnumerator.MoveNext())
            {
                linkNodesQueue.Clear();
                linkNodesQueue.Enqueue(new LinkNode<VertexType>() { Node = verticesEnumerator.Current.Key });
                var cyclesList = new List<IGraph<VertexType>>();

                Dictionary<VertexType, bool> visitedNodes = null;
                if (graph.VertexEqualityComparer == null)
                {
                    visitedNodes = new Dictionary<VertexType, bool>();
                }
                else
                {
                    visitedNodes = new Dictionary<VertexType, bool>(graph.VertexEqualityComparer);
                }

                visitedNodes.Add(verticesEnumerator.Current.Key, true);
                while (linkNodesQueue.Count > 0)
                {
                    var currentLinkNode = linkNodesQueue.Dequeue();
                    var edges = this.graph.VerticesDictionary[currentLinkNode.Node];
                    foreach (var edge in edges)
                    {
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
                            if (this.graph.VertexEqualityComparer.Equals(currentLinkNode.Node, edge.InitialVertex))
                            {
                                otherNode = edge.FinalVertex;
                                inverseEdge = false;
                            }
                        }

                        if (!inverseEdge || !this.graph.Directed)
                        {
                            var newLinkedNode = new LinkNode<VertexType>() { Node = otherNode, Link = currentLinkNode };
                            if (visitedNodes.ContainsKey(otherNode))
                            {
                                var cycle = this.GetCycleFromLink(newLinkedNode, this.graph.Directed);
                                cyclesList.Add(cycle);
                            }
                            else
                            {
                                visitedNodes.Add(otherNode, true);
                                linkNodesQueue.Enqueue(newLinkedNode);
                            }
                        }
                    }
                }

                result.Add(cyclesList);
            }

            return result;
        }

        /// <summary>
        /// Obtém todos os sub-grafos que são componentes conexas.
        /// </summary>
        /// <returns>As componentes conexas.</returns>
        public List<IGraph<VertexType>> GetConnectedComponents()
        {
            List<IGraph<VertexType>> result = new List<IGraph<VertexType>>();
            var nodesQueue = new Queue<VertexType>();
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
                        visitedNodes.Add(currentVertex, true);
                        var edges = this.graph.VerticesDictionary[currentVertex];
                        foreach (var edgeWithVertex in edges)
                        {
                            if (this.graph.VertexEqualityComparer != null)
                            {
                                if (this.graph.VertexEqualityComparer.Equals(currentVertex, edgeWithVertex.InitialVertex))
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
                        }
                    }

                    result.Add(currentGraph);
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a árvore de cobertura mínima.
        /// </summary>
        /// <param name="startVertex">O vértice raiz.</param>
        /// <param name="weight">Um dicionário com o peso das arestas.</param>
        /// <returns>A árvore de cobertura mínima.</returns>
        public IGraph<VertexType> GetMinimumSpanningTree(VertexType startVertex, Dictionary<IEdge<VertexType>, double> weight)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o ciclo definido por uma lista ligada.
        /// </summary>
        /// <param name="linkNode">Um nó da lista.</param>
        /// <param name="directed">Indica se o grafo em questão é dirigido.</param>
        /// <returns>O ciclo.</returns>
        private EdgeListGraph<VertexType> GetCycleFromLink(LinkNode<VertexType> linkNode, bool directed)
        {
            var result = new EdgeListGraph<VertexType>();
            var startLinkNode = linkNode;
            var nextLinkNode = linkNode.Link;
            result.AddEdge(nextLinkNode.Node, startLinkNode.Node);

            var state = true;
            while (state)
            {
                var previousNode = nextLinkNode.Node;
                nextLinkNode = nextLinkNode.Link;
                result.AddEdge(nextLinkNode.Node, previousNode);

                if (this.graph.VertexEqualityComparer == null)
                {
                    if (nextLinkNode.Node.Equals(startLinkNode.Node))
                    {
                        state = false;
                    }
                }
                else if (this.graph.VertexEqualityComparer.Equals(nextLinkNode.Node, startLinkNode.Node))
                {
                    state = false;
                }
            }

            return result;
        }
    }
}
