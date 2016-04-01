namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define as propriedades e métodos essenciais à definição de uma matriz.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da matriz.</typeparam>
    public interface IMatrix<ObjectType> : IEnumerable<ObjectType>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        ObjectType this[int line, int column] { get; set; }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        int GetLength(int dimension);

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        void SwapLines(int i, int j);

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        void SwapColumns(int i, int j);

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        IMatrix<ObjectType> GetSubMatrix(int[] lines, int[] columns);

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        IMatrix<ObjectType> GetSubMatrix(IntegerSequence lines, IntegerSequence columns);
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais à definição de uma matriz.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da matriz.</typeparam>
    public interface ILongMatrix<ObjectType> : IMatrix<ObjectType>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="line">A coordenada da linha onde a entrada se encontra.</param>
        /// <param name="column">A coordenada da coluna onde a entrada se encontra.</param>
        /// <returns>O valor da entrada.</returns>
        ObjectType this[long line, long column] { get; set; }

        /// <summary>
        /// Obtém o número de linhas ou colunas da matriz.
        /// </summary>
        /// <param name="dimension">Zero caso seja pretendido o número de linhas e um caso seja pretendido
        /// o número de colunas.
        /// </param>
        /// <returns>O número de entradas na respectiva dimensão.</returns>
        long GetLongLength(int dimension);

        /// <summary>
        /// Troca duas linhas da matriz.
        /// </summary>
        /// <param name="i">A primeira linha a ser trocada.</param>
        /// <param name="j">A segunda linha a ser trocada.</param>
        void SwapLines(long i, long j);

        /// <summary>
        /// Troca duas colunas da matriz.
        /// </summary>
        /// <param name="i">A primeira coluna a ser trocaada.</param>
        /// <param name="j">A segunda coluna a ser trocada.</param>
        void SwapColumns(long i, long j);

        /// <summary>
        /// Obtém a submatriz indicada no argumento.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        IMatrix<ObjectType> GetSubMatrix(long[] lines, long[] columns);

        /// <summary>
        /// Obtém a submatriz indicada no argumento considerado como sequência de inteiros.
        /// </summary>
        /// <param name="lines">As correnadas das linhas que constituem a submatriz.</param>
        /// <param name="columns">As correnadas das colunas que constituem a submatriz.</param>
        /// <returns>A submatriz procurada.</returns>
        IMatrix<ObjectType> GetSubMatrix(LongIntegerSequence lines, LongIntegerSequence columns);
    }

    /// <summary>
    /// Representa uma matriz quadrada.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public interface ISquareMatrix<CoeffType> : IMatrix<CoeffType>
    {
        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        bool IsSymmetric(IEqualityComparer<CoeffType> equalityComparer);
    }

    /// <summary>
    /// Representa uma matriz quadrada.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public interface ILongSquareMatrix<CoeffType> : ILongMatrix<CoeffType>
    {
        /// <summary>
        /// Verifica se se trata de uma matriz simétrica.
        /// </summary>
        /// <param name="equalityComparer">O comparador das entradas.</param>
        /// <returns>Verdadeiro caso se trate de uma matriz simétrica e falso no caso contrário.</returns>
        bool IsSymmetric(IEqualityComparer<CoeffType> equalityComparer);
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a uma matriz esparsa.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    /// <typeparam name="L">O tipo de objectos que constituem as linhas da matriz.</typeparam>
    public interface ISparseMatrix<ObjectType, L> : IMatrix<ObjectType>
        where L : ISparseMatrixLine<ObjectType>
    {
        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        L this[int line] { get; }

        /// <summary>
        /// Obtém o valor por defeito.
        /// </summary>
        ObjectType DefaultValue { get; }

        /// <summary>
        /// Obtém o número de linhas não nulas.
        /// </summary>
        int NumberOfLines { get; }

        /// <summary>
        /// Obtém um enumerador para todas as linhas não nulas da matriz.
        /// </summary>
        /// <remarks>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as linhas em sequência crescente pela chave.
        /// </remarks>
        /// <returns>As linhas não nulas da matriz.</returns>
        IEnumerable<KeyValuePair<int, L>> GetLines();

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        void Remove(int lineNumber);

        /// <summary>
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        bool ContainsLine(int line);

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        bool TryGetLine(int index, out L line);

        /// <summary>
        /// Obtém as colunas atribuídas à linha especificada.
        /// </summary>
        /// <remarks>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as colunas em sequência crescente pela chave.
        /// </remarks>
        /// <param name="line">A linha.</param>
        /// <returns>As colunas atribuídas.</returns>
        IEnumerable<KeyValuePair<int, ObjectType>> GetColumns(int line);
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a uma matriz esparsa.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    /// <typeparam name="L">O tipo de objectos que constituem as linhas da matriz.</typeparam>
    public interface ILongSparseMatrix<ObjectType, L> 
        : ISparseMatrix<ObjectType, L>, ILongMatrix<ObjectType>
        where L : ILongSparseMatrixLine<ObjectType>
    {
        /// <summary>
        /// Obtém a linha pelo respectivo valor.
        /// </summary>
        /// <param name="line">O índice.</param>
        /// <returns>A linha caso exista.</returns>
        L this[long line] { get; }

        /// <summary>
        /// Obtém um enumerador para todas as linhas não nulas da matriz.
        /// </summary>
        /// <remarks>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as linhas em sequência crescente pela chave.
        /// </remarks>
        /// <returns>As linhas não nulas da matriz.</returns>
        IEnumerable<KeyValuePair<long, L>> LongGetLines();

        /// <summary>
        /// Remove a linha.
        /// </summary>
        /// <param name="lineNumber">O número da linha a ser removida.</param>
        void Remove(long lineNumber);

        /// <summary>
        /// Verifica se a matriz esparsa contém a linha especificada.
        /// </summary>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a matriz contenha a linha e falso caso contrário.</returns>
        bool ContainsLine(long line);

        /// <summary>
        /// Tenta obter a linha especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da linha.</param>
        /// <param name="line">A linha.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        bool TryGetLine(long index, out L line);

        /// <summary>
        /// Obtém as colunas atribuídas à linha especificada.
        /// </summary>
        /// <remarks>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as colunas em sequência crescente pela chave.
        /// </remarks>
        /// <param name="line">A linha.</param>
        /// <returns>As colunas atribuídas.</returns>
        IEnumerable<KeyValuePair<long, ObjectType>> GetColumns(long line);
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a uma linha de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da linha da matriz.</typeparam>
    public interface ISparseMatrixLine<ObjectType> 
        : IEnumerable<KeyValuePair<int, ObjectType>>, IDisposable
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O objecto.</returns>
        ObjectType this[int index] { get; set; }

        /// <summary>
        /// Obtém o número de colunas não nulas.
        /// </summary>
        int NumberOfColumns { get; }

        /// <summary>
        /// Obtém um enumerador para todas as colunas não nulas.
        /// </summary>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as colunas em sequência crescente pela chave.
        /// <returns>O enumerador.</returns>
        IEnumerable<KeyValuePair<int, ObjectType>> GetColumns();

        /// <summary>
        /// Remove a entrada espeficada pelo índice.
        /// </summary>
        /// <param name="columnIndex">O índice da entrada a ser removido.</param>
        void Remove(int columnIndex);

        /// <summary>
        /// Verifica se a linha esparsa contém a coluna especificada.
        /// </summary>
        /// <param name="column">A coluna.</param>
        /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
        bool ContainsColumn(int column);

        /// <summary>
        /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
        /// </summary>
        /// <param name="column">O índice da coluna.</param>
        /// <param name="value">O valor na coluna.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        bool TryGetColumnValue(int column, out ObjectType value);
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a uma linha de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da linha da matriz.</typeparam>
    public interface ILongSparseMatrixLine<ObjectType>
        : ISparseMatrixLine<ObjectType>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O objecto.</returns>
        ObjectType this[long index] { get; set; }

        /// <summary>
        /// Obtém o número de colunas não nulas.
        /// </summary>
        long LongNumberOfColumns { get; }

        /// <summary>
        /// Obtém um enumerador para todas as colunas não nulas.
        /// </summary>
        /// Caso a matriz seja para ser incluída como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as colunas em sequência crescente pela chave.
        /// <returns>O enumerador.</returns>
        IEnumerable<KeyValuePair<long, ObjectType>> LongGetColumns();

        /// <summary>
        /// Remove a entrada espeficada pelo índice.
        /// </summary>
        /// <param name="columnIndex">O índice da entrada a ser removido.</param>
        void Remove(long columnIndex);

        /// <summary>
        /// Verifica se a linha esparsa contém a coluna especificada.
        /// </summary>
        /// <param name="column">A coluna.</param>
        /// <returns>Verdadeiro caso a linha contenha a coluna e falso caso contrário.</returns>
        bool ContainsColumn(long column);

        /// <summary>
        /// Tenta obter o valor da coluna caso esta exista na linha da matriz esparsa.
        /// </summary>
        /// <param name="column">O índice da coluna.</param>
        /// <param name="value">O valor na coluna.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        bool TryGetColumnValue(long column, out ObjectType value);

        /// <summary>
        /// Obtém o enumerador para as entradas.
        /// </summary>
        /// <returns>O enumerador para as entradas.</returns>
        IEnumerator<KeyValuePair<long, ObjectType>> LongGetEnumerator();
    }
}
