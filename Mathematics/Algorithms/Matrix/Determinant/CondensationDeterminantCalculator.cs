using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mathematics.Algorithms;

namespace Mathematics
{
    /// <summary>
    /// Calcula o valor do determinante com o recurso ao método da condensação.
    /// </summary>
    /// <remarks>
    /// O método da condensação consiste num método mais eficaz no que concerne ao cálculo
    /// do valor do determinante de uma matriz. Porém, este método requer uma alteração do
    /// objecto sobre o qual actua, passando a ser necessária uma réplica do mesmo em memória.
    /// </remarks>
    /// <typeparam name="ObjectType">O tipo de objeto a ser processado.</typeparam>
    /// <typeparam name="FieldType">O tipo do corpo que permite efectuar as operações sobre os elementos.</typeparam>
    public class CondensationDeterminantCalculator<ObjectType> : ADeterminant<ObjectType>
    {
        private IEuclidenDomain<ObjectType> domain;

        public CondensationDeterminantCalculator(IEuclidenDomain<ObjectType> domain)
            : base(domain)
        {
            this.domain = domain;
        }

        protected override ObjectType ComputeDeterminant(IMatrix<ObjectType> data)
        {
            var positiveResult = true;
            var determinantFactors = new List<ObjectType>();
            var matrixDimension = data.GetLength(0);
            if (matrixDimension == 0)
            {
                return this.ring.AdditiveUnity;
            }
            else
            {
                // Copia a matriz uma vez que o algoritmo a vai alterar
                var temporaryArray = new ArrayMatrix<ObjectType>(matrixDimension, matrixDimension);
                for (int i = 0; i < matrixDimension; ++i)
                {
                    for (int j = 0; j < matrixDimension; ++j)
                    {
                        temporaryArray[i, j] = data[i, j];
                    }
                }

                for (int i = 0; i < matrixDimension - 1; ++i)
                {
                    var pivotValue = temporaryArray[i, i];
                    if (this.ring.IsAdditiveUnity(pivotValue))
                    {
                        var nextPivotCandidate = this.GetNextNonEmptyPivotLineNumber(i, temporaryArray);
                        if (nextPivotCandidate == -1)
                        {
                            return this.ring.AdditiveUnity;
                        }
                        else
                        {
                            positiveResult = !positiveResult;
                            temporaryArray.SwapLines(i, nextPivotCandidate);
                            pivotValue = temporaryArray[i, i];
                        }
                    }

                    if (this.ring.IsMultiplicativeUnity(pivotValue))
                    {
                        for (int j = i + 1; j < matrixDimension; ++j)
                        {
                            var value = temporaryArray[j, i];
                            if (!this.ring.IsAdditiveUnity(value))
                            {
                                temporaryArray[j, i] = this.ring.AdditiveUnity;
                                for (int k = i + 1; k < matrixDimension; ++k)
                                {
                                    var temp = this.ring.Multiply(value, temporaryArray[i, k]);
                                    temporaryArray[j, k] = this.ring.Add(temporaryArray[j, k], this.ring.AdditiveInverse(temp));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = i + 1; j < matrixDimension; ++j)
                        {
                            var value = temporaryArray[j, i];
                            if (!this.ring.IsAdditiveUnity(value))
                            {
                                var gcd = MathFunctions.GreatCommonDivisor(pivotValue, value, this.domain);
                                var lcm = this.ring.Multiply(pivotValue, value);
                                lcm = this.domain.Quo(lcm, gcd);
                                var pivotCoffactor = this.domain.Quo(pivotValue, gcd);
                                var valueCoffactor = this.domain.Quo(value, gcd);
                                determinantFactors.Add(pivotCoffactor);
                                temporaryArray[j, i] = this.ring.AdditiveUnity;
                                for (int k = i + 1; k < matrixDimension; ++k)
                                {
                                    var positive = this.ring.Multiply(pivotCoffactor, temporaryArray[j, k]);
                                    var negative = this.ring.Multiply(valueCoffactor, temporaryArray[i, k]);
                                    temporaryArray[j, k] = this.ring.Add(positive, this.ring.AdditiveInverse(negative));
                                }
                            }
                        }
                    }
                }

                return this.GetDeterminantValue(temporaryArray, determinantFactors, positiveResult);
            }
        }

        /// <summary>
        /// Obtém o próximo pivô não nulo da matriz.
        /// </summary>
        /// <param name="startLine">A linha onde é iniciada a pesquisa.</param>
        /// <param name="data">A matriz.</param>
        /// <returns>A linha cujo pivô seja não nulo e -1 caso não exista.</returns>
        private int GetNextNonEmptyPivotLineNumber(int startLine, IMatrix<ObjectType> data)
        {
            var result = -1;
            var matrixDimension = data.GetLength(0);
            for (int i = startLine + 1; i < matrixDimension; ++i)
            {
                if (!this.ring.IsAdditiveUnity(data[i, startLine]))
                {
                    result = i;
                    i = matrixDimension;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o valor final do determinante.
        /// </summary>
        /// <param name="triangularMatrix">A matriz triangular resultante.</param>
        /// <param name="divisors">A lista de factores.</param>
        /// <param name="sign">O sinal.</param>
        /// <returns>O resultado do cálculo.</returns>
        private ObjectType GetDeterminantValue(ArrayMatrix<ObjectType> triangularMatrix, List<ObjectType> divisors, bool sign)
        {
            var dimensions = triangularMatrix.GetLength(0);
            var result = triangularMatrix[0, 0];
            for (int i = 1; i < dimensions; ++i)
            {
                result = this.ring.Multiply(result, triangularMatrix[i, i]);
            }

            for (int i = 0; i < divisors.Count; ++i)
            {
                result = this.domain.Quo(result, divisors[i]);
            }

            if (!sign)
            {
                result = this.ring.AdditiveInverse(result);
            }

            return result;
        }
    }
}
