namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma aresta.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos associados às arestas.</typeparam>
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
