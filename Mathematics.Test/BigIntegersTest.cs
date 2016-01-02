namespace Mathematics.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
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
                            Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                        }
                    }
                    else
                    {
                        Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                    }
                }
                else
                {
                    Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                }
            }
        }

        /// <summary>
        /// Testa a função diferença entre números enormes na versão paralela, recorrendo ao algoritmo CLA.
        /// </summary>
        [Description("Tests the parallel version of CLA parallel subtraction function.")]
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
                            Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                        }
                    }
                    else
                    {
                        Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                    }
                }
                else
                {
                    Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                }
            }
        }

        /// <summary>
        /// Testa a função que permite determinar o quociente e o resto de dois números do algoritmo sequencial.
        /// </summary>
        [Description("Test the sequential implementation of the sequential quotient and remainder function.")]
        [TestMethod]
        public void UlongArrayBigInt_SequentialQuotientAndRemainderTest()
        {
            // O dividendo é zero e o divisor diferente de zero
            var dividend = new UlongArrayBigInt(0);
            var divisor = new UlongArrayBigInt(new[] { 1ul, ulong.MaxValue });
            var result = UlongArrayBigInt.SequentialQuotientAndRemainder(dividend, divisor);
            Assert.AreEqual(dividend, result.Item1);
            Assert.AreEqual(dividend, result.Item2);

            // O dividendo tem um número inferior de elementos relativamente ao divisor
            dividend = new UlongArrayBigInt(new[] { 1ul, 2ul, 3ul });
            divisor = new UlongArrayBigInt(new[] { 0ul, 0ul, 0ul, 0ul, 0ul, 1ul });
            result = UlongArrayBigInt.SequentialQuotientAndRemainder(dividend, divisor);
            Assert.AreEqual(UlongArrayBigInt.Zero, result.Item1);
            Assert.AreEqual(dividend, result.Item2);

            // O dividendo tem o mesmo número de elementos que o divisor mas constitui um número inferior
            dividend = new UlongArrayBigInt(new[] { 1ul, 50ul, 3ul, 234ul });
            divisor = new UlongArrayBigInt(new[] { 2ul, 50ul, 3ul, 234ul });
            result = UlongArrayBigInt.SequentialQuotientAndRemainder(dividend, divisor);
            Assert.AreEqual(UlongArrayBigInt.Zero, result.Item1);
            Assert.AreEqual(dividend, result.Item2);

            // O dividendo é igual ao divisor
            dividend = new UlongArrayBigInt(new[] { 1ul, 50ul, 3ul, 234ul });
            divisor = new UlongArrayBigInt(new[] { 1ul, 50ul, 3ul, 234ul });
            result = UlongArrayBigInt.SequentialQuotientAndRemainder(dividend, divisor);
            Assert.AreEqual(UlongArrayBigInt.Unity, result.Item1);
            Assert.AreEqual(UlongArrayBigInt.Zero, result.Item2);

            // O dividendo é superior ao divisor mas possui o mesmo logaritmo inteiro
            dividend = new UlongArrayBigInt(new[] { 1ul, 50ul, 10ul, 234ul });
            divisor = new UlongArrayBigInt(new[] { 1ul, 500ul, 9ul, 234ul });
            result = UlongArrayBigInt.SequentialQuotientAndRemainder(dividend, divisor);
            var expectedRemainder = new UlongArrayBigInt(new[] { 0ul, 18446744073709551166ul });
            Assert.AreEqual(UlongArrayBigInt.Unity, result.Item1);
            Assert.AreEqual(expectedRemainder, result.Item2);

            // O dividendo contém o mesmo número de elementos e é divisível pelo divisor
            dividend = new UlongArrayBigInt(new[] { 1024ul, 512000ul, 9216ul, 239616ul });
            divisor = new UlongArrayBigInt(new[] { 1ul, 500ul, 9ul, 234ul });
            result = UlongArrayBigInt.SequentialQuotientAndRemainder(dividend, divisor);
            var expectedQuo = new UlongArrayBigInt(1024);
            Assert.AreEqual(expectedQuo, result.Item1);
            Assert.AreEqual(UlongArrayBigInt.Zero, result.Item2);

            // Divisão geral onde o logaritmo do dividendo é superior ao divisor mas não é divisível por este,
            // contendo ambos o mesmo número de elementos
            dividend = new UlongArrayBigInt(new[] { ulong.MaxValue, ulong.MaxValue, ulong.MaxValue });
            divisor = new UlongArrayBigInt(new[] { 3ul, 3ul, 1ul });
            result = UlongArrayBigInt.SequentialQuotientAndRemainder(dividend, divisor);
            expectedQuo = new UlongArrayBigInt(new[] { 18446744073709551613ul });
            expectedRemainder = new UlongArrayBigInt(new[] { 8ul, 6ul });
            Assert.AreEqual(expectedQuo, result.Item1);
            Assert.AreEqual(expectedRemainder, result.Item2);

            // Assert.Inconclusive("Ainda não foi implementado qualquer teste.");
        }

        /// <summary>
        /// Testa a divisão por zero na função quociente e resto de dois números.
        /// </summary>
        [Description("Test the devide by zero exception in sequential quotient and remainder function.")]
        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void UlongArrayBigInt_SequentialQuotientAndRemainderDivideByZeroTest()
        {
            var firstTextValue = "423481503948512039485938440895732";
            var secondTextValue = new UlongArrayBigInt(0);
            var firstValue = default(UlongArrayBigInt);
            if (UlongArrayBigInt.TryParse(firstTextValue, out firstValue))
            {
                var result = UlongArrayBigInt.SequentialQuotientAndRemainder(
                    firstValue,
                    new UlongArrayBigInt(0));
            }
            else
            {
                Assert.Inconclusive("An error has occured while reading the first value.");
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
                //0xFFFFFFFFFFFFFFFF, 
                //185948374650294856ul,
                5377562948593845950ul };
            var secondValues = new[] { 
                //0xFFFFFFFFFFFFFFFF, 
                //798574836475869405ul, 
                8446744073709551616ul };
            var expected = new[] { 
                //Tuple.Create(3ul, 4028236692093846342ul, 6481119284349108225ul),
                //Tuple.Create(0ul, 14849369287931291ul, 4387396512199280680ul),
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

        /// <summary>
        /// Testa a função que permite determinar o quociente e o resto da divisão de um número
        /// de dois símbolos por um número de um símbolo na base 2^64.
        /// </summary>
        [Description("Tests the division of a two symbol big integer number by a single symbol number in base 2^64")]
        [TestMethod]
        public void UlongArrayBigInt_Divide()
        {
            Assert.Inconclusive("The function is under development.");
            var highValues = new[] { 
                235624ul, 
                18446744073709551614ul,
                1ul
            };
            var lowValues = new[] { 
                1758473049583920574ul, 
                18446744073709551615ul,
                35849304958289340ul
            };
            var quotients = new[] { 
                1839203948219038477ul, 
                18446744073709551615ul,
                12345ul
            };
            var expected = new[]{
                Tuple.Create(2363249ul,492672094512157185ul),
                Tuple.Create(18446744073709551615ul,18446744073709551614ul),
                Tuple.Create(1497172408154543ul,7621ul)
            };

            for (var i = 0; i < highValues.Length; ++i)
            {
                var actual = UlongArrayBigInt.InternalDivide(
                    highValues[i],
                    lowValues[i],
                    quotients[i]);
                Assert.AreEqual(expected[i], actual);
            }
        }

        /// <summary>
        /// Testa a função que permite rodar à direita um vector de valores, mantendo
        /// fixo o seu tamanho.
        /// </summary>
        /// <remarks>
        /// A rotação à direita é relativa ao número representado pelo vector, ito é,
        /// a rotação é tal que o número resultante advém dividido por dois.
        /// </remarks>
        [TestMethod]
        public void UlongArrayBigInt_FixedLengthRotateRight()
        {
            var vectorToRotate = new ulong[] { 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101 };

            // Rotação de um número inferior ao tamanho da variável
            var places = 16;
            var currentVector = new ulong[vectorToRotate.Length];
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            var expected = new ulong[]{ 
                0xDCBAABCDEFABCDEF, 
                0X01011010FEDCBAFE, 
                0xDCBAABCDEFABCDEF, 
                0X01011010FEDCBAFE, 
                0x0000ABCDEFABCDEF };
            UlongArrayBigInt.InternalFixedLengthRotateRight(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Rotação de um número superior ao tamanho da variável
            places = 132; // 2 * 64 + 4
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[] { 
                0xAABCDEFABCDEF010, 
                0X11010FEDCBAFEDCB, 
                0x0ABCDEFABCDEF010, 
                0x0000000000000000, 
                0x0000000000000000 };
            UlongArrayBigInt.InternalFixedLengthRotateRight(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Rotação de um número superior a metade do tamanho do vector
            places = 212; // 3 * 64 + 20
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[]{ 
                0XF01011010FEDCBAF, 
                0x00000ABCDEFABCDE, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000 };
            UlongArrayBigInt.InternalFixedLengthRotateRight(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Menos do tamanho de uma variável para completara um vector
            places = 316;  // 5 * 64 - 4
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[] { 
                0x000000000000000A, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000 };
            UlongArrayBigInt.InternalFixedLengthRotateRight(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Tamanho igual ao do vector
            places = 320; // 3 * 64
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[] { 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000 };
            UlongArrayBigInt.InternalFixedLengthRotateRight(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);
        }
        /// <summary>
        /// Testa a função que permite rodar à esquerda um vector de valores, mantendo
        /// fixo o seu tamanho.
        /// </summary>
        /// <remarks>
        /// A rotação à direita é relativa ao número representado pelo vector, ito é,
        /// a rotação é tal que o número resultante advém multiplicado por dois.
        /// </remarks>
        [TestMethod]
        public void UlongArrayBigInt_FixedLengthRotateLeft()
        {
            var vectorToRotate = new ulong[] { 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101 };

            // Rotação de um número inferior ao tamanho da variável
            var places = 16;
            var currentVector = new ulong[vectorToRotate.Length];
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            var expected = new ulong[]{ 
                0xEFABCDEF01010000, 
                0XFEDCBAFEDCBAABCD, 
                0xEFABCDEF01011010, 
                0XFEDCBAFEDCBAABCD, 
                0xEFABCDEF01011010 };
            UlongArrayBigInt.InternalFixedLengthRotateLeft(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Rotação de um número superior ao tamanho da variável
            places = 132; // 2 * 64 + 4
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[] { 
                0x0000000000000000, 
                0x0000000000000000, 
                0xBCDEFABCDEF01010, 
                0X010FEDCBAFEDCBAA, 
                0xBCDEFABCDEF01011 };
            UlongArrayBigInt.InternalFixedLengthRotateLeft(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Rotação de um número superior a metade do tamanho do vector
            places = 212; // 3 * 64 + 20
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[]{ 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0xFABCDEF010100000, 
                0XEDCBAFEDCBAABCDE };
            UlongArrayBigInt.InternalFixedLengthRotateLeft(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Menos do tamanho de uma variável para completara um vector
            places = 316;  // 5 * 64 - 4
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[] { 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x1000000000000000 };
            UlongArrayBigInt.InternalFixedLengthRotateLeft(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);

            // Tamanho igual ao do vector
            places = 320; // 3 * 64
            Array.Copy(vectorToRotate, currentVector, vectorToRotate.Length);
            expected = new ulong[] { 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000, 
                0x0000000000000000 };
            UlongArrayBigInt.InternalFixedLengthRotateLeft(currentVector, places);
            CollectionAssert.AreEqual(expected, currentVector);
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
                        Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                    }
                }
                else
                {
                    Assert.Fail("Um problema ocorreu durante a leitura dos números.");
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
                        Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                    }
                }
                else
                {
                    Assert.Fail("Um problema ocorreu durante a leitura dos números.");
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
                            Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                        }
                    }
                    else
                    {
                        Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                    }
                }
                else
                {
                    Assert.Fail("Um problema ocorreu durante a leitura dos números.");
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
                            Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                        }
                    }
                    else
                    {
                        Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                    }
                }
                else
                {
                    Assert.Fail("Um problema ocorreu durante a leitura dos números.");
                }
            }
        }

        /// <summary>
        /// Testa a validade da sobrecarga do operador de rotação à esquerda para números inteiros enormes.
        /// </summary>
        [TestMethod]
        public void UlongArrayBigInt_TestRotateLeftOperator()
        {
            var vectorToRotate = new ulong[] { 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101 };

            var firstNumbers = new[] { 
                new UlongArrayBigInt(1),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate),
            };
            var secondNumbers = new[] { 
                200, 
                16, 
                132, 
                212, 
                316, 
                320 };
            var expected = new[] { 
                new UlongArrayBigInt(new[] { 0ul, 0ul, 0ul, 256ul }),
                new UlongArrayBigInt(new ulong[]{ 
                    0xEFABCDEF01010000, 
                    0XFEDCBAFEDCBAABCD, 
                    0xEFABCDEF01011010, 
                    0XFEDCBAFEDCBAABCD, 
                    0xEFABCDEF01011010,
                    0xABCDul}),
                new UlongArrayBigInt(new ulong[] { 
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0xBCDEFABCDEF01010, 
                    0X010FEDCBAFEDCBAA, 
                    0xBCDEFABCDEF01011,
                    0X010FEDCBAFEDCBAA, 
                    0xBCDEFABCDEF01011,
                    0xAul}),
                new UlongArrayBigInt(new ulong[]{ 
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0xFABCDEF010100000, 
                    0XEDCBAFEDCBAABCDE, 
                    0xFABCDEF01011010F, 
                    0XEDCBAFEDCBAABCDE, 
                    0xFABCDEF01011010F,
                    0xABCDE}),
                new UlongArrayBigInt(new ulong[] { 
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0x1000000000000000,
                    0XAABCDEFABCDEF010, 
                    0x11010FEDCBAFEDCB, 
                    0XAABCDEFABCDEF010, 
                    0x11010FEDCBAFEDCB,
                    0xABCDEFABCDEF010
                }),
                new UlongArrayBigInt(new ulong[]{
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0x0000000000000000, 
                    0x0000000000000000,
                    0x0000000000000000,
                    0xABCDEFABCDEF0101, 
                    0X1010FEDCBAFEDCBA, 
                    0xABCDEFABCDEF0101, 
                    0X1010FEDCBAFEDCBA, 
                    0xABCDEFABCDEF0101
                })
            };
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
            var vectorToRotate = new ulong[] { 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101, 
                0X1010FEDCBAFEDCBA, 
                0xABCDEFABCDEF0101 };

            var firstNumbers = new[] { 
                new UlongArrayBigInt(new[] { 0ul, 0ul, 0ul, 256ul }),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate),
                new UlongArrayBigInt(vectorToRotate)
            };
            var secondNumbers = new[] { 
                200,
                16,
                132,
                212,
                316,
                320
            };
            var expected = new[] { 
                new UlongArrayBigInt(1),
                new UlongArrayBigInt(new ulong[]{ 
                    0xDCBAABCDEFABCDEF, 
                    0X01011010FEDCBAFE, 
                    0xDCBAABCDEFABCDEF, 
                    0X01011010FEDCBAFE, 
                    0x0000ABCDEFABCDEF }),
                new UlongArrayBigInt(new ulong[] { 
                    0xAABCDEFABCDEF010, 
                    0X11010FEDCBAFEDCB, 
                    0x0ABCDEFABCDEF010}),
                new UlongArrayBigInt(new ulong[]{ 
                    0XF01011010FEDCBAF, 
                    0x00000ABCDEFABCDE}),
                new UlongArrayBigInt(new ulong[] { 
                    0x000000000000000A}),
                new UlongArrayBigInt()
            };
            for (int i = 0; i < expected.Length; ++i)
            {
                var actual = firstNumbers[i] >> secondNumbers[i];
                Assert.AreEqual(expected[i], actual);
            }
        }

        #endregion Testes à sobrecarga de operadores para a classe UlongArrayBigInt
    }

    /// <summary>
    /// Permite testar as funções definidas no provedor de funções que operam
    /// sobre números inteiros de precisão arbitrária.
    /// </summary>
    [TestClass]
    public class BigIntegerOperationsProvidersTest
    {
        /// <summary>
        /// Testa a função de divisão do providenciador do algoritmo.
        /// </summary>
        [Description("Tests the provider's division function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_RunElementartyTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Sem diferenças
            var firstArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            var secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            var actual = target.Run(firstArray, secondArray);
            CollectionAssert.AreEqual(new[] { 1UL }, actual.Item1);
            Assert.IsNull(actual.Item2);

            // O divisor é maior do que o dividendo
            firstArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1123
            };

            actual = target.Run(firstArray, secondArray);
            Assert.IsNull(actual.Item1);
            CollectionAssert.AreEqual(firstArray, actual.Item2);

            // Divisão exacta a menos de potência de 2
            firstArray = new ulong[]{
                0,
                0,
                0x25134948BCDDFE00,
                0x74628492837462AB,
                0xFFAADDEECC112383,
                0xBB
            };

            secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1123
            };

            actual = target.Run(firstArray, secondArray);
            var firstInteger = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            var secondInteger = Diagnostics.GetBigIntegerRepresentation(
                secondArray);
            var remainder = default(BigInteger);
            var quotient = BigInteger.DivRem(firstInteger, secondInteger, out remainder);
            Assert.AreEqual(quotient, Diagnostics.GetBigIntegerRepresentation(actual.Item1));
            Assert.IsTrue(remainder.IsZero);

            // Diferença insignificante
            firstArray = new ulong[]{
                0x25134948BCDDFE00,
                0x74628492837462AB,
                0xFFAADDEECC112383,
                0xBB
            };

            secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837461,
                0xBBFFAADDEECC1123
            };

            actual = target.Run(firstArray, secondArray);
            firstInteger = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            secondInteger = Diagnostics.GetBigIntegerRepresentation(
                secondArray);
            remainder = default(BigInteger);
            quotient = BigInteger.DivRem(firstInteger, secondInteger, out remainder);
            Assert.AreEqual(quotient, Diagnostics.GetBigIntegerRepresentation(actual.Item1));
            Assert.AreEqual(remainder, Diagnostics.GetBigIntegerRepresentation(actual.Item2));
        }

        /// <summary>
        /// Testa a função de divisão do providenciador do algoritmo.
        /// </summary>
        [Description("Tests the provider's division function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_RunOneSizedTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Ambos os termos com um elemento
            var firstArray = new ulong[] { 2050761UL };
            var secondArray = new ulong[] { 1343UL };
            var actual = target.Run(firstArray, secondArray);
            Assert.AreEqual(1527UL, actual.Item1[0]);
            Assert.IsNull(actual.Item2);

            // Ambos os termos com um número superior de dígitos
            firstArray = new ulong[] { 
                0x25134948BCDDFE00,
                0x74628492837462AB,
                0xFFAADDEECC112383
            };

            secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837461,
                0xBBFFAADDEECC1123
            };

            var firstInteger = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            var secondInteger = Diagnostics.GetBigIntegerRepresentation(
                secondArray);
            var remainder = default(BigInteger);
            var quotient = BigInteger.DivRem(firstInteger, secondInteger, out remainder);

            actual = target.Run(firstArray, secondArray);
            var actualQuot = Diagnostics.GetBigIntegerRepresentation(
                actual.Item1);
            var actualRem = Diagnostics.GetBigIntegerRepresentation(
                actual.Item2);
            Assert.AreEqual(quotient, actualQuot);
            Assert.AreEqual(remainder, actualRem);
        }

        /// <summary>
        /// Testa a função de divisão do providenciador do algoritmo.
        /// </summary>
        [Description("Tests the provider's division function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_RunDiffSizedTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            var firstItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var secondItemArray = new ulong[]{
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121,
                0xA1000BC150000300
            };

            var firstInteger = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray);
            var secondInteger = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray);
            var expectedRem = default(BigInteger);
            var expectedQuo = BigInteger.DivRem(firstInteger, secondInteger, out expectedRem);

            var actual = target.Run(firstItemArray, secondItemArray);
            var actualQuo = Diagnostics.GetBigIntegerRepresentation(
                actual.Item1);
            var actualRem = Diagnostics.GetBigIntegerRepresentation(
                actual.Item2);

            Assert.AreEqual(expectedQuo, actualQuo);
            Assert.AreEqual(expectedRem, actualRem);
        }

        /// <summary>
        /// Testa a função que permite averiguar a existência de diferenças
        /// entre dois vectores do mesmo tamanho.
        /// </summary>
        [Description("Tests same length non shifted find function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_SameLenNoShiftFindTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Sem diferenças
            var firstArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            var secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            var actualIndex = default(int);
            var actual = target.InternalFindSameLengthDifference(
                firstArray,
                secondArray,
                3,
                out actualIndex);
            Assert.AreEqual(-1, actualIndex);
            Assert.AreEqual(0, actual);

            // Com diferenças
            firstArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1121
            };

            actual = target.InternalFindSameLengthDifference(
                firstArray,
                secondArray,
                3,
                out actualIndex);
            Assert.AreEqual(2, actualIndex);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Testa todas as funções que permitem determinar as diferenças após um deslocamento.
        /// </summary>
        [Description("Tests same length shifted find function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_SameLenShiftedFindTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Testa dois vectores iguais com o mesmo comprimento
            var firstItemArray = new ulong[3] { 
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB3123
            };

            var secondItemArray = new ulong[3] { 
                0xA1A1000BC1500003,
                0x23EFFFF09829420A,
                0xB31
            };

            var actualIndex = default(int);
            var actual = target.InternalFindSameLengthDifference(
                firstItemArray,
                secondItemArray,
                3,
                8,
                out actualIndex);
            Assert.AreEqual(-1, actualIndex);
            Assert.AreEqual(0, actual);

            // Testa dois vectores com diferença no item de índcie 1 e o mesmo comprimento
            firstItemArray = new ulong[3] { 
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB3123
            };

            secondItemArray = new ulong[3] { 
                0xA1A1000BC1500003,
                0x23EFBFF09829420A,
                0xB31
            };

            actual = target.InternalFindSameLengthDifference(
                firstItemArray,
                secondItemArray,
                3,
                8,
                out actualIndex);
            Assert.AreEqual(1, actualIndex);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Testa a função que permite averiguar a existência de diferenças entre dois
        /// vectores sem a necessidade de aplicar um deslocamento ao nível da variável.
        /// </summary>
        [Description("Tests the different length no shift find function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLenNoShiftFindTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var secondItemArray = new ulong[]{
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var actualIndex = default(int);
            var actual = target.InternalFindOtherLengthDifference(
                firstItemArray,
                5,
                secondItemArray,
                3,
                out actualIndex);
            Assert.AreEqual(1, actualIndex);
            Assert.AreEqual(1, actual);

            // Existe uma diferença no segundo argumento
            firstItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C2,
                0xAAFFBBCCDDEE1121,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            actual = target.InternalFindOtherLengthDifference(
                firstItemArray,
                5,
                secondItemArray,
                3,
                out actualIndex);
            Assert.AreEqual(4, actualIndex);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Testa a função que permite averiguar a existência de diferenças entre dois
        /// vectores sem a necessidade de aplicar um deslocamento ao nível da variável.
        /// </summary>
        [Description("Tests the different length no shift find function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLenShiftFindTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFD00,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var secondItemArray = new ulong[]{
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1,
                0xAAFFBBCCDDEE1121,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var actualIndex = default(int);
            var actual = target.InternalFindOtherLengthDifference(
                firstItemArray,
                5,
                secondItemArray,
                3,
                8,
                out actualIndex);
            Assert.AreEqual(1, actualIndex);
            Assert.AreEqual(1, actual);

            // Não existe diferenças no segundo argumento
            firstItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFD00,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0x9AB31234462ABAFD,
                0xC3B1CA3123163271,
                0xB1,
                0xAAFFBBCCDDEE1121,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            actual = target.InternalFindOtherLengthDifference(
                firstItemArray,
                5,
                secondItemArray,
                3,
                8,
                out actualIndex);
            Assert.AreEqual(3, actualIndex);
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Testa a função que permite averiguar a existência de diferenças entre dois
        /// vectores após um deslocamento negativo ao nível da variável.
        /// </summary>
        [Description("Tests the different length negative shift find function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLenNegativeShiftFindTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var secondItemArray = new ulong[]{
                0x1234462ABAFDCE00,
                0xCA31231632789AB3,
                0xB1C3B1,
                0xAAFFBBCCDDEE1121,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var actualIndex = default(int);
            var actual = target.InternalFindOtherLengthDiffNegativeShift(
                firstItemArray,
                5,
                secondItemArray,
                3,
                8,
                out actualIndex);
            Assert.AreEqual(1, actualIndex);
            Assert.AreEqual(1, actual);

            firstItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0x1234462ABAFDCF00,
                0xCA31231632789AB3,
                0xB1C3B1,
                0xAAFFBBCCDDEE1121,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            actual = target.InternalFindOtherLengthDiffNegativeShift(
                firstItemArray,
                5,
                secondItemArray,
                3,
                8,
                out actualIndex);
            Assert.AreEqual(2, actualIndex);
            Assert.AreEqual(-1, actual);
        }

        /// <summary>
        /// Testa a função que permite determinar a diferença entre representações,
        /// considerando que não existe deslocamento e os vectores têm o mesmo tamanho.
        /// </summary>
        [Description("Tests the same length subtract function with no shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_SameLengthNoShiftSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Sem diferenças
            var firstArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            var secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            var actual = target.InternalSubtractSameLength(
                firstArray,
                secondArray,
                3,
                firstArray);
            Assert.AreEqual(0, actual);

            // Com diferenças
            firstArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            secondArray = new ulong[]{
                0xAB25134948BCDDFF,
                0x8374428434837462,
                0xBBFFAADDEECC1122
            };

            // Resultado esperado da diferença
            var firstInteger = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            var secondInteger = Diagnostics.GetBigIntegerRepresentation(
                secondArray);
            var expectedDifference = firstInteger - secondInteger;

            actual = target.InternalSubtractSameLength(
                firstArray,
                secondArray,
                3,
                firstArray);

            Assert.AreEqual(2, actual);

            var acutalDifference = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            Assert.AreEqual(expectedDifference, acutalDifference);
        }

        /// <summary>
        /// Testa a função que permite determinar a diferença entre representações,
        /// considerando que existe deslocamento positivo e os vectores têm o mesmo comprimento.
        /// </summary>
        [Description("Tests the same length subtract function with positive shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_SameLengthPosShiftSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Sem diferenças
            var firstArray = new ulong[]{
                0x25134948BCDDFE00,
                0x74628492837462AB,
                0xFFAADDEECC112283
            };

            var secondArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837462,
                0xFFAADDEECC1122
            };

            var actual = target.InternalSubtractSameLength(
                firstArray,
                secondArray,
                3,
                8,
                firstArray);
            Assert.AreEqual(0, actual);

            // Com diferenças
            firstArray = new ulong[]{
                0x25134948BCDDFE00,
                0x74628492837462AB,
                0xFFAADDEECC112283
            };

            secondArray = new ulong[]{
                0x9B25134948BCDDFE,
                0x8374628492837462,
                0xFFAADDEECC1122
            };

            // Resultado esperado da diferença
            var firstInteger = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            var secondInteger = Diagnostics.GetBigIntegerRepresentation(
                secondArray);
            secondInteger <<= 8;
            var expectedDifference = firstInteger - secondInteger;

            actual = target.InternalSubtractSameLength(
                firstArray,
                secondArray,
                3,
                8,
                firstArray);
            Assert.AreEqual(2, actual);

            var acutalDifference = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            Assert.AreEqual(expectedDifference, acutalDifference);

        }

        /// <summary>
        /// Testa a função que permite determinar a diferença entre representações,
        /// considerando que não existe deslocamento e os vectores têm comprimentos diferentes.
        /// </summary>
        [Description("Tests subtract function with different length and no shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLengthNoShiftSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0x0,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            var secondItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3
            };

            var actual = target.InternalSubtractDiffLength(
                firstItemArray,
                9,
                secondItemArray,
                4,
                firstItemArray);
            Assert.AreEqual(0, actual);

            // Existem diferenças no segundo argumento
            firstItemArray = new ulong[]{
                0x1,
                0x0,
                0x0,
                0x0,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3
            };

            actual = target.InternalSubtractDiffLength(
                firstItemArray,
                9,
                secondItemArray,
                4,
                firstItemArray);
            Assert.AreEqual(1, actual);
            Assert.AreEqual(1UL, firstItemArray[0]);
        }

        /// <summary>
        /// Testa a função que permite determinar a diferença entre representações,
        /// considerando que existe deslocamento positivo e os vectores têm comprimentos diferentes.
        /// </summary>
        [Description("Tests subtract function with different length and positive shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLengthPosShiftSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0x0,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            // Deslocamento de 8
            var secondItemArray = new ulong[]{
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1
            };

            var actual = target.InternalSubtractDiffLength(
                firstItemArray,
                9,
                secondItemArray,
                4,
                8,
                firstItemArray);
            Assert.AreEqual(0, actual);

            // Existem diferenças no segundo argumento
            firstItemArray = new ulong[]{
                0x1,
                0x0,
                0x0,
                0x0,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3
            };

            actual = target.InternalSubtractDiffLength(
                firstItemArray,
                9,
                secondItemArray,
                4,
                firstItemArray);
            Assert.AreEqual(1, actual);
            Assert.AreEqual(1UL, firstItemArray[0]);
        }

        /// <summary>
        /// Testa a função que permite determinar a diferença entre representações,
        /// considerando que existe deslocamento negativo e os vectores têm comprimentos diferentes.
        /// </summary>
        [Description("Tests subtract function with different length and negative shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLengthNegShiftSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            #region Teste 1

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0x0,
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1,
                0xAAFFBBCCDDEE1121
            };

            var secondItemArray = new ulong[]{
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3
            };

            var actual = target.InternalSubtractDiffLengthNegativeShift(
                firstItemArray,
                9,
                secondItemArray,
                4,
                8,
                firstItemArray);
            Assert.AreEqual(0, actual);

            // Não existe diferenças no segundo argumento
            firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0x1200000000000000,
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0xA1000BC150000312,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3
            };

            actual = target.InternalSubtractDiffLengthNegativeShift(
                firstItemArray,
                9,
                secondItemArray,
                4,
                8,
                firstItemArray);
            Assert.AreEqual(0, actual);

            #endregion Teste1

            #region Teste 2
            // Diferença no primeiro elemento
            firstItemArray = new ulong[]{
                0x1,
                0x0,
                0x0,
                0x1200000000000000,
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0xA1000BC150000312,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3
            };

            actual = target.InternalSubtractDiffLengthNegativeShift(
                firstItemArray,
                9,
                secondItemArray,
                4,
                8,
                firstItemArray);
            Assert.AreEqual(1, actual);
            Assert.AreEqual(1UL, firstItemArray[0]);

            firstItemArray = new ulong[]{
                0x25134948BCDDFE00,
                0x74628492837462AB,
                0xFFAADDEECC112383,
                0xBB
            };

            secondItemArray = new ulong[]{
                0xAB25134948BCDDFE,
                0x8374628492837461,
                0xBBFFAADDEECC1123
            };

            var firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray);
            var secondValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray);
            var expectedValue = firstValue - ((BigInteger.Pow(2, 64) * secondValue) >> 56);
            actual = target.InternalSubtractDiffLengthNegativeShift(
                firstItemArray,
                2,
                secondItemArray,
                1,
                56,
                firstItemArray);
            var actualValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray,
                actual);
            Assert.AreEqual(expectedValue, actualValue);

            #endregion Teste 2

            firstItemArray = new ulong[]{
                12903433358339145166,
                12811106116774819994,
                45507,
                12321773594080055585,
                11601285565005169408
            };

            secondItemArray = new ulong[]{
                11601285565005169408,
                17293805630443883169,
                6672719043484065998,
                14542824221779521721,
                4078015174020545124,
                11236798711824820117,
                191564612139687047,
                1,
                1,
                5,
                1288
            };

            firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray);
            secondValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray,
                8);
            expectedValue = firstValue * BigInteger.Pow(2, 129) - secondValue;
            actual = target.InternalInvSubtractDiffLengthNegShift(
                firstItemArray,
                3,
                63,
                secondItemArray,
                7,
                secondItemArray);
            actualValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray,
                actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Testa a função que permite determinar a diferença inversa entre representações,
        /// considerando que existe deslocamento positivo e os vectores têm o mesmo comprimento.
        /// </summary>
        /// <remarks>
        /// Entenda-se, por diferença inversa, a diferença entre o segundo argumento e o primeiro.
        /// </remarks>
        [Description("Tests the same length inverse subtract function with positive shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_SameLengthPosShiftInvSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Sem diferenças
            var firstArray = new ulong[]{
                0xAB25134948BCDD00,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            var secondArray = new ulong[]{
                0x62AB25134948BCDD,
                0x2283746284928374,
                0xBBFFAADDEECC11
            };

            var actual = target.InternalInvSubtractSameLength(
                secondArray,
                8,
                firstArray,
                3,
                firstArray);
            Assert.AreEqual(0, actual);

            // Com diferenças
            firstArray = new ulong[]{
                0xAB25134948BCDD00,
                0x8374628492837462,
                0xBBFFAADDEECC1122
            };

            secondArray = new ulong[]{
                0x62AB25134948BCDD,
                0x328376284928374,
                0xBBFFAAED1ECC11
            };

            var firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            var secondValue = Diagnostics.GetBigIntegerRepresentation(
                secondArray);
            var expected = (secondValue << 8) - firstValue;

            target.InternalInvSubtractSameLength(
                secondArray,
                8,
                firstArray,
                3,
                firstArray);
            var actualValue = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            Assert.AreEqual(expected, actualValue);
        }

        /// <summary>
        /// Testa a função que determina a diferença inversa onde o vector do minuendo é submetido
        /// a um deslocamento nas entradas mas nenhum deslocamento ao nível dos bits.
        /// </summary>
        [Description("Tests the inverse subtract function with different length but no shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_SameLengthInvSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            var firstArray = new ulong[]{
                12903433358339145166,
                12811106116774819994,
                45507,
                12321773594080055585,
                11601285565005169408
            };

            var secondArray = new ulong[]{
                11601285565005169408,
                17293805630443883169,
                12903433358339145166,
                10460997092868709918,
                7448880953912121190,
                3485609974556604077,
                7912916031094100824,
                9501804922703610793,
                1,
                5,
                1288
            };

            var firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstArray);
            var secondValue = Diagnostics.GetBigIntegerRepresentation(
                secondArray,
                8);
            var expectedValue = firstValue * BigInteger.Pow(2, 192) - secondValue;

            var actual = target.InternalInvSubtractDiffLength(
                firstArray,
                3,
                secondArray,
                8,
                secondArray);
            var actualValue = Diagnostics.GetBigIntegerRepresentation(
                secondArray,
                actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Testa a função que permite determinar a diferença inversa entre representações,
        /// considerando que existe deslocamento positivo e os vectores têm comprimentos diferentes.
        /// </summary>
        /// <remarks>
        /// Entenda-se, por diferença inversa, a diferença entre o segundo argumento e o primeiro.
        /// </remarks>
        [Description("Tests the different length inverse subtract function with positive shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLengthPosShiftInvSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0x0,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            // Deslocamento de 8
            var secondItemArray = new ulong[]{
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1
            };

            var actual = target.InternalInvSubtractDiffLength(
                secondItemArray,
                4,
                8,
                firstItemArray,
                9,
                firstItemArray);
            Assert.AreEqual(0, actual);

            // Existem diferenças no segundo argumento
            firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0x0,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3,
                0xAAFFBBCCDDEE1121
            };

            // Deslocamento de 8
            secondItemArray = new ulong[]{
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB332F4462ABAFD,
                0xC3B1CBC123163278,
                0xD1
            };

            var tempSecondary = new ulong[]{
                0x0,
                0x0,
                0x0,
                0x0,
                0xA1000BC150000300,
                0xEFFFF09829420AA1,
                0xB332F4462ABAFDCE,
                0xB1CBC1231632789A,
                0xD1C3
            };

            var firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray,
                9);
            var secondValue = Diagnostics.GetBigIntegerRepresentation(
                tempSecondary);
            var expectedValue = secondValue - firstValue;

            actual = target.InternalInvSubtractDiffLength(
                secondItemArray,
                4,
                8,
                firstItemArray,
                9,
                firstItemArray);
            var actualValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray,
                actual);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Testa a função que permite determinar a diferença inversa entre representações,
        /// considerando que existe deslocamento positivo e os vectores têm comprimentos diferentes.
        /// </summary>
        /// <remarks>
        /// Entenda-se, por diferença inversa, a diferença entre o segundo argumento e o primeiro.
        /// </remarks>
        [Description("Tests the different length inverse subtract function with positive shift.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DiffLengthNegShiftInvSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            #region Teste 1

            // Não existe diferenças no segundo argumento
            var firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0xAB00000000000000,
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1,
                0xAAFFBBCCDDEE1121
            };

            var secondItemArray = new ulong[]{
                0xA1000BC1500003AB,
                0xEFFFF09829420AA1,
                0xB31234462ABAFDCE,
                0xB1CA31231632789A,
                0xB1C3
            };

            var actual = target.InternalInvSubtractDiffLengthNegShift(
                secondItemArray,
                4,
                8,
                firstItemArray,
                9,
                firstItemArray);
            Assert.AreEqual(0, actual);

            // Com diferenças
            firstItemArray = new ulong[]{
                0x0,
                0x0,
                0x0,
                0xAB00000000000000,
                0xA1A1000BC1500003,
                0xCEEFFFF09829420A,
                0x9AB31234462ABAFD,
                0xC3B1CA3123163278,
                0xB1,
                0xAAFFBBCCDDEE1121
            };

            secondItemArray = new ulong[]{
                0xA1000BC1500003AB,
                0xEFFFF098F9420AA1,
                0xB3123446BABAFDCE,
                0xB1C231234632789A,
                0xD1C3
            };

            var firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray,
                9);
            var secondValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray);
            secondValue *= System.Numerics.BigInteger.Pow(2, 256);
            secondValue >>= 8;
            var expectedValue = secondValue - firstValue;
            actual = target.InternalInvSubtractDiffLengthNegShift(
                secondItemArray,
                4,
                8,
                firstItemArray,
                9,
                firstItemArray);
            var actualValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray,
                actual);
            Assert.AreEqual(expectedValue, actualValue);

            #endregion Teste 1

            #region Teste 2
            firstItemArray = new ulong[]{
                12903433358339145166,
                12811106116774819994,
                45507,
                12321773594080055585,
                11601285565005169408
            };

            secondItemArray = new ulong[]{
                11601285565005169408,
                17293805630443883169,
                12903433358339145166,
                12811106116774819994,
                45507,
                12321773594080055585,
                4390372272104738003,
                92327241564325172,
                12811106116774774487,
                6124970479629541538,
                720488029074886176
            };

            firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray);
            firstValue *= System.Numerics.BigInteger.Pow(2, 384);
            firstValue >>= 4;
            secondValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray);
            expectedValue = firstValue - secondValue;
            actual = target.InternalInvSubtractDiffLengthNegShift(
                firstItemArray,
                6,
                4,
                secondItemArray,
                11,
                secondItemArray);
            actualValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray,
                actual);
            Assert.AreEqual(expectedValue, actualValue);

            #endregion Teste 2

            #region Teste 3

            // O primeiro vector resulta do teste anterior
            secondItemArray = new ulong[]{
                11601285565005169408,
                6144485527959418034,
                11488200373593518626,
                16024876265138863332,
                15533116060673564896,
                2847104458729980969,
                2,
                1,
                1,
                5,
                1288
            };

            firstValue = Diagnostics.GetBigIntegerRepresentation(
                firstItemArray);
            firstValue <<= 62;
            secondValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray,
                6);
            expectedValue = firstValue - secondValue;
            actual = target.InternalInvSubtractDiffLengthNegShift(
                firstItemArray,
                1,
                2,
                secondItemArray,
                6,
                secondItemArray);
            actualValue = Diagnostics.GetBigIntegerRepresentation(
                secondItemArray,
                actual);
            Assert.AreEqual(expectedValue, actualValue);

            #endregion Teste 3
        }

        /// <summary>
        /// Testa a função que aplica decrementos ao quociente.
        /// </summary>
        [Description("Tests the decrement quotient function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_DecrementQuoTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            var quotient = new ulong[]{
                0,
                0,
                0,
                0,
                0,
                1152921504606846976,
                1
            };

            var quotientValue = Diagnostics.GetBigIntegerRepresentation(
                quotient);
            var subtractValue = BigInteger.Pow(2, 64 * 5) * 9007199254740992;
            var expectedValue = quotientValue - subtractValue;
            target.InternalDecrementQuo(
                quotient,
                9007199254740992,
                5);
            var actualValue = Diagnostics.GetBigIntegerRepresentation(
                quotient);
            Assert.AreEqual(expectedValue, actualValue);

            quotient = new ulong[]{
                0,
                0,
                0,
                0,
                0,
                1152921504606846976,
                1
            };

            quotientValue = Diagnostics.GetBigIntegerRepresentation(
                quotient);
            subtractValue = BigInteger.Pow(2, 64 * 2) * 9007199254740992;
            expectedValue = quotientValue - subtractValue;
            target.InternalDecrementQuo(
                quotient,
                9007199254740992,
                2);
            actualValue = Diagnostics.GetBigIntegerRepresentation(
                quotient);
            Assert.AreEqual(expectedValue, actualValue);
        }

        /// <summary>
        /// Testa a função que determina a diferença entre dois números representados por vectores.
        /// </summary>
        [Description("Tests the general subtract function.")]
        [TestMethod]
        public void BigIntegerOpsProvs_GeneralSubtractTest()
        {
            var target = new UlongBigIntegerSequentialQuotAndRemAlg();

            var firstItemArray = new ulong[]{

            };
        }
    }
}
