using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Test
{
    /// <summary>
    /// Permite efectuar teste às utilidades.
    /// </summary>
    [TestClass]
    public class UtilsTest
    {
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UtilsTest"/>.
        /// </summary>
        public UtilsTest()
        {
        }

        /// <summary>
        /// O contexto do teste.
        /// </summary>
        private TestContext testContextInstance;

        /// <summary>
        /// Obtém e atribui o contexto que providencia informação sobre a execução do teste.
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
        /// Testa o iterador de bits ligados num longo sem sinal.
        /// </summary>
        [Description("Testa o iterador de bits ligados num longo sem sinal.")]
        [TestMethod]
        public void BitScanTest()
        {
            var value = 0UL;
            var bitscan = new BitScanEnumerable(value);
            var bitscanEnum = bitscan.GetEnumerator();
            Assert.IsFalse(bitscanEnum.MoveNext());

            var arrayTest = new[] { 1, 10, 15, 45, 63 };
            value = 0UL;
            for(var i = 0; i < arrayTest.LongLength; ++i)
            {
                value |= (1UL << arrayTest[i]);
            }

            bitscan = new BitScanEnumerable(value);
            var arrayPos = 0;
            foreach (var bitPos in bitscan)
            {
                Assert.AreEqual(arrayTest[arrayPos++], bitPos);
            }
        }
    }
}
