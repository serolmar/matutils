namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Utilities;

    [TestClass()]
    public class DenseCondensationLinSysAlgorithmTest
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
    }
}
