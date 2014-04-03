namespace OdmpProblem.Test
{
    using OdmpProblem;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using Mathematics;

    [TestClass()]
    public class ComponentBoundsAlgorithmTest
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
            var costs = new List<List<int>>();
            costs.Add(new List<int>() { 12, 9, 7, 6, 4 });
            costs.Add(new List<int>() { 12, 11, 8, 7, 5 });
            costs.Add(new List<int>() { 13, 10, 9, 6, 3 });
            costs.Add(new List<int>() { 12, 8, 6, 4, 2 });

            var integerDomain = new IntegerDomain();
            var numberTdecomposition = new IntegerMinWeightTdecomposition<int>(
                Comparer<int>.Default,
                integerDomain);

            var result = numberTdecomposition.Run(13, costs);

            var upperCosts = new List<List<int>>();
            upperCosts.Add(new List<int>() { 12, 9, 7, 6, 4 });
            upperCosts.Add(new List<int>() { 12, 11, 8, 7, 5 });
            upperCosts.Add(new List<int>() { 13, 10, 9, 6, 3 });
            upperCosts.Add(new List<int>() { 12, 8, 6, 4, 2 });

            var boundsAlg = new ComponentBoundsAlgorithm<int>(
                numberTdecomposition,
                Comparer<int>.Default,
                integerDomain);
            var boundsResult = boundsAlg.Run(13, costs, upperCosts);
            Assert.AreEqual(result.Medians.Count, boundsResult.Length);
            for (int i = 0; i < boundsResult.Length; ++i)
            {
                var currentBound = boundsResult[i];
                Assert.AreEqual(currentBound.LowerBound, currentBound.UpperBound);
                var currentResultMedian = result.Medians[i];
                Assert.AreEqual(currentResultMedian, currentBound.LowerBound);
            }
        }
    }
}
