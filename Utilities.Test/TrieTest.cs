namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DicDrivenTrieSetTest
    {
        /// <summary>
        /// Fábrica responsável pela criação dos dicionários.
        /// </summary>
        private Func<IDictionary<char, DicDrivenTrieSet<char,string>.TrieNode>> dicFactory = () =>
            new Dictionary<char, DicDrivenTrieSet<char, string>.TrieNode>(
                EqualityComparer<char>.Default);

        /// <summary>
        /// Permite testar a funcionalidade de indexação.
        /// </summary>
        [TestMethod]
        [Description("Tests the indexing functionality.")]
        public void DicDrivenTrieSet_IndexerTest()
        {
            var values = this.GetTestValues();
            var target = new DicDrivenTrieSet<char, string>(
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
        public void DicDrivenTrieSet_CountTest()
        {
            var values = this.GetTestValues();
            var target = new DicDrivenTrieSet<char, string>(
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
        public void DicDrivenTrieSet_ReadOnlyTest()
        {
            var target = new DicDrivenTrieSet<char, string>(
                new[] { string.Empty },
                true,
                this.dicFactory);
            Assert.IsTrue(target.IsReadOnly);

            target = new DicDrivenTrieSet<char, string>(
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
        public void DicDrivenTrieSet_GetIteratorTest()
        {
            var values = this.GetTestValues();
            var target = new DicDrivenTrieSet<char, string>(
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
                Assert.AreEqual(current, i);

                var forwarded = iterator.GoForward(' ');
                Assert.IsFalse(forwarded);
            }
        }

        /// <summary>
        /// Testa a função de adição de novos elementos.
        /// </summary>
        [TestMethod]
        [Description("Tests the items addition function.")]
        public void DicDrivenTrieSet_AddTest()
        {
            var values = this.GetTestValues();
            var target = new DicDrivenTrieSet<char, string>(
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
        public void DicDrivenTrieSet_AddReadOnlyExceptionTest()
        {
            var target = new DicDrivenTrieSet<char, string>(
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
        public void DicDrivenTrieSet_ExceptWithTest()
        {
            var values = this.GetTestValues();
            var overlappingValues = this.GetTestOverlappingValues();
            var expected = values.Except(overlappingValues).ToArray();
            var target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);

            // Testa a versão da função actual sobre enumeráveis
            target.ExceptWith((IEnumerable<string>)overlappingValues);
            CollectionAssert.AreEquivalent(
                expected,
                target);

            // Testa a função actual sobre ávores associativas
            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            var exceptWith = new DicDrivenTrieSet<char, string>(
                overlappingValues,
                true,
                this.dicFactory);
            target.ExceptWith(exceptWith);
            CollectionAssert.AreEquivalent(
                expected,
                target);

            var expectedTarget = new DicDrivenTrieSet<char, string>(
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
        public void DicDrivenTrieSet_IntersectWithTest()
        {
            var values = this.GetTestValues();
            var overlappingValues = this.GetTestOverlappingValues();
            var expected = values.Intersect(overlappingValues).ToArray();
            var target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            target.IntersectWith((IEnumerable<string>)overlappingValues);
            CollectionAssert.AreEquivalent(
                expected,
                target);

            // Testa a função sobre as árvore associativas
            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            var intersectWith = new DicDrivenTrieSet<char, string>(
                overlappingValues,
                true,
                this.dicFactory);
            target.IntersectWith(intersectWith);
            CollectionAssert.AreEquivalent(
                expected,
                target);

            var expectedTarget = new DicDrivenTrieSet<char, string>(
                expected,
                false,
                this.dicFactory);
            Assert.IsTrue(target.SetEquals(expectedTarget));
        }

        /// <summary>
        /// Permite testar a função que verifica se a colecção ou árvore associativa
        /// especificada constitui um subconjunto próprio da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the proper substet checking function.")]
        public void DicDrivenTrieSet_IsProperSubsetOfTest()
        {
            var values = this.GetTestValues();
            var subsetValues = new string[values.Length - 1];
            var index = 0;
            for (int i = 0; i < 3; ++i)
            {
                subsetValues[index++] = values[i];
            }

            for (int i = 4; i < values.Length; ++i)
            {
                subsetValues[index++] = values[i];
            }

            // Subconjunto
            var target = new DicDrivenTrieSet<char, string>(
                subsetValues,
                false,
                this.dicFactory);
            var actual = target.IsProperSubsetOf(values);
            Assert.IsTrue(actual);

            // Igual - não é subconjunto próprio
            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = target.IsProperSubsetOf(values);
            Assert.IsFalse(actual);

            var subsetTarget = new DicDrivenTrieSet<char, string>(
                subsetValues,
                false,
                this.dicFactory);
            actual = subsetTarget.IsProperSubsetOf(target);
            Assert.IsTrue(actual);

            subsetTarget = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = subsetTarget.IsProperSubsetOf(target);
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Permite testar a função que verifia se a colecção ou árvore associativa
        /// especificada constitui um superconjunto próprio da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the proper superset checking function.")]
        public void DicDrivenTrieSet_IsProperSupersetOfTest()
        {
            var values = this.GetTestValues();
            var valuesLength = values.Length;
            var superSetValues = new string[valuesLength + 1];
            Array.Copy(values, superSetValues, valuesLength);
            superSetValues[valuesLength] = "esferográfica";

            var target = new DicDrivenTrieSet<char, string>(
                superSetValues,
                false,
                this.dicFactory);
            var actual = target.IsProperSupersetOf(values);
            Assert.IsTrue(actual);

            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = target.IsProperSupersetOf(values);
            Assert.IsFalse(actual);

            var superSetTarget = new DicDrivenTrieSet<char, string>(
                superSetValues,
                false,
                this.dicFactory);
            actual = superSetTarget.IsProperSupersetOf(target);
            Assert.IsTrue(actual);

            superSetTarget = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = superSetTarget.IsProperSupersetOf(target);
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Permite testar a função que verifica se a colecção ou árvore associativa
        /// especificada constitui um subconjunto da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the subset checking function.")]
        public void DicDrivenTrieSet_IsSubsetOfTest()
        {
            var values = this.GetTestValues();
            var subsetValues = new string[values.Length - 1];
            var index = 0;
            for (int i = 0; i < 3; ++i)
            {
                subsetValues[index++] = values[i];
            }

            for (int i = 4; i < values.Length; ++i)
            {
                subsetValues[index++] = values[i];
            }

            // Subconjunto
            var target = new DicDrivenTrieSet<char, string>(
                subsetValues,
                false,
                this.dicFactory);
            var actual = target.IsProperSubsetOf(values);
            Assert.IsTrue(actual);

            // Igual - não é subconjunto próprio
            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = target.IsSubsetOf(values);
            Assert.IsTrue(actual);

            var subsetTarget = new DicDrivenTrieSet<char, string>(
                subsetValues,
                false,
                this.dicFactory);
            actual = subsetTarget.IsSubsetOf(target);
            Assert.IsTrue(actual);

            subsetTarget = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = subsetTarget.IsSubsetOf(target);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Permite testar a função que verifica se a colecção ou árvore associativa
        /// especificada constitui um superconjunto da árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the superset checking function.")]
        public void DicDrivenTrieSet_IsSupersetOfTest()
        {
            var values = this.GetTestValues();
            var valuesLength = values.Length;
            var superSetValues = new string[valuesLength + 1];
            Array.Copy(values, superSetValues, valuesLength);
            superSetValues[valuesLength] = "esferográfica";

            var target = new DicDrivenTrieSet<char, string>(
                superSetValues,
                false,
                this.dicFactory);
            var actual = target.IsSupersetOf(values);
            Assert.IsTrue(actual);

            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = target.IsSupersetOf(values);
            Assert.IsTrue(actual);

            var superSetTarget = new DicDrivenTrieSet<char, string>(
                superSetValues,
                false,
                this.dicFactory);
            actual = superSetTarget.IsSupersetOf(target);
            Assert.IsTrue(actual);

            superSetTarget = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            actual = superSetTarget.IsSupersetOf(target);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Permite testar a função que verifica se uma colecção ou árvore associativa
        /// possuem elementos em comum com a árvore associativa actual.
        /// </summary>
        [TestMethod]
        [Description("Tests the overlap ckecking function.")]
        public void DicDrivenTrieSet_OverlapsTest()
        {
            var values = this.GetTestValues();
            var overlappingValues = this.GetTestOverlappingValues();
            var nonOverlappingValues = new[] { "esferográfica", "ardósia", "espectáculo" };
            var target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            var actual = target.Overlaps(overlappingValues);
            Assert.IsTrue(actual);

            actual = target.Overlaps(nonOverlappingValues);
            Assert.IsFalse(actual);

            var auxiliaryTarget = new DicDrivenTrieSet<char, string>(
                overlappingValues,
                false,
                this.dicFactory);
            actual = target.Overlaps(auxiliaryTarget);
            Assert.IsTrue(actual);
            actual = auxiliaryTarget.Overlaps(target);
            Assert.IsTrue(actual);

            auxiliaryTarget = new DicDrivenTrieSet<char, string>(
                nonOverlappingValues,
                false,
                this.dicFactory);
            actual = target.Overlaps(auxiliaryTarget);
            Assert.IsFalse(actual);
            actual = auxiliaryTarget.Overlaps(target);
            Assert.IsFalse(actual);
        }

        /// <summary>
        /// Permite testar a função que avergua a igualdade entre duas árvores
        /// associativas ou entre uma árvore associativa e uma colecção.
        /// </summary>
        [TestMethod]
        [Description("Tests the set equality function.")]
        public void DicDrivenTrieSet_SetEqualsTest()
        {
            var firstValues = this.GetTestValues();
            var secondValues = this.GetTestValues();
            var firstTarget = new DicDrivenTrieSet<char, string>(
                firstValues,
                false,
                this.dicFactory);
            var secondTarget = new DicDrivenTrieSet<char, string>(
                secondValues,
                false,
                this.dicFactory);

            // Testa a igualdade com uma colecção
            Assert.IsTrue(firstTarget.SetEquals(secondValues));

            // Testa a igualdade com outra árvore associativa
            Assert.IsTrue(firstTarget.SetEquals(secondTarget));

            // Testa a igualdade com valores diferentes
            var differentValues = this.GetTestOverlappingValues();
            var differentTarget = new DicDrivenTrieSet<char, string>(
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
        public void DicDrivenTrieSet_SymmetricExceptWithTest()
        {
            // Reocorre ao HashSet para verificar a validade do teste
            var values = new HashSet<string>( this.GetTestValues());
            var overalappingValues = new HashSet<string>( this.GetTestOverlappingValues());
            var expected = new HashSet<string>(values);
            expected.SymmetricExceptWith(overalappingValues);
            var target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            target.SymmetricExceptWith(overalappingValues);
            CollectionAssert.AreEquivalent(expected.ToArray(), target);

            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            var overlappingTarget = new DicDrivenTrieSet<char, string>(
                overalappingValues,
                false,
                this.dicFactory);
            target.SymmetricExceptWith(overlappingTarget);
            CollectionAssert.AreEquivalent(target, expected.ToArray());

            var expectedTarget = new DicDrivenTrieSet<char, string>(
                expected,
                false,
                this.dicFactory);
            var actual = target.SetEquals(expectedTarget);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Testa a função de união.
        /// </summary>
        [TestMethod]
        [Description("Tests the union function.")]
        public void DicDrivenTrieSet_UnionTest()
        {
            var values = this.GetTestValues();
            var overlappingValues = this.GetTestOverlappingValues();
            var expected = values.Union(overlappingValues).ToArray();
            var target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            target.UnionWith(overlappingValues);
            CollectionAssert.AreEquivalent(expected, target);

            target = new DicDrivenTrieSet<char, string>(
                values,
                false,
                this.dicFactory);
            var overlappingTarget = new DicDrivenTrieSet<char, string>(
                overlappingValues,
                false,
                this.dicFactory);
            target.UnionWith(overlappingTarget);
            CollectionAssert.AreEquivalent(expected, target);

            var expectedTarget = new DicDrivenTrieSet<char, string>(
                expected,
                false,
                this.dicFactory);
            var actual = target.SetEquals(expectedTarget);
            Assert.IsTrue(actual);
        }

        /// <summary>
        /// Testa o iterador associado a uma árvore associativa.
        /// </summary>
        [Description("Tests the iterator from trie set.")]
        [TestMethod]
        public void DicDrivenTrieSet_IteratorTest()
        {
            var target = new DicDrivenTrieSet<char, string>();
            target.Add("Isto");
            target.Add("um");
            target.Add("teste");
            target.Add("iterador");
            target.Add("associativa");

            var text = "Isto constitui um teste ao iterador da árvore associativa.";
            var expected = new[]{
                true,
                false,
                true,
                true,
                false,
                true,
                false,
                false,
                true
            };

            #region Verifica Existência

            var iterator = target.GetIterator();
            var length = text.Length;
            var actual = new List<bool>();
            var state = 0;
            var i = -1;
            while (state != -1)
            {
                ++i;
                if (state == 0)
                {
                    if (i < length)
                    {
                        var current = text[i];
                        if (current != ' ' && current != '.')
                        {
                            iterator.Reset();
                            if (iterator.GoForward(current))
                            {
                                state = 1;
                            }
                            else
                            {
                                // Ignora a palavra.
                                actual.Add(false);
                                iterator.Reset();
                                state = 2;
                            }
                        }
                    }
                    else
                    {
                        state = -1;
                    }
                }
                else if (state == 1)
                {
                    if (i < length)
                    {
                        var current = text[i];
                        if (current == ' ' || current == '.')
                        {
                            actual.Add(true);
                            iterator.Reset();
                            state = 0;
                        }
                        else
                        {
                            if (!iterator.GoForward(current))
                            {
                                // Ignora a palavra.
                                actual.Add(false);
                                iterator.Reset();
                                state = 2;
                            }
                        }
                    }
                    else
                    {
                        actual.Add(true);
                        state = -1;
                    }
                }
                else
                {
                    if (i < length)
                    {
                        var current = text[i];
                        if (current == ' ' || current == '.')
                        {
                            state = 0;
                        }
                    }
                    else
                    {
                        state = -1;
                    }
                }
            }

            #endregion Verifica Existência

            CollectionAssert.AreEquivalent(expected, actual);
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
