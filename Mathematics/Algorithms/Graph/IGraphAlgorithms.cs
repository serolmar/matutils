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
        /// <param name="weight">Um dicionário que permite mapear o peso das arestas.</param>
        /// <returns>A árvore.</returns>
        IGraph<VertexType> GetMinimumSpanningTree(
            VertexType startVertex, 
            Dictionary<IEdge<VertexType>, double> weight);
    }
}
