namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Implementa uma sequência de inteiros.
    /// </summary>
    public class IntegerSequence : ICollection<int>
    {
        /// <summary>
        /// Mantém os elementos da sequência.
        /// </summary>
        private List<Tuple<int, int>> sequenceElements = new List<Tuple<int, int>>();

        /// <summary>
        /// Indica se a colecção é apenas de leitura.
        /// </summary>
        private bool isReadonly = false;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="IntegerSequence"/>.
        /// </summary>
        public IntegerSequence()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="IntegerSequence"/>.
        /// </summary>
        /// <param name="isReadonly">Um valor que indica se a colecção é só de leitura.</param>
        protected IntegerSequence(bool isReadonly)
        {
            this.isReadonly = isReadonly;
        }

        /// <summary>
        /// Obtém o valor no índice especificado.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice não se encontrar nos limites da sequeência.
        /// </exception>
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

        /// <summary>
        /// Obtém o número de elementos na sequência.
        /// </summary>
        /// <returns>O número de elementos na sequência.</returns>
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

        /// <summary>
        /// Obtém um valor que identifica se a sequência é só de leitura.
        /// </summary>
        /// <returns>Verdadeiro se a sequência for só de leitura e falso caso contrário.</returns>
        public bool IsReadOnly
        {
            get
            {
                return this.isReadonly;
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O objecto a ser adicionado.</param>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        public void Add(int item)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else if (this.sequenceElements.Count == 0)
            {
                this.sequenceElements.Add(
                    Tuple.Create(item, item));
            }
            else
            {
                var itemClass = this.ClassifyItem(item, 0);
                if (itemClass.Item1 != itemClass.Item2)
                {
                    var fromStart = this.GetStartIndexValuePair(item, itemClass);
                    var fromEnd = this.GetEndIndexValuePair(item, itemClass);
                    this.ProcessAddition(fromStart, fromEnd);
                }
            }
        }

        /// <summary>
        /// Adicionar um conjunto de itens à colecção definido por um número inicial e um número final.
        /// </summary>
        /// <remarks>
        /// A adição é inclusiva.
        /// </remarks>
        /// <param name="start">O número inicial.</param>
        /// <param name="end">O número final.</param>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        /// <exception cref="ArgumentException">
        /// Se o número inicial for superior ao número final.
        /// </exception>
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
                var itemClass = this.ClassifyItem(start, 0);
                if (itemClass.Item1 == itemClass.Item2)
                {
                    var endItemClass = this.ClassifyItem(end, itemClass.Item2);
                    if (endItemClass.Item1 != endItemClass.Item2 || itemClass.Item1 != endItemClass.Item1)
                    {
                        var fromStart = this.GetStartIndexValuePair(start, itemClass);
                        var fromEnd = this.GetEndIndexValuePair(end, endItemClass);
                        this.ProcessAddition(fromStart, fromEnd);
                    }
                }
                else
                {
                    var fromStart = this.GetStartIndexValuePair(start, itemClass);
                    itemClass = this.ClassifyItem(end, itemClass.Item2);
                    var fromEnd = this.GetEndIndexValuePair(end, itemClass);
                    this.ProcessAddition(fromStart, fromEnd);
                }
            }
        }

        /// <summary>
        /// Adiciona uma sequência de inteiros.
        /// </summary>
        /// <param name="sequence">A sequência.</param>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        /// <exception cref="ArgumentNullException">Se a sequência for nula.</exception>
        public void Add(IntegerSequence sequence)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
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

        /// <summary>
        /// Remove todos os itens da colecção.
        /// </summary>
        /// <exception cref="CollectionsException">A sequência é só de leitura.</exception>
        public void Clear()
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else
            {
                this.sequenceElements.Clear();
            }
        }

        /// <summary>
        /// Verifica se o item está contido na colecção.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro caso o item se encontre na colecção e falso caso contrário.</returns>
        public bool Contains(int item)
        {
            var itemClass = this.ClassifyItem(item, 0);
            return itemClass.Item1 == itemClass.Item2;
        }

        /// <summary>
        /// Verifica se o intervalo está completamente contido na colecção.
        /// </summary>
        /// <param name="start">O valor correspondente ao início do intervalo.</param>
        /// <param name="end">O valor correspondente ao final do intervalo.</param>
        /// <returns>
        /// Verdadeiro caso o intervalor esteja integralmente contido na colecção e falso caso contrário.
        /// </returns>
        public bool Contains(int start, int end)
        {
            if (end < start)
            {
                return false;
            }
            else
            {
                var itemClass = this.ClassifyItem(start, 0);
                if (itemClass.Item1 == itemClass.Item2)
                {
                    var endItemClass = this.ClassifyItem(end, itemClass.Item2);
                    if (endItemClass.Item1 == endItemClass.Item2)
                    {
                        return itemClass.Item1 == endItemClass.Item1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Copia os itens para um vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="arrayIndex">O índice do vector onde inciar a cópia.</param>
        /// <exception cref="ArgumentNullException">Se o vector for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice se encontrar fora dos limites do vector.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Se não for possível efectuar a cópia devido às dimensões do vector.
        /// </exception>
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

        /// <summary>
        /// Remove a primeira ocorrência do item.
        /// </summary>
        /// <param name="item">O objecto a ser removido.</param>
        /// <returns>
        /// Verdadeiro se a remoção for bem-sucedida e falso caso contrário.
        /// </returns>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        public bool Remove(int item)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else
            {
                var itemClass = this.ClassifyItem(item, 0);
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

        /// <summary>
        /// Remove um intervalo de valores da sequência definido por um valor inicial e um final.
        /// </summary>
        /// <param name="startItem">O número inicial.</param>
        /// <param name="endItem">O número final.</param>
        /// <returns>Verdadeiro se a remoção for bem-sucedida e falso caso contrário.</returns>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        public bool Remove(int startItem, int endItem)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else if (endItem < startItem)
            {
                return false;
            }
            else
            {
                var startClassItem = this.ClassifyItem(startItem, 0);
                var endClassItem = this.ClassifyItem(endItem, startClassItem.Item2);
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

        /// <summary>
        /// Obtém um enumerador para a sequência.
        /// </summary>
        /// <returns>O enumerador.</returns>
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
        /// Obtém um enumerador para o inverso da sequência.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<int> GetReverseEnumerator()
        {
            for (int i = this.sequenceElements.Count - 1; i >= 0; --i)
            {
                var currentItem = this.sequenceElements[i];
                for (int j = currentItem.Item2; j >= currentItem.Item1; --j)
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
        /// Obtém o primeiro elemento da sequência.
        /// </summary>
        /// <returns>O primeiro elemento da sequência.</returns>
        public int GetFirstElement()
        {
            if (this.sequenceElements.Count == 0)
            {
                throw new CollectionsException("Integer sequence is empty.");
            }
            else
            {
                var element = this.sequenceElements[0];
                return element.Item1;
            }
        }

        /// <summary>
        /// Obtém o último elemento da sequência.
        /// </summary>
        /// <returns>O último elemento da sequência.</returns>
        public int GetLastElement()
        {
            if (this.sequenceElements.Count == 0)
            {
                throw new CollectionsException("Integer sequence is empty.");
            }
            else
            {
                var element = this.sequenceElements[this.sequenceElements.Count - 1];
                return element.Item2;
            }
        }

        /// <summary>
        /// Para cada bloco contíguo de inteiros, aplica a acção especificada.
        /// </summary>
        /// <param name="blockAction">O delegado da acção.</param>
        public void ForeachBlock(Action<int, int> blockAction)
        {
            if (blockAction == null)
            {
                throw new ArgumentNullException("blockAction");
            }
            else
            {
                var length = this.sequenceElements.Count;
                for (int i = 0; i < length; ++i)
                {
                    var current = this.sequenceElements[i];
                    blockAction.Invoke(current.Item1, current.Item2);
                }
            }
        }

        #region Funções de teste

        /// <summary>
        /// Assevera a integridade da colecção.
        /// </summary>
        internal void AssertIntegrity()
        {
            var elementsCount = this.sequenceElements.Count;
            if (elementsCount > 0)
            {
                var previousValue = this.sequenceElements[0].Item2;
                for (int i = 1; i < elementsCount; ++i)
                {
                    var current = this.sequenceElements[i];
                    if (current.Item1 <= previousValue)
                    {
                        throw new Exception("There is a sequence block which last element is greater than the first item in the next block.");
                    }
                    else
                    {
                        previousValue = current.Item2;
                    }
                }
            }
        }

        #endregion Funções de teste

        #region Funções privadas

        /// <summary>
        /// Classifica o item como sendo parte de um limite ou do seu exterior.
        /// </summary>
        /// <param name="item">O item a ser classificado.</param>
        /// <param name="startIndex">O índice de partida.</param>
        /// <returns>O par índice inferior/índice superior ao qual pertence o item.</returns>
        private Tuple<int, int> ClassifyItem(int item, int startIndex)
        {
            var sequenceElementsCount = this.sequenceElements.Count;
            if (sequenceElementsCount == 0)
            {
                return Tuple.Create(-1, 0);
            }
            else
            {
                var firstIndex = startIndex;
                if (firstIndex == sequenceElementsCount)
                {
                    --firstIndex;
                }

                var lastIndex = sequenceElementsCount - 1;
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
                    if (startItem <= firstValue.Item2 + 1)
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
        /// Função auxiliar que permite processar a adição de elementos à colecção.
        /// </summary>
        /// <param name="fromStart">A informação dos valores iniciais.</param>
        /// <param name="fromEnd">A informação dos valores finais.</param>
        private void ProcessAddition(
            Tuple<int, int> fromStart,
            Tuple<int, int> fromEnd)
        {
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
                if (breakValue != element.Item2)
                {
                    result.Add(Tuple.Create(breakValue + 1, element.Item2));
                }
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

        /// <summary>
        /// Obtém um enumerador para a sequência.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa uma sequência de inteiros longos.
    /// </summary>
    public class LongIntegerSequence : ICollection<long>
    {
        /// <summary>
        /// Mantém os elementos da sequência.
        /// </summary>
        private List<Tuple<long, long>> sequenceElements = new List<Tuple<long, long>>();

        /// <summary>
        /// Indica se a colecção é apenas de leitura.
        /// </summary>
        private bool isReadonly = false;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LongIntegerSequence"/>.
        /// </summary>
        public LongIntegerSequence()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LongIntegerSequence"/>.
        /// </summary>
        /// <param name="isReadonly">Um valor que indica se a colecção é só de leitura.</param>
        protected LongIntegerSequence(bool isReadonly)
        {
            this.isReadonly = isReadonly;
        }

        /// <summary>
        /// Obtém o valor no índice especificado.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice não se encontrar nos limites da sequeência.
        /// </exception>
        public long this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    long innerIndex = index;
                    var result = 0L;
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
                            innerIndex -= elementLength;
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

        /// <summary>
        /// Obtém o valor no índice especificado.
        /// </summary>
        /// <value>
        /// O valor.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice não se encontrar nos limites da sequeência.
        /// </exception>
        public long this[long index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    long innerIndex = index;
                    var result = 0L;
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
                            innerIndex -= elementLength;
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

        /// <summary>
        /// Obtém o número de elementos na sequência.
        /// </summary>
        /// <returns>O número de elementos na sequência.</returns>
        public int Count
        {
            get
            {
                var result = 0;
                for (int i = 0; i < this.sequenceElements.Count; ++i)
                {
                    var currentSequenceElement = this.sequenceElements[i];
                    result += (int)(currentSequenceElement.Item2 - currentSequenceElement.Item1) + 1;
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o número de elementos na sequência.
        /// </summary>
        /// <returns>O número de elementos na sequência.</returns>
        public long LongCount
        {
            get
            {
                var result = 0L;
                for (int i = 0; i < this.sequenceElements.Count; ++i)
                {
                    var currentSequenceElement = this.sequenceElements[i];
                    result += currentSequenceElement.Item2 - currentSequenceElement.Item1 + 1;
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um valor que identifica se a sequência é só de leitura.
        /// </summary>
        /// <returns>Verdadeiro se a sequência for só de leitura e falso caso contrário.</returns>
        public bool IsReadOnly
        {
            get
            {
                return this.isReadonly;
            }
        }

        /// <summary>
        /// Adiciona um item à colecção.
        /// </summary>
        /// <param name="item">O objecto a ser adicionado.</param>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        public void Add(long item)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else if (this.sequenceElements.Count == 0)
            {
                this.sequenceElements.Add(
                    Tuple.Create(item, item));
            }
            else
            {
                var itemClass = this.ClassifyItem(item, 0);
                if (itemClass.Item1 != itemClass.Item2)
                {
                    var fromStart = this.GetStartIndexValuePair(item, itemClass);
                    var fromEnd = this.GetEndIndexValuePair(item, itemClass);
                    this.ProcessAddition(fromStart, fromEnd);
                }
            }
        }

        /// <summary>
        /// Adicionar um conjunto de itens à colecção definido por um número inicial e um número final.
        /// </summary>
        /// <remarks>
        /// A adição é inclusiva.
        /// </remarks>
        /// <param name="start">O número inicial.</param>
        /// <param name="end">O número final.</param>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        /// <exception cref="ArgumentException">
        /// Se o número inicial for superior ao número final.
        /// </exception>
        public void Add(long start, long end)
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
                var itemClass = this.ClassifyItem(start, 0);
                if (itemClass.Item1 == itemClass.Item2)
                {
                    var endItemClass = this.ClassifyItem(end, itemClass.Item2);
                    if (endItemClass.Item1 != endItemClass.Item2 || itemClass.Item1 != endItemClass.Item1)
                    {
                        var fromStart = this.GetStartIndexValuePair(start, itemClass);
                        var fromEnd = this.GetEndIndexValuePair(end, endItemClass);
                        this.ProcessAddition(fromStart, fromEnd);
                    }
                }
                else
                {
                    var fromStart = this.GetStartIndexValuePair(start, itemClass);
                    itemClass = this.ClassifyItem(end, itemClass.Item2);
                    var fromEnd = this.GetEndIndexValuePair(end, itemClass);
                    this.ProcessAddition(fromStart, fromEnd);
                }
            }
        }

        /// <summary>
        /// Adiciona uma sequência de inteiros.
        /// </summary>
        /// <param name="sequence">A sequência.</param>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        /// <exception cref="ArgumentNullException">Se a sequência for nula.</exception>
        public void Add(LongIntegerSequence sequence)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
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

        /// <summary>
        /// Remove todos os itens da colecção.
        /// </summary>
        /// <exception cref="CollectionsException">A sequência é só de leitura.</exception>
        public void Clear()
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else
            {
                this.sequenceElements.Clear();
            }
        }

        /// <summary>
        /// Verifica se o item está contido na colecção.
        /// </summary>
        /// <param name="item">O item.</param>
        /// <returns>Verdadeiro caso o item se encontre na colecção e falso caso contrário.</returns>
        public bool Contains(long item)
        {
            var itemClass = this.ClassifyItem(item, 0);
            return itemClass.Item1 == itemClass.Item2;
        }

        /// <summary>
        /// Verifica se o intervalo está completamente contido na colecção.
        /// </summary>
        /// <param name="start">O valor correspondente ao início do intervalo.</param>
        /// <param name="end">O valor correspondente ao final do intervalo.</param>
        /// <returns>
        /// Verdadeiro caso o intervalor esteja integralmente contido na colecção e falso caso contrário.
        /// </returns>
        public bool Contains(long start, long end)
        {
            if (end < start)
            {
                return false;
            }
            else
            {
                var itemClass = this.ClassifyItem(start, 0);
                if (itemClass.Item1 == itemClass.Item2)
                {
                    var endItemClass = this.ClassifyItem(end, itemClass.Item2);
                    if (endItemClass.Item1 == endItemClass.Item2)
                    {
                        return itemClass.Item1 == endItemClass.Item1;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Copia os itens para um vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        /// <param name="arrayIndex">O índice do vector onde inciar a cópia.</param>
        /// <exception cref="ArgumentNullException">Se o vector for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice se encontrar fora dos limites do vector.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Se não for possível efectuar a cópia devido às dimensões do vector.
        /// </exception>
        public void CopyTo(long[] array, int arrayIndex)
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
                    for (var j = currentElement.Item1; j <= currentElement.Item2; ++j)
                    {
                        array[currentPointer++] = j;
                    }
                }
            }
        }

        /// <summary>
        /// Remove a primeira ocorrência do item.
        /// </summary>
        /// <param name="item">O objecto a ser removido.</param>
        /// <returns>
        /// Verdadeiro se a remoção for bem-sucedida e falso caso contrário.
        /// </returns>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        public bool Remove(long item)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else
            {
                var itemClass = this.ClassifyItem(item, 0);
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

        /// <summary>
        /// Remove um intervalo de valores da sequência definido por um valor inicial e um final.
        /// </summary>
        /// <param name="startItem">O número inicial.</param>
        /// <param name="endItem">O número final.</param>
        /// <returns>Verdadeiro se a remoção for bem-sucedida e falso caso contrário.</returns>
        /// <exception cref="CollectionsException">Se a colecção for só de leitura.</exception>
        public bool Remove(long startItem, long endItem)
        {
            if (this.isReadonly)
            {
                throw new CollectionsException("Can't edit readonly collections.");
            }
            else if (endItem < startItem)
            {
                return false;
            }
            else
            {
                var startClassItem = this.ClassifyItem(startItem, 0);
                var endClassItem = this.ClassifyItem(endItem, startClassItem.Item2);
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

        /// <summary>
        /// Obtém um enumerador para a sequência.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<long> GetEnumerator()
        {
            for (int i = 0; i < this.sequenceElements.Count; ++i)
            {
                var currentItem = this.sequenceElements[i];
                for (var j = currentItem.Item1; j <= currentItem.Item2; ++j)
                {
                    yield return j;
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador para o inverso da sequência.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<long> GetReverseEnumerator()
        {
            for (int i = this.sequenceElements.Count - 1; i >= 0; --i)
            {
                var currentItem = this.sequenceElements[i];
                for (var j = currentItem.Item2; j >= currentItem.Item1; --j)
                {
                    yield return j;
                }
            }
        }

        /// <summary>
        /// Obtém uma cópia da sequência actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        public LongIntegerSequence Clone()
        {
            var result = new LongIntegerSequence();
            result.sequenceElements.AddRange(this.sequenceElements);
            return result;
        }

        /// <summary>
        /// Obtém uma cópia da sequência actual como sendo um colecção apenas de leitura.
        /// </summary>
        /// <returns>A sequência de leitura.</returns>
        public LongIntegerSequence AsReadonly()
        {
            var result = this.Clone();
            result.isReadonly = true;
            return result;
        }

        /// <summary>
        /// Obtém o primeiro elemento da sequência.
        /// </summary>
        /// <returns>O primeiro elemento da sequência.</returns>
        public long GetFirstElement()
        {
            if (this.sequenceElements.Count == 0)
            {
                throw new CollectionsException("Integer sequence is empty.");
            }
            else
            {
                var element = this.sequenceElements[0];
                return element.Item1;
            }
        }

        /// <summary>
        /// Obtém o último elemento da sequência.
        /// </summary>
        /// <returns>O último elemento da sequência.</returns>
        public long GetLastElement()
        {
            if (this.sequenceElements.Count == 0)
            {
                throw new CollectionsException("Integer sequence is empty.");
            }
            else
            {
                var element = this.sequenceElements[this.sequenceElements.Count - 1];
                return element.Item2;
            }
        }

        /// <summary>
        /// Para cada bloco contíguo de inteiros, aplica a acção especificada.
        /// </summary>
        /// <param name="blockAction">O delegado da acção.</param>
        public void ForeachBlock(Action<long, long> blockAction)
        {
            if (blockAction == null)
            {
                throw new ArgumentNullException("blockAction");
            }
            else
            {
                var length = this.sequenceElements.Count;
                for (int i = 0; i < length; ++i)
                {
                    var current = this.sequenceElements[i];
                    blockAction.Invoke(current.Item1, current.Item2);
                }
            }
        }

        #region Funções de teste

        /// <summary>
        /// Assevera a integridade da colecção.
        /// </summary>
        internal void AssertIntegrity()
        {
            var elementsCount = this.sequenceElements.Count;
            if (elementsCount > 0)
            {
                var previousValue = this.sequenceElements[0].Item2;
                for (int i = 1; i < elementsCount; ++i)
                {
                    var current = this.sequenceElements[i];
                    if (current.Item1 <= previousValue)
                    {
                        throw new Exception("There is a sequence block which last element is greater than the first item in the next block.");
                    }
                    else
                    {
                        previousValue = current.Item2;
                    }
                }
            }
        }

        #endregion Funções de teste

        #region Funções privadas

        /// <summary>
        /// Classifica o item como sendo parte de um limite ou do seu exterior.
        /// </summary>
        /// <param name="item">O item a ser classificado.</param>
        /// <param name="startIndex">O índice de partida.</param>
        /// <returns>O par índice inferior/índice superior ao qual pertence o item.</returns>
        private Tuple<int, int> ClassifyItem(long item, int startIndex)
        {
            var sequenceElementsCount = this.sequenceElements.Count;
            if (sequenceElementsCount == 0)
            {
                return Tuple.Create(-1, 0);
            }
            else
            {
                var firstIndex = startIndex;
                if (firstIndex == sequenceElementsCount)
                {
                    --firstIndex;
                }

                var lastIndex = sequenceElementsCount - 1;
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
        private Tuple<int, long> GetStartIndexValuePair(long startItem, Tuple<int, int> itemClass)
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
                    if (startItem <= firstValue.Item2 + 1)
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
        private Tuple<int, long> GetEndIndexValuePair(long endItem, Tuple<int, int> itemClass)
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
        /// Função auxiliar que permite processar a adição de elementos à colecção.
        /// </summary>
        /// <param name="fromStart">A informação dos valores iniciais.</param>
        /// <param name="fromEnd">A informação dos valores finais.</param>
        private void ProcessAddition(
            Tuple<int, long> fromStart,
            Tuple<int, long> fromEnd)
        {
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
        private List<Tuple<long, long>> GetElementsFromBreak(long breakValue, Tuple<long, long> element)
        {
            var result = new List<Tuple<long, long>>();
            if (breakValue == element.Item1)
            {
                if (breakValue != element.Item2)
                {
                    result.Add(Tuple.Create(breakValue + 1, element.Item2));
                }
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

        /// <summary>
        /// Obtém um enumerador para a sequência.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa um compressor para uma sequência crescente de números inteiros.
    /// </summary>
    public class IncreasingIntegerSeqCompressor
        : IEnumerable<BigInteger>
    {
        /// <summary>
        /// Mantém o número actual.
        /// </summary>
        private BigInteger current;

        /// <summary>
        /// Mantém o tamanho actual das diferenças.
        /// </summary>
        private ulong deltaSize;

        /// <summary>
        /// Mantém a lista de itens.
        /// </summary>
        private ILongList<byte> list;

        /// <summary>
        /// Mantém o índice para a lista.
        /// </summary>
        private ulong lindex;

        /// <summary>
        /// Mantém o índice do bit.
        /// </summary>
        private byte bit;

        /// <summary>
        /// Instancia numa nova instância de objectos do tipo <see cref="IncreasingIntegerSeqCompressor"/>.
        /// </summary>
        /// <param name="start">O valor inicial.</param>
        /// <param name="listFactory">A fábrica responsável pela criação de lista.</param>
        public IncreasingIntegerSeqCompressor(
            BigInteger start,
            Func<ILongList<byte>> listFactory)
        {
            if (listFactory == null)
            {
                throw new ArgumentNullException("listFactory");
            }
            else
            {
                this.list = listFactory.Invoke();
            }
        }

        /// <summary>
        /// Instancia numa nova instância de objectos do tipo <see cref="IncreasingIntegerSeqCompressor"/>.
        /// </summary>
        /// <param name="stream">Fonte de dados para carregamento do compressor.</param>
        /// <param name="listFactory">A fábrica responsável pela criação de lista.</param>
        public IncreasingIntegerSeqCompressor(
            Stream stream,
            Func<ILongList<byte>> listFactory)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (listFactory == null)
            {
                throw new ArgumentNullException("listFactory");
            }
            else
            {
                this.list = listFactory.Invoke();
                this.LoadFromStream(stream);
            }
        }

        /// <summary>
        /// Obtém um enumerador para o compressor.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<BigInteger> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um enumerador não genérico para o compressor.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina o estado do compressor a partir da linha.
        /// </summary>
        /// <param name="stream">A linha.</param>
        private void LoadFromStream(
            Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
