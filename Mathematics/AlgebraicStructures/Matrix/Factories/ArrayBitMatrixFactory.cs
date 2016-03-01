namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar instâncias de matrizes <see cref="ArrayBitMathMatrix"/>.
    /// </summary>
    public class ArrayBitMatrixFactory : IMatrixFactory<int>
    {
        /// <summary>
        /// Cria uma matriz <see cref="ArrayBitMathMatrix"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <returns>A matriz.</returns>
        public IMathMatrix<int> CreateMatrix(int lines, int columns)
        {
            return new ArrayBitMathMatrix(lines, columns);
        }

        /// <summary>
        /// Cria uma matriz <see cref="ArrayBitMathMatrix"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz.</returns>
        public IMathMatrix<int> CreateMatrix(int lines, int columns, int defaultValue)
        {
            return new ArrayBitMathMatrix(lines, columns, defaultValue);
        }
    }
}
