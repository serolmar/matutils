using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class IntegerSequence : ICollection<int>, IIndexed<int, int>
    {
        /// <summary>
        /// Mantém os elementos da sequência.
        /// </summary>
        private List<Tuple<int, int>> sequenceElements = new List<Tuple<int, int>>();

        public int this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(int item)
        {
            throw new NotImplementedException();
        }

        public void AddRangeItem(int start, int end)
        {
            if (start > end)
            {
                throw new ArgumentException("Start range item must be less than end range item.");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Clear()
        {
            this.sequenceElements.Clear();
        }

        public bool Contains(int item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int item)
        {
            throw new NotImplementedException();
        }

        public bool RemoveRangeItem(int startItem, int endItem)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < this.sequenceElements.Count; ++i)
            {
                var currentItem = this.sequenceElements[i];
                for (int j = currentItem.Item1; j <= currentItem.Item2; ++j)
                {
                    yield return j;
                }
            }
        }

        /// <summary>
        /// Classifies the provided item as being part of some range or is out.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Tuple<int, int> ClassifyItem(int item)
        {
            var firstIndex = 0;
            var lastIndex = this.sequenceElements.Count;
            if (firstIndex != lastIndex)
            {
                var firstTuple = this.sequenceElements[firstIndex];
                var secondTuple = this.sequenceElements[lastIndex];
                if (item < firstTuple.Item1)
                {
                    return Tuple.Create(firstIndex-1, firstIndex);
                }
                else if (item >= firstTuple.Item1 && item <= firstTuple.Item2)
                {
                    return Tuple.Create(firstIndex,firstIndex);
                }
                else if(item > secondTuple.Item2)
                {
                    return Tuple.Create(lastIndex, lastIndex + 1);
                }
                else if (item >= secondTuple.Item1 && item <= secondTuple.Item2)
                {
                    return Tuple.Create(lastIndex, lastIndex);
                }
                else
                {
                    var intermediaryIndex = (firstIndex + lastIndex) / 2;
                    while (firstIndex < lastIndex - 1)
                    {
                        var intermediaryTuple = this.sequenceElements[intermediaryIndex];
                        if (item < intermediaryTuple.Item1)
                        {
                            lastIndex = intermediaryIndex;
                        }
                        else if (item > intermediaryTuple.Item2)
                        {
                            firstIndex = intermediaryIndex;
                        }
                        else
                        {
                            return Tuple.Create(intermediaryIndex, intermediaryIndex);
                        }

                        intermediaryIndex = (firstIndex + lastIndex) / 2;
                    }

                    return Tuple.Create(firstIndex, lastIndex);
                }
            }

            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
