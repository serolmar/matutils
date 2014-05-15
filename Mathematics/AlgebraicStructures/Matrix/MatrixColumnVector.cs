namespace Mathematics.AlgebraicStructures.Matrix
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa uma coluna de matriz como sendo um vector.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de obejctos que constituem as entradas das matrizes.</typeparam>
    internal class MatrixColumnVector<CoeffType> : IVector<CoeffType>
    {
        /// <summary>
        /// A matriz da qual é extraído o vector coluna.
        /// </summary>
        private IMatrix<CoeffType> matrix;

        /// <summary>
        /// O número da coluna.
        /// </summary>
        private int columnNumber;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="MatrixColumnVector{CoeffType}"/>.
        /// </summary>
        /// <param name="columnNumber">O número da coluna.</param>
        /// <param name="matrix">A matriz da qual se considera a coluna como sendo vector.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Se a matriz for nula.</exception>
        public MatrixColumnVector(int columnNumber, IMatrix<CoeffType> matrix)
        {
            if (columnNumber < 0)
            {
                throw new ArgumentOutOfRangeException("matrix");
            }
            else
            {
                this.columnNumber = columnNumber;
                this.matrix = matrix;
            }
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="MatrixColumnVector{CoeffType}"/>.
        /// </summary>
        private MatrixColumnVector()
        {
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada pelo índice respectivo.
        /// </summary>
        /// <param name="index">O índice da entrada.</param>
        /// <returns>O valor contido na posição especificada pelo índice.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao tamanho do vector.
        /// </exception>
        public CoeffType this[int index]
        {
            get
            {
                var length = matrix.GetLength(0);
                if (index < 0 || index >= length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    return this.matrix[index, this.columnNumber];
                }
            }
            set
            {
                var length = matrix.GetLength(0);
                if (index < 0 || index >= length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    this.matrix[index, this.columnNumber] = value;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        /// <value>
        /// O tamanho do vector.
        /// </value>
        public int Length
        {
            get
            {
                return this.matrix.GetLength(0);
            }
        }

        /// <summary>
        /// Obtém o sub-vector especificado pelos índices.
        /// </summary>
        /// <param name="indices">Os índices.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(int[] indices)
        {
            return new SubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Obtém o sub-vector especificado pela sequência especificada.
        /// </summary>
        /// <param name="indices">A sequência.</param>
        /// <returns>O sub-vector.</returns>
        public IVector<CoeffType> GetSubVector(IntegerSequence indices)
        {
            return new IntegerSequenceSubVector<CoeffType>(this, indices);
        }

        /// <summary>
        /// Funcionalidade não suportada para vectores coluna.
        /// </summary>
        /// <param name="first">A posição do primeiro elemento.</param>
        /// <param name="second">A posição do segundo elemento.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Sempre que uma troca é experimentada.
        /// </exception>
        public void SwapElements(int first, int second)
        {
            if (first != second)
            {
                throw new MathematicsException("Can't swap matrix column vector entries.");
            }
        }

        /// <summary>
        /// Determina se o vector actual é zero relativamente a um monóide.
        /// </summary>
        /// <param name="monoid">O monóide.</param>
        /// <returns>Verdadeiro caso o vector seja nulo e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide fornecido for nulo.</exception>
        public bool IsNull(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                for (int i = 0; i < this.matrix.GetLength(0); ++i)
                {
                    if (!monoid.IsAdditiveUnity(this.matrix[i, this.columnNumber]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Constrói uma cópida do vector actual.
        /// </summary>
        /// <returns>A cópia do vector actual.</returns>
        public IVector<CoeffType> Clone()
        {
            var result = new MatrixColumnVector<CoeffType>();
            result.matrix = this.matrix;
            result.columnNumber = this.columnNumber;
            return result;
        }

        /// <summary>
        /// Obtém um enumerador para os elementos do vector.
        /// </summary>
        /// <returns>O enumerador para os elementos do vector.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.matrix.GetLength(1); ++i)
            {
                yield return this.matrix[i, this.columnNumber];
            }
        }

        /// <summary>
        /// Obtém o enumerador não genérico para os elementos do vector.
        /// </summary>
        /// <returns>O enumerador não genérico para os elementos do vector.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
