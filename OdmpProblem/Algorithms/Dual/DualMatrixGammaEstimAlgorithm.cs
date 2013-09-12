namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Obtém uma estimativa para a variável gama.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados dos custos.</typeparam>
    public class DualMatrixGammaEstimAlgorithm<ElementType>
        : IAlgorithm<SparseDictionaryMatrix<ElementType>, DualHeuristicAlgInput<ElementType>, ElementType>
    {
        public ElementType Run(
            SparseDictionaryMatrix<ElementType> matrix, 
            DualHeuristicAlgInput<ElementType> input)
        {
            throw new NotImplementedException();
        }
    }
}
