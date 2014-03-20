namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;
    using Utilities;

    [TestClass()]
    public class LagrangeAlgorithmTest
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
        public void RunTest_Integer()
        {
            var integerDomain = new IntegerDomain();
            var lagAlg = new LagrangeAlgorithm<int>(integerDomain);
            var firstValue = 1176;
            var secondValue = 180;
            var result = lagAlg.Run(firstValue, secondValue);
            Assert.AreEqual(12, result.GreatestCommonDivisor);

            var actualExpression = result.FirstFactor * result.FirstItem + result.SecondFactor * result.SecondItem;
            Assert.AreEqual(result.GreatestCommonDivisor, actualExpression);

            actualExpression = result.GreatestCommonDivisor * result.FirstCofactor;
            Assert.AreEqual(result.FirstItem, actualExpression);

            actualExpression = result.GreatestCommonDivisor * result.SecondCofactor;
            Assert.AreEqual(result.SecondItem, actualExpression);
        }

        [TestMethod()]
        public void RunTest_BigInteger()
        {
            var integerDomain = new BigIntegerDomain();
            var lagAlg = new LagrangeAlgorithm<BigInteger>(integerDomain);
            var firstValue = BigInteger.Parse("91986494539681");
            var secondValue = BigInteger.Parse("19645957369297");
            var result = lagAlg.Run(firstValue, secondValue);
            Assert.AreEqual(9590959, result.GreatestCommonDivisor);

            var actualExpression = result.FirstFactor * result.FirstItem + result.SecondFactor * result.SecondItem;
            Assert.AreEqual(result.GreatestCommonDivisor, actualExpression);

            actualExpression = result.GreatestCommonDivisor * result.FirstCofactor;
            Assert.AreEqual(result.FirstItem, actualExpression);

            actualExpression = result.GreatestCommonDivisor * result.SecondCofactor;
            Assert.AreEqual(result.SecondItem, actualExpression);
        }

        [TestMethod()]
        public void RunTest_IntegerPolynomial()
        {
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);
            var integerParser = new IntegerParser<string>();
            var conversion = new ElementToElementConversion<int>();
            var fractionConversion = new ElementFractionConversion<int>(integerDomain);
            string variableName = "x";
            var univarPolDomain = new UnivarPolynomEuclideanDomain<Fraction<int>>(
                variableName,
                fractionField);

            var lagAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<Fraction<int>>>(univarPolDomain);
            var firstValue = TestsHelper.ReadFractionalCoeffsUnivarPol<int, IntegerDomain>(
                "(x-1/2)*(x+1/3)",
                integerDomain,
                integerParser,
                fractionConversion,
                variableName);

            var secondValue = TestsHelper.ReadFractionalCoeffsUnivarPol<int, IntegerDomain>(
                "(x-1/2)*(x-1)",
                integerDomain,
                integerParser,
                fractionConversion,
                variableName);

            var gcd = TestsHelper.ReadFractionalCoeffsUnivarPol<int, IntegerDomain>(
                "x-1/2",
                integerDomain,
                integerParser,
                fractionConversion,
                variableName);
            var result = lagAlg.Run(firstValue, secondValue);

            var mainGcdCoeff = result.GreatestCommonDivisor.GetLeadingCoefficient(fractionField);
            var monicGcd = result.GreatestCommonDivisor.Multiply(
                fractionField.MultiplicativeInverse(mainGcdCoeff),
                fractionField);
            Assert.AreEqual(gcd, monicGcd);

            var firstTermExpression = univarPolDomain.Multiply(result.FirstFactor, result.FirstItem);
            var secondTermExpression = univarPolDomain.Multiply(result.SecondFactor, result.SecondItem);
            var actualExpression = univarPolDomain.Add(firstTermExpression, secondTermExpression);
            Assert.AreEqual(result.GreatestCommonDivisor, actualExpression);

            actualExpression = univarPolDomain.Multiply(result.GreatestCommonDivisor, result.FirstCofactor);
            Assert.AreEqual(result.FirstItem, actualExpression);

            actualExpression = univarPolDomain.Multiply(result.GreatestCommonDivisor, result.SecondCofactor);
            Assert.AreEqual(result.SecondItem, actualExpression);
        }

        [TestMethod()]
        public void RunTest_BigIntegerPolynomial()
        {
            var integerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<BigInteger>(integerDomain);
            var integerParser = new BigIntegerParser<string>();
            var conversion = new BigIntegerToIntegerConversion();
            var fractionConversion = new OuterElementFractionConversion<int, BigInteger>(conversion, integerDomain);
            string variableName = "x";
            var univarPolDomain = new UnivarPolynomEuclideanDomain<Fraction<BigInteger>>(
                variableName,
                fractionField);

            var lagAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<Fraction<BigInteger>>>(univarPolDomain);
            var firstValue = TestsHelper.ReadFractionalCoeffsUnivarPol<BigInteger, BigIntegerDomain>(
                "(x-1/2)^2*(x+1/3)^5",
                integerDomain,
                integerParser,
                fractionConversion,
                variableName);

            var secondValue = TestsHelper.ReadFractionalCoeffsUnivarPol<BigInteger, BigIntegerDomain>(
                "(x-1/2)^3*(x+1/3)^2",
                integerDomain,
                integerParser,
                fractionConversion,
                variableName);

            var gcd = TestsHelper.ReadFractionalCoeffsUnivarPol<BigInteger, BigIntegerDomain>(
                "(x-1/2)^2*(x+1/3)^2",
                integerDomain,
                integerParser,
                fractionConversion,
                variableName);
            var result = lagAlg.Run(firstValue, secondValue);

            var mainGcdCoeff = result.GreatestCommonDivisor.GetLeadingCoefficient(fractionField);
            var monicGcd = result.GreatestCommonDivisor.Multiply(
                fractionField.MultiplicativeInverse(mainGcdCoeff),
                fractionField);
            Assert.AreEqual(gcd, monicGcd);

            var firstTermExpression = univarPolDomain.Multiply(result.FirstFactor, result.FirstItem);
            var secondTermExpression = univarPolDomain.Multiply(result.SecondFactor, result.SecondItem);
            var actualExpression = univarPolDomain.Add(firstTermExpression, secondTermExpression);
            Assert.AreEqual(result.GreatestCommonDivisor, actualExpression);

            actualExpression = univarPolDomain.Multiply(result.GreatestCommonDivisor, result.FirstCofactor);
            Assert.AreEqual(result.FirstItem, actualExpression);

            actualExpression = univarPolDomain.Multiply(result.GreatestCommonDivisor, result.SecondCofactor);
            Assert.AreEqual(result.SecondItem, actualExpression);
        }
    }
}
