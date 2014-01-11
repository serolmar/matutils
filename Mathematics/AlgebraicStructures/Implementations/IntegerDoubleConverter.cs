namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerDoubleConverter : IConversion<int, double>
    {
        public bool CanApplyDirectConversion(double objectToConvert)
        {
            return Math.Round(objectToConvert) == objectToConvert;
        }

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        public int DirectConversion(double objectToConvert)
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

        public double InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }
}
