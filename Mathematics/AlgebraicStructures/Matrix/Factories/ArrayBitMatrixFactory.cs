namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar instâncias de matrizes <see cref="ArrayBitMatrix"/>.
    /// </summary>
    public class ArrayBitMatrixFactory : IMatrixFactory<int>
    {
        /// <summary>
        /// Cria uma matriz <see cref="ArrayBitMatrix"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <returns>A matriz.</returns>
        public IMatrix<int> CreateMatrix(int lines, int columns)
        {
            return new ArrayBitMatrix(lines, columns);
        }

        /// <summary>
        /// Cria uma matriz <see cref="ArrayBitMatrix"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz.</returns>
        public IMatrix<int> CreateMatrix(int lines, int columns, int defaultValue)
        {
            return new ArrayBitMatrix(lines, columns, defaultValue);
        }
    }
}
