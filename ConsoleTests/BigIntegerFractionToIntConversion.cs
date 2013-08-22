namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Mathematics;

    class BigIntegerFractionToIntConversion : IConversion<int, Fraction<BigInteger, BigIntegerDomain>>
    {
        private ElementFractionConversion<BigInteger, BigIntegerDomain> elementFractionConversion;

        public BigIntegerFractionToIntConversion()
        {
            this.elementFractionConversion = new ElementFractionConversion<BigInteger, BigIntegerDomain>(
                new BigIntegerDomain());
        }

        public bool CanApplyDirectConversion(Fraction<BigInteger, BigIntegerDomain> objectToConvert)
        {
            if (objectToConvert == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        public int DirectConversion(Fraction<BigInteger, BigIntegerDomain> objectToConvert)
        {
            if (objectToConvert == null)
            {
                throw new ArgumentNullException("objectToConver");
            }
            else
            {
                var element = this.elementFractionConversion.DirectConversion(objectToConvert);
                return (int)element;
            }
        }

        public Fraction<BigInteger, BigIntegerDomain> InverseConversion(int objectToConvert)
        {
            var bigInt = new BigInteger(objectToConvert);
            return new Fraction<BigInteger, BigIntegerDomain>(bigInt, 1, this.elementFractionConversion.Domain);
        }
    }
}
