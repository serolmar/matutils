namespace Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma colecção onde os elementos são mantidos de forma ordenada.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos na colecção ordenada.</typeparam>
    public class InsertionSortedCollection<T> : ICollection<T>, ICollection
    {
        /// <summary>
        /// O contentor.
        /// </summary>
        private List<T> elements = new List<T>();

        /// <summary>
        /// O comparador.
        /// </summary>
        private IComparer<T> comparer = null;

        /// <summary>
        /// Valor que indica se os valores repetidos são ignorados.
        /// </summary>
        private bool ignoreRepeated = false;

        /// <summary>
        /// Indica se a colecção actual é só de leitura.
        /// </summary>
        private bool isReadOnly;

        /// <summary>
        /// Instancia uma nova instância da classe <see cref="InsertionSortedCollection{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador que especifica a ordem.</param>
        public InsertionSortedCollection(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                this.comparer = Comparer<T>.Default;
            }
            else
            {
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Instancia uma nova instância da classe <see cref="InsertionSortedCollection{T}"/>.
        /// </summary>
        /// <param name="items">Os itens que a colecção contém.</param>
        /// <param name="isReadOnly">Valor que indica se a colecção é só de leitura.</param>
        /// <param name="comparer">O comparador que especifica a ordem.</param>
        public InsertionSortedCollection(
            IEnumerable<T> items,
            bool isReadOnly,
            IComparer<T> comparer)
        {
            if (comparer == null)
            {
                this.comparer = Comparer<T>.Default;
            }
            else
            {
                this.comparer = comparer;
            }

            this.isReadOnly = isReadOnly;
            if (items != null)
            {
                this.elements.AddRange(items);
                var sorter = new MergeSorter<T>(this.comparer);
                sorter.Sort(this.elements);
            }
        }

        /// <summary>
        /// Instancia uma nova instância da classe <see cref="InsertionSortedCollection{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador que especifica a ordem.</param>
        /// <param name="ignoreRepeated">Valor que indica se são para serem ignorados os valores repetidos.</param>
        public InsertionSortedCollection(IComparer<T> comparer, bool ignoreRepeated)
        {
            if (comparer == null)
            {
                this.comparer = Comparer<T>.Default;
            }
            else
            {
                this.comparer = comparer;
            }

            this.ignoreRepeated = ignoreRepeated;
        }

        /// <summary>
        /// Instancia uma nova instância da classe <see cref="InsertionSortedCollection{T}"/>.
        /// </summary>
        /// <param name="items">Os itens que a colecção contém.</param>
        /// <param name="isReadOnly">Valor que indica se a colecção é só de leitura.</param>
        /// <param name="comparer">The comparer that specifies the ordering.</param>
        /// <param name="ignoreRepeated">Valor que indica se são para serem ignorados os valores repetidos.</param>
        public InsertionSortedCollection(
            IEnumerable<T> items,
            bool isReadOnly,
            IComparer<T> comparer,
            bool ignoreRepeated)
        {
            if (comparer == null)
            {
                this.comparer = Comparer<T>.Default;
            }
            else
            {
                this.comparer = comparer;
            }

            this.ignoreRepeated = ignoreRepeated;
            this.isReadOnly = isReadOnly;
            if (items != null)
            {
                this.elements.AddRange(items);
                var sorter = new MergeSorter<T>(this.comparer);
                sorter.Sort(this.elements);
            }
        }

        /// <summary>
        /// Obtém o elemento que se encontra na posição especificada.
        /// </summary>
        /// <param name="index">O índice do elemento.</param>
        /// <returns>O elemento.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= this.elements.Count)
                {
                    throw new IndexOutOfRangeException(
                        "The parameter index must be non-negative and less the size of the collection.");
                }
                else
                {
                    return this.elements[index];
                }
            }
        }

        /// <summary>
        /// Obtém o número de elementos inseridos.
        /// </summary>
        /// <value>O número de elementos inseridos.</value>
        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        /// <summary>
        /// Obtém o primeiro elemento do conjunto.
        /// </summary>
        /// <value>O elemento.</value>
        /// <exception cref="CollectionsException">Se a colecção se encontrar vazia.</exception>
        public T First
        {
            get
            {
                if (this.elements.Count > 0)
                {
                    return this.elements[0];
                }

                throw new CollectionsException("Empty set.");
            }
        }

        /// <summary>
        /// Obtém o último elemento do conjunto.
        /// </summary>
        /// <value>O elemento.</value>
        /// <exception cref="CollectionsException">Se a colecção se encontrar vazia.</exception>
        public T Last
        {
            get
            {
                if (this.elements.Count > 0)
                {
                    return this.elements[this.elements.Count - 1];
                }

                throw new CollectionsException("Empty set.");
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é só de leitura.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.isReadOnly;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a colecção é segura em termos de linhas de fluxo.
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// O objecto que permite realizar a sincronização.
        /// </summary>
        /// <value>O valor da instância actual.</value>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Insere um elemento na colecção.
        /// </summary>
        /// <param name="item">O elemento a ser inserido.</param>
        /// <returns>Verdadeiro caso o item seja inserido e falso caso contrário.</returns>
        public bool Add(T item)
        {
            if (this.isReadOnly)
            {
                throw new CollectionsException("The collection is readonly.");
            }
            else
            {
                int insertionIndex = this.FindGreatestPosition(item);
                if (insertionIndex == this.elements.Count)
                {
                    this.elements.Add(item);
                    return true;
                }
                else
                {
                    if (this.ignoreRepeated)
                    {
                        if (this.comparer.Compare(item, this.elements[insertionIndex]) != 0)
                        {
                            this.elements.Insert(insertionIndex, item);
                        }
                    }
                    else
                    {
                        if (this.comparer.Compare(item, this.elements[insertionIndex]) == 0)
                        {
                            ++insertionIndex;
                            this.elements.Insert(insertionIndex, item);
                        }
                        else
                        {
                            this.elements.Insert(insertionIndex, item);
                        }
                    }
                }

                return this.ignoreRepeated;
            }
        }

        /// <summary>
        /// Insere um elemento na colecção.
        /// </summary>
        /// <param name="item">O elemento a ser inserido.</param>
        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        /// <summary>
        /// Insere um conjunto de elementos ordenados na colecção.
        /// </summary>
        /// <param name="elementsToInsert">Os elementos a serem inseridos.</param>
        /// <exception cref="ArgumentNullException">Se o conjunto de elementso for nulo.</exception>
        public void AddRange(InsertionSortedCollection<T> elementsToInsert)
        {
            if (elementsToInsert == null)
            {
                throw new ArgumentNullException("elementsToInsert");
            }
            else
            {
                for (int i = 0; i < elementsToInsert.elements.Count; ++i)
                {
                    this.Add(elementsToInsert.elements[i]);
                }
            }
        }

        /// <summary>
        /// Insere um conjunto de elementos dado por um enumerador na colecção.
        /// </summary>
        /// <param name="elementsToInsert">Os elementos a serem inseridos.</param>
        /// <exception cref="ArgumentNullException">Se o conjunto de elementso for nulo.</exception>
        public void AddRange(IEnumerable<T> elementsToInsert)
        {
            if (elementsToInsert == null)
            {
                throw new ArgumentNullException("elementsToInsert");
            }
            else
            {
                foreach (var elementToInsert in elementsToInsert)
                {
                    this.Add(elementToInsert);
                }
            }
        }

        /// <summary>
        /// Verifica se um elemento está contido na colecção.
        /// </summary>
        /// <param name="item">O elemento a ser verificado.</param>
        /// <returns>Verdadeiro caso o elemento exista e falso caso contrário.</returns>
        public bool Contains(T item)
        {
            int index = this.FindPosition(item);
            if (index >= this.elements.Count || index < 0)
            {
                return false;
            }

            return this.comparer.Compare(item, this.elements[index]) == 0;
        }

        /// <summary>
        /// Remove um elemento considerando a ordem.
        /// </summary>
        /// <param name="item">O elemento a ser removido.</param>
        public bool Remove(T item)
        {
            if (this.isReadOnly)
            {
                throw new CollectionsException("The collection is readonly.");
            }
            else
            {
                int index = this.FindLowestPosition(item);
                if (index >= this.elements.Count)
                {
                    return false;
                }
                else
                {
                    this.elements.RemoveAt(index);
                    return true;
                }
            }
        }

        /// <summary>
        /// Remove o elemento que se encontra registado no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        public void RemoveAt(int index)
        {
            if (this.isReadOnly)
            {
                throw new CollectionsException("The collection is readonly.");
            }
            else
            {
                this.elements.RemoveAt(index);
            }
        }

        /// <summary>
        /// Limpa a colecção.
        /// </summary>
        public void Clear()
        {
            if (this.isReadOnly)
            {
                throw new CollectionsException("The collection is readonly.");
            }
            else
            {
                this.elements.Clear();
            }
        }

        /// <summary>
        /// Copia os elementos da colecção para um vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="arrayIndex">O índice onde é iniciada a cópia.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.elements.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copia os elementos da colecção para um vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="index">O índice onde é iniciada a cópia.</param>
        public void CopyTo(Array array, int index)
        {
            ((ICollection)this.elements).CopyTo(array, index);
        }

        /// <summary>
        /// Retorna um vector com os elementos da colecção.
        /// </summary>
        /// <returns>O vector.</returns>
        public T[] ToArray()
        {
            return this.elements.ToArray();
        }

        /// <summary>
        /// Conta o número de itens inferior ao valor especificado.
        /// </summary>
        /// <param name="obj">O valor a ser comparado.</param>
        /// <returns>O número de itens inferiores ao valor especificado.</returns>
        public int CountLessThan(T obj)
        {
            var position = this.FindLowestPosition(obj);
            return position;
        }

        /// <summary>
        /// Conta o número de itens superior ao valor especificado.
        /// </summary>
        /// <param name="obj">O valor a ser comparado.</param>
        /// <returns>O número de itens superior ao valor especificado.</returns>
        public int CountGreatThan(T obj)
        {
            var elementsCount = this.elements.Count;
            var position = this.FindGreatestPosition(obj);
            if (elementsCount == position)
            {
                return 0;
            }
            else
            {
                var current = this.elements[position];
                if (this.comparer.Compare(current, obj) == 0)
                {
                    return elementsCount - position - 1;
                }
                else
                {
                    return elementsCount - position;
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador para todos os elementos da colecção que são iguais ao elemento dadao.
        /// </summary>
        /// <param name="objectToFind">O elemento a comparar.</param>
        /// <returns>Um enumerador para os valores iguais.</returns>
        public IEnumerator<T> GetEqualsTo(T objectToFind)
        {
            int index = this.FindGreatestPosition(objectToFind);
            if (index < 0)
            {
                index = this.elements.Count;
            }

            while (index < this.elements.Count)
            {
                var item = this.elements[index];
                if (this.comparer.Compare(objectToFind, item) == 0)
                {
                    ++index;
                    yield return item;
                }
                else
                {
                    index = this.elements.Count;
                }
            }
        }

        /// <summary>
        /// Obtém o índice do primeiro objecto encontrado na colecção.
        /// </summary>
        /// <param name="objectTofind">O objecto a ser encontrado.</param>
        /// <returns>O índice do objecto caso este exista e -1 caso contrário.</returns>
        public int IndexOf(T objectTofind)
        {
            var result = this.FindLowestPosition(objectTofind);
            if (result >= this.elements.Count)
            {
                return -1;
            }
            else
            {
                var current = this.elements[result];
                if (this.comparer.Compare(objectTofind, current) == 0)
                {
                    return result;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Tenta encontrar um valor que se encontre na lista actual mas não se encontre
        /// na lista especificada.
        /// </summary>
        /// <param name="collection">A lista.</param>
        /// <param name="item">O valor.</param>
        /// <returns>
        /// Verdadeiro caso a lista contenha um valor que não se encontre na outra lista 
        /// e falso caso contrário.
        /// </returns>
        public bool TryFindValueNotIn(
            InsertionSortedCollection<T> collection,
            out T item)
        {
            item = default(T);
            var elementsEnumerator = this.elements.GetEnumerator();
            var collectionElementsEnumerator = collection.elements.GetEnumerator();
            if (elementsEnumerator.MoveNext())
            {
                if (collectionElementsEnumerator.MoveNext())
                {
                    var current = elementsEnumerator.Current;
                    var collectionCurrent = collectionElementsEnumerator.Current;
                    while (true)
                    {
                        if (this.comparer.Compare(collectionCurrent, current) < 0)
                        {
                            if (collectionElementsEnumerator.MoveNext())
                            {
                                collectionCurrent = collectionElementsEnumerator.Current;
                            }
                            else
                            {
                                item = current;
                                return true;
                            }
                        }
                        else if (this.comparer.Compare(current, collectionCurrent) < 0)
                        {
                            item = current;
                            return true;
                        }
                        else if (elementsEnumerator.MoveNext())
                        {
                            current = elementsEnumerator.Current;
                            if (collectionElementsEnumerator.MoveNext())
                            {
                                collectionCurrent = collectionElementsEnumerator.Current;
                            }
                            else
                            {
                                item = current;
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    item = elementsEnumerator.Current;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Obtém um enumerador para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.elements.GetEnumerator();
        }

        /// <summary>
        /// Obtém um enumerador invertido para a colecção.
        /// </summary>
        /// <returns>O enumerador invertido.</returns>
        public IEnumerator<T> GetReversedEnumerator()
        {
            for (int i = this.elements.Count - 1; i >= 0; --i)
            {
                yield return this.elements[i];
            }
        }

        /// <summary>
        /// Determina o índice onde se encontra o objecto especificado ou
        /// onde este poderá ser inserido.
        /// </summary>
        /// <remarks>
        /// A função retorna ao primeiro objecto encontrado.
        /// </remarks>
        /// <param name="obj">O objecto.</param>
        /// <returns>O índice.</returns>
        private int FindPosition(T obj)
        {
            if (this.elements.Count == 0)
            {
                return 0;
            }
            else if (this.comparer.Compare(obj, this.elements[this.elements.Count - 1]) > 0)
            {
                return this.elements.Count;
            }
            else if (this.comparer.Compare(obj, this.elements[0]) <= 0)
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
                    if ((sum & 1) == 0)
                    {
                        var intermediaryElement = this.elements[intermediaryIndex];
                        if (this.comparer.Compare(obj, intermediaryElement) == 0)
                        {
                            return intermediaryIndex;
                        }
                        else if (this.comparer.Compare(obj, this.elements[intermediaryIndex]) < 0)
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
                        var intermediaryElement = this.elements[intermediaryIndex];
                        var nextIntermediaryElement = this.elements[intermediaryIndex + 1];
                        if (
                            this.comparer.Compare(obj, intermediaryElement) > 0 &&
                            this.comparer.Compare(obj, nextIntermediaryElement) <= 0)
                        {
                            return intermediaryIndex + 1;
                        }
                        else if (
                            this.comparer.Compare(obj, intermediaryElement) == 0)
                        {
                            return intermediaryIndex;
                        }
                        else if (this.comparer.Compare(obj, intermediaryElement) > 0)
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
        /// Encontra a posição onde o elemento especificado se encontra.
        /// </summary>
        /// <param name="objectToInsert">O elemento a ser procurado.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindGreatestPosition(T objectToInsert)
        {
            if (elements.Count == 0)
            {
                return 0;
            }
            else if (comparer.Compare(objectToInsert, this.elements[this.elements.Count - 1]) >= 0)
            {
                return this.elements.Count;
            }
            else if (comparer.Compare(objectToInsert, this.elements[0]) < 0)
            {
                return 0;
            }
            else
            {
                int low = 0;
                int high = this.elements.Count - 1;
                while (low< high)
                {
                    int sum = high + low;
                    int intermediaryIndex = sum / 2;
                    if (sum % 2 == 0)
                    {
                        if (this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) < 0)
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
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) > 0 &&
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex + 1]) < 0)
                        {
                            return intermediaryIndex + 1;
                        }
                        else if (
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) == 0 &&
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex + 1]) < 0)
                        {
                            return intermediaryIndex;
                        }
                        else if (this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex + 1]) == 0)
                        {
                            low = intermediaryIndex + 1;
                        }
                        else if (this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) < 0)
                        {
                            high = intermediaryIndex;
                        }
                        else
                        {
                            low = intermediaryIndex;
                        }
                    }
                }

                return high;
            }
        }

        /// <summary>
        /// Encontra a posição onde o elemento especificado se encontra, retornando, em caso
        /// de empate, a posição do primeiro.
        /// </summary>
        /// <param name="objectToInsert">O elemento a ser procurado.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindLowestPosition(T objectToInsert)
        {
            if (this.elements.Count == 0)
            {
                return 0;
            }
            else if (this.comparer.Compare(objectToInsert, this.elements[this.elements.Count - 1]) > 0)
            {
                return this.elements.Count;
            }
            else if (this.comparer.Compare(objectToInsert, this.elements[0]) <= 0)
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
                        if (this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) <= 0)
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
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) > 0 &&
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex + 1]) <= 0)
                        {
                            return intermediaryIndex + 1;
                        }
                        else if (
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) == 0 &&
                            this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex + 1]) < 0)
                        {
                            high = intermediaryIndex;
                        }
                        else if (this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) > 0)
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
        /// Obtém um enumerador não genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
