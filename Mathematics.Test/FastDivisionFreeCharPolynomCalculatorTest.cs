namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Numerics;
    using Utilities;

    [TestClass()]
    public class FastDivisionFreeCharPolynomCalculatorTest
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
        public void RunTest_IntegerMatrix()
        {
            // A leitura é realizada por colunas.
            var matrixText = "[[1,-1,2], [3,4,5], [2,1,1]]";
            var integerDomain = new IntegerDomain();
            var variableName = "x";
            var integerParser = new IntegerParser<string>();
            var conversion = new ElementToElementConversion<int>();
            var matrix = TestsHelper.ReadMatrix(
                3,
                3,
                matrixText,
                new ArraySquareMatrixFactory<int>(),
                integerParser,
                true);
            var fastDivFreeCharacPolAlg = new FastDivisionFreeCharPolynomCalculator<int>(variableName, integerDomain);
            var expected = TestsHelper.ReadUnivarPolynomial("x^3-6*x^2+3*x+18", integerDomain, integerParser, conversion, variableName);
            var actual = fastDivFreeCharacPolAlg.Run(matrix as ISquareMatrix<int>);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void RunTest_BigIntegerMatrix()
        {
            // A leitura é realizada por colunas.
            var matrixText = "[[100000,1001,20005], [32534,4245341,56134513451], [21346136,1134613,1136135613]]";
            var integerDomain = new BigIntegerDomain();
            var variableName = "x";
            var integerParser = new BigIntegerParser<string>();
            var conversion = new BigIntegerToIntegerConversion();
            var matrix = TestsHelper.ReadMatrix(
                3,
                3,
                matrixText,
                new ArraySquareMatrixFactory<BigInteger>(),
                integerParser);
            var fastDivFreeCharacPolAlg = new FastDivisionFreeCharPolynomCalculator<BigInteger>(variableName, integerDomain);
            var expected = TestsHelper.ReadUnivarPolynomial("1*x^3+-1140480954*x^2-58754054577367644*x+4689162494877443109176", integerDomain, integerParser, conversion, variableName);
            var actual = fastDivFreeCharacPolAlg.Run(matrix as ISquareMatrix<BigInteger>);
            Assert.AreEqual(expected, actual);
        }
    }
}
