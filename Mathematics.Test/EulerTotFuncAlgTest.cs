// -----------------------------------------------------------------------
// <copyright file="EulerTotFuncAlgTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;

    /// <summary>
    /// Testa o cálculo da função totiente.
    /// </summary>
    [TestClass()]
    public class EulerTotFuncAlgTest
    {
        /// <summary>
        /// Testa a função totiente para inteiros.
        /// </summary>
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

        /// <summary>
        /// Testa a função totiente para inteiros de precisão arbitrária.
        /// </summary>
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
