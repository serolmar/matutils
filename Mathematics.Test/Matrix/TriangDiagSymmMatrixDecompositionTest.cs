namespace Mathematics.Test
{
    using Utilities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class TriangDiagSymmMatrixDecompositionTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        [Description("Testa a decomposição de uma matriz simétrica num produto triangular e diagonal.")]
        public void RunTest_TrianguDiagSummMatrixDcomp()
        {
            // Definiçã dos argumentos.
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);
            var upperTriangularMatrixFactory = new ArrayTriangUpperMatrixFactory<Fraction<int>>();
            var diagonalMatrixFactory = new ArrayDiagonalMatrixFactory<Fraction<int>>();

            // Definição da matriz.
            var matrix = this.GetDefaulMatrix(integerDomain);

            // Execução do algoritmo.
            var target = new TriangDiagSymmMatrixDecomposition<Fraction<int>>();
            var decomposition = target.Run(
                matrix,
                fractionField,
                upperTriangularMatrixFactory,
                diagonalMatrixFactory);

            // Calcula o valor esperado.
            var matrixFactory = new ArrayMatrixFactory<Fraction<int>>();
            var matrixMultiplicaton = new MatrixMultiplicationOperation<Fraction<int>>(
                matrixFactory,
                fractionField,
                fractionField);
            var actual = new TransposeMatrix<Fraction<int>>(decomposition.UpperTriangularMatrix)
                as IMatrix<Fraction<int>>;
            actual = matrixMultiplicaton.Multiply(actual, decomposition.DiagonalMatrix);
            actual = matrixMultiplicaton.Multiply(actual,decomposition.UpperTriangularMatrix);

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
        private ArraySquareMathMatrix<Fraction<int>> GetDefaulMatrix(IntegerDomain integerDomain)
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
    }
}
