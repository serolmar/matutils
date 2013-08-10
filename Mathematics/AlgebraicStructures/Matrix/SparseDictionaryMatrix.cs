namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public class SparseDictionaryMatrix<ObjectType> : ISparseMatrix<ObjectType>
    {
        /// <summary>
        /// O objecto a ser retornado quando o índice não foi definido.
        /// </summary>
        private ObjectType defaultValue;

        /// <summary>
        /// O valor que sucede o número da última linha introduzida.
        /// </summary>
        private int afterLastLine;

        /// <summary>
        /// O valor que sucede o número da última coluna introduzida.
        /// </summary>
        private int afterLastColumn;

        /// <summary>
        /// As linhas da matriz.
        /// </summary>
        private Dictionary<int, SparseDictionaryMatrixLine<ObjectType>> matrixLines =
            new Dictionary<int, SparseDictionaryMatrixLine<ObjectType>>();

        public SparseDictionaryMatrix(ObjectType defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0)
                {
                    throw new ArgumentOutOfRangeException("Line index must be a non-negative number.");
                }
                else if (column < 0)
                {
                    throw new ArgumentOutOfRangeException("Column index must be a non-negative number.");
                }
                else
                {
                    var currentLine = default(SparseDictionaryMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(line, out currentLine))
                    {
                        var currentColumn = default(ObjectType);
                        if (currentLine.MatrixEntries.TryGetValue(column, out currentColumn))
                        {
                            return currentColumn;
                        }
                        else
                        {
                            return defaultValue;
                        }
                    }
                    else
                    {
                        return this.defaultValue;
                    }
                }
            }
            set
            {
                if (line < 0)
                {
                    throw new ArgumentOutOfRangeException("Line index must be a non-negative number.");
                }
                else if (column < 0)
                {
                    throw new ArgumentOutOfRangeException("Column index must be a non-negative number.");
                }
                else
                {
                    if (line >= this.afterLastLine)
                    {
                        var newLine = new SparseDictionaryMatrixLine<ObjectType>(this);
                        newLine[column] = value;
                        this.matrixLines.Add(line, newLine);
                        this.afterLastLine = line + 1;
                    }
                    else
                    {
                        var currentLine = default(SparseDictionaryMatrixLine<ObjectType>);
                        if (this.matrixLines.TryGetValue(line, out currentLine))
                        {
                            currentLine[column] = value;
                        }
                        else
                        {
                            var newLine = new SparseDictionaryMatrixLine<ObjectType>(this);
                            this.matrixLines.Add(line, newLine);
                        }

                    }

                    if (column >= this.afterLastColumn)
                    {
                        this.afterLastColumn = column + 1;
                    }
                }
            }
        }

        public ObjectType DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        public int NumberOfLines
        {
            get
            {
                return this.matrixLines.Count;
            }
        }

        public IEnumerator<KeyValuePair<int, ISparseMatrixLine<ObjectType>>> GetLines()
        {
            foreach (var kvp in this.matrixLines)
            {
                yield return new KeyValuePair<int, ISparseMatrixLine<ObjectType>>(kvp.Key, kvp.Value);
            }
        }

        public void Remove(int lineNumber)
        {
            if (lineNumber >= 0)
            {
                var currentLine = default(SparseDictionaryMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(lineNumber, out currentLine))
                {
                    this.matrixLines.Remove(lineNumber);
                    if (lineNumber == this.afterLastLine - 1)
                    {
                        var maximumIndex = 0;
                        foreach (var kvp in this.matrixLines)
                        {
                            if (kvp.Key > maximumIndex)
                            {
                                maximumIndex = kvp.Key;
                            }
                        }

                        this.afterLastLine = maximumIndex + 1;
                    }

                    this.UpdateLastColumnNumber(currentLine.AfterLastColumnNumber);
                }
            }
        }

        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.afterLastLine;
            }
            else if (dimension == 1)
            {
                return this.afterLastColumn;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Matrix dimension value must be one of 0 or 1.");
            }
        }

        public IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ObjectType>(this, lines, columns);
        }

        public IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
        }

        public void SwapLines(int i, int j)
        {
            if (i < 0)
            {
                throw new IndexOutOfRangeException("Index i must be non negative.");
            }
            else if (j < 0)
            {
                throw new IndexOutOfRangeException("Index j must be non-negative.");
            }
            else
            {
                var firstLine = default(SparseDictionaryMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(i, out firstLine))
                {
                    var secondLine = default(SparseDictionaryMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(j, out secondLine))
                    {
                        this.matrixLines[i] = secondLine;
                        this.matrixLines[j] = firstLine;
                    }
                    else
                    {
                        this.matrixLines.Remove(i);
                        this.matrixLines.Add(j, firstLine);
                        if (j >= this.afterLastLine)
                        {
                            this.afterLastLine = j + 1;
                        }
                        else if (i == this.afterLastLine - 1)
                        {
                            var maximumIndex = 0;
                            foreach (var kvp in this.matrixLines)
                            {
                                if (kvp.Key > maximumIndex)
                                {
                                    maximumIndex = kvp.Key;
                                }
                            }

                            this.afterLastLine = maximumIndex + 1;
                        }
                    }
                }
                else
                {
                    var secondLine = default(SparseDictionaryMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(j, out secondLine))
                    {
                        this.matrixLines.Remove(j);
                        this.matrixLines.Add(i, secondLine);
                        if (i >= this.afterLastLine)
                        {
                            this.afterLastLine = i + 1;
                        }
                        else if (j == this.afterLastLine - 1)
                        {
                            var maximumIndex = 0;
                            foreach (var kvp in this.matrixLines)
                            {
                                if (kvp.Key > maximumIndex)
                                {
                                    maximumIndex = kvp.Key;
                                }
                            }

                            this.afterLastLine = maximumIndex + 1;
                        }
                    }
                }
            }
        }

        public void SwapColumns(int i, int j)
        {
            if (i < 0)
            {
                throw new IndexOutOfRangeException("Index i must be non negative.");
            }
            else if (j < 0)
            {
                throw new IndexOutOfRangeException("Index j must be non-negative.");
            }
            else
            {
                foreach (var lineKvp in this.matrixLines)
                {
                    var lineDictionary = lineKvp.Value.MatrixEntries;
                    var firstLineEntry = default(ObjectType);
                    if (lineDictionary.TryGetValue(i, out firstLineEntry))
                    {
                        var secondLineEntry = default(ObjectType);
                        if (lineDictionary.TryGetValue(j, out secondLineEntry))
                        {
                            lineDictionary[i] = secondLineEntry;
                            lineDictionary[j] = firstLineEntry;
                        }
                        else
                        {
                            lineDictionary.Remove(i);
                            lineDictionary.Add(j, firstLineEntry);
                            if (j >= this.afterLastColumn)
                            {
                                this.afterLastColumn = j + 1;
                                lineKvp.Value.AfterLastColumnNumber = j + 1;
                            }
                            else if (i == this.afterLastColumn - 1)
                            {
                                var maximumColumnNumber = 0;
                                lineKvp.Value.UpdateAfterLastLine();
                                foreach (var kvp in this.matrixLines)
                                {
                                    var comparisionValue = kvp.Value.AfterLastColumnNumber;
                                    if (comparisionValue > maximumColumnNumber)
                                    {
                                        maximumColumnNumber = comparisionValue;
                                    }
                                }

                                this.afterLastColumn = maximumColumnNumber;
                            }
                        }
                    }
                    else
                    {
                        var secondLineEntry = default(ObjectType);
                        if (lineDictionary.TryGetValue(j, out secondLineEntry))
                        {
                            lineDictionary.Remove(j);
                            lineDictionary.Add(i, secondLineEntry);
                            if (i >= this.afterLastColumn)
                            {
                                this.afterLastColumn = i + 1;
                                lineKvp.Value.AfterLastColumnNumber = i + 1;
                            }
                            else if (j == this.afterLastColumn - 1)
                            {
                                var maximumColumnNumber = 0;
                                lineKvp.Value.UpdateAfterLastLine();
                                foreach (var kvp in this.matrixLines)
                                {
                                    var comparisionValue = kvp.Value.AfterLastColumnNumber;
                                    if (comparisionValue > maximumColumnNumber)
                                    {
                                        maximumColumnNumber = comparisionValue;
                                    }
                                }

                                this.afterLastColumn = maximumColumnNumber;
                            }
                        }
                    }
                }
            }
        }

        public IEnumerator<ObjectType> GetEnumerator()
        {
            foreach (var lineKvp in this.matrixLines)
            {
                foreach (var columnKvp in lineKvp.Value)
                {
                    yield return columnKvp.Value;
                }
            }
        }

        /// <summary>
        /// Actualiza o valor da última coluna no caso de serem removidas entradas
        /// a uma linha.
        /// </summary>
        /// <param name="numberOfElementsInLine">O número de elementos na linha que foi removida.</param>
        internal void UpdateLastColumnNumber(int numberOfElementsInLine)
        {
            if (numberOfElementsInLine > this.afterLastColumn)
            {
                var maximumColumn = 0;
                foreach (var kvp in this.matrixLines)
                {
                    if (kvp.Value.AfterLastColumnNumber > maximumColumn)
                    {
                        maximumColumn = kvp.Value.AfterLastColumnNumber;
                    }
                }

                this.afterLastColumn = maximumColumn;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
