//namespace Utilities
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;
//    using System.Xml;

//    /// <summary>
//    /// Implementa um leitor e conversor entre estruturas de dados tabulares e tabelas em html.
//    /// </summary>
//    public class HtmlCssFormattedDataReaderWriter
//    {
//        /// <summary>
//        /// O css por defeito para a tabela.
//        /// </summary>
//        private string defaultTableClassName;

//        /// <summary>
//        /// O css por defeito para a tabela.
//        /// </summary>
//        private string defaultBodyClassName;

//        /// <summary>
//        /// O css para o cabeçalho.
//        /// </summary>
//        private string defaultHeaderClassName;

//        /// <summary>
//        /// Função que define o css por linha.
//        /// </summary>
//        private Func<int, string> rowStyleProvider;

//        /// <summary>
//        /// Função que define o css por célula.
//        /// </summary>
//        private Func<int, int, string> cellStyleProvider;

//        /// <summary>
//        /// A primeira linha do corpo da tabela.
//        /// </summary>
//        private int firstBodyLineNumber = 0;

//        /// <summary>
//        /// Define as células fundidas.
//        /// </summary>
//        private NonIntersectingMergingRegionsSet<int, RectangularRegion<int>> mergingRegionSet;

//        /// <summary>
//        /// Instancia uma nova instância de objectos do tipo <see cref="TableToHtmlConverter"/>.
//        /// </summary>
//        public HtmlCssFormattedDataReaderWriter()
//        {
//            this.mergingRegionSet = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
//        }

//        /// <summary>
//        /// Obtém ou atribui o css por defeito para a tabela.
//        /// </summary>
//        public string DefaultTableClassName
//        {
//            get
//            {
//                return this.defaultTableClassName;
//            }
//            set
//            {
//                this.defaultTableClassName = value;
//            }
//        }

//        /// <summary>
//        /// Obtém ou atribui o estilo por defeito para o corpo da tabela.
//        /// </summary>
//        public string DefaultBodyClassName
//        {
//            get
//            {
//                return this.defaultBodyClassName;
//            }
//            set
//            {
//                this.defaultBodyClassName = value;
//            }
//        }

//        /// <summary>
//        /// Obtém ou atribui o estilo por defeito para o cabeçalho.
//        /// </summary>
//        public string DefaultHeaderClassName
//        {
//            get
//            {
//                return this.defaultHeaderClassName;
//            }
//            set
//            {
//                this.defaultHeaderClassName = value;
//            }
//        }

//        /// <summary>
//        /// Obtém a colecção de regiões correspondentes a células que são para fundir.
//        /// </summary>
//        public NonIntersectingMergingRegionsSet<int, RectangularRegion<int>> MergingRegionSet
//        {
//            get
//            {
//                return this.mergingRegionSet;
//            }
//        }

//        /// <summary>
//        /// Obtém a representação da tabela em html.
//        /// </summary>
//        /// <param name="table">A tabela.</param>
//        /// <param name="id">O identificador a ser atribuído à tabela.</param>
//        /// <param name="offset">O deslocamento a partir da primeira linha do corpo da tabela.</param>
//        /// <param name="size">O número de linhas do corpo da tabela a serem imprimidas.</param>
//        /// <returns>A representação html da tabela.</returns>
//        public string GetHtmlFromTable(ITabularItem table, string id, int offset, int size)
//        {
//            return this.GetHtmlFromTable(table, id, offset, size, null);
//        }

//        /// <summary>
//        /// Obtém a representação da tabela em html.
//        /// </summary>
//        /// <param name="table">A tabela.</param>
//        /// <param name="id">O identificador a ser atribuído à tabela.</param>
//        /// <param name="offset">O deslocamento a partir da primeira linha do corpo da tabela.</param>
//        /// <param name="size">O número de linhas do corpo da tabela a serem imprimidas.</param>
//        /// <param name="cellFunct">Função que sobrecarrega a escrita da célula.</param>
//        /// <returns>A representação html da tabela.</returns>
//        public string GetHtmlFromTable(
//            ITabularItem table,
//            string id,
//            int offset,
//            int size,
//            Func<ITabularCell, XmlDocument, XmlNode> cellFunct)
//        {
//            if (table == null)
//            {
//                throw new ArgumentNullException("table");
//            }
//            else if (offset < 0)
//            {
//                throw new ArgumentOutOfRangeException("offset");
//            }
//            else if (size < 0)
//            {
//                throw new ArgumentOutOfRangeException("size");
//            }
//            else
//            {

//                XmlDocument document = new XmlDocument();
//                var tableElement = document.CreateElement("Table");
//                if (this.defaultTableClassName != null)
//                {
//                    tableElement.SetAttribute("class", this.defaultTableClassName);
//                }

//                if (!string.IsNullOrEmpty(id))
//                {
//                    tableElement.SetAttribute("id", id);
//                }
//                else
//                {
//                    tableElement.SetAttribute("id", Guid.NewGuid().ToString());
//                }

//                if (!string.IsNullOrEmpty(table.Name))
//                {
//                    tableElement.SetAttribute("name", table.Name);
//                }

//                var maximumNumberOfColumns = this.GetMaxNumberOfColumns(table);
//                if (innerTable.Headers.Count > 0)
//                {
//                    var tableHeaderElement = document.CreateElement("theader");
//                    var tableHeaderLine = document.CreateElement("tr");
//                    if (!string.IsNullOrEmpty(this.headerCss))
//                    {
//                        tableHeaderLine.SetAttribute("class", this.headerCss);
//                    }

//                    foreach (var header in innerTable.Headers)
//                    {
//                        var tableHeaderColumn = document.CreateElement("th");
//                        tableHeaderColumn.InnerText = header.Trim();
//                        tableHeaderLine.AppendChild(tableHeaderColumn);
//                    }

//                    this.AppendEmptyCells(document, tableHeaderLine, CellType.TH, maximumNumberOfColumns - innerTable.Headers.Count);
//                    tableHeaderElement.AppendChild(tableHeaderLine);
//                    tableElement.AppendChild(tableHeaderElement);
//                }

//                var isAlternatingLine = false;
//                var tableBodyElement = document.CreateElement("tbody");
//                for (int index = startLine; index < table.Count && index < endLine; ++index)
//                {
//                    var line = table[index];
//                    var tableLineElement = document.CreateElement("tr");
//                    if (!string.IsNullOrEmpty(this.alternateLineStyle))
//                    {
//                        if (isAlternatingLine)
//                        {
//                            tableLineElement.SetAttribute("class", this.alternateLineStyle);
//                        }
//                    }

//                    foreach (var column in line)
//                    {
//                        var mergedRegion = this.mergingRegionSet.GetMergingRegionForCell(column.RowNumber, column.ColumnNumber);
//                        if (mergedRegion != null)
//                        {
//                            var rowSpan = string.Empty;
//                            var columnSpan = string.Empty;
//                            if (column.RowNumber == mergedRegion.StartRow && column.ColumnNumber == mergedRegion.StartColumn)
//                            {
//                                if (mergedRegion.EndRow - mergedRegion.StartRow > 0)
//                                {
//                                    rowSpan = string.Format("{0}", mergedRegion.EndRow - mergedRegion.StartRow + 1);
//                                }

//                                if (mergedRegion.EndColumn - mergedRegion.StartColumn > 0)
//                                {
//                                    columnSpan = string.Format("{0}", mergedRegion.EndColumn - mergedRegion.StartColumn + 1);
//                                }

//                                if (!string.IsNullOrEmpty(rowSpan) || !string.IsNullOrEmpty(columnSpan))
//                                {
//                                    var columnElement = document.CreateElement("td");
//                                    if (!string.IsNullOrEmpty(column.Style.Name))
//                                    {
//                                        string cssCellStyle = string.Empty;
//                                        if (this.styleToCssMapper.TryGetValue(column.Style.Name, out cssCellStyle))
//                                        {
//                                            columnElement.SetAttribute("class", cssCellStyle);
//                                        }
//                                    }

//                                    if (!string.IsNullOrEmpty(rowSpan))
//                                    {
//                                        columnElement.SetAttribute("rowspan", rowSpan);
//                                    }

//                                    if (!string.IsNullOrEmpty(columnSpan))
//                                    {
//                                        columnElement.SetAttribute("colspan", columnSpan);
//                                    }

//                                    if (cellFunct == null)
//                                    {
//                                        var node = document.CreateTextNode(column.GetAsText());
//                                        columnElement.AppendChild(node);
//                                    }
//                                    else
//                                    {
//                                        try
//                                        {
//                                            var node = cellFunct(column, document);
//                                            columnElement.AppendChild(node);
//                                        }
//                                        catch (Exception)
//                                        {
//                                            columnElement.AppendChild(document.CreateTextNode("Error in cell."));
//                                        }
//                                    }

//                                    tableLineElement.AppendChild(columnElement);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            var columnElement = document.CreateElement("td");
//                            if (!string.IsNullOrEmpty(column.Style.Name))
//                            {
//                                string cssCellStyle = string.Empty;
//                                if (this.styleToCssMapper.TryGetValue(column.Style.Name, out cssCellStyle))
//                                {
//                                    columnElement.SetAttribute("class", cssCellStyle);
//                                }
//                            }

//                            if (cellFunct == null)
//                            {
//                                var node = document.CreateTextNode(column.GetAsText());
//                                columnElement.AppendChild(node);
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    var node = cellFunct(column, document);
//                                    columnElement.AppendChild(node);
//                                }
//                                catch (Exception)
//                                {
//                                    columnElement.AppendChild(document.CreateTextNode("Error in cell."));
//                                }
//                            }

//                            tableLineElement.AppendChild(columnElement);
//                        }
//                    }

//                    tableBodyElement.AppendChild(tableLineElement);
//                }

//                tableElement.AppendChild(tableBodyElement);
//                document.AppendChild(tableElement);
//                return document.InnerXml;
//            }
//        }

//        /// <summary>
//        /// Obtém o número máximo de colunas na tabela.
//        /// </summary>
//        /// <param name="table">A tabela.</param>
//        /// <param name="start">O número da linha inicial da tabela.</param>
//        /// <param name="end">O número da linha final da tabela.</param>
//        /// <returns>O número máximo de colunas.</returns>
//        private int GetMaxNumberOfColumns(ITabularItem table, int start, int end)
//        {
//            var result = 0;
//            var length = table.Count;
//            if (start < length)
//            {
//                var i = start;
//                result = table[i].Count;
//                ++i;
//                for (; i < length; ++i)
//                {
//                    var current = table[i].Count;
//                    if (current > result)
//                    {
//                        result = current;
//                    }
//                }
//            }

//            return result;
//        }

//        /// <summary>
//        /// Adenda célculas vazias ao documento.
//        /// </summary>
//        /// <param name="lineElement">A linha à qual serão adicionadas células vazias.</param>
//        /// <param name="cellType">O tipo da célula.</param>
//        /// <param name="count">O número de elementos a inserir.</param>
//        private void AppendEmptyCells(
//            XmlDocument document,
//            XmlElement lineElement,
//            CellType cellType, 
//            int count)
//        {
//            for (int i = 0; i < count; ++i)
//            {
//                var tableCellElement = document.CreateElement(cellType == CellType.TH ? "th" : "td");
//                lineElement.AppendChild(tableCellElement);
//            }
//        }

//        /// <summary>
//        /// Define o tipo de célula a ser construído.
//        /// </summary>
//        private enum CellType
//        {
//            TD,
//            TH
//        }
//    }
//}
