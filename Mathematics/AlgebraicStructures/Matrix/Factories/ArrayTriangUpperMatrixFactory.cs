namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Fábrica para matrizes triangulares superiores.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayTriangUpperMatrixFactory<CoeffType> : ISquareMatrixFactory<CoeffType>
    {
        /// <summary>
        /// Cria uma matriz triangular superior com a dimensão especificada.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <returns>A matriz triangular superior.</returns>
        public IMatrix<CoeffType> CreateMatrix(int dimension)
        {
            return new ArrayTriangUpperMatrix<CoeffType>(dimension);
        }

        /// <summary>
        /// Cria uma matriz triangular superior com a dimensão especificada e um valor por defeito.
        /// </summary>
        /// <param name="dimension">A dimensão.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz triangular superior.</returns>
        public IMatrix<CoeffType> CreateMatrix(int dimension, CoeffType defaultValue)
        {
            return new ArrayTriangUpperMatrix<CoeffType>(dimension, defaultValue);
        }
    }
}
