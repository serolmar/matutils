namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma colecção onde os elementos são mantidos de forma ordenada.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos na colecção ordenada.</typeparam>
    public class InsertionSortedCollection<T> : IEnumerable<T>
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
        private bool ignoreRepetaed = false;

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
        /// <param name="comparer">The comparer that specifies the ordering.</param>
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

            this.ignoreRepetaed = ignoreRepeated;
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
            get { return this.elements.Count; }
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
        /// Insere um elemento na colecção.
        /// </summary>
        /// <param name="objectToInsert">O elemento a ser inserido.</param>
        public void InsertSortElement(T objectToInsert)
        {
            int insertionIndex = this.FindPosition(objectToInsert);
            if (insertionIndex == this.elements.Count)
            {
                this.elements.Add(objectToInsert);
            }
            else
            {
                if (!ignoreRepetaed || this.comparer.Compare(objectToInsert, this.elements[insertionIndex]) != 0)
                {
                    this.elements.Insert(insertionIndex, objectToInsert);
                }
            }
        }

        /// <summary>
        /// Insere um conjunto de elementos ordenados na colecção.
        /// </summary>
        /// <param name="elementsToInsert">Os elementos a serem inseridos.</param>
        /// <exception cref="ArgumentNullException">Se o conjunto de elementso for nulo.</exception>
        public void InsertSortRange(InsertionSortedCollection<T> elementsToInsert)
        {
            if (elementsToInsert == null)
            {
                throw new ArgumentNullException("elementsToInsert");
            }
            else
            {
                for (int i = 0; i < elementsToInsert.elements.Count; ++i)
                {
                    this.InsertSortElement(elementsToInsert.elements[i]);
                }
            }
        }

        /// <summary>
        /// Insere um conjunto de elementos dado por um enumerador na colecção.
        /// </summary>
        /// <param name="elementsToInsert">Os elementos a serem inseridos.</param>
        /// <exception cref="ArgumentNullException">Se o conjunto de elementso for nulo.</exception>
        public void InsertSortEnum(IEnumerable<T> elementsToInsert)
        {
            if (elementsToInsert == null)
            {
                throw new ArgumentNullException("elementsToInsert");
            }
            else
            {
                foreach (var elementToInsert in elementsToInsert)
                {
                    this.InsertSortElement(elementToInsert);
                }
            }
        }

        /// <summary>
        /// Verifica se um elemento está contido na colecção.
        /// </summary>
        /// <param name="objectToFind">O elemento a ser verificado.</param>
        /// <returns>Verdadeiro caso o elemento exista e falso caso contrário.</returns>
        public bool HasElement(T objectToFind)
        {
            int index = this.FindPosition(objectToFind);
            if (index >= this.elements.Count || index < 0)
            {
                return false;
            }
            return this.comparer.Compare(objectToFind, this.elements[index]) == 0;
        }

        /// <summary>
        /// Remove um elemento considerando a ordem.
        /// </summary>
        /// <param name="objectToRemove">O elemento a ser removido.</param>
        public void RemoveElement(T objectToRemove)
        {
            int index = this.FindPosition(objectToRemove);
            if (index > this.elements.Count)
            {
                return;
            }

            this.elements.RemoveAt(index);
        }

        /// <summary>
        /// Limpa a colecção.
        /// </summary>
        public void Clear()
        {
            this.elements.Clear();
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
        /// Obtém um enumerador para todos os elementos da colecção que são iguais ao elemento dadao.
        /// </summary>
        /// <param name="objectToFind">O elemento a comparar.</param>
        /// <returns>Um enumerador para os valores iguais.</returns>
        public IEnumerator<T> GetEqualsTo(T objectToFind)
        {
            int index = this.FindPosition(objectToFind);
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
        /// Encontra a posição onde o elemento especificado se encontra.
        /// </summary>
        /// <param name="objectToInsert">O elemento a ser procurado.</param>
        /// <returns>O índice da posição onde o elemento se encontra.</returns>
        private int FindPosition(T objectToInsert)
        {
            if (elements.Count == 0)
            {
                return 0;
            }
            if (comparer.Compare(objectToInsert, this.elements[this.elements.Count - 1]) > 0)
            {
                return this.elements.Count;
            }
            if (comparer.Compare(objectToInsert, this.elements[0]) <= 0)
            {
                return 0;
            }
            int low = 0;
            int high = this.elements.Count - 1;
            while (low <= high - 1)
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
                        this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex + 1]) <= 0)
                    {
                        return intermediaryIndex + 1;
                    }
                    else if (
                        this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) >= 0 &&
                        this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex + 1]) <= 0)
                    {
                        return intermediaryIndex;
                    }
                    if (this.comparer.Compare(objectToInsert, this.elements[intermediaryIndex]) < 0)
                    {
                        high = intermediaryIndex;
                    }
                    else
                    {
                        low = intermediaryIndex;
                    }
                }
            }
            return -1;
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

    /// <summary>
    /// Implementa um comparador lexicográfico de colecções.
    /// </summary>
    /// <typeparam name="T">O tipo de elementos na colecção a comparar.</typeparam>
    public class LexicographicalComparer<T> : IComparer<ICollection<T>>
    {
        /// <summary>
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância da classe <see cref="LexicographicalComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de elementos.</param>
        public LexicographicalComparer(IComparer<T> comparer)
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

        #region IComparer<ICollection<T>> Members

        /// <summary>
        /// Compara dois objectos e retorna um valor que indica se um é menor, maior ou igual a outro.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// O valor 1 se o primeiro for maior do que o segundo, 0 se ambos forem iguais e -1 se o primeiro for
        /// menor do que o segundo.
        /// </returns>
        public int Compare(ICollection<T> x, ICollection<T> y)
        {
            IEnumerator<T> xEnum = x.GetEnumerator();
            IEnumerator<T> yEnum = y.GetEnumerator();
            bool xMoveNext = xEnum.MoveNext();
            bool yMoveNext = yEnum.MoveNext();
            while (xMoveNext && yMoveNext)
            {
                if (this.comparer.Compare(xEnum.Current, yEnum.Current) < 0)
                {
                    return -1;
                }
                else if (this.comparer.Compare(xEnum.Current, yEnum.Current) > 0)
                {
                    return 1;
                }
                xMoveNext = xEnum.MoveNext();
                yMoveNext = yEnum.MoveNext();
            }
            if (xMoveNext)
            {
                return 1;
            }
            if (yMoveNext)
            {
                return -1;
            }
            return 0;
        }

        #endregion
    }
}
