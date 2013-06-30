using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class Edge<VertexType> : IEdge<VertexType>
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
        /// Permite instanciar uma aresta entre dois vértices.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial.</param>
        /// <param name="finalVertex">O vértice final.</param>
        public Edge(VertexType initialVertex, VertexType finalVertex)
        {
            this.initialVertex = initialVertex;
            this.finalVertex = finalVertex;
        }

        /// <summary>
        /// Obtém o vértice inicial.
        /// </summary>
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
        public VertexType FinalVertex
        {
            get
            {
                return this.finalVertex;
            }
        }
    }
}
