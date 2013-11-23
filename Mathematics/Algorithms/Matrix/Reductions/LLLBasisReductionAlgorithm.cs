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
        : IAlgorithm<VectorType[], FieldCoeffType, FieldCoeffType, VectorType[]>
    {
        /// <summary>
        /// O espaço vectorial responsável pelas operações sobre o vector.
        /// </summary>
        private IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace;

        /// <summary>
        /// O objecto responsável pelas multiplicações dos valores aproximados pelo vector.
        /// </summary>
        private IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> groupVectorMultOperation;

        /// <summary>
        /// O objecto reponsável pela multiplicação entre elementos do grupo e do corpo.
        /// </summary>
        private IMultiplicationOperation<GroupCoeffType, FieldCoeffType, FieldCoeffType> groupFieldMultOperation;

        /// <summary>
        /// O objecto responsável pelos produtos escalares.
        /// </summary>
        private IScalarProductSpace<VectorType, FieldCoeffType> scalarProd;

        /// <summary>
        /// Permite comparar coeficientes bem como determinar o respectivo módulo.
        /// </summary>
        private INormSpace<FieldCoeffType, FieldCoeffType> fieldNormSpace;

        /// <summary>
        /// Obtém a melhor aproximação.
        /// </summary>
        private INearest<FieldCoeffType, GroupCoeffType> nearest;

        public LLLBasisReductionAlgorithm(
            IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace,
            IMultiplicationOperation<GroupCoeffType, VectorType, VectorType> groupVectorMultOperation,
            IMultiplicationOperation<GroupCoeffType, FieldCoeffType, FieldCoeffType> groupFieldMultOperation,
            IScalarProductSpace<VectorType, FieldCoeffType> scalarProd,
            INormSpace<FieldCoeffType, FieldCoeffType> fieldNormSpace,
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
            else if (scalarProd == null)
            {
                throw new ArgumentNullException("scalarProd");
            }
            else if (fieldNormSpace == null)
            {
                throw new ArgumentNullException("fieldNormSpace");
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
                this.scalarProd = scalarProd;
                this.fieldNormSpace = fieldNormSpace;
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
        /// <param name="reductionCoeff">O coeficiente associado à redução.</param>
        /// <param name="halfValue">O valor correspondente a metade da unidade.</param>
        /// <returns>O conjunto reduzido.</returns>
        public VectorType[] Run(
            VectorType[] data, 
            FieldCoeffType reductionCoeff,
            FieldCoeffType halfValue)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else if (reductionCoeff == null)
            {
                throw new ArgumentNullException("reductionCoeff");
            }
            else if (halfValue == null)
            {
                throw new ArgumentNullException("halfValue");
            }
            else if (data.Length == 0)
            {
                // Nada há a fazer sem dados.
                return data;
            }
            else
            {
                var bVec = new VectorType[data.Length];
                Array.Copy(data, bVec, data.Length);
                var bStarVec = new VectorType[data.Length];
                var uCoeffs = new ArraySquareMatrix<FieldCoeffType>(data.Length);
                for (int i = 0; i < data.Length; ++i)
                {
                    for (int j = 0; j < data.Length; ++j)
                    {
                        if (i == j)
                        {
                            uCoeffs[i, j] = this.fieldVectorSpace.Field.MultiplicativeUnity;
                        }
                        else
                        {
                            uCoeffs[i, j] = this.fieldVectorSpace.Field.AdditiveUnity;
                        }

                        bStarVec[i] = bVec[i];
                    }

                    for (int j = 0; j < i - 1; ++j)
                    {
                        var quo = this.scalarProd.Multiply(bStarVec[j], bStarVec[j]);
                        quo = this.fieldVectorSpace.Field.MultiplicativeInverse(quo);
                        uCoeffs[i, j] = this.scalarProd.Multiply(bVec[i], bStarVec[j]);
                        uCoeffs[i, j] = this.fieldVectorSpace.Field.Multiply(uCoeffs[i, j], quo);

                        var tempVec = this.fieldVectorSpace.MultiplyScalar(uCoeffs[i, j], bStarVec[j]);
                        tempVec = this.fieldVectorSpace.AdditiveInverse(tempVec);
                        bStarVec[i] = this.fieldVectorSpace.Add(bStarVec[i], tempVec);
                    }

                    this.Reduce(i, bVec, uCoeffs);
                }

                var index = 0;
                var last = data.Length - 1;
                while (index < last)
                {
                    var bStarSquaredNorm = this.scalarProd.Multiply(bStarVec[index], bStarVec[index]);
                    var nextBSttarSquaredNorm = this.scalarProd.Multiply(
                        bStarVec[index + 1],
                        bStarVec[index + 1]);
                    var comparisionValue = this.fieldVectorSpace.Field.Multiply(reductionCoeff, nextBSttarSquaredNorm);
                    if (this.fieldNormSpace.Compare(bStarSquaredNorm, comparisionValue) <= 0)
                    {
                        bStarSquaredNorm = nextBSttarSquaredNorm;
                        ++index;
                    }
                    else
                    {
                        var tempBStar = this.fieldVectorSpace.MultiplyScalar(uCoeffs[index, index], bStarVec[index]);
                        bStarVec[index] = this.fieldVectorSpace.Add(bStarVec[index], tempBStar);
                        var quo = this.scalarProd.Multiply(bStarVec[index + 1], bStarVec[index + 1]);
                        quo = this.fieldVectorSpace.Field.MultiplicativeInverse(quo);
                        uCoeffs[index, index] = this.scalarProd.Multiply(bVec[index], bStarVec[index + 1]);
                        uCoeffs[index, index] = this.fieldVectorSpace.Field.Multiply(
                            uCoeffs[index, index],
                            quo);
                        uCoeffs[index, index + 1] = this.fieldVectorSpace.Field.MultiplicativeUnity;
                        uCoeffs[index + 1, index + 1] = this.fieldVectorSpace.Field.AdditiveUnity;
                        tempBStar = this.fieldVectorSpace.MultiplyScalar(uCoeffs[index, index], bStarVec[index + 1]);
                        tempBStar = this.fieldVectorSpace.AdditiveInverse(tempBStar);
                        bStarVec[index] = this.fieldVectorSpace.Add(bStarVec[index], tempBStar);

                        // Realiza a troca dos coeficientes.
                        uCoeffs.SwapLines(index, index + 1);
                        this.SwapVectors(index, index + 1, bVec);
                        this.SwapVectors(index, index + 1, bStarVec);

                        var firstQuo = this.scalarProd.Multiply(bStarVec[index], bStarVec[index]);
                        firstQuo = this.fieldVectorSpace.Field.MultiplicativeInverse(firstQuo);
                        var secondQuo = this.scalarProd.Multiply(
                            bStarVec[index + 1],
                            bStarVec[index + 1]);
                        secondQuo = this.fieldVectorSpace.Field.MultiplicativeInverse(secondQuo);
                        for (var k = index + 2; k < data.Length; ++k)
                        {
                            uCoeffs[k, index] = this.scalarProd.Multiply(bVec[k], bStarVec[index]);
                            uCoeffs[k, index] = this.fieldVectorSpace.Field.Multiply(uCoeffs[k, index], firstQuo);
                            uCoeffs[k, index + 1] = this.scalarProd.Multiply(bVec[k], bStarVec[index + 1]);
                            uCoeffs[k, index + 1] = this.fieldVectorSpace.Field.Multiply(uCoeffs[k, index + 1], secondQuo);
                        }

                        // Teste de redução e actualização do ciclo
                        var coeffNorm = this.fieldNormSpace.GetNorm(uCoeffs[index + 1, index]);
                        if (this.fieldNormSpace.Compare(uCoeffs[index + 1, index], halfValue) > 0)
                        {
                            this.Reduce(index + 1, bVec, uCoeffs);
                        }

                        --index;
                        if (index < 0)
                        {
                            index = 0;
                        }
                    }
                }

                return bVec;
            }
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
            for (int i = index - 2; i > 0; --i)
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

        /// <summary>
        /// Permite trocar os vectores nas posições especificadas.
        /// </summary>
        /// <param name="firstIndex">A posição do primeiro vector a ser trocado.</param>
        /// <param name="secondIndex">A segunda posição do vector a ser trocado.</param>
        /// <param name="vectors">O conjunto de vectores.</param>
        private void SwapVectors(int firstIndex, int secondIndex, VectorType[] vectors)
        {
            var swap = vectors[firstIndex];
            vectors[firstIndex] = vectors[secondIndex];
            vectors[secondIndex] = swap;
        }
    }
}
