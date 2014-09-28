namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Fábrica para matrizes triangulares inferiores.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayTriangularLowerMatrixFactory<CoeffType> : ISquareMatrixFactory<CoeffType>
    {
        /// <summary>
        /// Cria uma matriz triangular inferior com a dimensão especificada.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <returns>A matriz triangular inferior.</returns>
        public IMatrix<CoeffType> CreateMatrix(int dimension)
        {
            return new ArrayTriangLowerMatrix<CoeffType>(dimension);
        }

        /// <summary>
        /// Cria uma matriz triangular inferior com a dimensão especificada e um valor por defeito.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz triagular inferior.</returns>
        public IMatrix<CoeffType> CreateMatrix(int dimension, CoeffType defaultValue)
        {
            return new ArrayTriangLowerMatrix<CoeffType>(dimension, defaultValue);
        }
    }
}
