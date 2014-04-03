namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;
    using Utilities;

    [TestClass()]
    public class SquareFreeFractionFactorizationAlgTest
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
        public void RunTest()
        {
            var polynomialText = "((2*x+1)*(x-4))^2*(x+3)^3";

            // Os objectos responsáveis pelas operações sobre os coeficientes
            var bigIntegerDomain = new BigIntegerDomain();
            var bigIntegerParser = new BigIntegerParser<string>();
            var bigIntToIntegerConversion = new BigIntegerToIntegerConversion();
            var bigIntFractionConversion = new OuterElementFractionConversion<int, BigInteger>(
                bigIntToIntegerConversion,
                bigIntegerDomain);

            var polynomial = TestsHelper.ReadFractionalCoeffsUnivarPol<BigInteger, BigIntegerDomain>(
                polynomialText,
                bigIntegerDomain,
                bigIntegerParser,
                bigIntFractionConversion,
                "x");

            var squareFreeFactorizationAlg = new SquareFreeFractionFactorizationAlg<BigInteger>(
                bigIntegerDomain);
            var result = squareFreeFactorizationAlg.Run(polynomial);

            // O teste passa se a expansão da factorização ser igual ao polinómio original.
            Assert.IsTrue(result.Factors.Count > 0, "At least two factors are expected.");
            var factorsEnum = result.Factors.GetEnumerator();
            if (factorsEnum.MoveNext())
            {
                var polynomialDomain = new UnivarPolynomPseudoDomain<BigInteger>(
                    "x",
                    bigIntegerDomain);
                var productPol = MathFunctions.Power(
                    factorsEnum.Current.Value,
                    factorsEnum.Current.Key,
                    polynomialDomain);
                while (factorsEnum.MoveNext())
                {
                    var temporary = MathFunctions.Power(
                        factorsEnum.Current.Value,
                        factorsEnum.Current.Key,
                        polynomialDomain);
                    productPol = polynomialDomain.Multiply(
                        productPol,
                        temporary);
                }

                var fractionField = new FractionField<BigInteger>(bigIntegerDomain);
                var expectedPol = new UnivariatePolynomialNormalForm<Fraction<BigInteger>>("x");
                foreach (var term in productPol)
                {
                    expectedPol = expectedPol.Add(
                        result.IndependentCoeff.Multiply(term.Value, bigIntegerDomain),
                        term.Key,
                        fractionField);
                }

                Assert.AreEqual(expectedPol, polynomial);
            }
        }
    }
}
