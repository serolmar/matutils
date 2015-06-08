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
}
