using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class FractionField<T> : IField<Fraction<T>>
    {
        private IEuclidenDomain<T> euclideanDomain;

        private IEqualityComparer<T> elementsComparer;

        public FractionField(IEuclidenDomain<T> euclideanDomain)
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

        public FractionField(IEqualityComparer<T> elementsComparer, IEuclidenDomain<T> euclideanDomain)
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

        public IEuclidenDomain<T> EuclideanDomain
        {
            get
            {
                return this.euclideanDomain;
            }
        }

        public Fraction<T> AdditiveUnity
        {
            get
            {
                return new Fraction<T>(this.euclideanDomain);
            }
        }

        public Fraction<T> MultiplicativeUnity
        {
            get
            {
                return new Fraction<T>(
                    this.euclideanDomain.MultiplicativeUnity,
                    this.euclideanDomain.MultiplicativeUnity,
                    this.euclideanDomain);
            }
        }

        public Fraction<T> MultiplicativeInverse(Fraction<T> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetInverse(this.euclideanDomain);
            }
        }

        public Fraction<T> AdditiveInverse(Fraction<T> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetSymmetric(this.euclideanDomain);
            }
        }

        public bool IsAdditiveUnity(Fraction<T> value)
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

        public Fraction<T> Add(Fraction<T> left, Fraction<T> right)
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
                return left.Add(right, this.euclideanDomain);
            }
        }

        public Fraction<T> Multiply(Fraction<T> left, Fraction<T> right)
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
                return left.Multiply(right, this.euclideanDomain);
            }
        }

        public bool IsMultiplicativeUnity(Fraction<T> value)
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

        public Fraction<T> AddRepeated(Fraction<T> element, int times)
        {
            if (this.euclideanDomain.IsAdditiveUnity(element.Numerator))
            {
                return new Fraction<T>(element.Numerator, element.Denominator, this.euclideanDomain);
            }
            else
            {
                var added = this.euclideanDomain.AddRepeated(element.Numerator, times);
                return new Fraction<T>(added, element.Denominator, this.euclideanDomain);
            }
        }

        public bool Equals(Fraction<T> x, Fraction<T> y)
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

        public int GetHashCode(Fraction<T> obj)
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
