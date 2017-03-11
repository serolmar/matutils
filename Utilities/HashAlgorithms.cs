// -----------------------------------------------------------------------
// <copyright file="ULongEqualityComparers.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Define uma função de confusão de um objecto com um número especificado de bits.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto do qual se pretende obter o código confuso.</typeparam>
    public interface IHashN<T>
    {
        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="bytes">O número de bytes no código confuso.</param>
        /// <returns>O código confuso.</returns>
        BigInteger GetHash(T obj, int bytes);
    }

    /// <summary>
    /// Define uma função de confusão de um objecto com 32 bits.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto do qual se pretende obter o código confuso.</typeparam>
    public interface IHash32<T>
    {
        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        uint GetHash32(T obj);
    }

    /// <summary>
    /// Define uma função de confusão de um objecto com 128 bits.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto do qual se pretende obter o código confuso.</typeparam>
    public interface IHash64<T>
    {
        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        ulong GetHash64(T obj);
    }

    /// <summary>
    /// Implementa o código confuso CRC de 32 bits.
    /// </summary>
    /// <remarks>
    /// Transcrição de: https://github.com/damieng/DamienGKit/blob/master/CSharp/DamienG.Library/Security/Cryptography/Crc32.cs
    /// </remarks>
    public class Crc32 : HashAlgorithm, IHash32<byte[]>
    {
        /// <summary>
        /// Mantém o polinómio por defeito.
        /// </summary>
        private static uint defaultPolynomial = 0xedb88320u;

        /// <summary>
        /// Mantém a semente por defeito.
        /// </summary>
        public static uint defaultSeed = 0xffffffffu;

        /// <summary>
        /// Mantém a tabela por defeito.
        /// </summary>
        private static uint[] defaultTable;

        /// <summary>
        /// A semente.
        /// </summary>
        private readonly uint seed;

        /// <summary>
        /// A tabela.
        /// </summary>
        private readonly uint[] table;

        /// <summary>
        /// O código confuso.
        /// </summary>
        private uint hash;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Crc32"/>.
        /// </summary>
        public Crc32()
            : this(defaultPolynomial, defaultSeed)
        {
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="Crc32"/>.
        /// </summary>
        /// <param name="polynomial">O polinómio.</param>
        /// <param name="seed">A semente.</param>
        public Crc32(uint polynomial, uint seed)
        {
            table = this.InitializeTable(polynomial);
            this.seed = this.hash = seed;
        }

        /// <summary>
        /// Obtém o tamanho do código confuso.
        /// </summary>
        public override int HashSize { get { return 32; } }

        /// <summary>
        /// Inicializa a implementação da classe <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        public override void Initialize()
        {
            this.hash = seed;
        }

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var hash = this.ComputeHash(
                    obj,
                    0,
                    obj.Length);
                return BitConverter.ToUInt32(hash, 0);
            }
        }

        /// <summary>
        /// Direcciona os dados escritos no objecto para o algoritmo de obtenção do código
        /// confuso.
        /// </summary>
        /// <param name="array">O vector do qual se pretende a determinação do código confuso.</param>
        /// <param name="ibStart">O índice incial no vector.</param>
        /// <param name="cbSize">O tamanho do subvector a ser considerado.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            this.hash = this.CalculateHash(table, this.hash, array, ibStart, cbSize);
        }

        /// <summary>
        /// Finaliza o cálculo do código confuso.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override byte[] HashFinal()
        {
            var hashBuffer = this.UInt32ToBigEndianBytes(~this.hash);
            this.HashValue = hashBuffer;
            return hashBuffer;
        }

        #region Funções privadas

        /// <summary>
        /// Determina o código confuso a partir da semente definida por defeito.
        /// </summary>
        /// <param name="buffer">O amortecedor do cálculo.</param>
        /// <returns>O código confuso calculado.</returns>
        private uint Compute(byte[] buffer)
        {
            return this.Compute(defaultSeed, buffer);
        }

        /// <summary>
        /// Determina o código confuso a partir da semente especificada, utilizando o polinómio por defeito.
        /// </summary>
        /// <param name="seed">A semente.</param>
        /// <param name="buffer">O amortecedor do cálculo.</param>
        /// <returns>O código confuso calculado.</returns>
        public uint Compute(uint seed, byte[] buffer)
        {
            return Compute(defaultPolynomial, seed, buffer);
        }

        /// <summary>
        /// Determina o código confuso a partir da semente especificada, utilizando o polinómio especificado.
        /// </summary>
        /// <param name="polynomial">O polinómio.</param>
        /// <param name="seed">A semente.</param>
        /// <param name="buffer">O amortecedor do cálculo.</param>
        /// <returns>O código confuso calculado.</returns>
        public uint Compute(uint polynomial, uint seed, byte[] buffer)
        {
            return ~this.CalculateHash(
                this.InitializeTable(polynomial),
                seed,
                buffer,
                0,
                buffer.Length);
        }

        /// <summary>
        /// Inicializa a tabela.
        /// </summary>
        /// <param name="polynomial">O polinómio.</param>
        /// <returns>A tabela.</returns>
        private uint[] InitializeTable(uint polynomial)
        {
            if (polynomial == defaultPolynomial && defaultTable != null)
            {
                return defaultTable;
            }
            else
            {
                var createTable = new uint[256];
                for (var i = 0; i < 256; i++)
                {
                    var entry = (uint)i;
                    for (var j = 0; j < 8; j++)
                    {
                        if ((entry & 1) == 1)
                        {
                            entry = (entry >> 1) ^ polynomial;
                        }
                        else
                        {
                            entry = entry >> 1;
                        }
                    }

                    createTable[i] = entry;
                }

                if (polynomial == defaultPolynomial)
                {
                    defaultTable = createTable;
                }

                return createTable;
            }
        }

        /// <summary>
        /// Calcula o código confuso.
        /// </summary>
        /// <param name="table">A tabela a ser utilizada no cálculo.</param>
        /// <param name="seed">A semente.</param>
        /// <param name="buffer">O amortecedor para o cálculo.</param>
        /// <param name="start">A posição inicial.</param>
        /// <param name="size">A posição final.</param>
        /// <returns>O código confuso.</returns>
        private uint CalculateHash(
            uint[] table,
            uint seed,
            IList<byte> buffer,
            int start,
            int size)
        {
            var hash = seed;
            for (var i = start; i < start + size; i++)
            {
                hash = (hash >> 8) ^ table[buffer[i] ^ hash & 0xff];
            }

            return hash;
        }

        /// <summary>
        /// Obtém os bytes na ordem correcta.
        /// </summary>
        /// <param name="uint32">O valor que contém os bytes.</param>
        /// <returns>Os bytes cujos bits se encontram na ordem correcta.</returns>
        private byte[] UInt32ToBigEndianBytes(uint uint32)
        {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }

            return result;
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Define o "City Hash".
    /// </summary>
    /// <remarks>
    /// https://github.com/google/cityhash/blob/master/src/city.cc
    /// </remarks>
    public class CityHash :
        IHash32<string>,
        IHash32<byte[]>,
        IHash64<string>,
        IHash64<byte[]>,
        IHashN<string>,
        IHashN<byte[]>
    {
        #region Campos

        /// <summary>
        /// Número primo.
        /// </summary>
        private const ulong k0 = 0xc3a5c85c97cb3127UL;

        /// <summary>
        /// Número primo.
        /// </summary>
        private const ulong k1 = 0xb492b66fbe98f273UL;

        /// <summary>
        /// Número primo.
        /// </summary>
        private const ulong k2 = 0x9ae16a3b2f90404fUL;

        /// <summary>
        /// Número mágico.
        /// </summary>
        private const uint c1 = 0xcc9e2d51U;

        /// <summary>
        /// Número mágico.
        /// </summary>
        private const uint c2 = 0x1b873593U;

        /// <summary>
        /// O objecto responsável pelo cálculo de códigos confusos CRC32.
        /// </summary>
        private IHash32<byte[]> crc32Hash = new Crc32();

        #endregion Campos

        #region Funções públicas

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(string obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                return this.CityHash32(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return this.CityHash32(obj, 0, obj.Length);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(string obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                return this.CityHash64(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(byte[] obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return this.CityHash64(obj, 0, obj.Length);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="seed">A semente.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64WithSeed(string obj, ulong seed)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                return this.CityHash64WithSeed(bytes, seed);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="seed">A semente.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64WithSeed(byte[] obj, ulong seed)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return this.CityHash64WithSeed(obj, seed);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="seed0">A primeira semente.</param>
        /// <param name="seed1">A segunda semente.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64WithSeeds(string obj, ulong seed0, ulong seed1)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                return this.CityHash64WithSeeds(bytes, seed0, seed1);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="seed0">A primeira semente.</param>
        /// <param name="seed1">A segunda semente.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64WithSeeds(byte[] obj, ulong seed0, ulong seed1)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return this.CityHash64WithSeeds(obj, seed0, seed1);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 128 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash128(string obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                var hash = this.CityHash128(bytes);
                return (((BigInteger)hash.Item2) << 64) + hash.Item2;
            }
        }

        /// <summary>
        /// Obtém o código confuso de 128 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash128(byte[] obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var hash = this.CityHash128(obj);
                return (((BigInteger)hash.Item2) << 64) + hash.Item2;
            }
        }

        /// <summary>
        /// Obtém o código confuso de 128 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="low">O valor com os bits menos significativos da semenete.</param>
        /// <param name="high">O valor com os bits mais significativos da semente.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash128WithSeed(
            string obj,
            ulong low,
            ulong high)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                var hash = this.CityHash128WithSeed(bytes, 0, bytes.Length, low, high);
                return (((BigInteger)hash.Item2) << 64) + hash.Item2;
            }
        }

        /// <summary>
        /// Obtém o código confuso de 128 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <param name="low">O valor com os bits menos significativos da semenete.</param>
        /// <param name="high">O valor com os bits mais significativos da semente.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash128WithSeed(
            byte[] obj,
            ulong low,
            ulong high)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var hash = this.CityHash128WithSeed(obj, 0, obj.Length, low, high);
                return (((BigInteger)hash.Item2) << 64) + hash.Item2;
            }
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="bytes">O número de bytes no código confuso.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash(string obj, int bytes)
        {
            if (bytes == 4)
            {
                return this.GetHash32(obj);
            }
            else if (bytes == 8)
            {
                return this.GetHash64(obj);
            }
            else if (bytes == 16)
            {
                return this.GetHash128(obj);
            }
            else
            {
                throw new NotSupportedException("Hash bytes number is not supported.");
            }
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="bytes">O número de bytes no código confuso.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash(byte[] obj, int bytes)
        {
            if (bytes == 4)
            {
                return this.GetHash32(obj);
            }
            else if (bytes == 8)
            {
                return this.GetHash64(obj);
            }
            else if (bytes == 16)
            {
                return this.GetHash128(obj);
            }
            else
            {
                throw new NotSupportedException("Hash bytes number is not supported.");
            }
        }

        #endregion Funções privadas

        #region Funções privadas

        /// <summary>
        /// Obtém os bits dos carácteres que se encontram nas posições
        /// identificadas pelo índice num valor inteiro.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice do primeiro carácter a ser considerado.</param>
        /// <returns>O valor inteiro.</returns>
        private uint Fetch32(byte[] value, int index)
        {
            return BitConverter.ToUInt32(value, index);
        }

        /// <summary>
        /// Obtém os bits dos carácteres que se encontram nas posições identificadas
        /// pelo índice num valor inteiro.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice do primeiro carácter a ser considerado.</param>
        /// <returns>O valor inteiro.</returns>
        private ulong Fetch64(byte[] value, int index = 0)
        {
            return BitConverter.ToUInt64(value, index);
        }

        /// <summary>
        /// Roda os bits do valor em um número especificado.
        /// </summary>
        /// <param name="val">O valor cujos bits serão rodados.</param>
        /// <param name="shift">A dimensão da rotação.</param>
        /// <returns>O resultado da rotação.</returns>
        private uint Rotate32(uint val, int shift)
        {
            return shift == 0 ? val : ((val >> shift) | (val << (32 - shift)));
        }

        /// <summary>
        /// Roda os bits do valor em um número especificado.
        /// </summary>
        /// <param name="val">O valor cujos bits serão rodados.</param>
        /// <param name="shift">A dimensão da rotação.</param>
        /// <returns>O resultado da rotação.</returns>
        private ulong Rotate(ulong val, int shift)
        {
            return shift == 0 ? val : ((val >> shift) | (val << (64 - shift)));
        }


        /// <summary>
        /// Um código confuso de inteiros, copiado do Murmur.
        /// </summary>
        /// <param name="h">O inteiro do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso do inteiro.</returns>
        private uint Fmix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }

        /// <summary>
        /// Troca a ordenação dos bits.
        /// </summary>
        /// <param name="value">O valor que contém os bits.</param>
        /// <returns>O resultado da troca.</returns>
        private uint BSwap32(uint value)
        {
            return
                (value >> 24) |
                (value & 0x00ff0000) >> 8 |
                (value & 0x0000ff00) << 8 |
                (value << 24);
        }

        /// <summary>
        /// Troca a ordenação dos bits.
        /// </summary>
        /// <param name="value">O valor que contém os bits.</param>
        /// <returns>O resultado da troca.</returns>
        private ulong BSwap64(ulong value)
        {
            return
                (value >> 56) |
                (value & 0x00ff000000000000) >> 40 |
                (value & 0x0000ff0000000000) >> 24 |
                (value & 0x000000ff00000000) >> 8 |
                (value & 0x00000000ff000000) << 8 |
                (value & 0x0000000000ff0000) << 24 |
                (value & 0x000000000000ff00) << 40 |
                (value << 56);
        }

        /// <summary>
        /// Combina dois valores de 32 bits.
        /// </summary>
        /// <param name="a">O primeiro valor a ser combinado.</param>
        /// <param name="h">O segundo valor a ser combinado.</param>
        /// <returns>O resultado da combinação.</returns>
        private uint Mur(uint a, uint h)
        {
            a *= c1;
            a = Rotate32(a, 17);
            a *= c2;
            h ^= a;
            h = Rotate32(h, 19);
            return h * 5 + 0xe6546b64;
        }

        /// <summary>
        /// Obtém o código confuso para valores textuais cujo comprimento
        /// esteja compreendido entre 13 e 24.
        /// </summary>
        /// <param name="value">O valor textual do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private uint Hash32Len13to24(byte[] value, int index, int len)
        {
            var a = this.Fetch32(value, index + (len >> 1) - 4);
            var b = this.Fetch32(value, index + 4);
            var c = this.Fetch32(value, index + len - 8);
            var d = this.Fetch32(value, index + len >> 1);
            var e = this.Fetch32(value, 0);
            var f = this.Fetch32(value, index + len - 4);
            var h = (uint)len;

            return this.Fmix(this.Mur(
                f,
                this.Mur(e,
                this.Mur(d,
                this.Mur(c,
                this.Mur(b,
                this.Mur(a, h)))))));
        }

        /// <summary>
        /// Obtém o código confuso para valores textuais cujo comprimento
        /// esteja compreendido entre 0 e 4.
        /// </summary>
        /// <param name="value">O vector de bits do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private uint Hash32Len0to4(byte[] value, int index, int len)
        {
            var b = 0U;
            var c = 9U;

            for (var i = index; i < len; ++i)
            {
                var v = (sbyte)value[i];
                b = b * c1 + (uint)v;
                c ^= b;
            }

            return this.Fmix(this.Mur(b,
                this.Mur((uint)len, c)));
        }

        /// <summary>
        /// Obtém o código confuso para valores textuais cujo comprimento
        /// esteja compreendido entre 5 e 12.
        /// </summary>
        /// <param name="value">O vector de bits do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private uint Hash32Len5to12(byte[] value, int index, int len)
        {
            var a = (uint)len;
            var b = (uint)len * 5;
            var c = 9U;
            var d = b;
            a += this.Fetch32(value, index);
            b += this.Fetch32(value, index + len - 4);
            c += this.Fetch32(value, index + (len >> 1) & 4);

            return this.Fmix(this.Mur(c,
                this.Mur(b,
                this.Mur(a, d))));
        }

        /// <summary>
        /// Calcula o código confuso do valor textual.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private uint CityHash32(byte[] value, int index, int len)
        {
            if (len <= 4)
            {
                return this.Hash32Len0to4(value, index, len);
            }
            else if (len <= 12)
            {
                return this.Hash32Len5to12(value, index, len);
            }
            else if (len <= 24)
            {
                return this.Hash32Len13to24(value, index, len);
            }
            else
            {
                var h = (uint)len;
                var g = c1 * h;
                var f = g;

                var a0 = this.Rotate32(this.Fetch32(value, index + len - 4) * c1, 17) * c2;
                var a1 = this.Rotate32(this.Fetch32(value, index + len - 8) * c1, 17) * c2;
                var a2 = this.Rotate32(this.Fetch32(value, index + len - 16) * c1, 17) * c2;
                var a3 = this.Rotate32(this.Fetch32(value, index + len - 12) * c1, 17) * c2;
                var a4 = this.Rotate32(this.Fetch32(value, index + len - 20) * c1, 17) * c2;

                h ^= a0;
                h = this.Rotate32(h, 19);
                h = h * 5 + 0xe6546b64;
                h ^= a2;
                h = Rotate32(h, 19);
                h = h * 5 + 0xe6546b64;
                g ^= a1;
                g = this.Rotate32(
                    g,
                    19);
                g = g * 5 + 0xe6546b64;
                g ^= a3;
                g = this.Rotate32(g, 19);
                g = g * 5 + 0xe6546b64;
                f += a4;
                f = this.Rotate32(f, 19);
                f = f * 5 + 0xe6546b64;

                var iters = (len - 1) / 20;
                var pointer = 0;
                do
                {
                    a0 = this.Rotate32(this.Fetch32(value, pointer) * c1, 17) * c2;
                    a1 = this.Fetch32(value, pointer + 4);
                    a2 = this.Rotate32(this.Fetch32(value, pointer + 8) * c1, 17) * c2;
                    a3 = this.Rotate32(this.Fetch32(value, pointer + 12) * c1, 17) * c2;
                    a4 = this.Fetch32(value, pointer + 16);

                    h ^= a0;
                    h = this.Rotate32(h, 18);
                    h = h * 5 + 0xe6546b64;
                    f += a1;
                    f = this.Rotate32(f, 19);
                    f = f * c1;
                    g += a2;
                    g = this.Rotate32(g, 18);
                    g = g * 5 + 0xe6546b64;
                    h ^= a3 + a1;
                    h = this.Rotate32(h, 19);
                    h = h * 5 + 0xe6546b64;
                    g ^= a4;
                    g = this.BSwap32(g) * 5;
                    h += a4 * 5;
                    h = this.BSwap32(h);
                    f += a0;

                    var temp = f;
                    f = g;
                    g = h;
                    h = temp;

                    pointer += 20;
                } while (--iters != 0);

                g = this.Rotate32(g, 11) * c1;
                g = this.Rotate32(g, 17) * c1;
                f = this.Rotate32(f, 11) * c1;
                f = this.Rotate32(f, 17) * c1;
                h = this.Rotate32(h + g, 19);
                h = h * 5 + 0xe6546b64;
                h = this.Rotate32(h, 17) * c1;
                h = this.Rotate32(h + f, 19);
                h = h * 5 + 0xe6546b64;
                h = this.Rotate32(h, 17) * c1;
                return h;
            }
        }

        /// <summary>
        /// Obtém a mistura de um valor com a sua rotação.
        /// </summary>
        /// <param name="val">O valor.</param>
        /// <returns>O resultado da mistura.</returns>
        private ulong ShiftMix(ulong val)
        {
            return val ^ (val >> 47);
        }

        /// <summary>
        /// Determina o código confuso de um valor de 128 bit num valor de 64 biy inspirado no
        /// Murmur.
        /// </summary>
        /// <param name="u">A primeira metade do valor de 128 bit.</param>
        /// <param name="v">A segunda metade do valor de 128 bit.</param>
        /// <returns>O código confuso resultante.</returns>
        private ulong HashLen16(ulong u, ulong v)
        {
            return this.Hash128to64(u, v);
        }

        /// <summary>
        /// Determina o código confuso de um valor de 128 bit inspirado no
        /// Murmur.
        /// </summary>
        /// <param name="u">A primeira metade do valor de 128 bit.</param>
        /// <param name="v">A segunda metade do valor de 128 bit.</param>
        /// <param name="mul">Um valor multiplicativo.</param>
        /// <returns>O código confuso resultante.</returns>
        private ulong HashLen16(ulong u, ulong v, ulong mul)
        {
            var a = (u ^ v) * mul;
            a ^= (a >> 47);
            var b = (v ^ a) * mul;
            b ^= (b >> 47);
            b *= mul;
            return b;
        }

        /// <summary>
        /// Código confuso de 64 bits a partir de um valor de 128 bits.
        /// </summary>
        /// <param name="x">O valor de 64 bits menos significativos.</param>
        /// <param name="y">O valor dos 64 bits mais significativos.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash128to64(ulong x, ulong y)
        {
            const ulong kMul = 0x9ddfea08eb382d69UL;
            var a = (x ^ y) * kMul;
            a ^= (a >> 47);
            var b = (y ^ a) * kMul;
            b ^= (b >> 47);
            b *= kMul;
            return b;
        }

        /// <summary>
        /// Obtém o código confuso de um valor textual com um número de
        /// carácteres compreendido entre 0 e 16.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen0to16(byte[] value, int index, int len)
        {
            if (len >= 8)
            {
                ulong mul = k2 + (ulong)len * 2;
                ulong a = this.Fetch64(value, index) + k2;
                ulong b = this.Fetch64(value, index + len - 8);
                ulong c = this.Rotate(b, 37) * mul + a;
                ulong d = (this.Rotate(a, 25) + b) * mul;
                return this.HashLen16(c, d, mul);
            }

            if (len >= 4)
            {
                ulong mul = k2 + (ulong)len * 2;
                ulong a = this.Fetch32(value, index);
                return this.HashLen16(
                    (ulong)len + (a << 3),
                    this.Fetch32(value, index + len - 4), mul);
            }

            if (len > 0)
            {
                byte a = (byte)value[0];
                byte b = (byte)value[len >> 1];
                byte c = (byte)value[len - 1];
                uint y = a + (((uint)b) << 8);
                uint z = (uint)len + (((uint)c) << 2);
                return this.ShiftMix(y * k2 ^ z * k0) * k2;
            }

            return k2;
        }

        /// <summary>
        /// Obtém o código confuso de um valor textual com um número de
        /// carácteres compreendido entre 17 e 32.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen17to32(byte[] value, int index, int len)
        {
            var mul = k2 + (ulong)len * 2;
            var a = this.Fetch64(value, index) * k1;
            var b = this.Fetch64(value, index + 8);
            var c = this.Fetch64(value, index + len - 8) * mul;
            var d = this.Fetch64(value, index + len - 16) * k2;
            return this.HashLen16(
                this.Rotate(a + b, 43) + this.Rotate(c, 30) + d,
                a + this.Rotate(b + k2, 18) + c,
                mul);
        }

        /// <summary>
        /// Obtém o código confuso de um valor textual com um número de
        /// carácteres compreendido entre 33 e 64.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen33to64(byte[] value, int index, int len)
        {
            var mul = k2 + (ulong)len * 2;
            var a = this.Fetch64(value, index) * k2;
            var b = this.Fetch64(value, index + 8);
            var c = this.Fetch64(value, index + len - 24);
            var d = this.Fetch64(value, index + len - 32);
            var e = this.Fetch64(value, index + 16) * k2;
            var f = this.Fetch64(value, index + 24) * 9;
            var g = this.Fetch64(value, index + len - 8);
            var h = this.Fetch64(value, index + len - 16) * mul;
            var u = this.Rotate(a + g, 43) + (this.Rotate(b, 30) + c) * 9;
            var v = ((a + g) ^ d) + f + 1;
            var w = this.BSwap64((u + v) * mul) + h;
            var x = this.Rotate(e + f, 42) + c;
            var y = (this.BSwap64((v + w) * mul) + g) * mul;
            var z = e + f + c;
            a = this.BSwap64((x + z) * mul + y) + b;
            b = ShiftMix((z + a) * mul + d + h) * mul;
            return b + x;
        }

        /// <summary>
        /// Obtém um código confuso de 16 bytes para 48 bytes.
        /// </summary>
        /// <remarks>
        /// É preferível que a função seja executada para valores
        /// aleatórios de <paramref name="a"/> e <paramref name="b"/>.
        /// </remarks>
        /// <param name="w">Valor de 8 bytes.</param>
        /// <param name="x">Valor de 8 bytes.</param>
        /// <param name="y">Valor de 8 bytes.</param>
        /// <param name="z">Valor de 8 bytes.</param>
        /// <param name="a">Valor de 8 bytes.</param>
        /// <param name="b">Valor de 8 bytes.</param>
        /// <returns>Um par de valores de 8 bytes cada.</returns>
        private Tuple<ulong, ulong> WeakHashLen32WithSeeds(
            ulong w,
            ulong x,
            ulong y,
            ulong z,
            ulong a,
            ulong b)
        {
            a += w;
            b = this.Rotate(b + a + z, 21);
            var c = a;
            a += x;
            a += y;
            b += this.Rotate(a, 44);
            return Tuple.Create(a + z, b + c);
        }

        /// <summary>
        /// Retorna um código confuso de 16 bytes para os primeiros
        /// 32 carácteres da cadeia de texto.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice a partir do qual se pretende obter o código confuso.</param>
        /// <param name="a">Um valor de 8 bytes.</param>
        /// <param name="b">Um valor de 8 bytes.</param>
        /// <returns>O código confuso.</returns>
        private Tuple<ulong, ulong> WeakHashLen32WithSeeds(
            byte[] value,
            int index,
            ulong a,
            ulong b)
        {
            return this.WeakHashLen32WithSeeds(
                this.Fetch64(value, index),
                this.Fetch64(value, index + 8),
                this.Fetch64(value, index + 16),
                this.Fetch64(value, index + 24),
                a,
                b);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits a partir de um valor textual.
        /// </summary>
        /// <param name="value">O vector de bits.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong CityHash64(byte[] value, int index, int len)
        {
            if (len <= 32)
            {
                if (len <= 16)
                {
                    return this.HashLen0to16(value, index, len);
                }
                else
                {
                    return this.HashLen17to32(value, index, len);
                }
            }
            else if (len <= 64)
            {
                return this.HashLen33to64(value, index, len);
            }


            var x = this.Fetch64(value, index + len - 40);
            var y = this.Fetch64(value, index + len - 16) + Fetch64(value, index + len - 56);
            var z = this.Hash128to64(this.Fetch64(value, index + len - 48) + (ulong)len, this.Fetch64(value, index + len - 24));
            var v = this.WeakHashLen32WithSeeds(value, index + len - 64, (ulong)len, z);
            var w = this.WeakHashLen32WithSeeds(value, index + len - 32, y + k1, x);
            x = x * k1 + this.Fetch64(value, index);

            len = (len - 1) & ~63;
            var pointer = index;
            do
            {
                x = Rotate(x + y + v.Item1 + this.Fetch64(value, pointer + 8), 37) * k1;
                y = Rotate(y + v.Item2 + this.Fetch64(value, pointer + 48), 42) * k1;
                x ^= w.Item2;
                y += v.Item1 + this.Fetch64(value, pointer + 40);
                z = Rotate(z + w.Item1, 33) * k1;
                v = WeakHashLen32WithSeeds(value, pointer, v.Item2 * k1, x + w.Item1);
                w = WeakHashLen32WithSeeds(value, pointer + 32, z + w.Item2, y + this.Fetch64(value, pointer + 16));

                var t = z;
                z = x;
                x = t;

                pointer += 64;
                len -= 64;
            } while (len != 0);
            return this.Hash128to64(
                this.Hash128to64(v.Item1, w.Item1) + ShiftMix(y) * k1 + z,
                this.Hash128to64(v.Item2, w.Item2) + x);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits a partir de um valor textual.
        /// </summary>
        /// <param name="s">O vector de bits.</param>
        /// <param name="seed0">A primeira semente.</param>
        /// <param name="seed1">A segunda semente.</param>
        /// <returns>O código confuso.</returns>
        ulong CityHash64WithSeeds(
            byte[] s,
            ulong seed0,
            ulong seed1)
        {
            var len = s.Length;
            return this.HashLen16(this.CityHash64(s, 0, len) - seed0, seed1);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits a partir de um valor textual.
        /// </summary>
        /// <param name="s">O vector de bits.</param>
        /// <param name="seed">A semente.</param>
        /// <returns>O código confuso.</returns>
        ulong CityHash64WithSeed(byte[] s, ulong seed)
        {
            return this.CityHash64WithSeeds(s, k2, seed);
        }

        /// <summary>
        /// Retorna uma código confuso de 128 para texto.
        /// </summary>
        /// <remarks>
        /// Serve de subrotina ao <see cref="CityHash128"/>.
        /// </remarks>
        /// <param name="s">O texto.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <param name="seedLow">A primeira parte da semente.</param>
        /// <param name="seedHigh">A segunda parte da semente.</param>
        /// <returns>O código confuso.</returns>
        private Tuple<ulong, ulong> CityMurmur(
            byte[] s,
            int index,
            int len,
            ulong seedLow,
            ulong seedHigh)
        {
            var a = seedLow;
            var b = seedHigh;
            var c = 0UL;
            var d = 0UL;
            var l = (long)len - 16;
            if (l <= 0)
            {
                a = this.ShiftMix(a * k1) * k1;
                c = b * k1 + this.HashLen0to16(s, index, len);
                d = ShiftMix(a + (len >= 8 ? this.Fetch64(s, index) : c));
            }
            else
            {
                c = this.HashLen16(this.Fetch64(s, index + len - 8) + k1, a);
                d = this.HashLen16(b + (ulong)len, c + this.Fetch64(s, index + len - 16));
                a += d;
                var pointer = index;
                do
                {
                    a ^= this.ShiftMix(this.Fetch64(s, pointer) * k1) * k1;
                    a *= k1;
                    b ^= a;
                    c ^= this.ShiftMix(this.Fetch64(s, pointer + 8) * k1) * k1;
                    c *= k1;
                    d ^= c;
                    pointer += 16;
                    l -= 16;
                } while (l > 0);
            }
            a = this.HashLen16(a, c);
            b = this.HashLen16(d, b);
            return Tuple.Create(a ^ b, this.HashLen16(b, a));
        }

        /// <summary>
        /// Obtém um código confuso de 128 bits, sendo proporcionada uma semente de
        /// 128 bit dividida em dois valores.
        /// </summary>
        /// <param name="s">O texto.</param>
        /// <param name="index">O índice onde se inicia o processo.</param>
        /// <param name="len">O comprimento do valor a ser considerado.</param>
        /// <param name="low">O valor baixo.</param>
        /// <param name="high">O valor alto.</param>
        /// <returns>O par com o código confuso.</returns>
        private Tuple<ulong, ulong> CityHash128WithSeed(
            byte[] s,
            int index,
            int len,
            ulong low,
            ulong high)
        {
            if (len < 128)
            {
                return this.CityMurmur(s, index, len, low, high);
            }

            var v = MutableTuple.Create(0UL, 0UL);
            var w = MutableTuple.Create(0UL, 0UL);
            var x = low;
            var y = high;
            var z = (ulong)len * k1;
            v.Item1 = this.Rotate(y ^ k1, 49) * k1 + this.Fetch64(s, index);
            v.Item2 = this.Rotate(v.Item1, 42) * k1 + Fetch64(s, index + 8);
            w.Item1 = this.Rotate(y + z, 35) * k1 + x;
            w.Item2 = this.Rotate(x + Fetch64(s, index + 88), 53) * k1;

            var pointer = index;
            do
            {
                x = this.Rotate(x + y + v.Item1 + this.Fetch64(s, pointer + 8), 37) * k1;
                y = this.Rotate(y + v.Item2 + this.Fetch64(s, pointer + 48), 42) * k1;
                x ^= w.Item2;
                y += v.Item1 + this.Fetch64(s, pointer + 40);
                z = Rotate(z + w.Item1, 33) * k1;
                v = this.WeakHashLen32WithSeeds(s, pointer, v.Item2 * k1, x + w.Item1);
                w = this.WeakHashLen32WithSeeds(s, pointer + 32, z + w.Item2, y + this.Fetch64(s, pointer + 16));

                var t = x;
                x = z;
                z = t;

                pointer += 64;
                x = this.Rotate(x + y + v.Item1 + this.Fetch64(s, pointer + 8), 37) * k1;
                y = this.Rotate(y + v.Item2 + this.Fetch64(s, pointer + 48), 42) * k1;
                x ^= w.Item2;
                y += v.Item1 + this.Fetch64(s, pointer + 40);
                z = this.Rotate(z + w.Item1, 33) * k1;
                v = this.WeakHashLen32WithSeeds(s, pointer, v.Item2 * k1, x + w.Item1);
                w = this.WeakHashLen32WithSeeds(s, pointer + 32, z + w.Item2, y + this.Fetch64(s, pointer + 16));

                t = x;
                x = z;
                z = t;

                pointer += 64;
                len -= 128;
            } while (len >= 128);

            x += this.Rotate(v.Item1 + z, 49) * k0;
            y = y * k0 + this.Rotate(w.Item2, 37);
            z = z * k0 + this.Rotate(w.Item1, 27);
            w.Item1 *= 9;
            v.Item1 *= k0;

            for (var tail_done = 0; tail_done < len; )
            {
                tail_done += 32;
                y = this.Rotate(x + y, 42) * k0 + v.Item2;
                w.Item1 += this.Fetch64(s, pointer + len - tail_done + 16);
                x = x * k0 + w.Item1;
                z += w.Item2 + this.Fetch64(s, pointer + len - tail_done);
                w.Item2 += v.Item1;
                v = this.WeakHashLen32WithSeeds(s, pointer + len - tail_done, v.Item1 + z, v.Item2);
                v.Item1 *= k0;
            }

            x = HashLen16(x, v.Item1);
            y = HashLen16(y + z, w.Item1);
            return Tuple.Create(HashLen16(x + v.Item2, w.Item2) + y,
                           HashLen16(x + w.Item2, y + v.Item2));
        }

        /// <summary>
        /// Obtém o código confuso de 128 bits.
        /// </summary>
        /// <param name="s">O valor textual.</param>
        /// <returns>O par que contém as partes do código confuso.</returns>
        private Tuple<ulong, ulong> CityHash128(byte[] s)
        {
            var len = s.Length;
            return len >= 16 ?
                this.CityHash128WithSeed(s, 16, len - 16,
                                    this.Fetch64(s), this.Fetch64(s, 8) + k0) :
                this.CityHash128WithSeed(s, 0, len, k0, k1);
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa o agloritmo Murmur.
    /// </summary>
    /// <remarks>
    /// Ver: https://github.com/aappleby/smhasher/blob/master/src/MurmurHash2.cpp.
    /// </remarks>
    public class MurmurHash2 :
        IHash32<string>,
        IHash32<byte[]>,
        IHash64<string>,
        IHash64<byte[]>,
        IHashN<string>,
        IHashN<byte[]>
    {
        #region Funções públicas

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(string obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(byte[] obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(string obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(byte[] obj)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="bytes">O número de bytes no código confuso.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash(string obj, int bytes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="bytes">O número de bytes no código confuso.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash(byte[] obj, int bytes)
        {
            throw new NotImplementedException();
        }

        #endregion Funções públicas

        #region Funções privadas

        /// <summary>
        /// Executa o algoritmo do Murmur sobre o vector de bytes.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor inicial para mistura.</param>
        /// <returns>O código confuso.</returns>
        private uint GetMurmurHash2(byte[] key, int len, uint seed)
        {
            var m = 0x5bd1e995U;
            var r = 24;

            uint h = seed ^ (uint)len;
            var p = 0;
            while (len >= 4)
            {
                var k = BitConverter.ToUInt32(key, p);

                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;

                p += 4;
                len -= 4;
            }

            switch (len)
            {
                case 3:
                    h ^= ((uint)key[p + 2]) << 16;
                    h ^= ((uint)key[p + 1]) << 8;
                    h ^= key[p];
                    h *= m;
                    break;
                case 2:
                    h ^= ((uint)key[p + 1]) << 8;
                    h ^= key[p];
                    h *= m;
                    break;
                case 1:
                    h ^= key[p];
                    h *= m;
                    break;
            };

            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return h;
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 64-bit sobre o vector de bytes
        /// para uma plataforma 64-bit.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor inicial para mistura.</param>
        /// <returns>O código confuso.</returns>
        private ulong GetMurmurHash64A(byte[] key, int len, ulong seed)
        {
            var m = 0xc6a4a7935bd1e995UL;
            var r = 47;

            var h = seed ^ ((ulong)len * m);

            var p = 0;
            while (p < len)
            {
                var k = BitConverter.ToUInt64(key, p);
                p += 8;

                k *= m;
                k ^= k >> r;
                k *= m;

                h ^= k;
                h *= m;
            }

            switch (len & 7)
            {
                case 7:
                    h ^= ((ulong)key[p + 6]) << 48;
                    h ^= ((ulong)key[p + 5]) << 40;
                    h ^= ((ulong)key[p + 4]) << 32;
                    h ^= ((ulong)key[p + 3]) << 24;
                    h ^= ((ulong)key[p + 2]) << 16;
                    h ^= ((ulong)key[p + 1]) << 8;
                    h ^= ((ulong)key[p]);
                    h *= m;
                    break;
                case 6:
                    h ^= ((ulong)key[p + 5]) << 40;
                    h ^= ((ulong)key[p + 4]) << 32;
                    h ^= ((ulong)key[p + 3]) << 24;
                    h ^= ((ulong)key[p + 2]) << 16;
                    h ^= ((ulong)key[p + 1]) << 8;
                    h ^= ((ulong)key[p]);
                    h *= m;
                    break;
                case 5:
                    h ^= ((ulong)key[p + 4]) << 32;
                    h ^= ((ulong)key[p + 3]) << 24;
                    h ^= ((ulong)key[p + 2]) << 16;
                    h ^= ((ulong)key[p + 1]) << 8;
                    h ^= ((ulong)key[p]);
                    h *= m;
                    break;
                case 4:
                    h ^= ((ulong)key[p + 3]) << 24;
                    h ^= ((ulong)key[p + 2]) << 16;
                    h ^= ((ulong)key[p + 1]) << 8;
                    h ^= ((ulong)key[p]);
                    h *= m;
                    break;
                case 3:
                    h ^= ((ulong)key[p + 2]) << 16;
                    h ^= ((ulong)key[p + 1]) << 8;
                    h ^= ((ulong)key[p]);
                    h *= m;
                    break;
                case 2:
                    h ^= ((ulong)key[p + 1]) << 8;
                    h ^= ((ulong)key[p]);
                    h *= m;
                    break;
                case 1:
                    h ^= ((ulong)key[p]);
                    h *= m;
                    break;
            };

            h ^= h >> r;
            h *= m;
            h ^= h >> r;

            return h;
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 64-bit sobre o vector de bytes
        /// para uma plataforma 32-bit.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seedLow">Um valor inicial para mistura.</param>
        /// <param name="seedHigh">Um valor inicial para mistura.</param>
        /// <returns>O código confuso.</returns>
        private ulong GetMurmurHash64B(byte[] key, int len, uint seedLow, uint seedHigh)
        {
            var m = 0x5bd1e995U;
            var r = 24;

            var h1 = (uint)(seedLow ^ len);
            var h2 = (seedHigh >> 32);

            var p = 0;
            while (len >= 8)
            {
                var k1 = BitConverter.ToUInt32(key, p);
                p += 4;
                k1 *= m;
                k1 ^= k1 >> r;
                k1 *= m;
                h1 *= m; h1 ^= k1;
                len -= 4;

                var k2 = BitConverter.ToUInt32(key, p);
                p += 4;
                k2 *= m; k2 ^= k2 >> r; k2 *= m;
                h2 *= m; h2 ^= k2;
                len -= 4;
            }

            if (len >= 4)
            {
                var k1 = BitConverter.ToUInt32(key, p);
                p += 4;
                k1 *= m; k1 ^= k1 >> r; k1 *= m;
                h1 *= m; h1 ^= k1;
                len -= 4;
            }

            switch (len)
            {
                case 3:
                    h2 ^= ((uint)key[p + 2]) << 16;
                    h2 ^= ((uint)key[p + 1]) << 8;
                    h2 ^= ((uint)key[p]);
                    h2 *= m;
                    break;
                case 2:
                    h2 ^= ((uint)key[p + 1]) << 8;
                    h2 ^= ((uint)key[p]);
                    h2 *= m;
                    break;
                case 1:
                    h2 ^= ((uint)key[p]);
                    h2 *= m;
                    break;
            };

            h1 ^= h2 >> 18;
            h1 *= m;
            h2 ^= h1 >> 22;
            h2 *= m;
            h1 ^= h2 >> 17;
            h1 *= m;
            h2 ^= h1 >> 19;
            h2 *= m;

            var h = (ulong)h1;

            h = (h << 32) | h2;

            return h;
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 32-bit sobre o vector de bytes,
        /// corrigindo um pequeno problema relativo à colisão de chaves nulas.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comrpimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor inicial para mistura.</param>
        /// <returns>O código confuso.</returns>
        private uint MurmurHash2A(byte[] key, int len, uint seed)
        {
            var m = 0x5bd1e995U;
            var r = 24;
            var l = (uint)len;

            var h = seed;
            var p = 0;
            while (len >= 4)
            {
                var k = BitConverter.ToUInt32(key, p);

                this.Mix(ref h, ref k, m, r);

                p += 4;
                len -= 4;
            }

            var t = 0U;

            switch (len)
            {
                case 3:
                    t ^= ((uint)key[2]) << 16;
                    t ^= ((uint)key[1]) << 8;
                    t ^= key[0];
                    break;
                case 2:
                    t ^= ((uint)key[1]) << 8;
                    t ^= key[0];
                    break;
                case 1:
                    t ^= key[0];
                    break;
            };

            this.Mix(ref h, ref t, m, r);
            this.Mix(ref h, ref l, m, r);

            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return h;
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 32-bit sobre o vector de bytes,
        /// não considerando a ordem dos bits.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor incial para mistura.</param>
        /// <returns>O código confuso.</returns>
        private uint MurmurHashNeutral2(byte[] key, int len, uint seed)
        {
            var m = 0x5bd1e995U;
            var r = 24;

            var h = (uint)(seed ^ len);

            var p = 0;
            while (len >= 4)
            {
                var k = 0U;

                k = key[p];
                k |= ((uint)key[p + 1]) << 8;
                k |= ((uint)key[p + 2]) << 16;
                k |= ((uint)key[p + 3]) << 24;

                k *= m;
                k ^= k >> r;
                k *= m;

                h *= m;
                h ^= k;

                p += 4;
                len -= 4;
            }

            switch (len)
            {
                case 3:
                    h ^= ((uint)key[p + 2]) << 16;
                    h ^= ((uint)key[p + 1]) << 8;
                    h ^= ((uint)key[p]);
                    h *= m;
                    break;
                case 2:
                    h ^= ((uint)key[p + 1]) << 8;
                    h ^= ((uint)key[p]);
                    h *= m;
                    break;
                case 1:
                    h ^= ((uint)key[p]);
                    h *= m;
                    break;
            };

            h ^= h >> 13;
            h *= m;
            h ^= h >> 15;

            return h;
        }

        private uint MurmurHashAligned2(byte[] key, int len, uint seed)
        {
            var m = 0x5bd1e995U;
            var r = 24;

            var h = (uint)(seed ^ len);
            int align = key == null ? 0 : (int)(key[0] & 3);

            if (align != 0 && (len >= 4))
            {
                var t = 0U;
                var d = 0U;

                var p = 0;
                switch (align)
                {
                    case 1:
                        t |= ((uint)key[p + 2]) << 16;
                        t |= ((uint)key[p + 1]) << 8;
                        t |= ((uint)key[p]);
                        break;
                    case 2:
                        t |= ((uint)key[p + 1]) << 8;
                        t |= ((uint)key[p]);
                        break;
                    case 3:
                        t |= ((uint)key[p]);
                        break;
                }

                t <<= (8 * align);
                p += 4 - align;
                len -= 4 - align;
                int sl = 8 * (4 - align);
                int sr = 8 * align;

                while (len >= 4)
                {
                    d = BitConverter.ToUInt32(key, p);
                    t = (t >> sr) | (d << sl);

                    var k = t;
                    this.Mix1(ref h, ref k, m, r);
                    t = d;
                    p += 4;
                    len -= 4;
                }

                d = 0;
                if (len >= align)
                {
                    switch (align)
                    {
                        case 3:
                            d |= ((uint)key[p + 2]) << 16;
                            d |= ((uint)key[p + 1]) << 8;
                            d |= ((uint)key[p]);
                            break;
                        case 2:
                            d |= ((uint)key[p + 1]) << 8;
                            d |= ((uint)key[p]);
                            break;
                        case 1:
                            d |= ((uint)key[p]);
                            break;
                    }

                    var k = (t >> sr) | (d << sl);
                    this.Mix1(ref h, ref k, m, r);

                    p += align;
                    len -= align;

                    //----------
                    // Handle tail bytes

                    switch (len)
                    {
                        case 3:
                            h ^= ((uint)key[p + 2]) << 16;
                            h ^= ((uint)key[p + 1]) << 8;
                            h ^= ((uint)key[p]);
                            h *= m;
                            break;
                        case 2:
                            h ^= ((uint)key[p + 1]) << 8;
                            h ^= ((uint)key[p]);
                            h *= m;
                            break;
                        case 1:
                            h ^= ((uint)key[p]);
                            h *= m;
                            break;
                    };
                }
                else
                {
                    switch (len)
                    {
                        case 3:
                            d |= ((uint)key[p + 2]) << 16;
                            d |= ((uint)key[p + 1]) << 8;
                            d |= ((uint)key[p]);
                            h ^= (t >> sr) | (d << sl);
                            h *= m;
                            break;
                        case 2:
                            d |= ((uint)key[p + 1]) << 8;
                            d |= ((uint)key[p]);
                            h ^= (t >> sr) | (d << sl);
                            h *= m;
                            break;
                        case 1:
                            d |= ((uint)key[p]);
                            h ^= (t >> sr) | (d << sl);
                            h *= m;
                            break;
                        case 0:
                            h ^= (t >> sr) | (d << sl);
                            h *= m;
                            break;
                    }
                }

                h ^= h >> 13;
                h *= m;
                h ^= h >> 15;

                return h;
            }
            else
            {
                var p = 0;
                while (len >= 4)
                {
                    var k = BitConverter.ToUInt32(key, p);

                    this.Mix1(ref h, ref k, m, r);

                    p += 4;
                    len -= 4;
                }

                //----------
                // Handle tail bytes

                switch (len)
                {
                    case 3:
                        h ^= ((uint)key[p + 2]) << 16;
                        h ^= ((uint)key[p + 1]) << 8;
                        h ^= ((uint)key[p]);
                        h *= m;
                        break;
                    case 2:
                        h ^= ((uint)key[p + 1]) << 8;
                        h ^= ((uint)key[p]);
                        h *= m;
                        break;
                    case 1:
                        h ^= ((uint)key[p]);
                        h *= m;
                        break;
                };

                h ^= h >> 13;
                h *= m;
                h ^= h >> 15;

                return h;
            }
        }

        /// <summary>
        /// Mistura os valores das variáveis.
        /// </summary>
        /// <param name="h">A primeira variável.</param>
        /// <param name="k">A segunda variável.</param>
        /// <param name="m">A semente a ser misturada.</param>
        /// <param name="r">Uma constante.</param>
        private void Mix(ref uint h, ref uint k, uint m, int r)
        {
            k *= m;
            k ^= k >> r;
            k *= m;
            h *= m;
            h ^= k;
        }

        /// <summary>
        /// Mistura os valores das variáveis.
        /// </summary>
        /// <param name="h">A primeira variável.</param>
        /// <param name="k">A segunda variável.</param>
        /// <param name="m">A semente a ser misturada.</param>
        /// <param name="r">Uma constante.</param>
        private void Mix1(ref uint h, ref uint k, uint m, int r)
        {
            k *= m;
            k ^= k >> r;
            k *= m;
            h *= m;
            h ^= k;
        }

        #endregion Funções privadas
    }
}
