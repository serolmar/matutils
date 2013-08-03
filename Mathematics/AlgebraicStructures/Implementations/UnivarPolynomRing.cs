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
        private RingType ring;

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

        public UnivariatePolynomialNormalForm<CoeffType, RingType> AdditiveUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.variableName, this.ring);
            }
        }

        public UnivariatePolynomialNormalForm<CoeffType, RingType> MultiplicativeUnity
        {
            get
            {
                return new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring.MultiplicativeUnity, 0, this.variableName, this.ring);
            }
        }

        public UnivariatePolynomialNormalForm<CoeffType, RingType> AdditiveInverse(UnivariatePolynomialNormalForm<CoeffType, RingType> number)
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

        public bool IsAdditiveUnity(UnivariatePolynomialNormalForm<CoeffType, RingType> value)
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

        public UnivariatePolynomialNormalForm<CoeffType, RingType> Add(UnivariatePolynomialNormalForm<CoeffType, RingType> left, UnivariatePolynomialNormalForm<CoeffType, RingType> right)
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

        public bool Equals(UnivariatePolynomialNormalForm<CoeffType, RingType> x, UnivariatePolynomialNormalForm<CoeffType, RingType> y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(UnivariatePolynomialNormalForm<CoeffType, RingType> obj)
        {
            throw new NotImplementedException();
        }

        public UnivariatePolynomialNormalForm<CoeffType, RingType> Multiply(UnivariatePolynomialNormalForm<CoeffType, RingType> left, UnivariatePolynomialNormalForm<CoeffType, RingType> right)
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

        public bool IsMultiplicativeUnity(UnivariatePolynomialNormalForm<CoeffType, RingType> value)
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
    }
}
