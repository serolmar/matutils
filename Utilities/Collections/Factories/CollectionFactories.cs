namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Fábrica responsável pela criação de dicionários associados com comparadores
    /// de igualdade.
    /// </summary>
    /// <typeparam name="Key">O tipo de obejctos que constituem as chaves do dicionário.</typeparam>
    /// <typeparam name="Value">O tipo de objectos que constituem os valores do dicionário.</typeparam>
    public class DictionaryEqualityComparerFactory<Key, Value>
        : IFactory<IDictionary<Key, Value>>
    {
        /// <summary>
        /// Mantém o comparador que deverá ser associado aos dicionários.
        /// </summary>
        private IEqualityComparer<Key> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DictionaryEqualityComparerFactory{Key, Value}"/>
        /// </summary>
        /// <param name="comparer">O comparador.</param>
        public DictionaryEqualityComparerFactory(IEqualityComparer<Key> comparer)
        {
            if (comparer == null)
            {
                this.comparer = EqualityComparer<Key>.Default;
            }
            else
            {
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <returns>O objecto criado.</returns>
        public IDictionary<Key, Value> Create()
        {
            return new Dictionary<Key, Value>(this.comparer);
        }
    }

    /// <summary>
    /// Fábrica responsável pela criação de dicionários ordenados associados
    /// com comparadores.
    /// </summary>
    /// <typeparam name="Key">O tipo de obejctos que constituem as chaves do dicionário.</typeparam>
    /// <typeparam name="Value">O tipo de objectos que constituem os valores do dicionário.</typeparam>
    public class SortedDictionaryComparerFactory<Key, Value>
        : IFactory<IDictionary<Key, Value>>
    {
        /// <summary>
        /// O comparador.
        /// </summary>
        private IComparer<Key> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SortedDictionaryComparerFactory{Key, Value}"/>
        /// </summary>
        /// <param name="comparer"></param>
        public SortedDictionaryComparerFactory(IComparer<Key> comparer)
        {
            if (comparer == null)
            {
                this.comparer = Comparer<Key>.Default;
            }
            else
            {
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <returns>O objecto criado.</returns>
        public IDictionary<Key, Value> Create()
        {
            return new SortedDictionary<Key, Value>(this.comparer);
        }
    }

    /// <summary>
    /// Fábrica responsável pela criação de vectores com uma
    /// determinada dimensão.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem as entradas do vector.</typeparam>
    public class ArrayCapacityFactory<T> : IFactory<T[], int>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <param name="item1">A capacidade do vector a ser criado.</param>
        /// <returns>O objecto criado.</returns>
        public T[] Create(int item1)
        {
            return new T[item1];
        }
    }

    /// <summary>
    /// Fábrica responsável pela criação de listas com uma
    /// determinada dimensão.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem as entradas do vector.</typeparam>
    public class ListCapacityFactory<T> : IFactory<List<T>, int>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <param name="item1">A capacidade da lista a ser criada.</param>
        /// <returns>O objecto criado.</returns>
        public List<T> Create(int item1)
        {
            return new List<T>(item1);
        }
    }
}
