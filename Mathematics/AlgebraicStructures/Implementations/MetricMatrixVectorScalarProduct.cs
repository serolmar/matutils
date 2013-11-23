﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    /// <summary>
    /// Permite determinar o produto escalar de dois vectores tendo em consideração a matriz
    /// dos coeficientes métricos.
    /// </summary>
    /// <remarks>
    /// Nenhuma validação é realizada sobre a matriz.
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo de dados que consituem as entradas dos vectores.</typeparam>
    public class MetricMatrixVectorScalarProduct<CoeffType> 
        : IScalarProductSpace<IVector<CoeffType>, CoeffType>
    {
        private IRing<CoeffType> ring;

        private IMatrix<CoeffType> metricMatrix;

        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IComparer<CoeffType> comparer;

        public MetricMatrixVectorScalarProduct(
            IMatrix<CoeffType> metricMatrix,
            IComparer<CoeffType> comparer, 
            IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if(metricMatrix == null)
            {
                throw new ArgumentNullException("metricMatrix");
            }
            else if (metricMatrix.GetLength(0) == metricMatrix.GetLength(1))
            {
                if (comparer == null)
                {
                    this.comparer = Comparer<CoeffType>.Default;
                }
                else
                {
                    this.comparer = comparer;
                }

                this.metricMatrix = metricMatrix;
                this.ring = ring;
            }
            else
            {
                throw new ArgumentException("The metric coefficients matrix must be square.");
            }
        }

        /// <summary>
        /// Obtém o anel responsável pelas operações sobre as entradas dos vectores.
        /// </summary>
        public IRing<CoeffType> Ring
        {
            get
            {
                return this.ring;
            }
        }

        /// <summary>
        /// Obtém a matriz dos coeficientes métricos.
        /// </summary>
        public IMatrix<CoeffType> MetricMatrix
        {
            get
            {
                return this.metricMatrix;
            }
        }

        /// <summary>
        /// Determina o produto escalar entre dois vectores dados os coeficientes métricos.
        /// </summary>
        /// <param name="left">O primeiro vector.</param>
        /// <param name="right">O segundo vector.</param>
        /// <returns>O resultado do produto escalar.</returns>
        public CoeffType Multiply(IVector<CoeffType> left, IVector<CoeffType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var matrixDimensions = this.metricMatrix.GetLength(0);
                var leftLength = left.Length;
                var rightLenth = right.Length;
                if (leftLength != matrixDimensions)
                {
                    throw new ArgumentException("The left vector must have the same rank as metric coefficients matrix.");
                }
                else if (rightLenth != matrixDimensions)
                {
                    throw new ArgumentException("The right vector must have the same rank as metric coefficients matrix.");
                }
                else
                {
                    var result = this.ring.AdditiveUnity;
                    for (int i = 0; i < matrixDimensions; ++i)
                    {
                        var leftValue = left[i];
                        if (!this.ring.IsAdditiveUnity(leftValue))
                        {
                            for (int j = 0; j < matrixDimensions; ++j)
                            {
                                var rightValue = right[j];
                                var metricValue = this.metricMatrix[i,j];
                                if (!this.ring.IsAdditiveUnity(rightValue) &&
                                    !this.ring.IsAdditiveUnity(metricValue))
                                {
                                    var value = this.ring.Multiply(metricValue, rightValue);
                                    value = this.ring.Multiply(leftValue, value);
                                    result = this.ring.Add(result, value);
                                }
                            }
                        }
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Compara dois valores para averiguar se são iguais ou se um é maior do que o outro.
        /// </summary>
        /// <param name="x">O primeiro valor a ser comparado.</param>
        /// <param name="y">O segundo valor a ser comparado.</param>
        /// <returns>
        /// O valor 1 caso o primeiro seja maior do que o segundo, 0 caso sejam iguais e -1 caso o segundo
        /// seja menor que o primeiro.
        /// </returns>
        public int Compare(CoeffType x, CoeffType y)
        {
            return this.comparer.Compare(x, y);
        }
    }
}
