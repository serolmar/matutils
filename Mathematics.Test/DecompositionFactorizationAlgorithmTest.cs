namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using Utilities;

    [TestClass()]
    public class DecompositionFactorizationAlgorithmTest
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
        public void RunTest_IntegerNumbersRhoAlg()
        {
            var integerNumber = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var conversion = new ElementToElementConversion<int>();
            var variableName = "x";
            var testPols = new List<UnivariatePolynomialNormalForm<int>>();
            testPols.Add(TestsHelper.ReadUnivarPolynomial("x^2+1", integerNumber, integerParser, conversion, variableName));
            testPols.Add(TestsHelper.ReadUnivarPolynomial("x^2+x+1", integerNumber, integerParser, conversion, variableName));

            var rhoAlgorithm = new PollardRhoAlgorithm<int>(
                testPols,
                new ModularIntegerFieldFactory(),
                integerNumber);
            var factorizationTarget = new DecompositionFactorizationAlgorithm<int, int>(
                rhoAlgorithm,
                1,
                integerNumber,
                integerNumber);
            var value = 72;
            var expected = new Dictionary<int, int>();
            expected.Add(2,3);
            expected.Add(3, 2);
            var actual = factorizationTarget.Run(value);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RunTest_BigIntegerNumbersRhoAlg()
        {
            var bigIntegerNumber = new BigIntegerDomain();
            var integerNumber = new IntegerDomain();
            var integerParser = new BigIntegerParser<string>();
            var conversion = new BigIntegerToIntegerConversion();
            var variableName = "x";
            var testPols = new List<UnivariatePolynomialNormalForm<BigInteger>>();
            testPols.Add(TestsHelper.ReadUnivarPolynomial("x^123+1", bigIntegerNumber, integerParser, conversion, variableName));
            testPols.Add(TestsHelper.ReadUnivarPolynomial("x^452+1537*x+1", bigIntegerNumber, integerParser, conversion, variableName));

            var rhoAlgorithm = new PollardRhoAlgorithm<BigInteger>(
                testPols,
                new ModularBigIntFieldFactory(),
                bigIntegerNumber);
            var factorizationTarget = new DecompositionFactorizationAlgorithm<BigInteger, int>(
                rhoAlgorithm,
                1,
                integerNumber,
                bigIntegerNumber);
            var value = BigInteger.Parse("1000000000001");
            var expected = new Dictionary<BigInteger, int>();
            expected.Add(137, 1);
            expected.Add(73, 1);
            expected.Add(BigInteger.Parse("99990001"), 1);
            var actual = factorizationTarget.Run(value);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
