using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Representa a submatriz de uma matriz sem ter a necessidade de copiá-la
    /// na íntegra.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de elementos na matriz.</typeparam>
    internal class SubMatrix<ObjectType> : IMatrix<ObjectType>
    {
        private IMatrix<ObjectType> matrix;

        private int[] lines;

        private int[] columns;

        public SubMatrix(IMatrix<ObjectType> matrix, int[] lines, int[] columns)
        {
            if (lines == null || columns == null)
            {
                throw new ArgumentException("Parameters lines and columns must be non null.");
            }
            else
            {
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i] < 0 || lines[i] >= matrix.GetLength(0))
                    {
                        throw new IndexOutOfRangeException("The lines parameter contain elements that are out of the coords range of matrix.");
                    }
                }

                for (int i = 0; i < lines.Length; ++i)
                {
                    if (columns[i] < 0 || columns[i] >= matrix.GetLength(1))
                    {
                        throw new IndexOutOfRangeException("The columns parameter contain elements that are out of the coords range of matrix.");
                    }
                }

                this.matrix = matrix;
                this.lines = lines;
                this.columns = columns;
            }
        }

        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.lines.Length)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Length)
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    return this.matrix[this.lines[line], this.columns[column]];
                }
            }
            set
            {
                if (line < 0 || line >= this.lines.Length)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Length)
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    this.matrix[this.lines[line], this.columns[column]] = value;
                }
            }
        }

        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.lines.Length;
            }
            else if (dimension == 1)
            {
                return this.columns.Length;
            }
            else
            {
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
            }
        }

        public IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns)
        {
            if (lines == null || columns == null)
            {
                throw new ArgumentException("Parameters lines and columns must be non null.");
            }
            else
            {
                return new SubMatrix<ObjectType>(this, lines, columns);
            }
        }

        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.lines.Length; ++i)
            {
                for (int j = 0; j < this.columns.Length; ++j)
                {
                    yield return this.matrix[this.lines[i], this.columns[j]];
                }
            }
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (0 < this.lines.Length)
            {
                resultBuilder.Append("[");
                if (0 < this.columns.Length)
                {
                    resultBuilder.Append(this.matrix[this.lines[0], this.columns[0]]);
                    for (int i = 1; i < this.columns.Length; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.matrix[this.lines[0], this.columns[i]]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.lines.Length; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.columns.Length)
                    {
                        resultBuilder.Append(this.matrix[this.lines[i], this.columns[0]]);
                        for (int j = 1; j < this.columns.Length; ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", this.matrix[this.lines[i], this.columns[j]]);
                        }
                    }

                    resultBuilder.Append("]");
                }

            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
