using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IEdge<out VertexType, out EdgeValueType>
    {
        /// <summary>
        /// O vértice inicial da aresta.
        /// </summary>
        VertexType InitialVertex { get; }

        /// <summary>
        /// O vértice final da aresta.
        /// </summary>
        VertexType FinalVertex { get; }

        /// <summary>
        /// Obtém o objecto associado à aresta.
        /// </summary>
        EdgeValueType Value { get; }
    }
}
