// -----------------------------------------------------------------------
// <copyright file="StatisticsTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mathematics;
    using Utilities;


    /// <summary>
    /// Efectua testes às funções estatísticas.
    /// </summary>
    [TestClass]
    public class StatisticsTest
    {
        /// <summary>
        /// Testa a função que permite calcular médias.
        /// </summary>
        [Description("Tests the generalized mean funtion.")]
        [TestMethod]
        public void Statistcs_EnumGeneralizeMeanAlgorithmTest()
        {
            var integerNumb = new IntegerDomain();
            var target = new EnumGeneralizedMeanAlgorithm<int, double, int>(
                i => i,
                d => d,
                (d, i) => d / i,
                new DoubleField(),
                integerNumb);

            var integerSequence = new IntegerSequence();
            for (var i = 1; i < 5000; ++i)
            {
                integerSequence.Add(i);
                var expected = (i + 1) / 2.0;
                var actual = target.Run(integerSequence);
                Assert.AreEqual(expected, actual);
            }

            integerSequence = new IntegerSequence();
            var n = 1000000;
            integerSequence.Add(1, n);
            var outerExpected = (n + 1) / 2.0;
            var outerActual = target.Run(integerSequence);
            Assert.AreEqual(outerExpected, outerActual);

            var bigIntegerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<BigInteger>(bigIntegerDomain);
            var fractionTarget = new EnumGeneralizedMeanAlgorithm<int, Fraction<BigInteger>, int>(
                i => new Fraction<BigInteger>(i, 1, bigIntegerDomain),
                d => d,
                (d, i) => d.Divide(i, bigIntegerDomain),
                fractionField,
                integerNumb);
            var fractionExpected = new Fraction<BigInteger>(n + 1, 2, bigIntegerDomain);
            var fractionActual = fractionTarget.Run(integerSequence);
            Assert.AreEqual(fractionExpected, fractionActual);

            // Teste com alteração da função directa
            fractionTarget.DirectFunction = i => new Fraction<BigInteger>(new BigInteger(i) * i, 1, bigIntegerDomain);
            fractionExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, bigIntegerDomain);
            fractionActual = fractionTarget.Run(integerSequence);
            Assert.AreEqual(fractionExpected, fractionActual);

            // Teste com transformação
            var transformedTarget = new EnumGeneralizedMeanAlgorithm<BigInteger, Fraction<BigInteger>, int>(
                i => new Fraction<BigInteger>(i, 1, bigIntegerDomain),
                d => d,
                (d, i) => d.Divide(i, bigIntegerDomain),
                fractionField,
                integerNumb);
            var transformedSeq = new TransformEnumerable<int, BigInteger>(
                integerSequence,
                i => new BigInteger(i) * i);
            var transformedExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, bigIntegerDomain);
            var transformedActual = transformedTarget.Run(transformedSeq);
            Assert.AreEqual(transformedExpected, transformedActual);
        }

        /// <summary>
        /// Testa a função que permite calcular médias, utilizando blocos com dimensões
        /// definidas à partida.
        /// </summary>
        [Description("Tests the generalized mean funtion with block processing.")]
        [TestMethod]
        public void Statistcs_EnumGeneralizeMeanBlockAlgorithmTest()
        {
            var integerNumb = new IntegerDomain();
            var target = new EnumGeneralizedMeanAlgorithm<int, double, int>(
                i => i,
                d => d,
                (d, i) => d / i,
                new DoubleField(),
                integerNumb);
            var blockNumber = 2500;

            var integerSequence = new IntegerSequence();

            for (var i = 1; i < 5500; ++i)
            {
                integerSequence.Add(i);
                var expected = (i + 1) / 2.0;
                var actual = target.Run<double>(
                    integerSequence,
                    blockNumber,
                    (j, k) => j / (double)k,
                    (d1, d2) => d1 * d2);
                Assert.IsTrue(Math.Abs(expected - actual) < 0.0001);
            }

            integerSequence = new IntegerSequence();
            var n = 1000500;
            integerSequence.Add(1, n);
            var outerExpected = (n + 1) / 2.0;
            var outerActual = target.Run<double>(
                    integerSequence,
                    blockNumber,
                    (j, k) => j / (double)k,
                    (d1, d2) => d1 * d2);
            Assert.AreEqual(outerExpected, outerActual);

            var integerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<BigInteger>(integerDomain);
            var fracTarget = new EnumGeneralizedMeanAlgorithm<int, Fraction<BigInteger>, int>(
                i => new Fraction<BigInteger>(i, 1, integerDomain),
                d => d,
                (d, i) => d.Divide(i, integerDomain),
                fractionField,
                integerNumb);

            var fractionExpected = new Fraction<BigInteger>(n + 1, 2, integerDomain);
            var fractionActual = fracTarget.Run<Fraction<BigInteger>>(
                integerSequence,
                blockNumber,
                (j, k) => new Fraction<BigInteger>(j, k, integerDomain),
                (d1, d2) => d1.Multiply(d2, integerDomain));

            Assert.AreEqual(fractionExpected, fractionActual);

            // Teste com alteração da função directa
            fracTarget.DirectFunction = i => new Fraction<BigInteger>(new BigInteger(i) * i, 1, integerDomain);
            fractionExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, integerDomain);
            fractionActual = fracTarget.Run<Fraction<BigInteger>>(
                integerSequence,
                blockNumber,
                (j, k) => new Fraction<BigInteger>(j, k, integerDomain),
                (d1, d2) => d1.Multiply(d2, integerDomain));

            // Teste com transformação
            var transformedTarget = new EnumGeneralizedMeanAlgorithm<BigInteger, Fraction<BigInteger>, int>(
                i => new Fraction<BigInteger>(i, 1, integerDomain),
                d => d,
                (d, i) => d.Divide(i, integerDomain),
                fractionField,
                integerNumb);
            var transformedSeq = new TransformEnumerable<int, BigInteger>(
                integerSequence,
                i => new BigInteger(i) * i);
            var transformedExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, integerDomain);
            var transformedActual = transformedTarget.Run<Fraction<BigInteger>>(
                transformedSeq,
                blockNumber,
                (j, k) => new Fraction<BigInteger>(j, k, integerDomain),
                (d1, d2) => d1.Multiply(d2, integerDomain));
            Assert.AreEqual(transformedExpected, transformedActual);
        }

        /// <summary>
        /// Testa a função que permite calcular médias.
        /// </summary>
        [Description("Tests the generalized mean funtion.")]
        [TestMethod]
        public void Statistcs_ListGeneralizeMeanAlgorithmTest()
        {
            var target = new ListGeneralizedMeanAlgorithm<int, double>(
                i => i,
                d=>d,
                (d, i) => d / i,
                new DoubleField());

            var integerSequence = new List<int>();
            for (var i = 1; i < 5000; ++i)
            {
                integerSequence.Add(i);
                var expected = (i + 1) / 2.0;
                var actual = target.Run(integerSequence);
                Assert.AreEqual(expected, actual);
            }

            var n = 1000000;
            for (var i = 5000; i <= n; ++i)
            {
                integerSequence.Add(i);
            }

            var outerExpected = (n + 1) / 2.0;
            var outerActual = target.Run(integerSequence);
            Assert.AreEqual(outerExpected, outerActual);

            var bigIntegerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<BigInteger>(bigIntegerDomain);
            var fractionTarget = new ListGeneralizedMeanAlgorithm<int, Fraction<BigInteger>>(
                i => new Fraction<BigInteger>(i, 1, bigIntegerDomain),
                d=>d,
                (d, i) => d.Divide(i, bigIntegerDomain),
                fractionField);
            var fractionExpected = new Fraction<BigInteger>(n + 1, 2, bigIntegerDomain);
            var fractionActual = fractionTarget.Run(integerSequence);
            Assert.AreEqual(fractionExpected, fractionActual);

            // Teste com alteração da função directa
            fractionTarget.DirectFunction = i => new Fraction<BigInteger>(new BigInteger(i) * i, 1, bigIntegerDomain);
            fractionExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, bigIntegerDomain);
            fractionActual = fractionTarget.Run(integerSequence);
            Assert.AreEqual(fractionExpected, fractionActual);

            // Teste com transformação
            var transformedTarget = new ListGeneralizedMeanAlgorithm<BigInteger, Fraction<BigInteger>>(
                i => new Fraction<BigInteger>(i, 1, bigIntegerDomain),
                d=>d,
                (d, i) => d.Divide(i, bigIntegerDomain),
                fractionField);
            var transformedSeq = new TransformList<int, BigInteger>(
                integerSequence,
                i => new BigInteger(i) * i);
            var transformedExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, bigIntegerDomain);
            var transformedActual = transformedTarget.Run(transformedSeq);
            Assert.AreEqual(transformedExpected, transformedActual);
        }

        /// <summary>
        /// Testa a função que permite calcular médias, utilizando blocos com dimensões
        /// definidas à partida.
        /// </summary>
        [Description("Tests the generalized mean funtion with block processing.")]
        [TestMethod]
        public void Statistcs_ListGeneralizeMeanBlockAlgorithmTest()
        {
            var target = new ListGeneralizedMeanAlgorithm<int, double>(
                i => i,
                d=>d,
                (d, i) => d / i,
                new DoubleField());
            var blockNumber = 2500;

            var integerSequence = new List<int>();

            for (var i = 1; i < 5500; ++i)
            {
                integerSequence.Add(i);
                var expected = (i + 1) / 2.0;
                var actual = target.Run<double>(
                    integerSequence,
                    blockNumber,
                    (j, k) => j / (double)k,
                    (d1, d2) => d1 * d2);
                Assert.IsTrue(Math.Abs(expected - actual) < 0.001);
            }

            var n = 1000500;
            for (var i = 5500; i <= n; ++i)
            {
                integerSequence.Add(i);
            }

            var outerExpected = (n + 1) / 2.0;
            var outerActual = target.Run<double>(
                    integerSequence,
                    blockNumber,
                    (j, k) => j / (double)k,
                    (d1, d2) => d1 * d2);
            Assert.AreEqual(outerExpected, outerActual);

            var integerDomain = new BigIntegerDomain();
            var fractionField = new FractionField<BigInteger>(integerDomain);
            var fracTarget = new ListGeneralizedMeanAlgorithm<int, Fraction<BigInteger>>(
                i => new Fraction<BigInteger>(i, 1, integerDomain),
                d=>d,
                (d, i) => d.Divide(i, integerDomain),
                fractionField);

            var fractionExpected = new Fraction<BigInteger>(n + 1, 2, integerDomain);
            var fractionActual = fracTarget.Run<Fraction<BigInteger>>(
                integerSequence,
                blockNumber,
                (j, k)=> new Fraction<BigInteger>(j,k, integerDomain),
                (d1, d2) => d1.Multiply(d2, integerDomain));

            Assert.AreEqual(fractionExpected, fractionActual);

            // Teste com alteração da função directa
            fracTarget.DirectFunction = i => new Fraction<BigInteger>(new BigInteger(i) * i, 1, integerDomain);
            fractionExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, integerDomain);
            fractionActual = fracTarget.Run<Fraction<BigInteger>>(
                integerSequence,
                blockNumber,
                (j, k) => new Fraction<BigInteger>(j, k, integerDomain),
                (d1, d2) => d1.Multiply(d2, integerDomain));

            // Teste com transformação
            var transformedTarget = new ListGeneralizedMeanAlgorithm<BigInteger, Fraction<BigInteger>>(
                i => new Fraction<BigInteger>(i, 1, integerDomain),
                d=>d,
                (d, i) => d.Divide(i, integerDomain),
                fractionField);
            var transformedSeq = new TransformList<int, BigInteger>(
                integerSequence,
                i => new BigInteger(i) * i);
            var transformedExpected = new Fraction<BigInteger>(
                (new BigInteger(n) + BigInteger.One) * (2 * new BigInteger(n) + 1), 6, integerDomain);
            var transformedActual = transformedTarget.Run<Fraction<BigInteger>>(
                transformedSeq,
                blockNumber,
                (j, k) => new Fraction<BigInteger>(j, k, integerDomain),
                (d1, d2) => d1.Multiply(d2, integerDomain));
            Assert.AreEqual(transformedExpected, transformedActual);
        }

        /// <summary>
        /// Testa a função tau de correlação.
        /// </summary>
        [Description("Tests the tau correlation function.")]
        [TestMethod]
        public void Statistics_TauCorrelationTest()
        {
            var mergeSorter = new MergeSorter<int>();
            var target = new TauCorrelation<int, int, double>(
                (a, c) => Array.Sort(a, c),
                (a, c) => mergeSorter.SortCountSwaps(a),
                (v, q) => v / q,
                v => Math.Sqrt(v));

            // Correlação perfeita
            var n = 100;
            var firstColumn = new int[n];
            var secondColumn = new int[n];
            for (var i = 0; i < n; ++i)
            {
                firstColumn[i] = i;
                secondColumn[i] = i;
            }

            var result = target.Run(
                firstColumn,
                secondColumn,
                Comparer<int>.Default,
                Comparer<int>.Default);

            Assert.AreEqual(1.0, result);

            // Correlação contrária
            for (var i = 0; i < n; ++i)
            {
                firstColumn[i] = n - i - 1;
                secondColumn[i] = i;
            }

            result = target.Run(
                firstColumn,
                secondColumn,
                Comparer<int>.Default,
                Comparer<int>.Default);

            Assert.AreEqual(0.0, result);
        }

        /// <summary>
        /// Testa o algoritmo que permite determinar a mediana com base
        /// num dicionário ordenado.
        /// </summary>
        [Description("Tests the dictionary median algorithm.")]
        [TestMethod]
        public void Statistics_DicMedianAlgTest()
        {
            var target = new DicMedianAlgorithm<int>();
            var source = new IntegerSequence();

            var expected = 1;
            for (var i = 1; i < 1000; i += 2)
            {
                source.Clear();
                source.Add(1, i);
                var actual = target.Run(source);
                Assert.AreEqual(expected, actual.Item1);
                Assert.AreEqual(expected, actual.Item2);

                ++expected;
            }

            expected = 1;
            for (var i = 2; i < 1000; i += 2)
            {
                source.Clear();
                source.Add(1, i);
                var actual = target.Run(source);
                Assert.AreEqual(expected++, actual.Item1);
                Assert.AreEqual(expected, actual.Item2);
            }

            var arraySource = new int[] { 1, 3, 2, 2, 1, 3, 3, 3, 3, 4, 2 };
            expected = 3;
            var outerActual = target.Run(arraySource);
            Assert.AreEqual(expected, outerActual.Item1);
            Assert.AreEqual(expected, outerActual.Item2);

            arraySource = new int[] { 1, 3, 2, 2, 1, 3, 3, 3, 4, 2 };
            expected = 2;
            outerActual = target.Run(arraySource);
            Assert.AreEqual(expected++, outerActual.Item1);
            Assert.AreEqual(expected, outerActual.Item2);
        }

        /// <summary>
        /// Testa o algoritmo que permite determinar a moda com base
        /// num dicionário.
        /// </summary>
        [Description("Tests the dictionary mode algorithm.")]
        [TestMethod]
        public void Statistics_DicModeAlgTest()
        {
            var target = new DicModeAlgorithm<int>();
            var source = new IntegerSequence();
            for (var i = 0; i < 1000; ++i)
            {
                source.Clear();
                source.Add(0, i);
                var innerActual = target.Run(source);
                Assert.AreEqual(source.Count, innerActual.Count);

                innerActual.Sort();
                for (var j = 0; j < source.Count; ++j)
                {
                    Assert.AreEqual(source[j], innerActual[j]);
                }
            }

            var arraySource = new int[] { 1, 3, 2, 2, 1, 3, 3, 3, 3, 4, 2 };
            var expected = 3;
            var actual = target.Run(arraySource);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(expected, actual[0]);
        }
    }
}
