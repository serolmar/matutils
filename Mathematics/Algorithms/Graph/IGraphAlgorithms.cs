using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface IGraphAlgorithms<VertexType>
    {
        /// <summary>
        /// Obtém a lista de ciclos simples de um grafo.
        /// </summary>
        /// <returns>A lista de ciclos simples.</returns>
        List<List<IGraph<VertexType>>> GetCycles();

        /// <summary>
        /// Obtém o conjunto de componentes conexas de um grafo.
        /// </summary>
        /// <returns>A lista de componentes conexas.</returns>
        List<IGraph<VertexType>> GetConnectedComponents();

        /// <summary>
        /// Obtém a lista de ciclos simples de um grafo classificados por componentes conexas bem
        /// como as respectivas componentes.
        /// </summary>
        /// <returns>A lista de ciclos simples e as componentes conexas.</returns>
        GraphCyclesComponentsPair<VertexType> GetCyclesAndConnectedComponents();

        /// <summary>
        /// Obtém uma árvore de cobertura mínima que se inicia num vértice.
        /// </summary>
        /// <param name="startVertex">O vértice inicial.</param>
        /// <param name="edgeValueFunction">A função que permite obter o valor do peso da aresta.</param>
        /// <param name="valueComparer">O comparador de valores.</param>
        /// <returns>A árvore.</returns>
        ITree<VertexType> GetMinimumSpanningTree<EdgeValueType>(
            VertexType startVertex, 
            Func<IEdge<VertexType>,EdgeValueType> edgeValueFunction,
            IComparer<EdgeValueType> valueComparer);
    }
}
