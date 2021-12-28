// -----------------------------------------------------------------------
// <copyright file="GeneralizedCollections.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define o dicionário ordenado do sistema que implementa a interface.
    /// </summary>
    /// <typeparam name="TKey">O tipo dos objectos que constituem as chaves.</typeparam>
    /// <typeparam name="TValue">O tipo dos objectos que constituem os valores.</typeparam>
    public class SortedDictionaryWrapper<TKey, TValue>
        : SortedDictionary<TKey, TValue>,
        ISortedDictionary<TKey, TValue>
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="SortedDictionaryWrapper{TKey, TValue}"/>.
        /// </summary>
        public SortedDictionaryWrapper() { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="SortedDictionaryWrapper{TKey, TValue}"/>.
        /// </summary>
        /// <param name="comparer">O comparador para as chaves.</param>
        public SortedDictionaryWrapper(IComparer<TKey> comparer) 
            : base(comparer) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="SortedDictionaryWrapper{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">O dicionário.</param>
        public SortedDictionaryWrapper(IDictionary<TKey, TValue> dictionary) 
            : base(dictionary) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="SortedDictionaryWrapper{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary">O dicionário.</param>
        /// <param name="comparer">O comparador para as chaves.</param>
        public SortedDictionaryWrapper(
            IDictionary<TKey, TValue> dictionary,
            IComparer<TKey> comparer)
            : base(dictionary, comparer) { }
    }

    /// <summary>
    /// Implementa um enumerável sobre o qual é aplicada uma função de transformação.
    /// </summary>
    /// <remarks>
    /// A aplicação de uma transformação aos elementos de um enumerável pode ser útil em alguns
    /// cenários. Por exemplo, a determinação da variância pode ser conseguida, calculando a
    /// média sobre a transformação que, a cada elemento, retorna o seu quadrado.
    /// </remarks>
    /// <typeparam name="T">
    /// O tipo dos objectos que constituem os elementos do enumerável original.
    /// </typeparam>
    /// <typeparam name="Q">
    /// O tipo dos objectos que constituem os elementos do enumerável transformado.
    /// </typeparam>
    public class TransformEnumerable<T, Q> : IEnumerable<Q>
    {
        /// <summary>
        /// Mantém o enumerador original.
        /// </summary>
        private IEnumerable<T> original;

        /// <summary>
        /// A função de transformação.
        /// </summary>
        private Func<T, Q> transform;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TransformEnumerable{T, Q}"/>.
        /// </summary>
        /// <param name="original">O enumerável original.</param>
        /// <param name="transform">A função de transformação.</param>
        public TransformEnumerable(
            IEnumerable<T> original,
            Func<T, Q> transform)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            else if (transform == null)
            {
                throw new ArgumentNullException("transform");
            }
            else
            {
                this.original = original;
                this.transform = transform;
            }
        }

        /// <summary>
        /// Obtém o enumerador do enumerável.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Q> GetEnumerator()
        {
            return new Enumerator(
                this.original.GetEnumerator(),
                this.transform);
        }

        /// <summary>
        /// Obtém a forma não genérica do enumerador do enumerável.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(
                this.original.GetEnumerator(),
                this.transform);
        }

        /// <summary>
        /// Implementa o enumerador.
        /// </summary>
        private class Enumerator : IEnumerator<Q>
        {
            /// <summary>
            /// Mantém o enumerador original.
            /// </summary>
            private IEnumerator<T> original;

            /// <summary>
            /// A função de transformação.
            /// </summary>
            private Func<T, Q> transform;

            /// <summary>
            /// Instancia uma nova instância de objectos to tipo <see cref="Enumerator"/>.
            /// </summary>
            /// <param name="original">O eumerador original.</param>
            /// <param name="transform">A função de transformação.</param>
            public Enumerator(
                IEnumerator<T> original,
                Func<T, Q> transform)
            {
                this.original = original;
                this.transform = transform;
            }

            /// <summary>
            /// Obtém o valor actual.
            /// </summary>
            public Q Current
            {
                get
                {
                    return this.transform.Invoke(
                        this.original.Current);
                }
            }

            /// <summary>
            /// Obtém a versão não genérica do valor actual.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.transform.Invoke(
                      this.original.Current);
                }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
                this.original.Dispose();
            }

            /// <summary>
            /// Move para o próximo elemento do enumerador.
            /// </summary>
            /// <returns>Verdadeiro caso seja possível o movimento e falso caso contrário.</returns>
            public bool MoveNext()
            {
                return this.original.MoveNext();
            }

            /// <summary>
            /// Coloca o enumerador no início.
            /// </summary>
            public void Reset()
            {
                this.original.Reset();
            }
        }
    }

    /// <summary>
    /// Implementa uma lista sobre a qual é aplicada uma função de transformação.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem os elementos da lista original.</typeparam>
    /// <typeparam name="Q">O tipo dos objectos que constituem os elementos da lista atransformada.</typeparam>
    public class TransformList<T, Q> :
        IList<Q>
    {
        /// <summary>
        /// Mantém a lista original.
        /// </summary>
        private IList<T> original;

        /// <summary>
        /// Mantém a função de transformação.
        /// </summary>
        private Func<T, Q> transform;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TransformList{T, Q}"/>
        /// </summary>
        /// <param name="original">A lista original.</param>
        /// <param name="transform">A transformada.</param>
        public TransformList(
            IList<T> original,
            Func<T, Q> transform)
        {
            if (original == null)
            {
                throw new ArgumentNullException("original");
            }
            else if (transform == null)
            {
                throw new ArgumentNullException("transform");
            }
            else
            {
                this.original = original;
                this.transform = transform;
            }
        }

        /// <summary>
        /// Obtém o valor identificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public Q this[int index]
        {
            get
            {
                return this.transform.Invoke(
                    this.original[index]);
            }
            set
            {
                throw new NotSupportedException("Transformation list is readonly.");
            }
        }

        /// <summary>
        /// Obtém o número de elementos da colecção.
        /// </summary>
        public int Count
        {
            get
            {
                return this.original.Count;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a lista é só de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira ocorrência do valor na lista.
        /// </summary>
        /// <param name="item">O valor.</param>
        /// <returns>O índice.</returns>
        public int IndexOf(Q item)
        {
            throw new NotSupportedException("IndexOf is not supporte in transformation list.");
        }

        /// <summary>
        /// Insere o valor no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O valor.</param>
        public void Insert(int index, Q item)
        {
            throw new NotSupportedException("Transformation list is readonly.");
        }

        /// <summary>
        /// Remove o elemento da posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Transformation list is readonly.");
        }

        /// <summary>
        /// Adiciona um item à lista.
        /// </summary>
        /// <param name="item">O item.</param>
        public void Add(Q item)
        {
            throw new NotSupportedException("Transformation list is readonly.");
        }

        /// <summary>
        /// Elimina todos os itens da lista.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException("Transformation list is readonly.");
        }

        /// <summary>
        /// Verifica se o item se encontra na lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro caso o item seja encontrado e falso caso contrário.</returns>
        public bool Contains(Q item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copia os valores da lista para um vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="arrayIndex">O índice do vector onde se incia a cópia.</param>
        public void CopyTo(Q[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else
            {
                var length = array.Length;
                if (arrayIndex < 0 || arrayIndex > length)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex", "Array index must be non-negative and less than the size of collection");
                }
                else
                {
                    var count = this.original.Count;
                    if (length - arrayIndex < count)
                    {
                        throw new CollectionsException("There is no enough entries in array to fit the list.");
                    }
                    else
                    {
                        var index = arrayIndex;
                        for (var i = 0; i < count; ++i)
                        {
                            array[index++] = this.transform.Invoke(
                                this.original[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove o item da lista.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(Q item)
        {
            throw new NotSupportedException("Transformation list is readonly.");
        }

        /// <summary>
        /// Obtém um enumerador para a lista.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Q> GetEnumerator()
        {
            return new Enumerator(
                this.original.GetEnumerator(),
                this.transform);
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a lista.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(
                this.original.GetEnumerator(),
                this.transform);
        }

        /// <summary>
        /// Implementa o enumerador.
        /// </summary>
        private class Enumerator : IEnumerator<Q>
        {
            /// <summary>
            /// Mantém o enumerador original.
            /// </summary>
            private IEnumerator<T> original;

            /// <summary>
            /// A função de transformação.
            /// </summary>
            private Func<T, Q> transform;

            /// <summary>
            /// Instancia uma nova instância de objectos to tipo <see cref="Enumerator"/>.
            /// </summary>
            /// <param name="original"></param>
            /// <param name="transform"></param>
            public Enumerator(
                IEnumerator<T> original,
                Func<T, Q> transform)
            {
                this.original = original;
                this.transform = transform;
            }

            /// <summary>
            /// Obtém o valor actual.
            /// </summary>
            public Q Current
            {
                get
                {
                    return this.transform.Invoke(
                        this.original.Current);
                }
            }

            /// <summary>
            /// Obtém a versão não genérica do valor actual.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.transform.Invoke(
                      this.original.Current);
                }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
                this.original.Dispose();
            }

            /// <summary>
            /// Move para o próximo elemento do enumerador.
            /// </summary>
            /// <returns>Verdadeiro caso seja possível o movimento e falso caso contrário.</returns>
            public bool MoveNext()
            {
                return this.original.MoveNext();
            }

            /// <summary>
            /// Coloca o enumerador no início.
            /// </summary>
            public void Reset()
            {
                this.original.Reset();
            }
        }
    }

    /// <summary>
    /// Implementação de uma fila dupla.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem as entradas da fila.</typeparam>
    public class Deque<T> :
        IList<T>,
        ICollection
    {
        /// <summary>
        /// O vector vazio.
        /// </summary>
        private static readonly T[] emptyArray = new T[0];

        /// <summary>
        /// Mantém a capacidade por defeito.
        /// </summary>
        private const int defaultCapacity = 4;

        /// <summary>
        /// O vector que contém as entradas da fila.
        /// </summary>
        private T[] array;

        /// <summary>
        /// Mantém o número de elementos da fila dupla.
        /// </summary>
        private int count;

        /// <summary>
        /// O índice da primeira posição do vector.
        /// </summary>
        private int offset;

        /// <summary>
        /// Mantém o objecto para sincronização de linhas de fluxo.
        /// </summary>
        private object synchRoot;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Deque{T}"/>.
        /// </summary>
        public Deque()
        {
            this.count = 0;
            this.offset = 0;
            this.array = new T[defaultCapacity];
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Deque{T}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade inicial da colecção.</param>
        public Deque(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            else if (capacity == 0)
            {
                this.count = 0;
                this.offset = 0;
                this.array = emptyArray;
            }
            else
            {
                this.count = 0;
                this.offset = 0;
                this.array = new T[capacity];
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index > this.count)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    if (this.offset == 0)
                    {
                        return this.array[index];
                    }
                    else
                    {
                        var len = this.array.Length;
                        var pos = index - (len - this.offset);
                        if (pos < 0)
                        {
                            return this.array[len + pos];
                        }
                        else
                        {
                            return this.array[pos];
                        }
                    }
                }
            }
            set
            {
                if (index < 0 || index > this.count)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    if (this.offset == 0)
                    {
                        this.array[index] = value;
                    }
                    else
                    {
                        var len = this.array.Length;
                        var pos = index - (len - this.offset);
                        if (pos < 0)
                        {
                            this.array[len + pos] = value;
                        }
                        else
                        {
                            this.array[pos] = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da capacidade.
        /// </summary>
        /// <remarks>
        /// Se o valor da capacidade for superior ao tamanho do objecto
        /// então esse tamanho será actualizado.
        /// </remarks>
        public int Capacity
        {
            get
            {
                return this.array.Length;
            }
            set
            {
                if (value < this.count)
                {
                    throw new ArgumentOutOfRangeException(
                        "capacity",
                        "The capacity must be greater or equal to the number of elements in dequeue.");
                }
                else if (value > this.count)
                {
                    this.SetNewCapacity(value);
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é sincronizada.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém o objecto de sincronização.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (this.synchRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<Object>(ref this.synchRoot, new object(), null);
                }

                return this.synchRoot;
            }
        }

        /// <summary>
        /// Insere o item no final da pilha dupla.
        /// </summary>
        /// <param name="item">O item a ser inserido.</param>
        public void EnqueueBack(T item)
        {
            var len = this.array.Length;
            var newCount = this.count + 1;
            if (newCount > len)
            {
                len = len > (int.MaxValue >> 1) ? int.MaxValue : len << 1;
                this.SetNewCapacity(len);
            }

            len = this.array.Length;
            if (this.offset == 0)
            {
                this.array[this.count] = item;
            }
            else
            {
                var pos = this.count - (len - this.offset);
                if (pos < 0)
                {
                    this.array[len + pos] = item;
                }
                else
                {
                    this.array[pos] = item;
                }
            }

            this.count = newCount;
        }

        /// <summary>
        /// Insere o item no início da pilha dupla.
        /// </summary>
        /// <param name="item">O item a ser inserido.</param>
        public void EnqueueFront(T item)
        {
            var len = this.array.Length;
            var newCount = this.count + 1;
            if (newCount > len)
            {
                len = len > (int.MaxValue >> 1) ? int.MaxValue : len << 1;
                this.SetNewCapacity(len);
            }

            len = this.array.Length;
            if (this.offset == 0)
            {
                this.offset = len - 1;
                this.array[this.offset] = item;
            }
            else
            {
                this.array[--this.offset] = item;
            }

            this.count = newCount;
        }

        /// <summary>
        /// Remove o item do final da pilha dupla.
        /// </summary>
        public void DequeueBack()
        {
            if (this.count == 0)
            {
                throw new InvalidOperationException("The dequeue is empty.");
            }
            else
            {
                --this.count;
                var len = this.array.Length;
                var pos = this.count - (len - this.offset);
                if (pos < 0)
                {
                    this.array[len + pos] = default(T);
                }
                else
                {
                    this.array[pos] = default(T);
                }
            }
        }

        /// <summary>
        /// Remove o item do início da pilha dupla.
        /// </summary>
        public void DequeueFront()
        {
            if (this.count == 0)
            {
                throw new InvalidOperationException("The dequeue is empty.");
            }
            else
            {
                this.array[this.offset] = default(T);
                ++this.offset;
                --this.count;
                if (this.offset == this.array.Length)
                {
                    this.offset = 0;
                }
            }
        }

        /// <summary>
        /// Obtém o item no final da pilha dupla sem o remover.
        /// </summary>
        /// <returns>O item.</returns>
        public T PeekBack()
        {
            if (this.count == 0)
            {
                throw new InvalidOperationException("The dequeue is empty.");
            }
            else
            {
                if (this.offset == 0)
                {
                    return this.array[this.count - 1];
                }
                else
                {
                    var len = this.array.Length;
                    var pos = this.count - 1 - (len - this.offset);
                    if (pos < 0)
                    {
                        return this.array[len + pos];
                    }
                    else
                    {
                        return this.array[pos];
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o item no início da pilha dupla sem o remover.
        /// </summary>
        /// <returns>O item.</returns>
        public T PeekFront()
        {
            if (this.count == 0)
            {
                throw new InvalidOperationException("The dequeue is empty.");
            }
            else
            {
                return this.array[this.offset];
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            if (this.offset == 0)
            {
                return Array.IndexOf<T>(this.array, item);
            }
            else
            {
                var side = this.array.Length - this.offset;
                if (this.count <= side)
                {
                    var firstIndex = Array.IndexOf<T>(this.array, item, this.offset, this.count);
                    if (firstIndex == -1)
                    {
                        return firstIndex;
                    }
                    else
                    {
                        return firstIndex - this.offset;
                    }
                }
                else
                {
                    var firstIndex = Array.IndexOf<T>(this.array, item, this.offset, side);
                    if (firstIndex == -1)
                    {
                        var len = this.count - side;
                        firstIndex = Array.IndexOf<T>(this.array, item, 0, len);

                        if (firstIndex == -1)
                        {
                            return firstIndex;
                        }
                        else
                        {
                            return firstIndex + this.offset;
                        }
                    }
                    else
                    {
                        if (firstIndex == -1)
                        {
                            return firstIndex;
                        }
                        else
                        {
                            return firstIndex - this.offset;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <remarks>
        /// É possível inserir o valor antes do início da colecção.
        /// </remarks>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(int index, T item)
        {
            if (index > count || index < -1)
            {
                throw new IndexOutOfRangeException("Index must be greater than -1 and smaller than count.");
            }
            else
            {
                var len = this.array.Length;
                var newCount = this.count + 1;
                if (newCount > len)
                {
                    len = len > (int.MaxValue >> 1) ? int.MaxValue : len << 1;
                    this.SetNewCapacity(len);
                }

                len = this.array.Length;
                if (this.offset == 0)
                {
                    if (index == -1)
                    {
                        this.offset = this.array.Length - 1;
                        this.array[this.offset] = item;
                        this.offset = len - 1;
                    }
                    else if (index < this.count)
                    {
                        Array.Copy(
                            this.array,
                            index,
                            this.array,
                            index + 1,
                            this.count - index);
                        this.array[index] = item;
                    }
                    else
                    {
                        this.array[index] = item;
                    }
                }
                else
                {
                    if (index == -1)
                    {
                        --this.offset;
                        this.array[this.offset] = item;
                    }
                    else
                    {
                        var side = len - this.offset;
                        if (index < side)
                        {
                            if (this.count < side)
                            {
                                var pos = this.offset + index;
                                Array.Copy(
                                    this.array,
                                    pos,
                                    this.array,
                                    pos + 1,
                                    this.count - pos);
                                this.array[pos] = item;
                            }
                            else
                            {
                                var lastItem = this.array[len - 1];
                                Array.Copy(
                                    this.array,
                                    0,
                                    this.array,
                                    1,
                                    this.count - index);
                                this.array[0] = lastItem;
                                var pos = this.offset + index;
                                Array.Copy(
                                    this.array,
                                    pos,
                                    this.array,
                                    pos + 1,
                                    len - pos - 1);
                                this.array[pos] = item;
                            }
                        }
                        else
                        {
                            var pos = index - side;
                            Array.Copy(
                                this.array,
                                pos,
                                this.array,
                                pos + 1,
                                this.count - index);
                            this.array[pos] = item;
                        }
                    }
                }

                this.count = newCount;
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                --this.count;
                if (this.offset == 0)
                {
                    if (index == 0)
                    {
                        this.array[index] = default(T);
                        ++this.offset;
                    }
                    else if (index < this.count)
                    {
                        Array.Copy(
                            this.array,
                            index + 1,
                            this.array,
                            index,
                            this.count - index);

                        this.array[this.count] = default(T);
                    }
                }
                else
                {
                    var len = this.array.Length;
                    var side = len - this.offset;
                    if (index < side)
                    {
                        if (this.count < side)
                        {
                            var pos = this.offset + index;
                            Array.Copy(
                                this.array,
                                pos + 1,
                                this.array,
                                pos,
                                this.count);
                            this.array[this.offset + this.count] = default(T);
                        }
                        else
                        {
                            var firstItem = this.array[0];
                            Array.Copy(
                                this.array,
                                1,
                                this.array,
                                0,
                                this.count - side);
                            this.array[this.count - side] = default(T);
                            var pos = this.offset + index;
                            Array.Copy(
                                this.array,
                                pos + 1,
                                this.array,
                                pos,
                                len - pos - 1);
                            this.array[len - 1] = firstItem;
                        }
                    }
                    else
                    {
                        var pos = index - side;
                        Array.Copy(
                            this.array,
                            pos + 1,
                            this.array,
                            pos,
                            this.count - side);
                        this.array[this.count - side] = default(T);
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            this.EnqueueBack(item);
        }

        /// <summary>
        /// Elimina todos os itens da colecção.
        /// </summary>
        public void Clear()
        {
            this.count = 0;
            this.offset = 0;
        }

        /// <summary>
        /// Retorna um valor que indica se o item especificado se encontra na colecção.
        /// </summary>
        /// <param name="item">O item a procurar.</param>
        /// <returns>Verdadeiro caso o item exista no vector e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            var index = this.IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Copia o conteúdo da colecção para um vector de sistema.
        /// </summary>
        /// <param name="array">O vector de sistema de destino.</param>
        /// <param name="arrayIndex">O índice a partir do qual é efectuada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else
            {
                this.InnerCopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Copia o conteúdo da colecção para um vector de sistema.
        /// </summary>
        /// <param name="array">O vector de sistema de destino.</param>
        /// <param name="index">O índice a partir do qual é efectuada a cópia.</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else
            {
                if (array.Rank == 1)
                {
                    this.InnerCopyTo(array, index);
                }
                else
                {
                    throw new ArgumentException("Array rank not supported.");
                }
            }
        }

        /// <summary>
        /// Remove um item da colecção.
        /// </summary>
        /// <param name="item">O item a ser removido.</param>
        /// <returns>Verdadeiro se o item for removido com sucesso e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            var index = this.IndexOf(item);
            if (index < 0)
            {
                this.RemoveAt(index);
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Obtém um enumerador genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var length = this.array.Length;
            for (var i = this.offset; i < length; ++i)
            {
                yield return this.array[i];
            }

            var len = this.count - (length - this.offset);
            for (var i = 0; i < len; ++i)
            {
                yield return this.array[i];
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Copia o conteúdo da colecção para um vector do sistema.
        /// </summary>
        /// <param name="array">O vector de sistema de destino.</param>
        /// <param name="index">O índice a partir do qual é efectuada a cópia.</param>
        private void InnerCopyTo(Array array, int index)
        {

            var arrayLength = array.Length;
            if (index < 0 || index > arrayLength)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "Index is out bounds of array.");
            }
            else if (index + this.count > arrayLength)
            {
                throw new ArgumentException(
                    "The number of elements in the source array is greater than the available number of elements from index to the end of the destination array.");
            }
            else if (this.offset == 0)
            {
                Array.Copy(
                    this.array,
                    0,
                    array,
                    index,
                    this.count);
            }
            else
            {
                var len = this.array.Length;
                var side = len - this.offset;
                if (this.count < side)
                {
                    Array.Copy(
                        this.array,
                        this.offset,
                        array,
                        index,
                        this.count);
                }
                else
                {
                    Array.Copy(
                        this.array,
                        this.offset,
                        array,
                        index,
                        side);
                    Array.Copy(
                        this.array,
                        0,
                        array,
                        index + side,
                        this.count - side);
                }
            }
        }

        /// <summary>
        /// Estabelece o valor da nova capacidade.
        /// </summary>
        /// <remarks>
        /// Nenhuma validação será efectuada a este nível.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade.</param>
        private void SetNewCapacity(int capacity)
        {
            var newArray = new T[capacity];
            if (this.offset == 0)
            {
                Array.Copy(this.array, newArray, this.count);
            }
            else
            {
                var len = this.array.Length - this.offset;
                if (this.count > len)
                {
                    Array.Copy(this.array, this.offset, newArray, 0, len);
                    Array.Copy(this.array, 0, newArray, len, this.count - len);
                }
                else
                {
                    Array.Copy(this.array, this.offset, newArray, 0, this.count);
                }
            }

            this.offset = 0;
            this.array = newArray;
        }
    }

    /// <summary>
    /// Implementa uma colecção do tipo meda.
    /// </summary>
    /// <typeparam name="T">
    /// O tipo de objectos associados aos itens da colecção.
    /// </typeparam>
    public class Heap<T> :
        ICollection<T>,
        ICollection
    {
        /// <summary>
        /// A capacidade por defeito.
        /// </summary>
        private const ulong defaultCapacity = 4;

        /// <summary>
        /// Mantém a instância de um objecto vazio.
        /// </summary>
        private static readonly T[] emptyArray = new T[0];

        /// <summary>
        /// O comparador de itens na colecção.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Mantém os elementos da colecção.
        /// </summary>
        private T[] items;

        /// <summary>
        /// Mantém o tamanho do vector.
        /// </summary>
        private ulong count;

        /// <summary>
        /// Mantém o objecto para sincronização de linhas de fluxo.
        /// </summary>
        private object synchRoot;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        public Heap()
        {
            this.comparer = Comparer<T>.Default;
            this.items = emptyArray;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade da colecção.</param>
        public Heap(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "capacity",
                    "Capacity must be a positive number.");
            }
            else
            {
                this.comparer = Comparer<T>.Default;
                if (capacity == 0)
                {
                    this.items = emptyArray;
                }
                else
                {
                    this.items = new T[capacity];
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade da colecção.</param>
        public Heap(ulong capacity)
        {
            this.comparer = Comparer<T>.Default;
            if (capacity == 0)
            {
                this.items = emptyArray;
            }
            else
            {
                this.items = new T[capacity];
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de itens.</param>
        public Heap(IComparer<T> comparer)
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
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade da colecção.</param>
        /// <param name="comparer">O comparador de itens.</param>
        public Heap(int capacity, IComparer<T> comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    "capacity",
                    "Capacity must be a positive number.");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
                if (capacity == 0)
                {
                    this.items = emptyArray;
                }
                else
                {
                    this.items = new T[capacity];
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade da colecção.</param>
        /// <param name="comparer">O comparador de itens.</param>
        public Heap(ulong capacity, IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
                if (capacity == 0)
                {
                    this.items = emptyArray;
                }
                else
                {
                    this.items = new T[capacity];
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="collection">A colecção para cópia.</param>
        public Heap(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                this.comparer = Comparer<T>.Default;
                this.ProcessCollection(collection);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Heap{T}"/>.
        /// </summary>
        /// <param name="collection">A colecção para cópia.</param>
        /// <param name="comparer">O comparador.</param>
        public Heap(IEnumerable<T> collection, IComparer<T> comparer)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comaprer");
            }
            else
            {
                this.comparer = comparer;
                this.ProcessCollection(collection);
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public int Count
        {
            get
            {
                return (int)this.count;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é sincronizada.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém o objecto de sincronização.
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (this.synchRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<Object>(
                        ref this.synchRoot,
                        new object(),
                        null);
                }

                return (this.synchRoot);
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da capacidade.
        /// </summary>
        public int Capacity
        {
            get
            {
                return this.items.Length;
            }
            set
            {
                if ((ulong)value < this.count)
                {
                    throw new CollectionsException(
                        "Capacity can't be less than the size of the collection.");
                }
                else if (value > this.items.Length)
                {
                    this.Reserve((ulong)value);
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da capacidade.
        /// </summary>
        public ulong UlongCapacity
        {
            get
            {
                return (ulong)this.items.LongLength;
            }
            set
            {
                if (value < this.count)
                {
                    throw new CollectionsException(
                        "Capacity can't be less than the size of the collection.");
                }
                else if (value > (ulong)this.items.LongLength)
                {
                    this.Reserve(value);
                }
            }
        }

        /// <summary>
        /// Obtém o valor do item que se encontra na raiz.
        /// </summary>
        public T Root
        {
            get
            {
                if (this.count == 0UL)
                {
                    throw new IndexOutOfRangeException("Heap is empty.");
                }
                else
                {
                    return this.items[0];
                }
            }
        }

        /// <summary>
        /// Obtém a raiz da meda e remove-a.
        /// </summary>
        /// <returns>O valor da raiz.</returns>
        public T PopRoot()
        {
            if (this.count == 0UL)
            {
                throw new IndexOutOfRangeException("Heap is empty.");
            }
            else
            {
                var root = this.items[0];
                this.RemoveAt(0);
                return root;
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            var len = (ulong)this.items.LongLength;
            if (this.count == len)
            {
                if (len == 0UL)
                {
                    len = defaultCapacity;
                }
                else
                {
                    len <<= 1;
                }
            }

            this.items[this.count] = item;
            this.HeapifyAdd(0UL, this.count);
            ++this.count;
        }

        /// <summary>
        /// Elimina todos os itens da colecção.
        /// </summary>
        public void Clear()
        {
            if (this.count > 0)
            {
                Array.Clear(this.items, 0, (int)this.count);
                this.count = 0UL;
            }
        }

        /// <summary>
        /// Retorna um valor que indica se o item se encontra na lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro se o item se encontrar na lista e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            var index = 0UL;
            if (this.TryFindValue(item, out index))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copia os valores da lista para uma ordenação.
        /// </summary>
        /// <remarks>
        /// A cópia é realizada na ordem em que os itens se encontram armazenados.
        /// </remarks>
        /// <param name="array">A ordenação de destino.</param>
        /// <param name="arrayIndex">O índice de destino onde será iniciada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Remove o item da lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro se a remoção for bem-sucedida e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            var index = 0UL;
            if (this.TryFindValue(item, out index))
            {
                this.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém um enumerador para a lista.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0UL; i < this.count; ++i)
            {
                yield return this.items[i];
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a lista.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <remarks>
        /// A cópia é realizada na ordem em que os itens se encontram armazenados.
        /// </remarks>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="index">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        public void CopyTo(Array array, int index)
        {
            this.items.CopyTo(array, index);
        }

        /// <summary>
        /// Coloca todos os itens na ordem pretendida, assumindo tratar-se
        /// da adição de um novo item à lista.
        /// </summary>
        /// <param name="firstItem">
        /// O primeiro item.
        /// </param>
        /// <param name="lastItem">
        /// O último item.
        /// </param>
        private void HeapifyAdd(ulong firstItem, ulong lastItem)
        {
            if (this.count > 1UL)
            {
                var child = lastItem;
                var childVal = this.items[child];
                while (child > firstItem)
                {
                    var parent = this.Parent(child);
                    var parentVal = this.items[parent];
                    if (this.comparer.Compare(
                        childVal,
                        parentVal) < 0)
                    {
                        this.items[child] = parentVal;
                        this.items[parent] = childVal;
                        child = parent;
                    }
                    else
                    {
                        child = firstItem;
                    }
                }
            }
        }

        /// <summary>
        /// Coloca todos os itens na ordem pretendida, assumindo tratar-se
        /// da remolçção de um item da lista.
        /// </summary>
        /// <param name="firstItem">
        /// O primeiro item.
        /// </param>
        /// <param name="lastItem">
        /// O último item.
        /// </param>
        private void SiftDown(ulong firstItem, ulong lastItem)
        {
            var root = firstItem;
            var leftRootIndex = this.Left(root);
            while (leftRootIndex <= lastItem)
            {
                var swap = root;
                var swapValue = this.items[root];
                var childValue = this.items[leftRootIndex];
                if (comparer.Compare(
                    childValue,
                    swapValue
                    ) < 0)
                {
                    swap = leftRootIndex;
                    ++leftRootIndex;
                    if (leftRootIndex <= lastItem)
                    {
                        var temp = this.items[leftRootIndex];
                        if (comparer.Compare(
                            temp,
                            childValue
                            ) < 0)
                        {
                            swap = leftRootIndex;
                            childValue = temp;
                        }
                    }
                }
                else
                {
                    ++leftRootIndex;
                    if (leftRootIndex <= lastItem)
                    {
                        childValue = this.items[leftRootIndex];
                        if (comparer.Compare(
                            childValue,
                            swapValue
                            ) < 0)
                        {
                            swap = leftRootIndex;
                        }
                    }
                }

                if (swap == root)
                {
                    return;
                }
                else
                {
                    this.items[swap] = swapValue;
                    this.items[root] = childValue;
                    root = swap;
                    leftRootIndex = this.Left(root);
                }
            }
        }

        /// <summary>
        /// Coloca todos os itens na ordem pretendida.
        /// </summary>
        /// <param name="firstIndex">O índice a partir do qual é para construir a meda.</param>
        /// <param name="lastIndex">
        /// O último índice até ao qual é para construir a meda.
        /// </param>
        private void Heapify(ulong firstIndex, ulong lastIndex)
        {
            for (var i = lastIndex; i > firstIndex; --i)
            {
                var parentPosition = this.Parent(i);

                if (this.comparer.Compare(this.items[i], this.items[parentPosition]) < 0)
                {
                    var swap = this.items[parentPosition];
                    this.items[parentPosition] = this.items[i];
                    this.items[i] = swap;
                }
            }
        }

        /// <summary>
        /// Reserva o espaço para os itens.
        /// </summary>
        /// <param name="capacity">A nova capacidade.</param>
        private void Reserve(ulong capacity)
        {
            var newVector = new T[capacity];
            if (this.count > 0)
            {
                Array.Copy(
                    this.items,
                    newVector,
                    (long)this.count);
            }

            this.items = newVector;
        }

        /// <summary>
        /// Processa a colecção.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        private void ProcessCollection(IEnumerable<T> collection)
        {
            var col = (ICollection<T>)collection;
            if (col == null)
            {
                using (var en = collection.GetEnumerator())
                {
                    if (en.MoveNext())
                    {
                        var firstItem = en.Current;
                        if (en.MoveNext())
                        {
                            this.items = new T[2];
                            this.items[0] = firstItem;
                            this.items[1] = en.Current;
                            this.count = 2UL;

                            var len = 2UL;
                            var i = 2UL;
                            while (en.MoveNext())
                            {
                                if (this.count == len)
                                {
                                    len <<= 1;
                                    this.Reserve(len);
                                }

                                this.items[i] = en.Current;
                                ++i;
                            }

                            this.count = i;
                        }
                        else
                        {
                            this.items = new T[1];
                            this.items[0] = firstItem;
                            this.count = 1UL;
                        }

                        this.Heapify(0, this.count - 1);
                    }
                }
            }
            else
            {
                var newCount = col.Count;
                if (newCount == 0)
                {
                    this.items = emptyArray;
                }
                else
                {
                    this.items = new T[newCount];
                    col.CopyTo(this.items, 0);
                    this.count = (ulong)newCount;
                    this.Heapify(0, this.count - 1);
                }
            }
        }

        /// <summary>
        /// Obtém o índice onde se encontra o nó ascendente dado
        /// o nó descendente.
        /// </summary>
        /// <param name="i">O índice do nó descendente.</param>
        /// <returns>O índice do nó ascendente.</returns>
        private ulong Parent(ulong i)
        {
            return (i - 1) >> 1;
        }

        /// <summary>
        /// Obtém o índice descendente que se encontra à esquerda.
        /// </summary>
        /// <param name="i">O índice do nó ascendente.</param>
        /// <returns>O índice o nó ascendente que se encontra à esquerda.</returns>
        private ulong Left(ulong i)
        {
            return (i << 1) + 1;
        }

        /// <summary>
        /// Obtém o índice do nó descendente que se encontra à direita.
        /// </summary>
        /// <param name="i">O índice do nó ascendente.</param>
        /// <returns>O índice do nó ascendente que se encontra à direita.</returns>
        private ulong Right(ulong i)
        {
            return (i + 1) << 1;
        }

        /// <summary>
        /// Remove o elemento especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        private void RemoveAt(ulong index)
        {
            --this.count;
            this.items[index] = this.items[this.count];
            if (index < this.count - 2 && this.count > 0)
            {
                this.SiftDown(index, this.count - 1);
            }

            this.items[this.count] = default(T);
        }

        /// <summary>
        /// Determina o índice de um item se este existir.
        /// </summary>
        /// <param name="value">O item.</param>
        /// <param name="index">O índice.</param>
        /// <returns>Verdadeiro caso o índice exista e falso caso contrário.</returns>
        private bool TryFindValue(
            T value,
            out ulong index)
        {
            index = 0;
            if (this.count < 8)
            {
                var cnt = this.count;
                for (var i = 0UL; i < cnt; ++i)
                {
                    var current = this.items[i];
                    if (this.comparer.Compare(
                        value,
                        current) == 0)
                    {
                        index = i;
                        return true;
                    }
                }
            }
            else
            {
                var cnt = this.count;
                var searchStack = new Stack<ulong>();
                searchStack.Push(0);
                while (searchStack.Count > 0)
                {
                    var top = searchStack.Pop();
                    var current = this.items[top];
                    var compareVal = this.comparer.Compare(
                        current,
                        value
                        );
                    if (compareVal == 0)
                    {
                        index = top;
                        return true;
                    }
                    else if (compareVal < 0)
                    {
                        var child = this.Left(top);
                        if (child < cnt)
                        {
                            searchStack.Push(child);
                            ++child;
                            if (child < cnt)
                            {
                                searchStack.Push(child);
                            }
                        }
                    }
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Define uma lista indexada por inteiros longos sobre
    /// a ordenação habitual.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da lista.</typeparam>
    public sealed class LongSystemArray<T> : ILongList<T>, IList<T>
    {
        /// <summary>
        /// A ordenação.
        /// </summary>
        private T[] array;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="LongSystemArray{T}"/>.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        public LongSystemArray(T[] array)
        {
            this.array = array;
        }

        /// <summary>
        /// Efectua a conversão implícita de uma ordenação habitual para uma
        /// ordenação indexada por inteiros longos.
        /// </summary>
        /// <param name="array">A ordenação a ser convertida.</param>
        /// <returns>O resultado da covnersão.</returns>
        public static implicit operator LongSystemArray<T>(T[] array)
        {
            if (array == null)
            {
                return null;
            }
            else
            {
                return new LongSystemArray<T>(array);
            }
        }

        /// <summary>
        /// Efectua a conversão implícita de uma ordenação indexada por inteiros longos
        /// para uma ordenação habitual.
        /// </summary>
        /// <param name="array">A ordenação indexada por inteiros longos.</param>
        /// <returns>A ordenação habitual.</returns>
        public static implicit operator T[](LongSystemArray<T> array)
        {
            if (null == (object)array)
            {
                return null;
            }
            else
            {
                return array.array;
            }
        }

        /// <summary>
        /// Determina a igualdade entre duas ordenações.
        /// </summary>
        /// <param name="first">A primeira ordenação.</param>
        /// <param name="second">A segunda ordenação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem iguais e falso caso contrário.</returns>
        public static bool operator ==(
            LongSystemArray<T> first,
            LongSystemArray<T> second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }
            else if (null == (object)first)
            {
                return false;
            }
            else if (null == (object)second)
            {
                return false;
            }
            else
            {
                return first.array == second.array;
            }
        }

        /// <summary>
        /// Determina a não igualdade entre duas ordenações.
        /// </summary>
        /// <param name="first">A primeira ordenação.</param>
        /// <param name="second">A segunda ordenação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem diferentes e falso caso contrário.</returns>
        public static bool operator !=(
            LongSystemArray<T> first,
            LongSystemArray<T> second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return false;
            }
            else if (null == (object)first)
            {
                return true;
            }
            else if (null == (object)second)
            {
                return true;
            }
            else
            {
                return first.array != second.array;
            }
        }

        /// <summary>
        /// Averigua a igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem iguais e falso caso contrário.</returns>
        public static bool operator ==(LongSystemArray<T> first, T[] second)
        {
            if (null == (object)first && null == second)
            {
                return true;
            }
            else if (null == (object)first)
            {
                return false;
            }
            else if (null == second)
            {
                return false;
            }
            else
            {
                return first.array == second;
            }
        }

        /// <summary>
        /// Averigua a não igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem diferentes e falso caso contrário.</returns>
        public static bool operator !=(LongSystemArray<T> first, T[] second)
        {
            if (null == (object)first && null == second)
            {
                return false;
            }
            else if (null == (object)first)
            {
                return true;
            }
            else if (null == second)
            {
                return true;
            }
            else
            {
                return first.array != second;
            }
        }

        /// <summary>
        /// Averigua a igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem iguais e falso caso contrário.</returns>
        public static bool operator ==(T[] first, LongSystemArray<T> second)
        {
            if (null == first && null == (object)second)
            {
                return true;
            }
            else if (null == first)
            {
                return false;
            }
            else if (null == (object)second)
            {
                return false;
            }
            else
            {
                return first == second.array;
            }
        }

        /// <summary>
        /// Averigua a não igualdade entre uma ordenação habitual e uma ordenação indexada por
        /// inteiros longos.
        /// </summary>
        /// <param name="first">O primeiro elemento da comparação.</param>
        /// <param name="second">O segundo elemento da comparação.</param>
        /// <returns>Verdadeiro se ambas as ordenações forem diferentes e falso caso contrário.</returns>
        public static bool operator !=(T[] first, LongSystemArray<T> second)
        {
            if (null == first && null == (object)second)
            {
                return false;
            }
            else if (null == first)
            {
                return true;
            }
            else if (null == (object)second)
            {
                return true;
            }
            else
            {
                return first != second.array;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[int index]
        {
            get
            {
                return this.array[index];
            }
            set
            {
                this.array[index] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[uint index]
        {
            get
            {
                return this.array[index];
            }
            set
            {
                this.array[index] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[long index]
        {
            get
            {
                return this.array[index];
            }
            set
            {
                this.array[index] = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[ulong index]
        {
            get
            {
                return this.array[index];
            }
            set
            {
                this.array[index] = value;
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public int Length
        {
            get
            {
                return this.array.Length;
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public uint UintLength
        {
            get
            {
                return (uint)this.array.Length;
            }
        }

        /// <summary>
        /// Obtém o tamanho longo do vector.
        /// </summary>
        public long LongLength
        {
            get
            {
                return this.array.LongLength;
            }
        }

        /// <summary>
        /// Obtém o tamanho longo do vector.
        /// </summary>
        public ulong ULongLength
        {
            get
            {
                return (ulong)this.array.LongLength;
            }
        }

        /// <summary>
        /// Obtém o número de elementos na lista.
        /// </summary>
        public int Count
        {
            get
            {
                return this.array.Length;
            }
        }

        /// <summary>
        /// Obtém o número de elementos na lista.
        /// </summary>
        public uint UintCount
        {
            get
            {
                return (uint)this.array.Length;
            }
        }

        /// <summary>
        /// Obtém o número de elementos na lista.
        /// </summary>
        public long LongCount
        {
            get
            {
                return this.array.LongLength;
            }
        }

        /// <summary>
        /// Obtém o número de elementos na lista.
        /// </summary>
        public ulong UlongCount
        {
            get
            {
                return (ulong)this.array.LongLength;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a lista é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            return Array.IndexOf(this.array, item);
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public long LongIndexOf(T item)
        {
            return Array.IndexOf(this.array, item);
        }

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <param name="item">O item a ser inserido.</param>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <param name="item">O item a ser inserido.</param>
        public void Insert(uint index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <param name="item">O item a ser inserido.</param>
        public void Insert(long index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o item na posição especificada.
        /// </summary>
        /// <param name="index">O índice da posição.</param>
        /// <param name="item">O item a ser inserido.</param>
        public void Insert(ulong index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o item da posição indicada.
        /// </summary>
        /// <param name="index">O índice da posição do item a ser removid.</param>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o item da posição indicada.
        /// </summary>
        /// <param name="index">O índice da posição do item a ser removid.</param>
        public void RemoveAt(uint index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o item da posição indicada.
        /// </summary>
        /// <param name="index">O índice da posição do item a ser removid.</param>
        public void RemoveAt(long index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o item da posição indicada.
        /// </summary>
        /// <param name="index">O índice da posição do item a ser removid.</param>
        public void RemoveAt(ulong index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Adiciona o item ao final da lista.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Limpa a lista.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        /// Retorna um valor que indica se o item se encontra na lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro se o item se encontrar na lista e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            return Array.IndexOf(this.array, item) != -1;
        }

        /// <summary>
        /// Remove o item da lista.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro se a remoção for bem-sucedida e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Copia os valores da lista para uma ordenação.
        /// </summary>
        /// <param name="array">A ordenação de destino.</param>
        /// <param name="arrayIndex">O índice de destino onde será iniciada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this.array, 0, array, arrayIndex, this.array.Length);
        }

        /// <summary>
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="dimensions">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        public void CopyTo(
            Array array,
            long[] dimensions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um enumerador para a lista.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)this.array).GetEnumerator();
        }

        /// <summary>
        /// Verifica se a ordenação corrente iguala o objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (object.ReferenceEquals(this.array, obj))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém o código confuso para o objecto.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            return this.array.GetHashCode();
        }

        /// <summary>
        /// Obtém a representação textual do objecto.
        /// </summary>
        /// <returns>A representação textual do objecto.</returns>
        public override string ToString()
        {
            return this.array.ToString();
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a lista.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Implmenta um vector cujo tamanho não se encontra limitado aos 2 GB.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas do vector.</typeparam>
    public sealed class GeneralLongArray<T> : ILongList<T>, IList<T>
    {
        /// <summary>
        /// Define uma matriz vazia a ser utilizada.
        /// </summary>
        private static readonly T[][][] emptyArray = new T[0][][];

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de objectos.
        /// </summary>
        private static uint mask;

        /// <summary>
        /// A potência na base 2 para o número máximo de itens que podem ser
        /// contidos num vector de sistema.
        /// </summary>
        private static int maxBinaryPower;

        /// <summary>
        /// A potência máxima para um objecto.
        /// </summary>
        private static int objMaxBinaryPower;

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de vectores.
        /// </summary>
        private static uint generalMask;

        /// <summary>
        /// Variável que indica se se irá avaliar a memória disponível
        /// em caso de instância.
        /// </summary>
        private bool assertMemory;

        /// <summary>
        /// Mantém os elementos.
        /// </summary>
        private T[][][] elements;

        /// <summary>
        /// Mantém o tamanho total do vector.
        /// </summary>
        private ulong length;

        /// <summary>
        /// Inicializa o tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        static GeneralLongArray()
        {
            var configSection = Utils.GetRuntimeConfiguration();
            if (configSection.GcAllowVeryLargeObjects.Enabled)
            {
                mask = 2147483647;
                maxBinaryPower = 31;
                objMaxBinaryPower = 31;
                generalMask = 2147483647;
            }
            else
            {
                generalMask = 67108863;
                objMaxBinaryPower = 26;
                var objType = typeof(T);
                if (objType == typeof(byte) ||
                    objType == typeof(sbyte) ||
                    objType == typeof(bool))
                {
                    maxBinaryPower = 30;
                    mask = 1073741823;
                }
                else if (objType == typeof(char)
                    || objType == typeof(short)
                    || objType == typeof(ushort))
                {
                    maxBinaryPower = 29;
                    mask = 536870911;
                }
                else if (objType == typeof(int)
                    || objType == typeof(uint)
                    || objType == typeof(double))
                {
                    maxBinaryPower = 28;
                    mask = 268435455;
                }
                else if (objType == typeof(long)
                    || objType == typeof(ulong)
                    || objType == typeof(double))
                {
                    maxBinaryPower = 27;
                    mask = 134217727;
                }
                else
                {
                    maxBinaryPower = 26;
                    mask = generalMask;
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(bool assertMemory = true)
        {
            this.elements = emptyArray;
            this.length = 0;
            this.assertMemory = assertMemory;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma lista geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a ordenação de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(int length, bool assertMemory = true)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)length);
                this.Instantiate((ulong)length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(uint length, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory(length);
            this.Instantiate(length);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(long length, bool assertMemory = true)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)length);
                this.Instantiate((ulong)length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="length">O tamanho do vector.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongArray(ulong length, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory((ulong)length);
            this.Instantiate((ulong)length);
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[long index]
        {
            get
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[ulong index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[uint index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public int Count
        {
            get
            {
                if (this.length > int.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use LongCount instaed.");
                }
                else
                {
                    return (int)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public uint UintCount
        {
            get
            {
                if (this.length > uint.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use LongCount instaed.");
                }
                else
                {
                    return (uint)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public long LongCount
        {
            get
            {
                if (this.length > long.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (long)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public ulong UlongCount
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #region Propriedades internas

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo das entradas da colecção.
        /// </summary>
        internal static int MaxBinaryPower
        {
            get
            {
                return maxBinaryPower;
            }
            set
            {
                maxBinaryPower = value;
                mask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo dos das entradas
        /// da colecção.
        /// </summary>
        internal static uint Mask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo geral.
        /// </summary>
        internal static int ObjMaxBinaryPower
        {
            get
            {
                return objMaxBinaryPower;
            }
            set
            {
                objMaxBinaryPower = value;
                generalMask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo geral.
        /// </summary>
        internal static uint GeneralMask
        {
            get
            {
                return generalMask;

            }
        }

        #endregion Propriedades internas

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public long LongIndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > long.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use TryGetIndexOf function instead.");
                }
                else
                {
                    return (long)result;
                }
            }
            else
            {
                return -1L;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > int.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use LongIndexOf function instead.");
                }
                else
                {
                    return (int)result;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        public bool TryGetIndexOf(T item, out ulong index)
        {
            return this.TryGetIndexOfAux(item, out index);
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(long index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(ulong index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(uint index, T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(long index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(ulong index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(uint index)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Elimina todos os itens da colecção.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Retorna um valor que indica se o item especificado se encontra na colecção.
        /// </summary>
        /// <param name="item">O item a procurar.</param>
        /// <returns>Verdadeiro caso o item exista no vector e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            var firstLength = this.elements.Length;
            if (item == null)
            {
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (innerCurr[k] == null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                var comparer = EqualityComparer<T>.Default;
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (comparer.Equals(innerCurr[k], item))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Remove um item da colecção.
        /// </summary>
        /// <param name="item">O item a ser removido.</param>
        /// <returns>Verdadeiro se o item for removido com sucesso e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection was of a fixed size.");
        }

        /// <summary>
        /// Copia o conteúdo da colecção para um vector de sistema.
        /// </summary>
        /// <param name="array">O vector de sistema de destino.</param>
        /// <param name="arrayIndex">O índice a partir do qual é efectuada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var firstLength = this.elements.Length;
            var currentIndex = arrayIndex;
            for (int i = 0; i < firstLength; ++i)
            {
                var curr = this.elements[i];
                var secondLength = curr.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    var elem = curr[j];
                    var thirdLength = elem.Length;
                    Array.Copy(elem, 0, array, currentIndex, thirdLength);
                    currentIndex += thirdLength;
                }
            }
        }

        /// <summary>
        /// Efectua a cópia da ordenação longa corrente para outra ordenação longa.
        /// </summary>
        /// <param name="array">A ordenação longa a reter a cópia.</param>
        /// <param name="index">O índice a partir do qual a cópia será efectuada.</param>
        public void CopyTo(
            GeneralLongArray<T> array,
            ulong index)
        {
            if(index < 0 || index > array.UlongCount)
            {
                throw new IndexOutOfRangeException("Index must be greater than zero or less than the size of the collection.");
            }
            else
            {
                var sum = index + this.length;
                if(sum > array.UlongCount)
                {
                    throw new ArgumentException("Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                }
                else if(sum != 0)
                {
                    // Determinação das coordenadas do índice no vector longo
                    var thirdDim = index & mask;
                    var secondDim = 0UL;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim != 0)
                    {
                        secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                    }

                    var firstIndex = (long)thirdDim;
                    var secondIndex = (long)secondDim;
                    var thirdIndex = (long)firstDim;

                    // Os vectores estão alinhados
                    if(firstIndex == 0)
                    {
                        // Cópia completamente alinhada
                        var firstLength = this.elements.Length;
                        var prevFirstLength = firstLength - 1;
                        var size = mask + 1;
                        var genSize = generalMask + 1;
                        for (var i = 0L; i < prevFirstLength; ++i)
                        {
                            var curr = this.elements[i];
                            for (var j = 0L; j < genSize; ++j)
                            {
                                var outerElem = array.elements[thirdIndex][secondIndex];
                                var elem = curr[j];
                                Array.Copy(elem, 0, outerElem, 0, size);
                                ++secondIndex;
                                if(secondIndex == genSize)
                                {
                                    ++thirdIndex;
                                    secondIndex = 0;
                                }
                            }
                        }

                        // Cópia da segunda dimensão
                        var lastCurr = this.elements[prevFirstLength];
                        var secondLength = lastCurr.LongLength;
                        var prevSecondLength = secondLength - 1;
                        for(var j =0L;j<prevSecondLength; ++j)
                        {
                            var outerElem = array.elements[thirdIndex][secondIndex];
                            var elem = lastCurr[j];
                            Array.Copy(elem, 0, outerElem, 0, size);
                            ++secondIndex;
                            if(secondIndex == genSize)
                            {
                                ++thirdIndex;
                                secondIndex = 0;
                            }
                        }

                        // Cópia final
                        var lastOuterElem = array.elements[thirdIndex][secondIndex];
                        var lastCopyElem = lastCurr[prevSecondLength];
                        Array.Copy(lastCopyElem, 0, lastOuterElem, 0, lastCopyElem.LongLength);
                    }
                    else
                    {
                        // Não há alinhamento nos vectores
                        var firstLength = this.elements.Length;
                        var prevFirstLength = firstLength - 1;
                        var size = mask + 1;
                        var copyLength = size - firstIndex;
                        var genSize = generalMask + 1;
                        var prevMaxBinPower = generalMask;
                        for (var i = 0L; i < prevFirstLength; ++i)
                        {
                            var curr = this.elements[i];
                            for (var j = 0L; j < prevMaxBinPower; ++j)
                            {
                                var outerElem = array.elements[thirdIndex][secondIndex];
                                var elem = curr[j];
                                Array.Copy(elem, 0, outerElem, firstIndex, copyLength);
                                ++secondIndex;
                                if(secondIndex == genSize)
                                {
                                    ++thirdIndex;
                                    secondIndex = 0;
                                }

                                outerElem = array.elements[thirdIndex][secondIndex];
                                Array.Copy(elem, copyLength, outerElem, 0, firstIndex);
                            }

                            var currElem = curr[prevMaxBinPower];
                            var currOuterElem = array.elements[thirdIndex][secondIndex];
                            Array.Copy(
                                currElem,
                                0,
                                currOuterElem,
                                firstIndex,
                                copyLength);
                            ++secondIndex;
                            if(secondIndex == genSize)
                            {
                                ++thirdIndex;
                                secondIndex = 0;
                            }

                            currOuterElem = array.elements[thirdIndex][secondIndex];
                            Array.Copy(
                                currElem,
                                copyLength,
                                currOuterElem,
                                0,
                                firstIndex);
                        }

                        // Cópia da segunda dimensão
                        var lastCurr = this.elements[prevFirstLength];
                        var secondLength = lastCurr.LongLength;
                        var prevSecondLength = secondLength - 1;
                        for (var j = 0L; j < prevSecondLength; ++j)
                        {
                            var outerElem = array.elements[thirdIndex][secondIndex];
                            var elem = lastCurr[j];
                            Array.Copy(elem, 0, outerElem, firstIndex, copyLength);
                            ++secondIndex;
                            if(secondIndex == genSize)
                            {
                                ++thirdIndex;
                                secondIndex = 0;
                            }

                            outerElem = array.elements[thirdIndex][secondIndex];
                            Array.Copy(elem, copyLength, outerElem, 0, firstIndex);
                        }

                        // Cópia final
                        var lastOuterElem = array.elements[thirdIndex][secondIndex];
                        var lastCopyElem = lastCurr[prevSecondLength];
                        var lastLength = lastCopyElem.LongLength;
                        if (firstIndex + lastLength - 1 < size)
                        {
                            Array.Copy(lastCopyElem, 0, lastOuterElem, firstIndex, lastLength);
                        }
                        else
                        {
                            Array.Copy(lastCopyElem, 0, lastOuterElem, firstIndex, copyLength);

                            ++secondIndex;
                            if (secondIndex == genSize)
                            {
                                ++thirdIndex;
                                secondIndex = 0;
                            }

                            lastOuterElem = array.elements[thirdIndex][secondIndex];
                            Array.Copy(lastCopyElem, copyLength, lastOuterElem, 0, lastLength - copyLength);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="dimensions">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        public void CopyTo(
            Array array,
            long[] dimensions)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }
            else
            {
                var rank = dimensions.LongLength;
                if (rank == 0)
                {
                    if (this.length != 0)
                    {
                        throw new ArgumentException(
                            "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                    }
                }
                else if (rank == 1)
                {
                    var firstLength = this.elements.Length;
                    var currentIndex = dimensions[0];
                    for (int i = 0; i < firstLength; ++i)
                    {
                        var curr = this.elements[i];
                        var secondLength = curr.Length;
                        for (int j = 0; j < secondLength; ++j)
                        {
                            var elem = curr[j];
                            var thirdLength = elem.Length;
                            Array.Copy(elem, 0, array, currentIndex, thirdLength);
                            currentIndex += thirdLength;
                        }
                    }
                }
                else
                {
                    this.AssertArrayStructure(array, dimensions);
                    var indexes = new long[rank];
                    var arrays = new Array[rank];
                    Array.Copy(dimensions, indexes, rank);
                    var currentArray = array;
                    arrays[0] = currentArray;
                    for (var i = 1L; i < rank; ++i)
                    {
                        var innerArray = (Array)currentArray.GetValue(indexes[i - 1]);
                        arrays[i] = innerArray;
                        currentArray = innerArray;
                    }

                    var elementsLength = this.elements.LongLength;
                    for (var i = 0L; i < elementsLength; ++i)
                    {
                        var currElem = this.elements[i];
                        var currElemLength = currElem.LongLength;
                        for (var j = 0L; j < currElemLength; ++j)
                        {
                            var current = currElem[j];
                            this.CopyCurrentArray(
                                current,
                                arrays,
                                indexes);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var firstLength = this.elements.Length;
            for (int i = 0; i < firstLength; ++i)
            {
                var curr = this.elements[i];
                var secondLength = curr.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    var elem = curr[j];
                    var thirdLength = elem.Length;
                    for (int k = 0; k < thirdLength; ++k)
                    {
                        yield return elem[k];
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #region Funções internas para testes

        /// <summary>
        /// Verifica se o tamanho da instância do vector está de acordo com
        /// a capacidade estabelecida.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso o tamanho do vector esteja de acordo com a capacidade
        /// estabelecida e falso caso contrário.
        /// </returns>
        internal bool AssertSizes()
        {
            var computedLength = 0;
            for (int i = 0; i < this.elements.Length; ++i)
            {
                var current = this.elements[i];
                for (int j = 0; j < current.Length; ++j)
                {
                    computedLength += current[j].Length;
                }
            }

            return this.length == (ulong)computedLength;
        }

        #endregion Funções internas para testes

        #region Funções privadas

        /// <summary>
        /// Verifica a validade da memória visível disponibilizada pelo sistema operativo.
        /// </summary>
        /// <param name="size">O tamanho da colecção.</param>
        private void AssertVisibleMemory(ulong size)
        {
            if (this.assertMemory)
            {
                var memory = Utils.GetMemoryInfo().TotalVisibleMemorySize;
                var factor = 64UL;
                var objType = typeof(T);
                if (objType == typeof(byte) ||
                        objType == typeof(sbyte) ||
                        objType == typeof(bool))
                {
                    factor = 1024;
                }
                else if (objType == typeof(char)
                    || objType == typeof(short)
                    || objType == typeof(ushort))
                {
                    factor = 512;
                }
                else if (objType == typeof(int)
                    || objType == typeof(uint)
                    || objType == typeof(double))
                {
                    factor = 256;
                }
                else if (objType == typeof(long)
                    || objType == typeof(ulong)
                    || objType == typeof(double))
                {
                    factor = 128;
                }

                var itemsNumber = memory * factor;
                if (itemsNumber < size)
                {
                    throw new OutOfMemoryException("There is no visbile memory available to proceed.");
                }
            }
        }

        /// <summary>
        /// Instancia a estrutura interna.
        /// </summary>
        /// <param name="length">O tamanho do vector.</param>
        private void Instantiate(ulong length)
        {
            this.length = length;
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (length == 0)
            {
                this.elements = new T[0][][];
            }
            else
            {
                var thirdDim = length & mask;
                var firstDim = length >> maxBinaryPower;
                if (firstDim == 0)
                {
                    var elems = new T[1][][];
                    var innerElems = new T[1][];
                    innerElems[0] = new T[thirdDim];
                    elems[0] = innerElems;
                    this.elements = elems;
                }
                else
                {
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    if (thirdDim == 0)
                    {
                        if (secondDim == 0)
                        {
                            var elems = new T[firstDim][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }
                        }
                        else
                        {
                            var elems = new T[firstDim + 1][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }

                            var innerElemOut = new T[secondDim][];
                            elems[firstDim] = innerElemOut;
                            for (var j = 0UL; j < secondDim; ++j)
                            {
                                innerElemOut[j] = new T[size];
                            }
                        }
                    }
                    else if (secondDim == generalSize)
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[1][];
                        elems[firstDim] = innerElemOut;
                        innerElemOut[0] = new T[thirdDim];
                    }
                    else
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[secondDim + 1][];
                        elems[firstDim] = innerElemOut;
                        for (var i = 0UL; i < secondDim; ++i)
                        {
                            innerElemOut[i] = new T[size];
                        }

                        innerElemOut[secondDim] = new T[thirdDim];
                    }
                }
            }
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        private bool TryGetIndexOfAux(T item, out ulong index)
        {
            var firstLength = (ulong)this.elements.Length;
            var generalSize = generalMask + 1;
            for (var i = 0UL; i < firstLength; ++i)
            {
                var secondElem = this.elements[i];
                var secondLength = (ulong)secondElem.Length;
                for (var j = 0UL; j < secondLength; ++j)
                {
                    var innerIndex = Array.IndexOf(secondElem, item);
                    if (innerIndex > -1)
                    {
                        checked
                        {
                            index = (i * generalSize + j) * (mask + 1) + (ulong)innerIndex;
                            return true;
                        }
                    }
                }
            }

            index = 0;
            return false;
        }

        /// <summary>
        /// Verifica a validade da estrutura da ordenação.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        /// <param name="dimensions">A descrição das dimensões.</param>
        private void AssertArrayStructure(
            Array array,
            long[] dimensions)
        {
            var rank = dimensions.Length;
            var objType = array.GetType();
            for (var i = 0; i < rank; ++i)
            {
                if (objType.IsArray && objType.GetArrayRank() == 1)
                {
                    objType = objType.GetElementType();
                }
                else
                {
                    throw new UtilitiesException(string.Format(
                        "Array must be rank one at level {0}.",
                        i));
                }
            }

            if (!objType.IsAssignableFrom(typeof(T)))
            {
                throw new ArrayTypeMismatchException("Source array type cannot be assigned to destination array type.");
            }
        }

        /// <summary>
        /// Realiza a cópia de uma ordenação para um conjunto de ordenações.
        /// </summary>
        /// <param name="array">A ordenação de partida.</param>
        /// <param name="arrays">A definição das ordenações de destino.</param>
        /// <param name="indexes">
        /// Os índices que definem o estado das ordenações de destino.
        /// </param>
        private void CopyCurrentArray(
            T[] array,
            Array[] arrays,
            long[] indexes)
        {
            var rank = arrays.LongLength;
            var indexPointer = rank - 1;
            var currentIndex = indexes[indexPointer];
            var currentArray = arrays[indexPointer];
            var arrayLength = array.LongLength;
            var arrayIndex = 0L;
            while (arrayLength > 0)
            {
                var difference = currentArray.LongLength - currentIndex;
                if (difference < arrayLength)
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        difference);
                    arrayLength -= difference;
                    arrayIndex += difference;

                    // Actualiza o estado dos índices
                    var state = true;
                    while (state)
                    {
                        --indexPointer;
                        if (indexPointer < 0)
                        {
                            throw new ArgumentException(
                                "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                        }
                        else
                        {
                            currentIndex = indexes[indexPointer];
                            currentArray = arrays[indexPointer];
                            var currentArrayLength = currentArray.LongLength;
                            ++currentIndex;
                            if (currentIndex < currentArrayLength)
                            {
                                indexes[indexPointer] = currentIndex;
                                ++indexPointer;
                                var innerArray = (Array)currentArray.GetValue(currentIndex);
                                arrays[indexPointer] = innerArray;
                                indexes[indexPointer] = 0L;
                                currentArray = innerArray;
                                ++indexPointer;
                                for (; indexPointer < rank; ++indexPointer)
                                {
                                    innerArray = (Array)currentArray.GetValue(0);
                                    arrays[indexPointer] = innerArray;
                                    indexes[indexPointer] = 0L;
                                    currentArray = innerArray;
                                }

                                --indexPointer;
                                currentIndex = 0;
                                state = false;
                            }
                        }
                    }
                }
                else
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        arrayLength);
                    currentIndex += arrayLength;
                    indexes[indexPointer] = currentIndex;

                    arrayLength = 0;
                }
            }
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa uma lista cujo tamanho não se encontra limitado aos 2 GB.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da lista.</typeparam>
    public class GeneralLongList<T> : ILongList<T>, IList<T>
    {
        #region Campos estáticos privados

        /// <summary>
        /// Mantém uma instância do vector vazio.
        /// </summary>
        private static readonly T[][][] emptyArray = new T[0][][];

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de objectos.
        /// </summary>
        private static uint mask;

        /// <summary>
        /// A potência na base 2 para o número máximo de itens que podem ser
        /// contidos num vector de sistema.
        /// </summary>
        private static int maxBinaryPower;

        /// <summary>
        /// A potência máxima para um objecto.
        /// </summary>
        private static int objMaxBinaryPower;

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de vectores.
        /// </summary>
        private static uint generalMask;

        /// <summary>
        /// Estabelece a capacidade estabelecida por defeito aquando da adição do primeiro elemento
        /// a uma lista cuja capacidade inicial é nula.
        /// </summary>
        private static ulong defaultCapacity = 1024;

        #endregion Campos estáticos privados

        #region Campos privados

        /// <summary>
        /// Variável que indica se se irá avaliar a memória disponível
        /// em caso de instância.
        /// </summary>
        private bool assertMemory = true;

        /// <summary>
        /// Mantém os elementos.
        /// </summary>
        private T[][][] elements;

        /// <summary>
        /// Mantém o tamanho total do vector.
        /// </summary>
        private ulong length;

        /// <summary>
        /// A primeira dimensão do comprimento da lista.
        /// </summary>
        private ulong firstDimLength;

        /// <summary>
        /// A segunda dimensão do comprimento da lista.
        /// </summary>
        private ulong secondDimLength;

        /// <summary>
        /// A terceira dimensão do comprimento da lista.
        /// </summary>
        private ulong thirdDimLength;

        /// <summary>
        /// Mantém o valor da capacidade do vector. 
        /// </summary>
        private ulong capacity;

        #endregion Campos privados

        /// <summary>
        /// Inicializa o tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        static GeneralLongList()
        {
            var configSection = Utils.GetRuntimeConfiguration();
            if (configSection.GcAllowVeryLargeObjects.Enabled)
            {
                mask = 2147483647;
                maxBinaryPower = 31;
                objMaxBinaryPower = 31;
                generalMask = 2147483647;
            }
            else
            {
                generalMask = 67108863;
                objMaxBinaryPower = 26;
                var objType = typeof(T);
                if (objType == typeof(byte) ||
                    objType == typeof(sbyte) ||
                    objType == typeof(bool))
                {
                    maxBinaryPower = 30;
                    mask = 1073741823;
                }
                else if (objType == typeof(char)
                    || objType == typeof(short)
                    || objType == typeof(ushort))
                {
                    maxBinaryPower = 29;
                    mask = 536870911;
                }
                else if (objType == typeof(int)
                    || objType == typeof(uint)
                    || objType == typeof(double))
                {
                    maxBinaryPower = 28;
                    mask = 268435455;
                }
                else if (objType == typeof(long)
                    || objType == typeof(ulong)
                    || objType == typeof(double))
                {
                    maxBinaryPower = 27;
                    mask = 134217727;
                }
                else
                {
                    maxBinaryPower = 26;
                    mask = generalMask;
                }
            }
        }

        #region Construtores públicos

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(bool assertMemory = true)
        {
            this.length = 0;
            this.capacity = 0;
            this.elements = emptyArray;
            this.assertMemory = assertMemory;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(int capacity, bool assertMemory = true)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)capacity);
                this.Instantiate((ulong)capacity);
                this.length = 0;
                this.firstDimLength = 0;
                this.secondDimLength = 0;
                this.thirdDimLength = 0;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(uint capacity, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory(capacity);
            this.Instantiate(capacity);
            this.length = 0;
            this.firstDimLength = 0;
            this.secondDimLength = 0;
            this.thirdDimLength = 0;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(long capacity, bool assertMemory = true)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }
            else
            {
                this.assertMemory = assertMemory;
                this.AssertVisibleMemory((ulong)capacity);
                this.Instantiate((ulong)capacity);
                this.length = 0;
                this.firstDimLength = 0;
                this.secondDimLength = 0;
                this.thirdDimLength = 0;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// </remarks>
        /// <param name="capacity">O valor da capacidade da lista.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(ulong capacity, bool assertMemory = true)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory(capacity);
            this.Instantiate(capacity);
            this.length = 0;
            this.firstDimLength = 0;
            this.secondDimLength = 0;
            this.thirdDimLength = 0;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralLongList{T}"/>.
        /// </summary>
        /// <remarks>
        /// A instanciação de uma ordenação geral suficientemente grande pode causar problemas de memória.
        /// É conveniente avaliar a quantidade de memória disponível visível para o utilizador antes
        /// de proceder à instanciação de listas muito grandes. Se o parâmetro <see cref="assertMemory"/> estiver
        /// activo, esta validação será efectuada internamente.
        /// 
        /// A lista criada será populada com os elementos contidos na colecção. De modo a manter-se a compatibilidade
        /// com o CLR (Common Language Runtime), caso seja passada um objecto do tipo <see cref="ICollection{T}"/>,
        /// a respectiva cópia será realizada por intermédio da chamada à função de cópia definida. Assim, caso o número
        /// de elementos na colecção proporcionada exceda o máximo permitido para um inteiro, a cópia resultará numa excepção
        /// ou não será bem sucedida.
        /// </remarks>
        /// <param name="collection">A colecção.</param>
        /// <param name="assertMemory">
        /// Valor que indica se é necessário verficiar se existe memória
        /// suficiente para continuar a instanciação.
        /// </param>
        public GeneralLongList(IEnumerable<T> collection, bool assertMemory = true)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            else
            {
                this.assertMemory = assertMemory;
                var longCollection = collection as ILongCollection<T>;
                if (longCollection == null)
                {
                    var intCollection = collection as ICollection<T>;
                    if (intCollection == null)
                    {
                        var enumerator = collection.GetEnumerator();
                        this.length = 0;
                        this.elements = emptyArray;
                        while (enumerator.MoveNext())
                        {
                            this.Add(enumerator.Current);
                        }
                    }
                    else
                    {
                        var count = intCollection.Count;
                        if (count == 0)
                        {
                            this.length = 0;
                            this.firstDimLength = 0;
                            this.secondDimLength = 0;
                            this.thirdDimLength = 0;
                            this.elements = emptyArray;
                        }
                        else
                        {
                            var innerMostArray = new T[count];
                            intCollection.CopyTo(innerMostArray, 0);
                            this.length = (ulong)count;
                            var sizes = this.GetSizes(this.length);
                            this.firstDimLength = sizes.Item1;
                            this.secondDimLength = sizes.Item2;
                            this.thirdDimLength = sizes.Item3;
                        }
                    }
                }
                else
                {
                    var count = longCollection.UlongCount;
                    this.Instantiate(count);
                    longCollection.CopyTo(
                        this.elements,
                        new[] { 0L, 0L, 0L });
                    this.length = count;
                    var sizes = this.GetSizes(this.length);
                    this.firstDimLength = sizes.Item1;
                    this.secondDimLength = sizes.Item2;
                    this.thirdDimLength = sizes.Item3;
                }
            }
        }

        #endregion Construtores públicos

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[long index]
        {
            get
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[ulong index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || (ulong)index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= 27;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public T this[uint index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        return this.elements[0][0][thirdDim];
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= objMaxBinaryPower;
                        return this.elements[firstDim][secondDim][thirdDim];
                    }
                }
            }
            set
            {
                if (index < 0 || index >= this.length)
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array.");
                }
                else
                {
                    var thirdDim = index & mask;
                    var firstDim = index >> maxBinaryPower;
                    if (firstDim == 0)
                    {
                        this.elements[0][0][thirdDim] = value;
                    }
                    else
                    {
                        var secondDim = firstDim & generalMask;
                        firstDim >>= 27;
                        this.elements[firstDim][secondDim][thirdDim] = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public int Count
        {
            get
            {
                if (this.length > int.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use LongCount instaed.");
                }
                else
                {
                    return (int)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public uint UintCount
        {
            get
            {
                if (this.length > uint.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (uint)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public long LongCount
        {
            get
            {
                if (this.length > long.MaxValue)
                {
                    throw new CollectionsException("The length of vector is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (long)this.length;
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho do vector.
        /// </summary>
        public ulong UlongCount
        {
            get
            {
                return this.length;
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public int Capacity
        {
            get
            {
                if (this.capacity > int.MaxValue)
                {
                    throw new CollectionsException("The capacity of the list is too big. Please use LongCapacity instaed.");
                }
                else
                {
                    return (int)this.capacity;
                }
            }
            set
            {
                var innerValue = (ulong)value;
                if (value < 0)
                {
                    innerValue = 0;
                }

                if (innerValue < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (innerValue > this.capacity)
                {
                    this.AssertVisibleMemory(innerValue);
                    this.IncreaseCapacityTo(innerValue);
                }
                else if (innerValue < this.capacity)
                {
                    this.DecreaseCapcityTo(innerValue);
                }
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public uint UintCapacity
        {
            get
            {
                if (this.capacity > uint.MaxValue)
                {
                    throw new CollectionsException("The capacity of the list is too big. Please use LongCapacity instaed.");
                }
                else
                {
                    return (uint)this.capacity;
                }
            }
            set
            {
                if (value < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (value > this.capacity)
                {
                    this.AssertVisibleMemory(value);
                    this.IncreaseCapacityTo(value);
                }
                else if (value < this.capacity)
                {
                    this.DecreaseCapcityTo(value);
                }
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public long LongCapacity
        {
            get
            {
                if (this.capacity > long.MaxValue)
                {
                    throw new CollectionsException("The capacity of the list is too big. Please use UlongCapacity instaed.");
                }
                else
                {
                    return (long)this.capacity;
                }
            }
            set
            {
                var innerValue = (ulong)value;
                if (value < 0)
                {
                    innerValue = 0;
                }

                if (innerValue < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (innerValue > this.capacity)
                {
                    this.AssertVisibleMemory(innerValue);
                    this.IncreaseCapacityTo(innerValue);
                }
                else if (innerValue < this.capacity)
                {
                    this.DecreaseCapcityTo(innerValue);
                }
            }
        }

        /// <summary>
        /// Obtém o valor da capacidade da lista.
        /// </summary>
        public ulong UlongCapacity
        {
            get
            {
                return this.capacity;
            }
            set
            {
                if (value < this.length)
                {
                    throw new CollectionsException("The provided capacity is too small.");
                }
                else if (value > this.capacity)
                {
                    this.AssertVisibleMemory(value);
                    this.IncreaseCapacityTo(value);
                }
                else if (value < this.capacity)
                {
                    this.DecreaseCapcityTo(value);
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é apenas de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #region Propriedades internas

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo das entradas da colecção.
        /// </summary>
        internal static int MaxBinaryPower
        {
            get
            {
                return maxBinaryPower;
            }
            set
            {
                maxBinaryPower = value;
                mask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo dos das entradas
        /// da colecção.
        /// </summary>
        internal static uint Mask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo geral.
        /// </summary>
        internal static int ObjMaxBinaryPower
        {
            get
            {
                return objMaxBinaryPower;
            }
            set
            {
                objMaxBinaryPower = value;
                generalMask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo geral.
        /// </summary>
        internal static uint GeneralMask
        {
            get
            {
                return generalMask;

            }
        }

        #endregion Propriedades internas

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public int IndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > int.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use LongIndexOf function.");
                }
                else
                {
                    return (int)result;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Obtém o índice da primeira posição do item.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>O índice se o item existir e -1 caso contrário.</returns>
        public long LongIndexOf(T item)
        {
            var result = default(ulong);
            if (this.TryGetIndexOfAux(item, out result))
            {
                if (result > long.MaxValue)
                {
                    throw new CollectionsException("The index value is too big. Please use TryGetIndexOf function instead.");
                }
                else
                {
                    return (long)result;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        public bool TryGetIndexOf(T item, out ulong index)
        {
            return this.TryGetIndexOfAux(item, out index);
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(int index, T item)
        {
            if (index < 0 || (ulong)index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert((ulong)index, item);
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(uint index, T item)
        {
            if (index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert(index, item);
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(long index, T item)
        {
            if (index < 0 || (ulong)index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert((ulong)index, item);
            }
        }

        /// <summary>
        /// Insere o objecto na posição explicitada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <param name="item">O objecto a ser inserido.</param>
        public void Insert(ulong index, T item)
        {
            if (index > this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                this.InnerInsert(index, item);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || (ulong)index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt((ulong)index);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(uint index)
        {
            if (index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt(index);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(long index)
        {
            if (index < 0 || (ulong)index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt((ulong)index);
            }
        }

        /// <summary>
        /// Remove o objecto que se encontra na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(ulong index)
        {
            if (index >= this.length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                this.InnerRemoveAt(index);
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O item a ser adicionado.</param>
        public void Add(T item)
        {
            this.InnerInsert(
                this.length,
                item);
        }

        /// <summary>
        /// Elimina todos os itens da colecção.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retorna um valor que indica se o item especificado se encontra na colecção.
        /// </summary>
        /// <param name="item">O item a procurar.</param>
        /// <returns>Verdadeiro caso o item exista no vector e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            return this.InnerContains(item, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Retorna um valor que indica se o item especificado se encontra na colecção.
        /// </summary>
        /// <param name="item">O item a procurar.</param>
        /// <param name="comparer">O comparador de itens.</param>
        /// <returns>Verdadeiro caso o item exista no vector e falso caso contrário.</returns>
        public bool Contains(T item, EqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                return this.InnerContains(item, comparer);
            }
        }

        /// <summary>
        /// Copia o conteúdo da colecção para um vector de sistema.
        /// </summary>
        /// <param name="array">O vector de sistema de destino.</param>
        /// <param name="arrayIndex">O índice a partir do qual é efectuada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            var firstLength = this.firstDimLength;
            var currentIndex = arrayIndex;
            for (var i = 0UL; i < firstLength; ++i)
            {
                var curr = this.elements[i];
                var secondLength = curr.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    var elem = curr[j];
                    var thirdLength = elem.Length;
                    Array.Copy(elem, 0, array, currentIndex, thirdLength);
                    currentIndex += thirdLength;
                }
            }

            var outerSecondlength = this.secondDimLength;
            var outerCurr = this.elements[firstLength];
            for (var i = 0UL; i < outerSecondlength; ++i)
            {
                var thirdCurr = outerCurr[i];
                var thirdLength = thirdCurr.Length;
                Array.Copy(thirdCurr, 0, array, currentIndex, thirdLength);
                currentIndex += thirdLength;
            }

            if ((long)outerSecondlength < outerCurr.LongLength)
            {
                var outerThirdCurr = outerCurr[outerSecondlength];
                Array.Copy(outerThirdCurr, 0, array, currentIndex, (long)this.thirdDimLength);
            }
        }

        /// <summary>
        /// Copia o conteúdo da colecção para uma ordenação geral em forma de matriz.
        /// </summary>
        /// <param name="array">A ordenação geral.</param>
        /// <param name="dimensions">
        /// Os valores que identificam a entrada da matriz onde a cópia será iniciada.
        /// </param>
        public void CopyTo(
            Array array,
            long[] dimensions)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (dimensions == null)
            {
                throw new ArgumentNullException("dimensions");
            }
            else
            {
                var rank = dimensions.LongLength;
                if (rank == 0)
                {
                    if (this.length != 0)
                    {
                        throw new ArgumentException(
                            "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                    }
                }
                else if (rank == 1)
                {
                    var firstLength = this.elements.Length;
                    var currentIndex = dimensions[0];
                    for (int i = 0; i < firstLength; ++i)
                    {
                        var curr = this.elements[i];
                        var secondLength = curr.Length;
                        for (int j = 0; j < secondLength; ++j)
                        {
                            var elem = curr[j];
                            var thirdLength = elem.Length;
                            Array.Copy(elem, 0, array, currentIndex, thirdLength);
                            currentIndex += thirdLength;
                        }
                    }
                }
                else
                {
                    var size = mask + 1;
                    var generalSize = generalMask + 1;
                    this.AssertArrayStructure(array, dimensions);
                    var indexes = new long[rank];
                    var arrays = new Array[rank];
                    Array.Copy(dimensions, indexes, rank);
                    var currentArray = array;
                    arrays[0] = currentArray;
                    for (var i = 1L; i < rank; ++i)
                    {
                        var innerArray = (Array)currentArray.GetValue(indexes[i - 1]);
                        arrays[i] = innerArray;
                        currentArray = innerArray;
                    }

                    // Rever
                    var elementsLength = this.firstDimLength;
                    if (elementsLength > 0)
                    {
                        --elementsLength;
                        for (var i = 0UL; i < elementsLength; ++i)
                        {
                            var currElem = this.elements[i];
                            for (var j = 0L; j < generalSize; ++j)
                            {
                                var current = currElem[j];
                                this.CopyCurrentArray(
                                    current,
                                    size,
                                    arrays,
                                    indexes);
                            }
                        }
                    }

                    var outerCurrElemn = this.elements[elementsLength];
                    elementsLength = this.secondDimLength;
                    if (elementsLength > 0)
                    {
                        --elementsLength;
                        for (var i = 0UL; i < elementsLength; ++i)
                        {
                            var innerCurrent = outerCurrElemn[i];
                            this.CopyCurrentArray(
                                innerCurrent,
                                size,
                                arrays,
                                indexes);
                        }
                    }

                    var lastArray = outerCurrElemn[elementsLength];
                    this.CopyCurrentArray(
                        lastArray,
                        (long)this.thirdDimLength,
                        arrays,
                        indexes);
                }
            }
        }

        /// <summary>
        /// Remove um item da colecção.
        /// </summary>
        /// <param name="item">O item a ser removido.</param>
        /// <returns>Verdadeiro se o item for removido com sucesso e falso caso contrário.</returns>
        public bool Remove(T item)
        {
            var index = default(ulong);
            if (this.TryGetIndexOfAux(item, out index))
            {
                this.InnerRemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Obtém um enumerador genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var firstLength = this.elements.Length;
            for (int i = 0; i < firstLength; ++i)
            {
                var curr = this.elements[i];
                var secondLength = curr.Length;
                for (int j = 0; j < secondLength; ++j)
                {
                    var elem = curr[j];
                    var thirdLength = elem.Length;
                    for (int k = 0; k < thirdLength; ++k)
                    {
                        yield return elem[k];
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Funções internas para testes

        /// <summary>
        /// Verifica se o tamanho da instância do vector está de acordo com
        /// a capacidade estabelecida.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso o tamanho do vector esteja de acordo com a capacidade
        /// estabelecida e falso caso contrário.
        /// </returns>
        internal bool AssertSizes()
        {
            var computedLength = 0;
            for (int i = 0; i < this.elements.Length; ++i)
            {
                var current = this.elements[i];
                for (int j = 0; j < current.Length; ++j)
                {
                    computedLength += current[j].Length;
                }
            }

            return this.capacity == (ulong)computedLength;
        }

        #endregion Funções internas para testes

        #region Funções privadas

        /// <summary>
        /// Insere um elemento na colecção.
        /// </summary>
        /// <param name="index">O índice onde o item será inserido.</param>
        /// <param name="item">O item a ser inserido.</param>
        private void InnerInsert(ulong index, T item)
        {
            if (this.length == this.capacity)
            {
                this.ExpandCapacity(this.length + 1);
            }

            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (index < this.length)
            {
                // Copia os elementos a partir do índice
                var thirdDim = index & mask;
                var firstDim = index >> maxBinaryPower;
                if (firstDim == 0)
                {
                    this.InsertMoveItems(
                        this.elements,
                        0,
                        0,
                        thirdDim,
                        this.firstDimLength,
                        this.secondDimLength,
                        this.thirdDimLength);

                    this.elements[0][0][thirdDim] = item;
                }
                else
                {
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    this.InsertMoveItems(
                        this.elements,
                        firstDim,
                        secondDim,
                        thirdDim,
                        this.firstDimLength,
                        this.secondDimLength,
                        this.thirdDimLength);
                    this.elements[firstDim][secondDim][thirdDim] = item;
                }
            }
            else
            {
                var firstIndex = this.firstDimLength;
                var secondIndex = this.secondDimLength;
                var thirdIndex = this.thirdDimLength;

                if (thirdIndex < size)
                {
                    this.elements[firstIndex][secondIndex][thirdIndex] = item;
                }
                else
                {
                    if (secondIndex < generalSize)
                    {
                        this.elements[firstIndex][secondIndex][0] = item;
                    }
                    else
                    {
                        this.elements[firstIndex][0][0] = item;
                    }
                }
            }

            this.IncrementLength();
        }

        /// <summary>
        /// Remove o elemento que se encontra na posição especificada.
        /// </summary>
        /// <param name="index">O índice do item a ser removido.</param>
        private void InnerRemoveAt(ulong index)
        {
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (index < this.length)
            {
                var firstEnd = this.firstDimLength;
                var secondEnd = this.secondDimLength;
                var thirdEnd = this.thirdDimLength;
                if (thirdEnd == 0)
                {
                    thirdEnd = mask;
                    if (secondEnd == 0)
                    {
                        secondEnd = generalMask;
                        --firstEnd;
                    }
                    else
                    {
                        --secondEnd;
                    }
                }
                else
                {
                    --thirdEnd;
                }

                var thirdDim = index & mask;
                var firstDim = index >> maxBinaryPower;
                if (firstDim == 0)
                {
                    this.RemoveMoveItems(
                        this.elements,
                        0,
                        0,
                        thirdDim,
                        firstEnd,
                        secondEnd,
                        thirdEnd);
                    this.elements[firstEnd][secondEnd][thirdEnd] = default(T);
                }
                else
                {
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    this.RemoveMoveItems(
                        this.elements,
                        firstDim,
                        secondDim,
                        thirdDim,
                        firstEnd,
                        secondEnd,
                        thirdEnd);
                    this.elements[firstEnd][secondEnd][thirdEnd] = default(T);
                }

                this.DecrementLength();
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="firstStart">A primeira dimensão inicial.</param>
        /// <param name="secondStart">A segunda dimensão inicial.</param>
        /// <param name="thirdStart">A terceira dimensão inicial.</param>
        /// <param name="firstEndLength">A primeira dimensão final.</param>
        /// <param name="secondEndLength">A segunda dimensão final.</param>
        /// <param name="thirdEndLength">A terceira dimensão final.</param>
        private void InsertMoveItems(
            T[][][] items,
            ulong firstStart,
            ulong secondStart,
            ulong thirdStart,
            ulong firstEndLength,
            ulong secondEndLength,
            ulong thirdEndLength)
        {
            if (firstStart == firstEndLength)
            {
                var secondCurrent = items[firstStart];
                this.InsertMoveItemsFinal(
                        secondCurrent,
                        secondStart,
                        thirdStart,
                        secondEndLength,
                        thirdEndLength);
            }
            else
            {
                var firstIndex = firstStart;
                var secondCurrent = items[firstIndex];
                var carriage = this.InsertMoveItems(
                        secondCurrent,
                        secondStart,
                        thirdStart);
                ++firstIndex;
                for (; firstIndex < firstEndLength; ++firstIndex)
                {
                    secondCurrent = items[firstIndex];
                    var innerCarriage = this.InsertMoveItems(
                        secondCurrent,
                        0,
                        0);
                    secondCurrent[0][0] = carriage;
                    carriage = innerCarriage;
                }

                secondCurrent = items[firstIndex];
                this.InsertMoveItemsFinal(
                        secondCurrent,
                        0,
                        0,
                        secondEndLength,
                        thirdEndLength);
                this.elements[firstIndex][0][0] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="firstStart">A primeira dimensão inicial.</param>
        /// <param name="secondStart">A segunda dimensão inicial.</param>
        /// <param name="thirdStart">A terceira dimensão inicial.</param>
        /// <param name="firstLastIndex">A primeira dimensão final.</param>
        /// <param name="secondLastIndex">A segunda dimensão final.</param>
        /// <param name="thirdLastIndex">A terceira dimensão final.</param>
        private void RemoveMoveItems(
            T[][][] items,
            ulong firstStart,
            ulong secondStart,
            ulong thirdStart,
            ulong firstLastIndex,
            ulong secondLastIndex,
            ulong thirdLastIndex)
        {
            var lastIndex = firstLastIndex;
            if (firstStart == lastIndex)
            {
                var secondCurrent = items[firstStart];
                this.RemoveMoveItemsFinal(
                        secondCurrent,
                        secondStart,
                        thirdStart,
                        secondLastIndex,
                        thirdLastIndex);
            }
            else
            {
                var secondCurrent = items[lastIndex];
                var carriage = this.RemoveMoveItems(
                        secondCurrent,
                        secondLastIndex,
                        thirdLastIndex);
                --lastIndex;
                for (; lastIndex > firstStart; --lastIndex)
                {
                    secondCurrent = items[lastIndex];
                    var innerCarriage = this.RemoveMoveItems(
                        secondCurrent,
                        generalMask,
                        mask);
                    secondCurrent[generalMask][mask] = carriage;
                    carriage = innerCarriage;
                }

                secondCurrent = items[lastIndex];
                this.RemoveMoveItemsFinal(
                        secondCurrent,
                        secondStart,
                        thirdStart,
                        generalMask,
                        mask);
                this.elements[lastIndex][generalMask][mask] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startSecondDim">A segunda dimensão inicial.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <returns>O valor do item que resta no final da ordenação dupla.</returns>
        private T InsertMoveItems(
            T[][] items,
            ulong startSecondDim,
            ulong startThirdDim)
        {
            var generalSize = generalMask + 1;
            var secondIndex = startSecondDim;
            var thirdCurrent = items[secondIndex];
            var result = this.InsertMoveItems(
                thirdCurrent,
                startThirdDim);
            ++secondIndex;
            for (; secondIndex < generalSize; ++secondIndex)
            {
                var thirdNext = items[secondIndex];
                var innerResult = this.InsertMoveItems(
                    thirdNext,
                    0);
                thirdNext[0] = result;
                thirdCurrent = thirdNext;
                result = innerResult;
            }

            return result;
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="secondLastIndex">A segunda dimensão final.</param>
        /// <param name="thirdLastIndex">A terceira dimensão final.</param>
        /// <returns>O valor do item que resta no final da ordenação dupla.</returns>
        private T RemoveMoveItems(
            T[][] items,
            ulong secondLastIndex,
            ulong thirdLastIndex)
        {
            var secondIndex = secondLastIndex;
            var thirdCurrent = items[secondIndex];
            var result = this.RemoveMoveItems(
                thirdCurrent,
                thirdLastIndex);

            for (; secondIndex > 0; )
            {
                --secondIndex;
                var thirdNext = items[secondIndex];
                var innerResult = this.RemoveMoveItems(
                    thirdNext,
                    mask);
                thirdNext[mask] = result;
                thirdCurrent = thirdNext;
                result = innerResult;
            }

            return result;
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startSecondDim">A segunda dimensão inicial.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="secondDimLength">O comprimento da segunda dimensão final.</param>
        /// <param name="thirdDimLength">O comprimento da terceira dimensão final.</param>
        private void InsertMoveItemsFinal(
            T[][] items,
            ulong startSecondDim,
            ulong startThirdDim,
            ulong secondDimLength,
            ulong thirdDimLength)
        {
            if (startSecondDim == secondDimLength)
            {
                var thirdCurrent = items[startSecondDim];
                this.InsertMoveItemsFinal(
                    thirdCurrent,
                    startThirdDim,
                    thirdDimLength - startThirdDim);
            }
            else
            {
                var secondIndex = startSecondDim;
                var thirdCurrent = items[secondIndex];
                var carriage = this.InsertMoveItems(
                       thirdCurrent,
                       startThirdDim);
                ++secondIndex;
                for (; secondIndex < secondDimLength; ++secondIndex)
                {
                    thirdCurrent = items[secondIndex];
                    var innerCarriage = this.InsertMoveItems(
                        thirdCurrent,
                        0L);
                    thirdCurrent[0] = carriage;
                    carriage = innerCarriage;
                }

                thirdCurrent = items[secondIndex];
                this.InsertMoveItemsFinal(
                    thirdCurrent,
                    0,
                    thirdDimLength);
                thirdCurrent[0] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startSecondDim">A segunda dimensão inicial.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="secondLastIndex">A segunda dimensão final.</param>
        /// <param name="thirdLastIndex">A terceira dimensão final.</param>
        private void RemoveMoveItemsFinal(
            T[][] items,
            ulong startSecondDim,
            ulong startThirdDim,
            ulong secondLastIndex,
            ulong thirdLastIndex)
        {
            if (startSecondDim == secondLastIndex)
            {
                var thirdCurrent = items[startSecondDim];
                this.RemoveMoveItemsFinal(
                    thirdCurrent,
                    startThirdDim,
                    thirdLastIndex - startThirdDim);
            }
            else
            {
                var secondIndex = secondLastIndex;
                var thirdCurrent = items[secondIndex];
                var carriage = this.RemoveMoveItems(
                       thirdCurrent,
                       thirdLastIndex);
                --secondIndex;
                for (; secondIndex > 0; --secondIndex)
                {
                    thirdCurrent = items[secondIndex];
                    var innerCarriage = this.RemoveMoveItems(
                        thirdCurrent,
                        mask);
                    thirdCurrent[mask] = carriage;
                    carriage = innerCarriage;
                }

                thirdCurrent = items[secondIndex];
                this.RemoveMoveItemsFinal(
                    thirdCurrent,
                    startThirdDim,
                    mask - startThirdDim);
                thirdCurrent[mask] = carriage;
            }
        }

        /// <summary>
        /// Move os itens para inserão.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startThirdDim">A terceira diemnsão inicial.</param>
        /// <returns>O item que resta no final da ordenação.</returns>
        private T InsertMoveItems(
            T[] items,
            ulong startThirdDim)
        {
            var result = items[mask];
            Array.Copy(
                items,
                (long)startThirdDim,
                items,
                (long)(startThirdDim + 1),
                (long)(mask - startThirdDim));
            return result;
        }

        /// <summary>
        /// Move os itens para remoção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="lastThirdIndex">A terceira dimensão final.</param>
        /// <returns>O item que resta no final da ordenação.</returns>
        private T RemoveMoveItems(
            T[] items,
            ulong lastThirdIndex)
        {
            var result = items[0];
            Array.Copy(
                items,
                1,
                items,
                0,
                (long)lastThirdIndex);

            return result;
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="thirdDimLength">O comprimento da terceira dimensão.</param>
        private void InsertMoveItemsFinal(
            T[] items,
            ulong startThirdDim,
            ulong thirdDimLength)
        {
            Array.Copy(
                items,
                (long)startThirdDim,
                items,
                (long)(startThirdDim + 1),
                (long)thirdDimLength);
        }

        /// <summary>
        /// Move os itens para inserção.
        /// </summary>
        /// <param name="items">Os itens a serem movidos.</param>
        /// <param name="startThirdDim">A terceira dimensão inicial.</param>
        /// <param name="lastThirdIndex">A terceira dimensão final.</param>
        private void RemoveMoveItemsFinal(
            T[] items,
            ulong startThirdDim,
            ulong lastThirdIndex)
        {
            Array.Copy(
                items,
                (long)(startThirdDim + 1),
                items,
                (long)startThirdDim,
                (long)lastThirdIndex);
        }

        /// <summary>
        /// Obtém um valor que indica se o item proporcionado se encontra na colecção,
        /// de acordo com o comparador especificado.
        /// </summary>
        /// <param name="item">O item a ser verificado.</param>
        /// <param name="comparer">O comparador de itens.</param>
        /// <returns>Verdadeiro caso o item esteja contido na colecção e falso caso contrário.</returns>
        private bool InnerContains(T item, IEqualityComparer<T> comparer)
        {
            var firstLength = this.elements.Length;
            if (item == null)
            {
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (innerCurr[k] == null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < firstLength; ++i)
                {
                    var curr = this.elements[i];
                    var secondLength = curr.Length;
                    for (int j = 0; j < secondLength; ++j)
                    {
                        var innerCurr = curr[j];
                        var thirdLength = innerCurr.Length;
                        for (int k = 0; k < thirdLength; ++k)
                        {
                            if (comparer.Equals(innerCurr[k], item))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Expande a capacidade nas funções de adição e inserção.
        /// </summary>
        /// <param name="minimum">O mínimo da expansão.</param>
        private void ExpandCapacity(ulong minimum)
        {
            if (this.capacity < minimum)
            {
                var maximumNumber = ulong.MaxValue;
                if (this.assertMemory)
                {
                    maximumNumber = this.GetMaximumAllocation();
                }

                if (maximumNumber <= this.capacity)
                {
                    throw new OutOfMemoryException("There is visible available memory to proceed.");
                }
                else if (this.capacity > (maximumNumber >> 1))
                {
                    this.capacity = minimum < maximumNumber ? minimum : maximumNumber;
                }
                else if (this.capacity == 0)
                {
                    var newCapacity = minimum < defaultCapacity ? minimum : defaultCapacity;
                    this.Instantiate(newCapacity);
                }
                else
                {
                    var aux = this.capacity << 1;
                    var newCapacity = aux < minimum ? minimum : aux;
                    this.IncreaseCapacityTo(newCapacity);
                }
            }
        }

        /// <summary>
        /// Obtém o número máximo de itens que podem ser alocados.
        /// </summary>
        /// <returns>O número máximo de itens.</returns>
        private ulong GetMaximumAllocation()
        {
            var memory = Utils.GetMemoryInfo().TotalVisibleMemorySize;
            var factor = 64UL;
            var objType = typeof(T);
            if (objType == typeof(byte) ||
                    objType == typeof(sbyte) ||
                    objType == typeof(bool))
            {
                factor = 1024;
            }
            else if (objType == typeof(char)
                || objType == typeof(short)
                || objType == typeof(ushort))
            {
                factor = 512;
            }
            else if (objType == typeof(int)
                || objType == typeof(uint)
                || objType == typeof(double))
            {
                factor = 256;
            }
            else if (objType == typeof(long)
                || objType == typeof(ulong)
                || objType == typeof(double))
            {
                factor = 128;
            }

            var itemsNumber = memory * factor;
            return itemsNumber;
        }

        /// <summary>
        /// Verifica a validade da memória visível disponibilizada pelo sistema operativo.
        /// </summary>
        /// <param name="size">O tamanho da colecção.</param>
        private void AssertVisibleMemory(ulong size)
        {
            if (this.assertMemory)
            {
                var itemsNumber = this.GetMaximumAllocation();
                if (itemsNumber < size)
                {
                    throw new OutOfMemoryException("There is no engough visible memory to proceed.");
                }
            }
        }

        /// <summary>
        /// Instancia a estrutura interna.
        /// </summary>
        /// <param name="capacity">O tamanho do vector.</param>
        private void Instantiate(ulong capacity)
        {
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (capacity == 0)
            {
                this.elements = new T[0][][];
            }
            else
            {
                var thirdDim = capacity & mask;
                var firstDim = capacity >> maxBinaryPower;
                if (firstDim == 0)
                {
                    var elems = new T[1][][];
                    var innerElems = new T[1][];
                    innerElems[0] = new T[thirdDim];
                    elems[0] = innerElems;
                    this.elements = elems;
                }
                else
                {
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    if (thirdDim == 0)
                    {
                        if (secondDim == 0)
                        {
                            var elems = new T[firstDim][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }
                        }
                        else
                        {
                            var elems = new T[firstDim + 1][][];
                            this.elements = elems;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new T[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new T[size];
                                }
                            }

                            var innerElemOut = new T[secondDim][];
                            elems[firstDim] = innerElemOut;
                            for (var j = 0UL; j < secondDim; ++j)
                            {
                                innerElemOut[j] = new T[size];
                            }
                        }
                    }
                    else if (secondDim == generalSize)
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[1][];
                        elems[firstDim] = innerElemOut;
                        innerElemOut[0] = new T[thirdDim];
                    }
                    else
                    {
                        var elems = new T[firstDim + 1][][];
                        this.elements = elems;
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new T[generalSize][];
                            elems[i] = innerElem;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new T[size];
                            }
                        }

                        var innerElemOut = new T[secondDim + 1][];
                        elems[firstDim] = innerElemOut;
                        for (var i = 0UL; i < secondDim; ++i)
                        {
                            innerElemOut[i] = new T[size];
                        }

                        innerElemOut[secondDim] = new T[thirdDim];
                    }
                }
            }

            this.capacity = capacity;
        }

        /// <summary>
        /// Tenta obter o índice da primeira ocorrência do item especificado.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <param name="index">O índice da primeira ocorrência do item.</param>
        /// <returns>Verdadeiro se o item se encontrar na colecção e falso caso contrário.</returns>
        private bool TryGetIndexOfAux(T item, out ulong index)
        {
            var firstLength = (ulong)this.elements.Length;
            var generalSize = generalMask + 1;
            for (var i = 0UL; i < firstLength; ++i)
            {
                var secondElem = this.elements[i];
                var secondLength = (ulong)secondElem.Length;
                for (var j = 0UL; j < secondLength; ++j)
                {
                    var innerIndex = Array.IndexOf(secondElem, item);
                    if (innerIndex > -1)
                    {
                        checked
                        {
                            index = (i * generalSize + j) * (mask + 1) + (ulong)innerIndex;
                            return true;
                        }
                    }
                }
            }

            index = 0;
            return false;
        }

        /// <summary>
        /// Aumenta a capacidade da lista.
        /// </summary>
        /// <remarks>
        /// Supõe-se aqui que o valor da nova capacidade se encontra validado e que
        /// é possível estabelecer uma nova capacidade para a lista.
        /// </remarks>
        /// <param name="newCapacity">A nova capacidade a ser estabelecida.</param>
        private void IncreaseCapacityTo(ulong newCapacity)
        {
            var thirdDim = newCapacity & mask;
            var firstDim = newCapacity >> maxBinaryPower;
            var length = this.elements.Length;
            if (length == 0)
            {
                this.Instantiate(newCapacity);
            }
            else if (firstDim == 0)
            {
                this.elements[0][0] = this.ExpandArray(
                        this.elements[0][0],
                        newCapacity);
            }
            else
            {
                var size = mask + 1;
                var generalSize = generalMask + 1;
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (thirdDim == 0)
                {
                    if (secondDim == 0)
                    {
                        var elementsLength = (ulong)this.elements.LongLength;
                        if (firstDim == elementsLength)
                        {
                            --elementsLength;
                            var current = this.elements[elementsLength];
                            if (current.LongLength == generalSize)
                            {
                                var genSize = generalSize - 1;
                                var innerCurrent = current[genSize];
                                if (innerCurrent.LongLength < size)
                                {
                                    current[genSize] = this.ExpandArray(
                                        innerCurrent,
                                        size);
                                }
                            }
                            else
                            {
                                this.elements[elementsLength] = this.ExpandDoubleArray(
                                    current,
                                    generalSize);

                            }
                        }
                        else
                        {
                            this.elements = this.ExpandTripleArray(
                                this.elements,
                                firstDim);
                        }
                    }
                    else
                    {
                        if (firstDim > (ulong)this.elements.Length - 1)
                        {
                            this.elements = this.ExpandTripleArray(
                                this.elements,
                                firstDim,
                                secondDim);
                        }
                        else
                        {
                            var current = this.elements[firstDim];
                            var currLength = (ulong)current.LongLength;
                            if (secondDim > currLength)
                            {
                                this.elements[firstDim] = this.ExpandDoubleArray(
                                this.elements[firstDim],
                                secondDim);
                            }
                            else
                            {
                                --currLength;
                                var innerCurrent = current[currLength];
                                current[currLength] = this.ExpandArray(
                                    innerCurrent,
                                    size);
                            }
                        }
                    }
                }
                else if (secondDim == generalSize)
                {
                    this.elements = this.ExpandTripleArray(
                        this.elements,
                        firstDim,
                        1,
                        thirdDim);
                }
                else
                {
                    var firstLength = (ulong)this.elements.LongLength;
                    if (firstDim == firstLength - 1)
                    {
                        var current = this.elements[firstDim];
                        var curreLen = (ulong)current.LongLength - 1;
                        if (secondDim == curreLen)
                        {
                            current[curreLen] = this.ExpandArray(
                                current[curreLen],
                                thirdDim);
                        }
                        else
                        {
                            this.elements[firstDim] = this.ExpandDoubleArray(
                                current,
                                secondDim,
                                thirdDim);
                        }
                    }
                    else
                    {
                        this.elements = this.ExpandTripleArray(
                            this.elements,
                            firstDim,
                            secondDim,
                            thirdDim);
                    }
                }
            }

            this.capacity = newCapacity;
        }

        /// <summary>
        /// Diminui a capacidade da lista.
        /// </summary>
        /// <param name="newCapacity">A nova capacidade.</param>
        private void DecreaseCapcityTo(ulong newCapacity)
        {
            var thirdDim = newCapacity & mask;
            var firstDim = newCapacity >> maxBinaryPower;
            var length = this.elements.Length;
            if (length == 0)
            {
                this.Instantiate(newCapacity);
            }
            if (firstDim == 0)
            {
                if (thirdDim == 0)
                {
                    this.elements = new T[0][][];
                }
                else
                {
                    this.elements = this.ContractTripleArray(
                        this.elements,
                        0,
                        0,
                        thirdDim);
                }
            }
            else
            {
                var size = mask + 1;
                var generalSize = generalMask + 1;
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (thirdDim == 0)
                {
                    if (secondDim == 0)
                    {
                        this.elements = this.ContractTripleArray(
                            this.elements,
                            firstDim);
                    }
                    else
                    {
                        if (firstDim < (ulong)this.elements.Length - 1)
                        {
                            this.elements = this.ContractTripleArray(
                                this.elements,
                                firstDim,
                                secondDim);
                        }
                        else
                        {
                            this.elements[firstDim] = this.ContractDoubleArray(
                                this.elements[firstDim],
                                secondDim);
                        }
                    }
                }
                else if (secondDim == generalSize)
                {
                    if (firstDim < (ulong)this.elements.Length - 1)
                    {
                        this.elements = this.ContractTripleArray(
                            this.elements,
                            firstDim,
                            1,
                            thirdDim);
                    }
                    else
                    {
                        var current = this.ContractDoubleArray(
                            this.elements[firstDim],
                            1,
                            thirdDim);
                        this.elements[firstDim] = current;
                    }
                }
                else
                {
                    var firstLength = (ulong)this.elements.LongLength;
                    this.elements = this.ContractTripleArray(
                        this.elements,
                        firstDim,
                        secondDim,
                        thirdDim);
                }
            }

            this.capacity = newCapacity;
        }

        /// <summary>
        /// Expande a ordenação até ao valor especificado.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        /// <param name="thirdDim">O valor especificado.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[] ExpandArray(T[] array, ulong thirdDim)
        {
            var newArray = new T[thirdDim];
            Array.Copy(array, newArray, array.Length);
            return newArray;
        }

        /// <summary>
        /// Contrai a ordenação até ao valor especificado.
        /// </summary>
        /// <param name="array">A ordenação a ser contraída.</param>
        /// <param name="thirdDim">O valor especificado.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[] ContractArray(T[] array, ulong thirdDim)
        {
            var newArray = new T[thirdDim];
            Array.Copy(array, newArray, newArray.LongLength);
            return newArray;
        }

        /// <summary>
        /// Expande a dupla ordenação até aos valores especificados.
        /// </summary>
        /// <remarks>
        /// O tamanho exterior refere-se apenas ao tamanho das ordenações
        /// completamente preenchidas, sendo automaticamente aumentado em uma unidade
        /// no caso do terceiro parâmetro.
        /// </remarks>
        /// <param name="doubleArray">A dupla ordenação.</param>
        /// <param name="secondDim">O tamanho da ordenação exterior.</param>
        /// <param name="thirdDim">O tamanho da última ordenação interior.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][] ExpandDoubleArray(
            T[][] doubleArray,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[secondDim + 1][];
            var outLength = doubleArray.LongLength;
            Array.Copy(doubleArray, result, outLength);
            var size = mask + 1;
            for (var i = (ulong)outLength; i < secondDim; ++i)
            {
                result[i] = new T[size];
            }

            result[secondDim] = new T[thirdDim];

            --outLength;
            var last = doubleArray[outLength];
            if (last.Length < size)
            {
                last = this.ExpandArray(last, size);
                result[outLength] = last;
            }

            return result;
        }

        /// <summary>
        /// Contrai uma ordenação dupla.
        /// </summary>
        /// <param name="doubleArray">A ordenação dupla.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <param name="thirdDim">A terceira dimensão.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[][] ContractDoubleArray(
            T[][] doubleArray,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[secondDim + 1][];
            var outLength = result.LongLength;
            Array.Copy(doubleArray, result, outLength);

            var newThirdArray = new T[thirdDim];
            var thirdArray = result[secondDim];
            Array.Copy(thirdArray, newThirdArray, newThirdArray.Length);
            result[secondDim] = newThirdArray;
            return result;
        }

        /// <summary>
        /// Expande a ordenação dupla até ao máximo em cada entrada.
        /// </summary>
        /// <param name="doubleArray">A ordenação dupla.</param>
        /// <param name="secondDim">O novo tamanho da expansão.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][] ExpandDoubleArray(
            T[][] doubleArray,
            ulong secondDim)
        {
            var result = new T[secondDim][];
            var outLength = doubleArray.LongLength;
            Array.Copy(doubleArray, result, outLength);
            var size = mask + 1;
            for (var i = (ulong)outLength; i < secondDim; ++i)
            {
                result[i] = new T[size];
            }

            --outLength;
            var last = doubleArray[outLength];
            if (last.Length < size)
            {
                last = this.ExpandArray(last, size);
                result[outLength] = last;
            }

            return result;
        }

        /// <summary>
        /// Contrai tamanho ao nível da segunda dimensão.
        /// </summary>
        /// <param name="doubleArray">A ordenação dupla.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <returns>O resultado da contracção.</returns>
        private T[][] ContractDoubleArray(
            T[][] doubleArray,
            ulong secondDim)
        {
            var result = new T[secondDim][];
            var outLength = result.LongLength;
            Array.Copy(doubleArray, result, outLength);
            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla ate ao máximo permitido.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla.</param>
        /// <param name="firstDim">O tamanho exterior máximo.</param>
        /// <returns>A ordenação tripla expandida.</returns>
        private T[][][] ExpandTripleArray(
            T[][][] tripleArray,
            ulong firstDim)
        {
            var result = new T[firstDim][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Contrai a ordenação tripla.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla.</param>
        /// <param name="firstDim">A dimensão de contracção.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[][][] ContractTripleArray(
            T[][][] tripleArray,
            ulong firstDim)
        {
            var result = new T[firstDim][][];
            var tripleLength = result.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla até ao máximo permitido, adendando um novo item
        /// especificado pelo parâmetro.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser expandida.</param>
        /// <param name="firstDim">O tamanho exterior que contém o máximo a ser atribuído.</param>
        /// <param name="secondDim">O tamanho da entrada interior a ser atribuída.</param>
        /// <returns>A ordenação tripla expandida.</returns>
        private T[][][] ExpandTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            var newInnerArray = new T[secondDim][];
            result[firstDim] = newInnerArray;
            for (var i = 0UL; i < secondDim; ++i)
            {
                newInnerArray[i] = new T[size];
            }

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Contrai a ordenação tripla.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser contraída.</param>
        /// <param name="firstDim">A primeira dimensão.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <returns>A ordenação tripla contraída.</returns>
        private T[][][] ContractTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = result.LongLength;
            Array.Copy(tripleArray, result, tripleLength);

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if ((ulong)outerCurrent.LongLength > secondDim)
            {
                result[tripleLength] = this.ContractDoubleArray(
                    outerCurrent,
                    secondDim);
            }

            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla até ao máximo permitido, adendando
        /// um item cujo tamanho é especificado pelos restantes parâmetros.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser expandida.</param>
        /// <param name="firstDim">O tamanho exterior que contém o máximo a ser atribuído.</param>
        /// <param name="secondDim">O tamanho interior que contém o máximo a ser atribuído.</param>
        /// <param name="thirdDim">O tamanho do item interior final.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][][] ExpandTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            var newInnerArray = new T[secondDim + 1][];
            result[firstDim] = newInnerArray;
            for (var i = 0UL; i < secondDim; ++i)
            {
                newInnerArray[i] = new T[size];
            }

            newInnerArray[secondDim] = new T[thirdDim];

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Contrai a ordenação tripla.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser contraída.</param>
        /// <param name="firstDim">A primeira dimensão.</param>
        /// <param name="secondDim">A segunda dimensão.</param>
        /// <param name="thirdDim">A terceria dimensão.</param>
        /// <returns>A ordenação contraída.</returns>
        private T[][][] ContractTripleArray(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[firstDim + 1][][];

            var tripleLength = result.LongLength;
            Array.Copy(tripleArray, result, tripleLength);

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if ((ulong)outerCurrent.Length > secondDim)
            {
                result[tripleLength] = this.ContractDoubleArray(
                    outerCurrent,
                    secondDim,
                    thirdDim);
            }

            return result;
        }

        /// <summary>
        /// Obtém os tamanhos das dimensões associados ao valor especificado.
        /// </summary>
        /// <param name="value">O valor especificado.</param>
        /// <returns>Os tamanhos.</returns>
        private Tuple<ulong, ulong, ulong> GetSizes(ulong value)
        {
            var thirdDim = value & mask;
            var firstDim = value >> maxBinaryPower;
            if (firstDim == 0)
            {
                if (thirdDim == 0)
                {
                    return Tuple.Create(0UL, 0UL, 0UL);
                }
                else
                {
                    return Tuple.Create(1UL, 1UL, thirdDim);
                }
            }
            else
            {
                var generalSize = generalMask + 1;
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (thirdDim == 0)
                {
                    if (secondDim > 0)
                    {
                        ++firstDim;
                    }
                }
                else if (secondDim == generalSize)
                {
                    secondDim = 1;
                    ++firstDim;
                }
                else
                {
                    ++secondDim;
                    ++firstDim;
                }

                return Tuple.Create(firstDim, secondDim, thirdDim);
            }
        }

        /// <summary>
        /// Verifica a validade da estrutura da ordenação.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        /// <param name="dimensions">A descrição das dimensões.</param>
        private void AssertArrayStructure(
            Array array,
            long[] dimensions)
        {
            var rank = dimensions.Length;
            var objType = array.GetType();
            for (var i = 0; i < rank; ++i)
            {
                if (objType.IsArray && objType.GetArrayRank() == 1)
                {
                    objType = objType.GetElementType();
                }
                else
                {
                    throw new UtilitiesException(string.Format(
                        "Array must be rank one at level {0}.",
                        i));
                }
            }

            if (!objType.IsAssignableFrom(typeof(T)))
            {
                throw new ArrayTypeMismatchException("Source array type cannot be assigned to destination array type.");
            }
        }

        /// <summary>
        /// Realiza a cópia de uma ordenação para um conjunto de ordenações.
        /// </summary>
        /// <param name="array">A ordenação de partida.</param>
        /// <param name="length">O tamanho do vector a ser copiado.</param>
        /// <param name="arrays">A definição das ordenações de destino.</param>
        /// <param name="indexes">
        /// Os índices que definem o estado das ordenações de destino.
        /// </param>
        private void CopyCurrentArray(
            T[] array,
            long length,
            Array[] arrays,
            long[] indexes)
        {
            var rank = arrays.LongLength;
            var indexPointer = rank - 1;
            var currentIndex = indexes[indexPointer];
            var currentArray = arrays[indexPointer];
            var arrayLength = length;
            var arrayIndex = 0L;
            while (arrayLength > 0)
            {
                var difference = currentArray.LongLength - currentIndex;
                if (difference < arrayLength)
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        difference);
                    arrayLength -= difference;
                    arrayIndex += difference;

                    // Actualiza o estado dos índices
                    var state = true;
                    while (state)
                    {
                        --indexPointer;
                        if (indexPointer < 0)
                        {
                            throw new ArgumentException(
                                "Destination array was not long enough. Check destIndex and length, and the array's lower bounds.");
                        }
                        else
                        {
                            currentIndex = indexes[indexPointer];
                            currentArray = arrays[indexPointer];
                            var currentArrayLength = currentArray.LongLength;
                            ++currentIndex;
                            if (currentIndex < currentArrayLength)
                            {
                                indexes[indexPointer] = currentIndex;
                                ++indexPointer;
                                var innerArray = (Array)currentArray.GetValue(currentIndex);
                                arrays[indexPointer] = innerArray;
                                indexes[indexPointer] = 0L;
                                currentArray = innerArray;
                                ++indexPointer;
                                for (; indexPointer < rank; ++indexPointer)
                                {
                                    innerArray = (Array)currentArray.GetValue(0);
                                    arrays[indexPointer] = innerArray;
                                    indexes[indexPointer] = 0L;
                                    currentArray = innerArray;
                                }

                                --indexPointer;
                                currentIndex = 0;
                                state = false;
                            }
                        }
                    }
                }
                else
                {
                    Array.Copy(
                        array,
                        arrayIndex,
                        currentArray,
                        currentIndex,
                        arrayLength);
                    currentIndex += arrayLength;
                    indexes[indexPointer] = currentIndex;

                    arrayLength = 0;
                }
            }
        }

        /// <summary>
        /// Incrementa as variáveis de comprimento em uma unidade.
        /// </summary>
        private void IncrementLength()
        {
            ++this.length;
            if (this.thirdDimLength == mask)
            {

                this.thirdDimLength = 0;
                if (this.secondDimLength == generalMask)
                {
                    this.secondDimLength = 0;
                    ++firstDimLength;
                }
                else
                {
                    ++this.secondDimLength;
                }
            }
            else
            {
                ++this.thirdDimLength;
            }
        }

        /// <summary>
        /// Decrementa as variáveis de comprimento em uma unidade.
        /// </summary>
        private void DecrementLength()
        {
            --this.length;
            if (this.thirdDimLength == 0)
            {
                this.thirdDimLength = mask;
                if (this.secondDimLength == 0)
                {
                    this.secondDimLength = generalMask;
                    --this.firstDimLength;
                }
                else
                {
                    --this.secondDimLength;
                }
            }
            else
            {
                --this.thirdDimLength;
            }
        }

        /// <summary>
        /// Aumenta as variáveis de tamanho em num número especificado
        /// de unidades.
        /// </summary>
        /// <param name="value">O número de unidades.</param>
        private void IncrementLength(ulong value)
        {
            this.length += value;
            var thirdDim = value & mask;
            var firstDim = value >> maxBinaryPower;
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (firstDim == 0)
            {
                if (thirdDim != 0)
                {
                    var lengthDiff = size - this.thirdDimLength;
                    if (lengthDiff > thirdDim)
                    {
                        this.thirdDimLength += thirdDim;
                    }
                    else
                    {
                        this.thirdDimLength = thirdDim - lengthDiff;
                        if (this.secondDimLength == generalSize)
                        {
                            this.secondDimLength = 0;
                            ++this.firstDimLength;
                        }
                        else
                        {
                            ++this.secondDimLength;
                        }
                    }
                }
                else
                {
                    this.thirdDimLength = value;
                }
            }
            else
            {
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                var lengthdiff = size - this.thirdDimLength;
                if (lengthdiff > thirdDim)
                {
                    this.thirdDimLength += thirdDim;
                    lengthdiff = generalSize - this.secondDimLength;
                    if (lengthdiff > secondDim)
                    {
                        this.secondDimLength += secondDim;
                        this.firstDimLength += firstDim;
                    }
                    else
                    {
                        this.secondDimLength = secondDim - lengthdiff;
                        this.firstDimLength += firstDim + 1;
                    }
                }
                else
                {
                    this.thirdDimLength = thirdDim - lengthdiff;
                    lengthdiff = generalSize - this.secondDimLength - 1;
                    if (lengthdiff > secondDim)
                    {
                        this.secondDimLength += (secondDim + 1);
                        this.firstDimLength += firstDim;
                    }
                    else
                    {
                        this.secondDimLength = secondDim - lengthdiff;
                        this.firstDimLength += firstDim + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Diminui as variáveis de comprimento em um número especificado
        /// de unidades.
        /// </summary>
        /// <param name="value">O número de unidades.</param>
        private void DecrementLength(ulong value)
        {
            this.length -= value;
            var thirdDim = value & mask;
            var firstDim = value >> maxBinaryPower;
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (firstDim == 0)
            {
                this.thirdDimLength -= value;
            }
            else
            {
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (this.thirdDimLength < value)
                {
                    var innerValue = value - this.thirdDimLength;
                    if (this.secondDimLength < innerValue)
                    {
                        innerValue -= this.secondDimLength;
                        this.firstDimLength -= innerValue;
                    }
                    else
                    {
                        this.secondDimLength -= innerValue;
                    }
                }
                else
                {
                    this.thirdDimLength -= value;
                }
            }
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa uma tabela baseada em códigos confusos cujo tamanho não se encontre
    /// limitado aos 2GB.
    /// </summary>
    /// <typeparam name="TKey">O tipo dos objectos que constituem as chaves.</typeparam>
    /// <typeparam name="TValue">O tipo dos objectos que constituem os valores.</typeparam>
    public class GeneralDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        #region Campos estáticos privados

        /// <summary>
        /// Define alguns números primos para serem utilizados na 
        /// reserva de espaço.
        /// </summary>
        /// <remarks>
        /// Com excepção dos dois primeiros termos, os restantes encontram-se
        /// numa relação a/b de aproximadamente 1,2.
        /// </remarks>
        private static readonly ulong[] primes = new ulong[]{
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 
            89, 107, 131, 163, 197, 239, 293, 353,
            431, 521, 631, 761, 919, 1103, 1327,
            1597, 1931, 2333, 2801, 3371, 4049, 
            4861, 5839, 7013, 8419, 10103, 12143, 
            14591, 17519, 21023, 25229, 30293, 36353, 
            43627, 52361, 62851, 75431, 90523, 108631,
            130363, 156437, 187751, 225307, 270371,
            324449, 389357, 467237, 560689, 672827, 
            807403, 968897, 1162687, 1395263, 1674319, 
            2009191, 2411033, 2893249, 3471899, 4166287, 
            4999559, 5999471, 7199369, 8639249, 10367101, 
            12440537, 14928671, 17914409, 21497293, 
            25796759, 30956117, 37147349, 44576837, 
            53492207, 64190669, 77028803, 92434613, 
            110921543, 133105859, 159727031, 191672443, 
            230006941, 276008387, 331210079, 397452101, 
            476942527, 572331049, 686797261, 824156741, 
            988988137, 1186785773
        };

        /// <summary>
        /// Mantém uma instância do vector vazio.
        /// </summary>
        private static readonly TKey[][][] emptyArray = new TKey[0][][];

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de objectos.
        /// </summary>
        private static uint mask;

        /// <summary>
        /// A potência na base 2 para o número máximo de itens que podem ser
        /// contidos num vector de sistema.
        /// </summary>
        private static int maxBinaryPower;

        /// <summary>
        /// A potência máxima para um objecto.
        /// </summary>
        private static int objMaxBinaryPower;

        /// <summary>
        /// A máscara para efectuar o resto da divisão nos vectores de vectores.
        /// </summary>
        private static uint generalMask;

        #endregion Campos estáticos privados

        #region Campos privados

        /// <summary>
        /// Variável que indica se se irá avaliar a memória disponível
        /// em caso de instância.
        /// </summary>
        private bool assertMemory = true;

        /// <summary>
        /// Mantém as entradas.
        /// </summary>
        private Entry[][][] entries;

        /// <summary>
        /// Mantém os apontadores para as entradas.
        /// </summary>
        private Nullable<ulong>[][][] buckets;

        /// <summary>
        /// Mantém o tamanho total do vector.
        /// </summary>
        private ulong count;

        /// <summary>
        /// O número de entradas removidas que ainda não foram ocupadas.
        /// </summary>
        private ulong freeListCount = 0UL;

        /// <summary>
        /// Apontador para a primeira entrada removida que ainda
        /// não foi ocupada.
        /// </summary>
        private Nullable<ulong> freeList;

        /// <summary>
        /// Mantém o valor da capacidade do vector. 
        /// </summary>
        private ulong capacity;

        /// <summary>
        /// O comparador.
        /// </summary>
        private IEqualityComparer64<TKey> comparer;

        /// <summary>
        /// Mantém a classe que representa a colecção das chaves.
        /// </summary>
        private KeyCollection keys;

        /// <summary>
        /// Mantém a classe que representa a colecção de valores.
        /// </summary>
        private ValueCollection values;

        #endregion Campos privados

        /// <summary>
        /// Inicializa o tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        static GeneralDictionary()
        {
            var configSection = Utils.GetRuntimeConfiguration();
            if (configSection.GcAllowVeryLargeObjects.Enabled)
            {
                mask = 2147483647;
                maxBinaryPower = 31;
                objMaxBinaryPower = 31;
                generalMask = 2147483647;
            }
            else
            {
                // O vector das entradas consiste num objecto.
                maxBinaryPower = 26;
                generalMask = 67108863;
                mask = generalMask;
            }
        }

        #region Construtores públicos

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(bool assertMemory = true)
        {
            this.Initialize(
                0,
                EqualityComparer64<TKey>.Default,
                assertMemory);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(int capacity, bool assertMemory = true)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "Capacity must be a non-negative number.");
            }
            else if (capacity > 0)
            {
                this.Initialize(
                    (ulong)capacity,
                    EqualityComparer64<TKey>.Default,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(uint capacity, bool assertMemory = true)
        {
            if (capacity > 0)
            {
                this.Initialize(
                    capacity,
                    EqualityComparer64<TKey>.Default,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(long capacity, bool assertMemory = true)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "Capacity must be a non-negative number.");
            }
            else if (capacity > 0)
            {
                this.Initialize(
                    (ulong)capacity,
                    EqualityComparer64<TKey>.Default,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(ulong capacity, bool assertMemory = true)
        {
            if (capacity > 0)
            {
                this.Initialize(
                    capacity,
                    EqualityComparer64<TKey>.Default,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de chaves.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(
            IEqualityComparer64<TKey> comparer,
            bool assertMemory = true)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.Initialize(
                    0,
                    comparer,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="comparer">O comparador de chaves.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(
            int capacity,
            IEqualityComparer64<TKey> comparer,
            bool assertMemory = true)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "Capacity must be a non-negative number.");
            }
            else if (capacity > 0)
            {
                this.Initialize(
                    (ulong)capacity,
                    comparer,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="comparer">O comparador a ser usado no dicionário.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(
            uint capacity,
            IEqualityComparer64<TKey> comparer,
            bool assertMemory = true)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else if (capacity > 0)
            {
                this.Initialize(
                    capacity,
                    comparer,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="comparer">O comparador de chaves.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(
            long capacity,
            IEqualityComparer64<TKey> comparer,
            bool assertMemory = true)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "Capacity must be a non-negative number.");
            }
            else if (capacity > 0)
            {
                this.Initialize(
                    (ulong)capacity,
                    comparer,
                    assertMemory);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="comparer">O comparador de chaves.</param>
        /// <param name="assertMemory">Parâmetro que indica se a memória disponível será analisada.</param>
        public GeneralDictionary(
            ulong capacity,
            IEqualityComparer64<TKey> comparer,
            bool assertMemory = true)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else if (capacity > 0)
            {
                this.Initialize(
                    capacity,
                    comparer,
                    assertMemory);
            }
        }

        #endregion Construtores públicos

        #region Propriedades públicas

        /// <summary>
        /// Obtém ou atribui o valor associado à chave.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <returns>O valor associado.</returns>
        public TValue this[TKey key]
        {
            get
            {
                var entry = default(Entry);
                if (this.TryFindEntry(key, out entry))
                {
                    return entry.Value;
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            set
            {
                this.SetEntry(key, value, false);
            }
        }

        /// <summary>
        /// Obtém o número de elementos na colecção.
        /// </summary>
        public int Count
        {
            get
            {
                var innerCount = this.count - this.freeListCount;
                if (innerCount > int.MaxValue)
                {
                    throw new CollectionsException("The length of dictionary is too big. Please use LongCount instaed.");
                }
                else
                {
                    return (int)innerCount;
                }
            }
        }

        /// <summary>
        /// Obtém o número de elementos na colecção.
        /// </summary>
        public uint UintCount
        {
            get
            {
                var innerCount = this.count - this.freeListCount;
                if (this.count > uint.MaxValue)
                {
                    throw new CollectionsException("The length of dictionary is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (uint)innerCount;
                }
            }
        }

        /// <summary>
        /// Obtém o número de elementos na colecção.
        /// </summary>
        public long LongCount
        {
            get
            {
                var innerCount = this.count - this.freeListCount;
                if (this.count > long.MaxValue)
                {
                    throw new CollectionsException("The length of dictionary is too big. Please use UlongCount instaed.");
                }
                else
                {
                    return (long)innerCount;
                }
            }
        }

        /// <summary>
        /// Obtém o número de elementos na colecção.
        /// </summary>
        public ulong UlongCount
        {
            get
            {
                var innerCount = this.count - this.freeListCount;
                return innerCount;
            }
        }

        /// <summary>
        /// Obtém o comparador usado pelo dicionário.
        /// </summary>
        public IEqualityComparer64<TKey> Comparer
        {
            get
            {
                return this.comparer;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é só de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }


        /// <summary>
        /// Obtém a colecção das chaves.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                if (this.keys == null)
                {
                    this.keys = new KeyCollection(this);
                }

                return this.keys;
            }
        }

        /// <summary>
        /// Obtém a colecção dos valores.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                if (this.values == null)
                {
                    this.values = new ValueCollection(this);
                }

                return this.values;
            }
        }

        #endregion Propriedades públicas

        #region Propriedades internas

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo das entradas da colecção.
        /// </summary>
        internal static int MaxBinaryPower
        {
            get
            {
                return maxBinaryPower;
            }
            set
            {
                maxBinaryPower = value;
                mask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo dos das entradas
        /// da colecção.
        /// </summary>
        internal static uint Mask
        {
            get
            {
                return mask;
            }
        }

        /// <summary>
        /// Obtém ou atribui a potência binária máxima para uma ordenação dos
        /// objectos do tipo geral.
        /// </summary>
        internal static int ObjMaxBinaryPower
        {
            get
            {
                return objMaxBinaryPower;
            }
            set
            {
                objMaxBinaryPower = value;
                generalMask = (1U << value) - 1;
            }
        }

        /// <summary>
        /// Obtém o valor da máscara para os objectos do tipo geral.
        /// </summary>
        internal static uint GeneralMask
        {
            get
            {
                return generalMask;

            }
        }

        #endregion Propriedades internas

        #region Funções públicas

        /// <summary>
        /// Adiciona uma associação à tabela.
        /// </summary>
        /// <param name="key">A chave da associação.</param>
        /// <param name="value">O valor da associação.</param>
        public void Add(TKey key, TValue value)
        {
            this.SetEntry(key, value, true);
        }

        /// <summary>
        /// Verifica se a chave proporcionada se encontra associada.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <returns>Verdadeiro caso a chave se encontre associada e falso caso contrário.</returns>
        public bool ContainsKey(TKey key)
        {
            var entry = default(Entry);
            return this.TryFindEntry(key, out entry);
        }

        /// <summary>
        /// Remove a associação atribuída à chave especificada.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <returns>Verdadeiro caso a associação seja removida e falso caso contrário.</returns>
        public bool Remove(TKey key)
        {
            if (this.count > 0)
            {
                var hashCode = this.comparer.GetHash64(key);
                var rem = hashCode % this.capacity;
                var last = default(Nullable<ulong>);
                Nullable<ulong> i = this.GetItem(
                    this.buckets,
                    rem);
                while (i.HasValue)
                {
                    var entry = this.GetItem(
                        this.entries,
                        i.Value);
                    if (entry.HashCode == hashCode &&
                        this.comparer.Equals(entry.Key, key))
                    {
                        if (last.HasValue)
                        {
                            var lastEntry = this.GetItem(
                                this.entries,
                                last.Value);
                            lastEntry.Next = entry.Next;
                        }
                        else
                        {
                            var item = entry.Next;
                            this.SetItem(
                                this.buckets,
                                rem,
                                entry.Next);
                        }

                        entry.HashCode = null;
                        entry.Next = this.freeList;
                        entry.Key = default(TKey);
                        entry.Value = default(TValue);
                        this.freeList = i;
                        ++this.freeListCount;
                        return true;
                    }

                    last = i;
                    i = entry.Next;
                }
            }

            return false;
        }

        /// <summary>
        /// Tenta obter o valor associado à chave.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <param name="value">Variável de saída que irá conter o valor associado.</param>
        /// <returns>Verdadeiro caso a chave esteja associada e falso caso contrário.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            var entry = default(Entry);
            if (this.TryFindEntry(key, out entry))
            {
                value = entry.Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        /// <summary>
        /// Associa um par chave / valor à tabela.
        /// </summary>
        /// <param name="item">O para a ser associado.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.SetEntry(item.Key, item.Value, true);
        }

        /// <summary>
        /// Elimina todas as associações da tabela.
        /// </summary>
        public void Clear()
        {
            if (this.count > 0)
            {
                var firstEntry = this.entries;
                var firstLength = firstEntry.LongLength;
                var firstBuck = this.buckets;
                for (var i = 0; i < firstLength; ++i)
                {
                    var secondEntry = firstEntry[i];
                    var secondLength = secondEntry.LongLength;
                    var secondBuck = firstBuck[i];
                    for (var j = 0; j < secondLength; ++j)
                    {
                        var thirdEntry = secondEntry[j];
                        var thirdLength = thirdEntry.LongLength;
                        var thirdBuck = secondBuck[j];
                        for (var k = 0; k < thirdLength; ++k)
                        {
                            thirdBuck[k] = null;
                        }

                        Array.Clear(thirdEntry, 0, (int)secondLength);
                    }
                }

                this.freeList = null;
                this.freeListCount = 0;
                this.count = 0;
            }
        }

        /// <summary>
        /// Averigua se o par chave / valor se encontra associado.
        /// </summary>
        /// <param name="item">O para chave / valor.</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var entry = default(Entry);
            if (this.TryFindEntry(item.Key, out entry))
            {
                return EqualityComparer<TValue>.Default.Equals(
                    item.Value,
                    entry.Value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Copia as associações para um vector de pares chave / valor.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="arrayIndex">O índice do vector a partir do qual é realziada a cópia.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (arrayIndex < 0 || arrayIndex > array.LongLength)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", "Index is outside the bounds of the array.");
            }
            else if ((ulong)(array.LongLength - arrayIndex) < this.count)
            {
                throw new ArgumentException("Array has no sufficient positions to hold collection entries.");
            }
            else
            {
                var index = arrayIndex;
                var entries = this.entries;
                var firstLength = entries.LongLength;
                for (var i = 0; i < firstLength; ++i)
                {
                    var secondEntry = entries[i];
                    var secondLength = secondEntry.LongLength;
                    for (var j = 0; j < secondLength; ++j)
                    {
                        var thirdEntry = secondEntry[j];
                        var thirdLength = thirdEntry.LongLength;
                        for (var k = 0; k < thirdLength; ++k)
                        {
                            var entry = thirdEntry[k];
                            if (entry != null && entry.HashCode.HasValue)
                            {
                                array[index++] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove a associação especificada pelo par chave / valor.
        /// </summary>
        /// <param name="item">O par chave / valor a ser removido.</param>
        /// <returns>Verdadeiro caso a eliminação ocorra e falso caso contrário.</returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var entry = default(Entry);
            if (this.TryFindEntry(item.Key, out entry))
            {
                if (EqualityComparer<TValue>.Default.Equals(
                    item.Value,
                    entry.Value))
                {
                    return this.Remove(item.Key);
                }
            }

            return false;
        }

        /// <summary>
        /// Obtém um enumerador para o conjunto de pares chave / valor associados.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion Funções públicas

        /// <summary>
        /// Obtém um enumerador não genérico para o conjunto de pares chave / valor associados.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Obtém menor número primo maior ou igual ao valor especificado.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>O número primo.</returns>
        private static ulong GetNextPrime(ulong value)
        {
            var index = BinarySearch(value, primes);
            if (index == -1)
            {
                var primeProduct = 30030UL;
                var wheel = GetWheel();

                var rem = value % primeProduct;
                var init = (value / primeProduct) * primeProduct;

                var wheelInd = BinarySearch(rem, wheel);
                if (wheelInd == -1)
                {
                    wheelInd = 0;
                    init += primeProduct;
                }

                var candidate = init + wheel[wheelInd];
                while (!IsPrime(candidate, wheel))
                {
                    ++wheelInd;
                    if (wheelInd == 5760)
                    {
                        wheelInd = 0;
                        init += primeProduct;
                    }

                    candidate = init + wheel[wheelInd];
                }

                return candidate;
            }
            else
            {
                return primes[index];
            }
        }

        /// <summary>
        /// Determina se um número é primo.
        /// </summary>
        /// <param name="value">
        /// O valor do qual se pretende determinara primalidade.
        /// </param>
        /// <param name="wheel">A roda usada no teste.</param>
        /// <returns>Verdadeiro caso o número seja primo e falso caso contrário.</returns>
        private static bool IsPrime(
            ulong value,
            ulong[] wheel)
        {
            var sqrt = (ulong)Math.Ceiling(Math.Sqrt(value));
            var basis = 0UL;
            var current = wheel[1];
            var ind = 2;
            while (current < sqrt)
            {
                if (value % current == 0)
                {
                    return false;
                }
                else
                {
                    current = basis + wheel[ind];
                    ++ind;
                    if (ind == 5760)
                    {
                        ind = 0;
                        basis += 30030UL;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Obtém a roda para a determinação do número primo.
        /// </summary>
        /// <returns>A roda.</returns>
        private static ulong[] GetWheel()
        {
            var primeProduct = 30030UL;
            var length = 5760;
            var result = new ulong[5760];
            var smallWheel = new ulong[]{
                6, 4, 2, 4, 2, 4 , 6, 2
            };

            var i = 0;
            var j = length - 1;

            result[i++] = 1UL;
            result[j--] = primeProduct - 1UL;

            var current = 17UL;
            var currInd = 4;

            while (i < j)
            {
                var isPrime = true;
                if (current % 7 == 0)
                {
                    isPrime = false;
                }

                if (isPrime)
                {
                    if (current % 11 == 0)
                    {
                        isPrime = false;
                    }
                }

                if (isPrime)
                {
                    if (current % 13 != 0)
                    {
                        result[i++] = current;
                        result[j--] = primeProduct - current;
                    }
                }

                current += smallWheel[currInd];
                ++currInd;
                if (currInd == 8)
                {
                    currInd = 0;
                }
            }


            return result;
        }

        /// <summary>
        /// Efectua a pesquisa binária para determinar se algum dos números
        /// primos é o menor primo superior ao valor proporcionado.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <param name="array">O vector onde será realizada a pesquisa.</param>
        /// <returns>O índice do número na colecção.</returns>
        private static long BinarySearch(
            ulong value,
            ulong[] array)
        {
            var high = array.LongLength - 1;
            var low = 0L;
            var highValue = array[high];
            var lowValue = array[low];
            if (value > highValue)
            {
                return -1L;
            }
            else if (value == highValue)
            {
                return high;
            }
            else if (value <= lowValue)
            {
                return low;
            }
            else
            {
                var mid = (high + low) >> 1;
                while (mid != low)
                {
                    var midVal = array[mid];
                    if (midVal < value)
                    {
                        low = mid;
                    }
                    else
                    {
                        high = mid;
                        if (value == array[high])
                        {
                            return high;
                        }
                    }

                    mid = (high + low) >> 1;
                }

                return high;
            }
        }

        /// <summary>
        /// Inicializa o dicionário.
        /// </summary>
        /// <param name="capacity">A capacidade do dicionário.</param>
        /// <param name="comparer">O comparador utilizado no dicionário.</param>
        /// <param name="assertMemory">
        /// Indica se existe memória suficiente na máquina para realizar a operação.
        /// </param>
        private void Initialize(
            ulong capacity,
            IEqualityComparer64<TKey> comparer,
            bool assertMemory)
        {
            this.assertMemory = assertMemory;
            this.AssertVisibleMemory((ulong)capacity);
            this.comparer = comparer;

            var innerCapacity = GetNextPrime(capacity);
            this.Instantiate(innerCapacity);
        }

        /// <summary>
        /// Instancia novos vectores.
        /// </summary>
        /// <param name="capacity">A capacidade.</param>
        private void Instantiate(ulong capacity)
        {
            var size = mask + 1;
            var generalSize = generalMask + 1;
            if (capacity == 0)
            {
                this.buckets = new Nullable<ulong>[0][][];
                this.entries = new Entry[0][][];
                this.capacity = 0;
            }
            else
            {
                var innerCapacity = GetNextPrime(capacity);
                var thirdDim = innerCapacity & mask;
                var firstDim = innerCapacity >> maxBinaryPower;
                if (firstDim == 0)
                {
                    var elems = new Entry[1][][];
                    var innerElems = new Entry[1][];
                    innerElems[0] = new Entry[thirdDim];
                    elems[0] = innerElems;
                    this.entries = elems;

                    var bucks = new Nullable<ulong>[1][][];
                    var innerBucks = new Nullable<ulong>[1][];
                    innerBucks[0] = new Nullable<ulong>[thirdDim];
                    bucks[0] = innerBucks;
                    this.buckets = bucks;
                }
                else
                {
                    var secondDim = firstDim & generalMask;
                    firstDim >>= objMaxBinaryPower;
                    if (thirdDim == 0)
                    {
                        if (secondDim == 0)
                        {
                            var elems = new Entry[firstDim][][];
                            this.entries = elems;
                            var bucks = new Nullable<ulong>[firstDim][][];
                            this.buckets = bucks;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new Entry[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new Entry[size];
                                }

                                var innerBuck = new Nullable<ulong>[generalSize][];
                                bucks[i] = innerBuck;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerBuck[j] = new Nullable<ulong>[size];
                                }
                            }
                        }
                        else
                        {
                            var elems = new Entry[firstDim + 1][][];
                            this.entries = elems;
                            var bucks = new Nullable<ulong>[firstDim + 1][][];
                            this.buckets = bucks;
                            for (var i = 0UL; i < firstDim; ++i)
                            {
                                var innerElem = new Entry[generalSize][];
                                elems[i] = innerElem;
                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerElem[j] = new Entry[size];
                                }

                                var innerBuck = new Nullable<ulong>[generalSize][];
                                bucks[i] = innerBuck;

                                for (int j = 0; j < generalSize; ++j)
                                {
                                    innerBuck[j] = new Nullable<ulong>[size];
                                }
                            }

                            var innerElemOut = new Entry[secondDim][];
                            elems[firstDim] = innerElemOut;
                            var innerBuckOut = new Nullable<ulong>[secondDim][];
                            for (var j = 0UL; j < secondDim; ++j)
                            {
                                innerElemOut[j] = new Entry[size];
                                innerBuckOut[j] = new Nullable<ulong>[size];
                            }
                        }
                    }
                    else if (secondDim == generalSize)
                    {
                        var elems = new Entry[firstDim + 1][][];
                        this.entries = elems;
                        var bucks = new Nullable<ulong>[firstDim + 1][][];
                        this.buckets = bucks;
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new Entry[generalSize][];
                            elems[i] = innerElem;
                            var buck = new Nullable<ulong>[generalSize][];
                            bucks[i] = buck;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new Entry[size];
                                buck[j] = new Nullable<ulong>[size];
                            }
                        }

                        var innerElemOut = new Entry[1][];
                        elems[firstDim] = innerElemOut;
                        innerElemOut[0] = new Entry[thirdDim];

                        var buckOut = new Nullable<ulong>[1][];
                        bucks[firstDim] = buckOut;
                        buckOut[0] = new Nullable<ulong>[thirdDim];
                    }
                    else
                    {
                        var elems = new Entry[firstDim + 1][][];
                        this.entries = elems;
                        var bucks = new Nullable<ulong>[firstDim + 1][][];
                        for (var i = 0UL; i < firstDim; ++i)
                        {
                            var innerElem = new Entry[generalSize][];
                            elems[i] = innerElem;
                            var innerBuck = new Nullable<ulong>[generalSize][];
                            bucks[i] = innerBuck;
                            for (int j = 0; j < generalSize; ++j)
                            {
                                innerElem[j] = new Entry[size];
                                innerBuck[j] = new Nullable<ulong>[size];
                            }
                        }

                        var innerElemOut = new Entry[secondDim + 1][];
                        elems[firstDim] = innerElemOut;
                        var innerBuckOut = new Nullable<ulong>[secondDim + 1][];
                        for (var i = 0UL; i < secondDim; ++i)
                        {
                            innerElemOut[i] = new Entry[size];
                            innerBuckOut[i] = new Nullable<ulong>[size];
                        }

                        innerElemOut[secondDim] = new Entry[thirdDim];
                        innerBuckOut[secondDim] = new Nullable<ulong>[thirdDim];
                    }
                }

                this.capacity = innerCapacity;
            }
        }

        /// <summary>
        /// Obtém o número máximo de itens que podem ser alocados.
        /// </summary>
        /// <returns>O número máximo de itens.</returns>
        private ulong GetMaximumAllocation()
        {
            var memory = Utils.GetMemoryInfo().TotalVisibleMemorySize;
            var factor = 64UL;
            var itemsNumber = memory * factor;
            return itemsNumber;
        }

        /// <summary>
        /// Verifica a validade da memória visível disponibilizada pelo sistema operativo.
        /// </summary>
        /// <param name="size">O tamanho da colecção.</param>
        private void AssertVisibleMemory(ulong size)
        {
            if (this.assertMemory)
            {
                var itemsNumber = this.GetMaximumAllocation();
                if (itemsNumber < size)
                {
                    throw new OutOfMemoryException("There is no engough visible memory to proceed.");
                }
            }
        }

        /// <summary>
        /// Tenta determinar a entrada associada à chave.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <param name="entry">A entrada se esta existir.</param>
        /// <returns>
        /// Verdadeiro se a entrada existir e falso caso contrário.
        /// </returns>
        private bool TryFindEntry(TKey key, out Entry entry)
        {
            var hashCode = this.comparer.GetHash64(key);
            var rem = hashCode % this.capacity;
            var i = this.GetItem(this.buckets, rem);
            while (i != null)
            {
                var innerEntry = this.GetItem(this.entries, i.Value);
                if (innerEntry.HashCode == hashCode &&
                    this.comparer.Equals(innerEntry.Key, key))
                {
                    entry = innerEntry;
                    return true;
                }

                i = this.GetItem(this.entries, i.Value).Next;
            }

            entry = default(Entry);
            return false;
        }

        /// <summary>
        /// Estabelece a entrada do vector.
        /// </summary>
        /// <param name="key">A chave.</param>
        /// <param name="value">O valor.</param>
        /// <param name="isToAdd">Valor que indica se o par é adicionado.</param>
        private void SetEntry(TKey key, TValue value, bool isToAdd)
        {
            var hashCode = this.comparer.GetHash64(key);
            var rem = hashCode % this.capacity;
            var i = this.GetItem(this.buckets, rem);
            while (i != null)
            {
                var entry = this.GetItem(this.entries, i.Value);
                if (entry.HashCode == hashCode &&
                    this.comparer.Equals(entry.Key, key))
                {
                    if (isToAdd)
                    {
                        throw new ArgumentException("Can't add duplicate key.");
                    }
                    else
                    {
                        entry.Value = value;
                        return;
                    }
                }

                i = entry.Next;
            }

            var outEntry = default(Entry);
            var index = default(ulong);
            if (this.freeListCount > 0)
            {
                index = this.freeList.Value;
                outEntry = this.GetItem(
                    this.entries,
                    index);
                freeList = outEntry.Next;
                --this.freeListCount;
            }
            else
            {
                if (this.count == this.capacity)
                {
                    this.Resize();
                    rem = hashCode % this.capacity;
                }

                outEntry = new Entry();
                this.SetItem(
                    this.entries,
                    count,
                    outEntry);
                index = count;
                ++this.count;
            }

            outEntry.HashCode = hashCode;
            outEntry.Next = this.GetItem(this.buckets, rem);
            outEntry.Key = key;
            outEntry.Value = value;

            this.SetItem(this.buckets, rem, index);
        }

        /// <summary>
        /// Aumenta o tamanho do dicionário.
        /// </summary>
        private void Resize()
        {
            var newCapacity = GetNextPrime(capacity + 1);
            this.IncreaseCapacityTo(newCapacity);

            var firstLength = this.buckets.LongLength;
            for (var i = 0; i < firstLength; ++i)
            {
                var firstEntry = this.buckets[i];
                var secondLength = firstEntry.LongLength;
                for (var j = 0; j < secondLength; ++j)
                {
                    var secondEntry = firstEntry[j];
                    var thirdLength = secondEntry.LongLength;
                    for (var k = 0; k < thirdLength; ++k)
                    {
                        secondEntry[k] = null;
                    }
                }
            }

            var ind = 0UL;
            firstLength = this.buckets.LongLength;
            for (var i = 0; i < firstLength; ++i)
            {
                var firstEntry = this.entries[i];
                var secondLength = firstEntry.LongLength;
                for (var j = 0; j < secondLength; ++j)
                {
                    var secondEntry = firstEntry[j];
                    var thirdLength = secondEntry.LongLength;
                    for (var k = 0; k < thirdLength; ++k)
                    {
                        var entry = secondEntry[k];
                        if (entry != null && entry.HashCode.HasValue)
                        {
                            var bucket = entry.HashCode.Value % this.capacity;
                            entry.Next = this.GetItem(this.buckets, bucket);
                            this.SetItem(this.buckets, bucket, ind);
                        }

                        ++ind;
                    }
                }
            }

            this.capacity = newCapacity;
        }

        /// <summary>
        /// Obtém o item na posição especificada do vector.
        /// </summary>
        /// <typeparam name="T">
        /// O tipo dos objectos que constituem as entradas do vector.
        /// </typeparam>
        /// <param name="array">O vector.</param>
        /// <param name="index">O índice do qual se pretende obter o valor.</param>
        /// <returns>O valor.</returns>
        private T GetItem<T>(
            T[][][] array,
            ulong index)
        {
            var thirdDim = index & mask;
            var firstDim = index >> maxBinaryPower;
            if (firstDim == 0)
            {
                return array[0][0][thirdDim];
            }
            else
            {
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                return array[firstDim][secondDim][thirdDim];
            }
        }

        /// <summary>
        /// Estabelece o valor do item na posição especificada.
        /// </summary>
        /// <typeparam name="T">
        /// O tipo de objectos que constituem as entradas do vector.
        /// </typeparam>
        /// <param name="array">O vector.</param>
        /// <param name="index">O índice.</param>
        /// <param name="value">O valor.</param>
        private void SetItem<T>(
            T[][][] array,
            ulong index,
            T value)
        {
            var thirdDim = index & mask;
            var firstDim = index >> maxBinaryPower;
            if (firstDim == 0)
            {
                array[0][0][thirdDim] = value;
            }
            else
            {
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                array[firstDim][secondDim][thirdDim] = value;
            }
        }

        /// <summary>
        /// Aumenta a capacidade do dicionário.
        /// </summary>
        /// <remarks>
        /// Supõe-se aqui que o valor da nova capacidade se encontra validado e que
        /// é possível estabelecer uma nova capacidade para o dicionário.
        /// </remarks>
        /// <param name="newCapacity">A nova capacidade a ser estabelecida.</param>
        private void IncreaseCapacityTo(ulong newCapacity)
        {
            var thirdDim = newCapacity & mask;
            var firstDim = newCapacity >> maxBinaryPower;
            var length = this.entries.Length;
            if (length == 0)
            {
                this.Instantiate(newCapacity);
            }
            else if (firstDim == 0)
            {
                this.entries[0][0] = this.ExpandArray(
                        this.entries[0][0],
                        newCapacity);
                this.buckets[0][0] = this.ExpandArray(
                    this.buckets[0][0],
                    newCapacity);
            }
            else
            {
                var size = mask + 1;
                var generalSize = generalMask + 1;
                var secondDim = firstDim & generalMask;
                firstDim >>= objMaxBinaryPower;
                if (thirdDim == 0)
                {
                    if (secondDim == 0)
                    {
                        var elementsLength = (ulong)this.entries.LongLength;
                        if (firstDim == elementsLength)
                        {
                            --elementsLength;
                            var current = this.entries[elementsLength];
                            var currBuck = this.buckets[elementsLength];
                            if (current.LongLength == generalSize)
                            {
                                var genSize = generalSize - 1;
                                var innerCurrent = current[genSize];
                                var innerBuckCurrent = currBuck[genSize];
                                if (innerCurrent.LongLength < size)
                                {
                                    current[genSize] = this.ExpandArray(
                                        innerCurrent,
                                        size);
                                    currBuck[genSize] = this.ExpandArray(
                                        innerBuckCurrent,
                                        size);
                                }
                            }
                            else
                            {
                                this.entries[elementsLength] = this.ExpandDoubleArray(
                                    current,
                                    generalSize);
                                this.buckets[elementsLength] = this.ExpandDoubleArray(
                                    currBuck,
                                    generalSize);

                            }
                        }
                        else
                        {
                            this.entries = this.ExpandTripleArray(
                                this.entries,
                                firstDim);
                            this.buckets = this.ExpandTripleArray(
                                this.buckets,
                                firstDim);
                        }
                    }
                    else
                    {
                        if (firstDim > (ulong)this.entries.Length - 1)
                        {
                            this.entries = this.ExpandTripleArray(
                                this.entries,
                                firstDim,
                                secondDim);
                            this.buckets = this.ExpandTripleArray(
                                this.buckets,
                                firstDim,
                                secondDim);
                        }
                        else
                        {
                            var current = this.entries[firstDim];
                            var currBuck = this.buckets[firstDim];
                            var currLength = (ulong)current.LongLength;
                            if (secondDim > currLength)
                            {
                                this.entries[firstDim] = this.ExpandDoubleArray(
                                this.entries[firstDim],
                                secondDim);
                                this.buckets[firstDim] = this.ExpandDoubleArray(
                                    this.buckets[firstDim],
                                    secondDim);
                            }
                            else
                            {
                                --currLength;
                                var innerCurrent = current[currLength];
                                current[currLength] = this.ExpandArray(
                                    innerCurrent,
                                    size);

                                var innerBuckCurr = currBuck[currLength];
                                currBuck[currLength] = this.ExpandArray(
                                    innerBuckCurr,
                                    size);
                            }
                        }
                    }
                }
                else if (secondDim == generalSize)
                {
                    this.entries = this.ExpandTripleArray(
                        this.entries,
                        firstDim,
                        1,
                        thirdDim);
                    this.buckets = this.ExpandTripleArray(
                        this.buckets,
                        firstDim,
                        1,
                        thirdDim);
                }
                else
                {
                    var firstLength = (ulong)this.entries.LongLength;
                    if (firstDim == firstLength - 1)
                    {
                        var current = this.entries[firstDim];
                        var currBuck = this.buckets[firstDim];
                        var curreLen = (ulong)current.LongLength - 1;
                        if (secondDim == curreLen)
                        {
                            current[curreLen] = this.ExpandArray(
                                current[curreLen],
                                thirdDim);
                            currBuck[curreLen] = this.ExpandArray(
                                currBuck[curreLen],
                                thirdDim);
                        }
                        else
                        {
                            this.entries[firstDim] = this.ExpandDoubleArray(
                                current,
                                secondDim,
                                thirdDim);
                            this.buckets[firstDim] = this.ExpandDoubleArray(
                                currBuck,
                                secondDim,
                                thirdDim);
                        }
                    }
                    else
                    {
                        this.entries = this.ExpandTripleArray(
                            this.entries,
                            firstDim,
                            secondDim,
                            thirdDim);
                        this.buckets = this.ExpandTripleArray(
                            this.buckets,
                            firstDim,
                            secondDim, thirdDim);
                    }
                }
            }

            this.capacity = newCapacity;
        }

        /// <summary>
        /// Expande a ordenação até ao valor especificado.
        /// </summary>
        /// <param name="array">A ordenação.</param>
        /// <param name="thirdDim">O valor especificado.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[] ExpandArray<T>(T[] array, ulong thirdDim)
        {
            var newArray = new T[thirdDim];
            Array.Copy(array, newArray, array.Length);
            return newArray;
        }

        /// <summary>
        /// Expande a dupla ordenação até aos valores especificados.
        /// </summary>
        /// <remarks>
        /// O tamanho exterior refere-se apenas ao tamanho das ordenações
        /// completamente preenchidas, sendo automaticamente aumentado em uma unidade
        /// no caso do terceiro parâmetro.
        /// </remarks>
        /// <param name="doubleArray">A dupla ordenação.</param>
        /// <param name="secondDim">O tamanho da ordenação exterior.</param>
        /// <param name="thirdDim">O tamanho da última ordenação interior.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][] ExpandDoubleArray<T>(
            T[][] doubleArray,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[secondDim + 1][];
            var outLength = doubleArray.LongLength;
            Array.Copy(doubleArray, result, outLength);
            var size = mask + 1;
            for (var i = (ulong)outLength; i < secondDim; ++i)
            {
                result[i] = new T[size];
            }

            result[secondDim] = new T[thirdDim];

            --outLength;
            var last = doubleArray[outLength];
            if (last.Length < size)
            {
                last = this.ExpandArray(last, size);
                result[outLength] = last;
            }

            return result;
        }

        /// <summary>
        /// Expande a ordenação dupla até ao máximo em cada entrada.
        /// </summary>
        /// <param name="doubleArray">A ordenação dupla.</param>
        /// <param name="secondDim">O novo tamanho da expansão.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][] ExpandDoubleArray<T>(
            T[][] doubleArray,
            ulong secondDim)
        {
            var result = new T[secondDim][];
            var outLength = doubleArray.LongLength;
            Array.Copy(doubleArray, result, outLength);
            var size = mask + 1;
            for (var i = (ulong)outLength; i < secondDim; ++i)
            {
                result[i] = new T[size];
            }

            --outLength;
            var last = doubleArray[outLength];
            if (last.Length < size)
            {
                last = this.ExpandArray(last, size);
                result[outLength] = last;
            }

            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla ate ao máximo permitido.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla.</param>
        /// <param name="firstDim">O tamanho exterior máximo.</param>
        /// <returns>A ordenação tripla expandida.</returns>
        private T[][][] ExpandTripleArray<T>(
            T[][][] tripleArray,
            ulong firstDim)
        {
            var result = new T[firstDim][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla até ao máximo permitido, adendando um novo item
        /// especificado pelo parâmetro.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser expandida.</param>
        /// <param name="firstDim">O tamanho exterior que contém o máximo a ser atribuído.</param>
        /// <param name="secondDim">O tamanho da entrada interior a ser atribuída.</param>
        /// <returns>A ordenação tripla expandida.</returns>
        private T[][][] ExpandTripleArray<T>(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            var newInnerArray = new T[secondDim][];
            result[firstDim] = newInnerArray;
            for (var i = 0UL; i < secondDim; ++i)
            {
                newInnerArray[i] = new T[size];
            }

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        /// <summary>
        /// Expande a ordenação tripla até ao máximo permitido, adendando
        /// um item cujo tamanho é especificado pelos restantes parâmetros.
        /// </summary>
        /// <param name="tripleArray">A ordenação tripla a ser expandida.</param>
        /// <param name="firstDim">O tamanho exterior que contém o máximo a ser atribuído.</param>
        /// <param name="secondDim">O tamanho interior que contém o máximo a ser atribuído.</param>
        /// <param name="thirdDim">O tamanho do item interior final.</param>
        /// <returns>A ordenação expandida.</returns>
        private T[][][] ExpandTripleArray<T>(
            T[][][] tripleArray,
            ulong firstDim,
            ulong secondDim,
            ulong thirdDim)
        {
            var result = new T[firstDim + 1][][];
            var tripleLength = tripleArray.LongLength;
            Array.Copy(tripleArray, result, tripleLength);
            var generalSize = generalMask + 1;
            var size = mask + 1;
            for (var i = (ulong)tripleLength; i < firstDim; ++i)
            {
                var newArray = new T[generalSize][];
                result[i] = newArray;
                for (int j = 0; j < generalSize; ++j)
                {
                    newArray[j] = new T[size];
                }
            }

            var newInnerArray = new T[secondDim + 1][];
            result[firstDim] = newInnerArray;
            for (var i = 0UL; i < secondDim; ++i)
            {
                newInnerArray[i] = new T[size];
            }

            newInnerArray[secondDim] = new T[thirdDim];

            --tripleLength;
            var outerCurrent = result[tripleLength];
            if (outerCurrent.Length < generalSize)
            {
                result[tripleLength] = this.ExpandDoubleArray(
                    outerCurrent,
                    generalSize);
            }
            else
            {
                var genSize = generalSize - 1;
                var innerCurrent = outerCurrent[genSize];
                if (innerCurrent.LongLength < size)
                {
                    outerCurrent[genSize] = this.ExpandArray(
                        innerCurrent,
                        size);
                }
            }

            return result;
        }

        #region Classes auxiliares

        /// <summary>
        /// Implementa o enumerador do dicionário.
        /// </summary>
        public class Enumerator
            : IEnumerator<KeyValuePair<TKey, TValue>>,
            IDictionaryEnumerator
        {
            /// <summary>
            /// Mantém o dicionário.
            /// </summary>
            private GeneralDictionary<TKey, TValue> dictionary;

            /// <summary>
            /// Mantém o primeiro índice apontador.
            /// </summary>
            private long firstIndex;

            /// <summary>
            /// Mantém o segundo índice apontador.
            /// </summary>
            private long secondIndex;

            /// <summary>
            /// Mantém o terceiro índice apontador.
            /// </summary>
            private long thirdIndex;

            /// <summary>
            /// Valor que indica que está antes do início.
            /// </summary>
            private bool isBeforeStart;

            /// <summary>
            /// Valor que indica que está após o final.
            /// </summary>
            private bool isAfterEnd;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="Enumerator"/>.
            /// </summary>
            /// <param name="dictionary">O dicionário.</param>
            internal Enumerator(GeneralDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
                this.firstIndex = 0;
                this.secondIndex = 0;
                this.thirdIndex = -1;
                this.isAfterEnd = false;
                this.isBeforeStart = true;
            }

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="Enumerator"/>.
            /// </summary>
            /// <param name="dictionary">O dicionário.</param>
            /// <param name="firstIndex">O primeiro índice apontador.</param>
            /// <param name="secondIndex">O segundo índice apontador.</param>
            /// <param name="thirdIndex">O terceiro índice apontador.</param>
            internal Enumerator(
                GeneralDictionary<TKey, TValue> dictionary,
                long firstIndex,
                long secondIndex,
                long thirdIndex)
            {
                this.dictionary = dictionary;
                this.firstIndex = firstIndex;
                this.secondIndex = secondIndex;
                this.thirdIndex = thirdIndex - 1;
                this.isAfterEnd = false;
                this.isBeforeStart = true;
            }

            /// <summary>
            /// Obtém o valor actual apontado pelo enumerador.
            /// </summary>
            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    if (this.isBeforeStart)
                    {
                        throw new CollectionsException("Enumerator wasn't started.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new CollectionsException("Enumerator has terminated.");
                    }
                    else
                    {
                        var secondEntry = this.dictionary.entries[this.firstIndex];
                        var thirdEntry = secondEntry[this.secondIndex];
                        var entry = thirdEntry[this.thirdIndex];
                        return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                    }
                }
            }

            /// <summary>
            /// Obtém o valor actual apontado pelo enumerador.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    if (this.isBeforeStart)
                    {
                        throw new CollectionsException("Enumerator wasn't started.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new CollectionsException("Enumerator has terminated.");
                    }
                    else
                    {
                        var secondEntry = this.dictionary.entries[this.firstIndex];
                        var thirdEntry = secondEntry[this.secondIndex];
                        var entry = thirdEntry[this.thirdIndex];
                        return new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
                    }
                }
            }

            /// <summary>
            /// Obtém a entrada actual do enumerador.
            /// </summary>
            public DictionaryEntry Entry
            {
                get
                {
                    if (this.isBeforeStart)
                    {
                        throw new CollectionsException("Enumerator wasn't started.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new CollectionsException("Enumerator has terminated.");
                    }
                    else
                    {
                        var secondEntry = this.dictionary.entries[this.firstIndex];
                        var thirdEntry = secondEntry[this.secondIndex];
                        var entry = thirdEntry[this.thirdIndex];
                        return new DictionaryEntry(entry.Key, entry.Value);
                    }
                }
            }

            /// <summary>
            /// Obtém a chave actual do enumerador.
            /// </summary>
            public object Key
            {
                get
                {
                    if (this.isBeforeStart)
                    {
                        throw new CollectionsException("Enumerator wasn't started.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new CollectionsException("Enumerator has terminated.");
                    }
                    else
                    {
                        var secondEntry = this.dictionary.entries[this.firstIndex];
                        var thirdEntry = secondEntry[this.secondIndex];
                        var entry = thirdEntry[this.thirdIndex];
                        return entry.Key;
                    }
                }
            }

            /// <summary>
            /// Obtém o valor actual do eumerador.
            /// </summary>
            public object Value
            {
                get
                {
                    if (this.isBeforeStart)
                    {
                        throw new CollectionsException("Enumerator wasn't started.");
                    }
                    else if (this.isAfterEnd)
                    {
                        throw new CollectionsException("Enumerator has terminated.");
                    }
                    else
                    {
                        var secondEntry = this.dictionary.entries[this.firstIndex];
                        var thirdEntry = secondEntry[this.secondIndex];
                        var entry = thirdEntry[this.thirdIndex];
                        return entry.Value;
                    }
                }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// Move o enumerador para o próximo elemento.
            /// </summary>
            /// <returns>Verdadeiro caso o enumerador seja movido e falso caso se encontre no final.</returns>
            public bool MoveNext()
            {
                this.isBeforeStart = false;
                var firstLength = this.dictionary.entries.LongLength;
                var firstEntry = this.dictionary.entries[this.firstIndex];
                var secondLength = firstEntry.LongLength;
                var secondEntry = firstEntry[this.secondIndex];
                var thirdLength = secondEntry.LongLength;
                var state = !this.isAfterEnd;
                while (state)
                {
                    ++thirdIndex;
                    if (thirdIndex < thirdLength)
                    {
                        var entry = secondEntry[this.thirdIndex];
                        if (entry != null && entry.HashCode.HasValue)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        ++this.secondIndex;
                        if (this.secondIndex < secondLength)
                        {
                            secondEntry = firstEntry[this.secondIndex];
                            thirdLength = secondEntry.LongLength;
                            this.thirdIndex = -1;
                        }
                        else
                        {
                            ++this.firstIndex;
                            this.secondIndex = 0;
                            if (this.firstIndex < firstLength)
                            {
                                firstEntry = this.dictionary.entries[this.firstIndex];
                                secondLength = firstEntry.LongLength;
                                secondEntry = firstEntry[0];
                                thirdLength = secondEntry.LongLength;
                                this.thirdIndex = -1;
                            }
                            else
                            {
                                this.isAfterEnd = true;
                                state = false;
                            }
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// Reestabelece o enumerador.
            /// </summary>
            public void Reset()
            {
                this.firstIndex = 0;
                this.secondIndex = 0;
                this.thirdIndex = -1;
                this.isBeforeStart = true;
                this.isAfterEnd = false;
            }

            /// <summary>
            /// Move o enumerador para o próximo elemento.
            /// </summary>
            /// <returns>Verdadeiro caso o enumerador seja movido e falso caso se encontre no final.</returns>
            bool IEnumerator.MoveNext()
            {
                this.isBeforeStart = false;
                var firstLength = this.dictionary.entries.LongLength;
                var firstEntry = this.dictionary.entries[this.firstIndex];
                var secondLength = firstEntry.LongLength;
                var secondEntry = firstEntry[this.secondIndex];
                var thirdLength = secondEntry.LongLength;
                var state = !this.isAfterEnd;
                while (state)
                {
                    ++thirdIndex;
                    if (thirdIndex < thirdLength)
                    {
                        var entry = secondEntry[this.thirdIndex];
                        if (entry != null && entry.HashCode.HasValue)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        ++this.secondIndex;
                        if (this.secondIndex < secondLength)
                        {
                            secondEntry = firstEntry[this.secondIndex];
                            thirdLength = secondEntry.LongLength;
                            this.thirdIndex = -1;
                        }
                        else
                        {
                            ++this.firstIndex;
                            this.secondIndex = 0;
                            if (this.firstIndex < firstLength)
                            {
                                firstEntry = this.dictionary.entries[this.firstIndex];
                                secondLength = firstEntry.LongLength;
                                secondEntry = firstEntry[0];
                                thirdLength = secondEntry.LongLength;
                                this.thirdIndex = -1;
                            }
                            else
                            {
                                this.isAfterEnd = true;
                                state = false;
                            }
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// Reestabelece o enumerador.
            /// </summary>
            void IEnumerator.Reset()
            {
                this.firstIndex = 0;
                this.secondIndex = 0;
                this.thirdIndex = -1;
                this.isBeforeStart = true;
                this.isAfterEnd = false;
            }
        }

        /// <summary>
        /// Representa uma colecção de chaves.
        /// </summary>
        public sealed class KeyCollection : ICollection<TKey>, ICollection
        {
            /// <summary>
            /// Mantém o dicionário sobre o qual incide a colecção.
            /// </summary>
            private GeneralDictionary<TKey, TValue> dictionary;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="KeyCollection"/>.
            /// </summary>
            /// <param name="dictionary">O dicionário.</param>
            public KeyCollection(GeneralDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            /// <summary>
            /// Obtém o número de elementos da colecção.
            /// </summary>
            public int Count
            {
                get
                {
                    return this.dictionary.Count;
                }
            }

            /// <summary>
            /// Obém um valor que indica se a colecção é apenas de leitura.
            /// </summary>
            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Obtém um valor que indica se a colecção é sincronizada.
            /// </summary>
            public bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Obtém o objecto de sincronização da colecção.
            /// </summary>
            public object SyncRoot
            {
                get
                {
                    return ((ICollection)this.dictionary).SyncRoot;
                }
            }

            /// <summary>
            /// Adiciona um item à colecção.
            /// </summary>
            /// <param name="item">O item.</param>
            public void Add(TKey item)
            {
                throw new NotSupportedException("Can't add to key collection.");
            }

            /// <summary>
            /// Remove todos os itens da colecção.
            /// </summary>
            public void Clear()
            {
                throw new NotSupportedException("Can't clear key collection.");
            }

            /// <summary>
            /// Obtém um valor que indica se a colecção contém o item especificado.
            /// </summary>
            /// <param name="item">O item.</param>
            /// <returns>Verdadeiro se a colecção contiver o item e falso caso contrário.</returns>
            public bool Contains(TKey item)
            {
                return this.dictionary.ContainsKey(item);
            }

            /// <summary>
            /// Remove o item especificado da colecção.
            /// </summary>
            /// <param name="item">O item a ser removido.</param>
            /// <returns>Verdadeiro se o item for removido e falso caso contrário.</returns>
            public bool Remove(TKey item)
            {
                throw new NotSupportedException("Can't remove from key collection.");
            }

            /// <summary>
            /// Copia os valores da colecção para um vector.
            /// </summary>
            /// <param name="array">O vector.</param>
            /// <param name="arrayIndex">O índice do vector a partir do qual é iniciada a cópia.</param>
            public void CopyTo(TKey[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                else if (arrayIndex < 0 || arrayIndex >= array.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        "arrayIndex", "Array index must be greater than zero and less than the size of array.");
                }
                else if (array.Length - arrayIndex < this.dictionary.Count)
                {
                    throw new ArgumentException("The size of array plus offset is to small to contain the copy.");
                }
                else
                {
                    var index = arrayIndex;
                    var entries = this.dictionary.entries;
                    var firstLength = entries.LongLength;
                    for (var i = 0; i < firstLength; ++i)
                    {
                        var secondEntry = entries[i];
                        var secondLength = secondEntry.LongLength;
                        for (var j = 0; j < secondLength; ++j)
                        {
                            var thirdEntry = secondEntry[j];
                            var thirdLength = thirdEntry.LongLength;
                            for (var k = 0; k < thirdLength; ++k)
                            {
                                var entry = thirdEntry[k];
                                if (entry != null && entry.HashCode.HasValue)
                                {
                                    array[index++] = entry.Key;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Copia os valores da colecção para o vector.
            /// </summary>
            /// <param name="array">O vector.</param>
            /// <param name="index">O índice a partir do qual é iniciada a cópia.</param>
            public void CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                else if (array.Rank != 1)
                {
                    throw new ArgumentException("Multidimensional arrays are not supported.");
                }
                else if (array.GetLowerBound(0) != 0)
                {
                    throw new ArgumentException("Lower bound must be zero.");
                }
                else if (index < 0 || index >= array.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        "index", "Array index must be greater than zero and less than the size of array.");
                }
                else if (array.Length - index < this.dictionary.Count)
                {
                    throw new ArgumentException("The size of array plus offset is to small to contain the copy.");
                }
                else
                {
                    var arrayIndex = index;
                    var entries = this.dictionary.entries;
                    var firstLength = entries.LongLength;
                    for (var i = 0; i < firstLength; ++i)
                    {
                        var secondEntry = entries[i];
                        var secondLength = secondEntry.LongLength;
                        for (var j = 0; j < secondLength; ++j)
                        {
                            var thirdEntry = secondEntry[j];
                            var thirdLength = thirdEntry.LongLength;
                            for (var k = 0; k < thirdLength; ++k)
                            {
                                var entry = thirdEntry[k];
                                if (entry != null && entry.HashCode.HasValue)
                                {
                                    array.SetValue(entry.Key, arrayIndex++);
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Obém o enumerador para a colecção.
            /// </summary>
            /// <returns>O enumerador.</returns>
            public IEnumerator<TKey> GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            /// <summary>
            /// Obtém o enumerador para a colecção.
            /// </summary>
            /// <returns>O enumerador.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            /// <summary>
            /// Implementa um enumerador para o conjunto das chaves.
            /// </summary>
            public class Enumerator : IEnumerator<TKey>, IEnumerator
            {
                /// <summary>
                /// Mantém o dicionário.
                /// </summary>
                private GeneralDictionary<TKey, TValue> dictionary;

                /// <summary>
                /// Mantém o primeiro índice apontador.
                /// </summary>
                private long firstIndex;

                /// <summary>
                /// Mantém o segundo índice apontador.
                /// </summary>
                private long secondIndex;

                /// <summary>
                /// Mantém o terceiro índice apontador.
                /// </summary>
                private long thirdIndex;

                /// <summary>
                /// Valor que indica que está antes do início.
                /// </summary>
                private bool isBeforeStart;

                /// <summary>
                /// Valor que indica que está após o final.
                /// </summary>
                private bool isAfterEnd;

                /// <summary>
                /// Instancia uma nova instância de objectos do tipo <see cref="Enumerator"/>.
                /// </summary>
                /// <param name="dictionary">O dicionário.</param>
                internal Enumerator(GeneralDictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.firstIndex = 0;
                    this.secondIndex = 0;
                    this.thirdIndex = -1;
                    this.isAfterEnd = false;
                    this.isBeforeStart = true;
                }

                /// <summary>
                /// Instancia uma nova instância de objectos do tipo <see cref="Enumerator"/>.
                /// </summary>
                /// <param name="dictionary">O dicionário.</param>
                /// <param name="firstIndex">O primeiro índice apontador.</param>
                /// <param name="secondIndex">O segundo índice apontador.</param>
                /// <param name="thirdIndex">O terceiro índice apontador.</param>
                internal Enumerator(
                    GeneralDictionary<TKey, TValue> dictionary,
                    long firstIndex,
                    long secondIndex,
                    long thirdIndex)
                {
                    this.dictionary = dictionary;
                    this.firstIndex = firstIndex;
                    this.secondIndex = secondIndex;
                    this.thirdIndex = thirdIndex - 1;
                    this.isAfterEnd = false;
                    this.isBeforeStart = true;
                }

                /// <summary>
                /// Obtém o valor actual apontado pelo enumerador.
                /// </summary>
                public TKey Current
                {
                    get
                    {
                        if (this.isBeforeStart)
                        {
                            throw new CollectionsException("Enumerator wasn't started.");
                        }
                        else if (this.isAfterEnd)
                        {
                            throw new CollectionsException("Enumerator has terminated.");
                        }
                        else
                        {
                            var secondEntry = this.dictionary.entries[this.firstIndex];
                            var thirdEntry = secondEntry[this.secondIndex];
                            var entry = thirdEntry[this.thirdIndex];
                            return entry.Key;
                        }
                    }
                }

                /// <summary>
                /// Obtém o valor actual apontado pelo enumerador.
                /// </summary>
                object IEnumerator.Current
                {
                    get
                    {
                        if (this.isBeforeStart)
                        {
                            throw new CollectionsException("Enumerator wasn't started.");
                        }
                        else if (this.isAfterEnd)
                        {
                            throw new CollectionsException("Enumerator has terminated.");
                        }
                        else
                        {
                            var secondEntry = this.dictionary.entries[this.firstIndex];
                            var thirdEntry = secondEntry[this.secondIndex];
                            var entry = thirdEntry[this.thirdIndex];
                            return entry.Key;
                        }
                    }
                }

                /// <summary>
                /// Descarta o enumerador.
                /// </summary>
                public void Dispose()
                {
                }

                /// <summary>
                /// Move o enumerador para o próximo elemento.
                /// </summary>
                /// <returns>Verdadeiro caso o enumerador seja movido e falso caso se encontre no final.</returns>
                public bool MoveNext()
                {
                    this.isBeforeStart = false;
                    var firstLength = this.dictionary.entries.LongLength;
                    var firstEntry = this.dictionary.entries[this.firstIndex];
                    var secondLength = firstEntry.LongLength;
                    var secondEntry = firstEntry[this.secondIndex];
                    var thirdLength = secondEntry.LongLength;
                    var state = !this.isAfterEnd;
                    while (state)
                    {
                        ++thirdIndex;
                        if (thirdIndex < thirdLength)
                        {
                            var entry = secondEntry[this.thirdIndex];
                            if (entry != null && entry.HashCode.HasValue)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            ++this.secondIndex;
                            if (this.secondIndex < secondLength)
                            {
                                secondEntry = firstEntry[this.secondIndex];
                                thirdLength = secondEntry.LongLength;
                                this.thirdIndex = -1;
                            }
                            else
                            {
                                ++this.firstIndex;
                                this.secondIndex = 0;
                                if (this.firstIndex < firstLength)
                                {
                                    firstEntry = this.dictionary.entries[this.firstIndex];
                                    secondLength = firstEntry.LongLength;
                                    secondEntry = firstEntry[0];
                                    thirdLength = secondEntry.LongLength;
                                    this.thirdIndex = -1;
                                }
                                else
                                {
                                    this.isAfterEnd = true;
                                    state = false;
                                }
                            }
                        }
                    }

                    return false;
                }

                /// <summary>
                /// Reestabelece o enumerador.
                /// </summary>
                public void Reset()
                {
                    this.firstIndex = 0;
                    this.secondIndex = 0;
                    this.thirdIndex = -1;
                    this.isBeforeStart = true;
                    this.isAfterEnd = false;
                }

                /// <summary>
                /// Move o enumerador para o próximo elemento.
                /// </summary>
                /// <returns>Verdadeiro caso o enumerador seja movido e falso caso se encontre no final.</returns>
                bool IEnumerator.MoveNext()
                {
                    this.isBeforeStart = false;
                    var firstLength = this.dictionary.entries.LongLength;
                    var firstEntry = this.dictionary.entries[this.firstIndex];
                    var secondLength = firstEntry.LongLength;
                    var secondEntry = firstEntry[this.secondIndex];
                    var thirdLength = secondEntry.LongLength;
                    var state = !this.isAfterEnd;
                    while (state)
                    {
                        ++thirdIndex;
                        if (thirdIndex < thirdLength)
                        {
                            var entry = secondEntry[this.thirdIndex];
                            if (entry != null && entry.HashCode.HasValue)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            ++this.secondIndex;
                            if (this.secondIndex < secondLength)
                            {
                                secondEntry = firstEntry[this.secondIndex];
                                thirdLength = secondEntry.LongLength;
                                this.thirdIndex = -1;
                            }
                            else
                            {
                                ++this.firstIndex;
                                this.secondIndex = 0;
                                if (this.firstIndex < firstLength)
                                {
                                    firstEntry = this.dictionary.entries[this.firstIndex];
                                    secondLength = firstEntry.LongLength;
                                    secondEntry = firstEntry[0];
                                    thirdLength = secondEntry.LongLength;
                                    this.thirdIndex = -1;
                                }
                                else
                                {
                                    this.isAfterEnd = true;
                                    state = false;
                                }
                            }
                        }
                    }

                    return false;
                }

                /// <summary>
                /// Reestabelece o enumerador.
                /// </summary>
                void IEnumerator.Reset()
                {
                    this.firstIndex = 0;
                    this.secondIndex = 0;
                    this.thirdIndex = -1;
                    this.isBeforeStart = true;
                    this.isAfterEnd = false;
                }
            }
        }

        /// <summary>
        /// Representa uma colecção de valores.
        /// </summary>
        public sealed class ValueCollection : ICollection<TValue>, ICollection
        {
            /// <summary>
            /// Mantém o dicionário sobre o qual incide a colecção.
            /// </summary>
            private GeneralDictionary<TKey, TValue> dictionary;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="ValueCollection"/>.
            /// </summary>
            /// <param name="dictionary">O dicionário.</param>
            public ValueCollection(GeneralDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            /// <summary>
            /// Obtém o número de elementos da colecção.
            /// </summary>
            public int Count
            {
                get
                {
                    return this.dictionary.Count;
                }
            }

            /// <summary>
            /// Obém um valor que indica se a colecção é apenas de leitura.
            /// </summary>
            public bool IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// Obtém um valor que indica se a colecção é sincronizada.
            /// </summary>
            public bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// Obtém o objecto de sincronização da colecção.
            /// </summary>
            public object SyncRoot
            {
                get
                {
                    return ((ICollection)this.dictionary).SyncRoot;
                }
            }

            /// <summary>
            /// Adiciona um item à colecção.
            /// </summary>
            /// <param name="item">O item.</param>
            public void Add(TValue item)
            {
                throw new NotSupportedException("Can't add to key collection.");
            }

            /// <summary>
            /// Remove todos os itens da colecção.
            /// </summary>
            public void Clear()
            {
                throw new NotSupportedException("Can't clear key collection.");
            }

            /// <summary>
            /// Obtém um valor que indica se a colecção contém o item especificado.
            /// </summary>
            /// <param name="item">O item.</param>
            /// <returns>Verdadeiro se a colecção contiver o item e falso caso contrário.</returns>
            public bool Contains(TValue item)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Remove o item especificado da colecção.
            /// </summary>
            /// <param name="item">O item a ser removido.</param>
            /// <returns>Verdadeiro se o item for removido e falso caso contrário.</returns>
            public bool Remove(TValue item)
            {
                throw new NotSupportedException("Can't remove from key collection.");
            }

            /// <summary>
            /// Copia os valores da colecção para um vector.
            /// </summary>
            /// <param name="array">O vector.</param>
            /// <param name="arrayIndex">O índice do vector a partir do qual é iniciada a cópia.</param>
            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                else if (arrayIndex < 0 || arrayIndex >= array.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        "arrayIndex", "Array index must be greater than zero and less than the size of array.");
                }
                else if (array.Length - arrayIndex < this.dictionary.Count)
                {
                    throw new ArgumentException("The size of array plus offset is to small to contain the copy.");
                }
                else
                {
                    var index = arrayIndex;
                    var entries = this.dictionary.entries;
                    var firstLength = entries.LongLength;
                    for (var i = 0; i < firstLength; ++i)
                    {
                        var secondEntry = entries[i];
                        var secondLength = secondEntry.LongLength;
                        for (var j = 0; j < secondLength; ++j)
                        {
                            var thirdEntry = secondEntry[j];
                            var thirdLength = thirdEntry.LongLength;
                            for (var k = 0; k < thirdLength; ++k)
                            {
                                var entry = thirdEntry[k];
                                if (entry != null && entry.HashCode.HasValue)
                                {
                                    array[index++] = entry.Value;
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Copia os valores da colecção para o vector.
            /// </summary>
            /// <param name="array">O vector.</param>
            /// <param name="index">O índice a partir do qual é iniciada a cópia.</param>
            public void CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                else if (array.Rank != 1)
                {
                    throw new ArgumentException("Multidimensional arrays are not supported.");
                }
                else if (array.GetLowerBound(0) != 0)
                {
                    throw new ArgumentException("Lower bound must be zero.");
                }
                else if (index < 0 || index >= array.Length)
                {
                    throw new ArgumentOutOfRangeException(
                        "index", "Array index must be greater than zero and less than the size of array.");
                }
                else if (array.Length - index < this.dictionary.Count)
                {
                    throw new ArgumentException("The size of array plus offset is to small to contain the copy.");
                }
                else
                {
                    var arrayIndex = index;
                    var entries = this.dictionary.entries;
                    var firstLength = entries.LongLength;
                    for (var i = 0; i < firstLength; ++i)
                    {
                        var secondEntry = entries[i];
                        var secondLength = secondEntry.LongLength;
                        for (var j = 0; j < secondLength; ++j)
                        {
                            var thirdEntry = secondEntry[j];
                            var thirdLength = thirdEntry.LongLength;
                            for (var k = 0; k < thirdLength; ++k)
                            {
                                var entry = thirdEntry[k];
                                if (entry != null && entry.HashCode.HasValue)
                                {
                                    array.SetValue(entry.Value, arrayIndex++);
                                }
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Obém o enumerador para a colecção.
            /// </summary>
            /// <returns>O enumerador.</returns>
            public IEnumerator<TValue> GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            /// <summary>
            /// Obtém o enumerador para a colecção.
            /// </summary>
            /// <returns>O enumerador.</returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this.dictionary);
            }

            /// <summary>
            /// Implementa um enumerador para o conjunto das chaves.
            /// </summary>
            public class Enumerator : IEnumerator<TValue>, IEnumerator
            {
                /// <summary>
                /// Mantém o dicionário.
                /// </summary>
                private GeneralDictionary<TKey, TValue> dictionary;

                /// <summary>
                /// Mantém o primeiro índice apontador.
                /// </summary>
                private long firstIndex;

                /// <summary>
                /// Mantém o segundo índice apontador.
                /// </summary>
                private long secondIndex;

                /// <summary>
                /// Mantém o terceiro índice apontador.
                /// </summary>
                private long thirdIndex;

                /// <summary>
                /// Valor que indica que está antes do início.
                /// </summary>
                private bool isBeforeStart;

                /// <summary>
                /// Valor que indica que está após o final.
                /// </summary>
                private bool isAfterEnd;

                /// <summary>
                /// Instancia uma nova instância de objectos do tipo <see cref="Enumerator"/>.
                /// </summary>
                /// <param name="dictionary">O dicionário.</param>
                internal Enumerator(GeneralDictionary<TKey, TValue> dictionary)
                {
                    this.dictionary = dictionary;
                    this.firstIndex = 0;
                    this.secondIndex = 0;
                    this.thirdIndex = -1;
                    this.isAfterEnd = false;
                    this.isBeforeStart = true;
                }

                /// <summary>
                /// Instancia uma nova instância de objectos do tipo <see cref="Enumerator"/>.
                /// </summary>
                /// <param name="dictionary">O dicionário.</param>
                /// <param name="firstIndex">O primeiro índice apontador.</param>
                /// <param name="secondIndex">O segundo índice apontador.</param>
                /// <param name="thirdIndex">O terceiro índice apontador.</param>
                internal Enumerator(
                    GeneralDictionary<TKey, TValue> dictionary,
                    long firstIndex,
                    long secondIndex,
                    long thirdIndex)
                {
                    this.dictionary = dictionary;
                    this.firstIndex = firstIndex;
                    this.secondIndex = secondIndex;
                    this.thirdIndex = thirdIndex - 1;
                    this.isAfterEnd = false;
                    this.isBeforeStart = true;
                }

                /// <summary>
                /// Obtém o valor actual apontado pelo enumerador.
                /// </summary>
                public TValue Current
                {
                    get
                    {
                        if (this.isBeforeStart)
                        {
                            throw new CollectionsException("Enumerator wasn't started.");
                        }
                        else if (this.isAfterEnd)
                        {
                            throw new CollectionsException("Enumerator has terminated.");
                        }
                        else
                        {
                            var secondEntry = this.dictionary.entries[this.firstIndex];
                            var thirdEntry = secondEntry[this.secondIndex];
                            var entry = thirdEntry[this.thirdIndex];
                            return entry.Value;
                        }
                    }
                }

                /// <summary>
                /// Obtém o valor actual apontado pelo enumerador.
                /// </summary>
                object IEnumerator.Current
                {
                    get
                    {
                        if (this.isBeforeStart)
                        {
                            throw new CollectionsException("Enumerator wasn't started.");
                        }
                        else if (this.isAfterEnd)
                        {
                            throw new CollectionsException("Enumerator has terminated.");
                        }
                        else
                        {
                            var secondEntry = this.dictionary.entries[this.firstIndex];
                            var thirdEntry = secondEntry[this.secondIndex];
                            var entry = thirdEntry[this.thirdIndex];
                            return entry.Value;
                        }
                    }
                }

                /// <summary>
                /// Descarta o enumerador.
                /// </summary>
                public void Dispose()
                {
                }

                /// <summary>
                /// Move o enumerador para o próximo elemento.
                /// </summary>
                /// <returns>Verdadeiro caso o enumerador seja movido e falso caso se encontre no final.</returns>
                public bool MoveNext()
                {
                    this.isBeforeStart = false;
                    var firstLength = this.dictionary.entries.LongLength;
                    var firstEntry = this.dictionary.entries[this.firstIndex];
                    var secondLength = firstEntry.LongLength;
                    var secondEntry = firstEntry[this.secondIndex];
                    var thirdLength = secondEntry.LongLength;
                    var state = !this.isAfterEnd;
                    while (state)
                    {
                        ++thirdIndex;
                        if (thirdIndex < thirdLength)
                        {
                            var entry = secondEntry[this.thirdIndex];
                            if (entry != null && entry.HashCode.HasValue)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            ++this.secondIndex;
                            if (this.secondIndex < secondLength)
                            {
                                secondEntry = firstEntry[this.secondIndex];
                                thirdLength = secondEntry.LongLength;
                                this.thirdIndex = -1;
                            }
                            else
                            {
                                ++this.firstIndex;
                                this.secondIndex = 0;
                                if (this.firstIndex < firstLength)
                                {
                                    firstEntry = this.dictionary.entries[this.firstIndex];
                                    secondLength = firstEntry.LongLength;
                                    secondEntry = firstEntry[0];
                                    thirdLength = secondEntry.LongLength;
                                    this.thirdIndex = -1;
                                }
                                else
                                {
                                    this.isAfterEnd = true;
                                    state = false;
                                }
                            }
                        }
                    }

                    return false;
                }

                /// <summary>
                /// Reestabelece o enumerador.
                /// </summary>
                public void Reset()
                {
                    this.firstIndex = 0;
                    this.secondIndex = 0;
                    this.thirdIndex = -1;
                    this.isBeforeStart = true;
                    this.isAfterEnd = false;
                }

                /// <summary>
                /// Move o enumerador para o próximo elemento.
                /// </summary>
                /// <returns>Verdadeiro caso o enumerador seja movido e falso caso se encontre no final.</returns>
                bool IEnumerator.MoveNext()
                {
                    this.isBeforeStart = false;
                    var firstLength = this.dictionary.entries.LongLength;
                    var firstEntry = this.dictionary.entries[this.firstIndex];
                    var secondLength = firstEntry.LongLength;
                    var secondEntry = firstEntry[this.secondIndex];
                    var thirdLength = secondEntry.LongLength;
                    var state = !this.isAfterEnd;
                    while (state)
                    {
                        ++thirdIndex;
                        if (thirdIndex < thirdLength)
                        {
                            var entry = secondEntry[this.thirdIndex];
                            if (entry != null && entry.HashCode.HasValue)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            ++this.secondIndex;
                            if (this.secondIndex < secondLength)
                            {
                                secondEntry = firstEntry[this.secondIndex];
                                thirdLength = secondEntry.LongLength;
                                this.thirdIndex = -1;
                            }
                            else
                            {
                                ++this.firstIndex;
                                this.secondIndex = 0;
                                if (this.firstIndex < firstLength)
                                {
                                    firstEntry = this.dictionary.entries[this.firstIndex];
                                    secondLength = firstEntry.LongLength;
                                    secondEntry = firstEntry[0];
                                    thirdLength = secondEntry.LongLength;
                                    this.thirdIndex = -1;
                                }
                                else
                                {
                                    this.isAfterEnd = true;
                                    state = false;
                                }
                            }
                        }
                    }

                    return false;
                }

                /// <summary>
                /// Reestabelece o enumerador.
                /// </summary>
                void IEnumerator.Reset()
                {
                    this.firstIndex = 0;
                    this.secondIndex = 0;
                    this.thirdIndex = -1;
                    this.isBeforeStart = true;
                    this.isAfterEnd = false;
                }
            }
        }

        /// <summary>
        /// Mantém os dados associados.
        /// </summary>
        private class Entry
        {
            /// <summary>
            /// Mantém o código confuso da chave.
            /// </summary>
            private Nullable<ulong> hashCode;

            /// <summary>
            /// Mantém o índice da próxima entrada.
            /// </summary>
            private Nullable<ulong> next;

            /// <summary>
            /// Mantém a cahve.
            /// </summary>
            private TKey key;

            /// <summary>
            /// Mantém o valor.
            /// </summary>
            private TValue value;

            /// <summary>
            /// Obtém ou atribui o código confuso da chave.
            /// </summary>
            public Nullable<ulong> HashCode
            {
                get
                {
                    return this.hashCode;
                }
                set
                {
                    this.hashCode = value;
                }
            }

            /// <summary>
            /// Obtém ou atriui o índice da próxima entrada.
            /// </summary>
            public Nullable<ulong> Next
            {
                get
                {
                    return this.next;
                }
                set
                {
                    this.next = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui a cahve.
            /// </summary>
            public TKey Key
            {
                get
                {
                    return this.key;
                }
                set
                {
                    this.key = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o valor.
            /// </summary>
            public TValue Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    this.value = value;
                }
            }
        }

        #endregion Classes auxiliares
    }
}
