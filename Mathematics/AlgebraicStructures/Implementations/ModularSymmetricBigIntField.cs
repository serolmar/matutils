namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Implementa aritmética modular entre números inteiros grandes cujo resultado se situa entre -p/2 e p/2.
    /// </summary>
    public class ModularSymmetricBigIntField : IModularField<BigInteger>
    {
        /// <summary>
        /// O módulo.
        /// </summary>
        private BigInteger modulus;

        /// <summary>
        /// O valor de metade do módulo.
        /// </summary>
        private BigInteger halfModulus;

        /// <summary>
        /// O algoritmo que permite obter o inverso de um número.
        /// </summary>
        private LagrangeAlgorithm<BigInteger> inverseAlgorithm;

        public ModularSymmetricBigIntField(BigInteger module)
        {
            if (module == 0 || module == 1 || module == -1)
            {
                throw new ArgumentException("Module can't neither 0, 1 nor -1.");
            }
            else
            {
                this.modulus = module;
                this.halfModulus = this.modulus >> 1;
                this.inverseAlgorithm = new LagrangeAlgorithm<BigInteger>(
                    new BigIntegerDomain());
            }
        }

        /// <summary>
        /// Obtém e atribui o valor do módulo no corpo aritmético.
        /// </summary>
        /// <remarks>
        /// Esta operação não é segura quando a classe se encontra a ser utilizada
        /// em várias fluxos paralelos de execução (threads).
        /// </remarks>
        public BigInteger Module
        {
            get
            {
                return this.modulus;
            }
            set
            {
                if (value == 0 || value == 1 || value == -1)
                {
                    throw new ArgumentException("Module can't neither 0, 1 nor -1.");
                }
                else
                {
                    this.modulus = value;
                    this.halfModulus = value >> 1;
                }
            }
        }

        public BigInteger AdditiveUnity
        {
            get
            {
                return BigInteger.Zero;
            }
        }

        public BigInteger MultiplicativeUnity
        {
            get
            {
                return BigInteger.One;
            }
        }

        /// <summary>
        /// Obtém a forma reduzida de um número relativamente ao módulo.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O número reduzido.</returns>
        public BigInteger GetReduced(BigInteger number)
        {
            var innerNumber = number % this.modulus;
            if (innerNumber < -this.halfModulus)
            {
                innerNumber = this.modulus + innerNumber;
            }
            else if (innerNumber > this.halfModulus)
            {
                innerNumber = innerNumber - this.modulus;
            }

            return innerNumber;
        }

        public BigInteger MultiplicativeInverse(BigInteger number)
        {
            var innerNumber = number % modulus;
            if (innerNumber != 0)
            {
                var bezoutResult = this.inverseAlgorithm.Run(this.modulus, innerNumber);
                if (bezoutResult.GreatestCommonDivisor == 1)
                {
                    var result = bezoutResult.SecondFactor;
                    if (result < -this.halfModulus)
                    {
                        result += this.modulus;
                    }
                    else if (result > this.halfModulus)
                    {
                        result -= this.modulus;
                    }

                    return result;
                }
                else if (bezoutResult.GreatestCommonDivisor == -1)
                {
                    var result = -bezoutResult.SecondFactor;
                    if (result < -this.halfModulus)
                    {
                        result += this.modulus;
                    }
                    else if (result > this.halfModulus)
                    {
                        result -= this.modulus;
                    }

                    return result;
                }
                else
                {
                    throw new MathematicsException(
                        string.Format("Number {0} mod {1} has no inverse.", number, this.modulus));
                }
            }
            else
            {
                throw new MathematicsException(
                    string.Format("Number {0} mod {1} has no inverse.", number, this.modulus));
            }
        }

        public BigInteger AddRepeated(BigInteger element, int times)
        {
            var bitgntTimes = (BigInteger)times;
            return this.Multiply(element, bitgntTimes);
        }

        public BigInteger AdditiveInverse(BigInteger number)
        {
            var innerNumber = -number % this.modulus;
            if (innerNumber < -this.halfModulus)
            {
                innerNumber += this.modulus;
            }
            else if (innerNumber > this.halfModulus)
            {
                innerNumber -= this.modulus;
            }

            return innerNumber;
        }

        public bool IsAdditiveUnity(BigInteger value)
        {
            return value % modulus == 0;
        }

        public bool Equals(BigInteger x, BigInteger y)
        {
            var innerX = x % this.modulus;
            var innerY = y % this.modulus;
            if (innerX < 0)
            {
                innerX = this.modulus - innerX;
            }

            if (innerY < 0)
            {
                innerY = this.modulus - innerY;
            }

            return innerX == innerY;
        }

        public int GetHashCode(BigInteger obj)
        {
            return obj.GetHashCode();
        }

        public BigInteger Add(BigInteger left, BigInteger right)
        {
            var result = (left + right) % this.modulus;
            if (result < -this.halfModulus)
            {
                result += this.modulus;
            }
            else if (result > this.halfModulus)
            {
                result -= this.modulus;
            }

            return result;
        }

        public bool IsMultiplicativeUnity(BigInteger value)
        {
            return value % modulus == 1;
        }

        public BigInteger Multiply(BigInteger left, BigInteger right)
        {
            var result = (left * right) % this.modulus;
            if (result < -this.halfModulus)
            {
                result += this.modulus;
            }
            else if (result > this.halfModulus)
            {
                result -= this.modulus;
            }

            return result;
        }
    }
}
