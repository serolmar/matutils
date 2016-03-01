namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar instâncias de matrizes <see cref="ArrayMathMatrix{ObjectType}"/>.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayMatrixFactory<ObjectType> : IMatrixFactory<ObjectType>
    {
        /// <summary>
        /// Cria uma matriz <see cref="ArrayMathMatrix{ObjectType}"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <returns>A matriz.</returns>
        public IMathMatrix<ObjectType> CreateMatrix(int lines, int columns)
        {
            return new ArrayMathMatrix<ObjectType>(lines, columns);
        }

        /// <summary>
        /// Cria uma matriz <see cref="ArrayMathMatrix{ObjectType}"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz.</returns>
        public IMathMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            return new ArrayMathMatrix<ObjectType>(lines, columns, defaultValue);
        }
    }
}
