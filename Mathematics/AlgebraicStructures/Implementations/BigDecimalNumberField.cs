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
        /// Permite determinar rapidamente o logaritmo na base dois de um número binário.
        /// </summary>
        private static FasterBigIntBinaryLogIntPartAlg integerPartLogAlg = new FasterBigIntBinaryLogIntPartAlg();

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

        /// <summary>
        /// Permite determinar a inversa multiplicativa de um número.
        /// </summary>
        /// <param name="number">O número do qual se pretende obter a inversa multiplicativa.</param>
        /// <returns>A inversa multiplicativa.</returns>
        public BigDecimalNumber MultiplicativeInverse(BigDecimalNumber number)
        {
            return BigDecimalNumber.Divide(BigDecimalNumber.One, number, this.bitsPrecision);
        }

        /// <summary>
        /// Permite efectuar adições repetidas de um número decimal.
        /// </summary>
        /// <param name="element">O número a ser adicionado.</param>
        /// <param name="times">O número de vezes que o número é adicionado.</param>
        /// <returns>O resultado da adição.</returns>
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

        /// <summary>
        /// Permite determinar o simétrico de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O simétrico.</returns>
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
            return BigDecimalNumber.Multiply(left, right);
        }

        public string ToString(BigDecimalNumber number)
        {
            if (number.Number == 0)
            {
                return "0";
            }
            else
            {
                if (number.NegativeExponent == 0)
                {
                    return number.Number.ToString();
                }
                else
                {
                    var result = string.Empty;
                    var tempNumber = number.Number;
                    if (number.Number < 0)
                    {
                        tempNumber = -number.Number;
                        result += "-";
                    }

                    var mantissaMask = BigInteger.One << (int)number.NegativeExponent;
                    var integerPart = tempNumber >> (int)number.NegativeExponent;
                    result += integerPart.ToString();
                    var mantissaPart = tempNumber & (mantissaMask - 1);
                    var mantissaPlaces = (uint)(integerPartLogAlg.Run(mantissaPart) + 1);
                    var exponent = number.NegativeExponent;
                    result += ".";
                    var decimalPlaces = -1;
                    while (mantissaPart != 0 && decimalPlaces < this.decimalPrecision)
                    {
                        var multiplied = (mantissaPart << 2) + mantissaPart;
                        mantissaPlaces += 2;
                        var mask = mantissaMask << 2;
                        if ((multiplied & mask) != 0)
                        {
                            ++mantissaPlaces;
                        }

                        --exponent;
                        mantissaMask = mantissaMask >> 1;
                        if (mantissaPlaces < exponent)
                        {
                            result += "0";
                            mantissaPart = multiplied;
                        }
                        else
                        {
                            mantissaPart = multiplied & (mantissaMask - 1);
                            var value = multiplied >> (int)exponent;
                            result += value;
                        }
                    }

                    var temporary = result;
                    result = string.Empty;
                    var i = this.decimalPrecision + 1;
                    var tempValue = (int)char.GetNumericValue(temporary[i]);
                    var carry = true;
                    if (tempValue < 5)
                    {
                        carry = false;
                    }

                    --i;
                    var tempChar = temporary[i--];
                    while (tempChar != '.')
                    {
                        tempValue = (int)char.GetNumericValue(tempChar);
                        if (carry)
                        {
                            ++tempValue;
                        }

                        carry = false;
                        if (tempValue == 10)
                        {
                            carry = true;
                            tempValue = 0;
                        }

                        result = tempValue + result;
                        tempChar = temporary[i--];
                    }

                    result = tempChar + result;
                    while (i >= 0)
                    {
                        tempChar = temporary[i--];
                        tempValue = (int)char.GetNumericValue(tempChar);
                        if (carry)
                        {
                            ++tempValue;
                        }

                        carry = false;
                        if (tempValue == 10)
                        {
                            carry = true;
                            tempValue = 0;
                        }

                        result = tempValue + result;
                    }

                    result = result.TrimEnd('0').TrimEnd('.');
                    return result;
                }
            }
        }

        public bool TryParse(string numberText, out BigDecimalNumber number)
        {
            return BigDecimalNumber.TryParse(numberText, this.bitsPrecision, out number);
        }
    }
}
