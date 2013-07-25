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
                if (line < 0 || line >= elements.GetLength(0))
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= elements.GetLength(1))
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
                if (line < 0 || line >= elements.GetLength(0))
                {
                    throw new IndexOutOfRangeException("Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= elements.GetLength(1))
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
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i] < 0 || lines[i] >= this.elements.GetLength(0))
                    {
                        throw new IndexOutOfRangeException("The lines parameter contain elements that are out of the coords range of matrix.");
                    }
                }

                for (int i = 0; i < lines.Length; ++i)
                {
                    if (columns[i] < 0 || columns[i] >= this.elements.GetLength(1))
                    {
                        throw new IndexOutOfRangeException("The columns parameter contain elements that are out of the coords range of matrix.");
                    }
                }

                var result = new ArrayMatrix<ObjectType>(lines.Length, columns.Length);
                for (int i = 0; i < lines.Length; ++i)
                {
                    for (int j = 0; j < columns.Length; ++j)
                    {
                        result[i, j] = this.elements[lines[i], columns[j]];
                    }
                }

                return result;
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
    }
}
