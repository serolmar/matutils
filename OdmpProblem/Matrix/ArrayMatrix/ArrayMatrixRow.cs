using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    class ArrayMatrixRow<T> : IMatrixRow<int, int, T>
    {
        private T[,] elements;

        private int lineNumber;

        public ArrayMatrixRow(int lineNumber, T[,] elements)
        {
            this.lineNumber = lineNumber;
            this.elements = elements;
        }

        public IMatrixColumn<int, T> this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0)
                {
                    throw new OdmpProblemException("Column index can't be negative.");
                }
                else if (columnIndex >= this.elements.GetLength(1))
                {
                    throw new OdmpProblemException("Column index can't be greater or equal to the number of elements in line.");
                }
                else
                {
                    return new MatrixColumn<int, T>(columnIndex, this.elements[this.lineNumber, columnIndex]);
                }
            }
        }

        public int Line
        {
            get
            {
                return this.lineNumber;
            }
        }

        public int Count
        {
            get
            {
                return this.elements.GetLength(1);
            }
        }

        public bool ContainsColumn(int index)
        {
            if (index < 0)
            {
                return false;
            }
            else
            {
                return index < this.elements.GetLength(1);
            }
        }

        public IEnumerator<IMatrixColumn<int, T>> GetEnumerator()
        {
            for (int i = 0; i < this.elements.GetLength(1); ++i)
            {
                yield return new MatrixColumn<int, T>(i, this.elements[this.lineNumber, i]);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
