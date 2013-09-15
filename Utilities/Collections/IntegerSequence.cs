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

        /// <summary>
        /// Indica se a colecção é apenas de leitura.
        /// </summary>
        private bool isReadonly = false;

        public IntegerSequence()
        {
        }

        protected IntegerSequence(bool isReadonly)
        {
            this.isReadonly = isReadonly;
        }

        public int this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    var result = 0;
                    for (int i = 0; i < this.sequenceElements.Count; ++i)
                    {
                        var currentSequenceElement = this.sequenceElements[i];
                        var elementLength = currentSequenceElement.Item2 - currentSequenceElement.Item1 + 1;
                        if (index < elementLength)
                        {
                            result = currentSequenceElement.Item1 + index;
                            return result;
                        }
                        else
                        {
                            index -= elementLength;
                            if (index < 0)
                            {
                                throw new ArgumentOutOfRangeException("index");
                            }
                        }
                    }

                    throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        public int Count
        {
            get
            {
                var result = 0;
                for (int i = 0; i < this.sequenceElements.Count; ++i)
                {
                    var currentSequenceElement = this.sequenceElements[i];
                    result += currentSequenceElement.Item2 - currentSequenceElement.Item1 + 1;
                }

                return result;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.isReadonly;
            }
        }

        public void Add(int item)
        {
            if (this.isReadonly)
            {
                throw new Exception("Can't edit readonly collections.");
            }
            else if (this.sequenceElements.Count == 0)
            {
                this.sequenceElements.Add(
                    Tuple.Create(item, item));
            }
            else
            {
                var itemClass = this.ClassifyItem(item);
                var fromStart = this.GetStartIndexValuePair(item, itemClass);
                var fromEnd = this.GetEndIndexValuePair(item, itemClass);
                if (fromStart.Item1 == -1 && fromEnd.Item1 == -1)
                {
                    this.sequenceElements.Insert(0, Tuple.Create(fromStart.Item2, fromEnd.Item2));
                }
                else if (fromStart.Item1 == this.sequenceElements.Count && fromEnd.Item1 == this.sequenceElements.Count)
                {
                    this.sequenceElements.Add(Tuple.Create(fromStart.Item2, fromEnd.Item2));
                }
                else
                {
                    this.RemoveBetweenIndices(fromStart.Item1, fromEnd.Item1);
                    this.sequenceElements.Insert(fromStart.Item1,
                        Tuple.Create(fromStart.Item2, fromEnd.Item2));
                }
            }
        }

        public void Add(int start, int end)
        {
            if (this.isReadonly)
            {
                throw new Exception("Can't edit readonly collections.");
            }
            else if (start > end)
            {
                throw new ArgumentException("Start range item must be less than end range item.");
            }
            else if (this.sequenceElements.Count == 0)
            {
                this.sequenceElements.Add(Tuple.Create(start, end));
            }
            else
            {
                var itemClass = this.ClassifyItem(start);
                var fromStart = this.GetStartIndexValuePair(start, itemClass);

                itemClass = this.ClassifyItem(end);
                var fromEnd = this.GetEndIndexValuePair(end, itemClass);
                if (fromStart.Item1 == -1 && fromEnd.Item1 == -1)
                {
                    this.sequenceElements.Insert(0, Tuple.Create(fromStart.Item2, fromEnd.Item2));
                }
                else if (fromStart.Item1 == this.sequenceElements.Count && fromEnd.Item1 == this.sequenceElements.Count)
                {
                    this.sequenceElements.Add(Tuple.Create(fromStart.Item2, fromEnd.Item2));
                }
                else
                {
                    this.RemoveBetweenIndices(fromStart.Item1, fromEnd.Item1);
                    this.sequenceElements.Insert(fromStart.Item1,
                        Tuple.Create(fromStart.Item2, fromEnd.Item2));
                }
            }
        }

        public void Add(IntegerSequence sequence)
        {
            if (this.isReadonly)
            {
                throw new Exception("Can't edit readonly collections.");
            }
            else if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }
            else
            {
                foreach (var item in sequence.sequenceElements)
                {
                    this.Add(item.Item1, item.Item2);
                }
            }
        }

        public void Clear()
        {
            if (this.isReadonly)
            {
                throw new Exception("Can't edit readonly collections.");
            }
            else
            {
                this.sequenceElements.Clear();
            }
        }

        public bool Contains(int item)
        {
            var itemClass = this.ClassifyItem(item);
            return itemClass.Item1 == itemClass.Item2;
        }

        public void CopyTo(int[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }
            else if (this.Count > array.Length - arrayIndex)
            {
                throw new ArgumentException("Can't copy the array. There is no enough positions.");
            }
            else
            {
                var currentPointer = arrayIndex;
                for (int i = 0; i < this.sequenceElements.Count; ++i)
                {
                    var currentElement = this.sequenceElements[i];
                    for (int j = currentElement.Item1; j <= currentElement.Item2; ++j)
                    {
                        array[currentPointer++] = j;
                    }
                }
            }
        }

        public bool Remove(int item)
        {
            if (this.isReadonly)
            {
                throw new Exception("Can't edit readonly collections.");
            }
            else
            {
                var itemClass = this.ClassifyItem(item);
                if (itemClass.Item1 == itemClass.Item2)
                {
                    var value = this.sequenceElements[itemClass.Item1];
                    this.sequenceElements.RemoveAt(itemClass.Item1);
                    var elementsToInsert = this.GetElementsFromBreak(item, value);
                    this.sequenceElements.InsertRange(itemClass.Item1, elementsToInsert);
                    return true;
                }
            }

            return false;
        }

        public bool Remove(int startItem, int endItem)
        {
            if (this.isReadonly)
            {
                throw new Exception("Can't edit readonly collections.");
            }
            else if (endItem < startItem)
            {
                return false;
            }
            else
            {
                var startClassItem = this.ClassifyItem(startItem);
                var endClassItem = this.ClassifyItem(endItem);
                if (startClassItem.Item1 == startClassItem.Item2)
                {
                    var startValue = this.sequenceElements[startClassItem.Item1];
                    if (endClassItem.Item1 == endClassItem.Item2)
                    {
                        if (startClassItem.Item1 == endClassItem.Item1)
                        {
                            this.sequenceElements.RemoveAt(startClassItem.Item1);
                            var firstIntervalValue = endItem + 1;
                            var secondIntervalValue = startValue.Item2;
                            if (firstIntervalValue <= secondIntervalValue)
                            {
                                this.sequenceElements.Insert(startClassItem.Item1, Tuple.Create(firstIntervalValue, secondIntervalValue));
                            }

                            firstIntervalValue = startValue.Item1;
                            secondIntervalValue = startItem - 1;
                            if (firstIntervalValue <= secondIntervalValue)
                            {
                                this.sequenceElements.Insert(startClassItem.Item1, Tuple.Create(firstIntervalValue, secondIntervalValue));
                            }
                        }
                        else
                        {
                            var endValue = this.sequenceElements[endClassItem.Item1];
                            this.RemoveBetweenIndices(startClassItem.Item1, endClassItem.Item1);

                            var firstIntervalValue = endItem + 1;
                            var secondIntervalValue = endValue.Item2;
                            if (firstIntervalValue <= secondIntervalValue)
                            {
                                this.sequenceElements.Insert(startClassItem.Item1, Tuple.Create(firstIntervalValue, secondIntervalValue));
                            }

                            firstIntervalValue = startValue.Item1;
                            secondIntervalValue = startItem - 1;
                            if (firstIntervalValue <= secondIntervalValue)
                            {
                                this.sequenceElements.Insert(startClassItem.Item1, Tuple.Create(firstIntervalValue, secondIntervalValue));
                            }
                        }
                    }
                    else
                    {
                        this.RemoveBetweenIndices(startClassItem.Item1, endClassItem.Item1);
                        var firstIntervalValue = startValue.Item1;
                        var secondIntervalValue = startItem - 1;
                        if (firstIntervalValue <= secondIntervalValue)
                        {
                            this.sequenceElements.Insert(startClassItem.Item1, Tuple.Create(firstIntervalValue, secondIntervalValue));
                        }
                    }
                }
                else
                {
                    if (endClassItem.Item1 == endClassItem.Item2)
                    {
                        var endValue = this.sequenceElements[endClassItem.Item1];
                        this.RemoveBetweenIndices(startClassItem.Item2, endClassItem.Item1);
                        var firstIntervalValue = endItem + 1;
                        var secondIntervalValue = endValue.Item2;
                        if (firstIntervalValue <= secondIntervalValue)
                        {
                            this.sequenceElements.Insert(startClassItem.Item1, Tuple.Create(firstIntervalValue, secondIntervalValue));
                        }
                    }
                    else
                    {
                        this.RemoveBetweenIndices(startClassItem.Item2, endClassItem.Item1);
                    }
                }

                return false;
            }
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
        /// Obtém uma cópia da sequência actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        public IntegerSequence Clone()
        {
            var result = new IntegerSequence();
            result.sequenceElements.AddRange(this.sequenceElements);
            return result;
        }

        /// <summary>
        /// Obtém uma cópia da sequência actual como sendo um colecção apenas de leitura.
        /// </summary>
        /// <returns>A sequência de leitura.</returns>
        public IntegerSequence AsReadonly()
        {
            var result = this.Clone();
            result.isReadonly = true;
            return result;
        }

        /// <summary>
        /// Classifica o item como sendo parte de um limite ou do seu exterior.
        /// </summary>
        /// <param name="item">O item a ser classificado.</param>
        /// <returns>O par índice inferior/índice superior ao qual pertence o item.</returns>
        private Tuple<int, int> ClassifyItem(int item)
        {
            if (this.sequenceElements.Count == 0)
            {
                return Tuple.Create(-1, 0);
            }
            else
            {
                var firstIndex = 0;
                var lastIndex = this.sequenceElements.Count - 1;
                if (firstIndex != lastIndex)
                {
                    var firstTuple = this.sequenceElements[firstIndex];
                    var secondTuple = this.sequenceElements[lastIndex];
                    if (item < firstTuple.Item1)
                    {
                        return Tuple.Create(firstIndex - 1, firstIndex);
                    }
                    else if (item >= firstTuple.Item1 && item <= firstTuple.Item2)
                    {
                        return Tuple.Create(firstIndex, firstIndex);
                    }
                    else if (item > secondTuple.Item2)
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
                else
                {
                    var tuple = this.sequenceElements[firstIndex];
                    if (item < tuple.Item1)
                    {
                        return Tuple.Create(firstIndex - 1, firstIndex);
                    }
                    else if (item > tuple.Item2)
                    {
                        return Tuple.Create(firstIndex, firstIndex + 1);
                    }
                    else
                    {
                        return Tuple.Create(firstIndex, lastIndex);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o par índice a remover/valor associado ao item inicial especificado.
        /// </summary>
        /// <param name="startItem">O item.</param>
        /// <param name="itemClass">A classificação do item.</param>
        /// <returns>O par.</returns>
        private Tuple<int, int> GetStartIndexValuePair(int startItem, Tuple<int, int> itemClass)
        {
            if (this.sequenceElements.Count == 0)
            {
                return Tuple.Create(-1, startItem);
            }
            else
            {
                if (itemClass.Item1 == -1)
                {
                    return Tuple.Create(0, startItem);
                }
                else if (itemClass.Item2 == this.sequenceElements.Count)
                {
                    var firstValue = this.sequenceElements[itemClass.Item1];
                    if (startItem == firstValue.Item2 + 1)
                    {
                        return Tuple.Create(this.sequenceElements.Count - 1, firstValue.Item1);
                    }
                    else
                    {
                        return Tuple.Create(this.sequenceElements.Count, startItem);
                    }
                }
                else
                {
                    var firstValue = this.sequenceElements[itemClass.Item1];
                    var secondValue = this.sequenceElements[itemClass.Item2];
                    if (startItem == firstValue.Item2 + 1)
                    {
                        return Tuple.Create(itemClass.Item1, firstValue.Item1);
                    }
                    else
                    {
                        return Tuple.Create(itemClass.Item2, startItem);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o par índice a remover/valor associado ao item final especificado.
        /// </summary>
        /// <param name="endItem">O item.</param>
        /// <param name="itemClass">A classificação do item.</param>
        /// <returns>O par.</returns>
        private Tuple<int, int> GetEndIndexValuePair(int endItem, Tuple<int, int> itemClass)
        {
            if (this.sequenceElements.Count == 0)
            {
                return Tuple.Create(-1, endItem);
            }
            else
            {
                if (itemClass.Item1 == -1)
                {
                    var secondValue = this.sequenceElements[itemClass.Item2];
                    if (endItem == secondValue.Item1 - 1)
                    {
                        return Tuple.Create(0, secondValue.Item2);
                    }
                    else
                    {
                        return Tuple.Create(-1, endItem);
                    }
                }
                else if (itemClass.Item2 == this.sequenceElements.Count)
                {
                    return Tuple.Create(this.sequenceElements.Count - 1, endItem);
                }
                else
                {
                    var firstValue = this.sequenceElements[itemClass.Item1];
                    var secondValue = this.sequenceElements[itemClass.Item2];
                    if (endItem == secondValue.Item1 - 1)
                    {
                        return Tuple.Create(itemClass.Item2, secondValue.Item2);
                    }
                    else
                    {
                        return Tuple.Create(itemClass.Item1, endItem);
                    }
                }
            }
        }

        /// <summary>
        /// Remove os elementos que se encontram entre os índices especificados.
        /// </summary>
        /// <param name="startIndex">O índice inicial.</param>
        /// <param name="endIndex">O índice final.</param>
        private void RemoveBetweenIndices(int startIndex, int endIndex)
        {
            var innerStartIndex = startIndex;
            var innerEndIndex = endIndex;
            if (startIndex != -1 || endIndex != -1)
            {
                if (innerStartIndex == -1)
                {
                    innerStartIndex = 0;
                }

                if (innerEndIndex == -1)
                {
                    innerEndIndex = 0;
                }

                var length = endIndex - startIndex + 1;
                while (length > 0 && this.sequenceElements.Count > 0)
                {
                    this.sequenceElements.RemoveAt(startIndex);
                    --length;
                }
            }
        }

        /// <summary>
        /// Obtém os elementos relacionados com a quebra de um outro elemento.
        /// </summary>
        /// <param name="breakValue">O valor de quebra.</param>
        /// <param name="element">O elemento.</param>
        /// <returns>A lista com os elementos separados.</returns>
        private List<Tuple<int, int>> GetElementsFromBreak(int breakValue, Tuple<int, int> element)
        {
            var result = new List<Tuple<int, int>>();
            if (breakValue == element.Item1)
            {
                result.Add(Tuple.Create(breakValue + 1, element.Item2));
            }
            else if (breakValue == element.Item2)
            {
                result.Add(Tuple.Create(element.Item1, breakValue - 1));
            }
            else
            {
                result.Add(Tuple.Create(element.Item1, breakValue - 1));

                result.Add(Tuple.Create(breakValue + 1, element.Item2));
            }

            return result;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
