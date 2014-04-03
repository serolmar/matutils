namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass()]
    public class SubsetSumLLLReductionAlgorithmTest
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
        public void SubsetSumLLLReductionAlgorithmConstructorTest()
        {
            var integerDomain = new IntegerDomain();
            var decimalField = new DecimalField();
            var vectorFactory = new ArrayVectorFactory<decimal>();
            var decimalComparer = Comparer<decimal>.Default;
            var nearest = new DecimalNearestInteger();

            // Permite calcular o produto escalar de vectores sobre o corpo de decimais.
            var scalarProd = new OrthoVectorScalarProduct<decimal>(
                        decimalComparer,
                        decimalField);

            // Permite converter um número decimal num inteiro, caso seja possível.
            var integerDecimalConverter = new IntegerDecimalConverter();

            var subsetSumAlg = new SubsetSumLLLReductionAlgorithm<int, decimal>(
                vectorFactory,
                scalarProd,
                nearest,
                Comparer<decimal>.Default,
                integerDecimalConverter,
                decimalField);

            var vector = new[] { 366, 385, 392, 401, 422, 437 };
            var result = subsetSumAlg.Run(vector, 1215, 3M / 4);
            var expected = new[] { 392, 401, 422 };
            CollectionAssert.AreEquivalent(expected, result);
        }
    }
}
