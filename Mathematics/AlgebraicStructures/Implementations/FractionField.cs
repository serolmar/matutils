using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class FractionField<T> : ElementFractionConversion<T>, IField<Fraction<T>>, IConversion<T, Fraction<T>>
    {
        private IEqualityComparer<T> elementsComparer;

        public FractionField(IEuclidenDomain<T> domain)
            : base(domain)
        {
            this.elementsComparer = null;
        }

        public FractionField(IEqualityComparer<T> elementsComparer, IEuclidenDomain<T> domain)
            : base(domain)
        {
            this.elementsComparer = elementsComparer;
        }

        public Fraction<T> AdditiveUnity
        {
            get
            {
                return new Fraction<T>(this.domain);
            }
        }

        public Fraction<T> MultiplicativeUnity
        {
            get
            {
                return new Fraction<T>(
                    this.domain.MultiplicativeUnity,
                    this.domain.MultiplicativeUnity,
                    this.domain);
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
                return number.GetInverse(this.domain);
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
                return number.GetSymmetric(this.domain);
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
                return this.domain.IsAdditiveUnity(value.Numerator);
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
                return left.Add(right, this.domain);
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
                return left.Multiply(right, this.domain);
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
                return this.domain.IsMultiplicativeUnity(value.Numerator) &&
                    this.domain.IsMultiplicativeUnity(value.Denominator);
            }
        }

        public Fraction<T> AddRepeated(Fraction<T> element, int times)
        {
            if (this.domain.IsAdditiveUnity(element.Numerator))
            {
                return new Fraction<T>(element.Numerator, element.Denominator, this.domain);
            }
            else
            {
                var added = this.domain.AddRepeated(element.Numerator, times);
                return new Fraction<T>(added, element.Denominator, this.domain);
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
