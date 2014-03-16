namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;
    using System.Collections;

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
            var multiplier = new FractionField<int, IntegerDomain>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<int,IntegerDomain>(13,17,domain), 
                new Fraction<int,IntegerDomain>(25,23,domain), 
                new Fraction<int,IntegerDomain>(11,15,domain), 
                new Fraction<int,IntegerDomain>(100,99,domain), 
                new Fraction<int,IntegerDomain>(1,2,domain), 
                new Fraction<int,IntegerDomain>(3,5,domain) };

            var expected = new[] { 
                new Fraction<int,IntegerDomain>(169,289,domain), 
                new Fraction<int,IntegerDomain>(15625,12167,domain), 
                new Fraction<int,IntegerDomain>(14641,50625,domain), 
                new Fraction<int,IntegerDomain>(100,99,domain), 
                new Fraction<int,IntegerDomain>(1,1048576,domain), 
                new Fraction<int,IntegerDomain>(1594323,1220703125,domain) };

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
            var multiplier = new FractionField<BigInteger, BigIntegerDomain>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<BigInteger,BigIntegerDomain>(13,17,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(25,23,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(11,15,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(100,99,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(1,2,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(3,5,domain) };

            var expected = new[] { 
                new Fraction<BigInteger,BigIntegerDomain>(169,289,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(15625,12167,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(14641,50625,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(100,99,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(1,1048576,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(1594323,1220703125,domain) };

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
            var multiplier = new FractionField<int, IntegerDomain>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<int,IntegerDomain>(13,17,domain), 
                new Fraction<int,IntegerDomain>(25,23,domain), 
                new Fraction<int,IntegerDomain>(11,15,domain), 
                new Fraction<int,IntegerDomain>(100,99,domain), 
                new Fraction<int,IntegerDomain>(1,2,domain), 
                new Fraction<int,IntegerDomain>(3,5,domain) };

            var expected = new[] { 
                new Fraction<int,IntegerDomain>(26,17,domain), 
                new Fraction<int,IntegerDomain>(75,23,domain), 
                new Fraction<int,IntegerDomain>(44,15,domain), 
                new Fraction<int,IntegerDomain>(100,99,domain), 
                new Fraction<int,IntegerDomain>(10,1,domain), 
                new Fraction<int,IntegerDomain>(39,5,domain) };

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
            var multiplier = new FractionField<BigInteger, BigIntegerDomain>(domain);

            var intPowers = new int[] { 2, 3, 4, 1, 20, 13 };
            var longPowers = new long[] { 2, 3, 4, 1, 20, 13 };
            var bigIntPowers = new BigInteger[] { 2, 3, 4, 1, 20, 13 };

            var values = new[] { 
                new Fraction<BigInteger,BigIntegerDomain>(13,17,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(25,23,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(11,15,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(100,99,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(1,2,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(3,5,domain) };

            var expected = new[] { 
                new Fraction<BigInteger,BigIntegerDomain>(26,17,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(75,23,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(44,15,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(100,99,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(10,1,domain), 
                new Fraction<BigInteger,BigIntegerDomain>(39,5,domain) };

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

        #endregion Outros Testes
    }
}
