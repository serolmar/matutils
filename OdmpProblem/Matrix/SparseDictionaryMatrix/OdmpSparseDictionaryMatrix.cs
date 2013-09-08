using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    public class OdmpSparseDictionaryMatrix<ComponentType, LineType, ColumnType, T> : IOdmpMatrix<ComponentType, LineType, ColumnType, T>
    {
        private T defaultValue;

        private Dictionary<LineType, OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T>> lines;

        private IEqualityComparer<ColumnType> columnsComparer;

        private ComponentType component;

        public OdmpSparseDictionaryMatrix(ComponentType component, T defaultValue)
            : this(component, null, defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public OdmpSparseDictionaryMatrix(ComponentType component, T defaultValue, IEqualityComparer<LineType> linesComparer, IEqualityComparer<ColumnType> columnsComparer)
        {
            if (linesComparer == null)
            {
                this.lines = new Dictionary<LineType, OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T>>();
            }
            else
            {
                this.lines = new Dictionary<LineType, OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T>>(linesComparer);
            }

            this.columnsComparer = columnsComparer;
        }

        internal OdmpSparseDictionaryMatrix(ComponentType component, Dictionary<LineType, OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T>> lines, T defaultValue)
        {
            if (lines == null)
            {
                this.lines = new Dictionary<LineType, OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T>>();
            }
            else
            {
                this.lines = lines;
            }

            this.component = component;
            this.defaultValue = defaultValue;
        }

        public IOdmpMatrixRow<LineType, ColumnType, T> this[LineType line]
        {
            get
            {
                OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T> result = null;
                if (!this.lines.TryGetValue(line, out result))
                {
                    throw new OdmpProblemException("int doesn't exist.");
                }
                else
                {
                    return result;
                }
            }
        }

        public T this[LineType line, ColumnType column]
        {
            get
            {
                OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T> dictionaryint = null;
                if (!this.lines.TryGetValue(line, out dictionaryint))
                {
                    return this.defaultValue;
                }
                else
                {
                    T result = default(T);
                    if (!dictionaryint.LineElements.TryGetValue(column, out result))
                    {
                        throw new OdmpProblemException("Line doesn't exist in specified line.");
                    }

                    return result;
                }
            }
            set
            {
                OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T> dictionaryint = null;
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
                    dictionaryint = new OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T>(line, this.columnsComparer);
                    this.lines.Add(line, dictionaryint);
                    dictionaryint.LineElements.Add(column, value);
                }
            }
        }

        public ComponentType Component
        {
            get
            {
                return this.component;
            }
        }

        public int Count
        {
            get
            {
                return this.lines.Count;
            }
        }

        public bool ContainsLine(LineType line)
        {
            return this.lines.ContainsKey(line);
        }

        public bool ContainsColumn(LineType line, ColumnType column)
        {
            OdmpSparseDictionaryMatrixRow<LineType, ColumnType, T> dictionaryint = null;
            if (!this.lines.TryGetValue(line, out dictionaryint))
            {
                return false;
            }
            else
            {
                return dictionaryint.LineElements.ContainsKey(column);
            }
        }

        public IEnumerator<IOdmpMatrixRow<LineType, ColumnType, T>> GetEnumerator()
        {
            return this.lines.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
