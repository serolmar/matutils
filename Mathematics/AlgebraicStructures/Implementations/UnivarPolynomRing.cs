using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class UnivarPolynomRing<CoeffType> :
        IRing<UnivariatePolynomialNormalForm<CoeffType>>
    {
        protected IRing<CoeffType> ring;

        protected string variableName;

        public UnivarPolynomRing(string variableName, IRing<CoeffType> ring)
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

        public virtual UnivariatePolynomialNormalForm<CoeffType> AdditiveUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType>(this.variableName);
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType> MultiplicativeUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType>(
                    this.ring.MultiplicativeUnity, 
                    0, 
                    this.variableName, 
                    this.ring);
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType> AdditiveInverse(
            UnivariatePolynomialNormalForm<CoeffType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetSymmetric(this.ring);
            }
        }

        public virtual bool IsAdditiveUnity(UnivariatePolynomialNormalForm<CoeffType> value)
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

        public virtual UnivariatePolynomialNormalForm<CoeffType> Add(
            UnivariatePolynomialNormalForm<CoeffType> left, 
            UnivariatePolynomialNormalForm<CoeffType> right)
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
                return left.Add(right, this.ring);
            }
        }

        public virtual bool Equals(
            UnivariatePolynomialNormalForm<CoeffType> x, 
            UnivariatePolynomialNormalForm<CoeffType> y)
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

        public virtual int GetHashCode(UnivariatePolynomialNormalForm<CoeffType> obj)
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

        public virtual UnivariatePolynomialNormalForm<CoeffType> Multiply(
            UnivariatePolynomialNormalForm<CoeffType> left, 
            UnivariatePolynomialNormalForm<CoeffType> right)
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
                return left.Multiply(right, this.ring);
            }
        }

        public virtual bool IsMultiplicativeUnity(UnivariatePolynomialNormalForm<CoeffType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsUnity(this.ring);
            }
        }

        public virtual UnivariatePolynomialNormalForm<CoeffType> AddRepeated(
            UnivariatePolynomialNormalForm<CoeffType> element, 
            int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType>(this.variableName);
                foreach (var termsKvp in element)
                {
                    result = result.Add(
                        this.ring.AddRepeated(termsKvp.Value, times),
                        termsKvp.Key, this.ring);
                }

                return result;
            }
        }
    }
}
