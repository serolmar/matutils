namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class BitMatrix : IMatrix<int>
    {
        private int numberOfLines;

        private int numberOfColumns;

        private BitList[] bitMatrix;

        public BitMatrix(int line, int column)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.bitMatrix = new BitList[line];
                for (int i = 0; i < line; ++i)
                {
                    this.bitMatrix[i] = new BitList(column);
                }

                this.numberOfLines = line;
                this.numberOfColumns = column;
            }
        }

        public BitMatrix(int line, int column, int defaultValue)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.bitMatrix = new BitList[line];
                for (int i = 0; i < line; ++i)
                {
                    this.bitMatrix[i] = new BitList(column, defaultValue != 0);
                }

                this.numberOfLines = line;
                this.numberOfColumns = column;
            }
        }

        public int this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    return this.bitMatrix[line][column];
                }
            }
            set
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    this.bitMatrix[line][column] = value;
                }
            }
        }

        public bool IsSymmetric(IEqualityComparer<int> equalityComparer)
        {
            var innerEqualityComparer = equalityComparer;
            if (innerEqualityComparer == null)
            {
                innerEqualityComparer = EqualityComparer<int>.Default;
            }

            if (this.numberOfLines != this.numberOfColumns)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < this.numberOfLines; ++i)
                {
                    for (int j = i + 1; j < this.numberOfColumns; ++j)
                    {
                        var currentEntry = this.bitMatrix[i][j];
                        var symmetricEntry = this.bitMatrix[j][i];
                        if (!innerEqualityComparer.Equals(currentEntry, symmetricEntry))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.numberOfLines;
            }
            else if (dimension == 1)
            {
                return this.numberOfColumns;
            }
            else
            {
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
            }
        }

        public IMatrix<int> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<int>(this, lines, columns);
        }

        public IMatrix<int> GetSubMatrix(Utilities.Collections.IntegerSequence lines, Utilities.Collections.IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<int>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a soma da matriz corrente com outra matriz, onde a soma é efectuada módulo 2.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <returns>O resultado da soma.</returns>
        public BitMatrix Sum(BitMatrix right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                if (this.numberOfLines == right.numberOfLines &&
                    this.numberOfColumns == right.numberOfColumns)
                {
                    var result = new BitMatrix(
                        this.numberOfLines,
                        this.numberOfColumns);
                    for (int i = 0; i < this.numberOfLines; ++i)
                    {
                        for (int j = 0; j < this.numberOfColumns; ++j)
                        {
                            result.bitMatrix[i][j] = (this.bitMatrix[i][j] + right.bitMatrix[i][j]) % 2;
                        }
                    }

                    return result;
                }
                else
                {
                    throw new ArgumentException("Matrices don't have the same dimensions.");
                }
            }
        }

        /// <summary>
        /// Obtém o produto da matriz corrente com outra matriz, onde as operações são efectuadas módulo 2.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <returns>O resultado do produto.</returns>
        public BitMatrix Multiply(BitMatrix right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var columnNumber = this.numberOfColumns;
                var lineNumber = right.numberOfColumns;
                if (columnNumber != lineNumber)
                {
                    throw new MathematicsException("To multiply two matrices, the number of columns of the first must match the number of lines of second.");
                }
                else
                {
                    var firstDimension = this.numberOfLines;
                    var secondDimension = right.numberOfColumns;
                    var result = new BitMatrix(
                        firstDimension,
                        secondDimension);
                    for (int i = 0; i < firstDimension; ++i)
                    {
                        for (int j = 0; j < secondDimension; ++j)
                        {
                            var addResult = 0;
                            for (int k = 0; k < columnNumber; ++k)
                            {
                                var multResult = (this.bitMatrix[i][k] * right.bitMatrix[k][j]) % 2; 
                                addResult = (addResult + multResult) % 2;
                            }

                            result.bitMatrix[i][j] = addResult;
                        }
                    }

                    return result;
                }
            }
        }

        public void SwapLines(int i, int j)
        {
            if (i < 0 || i > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                var swapLine = this.bitMatrix[i];
                this.bitMatrix[i] = this.bitMatrix[j];
                this.bitMatrix[j] = swapLine;
            }
        }

        public void SwapColumns(int i, int j)
        {
            if (i < 0 || i > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                for (int k = 0; k < this.numberOfLines; ++k)
                {
                    var swapColumn = this.bitMatrix[k][i];
                    this.bitMatrix[k][i] = this.bitMatrix[k][j];
                    this.bitMatrix[k][j] = swapColumn;
                }
            }
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (0 < this.numberOfLines)
            {
                resultBuilder.Append("[");
                if (0 < this.numberOfColumns)
                {
                    resultBuilder.Append(this.bitMatrix[0][0]);
                    for (int i = 1; i < this.numberOfColumns; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.bitMatrix[0][i]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.numberOfLines; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.numberOfColumns)
                    {
                        resultBuilder.Append(this.bitMatrix[i][0]);
                        for (int j = 1; j < this.numberOfColumns; ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", this.bitMatrix[i][j]);
                        }
                    }

                    resultBuilder.Append("]");
                }

            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = 0; j < this.numberOfColumns; ++j)
                {
                    yield return this.bitMatrix[i][j];
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
