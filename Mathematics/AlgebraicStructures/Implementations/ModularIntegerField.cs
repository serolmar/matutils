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

        public int AdditiveUnity
        {
            get
            {
                return 0;
            }
        }

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

        public int MultiplicativeInverse(int number)
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
                        result -= this.module;
                    }

                    return result;
                }
                else
                {
                    throw new MathematicsException(string.Format("Number {0} mod {1} has no inverse.", number, this.module));
                }
            }
            else
            {
                throw new MathematicsException(string.Format("Number {0} mod {1} has no inverse.", number, this.module));
            }
        }

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

        public int AdditiveInverse(int number)
        {
            var innerNumber = number % this.module;
            if (innerNumber < 0)
            {
                innerNumber = this.module - innerNumber;
            }

            return this.module - innerNumber;
        }

        public bool IsAdditiveUnity(int value)
        {
            return value % module == 0;
        }

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

        public int GetHashCode(int obj)
        {
            var innerObj = obj % this.module;
            if (innerObj < 0)
            {
                innerObj = this.module - innerObj;
            }

            return innerObj.GetHashCode();
        }

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

        public bool IsMultiplicativeUnity(int value)
        {
            return value % module == 1;
        }

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
