namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;
    using System.Collections;
    using Utilities;

    /// <summary>
    /// Testa as funções matemáticas simples.
    /// </summary>
    [TestClass()]
    public class MathFunctionsTest
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Obtém e atribui o contexto do teste, o qual fornece informação associada à execução
        /// dos testes definidos.
        ///</summary>
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

        #region Testes às potências

        [TestMethod()]
        public void PowerTest_Integer()
        {
            var multiplier = new IntegerDomain();

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 13, 25, 11, 100, 2, 3 };
            var expected = new[] { 169, 15625, 14641, 100, 1048576, 1594323 };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências genéricas.
            var integerNumber = new IntegerDomain();
            var longNumber = new LongDomain();
            var bigIntegerNumber = new BigIntegerDomain();

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier, integerNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier, longNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier, bigIntegerNumber);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void PowerTest_BigInteger()
        {
            var multiplier = new BigIntegerDomain();

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new BigInteger[] { 13, 25, 11, 100, 2, 3 };
            var expected = new BigInteger[] { 169, 15625, 14641, 100, 1048576, 1594323 };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências genéricas.
            var integerNumber = new IntegerDomain();
            var longNumber = new LongDomain();
            var bigIntegerNumber = new BigIntegerDomain();

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier, integerNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier, longNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier, bigIntegerNumber);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void PowerTest_IntegerFraction()
        {
            var domain = new IntegerDomain();
            var multiplier = new FractionField<int>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<int>(13,17,domain), 
                new Fraction<int>(25,23,domain), 
                new Fraction<int>(11,15,domain), 
                new Fraction<int>(100,99,domain), 
                new Fraction<int>(1,2,domain), 
                new Fraction<int>(3,5,domain) };

            var expected = new[] { 
                new Fraction<int>(169,289,domain), 
                new Fraction<int>(15625,12167,domain), 
                new Fraction<int>(14641,50625,domain), 
                new Fraction<int>(100,99,domain), 
                new Fraction<int>(1,1048576,domain), 
                new Fraction<int>(1594323,1220703125,domain) };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências genéricas.
            var integerNumber = new IntegerDomain();
            var longNumber = new LongDomain();
            var bigIntegerNumber = new BigIntegerDomain();

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier, integerNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier, longNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier, bigIntegerNumber);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void PowerTest_BigIntegerFraction()
        {
            var domain = new BigIntegerDomain();
            var multiplier = new FractionField<BigInteger>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<BigInteger>(13,17,domain), 
                new Fraction<BigInteger>(25,23,domain), 
                new Fraction<BigInteger>(11,15,domain), 
                new Fraction<BigInteger>(100,99,domain), 
                new Fraction<BigInteger>(1,2,domain), 
                new Fraction<BigInteger>(3,5,domain) };

            var expected = new[] { 
                new Fraction<BigInteger>(169,289,domain), 
                new Fraction<BigInteger>(15625,12167,domain), 
                new Fraction<BigInteger>(14641,50625,domain), 
                new Fraction<BigInteger>(100,99,domain), 
                new Fraction<BigInteger>(1,1048576,domain), 
                new Fraction<BigInteger>(1594323,1220703125,domain) };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências genéricas.
            var integerNumber = new IntegerDomain();
            var longNumber = new LongDomain();
            var bigIntegerNumber = new BigIntegerDomain();

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], intPowers[i], multiplier, integerNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], longPowers[i], multiplier, longNumber);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.Power(values[i], bigIntPowers[i], multiplier, bigIntegerNumber);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void PowerTest_IntegerPolynomial()
        {
            var integerDomain = new IntegerDomain();
            var longDomain = new LongDomain();
            var bigIntegerDomain = new BigIntegerDomain();

            var variableName = "x";
            var intPolDomain = new UnivarPolynomRing<int>(variableName, integerDomain);
            var longPolDomain = new UnivarPolynomRing<long>(variableName, longDomain);
            var bigIntegerPolDomain = new UnivarPolynomRing<BigInteger>(variableName, bigIntegerDomain);

            // Leitores
            var integerParser = new IntegerParser<string>();
            var longParser = new LongParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();

            var integerConversion = new ElementToElementConversion<int>();
            var longToIntegerConversion = new LongToIntegerConversion();
            var bigIntegerToIntegerConversion = new BigIntegerToIntegerConversion();

            var intPowers = new int[3] { 2, 3, 4};
            var longPowers = new long[3] { 2, 3, 4 };
            var bigIntPowers = new BigInteger[3] { 2, 3, 4};

            var polynomialsTexts = new string[3] { "x^3-2*x^2+3*x-1", "2*x^2+4*x+4", "x+1" };
            var expectedPolinomialsTexts = new string[3] { 
                "x^6-4*x^5+10*x^4-14*x^3+13*x^2-6*x+1", 
                "8*x^6+48*x^5+144*x^4+256*x^3+288*x^2+192*x+64", 
                "x^4+4*x^3+6*x^2+4*x+1" };

            // Coeficientes inteiros.
            for (int i = 0; i < 3; ++i)
            {
                var polynomialValue = TestsHelper.ReadUnivarPolynomial(
                    polynomialsTexts[i], 
                    integerDomain, 
                    integerParser, 
                    integerConversion, 
                    variableName);

                var expectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                    expectedPolinomialsTexts[i],
                    integerDomain,
                    integerParser,
                    integerConversion,
                    variableName);

                var actualPolynomial = MathFunctions.Power(polynomialValue, intPowers[i], intPolDomain);
                Assert.AreEqual(expectedPolynomial, actualPolynomial);
            }

            // Coeficientes longos.
            for (int i = 0; i < 3; ++i)
            {
                var polynomialValue = TestsHelper.ReadUnivarPolynomial(
                    polynomialsTexts[i],
                    longDomain,
                    longParser,
                    longToIntegerConversion,
                    variableName);

                var expectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                    expectedPolinomialsTexts[i],
                    longDomain,
                    longParser,
                    longToIntegerConversion,
                    variableName);

                var actualPolynomial = MathFunctions.Power(polynomialValue, intPowers[i], longPolDomain);
                Assert.AreEqual(expectedPolynomial, actualPolynomial);
            }

            // Coeficientes correspondentes a inteiros de precisão arbitrária.
            for (int i = 0; i < 3; ++i)
            {
                var polynomialValue = TestsHelper.ReadUnivarPolynomial(
                    polynomialsTexts[i],
                    bigIntegerDomain,
                    bigIntegerParser,
                    bigIntegerToIntegerConversion,
                    variableName);

                var expectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                    expectedPolinomialsTexts[i],
                    bigIntegerDomain,
                    bigIntegerParser,
                    bigIntegerToIntegerConversion,
                    variableName);

                var actualPolynomial = MathFunctions.Power(polynomialValue, intPowers[i], bigIntegerPolDomain);
                Assert.AreEqual(expectedPolynomial, actualPolynomial);
            }
        }

        [TestMethod()]
        public void PowerTest_FractionPolynomial()
        {
            var integerDomain = new IntegerDomain();
            var longDomain = new LongDomain();
            var bigIntegerDomain = new BigIntegerDomain();

            var variableName = "x";
            var integerPolynomialField = new UnivarPolynomEuclideanDomain<Fraction<int>>(
                variableName, 
                new FractionField<int>(integerDomain));
            var longPolynomialField = new UnivarPolynomEuclideanDomain<Fraction<long>>(
                variableName,
                new FractionField<long>(longDomain));
            var bigIntegerPolynomialField = new UnivarPolynomEuclideanDomain<Fraction<BigInteger>>(
                variableName,
                new FractionField<BigInteger>(bigIntegerDomain));

            // Leitores
            var integerParser = new IntegerParser<string>();
            var longParser = new LongParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();

            var integerConversion = new OuterElementFractionConversion<int, int>(new ElementToElementConversion<int>(), integerDomain);
            var longToIntegerConversion = new OuterElementFractionConversion<int, long>(new LongToIntegerConversion(), longDomain);
            var bigIntegerToIntegerConversion = new OuterElementFractionConversion<int, BigInteger>(new BigIntegerToIntegerConversion(), bigIntegerDomain);

            var intPowers = new int[3] { 2, 3, 4 };
            var longPowers = new long[3] { 2, 3, 4 };
            var bigIntPowers = new BigInteger[3] { 2, 3, 4 };

            var polynomialsTexts = new string[3] { "1/3*x^3-2/3*x^2+3/2*x-1/2", "2*x^2+4/3*x+4/9", "7/5*x+1" };
            var expectedPolinomialsTexts = new string[3] { 
                "1/9*x^6-4/9*x^5+13/9*x^4-7/3*x^3+35/12*x^2-3/2*x+1/4", 
                "8*x^6+16*x^5+16*x^4+256/27*x^3+32/9*x^2+64/81*x+64/729", 
                "2401/625*x^4+1372/125*x^3+294/25*x^2+28/5*x+1" };

            // Coeficientes inteiros.
            for (int i = 0; i < 3; ++i)
            {
                var polynomialValue = TestsHelper.ReadFractionalCoeffsUnivarPol(
                    polynomialsTexts[i],
                    integerDomain,
                    integerParser,
                    integerConversion,
                    variableName);

                var expectedPolynomial = TestsHelper.ReadFractionalCoeffsUnivarPol(
                    expectedPolinomialsTexts[i],
                    integerDomain,
                    integerParser,
                    integerConversion,
                    variableName);

                var actualPolynomial = MathFunctions.Power(polynomialValue, intPowers[i], integerPolynomialField);
                Assert.AreEqual(expectedPolynomial, actualPolynomial);
            }

            // Coeficientes longos.
            for (int i = 0; i < 3; ++i)
            {
                var polynomialValue = TestsHelper.ReadFractionalCoeffsUnivarPol(
                    polynomialsTexts[i],
                    longDomain,
                    longParser,
                    longToIntegerConversion,
                    variableName);

                var expectedPolynomial = TestsHelper.ReadFractionalCoeffsUnivarPol(
                    expectedPolinomialsTexts[i],
                    longDomain,
                    longParser,
                    longToIntegerConversion,
                    variableName);

                var actualPolynomial = MathFunctions.Power(polynomialValue, intPowers[i], longPolynomialField);
                Assert.AreEqual(expectedPolynomial, actualPolynomial);
            }

            // Coeficientes correspondentes a inteiros de precisão arbitrária.
            for (int i = 0; i < 3; ++i)
            {
                var polynomialValue = TestsHelper.ReadFractionalCoeffsUnivarPol(
                    polynomialsTexts[i],
                    bigIntegerDomain,
                    bigIntegerParser,
                    bigIntegerToIntegerConversion,
                    variableName);

                var expectedPolynomial = TestsHelper.ReadFractionalCoeffsUnivarPol(
                    expectedPolinomialsTexts[i],
                    bigIntegerDomain,
                    bigIntegerParser,
                    bigIntegerToIntegerConversion,
                    variableName);

                var actualPolynomial = MathFunctions.Power(polynomialValue, intPowers[i], bigIntegerPolynomialField);
                Assert.AreEqual(expectedPolynomial, actualPolynomial);
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(MathematicsException))]
        public void PowerTest_IntegerException()
        {
            MathFunctions.Power(1, -1, new IntegerDomain());
        }

        [TestMethod()]
        [ExpectedException(typeof(MathematicsException))]
        public void PowerTest_LongException()
        {
            MathFunctions.Power(1, -1L, new IntegerDomain());
        }

        [TestMethod()]
        [ExpectedException(typeof(MathematicsException))]
        public void PowerTest_BigIntegerException()
        {
            MathFunctions.Power(1, new BigInteger(-1), new IntegerDomain());
        }

        #endregion Testes às potências

        #region Testes às funções de adição repetida

        [TestMethod()]
        public void AddPowerTest_Integer()
        {
            var multiplier = new IntegerDomain();

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 13, 25, 11, 100, 2, 3 };
            var expected = new[] { 26, 75, 44, 100, 40, 39 };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void AddPowerTest_BigInteger()
        {
            var multiplier = new BigIntegerDomain();

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new BigInteger[] { 13, 25, 11, 100, 2, 3 };
            var expected = new BigInteger[] { 26, 75, 44, 100, 40, 39 };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void AddPowerTest_IntegerFraction()
        {
            var domain = new IntegerDomain();
            var multiplier = new FractionField<int>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<int>(13,17,domain), 
                new Fraction<int>(25,23,domain), 
                new Fraction<int>(11,15,domain), 
                new Fraction<int>(100,99,domain), 
                new Fraction<int>(1,2,domain), 
                new Fraction<int>(3,5,domain) };

            var expected = new[] { 
                new Fraction<int>(26,17,domain), 
                new Fraction<int>(75,23,domain), 
                new Fraction<int>(44,15,domain), 
                new Fraction<int>(100,99,domain), 
                new Fraction<int>(10,1,domain), 
                new Fraction<int>(39,5,domain) };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void AddPowerTest_BigIntegerFraction()
        {
            var domain = new BigIntegerDomain();
            var multiplier = new FractionField<BigInteger>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<BigInteger>(13,17,domain), 
                new Fraction<BigInteger>(25,23,domain), 
                new Fraction<BigInteger>(11,15,domain), 
                new Fraction<BigInteger>(100,99,domain), 
                new Fraction<BigInteger>(1,2,domain), 
                new Fraction<BigInteger>(3,5,domain) };

            var expected = new[] { 
                new Fraction<BigInteger>(26,17,domain), 
                new Fraction<BigInteger>(75,23,domain), 
                new Fraction<BigInteger>(44,15,domain), 
                new Fraction<BigInteger>(100,99,domain), 
                new Fraction<BigInteger>(10,1,domain), 
                new Fraction<BigInteger>(39,5,domain) };

            // Potências inteiras.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], intPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências longas
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], longPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }

            // Potências com inteiros grandes.
            for (int i = 0; i < intPowers.Length; ++i)
            {
                var actual = MathFunctions.AddPower(values[i], bigIntPowers[i], multiplier);
                Assert.AreEqual(expected[i], actual);
            }
        }

        #endregion Testes às funções de adição repetidas

        #region Testes ao Máximo Divisor Comum

        [TestMethod()]
        public void GreatCommonDivisorTest_IntegerNumber()
        {
            var domain = new IntegerDomain();
            var firstValues = new[] { 6, 48, 3251, 100, 3528, 427 };
            var secondValues = new[] { 12, 36, 4525, 75, 4116, 254 };
            var expected = new[] { 6, 12, 1, 25, 588, 1 };

            for (int i = 0; i < firstValues.Length; ++i)
            {
                var actual = MathFunctions.GreatCommonDivisor(firstValues[i], secondValues[i], domain);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void GreatCommonDivisorTest_LongNumber()
        {
            var domain = new LongDomain();
            var firstValues = new long[] { 6, 48, 3251, 100, 3528, 427 };
            var secondValues = new long[] { 12, 36, 4525, 75, 4116, 254 };
            var expected = new long[] { 6, 12, 1, 25, 588, 1 };

            for (int i = 0; i < firstValues.Length; ++i)
            {
                var actual = MathFunctions.GreatCommonDivisor(firstValues[i], secondValues[i], domain);
                Assert.AreEqual(expected[i], actual);
            }
        }

        [TestMethod()]
        public void GreatCommonDivisorTest_BigIntegerNumber()
        {
            var domain = new BigIntegerDomain();
            var firstValues = new BigInteger[] { 6, 48, 3251, 100, 3528, 427 };
            var secondValues = new BigInteger[] { 12, 36, 4525, 75, 4116, 254 };
            var expected = new BigInteger[] { 6, 12, 1, 25, 588, 1 };

            for (int i = 0; i < firstValues.Length; ++i)
            {
                var actual = MathFunctions.GreatCommonDivisor(firstValues[i], secondValues[i], domain);
                Assert.AreEqual(expected[i], actual);
            }
        }

        #endregion Testes ao Máximo Divisor Comum

        #region Outros Testes

        /// <summary>
        /// Permite testar a função que permite contar o número de "bits" num valor.
        /// </summary>
        [TestMethod()]
        public void PopCountTest()
        {
            var values = new uint[] { 0x0, 0x2, 0x200, 0xA, 0xAAB1, 0x1435, 0xFFFF };
            var expected = new int[] { 0, 1, 1, 2, 8, 6, 16 };
            for (int i = 0; i < values.Length; ++i)
            {
                var actual = MathFunctions.PopCount(values[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }

        /// <summary>
        /// Permite testar a função que determina o índice do "bit" mais alto que se encontra activo.
        /// </summary>
        [TestMethod()]
        public void GetHighestSettedBitIndexTest()
        {
            var values = new ulong[] { 0, 1, 0xFFFFFFFFFFFFFFFF, 10, 0xA00000000000000 };
            var expected = new[] { -1, 0, 63, 3, 59 };
            for (int i = 0; i < values.Length; ++i)
            {
                var actual = MathFunctions.GetHighestSettedBitIndex(values[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }

        #endregion Outros Testes
    }
}
