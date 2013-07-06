﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class ArrayMatrixRow<T> : IMatrixRow<int, T>
    {
        private T[,] elements;

        private int lineNumber;

        public ArrayMatrixRow(int lineNumber, T[,] elements)
        {
            this.lineNumber = lineNumber;
            this.elements = elements;
        }

        public T this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0)
                {
                    throw new MathematicsException("Column index can't be negative.");
                }
                else if (columnIndex >= this.elements.GetLength(1))
                {
                    throw new MathematicsException("Column index can't be greater or equal to the number of elements in line.");
                }
                else
                {
                    return this.elements[this.lineNumber, columnIndex];
                }
            }
        }

        public int LineNumber
        {
            get
            {
                return this.lineNumber;
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

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.elements.GetLength(1); ++i)
            {
                yield return this.elements[this.lineNumber, i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }


    }
}
