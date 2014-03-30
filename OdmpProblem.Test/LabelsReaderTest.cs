namespace OdmpProblem.Test
{
    using OdmpProblem;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Text;
    using System.Collections.Generic;

    [TestClass()]
    public class LabelsReaderTest
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
        public void ReadLabelsTest_EmptyLines()
        {
            var target = new LabelsReader();
            var emptyLineStringBuilder = new StringBuilder();
            emptyLineStringBuilder.AppendLine();
            emptyLineStringBuilder.AppendLine();
            emptyLineStringBuilder.AppendLine();
            emptyLineStringBuilder.AppendLine();
            var bytes = Encoding.ASCII.GetBytes(emptyLineStringBuilder.ToString());
            var stringReader = new MemoryStream(bytes);
            var readed = target.ReadLabels(stringReader, Encoding.ASCII);
            Assert.AreEqual(0, readed.Count);
        }
    }
}
