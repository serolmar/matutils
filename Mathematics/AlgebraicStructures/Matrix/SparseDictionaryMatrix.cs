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

        public SparseDictionaryMatrix(int lines, int columns, ObjectType defaultValue)
            : this(defaultValue)
        {
            if (lines < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.afterLastLine = lines;
                this.afterLastColumn = columns;
            }
        }

        public SparseDictionaryMatrix(int lines, int columns)
            : this(default(ObjectType))
        {
            if (lines < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.afterLastLine = lines;
                this.afterLastColumn = columns;
            }
        }

        public SparseDictionaryMatrix(ObjectType defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= this.afterLastColumn)
                {
                    throw new ArgumentOutOfRangeException("column");
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
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else if (column < 0 || column >= this.afterLastColumn)
                {
                    throw new ArgumentOutOfRangeException("column");
                }
                else
                {
                    if (!object.ReferenceEquals(this.defaultValue, value) &&
                        this.defaultValue != null &&
                        !this.defaultValue.Equals(value))
                    {
                        var currentLine = default(SparseDictionaryMatrixLine<ObjectType>);
                        if (this.matrixLines.TryGetValue(line, out currentLine))
                        {
                            currentLine[column] = value;
                        }
                        else
                        {
                            var newLine = new SparseDictionaryMatrixLine<ObjectType>(this);
                            newLine[column] = value;
                            this.matrixLines.Add(line, newLine);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        /// <exception cref="MathematicsException">Se a linha não existir.</exception>
        public ISparseMatrixLine<ObjectType> this[int line]
        {
            get
            {
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else
                {
                    var currentLine = default(SparseDictionaryMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(line, out currentLine))
                    {
                        return currentLine;
                    }
                    else
                    {
                        throw new MathematicsException("Line doesn't exist.");
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

        /// <summary>
        /// Obtém o número de linhas com entradas diferentes do valor por defeito.
        /// </summary>
        public int NumberOfLines
        {
            get
            {
                return this.matrixLines.Count;
            }
        }

        /// <summary>
        /// Exapande a matriz um número específico de vezes.
        /// </summary>
        /// <param name="numberOfLines">O número de linhas a acrescentar.</param>
        /// <param name="numberOfColumns">O número de colunas a acrescentar.</param>
        public void ExpandMatrix(int numberOfLines, int numberOfColumns)
        {
            if (numberOfLines < 0)
            {
                throw new ArgumentException("The number of lines must be non-negative.");
            }
            else if (numberOfColumns < 0)
            {
                throw new ArgumentException("The number of columns must be non-negative.");
            }
            else
            {
                this.afterLastLine += numberOfLines;
                this.afterLastColumn += numberOfColumns;
            }
        }

        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        public bool IsSymmetric(IEqualityComparer<ObjectType> equalityComparer)
        {
            var innerEqualityComparer = equalityComparer;
            if (innerEqualityComparer == null)
            {
                innerEqualityComparer = EqualityComparer<ObjectType>.Default;
            }

            if (this.afterLastLine != this.afterLastColumn)
            {
                return false;
            }
            else
            {
                foreach (var line in this.matrixLines)
                {
                    foreach (var column in line.Value)
                    {
                        if (line.Key != column.Key)
                        {
                            var entryLine = default(SparseDictionaryMatrixLine<ObjectType>);
                            if (this.matrixLines.TryGetValue(column.Key, out entryLine))
                            {
                                var entry = default(ObjectType);
                                if (entryLine.MatrixEntries.TryGetValue(line.Key, out entry))
                                {
                                    return innerEqualityComparer.Equals(column.Value, entry);
                                }
                                else
                                {
                                    return innerEqualityComparer.Equals(column.Value, this.defaultValue);
                                }
                            }
                            else
                            {
                                return innerEqualityComparer.Equals(column.Value, this.defaultValue);
                            }
                        }
                    }
                }

                // A matriz nula é simétrica
                return true;
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as linhas com valores diferentes do valor por defeito.
        /// </summary>
        /// <returns>O enumerador para as linhas.</returns>
        public IEnumerable<KeyValuePair<int, ISparseMatrixLine<ObjectType>>> GetLines()
        {
            return new LinesEnumerable<ObjectType>(this.matrixLines);
        }

        public void Remove(int lineNumber)
        {
            if (lineNumber >= 0)
            {
                this.matrixLines.Remove(lineNumber);
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
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new IndexOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastColumn)
            {
                throw new IndexOutOfRangeException("j");
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
                        }
                    }
                    else
                    {
                        var secondLineEntry = default(ObjectType);
                        if (lineDictionary.TryGetValue(j, out secondLineEntry))
                        {
                            lineDictionary.Remove(j);
                            lineDictionary.Add(i, secondLineEntry);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        public bool ContainsLine(int line)
        {
            return this.matrixLines.ContainsKey(line);
        }

        /// <summary>
        /// Obtém as colunas atribuídas à linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>As colunas atribuídas.</returns>
        public IEnumerable<KeyValuePair<int, ObjectType>> GetColumns(int line)
        {
            if (line < 0 || line >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else
            {
                var lineElement = default(SparseDictionaryMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(line, out lineElement))
                {
                    return lineElement.GetColumns();
                }
                else
                {
                    return Enumerable.Empty<KeyValuePair<int, ObjectType>>();
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class LinesEnumerable<T> : IEnumerable<KeyValuePair<int, ISparseMatrixLine<T>>>
        {
            private Dictionary<int, SparseDictionaryMatrixLine<T>> lines;

            public LinesEnumerable(Dictionary<int, SparseDictionaryMatrixLine<T>> lines)
            {
                this.lines = lines;
            }

            public IEnumerator<KeyValuePair<int, ISparseMatrixLine<T>>> GetEnumerator()
            {
                foreach (var line in this.lines)
                {
                    yield return new KeyValuePair<int, ISparseMatrixLine<T>>(line.Key, line.Value);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
