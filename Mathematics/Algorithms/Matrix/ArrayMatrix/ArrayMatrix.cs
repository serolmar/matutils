﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class ArrayMatrix<T> : IMatrix<int, int, T>
    {
        private T[,] elements;

        public ArrayMatrix(int lines, int columns)
        {
            if (lines <= 0 || columns <= 0)
            {
                throw new MathematicsException("Lines and columns must be positive.");
            }

            this.elements = new T[lines, columns];
        }

        public IMatrixRow<int, T> this[int line]
        {
            get
            {
                if (line < 0)
                {
                    throw new MathematicsException("Line number can't be negative.");
                }
                else if (line > this.elements.GetLength(0))
                {
                    throw new MathematicsException("Line number can't be greater or equal to the number of lines in matrix.");
                }
                else
                {
                    return new ArrayMatrixRow<T>(line, this.elements);
                }
            }
        }

        public T this[int line, int column]
        {
            get {
                if (line < 0)
                {
                    throw new MathematicsException("Line number can't be negative.");
                }
                else if (line > this.elements.GetLength(0))
                {
                    throw new MathematicsException("Line number can't be greater or equal to the number of lines in matrix.");
                }
                else if (column < 0)
                {
                    throw new MathematicsException("Column number can't be negative.");
                }
                else if (column > this.elements.GetLength(1))
                {
                    throw new MathematicsException("Column number can't be greater or equal to the number of columns in matrix.");
                }
                else
                {
                    return this.elements[line, column];
                }
            }
            set
            {
                if (line < 0)
                {
                    throw new MathematicsException("Line number can't be negative.");
                }
                else if (line > this.elements.GetLength(0))
                {
                    throw new MathematicsException("Line number can't be greater or equal to the number of lines in matrix.");
                }
                else if (column < 0)
                {
                    throw new MathematicsException("Column number can't be negative.");
                }
                else if (column > this.elements.GetLength(1))
                {
                    throw new MathematicsException("Column number can't be greater or equal to the number of columns in matrix.");
                }
                else
                {
                    this.elements[line, column] = value;
                }
            }
        }

        internal T[,] Elements
        {
            get
            {
                return this.elements;
            }
        }

        public bool ContainsLine(int line)
        {
            if (line < 0)
            {
                return false;
            }
            else
            {
                return line < this.elements.GetLength(0);
            }
        }

        public bool ContainsColumn(int line, int column)
        {
            if (line < 0 || line >= this.elements.GetLength(0))
            {
                return false;
            }
            else if (column < 0)
            {
                return false;
            }
            else
            {
                return column < this.elements.GetLength(1);
            }
        }

        public int GetLength(int dimension)
        {
            return this.elements.GetLength(dimension);
        }

        public IEnumerator<IMatrixRow<int, T>> GetEnumerator()
        {
            for (int i = 0; i < this.elements.GetLength(0); ++i)
            {
                yield return new ArrayMatrixRow<T>(i, this.elements);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
