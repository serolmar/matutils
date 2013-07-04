using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
    /// <summary>
    /// Grafo representado por uma lista de arestas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo que representa o vértice.</typeparam>
    class EdgeListGraph<VertexType> : IGraph<VertexType>
    {
        /// <summary>
        /// Indica se o grafo é direccionado.
        /// </summary>
        private bool directed = false;

        /// <summary>
        /// O comparador de vértices.
        /// </summary>
        private IEqualityComparer<VertexType> vertexEqualityComparer;

        private Dictionary<VertexType, List<IEdge<VertexType>>> vertices;

        private List<IEdge<VertexType>> edges;

        public EdgeListGraph(bool directed = false)
        {
            this.edges = new List<IEdge<VertexType>>();
            this.vertices = new Dictionary<VertexType, List<IEdge<VertexType>>>();
            this.directed = directed;
        }

        public EdgeListGraph(IEqualityComparer<VertexType> vertexEqualityComparer, bool directed = false)
        {
            this.edges = new List<IEdge<VertexType>>();
            this.vertices = new Dictionary<VertexType, List<IEdge<VertexType>>>(vertexEqualityComparer);
            this.vertexEqualityComparer = vertexEqualityComparer;
            this.directed = directed;
        }

        /// <summary>
        /// Obtém um enumerador para o conjunto de vértices.
        /// </summary>
        public IEnumerable<VertexType> Vertices
        {
            get
            {
                foreach (var kvp in this.vertices)
                {
                    yield return kvp.Key;
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador para o conjunto de arestas.
        /// </summary>
        public IEnumerable<IEdge<VertexType>> Edges
        {
            get
            {
                return this.edges;
            }
        }

        /// <summary>
        /// Obtém um valor indicando se o grafo é dirigido.
        /// </summary>
        public bool Directed
        {
            get
            {
                return this.directed;
            }
        }

        /// <summary>
        /// Obtém o comparador de vértices associado ao grafo.
        /// </summary>
        internal IEqualityComparer<VertexType> VertexEqualityComparer
        {
            get
            {
                return this.vertexEqualityComparer;
            }
        }

        /// <summary>
        /// Obtém o dicionário que contém os vértices e as arestas que daí saem.
        /// </summary>
        internal Dictionary<VertexType, List<IEdge<VertexType>>> VerticesDictionary
        {
            get
            {
                return this.vertices;
            }
        }

        /// <summary>
        /// Obtém a lista de arestas.
        /// </summary>
        internal List<IEdge<VertexType>> EdgesList
        {
            get
            {
                return this.edges;
            }
        }

        /// <summary>
        /// Retorna os vizinhos de um determinado vértice.
        /// </summary>
        /// <remarks>
        /// Se o grafo for dirigido, os vizinhos de um vértice são todos os vértices que são
        /// atingíveis por uma aresta com início nesse vértice.
        /// </remarks>
        /// <param name="vertex">O vértice.</param>
        /// <returns>O conjunto dos vértices vizinhos.</returns>
        public IEnumerable<VertexType> GetNeighbours(VertexType vertex)
        {
            List<IEdge<VertexType>> result = null;
            if (!this.vertices.TryGetValue(vertex, out result))
            {
                result = new List<IEdge<VertexType>>();
            }

            foreach (var edge in result)
            {
                if (this.vertexEqualityComparer != null)
                {
                    if (this.vertexEqualityComparer.Equals(vertex, edge.InitialVertex))
                    {
                        yield return edge.FinalVertex;
                    }
                    else if (!this.directed)
                    {
                        yield return edge.InitialVertex;
                    }
                }
                else
                {
                    if (vertex.Equals(edge.InitialVertex))
                    {
                        yield return edge.FinalVertex;
                    }
                    else if (!this.directed)
                    {
                        yield return edge.InitialVertex;
                    }
                }
            }
        }

        public void AddVertex(VertexType vertex)
        {
            if (vertex == null)
            {
                throw new ArgumentNullException("vertex");
            }
            else if (this.vertices.ContainsKey(vertex))
            {
                throw new MathematicsException("Vertex was already inserted.");
            }
            else
            {
                this.vertices.Add(vertex, new List<IEdge<VertexType>>());
            }
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        public void AddEdge(VertexType initialVertex, VertexType finalVertex)
        {
            if (initialVertex == null)
            {
                throw new ArgumentNullException("initialVertex");
            }
            else if (finalVertex == null)
            {
                throw new ArgumentNullException("finalVertex");
            }

            var edge = new Edge<VertexType>(initialVertex, finalVertex);
            this.edges.Add(edge);
            List<IEdge<VertexType>> edgesWithVertex = null;
            if (this.vertices.TryGetValue(initialVertex, out edgesWithVertex))
            {
                edgesWithVertex.Add(edge);
            }
            else
            {
                this.vertices.Add(initialVertex, new List<IEdge<VertexType>>() { edge });
            }

            edgesWithVertex = null;
            if (this.vertices.TryGetValue(finalVertex, out edgesWithVertex))
            {
                edgesWithVertex.Add(edge);
            }
            else
            {
                this.vertices.Add(initialVertex, new List<IEdge<VertexType>>() { edge });
            }

        }

        /// <summary>
        /// Remove um vértice do grafo.
        /// </summary>
        /// <param name="vertex">O vértice a ser removido.</param>
        public void RemoveVertex(VertexType vertex)
        {
            List<IEdge<VertexType>> vertexEdge = null;
            if (this.vertices.TryGetValue(vertex, out vertexEdge))
            {
                while (vertexEdge.Count > 0)
                {
                    this.edges.Remove(vertexEdge[0]);
                    vertexEdge.RemoveAt(0);
                }

                this.vertices.Remove(vertex);
            }
        }

        /// <summary>
        /// Remove o conjunto de arestas com origem num vértice e extremidade no outro.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        public void RemoveEdges(VertexType initialVertex, VertexType finalVertex)
        {
            List<IEdge<VertexType>> initialVertexEdges = null;
            if (this.vertices.TryGetValue(initialVertex, out initialVertexEdges))
            {
                List<IEdge<VertexType>> finalVertexEdges = null;
                if (this.vertices.TryGetValue(finalVertex, out finalVertexEdges))
                {
                    var count = 0;
                    while (count < initialVertexEdges.Count)
                    {
                        var currentEdge = initialVertexEdges[count];
                        if (this.vertexEqualityComparer == null)
                        {
                            if (currentEdge.InitialVertex.Equals(initialVertex) && currentEdge.FinalVertex.Equals(finalVertex))
                            {
                                initialVertexEdges.RemoveAt(count);
                            }
                            else
                            {
                                ++count;
                            }
                        }
                        else
                        {
                            if (this.vertexEqualityComparer.Equals(currentEdge.InitialVertex, initialVertex) &&
                                this.vertexEqualityComparer.Equals(currentEdge.FinalVertex, finalVertex))
                            {
                                initialVertexEdges.RemoveAt(count);
                            }
                            else
                            {
                                ++count;
                            }
                        }

                        finalVertexEdges.Remove(currentEdge);
                        this.edges.Remove(currentEdge);
                    }
                }
            }
        }
    }
}
