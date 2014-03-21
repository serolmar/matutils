namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    public class IntegerNullableDoubleConverter : IConversion<int, Nullable<double>>
    {
        private DoubleToIntegerConversion converter;

        public IntegerNullableDoubleConverter(double precision = 0.00001)
        {
            this.converter = new DoubleToIntegerConversion(precision);
        }

        public bool CanApplyDirectConversion(Nullable<double> objectToConvert)
        {
            if (objectToConvert.HasValue)
            {
                return this.converter.CanApplyDirectConversion(objectToConvert.Value);
            }
            else
            {
                return false;
            }
        }

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        public int DirectConversion(Nullable<double> objectToConvert)
        {
            if (objectToConvert.HasValue)
            {
                return this.converter.DirectConversion(objectToConvert.Value);
            }
            else
            {
                throw new Exception("Can't convert null value to integer.");
            }
        }

        public Nullable<double> InverseConversion(int objectToConvert)
        {
            return (double)objectToConvert;
        }
    }
}
