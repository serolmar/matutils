namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define as propriedades e métodos essenciais a uma linha de uma matriz esparsa.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as entradas da linha da matriz.</typeparam>
    public interface ISparseMatrixLine<ObjectType> : IEnumerable<KeyValuePair<int, ObjectType>>
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
}
