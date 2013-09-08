using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    class OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T> : IOdmpMatrixRow<LineType, ColumnType, T>
    {
        private Dictionary<ColumnType, T> lineElements;

        private LineType line;

        public OdmpSparseDictionaryMatrixRow(LineType line) : this(line, null, null) { }

        public OdmpSparseDictionaryMatrixRow(LineType line, IEqualityComparer<ColumnType> columnsEqualityComparer)
            : this(line, columnsEqualityComparer, null)
        {
            this.line = line;
        }

        internal OdmpSparseDictionaryMatrixRow(LineType line, IEqualityComparer<ColumnType> columnsEqualityComparer, Dictionary<ColumnType, T> dictionary)
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

        public IOdmpMatrixColumn<ColumnType, T> this[ColumnType columnIndex]
        {
            get
            {
                T value = default(T);
                if (!this.lineElements.TryGetValue(columnIndex, out value))
                {
                    throw new OdmpProblemException("int index doesn't exist.");
                }
                else
                {
                    return new OdmpMatrixColumn<ColumnType, T>(columnIndex, value);
                }
            }
        }

        public LineType Line
        {
            get
            {
                return this.line;
            }
        }

        public int Count
        {
            get
            {
                return this.lineElements.Count;
            }
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

        public IEnumerator<IOdmpMatrixColumn<ColumnType, T>> GetEnumerator()
        {
            foreach (var kvp in this.lineElements)
            {
                yield return new OdmpMatrixColumn<ColumnType, T>(kvp.Key, kvp.Value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
