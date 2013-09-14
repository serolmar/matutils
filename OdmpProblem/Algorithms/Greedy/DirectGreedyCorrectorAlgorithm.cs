namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities.Collections;

    /// <summary>
    /// Implementa o algoritmo que permite obter uma solução a partir de outra.
    /// </summary>
    /// <remarks>
    /// Este algoritmo pode ser usado tanto como subalgoritmo do algoritmo dual bem como
    /// na obtenção de uma solução admissível a partir do resultado do algoritmo dual.
    /// </remarks>
    public class DirectGreedyCorrectorAlgorithm<ElementType> :
        IAlgorithm<int, 
                   SparseDictionaryMatrixFactory<ElementType>, 
                   GreedyAlgSolution<ElementType>, 
                   ElementType[],
                   Tuple<int, ElementType>>
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
                            SparseDictionaryMatrix<ElementType>,
                            ElementType[],
                            Tuple<int, ElementType>> refGetAlgorithm;

        public DirectGreedyCorrectorAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
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
                this.refGetAlgorithm = new DirectGreedyRefGetAlgorithm<ElementType>(
                    comparer,
                    ring);
            }
        }

        /// <summary>
        /// Obtém uma solução corrigida a partir de uma inicial.
        /// </summary>
        /// <param name="refsNumber">O número de referências.</param>
        /// <param name="matrix">A matriz.</param>
        /// <param name="initialSolution">A solução inicial.</param>
        /// <param name="initialLineBoard">A linha condensada inicial.</param>
        /// <returns>
        /// O índice da linha escolhida e o respectivo custo ou -1 e o valor por defeito caso contrário.
        /// </returns>
        public Tuple<int, ElementType> Run(
            int refsNumber, 
            SparseDictionaryMatrixFactory<ElementType> matrix, 
            GreedyAlgSolution<ElementType> initialSolution, 
            ElementType[] initialLineBoard)
        {
            if (refsNumber < 1)
            {
                throw new ArgumentException("The number of references must be at least one.");
            }
            else if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (initialSolution == null)
            {
                throw new ArgumentNullException("initialSolution");
            }
            else if (initialLineBoard == null)
            {
                throw new ArgumentNullException("initialLineBoard");
            }
            else
            {
            }

            throw new NotImplementedException();
        }
    }
}
