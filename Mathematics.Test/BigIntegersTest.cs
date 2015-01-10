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
    }
}
