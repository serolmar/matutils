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
        :IAlgorithm<List<SparseDictionaryMatrix<ElementType>>, 
                    List<DualHeuristicAlgInput<ElementType>>, List<ElementType>>
    {
        /// <summary>
        /// O anel responsável pelas operações.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// O comparador de elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        IAlgorithm<SparseDictionaryMatrix<ElementType>, DualHeuristicAlgInput<ElementType>, ElementType>
            initializeAlgorithm;

        public DualCompsGammaEstimAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
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
                this.initializeAlgorithm = new DualMatrixGammaEstimAlgorithm<ElementType>(
                    comparer,
                    ring);
            }
        }

        /// <summary>
        /// Inicializa as variáveis relativas a várias componentes.
        /// </summary>
        /// <param name="matrices">As matrizes.</param>
        /// <param name="inputs">Os dados de entrada.</param>
        /// <returns>A lista das variáveis gama calculadas por componente.</returns>
        public List<ElementType> Run(
            List<SparseDictionaryMatrix<ElementType>> matrices, 
            List<DualHeuristicAlgInput<ElementType>> inputs)
        {
            if (matrices == null)
            {
                throw new ArgumentNullException("matrices");
            }
            else if (inputs == null)
            {
                throw new ArgumentNullException("input");
            }
            else if (matrices.Count != inputs.Count)
            {
                throw new ArgumentException("The number of matrices must match the number of inputs.");
            }
            else
            {
                var result = new List<ElementType>();
                var gamma = this.ring.AdditiveUnity;
                for(int i = 0; i<matrices.Count;++i)
                {
                    var currentGamma =this.initializeAlgorithm.Run(matrices[i], inputs[i]);
                    result.Add(currentGamma);
                    if (this.comparer.Compare(gamma, currentGamma) < 0)
                    {
                        gamma = currentGamma;
                    }
                }

                foreach (var input in inputs)
                {
                    input.Gamma = gamma;
                }

                return result;
            }
        }
    }
}
