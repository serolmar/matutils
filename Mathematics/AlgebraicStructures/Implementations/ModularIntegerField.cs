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
        /// <exception cref="ArgumentException">
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

    /// <summary>
    /// Implementa um sistema modular geral.
    /// </summary>
    /// <typeparam name="ModuleType">O tipo de objectos que constituem o módulo.</typeparam>
    public class GeneralModuleIntegralField<ModuleType> : IModularField<ModuleType>
    {
        /// <summary>
        /// Mantém o valor do módulo.
        /// </summary>
        private ModuleType module;

        /// <summary>
        /// O domínio responsável pelas operações sobre
        /// </summary>
        private IEuclidenDomain<ModuleType> domain;

        /// <summary>
        /// Mantém o algoritmo responsável pela determinação do máximo divisor comum e cofactor.
        /// </summary>
        private Func<ModuleType, ModuleType, IEuclidenDomain<ModuleType>, Tuple<ModuleType, ModuleType>> gcdFunc;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GeneralModuleIntegralField{ModuleType}"/>.
        /// </summary>
        /// <param name="module">O módulo.</param>
        /// <param name="domain">
        /// O domínio responsável pelas operações sobre os valores.
        /// </param>
        /// <param name="gcdFunc">
        /// A função responsável pela determinação do máximo divisor comum e cofactor do segundo valor.
        /// </param>
        /// <remarks>
        /// Os argumentos da função serão do tipo (módulo, número, domínio).
        /// </remarks>
        public GeneralModuleIntegralField(
            ModuleType module,
            IEuclidenDomain<ModuleType> domain,
            Func<ModuleType, ModuleType, IEuclidenDomain<ModuleType>, Tuple<ModuleType, ModuleType>> gcdFunc)
        {
            if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            else if (domain == null)
            {
                throw new ArgumentNullException("modularInverse");
            }
            else if (gcdFunc == null)
            {
                throw new ArgumentNullException("gcdFunc");
            }
            else
            {
                this.module = module;
                this.domain = domain;
                this.gcdFunc = gcdFunc;
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
        public ModuleType Module
        {
            get
            {
                return this.module;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Module cannot be a null value");
                }
                else
                {
                    this.module = value;
                }
            }
        }

        /// <summary>
        /// Obtém a identidade aditiva.
        /// </summary>
        public ModuleType AdditiveUnity
        {
            get
            {
                return this.domain.AdditiveUnity;
            }
        }

        /// <summary>
        /// Obtém a identidade multiplicativa.
        /// </summary>
        public ModuleType MultiplicativeUnity
        {
            get
            {
                return this.domain.MultiplicativeUnity;
            }
        }

        /// <summary>
        /// Determina a soma de dois valores.
        /// </summary>
        /// <param name="left">O primeiro valor.</param>
        /// <param name="right">O segundo valor.</param>
        /// <returns>O resultado da soma.</returns>
        public ModuleType Add(ModuleType left, ModuleType right)
        {
            var value = this.domain.Add(left, right);
            value = this.domain.Rem(value, this.module);
            return value;
        }

        /// <summary>
        /// Determina a inversa aditiva.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public ModuleType AdditiveInverse(ModuleType number)
        {
            return this.domain.AdditiveInverse(number);
        }

        /// <summary>
        /// Adiciona um valor, um determinado número de vezes.
        /// </summary>
        /// <param name="element">O elemento a ser adicionado.</param>
        /// <param name="times">O número de vezes.</param>
        /// <returns>O resultado da soma repetida.</returns>
        public ModuleType AddRepeated(ModuleType element, int times)
        {
            var value = this.domain.AddRepeated(element, times);
            value = this.domain.Rem(value, this.module);
            return value;
        }

        /// <summary>
        /// Determina a igualdade entre dois valores.
        /// </summary>
        /// <param name="x">O primeiro valor.</param>
        /// <param name="y">O segundo valor.</param>
        /// <returns>Verdadeiro se os valores forem iguais e falso caso contrário.</returns>
        public bool Equals(ModuleType x, ModuleType y)
        {
            return this.domain.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso do valor.
        /// </summary>
        /// <param name="obj">O valor.</param>
        /// <returns>O código confuso.</returns>
        public int GetHashCode(ModuleType obj)
        {
            return this.domain.GetHashCode(obj);
        }

        /// <summary>
        /// Obtém o valor na forma reduzida.
        /// </summary>
        /// <param name="element">O valor.</param>
        /// <returns>A forma reduzida do valor.</returns>
        public ModuleType GetReduced(ModuleType element)
        {
            return this.domain.Rem(element, this.module);
        }

        /// <summary>
        /// Determina se se trata da unidade aditiva.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Verdadeiro se se trata da identidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(ModuleType value)
        {
            return this.domain.IsAdditiveUnity(
                this.domain.Rem(value, this.module));
        }

        /// <summary>
        /// Determina se se trata da unidade multiplicativa.
        /// </summary>
        /// <param name="value">O valor a ser verificado.</param>
        /// <returns>Verdadeiro se se trata da unidade multiplicativa e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(ModuleType value)
        {
            return this.domain.IsMultiplicativeUnity(
                this.domain.Rem(value, this.module));
        }

        /// <summary>
        /// Determina a multiplicativa inversa do valor.
        /// </summary>
        /// <param name="number">O valor.</param>
        /// <returns>A inversa multiplicativa.</returns>
        public ModuleType MultiplicativeInverse(ModuleType number)
        {
            var bres = this.gcdFunc.Invoke(this.module, number, this.domain);
            var gcd = bres.Item1;
            if (this.domain.IsMultiplicativeUnity(gcd))
            {
                return this.domain.Rem(bres.Item2, this.module);
            }
            else
            {
                throw new MathematicsException(
                        string.Format("Number {0} mod {1} has no inverse.", number, this.module));
            }
        }

        /// <summary>
        /// Obtém o produto de dois valores.
        /// </summary>
        /// <param name="left">O primeiro valor.</param>
        /// <param name="right">O segundo valor.</param>
        /// <returns></returns>
        public ModuleType Multiply(ModuleType left, ModuleType right)
        {
            var value = this.domain.Multiply(left, right);
            value = this.domain.Rem(value, this.module);
            return value;
        }
    }
}
