using Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UtilitiesTest
{
    
    
    /// <summary>
    ///This is a test class for MultiDimensionalRangeTest and is intended
    ///to contain all MultiDimensionalRangeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultiDimensionalRangeTest
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
        ///A test for ComputeLinearPosition
        ///</summary>
        public void ComputeLinearPositionTestHelper<T>()
        {
            MultiDimensionalRange_Accessor<T> target = new MultiDimensionalRange_Accessor<T>(new int[] { 3, 4, 2 });
            int[] coords = new int[] { 2, 3, 1 };
            int expected = 23;
            int actual;
            actual = target.ComputeLinearPosition(coords);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("Mathematics.dll")]
        public void ComputeLinearPositionTest()
        {
            ComputeLinearPositionTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for ComputeLinearPosition
        ///</summary>
        public void ComputeCoordsFromPositionTestHelper<T>()
        {
            MultiDimensionalRange_Accessor<T> target = new MultiDimensionalRange_Accessor<T>(new int[] { 3, 4, 2 });
            int pos = 12;
            int[] expected = new int[] { 0, 0, 1 };
            int[] actual;
            actual = target.ComputeCoordsFromPosition(pos);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("Mathematics.dll")]
        public void ComputeCoordsFromPositionTest()
        {
            ComputeCoordsFromPositionTestHelper<GenericParameterHelper>();
        }
    }
}
