using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Collections;

namespace Mathematics
{
    public class ArrayMatrix<ObjectType> : IMatrix<ObjectType>
    {
        private int numberOfLines;

        private int numberOfColumns;

        private ObjectType[][] elements;

        public ArrayMatrix(int line, int column)
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
                this.elements = new ObjectType[line][];
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new ObjectType[column];
                }

                this.numberOfLines = line;
                this.numberOfColumns = column;
            }
        }

        public ArrayMatrix(int line, int column, ObjectType defaultValue)
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
                this.elements = new ObjectType[line][];
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new ObjectType[column];
                    for (int j = 0; j < column; ++j)
                    {
                        this.elements[i][j] = defaultValue;
                    }
                }

                this.numberOfLines = line;
                this.numberOfColumns = column;
            }
        }

        public ObjectType this[int line, int column]
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
                    return this.elements[line][column];
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
                    this.elements[line][column] = value;
                }
            }
        }

        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        public bool IsSymmetric(IEqualityComparer<ObjectType> equalityComparer)
        {
            var innerEqualityComparer = equalityComparer;
            if (innerEqualityComparer == null)
            {
                innerEqualityComparer = EqualityComparer<ObjectType>.Default;
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

        public IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ObjectType>(this, lines, columns);
        }

        public IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a soma da matriz corrente com outra matriz.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <param name="semigroup">O semigrupo.</param>
        /// <returns>O resultado da soma.</returns>
        public ArrayMatrix<ObjectType> Sum(ArrayMatrix<ObjectType> right, ISemigroup<ObjectType> semigroup)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (semigroup == null)
            {
                throw new ArgumentNullException("semigroup");
            }
            else
            {
                if (this.numberOfLines == right.numberOfLines &&
                    this.numberOfColumns == right.numberOfColumns)
                {
                    var result = new ArrayMatrix<ObjectType>(
                        this.numberOfLines,
                        this.numberOfColumns);
                    for (int i = 0; i < this.numberOfLines; ++i)
                    {
                        for (int j = 0; j < this.numberOfColumns; ++j)
                        {
                            result.elements[i][j] = semigroup.Add(
                                this.elements[i][j],
                                right.elements[i][j]);
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
        /// Obtém o produto da matriz corrente com outra matriz.
        /// </summary>
        /// <param name="right">A outra matriz.</param>
        /// <param name="ring">O anel.</param>
        /// <returns>O resultado do produto.</returns>
        public ArrayMatrix<ObjectType> Multiply(
            ArrayMatrix<ObjectType> right, 
            IRing<ObjectType> ring)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
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
                    var result = new ArrayMatrix<ObjectType>(
                        firstDimension,
                        secondDimension);
                    for (int i = 0; i < firstDimension; ++i)
                    {
                        for (int j = 0; j < secondDimension; ++j)
                        {
                            var addResult = ring.AdditiveUnity;
                            for (int k = 0; k < columnNumber; ++k)
                            {
                                var multResult = ring.Multiply(
                                    this.elements[i][k],
                                    right.elements[k][j]);
                                addResult = ring.Add(addResult, multResult);
                            }

                            result.elements[i][j] = addResult;
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
                var swapLine = this.elements[i];
                this.elements[i] = this.elements[j];
                this.elements[j] = swapLine;
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
                    var swapColumn = this.elements[k][i];
                    this.elements[k][i] = this.elements[k][j];
                    this.elements[k][j] = swapColumn;
                }
            }
        }

        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = 0; j < this.numberOfColumns; ++j)
                {
                    yield return this.elements[i][j];
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
                    resultBuilder.Append(this.elements[0][0]);
                    for (int i = 1; i < this.numberOfColumns; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.elements[0][i]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.numberOfLines; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.numberOfColumns)
                    {
                        resultBuilder.Append(this.elements[i][0]);
                        for (int j = 1; j < this.numberOfColumns; ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", this.elements[i][j]);
                        }
                    }

                    resultBuilder.Append("]");
                }

            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        public static IMatrix<ObjectType> GetIdentity<RingType>(int order, RingType ring)
            where RingType : IRing<ObjectType>
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (order <= 0)
            {
                throw new ArgumentOutOfRangeException("Order of identity matrix must be greater than one.");
            }
            else
            {
                var result = new ArrayMatrix<ObjectType>(order, order);
                for (int i = 0; i < order; ++i)
                {
                    for (int j = 0; j < order; ++j)
                    {
                        if (i == j)
                        {
                            result.elements[i][j] = ring.MultiplicativeUnity;
                        }
                        else
                        {
                            result.elements[i][j] = ring.AdditiveUnity;
                        }
                    }
                }

                return result;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
