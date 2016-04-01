namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma colecção de objectos que podem ser individualmente acedidos
    /// por um índice dado por um inteiro longo.
    /// </summary>
    /// <typeparam name="T">O tipo dos elementos que constituem as entradas da lista.</typeparam>
    public interface ILongList<T> : IList<T>
    {
        /// <summary>
        /// Obtém ou atribui o valor do elemento no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor do elemento no índice especificado.</returns>
        T this[long index] { get; set; }

        /// <summary>
        /// Obtém o número de elementos na lista.
        /// </summary>
        long LongCount { get; }

        /// <summary>
        /// Obtém o índice do item na lista.
        /// </summary>
        /// <param name="item">O parâmetro.</param>
        /// <returns>O índice da entrada ou -1 caso não exista.</returns>
        long LongIndexOf(T item);

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O item a ser inserido.</param>
        void Insert(long index, T item);

        /// <summary>
        /// Remove o item especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice da posição a remover.</param>
        void RemoveAt(long index);
    }
}
