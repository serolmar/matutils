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
        /// <typeparam name="D">O tipo da classe que define a multiplicação.</typeparam>
        /// <param name="val">O valor.</param>
        /// <param name="pow">O expoente.</param>
        /// <param name="multiplier">A classe que define o produto sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T Power<T, D>(T val, int pow, D multiplier)
            where D : IMultipliable<T>
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

            var innerPow = GetInversion(pow);

            var result = val;
            var rem = innerPow % 2;
            innerPow = innerPow / 2;
            while (pow > 0)
            {
                result = multiplier.Multiply(result, result);
                if (rem == 1)
                {
                    result = multiplier.Multiply(val, result);
                }

                rem = innerPow % 2;
                innerPow = innerPow / 2;
            }

            return result;
        }

        /// <summary>
        /// Obtém o valor da aplicação sucessiva da operação soma ao mesmo valor tendo em conta que esta é associativa.
        /// </summary>
        /// <typeparam name="T">O tipo do valor.</typeparam>
        /// <typeparam name="D">A classe que define a soma.</typeparam>
        /// <param name="val">O valor.</param>
        /// <param name="pow">O expoente.</param>
        /// <param name="modnoid">A classe que define a soma sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T AddPower<T, D>(T val, int pow, D modnoid)
            where D : IMonoid<T>
        {
            if (modnoid == null)
            {
                throw new MathematicsException("Parameter multiplier can't be null.");
            }

            if (pow < 0)
            {
                throw new MathematicsException("Can't computer the powers with negative expoents.");
            }

            if (pow == 0)
            {
                return modnoid.AdditiveUnity;
            }

            var innerPow = GetInversion(pow);

            var result = val;
            var rem = innerPow % 2;
            innerPow = innerPow / 2;
            while (innerPow > 0)
            {
                result = modnoid.Add(result, result);
                if (rem == 1)
                {
                    result = modnoid.Add(val, result);
                }

                rem = innerPow % 2;
                innerPow = innerPow / 2;
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

        /// <summary>
        /// Gets the power inversion.
        /// </summary>
        /// <param name="pow">The power to be inverted.</param>
        /// <returns>The inverted power.</returns>
        private static int GetInversion(int pow)
        {
            var result = 1;
            var rem = pow % 2;
            pow = pow / 2;
            while (pow > 0)
            {
                result = result * 2;
                if (rem == 1)
                {
                    result = result + 1;
                }

                rem = pow % 2;
                pow = pow / 2;
            }

            return result;
        }
    }
}
