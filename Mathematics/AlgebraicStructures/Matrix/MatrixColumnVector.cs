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
    internal class MatrixColumnVector<CoeffType> : IVector<CoeffType>
    {
        private IMatrix<CoeffType> matrix;

        private int columnNumber;

        public MatrixColumnVector(int columnNumber, IMatrix<CoeffType> matrix)
        {
            if (columnNumber < 0)
            {
                throw new ArgumentOutOfRangeException("lineNumber");
            }
            else
            {
                this.columnNumber = columnNumber;
                this.matrix = matrix;
            }
        }

        private MatrixColumnVector()
        {
        }

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

        public int Length
        {
            get
            {
                return this.matrix.GetLength(0);
            }
        }

        public IVector<CoeffType> GetSubVector(int[] indices)
        {
            return new SubVector<CoeffType>(this, indices);
        }

        public IVector<CoeffType> GetSubVector(IntegerSequence indices)
        {
            return new IntegerSequenceSubVector<CoeffType>(this, indices);
        }

        public void SwapElements(int first, int second)
        {
            if (first != second)
            {
                throw new MathematicsException("Can't swap matrix column vector entries.");
            }
        }

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

        public IVector<CoeffType> Clone()
        {
            var result = new MatrixColumnVector<CoeffType>();
            result.matrix = this.matrix;
            result.columnNumber = this.columnNumber;
            return result;
        }

        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.matrix.GetLength(1); ++i)
            {
                yield return this.matrix[i, this.columnNumber];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
