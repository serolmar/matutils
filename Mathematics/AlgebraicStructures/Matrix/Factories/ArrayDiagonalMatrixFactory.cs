// -----------------------------------------------------------------------
// <copyright file="ArrayDiagonalMatrixFactory.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Fábrica de matrizes quadradas diagonais.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayDiagonalMatrixFactory<CoeffType> : ISquareMatrixFactory<CoeffType>
    {
        /// <summary>
        /// Cria uma matriz quadrada com a dimensão especificada.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <returns>A matriz quadrada.</returns>
        public IMathMatrix<CoeffType> CreateMatrix(int dimension)
        {
            return new ArrayDiagonalMathMatrix<CoeffType>(dimension);
        }

        /// <summary>
        /// Cria uma matriz quadrada com a dimensão especificada.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor que surgirá por defeito.</param>
        /// <returns>A matriz quadrada.</returns>
        public IMathMatrix<CoeffType> CreateMatrix(int dimension, CoeffType defaultValue)
        {
            return new ArrayDiagonalMathMatrix<CoeffType>(dimension, defaultValue);
        }
    }
}
