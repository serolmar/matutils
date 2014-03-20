namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Mathematics;
    using Utilities;

    class BigIntegerFractionToIntConversion : IConversion<int, Fraction<BigInteger>>
    {
        private ElementFractionConversion<BigInteger> elementFractionConversion;

        public BigIntegerFractionToIntConversion()
        {
            this.elementFractionConversion = new ElementFractionConversion<BigInteger>(
                new BigIntegerDomain());
        }

        public bool CanApplyDirectConversion(Fraction<BigInteger> objectToConvert)
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

        public int DirectConversion(Fraction<BigInteger> objectToConvert)
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

        public Fraction<BigInteger> InverseConversion(int objectToConvert)
        {
            var bigInt = new BigInteger(objectToConvert);
            return new Fraction<BigInteger>(bigInt, 1, this.elementFractionConversion.Domain);
        }
    }
}
