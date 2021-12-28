// -----------------------------------------------------------------------
// <copyright file="FileTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    /// <summary>
    /// Testa todas as funções associadas a ficheiros.
    /// </summary>
    [TestClass]
    public class CsvFileTest
    {
        /// <summary>
        /// Permite testar a leitura de um ficheiro csv para  uma lista de listas de valores
        /// numéricos.
        /// </summary>
        [Description("Tests the csv parser while reading a list of lists of doubles.")]
        [TestMethod]
        public void CsvFileParser_DoublesToListOfListsTest()
        {
            var csvContentBuilder = new StringBuilder();
            csvContentBuilder.AppendLine("1;2;3");
            csvContentBuilder.AppendLine("4;5;6");
            csvContentBuilder.AppendLine("7;8;0.9");
            csvContentBuilder.AppendLine("0.7;0.8;9");

            var csvString = csvContentBuilder.ToString();
            var csvStringReader = new StringReader(csvString);
            var stringSymbolReader = new StringSymbolReader(csvStringReader, true, false);

            // O leitor que será passado para todas as células do CSV
            var doubleParser = new DoubleParser<string>();

            // Cria um leitor para o ficheiro.
            var csvParser = new CsvFileParser<List<List<double>>, double, string, string>(
                "new_line",
                "semi_colon",
                (i, j) => doubleParser);
            csvParser.AddIgnoreType("carriage_return");

            // Responsável pela adição de elementos à lista de listas.
            var adder = new ListTypeAdder<double>();

            // Efectua a leitura do csv.
            var container = new List<List<double>>();
            csvParser.Parse(stringSymbolReader, container, adder);

            // Realiza as verificações
            var expectedValues = new double[,] { 
                                                    { 1, 2, 3 }, 
                                                    { 4, 5, 6 }, 
                                                    { 7, 8, 0.9 }, 
                                                    { 0.7, 0.8, 9 } 
                                               };
            Assert.AreEqual(4, container.Count);
            for (int i = 0; i < container.Count; ++i)
            {
                var currentList = container[i];
                Assert.IsNotNull(currentList);
                Assert.AreEqual(3, currentList.Count);
                for (int j = 0; j < currentList.Count; ++j)
                {
                    var currentItem = currentList[j];
                    Assert.AreEqual(expectedValues[i, j], currentItem);
                }
            }
        }

        /// <summary>
        /// Testa o leitor de dados a partir do csv quando são passados números
        /// de precisão dupla.
        /// </summary>
        [Description("Tests the data reader for a table with doubles.")]
        [TestMethod]
        public void CsvDataReader_DoublesDataReaderTest()
        {
            var csvContentBuilder = new StringBuilder();
            csvContentBuilder.AppendLine("1;2;3");
            csvContentBuilder.AppendLine("4;5;6");
            csvContentBuilder.AppendLine("7;8;0.9");
            csvContentBuilder.AppendLine("0.7;0.8;9");

            var csvString = csvContentBuilder.ToString();
            var csvStringReader = new StringReader(csvString);
            var stringSymbolReader = new StringSymbolReader(csvStringReader, true, false);

            // O leitor que será passado para todas as células do CSV
            IParse<double, string, string> doubleParser = new DoubleParser<string>();
            var columns = new Tuple<IParse<double, string, string>, string>[3];
            columns[0] = Tuple.Create(doubleParser, "A");
            columns[1] = Tuple.Create(doubleParser, "B");
            columns[2] = Tuple.Create(doubleParser, "C");
            var target = new CsvDataReader<double, string, string>(
                stringSymbolReader,
                columns,
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                Utils.GetStringSymbolType(EStringSymbolReaderType.SEMI_COLON),
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));

            // Primeira linha
            var actual = target.Read();
            Assert.IsTrue(actual);
            var line = new double[] { 1, 2, 3 };
            for (int i = 0; i < line.Length; ++i)
            {
                var value = target.GetDouble(i);
                Assert.AreEqual(line[i], value);
            }

            // Segunda linha
            actual = target.Read();
            Assert.IsTrue(actual);
            line = new double[] { 4, 5, 6 };
            for (int i = 0; i < line.Length; ++i)
            {
                var value = target.GetDouble(i);
                Assert.AreEqual(line[i], value);
            }

            // Terceira linha
            actual = target.Read();
            Assert.IsTrue(actual);
            line = new double[] { 7, 8, 0.9 };
            for (int i = 0; i < line.Length; ++i)
            {
                var value = target.GetDouble(i);
                Assert.AreEqual(line[i], value);
            }

            // Quarta linha
            actual = target.Read();
            Assert.IsTrue(actual);
            line = new double[] { 0.7, 0.8, 9 };
            for (int i = 0; i < line.Length; ++i)
            {
                var value = target.GetDouble(i);
                Assert.AreEqual(line[i], value);
            }

            // Depois da última
            actual = target.Read();
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Testa o leitor de dados com delimitadores definidos.
        /// </summary>
        [Description("Tests the csv data reader with delimiters defined.")]
        [TestMethod]
        public void CsvDataReader_WithDelimitersTest()
        {
            var csvContentBuilder = new StringBuilder();
            csvContentBuilder.AppendLine("(Tito, Quico),1");
            csvContentBuilder.AppendLine("(Xico, Rico),2");
            csvContentBuilder.AppendLine("(Pico-pico, Sardanico\nNico),3");

            var csvString = csvContentBuilder.ToString();
            var csvStringReader = new StringReader(csvString);
            var stringSymbolReader = new StringSymbolReader(csvStringReader, true, false);

            var columnsParsers = new Tuple<IParse<object, string, string>, string>[2];
            IParse<object, string, string> stringParser = new StringConcatParser();
            var integerParser = new IntegerParser<string>();
            IParse<object, string, string> numberParser = new ObjectValueParser<int, string, string>(
                integerParser);
            columnsParsers[0] = Tuple.Create(stringParser, "Names");
            columnsParsers[1] = Tuple.Create(numberParser, "Scores");

            var target = new CsvDataReader<object, string, string>(
                stringSymbolReader,
                columnsParsers,
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                Utils.GetStringSymbolType(EStringSymbolReaderType.COMMA),
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            target.MapDelimiters(
                Utils.GetStringSymbolType(EStringSymbolReaderType.LEFT_PARENTHESIS),
                Utils.GetStringSymbolType(EStringSymbolReaderType.RIGHT_PARENTHESIS));
            target.AddIgnoreType(Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN));

            var expected = new List<List<object>>(){ 
                new List<object>(){"(Tito, Quico)", 1},
                new List<object>(){"(Xico, Rico)",2},
                new List<object>(){"(Pico-pico, Sardanico\nNico)",3}
            };

            var actual = new List<List<object>>();
            while (target.Read())
            {
                var current = new List<object>();
                current.Add(target.GetString(0));
                current.Add(target.GetInt32(1));
                actual.Add(current);
            }

            var count = expected.Count;
            Assert.AreEqual(count, actual.Count);
            for (int i = 0; i < count; ++i)
            {
                var currentExpected = expected[i];
                var currentActual = actual[i];
                var expectedCount = currentExpected.Count;
                Assert.AreEqual(expectedCount, currentActual.Count);
                for (int j = 0; j < expectedCount; ++j)
                {
                    Assert.AreEqual(currentExpected[j], currentActual[j]);
                }
            }
        }
    }

    /// <summary>
    /// Testa a classe de html.
    /// </summary>
    [TestClass]
    public class HtmlFileTest
    {
        /// <summary>
        /// Testa o objecto que permite escrever um <see cref="IDataReader"/> para uma
        /// tabela de html.
        /// </summary>
        [Description("Tests the html writer from a datareader.")]
        [TestMethod]
        public void HtmlDataReaderWriter_GetHmtlFromDataReaderTest()
        {
            // Leitura do csv
            var csvContentBuilder = new StringBuilder();
            csvContentBuilder.AppendLine("1;2;3");
            csvContentBuilder.AppendLine("4;5;6");
            csvContentBuilder.AppendLine("7;8;0.9");
            csvContentBuilder.AppendLine("0.7;0.8;9");

            var csvString = csvContentBuilder.ToString();
            var csvStringReader = new StringReader(csvString);
            var stringSymbolReader = new StringSymbolReader(csvStringReader, true, false);

            // O leitor que será passado para todas as células do CSV
            IParse<double, string, string> doubleParser = new DoubleParser<string>();
            var columns = new Tuple<IParse<double, string, string>, string>[3];
            columns[0] = Tuple.Create(doubleParser, "A");
            columns[1] = Tuple.Create(doubleParser, "B");
            columns[2] = Tuple.Create(doubleParser, "C");
            var reader = new CsvDataReader<double, string, string>(
                stringSymbolReader,
                columns,
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                Utils.GetStringSymbolType(EStringSymbolReaderType.SEMI_COLON),
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));

            // Escrita do html
            var target = new HtmlDataReaderWriter();
            target.TableAttributes = s => s.SetAttribute("Id", "table");
            target.TableHeaderAttributes = s => s.SetAttribute("Id", "header");
            target.TableBodyAttributes = s => s.SetAttribute("Id", "body");
            target.HeaderRowAttributes = s => s.SetAttribute("Id", "header_row");
            target.HeaderCellElement = (i, s, e) =>
            {
                e.SetAttribute("Id", string.Format("col_{0}", i));
                e.InnerText = string.Format("{0}_{1}", s, i);
            };

            target.BodyRowAttributes = (i, s) => s.SetAttribute("Id", string.Format("row_{0}", i));
            target.BodyCellElement = (i, j, o, e) =>
            {
                e.SetAttribute("Id", string.Format("cell_{0}_{1}", i, j));
                e.InnerText = ((double)o).ToString("0.0");
            };

            var actualHtml = target.GetHtmlFromDataReader(reader);

            // Verifica a tabela de html
            var existingColumns = new string[] { "A", "B", "C" };
            var values = new double[][]{
                new double[]{1,2,3},
                new double[]{4,5,6},
                new double[]{7,8,0.9},
                new double[]{0.7,0.8,9}};

            var stringReader = new StringReader(actualHtml);
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stringReader);
            Assert.AreEqual("table", xmlDocument.DocumentElement.Name.ToLower());
            var childNodes = xmlDocument.DocumentElement.GetElementsByTagName("thead");
            Assert.AreEqual(1, childNodes.Count);
            var node = childNodes[0];
            Assert.AreEqual(1, node.Attributes.Count);
            Assert.AreEqual("header", node.Attributes["id"].Value);
            Assert.AreEqual(1, node.ChildNodes.Count);
            node = node.ChildNodes[0];
            Assert.AreEqual("tr", node.Name.ToLower());
            Assert.AreEqual(1, node.Attributes.Count);
            Assert.AreEqual("header_row", node.Attributes["id"].Value);
            Assert.AreEqual(3, node.ChildNodes.Count);
            for (int i = 0; i < childNodes.Count; ++i)
            {
                var innerNode = node.ChildNodes[i];
                Assert.AreEqual("th", innerNode.Name.ToLower());
                Assert.AreEqual(1, innerNode.Attributes.Count);
                Assert.AreEqual(
                    string.Format("col_{0}", i),
                    innerNode.Attributes["id"].Value);
                Assert.AreEqual(
                    string.Format("{0}_{1}", existingColumns[i], i),
                    innerNode.InnerXml);
            }

            childNodes = xmlDocument.GetElementsByTagName("tbody");
            Assert.AreEqual(1, childNodes.Count);
            node = childNodes[0];
            Assert.AreEqual(1, node.Attributes.Count);
            Assert.AreEqual("body", node.Attributes["id"].Value);
            childNodes = node.ChildNodes;
            Assert.AreEqual(values.Length, childNodes.Count);
            for (int i = 0; i < values.Length; ++i)
            {
                node = childNodes[i];
                Assert.AreEqual("tr", node.Name.ToLower());
                Assert.AreEqual(1, node.Attributes.Count);
                Assert.AreEqual(
                    string.Format("row_{0}", i),
                    node.Attributes["id"].Value);
                var expectedRow = values[i];
                Assert.AreEqual(expectedRow.Length, node.ChildNodes.Count);
                for (int j = 0; j < expectedRow.Length; ++j)
                {
                    var innerNode = node.ChildNodes[j];
                    Assert.AreEqual("td", innerNode.Name.ToLower());
                    Assert.AreEqual(1, innerNode.Attributes.Count);
                    Assert.AreEqual(
                        string.Format("cell_{0}_{1}", i, j),
                        innerNode.Attributes["id"].Value);
                    Assert.AreEqual(
                        values[i][j].ToString("0.0"),
                        innerNode.InnerXml);
                }
            }
        }

        /// <summary>
        /// Testa o objecto que permite escrever um item tablar para uma tabela
        /// de html.
        /// </summary>
        [Description("Tests the html writer from a tabular item.")]
        [TestMethod]
        public void HtmlTableWriter_GetHtmlFromTableItemTest()
        {
            var tabularItem = new TabularListsItem();
            tabularItem.Add(new[] { "A", "B", "C", "D", "E", "F" });
            tabularItem.Add(new[] { "A1", "B1", string.Empty, string.Empty, "E1" });
            tabularItem.Add(new[] { "a" });
            tabularItem.Add(new[] { "b", "c", "d" });
            tabularItem.Add(new object[0]);
            tabularItem.Add(new[] { "e", "f" });
            tabularItem.Add(new[] { string.Empty, string.Empty, string.Empty, string.Empty });
            var header = new WindowReadonlyTabularItem<TabularListsItem, ITabularRow, ITabularCell>(
                tabularItem,
                0,
                2,
                0,
                6);
            var body = new WindowReadonlyTabularItem<TabularListsItem, ITabularRow, ITabularCell>(
                tabularItem,
                2,
                5,
                0,
                6);
            var target = new HtmlTableWriter();

            // Fusão das células do cabeçalho
            target.HeaderMergingRegions.Add(
                new MergingRegion<int>(2, 0, 3, 1));
            target.HeaderMergingRegions.Add(
                new MergingRegion<int>(4, 1, 8, 1));

            // Fusão das células do corpo
            target.BodyMergingRegions.Add(
                new MergingRegion<int>(0, 0, 5, 0));
            target.BodyMergingRegions.Add(
                new MergingRegion<int>(0, 1, 1, 2));
            target.BodyMergingRegions.Add(
                new MergingRegion<int>(2, 1, 3, 2));
            target.BodyMergingRegions.Add(
                new MergingRegion<int>(4, 1, 5, 2));
            target.BodyMergingRegions.Add(
                new MergingRegion<int>(0, 3, 3, 5));
            target.BodyMergingRegions.Add(
                new MergingRegion<int>(4, 3, 6, 6));

            var actualHtml = target.GetHtmlFromTableItem<IReadonlyTabularRow, IReadonlyTabularCell>(
                header,
                body);

            var headerValues = new[]{
                new[]{"A","B","C","E","F"},
                new[]{"A1","B1","E1"}
            };

            var headerAttributes = new Dictionary<string, string>[headerValues.Length][];
            for (int i = 0; i < headerValues.Length; ++i)
            {
                headerAttributes[i] = new Dictionary<string, string>[headerValues[i].Length];
                for (int j = 0; j < headerValues[i].Length; ++j)
                {
                    headerAttributes[i][j] = new Dictionary<string, string>();
                }
            }

            headerAttributes[0][2].Add("rowspan", "2");
            headerAttributes[0][2].Add("colspan", "2");
            headerAttributes[1][2].Add("colspan", "2");

            var bodyValues = new[]{
                new[]{"a"},
                new[]{"b","d",string.Empty},
                new string[]{},
                new[]{"e",string.Empty},
                new string[]{}
            };

            var bodyAttributes = new Dictionary<string, string>[bodyValues.Length][];
            for (int i = 0; i < bodyValues.Length; ++i)
            {
                bodyAttributes[i] = new Dictionary<string, string>[bodyValues[i].Length];
                for (int j = 0; j < bodyValues[i].Length; ++j)
                {
                    bodyAttributes[i][j] = new Dictionary<string, string>();
                }
            }

            bodyAttributes[0][0].Add("colspan", "6");
            bodyAttributes[1][0].Add("rowspan", "2");
            bodyAttributes[1][0].Add("colspan", "2");
            bodyAttributes[1][1].Add("rowspan", "2");
            bodyAttributes[1][1].Add("colspan", "2");
            bodyAttributes[1][2].Add("rowspan", "2");
            bodyAttributes[1][2].Add("colspan", "2");
            bodyAttributes[3][0].Add("rowspan", "2");
            bodyAttributes[3][0].Add("colspan", "4");
            bodyAttributes[3][1].Add("rowspan", "2");
            bodyAttributes[3][1].Add("colspan", "2");

            var stringReader = new StringReader(actualHtml);
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stringReader);

            Assert.AreEqual("table", xmlDocument.DocumentElement.Name.ToLower());

            // Teste ao cabeçalho
            var childNodes = xmlDocument.DocumentElement.GetElementsByTagName("thead");
            Assert.AreEqual(1, childNodes.Count);
            var node = childNodes[0];
            Assert.AreEqual(headerValues.Length, node.ChildNodes.Count);
            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                var row = node.ChildNodes[i];
                Assert.AreEqual("tr", row.Name);
                Assert.AreEqual(headerValues[i].Length, row.ChildNodes.Count);
                for (int j = 0; j < headerValues[i].Length; ++j)
                {
                    var cell = row.ChildNodes[j];
                    Assert.AreEqual("th", cell.Name);
                    Assert.AreEqual(headerValues[i][j], cell.InnerText);

                    Assert.AreEqual(headerAttributes[i][j].Count, cell.Attributes.Count);
                    foreach (XmlAttribute att in cell.Attributes)
                    {
                        if (headerAttributes[i][j].ContainsKey(att.Name))
                        {
                            Assert.AreEqual(
                                headerAttributes[i][j][att.Name],
                                att.Value);
                        }
                        else
                        {
                            Assert.Fail(string.Format(
                                "Found no attribute with the specified name: {0}",
                                att.Name));
                        }
                    }
                }
            }

            // Teste ao corpo
            childNodes = xmlDocument.DocumentElement.GetElementsByTagName("tbody");
            Assert.AreEqual(1, childNodes.Count);
            node = childNodes[0];
            Assert.AreEqual(bodyValues.Length, node.ChildNodes.Count);
            for (int i = 0; i < node.ChildNodes.Count; ++i)
            {
                var row = node.ChildNodes[i];
                Assert.AreEqual("tr", row.Name);
                Assert.AreEqual(bodyValues[i].Length, row.ChildNodes.Count);
                for (int j = 0; j < bodyValues[i].Length; ++j)
                {
                    var cell = row.ChildNodes[j];
                    Assert.AreEqual("td", cell.Name);
                    Assert.AreEqual(bodyValues[i][j], cell.InnerText);

                    Assert.AreEqual(bodyAttributes[i][j].Count, cell.Attributes.Count);
                    foreach (XmlAttribute att in cell.Attributes)
                    {
                        if (bodyAttributes[i][j].ContainsKey(att.Name))
                        {
                            Assert.AreEqual(
                                bodyAttributes[i][j][att.Name],
                                att.Value);
                        }
                        else
                        {
                            Assert.Fail(string.Format(
                                "Found no attribute with the specified name: {0}",
                                att.Name));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Testa a escrita para uma tablea de html de um enumerável de objectos.
        /// </summary>
        [Description("Tests the html writer for an object enumerable.")]
        [TestMethod]
        public void HtmlObjectWriter_GetHtmlTableFromEnumerableTest()
        {
            var persons = new List<TestPerson>();
            var boss = new TestPerson()
            {
                FirstName = "Chefe",
                LastName = "Machado",
                Age = 50,
                BirthDate = new DateTime(1966, 10, 5),
                Gender = 0,
                IsEmployee = true,
                MarritalStatus = 1
            };

            persons.Add(boss);
            persons.Add(new TestPerson()
            {
                FirstName = "Veterano",
                LastName = "Felisberto",
                Age = 30,
                BirthDate = new DateTime(1986, 11, 14),
                Gender = 0,
                IsEmployee = true,
                MarritalStatus = 0,
                HierarquicalSuperior = boss
            });

            persons.Add(new TestPerson()
            {
                FirstName = "Empadão",
                LastName = "Antunes",
                Age = 46,
                BirthDate = new DateTime(1970, 9, 4),
                Gender = 0,
                IsEmployee = true,
                MarritalStatus = 1,
                HierarquicalSuperior = boss
            });

            persons.Add(new TestPerson()
            {
                FirstName = "Camélia",
                LastName = "Florista",
                Age = 35,
                BirthDate = new DateTime(1981, 12, 13),
                Gender = 1,
                IsEmployee = false,
                MarritalStatus = 0
            });

            var target = new HtmlObjectWriter<TestPerson>(true);
            target.TableAttributes = a => { a.SetAttribute("class", "table"); };
            target.TableHeaderAttributes = a => { a.SetAttribute("class", "tableHeader"); };
            target.HeaderRowAttributesSetter = a => { a.SetAttribute("class", "header_row"); };
            target.HeaderCellElement = (i, s, e) =>
            {
                e.InnerText = s;
                e.SetAttribute("class", string.Format("cell_{0}", i));
            };

            target.TableBodyAttributes = a => { a.SetAttribute("class", "tableBody"); };
            target.BodyRowAttributes = (i, a) =>
            {
                if ((i & 1) == 0)
                {
                    a.SetAttribute("class", "even");
                }
                else
                {
                    a.SetAttribute("class", "odd");
                }
            };

            // A primeira coluna irá conter
            target.AddColumn(Tuple.Create(
                string.Empty,
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    e.InnerXml = string.Format("<img src=\"save_delete_{0}.jpg\"/>", j);
                })));

            target.AddColumn(Tuple.Create(
                "First Name",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    e.InnerText = p.FirstName;
                })));

            target.AddColumn(Tuple.Create(
                "Last Name",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    e.InnerText = p.LastName;
                })));

            target.AddColumn(Tuple.Create(
                "Age",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    e.InnerText = p.Age.ToString();
                })));

            target.AddColumn(Tuple.Create(
                "Birth Date",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    e.InnerText = p.BirthDate.ToString("dd/mm/yyyy");
                })));

            target.AddColumn(Tuple.Create(
                "Gender",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    e.InnerText = p.Gender.ToString();
                })));

            target.AddColumn(Tuple.Create(
                "Employee",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    if (p.IsEmployee)
                    {
                        e.InnerXml = "<input type=\"checkbox\" value=\"Bike\" checked=\"checked\" />";
                    }
                    else
                    {
                        e.InnerXml = "<input type=\"checkbox\" value=\"Bike\"/>";
                    }
                })));

            target.AddColumn(Tuple.Create(
                "Marrital Status",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    e.InnerText = p.MarritalStatus.ToString();
                })));

            target.AddColumn(Tuple.Create(
                "Hierarquical Superior",
                new Action<int, int, TestPerson, ElementSetter>((i, j, p, e) =>
                {
                    e.SetAttribute("class", string.Format(
                        "row_{0} column_{1}",
                        i,
                        j));

                    if (p.HierarquicalSuperior == null)
                    {
                        e.InnerText = "No superior";
                    }
                    else
                    {
                        e.InnerText = p.HierarquicalSuperior.ToString();
                    }
                })));

            var actualHtml = target.GetHtmlTableFromEnumerable(persons);
            var stringReader = new StringReader(actualHtml);
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stringReader);

            var table = xmlDocument.DocumentElement;
            Assert.AreEqual("table", table.Name.ToLower());
            var classAtt = table.GetAttribute("class");
            Assert.AreEqual("table", classAtt);
            Assert.AreEqual(2, table.ChildNodes.Count);

            // Teste ao cabeçalho
            var tableHeader = (XmlElement)table.ChildNodes[0];
            Assert.AreEqual("thead", tableHeader.Name.ToLower());
            classAtt = tableHeader.GetAttribute("class");
            Assert.AreEqual(classAtt, "tableHeader");

            Assert.AreEqual(1, tableHeader.ChildNodes.Count);
            var row = (XmlElement)tableHeader.ChildNodes[0];
            Assert.AreEqual("tr", row.Name);
            classAtt = row.GetAttribute("class");
            Assert.AreEqual("header_row", classAtt);

            Assert.AreEqual(target.Columns.Count, row.ChildNodes.Count);

            for (int i = 0; i < row.ChildNodes.Count; ++i)
            {
                var cell = (XmlElement)row.ChildNodes[i];
                Assert.AreEqual("th", cell.Name);
                Assert.AreEqual(target.Columns[i].Item1, cell.InnerText);
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("cell_{0}", i), classAtt);
            }

            var tableBody = (XmlElement)table.ChildNodes[1];
            Assert.AreEqual("tbody", tableBody.Name.ToLower());
            classAtt = tableBody.GetAttribute("class");
            Assert.AreEqual("tableBody", classAtt);
            Assert.AreEqual(persons.Count, tableBody.ChildNodes.Count);

            for (int i = 0; i < persons.Count; ++i)
            {
                var person = persons[i];
                row = (XmlElement)tableBody.ChildNodes[i];
                Assert.AreEqual("tr", row.Name.ToLower());
                classAtt = row.GetAttribute("class");
                if ((i & 1) == 0)
                {
                    Assert.AreEqual("even", classAtt);
                }
                else
                {
                    Assert.AreEqual("odd", classAtt);
                }

                Assert.AreEqual(target.Columns.Count, row.ChildNodes.Count);
                var j = 0;
                var cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.IsTrue(cell.InnerXml.Contains(string.Format("save_delete_{0}.jpg", j)));
                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.AreEqual(person.FirstName, cell.InnerText);
                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.AreEqual(person.LastName, cell.InnerText);
                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.AreEqual(person.Age.ToString(), cell.InnerText);
                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.AreEqual(person.BirthDate.ToString("dd/mm/yyyy"), cell.InnerText);
                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.AreEqual(person.Gender.ToString(), cell.InnerText);
                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.IsTrue(cell.InnerXml.Contains("input"));
                if (person.IsEmployee)
                {
                    Assert.IsTrue(cell.InnerXml.Contains("checked"));
                }
                else
                {
                    Assert.IsFalse(cell.InnerText.Contains("checked"));
                }

                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                Assert.AreEqual(person.MarritalStatus.ToString(), cell.InnerText);
                ++j;

                cell = (XmlElement)row.ChildNodes[j];
                classAtt = cell.GetAttribute("class");
                Assert.AreEqual(string.Format("row_{0} column_{1}", i, j), classAtt);
                if (person.HierarquicalSuperior == null)
                {
                    Assert.AreEqual("No superior", cell.InnerText);
                }
                else
                {
                    Assert.AreEqual(person.HierarquicalSuperior.ToString(), cell.InnerText);
                }
            }
        }
    }

    /// <summary>
    /// Testa a classe para leitura de uma lista de coordenadas do tipo MatrixMarket.
    /// </summary>
    [TestClass]
    public class MatrixReaderTest
    {
        /// <summary>
        /// Testa a leitura do leitor de triplos de coordenadas bidimensionais e valor.
        /// </summary>
        [Description("Tests the coordinates list reader.")]
        [TestMethod]
        public void CoordinatesListReader_MoveNextTest()
        {
            // Testa a leitura de inteiros mapeados por coordenadas inteiras.
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine("%Isto é um comentário.");
            textBuilder.AppendLine(" /*Comentário 1*/ 0  /*Comentário 2*/  0  /*Comentário 3  */0  /*Comentário.*/  ");
            textBuilder.AppendLine("1 1 3");
            textBuilder.AppendLine("1230 40 500");
            var expected = new List<Tuple<int, int, int>>(){
                Tuple.Create(0,0,0),
                Tuple.Create(1,1,3),
                Tuple.Create(1230, 40, 500)
            };

            var stringReader = new StringReader(textBuilder.ToString());
            var reader = new StringSymbolReader(stringReader, true, false);
            var integerParser = new IntegerParser<string>();
            var target = new CoordinatesListReader<int, int, int, string, string>(
                integerParser,
                integerParser,
                integerParser,
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE),
                reader);
            target.RegisterCommentDelimiter(
                Utils.GetStringSymbolType(EStringSymbolReaderType.MOD),
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE));
            target.RegisterCommentDelimiter(
            Utils.GetStringSymbolType(EStringSymbolReaderType.START_COMMENT),
                Utils.GetStringSymbolType(EStringSymbolReaderType.END_COMMENT));
            target.AddBlanck(Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN));

            var index = 0;
            while (target.MoveNext())
            {
                var expectedItem = expected[index++];
                var actual = target.Current;
                Assert.AreEqual(expectedItem.Item1, actual.Line);
                Assert.AreEqual(expectedItem.Item2, actual.Column);
                Assert.AreEqual(expectedItem.Item3, actual.Element);
            }

            // Testa a leitura de números de precisão dupla
            // e cujas coordenadas são dadas por expressões algébricas sobre inteiros
            textBuilder.Clear();
            textBuilder.AppendLine("%Isto é um comentário.");
            textBuilder.AppendLine(" /*Comentário 1*/ (1-1)  /*Comentário 2*/  ( ( 2 - 3 )*(3-4+1))  /*Comentário 3  */.1  /*Comentário.*/  ");
            textBuilder.AppendLine("1 1 .03");
            textBuilder.AppendLine("((5-2+1)^2) /*Comentário*/ ((3-1)^4) 2.05");
            var secondExpected = new List<Tuple<int, int, double>>()
            {
                Tuple.Create(0, 0, 0.1),
                Tuple.Create(1,1,0.03),
                Tuple.Create(16,16,2.05)
            };

            stringReader = new StringReader(textBuilder.ToString());
            reader = new StringSymbolReader(stringReader, false, false);
            var integerExpressionParser = new IntegerExpressionParser();
            var doubleParser = new DoubleParser<string>(
                System.Globalization.NumberStyles.Number,
                System.Globalization.NumberFormatInfo.InvariantInfo);
            var secondTarget = new CoordinatesListReader<int, int, double, string, string>(
                integerExpressionParser,
                integerExpressionParser,
                doubleParser,
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE),
                reader);
            secondTarget.RegisterDelimiter(
                Utils.GetStringSymbolType(EStringSymbolReaderType.LEFT_PARENTHESIS),
                Utils.GetStringSymbolType(EStringSymbolReaderType.RIGHT_PARENTHESIS));
            secondTarget.RegisterCommentDelimiter(
                Utils.GetStringSymbolType(EStringSymbolReaderType.MOD),
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE));
            secondTarget.RegisterCommentDelimiter(
                Utils.GetStringSymbolType(EStringSymbolReaderType.START_COMMENT),
                Utils.GetStringSymbolType(EStringSymbolReaderType.END_COMMENT));
            secondTarget.AddBlanck(Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN));

            index = 0;
            while (target.MoveNext())
            {
                var expectedItem = expected[index++];
                var actual = target.Current;
                Assert.AreEqual(expectedItem.Item1, actual.Line);
                Assert.AreEqual(expectedItem.Item2, actual.Column);
                Assert.AreEqual(expectedItem.Item3, actual.Element);
            }

            // Teste à leitura de uma matriz com erros
            textBuilder.Clear();
            textBuilder.AppendLine("Isto é um comentário.");
            textBuilder.AppendLine("(1) (2) (3.0) 1)");
            textBuilder.AppendLine("(1++1) (2-3) 2.0");
            textBuilder.AppendLine("(1) (2)");
            textBuilder.AppendLine(" /*Comentário 1*/ (1-1)  /*Comentário 2*/  ( ( 2 - 3 )*(3-4+1))  /*Comentário 3  */.1  /*Comentário.*/  ");
            textBuilder.AppendLine("1 1 .03");
            textBuilder.AppendLine("((5-2+1)^2) /*Comentário*/ ((3-1)^4) 2.05");
            secondExpected = new List<Tuple<int, int, double>>()
            {
                Tuple.Create(0,0,0.0),
                Tuple.Create(0,0,0.0),
                Tuple.Create(0,0,0.0),
                Tuple.Create(0,0,0.0),
                Tuple.Create(0, 0, 0.1),
                Tuple.Create(1,1,0.03),
                Tuple.Create(16,16,2.05)
            };

            var errors = new[] { true, true, true, true, false, false, false };

            stringReader = new StringReader(textBuilder.ToString());
            reader = new StringSymbolReader(stringReader, false, false);
            secondTarget = new CoordinatesListReader<int, int, double, string, string>(
                integerExpressionParser,
                integerExpressionParser,
                doubleParser,
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE),
                reader);
            secondTarget.RegisterDelimiter(
                Utils.GetStringSymbolType(EStringSymbolReaderType.LEFT_PARENTHESIS),
                Utils.GetStringSymbolType(EStringSymbolReaderType.RIGHT_PARENTHESIS));
            secondTarget.RegisterCommentDelimiter(
                Utils.GetStringSymbolType(EStringSymbolReaderType.MOD),
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE));
            secondTarget.RegisterCommentDelimiter(
                Utils.GetStringSymbolType(EStringSymbolReaderType.START_COMMENT),
                Utils.GetStringSymbolType(EStringSymbolReaderType.END_COMMENT));
            secondTarget.AddBlanck(Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN));
            while (secondTarget.MoveNext())
            {
                var currentError = errors[index];
                var actual = secondTarget.Current;
                if (currentError)
                {
                    Assert.IsTrue(actual.HasError);
                }
                else
                {
                    var expectedItem = secondExpected[index];
                    Assert.AreEqual(expectedItem.Item1, actual.Line);
                    Assert.AreEqual(expectedItem.Item2, actual.Column);
                    Assert.AreEqual(expectedItem.Item3, actual.Element);
                }

                ++index;
            }

            Assert.AreEqual(secondExpected.Count, index);
        }
    }

    /// <summary>
    /// Testa a leitura de bits.
    /// </summary>
    [TestClass]
    public class BitReaderTest
    {
        /// <summary>
        /// Testa a função de leitura de bits.
        /// </summary>
        [Description("Testa o leitor de bits.")]
        [TestMethod]
        public void BitReader_ReadBitTest()
        {
            var buffer = new byte[]{
                0xFD,
                0xA1,
                0xC6,
                0xEB,
            };

            var expectedBits = new int[]{
                1, 0, 1, 1, 1, 1, 1, 1,
                1, 0, 0, 0, 0, 1, 0, 1,
                0, 1, 1, 0, 0, 0, 1, 1,
                1, 1, 0, 1, 0, 1, 1, 1
            };

            var stream = new MemoryStream(buffer);
            var target = new BitReader(stream, 2);
            var length = expectedBits.Length;
            for (var i = 0; i < length; ++i)
            {
                var readed = target.ReadBit();
                Assert.AreEqual(expectedBits[i], readed);
            }
        }

        /// <summary>
        /// Testa o leitor de bits para um vector.
        /// </summary>
        [Description("Testa o leitor de vários bits")]
        [TestMethod]
        public void BitReader_BufferReadBitsTest()
        {
            // Teste geral
            var buffer = new byte[]{
                0xFD, // 253
                0xA1, // 161
                0xC6, // 198
                0xEB, // 235
                0xA7, // 167
                0xB9, // 185
                0xC1, // 193
                0x05  // 5
            };

            //var expectedBits = new int[]{
            //    1, 0, 1, 1, 1, 1, 1, 1,
            //    1, 0, 0, 0, 0, 1, 0, 1,
            //    0, 1, 1, 0, 0, 0, 1, 1,
            //    1, 1, 0, 1, 0, 1, 1, 1,
            //    1, 1, 1, 0, 0, 1, 0, 1,
            //    1, 0, 0, 1, 1, 1, 0, 1,
            //    1, 0, 0, 0, 0, 0, 1, 1,
            //    1, 0, 1, 0, 0, 0, 0, 0
            //};

            var expected = new[]{
                new byte[]{0x1}, new byte[]{0x2}, new byte[]{0x7},
                new byte[]{0x7}, new byte[]{0x8},
                new byte[]{ 0xD}, new byte[]{0x5E},
                new byte[]{0x7E}, new byte[]{0x9A, 0x1},
                new byte[]{0x0D, 0x2}, new byte[]{0x0B, 0x0} 
            };

            var stream = new MemoryStream(buffer);
            var target = new BitReader(stream, 2);
            var length = expected.Length - 1;
            var acc = 0;
            var twister = new MTRand();
            for (var i = 0; i < length; ++i)
            {
                acc += i + 1;
                var curr = expected[i];
                var len = curr.Length;
                var readBuffer = new byte[len];
                for (var j = 0; j < len; ++j)
                {
                    readBuffer[j] = (byte)twister.RandInt(256);
                }

                var readed = target.ReadBits(readBuffer, 0, i + 1);
                Assert.AreEqual(i + 1, readed);
                for (var j = 0; j < len; ++j)
                {
                    Assert.AreEqual(curr[j], readBuffer[j]);
                }
            }

            var last = expected[length];
            var lastBuffer = new byte[last.Length];
            for (var j = 0; j < last.Length; ++j)
            {
                lastBuffer[j] = (byte)twister.RandInt(256);
            }

            var lastReaded = target.ReadBits(lastBuffer, 0, length + 1);
            Assert.AreEqual(buffer.Length * 8 - acc, lastReaded);
            for (var j = 0; j < last.Length; ++j)
            {
                Assert.AreEqual(last[j], lastBuffer[j]);
            }

            // Teste com desvio
            stream.Seek(0, SeekOrigin.Begin);
            target = new BitReader(stream, 2);
            var expOff = new byte[buffer.Length];
            target.ReadBits(expOff, 0, 2);

            lastReaded = target.ReadBits(
                expOff,
                11,
                33);

            Assert.AreEqual(33, lastReaded);
            var exp = new byte[]{
                0x1,
                0xF8,
                0x43,
                0x8D,
                0xD7,
                0x0F,
                0x0,
                0x0,
            };

            for (var i = 0; i < exp.Length; ++i)
            {
                Assert.AreEqual(exp[i], expOff[i]);
            }

            // Teste sem desvio
            stream.Seek(0, SeekOrigin.Begin);
            target = new BitReader(stream, 2);
            expOff = new byte[buffer.Length];
            lastReaded = target.ReadBits(
                expOff,
                0,
                buffer.Length * 8);
            Assert.AreEqual(buffer.Length * 8, lastReaded);
            for (var i = 0; i < buffer.Length; ++i)
            {
                Assert.AreEqual(buffer[i], expOff[i]);
            }

            // Teste sem desvio na leitura mas com desvio na escrita
            stream.Seek(0, SeekOrigin.Begin);
            target = new BitReader(stream, 2);
            exp = new byte[]{
                0xD0, 
                0x1F, 
                0x6A, 
                0xBC, 
                0x7E, 
                0x9A, 
                0x1B, 
                0x5C,
                0x0
            };

            expOff = new byte[exp.Length];
            lastReaded = target.ReadBits(
                expOff,
                4,
                buffer.Length * 8);
            for (var i = 0; i < exp.Length; ++i)
            {
                Assert.AreEqual(exp[i], expOff[i]);
            }
        }

        /// <summary>
        /// Testa a função de leitura de bits.
        /// </summary>
        [Description("Testa a alteração e leitura da posição do cursor.")]
        [TestMethod]
        public void BitReader_PositionChangesTest()
        {
            var buffer = new byte[]{
                0xFD, // 253
                0xA1, // 161
                0xC6, // 198
                0xEB, // 235
                0xA7, // 167
                0xB9, // 185
                0xC1, // 193
                0x05  // 5
            };

            var expectedBits = new int[]{
                1, 0, 1, 1, 1, 1, 1, 1,
                1, 0, 0, 0, 0, 1, 0, 1,
                0, 1, 1, 0, 0, 0, 1, 1,
                1, 1, 0, 1, 0, 1, 1, 1,
                1, 1, 1, 0, 0, 1, 0, 1,
                1, 0, 0, 1, 1, 1, 0, 1,
                1, 0, 0, 0, 0, 0, 1, 1,
                1, 0, 1, 0, 0, 0, 0, 0
            };

            var stream = new MemoryStream(buffer);
            var target = new BitReader(stream, 2);

            // Testa o valor do comprimento.
            var length = target.Length;
            Assert.AreEqual(expectedBits.Length, length);

            // Testa o valor da posição após leitura de bits.
            for (var i = 0; i < length + 1; ++i)
            {
                var innerPos = target.Position;
                Assert.AreEqual(i, innerPos);
                target.ReadBit();
            }

            for (var i = 64; i < 164; ++i)
            {
                Assert.AreEqual(64, target.Position);
                target.ReadBit();
            }

            // Estabelece nova posição e efectua a leitura.
            target.Position = 20;
            var expected = new byte[] { 0, 0, 1, 1, };
            for (var i = 20; i < 20 + expected.Length; ++i)
            {
                var innerPos = target.Position;
                Assert.AreEqual(i, innerPos);
                var innerBit = target.ReadBit();
                Assert.AreEqual(expected[i - 20], innerBit);
            }

            // Tratamento da posição com leitura de muitos bits.
            target.Position = 40;
            expected = new byte[] { 0x39, 0x06, 0x1C, 0x01 };
            var readBuffer = new byte[1];
            var p = 0;
            for (var i = 40; i < target.Length; i += 6)
            {
                var innerPos = target.Position;
                Assert.AreEqual(i, innerPos);
                var readed = target.ReadBits(
                    readBuffer,
                    0,
                    6);
                Assert.AreEqual(6, readed);
                Assert.AreEqual(expected[p++], readBuffer[0]);
            }

            for (var i = 0; i < 10; ++i)
            {
                var innerPos = target.Position;
                Assert.AreEqual(64, innerPos);
                var readed = target.ReadBits(
                    readBuffer,
                    0,
                    6);
                Assert.AreEqual(0, readed);
            }
        }
    }

    /// <summary>
    /// Testa a escrita de bits.
    /// </summary>
    [TestClass]
    public class BitWriterTest
    {
        /// <summary>
        /// Testa a leitura de bits individuais.
        /// </summary>
        [Description("Testa a escrita de bits.")]
        [TestMethod]
        public void BitWriter_WriteBitTest()
        {
            var initialBuffer = new byte[]{
                0x1A, 0x2B, 0x3C, 0x4D, 0x5E,
                0x6F, 0x71, 0x82, 0x93, 0xAB,
                0xCD, 0xEF, 0x12, 0x43, 0x54
            };

            var memoryBuffer = new byte[initialBuffer.Length];
            var twister = new MTRand();
            for (var i = 0; i < memoryBuffer.Length; ++i)
            {
                memoryBuffer[i] = (byte)twister.RandInt(255);
            }

            var len = initialBuffer.Length * 8;
            var memoryStream = new MemoryStream(memoryBuffer);
            var readStream = new MemoryStream(initialBuffer);
            var target = new BitWriter(memoryStream);
            var reader = new BitReader(readStream);
            for (var i = 0; i < len; ++i)
            {
                var readed = reader.ReadBit();
                Assert.AreNotEqual(-1, readed);

                target.WriteBit((byte)readed);
            }

            target.Flush();
            CollectionAssert.AreEqual(
                (ICollection)initialBuffer,
                (ICollection)memoryBuffer);
        }

        [Description("Testa a escrita de vários bits.")]
        [TestMethod]
        public void BitWriter_WriteBitsTest()
        {
            // Testa a escrita sequencial
            var initialBuffer = new byte[]{
                0x1A, 0x2B, 0x3C, 0x4D, 0x5E,
                0x6F, 0x71, 0x82, 0x93, 0xAB,
                0xCD, 0xEF, 0x12, 0x43, 0x54
            };

            var memoryBuffer = new byte[initialBuffer.Length];
            var twister = new MTRand();
            for (var i = 0; i < memoryBuffer.Length; ++i)
            {
                memoryBuffer[i] = (byte)twister.RandInt(255);
            }

            var memoryStream = new MemoryStream(memoryBuffer);
            var readStream = new MemoryStream(initialBuffer);
            var target = new BitWriter(memoryStream);
            var reader = new BitReader(readStream);

            var readBuffer = new byte[2];
            for (var i = 0; i < 15; ++i)
            {
                reader.ReadBits(
                    readBuffer,
                    0,
                    i + 1);
                target.WriteBits(readBuffer, 0, i + 1);
            }

            target.Flush();
            CollectionAssert.AreEqual(
                (ICollection)initialBuffer,
                (ICollection)memoryBuffer);

            // Testa a escrita alinhada total
            for (var i = 0; i < memoryBuffer.Length; ++i)
            {
                memoryBuffer[i] = (byte)twister.RandInt(255);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            target = new BitWriter(memoryStream);
            target.WriteBits(
                initialBuffer, 
                0, 
                initialBuffer.Length * 8);

            target.Flush();
            CollectionAssert.AreEqual(
                (ICollection)initialBuffer,
                (ICollection)memoryBuffer);

            // Testa a escrita alinhada com resto
            for (var i = 0; i < memoryBuffer.Length; ++i)
            {
                memoryBuffer[i] = (byte)twister.RandInt(255);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            target = new BitWriter(memoryStream);
            target.WriteBits(
                initialBuffer,
                0,
                initialBuffer.Length * 8 - 4);

            target.ForceFlush();
            var len = initialBuffer.Length;
            for (var i = 0; i < len - 1; ++i)
            {
                Assert.AreEqual(initialBuffer[i], memoryBuffer[i]);
            }

            Assert.AreEqual(0x04, memoryBuffer[len - 1]);

            // Testa a escrita alinhada com desvio e com resto.
            for (var i = 0; i < memoryBuffer.Length; ++i)
            {
                memoryBuffer[i] = (byte)twister.RandInt(255);
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            target = new BitWriter(memoryStream);
            target.WriteBits(
                initialBuffer,
                4,
                initialBuffer.Length * 8 - 4);
            target.ForceFlush();

            var expectedBuffer = new byte[]{
                0xB1, 0xC2, 0xD3, 0xE4, 0xF5,
                0x16, 0x27, 0x38, 0xB9, 0xDA,
                0xFC, 0x2E, 0x31, 0x44, 0x05
            };

            for (var i = 0; i < len - 1; ++i)
            {
                Assert.AreEqual(expectedBuffer[i], memoryBuffer[i]);
            }

            // Testa a escrita alinhada com desvio e sem resto.
            memoryStream.Seek(0, SeekOrigin.Begin);
            target = new BitWriter(memoryStream);
            target.WriteBits(
                initialBuffer,
                4,
                (len - 1) * 8);
            target.ForceFlush();

            expectedBuffer[expectedBuffer.Length - 1] = 0x04;
            for (var i = 0; i < len - 1; ++i)
            {
                Assert.AreEqual(expectedBuffer[i], memoryBuffer[i]);
            }

            // Testa a escrita não alinhada
            memoryStream.Seek(0, SeekOrigin.Begin);
            target = new BitWriter(memoryStream);
            target.WriteBits(new byte[] { 0xA }, 0, 4);
            target.WriteBits(
                initialBuffer,
                4,
                initialBuffer.Length * 8 - 4);
            target.Flush();

            CollectionAssert.AreEqual(
                (ICollection)initialBuffer,
                (ICollection)memoryBuffer);
        }
    }

    /// <summary>
    /// Testa a escrita de uma sequência crescente de números longos positivos.
    /// </summary>
    [TestClass]
    public class UlongIncSeqWriterReaderTest
    {
        /// <summary>
        /// Testa a escrita e leitura de uma sequência de números primos.
        /// </summary>
        [Description("Testa a escrita e leitura de uma sequência de números primos.")]
        [TestMethod]
        public void TestUlongIncSeqWriterReader()
        {
            var values = new[] {
                2UL, 3UL, 5UL, 7UL, 11UL, 13UL, 17UL, 19UL, 23UL, 29UL, 31UL,
                37UL, 41UL, 43UL, 47UL, 53UL, 59UL, 61UL, 67UL, 71UL, 73UL };

            var buffer = new byte[1000];
            var memoryStream = new MemoryStream(buffer);
            var bitwriter = new BitWriter(memoryStream);

            var seqWriter = new UlongIncSeqWriter(bitwriter);
            for (var i = 0; i < values.Length; ++i)
            {
                seqWriter.WriteNext(values[i]);
            }

            seqWriter.CloseSequence();

            memoryStream.Seek(0, SeekOrigin.Begin);
            var bitreader = new BitReader(memoryStream);
            var seqReader = new UlongIncSeqReader(bitreader);
            for (var i = 0; i < values.Length; ++i)
            {
                seqReader.MoveNext();
                Assert.AreEqual(values[i], seqReader.CurrentValue);
            }

            Assert.IsFalse(seqReader.MoveNext());
        }
    }

    /// <summary>
    /// Representa um valor qualquer.
    /// </summary>
    /// <typeparam name="T">O tipo do valor.</typeparam>
    internal class ObjectValue<T> : IConvertible
    {
        /// <summary>
        /// Mantém o valor.
        /// </summary>
        private T value;

        /// <summary>
        /// Define o operador que permite efectuar conversões explícitas.
        /// </summary>
        /// <param name="current">O objecto actual.</param>
        /// <returns>O objecto resultante.</returns>
        public static explicit operator T(ObjectValue<T> current)
        {
            if (typeof(T).IsValueType)
            {
                if (current.value == null)
                {
                    throw new InvalidOperationException("Nullable object must have a value.");
                }
                else
                {
                    return current.value;
                }
            }
            else
            {
                return current.value;
            }
        }

        /// <summary>
        /// Define o operador que permite efectuar conversões implícitas.
        /// </summary>
        /// <param name="current">O objecto a ser convertido.</param>
        /// <returns>O objecto resultante.</returns>
        public static implicit operator ObjectValue<T>(T current)
        {
            return new ObjectValue<T>(current);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ObjectValue{T}"/>
        /// </summary>
        /// <param name="value">O valor.</param>
        public ObjectValue(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Obtém o valor.
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Obtém o código do tipo para o objecto corrente.
        /// </summary>
        /// <returns>O código do tipo.</returns>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        /// <summary>
        /// Converte o objecto corrente num valor lógico.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O reultado da conversão.</returns>
        public bool ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num "byte".
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O reultado da conversão.</returns>
        public byte ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num carácter.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public char ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente numa data.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(this.value);
        }

        /// <summary>
        /// Converte o objecto corrente num valor decimal.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public decimal ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num número de precisão dupla.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public double ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num inteiro curto.
        /// </summary>
        /// <param name="provider">O provedor da cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public short ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num inteiro.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public int ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num inteiro longo.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O reultado da conversão.</returns>
        public long ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num inteiro de um "byte".
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O reultado da conversão.</returns>
        public sbyte ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num número de precisão simples.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public float ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num valor textual.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public string ToString(IFormatProvider provider)
        {
            return Convert.ToString(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente no tipo espeificado.
        /// </summary>
        /// <param name="conversionType">O tipo a ser convertido.</param>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this.value, conversionType, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num inteiro curto sem sinal.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public ushort ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num inteiro sem sinal.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da convesão.</returns>
        public uint ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32(this.value, provider);
        }

        /// <summary>
        /// Converte o objecto corrente num inteiro longo sem sinal.
        /// </summary>
        /// <param name="provider">O provedor de cultura.</param>
        /// <returns>O resultado da conversão.</returns>
        public ulong ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64(this.value, provider);
        }
    }

    /// <summary>
    /// Leitor que permite encapsular leitores de valores por referência.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem os valores.</typeparam>
    /// <typeparam name="SymbValue">O tipo de objectos que constituem os valores dos símbolos lidos.</typeparam>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolos lidos.</typeparam>
    internal class ObjectValueParser<T, SymbValue, SymbType>
        : IParse<ObjectValue<T>, SymbValue, SymbType>
        where T : struct
    {
        /// <summary>
        /// O leitor dos valores.
        /// </summary>
        private IParse<T, SymbValue, SymbType> innerParser;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ObjectValueParser{T, SymbValue, SymbType}"/>.
        /// </summary>
        /// <param name="innerParser">O leitor interno.</param>
        public ObjectValueParser(IParse<T, SymbValue, SymbType> innerParser)
        {
            if (innerParser == null)
            {
                throw new ArgumentNullException("innerParser");
            }
            else
            {
                this.innerParser = innerParser;
            }
        }

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public ObjectValue<T> Parse(
            ISymbol<SymbValue, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            return innerParser.Parse(symbolListToParse, errorLogs);
        }
    }

    /// <summary>
    /// Permite obter texto a partir de uma lista de símbolos.
    /// </summary>
    internal class StringConcatParser : IParse<string, string, string>
    {
        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public string Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var result = string.Empty;
            if (symbolListToParse != null)
            {
                var length = symbolListToParse.Length;
                for (int i = 0; i < length; ++i)
                {
                    result += symbolListToParse[i].SymbolValue;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Classe de testes para testar funcionalidades.
    /// </summary>
    internal class TestPerson
    {
        /// <summary>
        /// O primeiro nome.
        /// </summary>
        private string firstName;

        /// <summary>
        /// O último nome.
        /// </summary>
        private string lastName;

        /// <summary>
        /// A idade.
        /// </summary>
        private ushort age;

        /// <summary>
        /// A data de nascimento.
        /// </summary>
        private DateTime birthDate;

        /// <summary>
        /// O sexo.
        /// </summary>
        private byte gender;

        /// <summary>
        /// O estado civil.
        /// </summary>
        private byte marritalStatus;

        /// <summary>
        /// Valor que indica se se trata de um empregado.
        /// </summary>
        private bool isEmployee;

        /// <summary>
        /// O superior hierárquico.
        /// </summary>
        private TestPerson hierarquicalSuperior;

        /// <summary>
        /// O primeiro nome.
        /// </summary>
        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("First name can't be null nor empty.");
                }
                else
                {
                    this.firstName = value.Trim();
                }
            }
        }

        /// <summary>
        /// O último nome.
        /// </summary>
        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Last name can't be null nor empty.");
                }
                else
                {
                    this.lastName = value;
                }
            }
        }

        /// <summary>
        /// A idade.
        /// </summary>
        public ushort Age
        {
            get
            {
                return this.age;
            }
            set
            {
                this.age = value;
            }
        }

        /// <summary>
        /// A data de nascimento.
        /// </summary>
        public DateTime BirthDate
        {
            get
            {
                return this.birthDate;
            }
            set
            {
                this.birthDate = value;
            }
        }

        /// <summary>
        /// O sexo.
        /// </summary>
        public byte Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                if (value == 0 || value == 1)
                {
                    this.gender = value;
                }
                else
                {
                    throw new Exception("Wrong value for gender.");
                }
            }
        }

        /// <summary>
        /// O estado civil.
        /// </summary>
        public byte MarritalStatus
        {
            get
            {
                return this.marritalStatus;
            }
            set
            {
                if (value == 0 || value == 1 || value == 2)
                {
                    this.marritalStatus = value;
                }
                else
                {
                    throw new Exception("Wrong value for marital status.");
                }
            }
        }

        /// <summary>
        /// Valor que indica se se trata de um empregado.
        /// </summary>
        public bool IsEmployee
        {
            get
            {
                return this.isEmployee;
            }
            set
            {
                this.isEmployee = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o superior hierárquico.
        /// </summary>
        public TestPerson HierarquicalSuperior
        {
            get
            {
                return this.hierarquicalSuperior;
            }
            set
            {
                this.hierarquicalSuperior = value;
            }
        }
    }
}
