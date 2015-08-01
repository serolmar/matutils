namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;
    using Utilities.Collections;

    /// <summary>
    /// Representação em termos de coordenadas de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    public class CoordinateSparseMatrix<CoeffType> : ISparseMatrix<CoeffType>
    {
        /// <summary>
        /// Mantém o valor por defeito.
        /// </summary>
        private CoeffType defaultValue;

        /// <summary>
        /// Mantém a lista dos elementos.
        /// </summary>
        private List<MutableTuple<int, int, MutableTuple<CoeffType>>> elements;

        /// <summary>
        /// O comparador que permite averiguara igualdade com o coeficiente por defeito.
        /// </summary>
        private IEqualityComparer<CoeffType> comparer;

        /// <summary>
        /// Mantém o número de linhas.
        /// </summary>
        private int numberOfLines;

        /// <summary>
        /// Mantém o número de colunas.
        /// </summary>
        private int numberOfColumns;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito a ser assumido pela matriz.</param>
        public CoordinateSparseMatrix(CoeffType defaultValue)
        {
            this.defaultValue = default(CoeffType);
            this.comparer = EqualityComparer<CoeffType>.Default;
            this.elements = new List<MutableTuple<int, int, MutableTuple<CoeffType>>>();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        public CoordinateSparseMatrix(int lines, int columns)
            : this(default(CoeffType))
        {
            if (lines < 0)
            {
                throw new ArgumentOutOfRangeException("lines");
            }
            else if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }
            else
            {
                this.numberOfColumns = lines;
                this.numberOfColumns = columns;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public CoordinateSparseMatrix(int lines, int columns, CoeffType defaultValue)
            : this(defaultValue)
        {
            if (lines < 0)
            {
                throw new ArgumentOutOfRangeException("lines");
            }
            else if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }
            else
            {
                this.numberOfColumns = lines;
                this.numberOfColumns = columns;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinateSparseMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador que permite identificar os valores por defeito inseridos.</param>
        public CoordinateSparseMatrix(
            int lines,
            int columns,
            CoeffType defaultValue,
            IEqualityComparer<CoeffType> comparer)
            : this(lines, columns, defaultValue)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
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
        public ISparseMatrixLine<CoeffType> this[int line]
        {
            get { throw new NotImplementedException(); }
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
        public CoeffType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("The parameter column is out of bounds.");
                }
                else
                {
                    var elementsCount = this.elements.Count;
                    if (elementsCount == 0)
                    {
                        return this.defaultValue;
                    }
                    else
                    {
                        var index = this.FindLowestPosition(line, column, 0, elementsCount);
                        if (index < this.elements.Count)
                        {
                            var current = this.elements[index];
                            if (this.CompareLineAndColumn(line, column, current) == 0)
                            {
                                return current.Item3.Item1;
                            }
                            else
                            {
                                return this.defaultValue;
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
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException("The parameter column is out of bounds.");
                }
                else
                {
                    var elementsCount = this.elements.Count;
                    if (elementsCount == 0)
                    {
                        if (!this.comparer.Equals(value, this.defaultValue))
                        {
                            this.elements.Add(
                                    MutableTuple.Create(line, column, MutableTuple<CoeffType>.Create(value)));
                        }
                    }
                    else
                    {
                        var index = this.FindLowestPosition(line, column, 0, elementsCount);
                        if (index < this.elements.Count)
                        {
                            var current = this.elements[index];
                            if (this.CompareLineAndColumn(line, column, current) == 0)
                            {
                                if (this.comparer.Equals(value, this.defaultValue))
                                {
                                    this.elements.RemoveAt(index);
                                }
                                else
                                {
                                    current.Item3.Item1 = value;
                                }
                            }
                            else
                            {
                                if (!this.comparer.Equals(value, this.defaultValue))
                                {
                                    this.elements.Insert(
                                        index,
                                        MutableTuple.Create(line, column, MutableTuple<CoeffType>.Create(value)));
                                }
                            }
                        }
                        else
                        {
                            if (!this.comparer.Equals(value, this.defaultValue))
                            {
                                this.elements.Add(
                                    MutableTuple.Create(line, column, MutableTuple<CoeffType>.Create(value)));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o valor por defeito que está associado à matriz.
        /// </summary>
        public CoeffType DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        /// <summary>
        /// Obtém o número de linhas da matriz.
        /// </summary>
        public int NumberOfLines
        {
            get
            {
                return this.numberOfLines;
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as linhas não nulas da matriz.
        /// </summary>
        /// <remarks>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as linhas em sequência crescente pela chave.
        /// </remarks>
        /// <returns>As linhas não nulas da matriz.</returns>
        public IEnumerable<KeyValuePair<int, ISparseMatrixLine<CoeffType>>> GetLines()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        public void Remove(int lineNumber)
        {
            var elementsCount = this.elements.Count;
            if (elementsCount > 0)
            {
                var both = this.FindBothPositions(lineNumber, 0, elementsCount);
            }
        }

        /// <summary>
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        public bool ContainsLine(int line)
        {
            var elementsCount = this.elements.Count;
            if (elementsCount == 0)
            {
                return false;
            }
            else
            {
                var index = this.FindLowestPosition(line, 0, elementsCount);
                if (index < elementsCount)
                {
                    var current = this.elements[index];
                    if (current.Item1 == line)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetLine(int index, out ISparseMatrixLine<CoeffType> line)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém as colunas atribuídas à linha especificada.
        /// </summary>
        /// <remarks>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as colunas em sequência crescente pela chave.
        /// </remarks>
        /// <param name="line">A linha.</param>
        /// <returns>As colunas atribuídas.</returns>
        public IEnumerable<KeyValuePair<int, CoeffType>> GetColumns(int line)
        {
            var elementsCount = this.elements.Count;
            if (elementsCount > 0)
            {

                var both = this.FindBothPositions(line, 0, elementsCount);
                var start = both.Item1;
                var end = both.Item2;
                for (int i = start; i < end; ++i)
                {
                    var current = this.elements[i];
                    yield return new KeyValuePair<int, CoeffType>(current.Item2, current.Item3.Item1);
                }
            }
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas conforme o valor do argumento
        /// seja 0 ou 1.
        /// </summary>
        /// <param name="dimension">O valor do argumento.</param>
        /// <returns>O valor número de linhas ou colunas.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// O valor do argumento é diferente de 0 e de 1.
        /// </exception>
        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.numberOfLines;
            }
            else if (dimension == 1)
            {
                return this.numberOfColumns;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(int[] lines, int[] columns)
        {
            return new SubMatrix<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(
            IntegerSequence lines,
            IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        public void SwapLines(int i, int j)
        {
            if (i < 0 || i >= this.numberOfLines)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.numberOfLines)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else
            {
                var first = i;
                var second = j;
                if (j < i)
                {
                    first = j;
                    second = i;
                }

                var elementsCount = this.elements.Count;
                var firstLine = this.FindBothPositions(first, 0, elementsCount);
                var secondLine = this.FindBothPositions(second, firstLine.Item2, elementsCount);
                var k = firstLine.Item1;
                var n = firstLine.Item2;
                var l = secondLine.Item1;
                var m = secondLine.Item2;
                while (k < n && l < m)
                {
                    var firstCurrentValue = this.elements[k];
                    var secondCurrentValue = this.elements[l];
                    var firstColumCoord = firstCurrentValue.Item2;
                    var firstColumnValue = firstCurrentValue.Item3.Item1;
                    firstCurrentValue.Item2 = secondCurrentValue.Item2;
                    firstCurrentValue.Item3.Item1 = secondCurrentValue.Item3.Item1;
                    secondCurrentValue.Item2 = firstColumCoord;
                    secondCurrentValue.Item3.Item1 = firstColumnValue;
                    ++k;
                    ++l;
                }

                // Remove da primeira linha e coloca na segunda
                while (k < n)
                {
                    var firstCurrentValue = this.elements[k];
                    this.elements.RemoveAt(k);
                    --n;
                    --l;
                    firstCurrentValue.Item1 = second;
                    this.elements.Insert(l, firstCurrentValue);
                }

                // Remove da segunda linha e coloca na primeira
                while (l < m)
                {
                    var secondCurrentValue = this.elements[l];
                    this.elements.RemoveAt(l);
                    secondCurrentValue.Item2 = first;
                    this.elements.Insert(k, secondCurrentValue);
                    ++k;
                }
            }
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        public void SwapColumns(int i, int j)
        {
            if (i < 0 || i >= this.numberOfLines)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.numberOfLines)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else
            {
                var first = i;
                var second = j;
                if (j < i)
                {
                    first = j;
                    second = i;
                }

                var length = this.elements.Count;
                var firstLineIndex = 0;
                while (firstLineIndex < length)
                {
                    var firstElement = this.elements[firstLineIndex];
                    var firstLineNumber = firstElement.Item1;

                    // O número de elementos associados a uma linha é limitado pelo número de colunas
                    var lastLineIndexLimit = firstLineIndex + this.numberOfColumns;
                    var lastLineIndex = this.FindGreatestPosition(
                        firstLineNumber,
                        firstLineIndex,
                        Math.Min(length, lastLineIndexLimit));

                    var firstColumnIndex = this.FindColumn(first, firstLineIndex, lastLineIndex);

                    // Note-se que a segunda coluna vem depois da primeira
                    if (firstColumnIndex < lastLineIndexLimit)
                    {
                        var secondColumnIndex = this.FindColumn(second, firstColumnIndex, lastLineIndex);
                        if (secondColumnIndex < lastLineIndexLimit)
                        {
                            // Ambas as colunas se encontram dentro dos limites
                            var firstColumnValue = this.elements[firstColumnIndex];
                            var secondColumnValue = this.elements[secondColumnIndex];

                            if (firstColumnValue.Item2 == first)
                            {
                                if (secondColumnValue.Item2 == second)
                                {
                                    // Ambas as colunas contêm valores
                                    var secondValue = secondColumnValue.Item3.Item1;
                                    secondColumnValue.Item3.Item1 = firstColumnValue.Item3.Item1;
                                    firstColumnValue.Item3.Item1 = secondValue;
                                    firstColumnValue.Item2 = second;
                                    secondColumnValue.Item2 = first;
                                }
                                else
                                {
                                    // Apenas a primeira coluna contém valores
                                    this.elements.Insert(secondColumnIndex, firstColumnValue);
                                    this.elements.RemoveAt(firstColumnIndex);
                                    firstColumnValue.Item2 = second;
                                }
                            }
                            else if(secondColumnValue.Item2 == second)
                            {
                                this.elements.RemoveAt(secondColumnIndex);
                                this.elements.Insert(firstColumnIndex, secondColumnValue);
                                secondColumnValue.Item2 = first;
                            }
                        }
                        else
                        {
                            // A primeira coluna encontra-se dentro dos limites mas a segunda não
                            var firstColumnValue = this.elements[firstColumnIndex];
                            if (firstColumnValue.Item2 == first)
                            {
                                this.elements.Insert(secondColumnIndex, firstColumnValue);
                                this.elements.RemoveAt(firstColumnIndex);
                                firstColumnValue.Item2 = second;
                            }
                        }
                    }

                    firstLineIndex = lastLineIndex;
                }
            }
        }

        /// <summary>
        /// Multiplica os valores da linha pelo escalar definido.
        /// </summary>
        /// <param name="line">A linha a ser considerada.</param>
        /// <param name="scalar">O escalar a ser multiplicado.</param>
        /// <param name="ring">O objecto responsável pela operações de multiplicação e determinação da unidade aditiva.</param>
        public void ScalarLineMultiplication(int line, CoeffType scalar, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (line < 0 || line >= this.numberOfLines)
            {
                throw new IndexOutOfRangeException("The parameter line is out of bounds.");
            }
            else
            {
                var elementsCount = this.elements.Count;
                if (elementsCount > 0)
                {
                    var both = this.FindBothPositions(line, 0, elementsCount);
                    var start = both.Item1;
                    if (start < elementsCount)
                    {
                        var end = both.Item2;
                        var defaultMultiplied = ring.Multiply(this.defaultValue, scalar);
                        if (this.comparer.Equals(this.defaultValue, defaultMultiplied))
                        {
                            // A multiplicação pelo valor por defeito continua a resultar no valor por defeito
                            var currentIndex = start;
                            while (currentIndex < end)
                            {
                                var current = this.elements[currentIndex];
                                var result = ring.Multiply(current.Item3.Item1, scalar);
                                if (this.comparer.Equals(this.defaultValue, result))
                                {
                                    this.elements.RemoveAt(currentIndex);
                                    --end;
                                }
                                else
                                {
                                    current.Item3.Item1 = result;
                                    ++currentIndex;
                                }
                            }
                        }
                        else
                        {
                            // A multiplicação pelo valor por defeito deixa de ser um valor por defeito
                            var currentIndex = start;
                            var column = 0;
                            while (currentIndex < end)
                            {
                                var current = this.elements[currentIndex];
                                var currentColumn = current.Item2;
                                while (column < currentColumn)
                                {
                                    this.elements.Insert(
                                        currentIndex,
                                        MutableTuple.Create(line, column, MutableTuple.Create(defaultMultiplied)));
                                    ++column;
                                    ++currentIndex;
                                    ++end;
                                }

                                var result = ring.Multiply(current.Item3.Item1, scalar);
                                if (this.comparer.Equals(this.defaultValue, result))
                                {
                                    this.elements.RemoveAt(currentIndex);
                                    --end;
                                }
                                else
                                {
                                    current.Item3.Item1 = result;
                                    ++currentIndex;
                                }

                                ++column;
                            }

                            // Insere os últimos valores na linha
                            for (; column < this.numberOfColumns; ++column)
                            {
                                this.elements.Insert(
                                        currentIndex,
                                        MutableTuple.Create(line, column, MutableTuple.Create(defaultMultiplied)));
                            }
                        }
                    }
                    else
                    {
                        // A linha é superior a todos os elementos existentes
                        var multiple = ring.Multiply(this.defaultValue, scalar);
                        if (!this.comparer.Equals(multiple, this.defaultValue))
                        {
                            // O valor obtido terá de ser adicionado para todas as colunas
                            for (int i = 0; i < this.numberOfColumns; ++i)
                            {
                                this.elements.Add(MutableTuple.Create(
                                    line,
                                    i,
                                    MutableTuple.Create(multiple)));
                            }
                        }
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
        public void CombineLines(int i, int j, CoeffType a, CoeffType b, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (i < 0 || i >= this.numberOfLines)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.numberOfLines)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i == j)
            {
                var scalar = ring.Add(a, b);
                this.ScalarLineMultiplication(i, scalar, ring);
            }
            else
            {
                var elementsCount = this.elements.Count;
                if (elementsCount > 0)
                {
                    var firstValues = default(Tuple<int, int>);
                    var secondValues = default(Tuple<int, int>);
                    if (i < j)
                    {
                        // A linha a substituir é anterior à linha a ser combinada
                        firstValues = this.FindBothPositions(i, 0, elementsCount);
                        secondValues = this.FindBothPositions(j, firstValues.Item2, elementsCount);
                    }
                    else
                    {
                        // A linha a substituir é posterior à linha a ser combinada
                        secondValues = this.FindBothPositions(j, 0, elementsCount);
                        firstValues = this.FindBothPositions(i, secondValues.Item2, elementsCount);
                    }

                    // Se a combinação de valores por defeito for um valor por defeito, o cálculo simplifica-se
                    var firstScalar = ring.Multiply(this.defaultValue, a);
                    var secondScalar = ring.Multiply(this.defaultValue, b);
                    var additionValue = ring.Add(firstScalar, secondScalar);
                    if (this.comparer.Equals(additionValue, this.defaultValue))
                    {
                        // Neste caso os valores por defeito são ignorados
                        var k = firstValues.Item1;
                        var n = firstValues.Item2;
                        var l = secondValues.Item1;
                        var m = secondValues.Item2;

                        while (k < n && l < m)
                        {
                            var currentFirst = this.elements[k];
                            var currentSecond = this.elements[l];
                            if (currentFirst.Item2 < currentSecond.Item2)
                            {
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstScalar,
                                    secondCurrentScalar);
                                if (!this.comparer.Equals(this.defaultValue, additionValue))
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                ++l;
                            }
                            else if (currentFirst.Item2 == currentSecond.Item2)
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    secondCurrentScalar);
                                if (this.comparer.Equals(
                                    currentAddition,
                                    this.defaultValue))
                                {
                                    // É marcado o item para remoção
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = additionValue;
                                }
                            }
                            else
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    this.defaultValue);
                                if (this.comparer.Equals(currentAddition, this.defaultValue))
                                {
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = currentAddition;
                                }

                                ++k;
                            }
                        }

                        // Continuação para os termos restantes
                        while (k < n)
                        {
                            var currentFirst = this.elements[k];
                            var firstCurrentScalar = ring.Multiply(
                                       a,
                                       currentFirst.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstCurrentScalar,
                                this.defaultValue);
                            if (this.comparer.Equals(currentAddition, this.defaultValue))
                            {
                                this.elements.RemoveAt(k);
                                --k;
                                --l;
                                --m;
                                --n;
                            }
                            else
                            {
                                currentFirst.Item3.Item1 = currentAddition;
                            }

                            ++k;
                        }

                        while (l < m)
                        {
                            var currentSecond = this.elements[l];
                            var secondCurrentScalar = ring.Multiply(
                                       b,
                                       currentSecond.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstScalar,
                                secondCurrentScalar);
                            if (!this.comparer.Equals(this.defaultValue, additionValue))
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            ++l;
                        }
                    }
                    else
                    {
                        // Neste caso os valores por defeito não são ignorados
                        var k = firstValues.Item1;
                        var n = firstValues.Item2;
                        var l = secondValues.Item1;
                        var m = secondValues.Item2;

                        while (k < n && l < m)
                        {
                            var currentFirst = this.elements[k];
                            var currentSecond = this.elements[l];
                            if (currentFirst.Item2 < currentSecond.Item2)
                            {
                                for (int aux = currentFirst.Item2 + 1; aux < currentSecond.Item2; ++aux)
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstScalar,
                                    secondCurrentScalar);
                                if (!this.comparer.Equals(this.defaultValue, additionValue))
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                ++l;
                            }
                            else if (currentFirst.Item2 == currentSecond.Item2)
                            {
                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var secondCurrentScalar = ring.Multiply(
                                    b,
                                    currentSecond.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    secondCurrentScalar);
                                if (this.comparer.Equals(
                                    currentAddition,
                                    this.defaultValue))
                                {
                                    // É marcado o item para remoção
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = additionValue;
                                }
                            }
                            else
                            {
                                for (int aux = currentSecond.Item2 + 1; aux < currentFirst.Item2; ++aux)
                                {
                                    // A soma não consiste no valor por defeito
                                    this.elements.Insert(
                                        k,
                                        MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                    // Como um elemento foi inserido, todos os índices terão de ser alterados
                                    ++k;
                                    ++n;
                                    ++l;
                                    ++m;
                                }

                                var firstCurrentScalar = ring.Multiply(
                                    a,
                                    currentFirst.Item3.Item1);
                                var currentAddition = ring.Add(
                                    firstCurrentScalar,
                                    this.defaultValue);
                                if (this.comparer.Equals(currentAddition, this.defaultValue))
                                {
                                    this.elements.RemoveAt(k);
                                    --k;
                                    --l;
                                    --m;
                                    --n;
                                }
                                else
                                {
                                    currentFirst.Item3.Item1 = currentAddition;
                                }

                                ++k;
                            }
                        }

                        // Continuação para os termos restantes
                        while (k < n)
                        {
                            var currentFirst = this.elements[k];
                            for (int aux = m; aux < currentFirst.Item2; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            var firstCurrentScalar = ring.Multiply(
                                       a,
                                       currentFirst.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstCurrentScalar,
                                this.defaultValue);
                            if (this.comparer.Equals(currentAddition, this.defaultValue))
                            {
                                this.elements.RemoveAt(k);
                                --l;
                                --m;
                                --n;
                            }
                            else
                            {
                                currentFirst.Item3.Item1 = currentAddition;
                                ++k;
                            }
                        }

                        while (l < m)
                        {
                            var currentSecond = this.elements[l];
                            for (int aux = n; aux < currentSecond.Item2; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            var secondCurrentScalar = ring.Multiply(
                                       b,
                                       currentSecond.Item3.Item1);
                            var currentAddition = ring.Add(
                                firstScalar,
                                secondCurrentScalar);
                            if (!this.comparer.Equals(this.defaultValue, additionValue))
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, currentSecond.Item2, MutableTuple.Create(currentAddition)));

                                // Como um elemento foi inserido, todos os índices terão de ser alterados
                                ++k;
                                ++n;
                                ++l;
                                ++m;
                            }

                            ++l;
                        }

                        if (m <= n)
                        {
                            for (int aux = k; aux < this.numberOfColumns; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));
                                ++k;
                            }
                        }
                        else
                        {
                            for (int aux = l; aux < this.numberOfColumns; ++aux)
                            {
                                // A soma não consiste no valor por defeito
                                this.elements.Insert(
                                    k,
                                    MutableTuple.Create(i, aux, MutableTuple.Create(additionValue)));
                                ++k;
                            }
                        }
                    }
                }
                else
                {
                    var firstScalar = ring.Multiply(this.defaultValue, a);
                    var secondScalar = ring.Multiply(this.defaultValue, b);
                    var additionValue = ring.Add(firstScalar, secondScalar);
                    if (!this.comparer.Equals(additionValue, this.defaultValue))
                    {
                        // O valor obtido terá de ser adicionado para todas as colunas
                        for (int k = 0; k < this.numberOfColumns; ++k)
                        {
                            this.elements.Add(MutableTuple.Create(
                                i,
                                k,
                                MutableTuple.Create(additionValue)));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador para todos os valores não nulos da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            foreach (var kvp in this.elements)
            {
                yield return kvp.Item3.Item1;
            }
        }

        #region Funções privadas

        /// <summary>
        /// Obtém um enumerador não genérico para todos os valores não nulos da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Encontra a posição onde o elemento especificado se encontra, retornando, em caso
        /// de empate, a posição do primeiro.
        /// </summary>
        /// <param name="line">O número da linha.</param>
        /// <param name="column">O número da coluna.</param>
        /// <param name="start">O início do intervalor de procura, inclusivé.</param>
        /// <param name="end">O final do intervalo de procura, exclusivé.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindLowestPosition(
            int line,
            int column,
            int start,
            int end)
        {
            if (this.CompareLineAndColumn(line, column, this.elements[start]) <= 0)
            {
                return start;
            }
            else if (this.CompareLineAndColumn(
                line,
                column,
                this.elements[end - 1]) > 0)
            {
                return end;
            }
            else
            {
                int low = start;
                int high = end - 1;
                while (low < high)
                {
                    int sum = high + low;
                    int intermediaryIndex = sum / 2;
                    if (sum % 2 == 0)
                    {
                        if (this.CompareLineAndColumn(line, column, this.elements[intermediaryIndex]) <= 0)
                        {
                            high = intermediaryIndex;
                        }
                        else
                        {
                            low = intermediaryIndex;
                        }
                    }
                    else
                    {
                        if (
                            this.CompareLineAndColumn(line, column, this.elements[intermediaryIndex]) > 0 &&
                            this.CompareLineAndColumn(line, column, this.elements[intermediaryIndex + 1]) <= 0)
                        {
                            return intermediaryIndex + 1;
                        }
                        else if (
                            this.CompareLineAndColumn(line, column, this.elements[intermediaryIndex]) == 0 &&
                            this.CompareLineAndColumn(line, column, this.elements[intermediaryIndex + 1]) < 0)
                        {
                            return intermediaryIndex;
                        }
                        else if (this.CompareLineAndColumn(line, column, this.elements[intermediaryIndex]) > 0)
                        {
                            low = intermediaryIndex;
                        }
                        else
                        {
                            high = intermediaryIndex;
                        }
                    }
                }

                return low;
            }
        }

        /// <summary>
        /// Encontra a menor posição onde o elemento especificado se encontra, retornando, em caso
        /// de empate, a posição do primeiro. Caso não exista, retorna a posição onde deverá ser inserido.
        /// </summary>
        /// <param name="line">O número da linha.</param>
        /// <param name="start">O início do intervalor de procura, inclusivé.</param>
        /// <param name="end">O final do intervalo de procura, exclusivé.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindLowestPosition(int line, int start, int end)
        {
            if (this.CompareLine(line, this.elements[start]) <= 0)
            {
                return start;
            }
            else if (this.CompareLine(
                line,
                this.elements[end - 1]) > 0)
            {
                return end;
            }
            else
            {
                return this.AuxiliaryFindLowestPosition(line, start, end - 1);
            }
        }

        /// <summary>
        /// Encontra a posição do primeiro elemento cujo valor é igual ao especificado. Caso não exista,
        /// é retornada a posição onde este poderá ser inserido. A função aplica-se apenas ao caso em que
        /// o elemento se encontra dentro dos limites dados.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <param name="low">O limite inferior do intervalo.</param>
        /// <param name="high">O limite superior do intervalo.</param>
        /// <returns>A posição.</returns>
        private int AuxiliaryFindLowestPosition(
            int line,
            int low,
            int high)
        {
            var innerLow = low;
            var innerHigh = high;
            while (innerLow < innerHigh)
            {
                int sum = innerHigh + innerLow;
                int intermediaryIndex = sum >> 2;
                if ((sum & 1) == 0)
                {
                    if (this.CompareLine(line, this.elements[intermediaryIndex]) <= 0)
                    {
                        innerHigh = intermediaryIndex;
                    }
                    else
                    {
                        innerLow = intermediaryIndex;
                    }
                }
                else
                {
                    if (
                        this.CompareLine(line, this.elements[intermediaryIndex]) > 0 &&
                        this.CompareLine(line, this.elements[intermediaryIndex + 1]) <= 0)
                    {
                        return intermediaryIndex + 1;
                    }
                    else if (
                        this.CompareLine(line, this.elements[intermediaryIndex]) == 0 &&
                        this.CompareLine(line, this.elements[intermediaryIndex + 1]) < 0)
                    {
                        high = intermediaryIndex;
                    }
                    else if (this.CompareLine(line, this.elements[intermediaryIndex]) > 0)
                    {
                        innerLow = intermediaryIndex;
                    }
                    else
                    {
                        innerHigh = intermediaryIndex;
                    }
                }
            }

            return innerLow;
        }

        /// <summary>
        /// Encontra o elemento a seguir à maior posição onde o elemento especificado se encontra. Se este não existir,
        /// retorna a posição onde deve ser inserido.
        /// </summary>
        /// <param name="line">O elemento a ser procurado.</param>
        /// <param name="start">O início do intervalor de procura, inclusivé.</param>
        /// <param name="end">O final do intervalo de procura, exclusivé.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindGreatestPosition(int line, int start, int end)
        {
            if (this.CompareLine(line, this.elements[start]) < 0)
            {
                return start;
            }
            else if (this.CompareLine(line, this.elements[end - 1]) >= 0)
            {
                return end;
            }
            else
            {
                return this.AuxiliaryFindGreatestPosition(line, start, end - 1);
            }
        }

        /// <summary>
        /// Obtém o índice do elemento que se encontra a seguir ao maior elemento igual ao especificado.
        /// Se este não existir, retorna a posição onde deverá ser inserido. Esta função é aplicada apenas
        /// ao caso em que a posição do elemento a procurar se encontra dentro dos limites dados.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <param name="low">O limite inferior do intervalo de procura.</param>
        /// <param name="high">O limite superior do intervalo de procura.</param>
        /// <returns>A posição procurada.</returns>
        private int AuxiliaryFindGreatestPosition(int line, int low, int high)
        {
            var innerLow = low;
            var innerHigh = high;
            while (innerLow < innerHigh)
            {
                int sum = innerHigh + innerLow;
                int intermediaryIndex = sum / 2;
                if (sum % 2 == 0)
                {
                    if (this.CompareLine(line, this.elements[intermediaryIndex]) < 0)
                    {
                        innerHigh = intermediaryIndex;
                    }
                    else
                    {
                        innerLow = intermediaryIndex;
                    }
                }
                else
                {
                    if (
                        this.CompareLine(line, this.elements[intermediaryIndex]) > 0 &&
                        this.CompareLine(line, this.elements[intermediaryIndex + 1]) < 0)
                    {
                        return intermediaryIndex + 1;
                    }
                    else if (
                        this.CompareLine(line, this.elements[intermediaryIndex]) == 0 &&
                        this.CompareLine(line, this.elements[intermediaryIndex + 1]) < 0)
                    {
                        return intermediaryIndex;
                    }
                    else if (this.CompareLine(line, this.elements[intermediaryIndex + 1]) == 0)
                    {
                        innerLow = intermediaryIndex + 1;
                    }
                    else if (this.CompareLine(line, this.elements[intermediaryIndex]) < 0)
                    {
                        innerHigh = intermediaryIndex;
                    }
                    else
                    {
                        innerLow = intermediaryIndex;
                    }
                }
            }

            return innerHigh;
        }

        /// <summary>
        /// Permite determinar ambas as posições, a máxima e a mínima relativas às linhas.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <param name="start">O início do intervalor de procura, inclusivé.</param>
        /// <param name="end">O final do intervalo de procura, exclusivé.</param>
        /// <returns>O par que contém as posições.</returns>
        private Tuple<int, int> FindBothPositions(int line, int start, int end)
        {
            var current = this.elements[end - 1];
            var lineComparision = this.CompareLine(line, current);
            if (lineComparision > 0)
            {
                return MutableTuple.Create(end, end);
            }
            else if (lineComparision == 0)
            {
                var high = end - 1;
                var low = high;

                // Efectua o ciclo para determinar o menor valor.
                current = this.elements[start];
                lineComparision = this.CompareLine(line, current);
                if (lineComparision < 0)
                {
                    low = this.AuxiliaryFindLowestPosition(line, start, high);
                }

                return MutableTuple.Create(low, high);
            }
            else
            {
                current = this.elements[start];
                lineComparision = this.CompareLine(line, current);
                if (lineComparision < 0)
                {
                    return MutableTuple.Create(start, start);
                }
                else if (lineComparision == 0)
                {
                    var low = start;
                    var high = this.AuxiliaryFindGreatestPosition(line, start, end - 1);
                    return MutableTuple.Create(low, high);
                }
                else
                {
                    var auxLow = start;
                    var auxHigh = end - 1;
                    while (auxLow < auxHigh)
                    {
                        int sum = auxHigh + auxLow;
                        int intermediaryIndex = sum / 2;
                        if (sum % 2 == 0)
                        {
                            if (this.CompareLine(line, this.elements[intermediaryIndex]) < 0)
                            {
                                auxHigh = intermediaryIndex;
                            }
                            else
                            {
                                auxLow = intermediaryIndex;
                            }
                        }
                        else
                        {
                            if (
                                this.CompareLine(line, this.elements[intermediaryIndex]) > 0 &&
                                this.CompareLine(line, this.elements[intermediaryIndex + 1]) < 0)
                            {
                                var result = intermediaryIndex + 1;
                                return MutableTuple.Create(result, result);
                            }
                            else if (
                                this.CompareLine(line, this.elements[intermediaryIndex]) == 0)
                            {
                                // Um elemento foi encontrado.
                                var low = this.AuxiliaryFindLowestPosition(line, auxLow, intermediaryIndex);
                                var high = this.AuxiliaryFindGreatestPosition(line, intermediaryIndex, auxHigh);
                                return MutableTuple.Create(low, high);
                            }
                            else if (this.CompareLine(line, this.elements[intermediaryIndex]) < 0)
                            {
                                auxHigh = intermediaryIndex;
                            }
                            else
                            {
                                auxLow = intermediaryIndex;
                            }
                        }
                    }

                    return MutableTuple.Create(auxLow, auxHigh);
                }
            }
        }

        /// <summary>
        /// Permite determinar a posição da coluna especificada.
        /// </summary>
        /// <param name="column">A coluna.</param>
        /// <param name="start">O índice a partir do qual se efectua a pesquisa.</param>
        /// <param name="end">O índice até ao qual é efectuada a pesquisa.</param>
        /// <returns>O índice onde se encontra a coluna.</returns>
        private int FindColumn(int column, int start, int end)
        {
            int low = start;
            int high = end - 1;
            while (low < high)
            {
                int sum = high + low;
                int intermediaryIndex = sum / 2;
                if ((sum & 1) == 0)
                {
                    var intermediaryElement = this.elements[intermediaryIndex].Item2;
                    if (column == intermediaryElement)
                    {
                        return intermediaryIndex;
                    }
                    else if (column < intermediaryElement)
                    {
                        high = intermediaryIndex;
                    }
                    else
                    {
                        low = intermediaryIndex;
                    }
                }
                else
                {
                    var intermediaryElement = this.elements[intermediaryIndex].Item2;
                    var nextIntermediaryElement = this.elements[intermediaryIndex + 1].Item2;
                    if (
                        column > intermediaryElement &&
                        column <= intermediaryElement)
                    {
                        return intermediaryIndex + 1;
                    }
                    else if (column == intermediaryElement)
                    {
                        return intermediaryIndex;
                    }
                    else if (column > intermediaryElement)
                    {
                        low = intermediaryIndex;
                    }
                    else
                    {
                        high = intermediaryIndex;
                    }
                }
            }

            return low;
        }

        /// <summary>
        /// Permite comparar as linhas.
        /// </summary>
        /// <param name="line">O número da linha a ser comparada.</param>
        /// <param name="element">O terno que contém as coordenadas e o valor da entrada da matriz.</param>
        /// <returns>
        /// O valor -1 se a linha for inferior, 0 se for igual e 1 se for superior à linha do elemento da matriz.
        /// </returns>
        private int CompareLine(int line, Tuple<int, int, MutableTuple<CoeffType>> element)
        {
            if (line < element.Item1)
            {
                return -1;
            }
            else if (line == element.Item1)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Permite comparar as linhas e as colunas.
        /// </summary>
        /// <param name="line">A linha a ser comparada.</param>
        /// <param name="column">A coluna a ser comparada.</param>
        /// <param name="element">O terno que contém as coordenadas e o valor da entrada da matriz.</param>
        /// <returns>
        /// O valor -1 se a linha for inferior, 0 se for igual e 1 se for superior à linha do elemento da matriz.
        /// </returns>
        public int CompareLineAndColumn(int line, int column, Tuple<int, int, MutableTuple<CoeffType>> element)
        {
            if (line < element.Item1)
            {
                return -1;
            }
            else if (line == element.Item1)
            {
                if (column < element.Item2)
                {
                    return -1;
                }
                else if (column == element.Item2)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }

        #endregion Funções privadas
    }
}
