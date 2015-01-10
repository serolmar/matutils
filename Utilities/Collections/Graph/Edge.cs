namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa a aresta de um grafo.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos associados aos vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos associado às arestas.</typeparam>
    public class Edge<VertexType, EdgeValueType> : IEdge<VertexType, EdgeValueType>
    {
        /// <summary>
        /// O vértice inicial da aresta.
        /// </summary>
        private VertexType initialVertex;

        /// <summary>
        /// O vértice final da aresta.
        /// </summary>
        private VertexType finalVertex;

        /// <summary>
        /// O objecto associado à aresta.
        /// </summary>
        private EdgeValueType value;

        /// <summary>
        /// Permite instanciar uma aresta entre dois vértices.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        /// <param name="value">O valor associado.</param>
        public Edge(VertexType initialVertex, VertexType finalVertex, EdgeValueType value)
        {
            this.initialVertex = initialVertex;
            this.finalVertex = finalVertex;
            this.value = value;
        }

        /// <summary>
        /// Obtém o vértice inicial.
        /// </summary>
        /// <value>O vértice inicial.</value>
        public VertexType InitialVertex
        {
            get
            {
                return this.initialVertex;
            }
        }

        /// <summary>
        /// Obtém o vértice final.
        /// </summary>
        /// <value>O vérticie final.</value>
        public VertexType FinalVertex
        {
            get
            {
                return this.finalVertex;
            }
        }

        /// <summary>
        /// Obtém o objecto associado à aresta.
        /// </summary>
        /// <value>O objecto na aresta.</value>
        public EdgeValueType Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
