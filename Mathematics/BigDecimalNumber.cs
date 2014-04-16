namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Representa um número decimal de precisão arbitrária.
    /// </summary>
    public struct BigDecimalNumber
    {
        /// <summary>
        /// Permite determinar rapidamente o logaritmo na base dois de um número binário.
        /// </summary>
        private static FasterBigIntBinaryLogIntPartAlg integerPartLogAlg = new FasterBigIntBinaryLogIntPartAlg();

        /// <summary>
        /// O número zero.
        /// </summary>
        private static BigDecimalNumber zero = new BigDecimalNumber(0);

        /// <summary>
        /// O número um.
        /// </summary>
        private static BigDecimalNumber one = new BigDecimalNumber(1);

        /// <summary>
        /// O número -1.
        /// </summary>
        private static BigDecimalNumber minusOne = new BigDecimalNumber(-1);

        /// <summary>
        /// O valor do expoente negativo.
        /// </summary>
        private int negativeExponent;

        /// <summary>
        /// O número.
        /// </summary>
        private BigInteger number;

        public BigDecimalNumber(int number)
        {
            this.negativeExponent = 0;
            this.number = number;
        }

        public BigDecimalNumber(long number)
        {
            this.negativeExponent = 0;
            this.number = number;
        }

        public BigDecimalNumber(BigInteger number)
        {
            this.negativeExponent = 0;
            this.number = number;
        }

        public BigDecimalNumber(double number, int mantissaBitsPrecision = 64)
        {
            var innerNumber = number;
            var signal = false;
            if (number < 0)
            {
                signal = true;
                innerNumber = -number;
            }

            var integerPart = (int)innerNumber;
            var currentExponent = 0;
            var currentNumber = new BigInteger(integerPart);
            var precision = 0;
            if (currentNumber > BigInteger.One)
            {
                precision = (int)BigInteger.Log(currentNumber);
            }

            var mantissa = innerNumber - integerPart;
            while (mantissa != 0 && precision <= mantissaBitsPrecision)
            {
                while (mantissa < 1)
                {
                    mantissa *= 2;
                    ++currentExponent;
                    currentNumber = currentNumber << 1;
                    ++precision;
                }

                mantissa -= 1;
                ++currentNumber;
            }

            if (signal)
            {
                this.number = -currentNumber;
            }
            else
            {
                this.number = currentNumber;
            }

            this.negativeExponent = currentExponent;
        }

        public BigDecimalNumber(float number, int mantissaBitsPrecision = 32)
        {
            var innerNumber = number;
            var signal = false;
            if (number < 0)
            {
                signal = true;
                innerNumber = -number;
            }

            var integerPart = (int)innerNumber;
            var currentExponent = 0;
            var currentNumber = new BigInteger(integerPart);
            var precision = 0;
            if (currentNumber > BigInteger.One)
            {
                precision = (int)BigInteger.Log(currentNumber);
            }

            var mantissa = innerNumber - integerPart;
            while (mantissa != 0 && precision <= mantissaBitsPrecision)
            {
                while (mantissa < 1)
                {
                    mantissa *= 2;
                    ++currentExponent;
                    currentNumber = currentNumber << 1;
                    ++precision;
                }

                mantissa -= 1;
                ++currentNumber;
            }

            if (signal)
            {
                this.number = -currentNumber;
            }
            else
            {
                this.number = currentNumber;
            }

            this.negativeExponent = currentExponent;
        }

        public BigDecimalNumber(decimal number, int mantissaBitsPrecision = 128)
        {
            var innerNumber = number;
            var signal = false;
            if (number < 0)
            {
                signal = true;
                innerNumber = -number;
            }

            var integerPart = (int)innerNumber;
            var currentExponent = 0;
            var currentNumber = new BigInteger(integerPart);
            var precision = 0;
            if (currentNumber > BigInteger.One)
            {
                precision = (int)BigInteger.Log(currentNumber);
            }

            var mantissa = innerNumber - integerPart;
            while (mantissa != 0 && precision <= mantissaBitsPrecision)
            {
                while (mantissa < 1)
                {
                    mantissa *= 2;
                    ++currentExponent;
                    currentNumber = currentNumber << 1;
                    ++precision;
                }

                mantissa -= 1;
                ++currentNumber;
            }

            if (signal)
            {
                this.number = -currentNumber;
            }
            else
            {
                this.number = currentNumber;
            }

            this.negativeExponent = currentExponent;
        }

        internal BigDecimalNumber(BigInteger number, int exponent)
        {
            this.number = number;
            this.negativeExponent = exponent;
        }

        #region Propriedades Públicas

        /// <summary>
        /// O número zero.
        /// </summary>
        public static BigDecimalNumber Zero
        {
            get
            {
                return zero;
            }
        }

        /// <summary>
        /// O número um.
        /// </summary>
        public static BigDecimalNumber One
        {
            get
            {
                return one;
            }
        }

        /// <summary>
        /// O número -1.
        /// </summary>
        public static BigDecimalNumber MinusOne
        {
            get
            {
                return minusOne;
            }
        }

        /// <summary>
        /// Permite obter o número inteiro.
        /// </summary>
        public BigInteger Number
        {
            get
            {
                return this.number;
            }
        }

        /// <summary>
        /// Obtém a potência negativa de dois ao qual o número se encontra multiplicado.
        /// </summary>
        public int NegativeExponent
        {
            get
            {
                return this.negativeExponent;
            }
        }

        #endregion Propriedades Públicas

        #region Operações Aritméticas

        /// <summary>
        /// Permite adicionar dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser adcicionado.</param>
        /// <param name="right">O segundo número a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        public static BigDecimalNumber Add(BigDecimalNumber left, BigDecimalNumber right)
        {
            var firstNumber = left;
            var secondNumber = right;
            if (left.negativeExponent < right.negativeExponent)
            {
                firstNumber = right;
                secondNumber = left;
            }

            var difference = firstNumber.negativeExponent - secondNumber.negativeExponent;
            var secondInternalNumber = secondNumber.number;
            if (difference > 0)
            {
                secondInternalNumber = secondInternalNumber << (int)difference;
            }

            var currentExponent = left.negativeExponent;
            var resultNumber = BigInteger.Add(firstNumber.number, secondInternalNumber);
            var unity = BigInteger.One;
            while ((resultNumber & unity) == 0 && currentExponent > 0)
            {
                --currentExponent;
                resultNumber = resultNumber >> 1;
            }

            return new BigDecimalNumber()
            {
                negativeExponent = currentExponent,
                number = resultNumber
            };
        }

        /// <summary>
        /// Permite subtrarir dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser subtraído.</param>
        /// <param name="right">O segundo número a ser subtraído.</param>
        /// <returns>O resultado da subtracção.</returns>
        public static BigDecimalNumber Subtract(BigDecimalNumber left, BigDecimalNumber right)
        {
            var firstNumber = left;
            var secondNumber = right;
            if (left.negativeExponent < right.negativeExponent)
            {
                firstNumber = right;
                secondNumber = left;
            }

            var difference = firstNumber.negativeExponent - secondNumber.negativeExponent;
            var secondInternalNumber = secondNumber.number;
            if (difference > 0)
            {
                secondInternalNumber = secondInternalNumber << (int)difference;
            }

            var currentExponent = left.negativeExponent;
            var resultNumber = BigInteger.Subtract(firstNumber.number, secondInternalNumber);
            var unity = BigInteger.One;
            while ((resultNumber & unity) == 0 && currentExponent > 0)
            {
                --currentExponent;
                resultNumber = resultNumber >> 1;
            }

            return new BigDecimalNumber()
            {
                negativeExponent = currentExponent,
                number = resultNumber
            };
        }

        /// <summary>
        /// Permite multiplicar dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser multiplicado.</param>
        /// <param name="right">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public static BigDecimalNumber Multiply(BigDecimalNumber left, BigDecimalNumber right)
        {
            var currentExponent = left.negativeExponent + right.negativeExponent;
            var currentResultNumber = BigInteger.Multiply(left.number, right.number);
            while ((currentResultNumber & 1) == 0 && currentExponent > 0)
            {
                currentResultNumber = currentResultNumber >> 1;
                --currentExponent;
            }

            return new BigDecimalNumber()
            {
                negativeExponent = currentExponent,
                number = currentResultNumber
            };
        }

        /// <summary>
        /// Permite dividir dois números.
        /// </summary>
        /// <param name="left">O primeiro número  a ser dividido.</param>
        /// <param name="right">O segundo número a ser dividido.</param>
        /// <param name="maxPrecision">O valor da precisão máxima para a divisão.</param>
        /// <returns>O resultdao da divisão.</returns>
        public static BigDecimalNumber Divide(
            BigDecimalNumber left,
            BigDecimalNumber right,
            int maxPrecision)
        {
            if (right.number == 0)
            {
                throw new DivideByZeroException();
            }
            else
            {
                var currentNumber = left.number;
                var currentExponent = left.negativeExponent;
                if (left.negativeExponent < right.negativeExponent)
                {
                    var difference = right.negativeExponent - left.negativeExponent;
                    currentNumber = currentNumber << (int)difference;
                    currentExponent = 0;
                }
                else
                {
                    currentExponent -= right.negativeExponent;
                }

                // Pode ser que se esteja na presença de múltiplos.
                var remainder = default(BigInteger);
                currentNumber = BigInteger.DivRem(currentNumber, right.number, out remainder);
                if (remainder != 0)
                {
                    if (currentExponent <= maxPrecision)
                    {
                        var divisorLog = (int)integerPartLogAlg.Run(right.number);
                        while (remainder != 0 && currentExponent < maxPrecision)
                        {
                            var remLog = (int)integerPartLogAlg.Run(remainder);
                            var lower = divisorLog - remLog + 1;
                            remainder = remainder << lower;
                            currentNumber = currentNumber << lower;
                            var quo = BigInteger.DivRem(remainder, right.number, out remainder);
                            currentNumber += quo;
                            currentExponent += lower;
                        }

                        while (currentExponent > maxPrecision)
                        {
                            currentNumber = currentNumber >> 1;
                            --currentExponent;
                        }
                    }
                }

                return new BigDecimalNumber()
                {
                    number = currentNumber,
                    negativeExponent = currentExponent
                };
            }
        }

        /// <summary>
        /// Obtém o simétrico de um número decimal de precisão arbitrária.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O valor simétrico.</returns>
        public static BigDecimalNumber Negate(BigDecimalNumber number)
        {
            var result = new BigDecimalNumber();
            result.number = number.number;
            result.negativeExponent = number.negativeExponent;
            return result;
        }

        #endregion Operações Aritméticas

        /// <summary>
        /// Permite ober a representação textual do número de precisão arbitrária.
        /// </summary>
        /// <returns>A representação actual.</returns>
        public override string ToString()
        {
            if (this.number == 0)
            {
                return "0";
            }
            else
            {
                if (this.negativeExponent == 0)
                {
                    return this.number.ToString();
                }
                else
                {
                    var result = string.Empty;
                    var tempNumber = this.number;
                    if (this.number < 0)
                    {
                        tempNumber = -this.number;
                        result += "-";
                    }

                    var mantissaMask = BigInteger.One << (int)this.negativeExponent;
                    var integerPart = tempNumber >> (int)this.negativeExponent;
                    result += integerPart.ToString();
                    var mantissaPart = tempNumber & (mantissaMask - 1);
                    var mantissaPlaces = (uint)(integerPartLogAlg.Run(mantissaPart) + 1);
                    var exponent = this.negativeExponent;
                    result += ".";
                    while (mantissaPart != 0)
                    {
                        var multiplied = (mantissaPart << 2) + mantissaPart;
                        mantissaPlaces += 2;
                        var mask = mantissaMask << 2;
                        var nextMask = mask << 1;
                        if ((multiplied & mask) != 0)
                        {
                            mask = nextMask;
                            ++mantissaPlaces;
                        }

                        --exponent;
                        mantissaMask = mantissaMask >> 1;
                        if (mantissaPlaces < exponent)
                        {
                            result += "0";
                        }
                        else
                        {
                            var difference = mantissaPlaces - exponent;
                            mantissaPart = multiplied & (mantissaMask - 1);
                            var value = multiplied >> (int)exponent;
                            result += value;
                        }
                    }

                    return result;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else if (!(obj is BigDecimalNumber))
            {
                return false;
            }
            else
            {
                var innerObj = (BigDecimalNumber)obj;
                return this.number == innerObj.number && this.negativeExponent == innerObj.negativeExponent;
            }
        }

        public override int GetHashCode()
        {
            return this.number.GetHashCode() ^ this.negativeExponent.GetHashCode();
        }

        /// <summary>
        /// Tenta fazer a leitura de um número decimal.
        /// </summary>
        /// <param name="text">O texto que contém a representação do número.</param>
        /// <param name="precision">A precisão segundo a qual o número é lido.</param>
        /// <param name="number">O número.</param>
        /// <returns>Verdadeiro caso a leitura tenha sido feita com sucesso e falso caso contrário.</returns>
        public static bool TryParse(string text, int precision, out BigDecimalNumber number)
        {
            number = new BigDecimalNumber();
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }
            else
            {
                var numberRegex = new Regex(@"^\s*(?:-){0,1}(\d*)(?:\.(\d+)){0,1}\s*$");
                var matches = numberRegex.Match(text);
                if (matches.Success)
                {
                    var currentNumber = BigInteger.Zero;
                    var matchText = matches.Groups[1].Value;
                    if (!string.IsNullOrWhiteSpace(matchText))
                    {
                        if (!BigInteger.TryParse(matchText, out currentNumber))
                        {
                            return false;
                        }
                    }

                    matchText = matches.Groups[2].Value.Trim().TrimEnd('0');
                    var exponent = 0;
                    if (!string.IsNullOrWhiteSpace(matchText))
                    {
                        var mantissa = BigInteger.Zero;
                        var currentPrecision = 0;
                        while (!string.IsNullOrWhiteSpace(matchText) && currentPrecision <= precision)
                        {
                            var innerText = string.Empty;
                            var i = matchText.Length - 1;
                            var currentChar = matchText[i];
                            var currentValue = (int)char.GetNumericValue(currentChar);
                            currentValue = currentValue << 1;
                            var carry = currentValue / 10;
                            currentValue = currentValue % 10;
                            if (!string.IsNullOrWhiteSpace(innerText) || currentValue != 0)
                            {
                                innerText = innerText.Insert(0, currentValue.ToString());
                            }

                            --i;
                            for (; i >= 0; --i)
                            {
                                currentChar = matchText[i];
                                currentValue = (int)char.GetNumericValue(currentChar);
                                currentValue = (currentValue << 1) + carry;
                                carry = currentValue / 10;
                                currentValue = currentValue % 10;
                                if (!string.IsNullOrWhiteSpace(innerText) || currentValue != 0)
                                {
                                    innerText = innerText.Insert(0, currentValue.ToString());
                                }
                            }

                            var temporary = mantissa;
                            mantissa = mantissa << 1;
                            if (carry > 0)
                            {
                                ++mantissa;
                            }

                            ++exponent;
                            ++currentPrecision;
                            matchText = innerText;
                        }

                        currentNumber = (currentNumber << exponent) + mantissa;
                    }

                    if (text.Contains("-"))
                    {
                        currentNumber = -currentNumber;
                    }

                    number = new BigDecimalNumber() { number = currentNumber, negativeExponent = exponent };
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
