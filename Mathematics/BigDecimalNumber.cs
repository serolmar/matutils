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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigDecimalNumber"/>.
        /// </summary>
        /// <param name="number">O número.</param>
        public BigDecimalNumber(int number)
        {
            this.exponent = 0;
            this.number = number;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigDecimalNumber"/>.
        /// </summary>
        /// <param name="number">O número.</param>
        public BigDecimalNumber(long number)
        {
            this.exponent = 0;
            this.number = number;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigDecimalNumber"/>.
        /// </summary>
        /// <param name="number">O número.</param>
        public BigDecimalNumber(BigInteger number)
        {
            this.exponent = 0;
            this.number = number;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigDecimalNumber"/>.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <param name="mantissaBitsPrecision">A precisão do número em bits.</param>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigDecimalNumber"/>.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <param name="mantissaBitsPrecision">A precisão do número em bits.</param>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigDecimalNumber"/>.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <param name="mantissaBitsPrecision">A precisão do número em bits.</param>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BigDecimalNumber"/>.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <param name="exponent">O expoente.</param>
        internal BigDecimalNumber(BigInteger number, int exponent)
        {
            this.number = number;
            this.exponent = exponent;
        }

        #region Propriedades Públicas

        /// <summary>
        /// Obtém o número zero.
        /// </summary>
        /// <value>O número zero.</value>
        public static BigDecimalNumber Zero
        {
            get
            {
                return zero;
            }
        }

        /// <summary>
        /// Obtém o número um.
        /// </summary>
        /// <value>O número um.</value>
        public static BigDecimalNumber One
        {
            get
            {
                return one;
            }
        }

        /// <summary>
        /// Obtém número -1.
        /// </summary>
        /// <value>O número -1.</value>
        public static BigDecimalNumber MinusOne
        {
            get
            {
                return minusOne;
            }
        }

        /// <summary>
        /// Permite obter o número inteiro afecto pela potência.
        /// </summary>
        /// <value>O número inteiro afecto pela potência.</value>
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
        /// <value>A potência.</value>
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
        /// <param name="maxPrecision">A precisão binária associada ao número.</param>
        /// <returns>O resultado da adição.</returns>
        public static BigDecimalNumber Add(
            BigDecimalNumber left,
            BigDecimalNumber right,
            int maxPrecision = 1024)
        {
            if (left.number.IsZero)
            {
                return right;
            }
            else if (right.number.IsZero)
            {
                return left;
            }
            else if (left.exponent == right.exponent)
            {
                var resultNumber = BigInteger.Add(left.number, right.number);
                var currentExponent = left.exponent;

                var log = (int)integerPartLogAlg.Run(resultNumber);
                if (log > maxPrecision)
                {
                    var difference = log - maxPrecision + 1;
                    resultNumber = resultNumber >> difference;
                    currentExponent += difference;
                }

                var unity = BigInteger.One;
                while ((resultNumber & unity) == 0 && currentExponent > 0)
                {
                    ++currentExponent;
                    resultNumber = resultNumber >> 1;
                }

                return new BigDecimalNumber()
                {
                    exponent = currentExponent,
                    number = resultNumber
                };
            }
            else
            {
                var firstNumber = left;
                var secondNumber = right;
                if (left.exponent < right.exponent)
                {
                    firstNumber = right;
                    secondNumber = left;
                }

                var firstTemporaryNumber = BigInteger.Abs(firstNumber.number);
                var secondTemporaryNumber = BigInteger.Abs(secondNumber.number);
                var firstNumberSlack = maxPrecision - (int)integerPartLogAlg.Run(firstTemporaryNumber) - 1;
                var secondNumberSlack = maxPrecision - (int)integerPartLogAlg.Run(secondTemporaryNumber) - 1;

                if (firstNumber.exponent + secondNumber.exponent > firstNumberSlack + secondNumberSlack)
                {
                    return firstNumber;
                }
                else
                {
                    var currentExponent = secondNumber.exponent;
                    var differenceExponent = firstNumber.exponent - secondNumber.exponent;
                    if (differenceExponent < firstNumberSlack)
                    {
                        firstTemporaryNumber = firstNumber.number << differenceExponent;
                    }
                    else
                    {
                        firstTemporaryNumber = firstNumber.number << firstNumberSlack;
                        var slackDiff = differenceExponent - secondNumberSlack;
                        secondTemporaryNumber = secondNumber.number >> slackDiff;
                        currentExponent += slackDiff;
                    }

                    var resultNumber = BigInteger.Add(firstTemporaryNumber, secondTemporaryNumber);
                    var log = (int)integerPartLogAlg.Run(resultNumber);
                    if (log > maxPrecision)
                    {
                        var difference = log - maxPrecision + 1;
                        resultNumber = resultNumber >> difference;
                        currentExponent += difference;
                    }

                    var unity = BigInteger.One;
                    while ((resultNumber & unity) == 0 && currentExponent > 0)
                    {
                        ++currentExponent;
                        resultNumber = resultNumber >> 1;
                    }

                    return new BigDecimalNumber()
                    {
                        Exponent = currentExponent,
                        Number = resultNumber
                    };
                }
            }
        }

        /// <summary>
        /// Permite subtrarir dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser subtraído.</param>
        /// <param name="right">O segundo número a ser subtraído.</param>
        /// <param name="maxPrecision">A precisão binária.</param>
        /// <returns>O resultado da subtracção.</returns>
        public static BigDecimalNumber Subtract(
            BigDecimalNumber left,
            BigDecimalNumber right,
            int maxPrecision = 1024)
        {
            if (left.number.IsZero)
            {
                return BigDecimalNumber.Negate(right);
            }
            else if (right.number.IsZero)
            {
                return left;
            }
            else if (left.exponent == right.exponent)
            {
                var resultNumber = BigInteger.Subtract(left.number, right.number);
                var currentExponent = left.exponent;

                var log = (int)integerPartLogAlg.Run(resultNumber);
                if (log > maxPrecision)
                {
                    var difference = log - maxPrecision + 1;
                    resultNumber = resultNumber >> difference;
                    currentExponent += difference;
                }

                var unity = BigInteger.One;
                while ((resultNumber & unity) == 0 && currentExponent > 0)
                {
                    ++currentExponent;
                    resultNumber = resultNumber >> 1;
                }

                return new BigDecimalNumber()
                {
                    exponent = currentExponent,
                    number = resultNumber
                };
            }
            else
            {
                var firstNumber = left;
                var secondNumber = right;
                if (left.exponent < right.exponent)
                {
                    firstNumber = right;
                    secondNumber = left;
                }

                var firstTemporaryNumber = BigInteger.Abs(firstNumber.number);
                var secondTemporaryNumber = BigInteger.Abs(secondNumber.number);
                var firstNumberSlack = maxPrecision - (int)integerPartLogAlg.Run(firstTemporaryNumber) - 1;
                var secondNumberSlack = maxPrecision - (int)integerPartLogAlg.Run(secondTemporaryNumber) - 1;

                if (firstNumber.exponent + secondNumber.exponent > firstNumberSlack + secondNumberSlack)
                {
                    return firstNumber;
                }
                else
                {
                    var currentExponent = secondNumber.exponent;
                    var differenceExponent = firstNumber.exponent - secondNumber.exponent;
                    if (differenceExponent < firstNumberSlack)
                    {
                        firstTemporaryNumber = firstNumber.number << differenceExponent;
                    }
                    else
                    {
                        firstTemporaryNumber = firstNumber.number << firstNumberSlack;
                        var slackDiff = differenceExponent - secondNumberSlack;
                        secondTemporaryNumber = secondNumber.number >> slackDiff;
                        currentExponent += slackDiff;
                    }

                    var resultNumber = BigInteger.Subtract(firstTemporaryNumber, secondTemporaryNumber);
                    var log = (int)integerPartLogAlg.Run(resultNumber);
                    if (log > maxPrecision)
                    {
                        var difference = log - maxPrecision + 1;
                        resultNumber = resultNumber >> difference;
                        currentExponent += difference;
                    }

                    var unity = BigInteger.One;
                    while ((resultNumber & unity) == 0 && currentExponent > 0)
                    {
                        ++currentExponent;
                        resultNumber = resultNumber >> 1;
                    }

                    return new BigDecimalNumber()
                    {
                        Exponent = currentExponent,
                        Number = resultNumber
                    };
                }
            }
        }

        /// <summary>
        /// Permite multiplicar dois números.
        /// </summary>
        /// <param name="left">O primeiro número a ser multiplicado.</param>
        /// <param name="right">O segundo número a ser multiplicado.</param>
        /// <param name="maxPrecision">A precisão binária associada ao número.</param>
        /// <returns>O resultado da multiplicação.</returns>
        public static BigDecimalNumber Multiply(
            BigDecimalNumber left,
            BigDecimalNumber right,
            int maxPrecision = 1024)
        {
            if (left.Number.IsZero || right.Number.IsZero)
            {
                return BigDecimalNumber.Zero;
            }
            else
            {
                var currentExponent = left.Exponent + right.Exponent;
                var currentResultNumber = BigInteger.Multiply(left.Number, right.Number);

                var log = (int)integerPartLogAlg.Run(currentResultNumber);
                if (log > maxPrecision)
                {
                    var difference = log - maxPrecision + 1;
                    currentResultNumber = currentResultNumber >> difference;
                    currentExponent += difference;
                }

                while ((currentResultNumber & 1) == 0 && currentExponent > 0)
                {
                    currentResultNumber = currentResultNumber >> 1;
                    ++currentExponent;
                }

                return new BigDecimalNumber()
                {
                    Exponent = currentExponent,
                    Number = currentResultNumber
                };
            }
        }

        /// <summary>
        /// Permite dividir dois números.
        /// </summary>
        /// <param name="left">O primeiro número  a ser dividido.</param>
        /// <param name="right">O segundo número a ser dividido.</param>
        /// <param name="maxPrecision">O valor da precisão máxima para a divisão.</param>
        /// <returns>O resultdao da divisão.</returns>
        /// <exception cref="DivideByZeroException">Se ocorrer alguma divisão por zero.</exception>
        public static BigDecimalNumber Divide(
            BigDecimalNumber left,
            BigDecimalNumber right,
            int maxPrecision)
        {
            if (right.number == 0)
            {
                throw new DivideByZeroException();
            }
            else if (left.number.IsZero)
            {
                return BigDecimalNumber.Zero;
            }
            else
            {
                var currentLeftNumber = BigInteger.Abs(left.number);
                var currentRightNumber = BigInteger.Abs(right.number);
                var differenceExponent = left.exponent - right.exponent;

                var divisorLog = (int)integerPartLogAlg.Run(currentRightNumber);
                if (currentLeftNumber < currentRightNumber)
                {
                    var remoLog = (int)integerPartLogAlg.Run(currentLeftNumber);
                    var lower = divisorLog - remoLog + 1;
                    currentLeftNumber = currentLeftNumber << lower;
                    differenceExponent -= lower;
                }

                // Pode ser que se esteja na presença de múltiplos.
                var currentExponent = 0;
                var remainder = default(BigInteger);
                currentLeftNumber = BigInteger.DivRem(currentLeftNumber, currentRightNumber, out remainder);
                if (remainder != 0)
                {
                    if (currentExponent < maxPrecision)
                    {
                        while (remainder != 0 && currentExponent < maxPrecision)
                        {
                            var remLog = (int)integerPartLogAlg.Run(remainder);
                            var lower = divisorLog - remLog + 1;
                            remainder = remainder << lower;
                            currentLeftNumber = currentLeftNumber << lower;
                            var quo = BigInteger.DivRem(remainder, currentRightNumber, out remainder);
                            currentLeftNumber += quo;
                            currentExponent += lower;
                        }

                        while (currentExponent >= maxPrecision)
                        {
                            currentLeftNumber = currentLeftNumber >> 1;
                            --currentExponent;
                        }
                    }
                }

                while ((currentLeftNumber & 1) == 0 && currentExponent > 0)
                {
                    currentLeftNumber = currentLeftNumber >> 1;
                    --currentExponent;
                }

                if (left.number < 0)
                {
                    currentLeftNumber = BigInteger.Negate(currentLeftNumber);
                }

                if (right.number < 0)
                {
                    currentLeftNumber = BigInteger.Negate(currentLeftNumber);
                }

                return new BigDecimalNumber()
                {
                    number = currentLeftNumber,
                    exponent = differenceExponent - currentExponent
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
        /// <exception cref="ArgumentOutOfRangeException">Se o valor da precisão for negativo.</exception>
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

        /// <summary>
        /// Determina se o objecto proporcionado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Verdadeiro caso o objecto seja igual e falso caso contrário.
        /// </returns>
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

        /// <summary>
        /// Retorna um código confuso para a instância corrente.
        /// </summary>
        /// <returns>
        /// O código confuso da instância corrente utilizado em alguns algoritmos.
        /// </returns>
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
                var numberRegex = new Regex(@"^\s*(-){0,1}(\d*)(?:\.(\d+)){0,1}\s*$");
                var matches = numberRegex.Match(text);
                if (matches.Success)
                {
                    var signal = false;
                    if (matches.Groups[1].Value.Contains("-"))
                    {
                        signal = true;
                    }

                    var currentNumber = BigInteger.Zero;
                    var matchText = matches.Groups[2].Value;
                    if (!string.IsNullOrWhiteSpace(matchText))
                    {
                        if (!BigInteger.TryParse(matchText, out currentNumber))
                        {
                            return false;
                        }
                    }

                    matchText = matches.Groups[3].Value.Trim().TrimEnd('0');
                    if (!string.IsNullOrWhiteSpace(matchText))
                    {
                        if (currentNumber.IsZero)
                        {
                            number = GetFromPureDecimalNumber(matchText, precision, signal);
                        }
                        else
                        {
                            number = GetFromCompleteNumber(currentNumber, matchText, precision, signal);
                        }
                    }
                    else if (!currentNumber.IsZero)
                    {
                        var log = (int)integerPartLogAlg.Run(currentNumber) + 1;
                        var currentExponent = 0;
                        if (log > precision)
                        {
                            var difference = log - precision;
                            currentNumber = currentNumber >> difference;
                            currentExponent -= difference;
                        }

                        if (signal)
                        {
                            currentNumber = -currentNumber;
                        }

                        number = new BigDecimalNumber()
                        {
                            number = currentNumber,
                            exponent = -currentExponent
                        };
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o valor a partir de uma mantissa.
        /// </summary>
        /// <param name="currentNumber">O número.</param>
        /// <param name="matchText">O valor da mantissa.</param>
        /// <param name="precision">A precisão.</param>
        /// <param name="signal">Se encontrou sinal.</param>
        /// <returns>O valor do número.</returns>
        private static BigDecimalNumber GetFromCompleteNumber(
            BigInteger currentNumber,
            string matchText,
            int precision,
            bool signal)
        {
            var currentNumberPrecision = (int)integerPartLogAlg.Run(currentNumber) + 1;
            if (currentNumberPrecision < precision)
            {
                var mantissa = BigInteger.Zero;
                var currentPrecision = 0;
                var currentExponent = 0;
                var innerPrecision = precision - currentNumberPrecision;
                while (!string.IsNullOrWhiteSpace(matchText) && currentPrecision < innerPrecision)
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

                    mantissa = mantissa << 1;
                    if (carry > 0)
                    {
                        ++mantissa;
                    }

                    ++currentExponent;
                    if (mantissa != 0)
                    {
                        currentPrecision = (int)integerPartLogAlg.Run(mantissa) + 1;
                    }

                    matchText = innerText;
                }

                currentNumber = (currentNumber << innerPrecision) +
                        (mantissa >> (currentExponent - innerPrecision));
                if ((currentNumber & 1) == 0)
                {
                    ++currentNumber;
                }

                currentExponent = innerPrecision;
                if (signal)
                {
                    currentNumber = -currentNumber;
                }

                return new BigDecimalNumber()
                {
                    number = currentNumber,
                    exponent = -currentExponent
                };
            }
            else if (currentNumberPrecision > precision)
            {
                var difference = currentNumberPrecision - precision;
                currentNumber = currentNumber >> difference;

                if (signal)
                {
                    currentNumber = -currentNumber;
                }

                return new BigDecimalNumber()
                {
                    number = currentNumber,
                    exponent = difference
                };
            }
            else
            {
                return new BigDecimalNumber()
                {
                    number = currentNumber,
                    exponent = 0
                };
            }
        }

        /// <summary>
        /// Efectua a leitura a partir de uma mantissa pura.
        /// </summary>
        /// <param name="matchText">A representação textual da mantissa.</param>
        /// <param name="precision">A precisão do número.</param>
        /// <param name="signal">Se encontrou sinal.</param>
        /// <returns>O valor do número lido.</returns>
        private static BigDecimalNumber GetFromPureDecimalNumber(
            string matchText,
            int precision,
            bool signal)
        {
            var mantissa = BigInteger.Zero;
            var currentPrecision = 0;
            var currentExponent = 0;
            var state = true;

            // Aumenta o expoente enquanto o valor for nulo.
            while (state)
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

                ++currentExponent;
                if (carry > 0)
                {
                    ++mantissa;
                    currentPrecision = 1;
                    state = false;
                }

                matchText = innerText;
            }

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

                mantissa = mantissa << 1;
                if (carry > 0)
                {
                    ++mantissa;
                }

                ++currentExponent;
                if (mantissa != 0)
                {
                    currentPrecision = (int)integerPartLogAlg.Run(mantissa) + 1;
                }

                matchText = innerText;
            }

            if (signal)
            {
                mantissa = -mantissa;
            }

            return new BigDecimalNumber()
            {
                number = mantissa,
                exponent = -currentExponent
            };
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
            var currentExponent = -this.exponent;
            if (currentExponent < precision)
            {
                return this.GetMixedDecimalNumberRepresentation(precision);
            }
            else
            {
                return this.GetPureDecimalNumberRepresentation(precision);
            }
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
        /// Obtém a representação textual de um número que contém parte inteira.
        /// </summary>
        /// <param name="precision">A precisão.</param>
        /// <returns>A representação textual do número.</returns>
        private string GetMixedDecimalNumberRepresentation(int precision)
        {
            var result = string.Empty;
            var tempNumber = this.number;
            if (this.number < 0)
            {
                tempNumber = -this.number;
                result += "-";
            }

            var currentExponent = -this.exponent;
            var tempNumberPrecision = (int)integerPartLogAlg.Run(tempNumber) + 1;
            var mantissaPrecision = Math.Min(currentExponent, precision);

            var mantissaMask = BigInteger.One << mantissaPrecision;
            var integerPart = tempNumber >> currentExponent;
            result += integerPart.ToString();
            var mantissaPart = tempNumber & (mantissaMask - 1);
            var mantissaPlaces = (uint)(integerPartLogAlg.Run(mantissaPart) + 1);
            result += ".";
            var decimalPlaces = -1;
            var decimalPrecision = (int)(precision * Math.Log10(2));
            if (!integerPart.IsZero)
            {
                decimalPrecision -= (int)((int)integerPartLogAlg.Run(integerPart) * Math.Log10(2));
            }

            while (mantissaPart != 0 && decimalPlaces < decimalPrecision)
            {
                var multiplied = (mantissaPart << 2) + mantissaPart;
                mantissaPlaces += 2;
                var mask = mantissaMask << 2;
                if ((multiplied & mask) != 0)
                {
                    ++mantissaPlaces;
                }

                --currentExponent;
                mantissaMask = mantissaMask >> 1;
                if (mantissaPlaces < currentExponent)
                {
                    result += "0";
                    mantissaPart = multiplied;
                }
                else
                {
                    mantissaPart = multiplied & (mantissaMask - 1);
                    var value = multiplied >> (int)currentExponent;
                    result += value;
                }

                ++decimalPlaces;
            }

            var temporary = result;
            result = string.Empty;
            var i = decimalPrecision + temporary.IndexOf('.') + 1;
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
        /// Otbém a representação textual de um número decimal sem parte inteira.
        /// </summary>
        /// <param name="precision">A precisão do número.</param>
        /// <returns>A representação textual do número.</returns>
        private string GetPureDecimalNumberRepresentation(int precision)
        {
            var tempNumber = this.number;
            var currentExponent = -this.exponent;

            // Estabelecimento das precisões.
            var log2 = Math.Log10(2);
            var decimalPrecision = (int)Math.Ceiling(precision * log2);
            var numberDecimalPlaces = decimalPrecision;

            var result = tempNumber.ToString();
            var decimalExponent = 0;
            while (currentExponent > 0)
            {
                var tempResult = result;
                result = string.Empty;
                var lastItem = tempResult.Length - 1;
                if (lastItem > 0)
                {
                    var currentValue = (int)char.GetNumericValue(tempResult[0]);
                    var quo = currentValue >> 1;
                    var rem = currentValue & 1;
                    if (quo == 0) // Inclui o valor caso seja diferente de zero.
                    {
                        --numberDecimalPlaces;
                    }
                    else
                    {
                        result += quo.ToString();
                    }

                    for (int i = 1; i < lastItem; ++i)
                    {
                        currentValue = (int)char.GetNumericValue(tempResult[i]);
                        if (rem == 1)
                        {
                            currentValue += 10;
                        }

                        quo = currentValue >> 1;
                        rem = currentValue & 1;
                        result += quo.ToString();
                    }

                    currentValue = (int)char.GetNumericValue(tempResult[lastItem]);
                    if (rem == 1)
                    {
                        currentValue += 10;
                    }

                    quo = currentValue >> 1;
                    rem = currentValue & 1;
                    result = result + quo.ToString();
                    if (rem == 1 && numberDecimalPlaces > 0)
                    {
                        --decimalExponent;
                        result += "5";
                    }
                }
                else
                {
                    var currentValue = (int)char.GetNumericValue(tempResult[lastItem]);
                    var quo = currentValue >> 1;
                    var rem = currentValue & 1;

                    quo = currentValue >> 1;
                    rem = currentValue & 1;
                    if (quo != 0)
                    {
                        result = result + quo.ToString();
                    }

                    if (rem == 1 && numberDecimalPlaces > 0)
                    {
                        --decimalExponent;
                        result += "5";
                    }
                }

                --currentExponent;
            }

            decimalExponent += result.Length - 1;
            if (decimalExponent > -decimalPrecision)
            {
                var prefix = string.Empty;
                while (decimalExponent < 0)
                {
                    prefix = prefix + "0";
                    ++decimalExponent;
                }

                result = prefix + result;
            }

            var j = decimalPrecision;
            if (result.Length > j)
            {
                var tempResult = result;
                result = string.Empty;
                var value = (int)char.GetNumericValue(tempResult[j]);
                var carry = true;
                if (value < 5)
                {
                    carry = false;
                }

                --j;
                while (j >= 0)
                {
                    var tempChar = tempResult[j--];
                    value = (int)char.GetNumericValue(tempChar);
                    if (carry)
                    {
                        ++value;
                    }

                    carry = false;
                    if (value == 10)
                    {
                        carry = true;
                        value = 0;
                    }

                    result = value + result;
                }

                while (j >= 0)
                {
                    var tempChar = tempResult[j--];
                    value = (int)char.GetNumericValue(tempChar);
                    if (carry)
                    {
                        ++value;
                    }

                    carry = false;
                    if (value == 10)
                    {
                        carry = true;
                        value = 0;
                    }

                    result = value + result;
                }
            }

            result = result.Insert(1, ".");
            result = result.TrimEnd('0');
            if (decimalExponent != 0)
            {
                result += string.Format("E{0}", decimalExponent);
            }

            if (this.number < 0)
            {
                tempNumber = -this.number;
                result = "-" + result;
            }

            return result;
        }
    }
}
