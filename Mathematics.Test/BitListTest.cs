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
    }
}
