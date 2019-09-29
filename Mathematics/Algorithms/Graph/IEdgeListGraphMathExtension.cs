// -----------------------------------------------------------------------
// <copyright file="IEdgeListGraphMathExtension.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Define um processador de algoritmos para grafos cujas arestas
    /// não necessitem de etiqueta.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeType">O tipo de objectos que constituem as arestas.</typeparam>
    /// <typeparam name="GraphType">O tipo de objectos que constituem o grafo.</typeparam>
    public interface IEdgeListGraphAlgorithmsProcessor<VertexType, EdgeType, GraphType>
        where EdgeType : IEdge<VertexType>
        where GraphType : IEdgeGraph<VertexType, EdgeType>
    {
        /// <summary>
        /// Obtém a lista de ciclos simples de um grafo.
        /// </summary>
        /// <returns>A lista de ciclos simples.</returns>
        List<List<GraphType>> GetCycles();

        /// <summary>
        /// Obtém o conjunto de componentes conexas de um grafo.
        /// </summary>
        /// <returns>A lista de componentes conexas.</returns>
        List<GraphType> GetConnectedComponents();

        /// <summary>
        /// Obtém a lista de ciclos simples de um grafo classificados por componentes conexas bem
        /// como as respectivas componentes.
        /// </summary>
        /// <returns>A lista de ciclos simples e as componentes conexas.</returns>
        GraphCyclesComponentsPair<VertexType, EdgeType, GraphType> GetCyclesAndConnectedComponents();
    }

    /// <summary>
    /// Define um processador de algoritmos para grafos cujas arestas
    /// requeiram etiquetas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">
    /// O tipo de objectos que constituem as etiquetas das arestas.
    /// </typeparam>
    public interface ILabeledEdgeListGraphAlgorithmsProcessor<VertexType, EdgeValueType>
        : IEdgeListGraphAlgorithmsProcessor<VertexType, ILabeledEdge<VertexType, EdgeValueType>, LabeledEdgeListGraphMathExtensions<VertexType, EdgeValueType>>
    {
        /// <summary>
        /// Obtém uma árvore de cobertura mínima que se inicia num vértice.
        /// </summary>
        /// <typeparam name="OuterType">O tipo de objectos que constituem os pesos das arestas.</typeparam>
        /// <param name="startVertex">O vértice inicial.</param>
        /// <param name="edgeValueFunction">A função que permite obter o valor do peso da aresta.</param>
        /// <param name="valueComparer">O comparador de valores.</param>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>A árvore.</returns>
        ITree<VertexValuePair<VertexType, OuterType>> GetMinimumSpanningTree<OuterType>(
            VertexType startVertex,
            Func<ILabeledEdge<VertexType, EdgeValueType>, OuterType> edgeValueFunction,
            IComparer<OuterType> valueComparer,
            IMonoid<OuterType> monoid);
    }

    /// <summary>
    /// Define uma extensão do grafo que suporta algoritmos que não necessitem
    /// de etiquetas nas arestas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem o vértice.</typeparam>
    /// <typeparam name="EdgeType">O tipo dos objectos que constituem as arestas.</typeparam>
    /// <typeparam name="GraphType">O tipo de obejctos que constituem o grafo.</typeparam>
    public interface IEdgeListGraphMathExtension<VertexType, EdgeType, GraphType>
        where EdgeType : IEdge<VertexType>
        where GraphType : IEdgeGraph<VertexType, EdgeType>
    {
        /// <summary>
        /// Obtém o processador de algoritmos sobre o grafo cujas arestas
        /// não necessitam de etiqueta.
        /// </summary>
        /// <returns>O processador de algoritmos.</returns>
        IEdgeListGraphAlgorithmsProcessor<VertexType, EdgeType, GraphType> GetAlgorithmsProcessor();
    }

    /// <summary>
    /// Define uma extensão do grafo que suporta algoritmos que necessitam
    /// de etiquetas nas arestas.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos que constituem as etiquetas.</typeparam>
    /// <typeparam name="GraphType">
    /// O tipo de objectos que constitui o tipo de grafo a ser retornado.
    /// </typeparam>
    public interface ILabeledEdgeListGraphMathExtension<VertexType, EdgeValueType, GraphType>
        : IEdgeListGraphMathExtension<VertexType, ILabeledEdge<VertexType, EdgeValueType>, GraphType>
        where GraphType : ILabeledEdgeGraph<VertexType, EdgeValueType>
    {
        /// <summary>
        /// Obtém o processador de algoritmos sobre o grafo cujas arestas
        /// necessitam de etiqueta.
        /// </summary>
        /// <returns>O processador de algoritmos.</returns>
        ILabeledEdgeListGraphAlgorithmsProcessor<VertexType, EdgeValueType> GetLabeledAlgorithmsProcessor();
    }
}
