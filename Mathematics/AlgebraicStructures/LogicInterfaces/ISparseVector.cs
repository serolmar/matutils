namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
