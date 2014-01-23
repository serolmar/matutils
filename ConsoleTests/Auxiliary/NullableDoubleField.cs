namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    public class NullableDoubleField : IField<Nullable<double>>
    {
        private DoubleField field = new DoubleField();

        public Nullable<double> AdditiveUnity
        {
            get
            {
                return this.field.AdditiveUnity;
            }
        }

        public Nullable<double> MultiplicativeUnity
        {
            get
            {
                return this.field.MultiplicativeUnity;
            }
        }

        public Nullable<double> MultiplicativeInverse(Nullable<double> number)
        {
            if (number.HasValue)
            {
                return this.field.MultiplicativeInverse(number.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        public Nullable<double> AddRepeated(Nullable<double> element, int times)
        {
            if (element.HasValue)
            {
                return this.field.AddRepeated(element.Value, times);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        public Nullable<double> AdditiveInverse(Nullable<double> number)
        {
            if (number.HasValue)
            {
                return this.field.AdditiveInverse(number.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        public bool IsAdditiveUnity(Nullable<double> value)
        {
            if (value.HasValue)
            {
                return this.field.IsAdditiveUnity(value.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        public bool Equals(Nullable<double> x, Nullable<double> y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x.HasValue && y.HasValue)
            {
                return this.field.Equals(x.Value, y.Value);
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode(Nullable<double> obj)
        {
            if (obj.HasValue)
            {
                return this.field.GetHashCode(obj.Value);
            }
            else
            {
                return typeof(Nullable<double>).GetHashCode();
            }
        }

        public Nullable<double> Add(Nullable<double> left, Nullable<double> right)
        {
            if (left.HasValue && right.HasValue)
            {
                return this.field.Add(left.Value, right.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        public bool IsMultiplicativeUnity(Nullable<double> value)
        {
            if (value.HasValue)
            {
                return this.field.IsMultiplicativeUnity(value.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }

        public Nullable<double> Multiply(Nullable<double> left, Nullable<double> right)
        {
            if (left.HasValue && right.HasValue)
            {
                return this.field.Multiply(left.Value, right.Value);
            }
            else
            {
                throw new Exception("Argument can't be null.");
            }
        }
    }
}
