using Mathematics.AlgebraicStructures.Polynomial;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UtilitiesTest
{


    /// <summary>
    ///This is a test class for DegreeEqualityComparerTest and is intended
    ///to contain all DegreeEqualityComparerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DegreeEqualityComparerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_SameObject()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = new List<int>() { 1 };
            IEnumerable<int> y = x;
            var actual = target.Equals(x, y);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_BothNulls()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = null;
            IEnumerable<int> y = null;
            var actual = target.Equals(x, y);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_TheFirstNull()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = null;
            IEnumerable<int> y = new List<int>();
            var actual = target.Equals(x, y);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_TheSecondNull()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = new List<int>();
            IEnumerable<int> y = null;
            var actual = target.Equals(x, y);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_EqualHavingFirstZeroesAtEnd()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = new List<int>() { 1, 3, 2, 0, 0, 0 };
            IEnumerable<int> y = new List<int>() { 1, 3, 2, 0 };
            var actual = target.Equals(x, y);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_EqualHavingSecondZeroesAtEnd()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = new List<int>() { 1, 3, 2 };
            IEnumerable<int> y = new List<int>() { 1, 3, 2, 0, 0 };
            var actual = target.Equals(x, y);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_NotEqual()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = new List<int>() { 1, 3, 2, 0, 1, 0 };
            IEnumerable<int> y = new List<int>() { 1, 3, 2, 0 };
            var actual = target.Equals(x, y);
            Assert.IsFalse(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_ZeroEmptyEqualityFirst()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = new List<int>();
            IEnumerable<int> y = new List<int>() { 0, 0, 0, 0, 0 };
            var actual = target.Equals(x, y);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void EqualsTest_ZeroEmptyEqualitySecond()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> x = new List<int>() { 0, 0, 0, 0, 0 };
            IEnumerable<int> y = new List<int>();
            var actual = target.Equals(x, y);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest_NullObject()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> obj = null;
            var expected = 0;
            var actual = target.GetHashCode(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest_EmptyList()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> obj = new List<int>();
            var expected = 0x2D2816FE;
            var actual = target.GetHashCode(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest_ZeroesList()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> obj = new List<int>() { 0, 0, 0, 0, 0 };
            var expected = 0x2D2816FE;
            var actual = target.GetHashCode(obj);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest_EndZeroesSameHash()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> obj1 = new List<int>(){1,3,2,0,0,0};
            IEnumerable<int> obj2 = new List<int>() { 1, 3, 2 };
            var expected = target.GetHashCode(obj1);
            var actual = target.GetHashCode(obj2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest_SameListsSameHash()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> obj1 = new List<int>() { 1, 3, 2};
            IEnumerable<int> obj2 = new List<int>() { 1, 3, 2 };
            var expected = target.GetHashCode(obj1);
            var actual = target.GetHashCode(obj2);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest_MiddleZeroesDifferentHash()
        {
            DegreeEqualityComparer target = new DegreeEqualityComparer();
            IEnumerable<int> obj1 = new List<int>() { 1, 3, 0, 0, 0, 2 };
            IEnumerable<int> obj2 = new List<int>() { 1, 3, 2 };
            var expected = target.GetHashCode(obj1);
            var actual = target.GetHashCode(obj2);
            Assert.AreNotEqual(expected, actual);
        }
    }
}
