namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa uma matriz especificada por sequências de inteiros ao invés de vectores.
    /// </summary>
    /// <remarks>
    /// A utilização de sequências permite optimizar determinados processos em termos de tempo
    /// e memória utilizada.
    /// </remarks>
    internal class IntegerSequenceSubMatrix<ObjectType> : IMatrix<ObjectType>
    {
        private IMatrix<ObjectType> matrix;

        private IntegerSequence lines;

        private IntegerSequence columns;

        public IntegerSequenceSubMatrix(IMatrix<ObjectType> matrix, IntegerSequence lines, IntegerSequence columns)
        {
            if (lines == null || columns == null)
            {
                throw new ArgumentException("Parameters lines and columns must be non null.");
            }
            else
            {
                this.matrix = matrix;
                this.lines = lines.Clone();
                this.columns = columns.Clone();
            }
        }

        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.lines.Count)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Count)
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
                if (line < 0 || line >= this.lines.Count)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.columns.Count)
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
                return this.lines.Count;
            }
            else if (dimension == 1)
            {
                return this.columns.Count;
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
            // Não é suportado por uma submatriz definida por uma sequência ordenada
            throw new MathematicsException("Can't swap integer sequence submatrix lines.");
        }

        public void SwapColumns(int i, int j)
        {
            // Não é suportado por uma submatriz definida por uma sequência ordenada
            throw new MathematicsException("Can't swap integer sequence submatrix columns.");
        }

        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.lines.Count; ++i)
            {
                for (int j = 0; j < this.columns.Count; ++j)
                {
                    yield return this.matrix[this.lines[i], this.columns[j]];
                }
            }
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (0 < this.lines.Count)
            {
                resultBuilder.Append("[");
                if (0 < this.columns.Count)
                {
                    resultBuilder.Append(this.matrix[this.lines[0], this.columns[0]]);
                    for (int i = 1; i < this.columns.Count; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.matrix[this.lines[0], this.columns[i]]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.lines.Count; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.columns.Count)
                    {
                        resultBuilder.Append(this.matrix[this.lines[i], this.columns[0]]);
                        for (int j = 1; j < this.columns.Count; ++j)
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
