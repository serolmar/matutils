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
        : ILLLBasisReductionAlgorithm<VectorType, FieldCoeffType>
    {
        /// <summary>
        /// O espaço vectorial responsável pelas operações sobre o vector.
        /// </summary>
        private IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace;

        /// <summary>
        /// O objecto responsável pelos produtos escalares.
        /// </summary>
        private IScalarProductSpace<VectorType, FieldCoeffType> scalarProd;
        /// <summary>
        /// Permite obter a melhor aproximação.
        /// </summary>
        private INearest<FieldCoeffType, FieldCoeffType> nearest;

        /// <summary>
        /// Permite a comparação entre dois coeficientes.
        /// </summary>
        private IComparer<FieldCoeffType> fieldCoeffTypeComparer;

        public LLLBasisReductionAlgorithm(
            IVectorSpace<FieldCoeffType, VectorType> fieldVectorSpace,
            IScalarProductSpace<VectorType, FieldCoeffType> scalarProd,
            INearest<FieldCoeffType, FieldCoeffType> nearest,
            IComparer<FieldCoeffType> fieldCoeffTypeComparer)
        {
            if (fieldVectorSpace == null)
            {
                throw new ArgumentNullException("fieldVectorSpace");
            }
            else if (scalarProd == null)
            {
                throw new ArgumentNullException("scalarProd");
            }
            else if (nearest == null)
            {
                throw new ArgumentNullException("nearest");
            }
            else if (fieldCoeffTypeComparer == null)
            {
                throw new ArgumentNullException("fieldCoeffTypeComparer");
            }
            else
            {
                this.fieldVectorSpace = fieldVectorSpace;
                this.scalarProd = scalarProd;
                this.nearest = nearest;
                this.fieldCoeffTypeComparer = fieldCoeffTypeComparer;
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
        /// Obtém a melhor aproximação.
        /// </summary>
        public INearest<FieldCoeffType, FieldCoeffType> Nearest
        {
            get
            {
                return this.nearest;
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pela determinação do produto escalar entre dois vectores.
        /// </summary>
        public IScalarProductSpace<VectorType, FieldCoeffType> ScalarProduct
        {
            get
            {
                return this.scalarProd;
            }
        }

        /// <summary>
        /// Obtém a redução LLL do conunto de vectores.
        /// </summary>
        /// <remarks>
        /// Se um valor for inferior a 1/2, então a respectiva aproximação inteira será o valor zero. Esta é uma
        /// verificação possível para averiguar se um coeficiente se encontra dentro desses limites.
        /// </remarks>
        /// <param name="data">O conjunto de vectores a serem reduzidos.</param>
        /// <param name="reductionCoeff">O coeficiente associado à redução entre 1/4 e 1, normalmente 3/4.</param>
        /// <returns>O conjunto reduzido.</returns>
        public VectorType[] Run(
            VectorType[] data,
            FieldCoeffType reductionCoeff)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else if (reductionCoeff == null)
            {
                throw new ArgumentNullException("reductionCoeff");
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
                var uCoeffs = new FieldCoeffType[data.Length][];
                var bNorm = new FieldCoeffType[data.Length];
                bStarVec[0] = bVec[0];
                bNorm[0] = this.scalarProd.Multiply(bStarVec[0], bStarVec[0]);
                for (int i = 1; i < data.Length; ++i)
                {
                    uCoeffs[i] = new FieldCoeffType[i];
                    bStarVec[i] = bVec[i];
                    for (int j = 0; j < i; ++j)
                    {
                        uCoeffs[i][j] = this.scalarProd.Multiply(bVec[i], bStarVec[j]);
                        uCoeffs[i][j] = this.fieldVectorSpace.Field.Multiply(
                            uCoeffs[i][j],
                            this.fieldVectorSpace.Field.MultiplicativeInverse(bNorm[j]));
                        var symmetric = this.fieldVectorSpace.Field.AdditiveInverse(uCoeffs[i][j]);
                        bStarVec[i] = this.fieldVectorSpace.Add(
                            bStarVec[i],
                            this.fieldVectorSpace.MultiplyScalar(symmetric, bStarVec[j]));
                    }

                    bNorm[i] = this.scalarProd.Multiply(bStarVec[i], bStarVec[i]);
                }

                var k = 1;
                while (k < data.Length)
                {
                    // Redução do tamanho do vector
                    for (int i = k - 1; i >= 0; --i)
                    {
                        var nearestCoeff = this.nearest.GetNearest(uCoeffs[k][i]);
                        if (!this.fieldVectorSpace.Field.IsAdditiveUnity(nearestCoeff))
                        {
                            var invNearestCoeff = this.fieldVectorSpace.Field.AdditiveInverse(nearestCoeff);
                            var tempVec = this.fieldVectorSpace.MultiplyScalar(invNearestCoeff, bVec[i]);
                            bVec[k] = this.fieldVectorSpace.Add(
                                bVec[k],
                                tempVec);
                            
                            // Actualiza todos os coeficientes
                            for (int j = 0; j < i; ++j)
                            {
                                var tempProduct = this.fieldVectorSpace.Field.Multiply(
                                    uCoeffs[i][j],
                                    nearestCoeff);
                                uCoeffs[k][i] = this.fieldVectorSpace.Field.Add(
                                    uCoeffs[i][j],
                                    tempProduct);
                            }

                            uCoeffs[k][i] = this.fieldVectorSpace.Field.Add(
                                uCoeffs[k][i],
                                this.fieldVectorSpace.Field.AdditiveInverse(nearestCoeff));
                        }
                    }

                    // Verifica a condição
                    var currentCoeff = uCoeffs[k][k-1];
                    var compareValue = this.fieldVectorSpace.Field.AdditiveInverse(
                        this.fieldVectorSpace.Field.Multiply(currentCoeff, currentCoeff));
                    compareValue = this.fieldVectorSpace.Field.Add(
                        reductionCoeff,
                        compareValue);
                    compareValue = this.fieldVectorSpace.Field.Multiply(
                        compareValue,
                        bNorm[k - 1]);
                    if (this.fieldCoeffTypeComparer.Compare(bNorm[k], compareValue) >= 0)
                    {
                        ++k;
                    }
                    else
                    {
                        // Troca os dois vectores
                        var swap = bVec[k];
                        bVec[k] = bVec[k - 1];
                        bVec[k - 1] = swap;

                        // Actualiza os restantes coeficientes e vectores
                        for (int i = k - 1; i <= k; ++i)
                        {
                            bStarVec[i] = bVec[i];
                            for (int j = 0; j < i; ++j)
                            {
                                uCoeffs[i][j] = this.scalarProd.Multiply(bVec[i], bStarVec[j]);
                                uCoeffs[i][j] = this.fieldVectorSpace.Field.Multiply(
                                    uCoeffs[i][j],
                                    this.fieldVectorSpace.Field.MultiplicativeInverse(bNorm[j]));
                                var symmetric = this.fieldVectorSpace.Field.AdditiveInverse(uCoeffs[i][j]);
                                bStarVec[i] = this.fieldVectorSpace.Add(
                                    bStarVec[i],
                                    this.fieldVectorSpace.MultiplyScalar(symmetric, bStarVec[j]));
                            }

                            bNorm[i] = this.scalarProd.Multiply(bStarVec[i], bStarVec[i]);
                        }

                        for (int i = k + 1; i < data.Length; ++i)
                        {
                            // Para k - 1
                            uCoeffs[i][k-1] = this.scalarProd.Multiply(bVec[i], bStarVec[k-1]);
                            uCoeffs[i][k-1] = this.fieldVectorSpace.Field.Multiply(
                                uCoeffs[i][k-1],
                                this.fieldVectorSpace.Field.MultiplicativeInverse(bNorm[k-1]));

                            // Para k
                            uCoeffs[i][k] = this.scalarProd.Multiply(bVec[i], bStarVec[k]);
                            uCoeffs[i][k] = this.fieldVectorSpace.Field.Multiply(
                                uCoeffs[i][k],
                                this.fieldVectorSpace.Field.MultiplicativeInverse(bNorm[k]));
                        }

                        if (k > 1)
                        {
                            --k;
                        }
                    }
                }

                return bVec;
            }
        }
    }
}
