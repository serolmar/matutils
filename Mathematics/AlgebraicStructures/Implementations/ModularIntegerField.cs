namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa as operações sobre inteiros no que concerne a aritmética modular.
    /// </summary>
    public class ModularIntegerField : IModularField<int>
    {
        /// <summary>
        /// O módulo.
        /// </summary>
        private int module;

        /// <summary>
        /// Permite efectuar um passo intermédio para a obtenção da inversa.
        /// </summary>
        private LagrangeAlgorithm<int> inverseAlgorithm;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ModularIntegerField"/>.
        /// </summary>
        /// <remarks>
        /// A determinação da inversa multiplicativa é efectuada por intermédio de algoritmos relacionados
        /// com o algoritmo que permite determinar o máximo divisor comum. Neste caso, é necessário indicar
        /// qual será o algoritmo responsável por essa operação.
        /// </remarks>
        /// <param name="module">O módulo.</param>
        /// <param name="bachetBezoutAlgorithm">
        /// O algoritmo responsável pela determinação da inversa multiplicativa.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Se o módulo for 0, 1 ou -1.
        /// </exception>
        public ModularIntegerField(int module)
        {
            if (module == 0 || module == 1 || module == -1)
            {
                throw new ArgumentException("Module can't neither 0, 1 nor -1.");
            }
            else
            {
                this.module = module;
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
        /// O valor do módulo.
        /// </value>
        /// <exception cref="ArgumentException">Se o módulo for 0, 1 ou -1.</exception>
        public int Module
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
            var innerNumber = number % this.module;
            if (innerNumber < 0)
            {
                innerNumber = this.module - innerNumber;
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
            var innerNumber = number % module;
            if (innerNumber != 0)
            {
                if (innerNumber < 0)
                {
                    innerNumber = this.module + innerNumber;
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
            var innerNumber = number % this.module;
            if (innerNumber < 0)
            {
                innerNumber = this.module - innerNumber;
            }

            return this.module - innerNumber;
        }

        /// <summary>
        /// Determina se um valor é uma unidade aditivia.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>Verdadeiro caso o valor seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(int value)
        {
            return value % module == 0;
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

        /// <summary>
        /// Retorna um código confuso para a instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// Um código confuso para a instância que pode ser usado em vários algoritmos habituais.
        /// </returns>
        public int GetHashCode(int obj)
        {
            var innerObj = obj % this.module;
            if (innerObj < 0)
            {
                innerObj = this.module - innerObj;
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
            var innerLeft = left % this.module;
            var innerRight = right % this.module;
            if (innerLeft < 0)
            {
                innerLeft += this.module;
            }

            if (innerRight < 0)
            {
                innerRight += this.module;
            }

            var leftComplement = this.module - innerLeft;
            var result = innerRight - leftComplement;
            if (result < 0)
            {
                result = this.module + result;
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
            return value % module == 1;
        }

        /// <summary>
        /// Calcula o produto modular de dois números.
        /// </summary>
        /// <param name="left">O primeiro número.</param>
        /// <param name="right">O segundo número.</param>
        /// <returns>O resultado da multiplicação modular.</returns>
        public int Multiply(int left, int right)
        {
            var innerLeft = left % this.module;
            var innerRight = right % this.module;
            if (innerLeft == 0 || innerRight == 0)
            {
                return 0;
            }

            if (innerLeft < 0)
            {
                innerLeft += this.module;
            }

            if (innerRight < 0)
            {
                innerRight += this.module;
            }

            // Processa o produto
            var halfModule = this.module / 2;
            var sign = 1;
            if (innerLeft > halfModule)
            {
                innerLeft = this.module - innerLeft;
                sign = -sign;
            }

            if (innerRight > halfModule)
            {
                innerRight = this.module - innerRight;
                sign = -sign;
            }

            var leftQuo = this.module / innerLeft;
            if (leftQuo >= innerRight)
            {
                var result = (innerLeft * innerRight * sign) % this.module;
                if (result >= 0)
                {
                    return result;
                }
                else
                {
                    return this.module + result;
                }
            }
            else
            {
                var rightRem = innerRight % leftQuo;
                var result = innerLeft * rightRem * sign;
                if (result < 0)
                {
                    result += this.module;
                }

                sign = -sign;
                innerLeft = this.module % innerLeft;
                innerRight = innerRight / leftQuo;
                while (true)
                {
                    if (innerLeft == 0)
                    {
                        return result;
                    }
                    else
                    {
                        leftQuo = this.module / innerLeft;
                        if (leftQuo >= innerRight)
                        {
                            var product = innerLeft * innerRight * sign;
                            if (product < 0)
                            {
                                result += product;
                                if (result < 0)
                                {
                                    result += this.module;
                                }
                            }
                            else
                            {
                                var productComplement = this.module - product;
                                result -= productComplement;
                                if (result < 0)
                                {
                                    result = this.module + result;
                                }
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
                                    result += this.module;
                                }
                            }
                            else
                            {
                                var productComplement = this.module - product;
                                result -= productComplement;
                                if (result < 0)
                                {
                                    result = this.module + result;
                                }
                            }

                            sign = -sign;
                            var tempInnerLeft = this.module % innerLeft;
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
