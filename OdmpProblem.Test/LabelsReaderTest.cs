using OdmpProblem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

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

        [TestMethod()]
        public void ReadLabelsTest_ValidLinesAtStart()
        {
            var target = new LabelsReader();
            var validLinesStringBuilder = new StringBuilder();
            validLinesStringBuilder.AppendLine("1 1;3 3; 4 8;0.5 ;0.6;0.7 ");
            validLinesStringBuilder.AppendLine(" 5 26 ;10 345; 4 13;0.8 ; 0.9 ;1.0 ");
            validLinesStringBuilder.AppendLine();

            var bytes = Encoding.ASCII.GetBytes(validLinesStringBuilder.ToString());
            var stringReader = new MemoryStream(bytes);
            var readed = target.ReadLabels(stringReader, Encoding.ASCII);

            Assert.AreEqual(2, readed.Count);

            var label = readed[0];
            Assert.AreEqual(1, label.MtdBits.Count);
            Assert.AreEqual(3, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.5, label.Price);
            Assert.AreEqual(0.6, label.Rate);
            Assert.AreEqual(0.7, label.CarsNumber);

            label = readed[1];
            Assert.AreEqual(5, label.MtdBits.Count);
            Assert.AreEqual(10, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.8, label.Price);
            Assert.AreEqual(0.9, label.Rate);
            Assert.AreEqual(1, label.CarsNumber);
        }

        [TestMethod()]
        public void ReadLabelsTest_ValidLinesAtMiddle()
        {
            var target = new LabelsReader();
            var validLinesStringBuilder = new StringBuilder();
            validLinesStringBuilder.AppendLine();
            validLinesStringBuilder.AppendLine("1 1;3 3; 4 8;0.5 ;0.6;0.7 ");
            validLinesStringBuilder.AppendLine(" 5 26 ;10 345; 4 13;0.8 ; 0.9 ;1.0 ");
            validLinesStringBuilder.AppendLine();

            var bytes = Encoding.ASCII.GetBytes(validLinesStringBuilder.ToString());
            var stringReader = new MemoryStream(bytes);
            var readed = target.ReadLabels(stringReader, Encoding.ASCII);

            Assert.AreEqual(2, readed.Count);

            var label = readed[0];
            Assert.AreEqual(1, label.MtdBits.Count);
            Assert.AreEqual(3, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.5, label.Price);
            Assert.AreEqual(0.6, label.Rate);
            Assert.AreEqual(0.7, label.CarsNumber);

            label = readed[1];
            Assert.AreEqual(5, label.MtdBits.Count);
            Assert.AreEqual(10, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.8, label.Price);
            Assert.AreEqual(0.9, label.Rate);
            Assert.AreEqual(1, label.CarsNumber);
        }

        [TestMethod()]
        public void ReadLabelsTest_ValidLinesAtEnd()
        {
            var target = new LabelsReader();
            var validLinesStringBuilder = new StringBuilder();
            validLinesStringBuilder.AppendLine();
            validLinesStringBuilder.AppendLine("1 1;3 3; 4 8;0.5 ;0.6;0.7 ");
            validLinesStringBuilder.AppendLine();
            validLinesStringBuilder.AppendLine(" 5 26 ;10 345; 4 13;0.8 ; 0.9 ;1.0 ");

            var bytes = Encoding.ASCII.GetBytes(validLinesStringBuilder.ToString());
            var stringReader = new MemoryStream(bytes);
            var readed = target.ReadLabels(stringReader, Encoding.ASCII);

            Assert.AreEqual(2, readed.Count);

            var label = readed[0];
            Assert.AreEqual(1, label.MtdBits.Count);
            Assert.AreEqual(3, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.5, label.Price);
            Assert.AreEqual(0.6, label.Rate);
            Assert.AreEqual(0.7, label.CarsNumber);

            label = readed[1];
            Assert.AreEqual(5, label.MtdBits.Count);
            Assert.AreEqual(10, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.8, label.Price);
            Assert.AreEqual(0.9, label.Rate);
            Assert.AreEqual(1, label.CarsNumber);
        }

        [TestMethod()]
        public void ReadLabelsTest_ValidLinesAndNoEmptyLines()
        {
            var target = new LabelsReader();
            var validLinesStringBuilder = new StringBuilder();
            validLinesStringBuilder.AppendLine("1 1;3 3; 4 8  ;0.5 ;0.6;0.7 ");
            validLinesStringBuilder.AppendLine(" 5 26 ;10    345; 4 13;0.8 ; 0.9 ;1.0 ");

            var bytes = Encoding.ASCII.GetBytes(validLinesStringBuilder.ToString());
            var stringReader = new MemoryStream(bytes);
            var readed = target.ReadLabels(stringReader, Encoding.ASCII);

            Assert.AreEqual(2, readed.Count);

            var label = readed[0];
            Assert.AreEqual(1, label.MtdBits.Count);
            Assert.AreEqual(3, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.5, label.Price);
            Assert.AreEqual(0.6, label.Rate);
            Assert.AreEqual(0.7, label.CarsNumber);

            label = readed[1];
            Assert.AreEqual(5, label.MtdBits.Count);
            Assert.AreEqual(10, label.MBits.Count);
            Assert.AreEqual(4, label.TmBits.Count);
            Assert.AreEqual(0.8, label.Price);
            Assert.AreEqual(0.9, label.Rate);
            Assert.AreEqual(1, label.CarsNumber);
        }

        [TestMethod()]
        [ExpectedException(typeof(OdmpProblemException))]
        public void ReadLabelsTest_InvalidLinesNoCarNumber()
        {
            var target = new LabelsReader();
            var validLinesStringBuilder = new StringBuilder();
            validLinesStringBuilder.AppendLine("1 1;3 3; 4 8;0.5 ;0.6;");
            validLinesStringBuilder.AppendLine(" 5 26 ;10 345; 4 13;0.8 ; 0.9 ;1.0 ");
            validLinesStringBuilder.AppendLine();

            var bytes = Encoding.ASCII.GetBytes(validLinesStringBuilder.ToString());
            var stringReader = new MemoryStream(bytes);
            var readed = target.ReadLabels(stringReader, Encoding.ASCII);
        }
    }
}
