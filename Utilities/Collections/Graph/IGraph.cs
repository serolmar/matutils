namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um grafo.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeType">O tipo de objectos que constituem as arastas.</typeparam>
    public interface IEdgeGraph<VertexType, EdgeType>
        where EdgeType : IEdge<VertexType>
    {
        /// <summary>
        /// Obtém um enumerador para todos os vértices.
        /// </summary>
        IEnumerable<VertexType> Vertices { get; }

        /// <summary>
        /// Obtém um enumerador para todas as arestas.
        /// </summary>
        IEnumerable<EdgeType> Edges { get; }

        /// <summary>
        /// Obtém os vizinhos de um determinado vértice.
        /// </summary>
        /// <param name="vertex">O vértice.</param>
        /// <returns>Os vizinhos do vértice.</returns>
        IEnumerable<VertexType> GetNeighbours(VertexType vertex);

        /// <summary>
        /// Adiciona um vértice.
        /// </summary>
        /// <param name="vertex">O vértice a ser adicionado.</param>
        void AddVertex(VertexType vertex);

        /// <summary>
        /// Adiciona uma aresta.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial da aresta.</param>
        /// <param name="finalVertex">O vértice final da aresta.</param>
        void AddEdge(VertexType initialVertex, VertexType finalVertex);

        /// <summary>
        /// Adiciona uma aresta.
        /// </summary>
        /// <param name="edge">A aresta.</param>
        void AddEdge(EdgeType edge);

        /// <summary>
        /// Remove um vértice.
        /// </summary>
        /// <param name="vertex">O vértice a ser removido.</param>
        void RemoveVertex(VertexType vertex);

        /// <summary>
        /// Remove todas as arestas definidas entre dois vértices.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial das arestas.</param>
        /// <param name="finalVertex">O vértice final das arestas.</param>
        void RemoveEdges(VertexType initialVertex, VertexType finalVertex);
    }

    /// <summary>
    /// Define um grafo cujas arestas possuem etiqueta.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos associados às arestas.</typeparam>
    public interface ILabeledEdgeGraph<VertexType, EdgeValueType>
        : IEdgeGraph<VertexType, ILabeledEdge<VertexType,EdgeValueType>>
    {
        /// <summary>
        /// Adiciona uma aresta.
        /// </summary>
        /// <param name="initialVertex">O vértice inicial da aresta.</param>
        /// <param name="finalVertex">O vértice final da aresta.</param>
        /// <param name="edgeValue">O objecto associado à aresta.</param>
        void AddEdge(VertexType initialVertex, VertexType finalVertex, EdgeValueType edgeValue);
    }
}
