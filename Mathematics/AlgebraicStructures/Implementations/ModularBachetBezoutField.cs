using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class ModularBachetBezoutField<ObjectType, DomainType, AlgorithmType> : IField<ObjectType>
        where DomainType : IEuclidenDomain<ObjectType>
        where AlgorithmType : IBachetBezoutAlgorithm<ObjectType, DomainType>
    {
        private ObjectType module;

        private AlgorithmType bachetBezoutAlgorithm;

        public ModularBachetBezoutField(ObjectType module, AlgorithmType bachetBezoutAlgorithm)
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
