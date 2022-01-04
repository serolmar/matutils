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
            }

            //var first = new LongSystemArray<char>(new char[] { 'A', 'G', 'G', 'T', 'A', 'B' });
            //var second = new LongSystemArray<char>(new char[] { 'G', 'X', 'T', 'X', 'A', 'Y', 'B' });

            //var result = target.Run(first, second, '_', '_');
            //var resEnum = result.GetEnumerator();
            //var subSeqs = new List<Tuple<ILongList<char>,ILongList<char>>>();
            //while (resEnum.MoveNext())
            //{
            //    subSeqs.Add(resEnum.Current);
            //}

            //var expected = new GeneralLongList<char[]>();
            //expected.Add(new char[] { 'G', 'T', 'A', 'B' });
            //expected.Add(new char[] { 'G', 'T', 'A', 'B' });

            //for (var i = 0; i < 2; ++i)
            //{
            //    for (var j = 0; j < 4; ++j)
            //    {
            //        Assert.AreEqual(expected[i][j], subSeqs[i][j]);
            //    }
            //}

            //first = new LongSystemArray<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F' });
            //second = new LongSystemArray<char>(new char[] { 'F', 'E', 'D', 'C', 'B', 'A' });

            //result = target.Run(first, second);
            //resEnum = result.GetEnumerator();
            //subSeqs = new List<Tuple<ILongList<char>, ILongList<char>>>();
            //while (resEnum.MoveNext())
            //{
            //    subSeqs.Add(resEnum.Current);
            //}

            //Assert.AreEqual(6, subSeqs.Count);
            //var actual = new char[6];
            //for (var i = 0; i < 6; ++i)
            //{
            //    actual[i] = subSeqs[i][0];
            //}

            //Assert.AreEqual(second.LongCount, actual.LongLength);
            //for (var i = 0L; i < actual.LongLength; ++i)
            //{
            //    Assert.AreEqual(second[i], actual[i]);
            //}

            //first = new LongSystemArray<char>(new[] { 'T', 'G', 'C', 'A', 'T', 'A'});
            //second = new LongSystemArray<char>(new[] { 'A', 'T', 'A', 'T', 'G', 'C' });

            //result = target.Run(first, second);
            //resEnum = result.GetEnumerator();
            //subSeqs = new List<char[]>();
            //while (resEnum.MoveNext())
            //{
            //    subSeqs.Add(resEnum.Current);
            //}
        }
    }
}
