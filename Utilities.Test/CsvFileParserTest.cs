namespace Utilities.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    [TestClass]
    public class CsvFileParserTest
    {
        /// <summary>
        /// Permite testar a leitura de um ficheiro csv para  uma lista de listas de valores
        /// numéricos.
        /// </summary>
        [TestMethod]
        public void Parse_DoublesToListOfLists()
        {
            var csvContentBuilder = new StringBuilder();
            csvContentBuilder.AppendLine("1;2;3");
            csvContentBuilder.AppendLine("4;5;6");
            csvContentBuilder.AppendLine("7;8;0.9");
            csvContentBuilder.AppendLine("0.7;0.8;9");

            var csvString = csvContentBuilder.ToString();
            var csvStringReader = new StringReader(csvString);
            var stringSymbolReader = new StringSymbolReader(csvStringReader, true, false);

            // Providencia os leitores de elementos em cada célula.
            var dataProvider = new DataReaderProvider<IParse<double, string, string>>(
                new DoubleParser<string>());

            // Cria um leitor para o ficheiro.
            var csvParser = new CsvFileParser<List<List<double>>, double, string, string>(
                "new_line",
                "semi_colon",
                dataProvider);
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
    }
}
