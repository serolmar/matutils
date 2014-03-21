namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using Utilities;

    [TestClass()]
    public class UnivariatePolynomialNormalFormTest
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
        public void GetPolynomialDerivativeTest_SimpleInteger()
        {
            // Representação dos polinómios.
            var polynomText = "x^1000-2*x^550+1000*x^10+50";
            var polDerivativeText = "1000*x^999-1100*x^549+10000*x^9";

            var variableName = "x";

            // Estabelece os domínios.
            var integerDomain = new IntegerDomain();
            var longDomain = new LongDomain();
            var bigIntegerDomain = new BigIntegerDomain();

            // Estabelece os conversores.
            var integerToIntegerConv = new ElementToElementConversion<int>();
            var integerToLongConv = new LongToIntegerConversion();
            var integerToBigIntegerConvsersion = new BigIntegerToIntegerConversion();

            // Estabelece os leitores individuais.
            var integerParser = new IntegerParser<string>();
            var longParser = new LongParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();

            // Estabelece os polinómios.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                integerDomain,
                integerParser,
                integerToIntegerConv,
                variableName);
            var integerExpectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                polDerivativeText,
                integerDomain,
                integerParser,
                integerToIntegerConv,
                variableName);
            var integerActualDerivative = integerPolynomial.GetPolynomialDerivative(integerDomain);

            // Verifica se os polinómios são válidos.
            Assert.AreEqual(integerExpectedPolynomial, integerActualDerivative);

            // Estabelece os polinómios.
            var longPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                longDomain,
                longParser,
                integerToLongConv,
                variableName);
            var longExpectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                polDerivativeText,
                longDomain,
                longParser,
                integerToLongConv,
                variableName);
            var longActualDerivative = longPolynomial.GetPolynomialDerivative(longDomain);

            // Verifica se os polinómios são válidos.
            Assert.AreEqual(longExpectedPolynomial, longActualDerivative);

            // Estabelece os polinómios.
            var bigIntegerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                bigIntegerDomain,
                bigIntegerParser,
                integerToBigIntegerConvsersion,
                variableName);
            var bigIntegerExpectedPolynomial = TestsHelper.ReadUnivarPolynomial(
                polDerivativeText,
                bigIntegerDomain,
                bigIntegerParser,
                integerToBigIntegerConvsersion,
                variableName);
            var bigIntegerActualDerivative = bigIntegerPolynomial.GetPolynomialDerivative(bigIntegerDomain);

            // Verifica se os polinómios são válidos.
            Assert.AreEqual(bigIntegerExpectedPolynomial, bigIntegerActualDerivative);
        }

        [TestMethod()]
        public void GetPolynomialDerivativeTest_IntegerFraction()
        {
            var polynomialText = "1/2*x^5+3/4*x^4-2/7*x^3+5/3*x^2+1/5*x+9";
            var polynomialDerivativeText = "5/2*x^4+3*x^3-6/7*x^2+10/3*x+1/5";
            var variableName = "x";

            var integerDomain = new IntegerDomain();
            var longDomain = new LongDomain();
            var bigIntegerDomain = new BigIntegerDomain();

            var integerParser = new IntegerParser<string>();
            var longParser = new LongParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();

            var longConversion = new LongToIntegerConversion();
            var bigIntegerConversion = new BigIntegerToIntegerConversion();

            var integerFractionConversion = new ElementFractionConversion<int>(integerDomain);
            var longfractionConversion = new OuterElementFractionConversion<int, long>(longConversion, longDomain);
            var bigIntegerfractionConversion = new OuterElementFractionConversion<int, BigInteger>(bigIntegerConversion, bigIntegerDomain);

            var integerFractionField = new FractionField<int>(integerDomain);
            var longFractionField = new FractionField<long>(longDomain);
            var bigIntegerFractionField = new FractionField<BigInteger>(bigIntegerDomain);

            // Coeficientes inteiros
            var integerPolynomial = TestsHelper.ReadFractionalCoeffsUnivarPol<int, IntegerDomain>(
                polynomialText,
                integerDomain,
                integerParser,
                integerFractionConversion,
                variableName);

            var integerPolynomialDerivative = TestsHelper.ReadFractionalCoeffsUnivarPol<int, IntegerDomain>(
                polynomialDerivativeText,
                integerDomain,
                integerParser,
                integerFractionConversion,
                variableName);
            var integerActualPolDerivative = integerPolynomial.GetPolynomialDerivative(integerFractionField);
            Assert.AreEqual(integerPolynomialDerivative, integerActualPolDerivative);

            // Coeficientes longos
            var longPolynomial = TestsHelper.ReadFractionalCoeffsUnivarPol<long, LongDomain>(
                polynomialText,
                longDomain,
                longParser,
                longfractionConversion,
                variableName);

            var longPolynomialDerivative = TestsHelper.ReadFractionalCoeffsUnivarPol<long, LongDomain>(
                polynomialDerivativeText,
                longDomain,
                longParser,
                longfractionConversion,
                variableName);
            var longActualPolDerivative = longPolynomial.GetPolynomialDerivative(longFractionField);
            Assert.AreEqual(longPolynomialDerivative, longActualPolDerivative);

            var bigIntegerPolynomial = TestsHelper.ReadFractionalCoeffsUnivarPol<BigInteger, BigIntegerDomain>(
                polynomialText,
                bigIntegerDomain,
                bigIntegerParser,
                bigIntegerfractionConversion,
                variableName);

            var bigIntegerPolynomialDerivative = TestsHelper.ReadFractionalCoeffsUnivarPol<BigInteger, BigIntegerDomain>(
                polynomialDerivativeText,
                bigIntegerDomain,
                bigIntegerParser,
                bigIntegerfractionConversion,
                variableName);
            var bigIntegerActualPolDerivative = bigIntegerPolynomial.GetPolynomialDerivative(bigIntegerFractionField);
            Assert.AreEqual(bigIntegerPolynomialDerivative, bigIntegerActualPolDerivative);
        }

        [TestMethod()]
        public void GetPolynomialDerivativeTest_IntegerPolynomial()
        {
            // Os valores a serem lidos
            var polynomialText = "[[1,2],[3,4]]*x^2-[[1,0],[0,1]]*x+[[7,6],[9,8]]";
            var polynomialDerivativeText = "[[2,4],[6,8]]*x+[[-1,0],[0,-1]]";
            var variableName = "x";

            var arrayDelimiters = new Dictionary<string,string>();
            arrayDelimiters.Add("left_bracket", "right_bracket");

            // Os domínios responsáveis sobre as operações sobre os inteiros.
            var integerDomain = new IntegerDomain();
            var longDomain = new LongDomain();
            var bigIntegerDomain = new BigIntegerDomain();

            // Os leitore sde inteiros
            var integerParser = new IntegerParser<string>();
            var longParser = new LongParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();

            // As fábricas responsáveis pela instanciação de matrizes
            var integerSquareArrayMatrixfactory = new ArraySquareMatrixFactory<int>();
            var longSquareArrayMatrixFactory = new ArraySquareMatrixFactory<long>();
            var bigIntegerSquareArrayMatrixfactory = new ArraySquareMatrixFactory<BigInteger>();

            // Os anéis de matrizes
            var integerGenericMatrixRing = new GeneralMatrixRing<int>(
                2,
                integerSquareArrayMatrixfactory,
                integerDomain);
            var longGenericMatrixRing = new GeneralMatrixRing<long>(
                2,
                longSquareArrayMatrixFactory,
                longDomain);
            var bigIntegerGenericMatrixRing = new GeneralMatrixRing<BigInteger>(
                2,
                bigIntegerSquareArrayMatrixfactory,
                bigIntegerDomain);

            // Os objectos responsáveis pela conversão entre os coeficientes e o grau (inteiro)
            var integerMatrixConversion = new CantConvertConversion<int, IMatrix<int>>();
            var longMatrixConversion = new CantConvertConversion<int, IMatrix<long>>();
            var bigIntegerMatrixConversion = new CantConvertConversion<int, IMatrix<BigInteger>>();

            var integerMatrixConfigParser = new ConfigMatrixParser<int>(
                integerParser,
                2,
                2,
                integerSquareArrayMatrixfactory);
            integerMatrixConfigParser.SeparatorSymbType = "comma";
            integerMatrixConfigParser.MapInternalDelimiters("left_bracket", "right_bracket");
            integerMatrixConfigParser.AddBlanckSymbolType("blancks");

            var longMatrixConfigParser = new ConfigMatrixParser<long>(
                longParser,
                2,
                2,
                longSquareArrayMatrixFactory);
            longMatrixConfigParser.SeparatorSymbType = "comma";
            longMatrixConfigParser.MapInternalDelimiters("left_bracket", "right_bracket");
            longMatrixConfigParser.AddBlanckSymbolType("blancks");

            var bigIntegerMatrixConfigParser = new ConfigMatrixParser<BigInteger>(
                bigIntegerParser,
                2,
                2,
                bigIntegerSquareArrayMatrixfactory);
            bigIntegerMatrixConfigParser.SeparatorSymbType = "comma";
            bigIntegerMatrixConfigParser.MapInternalDelimiters("left_bracket", "right_bracket");
            bigIntegerMatrixConfigParser.AddBlanckSymbolType("blancks");

            // Leitura dos polinómios e subsequente teste.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomialText,
                integerGenericMatrixRing,
                integerMatrixConfigParser,
                integerMatrixConversion,
                variableName,
                arrayDelimiters);
            var integerExpectedDerivative = TestsHelper.ReadUnivarPolynomial(
                polynomialDerivativeText,
                integerGenericMatrixRing,
                integerMatrixConfigParser,
                integerMatrixConversion,
                variableName,
                arrayDelimiters);
            var integerActualDerivative = integerPolynomial.GetPolynomialDerivative(integerGenericMatrixRing);
            Assert.IsTrue(
                new UnivarPolynomNormalFormEqualityComparer<IMatrix<int>>(integerGenericMatrixRing).Equals(integerExpectedDerivative, integerActualDerivative),
                string.Format("Expected {0} isn't equal to actual {1}.", integerExpectedDerivative, integerActualDerivative));

            var longPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomialText,
                longGenericMatrixRing,
                longMatrixConfigParser,
                longMatrixConversion,
                variableName,
                arrayDelimiters);
            var longExpectedDerivative = TestsHelper.ReadUnivarPolynomial(
                polynomialDerivativeText,
                longGenericMatrixRing,
                longMatrixConfigParser,
                longMatrixConversion,
                variableName,
                arrayDelimiters);
            var longActualDerivative = longPolynomial.GetPolynomialDerivative(longGenericMatrixRing);
            Assert.IsTrue(
                new UnivarPolynomNormalFormEqualityComparer<IMatrix<long>>(longGenericMatrixRing).Equals(longExpectedDerivative, longActualDerivative),
                string.Format("Expected {0} isn't equal to actual {1}.", integerExpectedDerivative, integerActualDerivative));

            var bigIntegerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomialText,
                bigIntegerGenericMatrixRing,
                bigIntegerMatrixConfigParser,
                bigIntegerMatrixConversion,
                variableName,
                arrayDelimiters);
            var bigIntegerExpectedDerivative = TestsHelper.ReadUnivarPolynomial(
                polynomialDerivativeText,
                bigIntegerGenericMatrixRing,
                bigIntegerMatrixConfigParser,
                bigIntegerMatrixConversion,
                variableName,
                arrayDelimiters);
            var bigIntegerActualDerivative = bigIntegerPolynomial.GetPolynomialDerivative(bigIntegerGenericMatrixRing);
            Assert.IsTrue(
                new UnivarPolynomNormalFormEqualityComparer<IMatrix<BigInteger>>(
                    bigIntegerGenericMatrixRing).Equals(bigIntegerExpectedDerivative, 
                    bigIntegerActualDerivative),
                string.Format("Expected {0} isn't equal to actual {1}.", integerExpectedDerivative, integerActualDerivative));
        }

        [TestMethod()]
        public void GetPolynomialDerivativeTest_IntegerPolynomialAsCoefficients()
        {
            var polynomialText = "(y^2+y+1)*x^3-2*x^2*y+x*(y^5-3)+4";
            var polynomialDerivative = "3*(y^2+y+1)*x^2-4*y*x+y^5-3";
            var variableName = "x";
            var coeffsVariableName = "y";

            // Os domínios responsáveis pelas operações sobre os inteiros.
            var integerDomain = new IntegerDomain();
            var longDomain = new LongDomain();
            var bigIntegerDomain = new BigIntegerDomain();

            // Os leitore sde inteiros
            var integerParser = new IntegerParser<string>();
            var longParser = new LongParser<string>();
            var bigIntegerParser = new BigIntegerParser<string>();

            var integerConversion = new ElementToElementConversion<int>();

            var integerPolConvertion = new UnivarPolynomNormalFormToIntegerConversion<int>(
                coeffsVariableName,
                integerConversion,
                integerDomain);

            var integerPolynomialRing = new UnivarPolynomRing<int>(coeffsVariableName, integerDomain);

            var integerPolynomialParser = new UnivarPolNormalFormParser<int>(
                coeffsVariableName,
                integerConversion,
                integerParser,
                integerDomain);

            var integerPolynomial = TestsHelper.ReadUnivarPolynomial<UnivariatePolynomialNormalForm<int>>(
                polynomialText,
                integerPolynomialRing,
                integerPolynomialParser,
                integerPolConvertion,
                variableName);
        }
    }
}
