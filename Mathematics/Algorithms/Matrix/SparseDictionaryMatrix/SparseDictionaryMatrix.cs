using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class SparseDictionaryMatrix<T> : IMatrix<int, int, int, T>
    {
        private T defaultValue;

        private int matrixNumber = -1;

        private int lastLineNumber = -1;

        private int lastColumnNumber = -1;

        private Dictionary<int, SparseDictionaryMatrixRow<T>> lines;

        public SparseDictionaryMatrix(T defaultValue, int matrixNumber)
            : this(null, defaultValue)
        {
            this.defaultValue = defaultValue;
            this.matrixNumber = matrixNumber;
        }

        internal SparseDictionaryMatrix(Dictionary<int, SparseDictionaryMatrixRow<T>> lines, T defaultValue)
        {
            if (lines == null)
            {
                this.lines = new Dictionary<int, SparseDictionaryMatrixRow<T>>();
            }
            else
            {
                this.lines = lines;
            }

            this.defaultValue = defaultValue;
        }

        public IMatrixRow<int, int, T> this[int line]
        {
            get
            {
                SparseDictionaryMatrixRow<T> result = null;
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

        public T this[int line, int column]
        {
            get
            {
                if (line < 0 || column < 0)
                {
                    throw new MathematicsException("Line or column can't be negative.");
                }

                if (line > this.lastLineNumber)
                {
                    throw new MathematicsException("Line doesn't exist.");
                }

                if (column > this.lastColumnNumber)
                {
                    throw new MathematicsException("Column doesn't exist.");
                }

                SparseDictionaryMatrixRow<T> dictionaryint = null;
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
                SparseDictionaryMatrixRow<T> dictionaryint = null;
                if (this.lines.TryGetValue(line, out dictionaryint))
                {
                    if (dictionaryint.LineElements.ContainsKey(column))
                    {
                        dictionaryint.LineElements[column] = value;
                    }
                    else
                    {
                        dictionaryint.LineElements.Add(column, value);
                        if (column > this.lastColumnNumber)
                        {
                            this.lastColumnNumber = column;
                        }
                    }
                }
                else
                {
                    dictionaryint = new SparseDictionaryMatrixRow<T>(line);
                    this.lines.Add(line, dictionaryint);
                    dictionaryint.LineElements.Add(column, value);
                    if (line > this.lastLineNumber)
                    {
                        this.lastLineNumber = line;
                    }

                    if (column > this.lastColumnNumber)
                    {
                        this.lastColumnNumber = column;
                    }
                }
            }
        }

        public int MatrixNumber
        {
            get
            {
                return this.matrixNumber;
            }
        }

        public bool ContainsLine(int line)
        {
            return this.lines.ContainsKey(line);
        }

        public bool ContainsColumn(int line, int column)
        {
            SparseDictionaryMatrixRow<T> dictionaryint = null;
            if (!this.lines.TryGetValue(line, out dictionaryint))
            {
                return false;
            }
            else
            {
                return dictionaryint.LineElements.ContainsKey(column);
            }
        }

        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.lastLineNumber + 1;
            }
            else if (dimension == 1)
            {
                return this.lastColumnNumber + 1;
            }
            else
            {
                throw new MathematicsException("Matrix has only two dimensions.");
            }
        }

        public IEnumerator<IMatrixRow<int, int, T>> GetEnumerator()
        {
            return this.lines.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
