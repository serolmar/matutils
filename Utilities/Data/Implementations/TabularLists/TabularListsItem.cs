namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Classe que permite representar uma tabela por intermédio de listas.
    /// </summary>
    internal class TabularListsItem : ITabularItem, ITabularItemEvent
    {
        /// <summary>
        /// Mantém a tabela dos elementos.
        /// </summary>
        protected List<List<object>> elements;

        public TabularListsItem()
        {
            this.elements = new List<List<object>>();
        }

        public ITabularRow this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    return new TabularListRow(index, this);
                }
            }
        }

        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        #region Funções do item tabular
        public void SetValue<ElementType>(int rowNumber, int cellNumber, ElementType value)
        {
            if (rowNumber < 0)
            {
                throw new ArgumentOutOfRangeException("rowNumber");
            }
            else if (cellNumber < 0)
            {
                throw new ArgumentOutOfRangeException("cellNumber");
            }
            else
            {
                var eventArgs = default(UpdateEventArgs<ITabularCell, object>);
                if (this.BeforeUpdateEvent != null)
                {
                    eventArgs = new UpdateEventArgs<ITabularCell, object>(
                    new TabularListCell(rowNumber, cellNumber, this),
                    value);
                    this.BeforeSetEvent.Invoke(this, eventArgs);
                }

                this.SetCellValue<ElementType>(rowNumber, cellNumber, value);

                if (this.AfterUpdateEvent != null)
                {
                    if (eventArgs == null)
                    {
                        eventArgs = new UpdateEventArgs<ITabularCell, object>(
                            new TabularListCell(rowNumber, cellNumber, this),
                            value);
                    }

                    this.AfterSetEvent(this, eventArgs);
                }
            }
        }

        /// <summary>
        /// Actualiza as linhas do item tabular que satisfazem uma determinada condição.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a atribuir à linha.</typeparam>
        /// <param name="values">O tipo de valores.</param>
        /// <param name="expression">A expressão de condição.</param>
        /// <returns>O número de linhas afectadas.</returns>
        public int UpdateCellsWhere<ElementType>(
            IEnumerable<KeyValuePair<int, ElementType>> values,
            Func<ITabularRow, bool> expression)
        {
            var result = 0;
            if (expression != null && values != null)
            {
                if (this.BeforeUpdateEvent == null && this.AfterUpdateEvent == null)
                {
                    for (int i = 0; i < this.elements.Count; ++i)
                    {
                        var currentRow = new TabularListRow(i, this);
                        var currentLine = this.elements[i];
                        if (expression.Invoke(currentRow))
                        {
                            foreach (var kvp in values)
                            {
                                for (int j = currentLine.Count; j <= kvp.Key; ++j)
                                {
                                    currentLine.Add(null);
                                }

                                currentLine[kvp.Key] = kvp.Value;
                            }

                            ++result;
                        }
                    }
                }
                else
                {
                    var cellsTable = new List<List<ITabularCell>>();
                    for (int i = 0; i < this.elements.Count; ++i)
                    {
                        var currentRow = new TabularListRow(i, this);
                        var currentLine = this.elements[i];
                        var cellsLine = new List<ITabularCell>();
                        if (expression.Invoke(currentRow))
                        {
                            foreach (var kvp in values)
                            {
                                var tabularCell = new TabularListCell(i, kvp.Key, this);
                                cellsLine.Add(tabularCell);
                            }

                            cellsTable.Add(cellsLine);
                            ++result;
                        }
                    }

                    var objectValues = new List<object>();
                    foreach (var kvp in values)
                    {
                        objectValues.Add(kvp.Value);
                    }

                    var eventArgs = new UpdateEventArgs<List<List<ITabularCell>>, List<object>>(
                        cellsTable,
                        objectValues);
                    if (this.BeforeUpdateEvent != null)
                    {
                        this.BeforeUpdateEvent.Invoke(this, eventArgs);
                    }

                    for (int i = 0; i < cellsTable.Count; ++i)
                    {
                        var currentCellsLine = cellsTable[i];

                        for (int j = 0; j < currentCellsLine.Count; ++j)
                        {
                            var currentCell = currentCellsLine[j];
                            this.SetCellValue(currentCell.RowNumber, currentCell.ColumnNumber, objectValues[j]);
                        }
                    }

                    if (this.AfterUpdateEvent != null)
                    {
                        this.AfterUpdateEvent.Invoke(this, eventArgs);
                    }
                }
            }

            return result;
        }

        public void Add<ElementType>(IEnumerable<ElementType> elements)
        {
            var elementsToAdd = new List<object>();
            if (elements != null)
            {
                foreach (var item in elements)
                {
                    elementsToAdd.Add(item);
                }
            }

            var eventArgs = new AddDeleteEventArgs<IEnumerable<object>>(elementsToAdd);
            if (this.BeforeAddEvent != null)
            {
                this.BeforeAddEvent.Invoke(this, eventArgs);
            }

            this.elements.Add(elementsToAdd);

            if (this.AfterAddEvent != null)
            {
                this.AfterAddEvent.Invoke(this, eventArgs);
            }
        }

        public void Add<ElementType>(IEnumerable<KeyValuePair<int, ElementType>> elements)
        {
            var elementsToAdd = new SortedDictionary<int, object>();
            if (elements == null)
            {
                foreach (var kvp in elements)
                {
                    if (elementsToAdd.ContainsKey(kvp.Key))
                    {
                        elementsToAdd[kvp.Key] = kvp.Value;
                    }
                    else
                    {
                        elementsToAdd.Add(kvp.Key, kvp.Value);
                    }
                }

                var eventArgs = new AddDeleteEventArgs<IEnumerable<KeyValuePair<int, object>>>(
                    elementsToAdd);
                if (this.BeforeAddKeyedValuesEvent != null)
                {
                    this.BeforeAddKeyedValuesEvent.Invoke(this, eventArgs);
                }

                var addElementsList = new List<object>();
                foreach (var kvp in elementsToAdd)
                {
                    while (addElementsList.Count < kvp.Key)
                    {
                        addElementsList.Add(null);
                    }

                    addElementsList.Add(kvp.Value);
                }

                this.elements.Add(addElementsList);
                if (this.AfterAddKeyedValuesEvent != null)
                {
                    this.AfterAddKeyedValuesEvent.Invoke(this, eventArgs);
                }
            }
        }

        public void AddRange<ElementType>(IEnumerable<IEnumerable<ElementType>> elements)
        {
            var elementsToAdd = new List<List<object>>();
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    var elementsList = new List<object>();
                    foreach (var elementItem in element)
                    {
                        elementsList.Add(elementItem);
                    }
                }
            }

            var eventArgs = new AddDeleteEventArgs<IEnumerable<IEnumerable<object>>>(elementsToAdd);
            if (this.BeforeAddRangeEvent != null)
            {
                this.BeforeAddRangeEvent.Invoke(this, eventArgs);
            }

            this.elements.AddRange(elementsToAdd);

            if (this.AfterAddRangeEvent != null)
            {
                this.AfterAddRangeEvent.Invoke(this, eventArgs);
            }
        }

        public void AddRange<ElementType>(IEnumerable<IEnumerable<KeyValuePair<int, ElementType>>> elements)
        {
            var elementsDictionary = new List<SortedDictionary<int, object>>();
            if (elements != null)
            {
                foreach (var elementsItem in elements)
                {
                    var innerDictionary = new Dictionary<int, object>();
                    foreach (var innerElement in elementsItem)
                    {
                        if (innerDictionary.ContainsKey(innerElement.Key))
                        {
                            innerDictionary[innerElement.Key] = innerElement.Value;
                        }
                        else
                        {
                            innerDictionary.Add(innerElement.Key, innerElement.Value);
                        }
                    }
                }
            }

            var eventArgs = new AddDeleteEventArgs<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>(
                elementsDictionary);
            if (this.BeforeAddRangeEvent != null)
            {
                this.BeforeKeyedValuesAddRangeEvent.Invoke(this, eventArgs);
            }

            foreach (var elementsDictionaryItem in elementsDictionary)
            {
                var addElementsList = new List<object>();
                foreach (var kvp in elementsDictionaryItem)
                {
                    while (addElementsList.Count < kvp.Key)
                    {
                        addElementsList.Add(null);
                    }

                    addElementsList.Add(kvp.Value);
                }

                this.elements.Add(addElementsList);
            }

            if (this.AfterAddRangeEvent != null)
            {
                this.AfterKeyedValuesAddRangeEvent.Invoke(this, eventArgs);
            }
        }

        public void Insert<ElementType>(int index, IEnumerable<ElementType> elements)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                var elementsToAdd = new List<object>();
                if (elements != null)
                {
                    foreach (var item in elements)
                    {
                        elementsToAdd.Add(item);
                    }
                }

                for (int i = this.elements.Count; i < index; ++i)
                {
                    this.elements.Add(new List<object>());
                }

                var eventArgs = new InsertEventArgs<IEnumerable<object>>(index, elementsToAdd);
                if (this.BeforeInsertEvent != null)
                {
                    this.BeforeInsertEvent.Invoke(this, eventArgs);
                }

                this.elements.Insert(index, elementsToAdd);

                if (this.AfterInsertEvent != null)
                {
                    this.AfterInsertEvent.Invoke(this, eventArgs);
                }
            }
        }

        public void Insert<ElementType>(int index, IEnumerable<KeyValuePair<int, ElementType>> elements)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                var elementsToAdd = new Dictionary<int, object>();
                if (elements == null)
                {
                    foreach (var kvp in elements)
                    {
                        if (elementsToAdd.ContainsKey(kvp.Key))
                        {
                            elementsToAdd[kvp.Key] = kvp.Value;
                        }
                        else
                        {
                            elementsToAdd.Add(kvp.Key, kvp.Value);
                        }
                    }

                    var eventArgs = new InsertEventArgs<IEnumerable<KeyValuePair<int, object>>>(
                        index,
                        elementsToAdd);
                    if (this.BeforeKeyedValuesInsertEvent != null)
                    {
                        this.BeforeKeyedValuesInsertEvent.Invoke(this, eventArgs);
                    }

                    var addElementsList = new List<object>();
                    foreach (var kvp in elementsToAdd)
                    {
                        while (addElementsList.Count < kvp.Key)
                        {
                            addElementsList.Add(null);
                        }

                        addElementsList.Add(kvp.Value);
                    }

                    this.elements.Insert(index, addElementsList);
                    if (this.AfterKeyedValuesInsertEvent != null)
                    {
                        this.AfterKeyedValuesInsertEvent.Invoke(this, eventArgs);
                    }
                }
            }
        }

        public void InsertRange<ElementType>(int index, IEnumerable<IEnumerable<ElementType>> elements)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            else
            {
                var elementsToAdd = new List<List<object>>();
                if (elements != null)
                {
                    foreach (var element in elements)
                    {
                        var elementsList = new List<object>();
                        foreach (var elementItem in element)
                        {
                            elementsList.Add(elementItem);
                        }
                    }
                }

                var eventArgs = new InsertEventArgs<IEnumerable<IEnumerable<object>>>(index, elementsToAdd);
                if (this.BeforeInsertRangeEvent != null)
                {
                    this.BeforeInsertRangeEvent.Invoke(this, eventArgs);
                }

                this.elements.InsertRange(index, elementsToAdd);

                if (this.AfterInsertRangeEvent != null)
                {
                    this.AfterInsertRangeEvent.Invoke(this, eventArgs);
                }
            }
        }

        public void InsertRange<ElementType>(
            int index,
            IEnumerable<IEnumerable<KeyValuePair<int, ElementType>>> elements)
        {
            var elementsDictionary = new List<SortedDictionary<int, object>>();
            if (elements != null)
            {
                foreach (var elementsItem in elements)
                {
                    var innerDictionary = new Dictionary<int, object>();
                    foreach (var innerElement in elementsItem)
                    {
                        if (innerDictionary.ContainsKey(innerElement.Key))
                        {
                            innerDictionary[innerElement.Key] = innerElement.Value;
                        }
                        else
                        {
                            innerDictionary.Add(innerElement.Key, innerElement.Value);
                        }
                    }
                }
            }

            var eventArgs = new InsertEventArgs<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>(
                index,
                elementsDictionary);
            if (this.BeforeKeyedValuesInsertRangeEvent != null)
            {
                this.BeforeKeyedValuesInsertRangeEvent.Invoke(this, eventArgs);
            }

            for (int i = elementsDictionary.Count - 1; i >= 0; --i)
            {
                var elementsDictionaryItem = elementsDictionary[i];
                var addElementsList = new List<object>();
                foreach (var kvp in elementsDictionaryItem)
                {
                    while (addElementsList.Count < kvp.Key)
                    {
                        addElementsList.Add(null);
                    }

                    addElementsList.Add(kvp.Value);
                }

                this.elements.Insert(index, addElementsList);
            }

            if (this.AfterKeyedValuesInsertRangeEvent != null)
            {
                this.AfterKeyedValuesInsertRangeEvent.Invoke(this, eventArgs);
            }
        }

        public void RemoveRow(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex < this.elements.Count)
            {
                var rowToRemove = new TabularListRow(rowIndex, this);
                var eventArgs = new AddDeleteEventArgs<ITabularRow>(rowToRemove);
                if (this.BeforeDeleteEvent != null)
                {
                    this.BeforeDeleteEvent.Invoke(this, eventArgs);
                }

                this.elements.RemoveAt(rowIndex);

                if (this.AfterDeleteEvent != null)
                {
                    this.AfterDeleteEvent.Invoke(this, eventArgs);
                }
            }
        }

        public int RemoveWhere(Func<ITabularRow, bool> expression)
        {
            var removedLines = 0;
            if (expression != null)
            {
                var rowsToRemove = new List<TabularListRow>();
                for (int i = 0; i < this.elements.Count; ++i)
                {
                    var rowToBeRemoved = new TabularListRow(i, this);
                    if (expression.Invoke(rowToBeRemoved))
                    {
                        rowsToRemove.Add(rowToBeRemoved);
                    }
                }

                var eventArgs = new AddDeleteEventArgs<IEnumerable<ITabularRow>>(rowsToRemove);
                if (this.BeforeDeleteRange != null)
                {
                    this.BeforeDeleteRange(this, eventArgs);
                }

                for (int i = 0; i < rowsToRemove.Count; ++i)
                {
                    var index = rowsToRemove[i].RowNumber - removedLines;
                    this.elements.RemoveAt(index);
                    ++removedLines;
                }

                if (this.AfterDeleteRange != null)
                {
                    this.AfterDeleteRange(this, eventArgs);
                }
            }

            return removedLines;
        }

        public object GetCellValue(int rowNumber, int cellNumber)
        {
            if (rowNumber >= this.elements.Count)
            {
                return null;
            }
            else
            {
                var currentRow = this.elements[rowNumber];
                if (cellNumber >= currentRow.Count)
                {
                    return null;
                }
                else
                {
                    return currentRow[cellNumber];
                }
            }
        }

        public int GetRowCount(int rowNumber)
        {
            if (rowNumber < 0 || rowNumber > this.elements.Count)
            {
                return 0;
            }
            else
            {
                return this.elements[rowNumber].Count;
            }
        }
        #endregion Funções do item tabular

        public IEnumerator<ITabularRow> GetEnumerator()
        {
            for (int i = 0; i < this.elements.Count; ++i)
            {
                yield return new TabularListRow(i, this);
            }
        }

        private void SetCellValue<ElementType>(int rowNumber, int cellNumber, ElementType value)
        {
            for (int i = this.elements.Count; i <= rowNumber; ++i)
            {
                this.elements.Add(new List<object>());
            }

            var line = this.elements[rowNumber];
            for (int i = line.Count; i <= cellNumber; ++i)
            {
                line.Add(null);
            }

            line[cellNumber] = value;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Eventos associados ao item
        public event UpdateEventHandler<ITabularCell, object> BeforeSetEvent;

        public event UpdateEventHandler<ITabularCell, object> AfterSetEvent;

        public event UpdateEventHandler<List<List<ITabularCell>>, List<object>> BeforeUpdateEvent;

        public event UpdateEventHandler<List<List<ITabularCell>>, List<object>> AfterUpdateEvent;

        public event AddDeleteEventHandler<IEnumerable<object>> BeforeAddEvent;

        public event AddDeleteEventHandler<IEnumerable<object>> AfterAddEvent;

        public event AddDeleteEventHandler<IEnumerable<KeyValuePair<int, object>>> BeforeAddKeyedValuesEvent;

        public event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> BeforeAddRangeEvent;

        public event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> AfterAddRangeEvent;

        public event AddDeleteEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            BeforeKeyedValuesAddRangeEvent;

        public event AddDeleteEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            AfterKeyedValuesAddRangeEvent;

        public event AddDeleteEventHandler<IEnumerable<KeyValuePair<int, object>>> AfterAddKeyedValuesEvent;

        public event InsertEventHandler<IEnumerable<object>> BeforeInsertEvent;

        public event InsertEventHandler<IEnumerable<object>> AfterInsertEvent;

        public event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> BeforeKeyedValuesInsertEvent;

        public event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> AfterKeyedValuesInsertEvent;

        public event InsertEventHandler<IEnumerable<IEnumerable<object>>> BeforeInsertRangeEvent;

        public event InsertEventHandler<IEnumerable<IEnumerable<object>>> AfterInsertRangeEvent;

        public event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            BeforeKeyedValuesInsertRangeEvent;

        public event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            AfterKeyedValuesInsertRangeEvent;

        public event AddDeleteEventHandler<ITabularRow> BeforeDeleteEvent;

        public event AddDeleteEventHandler<ITabularRow> AfterDeleteEvent;

        public event AddDeleteEventHandler<IEnumerable<ITabularRow>> BeforeDeleteRange;

        public event AddDeleteEventHandler<IEnumerable<ITabularRow>> AfterDeleteRange;
        #endregion Eventos associados ao item
    }
}
