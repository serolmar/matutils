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
        private int exponent;

        /// <summary>
        /// O número.
        /// </summary>
        private BigInteger number;

        public BigDecimalNumber(int number)
        {
            this.exponent = 0;
            this.number = number;
        }

        public BigDecimalNumber(long number)
        {
            this.exponent = 0;
            this.number = number;
        }

        public BigDecimalNumber(BigInteger number)
        {
            this.exponent = 0;
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

            this.exponent = -currentExponent;
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

            this.exponent = -currentExponent;
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

            this.exponent = -currentExponent;
        }

        internal BigDecimalNumber(BigInteger number, int exponent)
        {
            this.number = number;
            this.exponent = exponent;
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
            internal set
            {
                this.number = value;
            }
        }

        /// <summary>
        /// Obtém a potência negativa de dois ao qual o número se encontra multiplicado.
        /// </summary>
        public int Exponent
        {
            get
            {
                return this.exponent;
            }
            internal set
            {
                this.exponent = value;
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
            if (left.exponent < right.exponent)
            {
                firstNumber = right;
                secondNumber = left;
            }

            var difference = -firstNumber.exponent + secondNumber.exponent;
            var secondInternalNumber = secondNumber.number;
            if (difference > 0)
            {
                secondInternalNumber = secondInternalNumber << (int)difference;
            }

            var currentExponent = -left.exponent;
            var resultNumber = BigInteger.Add(firstNumber.number, secondInternalNumber);
            var unity = BigInteger.One;
            while ((resultNumber & unity) == 0 && currentExponent > 0)
            {
                --currentExponent;
                resultNumber = resultNumber >> 1;
            }

            return new BigDecimalNumber()
            {
                exponent = -currentExponent,
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
            if (-left.exponent < -right.exponent)
            {
                firstNumber = right;
                secondNumber = left;
            }

            var difference = -firstNumber.exponent + secondNumber.exponent;
            var secondInternalNumber = secondNumber.number;
            if (difference > 0)
            {
                secondInternalNumber = secondInternalNumber << (int)difference;
            }

            var currentExponent = -left.exponent;
            var resultNumber = BigInteger.Subtract(firstNumber.number, secondInternalNumber);
            var unity = BigInteger.One;
            while ((resultNumber & unity) == 0 && currentExponent > 0)
            {
                --currentExponent;
                resultNumber = resultNumber >> 1;
            }

            return new BigDecimalNumber()
            {
                exponent = -currentExponent,
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
            var currentExponent = -left.exponent - right.exponent;
            var currentResultNumber = BigInteger.Multiply(left.number, right.number);
            if (currentExponent < 0) // O expoente do número é positivo.
            {
                currentResultNumber = currentResultNumber >> currentExponent;
                currentExponent = 0;
            }
            else
            {
                while ((currentResultNumber & 1) == 0 && currentExponent > 0)
                {
                    currentResultNumber = currentResultNumber >> 1;
                    --currentExponent;
                }
            }

            return new BigDecimalNumber()
            {
                exponent = -currentExponent,
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
                var currentExponent = -left.exponent;
                if (-left.exponent < -right.exponent)
                {
                    var difference = right.exponent - left.exponent;
                    currentNumber = currentNumber << (int)difference;
                    currentExponent = 0;
                }
                else
                {
                    currentExponent += right.exponent;
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
                    exponent = -currentExponent
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
            result.exponent = number.exponent;
            return result;
        }

        #endregion Operações Aritméticas

        /// <summary>
        /// Permite ober a representação textual do número de precisão arbitrária.
        /// </summary>
        /// <remarks>
        /// A precisão binária estabelecida por defeito é 1024.
        /// </remarks>
        /// <returns>A representação actual.</returns>
        public override string ToString()
        {
            return this.GetString(1024);
        }

        /// <summary>
        /// Permite ober a representação textual do número de precisão arbitrária.
        /// </summary>
        /// <param name="precision">A precisão binária que deverá ser considerada.</param>
        /// <returns>A representação actual.</returns>
        public string ToString(int precision)
        {
            if (precision <= 0)
            {
                throw new ArgumentOutOfRangeException("precision");
            }
            else
            {
                return this.GetString(precision);
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
                return this.number == innerObj.number && this.exponent == innerObj.exponent;
            }
        }

        public override int GetHashCode()
        {
            return this.number.GetHashCode() ^ this.exponent.GetHashCode();
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
                    var currentExponent = 0;
                    if (!string.IsNullOrWhiteSpace(matchText))
                    {
                        var mantissa = BigInteger.Zero;
                        var currentPrecision = 0;
                        while (!string.IsNullOrWhiteSpace(matchText) && currentPrecision < precision)
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

                            ++currentExponent;
                            if (mantissa != 0)
                            {
                                currentPrecision = (int)integerPartLogAlg.Run(mantissa);
                            }

                            matchText = innerText;
                        }

                        currentNumber = (currentNumber << currentExponent) + mantissa;
                    }
                    else
                    {
                        var log = (int)integerPartLogAlg.Run(currentNumber) + 1;
                        if (log > precision)
                        {
                            var difference = log - precision;
                            currentNumber = currentNumber >> difference;
                            currentExponent -= difference;
                        }
                    }

                    if (text.Contains("-"))
                    {
                        currentNumber = -currentNumber;
                    }

                    number = new BigDecimalNumber()
                    {
                        number = currentNumber,
                        exponent = -currentExponent
                    };

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Permite obter a representação textual do número sem proceder a qualquer tipo de validação.
        /// </summary>
        /// <param name="precision">A precisão binária que deverá ser considerada.</param>
        /// <returns>A representação textual.</returns>
        private string GetString(int precision)
        {
            if (this.number == 0)
            {
                return "0";
            }
            else
            {
                if (this.exponent > 0)
                {
                    return this.GetPositiveExponentString(precision);
                }
                if (this.exponent == 0)
                {
                    return this.number.ToString();
                }
                else
                {
                    return this.GetNegativeExponentString(precision);
                }
            }
        }

        /// <summary>
        /// Permite obter a representação textual do número tendo em conta que o expoente associado é negativo.
        /// </summary>
        /// <param name="precision">A precisão binária pretendida.</param>
        /// <returns>A representação textual.</returns>
        private string GetNegativeExponentString(int precision)
        {
            var currentExpnent = -this.exponent;
            var result = string.Empty;
            var tempNumber = this.number;
            if (this.number < 0)
            {
                tempNumber = -this.number;
                result += "-";
            }

            var mantissaMask = BigInteger.One << currentExpnent;
            var integerPart = tempNumber >> currentExpnent;
            result += integerPart.ToString();
            var mantissaPart = tempNumber & (mantissaMask - 1);
            var mantissaPlaces = (uint)(integerPartLogAlg.Run(mantissaPart) + 1);
            result += ".";
            var decimalPlaces = -1;
            var decimalPrecision = (int)(precision * Math.Log10(2));
            while (mantissaPart != 0 && decimalPlaces < decimalPrecision)
            {
                var multiplied = (mantissaPart << 2) + mantissaPart;
                mantissaPlaces += 2;
                var mask = mantissaMask << 2;
                if ((multiplied & mask) != 0)
                {
                    ++mantissaPlaces;
                }

                --currentExpnent;
                mantissaMask = mantissaMask >> 1;
                if (mantissaPlaces < currentExpnent)
                {
                    result += "0";
                    mantissaPart = multiplied;
                }
                else
                {
                    mantissaPart = multiplied & (mantissaMask - 1);
                    var value = multiplied >> (int)currentExpnent;
                    result += value;
                }

                ++decimalPlaces;
            }

            var temporary = result;
            result = string.Empty;
            var i = decimalPrecision + 2;
            if (i < temporary.Length)
            {
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
            }
            else
            {
                result = temporary;
            }

            result = result.TrimEnd('0').TrimEnd('.');
            return result;
        }

        /// <summary>
        /// Permite obter a representação textual do número tendo em conta que o expoente associado é positivo.
        /// </summary>
        /// <param name="precision">A precisão binária do número.</param>
        /// <returns>A representação textual do número.</returns>
        private string GetPositiveExponentString(int precision)
        {
            var result = this.number.ToString();
            var decimalPrecision = (int)(precision * Math.Log10(2)) + 1;
            var currentExponent = this.exponent;
            var decimalExponent = 0;
            while (currentExponent > 0)
            {
                // Duplica o valor do resultado.
                var duplicated = string.Empty;
                var carry = 0;
                for (int i = result.Length - 1; i >= 0; --i)
                {
                    var digit = result[i];
                    var value = (int)char.GetNumericValue(digit);
                    var duple = (value << 1) + carry;

                    carry = 1;
                    if (duple < 10)
                    {
                        carry = 0;
                    }
                    else
                    {
                        duple = duple % 10;
                    }

                    duplicated = duple + duplicated;
                }

                if (carry > 0) // Arredonda o último dígito e elimina-o de consideração.
                {
                    if (duplicated.Length < decimalPrecision)
                    {
                        result = "1" + duplicated;
                    }
                    else
                    {
                        result = string.Empty;
                        var index = duplicated.Length - 1;
                        var digit = duplicated[index];
                        var value = (int)char.GetNumericValue(digit);
                        carry = 1;
                        if (value < 5)
                        {
                            carry = 0;
                        }

                        --index;
                        for (; index >= 0; --index)
                        {
                            digit = duplicated[index];
                            value = (int)char.GetNumericValue(digit);
                            value += carry;

                            carry = 0;
                            if (value > 10)
                            {
                                value = value % 10;
                                carry = 1;
                                result = "0" + result;
                            }
                            else
                            {
                                result = value + result;
                            }
                        }

                        result = (carry + 1) + result;
                        ++decimalExponent;
                    }
                }
                else
                {
                    result = duplicated;
                }

                --currentExponent;
            }

            if (decimalExponent > 0)
            {
                decimalExponent += decimalPrecision - 1;
                if (result.Length > 1)
                {
                    result = result.Insert(1, ".");
                }

                result += "E" + decimalExponent;
            }

            return result;
        }

        /// <summary>
        /// Permite arredondar o último dígito e eliminá-lo da representação textual.
        /// </summary>
        /// <param name="representation">A representação textual.</param>
        /// <returns>O resultado do arredondamento.</returns>
        private string RoundUpAndTrim(string representation)
        {
            var result = string.Empty;
            var index = representation.Length - 1;
            var digit = representation[index];
            var value = char.GetNumericValue(digit);
            var carry = 1;
            if (value < 5)
            {
                carry = 0;
            }

            for (; index >= 0; --index)
            {
                digit = representation[index];
                value = char.GetNumericValue(digit);
                value += carry;

                carry = 0;
                if (value > 10)
                {
                    value = value % 10;
                    carry = 1;
                    result = "0" + result;
                }
                else
                {
                    result = value + result;
                }
            }

            return result;
        }

        /// <summary>
        /// Permite obter o dobro de uma representação textual de um número inteiro.
        /// </summary>
        /// <param name="value">A representação textual do número inteiro.</param>
        /// <returns>A representação textual do dobro desse número.</returns>
        private string Duplicate(string value)
        {
            var result = string.Empty;
            var carry = 0;
            for (int i = value.Length - 1; i >= 0; --i)
            {
                var digit = value[i];

            }

            if (carry == 1)
            {
                result = "1" + result;
            }

            return result;
        }
    }
}
