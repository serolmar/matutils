namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;
    using Utilities.Collections;

    /// <summary>
    /// Testa a classe de inserção de elementos ordenados.
    /// </summary>
    [TestClass]
    public class InsertionSortedColletionTest
    {
        /// <summary>
        /// Testa a função de adição de elementos quando os valores repetidos não são ignorados.
        /// </summary>
        [TestMethod]
        [Description("Tests the addition function when repeated aren't being ignored.")]
        public void InsertionSortedCollection_AddNotIgnoringRepeatedTest()
        {
            var insertionValues = new[] { 4, 2, 1, 6, 3, 5, 7, 4, 6, 0, 0, 8, 9, 8, 6, 7 };
            var comparer = Comparer<int>.Default;
            var target = new InsertionSortedCollection<int>(comparer, false);
            for (int i = 0; i < insertionValues.Length; ++i)
            {
                target.Add(insertionValues[i]);
            }

            Assert.AreEqual(insertionValues.Length, target.Count);
            this.AssertOrder(target, comparer);
        }

        /// <summary>
        /// Testa a função de adição de elementos quando os valores repetidos são ignorados.
        /// </summary>
        [TestMethod]
        [Description("Tests the addition function when repeated are being ignored.")]
        public void InsertionSortedCollection_AddIgnoringRepeatedTest()
        {
            var insertionValues = new[] { 4, 2, 1, 6, 3, 5, 7, 4, 6, 0, 0, 8, 9, 8, 6, 7 };
            var comparer = Comparer<int>.Default;
            var target = new InsertionSortedCollection<int>(comparer, true);
            for (int i = 0; i < insertionValues.Length; ++i)
            {
                target.Add(insertionValues[i]);
            }

            Assert.AreEqual(10, target.Count);
            this.AssertOrder(target, comparer);
        }

        /// <summary>
        /// Testa a função de adição de elementos quando os valores repetidos não são ignorados,
        /// tendo em conta a estabilidade da inserção.
        /// </summary>
        [TestMethod]
        [Description("Tests the addition function when repeated aren't being ignored.")]
        public void InsertionSortedCollection_AddStableSortTest()
        {
            var insertionValues = new[] 
            {
                Tuple.Create(4,0),
                Tuple.Create(2,0),
                Tuple.Create(1,0),
                Tuple.Create(6,0),
                Tuple.Create(3,0),
                Tuple.Create(5,0),
                Tuple.Create(7,0),
                Tuple.Create(4,1),
                Tuple.Create(6,1),
                Tuple.Create(0,0),
                Tuple.Create(0,1),
                Tuple.Create(8,0),
                Tuple.Create(9,0),
                Tuple.Create(8,1),
                Tuple.Create(9,1),
                Tuple.Create(6,2),
                Tuple.Create(7,1),
            };

            var comparer = new RepresentativeComparer<Tuple<int, int>, int>(
                t => t.Item1,
                Comparer<int>.Default);
            var target = new InsertionSortedCollection<Tuple<int, int>>(comparer, false);
            for (int i = 0; i < insertionValues.Length; ++i)
            {
                target.Add(insertionValues[i]);
            }

            // Verifica a ordenação.
            Assert.AreEqual(insertionValues.Length, target.Count);
            this.AssertOrder(target, comparer);

            // Verifica a estabilidade
            var collectionEnum = target.GetEnumerator();
            if (collectionEnum.MoveNext())
            {
                var previous = collectionEnum.Current;
                while (collectionEnum.MoveNext())
                {
                    var current = collectionEnum.Current;
                    if (comparer.Compare(previous, current) == 0)
                    {
                        Assert.IsTrue(previous.Item2 < current.Item2);
                    }

                    previous = current;
                }
            }
        }

        /// <summary>
        /// Testa a função que permite verificar se um elemento está contido na colecção.
        /// </summary>
        [TestMethod]
        [Description("Tests the contains function.")]
        public void InsertionSortedCollection_ContainsTest()
        {
            var insertionValues = new[] { 4, 2, 1, 6, 3, 5, 7, 4, 6, 0, 0, 8, 9, 8, 6, 7, 12, 25 };
            var comparer = Comparer<int>.Default;
            var target = new InsertionSortedCollection<int>(comparer, false);
            for (int i = 0; i < insertionValues.Length; ++i)
            {
                target.Add(insertionValues[i]);
            }

            for (int i = 0; i < 10; ++i)
            {
                Assert.IsTrue(target.Contains(insertionValues[i]));
            }

            Assert.IsFalse(target.Contains(11));
            Assert.IsFalse(target.Contains(13));
        }

        /// <summary>
        /// Testa a função que permite remover um elemento da colecção.
        /// </summary>
        [TestMethod]
        [Description("Tests the remove function.")]
        public void InsertionSortedCollection_RemoveTest()
        {
            var insertionValues = new[] 
            {
                Tuple.Create(4,0),
                Tuple.Create(2,0),
                Tuple.Create(1,0),
                Tuple.Create(6,0),
                Tuple.Create(3,0),
                Tuple.Create(5,0),
                Tuple.Create(7,0),
                Tuple.Create(4,1),
                Tuple.Create(6,1),
                Tuple.Create(0,0),
                Tuple.Create(0,1),
                Tuple.Create(8,0),
                Tuple.Create(9,0),
                Tuple.Create(8,1),
                Tuple.Create(9,1),
                Tuple.Create(6,2),
                Tuple.Create(7,1),
            };

            var comparer = new RepresentativeComparer<Tuple<int, int>, int>(
                t => t.Item1,
                Comparer<int>.Default);
            var target = new InsertionSortedCollection<Tuple<int, int>>(comparer, false);
            for (int i = 0; i < insertionValues.Length; ++i)
            {
                target.Add(insertionValues[i]);
            }

            var firstValue = insertionValues[7];
            var secondValue = insertionValues[3];
            target.Remove(firstValue);
            target.Remove(secondValue);

            Assert.AreEqual(insertionValues.Length - 2, target.Count);
            foreach (var item in target)
            {
                if (item.Item1 == firstValue.Item1)
                {
                    Assert.AreNotEqual(item.Item2, 0);
                }
                else if (item.Item1 == secondValue.Item1)
                {
                    Assert.AreNotEqual(item.Item2, 0);
                }
            }
        }

        /// <summary>
        /// Testa a função que permite contar o número de elementos na colecção
        /// cujo valor é inferior ao elemento especificado.
        /// </summary>
        [TestMethod]
        [Description("Tests the count less than function.")]
        public void InsertionSortedCollection_CountLessThanTest()
        {
            var insertionValues = new[] { 4, 2, 1, 6, 3, 5, 7, 4, 6, 0, 0, 8, 9, 8, 6, 7, 13, 14 };
            var comparer = Comparer<int>.Default;
            var target = new InsertionSortedCollection<int>(comparer, false);
            for (int i = 0; i < insertionValues.Length; ++i)
            {
                target.Add(insertionValues[i]);
            }

            var counts = new[] { 0, 2, 3, 4, 5, 7, 8, 11, 13, 15 };
            for (int i = 0; i < counts.Length; ++i)
            {
                var actual = target.CountLessThan(i);
                Assert.AreEqual(counts[i], actual);
            }

            var auxActual = target.CountLessThan(-1);
            Assert.AreEqual(0, auxActual);

            auxActual = target.CountLessThan(11);
            Assert.AreEqual(16, auxActual);
        }

        /// <summary>
        /// Testa a função que permite contar o número de elementos na colecção
        /// cujo valor é superior ao elemento especificado.
        /// </summary>
        [TestMethod]
        [Description("Tests the remove function.")]
        public void InsertionSortedCollection_CountGreatThanTest()
        {
            var insertionValues = new[] { 4, 2, 1, 6, 3, 5, 7, 4, 6, 0, 0, 8, 9, 8, 6, 7 };
            var comparer = Comparer<int>.Default;
            var target = new InsertionSortedCollection<int>(comparer, false);
            for (int i = 0; i < insertionValues.Length; ++i)
            {
                target.Add(insertionValues[i]);
            }

            var counts = new[] { 14, 13, 12, 11, 9, 8, 5, 3, 1, 0 };
            for (int i = 0; i < counts.Length; ++i)
            {
                var actual = target.CountGreatThan(i);
                Assert.AreEqual(counts[i], actual);
            }
        }

        /// <summary>
        /// Averigua se todos os elementos se encontram ordenados.
        /// </summary>
        /// <typeparam name="T">
        /// O tipo do objectos que constituem as entradas da colecção.
        /// </typeparam>
        /// <param name="insertionSortedCollection">A colecção.</param>
        /// <param name="comparer">O comparador dos elementos.</param>
        private void AssertOrder<T>(
            InsertionSortedCollection<T> insertionSortedCollection,
            IComparer<T> comparer)
        {
            var collectionEnum = insertionSortedCollection.GetEnumerator();
            if (collectionEnum.MoveNext())
            {
                var previous = collectionEnum.Current;
                while (collectionEnum.MoveNext())
                {
                    var current = collectionEnum.Current;
                    Assert.IsTrue(comparer.Compare(previous, current) <= 0);

                    previous = current;
                }
            }
        }
    }

    /// <summary>
    /// Testa a classe que permite representar uma sequência de inteiros.
    /// </summary>
    [TestClass]
    public class IntegerSequenceTest
    {
        /// <summary>
        /// Testa a função que permite adicionar elementos à colecção.
        /// </summary>
        [Description("Tests the append function.")]
        [TestMethod]
        public void IntegerSequence_AddTest()
        {
            var elements = new int[] { 2, 3, 5, 1, 1, 0, 1, 9, 7, 8, 6, 6, 4 };
            var expected = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var target = new IntegerSequence();
            for (int i = 0; i < elements.Length; ++i)
            {
                target.Add(elements[i]);
                target.AssertIntegrity();
            }

            Assert.AreEqual(expected.Length, target.Count);
            for (int i = 0; i < expected.Length; ++i)
            {
                Assert.AreEqual(expected[i], target[i]);
            }

            var count = 5;
            target = new IntegerSequence();
            for (int i = 0; i < count; ++i)
            {
                target.Add(i);
                target.AssertIntegrity();
                target.Add(i);
                target.AssertIntegrity();
            }

            Assert.AreEqual(count, target.Count);
        }

        /// <summary>
        /// Testa a função que permite adicionar elementos à colecção segundo intervalos.
        /// </summary>
        [Description("Tests the append range function.")]
        [TestMethod]
        public void IntegerSequence_AddRangeTest()
        {
            var target = new IntegerSequence();
            target.Add(1, 5);
            Assert.AreEqual(5, target.Count);
            target.AssertIntegrity();
            target.Add(2, 3);
            Assert.AreEqual(5, target.Count);
            target.AssertIntegrity();
            target.Add(0, 0);
            Assert.AreEqual(6, target.Count);
            target.AssertIntegrity();
            target.Add(1, 9);
            Assert.AreEqual(10, target.Count);
            target.AssertIntegrity();

            target.Clear();
            target.Add(0, 4);
            target.AssertIntegrity();
            target.Add(10, 14);
            target.AssertIntegrity();
            Assert.AreEqual(10, target.Count);

            target.Clear();
            target.Add(0, 4);
            target.AssertIntegrity();
            target.Add(5, 9);
            target.AssertIntegrity();
            Assert.AreEqual(10, target.Count);
        }

        /// <summary>
        /// Testa a função que permite remover elementos da colecção.
        /// </summary>
        [Description("Tests the remove function.")]
        [TestMethod]
        public void IntegerSequence_RemoveTest()
        {
            var target = new IntegerSequence();
            var count = 10;
            target.Add(0, count - 1);
            Assert.AreEqual(count, target.Count);
            target.AssertIntegrity();
            target.Remove(count + 1);
            Assert.AreEqual(count, target.Count);

            var expected = count - 1;
            for (int i = 0; i < count; ++i)
            {
                target.Remove((i + 5) % count);
                Assert.AreEqual(expected--, target.Count);
                target.AssertIntegrity();
            }
        }

        /// <summary>
        /// Testa a função que permite remover elementos da colecção segundo intervalos.
        /// </summary>
        [Description("Tests the remove range function.")]
        [TestMethod]
        public void IntegerSequence_RemoveRangeTest()
        {
            var target = new IntegerSequence();
            target.Add(0, 9);
            Assert.AreEqual(10, target.Count);
            target.AssertIntegrity();
            target.Remove(5, 13);
            Assert.AreEqual(5, target.Count);
            target.AssertIntegrity();
            target.Remove(2, 3);
            Assert.AreEqual(3, target.Count);
            target.AssertIntegrity();
            target.Remove(-1, 5);
            Assert.AreEqual(0, target.Count);
            target.AssertIntegrity();

            target.Add(0, 4);
            target.Add(10, 14);
            Assert.AreEqual(10, target.Count);
            target.Remove(4, 10);
            Assert.AreEqual(8, target.Count);
            target.AssertIntegrity();
            target.Remove(4, 10);
            Assert.AreEqual(8, target.Count);
            target.AssertIntegrity();

            target.Clear();
            target.Add(1, 2);
            target.Add(4, 5);
            target.Remove(1, 5);
            Assert.AreEqual(0, target.Count);
            target.AssertIntegrity();

            target.Clear();
            target.Add(1, 2);
            target.Add(4, 5);
            target.Remove(0, 6);
            Assert.AreEqual(0, target.Count);
            target.AssertIntegrity();
        }

        /// <summary>
        /// Testa a função que averiguas e um determinado item está contido na colecção.
        /// </summary>
        [Description("Tests the contains function.")]
        [TestMethod]
        public void IntegerSequence_ContainsTest()
        {
            var target = new IntegerSequence();
            target.Add(0, 1);
            target.Add(4, 5);
            target.Add(8, 9);
            Assert.IsFalse(target.Contains(-1));
            Assert.IsTrue(target.Contains(0));
            Assert.IsTrue(target.Contains(1));
            Assert.IsFalse(target.Contains(2));
            Assert.IsFalse(target.Contains(3));
            Assert.IsTrue(target.Contains(4));
            Assert.IsTrue(target.Contains(5));
            Assert.IsFalse(target.Contains(6));
            Assert.IsFalse(target.Contains(7));
            Assert.IsTrue(target.Contains(8));
            Assert.IsTrue(target.Contains(9));
            Assert.IsFalse(target.Contains(10));
        }

        /// <summary>
        /// Testa a função que averiguar se um determinado intervalo está contido na colecção.
        /// </summary>
        [Description("Tests the range contains function.")]
        [TestMethod]
        public void IntegerSequence_ContainsRangeTest()
        {
            var target = new IntegerSequence();
            target.Add(0, 4);
            target.Add(10, 13);

            Assert.IsTrue(target.Contains(0, 4));
            Assert.IsTrue(target.Contains(10, 13));
            Assert.IsTrue(target.Contains(2, 3));
            Assert.IsFalse(target.Contains(-1, 5));
            Assert.IsFalse(target.Contains(2, 6));
            Assert.IsFalse(target.Contains(5, 9));
        }
    }
}
