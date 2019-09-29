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
            var matrixFactory = new ArrayMathMatrixFactory<Fraction<int>>();

            // Leitura da matriz que representa o sistema de equações.
            var coeffsMatrix = TestsHelper.ReadMatrix<Fraction<int>>(
                3,
                4,
                coefficientsMatrixText,
                (i, j) => new ArrayMathMatrix<Fraction<int>>(i, j),
                fractionFieldParser);

            // Leitura do vector de termos independente.
            var vectorMatrix = TestsHelper.ReadMatrix<Fraction<int>>(
                3,
                1,
                independentVectorText,
                (i, j) => new ArrayMathMatrix<Fraction<int>>(i, j),
                fractionFieldParser);

            var expectedMatrixVector = TestsHelper.ReadMatrix<Fraction<int>>(
                4,
                1,
                expectedText,
                (i, j) => new ArrayMathMatrix<Fraction<int>>(i, j),
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
            var vector = this.GetZeroFilledVector(domain);
            var actual = target.Run(matrix, vector);

            // Teste à característica da matriz
            Assert.AreEqual(2, actual.VectorSpaceBasis.Count);

            // Teste ao vector independente
            var size = matrix.GetLength(0);
            var nullVector = new ZeroVector<Fraction<int>>(size, fractionField);
            this.AssertVector(nullVector, matrix, actual.Vector, fractionField);

            // Teste aos vectores da base
            for (var i = 0; i < actual.VectorSpaceBasis.Count; ++i)
            {
                var vec = actual.VectorSpaceBasis[i];
                this.AssertVector(nullVector, matrix, vec, fractionField);
            }

            // Teste à matriz com vector independente
            vector = this.GetIndtVectorForSingularMatrix(domain);
            var expectedVector = new ArrayMathVector<Fraction<int>>(size);
            for (var i = 0; i < size; ++i)
            {
                expectedVector[i] = vector[i, 0];
            }

            actual = target.Run(matrix, vector);
            this.AssertVector(expectedVector, matrix, actual.Vector, fractionField);

            for (var i = 0; i < actual.VectorSpaceBasis.Count; ++i)
            {
                var vec = actual.VectorSpaceBasis[i];
                this.AssertVector(nullVector, matrix, vec, fractionField);
            }
        }

        /// <summary>
        /// Testa a solução de um sistema linear com base no algoritmo da
        /// decomposição de uma matriz simétrica.
        /// </summary>
        [TestMethod]
        [Description("Tests the decomposition general system solver.")]
        public void LdlDecompLinearSystemAlgorithm_RunTest()
        {
            var domain = new IntegerDomain();
            var fractionField = new FractionField<int>(domain);
            var decompositionAlg = new TriangDiagSymmMatrixDecomposition<Fraction<int>>(
                fractionField);
            var symmDecompSolver = new SymmetricLdlDecompLinearSystemAlgorithm<Fraction<int>>(
                decompositionAlg);
            var target = new LdlDecompLinearSystemAlgorithm<Fraction<int>>(
                symmDecompSolver,
                fractionField);
            var matrix = this.GetGeneralMatrix(domain);
            var vector = this.GetIndVectorForGenMatrix(domain);
            var actual = target.Run(matrix, vector);

            var lines = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            var nullVector = new ZeroVector<Fraction<int>>(lines, fractionField);
            var expectedVector = new ArrayMathVector<Fraction<int>>(lines);
            for (var i = 0; i < lines; ++i)
            {
                expectedVector[i] = vector[i, 0];
            }

            actual = target.Run(matrix, vector);
            this.AssertVector(expectedVector, matrix, actual.Vector, fractionField);

            for (var i = 0; i < actual.VectorSpaceBasis.Count; ++i)
            {
                var vec = actual.VectorSpaceBasis[i];
                this.AssertVector(nullVector, matrix, vec, fractionField);
            }
        }

        /// <summary>
        /// Vefica se o produto da matriz pelo vector actual resulta no esperado.
        /// </summary>
        /// <typeparam name="CoeffType">
        /// O tipo de objectos que constituem as entradas das estruturas.
        /// </typeparam>
        /// <param name="expected">O vector esperado.</param>
        /// <param name="matrix">a matriz.</param>
        /// <param name="field">
        /// O objecto responsável pelas operações sobre os coeficientes.
        /// </param>
        /// <param name="actual">O vector actual.</param>
        private void AssertVector<CoeffType>(
            IMathVector<CoeffType> expected,
            IMathMatrix<CoeffType> matrix,
            IMathVector<CoeffType> actual,
            IField<CoeffType> field)
        {
            var lines = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            for (var i = 0; i < lines; ++i)
            {
                var sum = field.AdditiveUnity;
                for (var j = 0; j < columns; ++j)
                {
                    var value = field.Multiply(
                        matrix[i, j],
                        actual[j]);
                    sum = field.Add(sum, value);
                }

                Assert.AreEqual(expected[i], sum);
            }
        }

        /// <summary>
        /// Obtém uma matriz singular a ser utilizada nos testes.
        /// </summary>
        /// <param name="integerDomain">
        /// O objecto responsável pelas operações sobre os números inteiros.
        /// </param>
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

        /// <summary>
        /// Obtém uma matriz para testar o sistema de equações geral.
        /// </summary>
        /// <param name="integerDomain">
        /// O objecto responsável pelas operações sobre inteiros.
        /// </param>
        /// <returns>A matriz de teste.</returns>
        private ArrayMathMatrix<Fraction<int>> GetGeneralMatrix(
            IntegerDomain integerDomain)
        {
            var result = new ArrayMathMatrix<Fraction<int>>(2, 3);
            result[0, 0] = new Fraction<int>(1, 1, integerDomain);
            result[0, 1] = new Fraction<int>(0, 1, integerDomain);
            result[0, 2] = new Fraction<int>(1, 1, integerDomain);

            result[1, 0] = new Fraction<int>(2, 1, integerDomain);
            result[1, 1] = new Fraction<int>(1, 1, integerDomain);
            result[1, 2] = new Fraction<int>(1, 1, integerDomain);

            return result;
        }

        /// <summary>
        /// Obtém o vector independente para utilizar com a matriz singular.
        /// </summary>
        /// <param name="domain">O objecto responsável pelas operações sobre os números inteiros.</param>
        /// <returns>O vector.</returns>
        private ArrayMathMatrix<Fraction<int>> GetIndtVectorForSingularMatrix(
            IntegerDomain domain)
        {
            var result = new ArrayMathMatrix<Fraction<int>>(6, 1);
            result[0, 0] = new Fraction<int>(3, 1, domain);
            result[1, 0] = new Fraction<int>(-3, 1, domain);
            result[2, 0] = new Fraction<int>(-8, 1, domain);
            result[3, 0] = new Fraction<int>(-5, 1, domain);
            result[4, 0] = new Fraction<int>(-5, 1, domain);
            result[5, 0] = new Fraction<int>(8, 1, domain);

            return result;
        }

        /// <summary>
        /// Obtém o vector independente para tetar a matriz geral.
        /// </summary>
        /// <param name="domain">O objecto responsável pelas operações sobre inteiros.</param>
        private ArrayMathMatrix<Fraction<int>> GetIndVectorForGenMatrix(
            IntegerDomain domain)
        {
            var result = new ArrayMathMatrix<Fraction<int>>(2, 1);
            result[0, 0] = new Fraction<int>(2, 1, domain);
            result[1, 0] = new Fraction<int>(5, 1, domain);

            return result;
        }

        /// <summary>
        /// Obtém um vector cujas entradas são nulas.
        /// </summary>
        /// <param name="integerDomain">
        /// O objecto responsável pelas operações sobre os números inteiros.
        /// </param>
        /// <returns>O vector.</returns>
        private ArrayMathMatrix<Fraction<int>> GetZeroFilledVector(
            IntegerDomain integerDomain)
        {
            var result = new ArrayMathMatrix<Fraction<int>>(
                6,
                1,
                new Fraction<int>(0, 1, integerDomain));
            return result;
        }
    }
}
