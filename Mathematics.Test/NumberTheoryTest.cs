using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utilities;

namespace Mathematics.Test
{
    /// <summary>
    /// Efectua testes aos corpos módulo um determinado valor.
    /// </summary>
    [TestClass]
    public class ModularFieldTest
    {
        /// <summary>
        /// Testa o corpo modular sobre polinómios.
        /// </summary>
        [TestMethod]
        public void UnivarPolModularFieldTest()
        {
            var integerDomain = new IntegerDomain();
            var conversion = new ElementFractionConversion<int>(integerDomain);
            var integerParser = new IntegerParser<string>();
            var fractionParser = new ElementFractionParser<int>(integerParser, integerDomain);
            var fractionField = new FractionField<int>(integerDomain);
            var polynomialDomain = new UnivarPolynomEuclideanDomain<Fraction<int>>(
                "x",
                fractionField);

            var module = TestsHelper.ReadUnivarPolynomial(
                "x^4+x^3+x^2+x+1",
                fractionField,
                fractionParser,
                conversion,
                "x"
                );


            var pol = TestsHelper.ReadUnivarPolynomial(
                "x-1",
                fractionField,
                fractionParser,
                conversion,
                "x"
                );

            // (x^4+x^3+x^2+x+1)-(x^3+2x^2+3x+1)(x-1)=5
            var expected = TestsHelper.ReadUnivarPolynomial(
                "-1/5*x^3-2/5*x^2-3/5*x-4/5",
                fractionField,
                fractionParser,
                conversion,
                "x"
                );

            var lgAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<Fraction<int>>>(polynomialDomain);
            var target = new GeneralModuleIntegralField<UnivariatePolynomialNormalForm<Fraction<int>>>(
                module,
                polynomialDomain,
                (m, n, d) =>
                {
                    var res = lgAlg.Run(m, n);
                    var gcd = res.GreatestCommonDivisor;
                    var secondFactor = res.SecondFactor;
                    var greatestDegreeCoeff = gcd.GetLeadingCoefficient(fractionField);
                    if (!fractionField.IsMultiplicativeUnity(greatestDegreeCoeff))
                    {
                        var inverse = fractionField.MultiplicativeInverse(greatestDegreeCoeff);
                        gcd = gcd.Multiply(inverse, fractionField);
                        secondFactor = secondFactor.Multiply(inverse, fractionField);
                    }

                    return Tuple.Create(gcd, secondFactor);
                });

            var actual = target.MultiplicativeInverse(pol);
            Assert.AreEqual(expected, actual);
        }
    }

    /// <summary>
    /// Efectua testes à expansão p-ádica.
    /// </summary>
    [TestClass]
    public class PAdicExpansionTest
    {
        /// <summary>
        /// Testa a expansão p-ádica de inteiros.
        /// </summary>
        [TestMethod]
        public void IntegerPAdicExpansionTest()
        {
            var integerDomain = new IntegerDomain();
            var target = new PAdicAlgorithm<int, int>(
                integerDomain,
                (p, q, d) =>
                {
                    var lagAlg = new LagrangeAlgorithm<int>(d);
                    var res = lagAlg.Run(p, q);
                    return Tuple.Create(res.FirstFactor, res.SecondFactor);
                },
                integerDomain);

            var prime = 3;
            var order = 5;
            var value = new Fraction<int>(-1, 1, integerDomain);
            var actual = target.Run(value, prime, order);
            var expectedCoeff = prime - 1;
            for (var i = 0; i < order; ++i)
            {
                Assert.AreEqual(expectedCoeff, actual.Coefficients[i]);
            }

            Assert.AreEqual(value, actual.Remainder);

            Assert.AreEqual(value,
                actual.GetExpansionValue(integerDomain, integerDomain));

            prime = 5;
            value = new Fraction<int>(1, 17, integerDomain);
            actual = target.Run(value, prime, order);
            Assert.AreEqual(value,
                actual.GetExpansionValue(integerDomain, integerDomain));

            var actualNormalized = actual.GetNormalized(
                integerDomain,
                integerDomain,
                integerDomain);
            var expectedCoefficients = new int[5] { 3, 0, 4, 3, 1 };
            var index = 0;
            var powEnumerator = actualNormalized.Coefficients.GetEnumerator();
            while (powEnumerator.MoveNext())
            {
                var currPow = powEnumerator.Current.Key;
                while (index < currPow)
                {
                    Assert.AreEqual(expectedCoefficients[index++], 0);
                }

                Assert.AreEqual(expectedCoefficients[index++], powEnumerator.Current.Value);
            }

            Assert.AreEqual(
                value,
                actualNormalized.GetExpansionValue(integerDomain, integerDomain));

            prime = 5;
            value = new Fraction<int>(58, 1, integerDomain);
            actual = target.Run(value, prime, 10);
            actualNormalized = actual.GetNormalized(
                integerDomain,
                integerDomain,
                integerDomain);

            Assert.AreEqual(
                value,
                actualNormalized.GetExpansionValue(integerDomain, integerDomain));
        }
    }
}
