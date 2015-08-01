namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Teste às funções definidas por uma região rectangular.
    /// </summary>
    [TestClass]
    public class RectangularRegionTest
    {
        /// <summary>
        /// Testa a função que averigua se duas regiões rectangulares se sobrepõem.
        /// </summary>
        [Description("Checks if two rectangular registion overlap")]
        [TestMethod]
        public void RectangularRegion_OverlapTest()
        {
            // Testes com rectângulos arbitrários
            var rect1 = new RectangularRegion<int>(1, 1, 5, 5);
            var rect2 = new RectangularRegion<int>(2, 2, 4, 4);
            var rect3 = new RectangularRegion<int>(3, 3, 5, 5);
            var rect4 = new RectangularRegion<int>(6, 1, 7, 2);
            var rect5 = new RectangularRegion<int>(8, 2, 10, 4);
            var rect6 = new RectangularRegion<int>(8, 1, 11, 2);

            Assert.IsTrue(rect1.OverLaps(rect1));
            Assert.IsTrue(rect1.OverLaps(rect2));
            Assert.IsTrue(rect2.OverLaps(rect1));
            Assert.IsTrue(rect1.OverLaps(rect3));
            Assert.IsTrue(rect3.OverLaps(rect1));
            Assert.IsTrue(rect2.OverLaps(rect3));
            Assert.IsTrue(rect3.OverLaps(rect2));
            Assert.IsFalse(rect1.OverLaps(rect4));
            Assert.IsTrue(rect5.OverLaps(rect6));
            Assert.IsTrue(rect6.OverLaps(rect5));

            // Flor que não intersecta
            var flower = this.GetNonIntersectionFlower();
            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                Assert.IsFalse(flower.Item1.OverLaps(flower.Item2[i]));
            }

            // Flor que toca
            flower = this.GetBorderIntersectionFlower();
            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                Assert.IsTrue(flower.Item1.OverLaps(flower.Item2[i]));
            }

            // Flor que intersecta
            flower = this.GetIntersectionFlower();
            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                Assert.IsTrue(flower.Item1.OverLaps(flower.Item2[i]));
            }
        }

        /// <summary>
        /// Realiza um teste sobre a função de intersecção da região rectangular.
        /// </summary>
        [Description("Tests the rectangular region intersection function.")]
        [TestMethod]
        public void RectangularRegion_IntersectTest()
        {
            // Conjunto vazio de intersecções
            var flower = this.GetNonIntersectionFlower();
            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                Assert.IsNull(flower.Item1.Intersect(flower.Item2[i]));
            }

            // Apenas intersecções de fronteira
            flower = this.GetBorderIntersectionFlower();
            var expectedList = new List<RectangularRegion<int>>();
            expectedList.Add(new RectangularRegion<int>(2, 2, 2, 2));
            expectedList.Add(new RectangularRegion<int>(2, 5, 2, 5));
            expectedList.Add(new RectangularRegion<int>(3, 2, 4, 2));
            expectedList.Add(new RectangularRegion<int>(3, 5, 4, 5));
            expectedList.Add(new RectangularRegion<int>(5, 2, 5, 2));
            expectedList.Add(new RectangularRegion<int>(5, 5, 5, 5));
            expectedList.Add(new RectangularRegion<int>(2, 3, 2, 4));
            expectedList.Add(new RectangularRegion<int>(5, 3, 5, 4));

            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                var expected = expectedList[i];
                var actual = flower.Item1.Intersect(flower.Item2[i]);
                Assert.AreEqual(expected, actual);
            }

            expectedList.Clear();
            expectedList.Add(new RectangularRegion<int>(2, 2, 3, 3));
            expectedList.Add(new RectangularRegion<int>(2, 7, 3, 8));
            expectedList.Add(new RectangularRegion<int>(4, 2, 6, 3));
            expectedList.Add(new RectangularRegion<int>(4, 7, 6, 8));
            expectedList.Add(new RectangularRegion<int>(7, 2, 8, 3));
            expectedList.Add(new RectangularRegion<int>(7, 7, 8, 8));
            expectedList.Add(new RectangularRegion<int>(2, 4, 3, 6));
            expectedList.Add(new RectangularRegion<int>(7, 4, 8, 6));

            flower = this.GetIntersectionFlower();
            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                var expected = expectedList[i];
                var actual = flower.Item1.Intersect(flower.Item2[i]);
                Assert.AreEqual(expected, actual);
            }

            flower = this.GetInnerLeafsFlower();
            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                var expected = flower.Item2[i];
                var actual = flower.Item1.Intersect(expected);
                Assert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Testa a função que permite determinar o conjunto de regiões rectangulares resultantes
        /// da diferença entre outras duas regiões rectangulares.
        /// </summary>
        [Description("Tests the rectangular region subtract function.")]
        [TestMethod]
        public void RectangularRegion_SubtractTest()
        {
            var rect = new RectangularRegion<int>(1, 1, 5, 5);
            var expected = new List<RectangularRegion<int>>();
            var actual = rect.Subtract(
                rect,
                j => { return ++j; },
                j => { return --j; });
            CollectionAssert.AreEquivalent(expected, actual);

            // Flor constituída por rectângulos que não se intersectam
            var flower = this.GetNonIntersectionFlower();
            expected = new List<RectangularRegion<int>>();
            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                expected.Clear();
                expected.Add(flower.Item1);
                actual = flower.Item1.Subtract(
                    flower.Item2[i],
                    j => { return ++j; },
                    j => { return --j; });
                CollectionAssert.AreEquivalent(expected, actual);
            }

            // Flor constituída por rectângulos que se intersectam na fronteira
            flower = this.GetBorderIntersectionFlower();
            var expectedList = new List<List<RectangularRegion<int>>>();
            expectedList.Add(new List<RectangularRegion<int>>(){
                new RectangularRegion<int>(2,3,5,5),
                new RectangularRegion<int>(3,2,5,2)});

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,5,4),
                new RectangularRegion<int>(3,5,5,5)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,2,5),
                new RectangularRegion<int>(3,3,5,5),
                new RectangularRegion<int>(5,2,5,2)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,5,4),
                new RectangularRegion<int>(2,5,2,5),
                new RectangularRegion<int>(5,5,5,5)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,4,5),
                new RectangularRegion<int>(5,3,5,5)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,5,4),
                new RectangularRegion<int>(2,5,4,5)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,5,2),
                new RectangularRegion<int>(2,5,5,5),
                new RectangularRegion<int>(3,3,5,4)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,5,2),
                new RectangularRegion<int>(2,3,4,5),
                new RectangularRegion<int>(5,5,5,5)
            });

            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                expected = expectedList[i];
                actual = flower.Item1.Subtract(flower.Item2[i],
                    j => { return ++j; },
                    j => { return --j; });
                CollectionAssert.AreEqual(expected, actual);
            }

            expectedList.Clear();
            flower = this.GetIntersectionFlower();
            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,4,8,8),
                new RectangularRegion<int>(4,2,8,3)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,8,6),
                new RectangularRegion<int>(4,7,8,8)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,3,8),
                new RectangularRegion<int>(4,4,8,8),
                new RectangularRegion<int>(7,2,8,3)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,8,6),
                new RectangularRegion<int>(2,7,3,8),
                new RectangularRegion<int>(7,7,8,8)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,6,8),
                new RectangularRegion<int>(7,4,8,8)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,8,6),
                new RectangularRegion<int>(2,7,6,8)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,8,3),
                new RectangularRegion<int>(2,7,8,8),
                new RectangularRegion<int>(4,4,8,6)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(2,2,8,3),
                new RectangularRegion<int>(2,4,6,8),
                new RectangularRegion<int>(7,7,8,8)
            });

            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                expected = expectedList[i];
                actual = flower.Item1.Subtract(flower.Item2[i],
                    j => { return ++j; },
                    j => { return --j; });
                CollectionAssert.AreEqual(expected, actual);
            }

            expectedList.Clear();
            flower = this.GetInnerLeafsFlower();
            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,3,6,6),
                new RectangularRegion<int>(3,1,6,2)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,6,2),
                new RectangularRegion<int>(1,5,6,6),
                new RectangularRegion<int>(3,3,6,4)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,6,4),
                new RectangularRegion<int>(3,5,6,6)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,2,6),
                new RectangularRegion<int>(3,3,6,6),
                new RectangularRegion<int>(5,1,6,2)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,6,2),
                new RectangularRegion<int>(1,3,2,6),
                new RectangularRegion<int>(3,5,6,6),
                new RectangularRegion<int>(5,3,6,4)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,6,4),
                new RectangularRegion<int>(1,5,2,6),
                new RectangularRegion<int>(5,5,6,6)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,4,6),
                new RectangularRegion<int>(5,3,6,6)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,6,2),
                new RectangularRegion<int>(1,3,4,6),
                new RectangularRegion<int>(5,5,6,6)
            });

            expectedList.Add(new List<RectangularRegion<int>>()
            {
                new RectangularRegion<int>(1,1,6,4),
                new RectangularRegion<int>(1,5,4,6)
            });

            for (int i = 0; i < flower.Item2.Count; ++i)
            {
                expected = expectedList[i];
                actual = flower.Item1.Subtract(flower.Item2[i],
                    j => { return ++j; },
                    j => { return --j; });
                CollectionAssert.AreEqual(expected, actual);
            }
        }

        /// <summary>
        /// Testa a função que permite fundir dois rectângulos.
        /// </summary>
        [Description("Tests the rectangular region merge function.")]
        [TestMethod]
        public void RectangularRegion_MergeTest()
        {
            var centerRect = new RectangularRegion<int>(6, 6, 8, 8);
            var actual = centerRect.Merge(centerRect, i => ++i, j => ++j);
            Assert.AreEqual(centerRect, actual);

            // Os rectângulos encontram-se espaçados
            var rectangularRegionList = new List<RectangularRegion<int>>();
            rectangularRegionList.Add(new RectangularRegion<int>(6, 1, 8, 3));
            rectangularRegionList.Add(new RectangularRegion<int>(1, 6, 3, 8));
            rectangularRegionList.Add(new RectangularRegion<int>(6, 10, 8, 12));
            rectangularRegionList.Add(new RectangularRegion<int>(10, 6, 12, 8));
            for (int i = 0; i < rectangularRegionList.Count; ++i)
            {
                var currentRectangularRegion = rectangularRegionList[i];
                var error = false;
                try
                {
                    centerRect.Merge(
                        currentRectangularRegion,
                        j => { return ++j; },
                        j => { return --j; });
                }
                catch (UtilitiesDataException)
                {
                    error = true;
                }

                Assert.IsTrue(error);
            }

            // Os rectângulos são todos vizinhos do rectângulo central
            var topRect = new RectangularRegion<int>(6, 3, 8, 5);
            var leftRect = new RectangularRegion<int>(3, 6, 5, 8);
            var bottomRect = new RectangularRegion<int>(6, 9, 8, 11);
            var rightRect = new RectangularRegion<int>(9, 6, 11, 8);

            var topExpected = new RectangularRegion<int>(6, 3, 8, 8);
            var leftExpected = new RectangularRegion<int>(3, 6, 8, 8);
            var bottomExpected = new RectangularRegion<int>(6, 6, 8, 11);
            var rightExpected = new RectangularRegion<int>(6, 6, 11, 8);

            Assert.AreEqual(topExpected, centerRect.Merge(topRect, i => ++i, i => --i));
            Assert.AreEqual(leftExpected, centerRect.Merge(leftRect, i => ++i, i => --i));
            Assert.AreEqual(bottomExpected, centerRect.Merge(bottomRect, i => ++i, i => --i));
            Assert.AreEqual(rightExpected, centerRect.Merge(rightRect, i => ++i, i => --i));

            // Os rectângulos intersectam o rectângulo central
            topRect = new RectangularRegion<int>(6, 3, 8, 7);
            leftRect = new RectangularRegion<int>(3, 6, 7, 8);
            bottomRect = new RectangularRegion<int>(6, 7, 8, 11);
            rightRect = new RectangularRegion<int>(7, 6, 11, 8);

            topExpected = new RectangularRegion<int>(6, 3, 8, 8);
            leftExpected = new RectangularRegion<int>(3, 6, 8, 8);
            bottomExpected = new RectangularRegion<int>(6, 6, 8, 11);
            rightExpected = new RectangularRegion<int>(6, 6, 11, 8);

            Assert.AreEqual(topExpected, centerRect.Merge(topRect, i => ++i, i => --i));
            Assert.AreEqual(leftExpected, centerRect.Merge(leftRect, i => ++i, i => --i));
            Assert.AreEqual(bottomExpected, centerRect.Merge(bottomRect, i => ++i, i => --i));
            Assert.AreEqual(rightExpected, centerRect.Merge(rightRect, i => ++i, i => --i));
        }

        /// <summary>
        /// Permite obter uma configuração de rectângulos que não se intersectam
        /// nem tocam em forma de flor.
        /// </summary>
        /// <returns>O tuplo que contém o par centro / folhas da flor.</returns>
        private Tuple<RectangularRegion<int>, List<RectangularRegion<int>>> GetNonIntersectionFlower()
        {
            // As folhas
            var leafs = new List<RectangularRegion<int>>();

            // O centro
            var center = new RectangularRegion<int>(3, 3, 8, 8);

            // As linhas inferior e superior
            for (int i = 1; i < 10; i += 2)
            {
                leafs.Add(new RectangularRegion<int>(i, 1, i + 1, 2));
                leafs.Add(new RectangularRegion<int>(i, 9, i + 1, 10));
            }

            // As colunas esquerda e direita
            for (int i = 3; i < 8; i += 2)
            {
                leafs.Add(new RectangularRegion<int>(1, i, 2, i + 1));
                leafs.Add(new RectangularRegion<int>(9, i, 10, i + 1));
            }

            return Tuple.Create(center, leafs);
        }

        /// <summary>
        /// Permite obter uma configuração de rectângulos que tocam um rectângulo central
        /// mas cuja intersecção resulta numa área nula.
        /// </summary>
        /// <returns>O tuplo que contém o par centro / folhas da flor.</returns>
        private Tuple<RectangularRegion<int>, List<RectangularRegion<int>>> GetBorderIntersectionFlower()
        {
            var leafs = new List<RectangularRegion<int>>();
            var center = new RectangularRegion<int>(2, 2, 5, 5);

            for (int i = 1; i < 6; i += 2)
            {
                leafs.Add(new RectangularRegion<int>(i, 1, i + 1, 2));
                leafs.Add(new RectangularRegion<int>(i, 5, i + 1, 6));
            }

            leafs.Add(new RectangularRegion<int>(1, 3, 2, 4));
            leafs.Add(new RectangularRegion<int>(5, 3, 6, 4));

            return Tuple.Create(center, leafs);
        }

        /// <summary>
        /// Permite obter uma configuração de rectângulos que intersectam um rectângulo central
        /// e cuja intersecção possui área não vazia.
        /// </summary>
        /// <returns>O tuplo que contém o par centro / folhas da flor.</returns>
        private Tuple<RectangularRegion<int>, List<RectangularRegion<int>>> GetIntersectionFlower()
        {
            var leafs = new List<RectangularRegion<int>>();
            var center = new RectangularRegion<int>(2, 2, 8, 8);

            for (int i = 1; i < 9; i += 3)
            {
                leafs.Add(new RectangularRegion<int>(i, 1, i + 2, 3));
                leafs.Add(new RectangularRegion<int>(i, 7, i + 2, 9));
            }

            leafs.Add(new RectangularRegion<int>(1, 4, 3, 6));
            leafs.Add(new RectangularRegion<int>(7, 4, 9, 6));

            return Tuple.Create(center, leafs);
        }

        /// <summary>
        /// Obtém uma flor na qual as folhas se encontram no interior do rectângulo central.
        /// </summary>
        /// <returns>O tuplo que contém o par centro / folhas da flor.</returns>
        private Tuple<RectangularRegion<int>, List<RectangularRegion<int>>> GetInnerLeafsFlower()
        {
            var center = new RectangularRegion<int>(1, 1, 6, 6);
            var leafs = new List<RectangularRegion<int>>();

            for (int i = 1; i < 6; i += 2)
            {
                for (int j = 1; j < 6; j += 2)
                {
                    leafs.Add(new RectangularRegion<int>(i, j, i + 1, j + 1));
                }
            }

            return Tuple.Create(center, leafs);
        }
    }

    /// <summary>
    /// Teste às funções definiadas por um conjunto de regiões quen se intersectam.
    /// </summary>
    [TestClass]
    public class NonIntersectingMergingRegionsSetTest
    {
        /// <summary>
        /// Testa se todas as regiões foram inseridas com sucesso.
        /// </summary>
        [Description("Tests the number of regions that were inserted.")]
        [TestMethod]
        public void NonIntersectingMergingRegionSet_AddAndCountTest()
        {
            var target = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
            var rectangularRegions = this.GetNonIntersectionFlower();
            target.Add(rectangularRegions.Item1);
            target.AssertIntegrity();
            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                target.Add(rectangularRegions.Item2[i]);
                target.AssertIntegrity();
            }

            Assert.AreEqual(rectangularRegions.Item2.Count + 1, target.Count);
        }

        /// <summary>
        /// Testa a tentativa de adição de uma região rectangular ao conjunto de regiões rectangulares
        /// que não se intersectam.
        /// </summary>
        [Description("Tries to add an intersecting region to non intersection regions set.")]
        [TestMethod]
        [ExpectedException(typeof(UtilitiesException))]
        public void NonIntersectiongRegionSet_TryAddOuterIntersectingRegionTest()
        {
            var target = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
            var rectangularRegions = this.GetNonIntersectionFlower();
            target.Add(rectangularRegions.Item1);
            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                target.Add(rectangularRegions.Item2[i]);
            }

            // Tenta adicionar uma região que se intersecte com as anteriores
            var addingRegion = new RectangularRegion<int>(0, 0, 10, 10);
            target.Add(addingRegion);
        }

        /// <summary>
        /// Testa a tentativa de adição de uma região rectangular ao conjunto de regiões rectangulares
        /// que não se intersectam.
        /// </summary>
        [Description("Tries to add an intersecting region to non intersection regions set.")]
        [TestMethod]
        [ExpectedException(typeof(UtilitiesException))]
        public void NonIntersectiongRegionSet_TryAddInnerIntersectingRegionTest()
        {
            var target = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
            var rectangularRegions = this.GetNonIntersectionFlower();
            target.Add(rectangularRegions.Item1);
            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                target.Add(rectangularRegions.Item2[i]);
            }

            // Tenta adicionar uma região que se intersecte com as anteriores
            var addingRegion = new RectangularRegion<int>(4, 4, 5, 5);
            target.Add(addingRegion);
        }

        /// <summary>
        /// Testa a tentativa de adição de uma região rectangular ao conjunto de regiões rectangulares
        /// que não se intersectam.
        /// </summary>
        [Description("Tries to add an intersecting region to non intersection regions set.")]
        [TestMethod]
        [ExpectedException(typeof(UtilitiesException))]
        public void NonIntersectiongRegionSet_TryAddCrossBorderIntersectingRegionTest()
        {
            var target = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
            var rectangularRegions = this.GetNonIntersectionFlower();
            target.Add(rectangularRegions.Item1);
            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                target.Add(rectangularRegions.Item2[i]);
            }

            // Tenta adicionar uma região que se intersecte com as anteriores
            var addingRegion = new RectangularRegion<int>(2, 2, 3, 3);
            target.Add(addingRegion);
        }

        /// <summary>
        /// Testa a função de remoção de um conjunto de regiões que não se intersectam.
        /// </summary>
        [Description("Tests the remove region function for non intersecting regions set.")]
        [TestMethod]
        public void NonIntersectingRegionSet_RemoveRegionTest()
        {
            var target = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
            var rectangularRegions = this.GetNonIntersectionFlower();
            target.Add(rectangularRegions.Item1);
            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                target.Add(rectangularRegions.Item2[i]);
            }

            var targetCount = target.Count;
            Assert.IsFalse(target.RemoveRegion(new RectangularRegion<int>(0, 0, 2, 2)));
            target.AssertIntegrity();
            Assert.IsTrue(target.RemoveRegion(rectangularRegions.Item1));
            target.AssertIntegrity();
            --targetCount;
            Assert.AreEqual(targetCount, target.Count);
            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                Assert.IsTrue(target.RemoveRegion(rectangularRegions.Item2[i]));
                target.AssertIntegrity();
                --targetCount;
                Assert.AreEqual(targetCount, target.Count);
            }
        }

        /// <summary>
        /// Testa a função que permite determinar a região à qual uma determinada célula pertence.
        /// </summary>
        [Description("Tests the function that gets the merged region to which cells belong to.")]
        [TestMethod]
        public void NonIntersectingRegionSet_GetMergingRegionForCellTest()
        {
            var target = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
            var rectangularRegions = this.GetNonIntersectionFlower();
            target.Add(rectangularRegions.Item1);
            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                target.Add(rectangularRegions.Item2[i]);
            }

            var actual = target.GetMergingRegionForCell(0, 0);
            Assert.IsNull(actual);
            actual = target.GetMergingRegionForCell(12, 12);
            Assert.IsNull(actual);
            for (int i = 3; i < 9; ++i)
            {
                for (int j = 3; j < 9; ++j)
                {
                    actual = target.GetMergingRegionForCell(i, j);
                    Assert.AreEqual(rectangularRegions.Item1, actual);
                }
            }

            for (int i = 0; i < rectangularRegions.Item2.Count; ++i)
            {
                var currentRectangularRegion = rectangularRegions.Item2[i];
                for (int j = currentRectangularRegion.TopLeftX; j <= currentRectangularRegion.BottomRightX; ++j)
                {
                    for (int k = currentRectangularRegion.TopLeftY; k < currentRectangularRegion.BottomRightY; ++k)
                    {
                        actual = target.GetMergingRegionForCell(j, k);
                        Assert.AreEqual(currentRectangularRegion, actual);
                    }
                }
            }
        }

        /// <summary>
        /// Testa a função que permite determinar as regiões da colecção que intersectam
        /// a região dada.
        /// </summary>
        [Description("Tests the function that gets the merging regions intersecting a provided region.")]
        [TestMethod]
        public void NonIntersectingRegionSet_GetIntersectingRegionsForTest()
        {
            var target = this.GetMatrixRegion();
            target.AssertIntegrity();
            var expected = new List<RectangularRegion<int>>();
            for (int i = 0; i < 15; i += 4)
            {
                expected.Add(new RectangularRegion<int>(i, 0, i + 1, 1));
            }

            var actual = target.GetIntersectingRegionsFor(new RectangularRegion<int>(1, 1, 16, 1));
            CollectionAssert.AreEquivalent(expected, actual);
            expected.Clear();
            expected.Add(new RectangularRegion<int>(0, 0, 1, 1));
            actual = target.GetIntersectingRegionsFor(new RectangularRegion<int>(-1, 0, 1, 0));
            CollectionAssert.AreEquivalent(expected, actual);
            expected.Clear();
            actual = target.GetIntersectingRegionsFor(new RectangularRegion<int>(-2, -2, -1, -1));
            CollectionAssert.AreEquivalent(expected, actual);
            actual = target.GetIntersectingRegionsFor(new RectangularRegion<int>(16, 16, 17, 17));
            CollectionAssert.AreEquivalent(expected, actual);
            actual = target.GetIntersectingRegionsFor(new RectangularRegion<int>(2, 2, 3, 3));
            CollectionAssert.AreEquivalent(expected, actual);
            actual = target.GetIntersectingRegionsFor(new RectangularRegion<int>(1, 1, 12, 12));
            expected = target.ToList();
            CollectionAssert.AreEquivalent(expected, actual);
            actual = target.GetIntersectingRegionsFor(new RectangularRegion<int>(-1, -1, 25, 25));
            CollectionAssert.AreEquivalent(expected, actual);
        }

        /// <summary>
        /// Testa a função que permite determinar as regiões da colecção que intersectam
        /// a região dada.
        /// </summary>
        [Description("Tests the function that gets the merging regions intersecting a provided region.")]
        [TestMethod]
        public void NonIntersectingRegionSet_RemoveIntersectingRegionsWithTest()
        {
            var target = this.GetMatrixRegion();
            target.AssertIntegrity();
            var expected = new List<RectangularRegion<int>>();
            for (int i = 0; i < 15; i += 4)
            {
                expected.Add(new RectangularRegion<int>(i, 0, i + 1, 1));
            }

            var actual = target.RemoveIntersectingRegionsWith(new RectangularRegion<int>(1, 1, 16, 1));
            target.AssertIntegrity();
            CollectionAssert.AreEquivalent(expected, actual);
            expected = this.GetMatrixRegion().Except(expected).ToList();
            CollectionAssert.AreEquivalent(expected, target.ToList());
            expected.Clear();
            expected.Add(new RectangularRegion<int>(0, 0, 1, 1));
            target = this.GetMatrixRegion();
            actual = target.RemoveIntersectingRegionsWith(new RectangularRegion<int>(-1, 0, 1, 0));
            target.AssertIntegrity();
            CollectionAssert.AreEquivalent(expected, actual);
            expected = this.GetMatrixRegion().Except(expected).ToList();
            CollectionAssert.AreEquivalent(expected, target.ToList());

            expected.Clear();
            target = this.GetMatrixRegion();
            actual = target.RemoveIntersectingRegionsWith(new RectangularRegion<int>(-2, -2, -1, -1));
            target.AssertIntegrity();
            CollectionAssert.AreEquivalent(expected, actual);
            expected = this.GetMatrixRegion().Except(expected).ToList();
            CollectionAssert.AreEquivalent(expected, target.ToList());
            expected.Clear();
            actual = target.RemoveIntersectingRegionsWith(new RectangularRegion<int>(16, 16, 17, 17));
            target.AssertIntegrity();
            CollectionAssert.AreEquivalent(expected, actual);
            expected = this.GetMatrixRegion().Except(expected).ToList();
            CollectionAssert.AreEquivalent(expected, target.ToList());
            expected.Clear();
            actual = target.RemoveIntersectingRegionsWith(new RectangularRegion<int>(2, 2, 3, 3));
            target.AssertIntegrity();
            CollectionAssert.AreEquivalent(expected, actual);
            expected = this.GetMatrixRegion().Except(expected).ToList();
            CollectionAssert.AreEquivalent(expected, target.ToList());
            expected.Clear();
            actual = target.RemoveIntersectingRegionsWith(new RectangularRegion<int>(1, 1, 12, 12));
            target.AssertIntegrity();
            expected = this.GetMatrixRegion().ToList();
            CollectionAssert.AreEquivalent(expected, actual);
            expected = this.GetMatrixRegion().Except(expected).ToList();
            CollectionAssert.AreEquivalent(expected, target.ToList());
            expected.Clear();
            target = this.GetMatrixRegion();
            actual = target.RemoveIntersectingRegionsWith(new RectangularRegion<int>(-1, -1, 25, 25));
            target.AssertIntegrity();
            expected = this.GetMatrixRegion().ToList();
            CollectionAssert.AreEquivalent(expected, actual);
            expected = this.GetMatrixRegion().Except(expected).ToList();
            CollectionAssert.AreEquivalent(expected, target.ToList());
        }

        /// <summary>
        /// Permite obter uma configuração de rectângulos que não se intersectam
        /// nem tocam em forma de flor.
        /// </summary>
        /// <returns>O tuplo que contém o par centro / folhas da flor.</returns>
        private Tuple<RectangularRegion<int>, List<RectangularRegion<int>>> GetNonIntersectionFlower()
        {
            // As folhas
            var leafs = new List<RectangularRegion<int>>();

            // O centro
            var center = new RectangularRegion<int>(3, 3, 8, 8);

            // As linhas inferior e superior
            for (int i = 1; i < 10; i += 2)
            {
                leafs.Add(new RectangularRegion<int>(i, 1, i + 1, 2));
                leafs.Add(new RectangularRegion<int>(i, 9, i + 1, 10));
            }

            // As colunas esquerda e direita
            for (int i = 3; i < 8; i += 2)
            {
                leafs.Add(new RectangularRegion<int>(1, i, 2, i + 1));
                leafs.Add(new RectangularRegion<int>(9, i, 10, i + 1));
            }

            return Tuple.Create(center, leafs);
        }

        /// <summary>
        /// Obtém uma matriz de regiões rectangulares que não se intersectam.
        /// </summary>
        /// <returns>A matriz de regiões.</returns>
        private NonIntersectingMergingRegionsSet<int, RectangularRegion<int>> GetMatrixRegion()
        {
            var result = new NonIntersectingMergingRegionsSet<int, RectangularRegion<int>>();
            for (int i = 0; i < 15; i += 4)
            {
                for (int j = 0; j < 15; j += 4)
                {
                    result.Add(new RectangularRegion<int>(i, j, i + 1, j + 1));
                }
            }

            return result;
        }
    }
}
