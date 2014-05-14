namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa as operações de corpo modular.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos sobre os quais actua o corpo.</typeparam>
    public class ModularBachetBezoutField<ObjectType> : IField<ObjectType>
    {
        /// <summary>
        /// O módulo.
        /// </summary>
        private ObjectType module;

        /// <summary>
        /// O algoritmo responsável pela determinação do inverso multiplicativo.
        /// </summary>
        private IBachetBezoutAlgorithm<ObjectType> bachetBezoutAlgorithm;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="ModularBachetBezoutField{ObjectType}"/>.
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
        /// <exception cref="System.ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
        public ModularBachetBezoutField(
            ObjectType module,
            IBachetBezoutAlgorithm<ObjectType> bachetBezoutAlgorithm)
        {
            if (bachetBezoutAlgorithm == null)
            {
                throw new ArgumentNullException("bachetBezoutAlgorithm");
            }
            else if (module == null)
            {
                throw new ArgumentNullException("module");
            }
            else
            {
                this.bachetBezoutAlgorithm = bachetBezoutAlgorithm;
                this.module = module;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do módulo.
        /// </summary>
        /// <value>
        /// O módulo.
        /// </value>
        /// <exception cref="System.ArgumentNullException">Se o valor atribuído for nulo.</exception>
        public ObjectType Module
        {
            get
            {
                return this.module;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("module");
                }
                else
                {
                    this.module = value;
                }
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public ObjectType MultiplicativeUnity
        {
            get
            {
                return this.bachetBezoutAlgorithm.Domain.MultiplicativeUnity;
            }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public ObjectType AdditiveUnity
        {
            get
            {
                return this.bachetBezoutAlgorithm.Domain.AdditiveUnity;
            }
        }

        /// <summary>
        /// Obtém a redução do objecto especificado relativamente ao módulo corrente.
        /// </summary>
        /// <param name="number">O polinómio a ser reduzido.</param>
        /// <returns>O polinómio reduzido.</returns>
        public ObjectType GetReduced(ObjectType number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                var result = this.bachetBezoutAlgorithm.Domain.Rem(number, this.module);
                return result;
            }
        }

        /// <summary>
        /// Determina a inversa multiplicativa de um objecto.
        /// </summary>
        /// <param name="number">O objecto.</param>
        /// <returns>A inversa multiplicativa.</returns>
        /// <exception cref="ArgumentNullException">Se o objecto proporcionado for nulo.</exception>
        /// <exception cref="MathematicsException">Se o objecto não possuir inversa.</exception>
        public ObjectType MultiplicativeInverse(ObjectType number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                var innerNumber = this.GetReduced(number);
                if (!this.bachetBezoutAlgorithm.Domain.IsAdditiveUnity(number))
                {
                    var bezoutResult = this.bachetBezoutAlgorithm.Run(this.module, innerNumber);
                    if (this.bachetBezoutAlgorithm.Domain.IsMultiplicativeUnity(bezoutResult.GreatestCommonDivisor))
                    {
                        return this.bachetBezoutAlgorithm.Domain.AdditiveInverse(bezoutResult.SecondFactor);
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
        }

        /// <summary>
        /// Calcula a adição modular repetida de um objecto.
        /// </summary>
        /// <remarks>
        /// A adição modular repetida de um objecto pode ser rapidamente determinada com base em operações de multiplicação.
        /// </remarks>
        /// <param name="left">O número a ser adicionado.</param>
        /// <param name="right">O número de vezes que é realizada a adição.</param>
        /// <returns>O resultado da adição modular repetida.</returns>
        public ObjectType AddRepeated(ObjectType element, int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                var innerNumber = this.GetReduced(element);
                innerNumber = this.bachetBezoutAlgorithm.Domain.AddRepeated(innerNumber, times);
                innerNumber = this.GetReduced(innerNumber);
                return innerNumber;
            }
        }

        /// <summary>
        /// Obtém o inverso aditivo de um objecto.
        /// </summary>
        /// <param name="number">O objecto.</param>
        /// <returns>O inverso aditivo do objecto.</returns>
        public ObjectType AdditiveInverse(ObjectType number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                var innerNumber = this.GetReduced(number);
                return this.bachetBezoutAlgorithm.Domain.AdditiveInverse(innerNumber);
            }
        }

        /// <summary>
        /// Determina se um número é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O objecto.</param>
        /// <returns>Verdadeiro caso o objecto seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsAdditiveUnity(ObjectType value)
        {
            return this.bachetBezoutAlgorithm.Domain.IsAdditiveUnity(value);
        }

        /// <summary>
        /// Determina se ambos os objectos são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se ambos os objectos forem iguais e falso caso contrário.
        /// </returns>
        public bool Equals(ObjectType x, ObjectType y)
        {
            return this.bachetBezoutAlgorithm.Domain.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// O código confuso do objecto adequado à utilização em alguns algoritmos habituais.
        /// </returns>
        public int GetHashCode(ObjectType obj)
        {
            return this.bachetBezoutAlgorithm.Domain.GetHashCode(obj);
        }

        /// <summary>
        /// Calcula a soma de dois objectos.
        /// </summary>
        /// <param name="left">O primeiro objecto a ser adicionado.</param>
        /// <param name="right">O segundo objecto a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        public ObjectType Add(ObjectType left, ObjectType right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var innerLeft = this.GetReduced(left);
                var innerRight = this.GetReduced(right);
                var result = this.bachetBezoutAlgorithm.Domain.Add(innerLeft, innerRight);
                result = this.GetReduced(result);
                return result;
            }
        }

        /// <summary>
        /// Determina se o número é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O número.</param>
        /// <returns>Verdadeiro caso o número seja uma unidade aditiva e falso caso contrário.</returns>
        public bool IsMultiplicativeUnity(ObjectType value)
        {
            return this.bachetBezoutAlgorithm.Domain.IsAdditiveUnity(value);
        }

        /// <summary>
        /// Calcula o produto de dois objectos.
        /// </summary>
        /// <param name="left">O primeiro objecto a ser multiplicado.</param>
        /// <param name="right">O segundo objecto a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public ObjectType Multiply(ObjectType left, ObjectType right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var innerLeft = this.GetReduced(left);
                var innerRight = this.GetReduced(right);
                var result = this.bachetBezoutAlgorithm.Domain.Multiply(innerLeft, innerRight);
                result = this.GetReduced(result);
                return result;
            }
        }
    }
}
