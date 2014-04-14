namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    public class BigDecimalNumberField : IField<BigDecimalNumber>
    {
        /// <summary>
        /// A precisão em número de bits.
        /// </summary>
        private uint bitsPrecision;

        /// <summary>
        /// O módulo que permite manter a precisão.
        /// </summary>
        private BigInteger modulus;

        public BigDecimalNumberField(uint bitsPrecision)
        {
            this.bitsPrecision = bitsPrecision;
            this.modulus = BigInteger.One << (int)bitsPrecision;
        }

        public BigDecimalNumber AdditiveUnity
        {
            get
            {
                return BigDecimalNumber.Zero;
            }
        }

        public BigDecimalNumber MultiplicativeUnity
        {
            get
            {
                return BigDecimalNumber.One;
            }
        }

        public BigDecimalNumber MultiplicativeInverse(BigDecimalNumber number)
        {
            throw new NotImplementedException();
        }

        public BigDecimalNumber AddRepeated(BigDecimalNumber element, int times)
        {
            throw new NotImplementedException();
        }

        public BigDecimalNumber AdditiveInverse(BigDecimalNumber number)
        {
            throw new NotImplementedException();
        }

        public bool IsAdditiveUnity(BigDecimalNumber value)
        {
            return value.Number.IsZero;
        }

        public bool Equals(BigDecimalNumber x, BigDecimalNumber y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(BigDecimalNumber obj)
        {
            throw new NotImplementedException();
        }

        public BigDecimalNumber Add(BigDecimalNumber left, BigDecimalNumber right)
        {
            throw new NotImplementedException();
        }

        public bool IsMultiplicativeUnity(BigDecimalNumber value)
        {
            return value.Number.IsOne && value.NegativeExponent == 1;
        }

        public BigDecimalNumber Multiply(BigDecimalNumber left, BigDecimalNumber right)
        {
            throw new NotImplementedException();
        }
    }
}
