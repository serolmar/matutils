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

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ModularSymmetricBigIntField"/>.
        /// </summary>
        /// <remarks>
        /// A determinação da inversa multiplicativa é efectuada por intermédio de algoritmos relacionados
        /// com o algoritmo que permite determinar o máximo divisor comum. Neste caso, é necessário indicar
        /// qual será o algoritmo responsável por essa operação.
        /// </remarks>
        /// <param name="module">O módulo.</param>
        /// <exception cref="System.ArgumentException">
        /// Se o módulo for 0, 1, ou -1.
        /// </exception>
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
        /// <value>O valor do módulo.</value>
        /// <exception cref="ArgumentException">Se o valor do módulo for 0, 1 ou -1.</exception>
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

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public BigInteger AdditiveUnity
        {
            get
            {
                return BigInteger.Zero;
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
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

        /// <summary>
        /// Permite determinar a inversa multiplicativa de um número.
        /// </summary>
        /// <param name="number">O número do qual se pretende obter a inversa multiplicativa.</param>
        /// <returns>A inversa multiplicativa.</returns>
        /// <exception cref="DivideByZeroException">Se o número passao for zero.</exception>
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

        /// <summary>
        /// Permite efectuar adições repetidas de um número.
        /// </summary>
        /// <param name="element">O número a ser adicionado.</param>
        /// <param name="times">O número de vezes que o número é adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        public BigInteger AddRepeated(BigInteger element, int times)
        {
            var bitgntTimes = (BigInteger)times;
            return this.Multiply(element, bitgntTimes);
        }

        /// <summary>
        /// Permite determinar o simétrico de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O simétrico.</returns>
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

        /// <summary>
        /// Determina se um valor é uma unidade aditivia.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(BigInteger value)
        {
            return value % modulus == 0;
        }

        /// <summary>
        /// Determina quando dois números modulares são iguais.
        /// </summary>
        /// <param name="x">O primeiro número modular a ser comparado.</param>
        /// <param name="y">O segundo número modular a ser comparado.</param>
        /// <returns>
        /// Verdadeiro caso ambos os objectos sejam iguais e falso no caso contrário.
        /// </returns>
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

        /// <summary>
        /// Retorna um código confuso para a instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// Um código confuso para a instância que pode ser usado em vários algoritmos habituais.
        /// </returns>
        public int GetHashCode(BigInteger obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Calcula a adição modular de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da adição.</returns>
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

        /// <summary>
        /// Determina se o valor é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade multiplicativa e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(BigInteger value)
        {
            return value % modulus == 1;
        }

        /// <summary>
        /// Calcula o produto modular de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da multiplicação.</returns>
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
