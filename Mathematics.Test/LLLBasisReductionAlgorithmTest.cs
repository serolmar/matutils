namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass()]
    public class LLLBasisReductionAlgorithmTest
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
        public void RunTest1()
        {
            var integerDomain = new IntegerDomain();
            var decimalField = new DecimalField();
            var vectorFactory = new ArrayVectorFactory<decimal>();
            var decimalComparer = Comparer<decimal>.Default;
            var nearest = new DecimalNearestInteger();

            var scalarProd = new OrthoVectorScalarProduct<decimal>(
                        decimalComparer,
                        decimalField);

            var integerDecimalConverter = new IntegerDecimalConverter();

            var lllReductionAlg = new LLLBasisReductionAlgorithm<IVector<decimal>, decimal, int>(
                new VectorSpace<decimal>(3, vectorFactory, decimalField),
                scalarProd,
                nearest,
                Comparer<decimal>.Default);

            var vectorSet = new IVector<decimal>[3];
            vectorSet[0] = new ArrayVector<decimal>(new decimal[] { 1, 1, 1 });
            vectorSet[1] = new ArrayVector<decimal>(new decimal[] { -1, 0, 2 });
            vectorSet[2] = new ArrayVector<decimal>(new decimal[] { 3, 5, 6 });

            var reduced = lllReductionAlg.Run(vectorSet, 3M / 4);

            // O resultado esperado.
            var expected = new IVector<decimal>[3];
            expected[0] = new ArrayVector<decimal>(new decimal[] { 0, 1, 0 });
            expected[1] = new ArrayVector<decimal>(new decimal[] { 1, 0, 1 });
            expected[2] = new ArrayVector<decimal>(new decimal[] { -1, 0, 2 });

            this.AsserVectorLists(expected, reduced);
        }

        [TestMethod()]
        public void RunTest2()
        {
            var integerDomain = new IntegerDomain();
            var decimalField = new DecimalField();
            var vectorFactory = new ArrayVectorFactory<decimal>();
            var decimalComparer = Comparer<decimal>.Default;
            var nearest = new DecimalNearestInteger();

            var scalarProd = new OrthoVectorScalarProduct<decimal>(
                        decimalComparer,
                        decimalField);

            var integerDecimalConverter = new IntegerDecimalConverter();

            var dim = 4;
            var vectorSet = new IVector<decimal>[dim];
            var lllReductionAlg = new LLLBasisReductionAlgorithm<IVector<decimal>, decimal, int>(
                new VectorSpace<decimal>(dim, vectorFactory, decimalField),
                scalarProd,
                nearest,
                Comparer<decimal>.Default);

            vectorSet[0] = new ArrayVector<decimal>(new decimal[] { 1, 1, 7, 2 });
            vectorSet[1] = new ArrayVector<decimal>(new decimal[] { 9, 8, 4, 6 });
            vectorSet[2] = new ArrayVector<decimal>(new decimal[] { 1, 8, 5, 7 });
            vectorSet[3] = new ArrayVector<decimal>(new decimal[] { 2, 3, 1, 1 });

            var reduced = lllReductionAlg.Run(vectorSet, 3M / 4);

            var expected = new IVector<decimal>[dim];
            expected[0] = new ArrayVector<decimal>(new decimal[] { 2, 3, 1, 1 });
            expected[1] = new ArrayVector<decimal>(new decimal[] { 3, -1, 1, 3 });
            expected[2] = new ArrayVector<decimal>(new decimal[] { -2, 2, 6, -1 });
            expected[3] = new ArrayVector<decimal>(new decimal[] { -4, 1, -4, 3 });

            this.AsserVectorLists(expected, reduced);
        }

        private void AsserVectorLists<T>(
            IVector<T>[] expected,
            IVector<T>[] actual,
            IEqualityComparer<T> equalityComparer = null)
        {
            var innerEqualityComparer = equalityComparer == null ? EqualityComparer<T>.Default : equalityComparer;

            // É conveniente melhorar o código de teste mas para já terá de servir.
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < actual.Length; ++i)
            {
                var currentActualVector = actual[i];
                var currentReduced = actual[i];
                var foundSameVector = false;
                for (int j = 0; j < expected.Length; ++j)
                {
                    var currentExpectedVector = expected[j];
                    Assert.AreEqual(currentExpectedVector.Length, currentActualVector.Length);
                    var sameVector = true;
                    for (int k = 0; k < currentActualVector.Length; ++k)
                    {
                        if (!innerEqualityComparer.Equals(currentActualVector[k], currentExpectedVector[k]))
                        {
                            sameVector = false;
                            k = currentActualVector.Length;
                        }
                    }

                    if (sameVector)
                    {
                        foundSameVector = true;
                    }
                }

                if (!foundSameVector)
                {
                    Assert.Fail();
                }
            }
        }
    }
}
