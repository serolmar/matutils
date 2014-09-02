namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using Utilities;

    [TestClass()]
    public class LinearLiftAlgorithmTest
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
            var mainPolText = "x^3+10*x^2-432*x+5040";
            var firstFactorText = "x";
            var secondFactorText = "x^2-2";
            var variableName = "x";
            var prime = 5;

            var integerDomain = new IntegerDomain();
            var integerParser = new IntegerParser<string>();
            var integerConversion = new ElementToElementConversion<int>();

            var mainPol = TestsHelper.ReadUnivarPolynomial(
                mainPolText,
                integerDomain,
                integerParser,
                integerConversion,
                variableName);
            var firstFactor = TestsHelper.ReadUnivarPolynomial(
                firstFactorText,
                integerDomain,
                integerParser,
                integerConversion,
                variableName);
            var secondFactor = TestsHelper.ReadUnivarPolynomial(
                secondFactorText,
                integerDomain,
                integerParser,
                integerConversion,
                variableName);
            
            // Testa o levantamento linear.
            var linearLift = new LinearLiftAlgorithm<int>(
                new ModularSymmetricIntFieldFactory(),
                new UnivarPolEuclideanDomainFactory<int>(),
                integerDomain);
            var liftingStatus = new LinearLiftingStatus<int>(mainPol, firstFactor, secondFactor, prime);
            var result = linearLift.Run(liftingStatus, 3);
            Assert.AreEqual(625, liftingStatus.LiftedFactorizationModule);

            var expected = liftingStatus.UFactor.Multiply(liftingStatus.WFactor, new ModularIntegerField(625));
            var actual = mainPol.ApplyFunction(coeff => this.GetSymmetricRemainder(coeff, 625), integerDomain);
            Assert.AreEqual(expected, actual);
        }

        private int GetSymmetricRemainder(int coeff, int modulus)
        {
            var result = coeff % modulus;
            if (result < 0)
            {
                result += modulus;
            }

            return result;
        }
    }
}
