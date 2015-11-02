namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Dynamic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Permite adicionar atributos ao elemento corrente.
    /// </summary>
    public class AttributeSetter
    {
        /// <summary>
        /// O elemento.
        /// </summary>
        protected XmlElement currentElement;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AttributeSetter"/>.
        /// </summary>
        internal AttributeSetter() { }

        /// <summary>
        /// Obtém o elemento actual.
        /// </summary>
        internal XmlElement CurrentElement
        {
            get
            {
                return this.currentElement;
            }
            set
            {
                this.currentElement = value;
            }
        }

        /// <summary>
        /// Estabelece os valores dos atributos.
        /// </summary>
        /// <param name="name">O nome do atributo.</param>
        /// <param name="value">O valor do atributo.</param>
        public virtual void SetAttribute(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Attribute name can't be null nor empty.");
            }
            else if (value == null)
            {
                this.currentElement.RemoveAttribute(name);
            }
            else
            {
                var attributeValue = value.ToString();
                if (string.IsNullOrWhiteSpace(attributeValue))
                {
                    this.currentElement.RemoveAttribute(name.ToLower());
                }
                else
                {
                    this.currentElement.SetAttribute(name.ToLower(), attributeValue);
                }
            }
        }
    }

    /// <summary>
    /// Permite lidar com elementos.
    /// </summary>
    public class ElementSetter : AttributeSetter
    {
        /// <summary>
        /// Obtém ou atribui o texto do elemento.
        /// </summary>
        public virtual string InnerText
        {
            get
            {
                return this.currentElement.InnerText;
            }
            set
            {
                this.currentElement.InnerText = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o XML interno do elemento.
        /// </summary>
        public virtual string InnerXml
        {
            get
            {
                return this.currentElement.InnerXml;
            }
            set
            {
                this.currentElement.InnerXml = value;
            }
        }

        /// <summary>
        /// Cria um elemento com o nome especificado.
        /// </summary>
        /// <param name="name">O nome do elemento.</param>
        /// <returns>O elemento criado.</returns>
        public virtual XmlElement Create(string name)
        {
            return this.currentElement.OwnerDocument.CreateElement(name);
        }

        /// <summary>
        /// Adiciona um elemento como descendente do elemento corrente.
        /// </summary>
        /// <param name="childElement">O elemento.</param>
        public virtual void AppendChild(XmlElement childElement)
        {
            this.currentElement.AppendChild(childElement);
        }
    }

    /// <summary>
    /// Permite adicionar  atributos a uma coluna, expectuando o "rowspan".
    /// </summary>
    public class ValidatedCellElementSetter : ElementSetter
    {
        /// <summary>
        /// Estabelece os valores dos atributos.
        /// </summary>
        /// <param name="name">O nome do atributo.</param>
        /// <param name="value">O valor do atributo.</param>
        public override void SetAttribute(string name, object value)
        {
            if (StringComparer.InvariantCultureIgnoreCase.Equals(
                name,
                "rowspan") ||
                StringComparer.InvariantCultureIgnoreCase.Equals(
                name,
                "colspan"))
            {
                throw new ArgumentNullException("The attributes rowspan and column span can't be setted for table cell element.");
            }
            else
            {
                base.SetAttribute(name, value);
            }
        }
    }

    /// <summary>
    /// Classe base que permite definir todas as configurações.
    /// </summary>
    public abstract class HtmlWriter
    {
        /// <summary>
        /// Obtém os atributos para a tabela.
        /// </summary>
        protected Action<AttributeSetter> tableAttributes;

        /// <summary>
        /// Obtém os atributos para o elemento do cabeçalho.
        /// </summary>
        protected Action<AttributeSetter> tableHeaderAttributes;

        /// <summary>
        /// Obtém os atributos para o corpo.
        /// </summary>
        protected Action<AttributeSetter> tableBodyAttributes;

        /// <summary>
        /// Obtém os atributos para as linhas do corpo.
        /// </summary>
        protected Action<int, AttributeSetter> bodyRowAttributesSetter;

        /// <summary>
        /// Obtém uma conversão dos valores das células para o corpo.
        /// </summary>
        protected Action<int, int, object, ElementSetter> bodyCellElement;

        /// <summary>
        /// Valor que indica se o texto resultante será identado.
        /// </summary>
        protected bool indent;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="HtmlWriter"/>.
        /// </summary>
        /// <param name="indent">Valor que indica se o texto será identado.</param>
        public HtmlWriter(bool indent = false)
        {
            this.tableAttributes = s => { };
            this.tableHeaderAttributes = s => { };
            this.tableBodyAttributes = s => { };
            this.bodyRowAttributesSetter = (i, s) => { };
            this.bodyCellElement = (i, j, o, e) =>
            {
                if (o != null) { e.InnerText = o.ToString(); }
            };

            this.indent = indent;
        }

        /// <summary>
        /// Obtém ou atribui os atributos para a tabela.
        /// </summary>
        public Action<AttributeSetter> TableAttributes
        {
            get
            {
                return this.tableAttributes;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Table attributes setter action can't be null.");
                }
                else
                {
                    this.tableAttributes = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui os atributos para o elemento do cabeçalho.
        /// </summary>
        public Action<AttributeSetter> TableHeaderAttributes
        {
            get
            {
                return this.tableHeaderAttributes;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Table header attributes setter action can't be null.");
                }
                else
                {
                    this.tableHeaderAttributes = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui os atributos para o corpo.
        /// </summary>
        public Action<AttributeSetter> TableBodyAttributes
        {
            get
            {
                return this.tableBodyAttributes;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Table body attributes setter can't be null.");
                }
                else
                {
                    this.tableBodyAttributes = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui os atributos para as linhas do corpo.
        /// </summary>
        public Action<int, AttributeSetter> BodyRowAttributes
        {
            get
            {
                return this.bodyRowAttributesSetter;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Body row attributes setter can't be null.");
                }
                else
                {
                    this.bodyRowAttributesSetter = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui uma conversão dos valores das células para o corpo.
        /// </summary>
        public Action<int, int, object, ElementSetter> BodyCellElement
        {
            get
            {
                return this.bodyCellElement;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Body cells values converter can't be null.");
                }
                else
                {
                    this.bodyCellElement = value;
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o texto resultante será ou não identado.
        /// </summary>
        public bool Indent
        {
            get
            {
                return this.indent;
            }
            set
            {
                this.indent = value;
            }
        }
    }

    /// <summary>
    /// Permite criar uma tabela de html a partir de um leitor <see cref="IDataReader"/>.
    /// </summary>
    public class HtmlDataReaderWriter : HtmlWriter
    {
        /// <summary>
        /// Obtém os atributos da linha do cabeçalho.
        /// </summary>
        protected Action<AttributeSetter> headerRowAttributes;

        /// <summary>
        /// Obtém o valor da célula convertido.
        /// </summary>
        protected Action<int, string, ElementSetter> headerCellElement;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="HtmlDataReaderWriter"/>.
        /// </summary>
        public HtmlDataReaderWriter()
        {
            this.headerRowAttributes = s => { };
            this.headerCellElement = (i, s, e) => e.InnerText = s;
        }

        /// <summary>
        /// Obtém ou atribui os atributos da linha do cabeçalho.
        /// </summary>
        public Action<AttributeSetter> HeaderRowAttributes
        {
            get
            {
                return this.headerRowAttributes;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Header row attributes action setter can't be null.");
                }
                else
                {
                    this.headerRowAttributes = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da célula convertido.
        /// </summary>
        public Action<int, string, ElementSetter> HeaderCellElement
        {
            get
            {
                return this.headerCellElement;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Header cell object converter can't be null.");
                }
                else
                {
                    this.headerCellElement = value;
                }
            }
        }

        /// <summary>
        /// Obtém html que representa uma tabela a partir de um leitor <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="datareader">O leitor.</param>
        /// <returns>O html como texto.</returns>
        public string GetHtmlFromDataReader(
            IDataReader datareader)
        {
            if (datareader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            else
            {
                var tableDocument = new XmlDocument();
                var attributeSetter = new AttributeSetter();
                var elementSetter = new ElementSetter();

                // Inicializa a tabela
                var table = tableDocument.CreateElement("table");
                attributeSetter.CurrentElement = table;
                this.tableAttributes.Invoke(attributeSetter);
                tableDocument.AppendChild(table);

                // Cabeçalho
                var tableHeader = tableDocument.CreateElement("thead");
                attributeSetter.CurrentElement = tableHeader;
                this.tableHeaderAttributes.Invoke(attributeSetter);
                table.AppendChild(tableHeader);

                // Linhas do cabeçalho
                var tableRow = tableDocument.CreateElement("tr");
                attributeSetter.CurrentElement = tableRow;
                this.headerRowAttributes.Invoke(attributeSetter);
                tableHeader.AppendChild(tableRow);

                // Colunas do cabeçalho
                var fieldCount = datareader.FieldCount;
                for (int i = 0; i < fieldCount; ++i)
                {
                    var columnName = datareader.GetName(i);
                    var tableCell = tableDocument.CreateElement("th");
                    elementSetter.CurrentElement = tableCell;
                    this.headerCellElement.Invoke(i, columnName, elementSetter);
                    tableRow.AppendChild(tableCell);
                }

                // Corpo
                var tableBody = tableDocument.CreateElement("tbody");
                attributeSetter.CurrentElement = tableBody;
                this.tableBodyAttributes.Invoke(attributeSetter);
                table.AppendChild(tableBody);

                // Linhas da tabela
                var line = 0;
                while (datareader.Read())
                {
                    tableRow = tableDocument.CreateElement("tr");
                    attributeSetter.CurrentElement = tableRow;
                    this.bodyRowAttributesSetter.Invoke(line, attributeSetter);
                    tableBody.AppendChild(tableRow);

                    for (int i = 0; i < fieldCount; ++i)
                    {
                        var tableCell = tableDocument.CreateElement("td");
                        elementSetter.CurrentElement = tableCell;
                        this.bodyCellElement.Invoke(
                            line,
                            i,
                            datareader.GetValue(i),
                            elementSetter);
                        tableRow.AppendChild(tableCell);
                    }

                    ++line;
                }

                // Escreve o xml
                var resultBuilder = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = this.indent,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = true
                };

                using (var writer = XmlWriter.Create(resultBuilder, settings))
                {
                    tableDocument.Save(writer);
                }

                return resultBuilder.ToString();
            }
        }
    }

    /// <summary>
    /// Criaçao de uma tabela de html a partir de um <see cref="ITabularItem"/>.
    /// </summary>
    public class HtmlTableWriter : HtmlWriter
    {
        /// <summary>
        /// O conjunto de regiões que identificam a fusão de células do cabeçalho.
        /// </summary>
        NonIntersectingMergingRegionsSet<int, MergingRegion<int>> headerMergingRegions;

        /// <summary>
        /// O conjunto de regiões que identificam a fusão de células do corpo.
        /// </summary>
        NonIntersectingMergingRegionsSet<int, MergingRegion<int>> bodyMergingRegions;

        /// <summary>
        /// Atribui os atributos a cada linha do cabeçalho.
        /// </summary>
        protected Action<int, AttributeSetter> headerRowAttributesSetter;

        /// <summary>
        /// Obtém o valor da célula convertido.
        /// </summary>
        protected Action<int, int, object, ElementSetter> headerCellElement;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="HtmlTableWriter"/>.
        /// </summary>
        public HtmlTableWriter()
            : base()
        {
            this.headerMergingRegions = new NonIntersectingMergingRegionsSet<int, MergingRegion<int>>();
            this.bodyMergingRegions = new NonIntersectingMergingRegionsSet<int, MergingRegion<int>>();
            this.headerRowAttributesSetter = (i, s) => { };
            this.headerCellElement = (i, j, o, e) =>
            {
                if (o != null)
                {
                    e.InnerText = o.ToString();
                }
            };
        }

        /// <summary>
        /// Obtém ou atribui a acção que permite estabelecer os atributos das linhas do cabeçalho.
        /// </summary>
        public Action<int, AttributeSetter> HeaderRowAttributesSetter
        {
            get
            {
                return this.headerRowAttributesSetter;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Header rows attributes setter can't be null.");
                }
                else
                {
                    this.headerRowAttributesSetter = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da célula convertido.
        /// </summary>
        public Action<int, int, object, ElementSetter> HeaderCellElement
        {
            get
            {
                return this.headerCellElement;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException("Header cell object converter can't be null.");
                }
                else
                {
                    this.headerCellElement = value;
                }
            }
        }

        /// <summary>
        /// Obtém o conjunto de regiões que identificam a fusão de células do cabeçalho.
        /// </summary>
        public NonIntersectingMergingRegionsSet<int, MergingRegion<int>> HeaderMergingRegions
        {
            get
            {
                return this.headerMergingRegions;
            }
        }

        /// <summary>
        /// Obtém o conjunto de regiões que identificam a fusão de células do corpo.
        /// </summary>
        public NonIntersectingMergingRegionsSet<int, MergingRegion<int>> BodyMergingRegions
        {
            get
            {
                return this.bodyMergingRegions;
            }
        }

        /// <summary>
        /// Obtém o html que representa uma tabela a partir de um leitor <see cref="ITabularItem"/>.
        /// </summary>
        /// <typeparam name="R">O tipo dos objectos que constituem as linhas.</typeparam>
        /// <typeparam name="L">O tipo dos objectos que constituem as células.</typeparam>
        /// <param name="tabularItemHeader">O item tabular que contém o cabeçalho.</param>
        /// <param name="tabularItemBody">O item tabular que constitui o corpo.</param>
        /// <returns>O html como texto.</returns>
        public string GetHtmlFromTableItem<R, L>(
            IGeneralTabularItem<R> tabularItemHeader,
            IGeneralTabularItem<R> tabularItemBody)
            where R : IGeneralTabularRow<L>
            where L : IGeneralTabularCell
        {
            if (tabularItemHeader == null)
            {
                throw new ArgumentNullException("tabularItemHeader");
            }
            else if (headerMergingRegions == null)
            {
                throw new ArgumentNullException("tabularBody");
            }
            else
            {
                var columnsNumber = Math.Max(
                    this.GetMaxLastColumnNumber<R, L>(tabularItemHeader),
                    this.GetMaxLastColumnNumber<R, L>(tabularItemBody)) + 1;
                var tableDocument = new XmlDocument();
                var attributeSetter = new AttributeSetter();
                var elementSetter = new ElementSetter();

                // Inicializa a tabela
                var table = tableDocument.CreateElement("table");
                attributeSetter.CurrentElement = table;
                this.tableAttributes.Invoke(attributeSetter);
                tableDocument.AppendChild(table);

                // Cabeçalho
                var tableHeader = tableDocument.CreateElement("thead");
                attributeSetter.CurrentElement = tableHeader;
                this.tableHeaderAttributes.Invoke(attributeSetter);
                table.AppendChild(tableHeader);

                this.PrintTabularItem<R, L>(
                    "th",
                    tableHeader,
                    columnsNumber,
                    tabularItemHeader,
                    this.headerMergingRegions,
                    this.headerRowAttributesSetter,
                    this.headerCellElement);

                // Corpo
                var tableBody = tableDocument.CreateElement("tbody");
                attributeSetter.CurrentElement = tableBody;
                this.tableBodyAttributes.Invoke(attributeSetter);
                table.AppendChild(tableBody);

                this.PrintTabularItem<R, L>(
                    "td",
                    tableBody,
                    columnsNumber,
                    tabularItemBody,
                    this.bodyMergingRegions,
                    this.bodyRowAttributesSetter,
                    this.bodyCellElement);

                // Escreve o xml
                var resultBuilder = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = this.indent,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace,
                    OmitXmlDeclaration = true
                };

                using (var writer = XmlWriter.Create(resultBuilder, settings))
                {
                    tableDocument.Save(writer);
                }

                return resultBuilder.ToString();
            }
        }

        /// <summary>
        /// Determina o máximo do número de colunas em cada linha.
        /// </summary>
        /// <typeparam name="R">O tipo dos objectos que constituem as linhas.</typeparam>
        /// <typeparam name="L">O tipo dos objectos que constituem as células.</typeparam>
        /// <param name="tabularItem">O item tabular.</param>
        /// <returns>O máximo do número de colunas.</returns>
        private int GetMaxLastColumnNumber<R, L>(
            IGeneralTabularItem<R> tabularItem)
            where R : IGeneralTabularRow<L>
            where L : IGeneralTabularCell
        {
            var result = 0;
            foreach (var row in tabularItem)
            {
                var lastColumnNumber = row.LastColumnNumber;
                if (result < lastColumnNumber)
                {
                    result = lastColumnNumber;
                }
            }

            return result;
        }

        /// <summary>
        /// Adenda células do vazias a uma linha do cabeçalho.
        /// </summary>
        /// <param name="cellElementName">O nome da célula.</param>
        /// <param name="lineElement">A linha à qual serão adicionadas células vazias.</param>
        /// <param name="currentLine">O número da linha de inserção.</param>
        /// <param name="count">O número de elementos a inserir.</param>
        /// <param name="linesNumber">O número de linhas na tabela.</param>
        /// <param name="columnsNumber">O número de colunas.</param>
        /// <param name="elementSetter">O objecto responsável pela alteração do elemento.</param>
        /// <param name="mergingRegions">As regiões de fusão de células.</param>
        /// <param name="elementsSetter">A função responsável pelo processamento dos elementos.</param>
        private void AppendEmptyCells(
            string cellElementName,
            XmlElement lineElement,
            int currentLine,
            int count,
            int linesNumber,
            int columnsNumber,
            ValidatedCellElementSetter elementSetter,
            NonIntersectingMergingRegionsSet<int,MergingRegion<int>> mergingRegions,
            Action<int,int,object,ElementSetter> elementsSetter)
        {
            for (int i = 0; i < count; ++i)
            {
                var mergingRegion = mergingRegions.GetMergingRegionForCell(
                    i,
                    currentLine);
                if (mergingRegion == null)
                {
                    var tableCellElement = lineElement.OwnerDocument.CreateElement(cellElementName);
                    elementSetter.CurrentElement = tableCellElement;
                    lineElement.AppendChild(tableCellElement);

                    elementsSetter.Invoke(
                        currentLine,
                        i,
                        null,
                        elementSetter);
                    lineElement.AppendChild(tableCellElement);
                }
                else if (mergingRegion.TopLeftX == i)
                {
                    var colDifference = mergingRegion.BottomRightX - mergingRegion.TopLeftY;
                    if (mergingRegion.TopLeftY == currentLine)
                    {
                        var tableCellElement = lineElement.OwnerDocument.CreateElement(cellElementName);
                        elementSetter.CurrentElement = tableCellElement;
                        lineElement.AppendChild(tableCellElement);
                        elementsSetter.Invoke(
                        currentLine,
                        i,
                        null,
                        elementSetter);

                        if (colDifference > 0)
                        {
                            var colSpan = Math.Min(
                                colDifference + 1,
                                columnsNumber - i);
                            if (colSpan > 1)
                            {
                                tableCellElement.SetAttribute("colspan", colSpan.ToString());
                            }
                        }

                        var rowDifference = mergingRegion.BottomRightY - mergingRegion.TopLeftY;
                        if (rowDifference > 0)
                        {
                            var rowSpan = Math.Min(
                                rowDifference + 1,
                                linesNumber - currentLine);
                            if (rowSpan > 1)
                            {
                                tableCellElement.SetAttribute("rowspan", rowSpan.ToString());
                            }
                        }
                    }

                    i += colDifference;
                }
            }
        }

        /// <summary>
        /// Imprime um item tabular para o elemento da tabela.
        /// </summary>
        /// <typeparam name="R">O tipo dos objectos que constituem as linhas do item tabular.</typeparam>
        /// <typeparam name="L">O tipo dos objectos que constituem as células do item tabular.</typeparam>
        /// <param name="cellElementName">O nome do elemento da célula.</param>
        /// <param name="element">O elemento da tabela que irá conter as linhas.</param>
        /// <param name="columnsNumber">O número de colunas esperado.</param>
        /// <param name="tabularItem">O item tabular.</param>
        /// <param name="mergingRegions">O conjunto de regiões de células fundidas.</param>
        /// <param name="attributessSetter">A função responsável pela adição de atributos.</param>
        /// <param name="elementsSetter">A função responsável pelo processamento de elementos.</param>
        private void PrintTabularItem<R, L>(
            string cellElementName,
            XmlElement element,
            int columnsNumber,
            IGeneralTabularItem<R> tabularItem,
            NonIntersectingMergingRegionsSet<int,MergingRegion<int>> mergingRegions,
            Action<int, AttributeSetter> attributessSetter,
            Action<int,int, object, ElementSetter> elementsSetter)
            where R : IGeneralTabularRow<L>
            where L : IGeneralTabularCell
        {
            var attributeSetter = new AttributeSetter();
            var validatedCellElementSetter = new ValidatedCellElementSetter();
            var linesNumber = tabularItem.LastRowNumber + 1;
            var linePointer = 0;
            foreach (var tabularLine in tabularItem)
            {
                var currentLine = tabularLine.RowNumber;
                for (; linePointer < currentLine; ++linePointer)
                {
                    var rowElement = element.OwnerDocument.CreateElement("tr");
                    attributeSetter.CurrentElement = rowElement;
                    attributessSetter.Invoke(
                        linePointer,
                        attributeSetter);
                    this.AppendEmptyCells(
                        cellElementName,
                        rowElement,
                        linePointer,
                        columnsNumber,
                        linesNumber,
                        columnsNumber,
                        validatedCellElementSetter,
                        mergingRegions,
                        elementsSetter);
                    element.AppendChild(rowElement);
                }

                var lineElement = element.OwnerDocument.CreateElement("tr");
                attributeSetter.CurrentElement = lineElement;
                attributessSetter.Invoke(
                    linePointer,
                    attributeSetter);
                var currentCellNumber = 0;
                foreach (var tabularCell in tabularLine)
                {
                    if (currentCellNumber <= tabularCell.ColumnNumber)
                    {
                        var emptyCellsCount = tabularCell.ColumnNumber - currentCellNumber;
                        this.AppendEmptyCells(
                            cellElementName,
                            lineElement,
                            linePointer,
                            emptyCellsCount,
                            linesNumber,
                            columnsNumber,
                            validatedCellElementSetter,
                            mergingRegions,
                            elementsSetter);

                        var mergingRegion = mergingRegions.GetMergingRegionForCell(
                            tabularCell.ColumnNumber,
                            currentLine);
                        if (mergingRegion == null)
                        {
                            var tableCellElement = lineElement.OwnerDocument.CreateElement(cellElementName);
                            validatedCellElementSetter.CurrentElement = tableCellElement;
                            lineElement.AppendChild(tableCellElement);

                            elementsSetter.Invoke(
                                currentLine,
                                tabularCell.ColumnNumber,
                                tabularCell.GetCellValue<object>(),
                                validatedCellElementSetter);
                            lineElement.AppendChild(tableCellElement);
                            currentCellNumber = tabularCell.ColumnNumber + 1;
                        }
                        else if (mergingRegion.TopLeftX == tabularCell.ColumnNumber)
                        {
                            var colDifference = mergingRegion.BottomRightX - mergingRegion.TopLeftX;
                            if (mergingRegion.TopLeftY == currentLine)
                            {
                                var tableCellElement = lineElement.OwnerDocument.CreateElement(cellElementName);
                                validatedCellElementSetter.CurrentElement = tableCellElement;
                                lineElement.AppendChild(tableCellElement);
                                elementsSetter.Invoke(
                                    currentLine,
                                    tabularCell.ColumnNumber,
                                    tabularCell.GetCellValue<object>(),
                                    validatedCellElementSetter);

                                if (colDifference > 0)
                                {
                                    var colSpan = Math.Min(
                                        colDifference + 1,
                                        columnsNumber - tabularCell.ColumnNumber);
                                    if (colSpan > 1)
                                    {
                                        tableCellElement.SetAttribute("colspan", colSpan.ToString());
                                    }
                                }

                                var rowDifference = mergingRegion.BottomRightY - mergingRegion.TopLeftY;
                                if (rowDifference > 0)
                                {
                                    var rowSpan = Math.Min(
                                        rowDifference + 1,
                                        linesNumber - currentLine);
                                    if (rowSpan > 1)
                                    {
                                        tableCellElement.SetAttribute("rowspan", rowSpan.ToString());
                                    }
                                }
                            }

                            currentCellNumber = tabularCell.ColumnNumber + colDifference + 1;
                        }
                    }
                }

                var finalEmptyCellsCount = columnsNumber - currentCellNumber;
                this.AppendEmptyCells(
                        cellElementName,
                        lineElement,
                        linePointer,
                        finalEmptyCellsCount,
                        linesNumber,
                        columnsNumber,
                        validatedCellElementSetter,
                        mergingRegions,
                        elementsSetter);

                element.AppendChild(lineElement);
                ++linePointer;
            }

            // Inserção das restantes linhas vazias
            for (; linePointer < linesNumber; ++linePointer)
            {
                var rowElement = element.OwnerDocument.CreateElement("th");
                attributeSetter.CurrentElement = rowElement;
                this.headerRowAttributesSetter.Invoke(
                    linePointer,
                    attributeSetter);
                this.AppendEmptyCells(
                        cellElementName,
                        rowElement,
                        linePointer,
                        columnsNumber,
                        linesNumber,
                        columnsNumber,
                        validatedCellElementSetter,
                        mergingRegions,
                        elementsSetter);
            }
        }
    }
}
