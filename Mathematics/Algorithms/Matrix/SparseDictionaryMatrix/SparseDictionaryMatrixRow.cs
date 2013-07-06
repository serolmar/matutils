using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class SparseDictionaryMatrixRow<Column, T> : IMatrixRow<Column, T>
    {
        private Dictionary<Column, T> lineElements;

        public SparseDictionaryMatrixRow() : this(null, null) { }

        public SparseDictionaryMatrixRow(IEqualityComparer<Column> columnsEqualityComparer) : this(columnsEqualityComparer, null)
        {
        }

        internal SparseDictionaryMatrixRow(IEqualityComparer<Column> columnsEqualityComparer, Dictionary<Column, T> dictionary)
        {
            if (columnsEqualityComparer == null)
            {
                this.lineElements = new Dictionary<Column, T>();
            }
            else
            {
                this.lineElements = new Dictionary<Column, T>(columnsEqualityComparer);
            }

            if (dictionary == null)
            {
                this.lineElements = new Dictionary<Column, T>();
            }
            else
            {
                this.lineElements = dictionary;
            }
        }

        public T this[Column columnIndex]
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
                    return value;
                }
            }
        }

        internal Dictionary<Column, T> LineElements
        {
            get
            {
                return this.lineElements;
            }
        }

        public bool ContainsColumn(Column index)
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
