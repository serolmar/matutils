namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Obtém uma estimativa para a variável gama a partir de um conjunto de componentes conexas.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados dos custos.</typeparam>
    public class DualCompsGammaEstimAlgorithm<ElementType>
        :IAlgorithm<List<SparseDictionaryMatrix<ElementType>>, DualHeuristicAlgInput<ElementType>, ElementType>
    {
        public ElementType Run(
            List<SparseDictionaryMatrix<ElementType>> matrices, 
            DualHeuristicAlgInput<ElementType> input)
        {
            throw new NotImplementedException();
        }
    }
}
