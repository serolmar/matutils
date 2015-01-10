namespace Mathematics.Test
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
        [Description("Teste que permite averiguar a validade das leituras dos números enormes representados por vectores de longos.")]
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
        [Description("Teste que permite averiguar o comportamento face a leituras dos números enormes representados por vectores de longos erradas.")]
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

        #region Testes à sobrecarga de operadores para a classe UlongArrayBigInt

        /// <summary>
        /// Teste ao operador de igualdade entre números inteiro enormes.
        /// </summary>
        [Description("Testa a validade da sobrecarga do operador de igualdade entre números inteiro enormes.")]
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
        /// Teste ao operador de igualdade entre números inteiro enormes.
        /// </summary>
        [Description("Testa a validade da sobrecarga do operador de igualdade entre números inteiro enormes.")]
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

        #endregion Testes à sobrecarga de operadores para a classe UlongArrayBigInt
    }
}
