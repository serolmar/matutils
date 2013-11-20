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
        : IAlgorithm<VectorType[], VectorType[]>
    {
        /// <summary>
        /// O espaço vectorial responsável pelas operações sobre o vector.
        /// </summary>
        IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace;

        /// <summary>
        /// O objecto responsável pelas multiplicações dos valores aproximados pelo vector.
        /// </summary>
        IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> groupVectorMultOperation;

        /// <summary>
        /// O objecto reponsável pela multiplicação entre elementos do grupo e do corpo.
        /// </summary>
        IMultiplicationOperation<GroupCoeffType, FieldCoeffType, FieldCoeffType> groupFieldMultOperation;

        /// <summary>
        /// Obtém a melhor aproximação.
        /// </summary>
        INearest<FieldCoeffType, GroupCoeffType> nearest;

        public LLLBasisReductionAlgorithm(
            IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace,
            IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> groupVectorMultOperation,
            IMultiplicationOperation<GroupCoeffType, FieldCoeffType, FieldCoeffType> groupFieldMultOperation,
            INearest<FieldCoeffType, GroupCoeffType> nearest)
        {
            if (fieldVectorSpace == null)
            {
                throw new ArgumentNullException("fieldVectorSpace");
            }
            else if (groupVectorMultOperation == null)
            {
                throw new ArgumentNullException("groupVectorMultOperation");
            }
            else if (groupFieldMultOperation == null)
            {
                throw new ArgumentNullException("groupFieldMultOperation");
            }
            else if (nearest == null)
            {
                throw new ArgumentNullException("nearest");
            }
            else
            {
                this.fieldVectorSpace = fieldVectorSpace;
                this.groupVectorMultOperation = groupVectorMultOperation;
                this.groupFieldMultOperation = groupFieldMultOperation;
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
        public IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> GroupVectorMultOperation
        {
            get
            {
                return this.groupVectorMultOperation;
            }
        }

        /// <summary>
        /// Obtém objecto reponsável pela multiplicação entre elementos do grupo e do corpo.
        /// </summary>
        IMultiplicationOperation<GroupCoeffType, FieldCoeffType, FieldCoeffType> GroupFieldMultOperation
        {
            get
            {
                return this.groupFieldMultOperation;
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
        /// <returns>O conjunto reduzido.</returns>
        public VectorType[] Run(VectorType[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var bVec = new VectorType[data.Length];
                Array.Copy(data, bVec, data.Length);
                var bStarVec = new VectorType[data.Length];
                var uCoeffs = new ArraySquareMatrix<FieldCoeffType>(data.Length);

            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Aplica a redução aos vectores da base resultante tendo em consideração os coeficientes da
        /// ortogonalização.
        /// </summary>
        /// <param name="index">O indice a partir do qual a redução se aplica.</param>
        /// <param name="bVec">O conjunto de vectores a serem reduzidos.</param>
        /// <param name="uCoeffs">O conjunto de coeficientes.</param>
        private void Reduce(
            int index,
            VectorType[] bVec, 
            ArraySquareMatrix<FieldCoeffType> uCoeffs)
        {
            for (int i = index - 1 - 1; i > 0; --i)
            {
                var nearest = this.nearest.GetNearest(uCoeffs[index, i]);
                var tempRound = this.groupVectorMultOperation.Multiply(
                    nearest,
                    bVec[i]);
                tempRound = this.fieldVectorSpace.AdditiveInverse(tempRound);
                bVec[index] = this.fieldVectorSpace.Add(bVec[index], tempRound);

                var length = uCoeffs.GetLength(0);
                for (int j = 0; j < length; ++j)
                {
                    if (!this.fieldVectorSpace.Field.IsAdditiveUnity(uCoeffs[i, j]))
                    {
                        var temp = this.groupFieldMultOperation.Multiply(nearest, uCoeffs[i, j]);
                        temp = this.fieldVectorSpace.Field.AdditiveInverse(temp);
                        uCoeffs[i, j] = this.fieldVectorSpace.Field.Add(uCoeffs[i, j], temp);
                    }
                }
            }
        }
    }
}
