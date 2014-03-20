namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;
    using Utilities;

    [TestClass()]
    public class UnivariatePolynomialNormalFormTest
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
        public void GetPolynomialDerivativeTest_SimpleInteger()
        {
            // Representação dos polinómios.
            var polynomText = "x^1000-2*x^550+1000*x^10+50";
            var polDerivativeText = "1000*x^999-1100*x^549+10000*x^9";

            var variableName = "x";

            // Estabelece os domínios.
            var integerDomain = new IntegerDomain();
            var longDomain = new LongDomain();
            var bigIntegerDomain = new BigIntegerDomain();

            // Estabelece os conversores.
            var integerToIntegerConv = new ElementToElementConversion<int>();
            var integerToLongConv = new LongToIntegerConversion();
            var integerToBigIntegerConvsersion = new BigIntegerToIntegerConversion();

            // Estabelece os leitores individuais.
            var integerParser = new IntegerParser<string>();
            var longParser = new LongParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();

            // Estabelece os polinómios.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                integerDomain,
                integerParser,
                integerToIntegerConv,
                variableName);
            var integerExpectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                polDerivativeText,
                integerDomain,
                integerParser,
                integerToIntegerConv,
                variableName);
            var integerActualDerivative = integerPolynomial.GetPolynomialDerivative(integerDomain);

            // Verifica se os polinómios são válidos.
            Assert.AreEqual(integerExpectedPolynomial, integerActualDerivative);

            // Estabelece os polinómios.
            var longPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                longDomain,
                longParser,
                integerToLongConv,
                variableName);
            var longExpectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                polDerivativeText,
                longDomain,
                longParser,
                integerToLongConv,
                variableName);
            var longActualDerivative = longPolynomial.GetPolynomialDerivative(longDomain);

            // Verifica se os polinómios são válidos.
            Assert.AreEqual(longExpectedPolynomial, longActualDerivative);

            // Estabelece os polinómios.
            var bigIntegerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                bigIntegerDomain,
                bigIntegerParser,
                integerToBigIntegerConvsersion,
                variableName);
            var bigIntegerExpectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                polDerivativeText,
                bigIntegerDomain,
                bigIntegerParser,
                integerToBigIntegerConvsersion,
                variableName);
            var bigIntegerActualDerivative = bigIntegerPolynomial.GetPolynomialDerivative(bigIntegerDomain);

            // Verifica se os polinómios são válidos.
            Assert.AreEqual(bigIntegerExpectedPolynomial, bigIntegerActualDerivative);
        }
    }
}
