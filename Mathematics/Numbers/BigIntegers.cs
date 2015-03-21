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
        /// Mantém o número zero.
        /// </summary>
        private static UlongArrayBigInt zero = new UlongArrayBigInt(0);

        /// <summary>
        /// Mantém o número unitário.
        /// </summary>
        private static UlongArrayBigInt unity = new UlongArrayBigInt(1);

        /// <summary>
        /// Mantém o simétrico da unidade.
        /// </summary>
        private static UlongArrayBigInt symmUnity = new UlongArrayBigInt(-1);

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
            if (array == null)
            {
                this.array = null;
            }
            else
            {
                var length = 0;
                for (int i = array.Length - 1; i > -1; --i)
                {
                    var current = array[i];
                    if (current != 0ul)
                    {
                        length = i + 1;
                        i = -1;
                    }
                }

                if (length == 0)
                {
                    this.array = null;
                }
                else
                {
                    this.array = new ulong[length];
                    Array.Copy(array, this.array, length);
                }

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

        /// <summary>
        /// Obtém um valor que indica se se trata do número zero.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.array == null;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se se trata do número um.
        /// </summary>
        public bool IsOne
        {
            get
            {
                return this.array != null && this.array.Length == 1 && this.array[0] == 1ul && !this.sign;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se se trata de um núemro par.
        /// </summary>
        public bool IsEven
        {
            get
            {
                return this.array == null || (this.array[0] & 1ul) == 0ul;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se se trata de uma potência de dois.
        /// </summary>
        public bool IsPowerOfTwo
        {
            get
            {
                if (this.array == null)
                {
                    return false;
                }
                else
                {
                    var last = this.array.Length - 1;
                    for (int i = 0; i < last; ++i)
                    {
                        if (this.array[i] != 0ul)
                        {
                            return false;
                        }
                    }

                    return MathFunctions.PopCount(this.array[last]) == 1;
                }
            }
        }

        /// <summary>
        /// Obtém o número 0.
        /// </summary>
        public static UlongArrayBigInt Zero
        {
            get
            {
                return zero;
            }
        }

        /// <summary>
        /// Obtém o número 1.
        /// </summary>
        public static UlongArrayBigInt Unity
        {
            get
            {
                return unity;
            }
        }

        /// <summary>
        /// Obtém o número -1.
        /// </summary>
        public static UlongArrayBigInt SymmUnity
        {
            get
            {
                return symmUnity;
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
            if (this.array == null)
            {
                return "0";
            }
            else
            {
                var index = this.array.Length - 1;
                var resultValue = new List<ulong>() { this.array[index] };
                --index;
                for (; index > -1; --index)
                {
                    this.DecimalMultiplyByBinaryPower64(resultValue);
                    this.DecimalRepAdd(resultValue, this.array[index]);
                }

                var result = this.GetDecimalRepresentation(resultValue);
                if (this.sign)
                {
                    result = "-" + result;
                }

                return result;
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
            return SequentialMultiply(first, second);
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

        /// <summary>
        /// Determina o produto entre dois números enormes.
        /// </summary>
        /// <param name="first">O primeiro factor a ser multiplicado.</param>
        /// <param name="second">O segundo factor a ser multiplicado.</param>
        /// <returns>O valor do produto entre os dois números.</returns>
        public static UlongArrayBigInt SequentialMultiply(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (first.array == null || second.array == null)
            {
                var result = new UlongArrayBigInt();
                result.sign = false;
                result.array = null;
                return result;
            }
            else
            {
                var result = new UlongArrayBigInt();
                if ((first.sign && second.sign) || (!first.sign && !second.sign))
                {
                    result.sign = false;
                }
                else
                {
                    result.sign = true;
                }

                result.array = SequentialMultiply(first.array, second.array);
                return result;
            }
        }

        /// <summary>
        /// Determina o quociente e o resto da divisão entre dois números.
        /// </summary>
        /// <param name="first">O dividendo.</param>
        /// <param name="second">O divisor.</param>
        /// <returns>O par quociente/resto da divisão.</returns>
        public static Tuple<UlongArrayBigInt, UlongArrayBigInt> SequentialQuotientAndRemainder(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            if (second.array == null)
            {
                throw new DivideByZeroException();
            }
            else if (first.array == null)
            {
                var result = new UlongArrayBigInt();
                result.sign = false;
                result.array = null;

                // O quociente e o resto são nulos
                return Tuple.Create(result, result);
            }
            else
            {
                var quo = new UlongArrayBigInt();
                var remainder = new UlongArrayBigInt();
                remainder.sign = false;
                if ((first.sign && second.sign) || (!first.sign && !second.sign))
                {
                    quo.sign = false;
                }
                else
                {
                    quo.sign = true;
                }

                var aux = SequentialQuotientAndRemainder(
                    first.array,
                    second.array);
                quo.array = aux.Item1;
                remainder.array = aux.Item2;
                return Tuple.Create(quo, remainder);
            }
        }

        #endregion Funções estáticas públicas

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

        /// <summary>
        /// Permite dividir um número de dois símbolos por um outro de apenas um. Trata-se
        /// da divisão inteira.
        /// </summary>
        /// <remarks>
        /// Na divisão não é efectuada qualquer validação no que concerne ao valor dos
        /// argumentos. A função proporciona casos correctos sempre que o maior valor
        /// do dividendo seja inferior ao divisor.
        /// Esta função serve apenas para efeitos de testes.
        /// </remarks>
        /// <param name="highDividend">O número de maior valor no dividendo.</param>
        /// <param name="lowDividend">O núemro de menor valor no dividendo.</param>
        /// <param name="divisor">O número que constitui o divisor.</param>
        /// <returns>O quociente e o resto da divisão inteira.</returns>
        internal static Tuple<ulong, ulong> InternalDivide(ulong highDividend, ulong lowDividend, ulong divisor)
        {
            return Divide(highDividend, lowDividend, divisor);
        }

        /// <summary>
        /// Efectua a rotação interna à direita, mantendo o tamanho do vector que contém a representação.
        /// </summary>
        /// <param name="value">O vector a ser rodado.</param>
        /// <param name="places">O número de posições a rodar.</param>
        internal static void InternalFixedLengthRotateRight(ulong[] value, int places)
        {
            FixedLengthRotateRight(value, places);
        }

        /// <summary>
        /// Efectua a rotação interna à esquerda, mantendo o tamanho do vector que contém a representação.
        /// </summary>
        /// <param name="value">O vector a ser rodado.</param>
        /// <param name="places">O número de posições a rodar.</param>
        internal static void InternalFixedLengthRotateLeft(ulong[] value, int places)
        {
            FixedLengthRotateLeft(value, places);
        }

        #endregion Funções internas

        #region Funções privadas

        /// <summary>
        /// Função auxiliar que permite escrever a representação textual de um número representado em notação
        /// decimal na base 10000000000000000000.
        /// </summary>
        /// <param name="decimalRep">A representação decimal como vector de longos sem sinal.</param>
        /// <returns>A representação textual do número.</returns>
        private string GetDecimalRepresentation(List<ulong> decimalRep)
        {
            var length = decimalRep.Count;
            var index = length - 1;
            var result = decimalRep[index].ToString();
            if (result.Length > 19)
            {
                result = result.Substring(0, 19);
            }

            --index;
            for (; index > -1; --index)
            {
                var next = decimalRep[index].ToString();
                var additional = 19 - next.Length;
                if (additional > 0)
                {
                    for (int i = 0; i < additional; ++i)
                    {
                        next = "0" + next;
                    }
                }
                else if (additional < 0)
                {
                    next = next.Substring(0, 19);
                }

                result += next;
            }

            return result;
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
            var carries = new List<ulong>();
            if (prod.Item2 == 0ul)
            {
                if (prod.Item1 != 0ul)
                {
                    carries.Add(prod.Item2);
                    carries.Add(prod.Item1);
                }
            }
            else
            {
                carries.Add(prod.Item2);
                if (prod.Item1 != 0ul)
                {
                    carries.Add(prod.Item1);
                }
            }

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
                    secondItem += prod.Item1;
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
        /// Permite dividir um número de dois símbolos por um outro de apenas um. Trata-se
        /// da divisão inteira.
        /// </summary>
        /// <remarks>
        /// Na divisão não é efectuada qualquer validação no que concerne ao valor dos
        /// argumentos. A função proporciona casos correctos sempre que o maior valor
        /// do dividendo seja inferior ao divisor.
        /// </remarks>
        /// <param name="highDividend">O número de maior valor no dividendo.</param>
        /// <param name="lowDividend">O núemro de menor valor no dividendo.</param>
        /// <param name="divisor">O número que constitui o divisor.</param>
        /// <returns>O quociente e o resto da divisão inteira.</returns>
        private static Tuple<ulong, ulong> Divide(ulong highDividend, ulong lowDividend, ulong divisor)
        {
            var symmDivisor = (~divisor) + 1;
            var halfDivisor = divisor >> 1;

            var quoRes = 0ul;
            var remRes = lowDividend;
            if (divisor <= lowDividend)
            {
                quoRes = lowDividend / divisor;
                remRes = lowDividend % divisor;
            }

            var innerHighDividend = highDividend;
            var sign = false;
            var k = symmDivisor / divisor + 1;
            symmDivisor %= divisor;
            if (symmDivisor < halfDivisor)
            {
                while (innerHighDividend > 0ul)
                {
                    var innerQuo = innerHighDividend * k;
                    var innerRem = 0ul;
                    var changeSign = false;
                    if (innerHighDividend < halfDivisor)
                    {
                        var prod = Multiply(innerHighDividend, symmDivisor);
                        innerHighDividend = prod.Item1;
                        innerRem = prod.Item2;
                        if (innerRem > divisor)
                        {
                            innerQuo += innerRem / divisor;
                            innerRem = innerRem % divisor;
                        }
                    }
                    else
                    {
                        innerQuo += symmDivisor;
                        var prod = Multiply(divisor - innerHighDividend, symmDivisor);
                        innerHighDividend = prod.Item1;
                        innerRem = prod.Item2;
                        if (innerRem > divisor)
                        {
                            innerQuo += innerRem / divisor;
                            innerRem = innerRem % divisor;
                        }

                        changeSign = true;
                    }

                    if (sign)
                    {
                        // Subtracção dos restos
                        if (innerRem < remRes)
                        {
                            remRes -= innerRem;
                        }
                        else
                        {
                            --innerQuo;
                            remRes += (divisor - innerRem);
                        }

                        quoRes -= innerQuo;
                    }
                    else
                    {
                        // Adição dos restos
                        var symRes = divisor - innerRem;
                        if (symRes < remRes)
                        {
                            ++innerQuo;
                            remRes -= symRes;
                        }
                        else
                        {
                            remRes += innerRem;
                        }

                        quoRes += innerQuo;
                    }

                    if (changeSign)
                    {
                        sign = !sign;
                    }
                }
            }
            else
            {
                while (innerHighDividend > 0ul)
                {
                    var innerQuo = innerHighDividend * k;
                    var innerRem = 0ul;
                    var changeSign = false;
                    if (innerHighDividend < halfDivisor)
                    {
                        innerQuo += innerHighDividend;
                        var prod = Multiply(innerHighDividend, divisor - symmDivisor);
                        innerHighDividend = prod.Item1;
                        innerRem = prod.Item2;
                        if (innerRem > divisor)
                        {
                            innerQuo += innerRem / divisor;
                            innerRem = innerRem % divisor;
                        }

                        changeSign = true;
                    }
                    else
                    {
                        innerQuo += innerHighDividend;
                        var difference = divisor - symmDivisor;
                        innerQuo += difference;
                        var prod = Multiply(divisor - innerHighDividend, difference);
                        innerHighDividend = prod.Item1;
                        innerRem = prod.Item2;
                        if (innerRem > divisor)
                        {
                            innerQuo += innerRem / divisor;
                            innerRem = innerRem % divisor;
                        }
                    }

                    if (sign)
                    {
                        // Subtracção dos restos
                        if (innerRem < remRes)
                        {
                            remRes -= innerRem;
                        }
                        else
                        {
                            --innerQuo;
                            remRes += (divisor - innerRem);
                        }

                        quoRes -= innerQuo;
                    }
                    else
                    {
                        // Adição dos restos
                        var symRes = divisor - innerRem;
                        if (symRes < remRes)
                        {
                            ++innerQuo;
                            remRes -= symRes;
                        }
                        else
                        {
                            remRes += innerRem;
                        }

                        quoRes += innerQuo;
                    }

                    if (changeSign)
                    {
                        sign = !sign;
                    }
                }
            }

            if (sign)
            {
                remRes = divisor - remRes;
                --quoRes;
            }

            return Tuple.Create(quoRes, remRes);
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
        /// Permite adicionar o número dado pelo segundo argumento ao primeiro.
        /// </summary>
        /// <remarks>
        /// Se o tamanho do vector que contém o primeiro número não for suficiente
        /// para albergar o resultado então este será imprevisível.
        /// </remarks>
        /// <param name="first">O número a ser adicionado.</param>
        /// <param name="firstLength">O comprimento do vector que contém dados válidos.</param>
        /// <param name="second">O segundo número.</param>
        /// <param name="secondOffset">O deslocamento do segundo número.</param>
        /// <returns>O tamanho do vector que contém o valor final da soma.</returns>
        private static int SequentialAdd(
            ulong[] first,
            int firstLength,
            ulong[] second,
            int secondOffset)
        {
            var result = firstLength;
            var secondLength = second.Length;
            var firstIndex = secondOffset;
            var secondIndex = 0;
            var carry = false;
            while (secondIndex < secondLength
                && firstIndex < firstLength)
            {
                var addValue = Add(
                    first[firstIndex],
                    second[secondIndex]);
                if (carry)
                {
                    if (addValue.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        first[firstIndex] = 0;
                        carry = true;
                    }
                    else
                    {
                        first[firstIndex] = addValue.Item2 + 1;
                        carry = addValue.Item1;
                    }
                }
                else
                {
                    first[firstIndex] = addValue.Item2;
                    carry = addValue.Item1;
                }

                ++firstIndex;
                ++secondIndex;
            }

            if (carry)
            {
                while (secondIndex < secondLength)
                {
                    var current = second[secondIndex];
                    if (current == 0xFFFFFFFFFFFFFFFF)
                    {
                        first[firstIndex] = 0;
                        ++firstIndex;
                        ++secondIndex;
                    }
                    else
                    {
                        carry = false;
                        first[firstIndex] = second[secondIndex] + 1;
                        ++firstIndex;
                        ++secondIndex;
                        while (secondIndex < secondLength)
                        {
                            first[firstIndex] = second[secondIndex];
                            ++firstIndex;
                            ++secondIndex;
                        }
                    }
                }

                while (firstIndex < firstLength)
                {
                    var current = first[firstIndex];
                    if (current == 0xFFFFFFFFFFFFFFFF)
                    {
                        first[firstIndex] = 0;
                        ++firstIndex;
                        ++secondIndex;
                    }
                    else
                    {
                        carry = false;
                        ++first[firstIndex];
                        firstIndex = firstLength;
                    }
                }

                if (carry)
                {
                    first[firstIndex] = 1ul;
                    ++result;
                }
            }
            else
            {
                while (secondIndex < secondLength)
                {
                    first[firstIndex] = second[secondIndex];
                    ++firstIndex;
                    ++secondIndex;
                }
            }

            return result;
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
        /// Permite subtrair o número dado pelo segundo argumento ao primeiro.
        /// </summary>
        /// <remarks>
        /// A aplicação da função altera o valor do minuendo, indicando o tamanho
        /// sub-vector com os valores válidos da subtracção. Se o subtraendo for superior
        /// ao minuendo após a aplicação do deslocamento, o resultado da subtracção será imprevisível.
        /// </remarks>
        /// <param name="minuend">O número a ser subtraído.</param>
        /// <param name="minuendValidLength">
        /// O tamanho do sub-vector que contém valores válidos do minuendo.
        /// </param>
        /// <param name="subtraend">O número a subtrair.</param>
        /// <param name="offset">
        /// O deslocamento do subtraendo.
        /// </param>
        /// <returns>O tamanho do sub-vector do minuendo que contém os valores válidos da subtracção.</returns>
        private static int SequentialSubtract(
            ulong[] minuend,
            int minuendValidLength,
            ulong[] subtraend,
            int offset)
        {
            var result = 0;
            var subtraendLength = subtraend.Length;
            var minuendIndex = offset;

            var carry = false;
            var complement = ~subtraend[0];
            var sum = Add(minuend[minuendIndex], complement);
            complement = sum.Item2 + 1;
            if (complement == 0)
            {
                carry = true;
            }
            else
            {
                carry = sum.Item1;
            }

            minuend[minuendIndex] = complement;
            for (int i = 1; i < subtraendLength; ++i)
            {
                ++minuendIndex;
                complement = ~subtraend[i];
                sum = Add(minuend[i], complement);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        minuend[minuendIndex] = 0;
                        carry = true;
                    }
                    else
                    {
                        minuend[minuendIndex] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = minuendIndex;
                    }
                }
                else
                {
                    minuend[minuendIndex] = sum.Item2;
                    carry = sum.Item1;
                    if (sum.Item2 != 0)
                    {
                        result = i;
                    }
                }
            }

            ++minuendIndex;
            for (int i = minuendIndex; i < minuendValidLength; ++i)
            {
                sum = Add(minuend[i], 0xFFFFFFFFFFFFFFFF);
                if (carry)
                {
                    if (sum.Item2 == 0xFFFFFFFFFFFFFFFF)
                    {
                        minuend[i] = 0;
                        carry = true;
                    }
                    else
                    {
                        minuend[i] = sum.Item2 + 1;
                        carry = sum.Item1;
                        result = i;
                    }
                }
                else
                {
                    minuend[i] = sum.Item2;
                    carry = sum.Item1;
                    if (sum.Item2 != 0)
                    {
                        result = i;
                    }
                }
            }

            ++result;
            return result;
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
        /// <param name="first">O primeiro número a ser adicionado.</param>
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
        /// Obtém o produto de dois números enormes, encontrando-se estes representados por vectores
        /// de longos sem sinal.
        /// </summary>
        /// <param name="first">O primeiro número a ser multiplicado.</param>
        /// <param name="second">O segundo número a ser multiplicado.</param>
        /// <returns>O resultado da multiplicação.</returns>
        private static ulong[] SequentialMultiply(ulong[] first, ulong[] second)
        {
            if (first == null || second == null)
            {
                return null;
            }
            else
            {
                var firstLenght = first.Length;
                var secondLength = second.Length;
                var length = firstLenght + secondLength;
                var partialResult = new ulong[length + 1];

                // Multiplicação do primeiro elemento para inicializar os vectores
                var currentFirst = first[0];
                var currentSecond = second[0];
                var prod = Multiply(currentFirst, currentSecond);
                partialResult[0] = prod.Item2;
                var carry = prod.Item1;
                for (int i = 1; i < firstLenght; ++i)
                {
                    currentFirst = first[i];
                    prod = Multiply(currentFirst, currentSecond);

                    var sum = Add(prod.Item2, carry);
                    partialResult[i] = sum.Item2;
                    carry = prod.Item1;

                    // Sendo b a base, o máximo número possível é dado por (b-1)^2=(b-2)b+1, e o transporte associado ao
                    // produto deixa de ser suficiente para gerar um novo transporte
                    if (sum.Item1)
                    {
                        ++carry;
                    }
                }

                partialResult[firstLenght] = carry;

                // Realiza a multiplicação tendo em conta as restantes componentes do segundo factor
                carry = 0ul;
                for (int i = 1; i < secondLength; ++i)
                {
                    currentFirst = first[0];
                    currentSecond = second[i];
                    prod = Multiply(currentFirst, currentSecond);
                    var currentPartial = partialResult[i];
                    var sum = Add(currentPartial, prod.Item2);
                    partialResult[i] = sum.Item2 + carry;
                    carry = prod.Item1;

                    for (int j = 1; j < firstLenght; ++j)
                    {
                        if (sum.Item1)
                        {
                            ++carry;
                        }

                        var innerIndex = i + j;
                        currentFirst = first[j];
                        prod = Multiply(currentFirst, currentSecond);
                        currentPartial = partialResult[innerIndex];
                        sum = Add(currentPartial, prod.Item2);
                        partialResult[innerIndex] = sum.Item2 + carry;
                        carry = prod.Item1;
                    }

                    if (sum.Item1)
                    {
                        ++carry;
                    }
                }

                partialResult[length] = carry;

                var lastIndex = length;
                var lastValue = partialResult[lastIndex];
                while (lastValue == 0ul)
                {
                    --lastIndex;
                    lastValue = partialResult[lastIndex];
                }

                if (lastIndex < length)
                {
                    length = lastIndex + 1;
                    var result = new ulong[length];
                    Array.Copy(partialResult, result, length);
                    return result;
                }
                else
                {
                    return partialResult;
                }
            }
        }

        /// <summary>
        /// Obtém o quociente e o resto da divisão de dois números.
        /// </summary>
        /// <param name="first">O dividendo.</param>
        /// <param name="second">O divisor.</param>
        /// <returns>O par quociente/resto da divisão.</returns>
        private static Tuple<ulong[], ulong[]> SequentialQuotientAndRemainder(
            ulong[] first,
            ulong[] second)
        {
            if (second == null)
            {
                throw new DivideByZeroException();
            }
            else if (first == null)
            {
                return Tuple.Create<ulong[], ulong[]>(null, null);
            }
            else if (second.Length == 1 && second[0] == 1ul)
            {
                // Caso o denominador seja unitário
                var length = first.Length;
                var result = new ulong[length];
                Array.Copy(first, result, length);
                return Tuple.Create<ulong[], ulong[]>(result, null);
            }
            else
            {
                var firstLength = first.Length;
                var secondLength = second.Length;
                if (secondLength < firstLength)
                {
                    throw new NotImplementedException("Encontra-se em fase de implementação");
                }
                else if (firstLength == secondLength)
                {
                    var binaryPowers = MathFunctions.GetUlongPowersOfTwo();

                    var firstLastIndex = first.Length - 1;
                    var secondLastIndex = second.Length - 1;

                    var currentFirstLastLog = MathFunctions.GetHighestSettedBitIndex(
                        first[firstLastIndex]);
                    var currentSecondLastLog = MathFunctions.GetHighestSettedBitIndex(
                        second[secondLastIndex]);

                    if (currentSecondLastLog < currentFirstLastLog)
                    {
                        var logDifference = currentFirstLastLog - currentSecondLastLog;
                        var shiftedDivisor = new ulong[secondLength];
                        AuxiliaryRotateLeft(second, shiftedDivisor, logDifference);

                        var found = -1;
                        var index = firstLength - 1;
                        var currentDividendValue = first[index];
                        var currentDivisorValue = shiftedDivisor[index];
                        if (currentDividendValue == currentDivisorValue)
                        {
                            --index;
                            while (index > -1)
                            {
                                currentDividendValue = first[index];
                                currentDivisorValue = shiftedDivisor[index];
                                if (currentDividendValue == currentDivisorValue)
                                {
                                    --index;
                                }
                                else
                                {
                                    found = index;
                                    index = -1;
                                }
                            }
                        }
                        else
                        {
                            found = index;
                        }

                        // O resto é nulo e o quociente é igual a uma potência de dois
                        if (found < 0)
                        {
                            return Tuple.Create<ulong[], ulong[]>(
                                new ulong[] { binaryPowers[logDifference] },
                                null);
                        }
                        else if (found < firstLength - 1) // O resto torna-se menor que o quociente
                        {
                            var remainder = default(ulong[]);
                            var quo = binaryPowers[logDifference];
                            if (currentDivisorValue < currentDividendValue)
                            {
                                // O resto iguala à diferença entre o dividendo e o divisor rodado
                                remainder = SequentialSubtract(first, shiftedDivisor);
                            }
                            else
                            {
                                //  Resto iguala a diferença entre o divisor rodado e o dividendo
                                remainder = SequentialSubtract(shiftedDivisor, first);
                                --quo;
                            }

                            return Tuple.Create(new ulong[] { quo }, remainder);
                        }
                        else
                        {
                            // Aplica o ciclo que permite determinar o valor do quociente e do resto
                            var remainder = default(ulong[]);
                            var quo = binaryPowers[logDifference];
                            var signal = false;
                            if (currentDivisorValue < currentDividendValue)
                            {
                                remainder = SequentialSubtract(first, shiftedDivisor);
                            }
                            else
                            {
                                remainder = SequentialSubtract(shiftedDivisor, first);
                                signal = true;

                            }

                            var currentDivisionResult = new DivisionResult()
                            {
                                CurrentDividend = remainder,
                                CurrentDivisor = second,
                                CurrentSign = signal,
                                CurrentQuotient = quo
                            };

                            var currentRemainderLength = ApplyGreatestLogDivision(
                                currentDivisionResult,
                                shiftedDivisor,
                                binaryPowers);

                            if (signal)
                            {
                                --currentDivisionResult.CurrentQuotient;
                                currentDivisionResult.CurrentDividend = SequentialSubtract(
                                    second,
                                    currentDivisionResult.CurrentDividend);
                            }

                            if (currentRemainderLength < remainder.Length)
                            {
                                var resultRem = new ulong[currentRemainderLength];
                                Array.Copy(
                                    currentDivisionResult.CurrentDividend, 
                                    resultRem, 
                                    currentRemainderLength);
                                return Tuple.Create(new ulong[] { currentDivisionResult.CurrentQuotient }, resultRem);
                            }
                            else
                            {
                                return Tuple.Create(
                                    new ulong[] { currentDivisionResult.CurrentQuotient }, 
                                    currentDivisionResult.CurrentDividend);
                            }
                        }
                    }
                    else if (currentFirstLastLog < currentSecondLastLog)
                    {
                        // O primeiro número é inferior ao segundo e, portanto,
                        // o resto será igual ao dividendo
                        var remainder = new ulong[firstLength];
                        Array.Copy(first, remainder, firstLength);
                        return Tuple.Create<ulong[], ulong[]>(null, remainder);
                    }
                    else
                    {
                        var compareValue = 0;
                        for (int i = firstLastIndex; i > -1; --i)
                        {
                            var firstCurrent = first[i];
                            var secondCurrent = second[i];
                            if (firstCurrent < secondCurrent)
                            {
                                compareValue = -1;
                                i = -1;
                            }
                            else if (secondCurrent < firstCurrent)
                            {
                                compareValue = 1;
                                i = -1;
                            }
                        }

                        if (compareValue < 0)
                        {
                            // O primeiro número é inferior ao segundo e, portanto,
                            // o resto será igual ao dividendo
                            var remainder = new ulong[firstLength];
                            Array.Copy(first, remainder, firstLength);
                            return Tuple.Create<ulong[], ulong[]>(null, remainder);
                        }
                        else if (compareValue > 0)
                        {
                            // Neste ponto, o quociente é unitário e o resto corresponde à diferença
                            var difference = SequentialSubtract(first, second);
                            return Tuple.Create(new ulong[] { 1ul }, difference);
                        }
                        else
                        {
                            // Ambos os números são iguais,sendo o quociente unitário e o resto nulo
                            return Tuple.Create<ulong[], ulong[]>(new ulong[] { 1ul }, null);
                        }
                    }
                }
                else
                {
                    // O divisor é superior ao dividendo
                    var remainder = new ulong[firstLength];
                    Array.Copy(first, remainder, firstLength);
                    return Tuple.Create<ulong[], ulong[]>(null, remainder);
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
                    var masked = current << counterRotate;
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
                                masked = current << counterRotate;
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
                            masked = current << counterRotate;
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
        /// Permite rodar o vector de valores para a direita, mantendo fixo o
        /// tamanho do vector.
        /// </summary>
        /// <param name="value">O vector a ser rodado.</param>
        /// <param name="places">O número de posições a serem rodadas.</param>
        private static void FixedLengthRotateRight(ulong[] value, int places)
        {
            // Se o valor for negativo, o "bit" correspondente ao sinal é eliminado
            var innerPlaces = places & 0x7FFFFFFF;

            // Inicia o processo de rotação
            var length = value.Length;
            var masterRotate = innerPlaces / 64;
            if (masterRotate < length)
            {
                var innerRotate = innerPlaces % 64;
                var counterRotate = 64 - innerPlaces;
                var writeIndex = 0;
                var index = masterRotate;
                var nextIndex = index + 1;
                while (nextIndex < length)
                {
                    var masked = value[nextIndex] << counterRotate;
                    value[writeIndex] = (value[index] >> innerRotate) | masked;
                    ++index;
                    ++nextIndex;
                    ++writeIndex;
                }

                value[writeIndex++] = value[index] >> innerRotate;
                while (writeIndex < length)
                {
                    value[writeIndex] = 0;
                    ++writeIndex;
                }
            }
            else // O vector passa a ser completamente nulo
            {
                for (int i = 0; i < length; ++i)
                {
                    value[i] = 0;
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
                var finalLength = length + masterRotate;
                if (innerRotate == 0)
                {
                    var result = new ulong[finalLength];
                    for (int i = 0; i < masterRotate; ++i)
                    {
                        result[i] = 0;
                    }

                    var index = 0;
                    for (int i = masterRotate; i < finalLength; ++i)
                    {
                        result[i] = value[index];
                        ++index;
                    }

                    return result;
                }
                else
                {
                    var counterRotate = 64 - innerRotate;
                    var current = value[length - 1];
                    var masked = current >> counterRotate;

                    if (masked == 0)
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
                        result[finalIndex] = current << innerRotate;
                        ++index;
                        ++finalIndex;
                        while (index < length)
                        {
                            current = value[index];
                            result[finalIndex] = (current << innerRotate) | masked;
                            masked = current >> counterRotate;
                            ++index;
                            ++finalIndex;
                        }

                        return result;
                    }
                    else
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
                        result[finalIndex] = current << innerRotate;
                        ++index;
                        ++finalIndex;
                        while (index < length)
                        {
                            current = value[index];
                            result[finalIndex] = (current << innerRotate) | masked;
                            masked = current >> counterRotate;
                            ++index;
                            ++finalIndex;
                        }

                        result[finalIndex] = masked;
                        return result;
                    }
                }
            }
        }

        /// <summary>
        /// Permite rodar o vector de valores para a esquerda, mantendo fixo o
        /// tamanho do vector.
        /// </summary>
        /// <param name="value">O vector a ser rodado.</param>
        /// <param name="places">O número de posições a serem rodadas.</param>
        private static void FixedLengthRotateLeft(ulong[] value, int places)
        {
            // Se o valor for negativo, o "bit" correspondente ao sinal é eliminado
            var innerPlaces = places & 0x7FFFFFFF;

            // Inicia o processo de rotação
            var length = value.Length;
            var masterRotate = innerPlaces / 64;
            if (masterRotate < length)
            {
                var innerRotate = innerPlaces % 64;
                var counterRotate = 64 - innerRotate;
                var writeIndex = length - 1;
                var index = length - masterRotate - 1;
                var nextIndex = index - 1;
                while (nextIndex > -1)
                {
                    var masked = value[nextIndex] >> counterRotate;
                    value[writeIndex] = (value[index] << innerRotate) | masked;
                    --index;
                    --nextIndex;
                    --writeIndex;
                }

                value[writeIndex--] = value[index] << innerRotate;
                while (writeIndex > -1)
                {
                    value[writeIndex] = 0;
                    --writeIndex;
                }
            }
            else // O vector passa a ser completamente nulo
            {
                for (int i = 0; i < length; ++i)
                {
                    value[i] = 0;
                }
            }
        }

        #endregion Funções estáticas privadas

        #region Funções privadas auxiliares

        /// <summary>
        /// Função auxiliar que permite rodar à equerda um vector de longos sem sinal em um número de bits
        /// inferior ao tamanho da variável.
        /// </summary>
        /// <remarks>
        /// Função definida para auxiliar o processo de divisão. Neste caso, o vector passado no argumento
        /// terá de conter pelo menos um valor.
        /// </remarks>
        /// <param name="value">O valor a ser rodado.</param>
        /// <param name="output">O valor de saída.</param>
        /// <param name="places">O número bits a ser aplicado na rotação, inferior a 64.</param>
        private static void AuxiliaryRotateLeft(ulong[] value, ulong[] output, int places)
        {
            if (places == 0)
            {
                Array.Copy(value, output, value.Length);
            }
            else
            {
                var counterRotate = 64 - places;
                var index = value.Length - 1;
                output[index] = value[index] << places;
                for (int i = index - 1; i > -1; --i, --index)
                {
                    var current = value[i];
                    output[index] |= (current >> counterRotate);
                    output[i] = current << places;
                }
            }
        }

        /// <summary>
        /// Divide o divisor ao dividendo, alterando este último parâmetro.
        /// </summary>
        /// <remarks>
        /// Trata-se de uma função auxiliar no processo da divisão sequencial.
        /// </remarks>
        /// <param name="divisionResult">O resultado da divisão actual.</param>
        /// <param name="shiftedDivisor">
        /// Vector que mantém a rotação do divisor. Este argumento existe apenas para evitar
        /// nova alocação de memória.
        /// </param>
        /// <param name="binaryPowers">O vector que contém todas as potências binárias.</param>
        /// <returns>O tamanho do vector dividendo que irá conter resultado do resto.</returns>
        private static int ApplyGreatestLogDivision(
            DivisionResult divisionResult,
            ulong[] shiftedDivisor,
            ulong[] binaryPowers)
        {
            var remainder = divisionResult.CurrentDividend;
            var second = divisionResult.CurrentDivisor;
            var quo = divisionResult.CurrentQuotient;
            var signal = divisionResult.CurrentSign;
            var firstLength = remainder.Length;

            var currentRemainderLength = firstLength;
            var firstLastIndex = remainder.Length - 1;
            var secondLastIndex = second.Length - 1;

            var currentFirstLastLog = MathFunctions.GetHighestSettedBitIndex(
                remainder[firstLastIndex]);
            var currentSecondLastLog = MathFunctions.GetHighestSettedBitIndex(
                second[secondLastIndex]);
            while (currentSecondLastLog <= currentFirstLastLog)
            {
                var logDifference = currentFirstLastLog - currentSecondLastLog;
                AuxiliaryRotateLeft(second, shiftedDivisor, logDifference);

                var found = -1;
                var index = firstLength - 1;
                var currentDividendValue = remainder[index];
                var currentDivisorValue = shiftedDivisor[index];
                if (currentDividendValue == currentDivisorValue)
                {
                    --index;
                    while (index > -1)
                    {
                        currentDividendValue = remainder[index];
                        currentDivisorValue = shiftedDivisor[index];
                        if (currentDividendValue == currentDivisorValue)
                        {
                            --index;
                        }
                        else
                        {
                            found = index;
                            index = -1;
                        }
                    }
                }
                else
                {
                    found = index;
                }

                // O resto é nulo
                if (found < 0)
                {
                    if (signal)
                    {
                        quo -= binaryPowers[logDifference];
                    }
                    else
                    {
                        quo += binaryPowers[logDifference];
                    }

                    divisionResult.CurrentQuotient = quo;
                    divisionResult.CurrentDividend = null;
                    divisionResult.CurrentSign = signal;
                    return 0;
                }
                else if (found < firstLength - 1) // O resto torna-se menor que o quociente
                {
                    if (currentDivisorValue < currentDividendValue)
                    {
                        // O resto iguala à diferença entre o dividendo e o divisor rodado
                        currentRemainderLength = SequentialSubtract(
                            remainder,
                            remainder.Length,
                            shiftedDivisor,
                            0);
                        if (signal)
                        {
                            --quo;
                        }
                        else
                        {
                            ++quo;
                        }
                    }
                    else
                    {
                        //  Resto iguala a diferença entre o divisor rodado e o dividendo
                        currentRemainderLength = SequentialSubtract(
                            shiftedDivisor,
                            shiftedDivisor.Length,
                            remainder,
                            0);
                        remainder = shiftedDivisor;
                        if (signal)
                        {
                            --quo;
                        }
                        else
                        {
                            ++quo;
                        }
                    }

                    if (currentRemainderLength < remainder.Length)
                    {
                        var resRem = new ulong[currentRemainderLength];
                        Array.Copy(remainder, resRem, currentRemainderLength);
                        divisionResult.CurrentQuotient = quo;
                        divisionResult.CurrentDividend = resRem;
                        divisionResult.CurrentSign = signal;
                        return currentRemainderLength;
                    }
                    else
                    {
                        divisionResult.CurrentQuotient = quo;
                        divisionResult.CurrentDividend = remainder;
                        divisionResult.CurrentSign = signal;
                        return currentRemainderLength;
                    }
                }
                else
                {

                    if (currentDivisorValue < currentDividendValue)
                    {
                        if (signal)
                        {
                            quo -= binaryPowers[logDifference];
                        }
                        else
                        {
                            quo += binaryPowers[logDifference];
                        }

                        currentRemainderLength = SequentialSubtract(
                            remainder,
                            remainder.Length,
                            shiftedDivisor, 0);
                    }
                    else
                    {
                        currentRemainderLength = SequentialSubtract(
                            shiftedDivisor,
                            shiftedDivisor.Length,
                            remainder,
                            0);
                        remainder = shiftedDivisor;
                        if (signal)
                        {
                            quo -= binaryPowers[logDifference];
                            signal = false;
                        }
                        else
                        {
                            quo += binaryPowers[logDifference];
                            signal = true;
                        }

                        quo += binaryPowers[logDifference] - 1;

                    }

                    currentFirstLastLog = MathFunctions.GetHighestSettedBitIndex(
                        remainder[firstLastIndex]);
                    currentSecondLastLog = MathFunctions.GetHighestSettedBitIndex(
                        second[secondLastIndex]);
                }
            }

            divisionResult.CurrentQuotient = quo;
            divisionResult.CurrentDividend = remainder;
            divisionResult.CurrentSign = signal;
            return currentRemainderLength;
        }

        #endregion Funções privadas auxiliares

        /// <summary>
        /// Clase que permite auxiliar a passagem de argumentos no algoritmo sequencial
        /// de divisão.
        /// </summary>
        private class DivisionResult
        {
            /// <summary>
            /// O dividendo actual.
            /// </summary>
            private ulong[] currentDividend;

            /// <summary>
            /// O divisor actual.
            /// </summary>
            private ulong[] currentDivisor;

            /// <summary>
            /// O quociente actual.
            /// </summary>
            private ulong currentQuotient;

            /// <summary>
            /// O sinal actual.
            /// </summary>
            private bool currentSign;

            /// <summary>
            /// Obtém ou atribui o dividendo actual.
            /// </summary>
            public ulong[] CurrentDividend
            {
                get
                {
                    return this.currentDividend;
                }
                set { this.currentDividend = value; }
            }

            /// <summary>
            /// Obtém ou atribuir o divisor actual.
            /// </summary>
            public ulong[] CurrentDivisor
            {
                get
                {
                    return this.currentDivisor;
                }
                set
                {
                    this.currentDivisor = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o quociente actual.
            /// </summary>
            public ulong CurrentQuotient
            {
                get
                {
                    return this.currentQuotient;
                }
                set
                {
                    this.currentQuotient = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o sinal actual do dividendo.
            /// </summary>
            public bool CurrentSign
            {
                get
                {
                    return this.currentSign;
                }
                set
                {
                    this.currentSign = value;
                }
            }
        }
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

    /// <summary>
    /// Classe usada apenas para efectuar alguns testes.
    /// </summary>
    public class TestWithAlg
    {
        /// <summary>
        /// Permite dividir um número de dois símbolos por um número de um símbolo.
        /// </summary>
        /// <param name="highDividend">O símbolo mais significativo do dividendo.</param>
        /// <param name="lowDividend">O símbolo menos significativo do dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>Os símbolos mais e menos significativo do resultado.</returns>
        public Tuple<ulong, ulong> Divide(ulong highDividend, ulong lowDividend, ulong divisor)
        {
            var intermediary = highDividend * 10 + lowDividend;
            var quo = intermediary / divisor;
            var rem = intermediary % divisor;
            return Tuple.Create(quo, rem);
        }

        /// <summary>
        /// Aplica o processo de divisão.
        /// </summary>
        /// <param name="first">O dividendo.</param>
        /// <param name="second">O divisor.</param>
        /// <returns>O quociente.</returns>
        public Tuple<ulong[], ulong[]> SequentialQuotientAndRemainder(
            ulong[] first,
            ulong[] second)
        {
            if (second == null)
            {
                throw new DivideByZeroException();
            }
            else if (first == null)
            {
                return null;
            }
            else if (second.Length == 1 && second[0] == 1ul)
            {
                // Caso o denominador seja unitário
                var length = first.Length;
                var result = new ulong[length];
                Array.Copy(first, result, length);
                return Tuple.Create<ulong[], ulong[]>(result, null);
            }
            else
            {
                var firstLength = first.Length;
                var secondLength = second.Length;
                if (firstLength < secondLength)
                {
                    var quoResult = default(ulong[]);
                    var remResult = new ulong[firstLength];
                    Array.Copy(first, remResult, firstLength);
                    return Tuple.Create(quoResult, remResult);
                }
                else
                {
                    var lengthDiff = firstLength - secondLength;
                    var firstIndex = firstLength - 1;
                    for (var secondIndex = secondLength - 1; secondIndex > -1; --secondIndex)
                    {
                        var currentFirst = first[firstIndex];
                        var currentSecond = second[secondIndex];
                        if (currentSecond < currentFirst)
                        {
                            secondIndex = -1;
                            firstIndex = lengthDiff;
                        }
                        else if (currentFirst < currentSecond)
                        {
                            secondIndex = -1;
                            firstIndex = lengthDiff - 1;
                        }

                        --firstIndex;
                    }

                    if (firstIndex < 0)
                    {
                        // O divisor é menor que o quociente
                        var quoResult = default(ulong[]);
                        var remResult = new ulong[firstLength];
                        Array.Copy(first, remResult, firstLength);
                        return Tuple.Create(quoResult, remResult);
                    }
                    else
                    {
                        var quoList = new List<ulong>();
                        var remResult = new ulong[secondLength];
                        var carryRem = new ulong[secondLength];
                        var lastRem = 0ul;
                        var lastDividendIndex = firstLength - 1;
                        if (firstIndex == lengthDiff - 1)
                        {
                            // Caso o primeiro coeficiente do dividendo seja inferior
                            // ao primeiro coeficiente do divisor
                            lastRem = first[firstLength - 1];
                            --lastDividendIndex;
                        }

                        // Testa a primeira divisão
                        int i = secondLength - 1;
                        var k = lastDividendIndex;
                        var testDivision = default(ulong);
                        if (lastRem == 0)
                        {
                            var innerCurrentDividend = first[lastDividendIndex];
                            var innerCurrentDivisor = second[i];
                            testDivision = innerCurrentDividend / innerCurrentDivisor;
                            var innerRem = innerCurrentDividend % innerCurrentDivisor;

                            carryRem[i] = testDivision;
                            remResult[i] = innerCurrentDividend - innerRem;

                        }
                        else
                        {
                            var innerCurrentDividend = first[lastDividendIndex];
                            var divisionResult = Divide(lastRem, first[lastDividendIndex], second[i]);
                            testDivision = divisionResult.Item1;
                            if (divisionResult.Item2 < innerCurrentDividend)
                            {
                                carryRem[i] = testDivision;
                                remResult[i] = innerCurrentDividend - divisionResult.Item2;
                            }
                            else
                            {
                                carryRem[i] = testDivision - 1;
                                remResult[i] = innerCurrentDividend + (10 - divisionResult.Item2);
                            }
                        }

                        var stopIndex = -1;
                        for (; i > -1; --i, --k)
                        {

                        }

                        while (firstIndex > -1)
                        {



                            --firstIndex;
                            --lastDividendIndex;
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
