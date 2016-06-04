// -----------------------------------------------------------------------
// <copyright file="Matrices.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #region Matriz de Ordenação

    /// <summary>
    /// Implementa uma matriz com base em vectore do sistema.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArrayMatrix<ObjectType> : ILongMatrix<ObjectType>
    {
        /// <summary>
        /// O número de linhas das matrizes.
        /// </summary>
        protected long numberOfLines;

        /// <summary>
        /// O número de colunas das matrizes.
        /// </summary>
        protected long numberOfColumns;

        /// <summary>
        /// O contentor para os coeficientes.
        /// </summary>
        protected ObjectType[][] elements;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="elements">O contentor com os elementos.</param>
        /// <param name="numberOfLines">O número de linhas.</param>
        /// <param name="numberOfColumns">O número de colunas.</param>
        internal ArrayMatrix(ObjectType[][] elements, long numberOfLines, long numberOfColumns)
        {
            this.elements = elements;
            this.numberOfLines = numberOfLines;
            this.numberOfColumns = numberOfColumns;
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o número de linhas ou colunas for negativo.</exception>
        public ArrayMatrix(long line, long column)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.InitializeMatrix(line, column);
            }
        }

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ArrayMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o número de linhas ou colunas for negativo.</exception>
        public ArrayMatrix(long line, long column, ObjectType defaultValue)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else if (column < 0)
            {
                throw new ArgumentOutOfRangeException("column");
            }
            else
            {
                this.InitializeMatrix(line, column, defaultValue);
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão.
        /// </exception>
        public virtual ObjectType this[int line, int column]
        {
            get
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException(
                        "Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException(
                        "Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    return this.elements[line][column];
                }
            }
            set
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException(
                        "Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException(
                        "Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    this.elements[line][column] = value;
                }
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão.
        /// </exception>
        public virtual ObjectType this[long line, long column]
        {
            get
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException(
                        "Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException(
                        "Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    return this.elements[line][column];
                }
            }
            set
            {
                if (line < 0 || line >= this.numberOfLines)
                {
                    throw new IndexOutOfRangeException(
                        "Index line must be non-negative and lesser than the number of lines in matrix.");
                }
                else if (column < 0 || column >= this.numberOfColumns)
                {
                    throw new IndexOutOfRangeException(
                        "Index column must be non-negative and lesser than the number of columns in matrix.");
                }
                else
                {
                    this.elements[line][column] = value;
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
        /// <exception cref="ArgumentException">Se o valor da dimensão diferir de zero ou um.</exception>
        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return (int)this.numberOfLines;
            }
            else if (dimension == 1)
            {
                return (int)this.numberOfColumns;
            }
            else
            {
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
            }
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        /// <exception cref="ArgumentException">Se o valor da dimensão diferir de zero ou um.</exception>
        public long GetLongLength(int dimension)
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
                throw new ArgumentException("Parameter dimension can only take the values 0 or 1.");
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
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(long[] lines, long[] columns)
        {
            return new SubMatrixLong<ObjectType>(this, lines, columns);
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
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(LongIntegerSequence lines, LongIntegerSequence columns)
        {
            return new LongIntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public virtual void SwapLines(int i, int j)
        {
            if (i < 0 || i > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                var swapLine = this.elements[i];
                this.elements[i] = this.elements[j];
                this.elements[j] = swapLine;
            }
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public void SwapLines(long i, long j)
        {
            if (i < 0 || i > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfLines)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                var swapLine = this.elements[i];
                this.elements[i] = this.elements[j];
                this.elements[j] = swapLine;
            }
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public virtual void SwapColumns(int i, int j)
        {
            if (i < 0 || i > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                for (int k = 0; k < this.numberOfLines; ++k)
                {
                    var swapColumn = this.elements[k][i];
                    this.elements[k][i] = this.elements[k][j];
                    this.elements[k][j] = swapColumn;
                }
            }
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice da linha ou da coluna for negativo ou não for inferior ao tamanho da dimensão respectiva.
        /// </exception>
        public void SwapColumns(long i, long j)
        {
            if (i < 0 || i > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (j < 0 || j > this.numberOfColumns)
            {
                throw new IndexOutOfRangeException("Index must be non-negative and less than the number of lines.");
            }
            else if (i != j)
            {
                for (int k = 0; k < this.numberOfLines; ++k)
                {
                    var swapColumn = this.elements[k][i];
                    this.elements[k][i] = this.elements[k][j];
                    this.elements[k][j] = swapColumn;
                }
            }
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public virtual IEnumerator<ObjectType> GetEnumerator()
        {
            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = 0; j < this.numberOfColumns; ++j)
                {
                    yield return this.elements[i][j];
                }
            }
        }

        /// <summary>
        /// Constrói uma representação textual da matriz.
        /// </summary>
        /// <returns>A representação textual da matriz.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            if (0 < this.numberOfLines)
            {
                resultBuilder.Append("[");
                if (0 < this.numberOfColumns)
                {
                    resultBuilder.Append(this.elements[0][0]);
                    for (int i = 1; i < this.numberOfColumns; ++i)
                    {
                        resultBuilder.AppendFormat(", {0}", this.elements[0][i]);
                    }
                }
                resultBuilder.Append("]");

                for (int i = 1; i < this.numberOfLines; ++i)
                {
                    resultBuilder.Append(", [");
                    if (0 < this.numberOfColumns)
                    {
                        resultBuilder.Append(this.elements[i][0]);
                        for (int j = 1; j < this.numberOfColumns; ++j)
                        {
                            resultBuilder.AppendFormat(", {0}", this.elements[i][j]);
                        }
                    }

                    resultBuilder.Append("]");
                }

            }

            resultBuilder.Append("]");
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Inicializa a matriz.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        protected virtual void InitializeMatrix(long line, long column)
        {
            this.elements = new ObjectType[line][];
            for (long i = 0; i < line; ++i)
            {
                this.elements[i] = new ObjectType[column];
            }

            this.numberOfLines = line;
            this.numberOfColumns = column;
        }

        /// <summary>
        /// Inicializa a matriz.
        /// </summary>
        /// <param name="line">O número de linhas.</param>
        /// <param name="column">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        protected virtual void InitializeMatrix(long line, long column, ObjectType defaultValue)
        {
            this.elements = new ObjectType[line][];
            if (EqualityComparer<object>.Default.Equals(defaultValue, default(ObjectType)))
            {
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new ObjectType[column];
                }
            }
            else
            {
                for (int i = 0; i < line; ++i)
                {
                    this.elements[i] = new ObjectType[column];
                    for (int j = 0; j < column; ++j)
                    {
                        this.elements[i][j] = defaultValue;
                    }
                }
            }

            this.numberOfLines = line;
            this.numberOfColumns = column;
        }

        /// <summary>
        /// Obtém o enumerador não genércio para a matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Representa uma matriz quadrada.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem as entradas das matrizes.</typeparam>
    public class ArraySquareMatrix<CoeffType> : ArrayMatrix<CoeffType>, ISquareMatrix<CoeffType>
    {
        /// <summary>
        /// Cria instâncias de obejctos do tipo <see cref="ArraySquareMatrix{CoeffType}"/>
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        public ArraySquareMatrix(int dimension) : base(dimension, dimension) { }

        /// <summary>
        /// Cria instâncias de obejctos do tipo <see cref="ArraySquareMatrix{CoeffType}"/>
        /// </summary>
        /// <param name="dimension">A dimensão da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ArraySquareMatrix(int dimension, CoeffType defaultValue) : base(dimension, dimension, defaultValue) { }

        /// <summary>
        /// Cria instâncias de obejctos do tipo <see cref="ArraySquareMatrix{CoeffType}"/>
        /// </summary>
        /// <param name="elements">O contentor de coeficientes.</param>
        /// <param name="dimension">A dimensão da matriz.</param>
        internal ArraySquareMatrix(CoeffType[][] elements, int dimension)
            : base(elements, dimension, dimension) { }

        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        public bool IsSymmetric(IEqualityComparer<CoeffType> equalityComparer)
        {
            var innerEqualityComparer = equalityComparer;
            if (innerEqualityComparer == null)
            {
                innerEqualityComparer = EqualityComparer<CoeffType>.Default;
            }

            for (int i = 0; i < this.numberOfLines; ++i)
            {
                for (int j = i + 1; j < this.numberOfColumns; ++j)
                {
                    var currentEntry = this.elements[i][j];
                    var symmetricEntry = this.elements[j][i];
                    if (!innerEqualityComparer.Equals(currentEntry, symmetricEntry))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    #endregion Matriz de Ordenação

    #region Matriz Dicionário

    /// <summary>
    /// Implementa a matriz base assente sobre dicionários ordenados.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    /// <typeparam name="L">O tipo dos objectos que constituem as linhas da matriz.</typeparam>
    public abstract class ASparseDictionaryMatrix<ObjectType, L>
        : ILongSparseMatrix<ObjectType, L>
        where L : ILongSparseMatrixLine<ObjectType>
    {
        /// <summary>
        /// O objecto a ser retornado quando o índice não foi definido.
        /// </summary>
        protected ObjectType defaultValue;

        /// <summary>
        /// O valor que sucede o número da última linha introduzida.
        /// </summary>
        protected long afterLastLine;

        /// <summary>
        /// O valor que sucede o número da última coluna introduzida.
        /// </summary>
        protected long afterLastColumn;

        /// <summary>
        /// Mantém o comparador para a verficação do valor por defeito.
        /// </summary>
        protected IEqualityComparer<ObjectType> objectComparer;

        /// <summary>
        /// As linhas da matriz.
        /// </summary>
        protected SortedDictionary<long, L> matrixLines =
            new SortedDictionary<long, L>(Comparer<long>.Default);

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseDictionaryMatrix{ObjectType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ASparseDictionaryMatrix(long lines, long columns, ObjectType defaultValue)
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
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseDictionaryMatrix{ObjectType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador de objectos.</param>
        public ASparseDictionaryMatrix(
            long lines,
            long columns,
            ObjectType defaultValue,
            IEqualityComparer<ObjectType> comparer)
            : this(lines, columns, defaultValue)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.objectComparer = comparer;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseDictionaryMatrix{ObjectType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        public ASparseDictionaryMatrix(long lines, long columns)
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
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseDictionaryMatrix{ObjectType, L}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ASparseDictionaryMatrix(ObjectType defaultValue)
        {
            this.defaultValue = defaultValue;
            this.objectComparer = EqualityComparer<ObjectType>.Default;
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
                    var currentLine = default(L);
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
                    if (this.objectComparer.Equals(value, this.defaultValue))
                    {
                        var currentLine = default(L);
                        if (this.matrixLines.TryGetValue(line, out currentLine))
                        {
                            currentLine[column] = value;
                            if (currentLine.NumberOfColumns == 0)
                            {
                                currentLine.Dispose();
                                this.matrixLines.Remove(line);
                            }
                        }
                    }
                    else
                    {
                        var currentLine = default(L);
                        if (this.matrixLines.TryGetValue(line, out currentLine))
                        {
                            currentLine[column] = value;
                        }
                        else
                        {
                            var newLine = this.CreateLine();
                            newLine[column] = value;
                            this.matrixLines.Add(line, newLine);
                        }
                    }
                }
            }
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
        public ObjectType this[long line, long column]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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
        /// <exception cref="CollectionsException">Se a linha não existir.</exception>
        public L this[int line]
        {
            get
            {
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else
                {
                    var currentLine = default(L);
                    if (this.matrixLines.TryGetValue(line, out currentLine))
                    {
                        return currentLine;
                    }
                    else
                    {
                        throw new CollectionsException("Line doesn't exist.");
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
        /// <exception cref="CollectionsException">Se a linha não existir.</exception>
        public L this[long line]
        {
            get
            {
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new ArgumentOutOfRangeException("line");
                }
                else
                {
                    var currentLine = default(L);
                    if (this.matrixLines.TryGetValue(line, out currentLine))
                    {
                        return currentLine;
                    }
                    else
                    {
                        throw new CollectionsException("Line doesn't exist.");
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

        #region Funções públicas

        /// <summary>
        /// Obtém um enumerador para todas as linhas com valores diferentes do valor por defeito.
        /// </summary>
        /// <returns>O enumerador para as linhas.</returns>
        public IEnumerable<KeyValuePair<int, L>> GetLines()
        {
            foreach (var kvp in this.matrixLines)
            {
                yield return new KeyValuePair<int, L>((int)kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Obtém um enumerador para todas as linhas com valores diferentes do valor por defeito.
        /// </summary>
        /// <returns>O enumerador para as linhas.</returns>
        public IEnumerable<KeyValuePair<long, L>> LongGetLines()
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
                var line = default(L);
                if (this.matrixLines.TryGetValue(
                    lineNumber,
                    out line))
                {
                    line.Dispose();
                    this.matrixLines.Remove(lineNumber);
                }
            }
        }

        /// <summary>
        /// Remove a linha especificada pelo número.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        public void Remove(long lineNumber)
        {
            if (lineNumber >= 0)
            {
                var line = default(L);
                if (this.matrixLines.TryGetValue(
                    lineNumber,
                    out line))
                {
                    line.Dispose();
                    this.matrixLines.Remove(lineNumber);
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
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        public bool ContainsLine(long line)
        {
            return this.matrixLines.ContainsKey(line);
        }

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetLine(int index, out L line)
        {
            var setLine = default(L);
            if (index < 0 || index >= this.afterLastLine)
            {
                line = setLine;
                return false;
            }
            else
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

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetLine(long index, out L line)
        {
            var setLine = default(L);
            if (index < 0 || index >= this.afterLastLine)
            {
                line = setLine;
                return false;
            }
            else
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
                var lineElement = default(L);
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

        /// <summary>
        /// Obtém as colunas atribuídas à linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>As colunas atribuídas.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número da linha for negativo ou for superior ao número da última linha.
        /// </exception>
        public IEnumerable<KeyValuePair<long, ObjectType>> GetColumns(long line)
        {
            if (line < 0 || line >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("line");
            }
            else
            {
                var lineElement = default(L);
                if (this.matrixLines.TryGetValue(line, out lineElement))
                {
                    return lineElement.LongGetColumns();
                }
                else
                {
                    return Enumerable.Empty<KeyValuePair<long, ObjectType>>();
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
                return (int)this.afterLastLine;
            }
            else if (dimension == 1)
            {
                return (int)this.afterLastColumn;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Matrix dimension value must be one of 0 or 1.");
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
        public long GetLongLength(int dimension)
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
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(long[] lines, long[] columns)
        {
            return new SubMatrixLong<ObjectType>(this, lines, columns);
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
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<ObjectType> GetSubMatrix(LongIntegerSequence lines, LongIntegerSequence columns)
        {
            return new LongIntegerSequenceSubMatrix<ObjectType>(this, lines, columns);
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
                var firstLine = default(L);
                if (this.matrixLines.TryGetValue(i, out firstLine))
                {
                    var secondLine = default(L);
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
                            var maximumIndex = 0L;
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
                    var secondLine = default(L);
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
                            var maximumIndex = 0L;
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
        public void SwapLines(long i, long j)
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
                var firstLine = default(L);
                if (this.matrixLines.TryGetValue(i, out firstLine))
                {
                    var secondLine = default(L);
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
                            var maximumIndex = 0L;
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
                    var secondLine = default(L);
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
                            var maximumIndex = 0L;
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
                            lineDictionary[i] = this.defaultValue;
                            lineDictionary[j] = firstLineEntry;
                        }
                    }
                    else
                    {
                        var secondLineEntry = default(ObjectType);
                        if (lineDictionary.TryGetColumnValue(j, out secondLineEntry))
                        {
                            lineDictionary[j] = this.defaultValue;
                            lineDictionary[i] = secondLineEntry;
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
        public void SwapColumns(long i, long j)
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
                            lineDictionary[i] = this.defaultValue;
                            lineDictionary[j] = firstLineEntry;
                        }
                    }
                    else
                    {
                        var secondLineEntry = default(ObjectType);
                        if (lineDictionary.TryGetColumnValue(j, out secondLineEntry))
                        {
                            lineDictionary[j] = this.defaultValue;
                            lineDictionary[i] = secondLineEntry;
                        }
                    }
                }
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

        #endregion Funções públicas

        /// <summary>
        /// Obtém um enumerador não genérico para a matriz.
        /// </summary>
        /// <returns>O enumeador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Cria a linha da matriz.
        /// </summary>
        /// <returns>A linha da matriz.</returns>
        protected abstract L CreateLine();

        /// <summary>
        /// Representa uma linha da matriz esparsa baseada em dicionários.
        /// </summary>
        protected class SparseMatrixLine : ILongSparseMatrixLine<ObjectType>
        {
            /// <summary>
            /// A matriz da qual a linha faz parte.
            /// </summary>
            private ASparseDictionaryMatrix<ObjectType, L> owner;

            /// <summary>
            /// As entradas.
            /// </summary>
            private SortedDictionary<long, ObjectType> matrixEntries;

            /// <summary>
            /// Cria instâncias de objectos do tipo <see cref="SparseMatrixLine"/>.
            /// </summary>
            /// <param name="owner">A matriz à qual pertence a linha..</param>
            public SparseMatrixLine(
                ASparseDictionaryMatrix<ObjectType, L> owner)
            {
                this.owner = owner;
                this.matrixEntries = new SortedDictionary<long, ObjectType>(Comparer<long>.Default);
            }

            /// <summary>
            /// Cria instâncias de objectos do tipo <see cref="SparseMatrixLine"/>.
            /// </summary>
            /// <remarks>
            /// Nenhuma verificação é realizada no que concerne à integridade das entradas proporcionadas no argumento
            /// face ao número de colunas declarado na matriz original.
            /// </remarks>
            /// <param name="matrixEntries">As entradas da matriz.</param>
            /// <param name="owner">A matriz à qual a linha actual pertence.</param>
            public SparseMatrixLine(
                SortedDictionary<long, ObjectType> matrixEntries,
                ASparseDictionaryMatrix<ObjectType, L> owner)
            {
                this.owner = owner;
                this.matrixEntries = matrixEntries;
            }

            /// <summary>
            /// Obtém ou atribui o valor da coluna especificada pelo índice.
            /// </summary>
            /// <value>
            /// O valor.
            /// </value>
            /// <param name="index">O índice da linha.</param>
            /// <returns>O valor.</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// Se o índice for negativo ou não for inferior ao número de colunas na matriz.
            /// </exception>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public ObjectType this[int index]
            {
                get
                {
                    if (index < 0 || index >= this.owner.GetLength(1))
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    else
                    {
                        if (this.owner == null)
                        {
                            throw new CollectionsException("The current line was disposed.");
                        }
                        else
                        {
                            var value = default(ObjectType);
                            if (this.matrixEntries.TryGetValue(index, out value))
                            {
                                return value;
                            }
                            else
                            {
                                return this.owner.DefaultValue;
                            }
                        }
                    }
                }
                set
                {
                    if (index < 0 || index >= this.owner.GetLength(1))
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    else
                    {
                        if (this.owner.objectComparer.Equals(
                                value,
                                this.owner.defaultValue))
                        {
                            this.matrixEntries.Remove(index);
                        }
                        else
                        {
                            if (this.matrixEntries.ContainsKey(index))
                            {
                                this.matrixEntries[index] = value;
                            }
                            else
                            {
                                this.matrixEntries.Add(index, value);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém ou atribui o valor da coluna especificada pelo índice.
            /// </summary>
            /// <value>
            /// O valor.
            /// </value>
            /// <param name="index">O índice da linha.</param>
            /// <returns>O valor.</returns>
            /// <exception cref="ArgumentOutOfRangeException">
            /// Se o índice for negativo ou não for inferior ao número de colunas na matriz.
            /// </exception>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public ObjectType this[long index]
            {
                get
                {
                    if (index < 0 || index >= this.owner.GetLength(1))
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    else
                    {
                        if (this.owner == null)
                        {
                            throw new CollectionsException("The current line was disposed.");
                        }
                        else
                        {
                            var value = default(ObjectType);
                            if (this.matrixEntries.TryGetValue(index, out value))
                            {
                                return value;
                            }
                            else
                            {
                                return this.owner.DefaultValue;
                            }
                        }
                    }
                }
                set
                {
                    if (index < 0 || index >= this.owner.GetLength(1))
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    else
                    {
                        if (this.owner.objectComparer.Equals(
                                value,
                                this.owner.defaultValue))
                        {
                            this.matrixEntries.Remove(index);
                        }
                        else
                        {
                            if (this.matrixEntries.ContainsKey(index))
                            {
                                this.matrixEntries[index] = value;
                            }
                            else
                            {
                                this.matrixEntries.Add(index, value);
                            }
                        }
                    }
                }
            }

            #region Funções públicas

            /// <summary>
            /// Obtém o comprimento total da linha que iguala a dimensão da matriz.
            /// </summary>
            /// <value>
            /// O comrpimento total da linha que iguala a dimensão da matriz.
            /// </value>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public int Length
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else
                    {
                        return this.owner.GetLength(1);
                    }
                }
            }

            /// <summary>
            /// Obtém o número de entradas não nulas.
            /// </summary>
            /// <value>
            /// O número de entradas não nulas.
            /// </value>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public int NumberOfColumns
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else
                    {
                        return this.matrixEntries.Count;
                    }
                }
            }

            /// <summary>
            /// Obtém o número de entradas não nulas.
            /// </summary>
            /// <value>
            /// O número de entradas não nulas.
            /// </value>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public long LongNumberOfColumns
            {
                get {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else
                    {
                        return this.matrixEntries.Count;
                    }
                }
            }

            /// <summary>
            /// Obtém as entradas das matrizes.
            /// </summary>
            /// <remarks>
            /// As entradas da matriz estão ordenadas por número de coluna.
            /// </remarks>
            /// <value>
            /// As entradas das matrizes.
            /// </value>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public SortedDictionary<long, ObjectType> MatrixEntries
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else
                    {
                        return this.matrixEntries;
                    }
                }
            }

            /// <summary>
            /// Obtém as colunas.
            /// </summary>
            /// <returns></returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public IEnumerable<KeyValuePair<int, ObjectType>> GetColumns()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    foreach (var kvp in this.matrixEntries)
                    {
                        yield return new KeyValuePair<int, ObjectType>((int)kvp.Key, kvp.Value);
                    }
                }
            }

            /// <summary>
            /// Obtém as colunas.
            /// </summary>
            /// <returns></returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public IEnumerable<KeyValuePair<long, ObjectType>> LongGetColumns()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    return this.matrixEntries;
                }
            }

            /// <summary>
            /// Remove uma entrada da linha.
            /// </summary>
            /// <param name="columnIndex">O índice da coluna a remover.</param>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public void Remove(int columnIndex)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    this.matrixEntries.Remove(columnIndex);
                }
            }

            /// <summary>
            /// Remove uma entrada da linha.
            /// </summary>
            /// <param name="columnIndex">O índice da coluna a remover.</param>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public void Remove(long columnIndex)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    this.matrixEntries.Remove(columnIndex);
                }
            }


            /// <summary>
            /// Verifica se a linha esparsa contém a coluna especificada.
            /// </summary>
            /// <param name="column">A coluna.</param>
            /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public bool ContainsColumn(int column)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    return this.matrixEntries.ContainsKey(column);
                }
            }

            /// <summary>
            /// Verifica se a linha esparsa contém a coluna especificada.
            /// </summary>
            /// <param name="column">A coluna.</param>
            /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public bool ContainsColumn(long column)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    return this.matrixEntries.ContainsKey(column);
                }
            }

            /// <summary>
            /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
            /// </summary>
            /// <param name="column">O índice da coluna.</param>
            /// <param name="value">O valor na coluna.</param>
            /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public bool TryGetColumnValue(int column, out ObjectType value)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    value = default(ObjectType);
                    if (column < 0 || column >= this.owner.GetLength(1))
                    {
                        return false;
                    }
                    else
                    {
                        return this.matrixEntries.TryGetValue(column, out value);
                    }
                }
            }

            /// <summary>
            /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
            /// </summary>
            /// <param name="column">O índice da coluna.</param>
            /// <param name="value">O valor na coluna.</param>
            /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public bool TryGetColumnValue(long column, out ObjectType value)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    value = default(ObjectType);
                    if (column < 0 || column >= this.owner.GetLength(1))
                    {
                        return false;
                    }
                    else
                    {
                        return this.matrixEntries.TryGetValue(column, out value);
                    }
                }
            }

            /// <summary>
            /// Obtém um enumerador para todas as entradas da linha da matriz.
            /// </summary>
            /// <returns>O enumerador.</returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public IEnumerator<KeyValuePair<int, ObjectType>> GetEnumerator()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    var defaultValue = this.owner.defaultValue;
                    var currentIndex = 0;
                    foreach (var kvp in this.matrixEntries)
                    {
                        var column = kvp.Key;
                        for (; currentIndex < column; ++currentIndex)
                        {
                            yield return new KeyValuePair<int, ObjectType>(
                                currentIndex,
                                defaultValue);
                        }

                        yield return new KeyValuePair<int,ObjectType>((int)kvp.Key, kvp.Value);
                        ++currentIndex;
                    }

                    var columns = this.owner.afterLastColumn;
                    for (; currentIndex < columns; ++currentIndex)
                    {
                        yield return new KeyValuePair<int, ObjectType>(
                                   currentIndex,
                                   defaultValue);
                    }
                }
            }

            /// <summary>
            /// Obtém um enumerador para todas as entradas da linha da matriz.
            /// </summary>
            /// <returns>O enumerador.</returns>
            /// <exception cref="CollectionsException">Se a linha foi descartada.</exception>
            public IEnumerator<KeyValuePair<long, ObjectType>> LongGetEnumerator()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    var defaultValue = this.owner.defaultValue;
                    var currentIndex = 0;
                    foreach (var kvp in this.matrixEntries)
                    {
                        var column = kvp.Key;
                        for (; currentIndex < column; ++currentIndex)
                        {
                            yield return new KeyValuePair<long, ObjectType>(
                                currentIndex,
                                defaultValue);
                        }

                        yield return kvp;
                        ++currentIndex;
                    }

                    var columns = this.owner.afterLastColumn;
                    for (; currentIndex < columns; ++currentIndex)
                    {
                        yield return new KeyValuePair<long, ObjectType>(
                                   currentIndex,
                                   defaultValue);
                    }
                }
            }

            /// <summary>
            /// Descarta a linha.
            /// </summary>
            public void Dispose()
            {
                this.owner = null;
            }

            #endregion Funções públicas

            /// <summary>
            /// Obtém um enumerador não genérico para as entradas da linha da matriz.
            /// </summary>
            /// <returns>O enumerador não genérico.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }

    /// <summary>
    /// Implementa uma matriz esparsa com base em dicionários.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem os argumentos.</typeparam>
    public class SparseDictionaryMatrix<ObjectType>
        : ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
    {
        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
        public SparseDictionaryMatrix(long lines, long columns, ObjectType defaultValue)
            : base(lines, columns, defaultValue) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador que permite verificar a igualdade com o valor por defeito..</param>
        public SparseDictionaryMatrix(
            long lines,
            long columns,
            ObjectType defaultValue,
            IEqualityComparer<ObjectType> comparer)
            : base(lines, columns, defaultValue, comparer) { }

        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
        public SparseDictionaryMatrix(long lines, long columns)
            : base(lines, columns) { }

        /// <summary>
        /// Cria instâncias e objectos do tipo <see cref="SparseDictionaryMatrix{ObjectType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseDictionaryMatrix(ObjectType defaultValue)
            : base(defaultValue) { }

        /// <summary>
        /// Exapande a matriz um número específico de vezes.
        /// </summary>
        /// <param name="numberOfLines">O número de linhas a acrescentar.</param>
        /// <param name="numberOfColumns">O número de colunas a acrescentar.</param>
        /// <exception cref="ArgumentException">Se o número de linhas for negativo.</exception>
        public void ExpandMatrix(long numberOfLines, long numberOfColumns)
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
                            var entryLine = default(ILongSparseMatrixLine<ObjectType>);
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
        /// Cria a linha da matriz.
        /// </summary>
        /// <returns>A linha da matriz.</returns>
        protected override ILongSparseMatrixLine<ObjectType> CreateLine()
        {
            return new ASparseDictionaryMatrix<ObjectType, ILongSparseMatrixLine<ObjectType>>
                                .SparseMatrixLine(this);
        }
    }

    #endregion Matriz Dicionário

    #region Matriz Coordenadas

    /// <summary>
    /// Representação em termos de coordenadas de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    /// <typeparam name="L">O tipo dos objectos que constituem as linhas da matriz.</typeparam>
    public abstract class ASparseCoordinateMatrix<CoeffType, L> : ILongSparseMatrix<CoeffType, L>
        where L : ILongSparseMatrixLine<CoeffType>
    {
        /// <summary>
        /// Mantém o valor por defeito.
        /// </summary>
        protected CoeffType defaultValue;

        /// <summary>
        /// Mantém a lista dos elementos.
        /// </summary>
        protected List<MutableTuple<long, long, MutableTuple<CoeffType>>> elements;

        /// <summary>
        /// O comparador que permite averiguara igualdade com o coeficiente por defeito.
        /// </summary>
        protected IEqualityComparer<CoeffType> comparer;

        /// <summary>
        /// Mantém o número de linhas.
        /// </summary>
        protected long afterLastLine;

        /// <summary>
        /// Mantém o número de colunas.
        /// </summary>
        protected long afterLastColumn;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseCoordinateMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ASparseCoordinateMatrix(CoeffType defaultValue)
        {
            this.defaultValue = default(CoeffType);
            this.comparer = EqualityComparer<CoeffType>.Default;
            this.elements = new List<MutableTuple<long, long, MutableTuple<CoeffType>>>();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseCoordinateMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        public ASparseCoordinateMatrix(long lines, long columns)
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
                this.afterLastLine = lines;
                this.afterLastColumn = columns;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseCoordinateMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ASparseCoordinateMatrix(long lines, long columns, CoeffType defaultValue)
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
                this.afterLastLine = lines;
                this.afterLastColumn = columns;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ASparseCoordinateMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas da matriz.</param>
        /// <param name="columns">O número de colunas da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador.</param>
        public ASparseCoordinateMatrix(
            long lines,
            long columns,
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
        /// <exception cref="CollectionsException">Se a linha não existir.</exception>
        public L this[int line]
        {
            get
            {
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection.");
                }
                else
                {
                    var both = this.FindBothPositions(
                        line,
                        0,
                        this.elements.Count);
                    return this.CreateLine(
                        line,
                        both.Item1,
                        both.Item2);
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
        /// <exception cref="CollectionsException">Se a linha não existir.</exception>
        public L this[long line]
        {
            get
            {
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection.");
                }
                else
                {
                    var both = this.FindBothPositions(
                        line,
                        0,
                        this.elements.Count);
                    return this.CreateLine(
                        line,
                        both.Item1,
                        both.Item2);
                }
            }
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
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.afterLastColumn)
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
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.afterLastColumn)
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
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                        line,
                                        column,
                                        MutableTuple<CoeffType>.Create(value)));
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
                                        MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                            line,
                                            column,
                                            MutableTuple<CoeffType>.Create(value)));
                                }
                            }
                        }
                        else
                        {
                            if (!this.comparer.Equals(value, this.defaultValue))
                            {
                                this.elements.Add(
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                        line,
                                        column,
                                        MutableTuple<CoeffType>.Create(value)));
                            }
                        }
                    }
                }
            }
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
        public CoeffType this[long line, long column]
        {
            get
            {
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.afterLastColumn)
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
                if (line < 0 || line >= this.afterLastLine)
                {
                    throw new IndexOutOfRangeException("The parameter line is out of bounds.");
                }
                else if (column < 0 || column >= this.afterLastColumn)
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
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                        line,
                                        column,
                                        MutableTuple<CoeffType>.Create(value)));
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
                                        MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                            line,
                                            column,
                                            MutableTuple<CoeffType>.Create(value)));
                                }
                            }
                        }
                        else
                        {
                            if (!this.comparer.Equals(value, this.defaultValue))
                            {
                                this.elements.Add(
                                    MutableTuple.Create<long, long, MutableTuple<CoeffType>>(
                                        line,
                                        column,
                                        MutableTuple<CoeffType>.Create(value)));
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
        /// Obtém o número de linhas não nulas.
        /// </summary>
        public int NumberOfLines
        {
            get
            {
                var result = 0;
                var count = this.elements.Count;
                var i = 0;
                if (i < count)
                {
                    var previous = this.elements[i].Item1;
                    ++result;
                    ++i;
                    for (; i < count; ++i)
                    {
                        var current = this.elements[i].Item1;
                        if (current != previous)
                        {
                            previous = current;
                            ++result;
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Otbém o comparador que permite a igualdade entre elementos.
        /// </summary>
        /// <remarks>
        /// Este comparador é necessário para averiguar se um determinado
        /// elemento é um elemento por defeito.
        /// </remarks>
        public IEqualityComparer<CoeffType> DefaultElementComparer
        {
            get
            {
                return this.comparer;
            }
        }

        /// <summary>
        /// Obtém ou atribui a dimensão em linhas da matriz.
        /// </summary>
        internal long AfterLastLine
        {
            get
            {
                return this.afterLastLine;
            }
            set
            {
                this.afterLastLine = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o número de dimensões em colunas da matriz.
        /// </summary>
        internal long AfterLastColumn
        {
            get
            {
                return this.afterLastColumn;
            }
            set
            {
                this.afterLastColumn = value;
            }
        }

        /// <summary>
        /// Otbém a lista dos elementos da matriz.
        /// </summary>
        internal List<MutableTuple<long, long, MutableTuple<CoeffType>>> Elements
        {
            get
            {
                return this.elements;
            }
        }

        #region Funções públicas

        /// <summary>
        /// Obtém um enumerador para todas as linhas não nulas da matriz.
        /// </summary>
        /// <remarks>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as linhas em sequência crescente pela chave.
        /// </remarks>
        /// <returns>As linhas não nulas da matriz.</returns>
        public IEnumerable<KeyValuePair<int, L>> GetLines()
        {
            var elements = this.elements;
            var elementsCount = elements.Count;
            if (elementsCount > 0)
            {
                var current = elements[0];
                var currentLine = current.Item1;
                var lastLineColumn = this.FindGreatestPosition(
                    currentLine,
                    1,
                    elementsCount);
                var newLine = this.CreateLine(
                    currentLine,
                    0,
                    lastLineColumn);
                yield return new KeyValuePair<int, L>((int)currentLine, newLine);
                while (lastLineColumn < elementsCount)
                {
                    current = elements[lastLineColumn];
                    currentLine = current.Item1;
                    var start = lastLineColumn;
                    lastLineColumn = this.FindGreatestPosition(
                        currentLine,
                        lastLineColumn + 1,
                        elementsCount);
                    newLine = this.CreateLine(
                        currentLine,
                        start,
                        lastLineColumn);
                    yield return new KeyValuePair<int, L>((int)currentLine, newLine);
                }
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
        public IEnumerable<KeyValuePair<long, L>> LongGetLines()
        {
            var elements = this.elements;
            var elementsCount = elements.Count;
            if (elementsCount > 0)
            {
                var current = elements[0];
                var currentLine = current.Item1;
                var lastLineColumn = this.FindGreatestPosition(
                    currentLine,
                    1,
                    elementsCount);
                var newLine = this.CreateLine(
                    currentLine,
                    0,
                    lastLineColumn);
                yield return new KeyValuePair<long, L>(currentLine, newLine);
                while (lastLineColumn < elementsCount)
                {
                    current = elements[lastLineColumn];
                    currentLine = current.Item1;
                    var start = lastLineColumn;
                    lastLineColumn = this.FindGreatestPosition(
                        currentLine,
                        lastLineColumn + 1,
                        elementsCount);
                    newLine = this.CreateLine(
                        currentLine,
                        start,
                        lastLineColumn);
                    yield return new KeyValuePair<long, L>(currentLine, newLine);
                }
            }
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
                var count = both.Item2 - both.Item1;
                var current = both.Item1;
                for (int i = 0; i < count; ++i)
                {
                    this.elements.RemoveRange(both.Item1, count);
                }
            }
        }

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        public void Remove(long lineNumber)
        {
            var elementsCount = this.elements.Count;
            if (elementsCount > 0)
            {
                var both = this.FindBothPositions(lineNumber, 0, elementsCount);
                var count = both.Item2 - both.Item1;
                var current = both.Item1;
                for (int i = 0; i < count; ++i)
                {
                    this.elements.RemoveRange(both.Item1, count);
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
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        public bool ContainsLine(long line)
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
        public bool TryGetLine(int index, out L line)
        {
            if (index < 0 || index >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                var count = this.elements.Count;
                var both = this.FindBothPositions(
                    index,
                    0,
                    count);
                line = default(L);
                if (index < count)
                {
                    var current = this.elements[both.Item1];
                    if (current.Item1 == index)
                    {
                        line = this.CreateLine(
                            index,
                            both.Item1,
                            both.Item2);
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
        public bool TryGetLine(long index, out L line)
        {
            if (index < 0 || index >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                var count = this.elements.Count;
                var both = this.FindBothPositions(
                    index,
                    0,
                    count);
                line = default(L);
                if (index < count)
                {
                    var current = this.elements[both.Item1];
                    if (current.Item1 == index)
                    {
                        line = this.CreateLine(
                            index,
                            both.Item1,
                            both.Item2);
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
                    yield return new KeyValuePair<int, CoeffType>((int)current.Item2, current.Item3.Item1);
                }
            }
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
        public IEnumerable<KeyValuePair<long, CoeffType>> GetColumns(long line)
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
                    yield return new KeyValuePair<long, CoeffType>(current.Item2, current.Item3.Item1);
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
                return (int)this.afterLastLine;
            }
            else if (dimension == 1)
            {
                return (int)this.afterLastColumn;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
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
        public long GetLongLength(int dimension)
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
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(long[] lines, long[] columns)
        {
            return new SubMatrixLong<CoeffType>(this, lines, columns);
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
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(LongIntegerSequence lines, LongIntegerSequence columns)
        {
            return new LongIntegerSequenceSubMatrix<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        public void SwapLines(int i, int j)
        {
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
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
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        public void SwapLines(long i, long j)
        {
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
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
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
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
                    var lastLineIndexLimit = firstLineIndex + this.afterLastColumn;
                    var lastLineIndex = this.FindGreatestPosition(
                        firstLineNumber,
                        firstLineIndex,
                        Math.Min(length, (int)lastLineIndexLimit));

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
                            else if (secondColumnValue.Item2 == second)
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
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        public void SwapColumns(long i, long j)
        {
            if (i < 0 || i >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastLine)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
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
                    var lastLineIndexLimit = firstLineIndex + this.afterLastColumn;
                    var lastLineIndex = this.FindGreatestPosition(
                        firstLineNumber,
                        firstLineIndex,
                        Math.Min(length, (int)lastLineIndexLimit));

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
                            else if (secondColumnValue.Item2 == second)
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

        #endregion Funções públicas

        /// <summary>
        /// Obtém um enumerador não genérico para todos os valores não nulos da matriz.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Funções Auxiliares

        /// <summary>
        /// Cria uma linha da matriz.
        /// </summary>
        /// <param name="line">O número da linha.</param>
        /// <param name="startIndex">
        /// O índice onde se encontra o primeiro elemento da linha, inclusivé.
        /// </param>
        /// <param name="endIndex">
        /// O índice onde se encontra o último elemento da linha, excusivé.
        /// </param>
        /// <returns>A linha criada.</returns>
        protected abstract L CreateLine(
            long line,
            int startIndex,
            int endIndex);

        /// <summary>
        /// Encontra a posição onde o elemento especificado se encontra, retornando, em caso
        /// de empate, a posição do primeiro.
        /// </summary>
        /// <param name="line">O número da linha.</param>
        /// <param name="column">O número da coluna.</param>
        /// <param name="start">O início do intervalor de procura, inclusivé.</param>
        /// <param name="end">O final do intervalo de procura, exclusivé.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        protected int FindLowestPosition(
            long line,
            long column,
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
        protected int FindLowestPosition(long line, int start, int end)
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
        protected int AuxiliaryFindLowestPosition(
            long line,
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
        protected int FindGreatestPosition(long line, int start, int end)
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
        protected int AuxiliaryFindGreatestPosition(long line, int low, int high)
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
        protected Tuple<int, int> FindBothPositions(long line, int start, int end)
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
        protected int FindColumn(long column, int start, int end)
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
        protected int CompareLine(long line, Tuple<long, long, MutableTuple<CoeffType>> element)
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
        protected int CompareLineAndColumn(long line, long column, Tuple<long, long, MutableTuple<CoeffType>> element)
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

        #endregion Funções Auxiliares

        #region Classes auxiliares

        /// <summary>
        /// Define uma linha de uma matriz cujas entradas são baseadas nas coordenadas.
        /// </summary>
        protected class CoordinateMatrixLine
            : ILongSparseMatrixLine<CoeffType>
        {
            /// <summary>
            /// A matriz à qual pertence a linha.
            /// </summary>
            protected ASparseCoordinateMatrix<CoeffType, L> owner;

            /// <summary>
            /// O número da linha.
            /// </summary>
            protected int lineNumber;

            /// <summary>
            /// O índice de partida no contentor da matriz, inclusivé.
            /// </summary>
            protected int startIndex;

            /// <summary>
            /// O índice final no contentor da matriz, exclusivé.
            /// </summary>
            protected int endIndex;

            /// <summary>
            /// Instancia uma nova instância de objetos do tipo <see cref="CoordinateMatrixLine"/>.
            /// </summary>
            /// <param name="owner">A matriz à qual irá pertencer a linha criada.</param>
            /// <param name="lineNumber">O número da linha.</param>
            public CoordinateMatrixLine(
                ASparseCoordinateMatrix<CoeffType, L> owner,
                int lineNumber)
            {
                this.owner = owner;
                this.lineNumber = lineNumber;
                this.startIndex = 0;
                this.endIndex = owner.elements.Count;
            }

            /// <summary>
            /// Instancia uma nova instância de objetos do tipo <see cref="CoordinateMatrixLine"/>.
            /// </summary>
            /// <param name="owner">A matriz à qual irá pertencer a linha criada.</param>
            /// <param name="lineNumber">O número da linha.</param>
            /// <param name="startIndex">O índice da primeira coluna, inclusivé.</param>
            /// <param name="endIndex">O índice da segunda coluna, exclusivé.</param>
            public CoordinateMatrixLine(
                ASparseCoordinateMatrix<CoeffType, L> owner,
                int lineNumber,
                int startIndex,
                int endIndex)
            {
                this.owner = owner;
                this.lineNumber = lineNumber;
                this.startIndex = startIndex;
                this.endIndex = endIndex;
            }

            /// <summary>
            /// Obtém ou atribui o coeficiente contido na coluna especificada
            /// pelo índice.
            /// </summary>
            /// <param name="index">O índice.</param>
            /// <returns>O coeficiente.</returns>
            public CoeffType this[int index]
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else if (index < 0 || index > this.owner.afterLastColumn)
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    else
                    {
                        this.UpdateLimits();
                        var count = this.owner.elements.Count;
                        var columnIndex = this.owner.FindColumn(
                            index,
                            this.startIndex,
                            this.endIndex);
                        if (columnIndex < count)
                        {
                            var current = this.owner.elements[columnIndex];
                            if (current.Item2 == index)
                            {
                                return current.Item3.Item1;
                            }
                            else
                            {
                                return this.owner.defaultValue;
                            }
                        }
                        else
                        {
                            return this.owner.defaultValue;
                        }
                    }
                }
                set
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    this.UpdateLimits();
                }
            }

            /// <summary>
            /// Obtém ou atribui o coeficiente contido na coluna especificada
            /// pelo índice.
            /// </summary>
            /// <param name="index">O índice.</param>
            /// <returns>O coeficiente.</returns>
            public CoeffType this[long index]
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else if (index < 0 || index > this.owner.afterLastColumn)
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    else
                    {
                        this.UpdateLimits();
                        var count = this.owner.elements.Count;
                        var columnIndex = this.owner.FindColumn(
                            index,
                            this.startIndex,
                            this.endIndex);
                        if (columnIndex < count)
                        {
                            var current = this.owner.elements[columnIndex];
                            if (current.Item2 == index)
                            {
                                return current.Item3.Item1;
                            }
                            else
                            {
                                return this.owner.defaultValue;
                            }
                        }
                        else
                        {
                            return this.owner.defaultValue;
                        }
                    }
                }
                set
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    this.UpdateLimits();
                }
            }

            /// <summary>
            /// Obtém o número de colunas com valores diferentes
            /// do valor por defeito.
            /// </summary>
            public int NumberOfColumns
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was dispoed.");
                    }
                    else
                    {
                        this.UpdateLimits();
                        return this.endIndex - this.startIndex;
                    }
                }
            }

            /// <summary>
            /// Obtém o número de colunas com valores diferentes
            /// do valor por defeito.
            /// </summary>
            public long LongNumberOfColumns
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was dispoed.");
                    }
                    else
                    {
                        this.UpdateLimits();
                        return this.endIndex - this.startIndex;
                    }
                }
            }

            /// <summary>
            /// Obtém um enumerável para as colunas.
            /// </summary>
            /// <returns>O enumerável para as colunas.</returns>
            public IEnumerable<KeyValuePair<int, CoeffType>> GetColumns()
            {
                var innerElmenets = this.owner.elements;
                var afterLast = this.endIndex;
                for (int i = this.startIndex; i < afterLast; ++i)
                {
                    var currentElement = innerElmenets[i];
                    yield return new KeyValuePair<int, CoeffType>(
                        (int)currentElement.Item2,
                        currentElement.Item3.Item1);
                }
            }

            /// <summary>
            /// Obtém um enumerável para as colunas.
            /// </summary>
            /// <returns>O enumerável para as colunas.</returns>
            public IEnumerable<KeyValuePair<long, CoeffType>> LongGetColumns()
            {
                var innerElmenets = this.owner.elements;
                var afterLast = this.endIndex;
                for (int i = this.startIndex; i < afterLast; ++i)
                {
                    var currentElement = innerElmenets[i];
                    yield return new KeyValuePair<long, CoeffType>(
                        currentElement.Item2,
                        currentElement.Item3.Item1);
                }
            }

            /// <summary>
            /// Remove a coluna especificada pelo índice.
            /// </summary>
            /// <param name="columnIndex">O índice da coluna.</param>
            public void Remove(int columnIndex)
            {
                if (columnIndex >= 0 && columnIndex < this.owner.afterLastColumn)
                {
                    this.UpdateLimits();
                    var index = this.owner.FindColumn(
                        columnIndex,
                        this.startIndex,
                        this.endIndex);
                    var elements = this.owner.elements;
                    if (index < elements.Count)
                    {
                        if (elements[index].Item2 == columnIndex)
                        {
                            // Remove o elemento especificado pelo índcie.
                            elements.RemoveAt(index);
                        }
                    }
                }
            }

            /// <summary>
            /// Remove a coluna especificada pelo índice.
            /// </summary>
            /// <param name="columnIndex">O índice da coluna.</param>
            public void Remove(long columnIndex)
            {
                if (columnIndex >= 0 && columnIndex < this.owner.afterLastColumn)
                {
                    this.UpdateLimits();
                    var index = this.owner.FindColumn(
                        columnIndex,
                        this.startIndex,
                        this.endIndex);
                    var elements = this.owner.elements;
                    if (index < elements.Count)
                    {
                        if (elements[index].Item2 == columnIndex)
                        {
                            // Remove o elemento especificado pelo índcie.
                            elements.RemoveAt(index);
                        }
                    }
                }
            }

            /// <summary>
            /// Verifica se a coluna associada ao índice especificado existe.
            /// </summary>
            /// <param name="column">O índice da coluna.</param>
            /// <returns>Verdadeiro caso o índice exista e falso caso contrário.</returns>
            public bool ContainsColumn(int column)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (column < 0 || column >= this.owner.afterLastColumn)
                {
                    return false;
                }
                else
                {
                    this.UpdateLimits();
                    var index = this.owner.FindColumn(
                        column,
                        this.startIndex,
                        this.endIndex);
                    var elements = this.owner.elements;
                    if (index < elements.Count)
                    {
                        return elements[index].Item2 == column;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            /// <summary>
            /// Verifica se a coluna associada ao índice especificado existe.
            /// </summary>
            /// <param name="column">O índice da coluna.</param>
            /// <returns>Verdadeiro caso o índice exista e falso caso contrário.</returns>
            public bool ContainsColumn(long column)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (column < 0 || column >= this.owner.afterLastColumn)
                {
                    return false;
                }
                else
                {
                    this.UpdateLimits();
                    var index = this.owner.FindColumn(
                        column,
                        this.startIndex,
                        this.endIndex);
                    var elements = this.owner.elements;
                    if (index < elements.Count)
                    {
                        return elements[index].Item2 == column;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            /// <summary>
            /// Tenta obter o valor associado à coluna especificada pelo índice
            /// caso este exista.
            /// </summary>
            /// <param name="column">A coluna.</param>
            /// <param name="value">O valor, caso este exista.</param>
            /// <returns>Verdadeiro se o valor existir e falso caso contrário.</returns>
            public bool TryGetColumnValue(int column, out CoeffType value)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    value = this.owner.defaultValue;
                    if (column < 0 || column >= this.owner.afterLastColumn)
                    {
                        return false;
                    }
                    else
                    {
                        this.UpdateLimits();
                        var index = this.owner.FindColumn(
                            column,
                            this.startIndex,
                            this.endIndex);
                        var elements = this.owner.elements;
                        if (index < elements.Count)
                        {
                            var currentElement = elements[index];
                            if (currentElement.Item2 == column)
                            {
                                value = currentElement.Item3.Item1;
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
            }

            /// <summary>
            /// Tenta obter o valor associado à coluna especificada pelo índice
            /// caso este exista.
            /// </summary>
            /// <param name="column">A coluna.</param>
            /// <param name="value">O valor, caso este exista.</param>
            /// <returns>Verdadeiro se o valor existir e falso caso contrário.</returns>
            public bool TryGetColumnValue(long column, out CoeffType value)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    value = this.owner.defaultValue;
                    if (column < 0 || column >= this.owner.afterLastColumn)
                    {
                        return false;
                    }
                    else
                    {
                        this.UpdateLimits();
                        var index = this.owner.FindColumn(
                            column,
                            this.startIndex,
                            this.endIndex);
                        var elements = this.owner.elements;
                        if (index < elements.Count)
                        {
                            var currentElement = elements[index];
                            if (currentElement.Item2 == column)
                            {
                                value = currentElement.Item3.Item1;
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
            }

            /// <summary>
            /// Descarta a linha actual.
            /// </summary>
            public void Dispose()
            {
                this.owner = null;
            }

            /// <summary>
            /// Obtém um enumerador para a coluna.
            /// </summary>
            /// <returns>O enumerador.</returns>
            public IEnumerator<KeyValuePair<int, CoeffType>> GetEnumerator()
            {
                this.UpdateLimits();
                var limit = this.endIndex;
                var elements = this.owner.elements;
                var defaultValue = this.owner.defaultValue;
                var currentIndex = 0;
                for (int i = this.startIndex; i < limit; ++i)
                {
                    var currentElement = elements[i];
                    var column = currentElement.Item2;
                    for (; currentIndex < column; ++currentIndex)
                    {
                        yield return new KeyValuePair<int, CoeffType>(
                            currentIndex,
                            defaultValue);
                    }

                    yield return new KeyValuePair<int, CoeffType>(
                        (int)column,
                        currentElement.Item3.Item1);
                    ++currentIndex;
                }

                var columns = this.owner.afterLastColumn;
                for (; currentIndex < columns; ++currentIndex)
                {
                    yield return new KeyValuePair<int, CoeffType>(
                            currentIndex,
                            defaultValue);
                }
            }

            /// <summary>
            /// Obtém um enumerador para a coluna.
            /// </summary>
            /// <returns>O enumerador.</returns>
            public IEnumerator<KeyValuePair<long, CoeffType>> LongGetEnumerator()
            {
                this.UpdateLimits();
                var limit = this.endIndex;
                var elements = this.owner.elements;
                var defaultValue = this.owner.defaultValue;
                var currentIndex = 0L;
                for (int i = this.startIndex; i < limit; ++i)
                {
                    var currentElement = elements[i];
                    var column = currentElement.Item2;
                    for (; currentIndex < column; ++currentIndex)
                    {
                        yield return new KeyValuePair<long, CoeffType>(
                            currentIndex,
                            defaultValue);
                    }

                    yield return new KeyValuePair<long, CoeffType>(
                        column,
                        currentElement.Item3.Item1);
                    ++currentIndex;
                }

                var columns = this.owner.afterLastColumn;
                for (; currentIndex < columns; ++currentIndex)
                {
                    yield return new KeyValuePair<long, CoeffType>(
                            currentIndex,
                            defaultValue);
                }
            }

            /// <summary>
            /// Obtém um enumerador não genérico para a coluna.
            /// </summary>
            /// <returns>O enumerador.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            /// <summary>
            /// Actualiza os limites da linha caso a matriz tenha sido alterada.
            /// </summary>
            private void UpdateLimits()
            {
                var count = this.owner.elements.Count;
                if (this.startIndex < count)
                {
                    var ownerPointed = this.owner.elements[this.startIndex].Item1;
                    if (ownerPointed > this.lineNumber)
                    {
                        this.startIndex = this.owner.FindLowestPosition(
                            this.lineNumber,
                            0,
                            count);
                    }
                    else if (ownerPointed == this.lineNumber)
                    {
                        if (this.startIndex > 0)
                        {
                            var aux = this.startIndex - 1;
                            ownerPointed = this.owner.elements[aux].Item1;
                            if (ownerPointed == this.lineNumber)
                            {
                                this.startIndex = this.owner.FindLowestPosition(
                                    this.lineNumber,
                                    0,
                                    this.startIndex);
                            }
                        }
                    }
                    else
                    {
                        this.startIndex = this.owner.FindLowestPosition(
                            this.lineNumber,
                            this.startIndex,
                            count);
                    }

                    if (this.startIndex < count)
                    {
                        if (this.owner.elements[this.startIndex].Item1 != this.lineNumber)
                        {
                            // A linha não existe.
                            this.endIndex = this.startIndex;
                        }
                        else if (this.endIndex < count)
                        {
                            ownerPointed = this.owner.elements[this.endIndex].Item2;
                            if (ownerPointed > this.lineNumber)
                            {
                                if (this.endIndex > 0)
                                {
                                    var aux = this.endIndex - 1;
                                    if (this.owner.elements[aux].Item1 > this.lineNumber)
                                    {
                                        this.endIndex = this.owner.FindGreatestPosition(
                                            this.lineNumber,
                                            this.startIndex,
                                            this.endIndex);
                                    }
                                }
                            }
                            else
                            {
                                if (this.endIndex < this.startIndex)
                                {
                                    this.endIndex = this.owner.FindGreatestPosition(
                                        this.lineNumber,
                                        this.startIndex,
                                        count);
                                }
                                else
                                {
                                    this.endIndex = this.owner.FindGreatestPosition(
                                        this.lineNumber,
                                        this.endIndex,
                                        count);
                                }
                            }
                        }
                        else
                        {
                            this.endIndex = count;
                        }
                    }
                    else
                    {
                        this.endIndex = count;
                    }
                }
                else
                {
                    var positions = this.owner.FindBothPositions(
                        this.lineNumber,
                        0,
                        count);
                    this.startIndex = positions.Item1;
                    this.endIndex = positions.Item2;
                }
            }
        }

        #endregion Classes auxiliares
    }

    /// <summary>
    /// Representação em termos de coordenadas de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    public class SparseCoordinateMatrix<CoeffType>
        : ASparseCoordinateMatrix<CoeffType, ILongSparseMatrixLine<CoeffType>>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseCoordinateMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito a ser assumido pela matriz.</param>
        public SparseCoordinateMatrix(CoeffType defaultValue)
            : base(defaultValue) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseCoordinateMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        public SparseCoordinateMatrix(long lines, long columns)
            : base(lines, columns) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseCoordinateMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public SparseCoordinateMatrix(long lines, long columns, CoeffType defaultValue)
            : base(lines, columns, defaultValue) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SparseCoordinateMatrix{CoeffType}"/>.
        /// </summary>
        /// <param name="lines">O número de linha contidas na matriz.</param>
        /// <param name="columns">O número de colunas contidas na matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador que permite identificar os valores por defeito inseridos.</param>
        public SparseCoordinateMatrix(
            long lines,
            long columns,
            CoeffType defaultValue,
            IEqualityComparer<CoeffType> comparer)
            : base(lines, columns, defaultValue, comparer) { }

        /// <summary>
        /// Cria uma linha da matriz.
        /// </summary>
        /// <param name="line">O número da linha.</param>
        /// <param name="startIndex">
        /// O índice onde se encontra o primeiro elemento da linha, inclusivé.
        /// </param>
        /// <param name="endIndex">
        /// O índice onde se encontra o último elemento da linha, excusivé.
        /// </param>
        /// <returns>A linha criada.</returns>
        protected override ILongSparseMatrixLine<CoeffType> CreateLine(
            long line,
            int startIndex,
            int endIndex)
        {
            return new ASparseCoordinateMatrix<CoeffType, ILongSparseMatrixLine<CoeffType>>.CoordinateMatrixLine(
                this,
                (int)line,
                startIndex,
                endIndex);
        }
    }

    #endregion Matriz Coordenadas

    #region Matriz CRS

    /// <summary>
    /// Implementa uma matriz cuja compressão é realizada por linha.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    /// <typeparam name="L">O tipo dos objectos que constituem as linhas da matriz.</typeparam>
    public abstract class ACrsMatrix<CoeffType, L>
        : ILongSparseMatrix<CoeffType, L>
        where L : ILongSparseMatrixLine<CoeffType>
    {
        /// <summary>
        /// Mantém o valor por defeito.
        /// </summary>
        protected CoeffType defaultValue;

        /// <summary>
        /// Mantém a lista das entradas cujo valor é diferente do valor por defeito
        /// associadas aos respectivos índices de coluna.
        /// </summary>
        protected List<MutableTuple<long, MutableTuple<CoeffType>>> elements;

        /// <summary>
        /// Índices no vector de elementos onde se encontram os inícios das linhas.
        /// </summary>
        protected int[] lineIndices;

        /// <summary>
        /// O comparador que permite identificar os valores por defeito.
        /// </summary>
        protected IEqualityComparer<CoeffType> comparer;

        /// <summary>
        /// Mantém o número de colunas da matriz.
        /// </summary>
        protected long afterLastColumn;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ACrsMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ACrsMatrix(CoeffType defaultValue)
            : this(0, 0, default(CoeffType), EqualityComparer<CoeffType>.Default) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ACrsMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        public ACrsMatrix(int lines, int columns)
            : this(lines, columns, default(CoeffType), EqualityComparer<CoeffType>.Default) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ACrsMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        public ACrsMatrix(int lines, int columns, CoeffType defaultValue)
            : this(defaultValue) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ACrsMatrix{CoeffType, L}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="comparer">O comparador de igualdade de coeficientes.</param>
        public ACrsMatrix(
            int lines,
            int columns,
            CoeffType defaultValue,
            IEqualityComparer<CoeffType> comparer)
        {
            if (lines < 0)
            {
                throw new ArgumentOutOfRangeException("lines");
            }
            else if (columns < 0)
            {
                throw new ArgumentOutOfRangeException("columns");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.lineIndices = new int[lines];
                this.afterLastColumn = columns;
                this.defaultValue = defaultValue;
                this.elements = new List<MutableTuple<long, MutableTuple<CoeffType>>>();
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        public CoeffType this[int line, int column]
        {
            get
            {
                if (line < 0)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                }
                else if (column < 0 || line >= this.afterLastColumn)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: column.");
                }
                else
                {
                    var lastLine = this.lineIndices.Length - 1;
                    var count = this.elements.Count;
                    if (line == lastLine)
                    {
                        var linePointer = this.lineIndices[line];
                        if (linePointer < count)
                        {
                            var columnIndex = this.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (columnIndex == count)
                            {
                                return this.defaultValue;
                            }
                            else
                            {
                                var current = this.elements[columnIndex];
                                if (current.Item1 == column)
                                {
                                    return current.Item2.Item1;
                                }
                                else
                                {
                                    return this.defaultValue;
                                }
                            }
                        }
                        else
                        {
                            return this.defaultValue;
                        }
                    }
                    else if (line < lastLine)
                    {
                        var linePointer = this.lineIndices[line];
                        var nextLinePointer = this.lineIndices[line + 1];
                        if (linePointer == nextLinePointer)
                        {
                            return this.defaultValue;
                        }
                        else
                        {
                            var columnIndex = this.FindColumn(
                                column,
                                linePointer,
                                nextLinePointer);
                            var current = this.elements[columnIndex];
                            if (current.Item1 == column)
                            {
                                return current.Item2.Item1;
                            }
                            else
                            {
                                return this.defaultValue;
                            }
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException(
                           "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                    }
                }
            }
            set
            {
                if (line < 0 || line >= this.lineIndices.Length)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                }
                else if (column < 0 || line >= this.afterLastColumn)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: column.");
                }
                else
                {
                    var lastLine = this.lineIndices.Length - 1;
                    if (line == lastLine)
                    {
                        var count = this.elements.Count;
                        var linePointer = this.lineIndices[line];
                        if (linePointer < count)
                        {
                            var columnIndex = this.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (columnIndex == count)
                            {
                                if (!this.comparer.Equals(
                                   this.defaultValue,
                                   value))
                                {
                                    this.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                        column,
                                        MutableTuple.Create(value)));
                                }
                            }
                            else
                            {
                                var current = this.elements[columnIndex];
                                if (current.Item1 == columnIndex)
                                {
                                    if (this.comparer.Equals(
                                        this.defaultValue,
                                        value))
                                    {
                                        this.elements.RemoveAt(columnIndex);
                                    }
                                    else
                                    {
                                        current.Item2.Item1 = value;
                                    }
                                }
                                else
                                {
                                    if (!this.comparer.Equals(
                                        this.defaultValue,
                                        value))
                                    {
                                        this.elements.Insert(
                                            columnIndex,
                                            MutableTuple.Create<long, MutableTuple<CoeffType>>(column, MutableTuple.Create(value)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!this.comparer.Equals(
                                this.defaultValue,
                                value))
                            {
                                this.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                    column,
                                    MutableTuple.Create(value)));
                            }
                        }
                    }
                    else if (line < lastLine)
                    {
                        var linePointer = this.lineIndices[line];
                        var nextLinePointer = this.lineIndices[line + 1];
                        if (linePointer == nextLinePointer)
                        {
                            if (!this.comparer.Equals(
                                this.defaultValue,
                                value))
                            {
                                this.elements.Insert(
                                    linePointer,
                                    MutableTuple.Create<long, MutableTuple<CoeffType>>(column, MutableTuple.Create(value)));
                                for (int i = line + 1; i <= lastLine; ++i)
                                {
                                    ++this.lineIndices[i];
                                }
                            }
                        }
                        else
                        {
                            var columnIndex = this.FindColumn(
                                   column,
                                   linePointer,
                                   nextLinePointer);
                            var current = this.elements[columnIndex];
                            if (current.Item1 == column)
                            {
                                if (this.comparer.Equals(
                                    this.defaultValue,
                                    value))
                                {
                                    this.elements.RemoveAt(columnIndex);
                                    for (int i = line + 1; i <= lastLine; ++i)
                                    {
                                        --this.lineIndices[i];
                                    }
                                }
                                else
                                {
                                    current.Item2.Item1 = value;
                                }
                            }
                            else
                            {
                                if (!this.comparer.Equals(
                                    this.defaultValue,
                                    value))
                                {
                                    this.elements.Insert(
                                    linePointer,
                                    MutableTuple.Create<long, MutableTuple<CoeffType>>(column, MutableTuple.Create(value)));
                                    for (int i = line + 1; i <= lastLine; ++i)
                                    {
                                        ++this.lineIndices[i];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException(
                           "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                    }
                }
            }
        }

        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        public CoeffType this[long line, long column]
        {
            get
            {
                if (line < 0)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                }
                else if (column < 0 || line >= this.afterLastColumn)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: column.");
                }
                else
                {
                    var lastLine = this.lineIndices.Length - 1;
                    var count = this.elements.Count;
                    if (line == lastLine)
                    {
                        var linePointer = this.lineIndices[line];
                        if (linePointer < count)
                        {
                            var columnIndex = this.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (columnIndex == count)
                            {
                                return this.defaultValue;
                            }
                            else
                            {
                                var current = this.elements[columnIndex];
                                if (current.Item1 == column)
                                {
                                    return current.Item2.Item1;
                                }
                                else
                                {
                                    return this.defaultValue;
                                }
                            }
                        }
                        else
                        {
                            return this.defaultValue;
                        }
                    }
                    else if (line < lastLine)
                    {
                        var linePointer = this.lineIndices[line];
                        var nextLinePointer = this.lineIndices[line + 1];
                        if (linePointer == nextLinePointer)
                        {
                            return this.defaultValue;
                        }
                        else
                        {
                            var columnIndex = this.FindColumn(
                                column,
                                linePointer,
                                nextLinePointer);
                            var current = this.elements[columnIndex];
                            if (current.Item1 == column)
                            {
                                return current.Item2.Item1;
                            }
                            else
                            {
                                return this.defaultValue;
                            }
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException(
                           "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                    }
                }
            }
            set
            {
                if (line < 0 || line >= this.lineIndices.Length)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                }
                else if (column < 0 || line >= this.afterLastColumn)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: column.");
                }
                else
                {
                    var lastLine = this.lineIndices.Length - 1;
                    if (line == lastLine)
                    {
                        var count = this.elements.Count;
                        var linePointer = this.lineIndices[line];
                        if (linePointer < count)
                        {
                            var columnIndex = this.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (columnIndex == count)
                            {
                                if (!this.comparer.Equals(
                                   this.defaultValue,
                                   value))
                                {
                                    this.elements.Add(MutableTuple.Create(
                                        column,
                                        MutableTuple.Create(value)));
                                }
                            }
                            else
                            {
                                var current = this.elements[columnIndex];
                                if (current.Item1 == columnIndex)
                                {
                                    if (this.comparer.Equals(
                                        this.defaultValue,
                                        value))
                                    {
                                        this.elements.RemoveAt(columnIndex);
                                    }
                                    else
                                    {
                                        current.Item2.Item1 = value;
                                    }
                                }
                                else
                                {
                                    if (!this.comparer.Equals(
                                        this.defaultValue,
                                        value))
                                    {
                                        this.elements.Insert(
                                            columnIndex,
                                            MutableTuple.Create(column, MutableTuple.Create(value)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!this.comparer.Equals(
                                this.defaultValue,
                                value))
                            {
                                this.elements.Add(MutableTuple.Create(
                                    column,
                                    MutableTuple.Create(value)));
                            }
                        }
                    }
                    else if (line < lastLine)
                    {
                        var linePointer = this.lineIndices[line];
                        var nextLinePointer = this.lineIndices[line + 1];
                        if (linePointer == nextLinePointer)
                        {
                            if (!this.comparer.Equals(
                                this.defaultValue,
                                value))
                            {
                                this.elements.Insert(
                                    linePointer,
                                    MutableTuple.Create(column, MutableTuple.Create(value)));
                                for (var i = line + 1; i <= lastLine; ++i)
                                {
                                    ++this.lineIndices[i];
                                }
                            }
                        }
                        else
                        {
                            var columnIndex = this.FindColumn(
                                   column,
                                   linePointer,
                                   nextLinePointer);
                            var current = this.elements[columnIndex];
                            if (current.Item1 == column)
                            {
                                if (this.comparer.Equals(
                                    this.defaultValue,
                                    value))
                                {
                                    this.elements.RemoveAt(columnIndex);
                                    for (var i = line + 1; i <= lastLine; ++i)
                                    {
                                        --this.lineIndices[i];
                                    }
                                }
                                else
                                {
                                    current.Item2.Item1 = value;
                                }
                            }
                            else
                            {
                                if (!this.comparer.Equals(
                                    this.defaultValue,
                                    value))
                                {
                                    this.elements.Insert(
                                    linePointer,
                                    MutableTuple.Create(column, MutableTuple.Create(value)));
                                    for (var i = line + 1; i <= lastLine; ++i)
                                    {
                                        ++this.lineIndices[i];
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new IndexOutOfRangeException(
                           "Index was out of range. Must be non-negative and less than the size of the collection. Parameter: line.");
                    }
                }
            }
        }

        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        public L this[int line]
        {
            get
            {
                if (line < 0 || line >= this.lineIndices.Length)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter name: line");
                }
                else
                {
                    return this.CreateLine(line);
                }
            }
        }

        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        public L this[long line]
        {
            get
            {
                if (line < 0 || line >= this.lineIndices.Length)
                {
                    throw new IndexOutOfRangeException(
                        "Index was out of range. Must be non-negative and less than the size of the collection. Parameter name: line");
                }
                else
                {
                    return this.CreateLine(line);
                }
            }
        }

        /// <summary>
        /// Obtém o valor por defeito.
        /// </summary>
        public CoeffType DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
        }

        /// <summary>
        /// Obtém o número de linhas não nulas.
        /// </summary>
        public int NumberOfLines
        {
            get
            {
                var result = 0;
                var linesCount = this.lineIndices.Length;
                var i = 0;
                if (i < linesCount)
                {
                    var previousLine = this.lineIndices[i];
                    ++i;
                    for (; i < linesCount; ++i)
                    {
                        var currentLine = this.lineIndices[i];
                        if (currentLine > previousLine)
                        {
                            ++result;
                            previousLine = currentLine;
                        }
                    }

                    if (linesCount > previousLine)
                    {
                        ++result;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o número de linhas não nulas.
        /// </summary>
        public long LongNumberOfLines
        {
            get
            {
                var result = 0;
                var linesCount = this.lineIndices.Length;
                var i = 0;
                if (i < linesCount)
                {
                    var previousLine = this.lineIndices[i];
                    ++i;
                    for (; i < linesCount; ++i)
                    {
                        var currentLine = this.lineIndices[i];
                        if (currentLine > previousLine)
                        {
                            ++result;
                            previousLine = currentLine;
                        }
                    }

                    if (linesCount > previousLine)
                    {
                        ++result;
                    }
                }

                return result;
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
        public IEnumerable<KeyValuePair<int, L>> GetLines()
        {
            var linesCount = this.lineIndices.Length;
            var i = 0;
            if (i < linesCount)
            {
                var previousLine = this.lineIndices[i];
                ++i;
                for (; i < linesCount; ++i)
                {
                    var currentLine = this.lineIndices[i];
                    if (currentLine != previousLine)
                    {
                        yield return new KeyValuePair<int, L>(
                            previousLine,
                            this.CreateLine(previousLine));
                        previousLine = currentLine;
                    }
                }

                if (previousLine != linesCount)
                {
                    yield return new KeyValuePair<int, L>(
                            previousLine,
                            this.CreateLine(previousLine));
                }
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
        public IEnumerable<KeyValuePair<long, L>> LongGetLines()
        {
            var linesCount = this.lineIndices.Length;
            var i = 0;
            if (i < linesCount)
            {
                var previousLine = this.lineIndices[i];
                ++i;
                for (; i < linesCount; ++i)
                {
                    var currentLine = this.lineIndices[i];
                    if (currentLine != previousLine)
                    {
                        yield return new KeyValuePair<long, L>(
                            previousLine,
                            this.CreateLine(previousLine));
                        previousLine = currentLine;
                    }
                }

                if (previousLine != linesCount)
                {
                    yield return new KeyValuePair<long, L>(
                            previousLine,
                            this.CreateLine(previousLine));
                }
            }
        }

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        public void Remove(int lineNumber)
        {
            var lastLine = this.lineIndices.Length - 1;
            if (lineNumber == lastLine)
            {
                var linePointer = this.lineIndices[lineNumber];
                var count = this.elements.Count;
                if (linePointer != count)
                {
                    this.elements.RemoveRange(
                        linePointer,
                        count - linePointer);
                }
            }
            else if (lineNumber >= 0 && lineNumber < lastLine)
            {
                var linePointer = this.lineIndices[lineNumber];
                var nextLinePointer = this.lineIndices[lineNumber + 1];
                if (linePointer != nextLinePointer)
                {
                    var counted = linePointer - nextLinePointer;
                    this.elements.RemoveRange(
                        linePointer,
                        counted);
                    for (int i = lineNumber + 1; i <= lastLine; ++i)
                    {
                        this.lineIndices[i] -= counted;
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("lineNumber");
            }
        }

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        public void Remove(long lineNumber)
        {
            var lastLine = this.lineIndices.Length - 1;
            if (lineNumber == lastLine)
            {
                var linePointer = this.lineIndices[lineNumber];
                var count = this.elements.Count;
                if (linePointer != count)
                {
                    this.elements.RemoveRange(
                        linePointer,
                        count - linePointer);
                }
            }
            else if (lineNumber >= 0 && lineNumber < lastLine)
            {
                var linePointer = this.lineIndices[lineNumber];
                var nextLinePointer = this.lineIndices[lineNumber + 1];
                if (linePointer != nextLinePointer)
                {
                    var counted = linePointer - nextLinePointer;
                    this.elements.RemoveRange(
                        linePointer,
                        counted);
                    for (var i = lineNumber + 1; i <= lastLine; ++i)
                    {
                        this.lineIndices[i] -= counted;
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("lineNumber");
            }
        }

        /// <summary>
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        public bool ContainsLine(int line)
        {
            var lineIndicesCount = this.lineIndices.Length;
            if (line < 0 || line >= lineIndicesCount)
            {
                return false;
            }
            else
            {
                if (line == lineIndicesCount - 1)
                {
                    var linePointer = this.lineIndices[line];
                    if (linePointer == this.elements.Count)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    var linePointer = this.lineIndices[line];
                    var nextLinePointer = this.lineIndices[line + 1];
                    if (linePointer == nextLinePointer)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        public bool ContainsLine(long line)
        {
            var lineIndicesCount = this.lineIndices.Length;
            if (line < 0 || line >= lineIndicesCount)
            {
                return false;
            }
            else
            {
                if (line == lineIndicesCount - 1)
                {
                    var linePointer = this.lineIndices[line];
                    if (linePointer == this.elements.Count)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    var linePointer = this.lineIndices[line];
                    var nextLinePointer = this.lineIndices[line + 1];
                    if (linePointer == nextLinePointer)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetLine(int index, out L line)
        {
            var lineIndicesCount = this.lineIndices.Length;
            if (index < 0 || index >= lineIndicesCount)
            {
                line = default(L);
                return false;
            }
            else
            {
                if (index == lineIndicesCount - 1)
                {
                    var linePointer = this.lineIndices[index];
                    if (linePointer == this.elements.Count)
                    {
                        line = default(L);
                        return false;
                    }
                    else
                    {
                        line = this.CreateLine(index);
                        return true;
                    }
                }
                else
                {
                    var linePointer = this.lineIndices[index];
                    var nextLinePointer = this.lineIndices[index + 1];
                    if (linePointer == nextLinePointer)
                    {
                        line = default(L);
                        return false;
                    }
                    else
                    {
                        line = this.CreateLine(index);
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryGetLine(long index, out L line)
        {
            var lineIndicesCount = this.lineIndices.Length;
            if (index < 0 || index >= lineIndicesCount)
            {
                line = default(L);
                return false;
            }
            else
            {
                if (index == lineIndicesCount - 1)
                {
                    var linePointer = this.lineIndices[index];
                    if (linePointer == this.elements.Count)
                    {
                        line = default(L);
                        return false;
                    }
                    else
                    {
                        line = this.CreateLine(index);
                        return true;
                    }
                }
                else
                {
                    var linePointer = this.lineIndices[index];
                    var nextLinePointer = this.lineIndices[index + 1];
                    if (linePointer == nextLinePointer)
                    {
                        line = default(L);
                        return false;
                    }
                    else
                    {
                        line = this.CreateLine(index);
                        return true;
                    }
                }
            }
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
            var lastLine = this.lineIndices.Length - 1;
            if (line == lastLine)
            {
                var linePointer = this.lineIndices[line];
                var count = this.elements.Count;
                if (linePointer < count)
                {
                    for (int i = linePointer; i < count; ++i)
                    {
                        var current = this.elements[i];
                        yield return new KeyValuePair<int, CoeffType>(
                            (int)current.Item1,
                            current.Item2.Item1);
                    }
                }
            }
            else if (line >= 0 && line < lastLine)
            {
                var linePointer = this.lineIndices[line];
                var nextLinePointer = this.lineIndices[line + 1];
                if (linePointer < nextLinePointer)
                {
                    for (int i = linePointer; i < nextLinePointer; ++i)
                    {
                        var current = this.elements[i];
                        yield return new KeyValuePair<int, CoeffType>(
                            (int)current.Item1,
                            current.Item2.Item1);
                    }
                }
            }
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
        public IEnumerable<KeyValuePair<long, CoeffType>> GetColumns(long line)
        {
            var lastLine = this.lineIndices.Length - 1;
            if (line == lastLine)
            {
                var linePointer = this.lineIndices[line];
                var count = this.elements.Count;
                if (linePointer < count)
                {
                    for (int i = linePointer; i < count; ++i)
                    {
                        var current = this.elements[i];
                        yield return new KeyValuePair<long, CoeffType>(
                            current.Item1,
                            current.Item2.Item1);
                    }
                }
            }
            else if (line >= 0 && line < lastLine)
            {
                var linePointer = this.lineIndices[line];
                var nextLinePointer = this.lineIndices[line + 1];
                if (linePointer < nextLinePointer)
                {
                    for (int i = linePointer; i < nextLinePointer; ++i)
                    {
                        var current = this.elements[i];
                        yield return new KeyValuePair<long, CoeffType>(
                            current.Item1,
                            current.Item2.Item1);
                    }
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
        public int GetLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.lineIndices.Length;
            }
            else if (dimension == 1)
            {
                return (int)this.afterLastColumn;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
        }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        public long GetLongLength(int dimension)
        {
            if (dimension == 0)
            {
                return this.lineIndices.Length;
            }
            else if (dimension == 1)
            {
                return this.afterLastColumn;
            }
            else
            {
                throw new ArgumentOutOfRangeException("dimension");
            }
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        public void SwapLines(int i, int j)
        {
            var linesCount = this.lineIndices.Length;
            if (i < 0 || i >= linesCount)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= linesCount)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
            {
                var first = i;
                var second = j;
                if (j < i)
                {
                    first = j;
                    second = i;
                }

                var lastLine = this.lineIndices.Length - 1;
                if (second == lastLine)
                {
                    if (first == second - 1)
                    {
                        var count = this.elements.Count;
                        var firstPointer = this.lineIndices[first];
                        var secondPointer = this.lineIndices[second];
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; l < count; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < secondPointer)
                        {
                            this.lineIndices[second] = k;
                            var firstCurrent = this.elements[k];
                            firstCurrent.Item1 = second;
                            ++k;
                            for (; k < secondPointer; ++k)
                            {
                                firstCurrent = this.elements[k];
                            }
                        }
                        else
                        {
                            this.lineIndices[second] += (k - secondPointer);
                        }
                    }
                    else
                    {
                        var firstPointer = this.lineIndices[first];
                        var firstLimit = this.lineIndices[first + 1];
                        var secondPointer = this.lineIndices[second];
                        var count = this.elements.Count;
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; k < firstLimit && l < count; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < firstLimit)
                        {
                            var countRemove = firstLimit - k;
                            var range = this.elements.GetRange(k, countRemove);
                            this.elements.RemoveRange(k, countRemove);
                            this.elements.AddRange(range);
                            for (k = firstPointer + 1; k <= secondPointer; ++k)
                            {
                                this.lineIndices[k] -= countRemove;
                            }
                        }
                        else if (l < count)
                        {
                            var countRemove = count - l;
                            var range = this.elements.GetRange(l, count);
                            this.elements.RemoveRange(l, count);
                            this.elements.InsertRange(k, range);
                            for (l = firstPointer + 1; l <= secondPointer; ++l)
                            {
                                this.lineIndices[l] += countRemove;
                            }
                        }
                    }
                }
                else
                {
                    if (first == second - 1)
                    {
                        var firstPointer = this.lineIndices[first];
                        var secondPointer = this.lineIndices[second];
                        var thirdPointer = this.lineIndices[second + 1];
                        var firstCount = secondPointer - firstPointer;
                        var secondCount = thirdPointer - secondPointer;
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; l < thirdPointer; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < secondPointer)
                        {
                            this.lineIndices[second] = k;
                            var firstCurrent = this.elements[k];
                            firstCurrent.Item1 = second;
                            ++k;
                            for (; k < secondPointer; ++k)
                            {
                                firstCurrent = this.elements[k];
                            }
                        }
                        else
                        {
                            this.lineIndices[second] += (k - secondPointer);
                        }
                    }
                    else
                    {
                        var firstPointer = this.lineIndices[first];
                        var firstLimit = this.lineIndices[first + 1];
                        var secondPointer = this.lineIndices[second];
                        var secondLimit = this.lineIndices[second + 1];
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; k < firstLimit && l < secondLimit; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < firstLimit)
                        {
                            var countRemove = firstLimit - k;
                            var range = this.elements.GetRange(k, countRemove);
                            this.elements.RemoveRange(k, countRemove);
                            this.elements.InsertRange(l, range);
                            for (k = firstPointer + 1; k <= secondPointer; ++k)
                            {
                                this.lineIndices[k] -= countRemove;
                            }
                        }
                        else if (l < secondLimit)
                        {
                            var countRemove = secondLimit - l;
                            var range = this.elements.GetRange(l, secondLimit);
                            this.elements.RemoveRange(l, secondLimit);
                            this.elements.InsertRange(k, range);
                            for (l = firstPointer + 1; l <= secondPointer; ++l)
                            {
                                this.lineIndices[l] += countRemove;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        public void SwapLines(long i, long j)
        {
            var linesCount = this.lineIndices.Length;
            if (i < 0 || i >= linesCount)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= linesCount)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
            {
                var first = i;
                var second = j;
                if (j < i)
                {
                    first = j;
                    second = i;
                }

                var lastLine = this.lineIndices.Length - 1;
                if (second == lastLine)
                {
                    if (first == second - 1)
                    {
                        var count = this.elements.Count;
                        var firstPointer = this.lineIndices[first];
                        var secondPointer = this.lineIndices[second];
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; l < count; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < secondPointer)
                        {
                            this.lineIndices[second] = k;
                            var firstCurrent = this.elements[k];
                            firstCurrent.Item1 = second;
                            ++k;
                            for (; k < secondPointer; ++k)
                            {
                                firstCurrent = this.elements[k];
                            }
                        }
                        else
                        {
                            this.lineIndices[second] += (k - secondPointer);
                        }
                    }
                    else
                    {
                        var firstPointer = this.lineIndices[first];
                        var firstLimit = this.lineIndices[first + 1];
                        var secondPointer = this.lineIndices[second];
                        var count = this.elements.Count;
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; k < firstLimit && l < count; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < firstLimit)
                        {
                            var countRemove = firstLimit - k;
                            var range = this.elements.GetRange(k, countRemove);
                            this.elements.RemoveRange(k, countRemove);
                            this.elements.AddRange(range);
                            for (k = firstPointer + 1; k <= secondPointer; ++k)
                            {
                                this.lineIndices[k] -= countRemove;
                            }
                        }
                        else if (l < count)
                        {
                            var countRemove = count - l;
                            var range = this.elements.GetRange(l, count);
                            this.elements.RemoveRange(l, count);
                            this.elements.InsertRange(k, range);
                            for (l = firstPointer + 1; l <= secondPointer; ++l)
                            {
                                this.lineIndices[l] += countRemove;
                            }
                        }
                    }
                }
                else
                {
                    if (first == second - 1)
                    {
                        var firstPointer = this.lineIndices[first];
                        var secondPointer = this.lineIndices[second];
                        var thirdPointer = this.lineIndices[second + 1];
                        var firstCount = secondPointer - firstPointer;
                        var secondCount = thirdPointer - secondPointer;
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; l < thirdPointer; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < secondPointer)
                        {
                            this.lineIndices[second] = k;
                            var firstCurrent = this.elements[k];
                            firstCurrent.Item1 = second;
                            ++k;
                            for (; k < secondPointer; ++k)
                            {
                                firstCurrent = this.elements[k];
                            }
                        }
                        else
                        {
                            this.lineIndices[second] += (k - secondPointer);
                        }
                    }
                    else
                    {
                        var firstPointer = this.lineIndices[first];
                        var firstLimit = this.lineIndices[first + 1];
                        var secondPointer = this.lineIndices[second];
                        var secondLimit = this.lineIndices[second + 1];
                        var k = firstPointer;
                        var l = secondPointer;
                        for (; k < firstLimit && l < secondLimit; ++k, ++l)
                        {
                            var firstCurrent = this.elements[k];
                            var secondCurrent = this.elements[l];
                            var tempVal = firstCurrent.Item2.Item1;
                            firstCurrent.Item1 = second;
                            firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                            secondCurrent.Item1 = first;
                            secondCurrent.Item2.Item1 = tempVal;
                        }

                        if (k < firstLimit)
                        {
                            var countRemove = firstLimit - k;
                            var range = this.elements.GetRange(k, countRemove);
                            this.elements.RemoveRange(k, countRemove);
                            this.elements.InsertRange(l, range);
                            for (k = firstPointer + 1; k <= secondPointer; ++k)
                            {
                                this.lineIndices[k] -= countRemove;
                            }
                        }
                        else if (l < secondLimit)
                        {
                            var countRemove = secondLimit - l;
                            var range = this.elements.GetRange(l, secondLimit);
                            this.elements.RemoveRange(l, secondLimit);
                            this.elements.InsertRange(k, range);
                            for (l = firstPointer + 1; l <= secondPointer; ++l)
                            {
                                this.lineIndices[l] += countRemove;
                            }
                        }
                    }
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
            if (i < 0 || i >= this.afterLastColumn)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastColumn)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
            {
                var first = i;
                var second = j;
                if (j < i)
                {
                    first = j;
                    second = i;
                }

                var linesCount = this.lineIndices.Length;
                var k = 0;
                if (k < linesCount)
                {
                    var previousLine = this.lineIndices[k];
                    ++k;
                    for (; k < linesCount; ++k)
                    {
                        var currentLine = this.lineIndices[k];
                        var secondColumnIndex = this.FindColumn(
                            second,
                            previousLine,
                            currentLine);
                        if (secondColumnIndex < currentLine)
                        {
                            var firstColumnIndex = this.FindColumn(
                                second,
                                secondColumnIndex,
                                currentLine);
                            if (firstColumnIndex < secondColumnIndex)
                            {
                                // Se ambos forem iguais a columna não poderá existir.
                                var firstCurrent = this.elements[firstColumnIndex];
                                var secondCurrent = this.elements[secondColumnIndex];
                                if (firstCurrent.Item1 == first)
                                {
                                    if (secondCurrent.Item1 == second)
                                    {
                                        var tempVal = firstCurrent.Item2.Item1;
                                        firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                                        secondCurrent.Item2.Item1 = tempVal;
                                    }
                                    else
                                    {
                                        // Desloca os elementos.
                                        var lead = firstColumnIndex + 1;
                                        for (; firstColumnIndex < secondColumnIndex; ++firstColumnIndex, ++lead)
                                        {
                                            this.elements[firstColumnIndex] = this.elements[lead];
                                        }

                                        firstCurrent.Item1 = second;
                                        this.elements[lead] = firstCurrent;
                                    }
                                }
                                else if (secondCurrent.Item1 == second)
                                {
                                    var lead = secondColumnIndex - 1;
                                    for (; secondColumnIndex > firstColumnIndex; --secondColumnIndex, --lead)
                                    {
                                        this.elements[secondColumnIndex] = this.elements[lead];
                                    }

                                    secondCurrent.Item1 = first;
                                }
                            }
                        }

                        previousLine = currentLine;
                    }
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        public void SwapColumns(long i, long j)
        {
            if (i < 0 || i >= this.afterLastColumn)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (j < 0 || j >= this.afterLastColumn)
            {
                throw new ArgumentOutOfRangeException("j");
            }
            else if (i != j)
            {
                var first = i;
                var second = j;
                if (j < i)
                {
                    first = j;
                    second = i;
                }

                var linesCount = this.lineIndices.Length;
                var k = 0;
                if (k < linesCount)
                {
                    var previousLine = this.lineIndices[k];
                    ++k;
                    for (; k < linesCount; ++k)
                    {
                        var currentLine = this.lineIndices[k];
                        var secondColumnIndex = this.FindColumn(
                            second,
                            previousLine,
                            currentLine);
                        if (secondColumnIndex < currentLine)
                        {
                            var firstColumnIndex = this.FindColumn(
                                second,
                                secondColumnIndex,
                                currentLine);
                            if (firstColumnIndex < secondColumnIndex)
                            {
                                // Se ambos forem iguais a columna não poderá existir.
                                var firstCurrent = this.elements[firstColumnIndex];
                                var secondCurrent = this.elements[secondColumnIndex];
                                if (firstCurrent.Item1 == first)
                                {
                                    if (secondCurrent.Item1 == second)
                                    {
                                        var tempVal = firstCurrent.Item2.Item1;
                                        firstCurrent.Item2.Item1 = secondCurrent.Item2.Item1;
                                        secondCurrent.Item2.Item1 = tempVal;
                                    }
                                    else
                                    {
                                        // Desloca os elementos.
                                        var lead = firstColumnIndex + 1;
                                        for (; firstColumnIndex < secondColumnIndex; ++firstColumnIndex, ++lead)
                                        {
                                            this.elements[firstColumnIndex] = this.elements[lead];
                                        }

                                        firstCurrent.Item1 = second;
                                        this.elements[lead] = firstCurrent;
                                    }
                                }
                                else if (secondCurrent.Item1 == second)
                                {
                                    var lead = secondColumnIndex - 1;
                                    for (; secondColumnIndex > firstColumnIndex; --secondColumnIndex, --lead)
                                    {
                                        this.elements[secondColumnIndex] = this.elements[lead];
                                    }

                                    secondCurrent.Item1 = first;
                                }
                            }
                        }

                        previousLine = currentLine;
                    }
                }
            }

            throw new NotImplementedException();
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
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(long[] lines, long[] columns)
        {
            return new SubMatrixLong<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns)
        {
            return new IntegerSequenceSubMatrix<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        public IMatrix<CoeffType> GetSubMatrix(LongIntegerSequence lines, LongIntegerSequence columns)
        {
            return new LongIntegerSequenceSubMatrix<CoeffType>(this, lines, columns);
        }

        /// <summary>
        /// Obtém o enumerador genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador genérico.</returns>
        public IEnumerator<CoeffType> GetEnumerator()
        {
            var count = this.elements.Count;
            for (int i = 0; i < count; ++i)
            {
                yield return this.elements[i].Item2.Item1;
            }
        }

        /// <summary>
        /// Obtém o enumerador não genérico para a matriz.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Permite criar uma linha.
        /// </summary>
        /// <param name="line">O número da linha.</param>
        /// <returns>A linha.</returns>
        protected abstract L CreateLine(long line);

        /// <summary>
        /// Permite determinar a posição da coluna especificada.
        /// </summary>
        /// <param name="column">A coluna.</param>
        /// <param name="start">O índice a partir do qual se efectua a pesquisa.</param>
        /// <param name="end">O índice até ao qual é efectuada a pesquisa.</param>
        /// <returns>O índice onde se encontra a coluna.</returns>
        protected int FindColumn(long column, int start, int end)
        {
            int low = start;
            int high = end - 1;
            while (low < high)
            {
                int sum = high + low;
                int intermediaryIndex = sum / 2;
                if ((sum & 1) == 0)
                {
                    var intermediaryElement = this.elements[intermediaryIndex].Item1;
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
                    var intermediaryElement = this.elements[intermediaryIndex].Item1;
                    var nextIntermediaryElement = this.elements[intermediaryIndex + 1].Item1;
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

        #region Classes Auxiliares

        /// <summary>
        /// Implementa uma linha geral de uma matriz no formato CRS.
        /// </summary>
        protected class CrsMatrixLine : ILongSparseMatrixLine<CoeffType>
        {
            /// <summary>
            /// A matriz que contém a linha.
            /// </summary>
            protected ACrsMatrix<CoeffType, L> owner;

            /// <summary>
            /// O número da linha.
            /// </summary>
            protected long line;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="CrsMatrixLine"/>.
            /// </summary>
            /// <param name="line">O número da linha.</param>
            /// <param name="owner">A matriz que contém a linha.</param>
            public CrsMatrixLine(
                long line,
                ACrsMatrix<CoeffType, L> owner)
            {
                this.line = line;
                this.owner = owner;
            }

            /// <summary>
            /// Obtém e atribui o valor da entrada no índice especificado.
            /// </summary>
            /// <param name="index">O índice.</param>
            /// <returns>O objecto.</returns>
            public CoeffType this[int index]
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else if (index < 0 || index >= this.owner.afterLastColumn)
                    {
                        throw new IndexOutOfRangeException(
                            "Index was out of range. Must be non-negative and less than the size of the collection. Parameter name: index.");
                    }
                    else
                    {
                        var lastLine = this.owner.lineIndices.Length - 1;
                        var linePointers = this.owner.lineIndices;
                        if (this.line == lastLine)
                        {
                            var elementsCount = this.owner.elements.Count;
                            var linePointer = linePointers[this.line];
                            if (linePointer < elementsCount)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    elementsCount);
                                if (found == elementsCount)
                                {
                                    return this.owner.defaultValue;
                                }
                                else
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        return current.Item2.Item1;
                                    }
                                    else
                                    {
                                        return this.owner.defaultValue;
                                    }
                                }
                            }
                            else
                            {
                                return this.owner.defaultValue;
                            }
                        }
                        else
                        {
                            var linePointer = this.owner.lineIndices[this.line];
                            var nextLinePointer = this.owner.lineIndices[this.line + 1];
                            if (linePointer < nextLinePointer)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    nextLinePointer);
                                if (found < nextLinePointer)
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        return current.Item2.Item1;
                                    }
                                    else
                                    {
                                        return this.owner.defaultValue;
                                    }
                                }
                                else
                                {
                                    return this.owner.defaultValue;
                                }
                            }
                            else
                            {
                                return this.owner.defaultValue;
                            }
                        }
                    }
                }
                set
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else if (index < 0 || index >= this.owner.afterLastColumn)
                    {
                        throw new IndexOutOfRangeException(
                            "Index was out of range. Must be non-negative and less than the size of the collection. Parameter name: index.");
                    }
                    else
                    {
                        var defaultValue = this.owner.defaultValue;
                        var comparer = this.owner.comparer;
                        var elementsCount = this.owner.elements.Count;
                        var linesCount = this.owner.lineIndices.Length;
                        if (this.line == linesCount - 1)
                        {
                            var linePointer = this.owner.lineIndices[this.line];
                            if (linePointer < elementsCount)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    elementsCount);
                                if (found < elementsCount)
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        if (comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.RemoveAt(found);
                                        }
                                        else
                                        {
                                            current.Item2.Item1 = value;
                                        }
                                    }
                                    else
                                    {
                                        if (!comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                                index,
                                                MutableTuple.Create(value)));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!comparer.Equals(defaultValue, value))
                                    {
                                        this.owner.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                            index,
                                            MutableTuple.Create(value)));
                                    }
                                }
                            }
                            else
                            {
                                if (!comparer.Equals(defaultValue, value))
                                {
                                    this.owner.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                        index,
                                        MutableTuple.Create(value)));
                                }
                            }
                        }
                        else
                        {
                            var linePointer = this.owner.lineIndices[this.line];
                            var nextLinePointer = this.owner.lineIndices[this.line + 1];
                            if (linePointer < nextLinePointer)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    nextLinePointer);
                                if (found == nextLinePointer)
                                {
                                    if (!comparer.Equals(defaultValue, value))
                                    {
                                        this.owner.elements.Insert(
                                            found,
                                            MutableTuple.Create<long, MutableTuple<CoeffType>>(index, MutableTuple.Create(value)));
                                        for (var i = this.line + 1; i < linesCount; ++i)
                                        {
                                            ++this.owner.lineIndices[i];
                                        }
                                    }
                                }
                                else
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        if (comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.RemoveAt(found);
                                            for (var i = this.line + 1; i < linesCount; ++i)
                                            {
                                                --this.owner.lineIndices[i];
                                            }
                                        }
                                        else
                                        {
                                            current.Item2.Item1 = value;
                                        }
                                    }
                                    else
                                    {
                                        if (!comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.Insert(
                                                found,
                                                MutableTuple.Create<long, MutableTuple<CoeffType>>(index, MutableTuple.Create(value)));
                                            for (var i = this.line + 1; i < linesCount; ++i)
                                            {
                                                ++this.owner.lineIndices[i];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!comparer.Equals(defaultValue, value))
                                {
                                    this.owner.elements.Insert(
                                        linePointer,
                                        MutableTuple.Create<long, MutableTuple<CoeffType>>(index, MutableTuple.Create(value)));
                                    for (var i = this.line + 1; i < linesCount; ++i)
                                    {
                                        ++this.owner.lineIndices[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém e atribui o valor da entrada no índice especificado.
            /// </summary>
            /// <param name="index">O índice.</param>
            /// <returns>O objecto.</returns>
            public CoeffType this[long index]
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else if (index < 0 || index >= this.owner.afterLastColumn)
                    {
                        throw new IndexOutOfRangeException(
                            "Index was out of range. Must be non-negative and less than the size of the collection. Parameter name: index.");
                    }
                    else
                    {
                        var lastLine = this.owner.lineIndices.Length - 1;
                        var linePointers = this.owner.lineIndices;
                        if (this.line == lastLine)
                        {
                            var elementsCount = this.owner.elements.Count;
                            var linePointer = linePointers[this.line];
                            if (linePointer < elementsCount)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    elementsCount);
                                if (found == elementsCount)
                                {
                                    return this.owner.defaultValue;
                                }
                                else
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        return current.Item2.Item1;
                                    }
                                    else
                                    {
                                        return this.owner.defaultValue;
                                    }
                                }
                            }
                            else
                            {
                                return this.owner.defaultValue;
                            }
                        }
                        else
                        {
                            var linePointer = this.owner.lineIndices[this.line];
                            var nextLinePointer = this.owner.lineIndices[this.line + 1];
                            if (linePointer < nextLinePointer)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    nextLinePointer);
                                if (found < nextLinePointer)
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        return current.Item2.Item1;
                                    }
                                    else
                                    {
                                        return this.owner.defaultValue;
                                    }
                                }
                                else
                                {
                                    return this.owner.defaultValue;
                                }
                            }
                            else
                            {
                                return this.owner.defaultValue;
                            }
                        }
                    }
                }
                set
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else if (index < 0 || index >= this.owner.afterLastColumn)
                    {
                        throw new IndexOutOfRangeException(
                            "Index was out of range. Must be non-negative and less than the size of the collection. Parameter name: index.");
                    }
                    else
                    {
                        var defaultValue = this.owner.defaultValue;
                        var comparer = this.owner.comparer;
                        var elementsCount = this.owner.elements.Count;
                        var linesCount = this.owner.lineIndices.Length;
                        if (this.line == linesCount - 1)
                        {
                            var linePointer = this.owner.lineIndices[this.line];
                            if (linePointer < elementsCount)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    elementsCount);
                                if (found < elementsCount)
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        if (comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.RemoveAt(found);
                                        }
                                        else
                                        {
                                            current.Item2.Item1 = value;
                                        }
                                    }
                                    else
                                    {
                                        if (!comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                                index,
                                                MutableTuple.Create(value)));
                                        }
                                    }
                                }
                                else
                                {
                                    if (!comparer.Equals(defaultValue, value))
                                    {
                                        this.owner.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                            index,
                                            MutableTuple.Create(value)));
                                    }
                                }
                            }
                            else
                            {
                                if (!comparer.Equals(defaultValue, value))
                                {
                                    this.owner.elements.Add(MutableTuple.Create<long, MutableTuple<CoeffType>>(
                                        index,
                                        MutableTuple.Create(value)));
                                }
                            }
                        }
                        else
                        {
                            var linePointer = this.owner.lineIndices[this.line];
                            var nextLinePointer = this.owner.lineIndices[this.line + 1];
                            if (linePointer < nextLinePointer)
                            {
                                var found = this.owner.FindColumn(
                                    index,
                                    linePointer,
                                    nextLinePointer);
                                if (found == nextLinePointer)
                                {
                                    if (!comparer.Equals(defaultValue, value))
                                    {
                                        this.owner.elements.Insert(
                                            found,
                                            MutableTuple.Create<long, MutableTuple<CoeffType>>(index, MutableTuple.Create(value)));
                                        for (var i = this.line + 1; i < linesCount; ++i)
                                        {
                                            ++this.owner.lineIndices[i];
                                        }
                                    }
                                }
                                else
                                {
                                    var current = this.owner.elements[found];
                                    if (current.Item1 == index)
                                    {
                                        if (comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.RemoveAt(found);
                                            for (var i = this.line + 1; i < linesCount; ++i)
                                            {
                                                --this.owner.lineIndices[i];
                                            }
                                        }
                                        else
                                        {
                                            current.Item2.Item1 = value;
                                        }
                                    }
                                    else
                                    {
                                        if (!comparer.Equals(defaultValue, value))
                                        {
                                            this.owner.elements.Insert(
                                                found,
                                                MutableTuple.Create<long, MutableTuple<CoeffType>>(index, MutableTuple.Create(value)));
                                            for (var i = this.line + 1; i < linesCount; ++i)
                                            {
                                                ++this.owner.lineIndices[i];
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!comparer.Equals(defaultValue, value))
                                {
                                    this.owner.elements.Insert(
                                        linePointer,
                                        MutableTuple.Create<long, MutableTuple<CoeffType>>(index, MutableTuple.Create(value)));
                                    for (var i = this.line + 1; i < linesCount; ++i)
                                    {
                                        ++this.owner.lineIndices[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém o número de colunas não nulas.
            /// </summary>
            public int NumberOfColumns
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else
                    {
                        var linesCount = this.owner.lineIndices.Length;
                        if (this.line == linesCount - 1)
                        {
                            return this.owner.elements.Count - this.owner.lineIndices[this.line];
                        }
                        else
                        {
                            return this.owner.lineIndices[this.line] - this.owner.lineIndices[this.line + 1];
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém o número de colunas não nulas.
            /// </summary>
            public long LongNumberOfColumns
            {
                get
                {
                    if (this.owner == null)
                    {
                        throw new CollectionsException("The current line was disposed.");
                    }
                    else
                    {
                        var linesCount = this.owner.lineIndices.Length;
                        if (this.line == linesCount - 1)
                        {
                            return this.owner.elements.Count - this.owner.lineIndices[this.line];
                        }
                        else
                        {
                            return this.owner.lineIndices[this.line] - this.owner.lineIndices[this.line + 1];
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém um enumerador para todas as colunas não nulas.
            /// </summary>
            /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
            /// retornar as colunas em sequência crescente pela chave.
            /// <returns>O enumerador.</returns>
            public IEnumerable<KeyValuePair<int, CoeffType>> GetColumns()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var elements = this.owner.elements;
                        var elementsCount = elements.Count;
                        for (; linePointer < elementsCount; ++linePointer)
                        {
                            var current = elements[linePointer];
                            yield return new KeyValuePair<int, CoeffType>(
                                (int)current.Item1,
                                current.Item2.Item1);
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        var elements = this.owner.elements;
                        for (; linePointer < nextLinePointer; ++linePointer)
                        {
                            var current = elements[linePointer];
                            yield return new KeyValuePair<int, CoeffType>(
                                (int)current.Item1,
                                current.Item2.Item1);
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém um enumerador para todas as colunas não nulas.
            /// </summary>
            /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
            /// retornar as colunas em sequência crescente pela chave.
            /// <returns>O enumerador.</returns>
            public IEnumerable<KeyValuePair<long, CoeffType>> LongGetColumns()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var elements = this.owner.elements;
                        var elementsCount = elements.Count;
                        for (; linePointer < elementsCount; ++linePointer)
                        {
                            var current = elements[linePointer];
                            yield return new KeyValuePair<long, CoeffType>(
                                current.Item1,
                                current.Item2.Item1);
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        var elements = this.owner.elements;
                        for (; linePointer < nextLinePointer; ++linePointer)
                        {
                            var current = elements[linePointer];
                            yield return new KeyValuePair<long, CoeffType>(
                                current.Item1,
                                current.Item2.Item1);
                        }
                    }
                }
            }

            /// <summary>
            /// Remove a entrada espeficada pelo índice.
            /// </summary>
            /// <param name="columnIndex">O índice da entrada a ser removido.</param>
            public void Remove(int columnIndex)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (columnIndex >= 0 || columnIndex < this.owner.afterLastColumn)
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var elementsCount = this.owner.elements.Count;
                        if (linePointer < elementsCount)
                        {
                            var found = this.owner.FindColumn(
                                columnIndex,
                                linePointer,
                                elementsCount);
                            if (found < elementsCount)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == columnIndex)
                                {
                                    this.owner.elements.RemoveAt(found);
                                }
                            }
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var found = this.owner.FindColumn(
                                columnIndex,
                                linePointer,
                                nextLinePointer);
                            if (found < nextLinePointer)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == columnIndex)
                                {
                                    this.owner.elements.RemoveAt(found);
                                    var lineIndices = this.owner.lineIndices;
                                    var count = lineIndices.Length;
                                    for (int i = nextLinePointer; i < count; ++i)
                                    {
                                        --lineIndices[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Remove a entrada espeficada pelo índice.
            /// </summary>
            /// <param name="columnIndex">O índice da entrada a ser removido.</param>
            public void Remove(long columnIndex)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (columnIndex >= 0 || columnIndex < this.owner.afterLastColumn)
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var elementsCount = this.owner.elements.Count;
                        if (linePointer < elementsCount)
                        {
                            var found = this.owner.FindColumn(
                                columnIndex,
                                linePointer,
                                elementsCount);
                            if (found < elementsCount)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == columnIndex)
                                {
                                    this.owner.elements.RemoveAt(found);
                                }
                            }
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var found = this.owner.FindColumn(
                                columnIndex,
                                linePointer,
                                nextLinePointer);
                            if (found < nextLinePointer)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == columnIndex)
                                {
                                    this.owner.elements.RemoveAt(found);
                                    var lineIndices = this.owner.lineIndices;
                                    var count = lineIndices.Length;
                                    for (int i = nextLinePointer; i < count; ++i)
                                    {
                                        --lineIndices[i];
                                    }
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Verifica se a linha esparsa contém a coluna especificada.
            /// </summary>
            /// <param name="column">A coluna.</param>
            /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
            public bool ContainsColumn(int column)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (column < 0 || column >= this.owner.afterLastColumn)
                {
                    return false;
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var count = this.owner.elements.Count;
                        if (linePointer < count)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (found < count)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
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
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                nextLinePointer);
                            if (found < nextLinePointer)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
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
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            /// <summary>
            /// Verifica se a linha esparsa contém a coluna especificada.
            /// </summary>
            /// <param name="column">A coluna.</param>
            /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
            public bool ContainsColumn(long column)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (column < 0 || column >= this.owner.afterLastColumn)
                {
                    return false;
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var count = this.owner.elements.Count;
                        if (linePointer < count)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (found < count)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
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
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                nextLinePointer);
                            if (found < nextLinePointer)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
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
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            /// <summary>
            /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
            /// </summary>
            /// <param name="column">O índice da coluna.</param>
            /// <param name="value">O valor na coluna.</param>
            /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
            public bool TryGetColumnValue(int column, out CoeffType value)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (column < 0 || column >= this.owner.afterLastColumn)
                {
                    value = this.owner.defaultValue;
                    return false;
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var count = this.owner.elements.Count;
                        if (linePointer < count)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (found < count)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
                                {
                                    value = current.Item2.Item1;
                                    return true;
                                }
                                else
                                {
                                    value = this.owner.defaultValue;
                                    return false;
                                }
                            }
                            else
                            {
                                value = this.owner.defaultValue;
                                return false;
                            }
                        }
                        else
                        {
                            value = this.owner.defaultValue;
                            return false;
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                nextLinePointer);
                            if (found < nextLinePointer)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
                                {
                                    value = current.Item2.Item1;
                                    return true;
                                }
                                else
                                {
                                    value = this.owner.defaultValue;
                                    return false;
                                }
                            }
                            else
                            {
                                value = this.owner.defaultValue;
                                return false;
                            }
                        }
                        else
                        {
                            value = this.owner.defaultValue;
                            return false;
                        }
                    }
                }
            }

            /// <summary>
            /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
            /// </summary>
            /// <param name="column">O índice da coluna.</param>
            /// <param name="value">O valor na coluna.</param>
            /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
            public bool TryGetColumnValue(long column, out CoeffType value)
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else if (column < 0 || column >= this.owner.afterLastColumn)
                {
                    value = this.owner.defaultValue;
                    return false;
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    if (this.line == lastLine)
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var count = this.owner.elements.Count;
                        if (linePointer < count)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                count);
                            if (found < count)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
                                {
                                    value = current.Item2.Item1;
                                    return true;
                                }
                                else
                                {
                                    value = this.owner.defaultValue;
                                    return false;
                                }
                            }
                            else
                            {
                                value = this.owner.defaultValue;
                                return false;
                            }
                        }
                        else
                        {
                            value = this.owner.defaultValue;
                            return false;
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var found = this.owner.FindColumn(
                                column,
                                linePointer,
                                nextLinePointer);
                            if (found < nextLinePointer)
                            {
                                var current = this.owner.elements[found];
                                if (current.Item1 == column)
                                {
                                    value = current.Item2.Item1;
                                    return true;
                                }
                                else
                                {
                                    value = this.owner.defaultValue;
                                    return false;
                                }
                            }
                            else
                            {
                                value = this.owner.defaultValue;
                                return false;
                            }
                        }
                        else
                        {
                            value = this.owner.defaultValue;
                            return false;
                        }
                    }
                }
            }

            /// <summary>
            /// Descarta a linha.
            /// </summary>
            public void Dispose()
            {
                this.owner = null;
            }

            /// <summary>
            /// Obtém um enumerador genérico para os elementos da linha,
            /// incluindo os valores por defeito.
            /// </summary>
            /// <returns>O enumerador para a linha.</returns>
            public IEnumerator<KeyValuePair<int, CoeffType>> GetEnumerator()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    var elements = this.owner.elements;
                    var defaultValue = this.owner.defaultValue;
                    if (this.line == lastLine)
                    {
                        var count = elements.Count;
                        var linePointer = this.owner.lineIndices[this.line];
                        if (linePointer < count)
                        {
                            var ind = 0;
                            for (int i = linePointer; i < count; ++i)
                            {
                                var current = elements[i];
                                var column = current.Item1;
                                for (; ind < column; ++ind)
                                {
                                    yield return new KeyValuePair<int, CoeffType>(
                                    ind,
                                    defaultValue);
                                }

                                yield return new KeyValuePair<int, CoeffType>(
                                    ind,
                                    current.Item2.Item1);
                                ++ind;
                            }

                            for (; ind < count; ++ind)
                            {
                                yield return new KeyValuePair<int, CoeffType>(
                                ind,
                                defaultValue);
                            }
                        }
                        else
                        {
                            var columns = this.owner.afterLastColumn;
                            for (int i = 0; i < columns; ++i)
                            {
                                yield return new KeyValuePair<int, CoeffType>(
                                    i,
                                    defaultValue);
                            }
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var ind = 0;
                            for (int i = linePointer; i < nextLinePointer; ++i)
                            {
                                var current = elements[i];
                                var column = current.Item1;
                                for (; ind < column; ++ind)
                                {
                                    yield return new KeyValuePair<int, CoeffType>(
                                    ind,
                                    defaultValue);
                                }

                                yield return new KeyValuePair<int, CoeffType>(
                                    ind,
                                    current.Item2.Item1);
                                ++ind;
                            }

                            for (; ind < nextLinePointer; ++ind)
                            {
                                yield return new KeyValuePair<int, CoeffType>(
                                ind,
                                defaultValue);
                            }
                        }
                        else
                        {
                            var columns = this.owner.afterLastColumn;
                            for (int i = 0; i < columns; ++i)
                            {
                                yield return new KeyValuePair<int, CoeffType>(
                                    i,
                                    defaultValue);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém um enumerador genérico para os elementos da linha,
            /// incluindo os valores por defeito.
            /// </summary>
            /// <returns>O enumerador para a linha.</returns>
            public IEnumerator<KeyValuePair<long, CoeffType>> LongGetEnumerator()
            {
                if (this.owner == null)
                {
                    throw new CollectionsException("The current line was disposed.");
                }
                else
                {
                    var lastLine = this.owner.lineIndices.Length - 1;
                    var elements = this.owner.elements;
                    var defaultValue = this.owner.defaultValue;
                    if (this.line == lastLine)
                    {
                        var count = elements.Count;
                        var linePointer = this.owner.lineIndices[this.line];
                        if (linePointer < count)
                        {
                            var ind = 0;
                            for (int i = linePointer; i < count; ++i)
                            {
                                var current = elements[i];
                                var column = current.Item1;
                                for (; ind < column; ++ind)
                                {
                                    yield return new KeyValuePair<long, CoeffType>(
                                    ind,
                                    defaultValue);
                                }

                                yield return new KeyValuePair<long, CoeffType>(
                                    ind,
                                    current.Item2.Item1);
                                ++ind;
                            }

                            for (; ind < count; ++ind)
                            {
                                yield return new KeyValuePair<long, CoeffType>(
                                ind,
                                defaultValue);
                            }
                        }
                        else
                        {
                            var columns = this.owner.afterLastColumn;
                            for (int i = 0; i < columns; ++i)
                            {
                                yield return new KeyValuePair<long, CoeffType>(
                                    i,
                                    defaultValue);
                            }
                        }
                    }
                    else
                    {
                        var linePointer = this.owner.lineIndices[this.line];
                        var nextLinePointer = this.owner.lineIndices[this.line + 1];
                        if (linePointer < nextLinePointer)
                        {
                            var ind = 0;
                            for (int i = linePointer; i < nextLinePointer; ++i)
                            {
                                var current = elements[i];
                                var column = current.Item1;
                                for (; ind < column; ++ind)
                                {
                                    yield return new KeyValuePair<long, CoeffType>(
                                    ind,
                                    defaultValue);
                                }

                                yield return new KeyValuePair<long, CoeffType>(
                                    ind,
                                    current.Item2.Item1);
                                ++ind;
                            }

                            for (; ind < nextLinePointer; ++ind)
                            {
                                yield return new KeyValuePair<long, CoeffType>(
                                ind,
                                defaultValue);
                            }
                        }
                        else
                        {
                            var columns = this.owner.afterLastColumn;
                            for (int i = 0; i < columns; ++i)
                            {
                                yield return new KeyValuePair<long, CoeffType>(
                                    i,
                                    defaultValue);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Obtém um enumerador não genérico para os elementos da linha,
            /// incluindo os valores por defeito.
            /// </summary>
            /// <returns>O enumerador.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        #endregion Classes Auxiliares
    }

    #endregion Matriz CRS
}
