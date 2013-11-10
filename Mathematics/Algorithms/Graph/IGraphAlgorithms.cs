using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IGraphAlgorithms<VertexType, EdgeValueType>
    {
        /// <summary>
        /// Obtém a lista de ciclos simples de um grafo.
        /// </summary>
        /// <returns>A lista de ciclos simples.</returns>
        List<List<IGraph<VertexType, EdgeValueType>>> GetCycles();

        /// <summary>
        /// Obtém o conjunto de componentes conexas de um grafo.
        /// </summary>
        /// <returns>A lista de componentes conexas.</returns>
        List<IGraph<VertexType, EdgeValueType>> GetConnectedComponents();

        /// <summary>
        /// Obtém a lista de ciclos simples de um grafo classificados por componentes conexas bem
        /// como as respectivas componentes.
        /// </summary>
        /// <returns>A lista de ciclos simples e as componentes conexas.</returns>
        GraphCyclesComponentsPair<VertexType, EdgeValueType> GetCyclesAndConnectedComponents();

        /// <summary>
        /// Obtém uma árvore de cobertura mínima que se inicia num vértice.
        /// </summary>
        /// <param name="startVertex">O vértice inicial.</param>
        /// <param name="edgeValueFunction">A função que permite obter o valor do peso da aresta.</param>
        /// <param name="valueComparer">O comparador de valores.</param>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>A árvore.</returns>
        ITree<VertexValuePair<VertexType, OuterType>> GetMinimumSpanningTree<OuterType>(
            VertexType startVertex,
            Func<IEdge<VertexType, EdgeValueType>, OuterType> edgeValueFunction,
            IComparer<OuterType> valueComparer,
            IMonoid<OuterType> monoid);
    }
}
