﻿namespace Mathematics.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Testa as várias implementações dos números inteiros enormes.
    /// </summary>
    [TestClass]
    public class BigIntegersTest
    {
        /// <summary>
        /// A instância do contexto do teste.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Obtém ou atribui o contexto, o qual proporciona informação sobre o teste,
        /// e a funcionalidade para a execução actual do teste.
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

        /// <summary>
        /// Permite testar a validade das leituras dos números enormes
        /// representados por vectores de longos.
        /// </summary>
        [Description("Checks the validity of big integer parse.")]
        [TestMethod]
        public void UlongArrayBigInt_TryParseRightNumbersTest()
        {
            var texts = new string[]{
                "0",
                "1",
                "   9999999999999999999   ",
                "8888812345678901234",
                "11111111222222233333344444455555566666777778888899999",
                "-1",
                "-9999999999999999999   ",
                "   -8888812345678901234",
                "-11111111222222233333344444455555566666777778888899999"
            };

            for (int i = 0; i < texts.Length; ++i)
            {
                var currentText = texts[i];
                var target = default(UlongArrayBigInt);
                var status = UlongArrayBigInt.TryParse(currentText, out target);
                Assert.IsTrue(status);

                var actual = target.ToString();
                Assert.AreEqual(currentText.Trim(), actual);
            }
        }

        /// <summary>
        /// Permite testar a validade das leituras dos números enormes
        /// representados por vectores de longos.
        /// </summary>
        [Description("Asserts the big integers parse in case of invalid strings.")]
        [TestMethod]
        public void UlongArrayBigInt_TryParseWrongtNumbersTest()
        {
            // Texto vazio
            var wrongTexts = new[] { string.Empty, "-", "--1234", "-1-", "1 2", "123a" };
            for (int i = 0; i < wrongTexts.Length; ++i)
            {
                var target = default(UlongArrayBigInt);
                var status = UlongArrayBigInt.TryParse(wrongTexts[i], out target);
                Assert.IsFalse(status);
            }
        }

        /// <summary>
        /// Testa a função de soma de números enormes na versão paralela, recorrendo ao algoritmo CLA.
        /// </summary>
        [Description("Tests the parallel version of the CLA parallel adition function.")]
        [TestMethod]
        public void UlongArrayBigInt_ParallelClaAddTest()
        {
            var firstTexts = new[] { 
                "0", 
                "123", 
                "999999999999999999", 
                "1", 
                "0", 
                "-1112", 
                "-1111111111111111111111" };
            var secondTexts = new[] { 
                "-0", 
                "-123",
                "1111999999999999999999", 
                "1", 
                "1234567890123456789012345", 
                "-1111", 
                "2222222222222222222222" };
            var expectedText = new[] { 
                "0", 
                "0", 
                "1112999999999999999998", 
                "2", 
                "1234567890123456789012345", 
                "-2223", 
                "1111111111111111111111" };
            for (int i = 0; i < expectedText.Length; ++i)
            {
                var firstTarget = default(UlongArrayBigInt);
                if (UlongArrayBigInt.TryParse(firstTexts[i], out firstTarget))
                {
                    var secondTarget = default(UlongArrayBigInt);
                    if (UlongArrayBigInt.TryParse(secondTexts[i], out secondTarget))
                    {
                        var expected = default(UlongArrayBigInt);
                        if (UlongArrayBigInt.TryParse(expectedText[i], out expected))
                        {
                            var actual = UlongArrayBigInt.ParallelClaAdd(firstTarget, secondTarget);
                            Assert.AreEqual(expected, actual);
                        }
                        else
                        {
                            Assert.Fail("A problem has occurred while parsing the numbers.");
                        }
                    }
                    else
                    {
                        Assert.Fail("A problem has occurred while parsing the numbers.");
                    }
                }
                else
                {
                    Assert.Fail("A problem has occurred while parsing the numbers.");
                }
            }
        }

        /// <summary>
        /// Testa a função diferença entre números enormes na versão paralela, recorrendo ao algoritmo CLA.
        /// </summary>
        [Description("Tests the parallel version of CLA parallel subtrarction function.")]
        [TestMethod]
        public void UlongArrayBigInt_ParallelClaSubtractTest()
        {
            var firstTexts = new[] { 
                "0", 
                "123", 
                "999999999999999999", 
                "1", 
                "0", 
                "-1112", 
                "-1111111111111111111111" };
            var secondTexts = new[] { 
                "-0", 
                "-123",
                "1111999999999999999999", 
                "1", 
                "1234567890123456789012345", 
                "-1111", 
                "2222222222222222222222" };
            var expectedText = new[] { 
                "0", 
                "246", 
                "-1111000000000000000000", 
                "0", 
                "-1234567890123456789012345", 
                "-1", 
                "-3333333333333333333333" };
            for (int i = 0; i < expectedText.Length; ++i)
            {
                var firstTarget = default(UlongArrayBigInt);
                if (UlongArrayBigInt.TryParse(firstTexts[i], out firstTarget))
                {
                    var secondTarget = default(UlongArrayBigInt);
                    if (UlongArrayBigInt.TryParse(secondTexts[i], out secondTarget))
                    {
                        var expected = default(UlongArrayBigInt);
                        if (UlongArrayBigInt.TryParse(expectedText[i], out expected))
                        {
                            var actual = UlongArrayBigInt.ParallelClaSubtract(firstTarget, secondTarget);
                            Assert.AreEqual(expected, actual);
                        }
                        else
                        {
                            Assert.Fail("A problem has occurred while parsing the numbers.");
                        }
                    }
                    else
                    {
                        Assert.Fail("A problem has occurred while parsing the numbers.");
                    }
                }
                else
                {
                    Assert.Fail("A problem has occurred while parsing the numbers.");
                }
            }
        }

        #region Testes às funções internas

        /// <summary>
        /// Testa a função que permite adicionar dois números longos sem sinal, retornando o resultado
        /// na base 10^19.
        /// </summary>
        [Description("Tests the ulong numbers addition getting the result in base 10^19.")]
        [TestMethod]
        public void UlongArrayBigInt_DecimalRepAdd()
        {
            var target = new UlongArrayBigInt();
            var firstValues = new[] { 0xFFFFFFFFFFFFFFFF, 185948374650294856ul };
            var secondValues = new[] { 0xFFFFFFFFFFFFFFFF, 798574836475869405ul };
            var expected = new[] { 
                Tuple.Create(3ul, 6893488147419103230ul),
                Tuple.Create(0ul, 984523211126164261ul)};
            for (int i = 0; i < firstValues.Length; ++i)
            {
                var result = target.InternalDecimalRepAdd(firstValues[i], secondValues[i]);
                Assert.AreEqual(expected[i], result);
            }
        }

        /// <summary>
        /// Testa a adição de um longo sem sinal a uma lista de valores que contém a representação decimal de um número
        /// enorme.
        /// </summary>
        [Description("Tests the addition of an ulong to a list containing the decimal representation of a big number.")]
        [TestMethod]
        public void UlongArrayBigInt_DecimalRepAddValueToList()
        {
            var target = new UlongArrayBigInt();
            var firstValues = new[] { 
                new List<ulong>(){3584938574839482017ul, 5377562948593845950ul, 9843ul}};
            var secondValues = new[] { 789345789403948593ul };
            var expected = new[] { 
                new List<ulong>(){4374284364243430610ul, 5377562948593845950ul, 9843ul}};
            for (int i = 0; i < firstValues.Length; ++i)
            {
                target.InternalDecimalRepAdd(firstValues[i], secondValues[i]);

                // A função altera a lista que é passada como argumento
                CollectionAssert.AreEqual(expected[i], firstValues[i]);
            }
        }

        /// <summary>
        /// Testa a função que permite adicionar dois números longos sem sinal, retornando o resultado
        /// na base 10^19.
        /// </summary>
        [Description("Tests the ulong numbers multiplication getting the result in base 10^19.")]
        [TestMethod]
        public void UlongArrayBigInt_DecimalRepMultiply()
        {
            var target = new UlongArrayBigInt();
            var firstValues = new[] { 
                0xFFFFFFFFFFFFFFFF, 
                185948374650294856ul,
                5377562948593845950ul };
            var secondValues = new[] { 
                0xFFFFFFFFFFFFFFFF, 
                798574836475869405ul, 
                8446744073709551616ul };
            var expected = new[] { 
                Tuple.Create(3ul, 4028236692093846342ul, 6481119284349108225ul),
                Tuple.Create(0ul, 14849369287931291ul, 4387396512199280680ul),
                Tuple.Create(0ul, 4542289796703513044ul, 2754042671477555200ul)};
            for (int i = 0; i < firstValues.Length; ++i)
            {
                var result = target.InternalDecimalRepMultiply(firstValues[i], secondValues[i]);
                Assert.AreEqual(expected[i], result);
            }
        }

        /// <summary>
        /// Testa a multiplicação do número 2^64 por um número enorme representado por uma lista
        /// de longos sem sinal em base 10^19.
        /// </summary>
        [Description("Tests the ulong numbers multiplication getting the result in base 10^19.")]
        [TestMethod]
        public void UlongArrayBigInt_DecimalMultiplyByBinaryPower64()
        {
            var target = new UlongArrayBigInt();
            var values = new[] { 
                new List<ulong>(){3584938574839482017ul, 5377562948593845950ul, 9843ul}};
            var expected = new[] { 
                new List<ulong>(){2522926646765289472ul, 9367087112480853278ul, 1221770268413915282ul, 18158ul}};
            for (int i = 0; i < values.Length; ++i)
            {
                target.InternalDecimalMultiplyByBinaryPower(values[i]);

                // A função altera a lista que é passada como argumento
                CollectionAssert.AreEqual(expected[i], values[i]);
            }
        }

        #endregion Testes às funções internas

        #region Testes à sobrecarga de operadores para a classe UlongArrayBigInt

        /// <summary>
        /// Teste ao operador de igualdade entre números inteiros enormes.
        /// </summary>
        [Description("Tests the equality operator overload for big integers.")]
        [TestMethod]
        public void UlongArrayBigInt_TestEqualityOperator()
        {
            var firstTexts = new[] { "0", "123", "1234567890123456789012345", "1", "0", "-1111" };
            var secondTexts = new[] { "-0", "-123", "1234567890123456789012345", "1", "1234567890123456789012345", "-1111" };
            var expected = new[] { true, false, true, true, false, true };
            for (int i = 0; i < expected.Length; ++i)
            {
                var firstTarget = default(UlongArrayBigInt);
                if (UlongArrayBigInt.TryParse(firstTexts[i], out firstTarget))
                {
                    var secondTarget = default(UlongArrayBigInt);
                    if (UlongArrayBigInt.TryParse(secondTexts[i], out secondTarget))
                    {
                        var actual = firstTarget == secondTarget;
                        Assert.AreEqual(expected[i], actual);
                    }
                    else
                    {
                        Assert.Fail("A problem has occurred while parsing the numbers.");
                    }
                }
                else
                {
                    Assert.Fail("A problem has occurred while parsing the numbers.");
                }
            }
        }

        /// <summary>
        /// Teste ao operador de igualdade entre números inteiros enormes.
        /// </summary>
        [Description("Tests the less than operator overload for big integers.")]
        [TestMethod]
        public void UlongArrayBigInt_TestLessOperator()
        {
            var firstTexts = new[] { "0", "123", "999999999999999999", "1", "0", "-1112", "-1" };
            var secondTexts = new[] { "-0", "-123", "1111999999999999999999", "1", "1234567890123456789012345", "-1111", "0" };
            var expected = new[] { false, false, true, false, true, false, true };
            for (int i = 0; i < expected.Length; ++i)
            {
                var firstTarget = default(UlongArrayBigInt);
                if (UlongArrayBigInt.TryParse(firstTexts[i], out firstTarget))
                {
                    var secondTarget = default(UlongArrayBigInt);
                    if (UlongArrayBigInt.TryParse(secondTexts[i], out secondTarget))
                    {
                        var actual = firstTarget < secondTarget;
                        Assert.AreEqual(expected[i], actual);
                    }
                    else
                    {
                        Assert.Fail("A problem has occurred while parsing the numbers.");
                    }
                }
                else
                {
                    Assert.Fail("A problem has occurred while parsing the numbers.");
                }
            }
        }

        /// <summary>
        /// Testa a validade da sobrecarga do operador de adição de números inteiros enormes.
        /// </summary>
        [Description("Tests the addition operator overload for big integers.")]
        [TestMethod]
        public void UlongArrayBigInt_AddOperatorTest()
        {
            var firstTexts = new[] { 
                "0", 
                "123", 
                "999999999999999999", 
                "1", 
                "0", 
                "-1112", 
                "-1111111111111111111111" };
            var secondTexts = new[] { 
                "-0", 
                "-123",
                "1111999999999999999999", 
                "1", 
                "1234567890123456789012345", 
                "-1111", 
                "2222222222222222222222" };
            var expectedText = new[] { 
                "0", 
                "0", 
                "1112999999999999999998", 
                "2", 
                "1234567890123456789012345", 
                "-2223", 
                "1111111111111111111111" };
            for (int i = 0; i < expectedText.Length; ++i)
            {
                var firstTarget = default(UlongArrayBigInt);
                if (UlongArrayBigInt.TryParse(firstTexts[i], out firstTarget))
                {
                    var secondTarget = default(UlongArrayBigInt);
                    if (UlongArrayBigInt.TryParse(secondTexts[i], out secondTarget))
                    {
                        var expected = default(UlongArrayBigInt);
                        if (UlongArrayBigInt.TryParse(expectedText[i], out expected))
                        {
                            var actual = firstTarget + secondTarget;
                            Assert.AreEqual(expected, actual);
                        }
                        else
                        {
                            Assert.Fail("A problem has occurred while parsing the numbers.");
                        }
                    }
                    else
                    {
                        Assert.Fail("A problem has occurred while parsing the numbers.");
                    }
                }
                else
                {
                    Assert.Fail("A problem has occurred while parsing the numbers.");
                }
            }
        }

        /// <summary>
        /// Testa a validade da sobrecarga do operador de multiplicação de números inteiros enormes.
        /// </summary>
        [Description("Tests the multiplication operator overload for big integers.")]
        [TestMethod]
        public void UlongArrayBigInt_MultiplyOperatorTest()
        {
            var firstTests = new[]{
                "0",
                "999999999999999999",
                "123456789012345678901234567890",
                "-123098765412309876541230987654"
            };
            var secondTests = new[]{
                "-1",
                "999999999999999999",
                "-120398471209384719238",
                "-341019084571029843571023948"
            };
            var expectedValues = new[]{
                "0",
                "999999999999999998000000000000000001",
                "-14864008657505984968540605278839196519227900067820",
                "41979028292729865179421161730533936344764861672126337992"
            };

            for (int i = 0; i < firstTests.Length; ++i)
            {
                var firstTarget = default(UlongArrayBigInt);
                if (UlongArrayBigInt.TryParse(firstTests[i], out firstTarget))
                {
                    var secondTarget = default(UlongArrayBigInt);
                    if (UlongArrayBigInt.TryParse(secondTests[i], out secondTarget))
                    {
                        var expected = default(UlongArrayBigInt);
                        if (UlongArrayBigInt.TryParse(expectedValues[i], out expected))
                        {
                            var actual = firstTarget * secondTarget;
                            Assert.AreEqual(expected, actual);
                        }
                        else
                        {
                            Assert.Fail("A problem has occurred while parsing the numbers.");
                        }
                    }
                    else
                    {
                        Assert.Fail("A problem has occurred while parsing the numbers.");
                    }
                }
                else
                {
                    Assert.Fail("A problem has occurred while parsing the numbers.");
                }
            }
        }

        /// <summary>
        /// Testa a validade da sobrecarga do operador de diferença entre números inteiros enormes.
        /// </summary>
        [Description("Tests the subtraction operator overload for big integers.")]
        [TestMethod]
        public void UlongArrayBigInt_SubtractOperatorTest()
        {
            var firstTexts = new[] { 
                "0", 
                "123", 
                "999999999999999999", 
                "1", 
                "0", 
                "-1112", 
                "-1111111111111111111111" };
            var secondTexts = new[] { 
                "-0", 
                "-123",
                "1111999999999999999999", 
                "1", 
                "1234567890123456789012345", 
                "-1111", 
                "2222222222222222222222" };
            var expectedText = new[] { 
                "0", 
                "246", 
                "-1111000000000000000000", 
                "0", 
                "-1234567890123456789012345", 
                "-1", 
                "-3333333333333333333333" };
            for (int i = 0; i < expectedText.Length; ++i)
            {
                var firstTarget = default(UlongArrayBigInt);
                if (UlongArrayBigInt.TryParse(firstTexts[i], out firstTarget))
                {
                    var secondTarget = default(UlongArrayBigInt);
                    if (UlongArrayBigInt.TryParse(secondTexts[i], out secondTarget))
                    {
                        var expected = default(UlongArrayBigInt);
                        if (UlongArrayBigInt.TryParse(expectedText[i], out expected))
                        {
                            var actual = firstTarget - secondTarget;
                            Assert.AreEqual(expected, actual);
                        }
                        else
                        {
                            Assert.Fail("A problem has occurred while parsing the numbers.");
                        }
                    }
                    else
                    {
                        Assert.Fail("A problem has occurred while parsing the numbers.");
                    }
                }
                else
                {
                    Assert.Fail("A problem has occurred while parsing the numbers.");
                }
            }
        }

        /// <summary>
        /// Testa a validade da sobrecarga do operador de rotação à esquerda para números inteiros enormes.
        /// </summary>
        [TestMethod]
        public void UlongArrayBigInt_TestRotateLeftOperator()
        {
            var firstNumbers = new[] { new UlongArrayBigInt(1) };
            var secondNumbers = new[] { 200 };
            var expected = new[] { new UlongArrayBigInt(new[] { 0ul, 0ul, 0ul, 256ul }) };
            for (int i = 0; i < expected.Length; ++i)
            {
                var actual = firstNumbers[i] << secondNumbers[i];
                Assert.AreEqual(expected[i], actual);
            }
        }

        /// <summary>
        /// Testa a validade da sobrecarga do operador de rotação à direita para números inteiros enormes.
        /// </summary>
        [TestMethod]
        public void UlongArrayBigInt_TestRotateRightOperator()
        {
            var firstNumbers = new[] { new UlongArrayBigInt(new[] { 0ul, 0ul, 0ul, 256ul }) };
            var secondNumbers = new[] { 200 };
            var expected = new[] { new UlongArrayBigInt(1) };
            for (int i = 0; i < expected.Length; ++i)
            {
                var actual = firstNumbers[i] >> secondNumbers[i];
                Assert.AreEqual(expected[i], actual);
            }
        }

        #endregion Testes à sobrecarga de operadores para a classe UlongArrayBigInt
    }
}
