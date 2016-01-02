namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Mantém algums funções de diagnóstico.
    /// </summary>
    public static class Diagnostics
    {
        /// <summary>
        /// Obtém um valor textual com a representação de um determinado número inteiro
        /// em termos dos respectivos bits onde os "bits" mais significantes surgem em primeiro lugar.
        /// </summary>
        /// <param name="value">O valor a ser analisado.</param>
        /// <param name="group">Permite configurar o número de bits que serão agrupados.</param>
        /// <param name="groupSeparator">O separador de grupos.</param>
        /// <returns>A representação textual.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o valor do grupo for negativo.</exception>
        public static string GetBitsReversed(uint value, int group, string groupSeparator = " ")
        {
            if (group < 0)
            {
                throw new ArgumentOutOfRangeException("group");
            }
            else
            {
                var result = string.Empty;
                var temp = value;
                var mask = (uint.MaxValue >> 1) + 1;
                for (int i = 0; i < 32; ++i)
                {
                    var separator = string.Empty;
                    if (group != 0 && i % group == 0)
                    {
                        separator = groupSeparator;
                    }

                    result += separator;
                    result += (temp & mask) == 0 ? "0" : "1";
                    mask >>= 1;
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um valor textual com a representação de um determinado número inteiro
        /// em termos dos respectivos bits onde os "bits" mais significantes surgem em primeiro lugar.
        /// </summary>
        /// <param name="value">O valor a ser analisado.</param>
        /// <param name="group">Permite configurar o número de bits que serão agrupados.</param>
        /// <param name="groupSeparator">O separador de grupos.</param>
        /// <returns>A representação textual.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o valor do grupo for negativo.</exception>
        public static string GetBitsReversed(ulong value, int group, string groupSeparator = " ")
        {
            if (group < 0)
            {
                throw new ArgumentOutOfRangeException("group");
            }
            else
            {
                var result = string.Empty;
                var temp = value;
                var mask = (ulong.MaxValue >> 1) + 1;
                for (int i = 0; i < 64; ++i)
                {
                    var separator = string.Empty;
                    if (group != 0 && i % group == 0)
                    {
                        separator = groupSeparator;
                    }

                    result += separator;
                    result += (temp & mask) == 0 ? "0" : "1";
                    mask >>= 1;
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um valor textual com a representação de um determinado número inteiro
        /// em termos dos respectivos bits onde os "bits" menos significantes surgem em primeiro lugar.
        /// </summary>
        /// <param name="value">O valor a ser analisado.</param>
        /// <param name="group">Permite configurar o número de bits que serão agrupados.</param>
        /// <param name="groupSeparator">O separador de grupos.</param>
        /// <returns>A representação textual.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o valor do grupo for negativo.</exception>
        public static string GetBits(uint value, int group, string groupSeparator = " ")
        {
            if (group < 0)
            {
                throw new ArgumentOutOfRangeException("group");
            }
            else
            {
                var result = string.Empty;
                var temp = value;
                var mask = 1;
                var separator = string.Empty;
                for (int i = 0; i < 32; )
                {
                    result += separator;
                    result += (temp & mask) == 0 ? "0" : "1";
                    mask <<= 1;

                    ++i;
                    separator = string.Empty;
                    if (group != 0 && i % group == 0)
                    {
                        separator = groupSeparator;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um valor textual com a representação de um determinado número inteiro
        /// em termos dos respectivos bits onde os "bits" menos significantes surgem em primeiro lugar.
        /// </summary>
        /// <param name="value">O valor a ser analisado.</param>
        /// <param name="group">Permite configurar o número de bits que serão agrupados.</param>
        /// <param name="groupSeparator">O separador de grupos.</param>
        /// <returns>A representação textual.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o valor do grupo for negativo.</exception>
        public static string GetBits(ulong value, int group, string groupSeparator = " ")
        {
            if (group < 0)
            {
                throw new ArgumentOutOfRangeException("group");
            }
            else
            {
                var result = string.Empty;
                var temp = value;
                var mask = 1ul;
                var separator = string.Empty;
                for (int i = 0; i < 64; )
                {
                    result += separator;
                    result += (temp & mask) == 0 ? "0" : "1";
                    mask <<= 1;

                    ++i;
                    separator = string.Empty;
                    if (group != 0 && i % group == 0)
                    {
                        separator = groupSeparator;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o valor textual com a representação em "bits" de um vector de valores.
        /// Os "bits" são apresentados do menos significante para o mais significante.
        /// </summary>
        /// <remarks>
        /// A representação textual dos "bits" é organizada em agrupamentos definidos pelos
        /// respectivos tamanhos e textos de separação. Se as extremidades de dois grupos
        /// coincidirem então é considerado o separador associado ao último grupo definido.
        /// </remarks>
        /// <param name="value">
        /// O vector de valores do qual se pretende extrair uma representação
        /// em termos de "bits".</param>
        /// <param name="groups">O tamanho dos vários agrupamentos de bits.</param>
        /// <param name="groupSeparators">O separador de grupos.</param>
        /// <returns></returns>
        public static string GetBits(
            uint[] value,
            int[] groups,
            string[] groupSeparators)
        {
            if (groups == null)
            {
                throw new ArgumentNullException("groups");
            }
            else if (groupSeparators == null)
            {
                throw new ArgumentException("groupSeparators");
            }
            else if (groups.Length != groupSeparators.Length)
            {
                throw new ArgumentException("The size of parameters 'groups' and 'groupSeparators' doesn't match.");
            }
            else
            {
                var result = string.Empty;
                var length = value.Length;
                var groupsLength = groups.Length;
                var indexRemainder = 0;
                var separator = string.Empty;
                for (int i = 0; i < length; ++i)
                {
                    var currentValue = value[i];
                    var mask = 1;
                    for (int j = 0; j < 32; ++j)
                    {
                        result += separator;
                        result += (currentValue & mask) == 0 ? "0" : "1";
                        mask <<= 1;
                        ++indexRemainder;
                        separator = string.Empty;
                        for (int k = 0; k < groupsLength; ++k)
                        {
                            if (indexRemainder % groups[k] == 0)
                            {
                                separator = groupSeparators[k];
                            }
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o valor textual com a representação em "bits" de um vector de valores.
        /// Os "bits" são apresentados do menos significante para o mais significante.
        /// </summary>
        /// <remarks>
        /// A representação textual dos "bits" é organizada em agrupamentos definidos pelos
        /// respectivos tamanhos e textos de separação. Se as extremidades de dois grupos
        /// coincidirem então é considerado o separador associado ao último grupo definido.
        /// </remarks>
        /// <param name="value">
        /// O vector de valores do qual se pretende extrair uma representação
        /// em termos de "bits".</param>
        /// <param name="groups">O tamanho dos vários agrupamentos de bits.</param>
        /// <param name="groupSeparators">O separador de grupos.</param>
        /// <returns></returns>
        public static string GetBits(
            ulong[] value,
            int[] groups,
            string[] groupSeparators)
        {
            if (groups == null)
            {
                throw new ArgumentNullException("groups");
            }
            else if (groupSeparators == null)
            {
                throw new ArgumentException("groupSeparators");
            }
            else if (groups.Length != groupSeparators.Length)
            {
                throw new ArgumentException("The size of parameters 'groups' and 'groupSeparators' doesn't match.");
            }
            else
            {
                var result = string.Empty;
                var length = value.Length;
                var groupsLength = groups.Length;
                var indexRemainder = 0;
                var separator = string.Empty;
                for (int i = 0; i < length; ++i)
                {
                    var currentValue = value[i];
                    var mask = 1ul;
                    for (int j = 0; j < 64; ++j)
                    {
                        result += separator;
                        result += (currentValue & mask) == 0 ? "0" : "1";
                        mask <<= 1;
                        ++indexRemainder;
                        separator = string.Empty;
                        for (int k = 0; k < groupsLength; ++k)
                        {
                            if (indexRemainder % groups[k] == 0)
                            {
                                separator = groupSeparators[k];
                            }
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém uma representação do vector de números em termos de "bits" onde os "bits"
        /// mais significantes surgem em primeiro lugar, sendo que os números menos significantes se
        /// encontram nas posições iniciais do vector.
        /// </summary>
        /// <param name="value">O vector.</param>
        /// <param name="groups">Os tamanhos dos grupos de separação de "bits" na representação.</param>
        /// <param name="groupSeparators">Os separadores de cada grupos.</param>
        /// <returns>A representação textual.</returns>
        public static string GetBitsReversed(
            uint[] value,
            int[] groups,
            string[] groupSeparators)
        {
            if (groups == null)
            {
                throw new ArgumentNullException("groups");
            }
            else if (groupSeparators == null)
            {
                throw new ArgumentException("groupSeparators");
            }
            else if (groups.Length != groupSeparators.Length)
            {
                throw new ArgumentException("The size of parameters 'groups' and 'groupSeparators' doesn't match.");
            }
            else
            {
                var result = string.Empty;
                var length = value.Length;
                var groupsLength = groups.Length;
                var indexRemainder = 0;
                var separator = string.Empty;
                for (int i = 0; i < length; ++i)
                {
                    var currentValue = value[i];
                    var mask = 1;
                    for (int j = 0; j < 32; ++j)
                    {
                        result = separator + result;
                        result = ((currentValue & mask) == 0 ? "0" : "1") + result;
                        mask <<= 1;
                        ++indexRemainder;
                        separator = string.Empty;
                        for (int k = 0; k < groupsLength; ++k)
                        {
                            if (indexRemainder % groups[k] == 0)
                            {
                                separator = groupSeparators[k];
                            }
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém uma representação do vector de números em termos de "bits" onde os "bits"
        /// mais significantes surgem em primeiro lugar, sendo que os números menos significantes se
        /// encontram nas posições iniciais do vector.
        /// </summary>
        /// <param name="value">O vector.</param>
        /// <param name="groups">Os tamanhos dos grupos de separação de "bits" na representação.</param>
        /// <param name="groupSeparators">Os separadores de cada grupos.</param>
        /// <returns>A representação textual.</returns>
        public static string GetBitsReversed(
            ulong[] value,
            int[] groups,
            string[] groupSeparators)
        {
            if (groups == null)
            {
                throw new ArgumentNullException("groups");
            }
            else if (groupSeparators == null)
            {
                throw new ArgumentException("groupSeparators");
            }
            else if (groups.Length != groupSeparators.Length)
            {
                throw new ArgumentException("The size of parameters 'groups' and 'groupSeparators' doesn't match.");
            }
            else
            {
                var result = string.Empty;
                var length = value.Length;
                var groupsLength = groups.Length;
                var indexRemainder = 0;
                var separator = string.Empty;
                for (int i = 0; i < length; ++i)
                {
                    var currentValue = value[i];
                    var mask = 1ul;
                    for (int j = 0; j < 64; ++j)
                    {
                        result = separator + result;
                        result = ((currentValue & mask) == 0 ? "0" : "1") + result;
                        mask <<= 1;
                        ++indexRemainder;
                        separator = string.Empty;
                        for (int k = 0; k < groupsLength; ++k)
                        {
                            if (indexRemainder % groups[k] == 0)
                            {
                                separator = groupSeparators[k];
                            }
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a representação hexadecimal de um número.
        /// </summary>
        /// <param name="number">
        /// O número do qual se pretende obter a representação.
        /// </param>
        /// <returns>A representação hexadecimal do número.</returns>
        public static string GetHex(int number)
        {
            return number.ToString("X");
        }

        /// <summary>
        /// Obtém a representação hexadecimal de um número.
        /// </summary>
        /// <param name="number">
        /// O número do qual se pretende obter a representação.
        /// </param>
        /// <returns>A representação hexadecimal do número.</returns>
        public static string GetHex(uint number)
        {
            return number.ToString("X");
        }

        /// <summary>
        /// Obtém a representação hexadecimal de um número.
        /// </summary>
        /// <param name="number">
        /// O número do qual se pretende obter a representação.
        /// </param>
        /// <returns>A representação hexadecimal do número.</returns>
        public static string GetHex(long number)
        {
            return number.ToString("X");
        }

        /// <summary>
        /// Obtém a representação hexadecimal de um número.
        /// </summary>
        /// <param name="number">
        /// O número do qual se pretende obter a representação.
        /// </param>
        /// <returns>A representação hexadecimal do número.</returns>
        public static string GetHex(ulong number)
        {
            return number.ToString("X");
        }

        /// <summary>
        /// Obtém a representação em termos de inteiros de precisão arbitrária.
        /// </summary>
        /// <param name="values">O vector de valore a ser convertido.</param>
        /// <returns>O resultado da conversão.</returns>
        public static BigInteger GetBigIntegerRepresentation(
            uint[] values)
        {
            var result = BigInteger.Zero;
            var i = values.Length - 1;
            if (i > -1)
            {
                result += values[i];

                --i;
                for (; i > -1; --i)
                {
                    result <<= 32;
                    result += values[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a representação em termos de inteiros de precisão arbitrária.
        /// </summary>
        /// <param name="values">O vector de valore a ser convertido.</param>
        /// <param name="length">O compriemnto útil do vector.</param>
        /// <returns>O resultado da conversão.</returns>
        public static BigInteger GetBigIntegerRepresentation(
            uint[] values,
            int length)
        {
            if (length > values.Length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            else
            {
                var result = BigInteger.Zero;
                var i = values.Length - 1;
                if (i > -1)
                {
                    result += values[i];

                    --i;
                    for (; i > -1; --i)
                    {
                        result <<= 32;
                        result += values[i];
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a representação em termos de inteiros de precisão arbitrária.
        /// </summary>
        /// <param name="values">O vector de valore a ser convertido.</param>
        /// <returns>O resultado da conversão.</returns>
        public static BigInteger GetBigIntegerRepresentation(
            ulong[] values)
        {
            var result = BigInteger.Zero;
            var i = values.Length - 1;
            if (i > -1)
            {
                result += values[i];

                --i;
                for (; i > -1; --i)
                {
                    result <<= 64;
                    result += values[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a representação em termos de inteiros de precisão arbitrária.
        /// </summary>
        /// <param name="values">O vector de valore a ser convertido.</param>
        /// <param name="length">O comprimento útil do vector.</param>
        /// <returns>O resultado da conversão.</returns>
        public static BigInteger GetBigIntegerRepresentation(
            ulong[] values,
            int length)
        {
            var result = BigInteger.Zero;
            var i = length - 1;
            if (i > -1)
            {
                result += values[i];

                --i;
                for (; i > -1; --i)
                {
                    result <<= 64;
                    result += values[i];
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a representação de um inteiro de precisão arbitrária como sendo um vector
        /// de inteiros longos, ignorando o sinal.
        /// </summary>
        /// <param name="value">O inteiro de precisão arbitrária.</param>
        /// <returns>A reprsentação como sendo um vector de inteiros longos.</returns>
        public static ulong[] GetUlongArrayRepresentation(BigInteger value)
        {
            var pow = BigInteger.Pow(2, 64);
            var resultList = new List<ulong>();
            var remainder = default(BigInteger);
            var quotient = BigInteger.DivRem(value, pow, out remainder);
            while (!quotient.IsZero)
            {
                resultList.Add((ulong)remainder);
                quotient = BigInteger.DivRem(quotient, pow, out remainder);
            }

            resultList.Add((ulong)remainder);
            return resultList.ToArray();
        }

        /// <summary>
        /// Permite realizar testes intermédios aos valores dos quocientes sucessivos
        /// e dos restos.
        /// </summary>
        /// <param name="divisor">O vector que contém o divisor.</param>
        /// <param name="quotient">O vector que contém o quociente.</param>
        /// <param name="remainder">O vector que contém o resto.</param>
        /// <param name="remainderLength">O número de entradas válidas no resto.</param>
        /// <param name="sign">O sinal associado ao resto.</param>
        /// <returns>O número de precisão arbitrária resultante.</returns>
        public static BigInteger TestQuot(
            ulong[] divisor,
            ulong[] quotient,
            ulong[] remainder,
            int remainderLength,
            bool sign)
        {
            var result = GetBigIntegerRepresentation(
                divisor);
            result *= GetBigIntegerRepresentation(
                quotient);
            if (sign)
            {
                result += GetBigIntegerRepresentation(
                    remainder,
                    remainderLength);
            }
            else
            {
                result -= GetBigIntegerRepresentation(
                       remainder,
                       remainderLength);
            }

            return result;
        }
    }
}
