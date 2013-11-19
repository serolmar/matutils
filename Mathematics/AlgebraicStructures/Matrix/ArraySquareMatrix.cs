namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma matriz quadrada.
    /// </summary>
    public class ArraySquareMatrix<CoeffType> : ArrayMatrix<CoeffType>, ISquareMatrix<CoeffType>
    {
        public ArraySquareMatrix(int dimension) : base(dimension, dimension) { }

        public ArraySquareMatrix(int dimension, CoeffType defaultValue) : base(dimension, dimension, defaultValue) { }

        internal ArraySquareMatrix(CoeffType[][] elements, int dimension)
            : base(elements, dimension, dimension) { }

        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        public bool IsSymmetric(IEqualityComparer<CoeffType> equalityComparer)
        {
            var innerEqualityComparer = equalityComparer;
            if (innerEqualityComparer == null)
            {
                innerEqualityComparer = EqualityComparer<CoeffType>.Default;
            }

            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = i + 1; j < this.numberOfColumns; ++j)
                {
                    var currentEntry = this.elements[i][j];
                    var symmetricEntry = this.elements[j][i];
                    if (!innerEqualityComparer.Equals(currentEntry, symmetricEntry))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
