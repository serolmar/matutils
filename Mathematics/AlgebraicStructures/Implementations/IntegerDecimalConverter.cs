namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerDecimalConverter : IConversion<int, decimal>
    {
        public bool CanApplyDirectConversion(decimal objectToConvert)
        {
            return Math.Round(objectToConvert) == objectToConvert;
        }

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        public int DirectConversion(decimal objectToConvert)
        {
            var value = Math.Round(objectToConvert);
            if (value != objectToConvert)
            {
                throw new MathematicsException(string.Format("Can't convert value {0} to integer.", objectToConvert));
            }
            else
            {
                return (int)value;
            }
        }

        public decimal InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }
}
