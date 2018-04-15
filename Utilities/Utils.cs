// -----------------------------------------------------------------------
// <copyright file="Utils.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Globalization;
    using System.Management;
    using System.Numerics;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// Implementa uma série de funções auxiliares.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// O objecto responsável pela sincronização dos fluxos que pretendem obter a informação de sistema.
        /// </summary>
        private static object machineInfoLockObject = new object();

        /// <summary>
        /// Objecto que contém informação sobre a máquina tal como número de processadores e número de núcleos
        /// de processamento.
        /// </summary>
        private static MachineInfo machineStatus = null;

        /// <summary>
        /// Define todos os tipos de símbolos definidos para o <see cref="StringSymbolReader"/>.
        /// </summary>
        private static string[] stringSymbolReaderTypes = {
            "integer","double","double_exponential","string","blancks","equal","double_equal","plus","double_plus","plus_equal",
            "minus","double_minus","minus_equal","times","double_times","times_equal","over","over_equal","exponential","colon",
            "double_colon","bitwise_and","double_and","and_equal","bitwise_or","double_or","or_equal","less_than","double_less","triple_less",
            "less_equal","great_than","double_great","triple_great","great_equal","left_parenthesis","right_parenthesis","left_bracket","right_bracket","double_quote",
            "quote", "left_bar","semi_colon","comma","tild","hat","question_mark","exclamation_mark","left_brace","right_brace",
            "at", "cardinal","dollar","pound","chapter","euro", "underscore", "right_bar", "point", "mod", "space", "new_line",
            "carriage_return", "start_comment", "end_comment", "line_comment", "any", "eof"};

        private static byte[] bitReverseTable256 = 
        {
          0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0, 
          0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8, 
          0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4, 
          0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C, 0xDC, 0x3C, 0xBC, 0x7C, 0xFC, 
          0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72, 0xF2, 
          0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA,
          0x06, 0x86, 0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6, 
          0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE, 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE,
          0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11, 0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1,
          0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39, 0xB9, 0x79, 0xF9, 
          0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
          0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD,
          0x03, 0x83, 0x43, 0xC3, 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3, 
          0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B, 0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB,
          0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57, 0xD7, 0x37, 0xB7, 0x77, 0xF7, 
          0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF, 0x7F, 0xFF
        };

        /// <summary>
        /// Obtém o estado actual da máquina.
        /// </summary>
        public static MachineInfo MachineStatus
        {
            get
            {
                lock (machineInfoLockObject)
                {
                    if (machineStatus == null)
                    {
                        machineStatus = new MachineInfo();
                    }
                }

                return machineStatus;
            }
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="b"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="b">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static byte GetTableDriventReverse(byte b)
        {
            return bitReverseTable256[b];
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="b"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// Esta função inverte a ordem dos bits, aplicando apenas três operações.
        /// Ver: http://graphics.stanford.edu/~seander/bithacks.html.
        /// <remarks>
        /// </remarks>
        /// <param name="b">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static byte Get3OperationReverse(byte b)
        {
            return (byte)((b * 0x0202020202UL & 0x010884422010UL) % 1023);
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="b"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// Esta função inverte a ordem dos bits, aplicando apenas quatro operações.
        /// Ver: http://graphics.stanford.edu/~seander/bithacks.html.
        /// <remarks>
        /// </remarks>
        /// <param name="b">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static byte Get4OperationReverse(byte b)
        {
            return (byte)(((b * 0x80200802UL) & 0x0884422110UL) * 0x0101010101UL >> 32);
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="b"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// Esta função inverte a ordem dos bits, aplicando apenas quatro operações.
        /// Ver: http://graphics.stanford.edu/~seander/bithacks.html.
        /// <remarks>
        /// </remarks>
        /// <param name="b">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static byte Get7OperationReverse(byte b)
        {
            return (byte)(((b * 0x0802UL & 0x22110UL) | (b * 0x8020UL & 0x88440UL)) * 0x10101UL >> 16);
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="s"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="s">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static short GetTableDrivenReverse(short s)
        {
            return bitReverseTable256[s & 0xFF];
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="us"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="us">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static ushort GetTableDrivenReverse(ushort us)
        {
            return bitReverseTable256[us];
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="c"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="c">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static char GetTableDrivenReverse(char c)
        {
            return (char)bitReverseTable256[c];
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="i"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="i">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static int GetTableDrivenReverse(int i)
        {
            var c = (bitReverseTable256[i & 0xff] << 24) |
                (bitReverseTable256[(i >> 8) & 0xff] << 16) |
                (bitReverseTable256[(i >> 16) & 0xff] << 8) |
                (bitReverseTable256[(i >> 24) & 0xff]);
            return c;
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="i"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="i">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static int GetParallelReverse(int i)
        {
            var v = i;

            // swap odd and even bits
            v = ((v >> 1) & 0x55555555) | ((v & 0x55555555) << 1);
            // swap consecutive pairs
            v = ((v >> 2) & 0x33333333) | ((v & 0x33333333) << 2);
            // swap nibbles ... 
            v = ((v >> 4) & 0x0F0F0F0F) | ((v & 0x0F0F0F0F) << 4);
            // swap bytes
            v = ((v >> 8) & 0x00FF00FF) | ((v & 0x00FF00FF) << 8);
            // swap 2-byte long pairs
            v = (v >> 16) | (v << 16);

            return v;
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="ui"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="ui">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static uint GetTableDrivenReverse(uint ui)
        {
            var c = (uint)(bitReverseTable256[ui & 0xff] << 24) |
                (uint)(bitReverseTable256[(ui >> 8) & 0xff] << 16) |
                (uint)(bitReverseTable256[(ui >> 16) & 0xff] << 8) |
                (uint)(bitReverseTable256[(ui >> 24) & 0xff]);
            return c;
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="ui"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="ui">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static uint GetParallelReverse(uint ui)
        {
            var v = ui;

            // swap odd and even bits
            v = ((v >> 1) & 0x55555555) | ((v & 0x55555555) << 1);
            // swap consecutive pairs
            v = ((v >> 2) & 0x33333333) | ((v & 0x33333333) << 2);
            // swap nibbles ... 
            v = ((v >> 4) & 0x0F0F0F0F) | ((v & 0x0F0F0F0F) << 4);
            // swap bytes
            v = ((v >> 8) & 0x00FF00FF) | ((v & 0x00FF00FF) << 8);
            // swap 2-byte long pairs
            v = (v >> 16) | (v << 16);

            return v;
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="l"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="l">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static long GetTableDrivenReverse(long l)
        {
            var c = (bitReverseTable256[l & 0xff] << 56) |
                (bitReverseTable256[(l >> 8) & 0xff] << 48) |
                (bitReverseTable256[(l >> 16) & 0xff] << 40) |
                (bitReverseTable256[(l >> 24) & 0xff] << 32) |
                (bitReverseTable256[l & 0xff] << 32) |
                (bitReverseTable256[(l >> 40) & 0xff] << 16) |
                (bitReverseTable256[(l >> 48) & 0xff] << 8) |
                (bitReverseTable256[(l >> 56) & 0xff]);
            return c;
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="l"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="l">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static long GetParallelReverse(long l)
        {
            var v = l;

            // swap odd and even bits
            v = ((v >> 1) & 0x5555555555555555L) | ((v & 0x5555555555555555L) << 1);
            // swap consecutive pairs
            v = ((v >> 2) & 0x3333333333333333L) | ((v & 0x3333333333333333L) << 2);
            // swap nibbles ... 
            v = ((v >> 4) & 0x0F0F0F0F0F0F0F0FL) | ((v & 0x0F0F0F0F0F0F0F0FL) << 4);
            // swap bytes
            v = ((v >> 8) & 0x00FF00FF00FF00FFL) | ((v & 0x00FF00FF00FF00FFL) << 8);
            // swap 2-byte long pairs
            v = ((v >> 16) & 0x0000FFFF0000FFFFL) | ((v & 0x0000FFFF0000FFFFL) << 16);
            // swap 4-byte long pairs
            v = (v >> 32) | (v << 32);

            return v;
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="ul"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="ul">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static ulong GetTableDrivenReverse(ulong ul)
        {
            var c = (ulong)((bitReverseTable256[ul & 0xff] << 56) |
                (bitReverseTable256[(ul >> 8) & 0xff] << 48) |
                (bitReverseTable256[(ul >> 16) & 0xff] << 40) |
                (bitReverseTable256[(ul >> 24) & 0xff] << 32) |
                (bitReverseTable256[ul & 0xff] << 32) |
                (bitReverseTable256[(ul >> 40) & 0xff] << 16) |
                (bitReverseTable256[(ul >> 48) & 0xff] << 8) |
                (bitReverseTable256[(ul >> 56) & 0xff]));
            return c;
        }

        /// <summary>
        /// Obtém um valor cujos bits são os de <paramref name="ul"/> escritos
        /// na ordem inversa.
        /// </summary>
        /// <param name="ul">O valor proporcionado.</param>
        /// <returns>O valor cujos bits surgem invertidos.</returns>
        public static ulong GetParallelReverse(ulong ul)
        {
            var v = ul;

            // swap odd and even bits
            v = ((v >> 1) & 0x5555555555555555UL) | ((v & 0x5555555555555555UL) << 1);
            // swap consecutive pairs
            v = ((v >> 2) & 0x3333333333333333UL) | ((v & 0x3333333333333333UL) << 2);
            // swap nibbles ... 
            v = ((v >> 4) & 0x0F0F0F0F0F0F0F0FUL) | ((v & 0x0F0F0F0F0F0F0F0FUL) << 4);
            // swap bytes
            v = ((v >> 8) & 0x00FF00FF00FF00FFUL) | ((v & 0x00FF00FF00FF00FFUL) << 8);
            // swap 2-byte long pairs
            v = ((v >> 16) & 0x0000FFFF0000FFFFUL) | ((v & 0x0000FFFF0000FFFFUL) << 16);
            // swap 4-byte long pairs
            v = (v >> 32) | (v << 32);

            return v;
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a esquerda.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static int RotateLeft(int val, int shift)
        {
            return (val << shift) | (val >> (32 - shift));
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a direita.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static int RotateRight(int val, int shift)
        {
            return (val >> shift) | (val << (32 - shift));
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a esquerda.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static uint RotateLeft(uint val, int shift)
        {
            return (val << shift) | (val >> (32 - shift));
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a direita.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static uint RotateRight(uint val, int shift)
        {
            return (val >> shift) | (val << (32 - shift));
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a esquerda.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static long RotateLeft(long val, int shift)
        {
            return (val << shift) | (val >> (64 - shift));
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a direita.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static long RotateRight(long val, int shift)
        {
            return (val >> shift) | (val << (64 - shift));
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a esquerda.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static ulong RotateLeft(ulong val, int shift)
        {
            return (val << shift) | (val >> (64 - shift));
        }

        /// <summary>
        /// Aplica uma rotação cíclica dos bits para a direita.
        /// </summary>
        /// <param name="val">O valor que contém os bits.</param>
        /// <param name="shift">O valor do deslocamento.</param>
        /// <returns>O resultado da rotação.</returns>
        public static ulong RotateRight(ulong val, int shift)
        {
            return (val >> shift) | (val << (64 - shift));
        }

        /// <summary>
        /// Método para preencher um vector com o valor especificado.
        /// </summary>
        /// <typeparam name="T">
        /// O tipo de objectos que constituem as entradas do vector.
        /// </typeparam>
        /// <param name="array">O vector.</param>
        /// <param name="value">O valor.</param>
        public static void FillArray<T>(T[] array, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            else
            {
                var len = array.LongLength;
                if (len > 0)
                {
                    array[0] = value;
                    --len;
                    var size = 1;
                    while (size < len)
                    {
                        Array.Copy(array, 0, array, size, size);
                        len -= size;
                        size <<= 1;
                    }

                    if (len > 0)
                    {
                        Array.Copy(array, 0, array, size, len);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o texto para o tipo de símbolo num leitor <see cref="StringSymbolReader"/> a partir de 
        /// um valor enumerado.
        /// </summary>
        /// <param name="type">O valor enumerado.</param>
        /// <returns>O tipo de símbolo.</returns>
        public static string GetStringSymbolType(EStringSymbolReaderType type)
        {
            return stringSymbolReaderTypes[(int)type];
        }

        /// <summary>
        /// Tenta ler valores a partir do texto.
        /// </summary>
        /// <param name="text">O texto.</param>
        /// <param name="cultureInfo">
        /// O objecto que representa a cultura utilizada na leitura dos números e datas.
        /// </param>
        /// <returns>Os valores.</returns>
        public static object GetTextValue(string text, CultureInfo cultureInfo)
        {
            var integerValue = 0;
            if (int.TryParse(text, out integerValue))
            {
                return integerValue;
            }
            else
            {
                var longValue = 0L;
                if (long.TryParse(text, out longValue))
                {
                    return longValue;
                }
                else
                {
                    var bigInteger = default(BigInteger);
                    if (BigInteger.TryParse(text, out bigInteger))
                    {
                        if (bigInteger < 0)
                        {
                            if (bigInteger >= long.MinValue)
                            {
                                if (bigInteger >= int.MinValue)
                                {
                                    return (int)bigInteger;
                                }
                                else
                                {
                                    return (long)bigInteger;
                                }
                            }
                            else
                            {
                                return bigInteger;
                            }
                        }
                        else if (bigInteger <= ulong.MaxValue)
                        {
                            if (bigInteger <= long.MaxValue)
                            {
                                if (bigInteger <= int.MaxValue)
                                {
                                    return (int)bigInteger;
                                }
                                else
                                {
                                    return (long)bigInteger;
                                }
                            }
                            else
                            {
                                return (ulong)bigInteger;
                            }
                        }
                        else
                        {
                            return bigInteger;
                        }
                    }
                    else
                    {
                        var doubleValue = 0.0;
                        if (double.TryParse(text, NumberStyles.Number, cultureInfo, out doubleValue))
                        {
                            return doubleValue;
                        }
                        else
                        {
                            var decimalValue = 0.0M;
                            if (decimal.TryParse(text, out decimalValue))
                            {
                                return decimalValue;
                            }
                            else
                            {
                                var dateTime = default(DateTime);
                                if (DateTime.TryParse(
                                    text,
                                    cultureInfo.DateTimeFormat,
                                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault,
                                    out dateTime))
                                {
                                    return dateTime;
                                }
                                else
                                {
                                    return text;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Cria um vector de apontadores em código não gerido.
        /// </summary>
        /// <typeparam name="T">O tipo de dados no vector.</typeparam>
        /// <param name="array">O vector.</param>
        /// <returns>Os objectos a serem passados.</returns>
        public static Tuple<IntPtr, IntPtr[]> AllocUnmanagedPointersArray<T>(T[] array)
        {
            if (array == null)
            {
                return null;
            }
            else
            {
                var arrayLength = array.Length;
                var managedPtrArray = new IntPtr[arrayLength];
                var ptrSize = Marshal.SizeOf(typeof(IntPtr));
                var unmanagedArrayPtr = Marshal.AllocHGlobal(ptrSize * arrayLength);

                // Procede à criação dos objectos em código não gerido
                for (int i = 0; i < arrayLength; ++i)
                {
                    var current = array[i];
                    var managedElementPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(T)));
                    managedPtrArray[i] = managedElementPtr;
                    Marshal.StructureToPtr(current, managedElementPtr, false);
                    Marshal.WriteIntPtr(unmanagedArrayPtr, i * ptrSize, managedElementPtr);
                }

                return Tuple.Create(unmanagedArrayPtr, managedPtrArray);
            }
        }

        /// <summary>
        /// Liberta o vector de apontadores reservado na memória não gerida.
        /// </summary>
        /// <param name="arrayPtr">O apontador.</param>
        public static void FreeUnmanagedArray(Tuple<IntPtr, IntPtr[]> arrayPtr)
        {
            if (arrayPtr != null)
            {
                Marshal.FreeHGlobal(arrayPtr.Item1);
                var elements = arrayPtr.Item2;
                var elementsLength = elements.Length;
                for (int i = 0; i < elementsLength; ++i)
                {
                    var current = elements[i];
                    Marshal.FreeHGlobal(current);
                }
            }
        }

        /// <summary>
        /// Obtém a memória virtual disponível em KB.
        /// </summary>
        /// <returns>A memória virtual.</returns>
        public static MemoryInfo GetMemoryInfo()
        {
            var operatingSystemVersion = Environment.OSVersion;
            var platform = operatingSystemVersion.Platform;
            switch (platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    var managementObjectSearcher = new ManagementObjectSearcher(
                        "Select FreePhysicalMemory, FreeVirtualMemory, TotalVisibleMemorySize from Win32_OperatingSystem");
                    var systemCollection = managementObjectSearcher.Get();
                    var systemCollectionEnumerator = systemCollection.GetEnumerator();
                    if (systemCollectionEnumerator.MoveNext())
                    {
                        var freePhysicalMemory = 0UL;
                        var freeVirtualMemory = 0UL;
                        var totalVisibleMemory = 0UL;

                        var currentSearch = systemCollectionEnumerator.Current;
                        var currentValue = currentSearch["FreePhysicalMemory"];
                        freePhysicalMemory = ulong.Parse(currentValue.ToString());

                        currentValue = currentSearch["FreeVirtualMemory"];
                        freeVirtualMemory = ulong.Parse(currentValue.ToString());

                        currentValue = currentSearch["TotalVisibleMemorySize"];
                        totalVisibleMemory = ulong.Parse(currentValue.ToString());

                        return new MemoryInfo()
                        {
                            FreePhysicalMemory = freePhysicalMemory,
                            FreeVirtualMemory = freeVirtualMemory,
                            TotalVisibleMemorySize = totalVisibleMemory
                        };
                    }
                    else
                    {
                        throw new UtilitiesDataException("Can't query the system for processor info.");
                    }
                case PlatformID.MacOSX:
                    throw new UtilitiesException("Unsupported platform: MacOSX");
                case PlatformID.Unix:
                    throw new UtilitiesException("Unsupported platform: Unix");
                case PlatformID.Xbox:
                    throw new UtilitiesException("Unsupported platform: Xbox");
                default:
                    throw new UtilitiesException("Unknown platform.");
            }
        }

        /// <summary>
        /// Obtém a configuração da secção de runtime da máquina.
        /// </summary>
        /// <returns>O objecto que representa a secção.</returns>
        public static RuntimeConfigurationSection GetRuntimeConfiguration()
        {
            var result = new RuntimeConfigurationSection();

            // Estabelecimento do leitor.
            var xmlReader = new RootObjectReader<RuntimeConfigurationSection>(
                "runtime",
                true,
                true,
                true);
            ConfigReader(xmlReader.RootReader);

            // Considera o configurador da máquina
            var config = ConfigurationManager.OpenMachineConfiguration();
            var configSection = (IgnoreSection)config.GetSection("runtime");
            var rawConfig = configSection.SectionInformation.GetRawXml();

            if (!string.IsNullOrWhiteSpace(rawConfig))
            {
                var textReader = new StringReader(rawConfig);
                var reader = XmlReader.Create(textReader);
                xmlReader.ReadRoot(reader, result);
            }

            // Considera o configurador local
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configSection = (IgnoreSection)config.GetSection("runtime");
            rawConfig = configSection.SectionInformation.GetRawXml();

            if (!string.IsNullOrWhiteSpace(rawConfig))
            {
                var textReader = new StringReader(rawConfig);
                var reader = XmlReader.Create(textReader);
                xmlReader.ReadRoot(reader, result);
            }

            return result;
        }

        #region Funções Privadas Auxiliares

        /// <summary>
        /// Configura o leitor para efectuar a leitura.
        /// </summary>
        /// <param name="elementReader">O leitor.</param>
        private static void ConfigReader(
            IXmlElementReader<object, RuntimeConfigurationSection> elementReader)
        {
            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "alwaysFlowImpersonationPolicy",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).AlwaysFlowsImpersonationPolicy),
                (a, v) => ((AlwaysFlowsImpersonationPolicy)a).Enabled = v);

            RegisterElement(
                "appDomainManagerAssembly",
                elementReader,
                t => ((RuntimeConfigurationSection)t).AppDomainManagerAssembly)
            .AttributesReader.RegisterAttribute(
                "value",
                (o, s) => ((AppDomainManagerAssembly)o).Value = s,
                true);

            RegisterElement(
                "appDomainManagerType",
                elementReader,
                t => ((RuntimeConfigurationSection)t).AppDomainManagerType)
            .AttributesReader.RegisterAttribute(
                "value",
                (o, s) => ((AppDomainManagerAssembly)o).Value = s,
                true);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "appDomainResourceMonitoring",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).AppDomainResourceMonitoring),
                (a, v) => ((AppDomainResourceMonitoring)a).Enabled = v);

            var appDomainResReader = RegisterElement(
                    "assemblyBinding",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).AssemblyBinding);
            appDomainResReader.AttributesReader.RegisterAttribute("xmlns", (t, a) => ((AssemblyBinding)t).Xmlns = a, true);
            appDomainResReader.AttributesReader.RegisterAttribute("appliesTo", (t, a) => ((AssemblyBinding)t).AppliesTo = a, false);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "bypassTrustedAppStrongNames",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).BypassTrustedAppStrongNames),
                (a, v) => ((BypassTrustedAppStrongNames)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "developerInstallation",
                RegisterElement(
                    "developmentMode",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).DevelopmentMode),
                (a, v) => ((DevelopmentMode)a).DeveloperInstallation = v);

            RegisterShortAttributeReader(
                    "enabled",
                RegisterElement(
                    "disableCachingBindingFailures",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).DisableCachingBindingFailures),
                (a, v) => ((DisableCachingBindingFailures)a).Enabled = v);

            RegisterShortAttributeReader(
                "enabled",
                RegisterElement(
                    "disableCommitThreadStack",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).DisableCommitThreadStack),
                (a, v) => ((DisableCommitThreadStack)a).Enabled = v);

            RegisterShortAttributeReader(
                "enabled",
                RegisterElement(
                    "disableFusionUpdatesFromADManager",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).DisableFusionUpdatesFromADManager),
                (a, v) => ((DisableFusionUpdatesFromADManager)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "enforceFIPSPolicy",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).EnforceFIPSPolicy),
                (a, v) => ((EnforceFIPSPolicy)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "etwEnable",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).EtwEnable),
                (a, v) => ((EtwEnable)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "forcePerformanceCounterUniqueSharedMemoryReads",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).ForcePerformanceCounterUniqueSharedMemoryReads),
                (a, v) => ((ForcePerformanceCounterUniqueSharedMemoryReads)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "gcAllowVeryLargeObjects",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).GcAllowVeryLargeObjects),
                (a, v) => ((GcAllowVeryLargeObjects)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "gcConcurrent",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).GcConcurrent),
                (a, v) => ((GcConcurrent)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "GCCpuGroup",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).GCCpuGroup),
                (a, v) => ((GCCpuGroup)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "gcServer",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).GcServer),
                (a, v) => ((GcServer)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "generatePublisherEvidence",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).GeneratePublisherEvidence),
                (a, v) => ((GeneratePublisherEvidence)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "legacyImpersonationPolicy",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).LegacyImpersonationPolicy),
                (a, v) => ((LegacyImpersonationPolicy)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "loadFromRemoteSources",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).LoadFromRemoteSources),
                (a, v) => ((LoadFromRemoteSources)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "NetFx40_LegacySecurityPolicy",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).NetFx40_LegacySecurityPolicy),
                (a, v) => ((NetFx40_LegacySecurityPolicy)a).Enabled = v);

            RegisterShortAttributeReader(
                "enabled",
                RegisterElement(
                    "NetFx40_PInvokeStackResilience",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).NetFx40_PInvokeStackResilience),
                (a, v) => ((NetFx40_PInvokeStackResilience)a).Enabled = v);

            RegisterShortAttributeReader(
                "enabled",
                RegisterElement(
                    "NetFx45_CultureAwareComparerGetHashCode_LongStrings",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).NetFx45_CultureAwareComparerGetHashCode_LongStrings),
                (a, v) => ((NetFx45_CultureAwareComparerGetHashCode_LongStrings)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "PreferComInsteadOfManagedRemoting",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).PreferComInsteadOfManagedRemoting),
                (a, v) => ((PreferComInsteadOfManagedRemoting)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "relativeBindForResources",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).RelativeBindForResources),
                (a, v) => ((RelativeBindForResources)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "shadowCopyVerifyByTimestamp",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).ShadowCopyVerifyByTimestamp),
                (a, v) => ((ShadowCopyVerifyByTimestamp)a).Enabled = v);

            var supportPortReader = RegisterElement(
                    "supportPortability",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).SupportPortability);
            RegisterBooleanAttributeReader(
                "enabled",
                supportPortReader,
                (a, v) => ((SupportPortability)a).Enabled = v);
            supportPortReader.AttributesReader.RegisterAttribute(
                "PKT",
                (a, v) => ((SupportPortability)a).Pkt = v,
                true);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "Thread_UseAllCpuGroups",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).Thread_UseAllCpuGroups),
                (a, v) => ((Thread_UseAllCpuGroups)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "ThrowUnobservedTaskExceptions",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).ThrowUnobservedTaskExceptions),
                (a, v) => ((ThrowUnobservedTaskExceptions)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "TimeSpan_LegacyFormatMode",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).TimeSpan_LegacyFormatMode),
                (a, v) => ((TimeSpan_LegacyFormatMode)a).Enabled = v);

            RegisterShortAttributeReader(
                "enabled",
                RegisterElement(
                    "UseRandomizedStringHashAlgorithm",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).UseRandomizedStringHashAlgorithm),
                (a, v) => ((UseRandomizedStringHashAlgorithm)a).Enabled = v);

            RegisterBooleanAttributeReader(
                "enabled",
                RegisterElement(
                    "UseSmallInternalThreadStacks",
                    elementReader,
                    t => ((RuntimeConfigurationSection)t).UseSmallInternalThreadStacks),
                (a, v) => ((UseSmallInternalThreadStacks)a).Enabled = v);
        }

        /// <summary>
        /// Facilita o registo de um elemento.
        /// </summary>
        /// <typeparam name="T">O tipo de objectos que constituem o elemento ascendente.</typeparam>
        /// <typeparam name="P">O tipo dos objectos que constituem o elemento descedente.</typeparam>
        /// <typeparam name="Q">O tipo dos objectos que constituem os elementos </typeparam>
        /// <param name="name">O nome do elemento.</param>
        /// <param name="elementReader">O leitor do elemento ascedente.</param>
        /// <param name="attachAction">A acção associada ao leitor dos descendentes.</param>
        /// <returns>O leitor do elemento descendente.</returns>
        private static IXmlElementReader<T, Q> RegisterElement<T, P, Q>(
            string name,
            IXmlElementReader<T, P> elementReader,
            Func<T, Q> attachAction)
        {
            var result = elementReader.RegisterElementReader<Q>(
                (t, v) => { },
                attachAction,
                name,
                true,
                true,
                true,
                0,
                1);
            return result;
        }

        /// <summary>
        /// Regista um leitor de atributos baseado numa variável verdadeiro/falso.
        /// </summary>
        /// <typeparam name="T">O tipo de objectos que constituem os elementos ascendentes.</typeparam>
        /// <typeparam name="P">O tipo de objectos que constituem os elementos descendentes.</typeparam>
        /// <param name="name">O nome do atributo.</param>
        /// <param name="elementReader">O leitor de elementos a ser configurado.</param>
        /// <param name="action">A acção de atribuição do valor lido.</param>
        private static void RegisterBooleanAttributeReader<T, P>(
            string name,
            IXmlElementReader<T, P> elementReader,
            Action<T, bool> action)
        {
            elementReader.AttributesReader.RegisterAttribute(
                name,
                (t, v) =>
                {
                    var result = default(bool);
                    if (bool.TryParse(v, out result))
                    {
                        action.Invoke(t, result);
                    }
                    else
                    {
                        throw new UtilitiesException("Parse error.");
                    }
                },
                    true);
        }

        /// <summary>
        /// Regista um leitor de atribuitos baseados numa variável inteira de precisão simples.
        /// </summary>
        /// <typeparam name="T">O tipo de objectos que constituem os elementos ascendentes.</typeparam>
        /// <typeparam name="P">O tipo de objectos que constituem os elementos descendentes.</typeparam>
        /// <param name="name">O nome do atributo.</param>
        /// <param name="elementsReader">O leitor de elementos a ser configurado.</param>
        /// <param name="action">A acção de atribuição do valor lido.</param>
        private static void RegisterShortAttributeReader<T, P>(
            string name,
            IXmlElementReader<T, P> elementsReader,
            Action<T, short> action)
        {
            elementsReader.AttributesReader.RegisterAttribute(
                name,
                (t, v) =>
                {
                    var result = default(short);
                    if (short.TryParse(v, out result))
                    {
                        action.Invoke(t, result);
                    }
                    else
                    {
                        throw new UtilitiesException("Parse error.");
                    }
                },
                    true);
        }

        #endregion Funções Privadas Auxiliares
    }

    /// <summary>
    /// Classe que possibilita a consulta do número de processadores e núcleos de processamento
    /// existentes na máquina.
    /// </summary>
    public class MachineInfo
    {
        /// <summary>
        /// Número de processadores.
        /// </summary>
        private int processors;

        /// <summary>
        /// Número de núcleos.
        /// </summary>
        private int cores;

        /// <summary>
        /// A memória física total.
        /// </summary>
        private long totalPhysicalMemory;

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="MachineInfo"/>.
        /// </summary>
        internal MachineInfo()
        {
            this.SetMachineInfo();
        }

        /// <summary>
        /// Obtém o núemro de processdores existentes na máquina.
        /// </summary>
        public int Processors
        {
            get
            {
                return this.processors;
            }
        }

        /// <summary>
        /// Obtém o número de núcleos de processamento existentes na máquina.
        /// </summary>
        public int Cores
        {
            get
            {
                return this.cores;
            }
        }

        /// <summary>
        /// Otbém a memória física total.
        /// </summary>
        private long TotalPhysicalMemory
        {
            get
            {
                return this.totalPhysicalMemory;
            }
        }

        /// <summary>
        /// Estabelece os valores informativos.
        /// </summary>
        private void SetMachineInfo()
        {
            var operatingSystemVersion = Environment.OSVersion;
            var platform = operatingSystemVersion.Platform;
            switch (platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    this.SetWindowsMachineInfo();
                    break;
                case PlatformID.MacOSX:
                    throw new UtilitiesException("Unsupported platform: MacOSX");
                case PlatformID.Unix:
                    throw new UtilitiesException("Unsupported platform: Unix");
                case PlatformID.Xbox:
                    throw new UtilitiesException("Unsupported platform: Xbox");
                default:
                    throw new UtilitiesException("Unknown platform.");
            }
        }

        /// <summary>
        /// Estabelece a informação da máquina em ambiente Windows.
        /// </summary>
        private void SetWindowsMachineInfo()
        {
            // Consulta o número de processadores.
            var managementObjectSearcher = new ManagementObjectSearcher(
                "Select NumberOfProcessors, TotalPhysicalMemory from Win32_ComputerSystem");
            var systemCollection = managementObjectSearcher.Get();
            var systemCollectionEnumerator = systemCollection.GetEnumerator();
            if (systemCollectionEnumerator.MoveNext())
            {
                var currentSearch = systemCollectionEnumerator.Current;
                var currentValue = currentSearch["NumberOfProcessors"];
                this.processors = int.Parse(currentValue.ToString());

                currentValue = currentSearch["TotalPhysicalMemory"];
                this.totalPhysicalMemory = long.Parse(currentValue.ToString());

                // Consulta o número de núcleos de processamento.
                managementObjectSearcher = new ManagementObjectSearcher(
                    "Select NumberOfCores from Win32_Processor");
                systemCollection = managementObjectSearcher.Get();
                var numberOfCores = 0;
                foreach (var systemInfo in systemCollection)
                {
                    numberOfCores += int.Parse(systemInfo["NumberOfCores"].ToString());
                }

                this.cores = numberOfCores;
            }
            else
            {
                throw new UtilitiesDataException("Can't query the system for processor info.");
            }
        }
    }

    /// <summary>
    /// Mantém a informação sobre o estado da memória.
    /// </summary>
    public struct MemoryInfo
    {
        /// <summary>
        /// A quantidade de memória física livre (KB).
        /// </summary>
        private ulong freePhysicalMemory;

        /// <summary>
        /// A quantidade de memória virtual livre (KB).
        /// </summary>
        private ulong freeVirtualMemory;

        /// <summary>
        /// O tamanho total da memória visível (KB).
        /// </summary>
        private ulong totalVisibleMemorySize;

        /// <summary>
        /// Obtém a quantidade de memória física livre (KB).
        /// </summary>
        public ulong FreePhysicalMemory
        {
            get
            {
                return this.freePhysicalMemory;
            }
            internal set
            {
                this.freePhysicalMemory = value;
            }
        }

        /// <summary>
        /// Obtém a quantidade de memória virtual livre.
        /// </summary>
        public ulong FreeVirtualMemory
        {
            get
            {
                return this.freeVirtualMemory;
            }
            internal set
            {
                this.freeVirtualMemory = value;
            }
        }

        /// <summary>
        /// Obtém o tamanho total da memória visível.
        /// </summary>
        public ulong TotalVisibleMemorySize
        {
            get
            {
                return this.totalVisibleMemorySize;
            }
            internal set
            {
                this.totalVisibleMemorySize = value;
            }
        }
    }
}
