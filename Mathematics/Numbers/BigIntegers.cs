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
    /// sem sinal.
    /// </summary>
    public struct UlongArrayBigInt
    {
        /// <summary>
        /// Verdadeiro caso o número seja afecto de sinal e falso caso contrário.
        /// </summary>
        private bool sign;

        /// <summary>
        /// O vector que contém a representação do inteiro muito grande com base em longos sem sinal.
        /// </summary>
        private ulong[] array;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="array">O vector de valores que inicializam o número.</param>
        public UlongArrayBigInt(ulong[] array)
        {
            this.sign = false;
            if (array == null)
            {
                this.array = default(ulong[]);
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
            this.sign = false;
            this.array = list.ToArray();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="sign">O sinal do número.</param>
        /// <param name="array">O vector de valores que inicializam o número.</param>
        public UlongArrayBigInt(bool sign, ulong[] array)
        {
            this.sign = sign;
            if (array == null)
            {
                this.array = default(ulong[]);
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
            this.array = list.ToArray();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="UlongArrayBigInt"/>.
        /// </summary>
        /// <param name="numb">O número inteiro.</param>
        public UlongArrayBigInt(int numb)
        {
            if (numb == 0)
            {
                this.sign = true;
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
                this.sign = true;
                this.array = null;
            }
            else
            {
                this.sign = true;
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
                this.sign = true;
                this.array = null;
            }
            else
            {
                this.sign = true;
                this.array = new ulong[] { numb };
            }
        }

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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                            return firstArray[firstArrayLength] < secondArray[firstArrayLength];
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
                            return firstArray[firstArrayLength] < secondArray[firstArrayLength];
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
                        return firstArray[firstArrayLength] > secondArray[firstArrayLength];
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
                        return firstArray[firstArrayLength] > secondArray[firstArrayLength];
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
                    else
                    {
                        var currentRes = DivideByBase(innerText);

                        readed.Add(currentRes.Item2);
                        var alignement = 0;
                        while (currentRes.Item1 != "0")
                        {
                            alignement += 5;
                            alignement %= 64;

                            currentRes = DivideByBase(currentRes.Item1);
                            if (alignement == 0)
                            {
                                readed.Add(currentRes.Item2);
                            }
                            else
                            {
                                AppendUlong(readed, currentRes.Item2, alignement);
                            }
                        }

                        value = new UlongArrayBigInt(sign, readed.ToArray());
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

        #endregion Funções estáticas públicas

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
        /// Premite dividir um número pela base 0x800000000000000.
        /// </summary>
        /// <param name="text">O número a ser dividido.</param>
        /// <returns>O quociente e o resto da divisão.</returns>
        private static Tuple<string, ulong> DivideByBase(string text)
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
        /// Permite adicionar dois números longos sem sinal, determinando o transporte caso esta soma exceda
        /// o tamano da variável.
        /// </summary>
        /// <remarks>
        /// É importante notar que, numa soma, o valor do transporte é sempre unitário.
        /// </remarks>
        /// <param name="first">O primeiro número a ser adicionado.</param>
        /// <param name="second">O segundo número a ser adicionado.</param>
        /// <returns>O par transporte/valor da soma.</returns>
        private static Tuple<bool, ulong> Add(ulong first, ulong second)
        {
            return Tuple.Create((~first | 1) < second, first + second);
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

                    // Troca os valores dos comprimentos com base no algoritmo do "ou exclusivo"
                    firstLength ^= secondLength;
                    secondLength ^= firstLength;
                    firstLength ^= secondLength;
                }

                // Efectua a adição dos vectores inciais
                var partialResult = new ulong[secondLength];
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
        /// <returns></returns>
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
                Parallel.For(0, firstLength, i =>
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
                    else if (carries[i])
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

                    Task.WaitAll(new[] { firstTask, secondTask });
                    return result;
                }
            }
        }

        #endregion Funções estáticas privadas
    }
}
