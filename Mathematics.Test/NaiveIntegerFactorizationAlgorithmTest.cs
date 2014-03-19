using Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Mathematics.Test
{
    [TestClass()]
    public class NaiveIntegerFactorizationAlgorithmTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void RunTest_Integer()
        {
            var integerSquareRootAlg = new IntegerSquareRootAlgorithm();
            var integerNumber = new IntegerDomain();
            var factorizationAlg = new NaiveIntegerFactorizationAlgorithm<int>(
                integerSquareRootAlg,
                integerNumber);
            var value = 12;
            var expected = new Dictionary<int, int>();
            expected.Add(2, 2);
            expected.Add(3, 1);
            var actual = factorizationAlg.Run(value);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod()]
        public void RunTest_BigInteger()
        {
            var integerSquareRootAlg = new BigIntSquareRootAlgorithm();
            var integerNumber = new BigIntegerDomain();
            var factorizationAlg = new NaiveIntegerFactorizationAlgorithm<BigInteger>(
                integerSquareRootAlg,
                integerNumber);
            var value = BigInteger.Parse("1000000000001");
            var expected = new Dictionary<BigInteger, int>();
            expected.Add(137, 1);
            expected.Add(73, 1);
            expected.Add(BigInteger.Parse("99990001"), 1);
            var actual = factorizationAlg.Run(value);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
