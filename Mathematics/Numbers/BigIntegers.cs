namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Mathematics;
    using Utilities.Collections;

    /// <summary>
    /// Representa um número inteiro grande como sendo um vector de inteiros longos
    /// sem sinal em base 2^64.
    /// </summary>
    public struct UlongArrayBigInt
    {
        /// <summary>
        /// Mantém o registo de todas as potências de dez suportadas por uma variável do tipo longo sem sinal.
        /// </summary>
        private static ulong[] tenthPowers = new ulong[]
        {
            10ul,
            100ul,
            1000ul,
            10000ul,
            100000ul,
            1000000ul,
            10000000ul,
            100000000ul,
            1000000000ul,
            10000000000ul,
            100000000000ul,
            1000000000000ul,
            10000000000000ul,
            100000000000000ul,
            1000000000000000ul,
            10000000000000000ul,
            100000000000000000ul,
            1000000000000000000ul,
            10000000000000000000ul
        };

        /// <summary>
        /// Verdadeiro caso o número seja afecto de sinal e falso caso contrário.
        /// </summary>
        private bool sign;

        /// <summary>
        /// O vector que contém a representação do inteiro muito grande com base em longos sem sinal.
        /// </summary>
        private ulong[] array;

        #region Construtores

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="array">O vector de valores que inicializam o número.</param>
        public UlongArrayBigInt(ulong[] array)
        {
            this.sign = false;
            if (array == null || array.Length == 0)
            {
                this.array = null;
            }
            else
            {
                var length = array.Length;
                this.array = new ulong[length];
                Array.Copy(array, this.array, length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="list"></param>
        public UlongArrayBigInt(List<ulong> list)
        {
            if (list == null || list.Count == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = false;
                this.array = list.ToArray();
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="sign">O sinal do número.</param>
        /// <param name="array">O vector de valores que inicializam o número.</param>
        public UlongArrayBigInt(bool sign, ulong[] array)
        {
            this.sign = sign;
            if (array == null || array.Length == 0)
            {
                this.array = null;
            }
            else
            {
                var length = array.Length;
                this.array = new ulong[length];
                Array.Copy(array, this.array, length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="sign">O sinal do número.</param>
        /// <param name="list"></param>
        public UlongArrayBigInt(bool sign, List<ulong> list)
        {
            this.sign = sign;
            if (list == null || list.Count == 0)
            {
                this.array = null;
            }
            else
            {
                this.array = list.ToArray();
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UlongArrayBigInt(int numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = numb < 0;
                this.array = new ulong[] { (ulong)Math.Abs(numb) };
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UlongArrayBigInt(uint numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = false;
                this.array = new ulong[] { (ulong)numb };
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UlongArrayBigInt(long numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = numb < 0;
                this.array = new ulong[] { (ulong)Math.Abs(numb) };
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UlongArrayBigInt(ulong numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = false;
                this.array = new ulong[] { numb };
            }
        }

        #endregion Construtores

        /// <summary>
        /// Obtém um valor que indica se o número está afecto do sinal.
        /// </summary>
        public bool Sign
        {
            get
            {
                return this.sign;
            }
        }

        #region Funções públicas

        /// <summary>
        /// Converte o inteiro actual num vector de "bytes".
        /// </summary>
        /// <returns>O vector.</returns>
        public byte[] ToByteArray()
        {
            var byteList = new List<byte>();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converte o inteiro actual num vector de longos sem sinal.
        /// </summary>
        /// <returns></returns>
        public ulong[] ToUnsignedLongArray()
        {
            var length = this.array.Length;
            var result = new ulong[length];
            Array.Copy(this.array, result, length);
            return result;
        }

        /// <summary>
        /// Obtém a representação no formato no qual estes números são
        /// representados no <see cref="System.Numerics.BigInteger"/>.
        /// </summary>
        /// <returns>O inteiro enorme.</returns>
        public BigInteger ToBigint()
        {
            // TODO: Melhorar o processo a partir da construção directa de um vector de "bytes"
            if (this.array == null || this.array.Length == 0)
            {
                return BigInteger.Zero;
            }
            else
            {
                var pow = BigInteger.One << 64;
                var i = this.array.Length - 1;
                var result = new BigInteger(this.array[i]);
                if (sign)
                {
                    result = BigInteger.Subtract(0, result);
                }

                for (; i >= 0; --i)
                {
                    result = BigInteger.Multiply(result, pow);
                    result = BigInteger.Add(result, this.array[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// Determina se o objecto proporcionado é igual ao corrente.
        /// </summary>
        /// <param name="obj">O objecto a ser comparado.</param>
        /// <returns>Verdadeiro caso ambos os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            if (obj is UlongArrayBigInt)
            {
                return this == (UlongArrayBigInt)obj;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determina um código confuso para o número enorme.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var result = typeof(UlongArrayBigInt).GetHashCode();
            if (this.array != null)
            {
                result ^= this.sign.GetHashCode();
                var length = this.array.Length;
                for (int i = 0; i < length; ++i)
                {
                    result ^= (19 * this.array[i] + 17u).GetHashCode();
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém uma representação textual do número.
        /// </summary>
        /// <returns>A representação textual do número.</returns>
        public override string ToString()
        {
            if (this.array == null || this.array.Length == 0)
            {
                return "0";
            }
            else
            {
                var index = this.array.Length - 1;
                var current = this.array[index];
                while (current == 0 && index >= 0)
                {
                    current = this.array[index];
                    --index;
                }

                if (index < 0)
                {
                    return "0";
                }
                else
                {
                    var decimalRep = new List<ulong>() { 1 };

                    // Determina a posição do primeiro "bit" não nulo
                    var pos = MathFunctions.GetHighestSettedBitIndex(current);
                    current = current << (64 - pos);
                    for (int i = 0; i < pos; ++i)
                    {
                        this.DuplicateDecimalRep(decimalRep);
                        if ((current & 0x8000000000000000) == 0x8000000000000000)
                        {
                            this.IncrementDecimalRep(decimalRep);
                        }

                        current = current << 1;
                    }

                    --index;
                    for (; index >= 0; --index)
                    {
                        current = this.array[index];
                        for (int i = 0; i < 64; ++i)
                        {
                            this.DuplicateDecimalRep(decimalRep);
                            if ((current & 0x8000000000000000) == 0x8000000000000000)
                            {
                                this.IncrementDecimalRep(decimalRep);
                            }

                            current = current << 1;
                        }
                    }

                    // Imprime o resultado
                    var result = this.GetDecimalRepresentation(decimalRep);
                    if (this.sign)
                    {
                        result = "-" + result;
                    }

                    return result;
                }
            }
        }

        #endregion Funções públicas

        #region Sobrecarga de operadores

        /// <summary>
        /// Sobrecarrega o operador de adição.
        /// </summary>
        /// <remarks>
        /// O operador de adição utiliza a versão sequencial e não a paralela.
        /// </remarks>
        /// <param name="first">O primeiro valor a ser somado.</param>
        /// <param name="second">O segundo valor a ser somado.</param>
        /// <returns>O resultado da soma.</returns>
        public static UlongArrayBigInt operator +(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            return SequentialAdd(first, second);
        }

        /// <summary>
        /// Sonrecarrega o operador de multiplicação.
        /// </summary>
        /// <param name="first">O primeiro valor a ser multiplicado.</param>
        /// <param name="second">O segundo valor a ser multiplicado.</param>
        /// <returns>O resultado do produto.</returns>
        public static UlongArrayBigInt operator *(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sobrecarrega o operador de subtracção.
        /// </summary>
        /// <param name="first">O valor do minuendo.</param>
        /// <param name="second">O valor do subtraendo.</param>
        /// <returns>O resultado da subtracção.</returns>
        public static UlongArrayBigInt operator -(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            return SequentialSubtract(first, second);
        }

        /// <summary>
        /// Determina o quociente inteiro entre dois números.
        /// </summary>
        /// <param name="first">O valor do dividendo.</param>
        /// <param name="second">O valor do divisor.</param>
        /// <returns>O valor do quociente entre os dois números.</returns>
        public static UlongArrayBigInt operator /(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina o resto da divisão inteira entre dois números.
        /// </summary>
        /// <param name="first">O valor do dividendo.</param>
        /// <param name="second">O valor do divisor.</param>
        /// <returns>O resto da divisão.</returns>
        public static UlongArrayBigInt operator %(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sobrecarrega o operador de igualdade entre os operandos.
        /// </summary>
        /// <param name="first">O primeiro operando.</param>
        /// <param name="second">O segundo operando.</param>
        /// <returns>Verdadeiro caso os operados sejam iguais e falso caso contrário.</returns>
        public static bool operator ==(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return true;
            }
            else
            {
                var firstArray = first.array;
                var secondArray = second.array;
                if (firstArray == null && secondArray == null)
                {
                    return true;
                }
                else if (firstArray == null)
                {
                    return false;
                }
                else if (secondArray == null)
                {
                    return false;
                }
                else
                {
                    var firstArrayLength = firstArray.Length;
                    var secondArrayLength = secondArray.Length;
                    if (firstArrayLength == secondArrayLength)
                    {
                        if (first.sign == second.sign)
                        {
                            for (int i = 0; i < firstArrayLength; ++i)
                            {
                                if (firstArray[i] != secondArray[i])
                                {
                                    return false;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Sobrecarrega o operador de diferença entre os operandos.
        /// </summary>
        /// <param name="first">O primeiro operando.</param>
        /// <param name="second">O segundo operando.</param>
        /// <returns>Verdadeiro caso os operados sejam iguais e falso caso contrário.</returns>
        public static bool operator !=(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (object.ReferenceEquals(first, second))
            {
                return false;
            }
            else
            {
                var firstArray = first.array;
                var secondArray = second.array;
                if (firstArray == null && secondArray == null)
                {
                    return false;
                }
                else if (firstArray == null)
                {
                    return true;
                }
                else if (secondArray == null)
                {
                    return true;
                }
                else
                {
                    var firstArrayLength = firstArray.Length;
                    var secondArrayLength = secondArray.Length;
                    if (firstArrayLength == secondArrayLength)
                    {
                        if (first.sign == second.sign)
                        {
                            for (int i = 0; i < firstArrayLength; ++i)
                            {
                                if (firstArray[i] == secondArray[i])
                                {
                                    return false;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Sobrecarrega o operador de menor.
        /// </summary>
        /// <param name="first">O primeiro argumento do operador.</param>
        /// <param name="second">O segundo argumento do operador.</param>
        /// <returns>Verdadeiro caso o primeiro argumento seja menor que o segundo e falso caso contrário.</returns>
        public static bool operator <(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (first.array == null && second.array == null)
            {
                return false;
            }
            else if (!(first.sign || second.sign))
            {
                var firstArray = first.array;
                var secondArray = second.array;
                if (firstArray == null)
                {
                    return true;
                }
                else if (secondArray == null)
                {
                    return false;
                }
                else
                {
                    var firstArrayLength = firstArray.Length;
                    var secondArrayLength = secondArray.Length;
                    if (firstArrayLength < secondArrayLength)
                    {
                        return true;
                    }
                    else if (firstArrayLength == secondArrayLength)
                    {
                        if (firstArrayLength == 0)
                        {
                            return false;
                        }
                        else
                        {
                            --firstArrayLength;
                            for (; firstArrayLength > -1; --firstArrayLength)
                            {
                                if (firstArray[firstArrayLength] < secondArray[firstArrayLength])
                                {
                                    return true;
                                }
                            }

                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (first.sign && second.sign)
            {
                var firstArray = first.array;
                var secondArray = second.array;
                if (firstArray == null)
                {
                    return false;
                }
                else if (secondArray == null)
                {
                    return true;
                }
                else
                {
                    var firstArrayLength = firstArray.Length;
                    var secondArrayLength = secondArray.Length;
                    if (firstArrayLength < secondArrayLength)
                    {
                        return false;
                    }
                    else if (firstArrayLength == secondArrayLength)
                    {
                        if (firstArrayLength == 0)
                        {
                            return true;
                        }
                        else
                        {
                            --firstArrayLength;
                            for (; firstArrayLength > -1; --firstArrayLength)
                            {
                                if (firstArray[firstArrayLength] < secondArray[firstArrayLength])
                                {
                                    return true;
                                }
                            }

                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (first.sign)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Sobrecarrega o operador de maior.
        /// </summary>
        /// <param name="first">O primeiro argumento do operador.</param>
        /// <param name="second">O segundo argumento do operador.</param>
        /// <returns>Verdadeiro caso o primeiro argumento seja menor que o segundo e falso caso contrário.</returns>
        public static bool operator >(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (first.array == null && second.array == null)
            {
                return false;
            }
            else if (!(first.sign || second.sign))
            {
                var firstArray = first.array;
                var secondArray = second.array;
                if (firstArray == null)
                {
                    return false;
                }
                else if (secondArray == null)
                {
                    return true;
                }
                else
                {
                    var firstArrayLength = firstArray.Length;
                    var secondArrayLength = secondArray.Length;
                    if (firstArrayLength > secondArrayLength)
                    {
                        return true;
                    }
                    else if (firstArrayLength == secondArrayLength)
                    {
                        --firstArrayLength;
                        for (; firstArrayLength > -1; --firstArrayLength)
                        {
                            if (firstArray[firstArrayLength] > secondArray[firstArrayLength])
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (first.sign && second.sign)
            {
                var firstArray = first.array;
                var secondArray = second.array;
                if (firstArray == null)
                {
                    return true;
                }
                else if (secondArray == null)
                {
                    return false;
                }
                else
                {
                    var firstArrayLength = firstArray.Length;
                    var secondArrayLength = secondArray.Length;
                    if (firstArrayLength > secondArrayLength)
                    {
                        return false;
                    }
                    else if (firstArrayLength == secondArrayLength)
                    {
                        --firstArrayLength;
                        for (; firstArrayLength > -1; --firstArrayLength)
                        {
                            if (firstArray[firstArrayLength] > secondArray[firstArrayLength])
                            {
                                return true;
                            }
                        }

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else if (first.sign)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Sobrecarrega o operador de rotação à direita.
        /// </summary>
        /// <param name="first">O valor a ser rodado.</param>
        /// <param name="second">O tamanho do deslocamento.</param>
        /// <returns>O número resultante da rotação.</returns>
        public static UlongArrayBigInt operator >>(UlongArrayBigInt first, int second)
        {
            var result = new UlongArrayBigInt();
            result.sign = first.sign;
            result.array = RotateRight(first.array, second);
            return result;
        }

        /// <summary>
        /// Sobrecarrega o operador de rotação à esquerda.
        /// </summary>
        /// <param name="first">O valor a ser rodado.</param>
        /// <param name="second">O tamanho do deslocamento.</param>
        /// <returns>O número resultante da rotação.</returns>
        public static UlongArrayBigInt operator <<(UlongArrayBigInt first, int second)
        {
            var result = new UlongArrayBigInt();
            result.sign = first.sign;
            result.array = RotateLeft(first.array, second);
            return result;
        }

        #endregion Sobrecarga de operadores

        #region Funções estáticas públicas

        /// <summary>
        /// Tenta realizar a leitura de um número inteiro enorme a partir da sua representação texual caso
        /// esta seja uma representação correcta.
        /// </summary>
        /// <param name="text">A representação textual do número.</param>
        /// <param name="value">Recebe o valor lido em caso de sucesso.</param>
        /// <returns>Verdadeiro caso a função seja bem-sucedida e falso caso contrário.</returns>
        public static bool TryParse(string text, out UlongArrayBigInt value)
        {
            if (text == null)
            {
                value = default(UlongArrayBigInt);
                return false;
            }
            else
            {
                var integerExpression = new Regex("^\\s*(-{0,1})(0*)(\\d+)\\s*$");
                var match = integerExpression.Match(text);
                if (match.Success)
                {
                    var sign = false;
                    var readed = new List<ulong>();

                    var innerText = match.Groups[1].Value;
                    if (innerText == "-")
                    {
                        sign = true;
                    }

                    // Trata os valores
                    innerText = match.Groups[3].Value;
                    if (innerText == "0")
                    {
                        value = new UlongArrayBigInt();
                        return true;
                    }
                    else if (innerText.Length < 20)
                    {
                        var parsed = ulong.Parse(innerText);
                        value = new UlongArrayBigInt();
                        value.sign = sign;
                        value.array = new[] { parsed };
                        return true;
                    }
                    else
                    {
                        var step = 19;
                        var index = 0;
                        var length = innerText.Length;
                        var parsing = innerText.Substring(index, step);
                        var parsed = ulong.Parse(parsing);
                        var result = new List<ulong>() { parsed };
                        index = 19;
                        while (index < length)
                        {
                            var nextIndex = index + step;
                            var len = step;
                            if (nextIndex > length)
                            {
                                nextIndex = length;
                                len = nextIndex - index;
                            }

                            parsing = innerText.Substring(index, len);
                            parsed = ulong.Parse(parsing);

                            Multiply(result, tenthPowers[len - 1]);
                            Add(result, parsed);

                            index = nextIndex;
                        }

                        value = new UlongArrayBigInt();
                        value.sign = sign;
                        value.array = result.ToArray();
                        return true;
                    }
                }
                else
                {
                    value = default(UlongArrayBigInt);
                    return false;
                }
            }
        }

        /// <summary>
        /// Tenta realizar a leitura de um número inteiro enorme a partir da sua representação texual caso
        /// esta seja uma representação correcta.
        /// </summary>
        /// <param name="text">A representação textual do número.</param>
        /// <param name="value">Recebe o valor lido em caso de sucesso.</param>
        /// <returns>Verdadeiro caso a função seja bem-sucedida e falso caso contrário.</returns>
        [Obsolete("Deprecated: function is of slow execution and shoud serve for learning purposes.")]
        public static bool TryParse59(string text, out UlongArrayBigInt value)
        {
            if (text == null)
            {
                value = default(UlongArrayBigInt);
                return false;
            }
            else
            {
                var integerExpression = new Regex("^\\s*(-{0,1})(0*)(\\d+)\\s*$");
                var match = integerExpression.Match(text);
                if (match.Success)
                {
                    var sign = false;
                    var readed = new List<ulong>();

                    var innerText = match.Groups[1].Value;
                    if (innerText == "-")
                    {
                        sign = true;
                    }

                    // Trata os valores
                    innerText = match.Groups[3].Value;
                    if (innerText == "0")
                    {
                        value = new UlongArrayBigInt();
                        return true;
                    }
                    else
                    {
                        var currentRes = DivideByBase59(innerText);

                        readed.Add(currentRes.Item2);
                        var alignement = 0;
                        while (currentRes.Item1 != "0")
                        {
                            alignement += 5;
                            alignement %= 64;

                            currentRes = DivideByBase59(currentRes.Item1);
                            if (alignement == 0)
                            {
                                readed.Add(currentRes.Item2);
                            }
                            else
                            {
                                AppendUlong(readed, currentRes.Item2, alignement);
                            }
                        }

                        value = new UlongArrayBigInt();
                        value.sign = sign;
                        value.array = readed.ToArray();
                        return true;
                    }
                }
                else
                {
                    value = default(UlongArrayBigInt);
                    return false;
                }
            }
        }

        /// <summary>
        /// Tenta realizar a leitura de um número inteiro enorme a partir da sua representação texual caso
        /// esta seja uma representação correcta.
        /// </summary>
        /// <param name="text">A representação textual do número.</param>
        /// <param name="value">Recebe o valor lido em caso de sucesso.</param>
        /// <returns>Verdadeiro caso a função seja bem-sucedida e falso caso contrário.</returns>
        [Obsolete("Deprecated: function is of slow execution and shoud serve for learning purposes.")]
        public static bool TryParse1(string text, out UlongArrayBigInt value)
        {
            if (text == null)
            {
                value = default(UlongArrayBigInt);
                return false;
            }
            else
            {
                var integerExpression = new Regex("^\\s*(-{0,1})(0*)(\\d+)\\s*$");
                var match = integerExpression.Match(text);
                if (match.Success)
                {
                    var sign = false;
                    var readed = new List<ulong>();

                    var innerText = match.Groups[1].Value;
                    if (innerText == "-")
                    {
                        sign = true;
                    }

                    // Trata os valores
                    innerText = match.Groups[3].Value;
                    if (innerText == "0")
                    {
                        value = new UlongArrayBigInt();
                        return true;
                    }
                    else
                    {
                        var currentRes = DivideByBase1(innerText);

                        readed.Add(currentRes.Item2);
                        var alignement = 0;
                        while (currentRes.Item1 != "0")
                        {
                            ++alignement;
                            alignement %= 64;

                            currentRes = DivideByBase1(currentRes.Item1);
                            if (alignement == 0)
                            {
                                readed.Add(currentRes.Item2);
                            }
                            else
                            {
                                AppendUlong(readed, currentRes.Item2, alignement);
                            }
                        }

                        value = new UlongArrayBigInt();
                        value.sign = sign;
                        value.array = readed.ToArray();
                        return true;
                    }
                }
                else
                {
                    value = default(UlongArrayBigInt);
                    return false;
                }
            }
        }

        /// <summary>
        /// Determina a soma de dois números enormes utilizando o algoritmo sequencial habitual.
        /// </summary>
        /// <param name="first">O primeiro número a ser adicionado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>O resultado da soma.</returns>
        public static UlongArrayBigInt SequentialAdd(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (first.sign && second.sign)
            {
                var result = new UlongArrayBigInt();
                result.sign = true;
                result.array = SequentialAdd(first.array, second.array);
                return result;
            }
            else if (first.sign)
            {
                var firstLength = first.array.Length;
                if (second.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = new ulong[firstLength];
                    Array.Copy(first.array, result.array, firstLength);
                    return result;
                }
                else
                {
                    var secondLength = second.array.Length;
                    if (firstLength < secondLength)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = SequentialSubtract(second.array, first.array);
                        return result;
                    }
                    else if (firstLength == secondLength)
                    {
                        var last = firstLength - 1;
                        var compare = 0;
                        for (int i = last; i > -1; --i)
                        {
                            var firstCurrent = first.array[i];
                            var secondCurrent = second.array[i];
                            if (firstCurrent < secondCurrent)
                            {
                                compare = -1;
                                i = -1;
                            }
                            else if (firstCurrent > secondCurrent)
                            {
                                compare = 1;
                                i = -1;
                            }
                        }

                        // A comparação encontra-se realizada neste ponto
                        if (compare < 0)
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = false;
                            result.array = SequentialSubtract(second.array, first.array);
                            return result;
                        }
                        else if (compare > 0)
                        {
                            return new UlongArrayBigInt();
                        }
                        else
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = true;
                            result.array = SequentialSubtract(first.array, second.array);
                            return result;
                        }
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = SequentialSubtract(first.array, second.array);
                        return result;
                    }
                }
            }
            else if (second.sign)
            {
                var secondLength = second.array.Length;
                if (first.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = new ulong[secondLength];
                    Array.Copy(second.array, result.array, secondLength);
                    return result;
                }
                else
                {
                    var firstLength = first.array.Length;
                    if (firstLength < secondLength)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = SequentialSubtract(second.array, first.array);
                        return result;
                    }
                    else if (firstLength == secondLength)
                    {
                        var last = firstLength - 1;
                        var compare = 0;
                        for (int i = last; i > -1; --i)
                        {
                            var firstCurrent = first.array[i];
                            var secondCurrent = second.array[i];
                            if (firstCurrent < secondCurrent)
                            {
                                compare = -1;
                                i = -1;
                            }
                            else if (firstCurrent > secondCurrent)
                            {
                                compare = 1;
                                i = -1;
                            }
                        }

                        // A comparação encontra-se realizada neste ponto
                        if (compare < 0)
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = true;
                            result.array = SequentialSubtract(second.array, first.array);
                            return result;
                        }
                        else if (compare > 0)
                        {
                            return new UlongArrayBigInt();
                        }
                        else
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = false;
                            result.array = SequentialSubtract(first.array, second.array);
                            return result;
                        }
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = SequentialSubtract(first.array, second.array);
                        return result;
                    }
                }
            }
            else
            {
                var result = new UlongArrayBigInt();
                result.sign = false;
                result.array = SequentialAdd(first.array, second.array);
                return result;
            }
        }

        /// <summary>
        /// Determina a soma de dois números, aplicando o algoritmo paralelo CLA (transporte e propagação).
        /// </summary>
        /// <param name="first">O primeiro número a ser adicionado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>O resultado da soma.</returns>
        public static UlongArrayBigInt ParallelClaAdd(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (first.sign && second.sign)
            {
                var result = new UlongArrayBigInt();
                result.sign = true;
                result.array = ParallelClaAdd(first.array, second.array);
                return result;
            }
            else if (first.sign)
            {
                var firstLength = first.array.Length;
                if (second.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = new ulong[firstLength];
                    Parallel.For(0, firstLength, i =>
                    {
                        result.array[i] = first.array[i];
                    });

                    return result;
                }
                else
                {
                    var secondLength = second.array.Length;
                    if (firstLength < secondLength)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = ParallelClaSubtract(second.array, first.array);
                        return result;
                    }
                    else if (firstLength == secondLength)
                    {
                        var last = firstLength - 1;
                        var compare = 0;
                        for (int i = last; i > -1; --i)
                        {
                            var firstCurrent = first.array[i];
                            var secondCurrent = second.array[i];
                            if (firstCurrent < secondCurrent)
                            {
                                compare = -1;
                                i = -1;
                            }
                            else if (firstCurrent > secondCurrent)
                            {
                                compare = 1;
                                i = -1;
                            }
                        }

                        // A comparação encontra-se realizada neste ponto
                        if (compare < 0)
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = false;
                            result.array = ParallelClaSubtract(second.array, first.array);
                            return result;
                        }
                        else if (compare > 0)
                        {
                            return new UlongArrayBigInt();
                        }
                        else
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = true;
                            result.array = ParallelClaSubtract(first.array, second.array);
                            return result;
                        }
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = ParallelClaSubtract(first.array, second.array);
                        return result;
                    }
                }
            }
            else if (second.sign)
            {
                var secondLength = second.array.Length;
                if (first.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = new ulong[secondLength];
                    Parallel.For(0, secondLength, i =>
                    {
                        result.array[i] = first.array[i];
                    });

                    return result;
                }
                else
                {
                    var firstLength = first.array.Length;
                    if (firstLength < secondLength)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = ParallelClaSubtract(second.array, first.array);
                        return result;
                    }
                    else if (firstLength == secondLength)
                    {
                        var last = firstLength - 1;
                        var compare = 0;
                        for (int i = last; i > -1; --i)
                        {
                            var firstCurrent = first.array[i];
                            var secondCurrent = second.array[i];
                            if (firstCurrent < secondCurrent)
                            {
                                compare = -1;
                                i = -1;
                            }
                            else if (firstCurrent > secondCurrent)
                            {
                                compare = 1;
                                i = -1;
                            }
                        }

                        // A comparação encontra-se realizada neste ponto
                        if (compare < 0)
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = true;
                            result.array = ParallelClaSubtract(second.array, first.array);
                            return result;
                        }
                        else if (compare > 0)
                        {
                            return new UlongArrayBigInt();
                        }
                        else
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = false;
                            result.array = ParallelClaSubtract(first.array, second.array);
                            return result;
                        }
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = ParallelClaSubtract(first.array, second.array);
                        return result;
                    }
                }
            }
            else
            {
                var result = new UlongArrayBigInt();
                result.sign = false;
                result.array = ParallelClaAdd(first.array, second.array);
                return result;
            }
        }

        /// <summary>
        /// Determina a diferença entre dois números.
        /// </summary>
        /// <param name="first">O minuendo.</param>
        /// <param name="second">O subtraendo.</param>
        /// <returns>A diferença.</returns>
        public static UlongArrayBigInt SequentialSubtract(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (first.sign && second.sign)
            {
                var firstLength = first.array.Length;
                var secondLength = second.array.Length;
                if (firstLength < secondLength)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = false;
                    result.array = SequentialSubtract(second.array, first.array);
                    return result;
                }
                else if (firstLength == secondLength)
                {
                    var last = firstLength - 1;
                    var compare = 0;
                    for (int i = last; i > -1; --i)
                    {
                        var firstCurrent = first.array[i];
                        var secondCurrent = second.array[i];
                        if (firstCurrent < secondCurrent)
                        {
                            compare = -1;
                            i = -1;
                        }
                        else if (firstCurrent > secondCurrent)
                        {
                            compare = 1;
                            i = -1;
                        }
                    }

                    // A comparação encontra-se realizada neste ponto
                    if (compare < 0)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = SequentialSubtract(second.array, first.array);
                        return result;
                    }
                    else if (compare == 0)
                    {
                        return new UlongArrayBigInt();
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = SequentialSubtract(first.array, second.array);
                        return result;
                    }
                }
                else
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = SequentialSubtract(first.array, second.array);
                    return result;
                }
            }
            else if (first.sign)
            {
                if (second.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    var length = first.array.Length;
                    result.array = new ulong[length];
                    Array.Copy(first.array, result.array, length);
                    return result;
                }
                else
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = SequentialAdd(first.array, second.array);
                    return result;
                }
            }
            else if (second.sign)
            {
                if (first.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = false;
                    var length = second.array.Length;
                    result.array = new ulong[length];
                    Array.Copy(second.array, result.array, length);
                    return result;
                }
                else
                {
                    var result = new UlongArrayBigInt();
                    result.sign = false;
                    result.array = SequentialAdd(first.array, second.array);
                    return result;
                }
            }
            else
            {
                if (first.array == null && second.array == null)
                {
                    return new UlongArrayBigInt();
                }
                else if (first.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    var length = second.array.Length;
                    result.array = new ulong[length];
                    Array.Copy(second.array, result.array, length);
                    return result;
                }
                else if (second.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    var length = first.array.Length;
                    result.array = new ulong[length];
                    Array.Copy(first.array, result.array, length);
                    return result;
                }
                else
                {
                    var firstLength = first.array.Length;
                    var secondLength = second.array.Length;
                    if (firstLength < secondLength)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = SequentialSubtract(second.array, first.array);
                        return result;
                    }
                    else if (firstLength == secondLength)
                    {
                        var last = firstLength - 1;
                        var compare = 0;
                        for (int i = last; i > -1; --i)
                        {
                            var firstCurrent = first.array[i];
                            var secondCurrent = second.array[i];
                            if (firstCurrent < secondCurrent)
                            {
                                compare = -1;
                                i = -1;
                            }
                            else if (firstCurrent > secondCurrent)
                            {
                                compare = 1;
                                i = -1;
                            }
                        }

                        // A comparação encontra-se realizada neste ponto
                        if (compare < 0)
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = true;
                            result.array = SequentialSubtract(second.array, first.array);
                            return result;
                        }
                        else if (compare == 0)
                        {
                            return new UlongArrayBigInt();
                        }
                        else
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = false;
                            result.array = SequentialSubtract(first.array, second.array);
                            return result;
                        }
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = SequentialSubtract(first.array, second.array);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Determina a diferença entre dois números, recorrendo ao algoritmo CLA.
        /// </summary>
        /// <param name="first">O minuendo.</param>
        /// <param name="second">O subtraendo.</param>
        /// <returns>A diferença.</returns>
        public static UlongArrayBigInt ParallelClaSubtract(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (first.sign && second.sign)
            {
                var firstLength = first.array.Length;
                var secondLength = second.array.Length;
                if (firstLength < secondLength)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = false;
                    result.array = ParallelClaSubtract(second.array, first.array);
                    return result;
                }
                else if (firstLength == secondLength)
                {
                    var last = firstLength - 1;
                    var compare = 0;
                    for (int i = last; i > -1; --i)
                    {
                        var firstCurrent = first.array[i];
                        var secondCurrent = second.array[i];
                        if (firstCurrent < secondCurrent)
                        {
                            compare = -1;
                            i = -1;
                        }
                        else if (firstCurrent > secondCurrent)
                        {
                            compare = 1;
                            i = -1;
                        }
                    }

                    // A comparação encontra-se realizada neste ponto
                    if (compare < 0)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = ParallelClaSubtract(second.array, first.array);
                        return result;
                    }
                    else if (compare == 0)
                    {
                        return new UlongArrayBigInt();
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = ParallelClaSubtract(first.array, second.array);
                        return result;
                    }
                }
                else
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = ParallelClaSubtract(first.array, second.array);
                    return result;
                }
            }
            else if (first.sign)
            {
                if (second.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    var length = first.array.Length;
                    result.array = new ulong[length];
                    Parallel.For(0, length, i =>
                    {
                        result.array[i] = first.array[i];
                    });

                    return result;
                }
                else
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    result.array = ParallelClaAdd(first.array, second.array);
                    return result;
                }
            }
            else if (second.sign)
            {
                if (first.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = false;
                    var length = second.array.Length;
                    result.array = new ulong[length];
                    Parallel.For(0, length, i =>
                    {
                        result.array[i] = second.array[i];
                    });

                    return result;
                }
                else
                {
                    var result = new UlongArrayBigInt();
                    result.sign = false;
                    result.array = ParallelClaAdd(first.array, second.array);
                    return result;
                }
            }
            else
            {
                if (first.array == null && second.array == null)
                {
                    return new UlongArrayBigInt();
                }
                else if (first.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    var length = second.array.Length;
                    result.array = new ulong[length];
                    Parallel.For(0, length, i =>
                    {
                        result.array[i] = second.array[i];
                    });

                    return result;
                }
                else if (second.array == null)
                {
                    var result = new UlongArrayBigInt();
                    result.sign = true;
                    var length = first.array.Length;
                    result.array = new ulong[length];
                    Parallel.For(0, length, i =>
                    {
                        result.array[i] = first.array[i];
                    });

                    return result;
                }
                else
                {
                    var firstLength = first.array.Length;
                    var secondLength = second.array.Length;
                    if (firstLength < secondLength)
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = true;
                        result.array = ParallelClaSubtract(second.array, first.array);
                        return result;
                    }
                    else if (firstLength == secondLength)
                    {
                        var last = firstLength - 1;
                        var compare = 0;
                        for (int i = last; i > -1; --i)
                        {
                            var firstCurrent = first.array[i];
                            var secondCurrent = second.array[i];
                            if (firstCurrent < secondCurrent)
                            {
                                compare = -1;
                                i = -1;
                            }
                            else if (firstCurrent > secondCurrent)
                            {
                                compare = 1;
                                i = -1;
                            }
                        }

                        // A comparação encontra-se realizada neste ponto
                        if (compare < 0)
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = true;
                            result.array = ParallelClaSubtract(second.array, first.array);
                            return result;
                        }
                        else if (compare == 0)
                        {
                            return new UlongArrayBigInt();
                        }
                        else
                        {
                            var result = new UlongArrayBigInt();
                            result.sign = false;
                            result.array = ParallelClaSubtract(first.array, second.array);
                            return result;
                        }
                    }
                    else
                    {
                        var result = new UlongArrayBigInt();
                        result.sign = false;
                        result.array = ParallelClaSubtract(first.array, second.array);
                        return result;
                    }
                }
            }
        }

        #endregion Funções estáticas públicas

        #region Funções privadas

        #region Funções internas

        /// <summary>
        /// Permite adicionar dois números inteiros longos em base binária 2^64, retornando
        /// o resultado na base 10^19.
        /// </summary>
        /// <remarks>
        /// Esta função presta-se fundamentalmente à realização de testes unitários.
        /// </remarks>
        /// <param name="first">O primeiro número a ser adicionado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>O transporte e o valor da soma.</returns>
        internal Tuple<ulong, ulong> InternalDecimalRepAdd(ulong first, ulong second)
        {
            return this.DecimalRepAdd(first, second);
        }

        /// <summary>
        /// Adiciona um valor à representação decimal de um número enorme.
        /// </summary>
        /// <remarks>
        /// Esta função presta-se fundamentalmente à realização de testes unitários.
        /// </remarks>
        /// <param name="decimalRepresentation">A representação decimal do número enorme.</param>
        /// <param name="value">O valor a ser adicionado.</param>
        internal void InternalDecimalRepAdd(List<ulong> decimalRepresentation, ulong value)
        {
            this.DecimalRepAdd(decimalRepresentation, value);
        }

        /// <summary>
        /// Permite multiplicar dois números longos sem sinal binários, retornando
        /// o resultado na base 10^19.
        /// </summary>
        /// <remarks>
        /// O algoritmo baseia-se na propriedade de que qualquer número longo sem sinal admite a
        /// representação {0,1}x10^19+x.
        /// Esta função presta-se fundamentelmente à realização de testes unitários.
        /// </remarks>
        /// <param name="first">O primeiro número a ser multiplicado.</param>
        /// <param name="second">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        internal Tuple<ulong, ulong, ulong> InternalDecimalRepMultiply(ulong first, ulong second)
        {
            return this.DecimalRepMultiply(first, second);
        }

        /// <summary>
        /// Multiplica a representação decimal em base 10^19 do número pela potÊncia 2^64.
        /// </summary>
        /// <remarks>
        /// Recorre-se aqui ao caso em que as unidades se encontram nas primeiras posições da lista
        /// que contém a representação.
        /// Esta função presta-se fundamentalmente à realização de testes unitários.
        /// </remarks>
        /// <param name="decimalRepresentation">A representação decimal do número.</param>
        internal void InternalDecimalMultiplyByBinaryPower(List<ulong> decimalRepresentation)
        {
            this.DecimalMultiplyByBinaryPower64(decimalRepresentation);
        }

        #endregion Funções internas

        /// <summary>
        /// Função auxiliar que permite escrever a representação textual de um número representado em notação
        /// decimal na base 10000000000000000000.
        /// </summary>
        /// <param name="decimalRep">A representação decimal como vector de longos sem sinal.</param>
        /// <returns>A representação textual do número.</returns>
        private string GetDecimalRepresentation(List<ulong> decimalRep)
        {
            var length = decimalRep.Count;
            var index = 0;
            var result = decimalRep[index].ToString();
            if (result.Length > 18)
            {
                result = result.Substring(0, 18);
            }

            ++index;
            for (; index < length; ++index)
            {
                var next = decimalRep[index].ToString();
                var additional = 18 - next.Length;
                if (additional > 0)
                {
                    for (int i = 0; i < additional; ++i)
                    {
                        next = "0" + next;
                    }
                }
                else if (additional < 0)
                {
                    next = next.Substring(0, 18);
                }

                result += next;
            }

            return result;
        }

        /// <summary>
        /// Incrementa o valor na representação decimal em uma unidade.
        /// </summary>
        /// <param name="decimalRepresentation">A representação decimal.</param>
        private void IncrementDecimalRep(List<ulong> decimalRepresentation)
        {
            var index = decimalRepresentation.Count - 1;
            var current = decimalRepresentation[index];
            while (current == 999999999999999999 && index > -1)
            {
                decimalRepresentation[index] = 0;
                --index;
            }

            if (index == -1)
            {
                decimalRepresentation.Insert(0, 1);
            }
            else
            {
                decimalRepresentation[index] = current + 1;
            }
        }

        /// <summary>
        /// Duplica o número dado pela representação decimal.
        /// </summary>
        /// <param name="decimalRepresentation">A representação decimal.</param>
        private void DuplicateDecimalRep(List<ulong> decimalRepresentation)
        {
            var numBase = 1000000000000000000u;
            var index = decimalRepresentation.Count - 1;
            var current = decimalRepresentation[index];
            current = current << 1;
            var carry = 1u;
            if (current < numBase)
            {
                carry = 0;
            }
            else
            {
                // A conta é aqui realizada de forma errada
                current -= numBase;
            }

            decimalRepresentation[index] = current;
            --index;
            while (index >= 0)
            {
                current = decimalRepresentation[index];
                current = (current << 1) + carry;
                if (current < numBase)
                {
                    carry = 0u;
                }
                else
                {
                    current -= numBase;
                    carry = 1u;
                }

                decimalRepresentation[index] = current;
                --index;
            }

            if (carry > 0)
            {
                decimalRepresentation.Insert(0, carry);
            }
        }

        /// <summary>
        /// Adiciona um valor à representação decimal de um número enorme.
        /// </summary>
        /// <param name="decimalRepresentation">A representação decimal do número enorme.</param>
        /// <param name="value">O valor a ser adicionado.</param>
        private void DecimalRepAdd(List<ulong> decimalRepresentation, ulong value)
        {
            var current = decimalRepresentation[0];
            var sum = this.DecimalRepAdd(current, value);
            var carry = sum.Item1;
            if (sum.Item2 < 10000000000000000000ul)
            {
                decimalRepresentation[0] = sum.Item2;
            }
            else
            {
                decimalRepresentation[0] = sum.Item2 - 10000000000000000000ul;
                ++carry;
            }

            var length = decimalRepresentation.Count;
            var index = 1;
            while (carry > 0ul && index < length)
            {
                current = decimalRepresentation[index];
                sum = this.DecimalRepAdd(current, carry);
                carry = sum.Item1;
                if (sum.Item2 < 10000000000000000000ul)
                {
                    decimalRepresentation[index] = sum.Item2;
                }
                else
                {
                    decimalRepresentation[index] = sum.Item2 - 10000000000000000000ul;
                    ++carry;
                }

                ++index;
            }

            if (carry > 0ul)
            {
                decimalRepresentation.Add(carry);
            }
        }

        /// <summary>
        /// Multiplica a representação decimal em base 10^19 do número pela potÊncia 2^64.
        /// </summary>
        /// <remarks>
        /// Recorre-se aqui ao caso em que as unidades se encontram nas primeiras posições da lista
        /// que contém a representação.
        /// </remarks>
        /// <param name="decimalRepresentation">A representação decimal do número.</param>
        private void DecimalMultiplyByBinaryPower64(List<ulong> decimalRepresentation)
        {
            var low = 8446744073709551616ul;
            var previous = decimalRepresentation[0];
            var prod = this.DecimalRepMultiply(previous, low);
            decimalRepresentation[0] = prod.Item3;
            var carries = new List<ulong>() { prod.Item2, prod.Item1 };
            var length = decimalRepresentation.Count;
            for (int i = 1; i < length; ++i)
            {
                var current = decimalRepresentation[i];
                prod = this.DecimalRepMultiply(current, low);
                var currentCarry = carries[0];
                carries.RemoveAt(0);

                var sum = this.DecimalRepAdd(currentCarry, prod.Item3);

                // Propaga todos os transportes
                var index = 0;
                var carriesLength = carries.Count;
                currentCarry = sum.Item1;

                // Propaga o transporte resultante da soma com o primeiro elemento
                while (currentCarry > 0ul && index < carriesLength)
                {
                    var innerSum = this.DecimalRepAdd(carries[index], currentCarry);
                    carries[index] = innerSum.Item2;
                    currentCarry = innerSum.Item1;
                    ++index;
                }

                if (currentCarry > 0ul)
                {
                    carries.Add(currentCarry);
                }

                // Propaga o transporte resultante do primeiro elemento do produto
                index = 0;
                currentCarry = prod.Item2;
                carriesLength = carries.Count;
                while (currentCarry > 0ul && index < carriesLength)
                {
                    var innerSum = this.DecimalRepAdd(carries[index], currentCarry);
                    carries[index] = innerSum.Item2;
                    currentCarry = innerSum.Item1;
                    ++index;
                }

                if (currentCarry > 0ul)
                {
                    carries.Add(currentCarry);
                }

                // Propaga o transporte do segundo elemento do produto
                index = 1;
                currentCarry = prod.Item1;
                carriesLength = carries.Count;
                while (currentCarry > 0ul && index < carriesLength)
                {
                    var innerSum = this.DecimalRepAdd(carries[index], currentCarry);
                    carries[index] = innerSum.Item2;
                    currentCarry = innerSum.Item1;
                    ++index;
                }

                if (currentCarry > 0ul)
                {
                    carries.Add(currentCarry);
                }

                // Realiza a soma com o elemento anterior (2^64 = 1x10^19 + 8446744073709551616)
                sum = this.DecimalRepAdd(previous, sum.Item2);
                index = 0;
                carriesLength = carries.Count;
                currentCarry = sum.Item1;

                // Propaga o transporte resultante da soma com o primeiro elemento
                carriesLength = carries.Count;
                while (currentCarry > 0ul && index < carriesLength)
                {
                    var innerSum = this.DecimalRepAdd(carries[index], currentCarry);
                    carries[index] = innerSum.Item2;
                    currentCarry = innerSum.Item1;
                    ++index;
                }

                if (currentCarry > 0ul)
                {
                    carries.Add(currentCarry);
                }

                decimalRepresentation[i] = sum.Item2;
                previous = current;
            }

            length = carries.Count;
            if (length > 0)
            {
                var sum = this.DecimalRepAdd(carries[0], previous);
                decimalRepresentation.Add(sum.Item2);
                var index = 1;
                while (sum.Item1 > 0 && index < length)
                {
                    sum = this.DecimalRepAdd(carries[index], sum.Item1);
                    decimalRepresentation.Add(sum.Item2);
                    ++index;
                }

                if (sum.Item1 > 0)
                {
                    decimalRepresentation.Add(sum.Item1);
                }
                else
                {
                    while (index < length)
                    {
                        decimalRepresentation.Add(carries[index]);
                        ++index;
                    }
                }
            }
            else
            {
                decimalRepresentation.Add(previous);
            }
        }

        /// <summary>
        /// Permite adicionar dois números inteiros longos em base binária 2^64, retornando
        /// o resultado na base 10^19.
        /// </summary>
        /// <param name="first">O primeiro número a ser adicionado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>O transporte e o valor da soma.</returns>
        private Tuple<ulong, ulong> DecimalRepAdd(ulong first, ulong second)
        {
            checked
            {
                var sum = Add(first, second);
                if (sum.Item1)
                {
                    var highRes = 1ul;
                    var lowRes = sum.Item2;
                    if (lowRes >= 10000000000000000000ul)
                    {
                        ++highRes;
                        lowRes -= 10000000000000000000ul;
                    }

                    lowRes += 8446744073709551616ul;
                    if (lowRes >= 10000000000000000000ul)
                    {
                        ++highRes;
                        lowRes -= 10000000000000000000ul;
                    }

                    return Tuple.Create(highRes, lowRes);
                }
                else
                {
                    var highRes = 0ul;
                    var lowRes = sum.Item2;
                    if (lowRes >= 10000000000000000000ul)
                    {
                        highRes = 1ul;
                        lowRes -= 10000000000000000000ul;
                    }

                    return Tuple.Create(highRes, lowRes);
                }
            }
        }

        /// <summary>
        /// Permite multiplicar dois números longos sem sinal binários, retornando
        /// o resultado na base 10^19.
        /// </summary>
        /// <remarks>
        /// O algoritmo baseia-se na propriedade de que qualquer número longo sem sinal admite a
        /// representação {0,1}x10^19+x.
        /// </remarks>
        /// <param name="first">O primeiro número a ser multiplicado.</param>
        /// <param name="second">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        private Tuple<ulong, ulong, ulong> DecimalRepMultiply(ulong first, ulong second)
        {
            var firstItem = 0ul;
            var secondItem = 0ul;
            var thirdItem = 0ul;

            var prod = Multiply(first, second);

            if (prod.Item2 < 10000000000000000000ul)
            {
                thirdItem = prod.Item2;
            }
            else
            {
                thirdItem = prod.Item2 - 10000000000000000000ul;
                secondItem = 1ul;
            }

            if (prod.Item1 > 0)
            {
                // Nunca ocorre o fenómeno de transporte da primeira vez
                if (prod.Item1 < 10000000000000000000ul)
                {
                    secondItem = prod.Item1;
                }
                else
                {
                    secondItem += prod.Item1 - 10000000000000000000ul;
                    firstItem = 1ul;
                }

                do
                {
                    prod = Multiply(prod.Item1, 8446744073709551616);

                    // Termo de ordem baixa
                    if (prod.Item2 < 10000000000000000000ul)
                    {
                        var sum = this.DecimalRepAdd(thirdItem, prod.Item2);
                        thirdItem = sum.Item2;
                        if (sum.Item1 > 0)
                        {
                            sum = this.DecimalRepAdd(sum.Item1, secondItem);
                            secondItem = sum.Item2;
                            firstItem += sum.Item1;
                        }
                    }
                    else
                    {
                        // Ocorrência de transporte
                        if (secondItem == 999999999999999999ul)
                        {
                            secondItem = 0ul;
                            ++firstItem;
                        }
                        else
                        {
                            ++secondItem;
                        }

                        var sum = this.DecimalRepAdd(thirdItem, prod.Item2 - 10000000000000000000ul);
                        thirdItem = sum.Item2;
                        if (sum.Item1 > 0)
                        {
                            sum = this.DecimalRepAdd(sum.Item1, secondItem);
                            secondItem = sum.Item2;
                            firstItem += sum.Item1;
                        }
                    }

                    // Termo de ordem alta
                    if (prod.Item1 < 10000000000000000000ul)
                    {
                        var sum = this.DecimalRepAdd(secondItem, prod.Item1);
                        secondItem = sum.Item2;
                        if (sum.Item1 > 0)
                        {
                            firstItem += sum.Item1;
                        }
                    }
                    else
                    {
                        ++firstItem;
                    }

                } while (prod.Item1 > 2);

                // Termina a operação, adidicionando o produto final
                var finalProd = prod.Item1 * 8446744073709551616;
                if (finalProd < 10000000000000000000ul)
                {
                    var sum = this.DecimalRepAdd(thirdItem, finalProd);
                    thirdItem = sum.Item2;
                    if (sum.Item1 > 0)
                    {
                        sum = this.DecimalRepAdd(sum.Item1, secondItem);
                        secondItem = sum.Item2;
                        firstItem += sum.Item1;
                    }
                }
                else
                {
                    if (secondItem == 999999999999999999ul)
                    {
                        secondItem = 0ul;
                        ++firstItem;
                    }
                    else
                    {
                        ++secondItem;
                    }

                    var sum = this.DecimalRepAdd(thirdItem, finalProd - 10000000000000000000ul);
                    thirdItem = sum.Item2;
                    if (sum.Item1 > 0)
                    {
                        sum = this.DecimalRepAdd(sum.Item1, secondItem);
                        secondItem = sum.Item2;
                        firstItem += sum.Item1;
                    }
                }
            }

            return Tuple.Create(firstItem, secondItem, thirdItem);
        }

        #endregion Funções privadas

        #region Funções estáticas privadas

        /// <summary>
        /// Adiciona um valor ao final da lista, deslocando os "bits" para a esquerda
        /// em quatro unidades (alinhamento associado à base 0x8000000000000000).
        /// </summary>
        /// <param name="list">A lista à qual se pretende adicionar o valor.</param>
        /// <param name="value">O valor a ser adicionado.</param>
        /// <param name="alignement">O alinhamento.</param>
        private static void AppendUlong(List<ulong> list, ulong value, int alignement)
        {
            var index = list.Count - 1;
            var complement = 64 - alignement;
            var mask = 0xFFFFFFFFFFFFFFFF >> complement;

            // O valor dos "bits" mais baixos são colocados no valor anterior.
            var lowest = (value & mask) << complement;
            list[index] = list[index] | lowest;

            var append = value >> alignement;
            if (append != 0)
            {
                list.Add(append);
            }
        }

        /// <summary>
        /// Permite devidir um número pela base 2.
        /// </summary>
        /// <param name="text">A representação textual do número a ser dividio.</param>
        /// <returns>O quociente e o resto da divisão.</returns>
        private static Tuple<string, byte> DivideByBase1(string text)
        {
            var length = text.Length;
            var index = 0;
            var quotient = string.Empty;
            var remainder = default(byte);
            if (length < 20)
            {
                var parsed = ulong.Parse(text);
                quotient = (parsed >> 1).ToString();
                remainder = (byte)(parsed & 1);
            }
            else
            {
                var parsing = text.Substring(index, 18);
                var parsed = ulong.Parse(parsing);
                quotient += (parsed >> 1).ToString();
                remainder = (byte)(parsed & 1);
                index += 18;
                while (index < length)
                {
                    var nextIndex = index + 18;
                    var len = 18;
                    if (nextIndex > length)
                    {
                        nextIndex = length;
                        len = length - index;
                        parsing = text.Substring(index, len);
                    }
                    else
                    {
                        parsing = text.Substring(index, len);
                    }

                    if (remainder == 1)
                    {
                        parsing = "1" + parsing;
                    }

                    parsed = ulong.Parse(parsing);
                    var temp = (parsed >> 1).ToString();
                    for (int i = temp.Length; i < len; ++i)
                    {
                        temp = "0" + temp;
                    }

                    quotient += temp;
                    remainder = (byte)(parsed & 1);
                    index = nextIndex;
                }
            }

            return Tuple.Create(quotient, remainder);
        }

        /// <summary>
        /// Premite dividir um número pela base 0x800000000000000.
        /// </summary>
        /// <param name="text">O número a ser dividido.</param>
        /// <returns>O quociente e o resto da divisão.</returns>
        private static Tuple<string, ulong> DivideByBase59(string text)
        {
            var length = text.Length;
            var numbBase = 0x800000000000000u;
            if (length < 19)
            {
                var currentValue = ulong.Parse(text);
                return Tuple.Create("0", currentValue);
            }
            else
            {
                var initialText = text.Substring(0, 18);
                var currentValue = ulong.Parse(initialText);
                var index = 18;

                // Ao início todos os zeros no resultado são ignorados
                while (currentValue < numbBase && index < length)
                {
                    var chrValue = (ulong)char.GetNumericValue(text[index]);
                    currentValue *= 10;
                    currentValue += chrValue;
                    ++index;
                }

                var result = (currentValue / numbBase).ToString();
                currentValue %= numbBase;
                while (index < length)
                {
                    var chrValue = (ulong)char.GetNumericValue(text[index]);
                    currentValue *= 10;
                    currentValue += chrValue;
                    if (currentValue < numbBase)
                    {
                        result += "0";
                    }
                    else if (currentValue == numbBase)
                    {
                        result += "1";
                        currentValue = 0;
                    }
                    else
                    {
                        result += (currentValue / numbBase).ToString();
                        currentValue %= numbBase;
                    }

                    ++index;
                }

                return Tuple.Create(result, currentValue);
            }
        }

        /// <summary>
        /// Multiplica o número enorme representado na lista pelo número longo sem sinal especificado.
        /// </summary>
        /// <param name="list">A representação do número enorme.</param>
        /// <param name="value">O número longo sem sinal.</param>
        private static void Multiply(List<ulong> list, ulong value)
        {
            var length = list.Count;
            var carry = 0ul;
            for (int index = 0; index < length; ++index)
            {
                var current = list[index];
                var multRes = Multiply(current, value);

                var sumRes = Add(multRes.Item2, carry);
                carry = 0ul;
                if (sumRes.Item1)
                {
                    carry = 1ul;
                }

                carry += multRes.Item1;
                current = sumRes.Item2;
                list[index] = current;
            }

            if (carry > 0)
            {
                list.Add(carry);
            }
        }

        /// <summary>
        /// Adiciona o valor ao número enorme representado por uma lista de longos sem sinal.
        /// </summary>
        /// <param name="list">A lista que contém a representação do número enorme.</param>
        /// <param name="value">O valor a ser adicionado.</param>
        private static void Add(List<ulong> list, ulong value)
        {
            var length = list.Count;
            var index = 0;
            var current = list[index];
            var sumRes = Add(current, value);
            list[index] = sumRes.Item2;
            var carry = sumRes.Item1;
            ++index;
            while (index < length && carry)
            {
                current = list[index];
                if (current == 0xFFFFFFFFFFFFFFFF)
                {
                    list[index] = 0ul;
                }
                else
                {
                    list[index] = current + 1;
                    carry = false;
                }

                ++index;
            }

            if (carry)
            {
                list.Add(1ul);
            }
        }

        /// <summary>
        /// Permite adicionar dois números longos sem sinal, determinando o transporte caso esta soma exceda
        /// o tamanho da variável.
        /// </summary>
        /// <remarks>
        /// É importante notar que, numa soma, o valor do transporte é sempre unitário.
        /// </remarks>
        /// <param name="first">O primeiro número a ser adicionado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>O par transporte/valor da soma.</returns>
        private static Tuple<bool, ulong> Add(ulong first, ulong second)
        {
            unchecked
            {
                return Tuple.Create((~first | 1) < second, first + second);
            }
        }

        /// <summary>
        /// Multiplica dois números longos sem sinal, incluindo o transporte caso este exceda
        /// o tamanho da variável.
        /// </summary>
        /// <param name="first">O primeiro número a ser multiplicado.</param>
        /// <param name="second">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        private static Tuple<ulong, ulong> Multiply(ulong first, ulong second)
        {
            checked
            {
                var firstLow = 0xFFFFFFFF & first;
                var firstHigh = first >> 32;
                var secondLow = 0xFFFFFFFF & second;
                var secondHigh = second >> 32;

                var z2 = firstHigh * secondHigh;
                var z0 = firstLow * secondLow;
                var mediumRes = Add(firstHigh * secondLow, firstLow * secondHigh);
                var z1 = mediumRes.Item2;

                var highz = z1 >> 32;
                var lowz = z1 << 32;

                var sumLowRes = Add(z0, lowz);
                var lowRes = sumLowRes.Item2;
                var highRes = z2 + highz;

                if (sumLowRes.Item1)
                {
                    ++highRes;
                }

                if (mediumRes.Item1)
                {
                    highRes += 0x100000000;
                }

                return Tuple.Create(highRes, lowRes);
            }
        }

        /// <summary>
        /// Permite determinar a soma de dois números inteiros enormes com base na sua representação.
        /// </summary>
        /// <param name="first">A representação do primeiro inteiro enorme.</param>
        /// <param name="second">A representação do segundo inteiro enorme.</param>
        /// <returns>O resultado da soma.</returns>
        private static ulong[] SequentialAdd(ulong[] first, ulong[] second)
        {
            if (first == null && second == null)
            {
                return null;
            }
            else if (first == null)
            {
                var length = second.Length;
                var result = new ulong[length];
                Array.Copy(second, result, length);
                return result;
            }
            else if (second == null)
            {
                var length = first.Length;
                var result = new ulong[length];
                Array.Copy(first, result, length);
                return result;
            }
            else
            {
                var innerFirst = first;
                var innerSecond = second;

                var firstLength = first.Length;
                var secondLength = second.Length;
                if (secondLength < firstLength)
                {
                    innerFirst = second;
                    innerSecond = first;
                    firstLength = second.Length;
                    secondLength = first.Length;
                }

                // Efectua a adição dos vectores inciais
                var partialResult = new ulong[firstLength];
                var carry = false;
                for (int i = 0; i < firstLength; ++i)
                {
                    var sum = Add(innerFirst[i], innerSecond[i]);
                    if (carry)
                    {
                        if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                        {
                            partialResult[i] = 0;
                            carry = true;
                        }
                        else
                        {
                            partialResult[i] = sum.Item2 + 1;
                            carry = sum.Item1;
                        }
                    }
                    else
                    {
                        partialResult[i] = sum.Item2;
                        carry = sum.Item1;
                    }
                }

                if (carry)
                {
                    // Verifica se existe algum valor diferente do máximo
                    var found = -1;
                    for (int i = firstLength; i < secondLength; ++i)
                    {
                        if (innerSecond[i] != 0xFFFFFFFFFFFFFFFF)
                        {
                            found = i;
                            i = secondLength;
                        }
                    }

                    if (found < 0)
                    {
                        var result = new ulong[secondLength + 1];
                        Array.Copy(partialResult, result, firstLength);
                        result[secondLength] = 1;
                        --secondLength;
                        for (; secondLength >= firstLength; --secondLength)
                        {
                            result[secondLength] = 0;
                        }

                        return result;
                    }
                    else
                    {
                        var result = new ulong[secondLength];
                        Array.Copy(partialResult, result, firstLength);
                        for (int i = firstLength; i < found; ++i)
                        {
                            result[i] = 0;
                        }

                        ++result[found++];
                        Array.Copy(innerSecond, found, result, found, secondLength - found);
                        return result;
                    }
                }
                else
                {
                    var result = new ulong[secondLength];
                    Array.Copy(partialResult, result, firstLength);

                    // Copia os restantes elementos
                    Array.Copy(second, firstLength, result, firstLength, secondLength - firstLength);
                    return result;
                }
            }
        }

        /// <summary>
        /// Determina a diferença entre dois números.
        /// </summary>
        /// <remarks>
        /// Se o primeiro número for inferior ao segundo, então o resultado da diferença
        /// poderá não corresponder ao esperado.
        /// </remarks>
        /// <param name="greatest">O maior número.</param>
        /// <param name="smallest">O menor número.</param>
        /// <returns>O resultado da diferença.</returns>
        private static ulong[] SequentialSubtract(ulong[] greatest, ulong[] smallest)
        {
            if (smallest == null)
            {
                var length = greatest.Length;
                var result = new ulong[length];
                Array.Copy(greatest, result, length);
                return result;
            }
            else
            {
                var greatestLength = greatest.Length;
                var smallestLength = smallest.Length;

                var partialResult = new ulong[greatestLength];
                var carry = false;
                var complement = ~smallest[0];
                var sum = Add(greatest[0], complement);
                complement = sum.Item2 + 1;
                if (complement == 0)
                {
                    carry = true;
                }
                else
                {
                    carry = sum.Item1;
                }

                partialResult[0] = complement;
                for (int i = 1; i < smallestLength; ++i)
                {
                    complement = ~smallest[i];
                    sum = Add(greatest[i], complement);
                    if (carry)
                    {
                        if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                        {
                            partialResult[i] = 0;
                            carry = true;
                        }
                        else
                        {
                            partialResult[i] = sum.Item2 + 1;
                            carry = sum.Item1;
                        }
                    }
                    else
                    {
                        partialResult[i] = sum.Item2;
                        carry = sum.Item1;
                    }
                }

                for (int i = smallestLength; i < greatestLength; ++i)
                {
                    sum = Add(greatest[i], 0xFFFFFFFFFFFFFFFF);
                    if (carry)
                    {
                        if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                        {
                            partialResult[i] = 0;
                            carry = true;
                        }
                        else
                        {
                            partialResult[i] = sum.Item2 + 1;
                            carry = sum.Item1;
                        }
                    }
                    else
                    {
                        partialResult[i] = sum.Item2;
                        carry = sum.Item1;
                    }
                }

                // Mesmo em caso de transporte, este será ignorado e eliminados todas as variáveis iniciais nulas
                var finalLength = greatestLength;
                for (int i = greatestLength - 1; i > -1; --i)
                {
                    if (partialResult[i] == 0)
                    {
                        --finalLength;
                    }
                    else
                    {
                        i = -1;
                    }
                }

                if (finalLength == 0)
                {
                    return null;
                }
                else
                {
                    var result = new ulong[finalLength];
                    Array.Copy(partialResult, result, finalLength);
                    return result;
                }
            }
        }

        /// <summary>
        /// Permite determinar a soma de dois inteiros enormes recorrendo a vários núcleos de processamento com base
        /// no método CLA (propagação e transporte).
        /// </summary>
        /// <remarks>
        /// A alternativa mais conhecida ao método do CLA consiste no ELM. O algoritmo CLA tira partido do facto de que,
        /// após uma soma de células individuais, nunca se dá a possibilidade de uma marca de transporte e de propagação
        /// em simultâneo.
        /// </remarks>
        /// <param name="first">O primeiro número a ser adicioinado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>A soma.</returns>
        private static ulong[] ParallelClaAdd(ulong[] first, ulong[] second)
        {
            if (first == null && second == null)
            {
                return null;
            }
            else if (first == null)
            {
                var length = second.Length;
                var result = new ulong[length];

                Array.Copy(second, result, length);
                return result;
            }
            else if (second == null)
            {
                var length = first.Length;
                var result = new ulong[length];

                // A cópia é realizada em paralelo
                Parallel.For(0, length, i =>
                {
                    result[i] = first[i];
                });

                return result;
            }
            else
            {
                var innerFirst = first;
                var innerSecond = second;

                var firstLength = first.Length;
                var secondLength = second.Length;
                if (secondLength < firstLength)
                {
                    innerFirst = second;
                    innerSecond = first;

                    // Troca os valores dos comprimentos com base no algoritmo do "ou exclusivo"
                    firstLength ^= secondLength;
                    secondLength ^= firstLength;
                    firstLength ^= secondLength;
                }

                // O vector de "bits" que marca as posições dos transportes
                var carries = new BitArray(firstLength);

                // O vector de "bits" que marca a posição das propagações
                var propagations = new BitArray(firstLength);

                // Determina a soma dos primeiros blocos
                var partialResult = new ulong[firstLength];
                Parallel.For(0, firstLength, i =>
                {
                    var additionResult = Add(innerFirst[i], innerSecond[i]);
                    if (additionResult.Item1)
                    {
                        carries[i] = true;
                    }

                    if (additionResult.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        propagations[i] = true;
                    }

                    partialResult[i] = additionResult.Item2;
                });

                // Aplica os transportes tendo em conta as propagações.
                Parallel.For(1, firstLength, i =>
                {
                    if (propagations[i])
                    {
                        for (int j = i - 1; j > -1; ++j)
                        {
                            // Caso alguma propagação já tenha sido avaliada, o transporte resultante pode ser utilizado
                            if (carries[j])
                            {
                                partialResult[i] = 0;
                                carries[i] = true;
                                j = -1;
                            }
                            else if (!propagations[j])
                            {
                                j = -1;
                            }
                        }
                    }
                    else if (carries[i - 1])
                    {
                        ++partialResult[i];
                    }
                });

                if (carries[firstLength - 1])
                {
                    var propagationsList = new BitList(secondLength - firstLength, false);
                    Parallel.For(firstLength, secondLength, i =>
                    {
                        if (innerSecond[i] == 0xFFFFFFFFFFFFFFFF)
                        {
                            propagationsList[i - firstLength] = 1;
                        }
                    });

                    var foundIndex = propagationsList.IndexOf(0);
                    if (foundIndex < 0)
                    {
                        var result = new ulong[secondLength + 1];
                        result[secondLength] = 1u;
                        var firstTask = new Task(() =>
                        {
                            Parallel.For(0, firstLength, i =>
                            {
                                result[i] = partialResult[i];
                            });
                        });

                        var secondTask = new Task(() =>
                        {
                            Parallel.For(firstLength, secondLength, i =>
                            {
                                result[i] = 0u;
                            });
                        });

                        Task.WaitAll(new[] { firstTask, secondTask });
                        return result;
                    }
                    else
                    {
                        foundIndex += firstLength;
                        var result = new ulong[secondLength];
                        var firstTask = new Task(() =>
                        {
                            Parallel.For(0, firstLength, i =>
                            {
                                result[i] = partialResult[i];
                            });
                        });

                        var secondTask = new Task(() =>
                        {
                            Parallel.For(firstLength, foundIndex, i =>
                            {
                                result[i] = 0u;
                            });
                        });

                        result[foundIndex] = innerSecond[foundIndex] + 1;

                        var thirdTask = new Task(() =>
                        {
                            Parallel.For(foundIndex + 1, secondLength, i =>
                            {
                                result[i] = 0u;
                            });
                        });

                        firstTask.Start();
                        secondTask.Start();
                        thirdTask.Start();
                        Task.WaitAll(new[] { firstTask, secondTask, thirdTask });
                        return result;
                    }
                }
                else
                {
                    // Não há transportes nesta fase
                    var result = new ulong[secondLength];
                    var firstTask = new Task(() =>
                    {
                        Parallel.For(0, firstLength, i =>
                        {
                            result[i] = partialResult[i];
                        });
                    });

                    var secondTask = new Task(() =>
                    {
                        Parallel.For(firstLength, secondLength, i =>
                        {
                            result[i] = innerSecond[i];
                        });
                    });

                    firstTask.Start();
                    secondTask.Start();
                    Task.WaitAll(new[] { firstTask, secondTask });
                    return result;
                }
            }
        }

        /// <summary>
        /// Obtém a diferença entre dois números de forma paralela, recorrendo ao método
        /// CLA para somas de dois números.
        /// </summary>
        /// <param name="greatest">O maior dos números.</param>
        /// <param name="smallest">O menor número a ser subtraído.</param>
        /// <returns>A diferença.</returns>
        private static ulong[] ParallelClaSubtract(ulong[] greatest, ulong[] smallest)
        {
            if (smallest == null)
            {
                var length = greatest.Length;
                var result = new ulong[length];
                Array.Copy(greatest, result, length);
                return result;
            }
            else
            {
                var greatestLength = greatest.Length;
                var smallestLength = smallest.Length;

                // O vector de "bits" que marca as posições dos transportes
                var carries = new BitArray(greatestLength);

                // O vector de "bits" que marca a posição das propagações
                var propagations = new BitArray(greatestLength);

                // Determina a soma dos primeiros blocos
                var partialResult = new ulong[greatestLength];
                var complement = ~smallest[0];
                var additionResult = Add(greatest[0], complement);
                complement = additionResult.Item2 + 1;
                if (complement == 0)
                {
                    carries[0] = true;
                }
                else
                {
                    carries[0] = additionResult.Item1;
                }

                partialResult[0] = complement;
                var firstTask = new Task(() =>
                {
                    Parallel.For(1, smallestLength, i =>
                    {
                        complement = ~smallest[i];
                        var innerAdditionResult = Add(greatest[i], complement);
                        if (innerAdditionResult.Item1)
                        {
                            carries[i] = true;
                        }

                        if (innerAdditionResult.Item2 == 0xFFFFFFFFFFFFFFFF)
                        {
                            propagations[i] = true;
                        }

                        partialResult[i] = innerAdditionResult.Item2;
                    });
                });

                var secondTask = new Task(() =>
                {
                    Parallel.For(smallestLength, greatestLength, i =>
                    {
                        var innerAdditionResult = Add(greatest[i], 0xFFFFFFFFFFFFFFFF);
                        if (innerAdditionResult.Item1)
                        {
                            carries[i] = true;
                        }

                        if (innerAdditionResult.Item2 == 0xFFFFFFFFFFFFFFFF)
                        {
                            propagations[i] = true;
                        }

                        partialResult[i] = innerAdditionResult.Item2;
                    });
                });

                firstTask.Start();
                secondTask.Start();
                Task.WaitAll(new[] { firstTask, secondTask });

                // Aplica os transportes tendo em conta as propagações.
                Parallel.For(1, greatestLength, i =>
                {
                    if (propagations[i])
                    {
                        for (int j = i - 1; j > -1; ++j)
                        {
                            // Caso alguma propagação já tenha sido avaliada, o transporte resultante pode ser utilizado
                            if (carries[j])
                            {
                                partialResult[i] = 0;
                                carries[i] = true;
                                j = -1;
                            }
                            else if (!propagations[j])
                            {
                                j = -1;
                            }
                        }
                    }
                    else if (carries[i - 1])
                    {
                        ++partialResult[i];
                    }
                });

                // Mesmo em caso de transporte, este será ignorado e eliminados todas as variáveis iniciais nulas
                var finalLength = greatestLength;
                for (int i = greatestLength - 1; i > -1; --i)
                {
                    if (partialResult[i] == 0)
                    {
                        --finalLength;
                    }
                    else
                    {
                        i = -1;
                    }
                }

                if (finalLength == 0)
                {
                    return null;
                }
                else
                {
                    var result = new ulong[finalLength];
                    Array.Copy(partialResult, result, finalLength);
                    Parallel.For(0, finalLength, i =>
                    {
                        result[i] = partialResult[i];
                    });

                    return result;
                }
            }
        }

        /// <summary>
        /// Efectua a rotação à direita da representação de um número enorme.
        /// </summary>
        /// <param name="value">O número enorme a ser rodado.</param>
        /// <param name="places">O tamanho da deslocação em "bits".</param>
        /// <returns>O valor rodado.</returns>
        private static ulong[] RotateRight(ulong[] value, int places)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                // Se o valor for negativo, o "bit" correspondente ao sinal é eliminado
                var innerPlaces = places & 0x7FFFFFFF;

                // Inicia o processo de rotação
                var length = value.Length;
                var masterRotate = innerPlaces / 64;
                if (masterRotate < length)
                {
                    // Determina o tamanho do novo vector
                    var innerRotate = innerPlaces % 64;
                    var counterRotate = 64 - innerRotate;
                    var index = length - 1;
                    var current = value[index];
                    var masked = current >> counterRotate;
                    current >>= innerRotate;
                    var finalLength = length - masterRotate;
                    if (current == 0)
                    {
                        --finalLength;
                        if (finalLength == 0)
                        {
                            return null;
                        }
                        else
                        {
                            var result = new ulong[finalLength];
                            var finalIndex = finalLength - 1;
                            --finalIndex;
                            --index;
                            while (finalIndex > -1)
                            {
                                current = value[index];
                                result[finalIndex] = (current >> innerRotate) | masked;
                                masked = current >> counterRotate;
                                --finalIndex;
                                --index;
                            }

                            return result;
                        }
                    }
                    else
                    {
                        var result = new ulong[finalLength];
                        var finalIndex = finalLength - 1;
                        result[finalIndex] = current;
                        --finalIndex;
                        --index;
                        while (finalIndex > -1)
                        {
                            current = value[index];
                            result[finalIndex] = (current >> innerRotate) | masked;
                            masked = current >> counterRotate;
                            --finalIndex;
                            --index;
                        }

                        return result;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Efectua a rotação à esquerda da representação de um número enorme.
        /// </summary>
        /// <param name="value">O número enorme a ser rodado.</param>
        /// <param name="places">O tamanho da deslocação em "bits".</param>
        /// <returns>O valor rodado.</returns>
        public static ulong[] RotateLeft(ulong[] value, int places)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                // Se o valor for negativo, o "bit" correspondente ao sinal é eliminado
                var innerPlaces = places & 0x7FFFFFFF;

                // Inicia o processo de rotação
                var length = value.Length;
                var masterRotate = innerPlaces / 64;
                var innerRotate = innerPlaces % 64;
                var counterRotate = 64 - innerRotate;
                var current = value[length - 1];
                var masked = current << counterRotate;
                var finalLength = length + masterRotate;
                if (masked == 0)
                {
                    var finalIndex = finalLength;
                    ++finalLength;
                    var result = new ulong[finalLength];
                    for (int i = 0; i < masterRotate; ++i)
                    {
                        result[i] = 0;
                    }

                    var index = 0;
                    finalIndex = masterRotate;
                    current = value[index];
                    masked = current >> counterRotate;
                    result[finalIndex] = current << innerRotate;
                    ++index;
                    ++finalIndex;
                    while (index < length)
                    {
                        current = value[index];
                        var innerMasked = current >> counterRotate;
                        result[finalIndex] = (current << innerRotate) | masked;
                        masked = current >> counterRotate;
                        ++index;
                        ++finalIndex;
                    }

                    return result;
                }
                else
                {
                    var finalIndex = finalLength - 1;
                    var result = new ulong[finalLength];
                    for (int i = 0; i < masterRotate; ++i)
                    {
                        result[i] = 0;
                    }

                    var index = 0;
                    finalIndex = masterRotate;
                    current = value[index];
                    masked = current >> counterRotate;
                    result[finalIndex] = current << innerRotate;
                    ++index;
                    ++finalIndex;
                    while (index < length)
                    {
                        current = value[index];
                        var innerMasked = current >> counterRotate;
                        result[finalIndex] = (current << innerRotate) | masked;
                        masked = current >> counterRotate;
                        ++index;
                        ++finalIndex;
                    }

                    return result;
                }
            }
        }

        #endregion Funções estáticas privadas
    }

    /// <summary>
    /// Representa um número inteiro grande como sendo um vector de inteiros sem
    /// sinal em base 10^10.
    /// </summary>
    /// <remarks>
    /// As unidades encontram-se representadas na primeira entrada do vector.
    /// </remarks>
    public struct UintArrayDecimalRepBigInt
    {
        /// <summary>
        /// A base na qual se encontra representado o número.
        /// </summary>
        private static uint numberBase = 1000000000;

        /// <summary>
        /// O logaritmo decimal da base.
        /// </summary>
        private static byte numberBaseLog = 9;

        /// <summary>
        /// O sinal do número.
        /// </summary>
        private bool sign;

        /// <summary>
        /// O vector que contém a representação.
        /// </summary>
        private uint[] array;

        #region Construtores

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="array">O vector de valores que inicializam o número.</param>
        public UintArrayDecimalRepBigInt(uint[] array)
        {
            this.sign = false;
            if (array == null || array.Length == 0)
            {
                this.array = null;
            }
            else
            {
                var length = array.Length;
                this.array = new uint[length];
                Array.Copy(array, this.array, length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="list"></param>
        public UintArrayDecimalRepBigInt(List<uint> list)
        {
            if (list == null || list.Count == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = false;
                this.array = list.ToArray();
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="sign">O sinal do número.</param>
        /// <param name="array">O vector de valores que inicializam o número.</param>
        public UintArrayDecimalRepBigInt(bool sign, uint[] array)
        {
            this.sign = sign;
            if (array == null || array.Length == 0)
            {
                this.array = null;
            }
            else
            {
                var length = array.Length;
                this.array = new uint[length];
                Array.Copy(array, this.array, length);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="sign">O sinal do número.</param>
        /// <param name="list"></param>
        public UintArrayDecimalRepBigInt(bool sign, List<uint> list)
        {
            this.sign = sign;
            if (list == null || list.Count == 0)
            {
                this.array = null;
            }
            else
            {
                this.array = list.ToArray();
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UintArrayDecimalRepBigInt(int numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = numb < 0;
                var innerNumb = (uint)Math.Abs(numb);
                var quo = innerNumb / numberBase;
                if (quo == 0)
                {
                    var rem = innerNumb % numberBase;
                    this.array = new[] { quo, rem };
                }
                else
                {
                    this.array = new[] { innerNumb };
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UintArrayDecimalRepBigInt(uint numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = false;
                var quo = numb / numberBase;
                if (quo == 0)
                {
                    var rem = numb % numberBase;
                    this.array = new[] { quo, rem };
                }
                else
                {
                    this.array = new[] { numb };
                }
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UintArrayDecimalRepBigInt(long numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = numb < 0;
                var innerNumb = Math.Abs(numb);
                var result = new List<uint>();
                var quo = innerNumb / numberBase;
                while (quo > 0)
                {
                    var rem = innerNumb % numberBase;
                    result.Add((uint)rem);
                    innerNumb = quo;
                }

                result.Add((uint)innerNumb);
                this.array = result.ToArray();
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UintArrayDecimalRepBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UintArrayDecimalRepBigInt(ulong numb)
        {
            if (numb == 0)
            {
                this.sign = false;
                this.array = null;
            }
            else
            {
                this.sign = false;
                var result = new List<uint>();
                var innerNumb = numb;
                var quo = innerNumb / numberBase;
                while (quo > 0)
                {
                    var rem = innerNumb % numberBase;
                    result.Add((uint)rem);
                    innerNumb = quo;
                }

                result.Add((uint)innerNumb);
                this.array = result.ToArray();
            }
        }

        #endregion Construtores

        /// <summary>
        /// Obtém o sinal do número.
        /// </summary>
        public bool Sign
        {
            get
            {
                return this.sign;
            }
        }

        #region Funções públicas

        /// <summary>
        /// Obtém a representação interna do número.
        /// </summary>
        /// <returns>A representação interna do número.</returns>
        public uint[] GetRepresentationArray()
        {
            if (this.array == null)
            {
                return new uint[0];
            }
            else
            {
                var length = this.array.Length;
                var result = new uint[length];
                Array.Copy(this.array, result, length);
                return result;
            }
        }

        /// <summary>
        /// Determina se o objecto proporcionado é igual ao corrente.
        /// </summary>
        /// <param name="obj">O objecto a ser comparado.</param>
        /// <returns>Verdadeiro caso ambos os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            if (obj is UintArrayDecimalRepBigInt)
            {
                var innerObj = (UintArrayDecimalRepBigInt)obj;
                if (this.array == null && innerObj.array == null)
                {
                    return true;
                }
                else if (this.array == null)
                {
                    return false;
                }
                else if (innerObj.array == null)
                {
                    return false;
                }
                else
                {
                    if (this.sign == innerObj.sign)
                    {
                        var length = this.array.Length;
                        var objLength = innerObj.array.Length;
                        if (length == objLength)
                        {
                            for (int i = 0; i < length; ++i)
                            {
                                if (this.array[i] != innerObj.array[i])
                                {
                                    return false;
                                }
                            }

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determina um código confuso para o número enorme.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var result = typeof(UintArrayDecimalRepBigInt).GetHashCode();
            if (this.array != null)
            {
                result ^= this.sign.GetHashCode();
                var length = this.array.Length;
                for (int i = 0; i < length; ++i)
                {
                    result ^= (19 * this.array[i] + 17u).GetHashCode();
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a representação textual do número.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            if (this.array == null)
            {
                return "0";
            }
            else
            {
                var result = string.Empty;
                if (this.sign)
                {
                    result = "-";
                }

                for (int i = this.array.Length - 1; i > 0; --i)
                {
                    var value = this.array[i].ToString();
                    while (value.Length < numberBaseLog)
                    {
                        value = "0" + value;
                    }

                    result += value;
                }

                result += this.array[0];
                return result;
            }
        }

        #endregion Funções públicas

        #region Funções estáticas públicas

        /// <summary>
        /// Tenta realizar a leitura de um número inteiro enorme a partir da sua representação texual caso
        /// esta seja uma representação correcta.
        /// </summary>
        /// <param name="text">A representação textual do número.</param>
        /// <param name="value">Recebe o valor lido em caso de sucesso.</param>
        /// <returns>Verdadeiro caso a função seja bem-sucedida e falso caso contrário.</returns>
        public static bool TryParse(string text, out UintArrayDecimalRepBigInt value)
        {
            var integerExpression = new Regex("^\\s*(-{0,1})(0*)(\\d+)\\s*$");
            var match = integerExpression.Match(text);
            if (match.Success)
            {
                var sign = false;
                var readed = new List<uint>();

                var innerText = match.Groups[1].Value;
                if (innerText == "-")
                {
                    sign = true;
                }

                // Trata os valores
                innerText = match.Groups[3].Value;
                if (innerText == "0")
                {
                    value = new UintArrayDecimalRepBigInt();
                    return true;
                }
                else
                {
                    var length = innerText.Length;
                    var baseLen = (int)numberBaseLog;
                    var len = baseLen;
                    if (length < len)
                    {
                        len = length;
                    }

                    var parsing = innerText.Substring(0, len);
                    var parsed = uint.Parse(parsing);
                    readed.Add(parsed);

                    var index = len;
                    while (index < length)
                    {
                        len = baseLen;
                        var nextIndex = index + len;
                        if (length < nextIndex)
                        {
                            len = length - index;
                            nextIndex = length;
                        }

                        parsing = innerText.Substring(0, len);
                        parsed = uint.Parse(parsing);
                        readed.Insert(0, parsed);
                        index = nextIndex;
                    }
                }

                value = new UintArrayDecimalRepBigInt(sign, readed);
                return true;
            }
            else
            {
                value = default(UintArrayDecimalRepBigInt);
                return false;
            }
        }

        #endregion

        #region Funções estáticas privadas

        /// <summary>
        /// Realiza a adição sequencial de dois vectores que constituem as representações
        /// de dois números sem sinal.
        /// </summary>
        /// <param name="first">O primeiro número a ser adicionado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        private static uint[] SequentialAdd(uint[] first, uint[] second)
        {
            if (first == null && second == null)
            {
                return null;
            }
            else if (first == null)
            {
                var secondLength = second.Length;
                var result = new uint[secondLength];
                Array.Copy(second, result, secondLength);
                return result;
            }
            else if (second == null)
            {
                var firstLength = first.Length;
                var result = new uint[firstLength];
                Array.Copy(first, result, firstLength);
                return result;
            }
            else
            {
                var innerFirst = first;
                var innerSecond = second;
                var firstLength = first.Length;
                var secondLength = second.Length;
                if (firstLength < secondLength)
                {
                    innerFirst = second;
                    innerSecond = first;
                    firstLength = second.Length;
                    secondLength = first.Length;
                }

                var partialResult = new uint[secondLength];
                var firstCurrent = innerFirst[0];
                var secondCurrent = innerSecond[0];
                var sum = firstCurrent + secondCurrent;
                var carry = 0u;
                if (sum >= numberBase)
                {
                    carry = 1u;
                    partialResult[0] = sum - numberBase;
                }
                else
                {
                    partialResult[0] = sum;
                }

                for (int i = 1; i < secondLength; ++i)
                {
                    firstCurrent = innerFirst[i];
                    secondCurrent = innerSecond[i];
                    sum = firstCurrent + secondCurrent + carry;
                    if (sum >= numberBase)
                    {
                        carry = 1u;
                        partialResult[i] = sum - numberBase;
                    }
                    else
                    {
                        carry = 0u;
                    }
                }

                if (carry == 0u)
                {
                    var result = new uint[firstLength];
                    Array.Copy(partialResult, result, firstLength);
                    Array.Copy(innerSecond, secondLength, result, secondLength, firstLength - secondLength);
                    return result;
                }
                else
                {
                    // Verifica até quando o transporte é propagado
                    var propagationIndex = -1;
                    var max = numberBase - 1;
                    for (int i = secondLength; i < firstLength; ++i)
                    {
                        if (innerFirst[i] != max)
                        {
                            propagationIndex = i;
                            i = firstLength;
                        }
                    }

                    if (propagationIndex < 0)
                    {
                        var result = new uint[firstLength + 1];
                        result[firstLength] = 1u;
                        Array.Copy(partialResult, result, firstLength);
                        for (int i = secondLength; i < firstLength; ++i)
                        {
                            result[i] = 0;
                        }

                        return result;
                    }
                    else
                    {
                        var result = new uint[firstLength];
                        Array.Copy(partialResult, result, firstLength);
                        for (int i = secondLength; i < propagationIndex; ++i)
                        {
                            result[i] = 0;
                        }

                        result[propagationIndex] = innerFirst[propagationIndex] + 1;
                        ++propagationIndex;
                        Array.Copy(innerFirst, propagationIndex, result, propagationIndex, firstLength - propagationIndex);
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Determina a diferença entre dois números.
        /// </summary>
        /// <remarks>
        /// Se o primeiro número for inferior ao segundo, então o resultado da diferença
        /// poderá não corresponder ao esperado.
        /// </remarks>
        /// <param name="greatest">O maior número.</param>
        /// <param name="smallest">O menor número.</param>
        /// <returns>O resultado da diferença.</returns>
        private static uint[] SequentialSubtract(uint[] greatest, uint[] smallest)
        {
            if (smallest == null)
            {
                var length = greatest.Length;
                var result = new uint[length];
                Array.Copy(greatest, result, length);
                return result;
            }
            else
            {
                var greatestLength = greatest.Length;
                var smallestLength = smallest.Length;
                var partialResult = new uint[smallestLength];
                var greatestCurrent = greatest[0];
                var smallestCurrent = smallest[0];
                var carry = 0u;
                if (greatestCurrent < smallestCurrent)
                {
                    partialResult[0] = (numberBase - greatestCurrent) + smallestCurrent;
                    carry = 1u;
                }
                else
                {
                    partialResult[0] = greatestCurrent - smallestCurrent;
                }

                for (int i = 1; i < smallestLength; ++i)
                {
                    greatestCurrent = greatest[i];
                    smallestCurrent = smallest[i];
                    if (carry == 0u)
                    {
                        if (greatestCurrent < smallestCurrent)
                        {
                            partialResult[0] = (numberBase - greatestCurrent) + smallestCurrent;
                            carry = 0u;
                        }
                        else
                        {
                            partialResult[i] = greatestCurrent - smallestCurrent;
                            carry = 1u;
                        }
                    }
                    else if (greatestCurrent <= smallestCurrent)
                    {
                        ++smallestCurrent;
                        partialResult[i] = (numberBase - greatestCurrent) + smallestCurrent;
                        carry = 1u;
                    }
                    else
                    {
                        ++smallestCurrent;
                        partialResult[i] = greatestCurrent - smallestCurrent;
                        carry = 1u;
                    }
                }

                if (carry == 0u)
                {
                    if (greatestLength == smallestLength)
                    {
                        var finalLength = greatestLength;
                        for (int i = greatestLength - 1; i > -1; --i)
                        {
                            if (partialResult[i] == 0)
                            {
                                --finalLength;
                            }
                            else
                            {
                                i = -1;
                            }
                        }

                        if (finalLength == 0)
                        {
                            return null;
                        }
                        else
                        {
                            var result = new uint[finalLength];
                            Array.Copy(partialResult, result, finalLength);
                            return result;
                        }
                    }
                    else
                    {
                        var result = new uint[greatestLength];
                        Array.Copy(partialResult, result, smallestLength);
                        Array.Copy(greatest, smallestLength, result, smallestLength, greatestLength - smallestLength);
                        return result;
                    }
                }
                else
                {
                    // Completa a subtracção a partir dos elementos de maior ordem do primeiro argumento
                    var stopPropagationIndex = -1;
                    for (int i = smallestLength; i < greatestLength; ++i)
                    {
                        if (greatest[i] != 0)
                        {
                            stopPropagationIndex = i;
                            i = greatestLength;
                        }
                    }

                    if (stopPropagationIndex == greatestLength - 1)
                    {
                        greatestCurrent = greatest[stopPropagationIndex];
                        if (greatestCurrent > 1u)
                        {
                            var result = new uint[greatestLength];
                            Array.Copy(partialResult, result, smallestLength);
                            result[stopPropagationIndex] = greatestCurrent - 1;
                            for (int i = smallestLength; i < stopPropagationIndex; ++i)
                            {
                                result[i] = 9u;
                            }

                            return result;
                        }
                        else
                        {
                            var result = new uint[stopPropagationIndex];
                            Array.Copy(partialResult, result, smallestLength);
                            for (int i = smallestLength; i < stopPropagationIndex; ++i)
                            {
                                result[i] = 9u;
                            }

                            return result;
                        }
                    }
                    else
                    {
                        var result = new uint[greatestLength];
                        Array.Copy(partialResult, result, smallestLength);
                        result[stopPropagationIndex]--;
                        ++stopPropagationIndex;
                        Array.Copy(
                            greatest,
                            stopPropagationIndex,
                            result,
                            stopPropagationIndex,
                            greatestLength - stopPropagationIndex);
                        return result;
                    }
                }
            }
        }

        #endregion Funções estáticas privadas
    }
}
