namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Mathematics;

    public class IntegerBigIntFractionConversion
        : IConversion<int, Fraction<BigInteger, BigIntegerDomain>>
    {
        private BigIntegerDomain bigIntegerDomain;

        public IntegerBigIntFractionConversion(BigIntegerDomain bigIntegerDomain)
        {
            if (bigIntegerDomain == null)
            {
                throw new ArgumentNullException("bigIntegerDomain");
            }
            else
            {
                this.bigIntegerDomain = bigIntegerDomain;
            }
        }

        public bool CanApplyDirectConversion(Fraction<BigInteger, BigIntegerDomain> objectToConvert)
        {
            throw new NotImplementedException();
        }

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            throw new NotImplementedException();
        }

        public int DirectConversion(Fraction<BigInteger, BigIntegerDomain> objectToConvert)
        {
            throw new NotImplementedException();
        }

        public Fraction<BigInteger, BigIntegerDomain> InverseConversion(int objectToConvert)
        {
            throw new NotImplementedException();
        }
    }
}
