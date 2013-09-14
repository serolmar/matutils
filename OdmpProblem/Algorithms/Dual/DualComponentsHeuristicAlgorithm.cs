namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Implementa o algoritmo dual sobre um conjunto de componentes conexas.
    /// </summary>
    /// <typeparam name="ElementType">O tipo de dados.</typeparam>
    public class DualComponentsHeuristicAlgorithm<ElementType>
        : IAlgorithm<int, 
                     List<SparseDictionaryMatrix<ElementType>>, 
                     List<DualHeuristicAlgInput<ElementType>>, ElementType>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os elementos.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// Comparador dos elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        /// <summary>
        /// O objecto responsáel pela aplicação do algoritmo dual a cada uma das componentes em particular.
        /// </summary>
        private IAlgorithm<int, SparseDictionaryMatrix<ElementType>, DualHeuristicAlgInput<ElementType>, ElementType>
            dualAlgorithm;

        public DualComponentsHeuristicAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
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
                this.comparer = comparer;
                this.ring = ring;
                this.dualAlgorithm = new DualHeuristicAlgorithm<ElementType>(
                    comparer,
                    ring);
            }
        }

        /// <summary>
        /// Aplica o algoritmo dual aos dados de entrada para um determinado número de referências.
        /// </summary>
        /// <remarks>
        /// Os dados de entrada são alterados pelo algoritmo e podem ser reutilizados em fases posteriores.
        /// O valor de gama tem de ser previamente estimado.
        /// </remarks>
        /// <param name="refsNumber">O número de referências.</param>
        /// <param name="matrix">A lista das matrizes com os custos.</param>
        /// <param name="inputs">Os dados de entrada.</param>
        /// <returns>O custo aproximado pela heurística.</returns>
        public ElementType Run(
            int refsNumber,
            List<SparseDictionaryMatrix<ElementType>> matrices, 
            List<DualHeuristicAlgInput<ElementType>> inputs)
        {
            if(matrices == null){
                throw new ArgumentNullException("matrices");
            }
            else if (inputs == null)
            {
                throw new ArgumentNullException("inputs");
            }
            else if (matrices.Count != inputs.Count)
            {
                throw new ArgumentException("The number of matrices must match the number of inputs.");
            }
            else
            {
                var result = this.ring.AdditiveUnity;
                for(int i = 0; i < matrices.Count; ++i){
                    var parcell = this.dualAlgorithm.Run(refsNumber, matrices[i], inputs[i]);
                    result = this.ring.Add(result, parcell);
                }

                return result;
            }
        }
    }
}
