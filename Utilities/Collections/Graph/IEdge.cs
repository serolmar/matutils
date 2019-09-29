namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma aresta geral.
    /// </summary>
    /// <typeparam name="VertexType">O tipo dos objectos que constituem os vértices.</typeparam>
    public interface IEdge<out VertexType>
    {
        /// <summary>
        /// O vértice inicial da aresta.
        /// </summary>
        VertexType InitialVertex { get; }

        /// <summary>
        /// O vértice final da aresta.
        /// </summary>
        VertexType FinalVertex { get; }
    }

    /// <summary>
    /// Define uma aresta com etiqueta.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos associados às arestas.</typeparam>
    public interface ILabeledEdge<out VertexType, out EdgeValueType> 
        : IEdge<VertexType>
    {
        /// <summary>
        /// Obtém o objecto associado à aresta.
        /// </summary>
        EdgeValueType Value { get; }
    }
}
