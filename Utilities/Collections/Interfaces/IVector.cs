namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define as propriedades e método essenciais a um vector.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos objectos que constituem as entradas do vector.</typeparam>
    public interface IVector<CoeffType> : IEnumerable<CoeffType>
    {
        /// <summary>
        /// Obtém e atribui o valor da entrada especificada.
        /// </summary>
        /// <param name="index">O índice da entrada.</param>
        /// <returns>O valor da entrada.</returns>
        CoeffType this[int index] { get; set; }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        int Length { get; }

        /// <summary>
        /// Obtém o sub-vector especificado pelos índices.
        /// </summary>
        /// <param name="indices">Os índices.</param>
        /// <returns>O sub-vector.</returns>
        IVector<CoeffType> GetSubVector(int[] indices);

        /// <summary>
        /// Otbém o sub-vector especificado pela sequência de índices.
        /// </summary>
        /// <param name="indices">A sequência de índices.</param>
        /// <returns>O sub-vector.</returns>
        IVector<CoeffType> GetSubVector(IntegerSequence indices);

        /// <summary>
        /// Troca dois elementos do vector.
        /// </summary>
        /// <param name="first">O primeiro elemento a ser trocado.</param>
        /// <param name="second">O segundo elemento a ser trocado.</param>
        void SwapElements(int first, int second);

        /// <summary>
        /// Obtém uma cópia do vector corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        IVector<CoeffType> Clone();
    }

    /// <summary>
    /// Define as propriedades e métodos essenciais a um vector esparso.
    /// </summary>
    public interface ISparseVector<ObjectType> : IVector<ObjectType>
    {
        /// <summary>
        /// Obtém o valor por defeito.
        /// </summary>
        ObjectType DefaultValue { get; }

        /// <summary>
        /// Obtém o número de entradas não nulas.
        /// </summary>
        int NumberOfEntries { get; }

        /// <summary>
        /// Obtém um enumerador para todas as entradas não nulas do vector.
        /// </summary>
        /// <remarks>
        /// Caso o vector seja para ser incluído como entrada em alguns algoritmos, o enumerável deverá
        /// retornar as entradas em sequência crescente pela chave.
        /// </remarks>
        /// <returns>As entradas não nulas do vector.</returns>
        IEnumerable<KeyValuePair<int, ObjectType>> GetEntries();

        /// <summary>
        /// Remove a entrada.
        /// </summary>
        /// <param name="entryNumber">O número da entrada a ser removida.</param>
        void Remove(int entryNumber);

        /// <summary>
        /// Verifica se o vector esparso contém a entrada especificada.
        /// </summary>
        /// <param name="entryIndex">A entrada.</param>
        /// <returns>Verdadeiro caso o vector contenha a entrada e falso caso contrário.</returns>
        bool ContainsEntry(int entryIndex);

        /// <summary>
        /// Tenta obter a entrada especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice da entrada.</param>
        /// <param name="entry">A entrada.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        bool TryGetLine(int index, out ObjectType entry);
    }
}
