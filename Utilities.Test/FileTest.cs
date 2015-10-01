namespace Utilities.Test
{
    using System;
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
            tabularItem.Add(new[] { "b", "c", "d"});
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
}
