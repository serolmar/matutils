using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class UnivarPolynomRing<CoeffType, RingType> :
        IRing<UnivariatePolynomialNormalForm<CoeffType, RingType>>
        where RingType : IRing<CoeffType>
    {
        protected RingType ring;

        protected string variableName;

        public UnivarPolynomRing(string variableName, RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                this.variableName = variableName;
                this.ring = ring;
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType, RingType> AdditiveUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.variableName, this.ring);
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType, RingType> MultiplicativeUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType, RingType>(
                    this.ring.MultiplicativeUnity, 
                    0, 
                    this.variableName, 
                    this.ring);
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType, RingType> AdditiveInverse(
            UnivariatePolynomialNormalForm<CoeffType, RingType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetSymmetric();
            }
        }

        public virtual bool IsAdditiveUnity(UnivariatePolynomialNormalForm<CoeffType, RingType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsZero;
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType, RingType> Add(
            UnivariatePolynomialNormalForm<CoeffType, RingType> left, 
            UnivariatePolynomialNormalForm<CoeffType, RingType> right)
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
                return left.Add(right);
            }
        }

        public virtual bool Equals(
            UnivariatePolynomialNormalForm<CoeffType, RingType> x, 
            UnivariatePolynomialNormalForm<CoeffType, RingType> y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else
            {
                return x.Equals(y);
            }
        }

        public virtual int GetHashCode(UnivariatePolynomialNormalForm<CoeffType, RingType> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.GetHashCode();
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType, RingType> Multiply(
            UnivariatePolynomialNormalForm<CoeffType, RingType> left, 
            UnivariatePolynomialNormalForm<CoeffType, RingType> right)
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
                return left.Multiply(right);
            }
        }

        public virtual bool IsMultiplicativeUnity(UnivariatePolynomialNormalForm<CoeffType, RingType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsUnity;
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType, RingType> AddRepeated(
            UnivariatePolynomialNormalForm<CoeffType, RingType> element, 
            int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.variableName, this.ring);
                foreach (var termsKvp in element)
                {
                    result = result.Add(
                        this.ring.AddRepeated(termsKvp.Value, times),
                        termsKvp.Key);
                }

                return result;
            }
        }
    }
}
