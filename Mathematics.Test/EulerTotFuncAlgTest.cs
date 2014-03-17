using Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Numerics;

namespace Mathematics.Test
{
    [TestClass()]
    public class EulerTotFuncAlgTest
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
            var squareRootAlgorithm = new IntegerSquareRootAlgorithm();
            var primeNumberFactory = new PrimeNumbersIteratorFactory();
            var integerNumber = new IntegerDomain();
            var totientAlgorithm = new EulerTotFuncAlg<int>(
                squareRootAlgorithm,
                primeNumberFactory,
                integerNumber);
            var values = new[] {1, 12, 343, 720, 73, 100 };
            var expected = new[] { 1, 4, 294, 192, 72, 40 };
            for (int i = 0; i < values.Length; ++i)
            {
                var actual = totientAlgorithm.Run(values[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void RunTest_BigInteger()
        {
            var squareRootAlgorithm = new BigIntSquareRootAlgorithm();
            var primeNumberFactory = new BigIntegerPrimeNumbersIteratorFactory();
            var integerNumber = new BigIntegerDomain();
            var totientAlgorithm = new EulerTotFuncAlg<BigInteger>(
                squareRootAlgorithm,
                primeNumberFactory,
                integerNumber);
            var values = new[] { 1, 12, 343, 720, 73, 100 };
            var expected = new[] { 1, 4, 294, 192, 72, 40 };
            for (int i = 0; i < values.Length; ++i)
            {
                var actual = totientAlgorithm.Run(values[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }
    }
}
