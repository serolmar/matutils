using Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
        public void GetPolynomialDerivativeTest_IntegerMatrix()
        {
            // Os valores a serem lidos
            var polynomialText = "[[1,2],[3,4]]*x^2-[[1,0],[0,1]]*x+[[7,6],[9,8]]";
            var polynomialDerivativeText = "[[2,4],[6,8]]*x+[[-1,0],[0,-1]]";
            var variableName = "x";

            var arrayDelimiters = new Dictionary<string, string>();
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
                arrayDelimiters,
                true);
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
                arrayDelimiters,
                true);
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
                arrayDelimiters,
                true);
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
            var polynomialDerivativeText = "3*(y^2+y+1)*x^2-4*y*x+y^5-3";
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

            // Definição das conversões.
            var integerConversion = new ElementToElementConversion<int>();
            var longConversion = new LongToIntegerConversion();
            var bigIntegerConversion = new BigIntegerToIntegerConversion();

            var integerPolConvertion = new UnivarPolynomNormalFormToIntegerConversion<int>(
                coeffsVariableName,
                integerConversion,
                integerDomain);
            var longPolConvertion = new UnivarPolynomNormalFormToIntegerConversion<long>(
                coeffsVariableName,
                longConversion,
                longDomain);
            var bigIntegerPolConvertion = new UnivarPolynomNormalFormToIntegerConversion<BigInteger>(
                coeffsVariableName,
                bigIntegerConversion,
                bigIntegerDomain);

            // Definição dos anéis polinomiais.
            var integerPolynomialRing = new UnivarPolynomRing<int>(coeffsVariableName, integerDomain);
            var longPolynomialRing = new UnivarPolynomRing<long>(coeffsVariableName, longDomain);
            var bigIntegerPolynomialRing = new UnivarPolynomRing<BigInteger>(coeffsVariableName, bigIntegerDomain);

            // Definição dos leitores polinomiais.
            var integerPolynomialParser = new UnivarPolNormalFormParser<int>(
                coeffsVariableName,
                integerConversion,
                integerParser,
                integerDomain);
            var longPolynomialParser = new UnivarPolNormalFormParser<long>(
                coeffsVariableName,
                longConversion,
                longParser,
                longDomain);
            var bigIntegerPolynomialParser = new UnivarPolNormalFormParser<BigInteger>(
                coeffsVariableName,
                bigIntegerConversion,
                bigIntegerParser,
                bigIntegerDomain);

            // Definição dos testes.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial<UnivariatePolynomialNormalForm<int>>(
                polynomialText,
                integerPolynomialRing,
                integerPolynomialParser,
                integerPolConvertion,
                variableName);
            var integerExpectedPol = TestsHelper.ReadUnivarPolynomial<UnivariatePolynomialNormalForm<int>>(
                polynomialDerivativeText,
                integerPolynomialRing,
                integerPolynomialParser,
                integerPolConvertion,
                variableName);
            var integerActualPlynomial = integerPolynomial.GetPolynomialDerivative(integerPolynomialRing);
            Assert.AreEqual(integerExpectedPol, integerActualPlynomial);

            var longPolynomial = TestsHelper.ReadUnivarPolynomial<UnivariatePolynomialNormalForm<long>>(
                polynomialText,
                longPolynomialRing,
                longPolynomialParser,
                longPolConvertion,
                variableName);
            var longExpectedPol = TestsHelper.ReadUnivarPolynomial<UnivariatePolynomialNormalForm<long>>(
                polynomialDerivativeText,
                longPolynomialRing,
                longPolynomialParser,
                longPolConvertion,
                variableName);
            var longActualPlynomial = longPolynomial.GetPolynomialDerivative(longPolynomialRing);
            Assert.AreEqual(longExpectedPol, longActualPlynomial);

            var bigIntegerPolynomial = TestsHelper.ReadUnivarPolynomial<UnivariatePolynomialNormalForm<BigInteger>>(
                polynomialText,
                bigIntegerPolynomialRing,
                bigIntegerPolynomialParser,
                bigIntegerPolConvertion,
                variableName);
            var bigIntegerExpectedPol = TestsHelper.ReadUnivarPolynomial<UnivariatePolynomialNormalForm<BigInteger>>(
                polynomialDerivativeText,
                bigIntegerPolynomialRing,
                bigIntegerPolynomialParser,
                bigIntegerPolConvertion,
                variableName);
            var bigIntegerActualPlynomial = bigIntegerPolynomial.GetPolynomialDerivative(bigIntegerPolynomialRing);
            Assert.AreEqual(bigIntegerExpectedPol, bigIntegerExpectedPol);
        }

        [TestMethod()]
        public void GetRootPowerSumsTest_Integer()
        {
            // Representação dos polinómios.
            var polynomText = "(x-3)*(x-2)^2*(x+1)^3";
            var variableName = "x";

            // Estabelece os domínios.
            var integerDomain = new IntegerDomain();

            // Estabelece os conversores.
            var integerToIntegerConv = new ElementToElementConversion<int>();

            // Estabelece os leitores individuais.
            var integerParser = new IntegerParser<string>();

            // Estabelece os polinómios.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                integerDomain,
                integerParser,
                integerToIntegerConv,
                variableName);
            var integerExpectedVector = new ArrayVector<int>(6);
            integerExpectedVector[0] = 4;
            integerExpectedVector[1] = 20;
            integerExpectedVector[2] = 40;
            integerExpectedVector[3] = 116;
            integerExpectedVector[4] = 304;
            integerExpectedVector[5] = 860;
            var integerActualVector = integerPolynomial.GetRootPowerSums(integerDomain);
            Assert.AreEqual(integerExpectedVector.Length, integerActualVector.Length, "Vector lengths aren't equal.");
            for (int i = 0; i < integerActualVector.Length; ++i)
            {
                Assert.AreEqual(integerExpectedVector[i], integerActualVector[i]);
            }
        }

        [TestMethod()]
        public void GetRootPowerSumsTest_IntegerFraction()
        {
            // Representação dos polinómios.
            var polynomText = "(x-3)*(x-2)^2*(x+1)^3";
            var variableName = "x";

            // Estabelece os domínios.
            var integerDomain = new IntegerDomain();

            // Estabelece o corpo responsável pelas operações sobre as fracções.
            var fractionField = new FractionField<int>(integerDomain);

            // Estabelece os conversores.
            var integerToFractionConversion = new ElementFractionConversion<int>(integerDomain);

            // Estabelece os leitores individuais.
            var integerParser = new IntegerParser<string>();

            // Estabelece o leitor de fracções.
            var fractionParser = new ElementFractionParser<int>(integerParser, integerDomain);

            // Estabelece os polinómios.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                fractionField,
                fractionParser,
                integerToFractionConversion,
                variableName);
            var integerFractionExpectedVector = new ArrayVector<Fraction<int>>(6);
            integerFractionExpectedVector[0] = new Fraction<int>(4, 1, integerDomain);
            integerFractionExpectedVector[1] = new Fraction<int>(20, 1, integerDomain);
            integerFractionExpectedVector[2] = new Fraction<int>(40, 1, integerDomain);
            integerFractionExpectedVector[3] = new Fraction<int>(116, 1, integerDomain);
            integerFractionExpectedVector[4] = new Fraction<int>(304, 1, integerDomain);
            integerFractionExpectedVector[5] = new Fraction<int>(860, 1, integerDomain);
            var integerFractionActualVector = integerPolynomial.GetRootPowerSums(
                fractionField, 
                new SparseDictionaryVectorFactory<Fraction<int>>());
            Assert.AreEqual(
                integerFractionExpectedVector.Length, 
                integerFractionActualVector.Length, 
                "Vector lengths aren't equal.");
            for (int i = 0; i < integerFractionActualVector.Length; ++i)
            {
                Assert.AreEqual(integerFractionExpectedVector[i], integerFractionActualVector[i]);
            }
        }

        [TestMethod()]
        public void ReplaceTest_Integer()
        {
            // Representação dos polinómios.
            var polynomText = "x^5+2*x^4+3*x^3+4*x^2+5*x+6";
            var variableName = "x";

            // Estabelece os domínios.
            var integerDomain = new IntegerDomain();

            // Estabelece os conversores.
            var integerToIntegerConv = new ElementToElementConversion<int>();

            // Estabelece os leitores individuais.
            var integerParser = new IntegerParser<string>();

            // Estabelece os polinómios.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                integerDomain,
                integerParser,
                integerToIntegerConv,
                variableName);
            var integerReplaceValues = new int[] { 0, 1, 2, 3 };
            var integerExpectedValues = new int[] { 6, 21, 120, 543 };
            for (int i = 0; i < integerReplaceValues.Length; ++i)
            {
                var integerActualValue = integerPolynomial.Replace(integerReplaceValues[i], integerDomain);
                Assert.AreEqual(integerExpectedValues[i], integerActualValue);
            }
        }

        [TestMethod()]
        public void ReplaceTest_ReplaceByFraction()
        {
            // Representação dos polinómios.
            var polynomText = "x^5+2*x^4+3*x^3+4*x^2+5*x+6";
            var variableName = "x";

            // Estabelece os domínios.
            var integerDomain = new IntegerDomain();

            // Estabelece os conversores.
            var integerToIntegerConv = new ElementToElementConversion<int>();

            // Estabelece os leitores individuais.
            var integerParser = new IntegerParser<string>();

            var fractionField = new FractionField<int>(integerDomain);

            var integerFractionAddOp = new ElementFractionAddOper<int>(integerDomain);

            // Estabelece os polinómios.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                integerDomain,
                integerParser,
                integerToIntegerConv,
                variableName);
            var fractionValues = new Fraction<int>[] { 
                new Fraction<int>(0,1,integerDomain),
                new Fraction<int>(1,1,integerDomain),
                new Fraction<int>(1,2,integerDomain),
                new Fraction<int>(1,3,integerDomain)};

            var fractionExpectedValues = new Fraction<int>[] { 
                new Fraction<int>(6,1,integerDomain),
                new Fraction<int>(21,1,integerDomain),
                new Fraction<int>(321,32,integerDomain),
                new Fraction<int>(2005,243,integerDomain)};

            for (int i = 0; i < fractionValues.Length; ++i)
            {
                var integerActualValue = integerPolynomial.Replace(
                    fractionValues[i], 
                    integerFractionAddOp,
                    fractionField);
                Assert.AreEqual(fractionExpectedValues[i], integerActualValue);
            }
        }

        [TestMethod()]
        public void ReplaceTest_ReplaceByMatrixWithMatrixAlgebra()
        {
            // Representação dos polinómios.
            var polynomText = "x^2 + 2*x + 1";
            var variableName = "x";

            var integerDomain = new IntegerDomain();
            var integerToIntegerConv = new ElementToElementConversion<int>();
            var integerParser = new IntegerParser<string>();
            var fractionField = new FractionField<int>(integerDomain);
            var fractionFieldParser = new FieldDrivenExpressionParser<Fraction<int>>(
                new SimpleElementFractionParser<int>(integerParser, integerDomain),
                fractionField);

            var polynomial = TestsHelper.ReadUnivarPolynomial<Fraction<int>>(
                polynomText,
                fractionField,
                fractionFieldParser,
                new ElementFractionConversion<int>(integerDomain),
                variableName);

            // Leitura da matriz.
            var matrix = TestsHelper.ReadMatrix<Fraction<int>>(
                2, 
                2, 
                "[[1/2+1/3,1/2-1/3],[1/5+1/4,1/5-1/4]]", 
                new ArrayMatrixFactory<Fraction<int>>(), 
                fractionFieldParser);

            var matrixAlgebra = new GeneralMatrixAlgebra<Fraction<int>>(
                2,
                new ArrayMatrixFactory<Fraction<int>>(),
                fractionField);
            var actual = polynomial.Replace(matrix, matrixAlgebra);
            var expected = TestsHelper.ReadMatrix<Fraction<int>>(
                2,
                2,
                "[[1237/360,167/360],[501/400,391/400]]",
                new ArrayMatrixFactory<Fraction<int>>(),
                fractionFieldParser);
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    Assert.AreEqual(expected[i, j], actual[i, j]);
                }
            }
        }

        [TestMethod()]
        public void GetRootPowerSumsTest()
        {
            // Representação dos polinómios.
            var polynomText = "(x-3)*(x-2)^2*(x+1)^3";
            var variableName = "x";

            // Estabelece os domínios.
            var integerDomain = new IntegerDomain();

            // Estabelece o corpo responsável pelas operações sobre as fracções.
            var fractionField = new FractionField<int>(integerDomain);

            // Estabelece os conversores.
            var integerToFractionConversion = new ElementFractionConversion<int>(integerDomain);

            // Estabelece os leitores individuais.
            var integerParser = new IntegerParser<string>();

            // Estabelece o leitor de fracções.
            var fractionParser = new ElementFractionParser<int>(integerParser, integerDomain);

            // Estabelece os polinómios.
            var integerPolynomial = TestsHelper.ReadUnivarPolynomial(
                polynomText,
                fractionField,
                fractionParser,
                integerToFractionConversion,
                variableName);

            var number = 10;
            var roots = new int[] { 3, 2, 2, -1, -1, -1 };
            var powerRoots = new int[] { 3, 2, 2, -1, -1, -1 };
            var integerFractionExpectedVector = new ArrayVector<Fraction<int>>(number);

            // Primeiro cálculo
            var sum = powerRoots[0];
            for (int i = 1; i < powerRoots.Length; ++i)
            {
                sum += powerRoots[i];
            }

            integerFractionExpectedVector[0] = new Fraction<int>(sum, 1, integerDomain);
            for (int i = 1; i < number; ++i)
            {
                for (int j = 0; j < roots.Length; ++j)
                {
                    powerRoots[j] *= roots[j];
                }

                sum = powerRoots[0];
                for (int j = 1; j < powerRoots.Length; ++j)
                {
                    sum += powerRoots[j];
                }

                integerFractionExpectedVector[i] = new Fraction<int>(sum, 1, integerDomain);
            }

            var integerFractionActualVector = integerPolynomial.GetRootPowerSums(
                number,
                fractionField, 
                new SparseDictionaryVectorFactory<Fraction<int>>());
            Assert.AreEqual(
                integerFractionExpectedVector.Length,
                integerFractionActualVector.Length,
                "Vector lengths aren't equal.");
            for (int i = 0; i < integerFractionActualVector.Length; ++i)
            {
                Assert.AreEqual(integerFractionExpectedVector[i], integerFractionActualVector[i]);
            }
        }
    }
}
