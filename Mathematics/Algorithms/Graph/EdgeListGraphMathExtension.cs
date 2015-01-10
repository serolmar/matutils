namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa uma extensão ao grafo baseado em listas de arestas de modo
    /// a incluir alguns algoritmos.
    /// </summary>
    /// <typeparam name="VertexType">O tipo dos objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo dos objectos que constituem as arestas.</typeparam>
    public class EdgeListGraphMathExtensions<VertexType, EdgeValueType>
        : EdgeListGraph<VertexType, EdgeValueType>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo 
        /// <see cref="EdgeListGraphMathExtensions{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public EdgeListGraphMathExtensions(bool directed = false) : base(directed) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo 
        /// <see cref="EdgeListGraphMathExtensions{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="vertexEqualityComparer">O comparador de vértices.</param>
        /// <param name="directed">Verdadeiro caso o grafo seja dirigido e falso caso contrário.</param>
        public EdgeListGraphMathExtensions(
            IEqualityComparer<VertexType> vertexEqualityComparer,
            bool directed = false)
            : base(vertexEqualityComparer, directed) { }

        /// <summary>
        /// Obtém o comparador de vértices associado ao grafo.
        /// </summary>
        /// <value>
        /// O comparador.
        /// </value>
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
        /// <value>
        /// O dicionário.
        /// </value>
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
        /// <value>
        /// A lista de arestas.
        /// </value>
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
    }
}
