namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa a aritmética modular tendo em conta que os valores variam entre -p/2 e p/2.
    /// </summary>
    public class ModularSymmetricIntField : IModularField<int>
    {
        /// <summary>
        /// O módulo.
        /// </summary>
        private int modulus;

        /// <summary>
        /// Metade do módulo.
        /// </summary>
        private int halfModulus;

        /// <summary>
        /// Permite efectuar um passo intermédio para a obtenção da inversa.
        /// </summary>
        private LagrangeAlgorithm<int> inverseAlgorithm;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ModularSymmetricIntField"/>.
        /// </summary>
        /// <remarks>
        /// A determinação da inversa multiplicativa é efectuada por intermédio de algoritmos relacionados
        /// com o algoritmo que permite determinar o máximo divisor comum. Neste caso, é necessário indicar
        /// qual será o algoritmo responsável por essa operação.
        /// </remarks>
        /// <param name="modulus">O módulo.</param>
        /// <exception cref="ArgumentException">
        /// Se o módulo for 0, 1 ou -1.
        /// </exception>
        public ModularSymmetricIntField(int modulus)
        {
            if (modulus == 0 || modulus == 1 || modulus == -1)
            {
                throw new ArgumentException("Module can't neither 0, 1 nor -1.");
            }
            else
            {
                this.modulus = modulus;
                this.halfModulus = modulus >> 1;
                this.inverseAlgorithm = new LagrangeAlgorithm<int>(
                    new IntegerDomain());
            }
        }

        /// <summary>
        /// Obtém e atribui o valor do módulo no corpo aritmético.
        /// </summary>
        /// <remarks>
        /// Esta operação não é segura quando a classe se encontra a ser utilizada
        /// em várias fluxos paralelos de execução (threads).
        /// </remarks>
        /// <value>
        /// O valor do módulo no corpo aritmético.
        /// </value>
        public int Module
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
        /// <value>A unidade aditiva.</value>
        public int AdditiveUnity
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public int MultiplicativeUnity
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Obtém a forma reduzida de um número relativamente ao módulo.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O número reduzido.</returns>
        public int GetReduced(int number)
        {
            var innerNumber = number % this.modulus;
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
        /// Permite determinar a inversa multiplicativa de um número.
        /// </summary>
        /// <param name="number">O número do qual se pretende obter a inversa multiplicativa.</param>
        /// <returns>A inversa multiplicativa.</returns>
        /// <exception cref="DivideByZeroException">Se o número passao for zero.</exception>
        public int MultiplicativeInverse(int number)
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
        /// Permite efectuar adições modulares repetidas de um número.
        /// </summary>
        /// <param name="element">O número a ser adicionado.</param>
        /// <param name="times">O número de vezes que o número é adicionado.</param>
        /// <returns>O resultado da adição modular.</returns>
        public int AddRepeated(int element, int times)
        {
            if (times == 0)
            {
                return 0;
            }
            else
            {
                return this.Multiply(element, times);
            }
        }

        /// <summary>
        /// Permite determinar o simétrico de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O simétrico.</returns>
        public int AdditiveInverse(int number)
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
        public bool IsAdditiveUnity(int value)
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
        public bool Equals(int x, int y)
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
        public int GetHashCode(int obj)
        {
            var innerObj = obj % this.modulus;
            if (innerObj < 0)
            {
                innerObj = this.modulus - innerObj;
            }

            return innerObj.GetHashCode();
        }

        /// <summary>
        /// Calcula a adição modular de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da adição modular.</returns>
        public int Add(int left, int right)
        {
            var innerLeft = left % this.modulus;
            var innerRight = right % this.modulus;
            if (innerLeft < 0)
            {
                innerLeft += this.modulus;
            }

            if (innerRight < 0)
            {
                innerRight += this.modulus;
            }

            var leftComplement = this.modulus - innerLeft;
            var result = innerRight - leftComplement;
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
        public bool IsMultiplicativeUnity(int value)
        {
            return value % modulus == 1;
        }

        /// <summary>
        /// Calcula o produto modular de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da multiplicação modular.</returns>
        public int Multiply(int left, int right)
        {
            var innerLeft = left % this.modulus;
            var innerRight = right % this.modulus;
            if (innerLeft == 0 || innerRight == 0)
            {
                return 0;
            }

            if (innerLeft < 0)
            {
                innerLeft += this.modulus;
            }

            if (innerRight < 0)
            {
                innerRight += this.modulus;
            }

            // Processa o produto
            var sign = 1;
            if (innerLeft > this.halfModulus)
            {
                innerLeft = this.modulus - innerLeft;
                sign = -sign;
            }

            if (innerRight > this.halfModulus)
            {
                innerRight = this.modulus - innerRight;
                sign = -sign;
            }

            var leftQuo = this.modulus / innerLeft;
            if (leftQuo >= innerRight)
            {
                var result = (innerLeft * innerRight * sign) % this.modulus;
                if (result < -this.halfModulus)
                {
                    result += this.modulus;
                }
                else if(result > this.halfModulus)
                {
                    result -= this.modulus;
                }

                return result;
            }
            else
            {
                var rightRem = innerRight % leftQuo;
                var result = innerLeft * rightRem * sign;
                if (result < 0)
                {
                    result += this.modulus;
                }

                sign = -sign;
                innerLeft = this.modulus % innerLeft;
                innerRight = innerRight / leftQuo;
                while (true)
                {
                    if (innerLeft == 0)
                    {
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
                        leftQuo = this.modulus / innerLeft;
                        if (leftQuo >= innerRight)
                        {
                            var product = innerLeft * innerRight * sign;
                            if (product < 0)
                            {
                                result += product;
                                if (result < 0)
                                {
                                    result += this.modulus;
                                }
                            }
                            else
                            {
                                var productComplement = this.modulus - product;
                                result -= productComplement;
                                if (result < 0)
                                {
                                    result = this.modulus + result;
                                }
                            }

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
                            rightRem = innerRight % leftQuo;
                            var product = innerLeft * rightRem * sign;
                            if (product < 0)
                            {
                                result += product;
                                if (result < 0)
                                {
                                    result += this.modulus;
                                }
                            }
                            else
                            {
                                var productComplement = this.modulus - product;
                                result -= productComplement;
                                if (result < 0)
                                {
                                    result = this.modulus + result;
                                }
                            }

                            sign = -sign;
                            var tempInnerLeft = this.modulus % innerLeft;
                            if (tempInnerLeft != 0)
                            {
                                innerLeft = tempInnerLeft;
                            }

                            innerRight = innerRight / leftQuo;
                        }
                    }
                }
            }
        }
    }
}
