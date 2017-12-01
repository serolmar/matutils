// -----------------------------------------------------------------------
// <copyright file="BitListTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics.Test
{
    using Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Implementa testes sobre as listas de bits.
    /// </summary>
    [TestClass()]
    public class BitListTest
    {
        /// <summary>
        /// Testa a função de adição de vários valores em simultâneo na lista de bits.
        /// </summary>
        [Description("Tests the add range for bit list.")]
        [TestMethod()]
        public void AddRangeTest_BitList()
        {
            var initialCount = 5;
            var rangeCount = 10;
            BitList target = new BitList(initialCount, true);
            BitList range = new BitList(rangeCount, false);
            var totalCount = initialCount + rangeCount;
            target.AddRange(range);
            Assert.AreEqual(totalCount, target.Count);
            var i = 0;
            for (; i < initialCount; ++i)
            {
                var bit = target[i];
                Assert.AreNotEqual(0, bit);
            }

            for (; i < totalCount; ++i)
            {
                var bit = target[i];
                Assert.AreEqual(0, bit);
            }
        }

        /// <summary>
        /// Testa a adição de vários valores em simultâneo no vector de bits.
        /// </summary>
        [Description("Tests the add range for bit array.")]
        [TestMethod()]
        public void AddRangeTest_BitsArray()
        {
            var initialCount = 5;
            var rangeCount = 10;
            BitList target = new BitList(initialCount, true);
            var bitsArray = new ulong[] { 0 };
            target.AddRange(bitsArray, rangeCount);
            var totalCount = initialCount + rangeCount;
            Assert.AreEqual(totalCount, target.Count);
            var i = 0;
            for (; i < initialCount; ++i)
            {
                var bit = target[i];
                Assert.AreNotEqual(0, bit);
            }

            for (; i < totalCount; ++i)
            {
                var bit = target[i];
                Assert.AreEqual(0, bit);
            }
        }

        /// <summary>
        /// Testa  função de leitura de valores numéricos sobre texto vazio.
        /// </summary>
        [Description("Tests the empty numeric read function.")]
        [TestMethod()]
        public void ReadNumericTest_EmptyValue()
        {
            string numericText = "";
            var expected = new BitList();
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testa a função de leitura numérica para pequenos valores.
        /// </summary>
        [Description("Tests small values numeric read function.")]
        [TestMethod()]
        public void ReadNumericTest_SmallValue()
        {
            string numericText = "10";
            var expected = new BitList() { 0, 1, 0, 1 };
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testa a função de leitura numérica para pequenos valores com zeros à esquerda.
        /// </summary>
        [Description("Tests small values numeric read function with leading zeroes.")]
        [TestMethod()]
        public void ReadNumericTest_LeadingZeroesSmallValue()
        {
            string numericText = "0000010";
            var expected = new BitList() { 0, 1, 0, 1 };
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testa a função de leitura numérica para grandes valores.
        /// </summary>
        [Description("Tests big values numeric read function.")]
        [TestMethod()]
        public void ReadNumericTest_BigValue()
        {
            // Centésima potência de 2
            string numericText = "1267650600228229401496703205376";
            var expected = new BitList();
            for (int i = 0; i < 100; ++i)
            {
                expected.Add(0);
            }

            expected.Add(1);
            var actual = BitList.ReadNumeric(numericText);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testa a leitura de valores binários.
        /// </summary>
        [Description("Tests the binary read function.")]
        [TestMethod()]
        public void ReadBinaryTest_SimpleBinary()
        {
            string binaryText = "11100010";
            BitList expected = new BitList() { 1, 1, 1, 0, 0, 0, 1, 0 };
            var actual = BitList.ReadBinary(binaryText);
            Assert.AreEqual(expected, actual);
        }

        #region Testes às operações lógicas

        /// <summary>
        /// Testa a função ou lógico.
        /// </summary>
        [Description("Tests the bit list logic or funtion.")]
        [TestMethod()]
        public void BitListOrTest()
        {
            var target = BitList.ReadBinary("10010101010111010100010101101110011110111010111010000101011001011100110110110101000111011");
            BitList other = BitList.ReadBinary("01");
            BitList expected = BitList.ReadBinary("11010101010111010100010101101110011110111010111010000101011001011100110110110101000111011");
            var actual = target.BitListOr(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testa a função ou lógico exclusivo.
        /// </summary>
        [Description("Tests the bit list logic xor function.")]
        [TestMethod()]
        public void BitListXorTest()
        {
            var target = BitList.ReadBinary("1010");
            BitList other = BitList.ReadBinary("1001100");
            BitList expected = BitList.ReadBinary("0011100");
            var actual = target.BitListXor(other);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testa a função de negação lógica.
        /// </summary>
        [Description("Tests the bit list logic negation function.")]
        [TestMethod()]
        public void BitListNotTest()
        {
            BitList target = BitList.ReadBinary("11111111111111111111");
            BitList expected = BitList.ReadBinary("00000000000000000000");
            var actual = target.BitListNot();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Testa a função de e lógico.
        /// </summary>
        [Description("Tests the bit list logic and function.")]
        [TestMethod()]
        public void BitListAndTest()
        {
            var target = BitList.ReadBinary("10010101010111010100010101101110011110111010111010000101011001011100110110110101000111011");
            BitList other = BitList.ReadBinary("010101");
            BitList expected = BitList.ReadBinary("00010100000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            var actual = target.BitListAnd(other);
            Assert.AreEqual(expected, actual);
        }

        #endregion Testes às operações lógicas
    }
}
