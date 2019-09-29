// -----------------------------------------------------------------------
// <copyright file="SamplesGeneratorsTest.cs" company="Sérgio O. Marques">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Utilities.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Testa a classe que permite gerar amostras sonoras.
    /// </summary>
    [TestClass]
    public class SamplesGeneratorsTest
    {
        /// <summary>
        /// Testa a função que permite gerar impulsos no intervalo [0,1).
        /// </summary>
        [Description("Tests the pulse generation function.")]
        [TestMethod]
        public void SamplesGeneratorsFunctions_GetRestrictedPulseTest()
        {
            var len = 10;
            var delta = 1.0 / (5 * len);
            var values = new double[len];
            for (var i = 0; i < len; ++i)
            {
                values[i] = i * 1.0 / len;
            }

            var pusleFunc = SamplesGeneratorFunctions.GetRestrictedPulse(
                values,
                delta);

            var increase = delta / 10;
            var x = 0.0;
            while (x < 1.0)
            {
                var actual = pusleFunc.Invoke(x);
                var expected = 0.0;
                for (var i = 0; i < len; ++i)
                {
                    var val = values[i];
                    if (Math.Abs(x - val) <= delta)
                    {
                        expected = 1.0;
                        i = len;
                    }
                }

                Assert.AreEqual(expected, actual);
                x += increase;
            }
        }

        /// <summary>
        /// Testa a função que permite determinar aproximações do integral.
        /// </summary>
        [Description("Tests the integrator function.")]
        [TestMethod]
        public void SamplesProcessorFunctions_GetIntegratorTest()
        {
            // Função constante
            var c = 4;
            var delta = 0.0001;
            var integratorFunc = SamplesProcessorFunctions.GetIntegrator(
                x => c,
                delta);
            var t = 0.0;
            while (t < 10)
            {
                var actual = integratorFunc.Invoke(t);
                var expected = c * (t + delta);
                Assert.AreEqual(expected, actual);
                t += delta;
            }

            integratorFunc = SamplesProcessorFunctions.GetIntegrator(
                x => c * x,
                delta);
            t = 0.0;
            while (t < 10)
            {
                var actual = integratorFunc.Invoke(t);
                var expected = c * (t + delta) * (t + 2 * delta) / 2;
                Assert.AreEqual(expected, actual, 0.000000001);
                t += delta;
            }
        }

        /// <summary>
        /// Efectua um teste complexo sobre o solucionador de equações
        /// às diferenças.
        /// </summary>
        [Description("Tests the difference equation solver.")]
        [TestMethod]
        public void SamplesProcessorFunctions_GetDifferenceEquationsTest()
        {
            // Testa a função exponencial
            var func = SamplesProcessorFunctions.GetDifferenceEquationSolver(
                x => 0,
                0.0,
                new double[] { },
                new double[] { 2.0 },
                new double[] { },
                new double[] { 1.0 });
            var expected = 2.0;
            var actual = func.Invoke(1.0);
            Assert.AreEqual(expected, actual);
            while (expected < 10)
            {
                expected *= 2.0;
                actual = func.Invoke(expected); // A função de entrada é constante
                Assert.AreEqual(expected, actual);
            }

            // y[n] = 2 * x[n] + 3 * x[n-1] - 2 * y[n-1]
            // x[0] = 0, y[0] = 0
            func = SamplesProcessorFunctions.GetDifferenceEquationSolver(
                x => x,
                2.0,
                new double[] { 3.0 },
                new double[] { 2.0 },
                new double[] { 0.0 },
                new double[] { 0.0 });

            var expectedArray = new double[] { 3.0, 1.0, 13.0, -9.0, 55.0 };

            var t = 1.0;
            for (var i = 0; i < expectedArray.LongLength; ++i)
            {
                expected = expectedArray[i];
                actual = func.Invoke(t);
                t += 1;
            }
        }

        /// <summary>
        /// Testa a função de retardo.
        /// </summary>
        [Description("Tests the delay function.")]
        [TestMethod]
        public void SamplesProcessorFunctions_GetDelayTest()
        {
            var f = SamplesGeneratorFunctions.GetSineFunc();
            var startValue = 2.0;
            var delay = SamplesProcessorFunctions.GetDelay(
                f,
                startValue);

            // Testa o retardo simples.
            var t = 0.0;
            var expected = startValue;
            while (t < 100)
            {
                var actual = delay.Invoke(t);
                Assert.AreEqual(expected, actual);

                expected = f.Invoke(t);
                t += 1.0;
            }

            // Testa um retardo composto
            var startValue1 = 2.0;
            var startValue2 = 1.0;
            var startValue3 = 3.0;
            delay = SamplesProcessorFunctions.GetDelay(
                f,
                startValue);
            var delay1 = SamplesProcessorFunctions.GetDelay(
                delay,
                1.0);
            var delay2 = SamplesProcessorFunctions.GetDelay(
                delay1,
                3.0);

            t = 0.0;
            while (t < 100)
            {
                var actual = delay2.Invoke(t);
                Assert.AreEqual(startValue3, actual);

                startValue3 = startValue2;
                startValue2 = startValue1;
                startValue1 = f.Invoke(t);
                t += 1.0;
            }

            // Testa um retardo complexo
            Func<double, double> func = x => x;
            delay1 = SamplesProcessorFunctions.GetDelay(
                func,
                1.0);
            delay2 = SamplesProcessorFunctions.GetDelay(
                func,
                1.0);
            Func<double, double> delay3 = SamplesProcessorFunctions.GetDelay(
                x => delay2.Invoke(x),
                2.0);
            Func<double, double> xMain = x => x + delay1.Invoke(x) + 0.5 * delay3.Invoke(x);

            var expectedItems = new double[]{
                2, 1.5, 3, 5.5, 8, 10.5, 13, 15.5,
                18, 20.5, 23, 25.5, 28, 30.5, 33, 35.5
            };

            t = 0.0;
            for (var i = 0; i < expectedItems.LongLength; ++i)
            {
                var innerExpected = expectedItems[i];
                var innerActual = xMain.Invoke(t);
                Assert.AreEqual(innerExpected, innerActual);
                t += 1.0;
            }
        }

        /// <summary>
        /// Testa a função de rectroacção em cenários complexos.
        /// </summary>
        [Description("Tests the feedback composition function.")]
        [TestMethod]
        public void SamplesProcessorFunctions_GetFeedbackCompositeTest()
        {
            // y[n] = x[n] + x[n-1] + 0.5 * x[n-2] - y[n-1]
            Func<double, double> func = x => x;
            Func<double, double> delay1 = SamplesProcessorFunctions.GetDelay(
                func,
                1.0);
            Func<double, double> delay2 = SamplesProcessorFunctions.GetDelay(
                func,
                1.0);
            Func<double, double> delay3 = SamplesProcessorFunctions.GetDelay(
                x => delay2.Invoke(x),
                2.0);
            Func<double, double> xMain = x => x + delay1.Invoke(x) + 0.5 * delay3.Invoke(x);

            var comp = SamplesProcessorFunctions.GetFeedbackComposite(
                (x, ix) => xMain.Invoke(x) - ix,
                1.0);

            // Comparação com o solucionador de equações às diferenças
            var diffSolv = SamplesProcessorFunctions.GetDifferenceEquationSolver(
                func,
                1.0,
                new double[] { 1.0, 0.5 },
                new double[] { -1.0 },
                new double[] { 1.0, 2.0 },
                new double[] { 1.0 });

            var t = 0.0;
            while (t < 100)
            {
                var expected = diffSolv.Invoke(t);
                var actual = comp.Invoke(t);
                Assert.AreEqual(expected, actual);

                t += 1.0;
            }
        }
    }
}
