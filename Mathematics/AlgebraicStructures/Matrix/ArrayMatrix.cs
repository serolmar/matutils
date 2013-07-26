using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class ArrayMatrix<ObjectType> : IMatrix<ObjectType>
    {
        private ObjectType[,] elements;

        public ArrayMatrix(int line, int column)
        {
            if (line < 0 || column < 0)
            {
                throw new ArgumentOutOfRangeException("The arguments line and column must be non-negative in matrix definition.");
            }

            this.elements = new ObjectType[line, column];
        }

        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.elements.GetLength(0))
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.elements.GetLength(1))
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    return this.elements[line, column];
                }
            }
            set
            {
                if (line < 0 || line >= this.elements.GetLength(0))
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.elements.GetLength(1))
                {
                    throw new IndexOutOfRangeException("Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    this.elements[line, column] = value;
                }
            }
        }

        public int GetLength(int dimension)
        {
            if (dimension != 0 && dimension != 1)
            {
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
            }

            return this.elements.GetLength(dimension);
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
                var lineNumber = this.elements.GetLength(0);
                var columnNumber = this.elements.GetLength(1);
                if (lineNumber == right.elements.GetLength(0) &&
                    columnNumber == right.elements.GetLength(1))
                {
                    var result = new ArrayMatrix<ObjectType>(
                        lineNumber,
                        columnNumber);
                    for (int i = 0; i < lineNumber; ++i)
                    {
                        for (int j = 0; j < columnNumber; ++j)
                        {
                            result.elements[i, j] = semigroup.Add(
                                this.elements[i, j],
                                right.elements[i, j]);
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
                var columnNumber = this.elements.GetLength(1);
                var lineNumber = right.elements.GetLength(0);
                if (columnNumber != lineNumber)
                {
                    throw new MathematicsException("To multiply two matrices, the number of columns of the first must match the number of lines of second.");
                }
                else
                {
                    var firstDimension = this.elements.GetLength(0);
                    var secondDimension = right.elements.GetLength(1);
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
                                    this.elements[i, k],
                                    right.elements[k, j]);
                                addResult = ring.Add(addResult, multResult);
                            }

                            result.elements[i, j] = addResult;
                        }
                    }

                    return result;
                }
            }
        }

        public IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.elements.GetLength(0); ++i)
            {
                for (int j = 0; j < this.elements.GetLength(1); ++j)
                {
                    yield return this.elements[i, j];
                }
            }
        }

        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (0 < this.elements.GetLength(0))
            {
                resultBuilder.Append("[");
                if (0 < this.elements.GetLength(1))
                {
                    resultBuilder.Append(this.elements[0, 0]);
                    for (int i = 1; i < this.elements.GetLength(1); ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.elements[0, i]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.elements.GetLength(0); ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.elements.GetLength(1))
                    {
                        resultBuilder.Append(this.elements[i, 0]);
                        for (int j = 1; j < this.elements.GetLength(1); ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", this.elements[i, j]);
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
