using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class SparseDictionaryMatrix<Line, Column, T> : IMatrix<Line, Column, T>
    {
        private T defaultValue;

        private Dictionary<Line, SparseDictionaryMatrixRow<Column, T>> lines;

        private IEqualityComparer<Column> columnsComparer;

        public SparseDictionaryMatrix(T defaultValue)
            : this(null, defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public SparseDictionaryMatrix(T defaultValue, IEqualityComparer<Line> linesComparer, IEqualityComparer<Column> columnsComparer)
        {
            if (linesComparer == null)
            {
                this.lines = new Dictionary<Line, SparseDictionaryMatrixRow<Column, T>>();
            }
            else
            {
                this.lines = new Dictionary<Line, SparseDictionaryMatrixRow<Column, T>>(linesComparer);
            }

            this.columnsComparer = columnsComparer;
        }

        internal SparseDictionaryMatrix(Dictionary<Line, SparseDictionaryMatrixRow<Column, T>> lines, T defaultValue)
        {
            if (lines == null)
            {
                this.lines = new Dictionary<Line, SparseDictionaryMatrixRow<Column, T>>();
            }
            else
            {
                this.lines = lines;
            }

            this.defaultValue = defaultValue;
        }

        public IMatrixRow<Column, T> this[Line line]
        {
            get
            {
                SparseDictionaryMatrixRow<Column, T> result = null;
                if (!this.lines.TryGetValue(line, out result))
                {
                    throw new MathematicsException("int doesn't exist.");
                }
                else
                {
                    return result;
                }
            }
        }

        public T this[Line line, Column column]
        {
            get
            {
                SparseDictionaryMatrixRow<Column, T> dictionaryint = null;
                if (!this.lines.TryGetValue(line, out dictionaryint))
                {
                    return this.defaultValue;
                }
                else
                {
                    T result = default(T);
                    if (!dictionaryint.LineElements.TryGetValue(column, out result))
                    {
                        throw new MathematicsException("Line doesn't exist in specified line.");
                    }

                    return result;
                }
            }
            set
            {
                SparseDictionaryMatrixRow<Column, T> dictionaryint = null;
                if (this.lines.TryGetValue(line, out dictionaryint))
                {
                    if (dictionaryint.LineElements.ContainsKey(column))
                    {
                        dictionaryint.LineElements[column] = value;
                    }
                    else
                    {
                        dictionaryint.LineElements.Add(column, value);
                    }
                }
                else
                {
                    dictionaryint = new SparseDictionaryMatrixRow<Column, T>(this.columnsComparer);
                    this.lines.Add(line, dictionaryint);
                    dictionaryint.LineElements.Add(column, value);
                }
            }
        }

        public bool ContainsLine(Line line)
        {
            return this.lines.ContainsKey(line);
        }

        public bool ContainsColumn(Line line, Column column)
        {
            SparseDictionaryMatrixRow<Column, T> dictionaryint = null;
            if (!this.lines.TryGetValue(line, out dictionaryint))
            {
                return false;
            }
            else
            {
                return dictionaryint.LineElements.ContainsKey(column);
            }
        }

        public IEnumerator<IMatrixRow<Column, T>> GetEnumerator()
        {
            return this.lines.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
