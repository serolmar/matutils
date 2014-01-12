namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DecimalField : IField<decimal>
    {
        public decimal AdditiveUnity
        {
            get
            {
                return 0;
            }
        }

        public decimal MultiplicativeUnity
        {
            get
            {
                return 1;
            }
        }

        public decimal MultiplicativeInverse(decimal number)
        {
            if (number == 0)
            {
                throw new DivideByZeroException();
            }
            else
            {
                return 1 / number;
            }
        }

        public decimal AddRepeated(decimal element, int times)
        {
            return element * times;
        }

        public decimal AdditiveInverse(decimal number)
        {
            return -number;
        }

        public bool IsAdditiveUnity(decimal value)
        {
            return value == 0;
        }

        public bool Equals(decimal x, decimal y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(decimal obj)
        {
            return obj.GetHashCode();
        }

        public decimal Add(decimal left, decimal right)
        {
            return left + right;
        }

        public bool IsMultiplicativeUnity(decimal value)
        {
            return value == 1;
        }

        public decimal Multiply(decimal left, decimal right)
        {
            return left * right;
        }
    }
}
