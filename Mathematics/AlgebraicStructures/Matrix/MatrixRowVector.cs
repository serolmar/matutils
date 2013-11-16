namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa uma linha de matriz como sendo um vector.
    /// </summary>
    internal class MatrixRowVector<CoeffType> : IVector<CoeffType>
    {
        private IMatrix<CoeffType> matrix;

        private int lineNumber;

        public MatrixRowVector(int lineNumber, IMatrix<CoeffType> matrix)
        {
            if (lineNumber < 0)
            {
                throw new ArgumentOutOfRangeException("lineNumber");
            }
            else
            {
                this.lineNumber = lineNumber;
                this.matrix = matrix;
            }
        }

        private MatrixRowVector()
        {
        }

        public CoeffType this[int index]
        {
            get
            {
                var length = matrix.GetLength(1);
                if (index < 0 || index >= length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    return this.matrix[this.lineNumber, index];
                }
            }
            set
            {
                var length = matrix.GetLength(1);
                if (index < 0 || index >= length)
                {
                    throw new IndexOutOfRangeException("Index must be non-negative and less than the size of the vector.");
                }
                else
                {
                    this.matrix[this.lineNumber, index] = value;
                }
            }
        }

        public int Length
        {
            get {
                return this.matrix.GetLength(1);
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
                throw new MathematicsException("Can't swap matrix row vector entries.");
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
                for (int i = 0; i < this.matrix.GetLength(1); ++i)
                {
                    if (!monoid.IsAdditiveUnity(this.matrix[this.lineNumber, i]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public IVector<CoeffType> Clone()
        {
            var result = new MatrixRowVector<CoeffType>();
            result.matrix = matrix;
            result.lineNumber = this.lineNumber;
            return result;
        }

        public IEnumerator<CoeffType> GetEnumerator()
        {
            for (int i = 0; i < this.matrix.GetLength(1); ++i)
            {
                yield return this.matrix[this.lineNumber, i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
