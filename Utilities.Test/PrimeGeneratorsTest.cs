// -----------------------------------------------------------------------
// <copyright file="FileTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
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
    /// Testa o crivo elementar para determinar primos.
    /// </summary>
    [TestClass]
    public class PrimeGeneratorsTest
    {
        /// <summary>
        /// Valida o incremento da roda.
        /// </summary>
        [Description("Validate the wheel level increment.")]
        [TestMethod]
        public void WheelLevel_IncrementLevelTest()
        {
            var maxLevel = 8;
            var currLevel = 0;
            var primes = new List<long>();
            ILevelWheel target = new LevelZeroWheel();
            this.ValidateLevelWheel(target, primes);
            primes.Add(target.StartPoint);
            ++currLevel;
            for (; currLevel < maxLevel; ++currLevel)
            {
                target = target.GetNextLevelWheel();
                this.ValidateLevelWheel(target, primes);
                primes.Add(target.StartPoint);
            }
        }

        /// <summary>
        /// Testa a função que permite levar uma roda para um determinado valor.
        /// </summary>
        [Description("Test the goto value function on level wheels.")]
        [TestMethod]
        public void WheelLevel_GotoValueTest()
        {
            var target = new GreatestLevelWheel(3UL);
            target.GotoValue(2L);
            Assert.AreEqual(7L, target.Current);

            target.GotoValue(121);
            Assert.AreEqual(121L, target.Current);

            target.GotoValue(25);
            Assert.AreEqual(29L, target.Current);

            target.GotoValue(183);
            Assert.AreEqual(187L, target.Current);

            target.GotoValue(-183L);
            Assert.AreEqual(-181L, target.Current);

            target.GotoValue(1L);
            Assert.AreEqual(1L, target.Current);

            target.GotoValue(45030L);
            Assert.AreEqual(45031L, target.Current);
        }

        /// <summary>
        /// Valida os valores gerados pela roda.
        /// </summary>
        /// <param name="wheel">A roda.</param>
        /// <param name="size">O tamanho a ser considerado.</param>
        /// <param name="primes">Uma lista com os números primos.</param>
        private void ValidateLevelWheel(
            ILevelWheel wheel,
            List<long> primes)
        {
            var size = wheel.Size;
            var prev = wheel.Current;
            for (var i = 0; i < primes.Count; ++i)
            {
                Assert.AreNotEqual(0, prev % primes[i], string.Format(
                    "Wheel number {0} is divisible by {1}.", 
                    prev,
                    primes[i]));
            }

            for (var i = 1UL; i < size; ++i)
            {
                wheel.MoveRight();
                var current = wheel.Current;
                Assert.AreNotEqual(prev, current, string.Format(
                    "The same values were obtained after increment wheel: {0}.",
                    current));
                for (var j = 0; j < primes.Count; ++j)
                {
                    Assert.AreNotEqual(0, current % primes[j], string.Format(
                    "Wheel number {0} is divisible by {1}.",
                    current,
                    primes[j]));
                }

                prev = current;
            }
        }
    }
}
