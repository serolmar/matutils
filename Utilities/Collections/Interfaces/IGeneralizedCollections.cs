// -----------------------------------------------------------------------
// <copyright file="IGeneralizedCollections.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma colecção de objectos.
    /// </summary>
    /// <typeparam name="T">
    /// O tipo de objectos que constituem as entradas das colecções.
    /// </typeparam>
    public interface ILongCollection<T> : ICollection<T>
    {
        /// <summary>
        /// Obtém o número de elementos da colecção.
        /// </summary>
        uint UintCount { get; }

        /// <summary>
        /// Obtém o número de elementos da colecção.
        /// </summary>
        long LongCount { get; }

        /// <summary>
        /// Obtém o número de elementos da colecção.
        /// </summary>
        ulong UlongCount { get; }

        /// <summary>
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="dimensions">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        void CopyTo(
            Array array, 
            long[] dimensions);
    }

    /// <summary>
    /// Representa uma colecção de objectos que podem ser individualmente acedidos
    /// por um índice dado por um inteiro longo.
    /// </summary>
    /// <typeparam name="T">O tipo dos elementos que constituem as entradas da lista.</typeparam>
    public interface ILongList<T> : IList<T>, ILongCollection<T>
    {
        /// <summary>
        /// Obtém ou atribui o valor do elemento no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor do elemento no índice especificado.</returns>
        T this[uint index] { get; set; }

        /// <summary>
        /// Obtém ou atribui o valor do elemento no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor do elemento no índice especificado.</returns>
        T this[long index] { get; set; }

        /// <summary>
        /// Obtém ou atribui o valor do elemento no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor do elemento no índice especificado.</returns>
        T this[ulong index] { get; set; }

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
        void Insert(uint index, T item);

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O item a ser inserido.</param>
        void Insert(long index, T item);

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O item a ser inserido.</param>
        void Insert(ulong index, T item);

        /// <summary>
        /// Remove o item especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice da posição a remover.</param>
        void RemoveAt(uint index);

        /// <summary>
        /// Remove o item especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice da posição a remover.</param>
        void RemoveAt(long index);

        /// <summary>
        /// Remove o item especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice da posição a remover.</param>
        void RemoveAt(ulong index);
    }

    /// <summary>
    /// Respresenta um dicionário cujo contentor suporta um número
    /// elevado de itens.
    /// </summary>
    /// <typeparam name="TKey">O tipo dos objectos que constituem as chaves.</typeparam>
    /// <typeparam name="TValue">O tipo dos objectos que constituem os valores.</typeparam>
    public interface ILongDicationary<TKey, TValue> 
        : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// Obtém o número de elementos no dicionário.
        /// </summary>
        uint UintCount { get; }

        /// <summary>
        /// Obtém o número de elementos no dicionário.
        /// </summary>
        long LongCount { get; }

        /// <summary>
        /// Obtém o número de elementos no dicionário.
        /// </summary>
        ulong UlongCount { get; }
    }
}
