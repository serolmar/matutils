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
        /// Insert an element into the collection.
        /// </summary>
        /// <param name="objectToInsert">The element to insert.</param>
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

        public void InsertSortRange(InsertionSortedCollection<T> elementsToInsert)
        {
            throw new NotImplementedException();
        }

        public void InsertSortEnum(IEnumerable<T> elementsToInsert)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if an element exists in the collection.
        /// </summary>
        /// <param name="objectToFind">The element to check.</param>
        /// <returns>True if element exists and false otherwise.</returns>
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
