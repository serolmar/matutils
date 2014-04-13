namespace Mathematics.Test
{
    using Mathematics.AlgebraicStructures.Polynomial;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using Mathematics;
    using Utilities;
    
    [TestClass()]
    public class SymmetricPolynomialTest
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
        public void GetElementarySymmetricRepresentationTest()
        {
            var domain = new IntegerDomain();
            var coeffsParser = new IntegerParser<string>();
            var conversion = new ElementToElementConversion<int>();

            var dictionary = new Dictionary<int, int>();
            dictionary.Add(5, 2);
            dictionary.Add(0, 2);

            var varDictionary = new Dictionary<int, Tuple<bool, string, int>>();
            varDictionary.Add(1, Tuple.Create(true, "s[1]", 0));

            var symmetric = new SymmetricPolynomial<int>(
                new List<string>() { "x", "y", "z", "w" },
                dictionary,
                1,
                domain);

            var rep = symmetric.GetElementarySymmetricRepresentation(varDictionary, new IntegerDomain());
            var expanded = rep.GetExpanded(domain);

            var expectedPolText = "5*s4^2*s2+-5*s4*s2^3+5*s4*s3^2+5*s2^2*s3^2+1*s2^5";
            var expected = TestsHelper.ReadPolynomial(
                expectedPolText,
                domain,
                conversion,
                coeffsParser);

            Assert.AreEqual(expected, expanded);
        }
    }
}
