namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
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

            var result = multiplier.MultiplicativeUnity;
            var innerPow = pow;
            var innerVal = val;
            while (innerPow > 0)
            {
                if (innerPow % 2 == 1)
                {
                    result = multiplier.Multiply(result, innerVal);
                }

                innerPow = innerPow / 2;
                innerVal = multiplier.Multiply(innerVal, innerVal);
            }

            return result;
        }

        /// <summary>
        /// Obtém a potência de um valor.
        /// </summary>
        /// <typeparam name="T">O tipo do valor.</typeparam>
        /// <typeparam name="D">O tipo da classe que define a multiplicação.</typeparam>
        /// <param name="val">O valor.</param>
        /// <param name="pow">O expoente.</param>
        /// <param name="multiplier">A classe que define o produto sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T Power<T, D>(T val, long pow, D multiplier)
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

            var result = multiplier.MultiplicativeUnity;
            var innerPow = pow;
            var innerVal = val;
            while (innerPow > 0)
            {
                if (innerPow % 2 == 1)
                {
                    result = multiplier.Multiply(result, innerVal);
                }

                innerPow = innerPow / 2;
                innerVal = multiplier.Multiply(innerVal, innerVal);
            }

            return result;
        }

        /// <summary>
        /// Obtém a potência de um valor.
        /// </summary>
        /// <typeparam name="T">O tipo do valor.</typeparam>
        /// <typeparam name="D">O tipo da classe que define a multiplicação.</typeparam>
        /// <param name="val">O valor.</param>
        /// <param name="pow">O expoente.</param>
        /// <param name="multiplier">A classe que define o produto sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T Power<T, D>(T val, BigInteger pow, D multiplier)
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

            var result = multiplier.MultiplicativeUnity;
            var innerPow = pow;
            var innerVal = val;
            while (innerPow > 0)
            {
                if (innerPow % 2 == 1)
                {
                    result = multiplier.Multiply(result, innerVal);
                }

                innerPow = innerPow / 2;
                innerVal = multiplier.Multiply(innerVal, innerVal);
            }

            return result;
        }

        /// <summary>
        /// Obtém a potência de um valor.
        /// </summary>
        /// <typeparam name="T">O tipo do valor.</typeparam>
        /// <typeparam name="D">O tipo da classe que define a multiplicação.</typeparam>
        /// <param name="val">O valor.</param>
        /// <param name="pow">O expoente.</param>
        /// <param name="integerNumber">A classe que define as operações sobre números inteiros.</param>
        /// <returns>A potência do valor.</returns>
        public static T Power<T, Deg, D>(T val, Deg pow, D domain, IIntegerNumber<Deg> integerNumber)
            where D : IRing<T>
        {
            if (integerNumber == null)
            {
                throw new MathematicsException("Parameter integerNumber can't be null.");
            }
            else if (integerNumber.Compare(pow, integerNumber.AdditiveUnity) < 0)
            {
                throw new MathematicsException("Can't computer the powers with negative expoents.");
            }
            else
            {
                var result = domain.MultiplicativeUnity;
                var innerPow = pow;
                var innerVal = val;
                var two = integerNumber.Successor(integerNumber.MultiplicativeUnity);
                while (integerNumber.Compare(innerPow, integerNumber.AdditiveUnity) > 0)
                {
                    var remQuo = integerNumber.GetQuotientAndRemainder(innerPow, two);
                    if (integerNumber.IsMultiplicativeUnity(remQuo.Remainder))
                    {
                        result = domain.Multiply(result, innerVal);
                    }

                    innerPow = remQuo.Quotient;
                    innerVal = domain.Multiply(innerVal, innerVal);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o valor da aplicação sucessiva da operação soma ao mesmo valor tendo em conta que esta é associativa.
        /// </summary>
        /// <typeparam name="T">O tipo do valor.</typeparam>
        /// <typeparam name="D">A classe que define a soma.</typeparam>
        /// <param name="val">O valor.</param>
        /// <param name="pow">O expoente.</param>
        /// <param name="monoid">A classe que define a soma sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T AddPower<T, D>(T val, int pow, D monoid)
            where D : IMonoid<T>
        {
            if (monoid == null)
            {
                throw new MathematicsException("Parameter multiplier can't be null.");
            }

            if (pow < 0)
            {
                throw new MathematicsException("Can't computer the powers with negative expoents.");
            }

            var result = monoid.AdditiveUnity;
            var innerPow = pow;
            var innerVal = val;
            while (innerPow > 0)
            {
                if (innerPow % 2 == 1)
                {
                    result = monoid.Add(result, innerVal);
                }

                innerPow = innerPow / 2;
                innerVal = monoid.Add(innerVal, innerVal);
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
        /// <param name="monoid">A classe que define a soma sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T AddPower<T, D>(T val, long pow, D monoid)
            where D : IMonoid<T>
        {
            if (monoid == null)
            {
                throw new MathematicsException("Parameter multiplier can't be null.");
            }

            if (pow < 0)
            {
                throw new MathematicsException("Can't computer the powers with negative expoents.");
            }

            var result = monoid.AdditiveUnity;
            var innerPow = pow;
            var innerVal = val;
            while (innerPow > 0)
            {
                if (innerPow % 2 == 1)
                {
                    result = monoid.Add(result, innerVal);
                }

                innerPow = innerPow / 2;
                innerVal = monoid.Add(innerVal, innerVal);
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
        /// <param name="monoid">A classe que define a soma sobre o valor.</param>
        /// <returns>A potência do valor.</returns>
        public static T AddPower<T, D>(T val, BigInteger pow, D monoid)
            where D : IMonoid<T>
        {
            if (monoid == null)
            {
                throw new MathematicsException("Parameter multiplier can't be null.");
            }

            if (pow < 0)
            {
                throw new MathematicsException("Can't computer the powers with negative expoents.");
            }

            var result = monoid.AdditiveUnity;
            var innerPow = pow;
            var innerVal = val;
            while (innerPow > 0)
            {
                if (innerPow % 2 == 1)
                {
                    result = monoid.Add(result, innerVal);
                }

                innerPow = innerPow / 2;
                innerVal = monoid.Add(innerVal, innerVal);
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
            while (!euclideanDomain.IsAdditiveUnity(b))
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
        public static int GetInversion(int pow)
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

        /// <summary>
        /// Permite obter o número de bits ligados num vector de bits.
        /// </summary>
        /// <remarks>
        /// O algoritmo é descrito em http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel.
        /// Adaptado de http://stackoverflow.com/questions/5063178/counting-bits-set-in-a-net-bitarray-class.
        /// </remarks>
        /// <param name="bitArray">O vector de bits.</param>
        /// <returns>O número de bits ligados.</returns>
        public static int CountSettedBits(BitArray bitArray)
        {
            if (bitArray == null)
            {
                throw new ArgumentNullException("bitArray");
            }
            else
            {
                var integerValues = new int[(bitArray.Count >> 5) + 1];
                bitArray.CopyTo(integerValues, 0);
                var count = 0;
                integerValues[integerValues.Length - 1] &= ~(-1 << (bitArray.Count % 32));
                for (int i = 0; i < integerValues.Length; i++)
                {
                    var current = integerValues[i];
                    unchecked
                    {
                        current = current - ((current >> 1) & 0x55555555);
                        current = (current & 0x33333333) + ((current >> 2) & 0x33333333);
                        current = ((current + (current >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
                    }

                    count += current;
                }

                return count;
            }
        }

        /// <summary>
        /// Conta o número de bits ligados associados ao inteiro.
        /// </summary>
        /// <param name="bitSet">O inteiro.</param>
        /// <returns>O número de bits ligados.</returns>
        public static int PopCount(uint bitSet)
        {
            var current = bitSet;
            unchecked
            {
                current = current - ((current >> 1) & 0x55555555);
                current = (current & 0x33333333) + ((current >> 2) & 0x33333333);
                current = ((current + (current >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
            }

            return (int)current;
        }
    }
}
