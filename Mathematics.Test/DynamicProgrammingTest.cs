// -----------------------------------------------------------------------
// <copyright file="DynammicProgrammingTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    /// <summary>
    /// Efectua testes sobre as funções bioinformáticas.
    /// </summary>
    [TestClass]
    public class DynammicProgrammingTest
    {
        /// <summary>
        /// Testa a função que permite determinar o tamanho da maior subsequência
        /// comum a duas sequências.
        /// </summary>
        [Description("Tests the longest common sequence algorithm.")]
        [TestMethod]
        public void DynammicProgramming_LCSLengthTest()
        {
            var target = new LongestCommonSeqLen<char, char, int>(
                (c1, c2) => c1 == c2,
                new IntegerDomain());
            var first = new char[] { 'A', 'G', 'G', 'T', 'A', 'B' };
            var second = new char[] { 'G', 'X', 'T', 'X', 'A', 'Y', 'B' };

            var result = target.Run(first, second);
            Assert.AreEqual(4, result);
        }

        /// <summary>
        /// Testa a funcionalidade que permite obter todas as subsequências
        /// com maior comprimento comuns a duas sequências dadas.
        /// </summary>
        [Description("Tests the longest common sequence algorithm.")]
        [TestMethod]
        public void DynammicProgramming_AllLCSTest()
        {
            var target = new AllLongestCommonSequence<char, char>(
                (c1, c2) => c1 == c2);
            var repChar = '_';

            var first = new GeneralLongArray<char>(new char[] { 'T', 'G', 'T', 'A', 'T', 'A' });
            var second = new GeneralLongArray<char>(new char[] { 'A', 'T', 'A', 'T', 'G', 'T' });
            var result = target.Run(first, second, repChar, repChar);
            var resEnum = result.GetEnumerator();
            while (resEnum.MoveNext())
            {
                var current = resEnum.Current;
                this.AssertCommonSequence(
                    current.Item1, 
                    current.Item2, 
                    (c1, c2) => c1 == c2, 
                    3L);
            }

            first = new GeneralLongArray<char>(new char[] { 'A', 'G', 'G', 'T', 'A', 'B' });
            second = new GeneralLongArray<char>(new char[] { 'G', 'X', 'T', 'X', 'A', 'Y', 'B' });

            result = target.Run(first, second, '_', '_');
            resEnum = result.GetEnumerator();
            while (resEnum.MoveNext())
            {
                var current = resEnum.Current;
                this.AssertCommonSequence(
                    current.Item1,
                    current.Item2,
                    (c1, c2) => c1 == c2,
                    4L);
            }

            first = new GeneralLongArray<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F' });
            second = new GeneralLongArray<char>(new char[] { 'F', 'E', 'D', 'C', 'B', 'A' });

            result = target.Run(first, second, '_', '_');
            resEnum = result.GetEnumerator();
            while (resEnum.MoveNext())
            {
                var current = resEnum.Current;
                this.AssertCommonSequence(
                    current.Item1,
                    current.Item2,
                    (c1, c2) => c1 == c2,
                    1L);
            }
        }

        /// <summary>
        /// Determina se a subsequência comum possui determinado comprimento.
        /// </summary>
        /// <remarks>
        /// As sequências deverão ser iguais desde que o procedimentos esteja correcto.
        /// </remarks>
        /// <typeparam name="T">O tipo de objectos que constitui a primeira sequência.</typeparam>
        /// <typeparam name="P">O tipo de objectos que constitui a segunda sequência.</typeparam>
        /// <param name="first">A primeira sequência.</param>
        /// <param name="second">A segunda sequência.</param>
        /// <param name="comparision">O comparador de elementos da sequência.</param>
        /// <param name="count">O número esperado.</param>
        private void AssertCommonSequence<T, P>(
            GeneralLongArray<T> first,
            GeneralLongArray<P> second,
            Func<T, P, bool> comparision,
            long count)
        {
            var firstCount = first.LongCount;
            var secondCount = second.LongCount;
            Assert.AreEqual(firstCount, secondCount);

            var real = 0L; ;
            for (var i = 0L; i < firstCount; ++i)
            {
                var firstItem = first[i];
                var secondItem = second[i];
                if (comparision(firstItem, secondItem))
                {
                    ++real;
                }
            }

            Assert.AreEqual(count, real);
        }
    }
}
