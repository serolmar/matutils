namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class TransposeMatrix<ObjectType> : IMatrix<ObjectType>
    {
        private IMatrix<ObjectType> matrix;

        public TransposeMatrix(IMatrix<ObjectType> matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else
            {
                this.matrix = matrix;
            }
        }

        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.matrix.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= this.matrix.GetLength(0))
                {
                    throw new ArgumentOutOfRangeException("column");
                }
                else
                {
                    return this.matrix[column, line];
                }
            }
            set
            {
                if (line < 0 || line >= this.matrix.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= this.matrix.GetLength(0))
                {
                    throw new ArgumentOutOfRangeException("column");
                }
                else
                {
                    this.matrix[column, line] = value;
                }
            }
        }

        /// <summary>
        /// Obtém a matriz original.
        /// </summary>
        public IMatrix<ObjectType> Matrix
        {
            get
            {
                return this.matrix;
            }
        }

        public bool IsSymmetric(IEqualityComparer<ObjectType> equalityComparer)
        {
            throw new NotImplementedException();
        }

        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.matrix.GetLength(1);
            }
            else if (dimension == 1)
            {
                return this.matrix.GetLength(0);
            }
            else
            {
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
            }
        }

        public IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ObjectType>(this, lines, columns);
        }

        public IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
        }

        public void SwapLines(int i, int j)
        {
            this.matrix.SwapColumns(i, j);
        }

        public void SwapColumns(int i, int j)
        {
            this.SwapLines(i, j);
        }

        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.matrix.GetLength(1); ++i)
            {
                for (int j = 0; j < this.matrix.GetLength(0); ++j)
                {
                    yield return this.matrix[j, i];
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
