namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DictionaryTrieTest
    {
        /// <summary>
        /// Fábrica responsável pela criação dos dicionários.
        /// </summary>
        private DictionaryEqualityComparerFactory<char, DictionaryTrie<char, string>.TrieNode> dicFactory =
            new DictionaryEqualityComparerFactory<char, DictionaryTrie<char, string>.TrieNode>(
                EqualityComparer<char>.Default);

        /// <summary>
        /// Permite testar a funcionalidade de indexação.
        /// </summary>
        [TestMethod]
        [Description("Tests the indexing functionality.")]
        public void DictionaryTrie_IndexerTest()
        {
            var values = this.GetTestValues();
            var target = new DictionaryTrie<char, string>(
                values,
                false,
                this.dicFactory);
            for (int i = 0; i < values.Length; ++i)
            {
                var expected = values[i];
                var actual = target[i];
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Permite testar a propriedade que indica a contagem dos itens.
        /// </summary>
        [TestMethod]
        [Description("Tests the count property")]
        public void DictionaryTrie_CountTest()
        {
            var values = this.GetTestValues();
            var target = new DictionaryTrie<char, string>(
                values,
                false,
                this.dicFactory);
            Assert.AreEqual(values.Length, target.Count);
        }

        /// <summary>
        /// Permite testar a propriedade que indica se a colecção é só de leitura.
        /// </summary>
        [TestMethod]
        [Description("Tests the reaondly property")]
        public void DictionaryTrie_ReadOnlyTest()
        {
            var target = new DictionaryTrie<char, string>(
                new[] { string.Empty },
                true,
                this.dicFactory);
            Assert.IsTrue(target.IsReadOnly);

            target = new DictionaryTrie<char, string>(
                new[] { string.Empty },
                false,
                this.dicFactory);
            Assert.IsFalse(target.IsReadOnly);
        }

        /// <summary>
        /// Permite testar o funcionamento do iterador.
        /// </summary>
        [TestMethod]
        [Description("Tests the iterator functioning")]
        public void DictionaryTrie_GetIteratorTest()
        {
            var values = this.GetTestValues();
            var target = new DictionaryTrie<char, string>(
                values,
                false,
                this.dicFactory);
            var iterator = target.GetIterator();

            // A sequência vazia não existe
            Assert.IsFalse(iterator.Exists);

            for (int i = 0; i < values.Length; ++i)
            {
                iterator.Reset();
                var value = values[i];
                iterator.Reset();
                for (int j = 0; j < value.Length; ++j)
                {
                    var state = iterator.GoForward(value[j]);
                    Assert.IsTrue(state);
                }

                // O valor existe, uma vez que foi introduizo.
                Assert.IsTrue(iterator.Exists);

                // O valor procurado encontra-se associado ao final do iterador.
                var current = iterator.Current;
                Assert.AreEqual(current, value);

                var forwarded = iterator.GoForward(' ');
                Assert.IsFalse(forwarded);
            }
        }

        /// <summary>
        /// Testa a função de adição de novos elementos.
        /// </summary>
        [TestMethod]
        [Description("Tests the items addition function.")]
        public void DictionaryTrie_AddTest()
        {
            var values = this.GetTestValues();
            var target = new DictionaryTrie<char, string>(
                values,
                false,
                this.dicFactory);

            // Tenta adicionar um item que já existe
            var additionResult = target.Add("amordaçar");
            Assert.IsFalse(additionResult);
            Assert.AreEqual(values.Length, target.Count);

            // Adiciona um novo elemento
            additionResult = target.Add("ambivalentes");
            Assert.IsTrue(additionResult);
            Assert.AreEqual(values.Length + 1, target.Count);
        }

        /// <summary>
        /// Testa a função de adição no caso da árvore associativa ser apenas de leitura.
        /// </summary>
        [TestMethod]
        [Description("Tests the exception thrown when trie is readonly.")]
        [ExpectedException(typeof(UtilitiesException))]
        public void DictionaryTrie_AddReadOnlyExceptionTest()
        {
            var target = new DictionaryTrie<char, string>(
                new[] { string.Empty },
                true,
                this.dicFactory);
            target.Add(string.Empty);
        }

        /// <summary>
        /// Testa a função que remove elementos em comum com uma colecção pré-definida.
        /// </summary>
        [TestMethod]
        [Description("Tests the except with function.")]
        public void DictionaryTrie_ExceptWithTest()
        {
            var values = this.GetTestValues();
            var overlappingValues = this.GetTestOverlappingValues();
            var expected = values.Except(overlappingValues).ToArray();
            var target = new DictionaryTrie<char, string>(
                values,
                false,
                this.dicFactory);

            // Testa a versão da função actual sobre enumeráveis
            target.ExceptWith((IEnumerable<string>)overlappingValues);
            CollectionAssert.AreEquivalent(
                expected, 
                target);

            // Testa a função actual sobre ávores associativas
            target = new DictionaryTrie<char, string>(
                values,
                false,
                this.dicFactory);
            var exceptWith = new DictionaryTrie<char, string>(
                overlappingValues,
                true,
                this.dicFactory);
            target.ExceptWith(exceptWith);
            CollectionAssert.AreEquivalent(
                expected,
                target);

            var expectedTarget = new DictionaryTrie<char, string>(
                expected,
                false,
                this.dicFactory);
            Assert.IsTrue(target.SetEquals(expectedTarget));
        }

        /// <summary>
        /// Testa a função de intersecção de conjuntos.
        /// </summary>
        [TestMethod]
        [Description("Tests the intersection function.")]
        public void DictionaryTrie_IntersectWithTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Permite testar a função que verifica se a colecção ou árvore associativa
        /// especificada constitui um subconjunto próprio da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the proper substet checking function.")]
        public void DictionaryTrie_IsProperSubsetOfTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Permite testar a função que verifia se a colecção ou árvore associativa
        /// especificada constitui um superconjunto próprio da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the proper superset checking function.")]
        public void DictionaryTrie_IsProperSupersetOfTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Permite testar a função que verifica se a colecção ou árvore associativa
        /// especificada constitui um subconjunto da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the subset checking function.")]
        public void DictionaryTrie_IsSubsetOfTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Permite testar a função que verifica se a colecção ou árvore associativa
        /// especificada constitui um superconjunto da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the superset checking function.")]
        public void DictionaryTrie_IsSupersetOfTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Permite testar a função que verifica se uma colecção ou árvore associativa
        /// possuem elementos em comum com a árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the overlap ckecking function.")]
        public void DictionaryTrie_OverlapsTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Permite testar a função que avergua a igualdade entre duas árvores
        /// associativas ou entre uma árvore associativa e uma colecção.
        /// </summary>
        [TestMethod]
        [Description("Tests the set equality function.")]
        public void DictionaryTrie_SetEqualsTest()
        {
            var firstValues = this.GetTestValues();
            var secondValues = this.GetTestValues();
            var firstTarget = new DictionaryTrie<char, string>(
                firstValues,
                false,
                this.dicFactory);
            var secondTarget = new DictionaryTrie<char, string>(
                secondValues,
                false,
                this.dicFactory);

            // Testa a igualdade com uma colecção
            Assert.IsTrue(firstTarget.SetEquals(secondValues));

            // Testa a igualdade com outra árvore associativa
            Assert.IsTrue(firstTarget.SetEquals(secondTarget));

            // Testa a igualdade com valores diferentes
            var differentValues = this.GetTestOverlappingValues();
            var differentTarget = new DictionaryTrie<char, string>(
                differentValues,
                false,
                this.dicFactory);
            Assert.IsFalse(firstTarget.SetEquals(differentValues));
            Assert.IsFalse(firstTarget.SetEquals(differentTarget));
        }

        /// <summary>
        /// Permite testar a função que remove todos os elementos que ambas as colecções
        /// possuem em comum.
        /// </summary>
        [TestMethod]
        [Description("Tests the function that removes all elemnents in both collections.")]
        public void DictionaryTrie_SymmetricExceptWithTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Testa a função de união.
        /// </summary>
        [TestMethod]
        [Description("Tests the union function.")]
        public void DictionaryTrie_UnionTest()
        {
            Assert.Inconclusive("Test not yet implemented.");
        }

        /// <summary>
        /// Obtém um conjunto de valores de teste.
        /// </summary>
        /// <returns>Os valores de teste.</returns>
        private string[] GetTestValues()
        {
            var values = new[] { 
                "amordaçar", 
                "ambivalente", 
                "carro", 
                "cartas", 
                "carros", 
                "espasmos", 
                "espasmo", 
                "introdução", 
                "reacção", 
                "recepção" 
            };

            return values;
        }

        /// <summary>
        /// Obtém um conjunto de valores de teste com alguns dos elementos
        /// em comum com o conjunto de valores de teste inicial.
        /// </summary>
        /// <returns>Os valores de teste.</returns>
        private string[] GetTestOverlappingValues()
        {
            var values = new[]{
                "espasmo",
                "temporário",
                "ambivalente",
                "pois",
                "carro",
                "suínos",
                "cartas",
                "carta",
                "carros",
                "introspecção"
            };

            return values;
        }
    }
}
