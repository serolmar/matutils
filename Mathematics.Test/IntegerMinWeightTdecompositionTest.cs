namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    [TestClass()]
    public class IntegerMinWeightTdecompositionTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void RunTest_ListOfLists()
        {
            var costs = new List<List<int>>();
            costs.Add(new List<int>() { 12, 9, 7, 6, 4 });
            costs.Add(new List<int>() { 12, 11, 8, 7, 5 });
            costs.Add(new List<int>() { 13, 10, 9, 6, 3 });
            costs.Add(new List<int>() { 12, 8, 6, 4, 2 });

            var integerDomain = new IntegerDomain();
            var numberTdecomposition = new IntegerMinWeightTdecomposition<int>(
                Comparer<int>.Default,
                integerDomain);

            var result = numberTdecomposition.Run(18, costs);
            var expectedMedians = new List<int>() { 3, 5, 5, 5 };
            Assert.AreEqual(17, result.Cost);
            CollectionAssert.AreEquivalent(expectedMedians, result.Medians.ToList());
        }

        [TestMethod()]
        public void RunTest_TabularItem()
        {
            var csvStringBuilder = new StringBuilder();
            csvStringBuilder.AppendLine("12, 9,7,6,4");
            csvStringBuilder.AppendLine("12,11,8,7,5");
            csvStringBuilder.AppendLine("13,10,9,6,3");
            csvStringBuilder.Append("12,8,6,4,2");
            var csvString = csvStringBuilder.ToString();
            var csvStringReader = new StringReader(csvString);

            var parser = new IntegerParser<string>();
            var csvParser = new CsvFileParser<ITabularItem, int, string, string>(
                "new_line",
                "comma",
                (i, j) => parser);
            csvParser.AddIgnoreType("space");
            csvParser.AddIgnoreType("carriage_return");

            var csvSymbolReader = new StringSymbolReader(csvStringReader, true, false);
            var adder = new TabularItemAdder<int>();
            var tabularItem = new TabularListsItem();
            csvParser.Parse(csvSymbolReader, tabularItem, adder);

            var integerDomain = new IntegerDomain();
            var numberTdecomposition = new IntegerMinWeightTdecomposition<int>(
                Comparer<int>.Default,
                integerDomain);

            var result = numberTdecomposition.Run(18, tabularItem);
            var expectedMedians = new List<int>() { 3, 5, 5, 5 };
            Assert.AreEqual(17, result.Cost);
            CollectionAssert.AreEquivalent(expectedMedians, result.Medians.ToList());
        }
    }
}
