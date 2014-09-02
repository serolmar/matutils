namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Utilities;

    [TestClass()]
    public class UnivarPolynomEuclideanDomainTest
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
        public void GetQuotientAndRemainderTest()
        {
            var dividend = "    x^3-1/3*x^2+ - -x/5-1/2";
            var divisor = "x^2-x/2+1";

            // Os objectos responsáveis pelas operações sobre os coeficientes.
            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var conversion = new ElementFractionConversion<int>(integerDomain);
            var fractionField = new FractionField<int>(integerDomain);

            // A leitura dos polinómios.
            var dividendPol = TestsHelper.ReadFractionalCoeffsUnivarPol<int, IntegerDomain>(
                dividend,
                integerDomain,
                integerParser,
                conversion,
                "x");
            var divisorPol = TestsHelper.ReadFractionalCoeffsUnivarPol<int, IntegerDomain>(
                divisor,
                integerDomain,
                integerParser,
                conversion,
                "x");
            var polynomialDomain = new UnivarPolynomEuclideanDomain<Fraction<int>>(
                        "x",
                        fractionField);
            var result = polynomialDomain.GetQuotientAndRemainder(dividendPol, divisorPol);
            var expected = divisorPol.Multiply(result.Quotient, fractionField);
            expected = expected.Add(result.Remainder, fractionField);

            Assert.AreEqual(expected, dividendPol);
        }
    }
}
