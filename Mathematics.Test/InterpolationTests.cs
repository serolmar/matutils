namespace Mathematics.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Utilities;

    /// <summary>
    /// Testes aos métodos de interpolação.
    /// </summary>
    [TestClass]
    public class InterpolationTests
    {
        /// <summary>
        /// Testa a interpolação na forma normal considerando apenas um ponto.
        /// </summary>
        [TestMethod]
        [Description("A test for one point normal form interpolation.")]
        public void InterpolationNormalFormConstTest()
        {
            var pointContainer = new PointContainer2D<double, double>();
            pointContainer.Add(1, 5);
            var doubleField = new DoubleField();
            var interpolator = new UnivarNormalFromInterpolator<double, double>(
                pointContainer,
                "x",
                new DoubleToIntegerConversion(),
                doubleField,
                doubleField,
                doubleField);
            
            var actual = interpolator.Interpolate(0);
            Assert.AreEqual(5, actual);

            actual = interpolator.Interpolate(2);
            Assert.AreEqual(5, actual);

            var interpolationPol = interpolator.InterpolatingPolynomial;
            var expected = new UnivariatePolynomialNormalForm<double>(5, 0, "x", doubleField);
            Assert.AreEqual(expected, interpolationPol);
        }

        /// <summary>
        /// Testa a interpolação na forma normal considerando apenas um ponto.
        /// </summary>
        [TestMethod]
        [Description("A test for several points normal form interpolation.")]
        public void InterpolationNormalFormMultiPointTest()
        {
            var pointContainer = new PointContainer2D<double, double>();
            var interpolationPoints = new double[] { 0, 1, -2, 3, -4 };
            for (int i = 0; i < interpolationPoints.Length; ++i)
            {
                pointContainer.Add(interpolationPoints[i], 0);
            }
                
            var doubleField = new DoubleField();
            var interpolator = new UnivarNormalFromInterpolator<double, double>(
                pointContainer,
                "x",
                new DoubleToIntegerConversion(),
                doubleField,
                doubleField,
                doubleField);


            // Verifica os valores da interpolação.
            for (int i = 0; i < interpolationPoints.Length; ++i)
            {
                var actual = interpolator.Interpolate(interpolationPoints[i]);
                Assert.AreEqual(0, actual);
            }

            var interpolationPol = interpolator.InterpolatingPolynomial;
            var expected = TestsHelper.ReadUnivarPolynomial(
                "0",
                doubleField,
                new DoubleParser<string>(),
                new DoubleToIntegerConversion(),
                "x",
                true);
            
            Assert.AreEqual(expected, interpolationPol);

            pointContainer.Add(5, -1);
            pointContainer.Add(-6, 1);

            for (int i = 0; i < interpolationPoints.Length; ++i)
            {
                var actual = interpolator.Interpolate(interpolationPoints[i]);
                Assert.IsTrue(Math.Abs(actual - 0) < 0.000001);
            }

            var actualValue = interpolator.Interpolate(5);
            Assert.IsTrue(Math.Abs(actualValue + 1) < 0.000001);

            actualValue = interpolator.Interpolate(-6);
            Assert.IsTrue(Math.Abs(actualValue - 1) < 0.000001);
        }
    }
}
