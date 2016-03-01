namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;
    using Utilities;

    [TestClass()]
    public class SequentialLanczosAlgorithmTest
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

        [TestMethod()]
        public void RunTest()
        {
            var inputMatrix = "[[1,2,1],[2,-1,-1],[1,-1,3]]";
            var inputVector = "[[1,2,3]]";
            var expectedText = "[[21/19,-6/19,10/19]]";

            var integerDomain = new BigIntegerDomain();
            var integerParser = new BigIntegerParser<string>();
            var fractionField = new FractionField<BigInteger>(integerDomain);
            var fractionFieldParser = new FieldDrivenExpressionParser<Fraction<BigInteger>>(
                new SimpleElementFractionParser<BigInteger>(integerParser, integerDomain),
                fractionField);
            var matrixFactory = new ArrayMatrixFactory<Fraction<BigInteger>>();

            // Leitura da matriz que representa o sistema de equações.
            var coeffsMatrix = TestsHelper.ReadMatrix<Fraction<BigInteger>>(
                3,
                3,
                inputMatrix,
                matrixFactory,
                fractionFieldParser);

            // Leitura do vector de termos independente.
            var vectorMatrix = TestsHelper.ReadMatrix<Fraction<BigInteger>>(
                3,
                1,
                inputVector,
                new ArrayMatrixFactory<Fraction<BigInteger>>(),
                fractionFieldParser);

            var expectedMatrix = TestsHelper.ReadMatrix<Fraction<BigInteger>>(
                3,
                1,
                expectedText,
                new ArrayMatrixFactory<Fraction<BigInteger>>(),
                fractionFieldParser);

            var systemSolver = new SequentialLanczosAlgorithm<Fraction<BigInteger>, FractionField<BigInteger>>(
                        matrixFactory,
                        fractionField);
            var squareMatrix = (coeffsMatrix as ArrayMathMatrix<Fraction<BigInteger>>).AsSquare();
            var actual = systemSolver.Run(squareMatrix, vectorMatrix);

            for (int i = 0; i < 3; ++i)
            {
                Assert.AreEqual(expectedMatrix[i, 0], actual[i, 0]);
            }
        }
    }
}
