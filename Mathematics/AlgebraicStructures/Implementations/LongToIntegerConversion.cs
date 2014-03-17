namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite realizar conversões entre inteiros e longos.
    /// </summary>
    public class LongToIntegerConversion : IConversion<int,long>
    {
        public bool CanApplyDirectConversion(long objectToConvert)
        {
            return objectToConvert <= int.MaxValue && objectToConvert >= int.MinValue;
        }

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        public int DirectConversion(long objectToConvert)
        {
            return (int)objectToConvert;
        }

        public long InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }
}
