namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma tabela generalizada.
    /// </summary>
    /// <typeparam name="R">O tipo dos objectos que constituem as linhas da tabela.</typeparam>
    /// <typeparam name="L">O tipo dos objectos que constituem as células da tabela.</typeparam>
    public abstract class AGeneralTabularListItem<R, L> : IGeneralTabularItem<R>
        where R : IGeneralTabularRow<L>
        where L : IGeneralTabularCell
    {
        /// <summary>
        /// Mantém a tabela dos elementos.
        /// </summary>
        protected List<List<object>> elements;

        /// <summary>
        /// O conjunto de validações às células.
        /// </summary>
        protected List<IDataValidation<int, object>> dataCellValidations;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="IGeneralTabularItem{R}"/>.
        /// </summary>
        public AGeneralTabularListItem()
        {
            this.elements = new List<List<object>>();
            this.dataCellValidations = new List<IDataValidation<int, object>>();
        }

        /// <summary>
        /// Obtém a linha especificada pelo índice.
        /// </summary>
        /// <value>A linha.</value>
        /// <param name="index">O índice.</param>
        /// <returns>A linha.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o índice for negativo.</exception>
        public R this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    return this.GetRow(index);
                }
            }
        }

        /// <summary>
        /// Obtém o número de linhas na tabela.
        /// </summary>
        /// <value>O número de linhas.</value>
        public int Count
        {
            get
            {
                return this.elements.Count;
            }
        }

        /// <summary>
        /// Obtém o número da última linha.
        /// </summary>
        public int LastRowNumber
        {
            get
            {
                return this.elements.Count - 1;
            }
        }

        #region Validações

        /// <summary>
        /// Adiciona uma validação À tabela.
        /// </summary>
        /// <param name="validation">A validação.</param>
        /// <param name="validateExisting">
        /// Valor que indica se os itens existentes serão validados.
        /// </param>
        /// <exception cref="ArgumentNullException">Se a validação for nula.</exception>
        /// <exception cref="UtilitiesDataException">Se a validação falhar para os dados existentes.</exception>
        public void AddValidation(IDataValidation<int, object> validation, bool validateExisting = true)
        {
            if (validation == null)
            {
                throw new ArgumentNullException("validation");
            }
            else
            {
                if (validateExisting)
                {
                    for (int i = 0; i < this.elements.Count; ++i)
                    {
                        if (!this.ValidateRow(this.elements[i], validation))
                        {
                            throw new UtilitiesDataException(string.Format(
                                "Data row {0} doesn't comply with the provided validation.",
                                i));
                        }
                    }
                }

                this.dataCellValidations.Add(validation);
            }
        }

        /// <summary>
        /// Remove a validação.
        /// </summary>
        /// <param name="validation">A validação.</param>
        public void RemoveValidation(IDataValidation<int, object> validation)
        {
            if (validation != null)
            {
                this.dataCellValidations.Remove(validation);
            }
        }

        /// <summary>
        /// Elimina todas as validações.
        /// </summary>
        public void ClearValidations()
        {
            this.dataCellValidations.Clear();
        }

        #endregion Validações

        /// <summary>
        /// Obtém o valor não genérico da célula.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="cellNumber">O número da coluna.</param>
        /// <returns>O valor da célula.</returns>
        public virtual object GetCellValue(int rowNumber, int cellNumber)
        {
            if (rowNumber < 0)
            {
                throw new ArgumentOutOfRangeException("cellNumber");
            }
            else if (rowNumber >= this.elements.Count)
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

        /// <summary>
        /// Obtém o número de células na linha especificada.
        /// </summary>
        /// <param name="rowNumber">O índice da linha.</param>
        /// <returns>O número de células na linha.</returns>
        public virtual int ColumnsCount(int rowNumber)
        {
            if (rowNumber < 0 || rowNumber >= this.elements.Count)
            {
                return 0;
            }
            else
            {
                return this.elements[rowNumber].Count;
            }
        }

        /// <summary>
        /// Obtém o número da última coluna na linha especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número da coluna.</returns>
        public virtual int GetLastColumnNumber(int rowNumber)
        {
            if (rowNumber < 0 || rowNumber >= this.elements.Count)
            {
                return -1;
            }
            else
            {
                return this.elements[rowNumber].Count - 1;
            }
        }

        /// <summary>
        /// Obtém um enumerador para a tabela.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public virtual IEnumerator<R> GetEnumerator()
        {
            for (int i = 0; i < this.elements.Count; ++i)
            {
                yield return this.GetRow(i);
            }
        }

        /// <summary>
        /// Constrói uma representação textual da tabela.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            var elementsEnumerator = this.elements.GetEnumerator();
            if (elementsEnumerator.MoveNext())
            {
                var currentLineEnumerator = elementsEnumerator.Current.GetEnumerator();
                if (currentLineEnumerator.MoveNext())
                {
                    resultBuilder.Append(currentLineEnumerator.Current);
                    while (currentLineEnumerator.MoveNext())
                    {
                        resultBuilder.Append("; ");
                        resultBuilder.Append(currentLineEnumerator.Current);
                    }
                }

                while (elementsEnumerator.MoveNext())
                {
                    resultBuilder.Append(System.Environment.NewLine);
                    var currentLinEnum = elementsEnumerator.Current.GetEnumerator();
                    if (currentLinEnum.MoveNext())
                    {
                        resultBuilder.Append(currentLinEnum.Current);
                        while (currentLinEnum.MoveNext())
                        {
                            resultBuilder.Append("; ");
                            resultBuilder.Append(currentLinEnum.Current);
                        }
                    }
                }
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Obtém a linha especificada pelo índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>A linha.</returns>
        protected abstract R GetRow(int rowNumber);

        /// <summary>
        /// Valida uma linha completa.
        /// </summary>
        /// <param name="row">A linha a ser validada.</param>
        /// <param name="validation">A validação.</param>
        /// <returns>Verdadeiro caso a linha seja válida e falso caso contrário.</returns>
        protected bool ValidateRow(List<object> row, IDataValidation<int, object> validation)
        {
            if (validation.Columns == null)
            {
                return validation.Validate(row);
            }
            else
            {
                var objectsSet = new List<object>();
                foreach (var currentValidationColumn in validation.Columns)
                {
                    var objectValue = default(object);
                    if (currentValidationColumn < row.Count)
                    {
                        objectValue = row[currentValidationColumn];
                    }

                    objectsSet.Add(objectValue);
                }

                return validation.Validate(objectsSet);
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a tabela.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Impelmenta uma tabela só de leitura.
    /// </summary>
    public class ReadonlyTabularListItem
        : AGeneralTabularListItem<IReadonlyTabularRow, IReadonlyTabularCell>, IReadonlyTabularItem
    {
        /// <summary>
        /// Isntancia uma nova instância de objectos do tipo <see cref="ReadonlyTabularListItem"/>.
        /// </summary>
        /// <remarks>
        /// Permite realizar instâncias por classes internas à livraria.
        /// </remarks>
        internal ReadonlyTabularListItem()
        {
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ReadonlyTabularListItem"/>.
        /// </summary>
        /// <param name="items">A lista dos elementos que fazem parte da tabela.</param>
        public ReadonlyTabularListItem(List<List<object>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            else
            {
                this.elements = items;
            }
        }

        /// <summary>
        /// Obtém o contentor para os elementos.
        /// </summary>
        internal List<List<object>> Elements
        {
            get
            {
                return this.elements;
            }
        }

        /// <summary>
        /// Obtém a linha especificada pelo índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>A linha.</returns>
        protected override IReadonlyTabularRow GetRow(int rowNumber)
        {
            return new ReadonlyTabularListRow<IReadonlyTabularItem, IReadonlyTabularRow, IReadonlyTabularCell>(
                rowNumber,
                this);
        }
    }

    /// <summary>
    /// Implementa uma sub-tabela de uma tabela, especificando as linhas e colunas limite.
    /// </summary>
    /// <typeparam name="P">O tipo de objectos que constitui a tabela geral.</typeparam>
    /// <typeparam name="R">O tipo de objectos que constitui a linha da tabela.</typeparam>
    /// <typeparam name="L">O tipo de objectos que constitui a coluna da tabela.</typeparam>
    public class WindowReadonlyTabularItem<P, R, L>
        : IReadonlyTabularItem
        where P : IGeneralTabularItem<R>
        where R : IGeneralTabularRow<L>
        where L : IGeneralTabularCell
    {
        /// <summary>
        /// O item tabular apontado pela sub-tabela.
        /// </summary>
        protected P tabularItem;

        /// <summary>
        /// A linha incial da sub-tabela.
        /// </summary>
        protected int startRow;

        /// <summary>
        /// O número de linhas considerado na sub-tabela.
        /// </summary>
        protected int rowCount;

        /// <summary>
        /// A linha final da sub-tabela.
        /// </summary>
        protected int startColumn;

        /// <summary>
        /// O número de colunas considerado na sub-tabela.
        /// </summary>
        protected int columnCount;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="WindowReadonlyTabularItem{P,R,L}"/>.
        /// </summary>
        /// <param name="tabularItem">A tabela da qual se pretende obter a sub-tabela.</param>
        /// <param name="startRow">A linha inicial da sub-tabela.</param>
        /// <param name="rowCount">O número de linhas consideradas na sub-tabela.</param>
        /// <param name="startColumn">A coluna incial da sub-tabela.</param>
        /// <param name="columnCount">O número de colunas consideradas na sub-tabela.</param>
        public WindowReadonlyTabularItem(
            P tabularItem,
            int startRow,
            int rowCount,
            int startColumn,
            int columnCount)
        {
            if (tabularItem == null)
            {
                throw new ArgumentNullException("tabularItem");
            }
            else if (startRow < 0)
            {
                throw new ArgumentOutOfRangeException("startRow");
            }
            else if (rowCount < 0)
            {
                throw new ArgumentOutOfRangeException("rowCount");
            }
            else if (startColumn < 0)
            {
                throw new ArgumentOutOfRangeException("startColumn");
            }
            else if (columnCount < 0)
            {
                throw new ArgumentOutOfRangeException("endColumn");
            }
            else
            {
                this.tabularItem = tabularItem;
                this.startRow = startRow;
                this.rowCount = rowCount;
                this.startColumn = startColumn;
                this.columnCount = columnCount;
            }
        }

        /// <summary>
        /// Obtém o objecto especificado pelo índice.
        /// </summary>
        /// <value>
        /// O objecto.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        public IReadonlyTabularRow this[int index]
        {
            get
            {
                if (index < 0 || index >= this.rowCount)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    return new ReadonlyTabularListRow<P, R, L>(index, this.tabularItem);
                }
            }
        }

        /// <summary>
        /// Obtém o número da última linha.
        /// </summary>
        public int LastRowNumber
        {
            get
            {
                return this.rowCount - 1;
            }
        }

        /// <summary>
        /// Obtém o número de elementos.
        /// </summary>
        /// <value>
        /// O número de elementos.
        /// </value>
        public int Count
        {
            get
            {
                return this.rowCount;
            }
        }

        /// <summary>
        /// Obtém o valor não genérico da célula.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O valor da célula.</returns>
        public int ColumnsCount(int rowNumber)
        {
            if (rowNumber < 0 || rowNumber >= this.rowCount)
            {
                throw new ArgumentOutOfRangeException("rowNumber");
            }
            else
            {
                return this.columnCount;
            }
        }

        /// <summary>
        /// Obtém o número da última coluna na linha especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número da coluna.</returns>
        public int GetLastColumnNumber(int rowNumber)
        {
            if (rowNumber < 0 || rowNumber >= this.rowCount)
            {
                throw new ArgumentOutOfRangeException("rowNumber");
            }
            else
            {
                return this.columnCount - 1;
            }
        }

        /// <summary>
        /// Obtém o valor não genérico da célula.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="cellNumber">O número da coluna.</param>
        /// <returns>O valor da célula.</returns>
        public object GetCellValue(int rowNumber, int cellNumber)
        {
            if (rowNumber < 0 || rowNumber >= this.rowCount)
            {
                throw new IndexOutOfRangeException("rowNumber");
            }
            else if (cellNumber < 0 || cellNumber >= this.columnCount)
            {
                throw new IndexOutOfRangeException("columnNumber");
            }
            else
            {
                return this.tabularItem.GetCellValue(
                    this.startRow + rowNumber,
                    this.startColumn + cellNumber);
            }
        }

        /// <summary>
        /// Obtém um enumerador para a tabela.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<IReadonlyTabularRow> GetEnumerator()
        {
            for (int i = 0; i < this.rowCount; ++i)
            {
                yield return new ReadonlyTabularListRow<WindowReadonlyTabularItem<P, R, L>, IReadonlyTabularRow, IReadonlyTabularCell>(
                    i,
                    this);
            }
        }

        /// <summary>
        /// Obtém a linha especificada pelo índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>A linha.</returns>
        protected IReadonlyTabularRow GetRow(int rowNumber)
        {
            return new ReadonlyTabularListRow<WindowReadonlyTabularItem<P, R, L>, IReadonlyTabularRow, IReadonlyTabularCell>(
                   this.startRow + rowNumber,
                   this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Classe que permite representar uma tabela por intermédio de listas.
    /// </summary>
    public class TabularListsItem
        : AGeneralTabularListItem<ITabularRow, ITabularCell>, ITabularItem
    {
        #region Funções do item tabular

        /// <summary>
        /// Atribui um valor na posição especificada.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objecto que constitui o valor.</typeparam>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="cellNumber">O número da coluna.</param>
        /// <param name="value">O valor.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se a linha ou a coluna forem negativos.</exception>
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

                var lineToValidate = this.elements[rowNumber];
                for (int i = 0; i < this.dataCellValidations.Count; ++i)
                {
                    var currentDataValidation = this.dataCellValidations[i];
                    if (currentDataValidation.Columns == null)
                    {
                        if (!currentDataValidation.Validate(lineToValidate))
                        {
                            throw new UtilitiesDataException("A validation has failed for the specified value.");
                        }
                    }
                    else
                    {
                        var objectSet = new List<object>();
                        foreach (var currentIndexColumn in currentDataValidation.Columns)
                        {
                            var objectValue = default(object);
                            if (currentIndexColumn == cellNumber)
                            {
                                objectValue = value;
                            }
                            else if (currentIndexColumn < lineToValidate.Count)
                            {
                                objectValue = lineToValidate[currentIndexColumn];
                            }

                            objectSet.Add(objectValue);
                        }

                        if (!currentDataValidation.Validate(objectSet))
                        {
                            throw new UtilitiesDataException("A validation has failed for the specified value.");
                        }
                    }
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

        /// <summary>
        /// Adiciona uma linha definda por uma colecção de elementos.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="elements">Os elementos.</param>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar para a linha.</exception>
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

            for (int i = 0; i < this.dataCellValidations.Count; ++i)
            {
                if (!this.ValidateRow(elementsToAdd, this.dataCellValidations[i]))
                {
                    throw new UtilitiesDataException("Added line isn't valid.");
                }
            }

            this.elements.Add(elementsToAdd);

            if (this.AfterAddEvent != null)
            {
                this.AfterAddEvent.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Adiciona uma linha especificada por mapeamentos entre colunas e valores.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="elements">O conjunto de mapeamentos.</param>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar no processo.</exception>
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

                for (int i = 0; i < this.dataCellValidations.Count; ++i)
                {
                    var currentValidation = this.dataCellValidations[i];
                    if (currentValidation.Columns == null)
                    {
                        currentValidation.Validate(addElementsList);
                    }
                }

                for (int i = 0; i < this.dataCellValidations.Count; ++i)
                {
                    if (!this.ValidateRow(addElementsList, this.dataCellValidations[i]))
                    {
                        throw new UtilitiesDataException("Added line isn't valid.");
                    }
                }

                this.elements.Add(addElementsList);
                if (this.AfterAddKeyedValuesEvent != null)
                {
                    this.AfterAddKeyedValuesEvent.Invoke(this, eventArgs);
                }
            }
        }

        /// <summary>
        /// Adiciona um conjunto de linhas especificadas por vários valores.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="elements">O conjunto de colecções de elementos.</param>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar no processo.</exception>
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

            foreach (var elementToAdd in elementsToAdd)
            {
                for (int i = 0; i < this.dataCellValidations.Count; ++i)
                {
                    if (!this.ValidateRow(elementToAdd, this.dataCellValidations[i]))
                    {
                        throw new UtilitiesDataException("Added line isn't valid.");
                    }
                }
            }

            this.elements.AddRange(elementsToAdd);

            if (this.AfterAddRangeEvent != null)
            {
                this.AfterAddRangeEvent.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Adiciona um conjunto de linhas especificadas por vários mapeamentos entre coluna e valor.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="elements">O conjunto de colecções de elementos mapeados.</param>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar no processo.</exception>
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

                for (int i = 0; i < this.dataCellValidations.Count; ++i)
                {
                    if (!this.ValidateRow(addElementsList, this.dataCellValidations[i]))
                    {
                        throw new UtilitiesDataException("Added line isn't valid.");
                    }
                }

                this.elements.Add(addElementsList);
            }

            if (this.AfterAddRangeEvent != null)
            {
                this.AfterKeyedValuesAddRangeEvent.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Insere uma linha especificada por mapeamentos entre colunas e valores no índice especificado.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="elements">O conjunto de mapeamentos.</param>
        /// <param name="index">O índice.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o índice for negativo.</exception>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar no processo.</exception>
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

                for (int i = 0; i < this.dataCellValidations.Count; ++i)
                {
                    if (!this.ValidateRow(elementsToAdd, this.dataCellValidations[i]))
                    {
                        throw new UtilitiesDataException("Added line isn't valid.");
                    }
                }

                this.elements.Insert(index, elementsToAdd);

                if (this.AfterInsertEvent != null)
                {
                    this.AfterInsertEvent.Invoke(this, eventArgs);
                }
            }
        }

        /// <summary>
        /// Insere uma linha especificada por mapeamentos entre colunas e valores no índice especificado.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="index">O índice.</param>
        /// <param name="elements">O conjunto de mapeamentos.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o índice for negativo.</exception>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar no processo.</exception>
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

                    for (int i = 0; i < this.dataCellValidations.Count; ++i)
                    {
                        if (!this.ValidateRow(addElementsList, this.dataCellValidations[i]))
                        {
                            throw new UtilitiesDataException("Added line isn't valid.");
                        }
                    }

                    this.elements.Insert(index, addElementsList);

                    if (this.AfterKeyedValuesInsertEvent != null)
                    {
                        this.AfterKeyedValuesInsertEvent.Invoke(this, eventArgs);
                    }
                }
            }
        }

        /// <summary>
        /// Adiciona um conjunto de linhas especificadas por vários valores no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="elements">O conjunto de colecções de elementos.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o índice for negativo.</exception>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar no processo.</exception>
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

                foreach (var elementToAdd in elementsToAdd)
                {
                    for (int i = 0; i < this.dataCellValidations.Count; ++i)
                    {
                        if (!this.ValidateRow(elementToAdd, this.dataCellValidations[i]))
                        {
                            throw new UtilitiesDataException("Added line isn't valid.");
                        }
                    }
                }

                this.elements.InsertRange(index, elementsToAdd);

                if (this.AfterInsertRangeEvent != null)
                {
                    this.AfterInsertRangeEvent.Invoke(this, eventArgs);
                }
            }
        }

        /// <summary>
        /// Adiciona um conjunto de linhas especificadas por vários mapeamentos entre coluna e valor no índice especificado..
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <typeparam name="ElementType">O tipo de objectos que constituem os elementos.</typeparam>
        /// <param name="elements">O conjunto de colecções de elementos mapeados.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o índice for negativo.</exception>
        /// <exception cref="UtilitiesDataException">Se alguma validação falhar no processo.</exception>
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

                for (int j = 0; j < this.dataCellValidations.Count; ++j)
                {
                    if (!this.ValidateRow(addElementsList, this.dataCellValidations[j]))
                    {
                        throw new UtilitiesDataException("Added line isn't valid.");
                    }
                }

                this.elements.Insert(index, addElementsList);
            }

            if (this.AfterKeyedValuesInsertRangeEvent != null)
            {
                this.AfterKeyedValuesInsertRangeEvent.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Remove a linha com o índice especificado.
        /// </summary>
        /// <param name="rowIndex">O índice da linha.</param>
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

        /// <summary>
        /// Remove a linha que satisfaz alguma expressão.
        /// </summary>
        /// <param name="expression">A expressão.</param>
        /// <returns>A linha a remover.</returns>
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

        #endregion Funções do item tabular

        /// <summary>
        /// Obtém a linha associada ao índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>A linha.</returns>
        protected override ITabularRow GetRow(int rowNumber)
        {
            return new TabularListRow(rowNumber, this);
        }

        /// <summary>
        /// Atribui o valor a uma célula.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de objecto a ser atribuído.</typeparam>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="cellNumber">O número da coluna.</param>
        /// <param name="value">O valor.</param>
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

        #region Eventos associados ao item

        /// <summary>
        /// Ocorre antes do valor da célula ser alterado.
        /// </summary>
        public event UpdateEventHandler<ITabularCell, object> BeforeSetEvent;

        /// <summary>
        /// Ocorre depois do valor da célula ser alterado.
        /// </summary>
        public event UpdateEventHandler<ITabularCell, object> AfterSetEvent;

        /// <summary>
        /// Ocorrea antes de uma célula ser actualizada.
        /// </summary>
        public event UpdateEventHandler<List<List<ITabularCell>>, List<object>> BeforeUpdateEvent;

        /// <summary>
        /// Ocorre depois de uma célula ser actualizada.
        /// </summary>
        public event UpdateEventHandler<List<List<ITabularCell>>, List<object>> AfterUpdateEvent;

        /// <summary>
        /// Ocorre antes da adição de uma linha.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<object>> BeforeAddEvent;

        /// <summary>
        /// Ocorre depois da adição de uma linha.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<object>> AfterAddEvent;

        /// <summary>
        /// Ocorre antes da adição de um mapeamento coluna - valor.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<KeyValuePair<int, object>>> BeforeAddKeyedValuesEvent;

        /// <summary>
        /// Ocorre depois da adição de um mapeamento coluna - valor.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<KeyValuePair<int, object>>> AfterAddKeyedValuesEvent;

        /// <summary>
        /// Ocorre antes da adição de várias linhas.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> BeforeAddRangeEvent;

        /// <summary>
        /// Ocorre depois da adição de várias linhas.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> AfterAddRangeEvent;

        /// <summary>
        /// Ocorre antes da adição de vários mapeamentos coluna - valor.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            BeforeKeyedValuesAddRangeEvent;

        /// <summary>
        /// Ocorre depois da adição de vários mapeamentos coluna - valor.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            AfterKeyedValuesAddRangeEvent;

        /// <summary>
        /// Ocorre antes da inserção de uma linha.
        /// </summary>
        public event InsertEventHandler<IEnumerable<object>> BeforeInsertEvent;

        /// <summary>
        /// Ocorre depois da inserção de uma linha.
        /// </summary>
        public event InsertEventHandler<IEnumerable<object>> AfterInsertEvent;

        /// <summary>
        /// Ocorre antes da inserção de um mapeamento coluna - valor.
        /// </summary>
        public event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> BeforeKeyedValuesInsertEvent;

        /// <summary>
        /// Ocorre depois de um mapeamento coluna - valor.
        /// </summary>
        public event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> AfterKeyedValuesInsertEvent;

        /// <summary>
        /// Ocorre antes da inserção de várias linhas.
        /// </summary>
        public event InsertEventHandler<IEnumerable<IEnumerable<object>>> BeforeInsertRangeEvent;

        /// <summary>
        /// Ocorre depois da inserção de várias linhas.
        /// </summary>
        public event InsertEventHandler<IEnumerable<IEnumerable<object>>> AfterInsertRangeEvent;

        /// <summary>
        /// Ocorre antes da inserção de vários mapeamentos coluna - valor.
        /// </summary>
        public event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            BeforeKeyedValuesInsertRangeEvent;

        /// <summary>
        /// Ocorre depois da inserção de várias mapeamentos coluna - valor.
        /// </summary>
        public event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>>
            AfterKeyedValuesInsertRangeEvent;

        /// <summary>
        /// Ocorre antes da remoção de uma linha.
        /// </summary>
        public event AddDeleteEventHandler<ITabularRow> BeforeDeleteEvent;

        /// <summary>
        /// Ocorre depois da remoção de uma linha.
        /// </summary>
        public event AddDeleteEventHandler<ITabularRow> AfterDeleteEvent;

        /// <summary>
        /// Ocorre antes da remoção de várias linhas.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<ITabularRow>> BeforeDeleteRange;

        /// <summary>
        /// Ocorre depois da remoção de várias linhas.
        /// </summary>
        public event AddDeleteEventHandler<IEnumerable<ITabularRow>> AfterDeleteRange;

        #endregion Eventos associados ao item
    }
}
