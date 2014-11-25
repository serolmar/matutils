﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Algoritmo da decomposição de Cholesky de uma matriz simétrica.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes que constituem as entradas das matrizes.</typeparam>
    public class TriangDiagSymmMatrixDecomposition<CoeffType>
        : IAlgorithm<
            ISquareMatrix<CoeffType>,
            IField<CoeffType>,
            ISquareMatrixFactory<CoeffType>,
            ISquareMatrixFactory<CoeffType>,
            TriangDiagSymmMatrixDecompResult<CoeffType>>
    {
        /// <summary>
        /// Determina o número de ciclos executados antes de verificar se foi determinado
        /// algum valor na diagonal igual a zero.
        /// </summary>
        private const int checkBlockSize = 100000;

        /// <summary>
        /// Obtém a decomposição de uma matriz M=LDL^* onde L é uma matriz triangular e D é uma matriz diagonal.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <param name="upperTriangularMatrixFactory">A fábrica responsável pela criação do contentor para a
        /// matriz triangular.
        /// </param>
        /// <param name="diagonalMatrixFactory">A fábrica para o contentor da matriz diagonal.</param>
        /// <returns>As matrizes triangular e diagonal.</returns>
        public TriangDiagSymmMatrixDecompResult<CoeffType> Run(
            ISquareMatrix<CoeffType> matrix,
            IField<CoeffType> field,
            ISquareMatrixFactory<CoeffType> upperTriangularMatrixFactory,
            ISquareMatrixFactory<CoeffType> diagonalMatrixFactory)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (upperTriangularMatrixFactory == null)
            {
                throw new ArgumentNullException("lowerTriangularMatrixFactory");
            }
            else if (diagonalMatrixFactory == null)
            {
                throw new ArgumentNullException("diagonalMatrixFactory");
            }
            else
            {
                var nullDiagonal = false;
                var matrixDimension = matrix.GetLength(0);
                var triangularMatrix = upperTriangularMatrixFactory.CreateMatrix(
                    matrixDimension,
                    field.AdditiveUnity);
                var diagonalMatrix = diagonalMatrixFactory.CreateMatrix(
                    matrixDimension,
                    field.AdditiveUnity);
                for (int i = 0; i < matrixDimension; ++i)
                {
                    var diagonalTask = new Task(() =>
                    {
                        var sumValue = field.AdditiveUnity;
                        for (int j = 0; j < i; ++j)
                        {
                            var value = triangularMatrix[j, i];
                            value = field.Multiply(value, value);
                            value = field.Multiply(value, diagonalMatrix[j, j]);
                            sumValue = field.Add(sumValue, value);
                        }

                        sumValue = field.Add(
                            matrix[i, i],
                            field.AdditiveInverse(sumValue));
                        diagonalMatrix[i, i] = sumValue;
                        if (field.IsAdditiveUnity(sumValue))
                        {
                            nullDiagonal = true;
                        }
                    });

                    var triangularTask = new Task(() =>
                    {
                        Parallel.For(i + 1, matrixDimension, (j, state) =>
                        {
                            var status = true;
                            var stop = false;
                            var index = checkBlockSize;
                            var sumValue = field.AdditiveUnity;
                            while (status)
                            {
                                if (nullDiagonal)
                                {
                                    stop = true;
                                    status = false;
                                }
                                else
                                {
                                    var limit = i;
                                    if (index < i)
                                    {
                                        limit = index;
                                        index += checkBlockSize;
                                    }
                                    else
                                    {
                                        status = false;
                                    }

                                    for (int k = 0; k < limit; ++k)
                                    {
                                        var value = triangularMatrix[k, i];
                                        value = field.Multiply(value, triangularMatrix[k, j]);
                                        value = field.Multiply(value, diagonalMatrix[k, k]);
                                        sumValue = field.Add(sumValue, value);
                                    }
                                }
                            }

                            if (stop)
                            {
                                state.Break();
                            }
                            else
                            {
                                triangularMatrix[i, j] = field.Add(
                                    matrix[i, j],
                                    field.AdditiveInverse(sumValue));
                            }
                        });
                    });

                    diagonalTask.Start();
                    triangularTask.Start();
                    Task.WaitAll(new[] { diagonalTask, triangularTask });

                    var diagonalValue = diagonalMatrix[i, i];
                    if (field.IsAdditiveUnity(diagonalValue))
                    {
                        // Por conveniência, a linha deverá ser colocada a zero
                        // apesar de qualquer valor poder ser admitido
                        throw new MathematicsException("Error: matrix is singular.");
                    }
                    else
                    {
                        triangularMatrix.ScalarLineMultiplication(
                                i,
                                field.MultiplicativeInverse(diagonalMatrix[i, i]),
                                field);
                    }

                    triangularMatrix[i, i] = field.MultiplicativeUnity;
                }

                return new TriangDiagSymmMatrixDecompResult<CoeffType>(
                    triangularMatrix,
                    diagonalMatrix);
            }
        }
    }
}