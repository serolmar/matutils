namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Text.RegularExpressions;
    using Mathematics;

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

        public static UlongArrayBigInt operator +(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        public static UlongArrayBigInt operator *(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        public static UlongArrayBigInt operator -(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        public static UlongArrayBigInt operator /(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        public static UlongArrayBigInt operator %(UlongArrayBigInt first, UlongArrayBigInt second)
        {
            throw new NotImplementedException();
        }

        #endregion Sobrecarga de operadores

        #region Funções estáticas

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
                    if (!string.IsNullOrEmpty(innerText))
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
                    }

                    value = new UlongArrayBigInt(sign, readed.ToArray());
                    return true;
                }
                else
                {
                    value = default(UlongArrayBigInt);
                    return false;
                }
            }
        }

        #endregion

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

        #endregion Funções estáticas privadas
    }
}
