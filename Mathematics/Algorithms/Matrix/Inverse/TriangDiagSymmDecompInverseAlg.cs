namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Algoritmo que permite determinar a inversa de uma matriz simétrica com base no algoritmo da decomposição
    /// de Cholesky.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas das matrizes.</typeparam>
    public class TriangDiagSymmDecompInverseAlg<CoeffType>
        : IAlgorithm<
            TriangDiagSymmMatrixDecompResult<CoeffType>,
            ISquareMatrixFactory<CoeffType>,
            IField<CoeffType>,
            IMathMatrix<CoeffType>>
    {
        /// <summary>
        /// Determina a inversa da matriz correspondente à decomposição especificada.
        /// </summary>
        /// <param name="decompositionResult">A decomposição.</param>
        /// <param name="matrixFactory">A fábrica responsável pela criação da matriz resultante.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os objectos.</param>
        /// <returns>A inversa da matriz associada à decomposição.</returns>
        /// <exception cref="ArgumentNullException">Se algums dos argumentos for nulo.</exception>
        public IMathMatrix<CoeffType> Run(
            TriangDiagSymmMatrixDecompResult<CoeffType> decompositionResult,
            ISquareMatrixFactory<CoeffType> matrixFactory,
            IField<CoeffType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else if (decompositionResult == null)
            {
                throw new ArgumentNullException("decompositionResult");
            }
            else
            {
                var dimension = decompositionResult.UpperTriangularMatrix.GetLength(0);
                for (var i = 0; i < dimension; ++i)
                {
                    var d = decompositionResult.DiagonalMatrix[i, i];
                    if (field.IsAdditiveUnity(d))
                    {
                        throw new MathematicsException("The provided decomposition has no inverse.");
                    }
                }

                var result = matrixFactory.CreateMatrix(dimension);

                for (int j = dimension - 1; j >= 0; --j)
                {
                    // Elemento na diagonal.
                    var i = j;
                    var diagonalValue = field.MultiplicativeInverse(
                        decompositionResult.DiagonalMatrix[i, j]);
                    var k = i + 1;
                    for (; k < dimension; ++k)
                    {
                        var product = field.Multiply(
                            decompositionResult.UpperTriangularMatrix[i, k],
                            result[i, k]);
                        diagonalValue = field.Add(
                            diagonalValue,
                            field.AdditiveInverse(product));
                    }

                    result[i, j] = diagonalValue;
                    --i;
                    while (i >= 0)
                    {
                        k = i + 1;
                        var upperValue = field.Multiply(
                            decompositionResult.UpperTriangularMatrix[i, k],
                            result[k, j]);
                        upperValue = field.AdditiveInverse(upperValue);

                        ++k;
                        for (; k <= j; ++k)
                        {
                            var product = field.Multiply(
                                decompositionResult.UpperTriangularMatrix[i, k],
                                result[k, j]);
                            upperValue = field.Add(
                                upperValue,
                                field.AdditiveInverse(product));
                        }

                        // Como a matriz é simétrica, inverte a orientação.
                        for (; k < dimension; ++k)
                        {
                            var product = field.Multiply(
                                decompositionResult.UpperTriangularMatrix[i, k],
                                result[j, k]);
                            upperValue = field.Add(
                                upperValue,
                                field.AdditiveInverse(product));
                        }

                        result[i, j] = upperValue;
                        --i;
                    }
                }

                this.SetLowerTerms(result);
                return result;
            }
        }

        /// <summary>
        /// Estabelece os valores na parte trangular inferior da matriz.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        private void SetLowerTerms(IMathMatrix<CoeffType> matrix)
        {
            var dimension = matrix.GetLength(0);
            for (int i = 0; i < dimension; ++i)
            {
                for (int j = i+1; j < dimension; ++j)
                {
                    matrix[j, i] = matrix[i, j];
                }
            }
        }
    }
}
