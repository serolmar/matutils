namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DoubleField : IField<double>
    {
        public double AdditiveUnity
        {
            get {
                return 0;
            }
        }

        public double MultiplicativeUnity
        {
            get {
                return 1;
            }
        }

        public double MultiplicativeInverse(double number)
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

        public double AddRepeated(double element, int times)
        {
            return element * times;
        }

        public double AdditiveInverse(double number)
        {
            return -number;
        }

        public bool IsAdditiveUnity(double value)
        {
            return value == 0;
        }

        public bool Equals(double x, double y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(double obj)
        {
            return obj.GetHashCode();
        }

        public double Add(double left, double right)
        {
            return left + right;
        }

        public bool IsMultiplicativeUnity(double value)
        {
            return value == 1;
        }

        public double Multiply(double left, double right)
        {
            return left * right;
        }
    }
}
