using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    class SparseDictionaryMatrixRow<LineType, ColumnType, T> : IMatrixRow<LineType, ColumnType, T>
    {
        private Dictionary<ColumnType, T> lineElements;

        private LineType line;

        public SparseDictionaryMatrixRow(LineType line) : this(line, null, null) { }

        public SparseDictionaryMatrixRow(LineType line, IEqualityComparer<ColumnType> columnsEqualityComparer)
            : this(line, columnsEqualityComparer, null)
        {
            this.line = line;
        }

        internal SparseDictionaryMatrixRow(LineType line, IEqualityComparer<ColumnType> columnsEqualityComparer, Dictionary<ColumnType, T> dictionary)
        {
            if (columnsEqualityComparer == null)
            {
                this.lineElements = new Dictionary<ColumnType, T>();
            }
            else
            {
                this.lineElements = new Dictionary<ColumnType, T>(columnsEqualityComparer);
            }

            if (dictionary == null)
            {
                this.lineElements = new Dictionary<ColumnType, T>();
            }
            else
            {
                this.lineElements = dictionary;
                this.line = line;
            }
        }

        public IMatrixColumn<ColumnType, T> this[ColumnType columnIndex]
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
                    return new MatrixColumn<ColumnType, T>(columnIndex, value);
                }
            }
        }

        public LineType Line
        {
            get { throw new NotImplementedException(); }
        }

        internal Dictionary<ColumnType, T> LineElements
        {
            get
            {
                return this.lineElements;
            }
        }

        public bool ContainsColumn(ColumnType index)
        {
            return this.lineElements.ContainsKey(index);
        }

        public IEnumerator<IMatrixColumn<ColumnType, T>> GetEnumerator()
        {
            foreach (var kvp in this.lineElements)
            {
                yield return new MatrixColumn<ColumnType, T>(kvp.Key, kvp.Value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
