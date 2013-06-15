using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class SparseDictionaryMatrixRow<T> : IMatrixRow<int, int, T>
    {
        private Dictionary<int, T> lineElements;

        private int rowNumber;

        public SparseDictionaryMatrixRow(int rowNumber) : this(null, rowNumber) { }

        internal SparseDictionaryMatrixRow(Dictionary<int, T> dictionary, int rowNumber)
        {
            if (dictionary == null)
            {
                this.lineElements = new Dictionary<int, T>();
            }
            else
            {
                this.lineElements = dictionary;
            }

            this.rowNumber = rowNumber;
        }

        public T this[int columnIndex]
        {
            get
            {
                T value = default(T);
                if (!this.lineElements.TryGetValue(columnIndex, out value))
                {
                    throw new MathematicsException("int index doesn't exist.");
                }
                else
                {
                    if (columnIndex < 0)
                    {
                        throw new MathematicsException("Negative indices aren't allowed.");
                    }

                    return value;
                }
            }
        }

        public int LineNumber
        {
            get
            {
                return this.rowNumber;
            }
        }

        internal Dictionary<int, T> LineElements
        {
            get
            {
                return this.lineElements;
            }
        }

        public bool ContainsColumn(int index)
        {
            return this.lineElements.ContainsKey(index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.lineElements.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

    }
}
