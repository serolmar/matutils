namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Responsável pela aplicação do algoritmo dual.
    /// </summary>
    public class DualHeuristicAlgorithm<ElementType>
        : IAlgorithm<int, DualHeuristicAlgInput<ElementType>, ElementType>
    {
        /// <summary>
        /// Aplica o algoritmo dual aos dados de entrada para um determinado número de referências.
        /// </summary>
        /// <remarks>
        /// Os dados de entrada são alterados pelo algoritmo e podem ser reutilizados em fases posteriores.
        /// </remarks>
        /// <param name="refsNumber">O número de referências.</param>
        /// <param name="input">Os dados de entrada.</param>
        /// <returns>O custo aproximado pela heurística.</returns>
        public ElementType Run(int refsNumber, DualHeuristicAlgInput<ElementType> input)
        {
            throw new NotImplementedException();
        }
    }
}
