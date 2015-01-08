namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        /// Verdadeiro caso o sinal seja positivo e falso caso seja negativo.
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
            this.sign = true;
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
            this.sign = true;
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
                this.sign = numb > 0;
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
                this.sign = true;
                this.array = null;
            }
            else
            {
                this.sign = numb > 0;
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
        /// Obtém o sinal do número em questão.
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
                    var sign = true;
                    var readed = new List<ulong>();

                    var innerText = match.Groups[1].Value;
                    if (string.IsNullOrWhiteSpace(innerText))
                    {
                        sign = true;
                    }
                    else
                    {
                        sign = false;
                    }

                    // Trata os valores
                    var numbBase = 0x400000000000000u;
                    innerText = match.Groups[3].Value;
                    var length = innerText.Length;
                    if (length < 19)
                    {
                        var currentValue = ulong.Parse(innerText);
                        readed.Add(currentValue);
                    }
                    else
                    {
                        var initialText = innerText.Substring(0, 18);
                        var currentValue = ulong.Parse(initialText);
                        var divisionResult = (currentValue / numbBase).ToString();
                        var remainderResult = currentValue % numbBase;
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
                var length = this.array.Length;
                var index = 0;
                var current = this.array[index];
                while (current == 0 && index < length)
                {
                    current = this.array[index];
                    ++index;
                }

                if (index == length)
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
                    }

                    ++index;
                    for (; index < length; ++index)
                    {
                        current = this.array[index];
                        for (int i = 0; i < 64; ++i)
                        {
                            this.DuplicateDecimalRep(decimalRep);
                            if ((current & 0x8000000000000000) == 0x8000000000000000)
                            {
                                this.IncrementDecimalRep(decimalRep);
                            }
                        }
                    }

                    // Imprime o resultado
                    index = 0;
                    var result = decimalRep[index].ToString();
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

                        result += next;
                    }
                }

                return base.ToString();
            }
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
            var numBase = 10000000000000000000;
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
                current = numBase - current;
            }

            decimalRepresentation[index] = current;
            --index;
            while (index >= 0)
            {
                current = decimalRepresentation[index];
                current = (current << 1) + carry;
                if (current < numBase)
                {
                    carry = 0;
                }
                else
                {
                    current = numBase - current;
                }

                decimalRepresentation[index] = current;
            }

            if (carry > 0)
            {
                decimalRepresentation.Insert(0, carry);
            }
        }

        /// <summary>
        /// Premite dividir um número pela base especificada.
        /// </summary>
        /// <param name="text">O número a ser dividido.</param>
        /// <param name="baseNumb">A base.</param>
        /// <returns>O quociente e o resto da divisão.</returns>
        private Tuple<string, ulong> DivideByBase(string text, ulong baseNumb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adiciona um valor ao final da lista, deslocando os "bits" para a esquerda
        /// em uma unidade.
        /// </summary>
        /// <param name="list">A lista à qual se pretende adicionar o valor.</param>
        /// <param name="value">O valor a ser adicionado.</param>
        /// <param name="alignment">O valor do alinhamnto.</param>
        private void AppendUlong(List<ulong> list, ulong value, int alignment)
        {
            var index = list.Count - 1;

            // O valor dos "bits" mais baixos são colocados no valor anterior.
            var lowest = (value & 1) << alignment;
            list[index] = list[index] | lowest;

            var append = value >> (64 - alignment);
            if (append != 0)
            {
                list.Add(append);
            }
        }
    }
}
