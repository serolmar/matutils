namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Numerics;

    public class ModularBigIntegerField : IModularField<BigInteger>
    {
        /// <summary>
        /// O módulo.
        /// </summary>
        private BigInteger module;

        /// <summary>
        /// O algoritmo que permite obter o inverso de um número.
        /// </summary>
        private LagrangeAlgorithm<BigInteger> inverseAlgorithm;

        public ModularBigIntegerField(BigInteger module)
        {
            if (module == 0 || module == 1 || module == -1)
            {
                throw new ArgumentException("Module can't neither 0, 1 nor -1.");
            }
            else
            {
                this.module = module;
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
                return this.module;
            }
            set
            {
                if (value == 0 || value == 1 || value == -1)
                {
                    throw new ArgumentException("Module can't neither 0, 1 nor -1.");
                }
                else
                {
                    this.module = value;
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
            var innerNumber = number % this.module;
            if (innerNumber < 0)
            {
                innerNumber = this.module - innerNumber;
            }

            return innerNumber;
        }

        public BigInteger MultiplicativeInverse(BigInteger number)
        {
            var innerNumber = number % module;
            if (innerNumber != 0)
            {
                if (innerNumber < 0)
                {
                    innerNumber = this.module - innerNumber;
                }

                var bezoutResult = this.inverseAlgorithm.Run(this.module, innerNumber);
                if (bezoutResult.GreatestCommonDivisor == 1)
                {
                    var result = bezoutResult.SecondFactor;
                    if (result < 0)
                    {
                        result += this.module;
                    }

                    return result;
                }
                else
                {
                    throw new MathematicsException(
                        string.Format("Number {0} mod {1} has no inverse.", number, this.module));
                }
            }
            else
            {
                throw new MathematicsException(
                    string.Format("Number {0} mod {1} has no inverse.", number, this.module));
            }
        }

        public BigInteger AddRepeated(BigInteger element, int times)
        {
            var bitgntTimes = (BigInteger)times;
            return this.Multiply(element, bitgntTimes);
        }

        public BigInteger AdditiveInverse(BigInteger number)
        {
            var innerNumber = number % this.module;
            if (innerNumber < 0)
            {
                innerNumber = this.module - innerNumber;
            }

            return this.module - innerNumber;
        }

        public bool IsAdditiveUnity(BigInteger value)
        {
            return value % module == 0;
        }

        public bool Equals(BigInteger x, BigInteger y)
        {
            var innerX = x % this.module;
            var innerY = y % this.module;
            if (innerX < 0)
            {
                innerX = this.module - innerX;
            }

            if (innerY < 0)
            {
                innerY = this.module - innerY;
            }

            return innerX == innerY;
        }

        public int GetHashCode(BigInteger obj)
        {
            return obj.GetHashCode();
        }

        public BigInteger Add(BigInteger left, BigInteger right)
        {
            return (left + right) % this.module;
        }

        public bool IsMultiplicativeUnity(BigInteger value)
        {
            return value % module == 1;
        }

        public BigInteger Multiply(BigInteger left, BigInteger right)
        {
            return (left * right) % this.module;
        }
    }
}
