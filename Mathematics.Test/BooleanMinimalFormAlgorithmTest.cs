namespace Mathematics.Test
{
    using Mathematics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    ///This is a test class for BooleanMinimalFormAlgorithmTest and is intended
    ///to contain all BooleanMinimalFormAlgorithmTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BooleanMinimalFormAlgorithmTest
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
        public void RunTest_Case1()
        {
            var target = new BooleanMinimalFormAlgorithm();
            var data = new BooleanMinimalFormInOut();
            var combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[1] = EBooleanMinimalFormOutStatus.ON;
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[4] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            combination = new LogicCombinationBitArray(5, EBooleanMinimalFormOutStatus.OFF);
            combination[0] = EBooleanMinimalFormOutStatus.ON;
            combination[2] = EBooleanMinimalFormOutStatus.ON;
            combination[3] = EBooleanMinimalFormOutStatus.ON;
            combination[4] = EBooleanMinimalFormOutStatus.ON;
            data.Add(combination, EBooleanMinimalFormOutStatus.ON);

            var actual = target.Run(data);
            foreach (var comb in actual)
            {
                var valid = false;
                foreach (var innerComb in data)
                {
                    Assert.AreEqual(comb.LogicInput.Length, innerComb.LogicInput.Length);
                    var innerValid = true;
                    for (int i = 0; i < comb.LogicInput.Length; ++i)
                    {
                        if (comb.LogicInput[i] != EBooleanMinimalFormOutStatus.DONT_CARE &&
                            innerComb.LogicInput[i] != EBooleanMinimalFormOutStatus.DONT_CARE &&
                            comb.LogicInput[i] != innerComb.LogicInput[i])
                        {
                            innerValid = false;
                            i = comb.LogicInput.Length;
                        }
                    }

                    if (innerValid)
                    {
                        valid = true;
                        break;
                    }
                }

                Assert.IsTrue(valid, "A combinação parece não ser válida.");
            }
        }
    }
}
