using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    /// <summary>
    /// Represents a collection where the elements are sorted relatively to a specified order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InsertionSortedCollection<T> : IEnumerable<T>
    {
        private List<T> elements = new List<T>();
        private IComparer<T> comparer = null;
        private bool ignoreRepetaed = false;

        /// <summary>
        /// Instantiates a new instance of the InsertionSortedCollection class.
        /// </summary>
        /// <param name="comparer">The comparer that specifies the ordering.</param>
        public InsertionSortedCollection(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentException("Argument comparer can not be null.");
            }
            this.comparer = comparer;
        }

        /// <summary>
        /// Instantiates a new instance of the InsertionSortedCollection class.
        /// </summary>
        /// <param name="comparer">The comparer that specifies the ordering.</param>
        public InsertionSortedCollection(IComparer<T> comparer, bool ignoreRepeated)
        {
            if (comparer == null)
            {
                throw new ArgumentException("Argument comparer can not be null.");
            }
            this.comparer = comparer;
            this.ignoreRepetaed = ignoreRepeated;
        }

        /// <summary>
        /// Gets the number of inserted elements.
        /// </summary>
        public int Count
        {
            get { return this.elements.Count; }
        }

        /// <summary>
        /// Gets the first element in the ordered set.
        /// </summary>
        public T First
        {
            get
            {
                if (this.elements.Count > 0)
                {
                    return this.elements[0];
                }

                throw new Exception("Empty set.");
            }
        }

        /// <summary>
        /// Gets the last element in the ordered set.
        /// </summary>
        public T Last
        {
            get
            {
                if (this.elements.Count > 0)
                {
                    return this.elements[this.elements.Count - 1];
                }

                throw new Exception("Empty set.");
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
        /// Removes the element considering the ordering.
        /// </summary>
        /// <param name="objectToRemove">The element to remove.</param>
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
        /// Clears all elements.
        /// </summary>
        public void Clear()
        {
            this.elements.Clear();
        }

        /// <summary>
        /// Returns an array containing a copy of the elements collection.
        /// </summary>
        /// <returns>The array.</returns>
        public T[] ToArray()
        {
            return this.elements.ToArray();
        }

        /// <summary>
        /// Gets an enumerator for all values that are considered as equal
        /// to the specified argument.
        /// </summary>
        /// <param name="objectToFind">The argument.</param>
        /// <returns>An enumerator to equal values.</returns>
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
        /// Finds the position where element should be. It takes a complexity time of order O(log(n)).
        /// </summary>
        /// <param name="objectToInsert">The object to check.</param>
        /// <returns>The index of element position in collection container.</returns>
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

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class LexicographicalComparer<T> : IComparer<ICollection<T>>
    {
        private IComparer<T> comparer;

        public LexicographicalComparer(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentException("Argument comparer can not be null.");
            }
            this.comparer = comparer;
        }

        #region IComparer<ICollection<T>> Members

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
