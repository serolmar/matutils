using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class FractionField<T, D> : IField<Fraction<T, D>>
        where D : IEuclidenDomain<T>
    {
        private D euclideanDomain;

        private IEqualityComparer<T> elementsComparer;

        public FractionField(D euclideanDomain)
        {
            if (euclideanDomain == null)
            {
                throw new ArgumentNullException("euclideanDomain");
            }
            else
            {
                this.euclideanDomain = euclideanDomain;
                this.elementsComparer = null;
            }
        }

        public FractionField(IEqualityComparer<T> elementsComparer, D euclideanDomain)
        {
            if (euclideanDomain == null)
            {
                throw new ArgumentNullException("euclideanDomain");
            }
            else
            {
                this.euclideanDomain = euclideanDomain;
                this.elementsComparer = elementsComparer;
            }
        }

        public Fraction<T, D> AdditiveUnity
        {
            get
            {
                return new Fraction<T, D>(this.euclideanDomain);
            }
        }

        public Fraction<T, D> MultiplicativeUnity
        {
            get
            {
                return new Fraction<T, D>(
                    this.euclideanDomain.MultiplicativeUnity,
                    this.euclideanDomain.MultiplicativeUnity,
                    this.euclideanDomain);
            }
        }

        public Fraction<T, D> MultiplicativeInverse(Fraction<T, D> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetInverse();
            }
        }

        public Fraction<T, D> AdditiveInverse(Fraction<T, D> number)
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

        public bool IsAdditiveUnity(Fraction<T, D> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return this.euclideanDomain.IsAdditiveUnity(value.Numerator);
            }
        }

        public Fraction<T, D> Add(Fraction<T, D> left, Fraction<T, D> right)
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

        public Fraction<T, D> Multiply(Fraction<T, D> left, Fraction<T, D> right)
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

        public bool IsMultiplicativeUnity(Fraction<T, D> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return this.euclideanDomain.IsMultiplicativeUnity(value.Numerator) &&
                    this.euclideanDomain.IsMultiplicativeUnity(value.Denominator);
            }
        }

        public bool Equals(Fraction<T, D> x, Fraction<T, D> y)
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
            else if (this.elementsComparer == null)
            {
                return x.Numerator.Equals(y.Numerator) && x.Denominator.Equals(y.Denominator);
            }
            else
            {
                return this.elementsComparer.Equals(x.Numerator, y.Numerator) &&
                    this.elementsComparer.Equals(x.Denominator, y.Denominator);
            }
        }

        public int GetHashCode(Fraction<T, D> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else if (this.elementsComparer == null)
            {
                return 19 * obj.Numerator.GetHashCode() + 17 * obj.Denominator.GetHashCode();
            }
            else
            {
                return 19 * this.elementsComparer.GetHashCode(obj.Numerator) +
                    17 * this.elementsComparer.GetHashCode(obj.Denominator);
            }
        }
    }
}
