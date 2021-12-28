namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using Utilities;

    [TestClass()]
    public class FiniteFieldPolFactorizationAlgorithmTest
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
        public void RunTest_TestFactors1()
        {
            var polText = "x^3+10*x^2-432*x+5040";
            var variableName = "x";
            var prime = 31;

            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var integerConversion = new ElementToElementConversion<int>();

            // Faz a leitura do polinómio.
            var pol = TestsHelper.ReadUnivarPolynomial(
                polText,
                integerDomain,
                integerParser,
                integerConversion,
                variableName);

            // Testa os factores.
            var integerModule = new ModularIntegerField(prime);
            var finiteFieldPolAlg = new FiniteFieldPolFactorizationAlgorithm<int>(
                new DenseCondensationLinSysAlgorithm<int>(integerModule),
                integerDomain);
            var result = finiteFieldPolAlg.Run(pol, integerModule);

            var factorsEnumerator = result.Factors.GetEnumerator();
            if (factorsEnumerator.MoveNext())
            {
                var expected = factorsEnumerator.Current;
                while (factorsEnumerator.MoveNext())
                {
                    expected = expected.Multiply(factorsEnumerator.Current, integerModule);
                }

                expected = expected.Multiply(result.IndependentCoeff, integerModule);
                Assert.AreEqual(expected, pol.ApplyFunction(coeff => this.GetSymmetricRemainder(coeff, prime), integerModule));
            }
            else
            {
                Assert.Fail("At least the main polynomial may be regarded as a factor.");
            }
        }

        [TestMethod()]
        public void RunTest_TestFactors2()
        {
            var polText = "x^3+2";
            var variableName = "x";
            var prime = 5;

            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var integerConversion = new ElementToElementConversion<int>();

            // Faz a leitura do polinómio.
            var pol = TestsHelper.ReadUnivarPolynomial(
                polText,
                integerDomain,
                integerParser,
                integerConversion,
                variableName);

            // Testa os factores.
            var integerModule = new ModularIntegerField(prime);
            var finiteFieldPolAlg = new FiniteFieldPolFactorizationAlgorithm<int>(
                new DenseCondensationLinSysAlgorithm<int>(integerModule),
                integerDomain);
            var result = finiteFieldPolAlg.Run(pol, integerModule);

            var factorsEnumerator = result.Factors.GetEnumerator();
            if (factorsEnumerator.MoveNext())
            {
                var expected = factorsEnumerator.Current;
                while (factorsEnumerator.MoveNext())
                {
                    expected = expected.Multiply(factorsEnumerator.Current, integerModule);
                }

                expected = expected.Multiply(result.IndependentCoeff, integerModule);
                Assert.AreEqual(expected, pol.ApplyFunction(coeff => this.GetSymmetricRemainder(coeff, prime), integerModule));
            }
            else
            {
                Assert.Fail("At least the main polynomial may be regarded as a factor.");
            }
        }

        [TestMethod()]
        public void RunTest_TestFactors3()
        {
            var polText = "x^8+x^7+x^4+x^3+x+1";
            var variableName = "x";
            var prime = 3;

            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var integerConversion = new ElementToElementConversion<int>();

            // Faz a leitura do polinómio.
            var pol = TestsHelper.ReadUnivarPolynomial(
                polText,
                integerDomain,
                integerParser,
                integerConversion,
                variableName);

            // Testa os factores.
            var integerModule = new ModularIntegerField(prime);
            var finiteFieldPolAlg = new FiniteFieldPolFactorizationAlgorithm<int>(
                new DenseCondensationLinSysAlgorithm<int>(integerModule),
                integerDomain);
            var result = finiteFieldPolAlg.Run(pol, integerModule);

            // Factores esperados
            var expected = new List<UnivariatePolynomialNormalForm<int>>();
            expected.Add(
                TestsHelper.ReadUnivarPolynomial(
                    "x+1",
                    integerDomain,
                    integerParser,
                    integerConversion,
                    variableName)
            );
            expected.Add(
                TestsHelper.ReadUnivarPolynomial(
                    "x+2",
                    integerDomain,
                    integerParser,
                    integerConversion,
                    variableName)
            );
            expected.Add(
                TestsHelper.ReadUnivarPolynomial(
                    "2+2*x+2*x^2+x^3+x^4+x^5+x^6",
                    integerDomain,
                    integerParser,
                    integerConversion,
                    variableName)
            );

            var actual = new List<UnivariatePolynomialNormalForm<int>>(
                result.Factors);

            CollectionAssert.AreEquivalent(expected, actual);
        }

        private int GetSymmetricRemainder(int coeff, int modulus)
        {
            var remainder = coeff % modulus;
            if (remainder < 0)
            {
                remainder += modulus;
            }

            return remainder;
        }
    }
}
