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
        private List<Tuple<int, int, MutableTuple<CoeffType>>> elements;

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
            this.elements = new List<Tuple<int, int, MutableTuple<CoeffType>>>();
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
                    var index = this.FindLowestPosition(line, column);
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
                    if (this.comparer.Equals(value, this.defaultValue))
                    {
                        var key = Tuple.Create(line, column);
                        throw new NotImplementedException();
                    }
                    else
                    {
                        var index = this.FindLowestPosition(line, column);
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
                                    Tuple.Create(line, column, MutableTuple<CoeffType>.Create(value)));
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

        public IEnumerable<KeyValuePair<int, ISparseMatrixLine<CoeffType>>> GetLines()
        {
            throw new NotImplementedException();
        }

        public void Remove(int lineNumber)
        {
            throw new NotImplementedException();
        }

        public bool ContainsLine(int line)
        {
            throw new NotImplementedException();
        }

        public bool TryGetLine(int index, out ISparseMatrixLine<CoeffType> line)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KeyValuePair<int, CoeffType>> GetColumns(int line)
        {
            throw new NotImplementedException();
        }

        public int GetLength(int dimension)
        {
            throw new NotImplementedException();
        }

        public IMatrix<CoeffType> GetSubMatrix(int[] lines, int[] columns)
        {
            throw new NotImplementedException();
        }

        public IMatrix<CoeffType> GetSubMatrix(
            IntegerSequence lines,
            IntegerSequence columns)
        {
            throw new NotImplementedException();
        }

        public void SwapLines(int i, int j)
        {
            throw new NotImplementedException();
        }

        public void SwapColumns(int i, int j)
        {
            throw new NotImplementedException();
        }

        public void ScalarLineMultiplication(int line, CoeffType scalar, IRing<CoeffType> ring)
        {
            throw new NotImplementedException();
        }

        public void CombineLines(int i, int j, CoeffType a, CoeffType b, IRing<CoeffType> ring)
        {
            throw new NotImplementedException();
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
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindLowestPosition(int line, int column)
        {
            if (this.elements.Count == 0)
            {
                return 0;
            }
            else if (this.CompareLineAndColumn(
                line,
                column,
                this.elements[this.elements.Count - 1]) > 0)
            {
                return this.elements.Count;
            }
            else if (this.CompareLineAndColumn(line, column, this.elements[0]) <= 0)
            {
                return 0;
            }
            else
            {
                int low = 0;
                int high = this.elements.Count - 1;
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
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindLowestPosition(int line)
        {
            if (this.elements.Count == 0)
            {
                return 0;
            }
            else if (this.CompareLine(
                line,
                this.elements[this.elements.Count - 1]) > 0)
            {
                return this.elements.Count;
            }
            else if (this.CompareLine(line, this.elements[0]) <= 0)
            {
                return 0;
            }
            else
            {
                return this.AuxiliaryFindLowestPosition(line, 0, this.elements.Count - 1);
            }
        }

        /// <summary>
        /// Encontra a posição do primeiro elemento cujo valor é igual ao especificado. Caso não exista,
        /// é retornada a posição onde este poderá ser inserido. A função aplica-se apenas ao caso em que
        /// o elemento se encontra dentro dos limites dados.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <param name="low_">O limite inferior do intervalo.</param>
        /// <param name="high_">O limite superior do intervalo.</param>
        /// <returns>A posição.</returns>
        private int AuxiliaryFindLowestPosition(int line, int low_, int high_)
        {
            var innerLow = low_;
            var innerHigh = high_;
            while (innerLow < innerHigh)
            {
                int sum = innerHigh + innerLow;
                int intermediaryIndex = sum / 2;
                if (sum % 2 == 0)
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
                        return intermediaryIndex;
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
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindGreatestPosition(int line)
        {
            if (this.elements.Count == 0)
            {
                return 0;
            }
            else if (this.CompareLine(line, this.elements[this.elements.Count - 1]) >= 0)
            {
                return this.elements.Count;
            }
            else if (this.CompareLine(line, this.elements[0]) < 0)
            {
                return 0;
            }
            else
            {
                return this.AuxiliaryFindGreatestPosition(line, 0, this.elements.Count - 1);
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
        /// <returns>O par que contém as posições.</returns>
        private Tuple<int, int> FindBothPositions(int line)
        {
            var elementsCount = this.elements.Count;
            if (elementsCount == 0)
            {
                return Tuple.Create(0, 0);
            }
            else
            {
                var current = this.elements[elementsCount - 1];
                var lineComparision = this.CompareLine(line, current);
                if (lineComparision > 0)
                {
                    return Tuple.Create(elementsCount, elementsCount);
                }
                else if (lineComparision == 0)
                {
                    var high = elementsCount - 1;
                    var low = high;

                    // Efectua o ciclo para determinar o menor valor.
                    current = this.elements[0];
                    lineComparision = this.CompareLine(line, current);
                    if (lineComparision < 0)
                    {
                        low = this.AuxiliaryFindLowestPosition(line, 0, high);
                    }

                    return Tuple.Create(low, high);
                }
                else
                {
                    current = this.elements[0];
                    lineComparision = this.CompareLine(line, current);
                    if (lineComparision < 0)
                    {
                        return Tuple.Create(0, 0);
                    }
                    else if (lineComparision == 0)
                    {
                        var low = 0;
                        var high = this.AuxiliaryFindGreatestPosition(line, 0, elementsCount - 1);
                        return Tuple.Create(low, high);
                    }
                    else
                    {
                        var auxLow = 0;
                        var auxHigh = elementsCount - 1;
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
                                    return Tuple.Create(result, result);
                                }
                                else if (
                                    this.CompareLine(line, this.elements[intermediaryIndex]) == 0)
                                {
                                    // Um elemento foi encontrado.
                                    var low = this.AuxiliaryFindLowestPosition(line, auxLow, intermediaryIndex);
                                    var high = this.AuxiliaryFindGreatestPosition(line, intermediaryIndex, auxHigh);
                                    return Tuple.Create(low, high);
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

                        return Tuple.Create(auxLow, auxHigh);
                    }
                }
            }
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
