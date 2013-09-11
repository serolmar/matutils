namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Implementa o algoritmo de inicialização.
    /// </summary>
    public class DualHeuristicInitAlgorithm<ElementType>
        : IAlgorithm<SparseDictionaryMatrix<ElementType>, 
                     GreedyAlgSolution<ElementType>, 
                     DualHeuristicAlgInput<ElementType>, bool>
    {
        /// <summary>
        /// Obtém os valores iniciais das variáveis lambda.
        /// </summary>
        /// <param name="matrix">A matriz dos custos.</param>
        /// <param name="greedySolution">A solução do algoritmo guloso.</param>
        /// <param name="dualInput">A entrada para o algoritmo dual propriamente dito.</param>
        /// <returns>Verdadeiro caso o algoritmo seja bem sucedido e falso caso contrário.</returns>
        public bool Run(
            SparseDictionaryMatrix<ElementType> matrix, 
            GreedyAlgSolution<ElementType> greedySolution, 
            DualHeuristicAlgInput<ElementType> dualInput)
        {
            throw new NotImplementedException();
        }
    }
}
