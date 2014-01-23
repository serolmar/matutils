namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    public class IntegerNullableDoubleConverter : IConversion<int, Nullable<double>>
    {
        private IntegerDoubleConverter converter = new IntegerDoubleConverter();

        public bool CanApplyDirectConversion(Nullable<double> objectToConvert)
        {
            if (objectToConvert.HasValue)
            {
                return false;
            }
            else
            {
                return this.converter.CanApplyDirectConversion(objectToConvert.Value);
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
