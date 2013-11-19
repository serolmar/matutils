namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Realiza a redução LLL sobre uma base.
    /// </summary>
    /// <typeparam name="VectorType">O tipo do vector.</typeparam>
    /// <typeparam name="FieldCoeffType">O tipo de coeficiente.</typeparam>
    /// <typeparam name="GroupCoeffType">O tipo de aproximante.</typeparam>
    public class LLLBasisReductionAlgorithm<VectorType, FieldCoeffType, GroupCoeffType>
        : IAlgorithm<VectorType[], ReductionSolution<VectorType, FieldCoeffType, GroupCoeffType>>
    {
        /// <summary>
        /// O espaço vectorial responsável pelas operações sobre o vector.
        /// </summary>
        IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace;

        /// <summary>
        /// O objecto responsável pelas multiplicações dos valores aproximados pelo vector.
        /// </summary>
        IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> multiplicationOperation;

        /// <summary>
        /// Obtém a melhor aproximação.
        /// </summary>
        INearest<FieldCoeffType, GroupCoeffType> nearest;

        public LLLBasisReductionAlgorithm(
            IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace,
            IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> multiplicationOperation,
            INearest<FieldCoeffType, GroupCoeffType> nearest)
        {
            if (fieldVectorSpace == null)
            {
                throw new ArgumentNullException("fieldVectorSpace");
            }
            else if (multiplicationOperation == null)
            {
                throw new ArgumentNullException("multiplicationOperation");
            }
            else if (nearest == null)
            {
                throw new ArgumentNullException("nearest");
            }
            else
            {
                this.fieldVectorSpace = fieldVectorSpace;
                this.multiplicationOperation = multiplicationOperation;
                this.nearest = nearest;
            }
        }

        /// <summary>
        /// O espaço vectorial responsável pelas operações sobre o vector.
        /// </summary>
        public IVectorSpace<FieldCoeffType, VectorType> FieldVectorSpace
        {
            get
            {
                return this.fieldVectorSpace;
            }
        }

        /// <summary>
        /// O objecto responsável pelas multiplicações dos valores aproximados pelo vector.
        /// </summary>
        public IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> MultiplicationOperation
        {
            get
            {
                return this.multiplicationOperation;
            }
        }

        /// <summary>
        /// Obtém a melhor aproximação.
        /// </summary>
        public INearest<FieldCoeffType, GroupCoeffType> Nearest
        {
            get
            {
                return this.nearest;
            }
        }

        /// <summary>
        /// Obtém a redução LLL do conunto de vectores.
        /// </summary>
        /// <param name="data">O conjunto de vectores a serem reduzidos.</param>
        /// <returns>O conjunto reduzido e a matriz dos coeficientes que lhe dá origem.</returns>
        public ReductionSolution<VectorType, FieldCoeffType, GroupCoeffType> Run(VectorType[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
            }

            throw new NotImplementedException();
        }
    }
}
