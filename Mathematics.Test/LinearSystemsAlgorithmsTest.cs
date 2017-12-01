// -----------------------------------------------------------------------
// <copyright file="LinearSystemsAlgorithmsTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Utilities;

    /// <summary>
    /// Testa a resolução de um sistema de equações com base no método da condensação.
    /// </summary>
    [TestClass]
    public class LinearSystemsAlgorithmsTest
    {
        /// <summary>
        /// Testa o algoritmo de resolução de sistemas lineares de equações com
        /// base no método da condensação.
        /// </summary>
        [TestMethod]
        public void DenseCondensationLinSysAlgorithm_RunTest()
        {
            var coefficientsMatrixText = "[[1,0,0],[0,0,0],[0,3,3],[2,0,1]]";
            var independentVectorText = "[[1,3,3]]";
            var expectedText = "[[1,0,1,0]]";

            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var fractionField = new FractionField<int>(integerDomain);
            var fractionFieldParser = new FieldDrivenExpressionParser<Fraction<int>>(
                new SimpleElementFractionParser<int>(integerParser, integerDomain),
                fractionField);
            var matrixFactory = new ArrayMatrixFactory<Fraction<int>>();

            // Leitura da matriz que representa o sistema de equações.
            var coeffsMatrix = TestsHelper.ReadMatrix<Fraction<int>>(
                3,
                4,
                coefficientsMatrixText,
                matrixFactory,
                fractionFieldParser);

            // Leitura do vector de termos independente.
            var vectorMatrix = TestsHelper.ReadMatrix<Fraction<int>>(
                3,
                1,
                independentVectorText,
                new ArrayMatrixFactory<Fraction<int>>(),
                fractionFieldParser);

            var expectedMatrixVector = TestsHelper.ReadMatrix<Fraction<int>>(
                4,
                1,
                expectedText,
                matrixFactory,
                fractionFieldParser);

            var algorithm = new DenseCondensationLinSysAlgorithm<Fraction<int>>(fractionField);
            var actual = algorithm.Run(coeffsMatrix, vectorMatrix);

            Assert.AreEqual(expectedMatrixVector.GetLength(0), actual.Vector.Length);
            for (int i = 0; i < actual.Vector.Length; ++i)
            {
                Assert.AreEqual(expectedMatrixVector[i, 0], actual.Vector[i]);
            }
        }

        /// <summary>
        /// Testa o algoritmo de resolução de um sistema simétrico com base no método
        /// da decomposição LDL.
        /// </summary>
        [TestMethod]
        [Description("Tests the symmetric linear system solver with LDL decomposition.")]
        public void SymmetricLdlDecompLinearSystemAlgorithm_RunTest()
        {
            var domain = new IntegerDomain();
            var fractionField = new FractionField<int>(domain);
            var decompositionAlg = new TriangDiagSymmMatrixDecomposition<Fraction<int>>(
                fractionField);
            var target = new SymmetricLdlDecompLinearSystemAlgorithm<Fraction<int>>(
                decompositionAlg);
            var matrix = this.GetSingularMatrix(domain);

            Assert.Inconclusive();
        }

        /// <summary>
        /// Obtém uma matriz singular a ser utilizada nos testes.
        /// </summary>
        /// <param name="integerDomain">O objecto responsável pelas operações sobre os números inteiros.</param>
        /// <returns>A matriz.</returns>
        private ArraySquareMathMatrix<Fraction<int>> GetSingularMatrix(
            IntegerDomain integerDomain)
        {
            var result = new ArraySquareMathMatrix<Fraction<int>>(6);

            result[0, 0] = new Fraction<int>(3, 1, integerDomain);
            result[0, 1] = new Fraction<int>(3, 1, integerDomain);
            result[0, 2] = new Fraction<int>(0, 1, integerDomain);
            result[0, 3] = new Fraction<int>(3, 1, integerDomain);
            result[0, 4] = new Fraction<int>(-4, 1, integerDomain);
            result[0, 5] = new Fraction<int>(2, 1, integerDomain);

            result[1, 0] = new Fraction<int>(3, 1, integerDomain);
            result[1, 1] = new Fraction<int>(7, 1, integerDomain);
            result[1, 2] = new Fraction<int>(4, 1, integerDomain);
            result[1, 3] = new Fraction<int>(7, 1, integerDomain);
            result[1, 4] = new Fraction<int>(-6, 1, integerDomain);
            result[1, 5] = new Fraction<int>(2, 1, integerDomain);

            result[2, 0] = new Fraction<int>(0, 1, integerDomain);
            result[2, 1] = new Fraction<int>(4, 1, integerDomain);
            result[2, 2] = new Fraction<int>(5, 1, integerDomain);
            result[2, 3] = new Fraction<int>(5, 1, integerDomain);
            result[2, 4] = new Fraction<int>(-1, 1, integerDomain);
            result[2, 5] = new Fraction<int>(-1, 1, integerDomain);

            result[3, 0] = new Fraction<int>(3, 1, integerDomain);
            result[3, 1] = new Fraction<int>(7, 1, integerDomain);
            result[3, 2] = new Fraction<int>(5, 1, integerDomain);
            result[3, 3] = new Fraction<int>(8, 1, integerDomain);
            result[3, 4] = new Fraction<int>(-5, 1, integerDomain);
            result[3, 5] = new Fraction<int>(1, 1, integerDomain);

            result[4, 0] = new Fraction<int>(-4, 1, integerDomain);
            result[4, 1] = new Fraction<int>(-6, 1, integerDomain);
            result[4, 2] = new Fraction<int>(-1, 1, integerDomain);
            result[4, 3] = new Fraction<int>(-5, 1, integerDomain);
            result[4, 4] = new Fraction<int>(8, 1, integerDomain);
            result[4, 5] = new Fraction<int>(-5, 1, integerDomain);

            result[5, 0] = new Fraction<int>(2, 1, integerDomain);
            result[5, 1] = new Fraction<int>(2, 1, integerDomain);
            result[5, 2] = new Fraction<int>(-1, 1, integerDomain);
            result[5, 3] = new Fraction<int>(1, 1, integerDomain);
            result[5, 4] = new Fraction<int>(-5, 1, integerDomain);
            result[5, 5] = new Fraction<int>(5, 1, integerDomain);

            return result;
        }
    }
}
