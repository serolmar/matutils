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
        /// Obtém a redução do polinómio especificado relativamente ao módulo corrente.
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

        public bool IsAdditiveUnity(ObjectType value)
        {
            return this.bachetBezoutAlgorithm.Domain.IsAdditiveUnity(value);
        }

        public bool Equals(ObjectType x, ObjectType y)
        {
            return this.bachetBezoutAlgorithm.Domain.Equals(x, y);
        }

        public int GetHashCode(ObjectType obj)
        {
            return this.bachetBezoutAlgorithm.Domain.GetHashCode(obj);
        }

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

        public bool IsMultiplicativeUnity(ObjectType value)
        {
            return this.bachetBezoutAlgorithm.Domain.IsAdditiveUnity(value);
        }

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
