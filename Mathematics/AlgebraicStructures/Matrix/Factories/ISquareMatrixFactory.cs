namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface para uma fábrica de matrizes quadradas.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de entrada na matriz.</typeparam>
    public interface ISquareMatrixFactory<ObjectType>
    {
        /// <summary>
        /// Cria uma matriz quadrada com a dimensão especificada.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <returns>A matriz quadrada.</returns>
        IMathMatrix<ObjectType> CreateMatrix(int dimension);

        /// <summary>
        /// Cria uma matriz quadrada com a dimensão especificada.
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor que surgirá por defeito.</param>
        /// <returns>A matriz quadrada.</returns>
        IMathMatrix<ObjectType> CreateMatrix(int dimension, ObjectType defaultValue);
    }
}
