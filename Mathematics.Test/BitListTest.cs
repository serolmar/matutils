using Utilities.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Mathematics.Test
{
    [TestClass()]
    public class BitListTest
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
        public void AddRangeTest_BitList()
        {
            var initialCount = 5;
            var rangeCount = 10;
            BitList target = new BitList(initialCount, true);
            BitList range = new BitList(rangeCount,false);
            var totalCount = initialCount + rangeCount;
            target.AddRange(range);
            Assert.AreEqual(totalCount, target.Count);
            var i = 0;
            for (; i < initialCount; ++i)
            {
                var bit = target[i];
                Assert.AreNotEqual(0, bit);
            }

            for (; i < totalCount; ++i)
            {
                var bit = target[i];
                Assert.AreEqual(0, bit);
            }
        }

        [TestMethod()]
        public void AddRangeTest_BitsArray()
        {
            var initialCount = 5;
            var rangeCount = 10;
            BitList target = new BitList(initialCount, true);
            var bitsArray = new ulong[] { 0 };
            target.AddRange(bitsArray, rangeCount);
            var totalCount = initialCount + rangeCount;
            Assert.AreEqual(totalCount, target.Count);
            var i = 0;
            for (; i < initialCount; ++i)
            {
                var bit = target[i];
                Assert.AreNotEqual(0, bit);
            }

            for (; i < totalCount; ++i)
            {
                var bit = target[i];
                Assert.AreEqual(0, bit);
            }
        }

        [TestMethod()]
        public void ReadNumericTest_EmptyValue()
        {
            string numericText = "";
            var expected = new BitList();
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ReadNumericTest_SmallValue()
        {
            string numericText = "10";
            var expected = new BitList() { 0, 1, 0, 1 };
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ReadNumericTest_LeadingZeroesSmallValue()
        {
            string numericText = "0000010";
            var expected = new BitList() { 0, 1, 0, 1 };
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ReadNumericTest_BigValue()
        {
            // Centésima potência de 2
            string numericText = "1267650600228229401496703205376";
            var expected = new BitList();
            for (int i = 0; i < 100; ++i)
            {
                expected.Add(0);
            }

            expected.Add(1);
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }
    }
}
