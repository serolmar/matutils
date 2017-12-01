// -----------------------------------------------------------------------
// <copyright file="TriangDiagSymmMatrixDecompositionTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics.Test
{
    using Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Efectua um teste aos algoritmos de decomposição.
    /// </summary>
    [TestClass]
    public class TriangDiagSymmMatrixDecompositionTest
    {
        /// <summary>
        /// Testa a decomposição de uma matriz simétrica num produto triangular e diagonal.
        /// </summary>
        [TestMethod]
        [Description("Tests the matrix decomposition.")]
        public void RunTest_TrianguDiagSummMatrixDecomposition()
        {
            // Matrix normal
            var domain = new IntegerDomain();
            var fractionField = new FractionField<int>(domain);
            var target = new TriangDiagSymmMatrixDecomposition<Fraction<int>>(fractionField);

            var matrix = this.GetDefaulMatrix(domain);
            Assert.IsTrue(matrix.IsSymmetric(fractionField));
            this.TestDecomposition(
                target, 
                matrix);

            // Matriz singular
            matrix = this.GetSingularMatrix(domain);
            Assert.IsTrue(matrix.IsSymmetric(fractionField));
            this.TestDecomposition(target, matrix);
        }

        /// <summary>
        /// Testa a decomposição de uma matriz simétrica num produto triangular e diagonal.
        /// </summary>
        [TestMethod]
        [Description("Tests the parallel matrix decomposition.")]
        public void RunTest_ParallelTrianguDiagSummMatrixDecomp()
        {
            var domain = new IntegerDomain();
            var fractionField = new FractionField<int>(domain);
            var target = new ParallelTriangDiagSymmMatrixDecomp<Fraction<int>>(fractionField);

            // Matrix normal
            var matrix = this.GetDefaulMatrix(domain);
            Assert.IsTrue(matrix.IsSymmetric(fractionField));
            this.TestDecomposition(target, matrix);

            // Matriz singular
            matrix = this.GetSingularMatrix(domain);
            Assert.IsTrue(matrix.IsSymmetric(fractionField));
            this.TestDecomposition(target, matrix);
        }

        /// <summary>
        /// Testa a deomposição sequencial relativa à matriz.
        /// </summary>
        /// <param name="target">O algoritmo.</param>
        /// <param name="matrix">A matriz.</param>
        private void TestDecomposition(
            ATriangDiagSymmMatrixDecomp<Fraction<int>> target,
            ISquareMathMatrix<Fraction<int>> matrix)
        {
            // Execução do algoritmo.
            var decomposition = target.Run(
                matrix);

            // Calcula o valor esperado.
            var matrixFactory = new ArrayMatrixFactory<Fraction<int>>();
            var matrixMultiplicaton = new MatrixMultiplicationOperation<Fraction<int>>(
                matrixFactory,
                target.Field,
                target.Field);
            var actual = new TransposeMatrix<Fraction<int>>(decomposition.UpperTriangularMatrix)
                as IMatrix<Fraction<int>>;
            actual = matrixMultiplicaton.Multiply(actual, decomposition.DiagonalMatrix);
            actual = matrixMultiplicaton.Multiply(actual, decomposition.UpperTriangularMatrix);

            // Valida as asserções.
            Assert.AreEqual(matrix.GetLength(0), actual.GetLength(0));
            Assert.AreEqual(matrix.GetLength(1), actual.GetLength(1));
            for (int i = 0; i < actual.GetLength(0); ++i)
            {
                for (int j = 0; j < actual.GetLength(1); ++j)
                {
                    Assert.AreEqual(matrix[i, j], actual[i, j]);
                }
            }
        }

        /// <summary>
        /// Obtém a matriz que será sujeita ao teste.
        /// </summary>
        /// <param name="integerDomain">O objecto responsável pelas operações sobre os números inteiros.</param>
        /// <returns>A matriz.</returns>
        private ArraySquareMathMatrix<Fraction<int>> GetDefaulMatrix(
            IntegerDomain integerDomain)
        {
            var result = new ArraySquareMathMatrix<Fraction<int>>(3);

            // Primeira linha
            result[0, 0] = new Fraction<int>(4, 1, integerDomain);
            result[0, 1] = new Fraction<int>(12, 1, integerDomain);
            result[0, 2] = new Fraction<int>(-16, 1, integerDomain);

            // Segunda linha
            result[1, 0] = new Fraction<int>(12, 1, integerDomain);
            result[1, 1] = new Fraction<int>(37, 1, integerDomain);
            result[1, 2] = new Fraction<int>(-43, 1, integerDomain);

            // Terceira linha
            result[2, 0] = new Fraction<int>(-16, 1, integerDomain);
            result[2, 1] = new Fraction<int>(-43, 1, integerDomain);
            result[2, 2] = new Fraction<int>(98, 1, integerDomain);

            return result;
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
