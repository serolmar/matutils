namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerDoubleMultiplicationOperation : IMultiplicationOperation<int, double, double>
    {
        public double Multiply(int left, double right)
        {
            return left * right;
        }
    }
}
