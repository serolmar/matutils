namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Mathematics;

    public class IntegerBigIntFractionConversion
        : IConversion<int, Fraction<BigInteger>>
    {
        private IIntegerNumber<BigInteger> integerNumber;

        private BigIntegerToIntegerConversion bigIntegerToIntegerConversion;

        public IntegerBigIntFractionConversion(
            IIntegerNumber<BigInteger> integerNumber,
            BigIntegerToIntegerConversion bigIntegerToIntegerConversion)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (bigIntegerToIntegerConversion == null)
            {
                throw new ArgumentNullException("bigIntegerToIntegerConversion");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.bigIntegerToIntegerConversion = bigIntegerToIntegerConversion;
            }
        }

        public bool CanApplyDirectConversion(Fraction<BigInteger> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else if (this.integerNumber.IsMultiplicativeUnity(objectToConvert.Denominator))
            {
                return this.bigIntegerToIntegerConversion.CanApplyDirectConversion(objectToConvert.Numerator);
            }
            else if (this.integerNumber.IsMultiplicativeUnity(this.integerNumber.AdditiveInverse(objectToConvert.Denominator)))
            {
                return true;
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

        public int DirectConversion(Fraction<BigInteger> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConvert");
            }
            else if (this.integerNumber.IsMultiplicativeUnity(objectToConvert.Denominator))
            {
                return this.bigIntegerToIntegerConversion.DirectConversion(objectToConvert.Numerator);
            }
                else if (this.integerNumber.IsMultiplicativeUnity(this.integerNumber.AdditiveInverse(objectToConvert.Denominator)))
            {
                return this.bigIntegerToIntegerConversion.DirectConversion(
                    this.integerNumber.AdditiveInverse(objectToConvert.Numerator));
            }
            else
            {
                throw new MathematicsException(string.Format("Can't convert {0} to an integer value.", objectToConvert));
            }
        }

        public Fraction<BigInteger> InverseConversion(int objectToConvert)
        {
            return new Fraction<BigInteger>(
                objectToConvert,
                this.integerNumber.MultiplicativeUnity,
                this.integerNumber);
        }
    }
}
