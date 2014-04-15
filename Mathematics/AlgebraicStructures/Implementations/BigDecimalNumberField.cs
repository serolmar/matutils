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
        /// Permite calcular de forma rápida o logaritmo binário de um inteiro de precisão arbitrária.
        /// </summary>
        private FasterBigIntBinaryLogIntPartAlg binaryLogAlg = new FasterBigIntBinaryLogIntPartAlg();

        /// <summary>
        /// A precisão em número de bits.
        /// </summary>
        private int bitsPrecision;

        /// <summary>
        /// A precisão decimal associada aos números.
        /// </summary>
        private int decimalPrecision;

        public BigDecimalNumberField(int bitsPrecision)
        {
            if (bitsPrecision < 0)
            {
                throw new ArgumentOutOfRangeException("bitsPrecision");
            }
            else
            {
                this.bitsPrecision = bitsPrecision;
                this.decimalPrecision = (int)(bitsPrecision * Math.Log10(2));
            }
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

        public int BitsPrecision
        {
            get
            {
                return this.bitsPrecision;
            }
        }

        public int DecimalPrecision
        {
            get
            {
                return this.decimalPrecision;
            }
        }

        public BigDecimalNumber MultiplicativeInverse(BigDecimalNumber number)
        {
            return BigDecimalNumber.Divide(BigDecimalNumber.One, number, this.bitsPrecision);
        }

        public BigDecimalNumber AddRepeated(BigDecimalNumber element, int times)
        {
            var number = element.Number * times;
            var exponent = element.NegativeExponent;
            var numberLog = (int)this.binaryLogAlg.Run(BigInteger.Abs(number));
            if (numberLog > this.bitsPrecision)
            {
                var difference = numberLog - this.bitsPrecision;
                number = number >> difference;
                exponent += difference;
            }

            var result = new BigDecimalNumber(number, element.NegativeExponent);
            return result;
        }

        public BigDecimalNumber AdditiveInverse(BigDecimalNumber number)
        {
            return BigDecimalNumber.Negate(number);
        }

        public bool IsAdditiveUnity(BigDecimalNumber value)
        {
            return value.Number.IsZero;
        }

        public bool Equals(BigDecimalNumber x, BigDecimalNumber y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(BigDecimalNumber obj)
        {
            return obj.GetHashCode();
        }

        public BigDecimalNumber Add(BigDecimalNumber left, BigDecimalNumber right)
        {
            return BigDecimalNumber.Add(left, right);
        }

        public bool IsMultiplicativeUnity(BigDecimalNumber value)
        {
            return value.Number.IsOne && value.NegativeExponent == 1;
        }

        public BigDecimalNumber Multiply(BigDecimalNumber left, BigDecimalNumber right)
        {
            var exponent = left.NegativeExponent + right.NegativeExponent;
            var number = left.Number * right.Number;
            var numberLog = (int)this.binaryLogAlg.Run(BigInteger.Abs(number));
            if (numberLog > this.bitsPrecision)
            {
                var difference = numberLog - this.bitsPrecision;
                number = number >> difference;
                exponent += difference;
            }

            return new BigDecimalNumber(number, exponent);
        }

        public string ToString(BigDecimalNumber number)
        {
            throw new NotImplementedException();
        }

        public bool TryParse(string numberText, out BigDecimalNumber number)
        {
            throw new NotImplementedException();
        }
    }
}
