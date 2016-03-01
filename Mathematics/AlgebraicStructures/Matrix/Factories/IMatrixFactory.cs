using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Interface para uma fábrica de matrizes.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de entrada na matriz.</typeparam>
    public interface IMatrixFactory<ObjectType>
    {
        /// <summary>
        /// Cria uma matriz com o número especificado de linhas e colunas.
        /// </summary>
        /// <param name="lines">O conjunto de linhas.</param>
        /// <param name="columns">O conjunto de colunas.</param>
        /// <returns>A matriz.</returns>
        IMathMatrix<ObjectType> CreateMatrix(int lines, int columns);

        /// <summary>
        /// Cria uma matriz com o número especificado de linhas e colunas.
        /// </summary>
        /// <param name="lines">O conjunto de linhas.</param>
        /// <param name="columns">O conjunto de colunas.</param>
        /// <param name="defaultValue">O valor que surgirá por defeito.</param>
        /// <returns>A matriz.</returns>
        IMathMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue);
    }
}
