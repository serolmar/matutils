namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Indica que não é possível converter entre os dois tipos de dados.
    /// </summary>
    public class CantConvertConversion<FirstType, SecondType> : IConversion<FirstType, SecondType>
    {
        public bool CanApplyDirectConversion(SecondType objectToConvert)
        {
            // Nuca é possível converter.
            return false;
        }

        public bool CanApplyInverseConversion(FirstType objectToConvert)
        {
            // Nunca é possível converter.
            return false;
        }

        public FirstType DirectConversion(SecondType objectToConvert)
        {
            throw new MathematicsException(
                string.Format("Can't convert from type {0} to type {1}.",
                typeof(SecondType).ToString(),
                typeof(FirstType).ToString()));
        }

        public SecondType InverseConversion(FirstType objectToConvert)
        {
            throw new MathematicsException(
                string.Format("Can't convert from type {0} to type {1}.",
                typeof(SecondType).ToString(),
                typeof(FirstType).ToString()));
        }
    }
}
