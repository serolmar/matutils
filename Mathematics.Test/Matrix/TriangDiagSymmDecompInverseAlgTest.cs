namespace Mathematics.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    [TestClass()]
    public class TriangDiagSymmDecompInverseAlgTest
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
        public void Run_TriangDiagSymmDecompInverseAlg()
        {
            // Definição dos algoritmos.
            var target = new TriangDiagSymmDecompInverseAlg<Fraction<int>>();
            var triangDecomp = new TriangDiagSymmMatrixDecomposition<Fraction<int>>();

            // Definição dos domínios e fábricas.
            var integerDomain = new IntegerDomain();
            var fractionField = new FractionField<int>(integerDomain);
            var arrayUpperTriangMatrixFactory = new ArrayTriangUpperMatrixFactory<Fraction<int>>();
            var arrayDiagonalMatrixFactory = new ArrayDiagonalMatrixFactory<Fraction<int>>();
            var arraySquareMatrixFactory = new ArraySquareMatrixFactory<Fraction<int>>();
            var arrayMatrixFactory = new ArrayMatrixFactory<Fraction<int>>();

            // A matriz
            var matrix = this.GetDefaulMatrix(integerDomain);

            // Cálculos
            var triangDiagDecomp = triangDecomp.Run(
                matrix,
                fractionField,
                arrayUpperTriangMatrixFactory,
                arrayDiagonalMatrixFactory);
            var inverseMatrix = target.Run(
                triangDiagDecomp, 
                arraySquareMatrixFactory, 
                fractionField);
           
            // Verificação dos valores.
            var expected = ArrayMatrix<Fraction<int>>.GetIdentity(3, fractionField);
            var matrixMultiplication = new MatrixMultiplicationOperation<Fraction<int>>(
                arrayMatrixFactory,
                fractionField,
                fractionField);
            var actual = matrixMultiplication.Multiply(inverseMatrix, matrix);
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    Assert.AreEqual(expected[i, j], actual[i, j]);
                }
            }
        }

        /// <summary>
        /// Obtém a matriz que será sujeita ao teste.
        /// </summary>
        /// <param name="integerDomain">O objecto responsável pelas operações sobre os números inteiros.</param>
        /// <returns>A matriz.</returns>
        private ArraySquareMatrix<Fraction<int>> GetDefaulMatrix(IntegerDomain integerDomain)
        {
            var result = new ArraySquareMatrix<Fraction<int>>(3);

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
