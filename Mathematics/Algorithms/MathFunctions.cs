namespace Mathematics.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class MathFunctions
    {
        /// <summary>
        /// Obtém a potência de um valor.
        /// </summary>
        /// <typeparam name="T">O tipo do valor.</typeparam>
        /// <param name="val">O valor.</param>
        /// <param name="pow">O expoente.</param>
        /// <param name="multiplier">A classe que define o produto sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T Power<T>(T val, int pow, IMultipliable<T> multiplier)
        {
            if (multiplier == null)
            {
                throw new MathematicsException("Parameter multiplier can't be null.");
            }

            if (pow < 0)
            {
                throw new MathematicsException("Can't computer the powers with negative expoents.");
            }

            if (pow == 0)
            {
                return multiplier.MultiplicativeUnity;
            }

            var result = val;
            var rem = pow % 2;
            pow = pow / 2;
            while (pow > 0)
            {
                result = multiplier.Multiply(result, result);
                if (rem == 1)
                {
                    result = multiplier.Multiply(val, result);
                }

                rem = pow % 2;
                pow = pow / 2;
            }

            return result;
        }

        /// <summary>
        /// Calcula o máximo divisor comum entre dois números.
        /// </summary>
        /// <typeparam name="T">O tipo dos valores.</typeparam>
        /// <typeparam name="D">O tipo do domínio euclideano.</typeparam>
        /// <param name="firstValue">O primeiro valor para calcular o máximo divisor comum.</param>
        /// <param name="secondValue">O segunda valor para calcular o máximo divisor comum.</param>
        /// <param name="euclideanDomain">O domínio euclideano.</param>
        /// <returns>O máximo divisor comum entre os dois valores.</returns>
        public static T GreatCommonDivisor<T, D>(T firstValue, T secondValue, D euclideanDomain)
            where D : IEuclidenDomain<T>
        {
            if (firstValue == null)
            {
                throw new ArgumentNullException("firstValue");
            }

            if (secondValue == null)
            {
                throw new ArgumentNullException("secondValue");
            }

            if (euclideanDomain == null)
            {
                throw new ArgumentNullException("euclideanDomain");
            }

            if (euclideanDomain.IsAdditiveUnity(firstValue))
            {
                throw new ArgumentException("First value in great common divisor can't be an additive unity.");
            }

            if (euclideanDomain.IsAdditiveUnity(secondValue))
            {
                throw new ArgumentException("Second value in great common divisor can't be an additive unity.");
            }

            var a = firstValue;
            var b = secondValue;
            while (!euclideanDomain.Equals(b, euclideanDomain.AdditiveUnity))
            {
                T temp = b;
                b = euclideanDomain.Rem(a, b);
                a = temp;
            }

            return a;
        }
    }
}
