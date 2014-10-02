namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar uma matriz quadrada baseada num vector de vectores.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de elementos contidos na matriz.</typeparam>
    public class ArraySquareMatrixFactory<ObjectType> : ISquareMatrixFactory<ObjectType>, IMatrixFactory<ObjectType>
    {
        /// <summary>
        /// Cria uma matriz <see cref="ArraySquareMatrixFactory{ObjectType}"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <returns>A matriz.</returns>
        /// <exception cref="MathematicsException">Se o número de linhas for diferente do número de colunas.</exception>
        public IMatrix<ObjectType> CreateMatrix(int lines, int columns)
        {
            if (lines != columns)
            {
                throw new MathematicsException("The number of lines doesn't match the number of columns.");
            }
            else
            {
                return new ArraySquareMatrix<ObjectType>(lines);
            }
        }

        /// <summary>
        /// Cria uma matriz <see cref="ArraySquareMatrixFactory{ObjectType}"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz.</returns>
        /// <exception cref="MathematicsException">Se o número de linhas for diferente do número de colunas.</exception>
        public IMatrix<ObjectType> CreateMatrix(int lines, int columns, ObjectType defaultValue)
        {
            if (lines != columns)
            {
                throw new MathematicsException("The number of lines doesn't match the number of columns.");
            }
            else
            {
                return new ArraySquareMatrix<ObjectType>(lines, defaultValue);
            }
        }

        /// <summary>
        /// Cria uma matriz <see cref="ArraySquareMatrixFactory{ObjectType}"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <returns>A matriz.</returns>
        public IMatrix<ObjectType> CreateMatrix(int dimension)
        {
            return new ArraySquareMatrix<ObjectType>(dimension);
        }

        /// <summary>
        /// Cria uma matriz <see cref="ArraySquareMatrixFactory{ObjectType}"/> com o número de linhas e colunas especificado.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz.</returns>
        public IMatrix<ObjectType> CreateMatrix(int dimension, ObjectType defaultValue)
        {
            return new ArraySquareMatrix<ObjectType>(dimension, defaultValue);
        }
    }
}
