namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

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

    /// <summary>
    /// Permite realizar testes sobre uma lista de bits.
    /// </summary>
    [TestClass]
    public class BitListTest
    {
        /// <summary>
        /// Testa as funções de escrita e leitura relativas à representação textual
        /// das listas de bits como números inteiros sem sinal.
        /// </summary>
        [Description("Tests the number text representation read and write.")]
        [TestMethod]
        public void BitList_NumberReadWriteTextTest()
        {
            var textExps = new[] { 
                "100000001",
                "0",
                "198174651938745138169854193845083450139846050298710285720345709345134",
                "530498734",
                "2345978654197234651938745"
            };

            for (int i = 0; i < textExps.Length; ++i)
            {
                var expected = textExps[i];
                var current = BitList.ReadNumeric(expected);
                var actual = current.ToNumericString();
                Assert.AreEqual(expected, actual);
            }
        }
    }

    /// <summary>
    /// Permite realizar testes sobre os algoritmos de ordenação.
    /// </summary>
    [TestClass]
    public class SortersTests
    {
        /// <summary>
        /// Testa o algoritmo de ordenação baseado numa pilha.
        /// </summary>
        [Description("Tests the heap sort function.")]
        [TestMethod]
        public void HeapSorter_SortTest()
        {
            // Testa o algoritmo que utiliza uma pilha.
            var heapSorter = new HeapSorter<uint>();
            this.TestSort(heapSorter);
        }

        /// <summary>
        /// Testa o algoritmo de ordenação por borbulhamento.
        /// </summary>
        [Description("Tests the bubble sort function")]
        [TestMethod]
        public void BubbleSorter_SortTest()
        {
            var bubbleSorter = new BubbleSorter<uint>();
            this.TestSort(bubbleSorter);
        }

        /// <summary>
        /// Testa a função de ordenação por fusão de sub-colecções.
        /// </summary>
        [Description("Tests the merge sort function")]
        [TestMethod]
        public void MergeSorter_SortTest()
        {
            var mergeSorter = new MergeSorter<uint>();
            this.TestSort(mergeSorter);
        }

        /// <summary>
        /// Testa a função de ordenação rápida.
        /// </summary>
        [Description("Tests the quick sort function.")]
        [TestMethod]
        public void QuickSorter_SortTest()
        {
            var quickSorter = new QuickSorter<uint>();
            this.TestSort(quickSorter);
        }

        /// <summary>
        /// Testa o algoritmo de ordenação por inserção.
        /// </summary>
        [Description("Tests the insertion sort function.")]
        [TestMethod]
        public void InsertionSorter_SortTest()
        {
            var insertionSorter = new InsertionSorter<uint>();
            this.TestSort(insertionSorter);
        }

        /// <summary>
        /// Testa o algoritmo de ordenação por pesquisa binária.
        /// </summary>
        [Description("Tests the binary search insertion sort function.")]
        [TestMethod]
        public void BinarySearchInsertionSorter_SortTest()
        {
            var binarySearchInsertionSorter = new BinarySearchInsertionSorter<uint>();
            this.TestSort(binarySearchInsertionSorter);
        }

        /// <summary>
        /// Testa a função de ordenação para inteiros.
        /// </summary>
        [Description("Tests the integer counting sorter sort function.")]
        [TestMethod]
        public void CountingSorter_SortTest()
        {
            var countingSorter = new CountingSorter();
            this.TestSort(countingSorter);
        }

        /// <summary>
        /// Testa o algoritmo de ordenação por contagem.
        /// </summary>
        [Description("Tests the counting sorter sort funtion.")]
        [TestMethod]
        public void CountingSorterT_SortTest()
        {
            var countingSorter = new CountingSorter<uint>(i => i);
            this.TestSort(countingSorter);
        }

        /// <summary>
        /// Testa o algoritmo iterativo da ordenação lexicográfica baseada na
        /// divisão.
        /// </summary>
        [Description("Tests the iterative LSD radix sorter sort function.")]
        [TestMethod]
        public void IterativeLsdRadixSorter_SortTest()
        {
            var target = new IterativeLsdRadixSorter(3);
            this.TestSort(target);

            target = new IterativeLsdRadixSorter(2);
            this.TestSort(target);

            target = new IterativeLsdRadixSorter(100);
            this.TestSort(target);
        }

        /// <summary>
        /// Testa a função de ordenação lexicográfica baseada na árvore associativa.
        /// </summary>
        [Description("Tests the trie based lexicographic sorter sort function.")]
        [TestMethod]
        public void TrieLexicographicCollectionSorter_SortTest()
        {
            var target = new TrieLexicographicCollectionSorter<char, string>(
                s => s);
            var comparer = StringComparer.InvariantCulture;
            this.TestSort(target, comparer);
        }

        /// <summary>
        /// Testa a função de ordenação lexicográfica dependente do comprimento
        /// baseada na árvore associativa.
        /// </summary>
        [Description("Tests the trie based shortlex sorter sort function.")]
        [TestMethod]
        public void TrieShortLexCollectionSorter_SortTest()
        {
            var target = new TrieLexicographicCollectionSorter<char, string>(
                s => s,
                TrieLexicographicCollectionSorter<char, string>.OrderingType.SHORTLEX);
            var comparer = new CollectionShortLexComparer<char>();
            this.TestSort(target, comparer);
        }

        /// <summary>
        /// Testa a ordenação de inteiros com base na ordenação lexicográfica dependente
        /// do comprimento e na decomposição dos números inteiros de acordo com uma base
        /// especificada.
        /// </summary>
        /// <remarks>
        /// Trata-se do exemplo de aplicação da árvore associativa à ordenação de inteiros.
        /// Convém notar que a implementação que resulta é eficiente apenas para valores pequenos
        /// da base.
        /// </remarks>
        [Description("Tests the integer sort based on shortlex trie sort function.")]
        [TestMethod]
        public void TrieShortLexCollectionSorter_IntegerSortTest()
        {
            var integerEnumerable = new UintRadixEnumerator(3, 0);

            // O enumerável definido será sempre reutilizado durante o processo.
            var target = new TrieLexicographicCollectionSorter<uint, uint>(
                i =>
                {
                    integerEnumerable.Number = i;
                    return integerEnumerable;
                },
                TrieLexicographicCollectionSorter<uint, uint>.OrderingType.SHORTLEX);

            this.TestSort(target);
        }

        /// <summary>
        /// Testa qualquer algoritmo de ordenação desde que seja proporcionado
        /// por uma determinada interface.
        /// </summary>
        /// <param name="sorter">O ordenador a ser testado.</param>
        private void TestSort(ISorter<uint> sorter)
        {
            // Nenhum elemento na colecção - o teste consiste em não se dar erro.
            var collection = new uint[0];
            sorter.Sort(collection);

            // Um elemento na colecção
            collection = new uint[1] { 0 };
            sorter.Sort(collection);
            Assert.AreEqual(0U, collection[0]);

            // Dois elementos na colecção na ordem inversa
            collection = new uint[] { 2, 1 };
            sorter.Sort(collection);
            Assert.AreEqual(1U, collection[0]);
            Assert.AreEqual(2U, collection[1]);

            // Todos diferentes mas ordenados
            collection = new uint[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            sorter.Sort(collection);
            this.AssertOrdered(collection);

            //Todos diferentes com ordenação arbitrária
            collection = new uint[] { 6, 2, 7, 4, 5, 6, 9, 0, 1, 3, 8 };
            sorter.Sort(collection);
            this.AssertOrdered(collection);

            // Com repetidos mas ordenados
            collection = new uint[] { 0, 0, 1, 1, 1, 2, 2, 3, 3, 3, 3 };
            sorter.Sort(collection);
            this.AssertOrdered(collection);

            // Com repetidos mas desordenados
            collection = new uint[] { 0, 1, 0, 1, 2, 3, 2, 1, 3, 3, 3 };
            sorter.Sort(collection);
            this.AssertOrdered(collection);
        }

        /// <summary>
        /// Testa qualquer algoritmo de ordenação no que concerne ao texto.
        /// </summary>
        /// <param name="sorter">O ordenador.</param>
        /// <param name="comparer">O comparador.</param>
        private void TestSort(
            ISorter<string> sorter,
            IComparer<string> comparer)
        {
            var collection = new string[0];
            sorter.Sort(collection);

            // Um elemento vazio na colecção
            collection = new[] { string.Empty };
            sorter.Sort(collection);
            Assert.AreEqual(string.Empty, collection[0]);

            // Um elemento não vazio na colecção
            collection = new[] { "abc" };
            sorter.Sort(collection);
            Assert.AreEqual("abc", collection[0]);

            // Dois elementos na ordem inversa
            collection = new[] { "abc", string.Empty };
            sorter.Sort(collection);
            this.AssertOrdered(collection, comparer);

            // Elementos na ordem correcta com a ordenação habitual
            collection = new[]{
                "aa",
                "aa",
                "aa",
                "ab",
                "b",
                "baa",
                "bac",
                "c",
                "cabcd",
                "cb"
            };

            sorter.Sort(collection);
            this.AssertOrdered(collection, comparer);

            // Elementos na ordem correcta com a ordenação por comprimento
            collection = new[]{
                "b",
                "c",
                "aa",
                "aa",
                "aa",
                "ab",
                "cb",
                "baa",
                "bac",
                "cabcd"
            };

            sorter.Sort(collection);
            this.AssertOrdered(collection, comparer);

            // Elementos numa ordem arbitrária
            collection = new[]{
                "bac",
                "c",
                "aa",
                "aa",
                "ab",
                "aa",
                "b",
                "cabcd",
                "cb",
                "baa",
                "aab",
                "baa",
                "baa",
                "acb",
                "aa",
                "b",
                "b"
            };

            sorter.Sort(collection);
            this.AssertOrdered(collection, comparer);
        }

        /// <summary>
        /// Função que permite averiguar se um vector se encontra ordenado.
        /// </summary>
        /// <param name="collection">O vector.</param>
        private void AssertOrdered(uint[] collection)
        {
            var count = collection.Length;
            var i = 0;
            if (i < count)
            {
                var current = collection[0];
                ++i;
                for (; i < count; ++i)
                {
                    var next = collection[i];
                    Assert.IsTrue(current <= next);
                    current = next;
                }
            }
        }

        /// <summary>
        /// Função que permite averiguar se um vector se encontra ordenado.
        /// </summary>
        /// <param name="collection">O vector.</param>
        /// <param name="comparer">O comparador de elementos.</param>
        private void AssertOrdered<T>(
            T[] collection,
            IComparer<T> comparer)
        {
            var count = collection.Length;
            var i = 0;
            if (i < count)
            {
                var current = collection[0];
                ++i;
                for (; i < count; ++i)
                {
                    var next = collection[i];
                    Assert.IsTrue(comparer.Compare(current, next) <= 0);
                    current = next;
                }
            }
        }
    }

    /// <summary>
    /// Testa as colecções generalizadas.
    /// </summary>
    [TestClass]
    public class GeneralizedCollectionsTest
    {
        /// <summary>
        /// Efectua um teste sobre as funções da pilha de dupla entrada.
        /// </summary>
        [Description("Tests the dequeue collection.")]
        [TestMethod]
        public void GeneralizedCollections_DequeueTest()
        {
            var target = new Deque<int>(4);
            var expected = new List<int>();

            CollectionAssert.AreEqual(
                expected, 
                target);

            // Inserção no final.
            var n = 10000;
            for (var i = 0; i < n; ++i)
            {
                target.EnqueueBack(i);
                expected.Add(i);
            }

            CollectionAssert.AreEqual(expected, target);

            target.Clear();
            expected.Clear();
            CollectionAssert.AreEqual(expected, target);

            // Inserção ao início
            for (var i = 0; i < n; ++i)
            {
                target.EnqueueFront(i);
                expected.Insert(0, i);
            }

            CollectionAssert.AreEqual(expected, target);

            target.Clear();
            expected.Clear();

            // Inserção alternada
            var front = true;
            for (var i = 0; i < n; ++i)
            {
                if (front)
                {
                    front = false;
                    target.EnqueueBack(i);
                    expected.Add(i);
                }
                else
                {
                    front = true;
                    target.EnqueueFront(i);
                    expected.Insert(0, i);
                }
            }

            CollectionAssert.AreEqual(expected, target);

            // Testa a função de inserção
            for (var i = 0; i < n; ++i)
            {
                target.Insert(i, i);
                expected.Insert(i, i);
            }

            CollectionAssert.AreEqual(expected, target);

            // Testa a função de remoção
            for (var i = 0; i < n; ++i)
            {
                target.RemoveAt(i);
                expected.RemoveAt(i);
            }

            CollectionAssert.AreEqual(expected, target);

            for (var i = 0; i < n; ++i)
            {
                var curr = target.IndexOf(i);
                var exp = expected.IndexOf(i);
                Assert.AreEqual(exp, curr);
            }

            var array = new int[target.Count];
            target.CopyTo(array, 0);
            CollectionAssert.AreEqual(expected, array);

            // Teste das funções de obtenção dos valores
            target.Clear();
            target.Add(0);
            var temp = target.PeekBack();
            Assert.AreEqual(0, temp);
            temp = target.PeekFront();
            Assert.AreEqual(0, temp);
            for (int i = 1; i < n; ++i)
            {
                target.Add(i);
                temp = target.PeekBack();
                Assert.AreEqual(i, temp);
                target.Insert(-1, i);
                temp = target.PeekFront();
                Assert.AreEqual(i, temp);
            }
        }
        
        /// <summary>
        /// Testa a colecção tipo meda.
        /// </summary>
        [Description("Tests the heap collection.")]
        [TestMethod]
        public void GeneralizedCollections_HeapTest()
        {
            var array = new[] { 
                1, 5, 2, 6, 3, 4, 
                3, 2, 6, 4, 7, 6, 
                3, 4, 7, 3 };
            var target = new Heap<int>(array);
            Assert.AreEqual(array.Length, target.Count);

            // Testa à função de obtenção do mínimo
            Array.Sort(array);
            var i = 0;
            while (target.Count > 0)
            {
                var min = target.PopRoot();
                Assert.AreEqual(array[i], min);
                ++i;
            }

            target = new Heap<int>(array);
            var actual = target.Contains(3);
            Assert.IsTrue(actual);
            actual = target.Contains(10);
            Assert.IsFalse(actual);

            actual = target.Remove(7);
            Assert.IsTrue(actual);
            actual = target.Contains(7);
            Assert.IsTrue(actual);
            actual = target.Remove(7);
            Assert.IsTrue(actual);
            actual = target.Contains(7);
            Assert.IsFalse(actual);
            var root = target.Root;
            Assert.AreEqual(root, array[0]);
            actual = target.Remove(array[0]);
            Assert.IsTrue(actual);
            Assert.AreEqual(array[1], target.Root);

            var oldCount = target.Count;
            target.Add(0);
            Assert.AreEqual(oldCount + 1, target.Count);
            Assert.AreEqual(0, target.Root);
        }

        /// <summary>
        /// Testa a atribuição da ordenação generalizada em ambos os sentidos.
        /// </summary>
        [Description("Tests the long system array attribution.")]
        [TestMethod]
        public void GeneralizedCollections_LongSystemArrayGeneralTest()
        {
            // Testa a nulidade de uma atribuição.
            int[] array = null;
            LongSystemArray<int> longArray = array;
            Assert.IsNull(longArray);
            Assert.IsTrue(array == longArray);
            Assert.IsFalse(array != longArray);
            Assert.IsTrue(longArray == array);
            Assert.IsFalse(longArray != array);
            var otherArray = array;
            Assert.IsTrue(otherArray == longArray);
            Assert.IsFalse(otherArray != longArray);

            // Testa a atribuição.
            array = new int[5];
            var random = new Random();
            for (int i = 0; i < 5; ++i)
            {
                array[i] = random.Next();
            }

            longArray = array;
            Assert.AreEqual(5, longArray.Count);
            Assert.AreEqual(5L, longArray.LongCount);
            Assert.IsTrue(array == longArray);
            Assert.IsTrue(longArray == array);
            otherArray = array;
            Assert.IsTrue(otherArray == longArray);
            Assert.IsTrue(longArray.Equals(array));
            Assert.IsTrue(longArray.Equals(otherArray));
            Assert.AreEqual(array.GetHashCode(), longArray.GetHashCode());
            Assert.AreEqual(longArray.GetHashCode(), otherArray.GetHashCode());

            for (var i = 0; i < longArray.Count; ++i)
            {
                Assert.AreEqual(array[i], longArray[i]);
            }

            for (var i = 0L; i < longArray.LongCount; ++i)
            {
                Assert.AreEqual(array[i], longArray[i]);
            }
        }

        /// <summary>
        /// Efectua um teste geral à ordenação que pode assumir um tamanho
        /// arbitrário de acordo com uma arquitectura de 64 bits.
        /// </summary>
        [Description("Tests the general long array functions.")]
        [TestMethod]
        public void GeneralizedCollections_GeneralLongArrayGeneralTest()
        {
            GeneralLongArray<int>.MaxBinaryPower = 3;
            GeneralLongArray<int>.ObjMaxBinaryPower = 2;

            for (int i = 0; i < 50; ++i)
            {
                var target = new GeneralLongArray<int>(i, false);
                Assert.IsTrue(target.AssertSizes(), "The computed sizes don't match array configuration.");
                this.FillArray(target);
                this.AssertArray(target);
            }
        }

        /// <summary>
        /// Testa a função que permite copiar o conteúdo da ordenação geral para uma matriz.
        /// </summary>
        [Description("Tests the function that copies the contents of array to a matrix.")]
        [TestMethod]
        public void GeneralizedCollections_GeneralLongArrayCopyToMatrixTest()
        {
            GeneralLongArray<int>.MaxBinaryPower = 3;
            GeneralLongArray<int>.ObjMaxBinaryPower = 2;

            var firstLength = 5;
            var secondLength = 5;
            var thirdLength = 5;
            var array = new int[2 * firstLength][][];
            for (int i = 0; i < 2 * firstLength; ++i)
            {
                var innerArray = new int[secondLength][];
                array[i] = innerArray;
                for (var j = 0; j < secondLength; ++j)
                {
                    var current = new int[thirdLength];
                    innerArray[j] = current;
                }
            }

            var length = firstLength * secondLength * thirdLength;
            for (int i = 0; i <= length; ++i)
            {
                var target = new GeneralLongArray<int>(i, false);
                this.FillArray(target);

                var firstStartIndex = 0L;
                var secondStartIndex = 0L;
                var thirdStartIndex = 0L;

                while (firstStartIndex < firstLength)
                {

                    target.CopyTo(
                        array,
                        new long[] { firstStartIndex, secondStartIndex, thirdStartIndex });

                    var currentValue = 0;
                    var j = firstStartIndex;
                    var k = secondStartIndex;
                    var l = thirdStartIndex;
                    while (currentValue < i)
                    {
                        Assert.AreEqual(
                            currentValue,
                            array[j][k][l]);
                        ++currentValue;

                        ++l;
                        if (l == thirdLength)
                        {
                            l = 0;
                            ++k;
                            if (k == secondLength)
                            {
                                k = 0;
                                ++j;
                            }
                        }
                    }

                    ++thirdStartIndex;
                    if (thirdStartIndex == thirdLength)
                    {
                        thirdStartIndex = 0;
                        ++secondStartIndex;
                        if (secondStartIndex == secondLength)
                        {
                            secondStartIndex = 0;
                            ++firstStartIndex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Testa a função que estabelece a nova capacidade para uma lista geral.
        /// </summary>
        [Description("Tests the new capacity function.")]
        [TestMethod]
        public void GeneralizedCollections_GeneralLongListNewCapacityTest()
        {
            GeneralLongList<int>.MaxBinaryPower = 2;
            GeneralLongList<int>.ObjMaxBinaryPower = 1;

            var target = new GeneralLongList<int>(false);
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 20; ++j)
                {
                    target.Capacity = i;
                    Assert.IsTrue(target.AssertSizes(), "The computed sizes don't match array configuration.");
                    target.Capacity = j;
                    var assertion = target.AssertSizes();
                    Assert.IsTrue(assertion, "The computed sizes don't match array configuration.");
                }
            }
        }

        /// <summary>
        /// Testa as funções de adição e inserção de um novo item a uma lista geral.
        /// </summary>
        [Description("Tests the addition function for the generalized list.")]
        [TestMethod]
        public void GeneralizedCollections_GeneralLongListAddInsertTest()
        {
            GeneralLongList<int>.MaxBinaryPower = 6;
            GeneralLongList<int>.ObjMaxBinaryPower = 9;

            // Testa a função de adição
            var target = new GeneralLongList<int>(false);
            var count = 1050;
            for (int i = 0; i < count; ++i)
            {
                target.Add(i);
                target.AssertSizes();
                Assert.AreEqual(i + 1, target.Count);
                for (int j = 0; j <= i; ++j)
                {
                    Assert.AreEqual(j, target[j]);
                }
            }

            var list = new List<int>(target);

            var length = count;
            for (int i = 0; i < count; ++i)
            {
                length += 2;
                list.Insert(count - i - 1, i);
                list.Insert(i, i);
                target.Insert(count - i - 1, i);
                --count;
                target.Insert(i, i);
                target.AssertSizes();
                Assert.AreEqual(length, target.Count);
                for (int j = 0; j < list.Count; ++j)
                {
                    Assert.AreEqual(list[j], target[j]);
                }
            }
        }

        /// <summary>
        /// Testa a função que permite remover um item da lista que se encontra
        /// na posição especificada.
        /// </summary>
        [Description("Tests the remove at function")]
        [TestMethod]
        public void GeneralizedCollections_GeneralLongListRemoveAtTest()
        {
            GeneralLongList<int>.MaxBinaryPower = 10;
            GeneralLongList<int>.ObjMaxBinaryPower = 11;

            var count = 10324;
            var target = new GeneralLongList<int>(count);
            for (int i = 0; i < count; ++i)
            {
                target.Add(i);
            }

            var list = new List<int>(target);
            var iterations = count >> 1;
            var comparisionValue = -1;
            for (var i = 0; i < iterations; ++i)
            {
                comparisionValue += 2;
                target.RemoveAt(i);
                --count;
                Assert.AreEqual(count, target.LongCount);
                Assert.AreEqual(comparisionValue, target[i]);
            }
        }

        /// <summary>
        /// Testa a função que permite copiar o conteúdo da ordenação geral para uma matriz.
        /// </summary>
        [Description("Tests the function that copies the contents of array to a matrix.")]
        [TestMethod]
        public void GeneralizedCollections_GeneralLongListCopyToMatrixTest()
        {
            GeneralLongArray<int>.MaxBinaryPower = 5;
            GeneralLongArray<int>.ObjMaxBinaryPower = 5;

            var firstLength = 6;
            var secondLength = 5;
            var thirdLength = 5;
            var array = new int[2 * firstLength][][];
            for (int i = 0; i < 2 * firstLength; ++i)
            {
                var innerArray = new int[secondLength][];
                array[i] = innerArray;
                for (var j = 0; j < secondLength; ++j)
                {
                    var current = new int[thirdLength];
                    innerArray[j] = current;
                }
            }

            var target = new GeneralLongList<int>();
            var length = firstLength * secondLength * thirdLength;
            for (int i = 0; i <= length; ++i)
            {
                target.Add(i);

                var firstStartIndex = 0L;
                var secondStartIndex = 0L;
                var thirdStartIndex = 0L;

                while (firstStartIndex < firstLength)
                {

                    target.CopyTo(
                        array,
                        new long[] { firstStartIndex, secondStartIndex, thirdStartIndex });

                    var currentValue = 0;
                    var j = firstStartIndex;
                    var k = secondStartIndex;
                    var l = thirdStartIndex;
                    while (currentValue < i)
                    {
                        Assert.AreEqual(
                            currentValue,
                            array[j][k][l]);
                        ++currentValue;

                        ++l;
                        if (l == thirdLength)
                        {
                            l = 0;
                            ++k;
                            if (k == secondLength)
                            {
                                k = 0;
                                ++j;
                            }
                        }
                    }

                    ++thirdStartIndex;
                    if (thirdStartIndex == thirdLength)
                    {
                        thirdStartIndex = 0;
                        ++secondStartIndex;
                        if (secondStartIndex == secondLength)
                        {
                            secondStartIndex = 0;
                            ++firstStartIndex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Preenche cada entrada da ordenação com o valor correspondente ao seu índice.
        /// </summary>
        /// <param name="generalArray">A ordenação a ser preenchida.</param>
        private void FillArray(GeneralLongArray<int> generalArray)
        {
            for (var i = 0; i < generalArray.Count; ++i)
            {
                generalArray[i] = i;
            }
        }

        /// <summary>
        /// Verifica se o valor atribuído a cada entrada de uma ordenação coincide com
        /// o respectivo índice.
        /// </summary>
        /// <param name="generalArray">A ordenação.</param>
        private void AssertArray(GeneralLongArray<int> generalArray)
        {
            for (var i = 0; i < generalArray.Count; ++i)
            {
                Assert.AreEqual(i, generalArray[i]);
            }
        }

        // <summary>
        /// Preenche cada entrada da lista com o valor correspondente ao seu índice.
        /// </summary>
        /// <param name="generalList">A lista a ser preenchida.</param>
        private void FillList(GeneralLongList<int> generalList)
        {
            for (var i = 0; i < generalList.Count; ++i)
            {
                generalList[i] = i;
            }
        }

        /// <summary>
        /// Verifica se o valor atribuído a cada entrada de uma lista coincide com
        /// o respectivo índice.
        /// </summary>
        /// <param name="generalList">A lista.</param>
        private void AsserList(GeneralLongList<int> generalList)
        {
            for (var i = 0; i < generalList.Count; ++i)
            {
                Assert.AreEqual(i, generalList[i]);
            }
        }
    }

    /// <summary>
    /// Define um enumerador que permite realizar um teste sobre a ordenação 
    /// de inteiros com base numa árvore associativa.
    /// </summary>
    public class UintRadixEnumerator : IEnumerable<uint>
    {
        /// <summary>
        /// Obtém a base da decomposição.
        /// </summary>
        private uint radix;

        /// <summary>
        /// O número relativo ao iterador.
        /// </summary>
        private uint number;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintRadixEnumerator"/>.
        /// </summary>
        /// <param name="radix">A base da decompsição.</param>
        /// <param name="number">O número a ser decomposto.</param>
        public UintRadixEnumerator(uint radix, uint number)
        {
            if (radix < 2)
            {
                throw new ArgumentOutOfRangeException("Radix must be a number greater than 1.");
            }
            else
            {
                this.radix = radix;
                this.number = number;
            }
        }

        /// <summary>
        /// Obtém ou atribui a base da decomposição.
        /// </summary>
        public uint Radix
        {
            get
            {
                return this.radix;
            }
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException("Radix must be a number greater than 1.");
                }
                else
                {
                    this.radix = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o número do iterador.
        /// </summary>
        public uint Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = value;
            }
        }

        /// <summary>
        /// Obtém o enumerador para o enumerável.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<uint> GetEnumerator()
        {
            var innerRadix = this.radix;
            var innerNumber = this.number;
            var aux = new List<uint>();
            while (innerNumber > 0)
            {
                aux.Add(innerNumber % radix);
                innerNumber /= innerRadix;
            }

            for (int i = aux.Count - 1; i > -1; --i)
            {
                yield return aux[i];
            }
        }

        /// <summary>
        /// Otbém o enumerador não genérico para o enumerável.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
