

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Grafo representado por uma lista de arestas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo que representa o vértice.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo do objecto associado à aresta.</typeparam>
    public class EdgeListGraph<VertexType, EdgeValueType> : IGraph<VertexType, EdgeValueType>
    {
        /// <summary>
        /// Indica se o grafo é direccionado.
        /// </summary>
        private bool directed = false;

        /// <summary>
        /// O comparador de vértices.
        /// </summary>
        private IEqualityComparer<VertexType> vertexEqualityComparer;

        private Dictionary<VertexType, List<Edge<VertexType, EdgeValueType>>> vertices;

        private List<Edge<VertexType, EdgeValueType>> edges;

        public EdgeListGraph(bool directed = false)
        {
            this.edges = new List<Edge<VertexType, EdgeValueType>>();
            this.vertices = new Dictionary<VertexType, List<Edge<VertexType, EdgeValueType>>>();
            this.directed = directed;
        }

        public EdgeListGraph(IEqualityComparer<VertexType> vertexEqualityComparer, bool directed = false)
        {
            this.edges = new List<Edge<VertexType, EdgeValueType>>();
            this.vertices = new Dictionary<VertexType, List<Edge<VertexType, EdgeValueType>>>(vertexEqualityComparer);
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
        public IEnumerable<IEdge<VertexType, EdgeValueType>> Edges
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
        internal Dictionary<VertexType, List<Edge<VertexType, EdgeValueType>>> VerticesDictionary
        {
            get
            {
                return this.vertices;
            }
        }

        /// <summary>
        /// Obtém a lista de arestas.
        /// </summary>
        internal List<Edge<VertexType, EdgeValueType>> EdgesList
        {
            get
            {
                return this.edges;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pelos algoritmos sobre o grafo actual.
        /// </summary>
        /// <returns>O objecto responsável pelos alroritmos.</returns>
        public IGraphAlgorithms<VertexType, EdgeValueType> GetAlgorithmsProcessor()
        {
            return new EdgeListGraphAlgorithms<VertexType, EdgeValueType>(this);
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
            var result = default(List<Edge<VertexType, EdgeValueType>>);
            if (!this.vertices.TryGetValue(vertex, out result))
            {
                result = new List<Edge<VertexType, EdgeValueType>>();
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

        /// <summary>
        /// Adiciona um vértice ao grafo.
        /// </summary>
        /// <param name="vertex">O vértice.</param>
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
                this.vertices.Add(vertex, new List<Edge<VertexType, EdgeValueType>>());
            }
        }

        /// <summary>
        /// Adiciona uma aresta ao grafo.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        /// <param name="value">O valor associado à aresta.</param>
        public void AddEdge(VertexType initialVertex, VertexType finalVertex, EdgeValueType value)
        {
            if (initialVertex == null)
            {
                throw new ArgumentNullException("initialVertex");
            }
            else if (finalVertex == null)
            {
                throw new ArgumentNullException("finalVertex");
            }

            var edge = new Edge<VertexType, EdgeValueType>(initialVertex, finalVertex, value);
            this.AddEdge(edge);
        }

        /// <summary>
        /// Remove um vértice do grafo.
        /// </summary>
        /// <param name="vertex">O vértice a ser removido.</param>
        public void RemoveVertex(VertexType vertex)
        {
            var vertexEdge = default(List<Edge<VertexType, EdgeValueType>>);
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
            var initialVertexEdges = default(List<Edge<VertexType, EdgeValueType>>);
            if (this.vertices.TryGetValue(initialVertex, out initialVertexEdges))
            {
                var finalVertexEdges = default(List<Edge<VertexType, EdgeValueType>>);
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

        /// <summary>
        /// Adiciona uma aresta ao grafo actual.
        /// </summary>
        /// <param name="edge">A aresta a ser adicionada.</param>
        internal void AddEdge(Edge<VertexType, EdgeValueType> edge)
        {
            this.edges.Add(edge);
            var edgesWithVertex = default(List<Edge<VertexType, EdgeValueType>>);
            if (this.vertices.TryGetValue(edge.InitialVertex, out edgesWithVertex))
            {
                edgesWithVertex.Add(edge);
            }
            else
            {
                this.vertices.Add(edge.InitialVertex, new List<Edge<VertexType, EdgeValueType>>() { edge });
            }

            edgesWithVertex = null;
            if (this.vertices.TryGetValue(edge.FinalVertex, out edgesWithVertex))
            {
                edgesWithVertex.Add(edge);
            }
            else
            {
                this.vertices.Add(edge.FinalVertex, new List<Edge<VertexType, EdgeValueType>>() { edge });
            }
        }
    }
}
