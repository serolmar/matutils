namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Implementa uma matriz esparsa com base em dicionários.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem os argumentos.</typeparam>
    public class SparseDictionaryMatrix<ObjectType> : ISparseMatrix<ObjectType>
    {
        /// <summary>
        /// O objecto responsável pela sicronização de fluxos de execução.
        /// </summary>
        private object lockObject = new object();

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
        private SortedDictionary<int, ISparseMatrixLine<ObjectType>> matrixLines =
            new SortedDictionary<int, ISparseMatrixLine<ObjectType>>(Comparer<int>.Default);

        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
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

        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
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

        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseDictionaryMatrix(ObjectType defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo ou não for inferior ao tamanho
        /// da dimensão respectiva.
        /// </exception>
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
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    lock (this.lockObject)
                    {
                        if (this.matrixLines.TryGetValue(line, out currentLine))
                        {
                            var currentColumn = default(ObjectType);
                            if (currentLine.TryGetColumnValue(column, out currentColumn))
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
                        this.defaultValue == null ||
                        (this.defaultValue != null &&
                        !this.defaultValue.Equals(value)))
                    {
                        var currentLine = default(ISparseMatrixLine<ObjectType>);
                        lock (this.lockObject)
                        {
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
        }

        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a linha especificada for negativa ou não for inferior ao número de linhas.
        /// </exception>
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
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    lock (this.lockObject)
                    {
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
        }

        /// <summary>
        /// Obtém o valor por defeito.
        /// </summary>
        /// <value>
        /// O valor por defeito.
        /// </value>
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
        /// <value>
        /// O número de linhas com entradas diferentes do valor por defeito.
        /// </value>
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
        /// <exception cref="ArgumentException">Se o número de linhas for negativo.</exception>
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
                            var entryLine = default(ISparseMatrixLine<ObjectType>);
                            if (this.matrixLines.TryGetValue(column.Key, out entryLine))
                            {
                                var entry = default(ObjectType);
                                if (entryLine.TryGetColumnValue(line.Key, out entry))
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
            return this.matrixLines;
        }

        /// <summary>
        /// Remove a linha especificada pelo número.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        public void Remove(int lineNumber)
        {
            if (lineNumber >= 0)
            {
                lock (this.lockObject)
                {
                    this.matrixLines.Remove(lineNumber);
                }
            }
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a dimensão for diferente de zero ou um.
        /// </exception>
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

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <remarks>
        /// Se uma linha a trocar exceder o tamanho actual da matriz, esta é aumentada na proporção correcta.
        /// </remarks>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se um dos números de linhas for negativo.
        /// </exception>
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
                lock (this.lockObject)
                {
                    var firstLine = default(ISparseMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(i, out firstLine))
                    {
                        var secondLine = default(ISparseMatrixLine<ObjectType>);
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
                        var secondLine = default(ISparseMatrixLine<ObjectType>);
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
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se um dos números de colunas for negativo.
        /// </exception>
        public void SwapColumns(int i, int j)
        {
            if (i < 0 || i >= this.afterLastColumn)
            {
                throw new IndexOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastColumn)
            {
                throw new IndexOutOfRangeException("j");
            }
            else
            {
                lock (this.lockObject)
                {
                    foreach (var lineKvp in this.matrixLines)
                    {
                        var lineDictionary = lineKvp.Value;
                        var firstLineEntry = default(ObjectType);
                        if (lineDictionary.TryGetColumnValue(i, out firstLineEntry))
                        {
                            var secondLineEntry = default(ObjectType);
                            if (lineDictionary.TryGetColumnValue(j, out secondLineEntry))
                            {
                                lineDictionary[i] = secondLineEntry;
                                lineDictionary[j] = firstLineEntry;
                            }
                            else
                            {
                                lineDictionary.Remove(i);
                                lineDictionary[j] = firstLineEntry;
                            }
                        }
                        else
                        {
                            var secondLineEntry = default(ObjectType);
                            if (lineDictionary.TryGetColumnValue(j, out secondLineEntry))
                            {
                                lineDictionary.Remove(j);
                                lineDictionary[i] = secondLineEntry;
                            }
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
            lock (this.lockObject)
            {
                return this.matrixLines.ContainsKey(line);
            }
        }

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetLine(int index, out ISparseMatrixLine<ObjectType> line)
        {
            var setLine = default(ISparseMatrixLine<ObjectType>);
            if (index < 0 || index >= this.afterLastLine)
            {
                line = setLine;
                return false;
            }
            else
            {
                lock (this.lockObject)
                {
                    if (this.matrixLines.TryGetValue(index, out setLine))
                    {
                        line = setLine;
                        return true;
                    }
                    else
                    {
                        line = setLine;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém as colunas atribuídas à linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>As colunas atribuídas.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número da linha for negativo ou for superior ao número da última linha.
        /// </exception>
        public IEnumerable<KeyValuePair<int, ObjectType>> GetColumns(int line)
        {
            if (line < 0 || line >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else
            {
                var lineElement = default(ISparseMatrixLine<ObjectType>);
                lock (this.lockObject)
                {
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
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">
        /// O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.
        /// </param>
        public virtual void ScalarLineMultiplication(
            int line,
            ObjectType scalar,
            IRing<ObjectType> ring)
        {
            if (line < 0 || line >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (scalar == null)
            {
                throw new ArgumentNullException("scalar");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                if (this.defaultValue == null)
                {
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            foreach (var kvp in currentLine)
                            {
                                otherMatrixElements.Add(kvp.Key, scalar);
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsAdditiveUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, value);
                                }
                                else if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                    }
                }
                else if (ring.IsAdditiveUnity(this.defaultValue))
                {
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            this.Remove(line);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                    }
                }
                else
                {
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    if (this.TryGetLine(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, scalar);
                            }

                            this.SetLine(line, otherMatrixElements);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            var i = 0;
                            var multiplicationValue = ring.Multiply(this.defaultValue, scalar);
                            foreach (var kvp in currentLine)
                            {
                                var currentColumnValue = kvp.Value;
                                for (; i < kvp.Key; ++i)
                                {
                                    otherMatrixElements.Add(i, multiplicationValue);
                                }

                                var valueToAdd = scalar;
                                if (!ring.IsMultiplicativeUnity(this.defaultValue))
                                {
                                    valueToAdd = ring.Multiply(this.defaultValue, scalar);
                                }

                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(
                                        i,
                                        valueToAdd);
                                }

                                ++i;
                            }

                            for (; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, multiplicationValue);
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(line, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(line);
                            }
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                        var valueToAdd = ring.Multiply(this.defaultValue, scalar);;

                        if (!this.defaultValue.Equals(valueToAdd))
                        {
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, valueToAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(line, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(line);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">
        /// O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.
        /// </param>
        [Obsolete]
        public virtual void ScalarLineMultiplication_Old(
            int line,
            ObjectType scalar,
            IRing<ObjectType> ring)
        {
            if (line < 0 || line >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (scalar == null)
            {
                throw new ArgumentNullException("scalar");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                if (this.defaultValue == null)
                {
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            foreach (var kvp in currentLine)
                            {
                                otherMatrixElements.Add(kvp.Key, scalar);
                            }

                            this.matrixLines[line] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsAdditiveUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, value);
                                }
                                else if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.matrixLines[line] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                    }
                }
                else if (ring.IsAdditiveUnity(this.defaultValue))
                {
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            this.matrixLines.Remove(line);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            foreach (var kvp in currentLine)
                            {
                                var value = kvp.Value;
                                if (ring.IsMultiplicativeUnity(value))
                                {
                                    otherMatrixElements.Add(kvp.Key, scalar);
                                }
                                else
                                {
                                    otherMatrixElements.Add(
                                        kvp.Key,
                                        ring.Multiply(value, scalar));
                                }
                            }

                            this.matrixLines[line] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                    }
                }
                else
                {
                    var currentLine = default(ISparseMatrixLine<ObjectType>);
                    if (this.matrixLines.TryGetValue(line, out currentLine))
                    {
                        if (ring.IsAdditiveUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, scalar);
                            }

                            this.matrixLines[line] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else if (!ring.IsMultiplicativeUnity(scalar))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                            var i = 0;
                            var multiplicationValue = ring.Multiply(this.defaultValue, scalar);
                            foreach (var kvp in currentLine)
                            {
                                var currentColumnValue = kvp.Value;
                                for (; i < kvp.Key; ++i)
                                {
                                    otherMatrixElements.Add(i, multiplicationValue);
                                }

                                var valueToAdd = scalar;
                                if (!ring.IsMultiplicativeUnity(this.defaultValue))
                                {
                                    valueToAdd = ring.Multiply(this.defaultValue, scalar);
                                }

                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(
                                        i,
                                        valueToAdd);
                                }

                                ++i;
                            }

                            for (; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, multiplicationValue);
                            }

                            this.matrixLines[line] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                                Comparer<int>.Default);
                        var valueToAdd = scalar;
                        if (!ring.IsMultiplicativeUnity(this.defaultValue))
                        {
                            valueToAdd = ring.Multiply(this.defaultValue, scalar);
                        }

                        if (!this.defaultValue.Equals(valueToAdd))
                        {
                            for (int i = 0; i < this.afterLastColumn; ++i)
                            {
                                otherMatrixElements.Add(i, valueToAdd);
                            }
                        }

                        this.matrixLines.Add(line, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                    }
                }
            }
        }

        /// <summary>
        /// Substitui a linha especificada por uma combinação linear desta com uma outra. Por exemplo, li = a * li + b * lj, isto é,
        /// a linha i é substituída pela soma do produto de a pela linha i com o produto de b peloa linha j.
        /// </summary>
        /// <param name="i">A linha a ser substituída.</param>
        /// <param name="j">A linha a ser combinada.</param>
        /// <param name="a">O escalar a ser multiplicado pela primeira linha.</param>
        /// <param name="b">O escalar a ser multiplicado pela segunda linha.</param>
        /// <param name="ring">O objecto responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="MathematicsException">
        /// Se ocorrer uma tentativa de combinar algum valor nulo de uma linha com um valor não nulo de outra.
        /// </exception>
        public virtual void CombineLines(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (a == null)
            {
                throw new ArgumentNullException("a");
            }
            else if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (this.defaultValue == null)
            {
                this.CombineLinesWithNullValueForDefault(i, j, a, b, ring);
            }
            else if (ring.IsAdditiveUnity(this.defaultValue))
            {
                this.CombineLinesWithAdditiveUnityForDefault(i, j, a, b, ring);
            }
            else
            {
                this.CombineLinesWithSomeValueForDefault(i, j, a, b, ring);
            }
        }

        /// <summary>
        /// Obtém um enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumeador genérico.</returns>
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
        /// Obtém um enumerador não genérico para a matriz.
        /// </summary>
        /// <returns>O enumeador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Private Methods

        /// <summary>
        /// Verifica a integridade das linhas que irão ser adicionadas caso existam valores nulos.
        /// </summary>
        /// <param name="replacementLineNumber">O número da linha a ser substituída.</param>
        /// <param name="replacementLine">A linha a ser substituída.</param>
        /// <param name="combinationLineNumber">O número da linha a ser combinada.</param>
        /// <param name="combinationLine">A linha a ser combinada.</param>
        private void CheckNullDefaultValueLinesIntegrityForCombination(
            int replacementLineNumber,
            ISparseMatrixLine<ObjectType> replacementLine,
            int combinationLineNumber,
            ISparseMatrixLine<ObjectType> combinationLine)
        {
            var replacementLineEnum = replacementLine.GetEnumerator();
            var combinationLineEnum = combinationLine.GetEnumerator();
            var replacementEnumState = replacementLineEnum.MoveNext();
            var combinationState = combinationLineEnum.MoveNext();

            while (replacementEnumState && combinationState)
            {
                if (replacementLineEnum.Current.Key != combinationLineEnum.Current.Key)
                {
                    throw new MathematicsException(string.Format(
                        "Trying to combine a null value from line {0} with a non null value from line {1}.",
                        replacementLineNumber,
                        combinationLineNumber));
                }

                replacementEnumState = replacementLineEnum.MoveNext();
                combinationState = combinationLineEnum.MoveNext();
            }

            if (replacementEnumState)
            {
                throw new MathematicsException(string.Format(
                    "Trying to combine a null value from line {0} with a non null value from line {1}.",
                    replacementLineNumber,
                    combinationLineNumber));
            }
            else if (combinationState)
            {
                throw new MathematicsException(string.Format(
                    "Trying to combine a null value from line {0} with a non null value from line {1}.",
                    replacementLineNumber,
                    combinationLineNumber));
            }

        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja nulo.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a multiplicar pela primeira linha.</param>
        /// <param name="b">O factor a multiplicar pela segunda linha.</param>
        /// <param name="ring">O anel respons+avel pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithNullValueForDefault(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ISparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    // Assevera se é possível efectuar a adição e lança uma excepção em caso contrário.
                    this.CheckNullDefaultValueLinesIntegrityForCombination(
                        i,
                        replacementLine,
                        j,
                        combinationLine);

                    var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                    Comparer<int>.Default);

                    var replacementLineEnum = replacementLine.GetEnumerator();
                    var combinationLineEnum = combinationLine.GetEnumerator();
                    var replacementEnumState = replacementLineEnum.MoveNext();
                    var combinationState = combinationLineEnum.MoveNext();
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.AdditiveUnity);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, combinationLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(b, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (ring.IsMultiplicativeUnity(a))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            otherMatrixElements = (replacementLine as SparseDictionaryMatrixLine<ObjectType>).MatrixEntries;
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.Multiply(a, replacementLineEnum.Current.Value));
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Add(
                                        replacementLineEnum.Current.Value,
                                        combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                valueToAdd = ring.Add(
                                    replacementLineEnum.Current.Value,
                                    valueToAdd);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            valueToAdd = ring.Add(
                                valueToAdd,
                                combinationLineEnum.Current.Value);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }
                    else
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            var valueToAdd = ring.Add(
                                firstValueToAdd,
                                secondValueToAdd);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }

                    this.SetLine(i, otherMatrixElements);
                }
                else if (replacementLine.Any())
                {
                    throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            j,
                            i));
                }
            }
            else
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (combinationLine.Any())
                    {
                        throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            i,
                            j));
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja uma unidade aditiva.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithAdditiveUnityForDefault(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ISparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            this.Remove(i);  // A linha i é removida do contexto.
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in combinationLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, b);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {

                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in replacementLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, a);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas são somadas.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replacementLineEnum = replacementLine.GetEnumerator();
                            var combinationLineEnum = combinationLine.GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Add(replacementLineEnum.Current.Value, combinationLineEnum.Current.Value);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replacementLineEnum = replacementLine.GetEnumerator();
                            var combinationLineEnum = combinationLine.GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replacementLineEnum.Current.Value, valueToAdd);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replacementLineEnum = replacementLine.GetEnumerator();
                        var combinationLineEnum = combinationLine.GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replacementLineEnum = replacementLine.GetEnumerator();
                        var combinationLineEnum = combinationLine.GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        this.Remove(i);
                    }
                    else if (!ring.IsAdditiveUnity(a))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                            Comparer<int>.Default);
                        foreach (var currentLine in replacementLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, a);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
            else // Existe a linha a combinar mas não existe a linha a ser substituída.
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                            Comparer<int>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            otherMatrixElements.Add(currentLine.Key, currentLine.Value);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else if (!ring.IsAdditiveUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                            Comparer<int>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, b);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja um valor arbitrário.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        private void CombineLinesWithSomeValueForDefault(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ISparseMatrixLine<ObjectType>);
            if (this.TryGetLine(i, out replacementLine))
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // É introduzida uma linha nula.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);

                            // Neste ponto é importante multiplicar os valores por defeito.
                            var defaultValueProduct = ring.Multiply(this.defaultValue, b);
                            var combEnum = combinationLine.GetEnumerator();
                            var k = 0;
                            while (combEnum.MoveNext())
                            {
                                var combKey = combEnum.Current.Key;
                                if (this.defaultValue.Equals(defaultValueProduct))
                                {
                                    k = combKey;
                                }
                                else
                                {
                                    while (k < combKey)
                                    {
                                        otherMatrixElements.Add(k, defaultValueProduct);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combEnum.Current.Value, b);
                                if (!ring.Equals(this.defaultValue, valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (!this.defaultValue.Equals(defaultValueProduct))
                            {
                                while (k < this.afterLastColumn)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                    ++k;
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var defaultValueProduct = ring.Multiply(this.defaultValue, a);
                            var replaceEnum = replacementLine.GetEnumerator();
                            var k = 0;
                            while (replaceEnum.MoveNext())
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                while (k < replaceKey)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas somam-se.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replaceEnum = replacementLine.GetEnumerator();
                            var combineEnum = combinationLine.GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultAdd = ring.Add(this.defaultValue, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(replaceEnum.Current.Value, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                        else // Soma da linha i com um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replaceEnum = replacementLine.GetEnumerator();
                            var combineEnum = combinationLine.GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replaceEnum.Current.Value, valueToAdd);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b)) // Soma de um múltiplo da linha i com a linha j.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replaceEnum = replacementLine.GetEnumerator();
                        var combineEnum = combinationLine.GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var defaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                    else // Adiciona múltiplos de ambas as linhas.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replaceEnum = replacementLine.GetEnumerator();
                        var combineEnum = combinationLine.GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var firstsDefaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiplied = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstsDefaultMultiplied, secondDefaultMultiplied);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var firstValueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                            valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a)) // É inserida a linha nula na posição i.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, ring.AdditiveUnity);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (!ring.IsAdditiveUnity(b))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var k = 0;
                            var defaultMultiply = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in replacementLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiply))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiply);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(entry.Value, defaultMultiply);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                            else
                            {
                                this.Remove(i);
                            }
                        }
                    }
                    else if (!ring.IsAdditiveUnity(b)) // Adiciona os valores por defeito da linha inexistente.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var k = 0;
                        var firstDefaultMultiply = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiply = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstDefaultMultiply, secondDefaultMultiply);

                        foreach (var entry in replacementLine)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiply);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }


                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                        else
                        {
                            this.Remove(i);
                        }
                    }
                }
            }
            else
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.TryGetLine(j, out combinationLine))
                {
                    // Existe a linha a combinar mas não existe a linha a ser substituída.
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // Terá de ser acrescentada uma linha vazia.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // É acrescentada uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var entry in combinationLine)
                            {
                                otherMatrixElements.Add(entry.Key, entry.Value);
                            }

                            this.SetLine(i, otherMatrixElements);
                        }
                        else // É acrescentado um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in combinationLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiplied))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiplied);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(entry.Value, b);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.SetLine(i, otherMatrixElements);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, this.defaultValue);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Add(firstDefaultMultiplication, entry.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var secondMultiplication = ring.Multiply(this.defaultValue, b);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, secondMultiplication);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, b);
                            valueToAdd = ring.Add(firstDefaultMultiplication, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.SetLine(i, otherMatrixElements);
                        }
                    }
                }
                else // Não existem ambas as linhas.
                {
                    var defaultValueToAdd = ring.Add(a, b);
                    defaultValueToAdd = ring.Multiply(defaultValueToAdd, this.defaultValue);
                    if (!this.defaultValue.Equals(defaultValueToAdd))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, defaultValueToAdd);
                        }

                        this.SetLine(i, otherMatrixElements);
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja nulo.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a multiplicar pela primeira linha.</param>
        /// <param name="b">O factor a multiplicar pela segunda linha.</param>
        /// <param name="ring">O anel respons+avel pelas operações sobre as entradas da matriz.</param>
        [Obsolete]
        private void CombineLinesWithNullValueForDefault_Old(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ISparseMatrixLine<ObjectType>);
            if (this.matrixLines.TryGetValue(i, out replacementLine))
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(j, out combinationLine))
                {
                    // Assevera se é possível efectuar a adição e lança uma excepção em caso contrário.
                    this.CheckNullDefaultValueLinesIntegrityForCombination(
                        i,
                        replacementLine,
                        j,
                        combinationLine);

                    var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                    Comparer<int>.Default);

                    var replacementLineEnum = replacementLine.GetEnumerator();
                    var combinationLineEnum = combinationLine.GetEnumerator();
                    var replacementEnumState = replacementLineEnum.MoveNext();
                    var combinationState = combinationLineEnum.MoveNext();
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.AdditiveUnity);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, combinationLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(b, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (ring.IsMultiplicativeUnity(a))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            otherMatrixElements = (replacementLine as SparseDictionaryMatrixLine<ObjectType>).MatrixEntries;
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                otherMatrixElements.Add(key, ring.Multiply(a, replacementLineEnum.Current.Value));
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b))
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Add(
                                        replacementLineEnum.Current.Value,
                                        combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                        else
                        {
                            while (replacementEnumState && combinationState)
                            {
                                var key = replacementLineEnum.Current.Key;
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                valueToAdd = ring.Add(
                                    replacementLineEnum.Current.Value,
                                    valueToAdd);
                                otherMatrixElements.Add(key, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            valueToAdd = ring.Add(
                                valueToAdd,
                                combinationLineEnum.Current.Value);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }
                    else
                    {
                        while (replacementEnumState && combinationState)
                        {
                            var key = replacementLineEnum.Current.Key;
                            var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            var valueToAdd = ring.Add(
                                firstValueToAdd,
                                secondValueToAdd);
                            otherMatrixElements.Add(key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                            combinationState = combinationLineEnum.MoveNext();
                        }
                    }

                    this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                }
                else if (replacementLine.Any())
                {
                    throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            j,
                            i));
                }
            }
            else
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(j, out combinationLine))
                {
                    if (combinationLine.Any())
                    {
                        throw new MathematicsException(string.Format(
                            "Trying to combine a null value from line {0} with a non null value from line {1}.",
                            i,
                            j));
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja uma unidade aditiva.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        [Obsolete]
        private void CombineLinesWithAdditiveUnityForDefault_Old(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ISparseMatrixLine<ObjectType>);
            if (this.matrixLines.TryGetValue(i, out replacementLine))
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b))
                        {
                            this.matrixLines.Remove(i);  // A linha i é removida do contexto.
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in combinationLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, b);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {

                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in replacementLine)
                            {
                                var valueToAdd = ring.Multiply(line.Value, a);
                                otherMatrixElements.Add(line.Key, valueToAdd);
                            }

                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas são somadas.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replacementLineEnum = replacementLine.GetEnumerator();
                            var combinationLineEnum = combinationLine.GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Add(replacementLineEnum.Current.Value, combinationLineEnum.Current.Value);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replacementLineEnum = replacementLine.GetEnumerator();
                            var combinationLineEnum = combinationLine.GetEnumerator();
                            var replacementEnumState = replacementLineEnum.MoveNext();
                            var combinationState = combinationLineEnum.MoveNext();
                            var state = replacementEnumState && combinationState;

                            while (replacementEnumState && combinationState)
                            {
                                var replacementKey = replacementLineEnum.Current.Key;
                                var combinationKey = combinationLineEnum.Current.Key;
                                if (replacementKey < combinationKey)
                                {
                                    otherMatrixElements.Add(replacementKey, replacementLineEnum.Current.Value);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    state = replacementEnumState;
                                }
                                else if (replacementKey == combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replacementLineEnum.Current.Value, valueToAdd);
                                    otherMatrixElements.Add(replacementKey, valueToAdd);
                                    replacementEnumState = replacementLineEnum.MoveNext();
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = replacementEnumState && combinationState;
                                }
                                else if (replacementKey > combinationKey)
                                {
                                    var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                    otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                    combinationState = combinationLineEnum.MoveNext();
                                    state = combinationState;
                                }
                            }

                            while (replacementEnumState)
                            {
                                otherMatrixElements.Add(replacementLineEnum.Current.Key, replacementLineEnum.Current.Value);
                                replacementEnumState = replacementLineEnum.MoveNext();
                            }

                            while (combinationState)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                            }

                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replacementLineEnum = replacementLine.GetEnumerator();
                        var combinationLineEnum = combinationLine.GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combinationLineEnum.Current.Value);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, combinationLineEnum.Current.Value);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replacementLineEnum = replacementLine.GetEnumerator();
                        var combinationLineEnum = combinationLine.GetEnumerator();
                        var replacementEnumState = replacementLineEnum.MoveNext();
                        var combinationState = combinationLineEnum.MoveNext();
                        var state = replacementEnumState && combinationState;

                        while (replacementEnumState && combinationState)
                        {
                            var replacementKey = replacementLineEnum.Current.Key;
                            var combinationKey = combinationLineEnum.Current.Key;
                            if (replacementKey < combinationKey)
                            {
                                var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                state = replacementEnumState;
                            }
                            else if (replacementKey == combinationKey)
                            {
                                var firstValueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                otherMatrixElements.Add(replacementKey, valueToAdd);
                                replacementEnumState = replacementLineEnum.MoveNext();
                                combinationState = combinationLineEnum.MoveNext();
                                state = replacementEnumState && combinationState;
                            }
                            else if (replacementKey > combinationKey)
                            {
                                var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                                otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                                combinationState = combinationLineEnum.MoveNext();
                                state = combinationState;
                            }
                        }

                        while (replacementEnumState)
                        {
                            var valueToAdd = ring.Multiply(replacementLineEnum.Current.Value, a);
                            otherMatrixElements.Add(replacementLineEnum.Current.Key, valueToAdd);
                            replacementEnumState = replacementLineEnum.MoveNext();
                        }

                        while (combinationState)
                        {
                            var valueToAdd = ring.Multiply(combinationLineEnum.Current.Value, b);
                            otherMatrixElements.Add(combinationLineEnum.Current.Key, valueToAdd);
                            combinationState = combinationLineEnum.MoveNext();
                        }

                        this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        this.matrixLines.Remove(i);
                    }
                    else if (!ring.IsAdditiveUnity(a))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                            Comparer<int>.Default);
                        foreach (var currentLine in replacementLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, a);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                    }
                }
            }
            else // Existe a linha a combinar mas não existe a linha a ser substituída.
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(j, out combinationLine))
                {
                    if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                            Comparer<int>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            otherMatrixElements.Add(currentLine.Key, currentLine.Value);
                        }

                        this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                    }
                    else if (!ring.IsAdditiveUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(
                            Comparer<int>.Default);
                        foreach (var currentLine in combinationLine)
                        {
                            var valueToAdd = ring.Multiply(currentLine.Value, b);
                            otherMatrixElements.Add(currentLine.Key, valueToAdd);
                        }

                        this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                    }
                }
            }
        }

        /// <summary>
        /// Efectua a combinação das linhas caso o valor por defeito seja um valor arbitrário.
        /// </summary>
        /// <param name="i">O número da linha a ser substituída.</param>
        /// <param name="j">O número da linha a ser combinada.</param>
        /// <param name="a">O factor a ser multipicado pela linha a ser substituída.</param>
        /// <param name="b">O factor a ser multiplicado pela linha a ser combinada.</param>
        /// <param name="ring">O anel responsável pelas operações sobre as entradas da matriz.</param>
        [Obsolete]
        private void CombineLinesWithSomeValueForDefault_Old(
            int i,
            int j,
            ObjectType a,
            ObjectType b,
            IRing<ObjectType> ring)
        {
            var replacementLine = default(ISparseMatrixLine<ObjectType>);
            if (this.matrixLines.TryGetValue(i, out replacementLine))
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(j, out combinationLine))
                {
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // É introduzida uma linha nula.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // A linha i passa a ser uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var line in combinationLine)
                            {
                                otherMatrixElements.Add(line.Key, line.Value);
                            }

                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else if (!ring.IsAdditiveUnity(b)) // A linha i passa a ser múltipla da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);

                            // Neste ponto é importante multiplicar os valores por defeito.
                            var defaultValueProduct = ring.Multiply(this.defaultValue, b);
                            var combEnum = combinationLine.GetEnumerator();
                            var k = 0;
                            while (combEnum.MoveNext())
                            {
                                var combKey = combEnum.Current.Key;
                                if (this.defaultValue.Equals(defaultValueProduct))
                                {
                                    k = combKey;
                                }
                                else
                                {
                                    while (k < combKey)
                                    {
                                        otherMatrixElements.Add(k, defaultValueProduct);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combEnum.Current.Value, b);
                                if (!ring.Equals(this.defaultValue, valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (!this.defaultValue.Equals(defaultValueProduct))
                            {
                                while (k < this.afterLastColumn)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                    ++k;
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                            }
                            else
                            {
                                this.matrixLines.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsAdditiveUnity(b))
                    {
                        if (!ring.IsMultiplicativeUnity(a)) // A linha i passa a ser múltipla dela própria e se a = 1, mantém-se.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var defaultValueProduct = ring.Multiply(this.defaultValue, a);
                            var replaceEnum = replacementLine.GetEnumerator();
                            var k = 0;
                            while (replaceEnum.MoveNext())
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                while (k < replaceKey)
                                {
                                    otherMatrixElements.Add(k, defaultValueProduct);
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (ring.IsMultiplicativeUnity(b)) // As linhas somam-se.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replaceEnum = replacementLine.GetEnumerator();
                            var combineEnum = combinationLine.GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultAdd = ring.Add(this.defaultValue, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(replaceEnum.Current.Value, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                            }
                            else
                            {
                                this.matrixLines.Remove(i);
                            }
                        }
                        else // Soma da linha i com um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var replaceEnum = replacementLine.GetEnumerator();
                            var combineEnum = combinationLine.GetEnumerator();
                            var replaceState = replaceEnum.MoveNext();
                            var combineState = combineEnum.MoveNext();
                            var state = replaceState && combineState;
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                            while (state)
                            {
                                var replaceKey = replaceEnum.Current.Key;
                                var combineKey = combineEnum.Current.Key;
                                if (replaceKey < combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    state = replaceState;
                                }
                                else if (replaceKey == combineKey)
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = replaceKey;
                                    }
                                    else
                                    {
                                        while (k < replaceKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                    valueToAdd = ring.Add(replaceEnum.Current.Value, valueToAdd);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    replaceState = replaceEnum.MoveNext();
                                    combineState = combineEnum.MoveNext();
                                    state = replaceState && combineState;
                                }
                                else
                                {
                                    if (this.defaultValue.Equals(defaultAdd))
                                    {
                                        k = combineKey;
                                    }
                                    else
                                    {
                                        while (k < combineKey)
                                        {
                                            otherMatrixElements.Add(k, defaultAdd);
                                            ++k;
                                        }
                                    }

                                    var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                    if (!this.defaultValue.Equals(valueToAdd))
                                    {
                                        otherMatrixElements.Add(k, valueToAdd);
                                    }

                                    ++k;
                                    combineState = combineEnum.MoveNext();
                                    state = combineState;
                                }
                            }

                            while (replaceState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < replaceEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, replaceEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                            }

                            while (combineState)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineEnum.Current.Key;
                                }
                                else
                                {
                                    while (k < combineEnum.Current.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(this.defaultValue, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                            }

                            // Adiciona os valores que restam.
                            if (!this.defaultValue.Equals(defaultAdd))
                            {
                                for (; k < this.afterLastColumn; ++k)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                }
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                            }
                            else
                            {
                                this.matrixLines.Remove(i);
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b)) // Soma de um múltiplo da linha i com a linha j.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replaceEnum = replacementLine.GetEnumerator();
                        var combineEnum = combinationLine.GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var defaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var defaultAdd = ring.Add(defaultMultiplied, this.defaultValue);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, this.defaultValue);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Add(defaultMultiplied, combineEnum.Current.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else
                        {
                            this.matrixLines.Remove(i);
                        }
                    }
                    else // Adiciona múltiplos de ambas as linhas.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var replaceEnum = replacementLine.GetEnumerator();
                        var combineEnum = combinationLine.GetEnumerator();
                        var replaceState = replaceEnum.MoveNext();
                        var combineState = combineEnum.MoveNext();
                        var state = replaceState && combineState;
                        var k = 0;
                        var firstsDefaultMultiplied = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiplied = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstsDefaultMultiplied, secondDefaultMultiplied);

                        while (state)
                        {
                            var replaceKey = replaceEnum.Current.Key;
                            var combineKey = combineEnum.Current.Key;
                            if (replaceKey < combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                state = replaceState;
                            }
                            else if (replaceKey == combineKey)
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = replaceKey;
                                }
                                else
                                {
                                    while (k < replaceKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var firstValueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                                var secondValueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                var valueToAdd = ring.Add(firstValueToAdd, secondValueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                replaceState = replaceEnum.MoveNext();
                                combineState = combineEnum.MoveNext();
                                state = replaceState && combineState;
                            }
                            else
                            {
                                if (this.defaultValue.Equals(defaultAdd))
                                {
                                    k = combineKey;
                                }
                                else
                                {
                                    while (k < combineKey)
                                    {
                                        otherMatrixElements.Add(k, defaultAdd);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                                valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                                combineState = combineEnum.MoveNext();
                                state = combineState;
                            }
                        }

                        while (replaceState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = replaceEnum.Current.Key;
                            }
                            else
                            {
                                while (k < replaceEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(replaceEnum.Current.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiplied);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            replaceState = replaceEnum.MoveNext();
                        }

                        while (combineState)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = combineEnum.Current.Key;
                            }
                            else
                            {
                                while (k < combineEnum.Current.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(combineEnum.Current.Value, b);
                            valueToAdd = ring.Add(firstsDefaultMultiplied, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                            combineState = combineEnum.MoveNext();
                        }

                        // Adiciona os valores que restam.
                        if (!this.defaultValue.Equals(defaultAdd))
                        {
                            for (; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, defaultAdd);
                            }
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else
                        {
                            this.matrixLines.Remove(i);
                        }
                    }
                }
                else // Existe a linha a ser substituída mas não existe a linha a combinar.
                {
                    if (ring.IsAdditiveUnity(a)) // É inserida a linha nula na posição i.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, ring.AdditiveUnity);
                        }

                        this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                    }
                    else if (ring.IsMultiplicativeUnity(a))
                    {
                        if (!ring.IsAdditiveUnity(b))
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var k = 0;
                            var defaultMultiply = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in replacementLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiply))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiply);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Add(entry.Value, defaultMultiply);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                            }
                            else
                            {
                                this.matrixLines.Remove(i);
                            }
                        }
                    }
                    else if (!ring.IsAdditiveUnity(b)) // Adiciona os valores por defeito da linha inexistente.
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var k = 0;
                        var firstDefaultMultiply = ring.Multiply(this.defaultValue, a);
                        var secondDefaultMultiply = ring.Multiply(this.defaultValue, b);
                        var defaultAdd = ring.Add(firstDefaultMultiply, secondDefaultMultiply);

                        foreach (var entry in replacementLine)
                        {
                            if (this.defaultValue.Equals(defaultAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultAdd);
                                    ++k;
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, a);
                            valueToAdd = ring.Add(valueToAdd, secondDefaultMultiply);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }


                        if (otherMatrixElements.Any())
                        {
                            this.matrixLines[i] = new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this);
                        }
                        else
                        {
                            this.matrixLines.Remove(i);
                        }
                    }
                }
            }
            else
            {
                var combinationLine = default(ISparseMatrixLine<ObjectType>);
                if (this.matrixLines.TryGetValue(j, out combinationLine))
                {
                    // Existe a linha a combinar mas não existe a linha a ser substituída.
                    if (ring.IsAdditiveUnity(a))
                    {
                        if (ring.IsAdditiveUnity(b)) // Terá de ser acrescentada uma linha vazia.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            for (int k = 0; k < this.afterLastColumn; ++k)
                            {
                                otherMatrixElements.Add(k, ring.AdditiveUnity);
                            }

                            this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                        }
                        else if (ring.IsMultiplicativeUnity(b)) // É acrescentada uma cópia da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            foreach (var entry in combinationLine)
                            {
                                otherMatrixElements.Add(entry.Key, entry.Value);
                            }

                            this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                        }
                        else // É acrescentado um múltiplo da linha j.
                        {
                            var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                            var k = 0;
                            var defaultMultiplied = ring.Multiply(this.defaultValue, b);
                            foreach (var entry in combinationLine)
                            {
                                if (this.defaultValue.Equals(defaultMultiplied))
                                {
                                    k = entry.Key;
                                }
                                else
                                {
                                    while (k < entry.Key)
                                    {
                                        otherMatrixElements.Add(k, defaultMultiplied);
                                        ++k;
                                    }
                                }

                                var valueToAdd = ring.Multiply(entry.Value, b);
                                if (!this.defaultValue.Equals(valueToAdd))
                                {
                                    otherMatrixElements.Add(k, valueToAdd);
                                }

                                ++k;
                            }

                            if (otherMatrixElements.Any())
                            {
                                this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                            }
                        }
                    }
                    else if (ring.IsMultiplicativeUnity(b))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, this.defaultValue);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Add(firstDefaultMultiplication, entry.Value);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                        }
                    }
                    else
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        var k = 0;
                        var firstDefaultMultiplication = ring.Multiply(this.defaultValue, a);
                        var secondMultiplication = ring.Multiply(this.defaultValue, b);
                        var defaultValueToAdd = ring.Add(firstDefaultMultiplication, secondMultiplication);
                        foreach (var entry in combinationLine)
                        {
                            if (this.defaultValue.Equals(defaultValueToAdd))
                            {
                                k = entry.Key;
                            }
                            else
                            {
                                while (k < entry.Key)
                                {
                                    otherMatrixElements.Add(k, defaultValueToAdd);
                                }
                            }

                            var valueToAdd = ring.Multiply(entry.Value, b);
                            valueToAdd = ring.Add(firstDefaultMultiplication, valueToAdd);
                            if (!this.defaultValue.Equals(valueToAdd))
                            {
                                otherMatrixElements.Add(k, valueToAdd);
                            }

                            ++k;
                        }

                        if (otherMatrixElements.Any())
                        {
                            this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                        }
                    }
                }
                else // Não existem ambas as linhas.
                {
                    var defaultValueToAdd = ring.Add(a, b);
                    defaultValueToAdd = ring.Multiply(defaultValueToAdd, this.defaultValue);
                    if (!this.defaultValue.Equals(defaultValueToAdd))
                    {
                        var otherMatrixElements = new SortedDictionary<int, ObjectType>(Comparer<int>.Default);
                        for (int k = 0; k < this.afterLastColumn; ++k)
                        {
                            otherMatrixElements.Add(k, defaultValueToAdd);
                        }

                        this.matrixLines.Add(i, new SparseDictionaryMatrixLine<ObjectType>(otherMatrixElements, this));
                    }
                }
            }
        }

        /// <summary>
        /// Atribui o conjunto de elementos à linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="values">O conjunto de valores.</param>
        private void SetLine(int index, SortedDictionary<int, ObjectType> values)
        {
            lock (this.lockObject)
            {
                if (this.matrixLines.ContainsKey(index))
                {
                    this.matrixLines[index] = new SparseDictionaryMatrixLine<ObjectType>(values, this);
                }
                else
                {
                    this.matrixLines.Add(index, new SparseDictionaryMatrixLine<ObjectType>(values, this));
                }
            }
        }

        #endregion Private Methods
    }
}
