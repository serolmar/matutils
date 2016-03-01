namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;

    /// <summary>
    /// Implementa o algoritmo que permite obter uma solução a partir de outra quando o problema
    /// se encontra dividido em componentes conexas.
    /// </summary>
    /// <remarks>
    /// Este algoritmo pode ser usado tanto como subalgoritmo do algoritmo dual bem como
    /// na obtenção de uma solução admissível a partir do resultado do algoritmo dual.
    /// </remarks>
    public class GreedyCompsCorrectorAlgorithm<ElementType>
        : IAlgorithm<int,
                   List<SparseDictionaryMathMatrix<ElementType>>, 
                   List<GreedyAlgSolution<ElementType>>, 
                   ElementType[][],
                   List<GreedyAlgSolution<ElementType>>>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os elementos.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// O comparador responsável pela comparação dos elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        /// <summary>
        /// Mantém o algoritmo que permite a obtenção da próxima referência.
        /// </summary>
        private IAlgorithm<
                            IntegerSequence,
                            SparseDictionaryMathMatrix<ElementType>,
                            ElementType[],
                            Tuple<int, ElementType>> directRefGetAlgorithm;

        /// <summary>
        /// Mantém o algoritmo que permite a obtenção da próxima referência.
        /// </summary>
        private IAlgorithm<
                            IntegerSequence,
                            SparseDictionaryMathMatrix<ElementType>,
                            ElementType[],
                            Tuple<int, ElementType>> inverseRefGetAlgorithm;

        public GreedyCompsCorrectorAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.ring = ring;
                this.comparer = comparer;
                this.directRefGetAlgorithm = new DirectGreedyRefGetAlgorithm<ElementType>(
                    comparer,
                    ring);
                this.inverseRefGetAlgorithm = new InverseGreedyRefGetAlgorithm<ElementType>(
                    comparer,
                    ring);
            }
        }

        /// <summary>
        /// Obtém uma solução corrigida a partir de uma inicial.
        /// </summary>
        /// <param name="refsNumber">O número de referências.</param>
        /// <param name="matrix">A matriz.</param>
        /// <param name="solution">A solução inicial.</param>
        /// <param name="lineBoards">O conjunto de linhas corrigidas por componente conexa.</param>
        /// <returns>A solução corrigida.</returns>
        public List<GreedyAlgSolution<ElementType>> Run(
            int refsNumber, 
            List<SparseDictionaryMathMatrix<ElementType>> matrix, 
            List<GreedyAlgSolution<ElementType>> solution, 
            ElementType[][] lineBoards)
        {
            throw new NotImplementedException();
        }
    }
}
