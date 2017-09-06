// -----------------------------------------------------------------------
// <copyright file="HashAlgorithms.cs" company="Sérgio O. Marques">
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
    /// Define uma função de confusão de um objecto com um número especificado de bytes.
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
    /// Define uma função de confusão que permite rotação.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto do qual se pretende obter o código confuso.</typeparam>
    public interface IRollingHash32<in T>
    {
        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        uint HashValue { get; }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        void Eat(T obj);

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        void Update(T inObj, T outObj);
    }

    /// <summary>
    /// Define uma função de confusão que permite rotação.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto do qual se pretende obter o código confuso.</typeparam>
    public interface IRollingHash64<in T>
    {
        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        ulong HashValue { get; }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        void Eat(T obj);

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        void Update(T inObj, T outObj);
    }

    /// <summary>
    /// Define uma função de confusão que permite rotação.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto do qual se pretende obter o código confuso.</typeparam>
    public interface IRollingHashN<in T>
    {
        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        BigInteger HashValue { get; }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        void Eat(T obj);

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        void Update(T inObj, T outObj);
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
        private static uint defaultPolynomial = 0xedb88320U;

        /// <summary>
        /// Mantém a semente por defeito.
        /// </summary>
        public static uint defaultSeed = 0xffffffffU;

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

        /// <summary>
        /// Calcula o código confuso.
        /// </summary>
        /// <param name="table">A tabela a ser utilizada no cálculo.</param>
        /// <param name="seed">A semente.</param>
        /// <param name="buffer">O amortecedor para o cálculo.</param>
        /// <param name="start">A posição inicial.</param>
        /// <param name="size">A posição final.</param>
        /// <returns>O código confuso.</returns>
        internal uint InternalCalculateHash(
            uint[] table,
            uint seed,
            IList<byte> buffer,
            int start,
            int size)
        {
            return this.CalculateHash(table, seed, buffer, start, size);
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
            var hash = seed ^ 0xffffffffU;
            var after = start + size;
            for (var i = start; i < after; i++)
            {
                hash = (hash >> 8) ^ table[(buffer[i] ^ hash) & 0xff];
            }

            hash ^= 0xffffffffU;
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
    /// Implementa o agloritmo Murmur2.
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
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                return this.GetMurmurHash2(bytes, bytes.Length, 0);
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
                return this.GetMurmurHash2(obj, obj.Length, 0);
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
                return this.GetMurmurHash64A(bytes, bytes.Length, 0);
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
                return this.GetMurmurHash64A(obj, obj.Length, 0);
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
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var objData = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                if (bytes == 4)
                {
                    return this.GetMurmurHash2(objData, objData.Length, 0);
                }
                else if (bytes == 8)
                {
                    return this.GetMurmurHash64A(objData, objData.Length, 0);
                }
                else
                {
                    throw new NotSupportedException("The provided number of bytes is not supported for hash function.");
                }
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
            if (obj == null)
            {
                return 0;
            }
            else
            {
                if (bytes == 4)
                {
                    return this.GetMurmurHash2(obj, obj.Length, 0);
                }
                else if (bytes == 8)
                {
                    return this.GetMurmurHash64A(obj, obj.Length, 0);
                }
                else
                {
                    throw new NotSupportedException("The provided number of bytes is not supported for hash function.");
                }
            }
        }

        /// <summary>
        /// Mistura os valores das variáveis.
        /// </summary>
        /// <param name="h">A primeira variável.</param>
        /// <param name="k">A segunda variável.</param>
        /// <param name="m">A semente a ser misturada.</param>
        /// <param name="r">Uma constante.</param>
        public static void Mix(ref uint h, ref uint k, uint m, int r)
        {
            k *= m;
            k ^= k >> r;
            k *= m;
            h *= m;
            h ^= k;
        }

        #endregion Funções públicas

        #region Funções internas

        /// <summary>
        /// Executa o algoritmo do Murmur sobre o vector de bytes.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor inicial para mistura.</param>
        /// <returns>O código confuso.</returns>
        internal uint GetMurmurHash2Internal(byte[] key, int len, uint seed)
        {
            return this.GetMurmurHash2(key, len, seed);
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 64-bit sobre o vector de bytes
        /// para uma plataforma 64-bit.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor inicial para mistura.</param>
        /// <returns>O código confuso.</returns>
        internal ulong GetMurmurHash64AInternal(byte[] key, int len, ulong seed)
        {
            return this.GetMurmurHash64A(key, len, seed);
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 64-bit sobre o vector de bytes
        /// para uma plataforma 64-bit.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seedLow">Um valor inicial para mistura.</param>
        /// <param name="seedHigh">Um valor inicial para a mistura.</param>
        /// <returns>O código confuso.</returns>
        internal ulong GetMurmurHash6BAInternal(byte[] key, int len, uint seedLow, uint seedHigh)
        {
            return this.GetMurmurHash64B(key, len, seedLow, seedHigh);
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 32-bit sobre o vector de bytes,
        /// corrigindo um pequeno problema relativo à colisão de chaves nulas.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comrpimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor inicial para mistura.</param>
        /// <returns>O código confuso.</returns>
        internal uint MurmurHash2AInternal(byte[] key, int len, uint seed)
        {
            return this.MurmurHash2A(key, len, seed);
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 32-bit sobre o vector de bytes,
        /// não considerando a ordem dos bits.
        /// </summary>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor incial para mistura.</param>
        /// <returns>O código confuso.</returns>
        internal uint MurmurHashNeutral2Internal(byte[] key, int len, uint seed)
        {
            return this.MurmurHashNeutral2(key, len, seed);
        }

        /// <summary>
        /// Executa o algoritmo do Murmur de 32-bit sobre o vector de bytes,
        /// efectuando leituras alinhadas.
        /// </summary>
        /// <remarks>
        /// Trata-se de um algoritmo mais seguro em determinadas plataformas.
        /// </remarks>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor incial para mistura.</param>
        /// <returns>O código confuso.</returns>
        internal uint MurmurHashAligned2Internal(byte[] key, int len, uint seed)
        {
            return this.MurmurHashAligned2(key, len, seed);
        }

        #endregion Funções internas

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
            var i = 0;
            var end = len / 8;
            while (i < end)
            {
                var k = BitConverter.ToUInt64(key, p);
                ++i;
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

                Mix(ref h, ref k, m, r);

                p += 4;
                len -= 4;
            }

            var t = 0U;

            switch (len)
            {
                case 3:
                    t ^= ((uint)key[p + 2]) << 16;
                    t ^= ((uint)key[p + 1]) << 8;
                    t ^= key[p];
                    break;
                case 2:
                    t ^= ((uint)key[p + 1]) << 8;
                    t ^= key[p];
                    break;
                case 1:
                    t ^= key[p];
                    break;
            };

            Mix(ref h, ref t, m, r);
            Mix(ref h, ref l, m, r);

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

        /// <summary>
        /// Executa o algoritmo do Murmur de 32-bit sobre o vector de bytes,
        /// efectuando leituras alinhadas.
        /// </summary>
        /// <remarks>
        /// Trata-se de um algoritmo mais seguro em determinadas plataformas.
        /// </remarks>
        /// <param name="key">O vector.</param>
        /// <param name="len">O comprimento do vector sobre o qual se aplica o algoritmo.</param>
        /// <param name="seed">Um valor incial para mistura.</param>
        /// <returns>O código confuso.</returns>
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

    /// <summary>
    /// Implementa o agloritmo Murmur3.
    /// </summary>
    /// <remarks>
    /// Ver: https://github.com/aappleby/smhasher/blob/master/src/MurmurHash2.cpp.
    /// </remarks>
    public class MurmurHash3 :
        IHash32<string>,
        IHash32<byte[]>,
        IHashN<string>,
        IHashN<byte[]>
    {
        #region Funções públicas

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(string obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                return this.MurmurHash3A32(bytes, 0, bytes.Length, 0);
            }
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return this.MurmurHash3A32(obj, 0, obj.Length, 0);
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
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var objData = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                if (bytes == 4)
                {
                    return this.MurmurHash3A32(objData, 0, objData.Length, 0);
                }
                else if (bytes == 16)
                {
                    return new BigInteger(this.MurmurHash3B128(objData, 0, objData.Length, 0));
                }
                else
                {
                    throw new NotSupportedException("The provided number of bytes is not supported for hash function.");
                }
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
            if (obj == null)
            {
                return 0;
            }
            else if (bytes == 4)
            {
                return this.MurmurHash3A32(obj, 0, obj.Length, 0);
            }
            else if (bytes == 16)
            {
                return new BigInteger(this.MurmurHash3B128(obj, 0, obj.Length, 0));
            }
            else
            {
                throw new NotSupportedException("The provided number of bytes is not supported for hash function.");
            }
        }

        #endregion Funções públicas

        #region Funções internas

        /// <summary>
        /// Determina o código confuso 32 bit de um valor em arquitectura x86.
        /// </summary>
        /// <param name="key">O valor do qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice incial.</param>
        /// <param name="len">O tamanho dos dados.</param>
        /// <param name="seed">Um valor inicial.</param>
        /// <returns>O código confuso.</returns>
        internal uint InternalMurmurHash3A32(
            byte[] key,
            int start,
            int len,
            uint seed)
        {
            return this.MurmurHash3A32(key, start, len, seed);
        }

        /// <summary>
        /// Determina o código confuso de 128 bit de um valor em arquitectura x64.
        /// </summary>
        /// <param name="key">O valor do qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice incial.</param>
        /// <param name="len">O tamanho dos dados.</param>
        /// <param name="seed">Um valor inicial.</param>
        /// <returns>O código confuso.</returns>
        internal byte[] InternalMurmurHash3A128(
            byte[] key,
            int start,
            int len,
            uint seed)
        {
            return this.MurmurHash3A128(key, start, len, seed);
        }

        /// <summary>
        /// Determina o código confuso de 128 bit de um valor em arquitectura x64.
        /// </summary>
        /// <param name="key">O valor do qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice incial.</param>
        /// <param name="len">O tamanho dos dados.</param>
        /// <param name="seed">Um valor inicial.</param>
        /// <returns>O código confuso.</returns>
        internal byte[] InternalMurmurHash3B128(
            byte[] key,
            int start,
            int len,
            uint seed)
        {
            return this.MurmurHash3B128(key, start, len, seed);
        }

        #endregion Funções internas

        #region Funções privadas

        /// <summary>
        /// Determina o código confuso 32 bit de um valor em arquitectura x86.
        /// </summary>
        /// <param name="key">O valor do qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice incial.</param>
        /// <param name="len">O tamanho dos dados.</param>
        /// <param name="seed">Um valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private uint MurmurHash3A32(
            byte[] key,
            int start,
            int len,
            uint seed)
        {
            var nblocks = len / 4;
            var h1 = seed;

            var c1 = 0xcc9e2d51U;
            var c2 = 0x1b873593U;

            var p = start + nblocks * 4;
            for (int i = start; i < p; i += 4)
            {
                var k1 = BitConverter.ToUInt32(key, i);

                k1 *= c1;
                k1 = Utils.RotateLeft(k1, 15);
                k1 *= c2;

                h1 ^= k1;
                h1 = Utils.RotateLeft(h1, 13);
                h1 = h1 * 5 + 0xe6546b64;
            }

            var ok1 = 0U;

            switch (len & 3)
            {
                case 3:
                    ok1 ^= ((uint)key[p + 2]) << 16;
                    ok1 ^= ((uint)key[p + 1]) << 8;
                    ok1 ^= (uint)key[p];
                    ok1 *= c1;
                    ok1 = Utils.RotateLeft(ok1, 15);
                    ok1 *= c2;
                    h1 ^= ok1;
                    break;
                case 2:
                    ok1 ^= ((uint)key[p + 1]) << 8;
                    ok1 ^= (uint)key[p];
                    ok1 *= c1;
                    ok1 = Utils.RotateLeft(ok1, 15);
                    ok1 *= c2;
                    h1 ^= ok1;
                    break;
                case 1:
                    ok1 ^= (uint)key[p];
                    ok1 *= c1;
                    ok1 = Utils.RotateLeft(ok1, 15);
                    ok1 *= c2;
                    h1 ^= ok1;
                    break;
            };

            h1 ^= (uint)len;
            h1 = this.Fmix32(h1);

            return h1;
        }

        /// <summary>
        /// Determina o código confuso de 128 bit de um valor em arquitectura x86.
        /// </summary>
        /// <param name="key">O valor do qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice incial.</param>
        /// <param name="len">O tamanho dos dados.</param>
        /// <param name="seed">Um valor inicial.</param>
        /// <returns>O código confuso.</returns>
        byte[] MurmurHash3A128(
            byte[] key,
            int start,
            int len,
            uint seed)
        {
            var nblocks = len / 16;

            var h1 = seed;
            var h2 = seed;
            var h3 = seed;
            var h4 = seed;

            var c1 = 0x239b961bU;
            var c2 = 0xab0e9789U;
            var c3 = 0x38b34ae5U;
            var c4 = 0xa1e38b93U;

            for (int i = start; i < nblocks; ++i)
            {
                var k1 = BitConverter.ToUInt32(key, i * 16 + 0);
                var k2 = BitConverter.ToUInt32(key, i * 16 + 4);
                var k3 = BitConverter.ToUInt32(key, i * 16 + 8);
                var k4 = BitConverter.ToUInt32(key, i * 16 + 12);

                k1 *= c1;
                k1 = Utils.RotateLeft(k1, 15);
                k1 *= c2;
                h1 ^= k1;

                h1 = Utils.RotateLeft(h1, 19);
                h1 += h2;
                h1 = h1 * 5 + 0x561ccd1b;

                k2 *= c2;
                k2 = Utils.RotateLeft(k2, 16);
                k2 *= c3;
                h2 ^= k2;

                h2 = Utils.RotateLeft(h2, 17);
                h2 += h3;
                h2 = h2 * 5 + 0x0bcaa747;

                k3 *= c3;
                k3 = Utils.RotateLeft(k3, 17);
                k3 *= c4;
                h3 ^= k3;

                h3 = Utils.RotateLeft(h3, 15);
                h3 += h4;
                h3 = h3 * 5 + 0x96cd1c35;

                k4 *= c4;
                k4 = Utils.RotateLeft(k4, 18);
                k4 *= c1;
                h4 ^= k4;

                h4 = Utils.RotateLeft(h4, 13);
                h4 += h1;
                h4 = h4 * 5 + 0x32ac3b17;
            }

            var ok1 = 0U;
            var ok2 = 0U;
            var ok3 = 0U;
            var ok4 = 0U;

            var p = start + nblocks * 16;
            switch (len & 15)
            {
                case 15:
                    ok4 ^= ((uint)key[p + 14]) << 16;
                    goto case 14;
                case 14:
                    ok4 ^= ((uint)key[p + 13]) << 8;
                    goto case 13;
                case 13: ok4 ^= ((uint)key[p + 12]) << 0;
                    ok4 *= c4;
                    ok4 = Utils.RotateLeft(ok4, 18);
                    ok4 *= c1;
                    h4 ^= ok4;
                    goto case 12;
                case 12:
                    ok3 ^= ((uint)key[p + 11]) << 24;
                    goto case 11;
                case 11:
                    ok3 ^= ((uint)key[p + 10]) << 16;
                    goto case 10;
                case 10:
                    ok3 ^= ((uint)key[p + 9]) << 8;
                    goto case 9;
                case 9:
                    ok3 ^= ((uint)key[p + 8]) << 0;
                    ok3 *= c3;
                    ok3 = Utils.RotateLeft(ok3, 17);
                    ok3 *= c4;
                    h3 ^= ok3;
                    goto case 8;
                case 8:
                    ok2 ^= ((uint)key[p + 7]) << 24;
                    goto case 7;
                case 7:
                    ok2 ^= ((uint)key[p + 6]) << 16;
                    goto case 6;
                case 6:
                    ok2 ^= ((uint)key[p + 5]) << 8;
                    goto case 5;
                case 5:
                    ok2 ^= ((uint)key[p + 4]) << 0;
                    ok2 *= c2;
                    ok2 = Utils.RotateLeft(ok2, 16);
                    ok2 *= c3; h2 ^= ok2;
                    goto case 4;
                case 4:
                    ok1 ^= ((uint)key[p + 3]) << 24;
                    goto case 3;
                case 3:
                    ok1 ^= ((uint)key[p + 2]) << 16;
                    goto case 2;
                case 2:
                    ok1 ^= ((uint)key[p + 1]) << 8;
                    goto case 1;
                case 1:
                    ok1 ^= ((uint)key[p]) << 0;
                    ok1 *= c1;
                    ok1 = Utils.RotateLeft(ok1, 15);
                    ok1 *= c2;
                    h1 ^= ok1;
                    break;
            };

            h1 ^= (uint)len;
            h2 ^= (uint)len;
            h3 ^= (uint)len;
            h4 ^= (uint)len;

            h1 += h2;
            h1 += h3;
            h1 += h4;
            h2 += h1;
            h3 += h1;
            h4 += h1;

            h1 = this.Fmix32(h1);
            h2 = this.Fmix32(h2);
            h3 = this.Fmix32(h3);
            h4 = this.Fmix32(h4);

            h1 += h2;
            h1 += h3;
            h1 += h4;
            h2 += h1;
            h3 += h1;
            h4 += h1;

            var temp = new byte[16];
            Array.Copy(BitConverter.GetBytes(h1), temp, 4);
            Array.Copy(BitConverter.GetBytes(h2), 0, temp, 4, 4);
            Array.Copy(BitConverter.GetBytes(h3), 0, temp, 8, 4);
            Array.Copy(BitConverter.GetBytes(h4), 0, temp, 12, 4);

            return temp;
        }

        /// <summary>
        /// Determina o código confuso de 128 bit de um valor em arquitectura x64.
        /// </summary>
        /// <param name="key">O valor do qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice incial.</param>
        /// <param name="len">O tamanho dos dados.</param>
        /// <param name="seed">Um valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private byte[] MurmurHash3B128(
            byte[] key,
            int start,
            int len,
            uint seed)
        {
            var nblocks = len / 16;

            var h1 = (ulong)seed;
            var h2 = (ulong)seed;

            var c1 = 0x87c37b91114253d5UL;
            var c2 = 0x4cf5ad432745937fUL;

            for (int i = start; i < nblocks; ++i)
            {
                var k1 = BitConverter.ToUInt64(key, i * 16);
                var k2 = BitConverter.ToUInt64(key, i * 16 + 8);

                k1 *= c1;
                k1 = Utils.RotateLeft(k1, 31);
                k1 *= c2;
                h1 ^= k1;

                h1 = Utils.RotateLeft(h1, 27);
                h1 += h2;
                h1 = h1 * 5 + 0x52dce729;

                k2 *= c2;
                k2 = Utils.RotateLeft(k2, 33);
                k2 *= c1;
                h2 ^= k2;

                h2 = Utils.RotateLeft(h2, 31);
                h2 += h1;
                h2 = h2 * 5 + 0x38495ab5;
            }

            //----------
            // tail

            var p = start + nblocks * 16;
            var ok1 = 0UL;
            var ok2 = 0UL;

            switch (len & 15)
            {
                case 15:
                    ok2 ^= ((ulong)key[p + 14]) << 48;
                    goto case 14;
                case 14:
                    ok2 ^= ((ulong)key[p + 13]) << 40;
                    goto case 13;
                case 13:
                    ok2 ^= ((ulong)key[p + 12]) << 32;
                    goto case 12;
                case 12:
                    ok2 ^= ((ulong)key[p + 11]) << 24;
                    goto case 11;
                case 11:
                    ok2 ^= ((ulong)key[p + 10]) << 16;
                    goto case 10;
                case 10:
                    ok2 ^= ((ulong)key[p + 9]) << 8;
                    goto case 9;
                case 9: ok2 ^= ((ulong)key[p + 8]) << 0;
                    ok2 *= c2;
                    ok2 = Utils.RotateLeft(ok2, 33);
                    ok2 *= c1;
                    h2 ^= ok2;
                    goto case 8;
                case 8:
                    ok1 ^= ((ulong)key[p + 7]) << 56;
                    goto case 7;
                case 7:
                    ok1 ^= ((ulong)key[p + 6]) << 48;
                    goto case 6;
                case 6:
                    ok1 ^= ((ulong)key[p + 5]) << 40;
                    goto case 5;
                case 5:
                    ok1 ^= ((ulong)key[p + 4]) << 32;
                    goto case 4;
                case 4:
                    ok1 ^= ((ulong)key[p + 3]) << 24;
                    goto case 3;
                case 3:
                    ok1 ^= ((ulong)key[p + 2]) << 16;
                    goto case 2;
                case 2:
                    ok1 ^= ((ulong)key[p + 1]) << 8;
                    goto case 1;
                case 1:
                    ok1 ^= ((ulong)key[p]) << 0;
                    ok1 *= c1;
                    ok1 = Utils.RotateLeft(ok1, 31);
                    ok1 *= c2;
                    h1 ^= ok1;
                    break;
            };

            //----------
            // finalization

            h1 ^= (ulong)len;
            h2 ^= (ulong)len;

            h1 += h2;
            h2 += h1;

            h1 = this.Fmix64(h1);
            h2 = this.Fmix64(h2);

            h1 += h2;
            h2 += h1;

            var temp = new byte[16];
            Array.Copy(BitConverter.GetBytes(h1), temp, 8);
            Array.Copy(BitConverter.GetBytes(h2), 0, temp, 8, 8);
            return temp;
        }

        /// <summary>
        /// Mistura os bits de um valor.
        /// </summary>
        /// <param name="h">O valor.</param>
        /// <returns>O resultado da mistura.</returns>
        private uint Fmix32(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        /// <summary>
        /// Mistura os bits de um valor.
        /// </summary>
        /// <param name="k">O valor.</param>
        /// <returns>O resultado da mistura.</returns>
        private ulong Fmix64(ulong k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccdUL;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53UL;
            k ^= k >> 33;

            return k;
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Calcular o código confuso Murmur de forma incremental.
    /// </summary>
    public class IncrementalMurmurHash2A
    {
        #region Campos privados

        /// <summary>
        /// Constante para mistura.
        /// </summary>
        private const int m = 0x5bd1e995;

        /// <summary>
        /// Rotação a aplicar durante a mistura.
        /// </summary>
        private const int r = 24;

        /// <summary>
        /// O valor incremental do código confuso.
        /// </summary>
        private uint mHash;

        /// <summary>
        /// Um valor calculado para mistura.
        /// </summary>
        private uint mTail;

        /// <summary>
        /// Contagem de passos até que seja realizada a mistura
        /// com o valor calculado.
        /// </summary>
        private uint mCount;

        /// <summary>
        /// O tamanho total do texto adicionado.
        /// </summary>
        private uint mSize;

        #endregion Campos privados

        #region Funções públicas

        /// <summary>
        /// Inicializa o cálculo do código confuso.
        /// </summary>
        /// <remarks>
        /// A execução da função reinicializa o processo e o texto
        /// anteriormente adicionado é desconsiderado.
        /// </remarks>
        /// <param name="seed">O valor de inicialização.</param>
        public void Begin(uint seed = 0)
        {
            this.mHash = seed;
            this.mTail = 0;
            this.mCount = 0;
            this.mSize = 0;
        }

        /// <summary>
        /// Inclui um vector de bytes no código actual.
        /// </summary>
        /// <param name="data">O vector de bytes.</param>
        /// <param name="len">O comprimento a ser considerado.</param>
        public void Add(byte[] data, int len)
        {
            this.mSize += (uint)len;

            this.MixTail(data, len);

            var p = 0;
            while (len >= 4)
            {
                var k = BitConverter.ToUInt32(data, p);
                MurmurHash2.Mix(ref this.mHash, ref k, m, r);
                p += 4;
                len -= 4;
            }

            len = this.MixTail(data, len);
        }

        /// <summary>
        /// Obtém o valor actual do código confuso.
        /// </summary>
        /// <returns>O valor do código confuso.</returns>
        public uint GetCurrentHash()
        {
            var mHashTemp = this.mHash;
            var mTailTemp = this.mTail;
            var currentSize = this.mSize;
            MurmurHash2.Mix(ref mHashTemp, ref mTailTemp, m, r);
            MurmurHash2.Mix(ref mHashTemp, ref currentSize, m, r);

            mHashTemp ^= mHashTemp >> 13;
            mHashTemp *= m;
            mHashTemp ^= mHashTemp >> 15;

            return mHashTemp;
        }

        #endregion Funções públicas

        /// <summary>
        /// Efectua a mistura da cauda.
        /// </summary>
        /// <param name="data">Os dados sobre os quais se realiza a mistura.</param>
        /// <param name="len">O comprimento total.</param>
        /// <returns>O comprimento em falta.</returns>
        private int MixTail(byte[] data, int len)
        {
            var innerLen = len;
            var p = 0;
            while (innerLen > 0 && ((innerLen < 4) || this.mCount > 0))
            {
                this.mTail |= BitConverter.ToUInt32(data, p++) << (int)(this.mCount * 8);

                this.mCount++;
                innerLen--;

                if (this.mCount == 4)
                {
                    MurmurHash2.Mix(ref this.mHash, ref this.mTail, m, r);
                    this.mTail = 0;
                    this.mCount = 0;
                }
            }

            return innerLen;
        }
    }

    /// <summary>
    /// Implementa o algoritmo FNV para códigos confusos.
    /// </summary>
    /// <remarks>
    /// http://www.isthe.com/chongo/tech/comp/fnv/index.html#FNV-reference-source
    /// </remarks>
    public class FNVHash :
        IHash32<string>,
        IHash32<byte[]>,
        IHash64<string>,
        IHash64<byte[]>,
        IHashN<string>,
        IHashN<byte[]>
    {
        #region Campos privados

        /// <summary>
        /// O valor de inicialização para o FNV-0 de 32 bit.
        /// </summary>
        private const uint FNV032Init = 0;

        /// <summary>
        /// O valor de inicialização para o FNV-1 de 32 bit.
        /// </summary>
        private const uint FNV132Init = 0x811C9DC5U;

        /// <summary>
        /// O valor de inicialização para o FNV-0 de 64 bit.
        /// </summary>
        private const ulong FNV064Init = 0;

        /// <summary>
        /// O valor de inicialização para o FNV-1 de 64 bit.
        /// </summary>
        private const ulong FNV164Init = 0xCBF29CE484222325UL;

        #endregion Campos privados

        #region Propriedades

        /// <summary>
        /// Obtém a constante de inicialização do FNV 32 bit.
        /// </summary>
        public uint FNV032InitConst
        {
            get
            {
                return FNV032Init;
            }
        }

        /// <summary>
        /// Obtém a constante de inicialização do FNV-A 32 bit.
        /// </summary>
        public uint FNV132InitConst
        {
            get
            {
                return FNV132Init;
            }
        }

        /// <summary>
        /// Obtém a constante de inicialização do FNV 64 bit.
        /// </summary>
        public ulong FNV064InitConst
        {
            get
            {
                return FNV064Init;
            }
        }

        /// <summary>
        /// Obtém a constante de inicialização do FNV-A 64 bit.
        /// </summary>
        public ulong FNV164InitConst
        {
            get
            {
                return FNV164Init;
            }
        }

        #endregion

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
                return this.FNVHash32A(bytes, bytes.Length, FNV132Init);
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
                return this.FNVHash32A(obj, obj.Length, FNV132Init);
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
                return this.FNVHash64A(bytes, bytes.Length, FNV164Init);
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
                return this.FNVHash64A(obj, obj.Length, FNV164Init);
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
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var objData = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                if (bytes == 4)
                {
                    return this.GetHash32(obj);
                }
                else if (bytes == 8)
                {
                    return this.GetHash64(obj);
                }
                else
                {
                    throw new NotSupportedException("The provided number of bytes is not supported for hash function.");
                }
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
            if (obj == null)
            {
                return 0;
            }
            else
            {
                if (bytes == 4)
                {
                    return this.GetHash32(obj);
                }
                else if (bytes == 8)
                {
                    return this.GetHash64(obj);
                }
                else
                {
                    throw new NotSupportedException("The provided number of bytes is not supported for hash function.");
                }
            }
        }

        #endregion Funções públicas

        #region Funções internas

        /// <summary>
        /// Obtém o código confuso FNV de 32 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        internal uint InternalGetFNVHash32(byte[] obj, int length, uint hval)
        {
            return this.FNVHash32(obj, length, hval);
        }

        /// <summary>
        /// Obtém o código confuso FNV-A de 32 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        internal uint InternalGetFNVHash32A(byte[] obj, int length, uint hval)
        {
            return this.FNVHash32A(obj, length, hval);
        }

        /// <summary>
        /// Obtém o código confuso FNV de 64 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        internal ulong InternalGetFNVHash64(byte[] obj, int length, ulong hval)
        {
            return this.FNVHash64(obj, length, hval);
        }

        /// <summary>
        /// Obtém o código confuso FNV-A de 64 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        internal ulong InternalGetFNVHash64A(byte[] obj, int length, ulong hval)
        {
            return this.FNVHash64A(obj, length, hval);
        }

        #endregion Funções internas

        #region Funções privadas

        /// <summary>
        /// Obtém o código confuso FNV de 32 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        private uint FNVHash32(byte[] obj, int length, uint hval)
        {
            var result = hval;
            for (var i = 0; i < length; ++i)
            {
                result += (result << 1) + (result << 4) + (result << 7) + (result << 8) + (result << 24);
                result ^= obj[i];
            }

            return result;
        }

        /// <summary>
        /// Obtém o código confuso FNV-A de 32 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        private uint FNVHash32A(byte[] obj, int length, uint hval)
        {
            var result = hval;
            for (var i = 0; i < length; ++i)
            {
                result ^= obj[i];
                result += (result << 1) + (result << 4) + (result << 7) + (result << 8) + (result << 24);
            }

            return result;
        }

        /// <summary>
        /// Obtém o código confuso FNV de 64 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        private ulong FNVHash64(byte[] obj, int length, ulong hval)
        {
            var result = hval;
            for (var i = 0; i < length; ++i)
            {
                result += (result << 1) + (result << 4) + (result << 5) + (result << 7) + (result << 8) + (result << 40);
                result ^= obj[i];
            }

            return result;
        }

        /// <summary>
        /// Obtém o código confuso FNV-A de 64 bit.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="length">O comprimento a ser considerado.</param>
        /// <param name="hval">O valor inicial.</param>
        /// <returns>O valor do código confuso.</returns>
        private ulong FNVHash64A(byte[] obj, int length, ulong hval)
        {
            var result = hval;
            for (var i = 0; i < length; ++i)
            {
                result ^= obj[i];
                result += (result << 1) + (result << 4) + (result << 5) + (result << 7) + (result << 8) + (result << 40);
            }

            return result;
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa o algoritmo do código confuso do Spooky.
    /// </summary>
    /// <remarks>Suporta o método incremental.</remarks>
    public class SpookyHashV1 :
        IHash32<string>,
        IHash32<byte[]>,
        IHash64<string>,
        IHash64<byte[]>,
        IHashN<string>,
        IHashN<byte[]>
    {
        #region Campos privados

        /// <summary>
        /// Número de inteiros longos no estado interno.
        /// </summary>
        private const int scNumVars = 12;

        /// <summary>
        /// Tamanho do estado interno.
        /// </summary>
        private const int scBlockSize = scNumVars * 8;

        /// <summary>
        /// Tamanho em bytes do amortecedor com dados ainda não processados.
        /// </summary>
        private const int scBufSize = scBlockSize * 2;

        /// <summary>
        /// Uma constante.
        /// </summary>
        /// <remarks>
        /// As propriedades da constante são:
        /// 1 - É não nula.
        /// 2 - É ímpar.
        /// 3 - Não constitui uma mistura regular de zeros e uns.
        /// 4 - Não possui qualquer outra propriedade notável.
        /// </remarks>
        private const ulong scConst = 0xDEADBEEFDEADBEEFUL;

        /// <summary>
        /// Indica se é suportado o alinhamento das leituras.
        /// </summary>
        private bool allowUnalignedReads;

        #endregion Campos privados

        #region Construtores

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SpookyHashV1"/>.
        /// </summary>
        public SpookyHashV1()
        {
            this.allowUnalignedReads = true;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SpookyHashV1"/>.
        /// </summary>
        /// <param name="allowUnalignedReads">Valor que indica se será suportado o alinhamento da leitura.</param>
        public SpookyHashV1(bool allowUnalignedReads)
        {
            this.allowUnalignedReads = allowUnalignedReads;
        }

        #endregion Construtores

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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(bytes, 0, bytes.Length, ref low, ref high);
                return (uint)low;
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(obj, 0, obj.Length, ref low, ref high);
                return (uint)low;
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(bytes, 0, bytes.Length, ref low, ref high);
                return low;
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(obj, 0, obj.Length, ref low, ref high);
                return low;
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
                var message = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                var low = 0UL;
                var high = 0UL;
                this.Hash128(message, 0, message.Length, ref low, ref high);
                return this.GetValue(low, high);
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(obj, 0, obj.Length, ref low, ref high);
                return this.GetValue(low, high);
            }
            else
            {
                throw new NotSupportedException("Hash bytes number is not supported.");
            }
        }

        #endregion Funções públicas

        #region Funções internas

        /// <summary>
        /// Para efeitos de teste da função principal.
        /// </summary>
        /// <param name="message">A mensagem a ser testada.</param>
        /// <param name="start">O índice a partir do qual é aplicado o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem à qual se pretende aplicar o algoritmo.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O par (baixo, alto) com os valores parciais do código confuso.</returns>
        internal Tuple<ulong, ulong> InternalGetHash128(
            byte[] message,
            int start,
            int length,
            ulong seed)
        {
            var low = seed;
            var high = seed;
            this.Hash128(message, start, length, ref low, ref high);
            return Tuple.Create(low, high);
        }

        #endregion Funções internas

        #region Funções privadas

        /// <summary>
        /// Determina o código confuso de 128 bit.
        /// </summary>
        /// <param name="message">A mensagem da qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice do início da mensagem.</param>
        /// <param name="length">O tamanho na mensagem a ser considerado.</param>
        /// <param name="low">O valor baixo alterado do código confuso.</param>
        /// <param name="high">O valor alto alterado do código confuso.</param>
        private void Hash128(
            byte[] message,
            int start,
            int length,
            ref ulong low,
            ref ulong high)
        {
            if (length < scBufSize)
            {
                // Aplica a fórmula curta.
                this.Short(message, start, length, ref low, ref high);
            }
            else
            {
                var buf = new byte[scNumVars * 8];
                var h0 = low;
                var h3 = low;
                var h6 = low;
                var h9 = low;
                var h1 = high;
                var h4 = high;
                var h7 = high;
                var h10 = high;
                var h2 = scConst;
                var h5 = scConst;
                var h8 = scConst;
                var h11 = scConst;

                var pointer = start;
                var endLength = (length / scBlockSize) * scNumVars * 8;
                var displacement = scNumVars * 8;
                if (allowUnalignedReads)
                {
                    for (; pointer < endLength; pointer += displacement)
                    {
                        this.Mix(
                            message,
                            pointer,
                            ref h0,
                            ref h1,
                            ref h2,
                            ref h3,
                            ref h4,
                            ref h5,
                            ref h6,
                            ref h7,
                            ref h8,
                            ref h9,
                            ref h10,
                            ref h11);
                    }
                }
                else
                {
                    for (; pointer < endLength; pointer += displacement)
                    {
                        Array.Copy(message, pointer, buf, 0, scBlockSize * 8);
                        this.Mix(
                            buf,
                            pointer,
                            ref h0,
                            ref h1,
                            ref h2,
                            ref h3,
                            ref h4,
                            ref h5,
                            ref h6,
                            ref h7,
                            ref h8,
                            ref h9,
                            ref h10,
                            ref h11);
                    }
                }

                var remainder = length - endLength;
                Array.Clear(buf, 0, buf.Length);
                Array.Copy(message, pointer, buf, 0, remainder);
                buf[scBlockSize - 1] = (byte)remainder;
                this.Mix(
                            buf,
                            0,
                            ref h0,
                            ref h1,
                            ref h2,
                            ref h3,
                            ref h4,
                            ref h5,
                            ref h6,
                            ref h7,
                            ref h8,
                            ref h9,
                            ref h10,
                            ref h11);
                this.End(
                    ref h0,
                    ref h1,
                    ref h2,
                    ref h3,
                    ref h4,
                    ref h5,
                    ref h6,
                    ref h7,
                    ref h8,
                    ref h9,
                    ref h10,
                    ref h11);
                low = h0;
                high = h1;
            }
        }

        /// <summary>
        /// Determina o código confuso de 128 bit para mensagens curtas.
        /// </summary>
        /// <param name="message">A mensagem da qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice do início da mensagem.</param>
        /// <param name="length">O tamanho na mensagem a ser considerado.</param>
        /// <param name="low">Os bytes menos significativos de um valor de 128 bit.</param>
        /// <param name="high">Os bytes mais significativos de um valor de 128 bit.</param>
        /// <returns>O código confuso.</returns>
        private void Short(
            byte[] message,
            int start,
            int length,
            ref ulong low,
            ref ulong high)
        {
            var i = start;
            var buf = new ulong[scBufSize];

            var remainder = length % 32;
            var a = low;
            var b = high;
            var c = scConst;
            var d = scConst;

            if (length > 15)
            {
                var end = i + (length / 32) * 32;
                for (; i < end; i += 32)
                {
                    c += BitConverter.ToUInt64(message, i);
                    d += BitConverter.ToUInt64(message, i + 8);
                    this.ShortMix(ref a, ref b, ref c, ref d);
                    a += BitConverter.ToUInt64(message, i + 16);
                    b += BitConverter.ToUInt64(message, i + 24);
                }

                if (remainder >= 16)
                {
                    c += BitConverter.ToUInt64(message, i);
                    d += BitConverter.ToUInt64(message, i + 8);
                    this.ShortMix(ref a, ref b, ref c, ref d);
                    i += 16;
                    remainder -= 16;
                }
            }

            d = ((ulong)length) << 56;
            switch (remainder)
            {
                case 15:
                    d += ((ulong)message[i + 14]) << 48;
                    goto case 14;
                case 14:
                    d += ((ulong)message[i + 13]) << 40;
                    goto case 13;
                case 13:
                    d += ((ulong)message[i + 12]) << 32;
                    goto case 12;
                case 12:
                    d += BitConverter.ToUInt32(message, i + 8);
                    c += BitConverter.ToUInt64(message, i);
                    break;
                case 11:
                    d += ((ulong)message[i + 10]) << 16;
                    goto case 10;
                case 10:
                    d += ((ulong)message[i + 9]) << 8;
                    goto case 9;
                case 9:
                    d += ((ulong)message[i + 8]);
                    goto case 8;
                case 8:
                    c += BitConverter.ToUInt64(message, i);
                    break;
                case 7:
                    c += ((ulong)message[i + 6]) << 48;
                    goto case 6;
                case 6:
                    c += ((ulong)message[i + 5]) << 40;
                    goto case 5;
                case 5:
                    c += ((ulong)message[i + 4]) << 32;
                    goto case 4;
                case 4:
                    c += BitConverter.ToUInt32(message, i);
                    break;
                case 3:
                    c += ((ulong)message[i + 2]) << 16;
                    goto case 2;
                case 2:
                    c += ((ulong)message[i + 1]) << 8;
                    goto case 1;
                case 1:
                    c += ((ulong)message[i]);
                    break;
                case 0:
                    d += scConst;
                    c += scConst;
                    break;
            }

            // Investigar a diferença entre leituras de memória alinhadas e não alinhadas.
            this.ShortEnd(ref a, ref b, ref c, ref d);
            low = a;
            high = b;
        }

        /// <summary>
        /// Efectua a mistura dos estados com os dados.
        /// </summary>
        /// <param name="data">Os dados.</param>
        /// <param name="pointer">O índice inicial dos dados.</param>
        /// <param name="s0">s0.</param>
        /// <param name="s1">s1.</param>
        /// <param name="s2">s2.</param>
        /// <param name="s3">s3.</param>
        /// <param name="s4">s4.</param>
        /// <param name="s5">s5.</param>
        /// <param name="s6">s6.</param>
        /// <param name="s7">s7.</param>
        /// <param name="s8">s8.</param>
        /// <param name="s9">s9.</param>
        /// <param name="s10">s10.</param>
        /// <param name="s11">s11.</param>
        private void Mix(
            byte[] data,
            int pointer,
            ref ulong s0,
            ref ulong s1,
            ref ulong s2,
            ref ulong s3,
            ref ulong s4,
            ref ulong s5,
            ref ulong s6,
            ref ulong s7,
            ref ulong s8,
            ref ulong s9,
            ref ulong s10,
            ref ulong s11)
        {
            var i = pointer;
            s0 += BitConverter.ToUInt64(data, i);
            i += 8;
            s2 ^= s10;
            s11 ^= s0;
            s0 = Utils.RotateLeft(s0, 11);
            s11 += s1;
            s1 += BitConverter.ToUInt64(data, i);
            i += 8;
            s3 ^= s11;
            s0 ^= s1;
            s1 = Utils.RotateLeft(s1, 32);
            s0 += s2;
            s2 += BitConverter.ToUInt64(data, i);
            i += 8;
            s4 ^= s0;
            s1 ^= s2;
            s2 = Utils.RotateLeft(s2, 43);
            s1 += s3;
            s3 += BitConverter.ToUInt64(data, i);
            i += 8;
            s5 ^= s1;
            s2 ^= s3;
            s3 = Utils.RotateLeft(s3, 31);
            s2 += s4;
            s4 += BitConverter.ToUInt64(data, i);
            i += 8;
            s6 ^= s2;
            s3 ^= s4;
            s4 = Utils.RotateLeft(s4, 17);
            s3 += s5;
            s5 += BitConverter.ToUInt64(data, i);
            i += 8;
            s7 ^= s3;
            s4 ^= s5;
            s5 = Utils.RotateLeft(s5, 28);
            s4 += s6;
            s6 += BitConverter.ToUInt64(data, i);
            i += 8;
            s8 ^= s4;
            s5 ^= s6;
            s6 = Utils.RotateLeft(s6, 39);
            s5 += s7;
            s7 += BitConverter.ToUInt64(data, i);
            i += 8;
            s9 ^= s5;
            s6 ^= s7;
            s7 = Utils.RotateLeft(s7, 57);
            s6 += s8;
            s8 += BitConverter.ToUInt64(data, i);
            i += 8;
            s10 ^= s6;
            s7 ^= s8;
            s8 = Utils.RotateLeft(s8, 55);
            s7 += s9;
            s9 += BitConverter.ToUInt64(data, i);
            i += 8;
            s11 ^= s7;
            s8 ^= s9;
            s9 = Utils.RotateLeft(s9, 54);
            s8 += s10;
            s10 += BitConverter.ToUInt64(data, i);
            i += 8;
            s0 ^= s8;
            s9 ^= s10;
            s10 = Utils.RotateLeft(s10, 22);
            s9 += s11;
            s11 += BitConverter.ToUInt64(data, i);
            i += 8;
            s1 ^= s9;
            s10 ^= s11;
            s11 = Utils.RotateLeft(s11, 46);
            s10 += s0;
        }

        /// <summary>
        /// Efectua a mistura final parcial.
        /// </summary>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        /// <param name="h4">h4.</param>
        /// <param name="h5">h5.</param>
        /// <param name="h6">h6.</param>
        /// <param name="h7">h7.</param>
        /// <param name="h8">h8.</param>
        /// <param name="h9">h9.</param>
        /// <param name="h10">h10.</param>
        /// <param name="h11">h11.</param>
        private void EndPartial(
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3,
            ref ulong h4,
            ref ulong h5,
            ref ulong h6,
            ref ulong h7,
            ref ulong h8,
            ref ulong h9,
            ref ulong h10,
            ref ulong h11)
        {
            h11 += h1;
            h2 ^= h11;
            h1 = Utils.RotateLeft(h1, 44);
            h0 += h2;
            h3 ^= h0;
            h2 = Utils.RotateLeft(h2, 15);
            h1 += h3;
            h4 ^= h1;
            h3 = Utils.RotateLeft(h3, 34);
            h2 += h4;
            h5 ^= h2;
            h4 = Utils.RotateLeft(h4, 21);
            h3 += h5;
            h6 ^= h3;
            h5 = Utils.RotateLeft(h5, 38);
            h4 += h6;
            h7 ^= h4;
            h6 = Utils.RotateLeft(h6, 33);
            h5 += h7;
            h8 ^= h5;
            h7 = Utils.RotateLeft(h7, 10);
            h6 += h8;
            h9 ^= h6;
            h8 = Utils.RotateLeft(h8, 13);
            h7 += h9;
            h10 ^= h7;
            h9 = Utils.RotateLeft(h9, 38);
            h8 += h10;
            h11 ^= h8;
            h10 = Utils.RotateLeft(h10, 53);
            h9 += h11;
            h0 ^= h9;
            h11 = Utils.RotateLeft(h11, 42);
            h10 += h0;
            h1 ^= h10;
            h0 = Utils.RotateLeft(h0, 54);
        }

        /// <summary>
        /// Efectua a mistura final.
        /// </summary>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        /// <param name="h4">h4.</param>
        /// <param name="h5">h5.</param>
        /// <param name="h6">h6.</param>
        /// <param name="h7">h7.</param>
        /// <param name="h8">h8.</param>
        /// <param name="h9">h9.</param>
        /// <param name="h10">h10.</param>
        /// <param name="h11">h11.</param>
        private void End(
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3,
            ref ulong h4,
            ref ulong h5,
            ref ulong h6,
            ref ulong h7,
            ref ulong h8,
            ref ulong h9,
            ref ulong h10,
            ref ulong h11)
        {
            this.EndPartial(ref h0, ref h1, ref h2, ref h3, ref h4, ref h5, ref h6, ref h7, ref h8, ref h9, ref h10, ref h11);
            this.EndPartial(ref h0, ref h1, ref h2, ref h3, ref h4, ref h5, ref h6, ref h7, ref h8, ref h9, ref h10, ref h11);
            this.EndPartial(ref h0, ref h1, ref h2, ref h3, ref h4, ref h5, ref h6, ref h7, ref h8, ref h9, ref h10, ref h11);
        }

        /// <summary>
        /// Efectua uma mistura de valores de pequenas dimensões.
        /// </summary>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        private void ShortMix(
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3)
        {
            h2 = Utils.RotateLeft(h2, 50);
            h2 += h3;
            h0 ^= h2;
            h3 = Utils.RotateLeft(h3, 52);
            h3 += h0;
            h1 ^= h3;
            h0 = Utils.RotateLeft(h0, 30);
            h0 += h1;
            h2 ^= h0;
            h1 = Utils.RotateLeft(h1, 41);
            h1 += h2;
            h3 ^= h1;
            h2 = Utils.RotateLeft(h2, 54);
            h2 += h3;
            h0 ^= h2;
            h3 = Utils.RotateLeft(h3, 48);
            h3 += h0;
            h1 ^= h3;
            h0 = Utils.RotateLeft(h0, 38);
            h0 += h1;
            h2 ^= h0;
            h1 = Utils.RotateLeft(h1, 37);
            h1 += h2;
            h3 ^= h1;
            h2 = Utils.RotateLeft(h2, 62);
            h2 += h3;
            h0 ^= h2;
            h3 = Utils.RotateLeft(h3, 34);
            h3 += h0;
            h1 ^= h3;
            h0 = Utils.RotateLeft(h0, 5);
            h0 += h1;
            h2 ^= h0;
            h1 = Utils.RotateLeft(h1, 36);
            h1 += h2;
            h3 ^= h1;
        }

        /// <summary>
        /// Efectua a mistura final de valores de pequenas dimensões.
        /// </summary>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        private void ShortEnd(
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3)
        {
            h3 ^= h2;
            h2 = Utils.RotateLeft(h2, 15);
            h3 += h2;
            h0 ^= h3;
            h3 = Utils.RotateLeft(h3, 52);
            h0 += h3;
            h1 ^= h0;
            h0 = Utils.RotateLeft(h0, 26);
            h1 += h0;
            h2 ^= h1;
            h1 = Utils.RotateLeft(h1, 51);
            h2 += h1;
            h3 ^= h2;
            h2 = Utils.RotateLeft(h2, 28);
            h3 += h2;
            h0 ^= h3;
            h3 = Utils.RotateLeft(h3, 9);
            h0 += h3;
            h1 ^= h0;
            h0 = Utils.RotateLeft(h0, 47);
            h1 += h0;
            h2 ^= h1;
            h1 = Utils.RotateLeft(h1, 54);
            h2 += h1;
            h3 ^= h2;
            h2 = Utils.RotateLeft(h2, 32);
            h3 += h2;
            h0 ^= h3;
            h3 = Utils.RotateLeft(h3, 25);
            h0 += h3;
            h1 ^= h0;
            h0 = Utils.RotateLeft(h0, 63);
            h1 += h0;
        }

        /// <summary>
        /// Obtém o valor inteiro a partir das suas partes.
        /// </summary>
        /// <param name="low">O valor baixo.</param>
        /// <param name="high">O valor alto.</param>
        /// <returns>O inteiro.</returns>
        private BigInteger GetValue(ulong low, ulong high)
        {
            var temp = new byte[16];
            var lowBytes = BitConverter.GetBytes(low);
            var highBytes = BitConverter.GetBytes(high);
            Array.Copy(lowBytes, temp, 8);
            Array.Copy(highBytes, 0, temp, 8, 8);
            return new BigInteger(temp);
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa o algoritmo do código confuso do Spooky.
    /// </summary>
    /// <remarks>
    /// Suporta o método incremental.
    /// </remarks>
    public class SpookyHashV2 :
        IHash32<string>,
        IHash32<byte[]>,
        IHash64<string>,
        IHash64<byte[]>,
        IHashN<string>,
        IHashN<byte[]>
    {
        #region Campos privados

        /// <summary>
        /// Número de inteiros longos no estado interno.
        /// </summary>
        private const int scNumVars = 12;

        /// <summary>
        /// Tamanho do estado interno.
        /// </summary>
        private const int scBlockSize = scNumVars * 8;

        /// <summary>
        /// Tamanho em bytes do amortecedor com dados ainda não processados.
        /// </summary>
        private const int scBufSize = scBlockSize * 2;

        /// <summary>
        /// Uma constante.
        /// </summary>
        /// <remarks>
        /// As propriedades da constante são:
        /// 1 - É não nula.
        /// 2 - É ímpar.
        /// 3 - Não constitui uma mistura regular de zeros e uns.
        /// 4 - Não possui qualquer outra propriedade notável.
        /// </remarks>
        private const ulong scConst = 0xDEADBEEFDEADBEEFUL;

        /// <summary>
        /// Indica se é suportado o alinhamento das leituras.
        /// </summary>
        private bool allowUnalignedReads;

        #endregion Campos privados

        #region Construtores

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SpookyHashV1"/>.
        /// </summary>
        public SpookyHashV2()
        {
            this.allowUnalignedReads = true;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SpookyHashV1"/>.
        /// </summary>
        /// <param name="allowUnalignedReads">Valor que indica se será suportado o alinhamento da leitura.</param>
        public SpookyHashV2(bool allowUnalignedReads)
        {
            this.allowUnalignedReads = allowUnalignedReads;
        }

        #endregion Construtores

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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(bytes, 0, bytes.Length, ref low, ref high);
                return (uint)low;
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(obj, 0, obj.Length, ref low, ref high);
                return (uint)low;
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(bytes, 0, bytes.Length, ref low, ref high);
                return low;
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(obj, 0, obj.Length, ref low, ref high);
                return low;
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
                var message = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
                var low = 0UL;
                var high = 0UL;
                this.Hash128(message, 0, message.Length, ref low, ref high);
                return this.GetValue(low, high);
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
                var low = 0UL;
                var high = 0UL;
                this.Hash128(obj, 0, obj.Length, ref low, ref high);
                return this.GetValue(low, high);
            }
            else
            {
                throw new NotSupportedException("Hash bytes number is not supported.");
            }
        }

        #endregion Funções públicas

        #region Funções internas

        /// <summary>
        /// Para efeitos de teste da função principal.
        /// </summary>
        /// <param name="message">A mensagem a ser testada.</param>
        /// <param name="start">O índice a partir do qual é aplicado o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem à qual se pretende aplicar o algoritmo.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O par (baixo, alto) com os valores parciais do código confuso.</returns>
        internal Tuple<ulong, ulong> InternalGetHash128(
            byte[] message,
            int start,
            int length,
            ulong seed)
        {
            var low = seed;
            var high = seed;
            this.Hash128(message, start, length, ref low, ref high);
            return Tuple.Create(low, high);
        }

        #endregion Funções internas

        #region Funções privadas

        /// <summary>
        /// Determina o código confuso de 128 bit.
        /// </summary>
        /// <param name="message">A mensagem da qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice do início da mensagem.</param>
        /// <param name="length">O tamanho na mensagem a ser considerado.</param>
        /// <param name="low">O valor baixo alterado do código confuso.</param>
        /// <param name="high">O valor alto alterado do código confuso.</param>
        private void Hash128(
            byte[] message,
            int start,
            int length,
            ref ulong low,
            ref ulong high)
        {
            if (length < scBufSize)
            {
                // Aplica a fórmula curta.
                this.Short(message, start, length, ref low, ref high);
            }
            else
            {
                var buf = new byte[scNumVars * 8];
                var h0 = low;
                var h3 = low;
                var h6 = low;
                var h9 = low;
                var h1 = high;
                var h4 = high;
                var h7 = high;
                var h10 = high;
                var h2 = scConst;
                var h5 = scConst;
                var h8 = scConst;
                var h11 = scConst;

                var pointer = start;
                var endLength = (length / scBlockSize) * scNumVars * 8;
                var displacement = scNumVars * 8;
                if (allowUnalignedReads)
                {
                    for (; pointer < endLength; pointer += displacement)
                    {
                        this.Mix(
                            message,
                            pointer,
                            ref h0,
                            ref h1,
                            ref h2,
                            ref h3,
                            ref h4,
                            ref h5,
                            ref h6,
                            ref h7,
                            ref h8,
                            ref h9,
                            ref h10,
                            ref h11);
                    }
                }
                else
                {
                    for (; pointer < endLength; pointer += displacement)
                    {
                        Array.Copy(message, pointer, buf, 0, scBlockSize);
                        this.Mix(
                            buf,
                            pointer,
                            ref h0,
                            ref h1,
                            ref h2,
                            ref h3,
                            ref h4,
                            ref h5,
                            ref h6,
                            ref h7,
                            ref h8,
                            ref h9,
                            ref h10,
                            ref h11);
                    }
                }

                var remainder = length - endLength;
                Array.Clear(buf, 0, buf.Length);
                Array.Copy(message, pointer, buf, 0, remainder);
                buf[scBlockSize - 1] = (byte)remainder;

                this.End(
                    buf,
                    ref h0,
                    ref h1,
                    ref h2,
                    ref h3,
                    ref h4,
                    ref h5,
                    ref h6,
                    ref h7,
                    ref h8,
                    ref h9,
                    ref h10,
                    ref h11);
                low = h0;
                high = h1;
            }
        }

        /// <summary>
        /// Determina o código confuso de 128 bit para mensagens curtas.
        /// </summary>
        /// <param name="message">A mensagem da qual se pretende obter o código confuso.</param>
        /// <param name="start">O índice do início da mensagem.</param>
        /// <param name="length">O tamanho na mensagem a ser considerado.</param>
        /// <param name="low">Os bytes menos significativos de um valor de 128 bit.</param>
        /// <param name="high">Os bytes mais significativos de um valor de 128 bit.</param>
        /// <returns>O código confuso.</returns>
        private void Short(
            byte[] message,
            int start,
            int length,
            ref ulong low,
            ref ulong high)
        {
            var i = start;
            var buf = new ulong[scBufSize];

            var remainder = length % 32;
            var a = low;
            var b = high;
            var c = scConst;
            var d = scConst;

            if (length > 15)
            {
                var end = i + (length / 32) * 32;
                for (; i < end; i += 32)
                {
                    c += BitConverter.ToUInt64(message, i);
                    d += BitConverter.ToUInt64(message, i + 8);
                    this.ShortMix(ref a, ref b, ref c, ref d);
                    a += BitConverter.ToUInt64(message, i + 16);
                    b += BitConverter.ToUInt64(message, i + 24);
                }

                if (remainder >= 16)
                {
                    c += BitConverter.ToUInt64(message, i);
                    d += BitConverter.ToUInt64(message, i + 8);
                    this.ShortMix(ref a, ref b, ref c, ref d);
                    i += 16;
                    remainder -= 16;
                }
            }

            d += ((ulong)length) << 56;
            switch (remainder)
            {
                case 15:
                    d += ((ulong)message[i + 14]) << 48;
                    goto case 14;
                case 14:
                    d += ((ulong)message[i + 13]) << 40;
                    goto case 13;
                case 13:
                    d += ((ulong)message[i + 12]) << 32;
                    goto case 12;
                case 12:
                    d += BitConverter.ToUInt32(message, i + 8);
                    c += BitConverter.ToUInt64(message, i);
                    break;
                case 11:
                    d += ((ulong)message[i + 10]) << 16;
                    goto case 10;
                case 10:
                    d += ((ulong)message[i + 9]) << 8;
                    goto case 9;
                case 9:
                    d += ((ulong)message[i + 8]);
                    goto case 8;
                case 8:
                    c += BitConverter.ToUInt64(message, i);
                    break;
                case 7:
                    c += ((ulong)message[i + 6]) << 48;
                    goto case 6;
                case 6:
                    c += ((ulong)message[i + 5]) << 40;
                    goto case 5;
                case 5:
                    c += ((ulong)message[i + 4]) << 32;
                    goto case 4;
                case 4:
                    c += BitConverter.ToUInt32(message, i);
                    break;
                case 3:
                    c += ((ulong)message[i + 2]) << 16;
                    goto case 2;
                case 2:
                    c += ((ulong)message[i + 1]) << 8;
                    goto case 1;
                case 1:
                    c += ((ulong)message[i]);
                    break;
                case 0:
                    d += scConst;
                    c += scConst;
                    break;
            }

            // Investigar a diferença entre leituras de memória alinhadas e não alinhadas.
            this.ShortEnd(ref a, ref b, ref c, ref d);
            low = a;
            high = b;
        }

        /// <summary>
        /// Efectua a mistura dos estados com os dados.
        /// </summary>
        /// <param name="data">Os dados.</param>
        /// <param name="pointer">O índice inicial dos dados.</param>
        /// <param name="s0">s0.</param>
        /// <param name="s1">s1.</param>
        /// <param name="s2">s2.</param>
        /// <param name="s3">s3.</param>
        /// <param name="s4">s4.</param>
        /// <param name="s5">s5.</param>
        /// <param name="s6">s6.</param>
        /// <param name="s7">s7.</param>
        /// <param name="s8">s8.</param>
        /// <param name="s9">s9.</param>
        /// <param name="s10">s10.</param>
        /// <param name="s11">s11.</param>
        private void Mix(
            byte[] data,
            int pointer,
            ref ulong s0,
            ref ulong s1,
            ref ulong s2,
            ref ulong s3,
            ref ulong s4,
            ref ulong s5,
            ref ulong s6,
            ref ulong s7,
            ref ulong s8,
            ref ulong s9,
            ref ulong s10,
            ref ulong s11)
        {
            var i = pointer;
            s0 += BitConverter.ToUInt64(data, i);
            i += 8;
            s2 ^= s10;
            s11 ^= s0;
            s0 = Utils.RotateLeft(s0, 11);
            s11 += s1;
            s1 += BitConverter.ToUInt64(data, i);
            i += 8;
            s3 ^= s11;
            s0 ^= s1;
            s1 = Utils.RotateLeft(s1, 32);
            s0 += s2;
            s2 += BitConverter.ToUInt64(data, i);
            i += 8;
            s4 ^= s0;
            s1 ^= s2;
            s2 = Utils.RotateLeft(s2, 43);
            s1 += s3;
            s3 += BitConverter.ToUInt64(data, i);
            i += 8;
            s5 ^= s1;
            s2 ^= s3;
            s3 = Utils.RotateLeft(s3, 31);
            s2 += s4;
            s4 += BitConverter.ToUInt64(data, i);
            i += 8;
            s6 ^= s2;
            s3 ^= s4;
            s4 = Utils.RotateLeft(s4, 17);
            s3 += s5;
            s5 += BitConverter.ToUInt64(data, i);
            i += 8;
            s7 ^= s3;
            s4 ^= s5;
            s5 = Utils.RotateLeft(s5, 28);
            s4 += s6;
            s6 += BitConverter.ToUInt64(data, i);
            i += 8;
            s8 ^= s4;
            s5 ^= s6;
            s6 = Utils.RotateLeft(s6, 39);
            s5 += s7;
            s7 += BitConverter.ToUInt64(data, i);
            i += 8;
            s9 ^= s5;
            s6 ^= s7;
            s7 = Utils.RotateLeft(s7, 57);
            s6 += s8;
            s8 += BitConverter.ToUInt64(data, i);
            i += 8;
            s10 ^= s6;
            s7 ^= s8;
            s8 = Utils.RotateLeft(s8, 55);
            s7 += s9;
            s9 += BitConverter.ToUInt64(data, i);
            i += 8;
            s11 ^= s7;
            s8 ^= s9;
            s9 = Utils.RotateLeft(s9, 54);
            s8 += s10;
            s10 += BitConverter.ToUInt64(data, i);
            i += 8;
            s0 ^= s8;
            s9 ^= s10;
            s10 = Utils.RotateLeft(s10, 22);
            s9 += s11;
            s11 += BitConverter.ToUInt64(data, i);
            i += 8;
            s1 ^= s9;
            s10 ^= s11;
            s11 = Utils.RotateLeft(s11, 46);
            s10 += s0;
        }

        /// <summary>
        /// Efectua a mistura final parcial.
        /// </summary>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        /// <param name="h4">h4.</param>
        /// <param name="h5">h5.</param>
        /// <param name="h6">h6.</param>
        /// <param name="h7">h7.</param>
        /// <param name="h8">h8.</param>
        /// <param name="h9">h9.</param>
        /// <param name="h10">h10.</param>
        /// <param name="h11">h11.</param>
        private void EndPartial(
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3,
            ref ulong h4,
            ref ulong h5,
            ref ulong h6,
            ref ulong h7,
            ref ulong h8,
            ref ulong h9,
            ref ulong h10,
            ref ulong h11)
        {
            h11 += h1;
            h2 ^= h11;
            h1 = Utils.RotateLeft(h1, 44);
            h0 += h2;
            h3 ^= h0;
            h2 = Utils.RotateLeft(h2, 15);
            h1 += h3;
            h4 ^= h1;
            h3 = Utils.RotateLeft(h3, 34);
            h2 += h4;
            h5 ^= h2;
            h4 = Utils.RotateLeft(h4, 21);
            h3 += h5;
            h6 ^= h3;
            h5 = Utils.RotateLeft(h5, 38);
            h4 += h6;
            h7 ^= h4;
            h6 = Utils.RotateLeft(h6, 33);
            h5 += h7;
            h8 ^= h5;
            h7 = Utils.RotateLeft(h7, 10);
            h6 += h8;
            h9 ^= h6;
            h8 = Utils.RotateLeft(h8, 13);
            h7 += h9;
            h10 ^= h7;
            h9 = Utils.RotateLeft(h9, 38);
            h8 += h10;
            h11 ^= h8;
            h10 = Utils.RotateLeft(h10, 53);
            h9 += h11;
            h0 ^= h9;
            h11 = Utils.RotateLeft(h11, 42);
            h10 += h0;
            h1 ^= h10;
            h0 = Utils.RotateLeft(h0, 54);
        }

        /// <summary>
        /// Efectua a mistura final.
        /// </summary>
        /// <param name="data">Os dados a serem processados.</param>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        /// <param name="h4">h4.</param>
        /// <param name="h5">h5.</param>
        /// <param name="h6">h6.</param>
        /// <param name="h7">h7.</param>
        /// <param name="h8">h8.</param>
        /// <param name="h9">h9.</param>
        /// <param name="h10">h10.</param>
        /// <param name="h11">h11.</param>
        private void End(
            byte[] data,
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3,
            ref ulong h4,
            ref ulong h5,
            ref ulong h6,
            ref ulong h7,
            ref ulong h8,
            ref ulong h9,
            ref ulong h10,
            ref ulong h11)
        {
            h0 += BitConverter.ToUInt64(data, 0);
            h1 += BitConverter.ToUInt64(data, 8);
            h2 += BitConverter.ToUInt64(data, 16);
            h3 += BitConverter.ToUInt64(data, 24);
            h4 += BitConverter.ToUInt64(data, 32);
            h5 += BitConverter.ToUInt64(data, 40);
            h6 += BitConverter.ToUInt64(data, 48);
            h7 += BitConverter.ToUInt64(data, 56);
            h8 += BitConverter.ToUInt64(data, 64);
            h9 += BitConverter.ToUInt64(data, 72);
            h10 += BitConverter.ToUInt64(data, 80);
            h11 += BitConverter.ToUInt64(data, 88);
            this.EndPartial(ref h0, ref h1, ref h2, ref h3, ref h4, ref h5, ref h6, ref h7, ref h8, ref h9, ref h10, ref h11);
            this.EndPartial(ref h0, ref h1, ref h2, ref h3, ref h4, ref h5, ref h6, ref h7, ref h8, ref h9, ref h10, ref h11);
            this.EndPartial(ref h0, ref h1, ref h2, ref h3, ref h4, ref h5, ref h6, ref h7, ref h8, ref h9, ref h10, ref h11);
        }

        /// <summary>
        /// Efectua uma mistura de valores de pequenas dimensões.
        /// </summary>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        private void ShortMix(
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3)
        {
            h2 = Utils.RotateLeft(h2, 50);
            h2 += h3;
            h0 ^= h2;
            h3 = Utils.RotateLeft(h3, 52);
            h3 += h0;
            h1 ^= h3;
            h0 = Utils.RotateLeft(h0, 30);
            h0 += h1;
            h2 ^= h0;
            h1 = Utils.RotateLeft(h1, 41);
            h1 += h2;
            h3 ^= h1;
            h2 = Utils.RotateLeft(h2, 54);
            h2 += h3;
            h0 ^= h2;
            h3 = Utils.RotateLeft(h3, 48);
            h3 += h0;
            h1 ^= h3;
            h0 = Utils.RotateLeft(h0, 38);
            h0 += h1;
            h2 ^= h0;
            h1 = Utils.RotateLeft(h1, 37);
            h1 += h2;
            h3 ^= h1;
            h2 = Utils.RotateLeft(h2, 62);
            h2 += h3;
            h0 ^= h2;
            h3 = Utils.RotateLeft(h3, 34);
            h3 += h0;
            h1 ^= h3;
            h0 = Utils.RotateLeft(h0, 5);
            h0 += h1;
            h2 ^= h0;
            h1 = Utils.RotateLeft(h1, 36);
            h1 += h2;
            h3 ^= h1;
        }

        /// <summary>
        /// Efectua a mistura final de valores de pequenas dimensões.
        /// </summary>
        /// <param name="h0">h0.</param>
        /// <param name="h1">h1.</param>
        /// <param name="h2">h2.</param>
        /// <param name="h3">h3.</param>
        private void ShortEnd(
            ref ulong h0,
            ref ulong h1,
            ref ulong h2,
            ref ulong h3)
        {
            h3 ^= h2;
            h2 = Utils.RotateLeft(h2, 15);
            h3 += h2;
            h0 ^= h3;
            h3 = Utils.RotateLeft(h3, 52);
            h0 += h3;
            h1 ^= h0;
            h0 = Utils.RotateLeft(h0, 26);
            h1 += h0;
            h2 ^= h1;
            h1 = Utils.RotateLeft(h1, 51);
            h2 += h1;
            h3 ^= h2;
            h2 = Utils.RotateLeft(h2, 28);
            h3 += h2;
            h0 ^= h3;
            h3 = Utils.RotateLeft(h3, 9);
            h0 += h3;
            h1 ^= h0;
            h0 = Utils.RotateLeft(h0, 47);
            h1 += h0;
            h2 ^= h1;
            h1 = Utils.RotateLeft(h1, 54);
            h2 += h1;
            h3 ^= h2;
            h2 = Utils.RotateLeft(h2, 32);
            h3 += h2;
            h0 ^= h3;
            h3 = Utils.RotateLeft(h3, 25);
            h0 += h3;
            h1 ^= h0;
            h0 = Utils.RotateLeft(h0, 63);
            h1 += h0;
        }

        /// <summary>
        /// Obtém o valor inteiro a partir das suas partes.
        /// </summary>
        /// <param name="low">O valor baixo.</param>
        /// <param name="high">O valor alto.</param>
        /// <returns>O inteiro.</returns>
        private BigInteger GetValue(ulong low, ulong high)
        {
            var temp = new byte[16];
            var lowBytes = BitConverter.GetBytes(low);
            var highBytes = BitConverter.GetBytes(high);
            Array.Copy(lowBytes, temp, 8);
            Array.Copy(highBytes, 0, temp, 8, 8);
            return new BigInteger(temp);
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa um algoritmo simples para código confuso.
    /// </summary>
    public class PearHash :
        IHash32<string>,
        IHash32<byte[]>,
        IHash64<string>,
        IHash64<byte[]>,
        IHashN<string>,
        IHashN<byte[]>
    {
        #region Campos privados

        /// <summary>
        /// Tabela auxiliar que contém uma ordenação arbitrária dos primeiros 256 valores.
        /// </summary>
        private byte[] hashTable = new byte[]{
            98,  6, 85,150, 36, 23,112,164,135,207,169,  5, 26, 64,165,219,
            61, 20, 68, 89,130, 63, 52,102, 24,229,132,245, 80,216,195,115,
            90,168,156,203,177,120,  2,190,188,  7,100,185,174,243,162, 10, 
            237, 18,253,225,  8,208,172,244,255,126,101, 79,145,235,228,121, 
            123,251, 67,250,161,  0,107, 97,241,111,181, 82,249, 33, 69, 55, 
            59,153, 29,  9,213,167, 84, 93, 30, 46, 94, 75,151,114, 73,222, 
            197, 96,210, 45, 16,227,248,202, 51,152,252,125, 81,206,215,186, 
            39,158,178,187,131,136,  1, 49, 50, 17,141, 91, 47,129, 60, 99, 
            154, 35, 86,171,105, 34, 38,200,147, 58, 77,118,173,246, 76,254, 
            133,232,196,144,198,124, 53,  4,108, 74,223,234,134,230,157,139, 
            189,205,199,128,176, 19,211,236,127,192,231, 70,233, 88,146, 44, 
            183,201, 22, 83, 13,214,116,109,159, 32, 95,226,140,220, 57, 12, 
            221, 31,209,182,143, 92,149,184,148, 62,113, 65, 37, 27,106,166, 
            3, 14,204, 72, 21, 41, 56, 66, 28,193, 40,217, 25, 54,179,117, 
            238, 87,240,155,180,170,242,212,191,163, 78,218,137,194,175,110, 
            43,119,224, 71,122,142, 42,160,104, 48,247,103, 15, 11,138,239  
        };

        #endregion Campos privados

        #region Construtores

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PearHash"/>.
        /// </summary>
        public PearHash() { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PearHash"/>.
        /// </summary>
        /// <param name="table">Uma tabela que contém os primeiros 256 valores 
        /// numa ordem arbitrária.
        /// </param>
        public PearHash(byte[] table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("hashTable");
            }
            else
            {
                // Verificação da tabela
                var existing = new bool[256];
                for (var i = 0; i < 125; ++i)
                {
                    existing[i] = false;
                }

                if (table.Length != 256)
                {
                    throw new ArgumentException(string.Format("Invalid hash table length {0}. Expected 256.", table.Length));
                }
                else
                {
                    for (var i = 0; i < 256; ++i)
                    {
                        var current = table[i];
                        if (existing[current])
                        {
                            throw new ArgumentException("Table has repeated elements.");
                        }
                        else
                        {
                            existing[current] = true;
                        }
                    }
                }
            }
        }

        #endregion Construtores

        #region Funções públicas

        /// <summary>
        /// Obtém o código confuso de 8 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public byte GetHash8(string obj)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o código confuso de 8 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public byte GetHash8(byte[] obj)
        {
            return this.GetPearHash8(obj, 0, obj.Length);
        }

        /// <summary>
        /// Obtém o código confuso de 16 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public ushort GetHash16(string obj)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
            return this.GetPearHash16(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Obtém o código confuso de 16 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public ushort GetHash16(byte[] obj)
        {
            return this.GetPearHash16(obj, 0, obj.Length);
        }

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(string obj)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
            return this.GetPearHash32(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(byte[] obj)
        {
            return this.GetPearHash32(obj, 0, obj.Length);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(string obj)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
            return this.GetPearHash64(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Obtém o código confuso de  bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(byte[] obj)
        {
            return this.GetPearHash64(obj, 0, obj.Length);
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="bytes">O número de bytes no código confuso.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash(string obj, int bytes)
        {
            var array = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
            return this.GetPearHashN(array, 0, array.Length, bytes);
        }

        #endregion Funções públicas

        #region Funções privadas

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="bytes">O número de bytes no código confuso.</param>
        /// <returns>O código confuso.</returns>
        public BigInteger GetHash(byte[] obj, int bytes)
        {
            return this.GetPearHashN(obj, 0, obj.Length, bytes);
        }

        /// <summary>
        /// Implementa um algoritmo de código confuso de 8 bit.
        /// </summary>
        /// <param name="obj">A chave.</param>
        /// <param name="start">A posição na chave onde o algoritmo deve ser inicializado.</param>
        /// <param name="length">O tamanho da chave.</param>
        /// <returns>O valor do código confuso.</returns>
        private byte GetPearHash8(byte[] obj, int start, int length)
        {
            var pointer = start;
            var hash = (byte)(length % 256);
            for (var i = 0; i < length; ++i)
            {
                var current = obj[pointer];
                hash = this.hashTable[(hash + i) % 256];
                ++pointer;
            }

            return hash;
        }

        /// <summary>
        /// Implementa um algoritmo de código confuso de 16 bit.
        /// </summary>
        /// <param name="obj">A chave.</param>
        /// <param name="start">A posição na chave onde o algoritmo deve ser inicializado.</param>
        /// <param name="length">O tamanho da chave.</param>
        /// <returns>O valor do código confuso.</returns>
        private ushort GetPearHash16(byte[] obj, int start, int length)
        {
            if (obj == null || obj.Length == 0)
            {
                return 0;
            }
            else
            {
                var hh = new byte[2];
                for (var j = 0; j < 2; ++j)
                {
                    var pointer = start;
                    var h = this.hashTable[(obj[pointer] + j) % 256];
                    ++pointer;
                    for (var i = 1; i < length; ++i)
                    {
                        h = this.hashTable[obj[pointer++] ^ h];
                    }

                    hh[j] = h;
                }

                return (ushort)((((ushort)hh[1]) << 8) | (ushort)hh[0]);
            }
        }

        /// <summary>
        /// Implementa um algoritmo de código confuso de 32 bit.
        /// </summary>
        /// <param name="obj">A chave.</param>
        /// <param name="start">A posição na chave onde o algoritmo deve ser inicializado.</param>
        /// <param name="length">O tamanho da chave.</param>
        /// <returns>O valor do código confuso.</returns>
        private uint GetPearHash32(byte[] obj, int start, int length)
        {
            if (obj == null || obj.Length == 0)
            {
                return 0;
            }
            else
            {
                var hh = new byte[4];
                for (var j = 0; j < 4; ++j)
                {
                    var pointer = start;
                    var h = this.hashTable[(obj[pointer] + j) % 256];
                    ++pointer;
                    for (var i = 1; i < length; ++i)
                    {
                        h = this.hashTable[obj[pointer++] ^ h];
                    }

                    hh[j] = h;
                }

                return
                    (((uint)hh[3]) << 24) |
                    (((uint)hh[2]) << 16) |
                    (((uint)hh[1]) << 8) |
                    hh[0];
            }
        }

        /// <summary>
        /// Implementa um algoritmo de código confuso de 64 bit.
        /// </summary>
        /// <param name="obj">A chave.</param>
        /// <param name="start">A posição na chave onde o algoritmo deve ser inicializado.</param>
        /// <param name="length">O tamanho da chave.</param>
        /// <returns>O valor do código confuso.</returns>
        private ulong GetPearHash64(byte[] obj, int start, int length)
        {
            if (obj == null || obj.Length == 0)
            {
                return 0;
            }
            else
            {
                var hh = new byte[8];
                for (var j = 0; j < 8; ++j)
                {
                    var pointer = start;
                    var h = this.hashTable[(obj[pointer] + j) % 256];
                    ++pointer;
                    for (var i = 1; i < length; ++i)
                    {
                        h = this.hashTable[obj[pointer++] ^ h];
                    }

                    hh[j] = h;
                }

                return
                    (((ulong)hh[7]) << 56) |
                    (((ulong)hh[6]) << 48) |
                    (((ulong)hh[5]) << 40) |
                    (((ulong)hh[4]) << 32) |
                    (((ulong)hh[3]) << 24) |
                    (((ulong)hh[2]) << 16) |
                    (((ulong)hh[1]) << 8) |
                    hh[0];
            }
        }

        /// <summary>
        /// Implementa um algoritmo de código confuso de N bytes.
        /// </summary>
        /// <param name="obj">A chave.</param>
        /// <param name="start">A posição na chave onde o algoritmo deve ser inicializado.</param>
        /// <param name="length">O tamanho da chave.</param>
        /// <param name="bytes">O número de bytes.</param>
        /// <returns>O valor do código confuso.</returns>
        private BigInteger GetPearHashN(byte[] obj, int start, int length, int bytes)
        {
            if (obj == null || obj.Length == 0)
            {
                return 0;
            }
            else
            {
                var hh = new byte[bytes];
                for (var j = 0; j < bytes; ++j)
                {
                    var pointer = start;
                    var h = this.hashTable[(obj[pointer] + j) % 256];
                    ++pointer;
                    for (var i = 1; i < length; ++i)
                    {
                        h = this.hashTable[obj[pointer++] ^ h];
                    }

                    hh[j] = h;
                }

                return new BigInteger(hh);
            }
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa o algoritmo xxHash.
    /// </summary>
    /// <remarks>
    /// A implemetnação suporta o processo incremental.
    /// </remarks>
    public class XXHash :
        IHash32<byte[]>,
        IHash32<string>,
        IHash64<byte[]>,
        IHash64<string>,
        IHashN<byte[]>,
        IHashN<string>
    {
        #region Campos privados

        /// <summary>
        /// Um primo.
        /// </summary>
        private const uint prime32_1 = 2654435761U;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const uint prime32_2 = 2246822519U;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const uint prime32_3 = 3266489917U;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const uint prime32_4 = 668265263U;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const uint prime32_5 = 374761393U;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const ulong prime64_1 = 11400714785074694791UL;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const ulong prime64_2 = 14029467366897019727UL;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const ulong prime64_3 = 1609587929392839161UL;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const ulong prime64_4 = 9650029242287828579UL;

        /// <summary>
        /// Um primo.
        /// </summary>
        private const ulong prime64_5 = 2870177450012600261UL;

        #endregion Campos privados

        #region Funções públicas

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(string obj)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
            return this.GetXXHash32(bytes, 0, bytes.Length, 0U);
        }

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(byte[] obj)
        {
            return this.GetXXHash32(obj, 0, obj.Length, 0U);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(string obj)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(obj);
            return this.GetXXHash64(bytes, 0, bytes.Length, 0UL);
        }

        /// <summary>
        /// Obtém o código confuso de  bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64(byte[] obj)
        {
            return this.GetXXHash64(obj, 0, obj.Length, 0UL);
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
            else
            {
                throw new NotSupportedException("Number of bytes is not supported.");
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
            else
            {
                throw new NotSupportedException("Number of bytes is not supported.");
            }
        }

        #endregion Funções públicas

        #region Funções internas

        /// <summary>
        /// Obtém o código confuso de 32 bit para efeitos de teste.
        /// </summary>
        /// <param name="obj">O vector com a mensagem.</param>
        /// <param name="start">O índice do vector onde se inicia a mensagem.</param>
        /// <param name="length">O tamanho da mensagem.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        internal uint InternalGetXXHash32(
            byte[] obj,
            int start,
            int length,
            uint seed)
        {
            return this.GetXXHash32(obj, start, length, seed);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bit para efeitos de teste.
        /// </summary>
        /// <param name="obj">O vector com a mensagem.</param>
        /// <param name="start">O índice do vector onde se inicia a mensagem.</param>
        /// <param name="length">O tamanho da mensagem.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        internal ulong InternalGetXXHash64(
            byte[] obj,
            int start,
            int length,
            ulong seed)
        {
            return this.GetXXHash64(obj, start, length, seed);
        }

        #endregion Funções internas

        #region Funções privadas

        /// <summary>
        /// Obtém o código confuso de 32 bit.
        /// </summary>
        /// <param name="obj">O vector com a mensagem.</param>
        /// <param name="start">O índice do vector onde se inicia a mensagem.</param>
        /// <param name="length">O tamanho da mensagem.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private uint GetXXHash32(byte[] obj, int start, int length, uint seed)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var pointer = start;
                    var h32 = 0U;
                    if (length >= 16)
                    {
                        var limit = final - 16;
                        var v1 = seed + prime32_1 + prime32_2;
                        var v2 = seed + prime32_2;
                        var v3 = seed + 0;
                        var v4 = seed - prime32_1;

                        do
                        {
                            v1 = this.XXH32Round(v1, BitConverter.ToUInt32(obj, pointer));
                            pointer += 4;
                            v2 = this.XXH32Round(v2, BitConverter.ToUInt32(obj, pointer));
                            pointer += 4;
                            v3 = this.XXH32Round(v3, BitConverter.ToUInt32(obj, pointer));
                            pointer += 4;
                            v4 = this.XXH32Round(v4, BitConverter.ToUInt32(obj, pointer));
                            pointer += 4;
                        } while (pointer <= limit);

                        h32 = Utils.RotateLeft(v1, 1) +
                            Utils.RotateLeft(v2, 7) +
                            Utils.RotateLeft(v3, 12) +
                            Utils.RotateLeft(v4, 18);
                    }
                    else
                    {
                        h32 = seed + prime32_5;
                    }

                    h32 += (uint)length;

                    while (pointer + 4 <= final)
                    {
                        h32 += BitConverter.ToUInt32(obj, pointer) * prime32_3;
                        h32 = Utils.RotateLeft(h32, 17) * prime32_4;
                        pointer += 4;
                    }

                    while (pointer < final)
                    {
                        h32 += obj[pointer] * prime32_5;
                        h32 = Utils.RotateLeft(h32, 11) * prime32_1;
                        ++pointer;
                    }

                    h32 ^= h32 >> 15;
                    h32 *= prime32_2;
                    h32 ^= h32 >> 13;
                    h32 *= prime32_3;
                    h32 ^= h32 >> 16;

                    return h32;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bit.
        /// </summary>
        /// <param name="obj">O vector com a mensagem.</param>
        /// <param name="start">O índice do vector onde se inicia a mensagem.</param>
        /// <param name="length">O tamanho da mensagem.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private ulong GetXXHash64(byte[] obj, int start, int length, ulong seed)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var pointer = start;
                    var h64 = 0UL;
                    if (length >= 32)
                    {
                        var limit = final - 32;
                        var v1 = seed + prime64_1 + prime64_2;
                        var v2 = seed + prime64_2;
                        var v3 = seed + 0;
                        var v4 = seed - prime64_1;

                        do
                        {
                            v1 = this.XXH64Round(v1, BitConverter.ToUInt64(obj, pointer));
                            pointer += 8;
                            v2 = this.XXH64Round(v2, BitConverter.ToUInt64(obj, pointer));
                            pointer += 8;
                            v3 = this.XXH64Round(v3, BitConverter.ToUInt64(obj, pointer));
                            pointer += 8;
                            v4 = this.XXH64Round(v4, BitConverter.ToUInt64(obj, pointer));
                            pointer += 8;
                        } while (pointer <= limit);

                        h64 = Utils.RotateLeft(v1, 1) +
                            Utils.RotateLeft(v2, 7) +
                            Utils.RotateLeft(v3, 12) +
                            Utils.RotateLeft(v4, 18);
                        h64 = this.MergeRound64(h64, v1);
                        h64 = this.MergeRound64(h64, v2);
                        h64 = this.MergeRound64(h64, v3);
                        h64 = this.MergeRound64(h64, v4);

                    }
                    else
                    {
                        h64 = seed + prime64_5;
                    }

                    h64 += (ulong)length;

                    while (pointer + 8 <= final)
                    {
                        var k1 = this.XXH64Round(0, BitConverter.ToUInt64(obj, pointer));
                        h64 ^= k1;
                        h64 = Utils.RotateLeft(h64, 27) * prime64_1 + prime64_4;
                        pointer += 8;
                    }

                    if (pointer + 4 <= final)
                    {
                        h64 ^= (ulong)(BitConverter.ToUInt32(obj, pointer)) * prime64_1;
                        h64 = Utils.RotateLeft(h64, 23) * prime64_2 + prime64_3;
                        pointer += 4;
                    }

                    while (pointer < final)
                    {
                        h64 ^= obj[pointer] * prime64_5;
                        h64 = Utils.RotateLeft(h64, 11) * prime64_1;
                        ++pointer;
                    }

                    h64 ^= h64 >> 33;
                    h64 *= prime64_2;
                    h64 ^= h64 >> 29;
                    h64 *= prime64_3;
                    h64 ^= h64 >> 32;

                    return h64;
                }
            }
        }

        /// <summary>
        /// Aplica uma mistura aos argumentos.
        /// </summary>
        /// <param name="seed">O primeiro argumento.</param>
        /// <param name="input">O segundo argumento.</param>
        /// <returns>O valor da mistura.</returns>
        private uint XXH32Round(uint seed, uint input)
        {
            seed += input * prime32_2;
            seed = Utils.RotateLeft(seed, 13);
            seed *= prime32_1;
            return seed;
        }

        /// <summary>
        /// Aplica uma mistura aos argumentos.
        /// </summary>
        /// <param name="seed">O primeiro argumento.</param>
        /// <param name="input">O segundo argumento.</param>
        /// <returns>O valor da mistura.</returns>
        public ulong XXH64Round(ulong seed, ulong input)
        {
            seed += input * prime64_2;
            seed = Utils.RotateLeft(seed, 31);
            seed *= prime64_1;
            return seed;
        }

        /// <summary>
        /// Funde os valores dos argumentos.
        /// </summary>
        /// <param name="acc">O primeiro argumento.</param>
        /// <param name="val">O segundo argumento.</param>
        /// <returns>O resultado.</returns>
        public ulong MergeRound64(ulong acc, ulong val)
        {
            var innerVal = this.XXH64Round(0, val);
            var innerAcc = acc ^ innerVal;
            innerAcc = innerAcc * prime64_1 + prime64_4;
            return innerAcc;
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class KarRabHashByte32 :
        IRollingHash32<byte>,
        IHash32<byte[]>
    {
        /// <summary>
        /// O número máximo de bits suportado pelo código confuso.
        /// </summary>
        private const uint maxBits = 32;

        /// <summary>
        /// Número primo utilizado no processo.
        /// </summary>
        protected const uint prmNumb = 37;

        /// <summary>
        /// O número de elementos a suportar na sequência.
        /// </summary>
        protected uint seqNumber;

        /// <summary>
        /// O corrente de elementos.
        /// </summary>
        protected int count;

        /// <summary>
        /// O valor da máscara correspondente ao número de bits.
        /// </summary>
        protected uint hashMask;

        /// <summary>
        /// O valor actual do código confuso.
        /// </summary>
        protected uint hashValue;

        /// <summary>
        /// Variável de estado.
        /// </summary>
        protected uint bton;

        /// <summary>
        /// O conjunto de valores para os códigos confusos.
        /// </summary>
        private uint[] hashValues = new uint[256];

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="KarRabHashByte32"/>.
        /// </summary>
        /// <param name="seqNum">O número de elementos na sequência.</param>
        /// <param name="bits">O número de bits a ser considerado.</param>
        /// <exception cref="ArgumentException">
        /// Se o número de bits for superior ao máximo permitido pelo tipo do código confuso.
        /// </exception>
        public KarRabHashByte32(uint seqNum, uint bits)
        {
            if (bits > maxBits)
            {
                throw new ArgumentException(string.Format(
                    "The number of bits must no exceed {0}.",
                    maxBits));
            }
            else
            {
                this.count = 0;
                this.seqNumber = seqNum;
                this.hashMask = this.MaskFunc((int)bits);
                this.InitializeHashValues();
                this.bton = 1U;
                for (var i = 0; i < seqNum; ++i)
                {
                    this.bton *= prmNumb;
                    this.bton &= this.hashMask;
                }
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public uint HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                var result = 0U;
                var len = obj.Length;
                for (var i = 0; i < len; ++i)
                {
                    var x = 1U;
                    for (var j = 0U; j < len - i - 1; ++j)
                    {
                        x = (x * prmNumb) & this.hashMask;
                    }

                    x = (x * this.hashValues[obj[i]]) & this.hashMask;
                    result = (result + x) & this.hashMask;
                }

                return result;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            if (this.count == this.seqNumber)
            {
                throw new ArgumentException();
            }
            else
            {
                ++this.count;
                this.hashValue = (prmNumb * this.hashValue + this.hashValues[obj]) &
                    this.hashMask;
            }
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            this.hashValue = (prmNumb * this.hashValue + this.hashValues[inObj]
                - this.bton * this.hashValues[outObj]) & this.hashMask;
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen = new MTRand();
            for (var i = 0; i < 256; ++i)
            {
                this.hashValues[i] = randomGen.RandInt(this.hashMask);
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private uint MaskFunc(int bits)
        {
            var result = 1U << (bits - 1);
            return result ^ (result - 1);
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class KarRabHashString32 :
        IRollingHash32<char>,
        IHash32<string>
    {
        /// <summary>
        /// O número máximo de bits suportado pelo código confuso.
        /// </summary>
        private const uint maxBits = 32;

        /// <summary>
        /// Número primo utilizado no processo.
        /// </summary>
        private const uint prmNumb = 37;

        /// <summary>
        /// O número de elementos a suportar na sequência.
        /// </summary>
        private uint seqNumber;

        /// <summary>
        /// O corrente de elementos.
        /// </summary>
        private int count;

        /// <summary>
        /// O valor da máscara correspondente ao número de bits.
        /// </summary>
        private uint hashMask;

        /// <summary>
        /// O valor actual do código confuso.
        /// </summary>
        private uint hashValue;

        /// <summary>
        /// Variável de estado.
        /// </summary>
        private uint bton;

        /// <summary>
        /// O conjunto de valores para os códigos confusos.
        /// </summary>
        private uint[] hashValues = new uint[256];

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="KarRabHashString32"/>.
        /// </summary>
        /// <param name="seqNum">O número de elementos na sequência.</param>
        /// <param name="bits">O número de bits a ser considerado.</param>
        /// <exception cref="ArgumentException">
        /// Se o número de bits for superior ao máximo permitido pelo tipo do código confuso.
        /// </exception>
        public KarRabHashString32(uint seqNum, uint bits)
        {
            if (bits > maxBits)
            {
                throw new ArgumentException(string.Format(
                    "The number of bits must no exceed {0}.",
                    maxBits));
            }
            else
            {
                this.count = 0;
                this.seqNumber = seqNum;
                this.hashMask = this.MaskFunc((int)bits);
                this.InitializeHashValues();
                this.bton = 1U;
                for (var i = 0; i < seqNum; ++i)
                {
                    this.bton *= prmNumb;
                    this.bton &= this.hashMask;
                }
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public uint HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                var result = 0U;
                var len = obj.Length;
                for (var i = 0; i < len; ++i)
                {
                    var x = 1U;
                    for (var j = 0U; j < len - i - 1; ++j)
                    {
                        x = (x * prmNumb) & this.hashMask;
                    }

                    x = (x * this.hashValues[obj[i]]) & this.hashMask;
                    result = (result + x) & this.hashMask;
                }

                return result;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(char obj)
        {
            if (this.count == this.seqNumber)
            {
                throw new ArgumentException();
            }
            else
            {
                ++this.count;
                this.hashValue = (prmNumb * this.hashValue + this.hashValues[obj]) &
                    this.hashMask;
            }
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(char inObj, char outObj)
        {
            this.hashValue = (prmNumb * this.hashValue + this.hashValues[inObj]
                - this.bton * this.hashValues[outObj]) & this.hashMask;
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen = new MTRand();
            for (var i = 0; i < 256; ++i)
            {
                this.hashValues[i] = randomGen.RandInt(this.hashMask);
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private uint MaskFunc(int bits)
        {
            var result = 1U << (bits - 1);
            return result ^ (result - 1);
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class KarRabHashByte64 :
        IRollingHash64<byte>,
        IHash64<byte[]>
    {
        /// <summary>
        /// O número máximo de bits suportado pelo código confuso.
        /// </summary>
        private const ulong maxBits = 64;

        /// <summary>
        /// Número primo utilizado no processo.
        /// </summary>
        protected const ulong prmNumb = 37;

        /// <summary>
        /// O número de elementos a suportar na sequência.
        /// </summary>
        protected ulong seqNumber;

        /// <summary>
        /// O corrente de elementos.
        /// </summary>
        protected int count;

        /// <summary>
        /// O valor da máscara correspondente ao número de bits.
        /// </summary>
        protected ulong hashMask;

        /// <summary>
        /// O valor actual do código confuso.
        /// </summary>
        protected ulong hashValue;

        /// <summary>
        /// Variável de estado.
        /// </summary>
        protected ulong bton;

        /// <summary>
        /// O conjunto de valores para os códigos confusos.
        /// </summary>
        private ulong[] hashValues = new ulong[256];

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="KarRabHashByte64"/>.
        /// </summary>
        /// <param name="seqNum">O número de elementos na sequência.</param>
        /// <param name="bits">O número de bits a ser considerado.</param>
        /// <exception cref="ArgumentException">
        /// Se o número de bits for superior ao máximo permitido pelo tipo do código confuso.
        /// </exception>
        public KarRabHashByte64(ulong seqNum, ulong bits)
        {
            if (bits > maxBits)
            {
                throw new ArgumentException(string.Format(
                    "The number of bits must no exceed {0}.",
                    maxBits));
            }
            else
            {
                this.count = 0;
                this.seqNumber = seqNum;
                this.hashMask = this.MaskFunc((int)bits);
                this.InitializeHashValues();
                this.bton = 1UL;
                for (var i = 0UL; i < seqNum; ++i)
                {
                    this.bton *= prmNumb;
                    this.bton &= this.hashMask;
                }
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public ulong HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 64 bits.
        /// </summary>
        public ulong GetHash64(byte[] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                var result = 0UL;
                var len = obj.Length;
                for (var i = 0; i < len; ++i)
                {
                    var x = 1UL;
                    for (var j = 0; j < len - i - 1; ++j)
                    {
                        x = (x * prmNumb) & this.hashMask;
                    }

                    x = (x * this.hashValues[obj[i]]) & this.hashMask;
                    result = (result + x) & this.hashMask;
                }

                return result;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            if (this.count == (int)this.seqNumber)
            {
                throw new ArgumentException();
            }
            else
            {
                ++this.count;
                this.hashValue = (prmNumb * this.hashValue + this.hashValues[obj]) &
                    this.hashMask;
            }
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            this.hashValue = (prmNumb * this.hashValue + this.hashValues[inObj]
                - this.bton * this.hashValues[outObj]) & this.hashMask;
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen1 = new MTRand();
            var randValue1 = (uint)(this.hashMask >> 32);
            if (randValue1 == 0U)
            {
                for (var i = 0UL; i < 256; ++i)
                {
                    this.hashValues[i] = randomGen1.RandInt((uint)this.hashMask);
                }
            }
            else
            {
                var randomGen2 = new MTRand();
                for (var i = 0UL; i < 256; ++i)
                {
                    var temp = ((ulong)randomGen1.RandInt(randValue1) << 32) |
                        ((ulong)randomGen2.RandInt(0xFFFFFFFFU));

                    this.hashValues[i] = temp;
                }
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private ulong MaskFunc(int bits)
        {
            var result = 1UL << (bits - 1);
            return result ^ (result - 1);
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class KarRabHashString64 :
        IRollingHash64<char>,
        IHash64<string>
    {
        /// <summary>
        /// O número máximo de bits suportado pelo código confuso.
        /// </summary>
        private const ulong maxBits = 64;

        /// <summary>
        /// Número primo utilizado no processo.
        /// </summary>
        private const ulong prmNumb = 37;

        /// <summary>
        /// O número de elementos a suportar na sequência.
        /// </summary>
        private ulong seqNumber;

        /// <summary>
        /// O corrente de elementos.
        /// </summary>
        private int count;

        /// <summary>
        /// O valor da máscara correspondente ao número de bits.
        /// </summary>
        private ulong hashMask;

        /// <summary>
        /// O valor actual do código confuso.
        /// </summary>
        private ulong hashValue;

        /// <summary>
        /// Variável de estado.
        /// </summary>
        private ulong bton;

        /// <summary>
        /// O conjunto de valores para os códigos confusos.
        /// </summary>
        private ulong[] hashValues = new ulong[256];

        /// <summary>
        /// Instancia uma nova instância de objectos do tipos <see cref="KarRabHashString64"/>.
        /// </summary>
        /// <param name="seqNum">O número de elementos na sequência.</param>
        /// <param name="bits">O número de bits a ser considerado.</param>
        /// <exception cref="ArgumentException">
        /// Se o número de bits for superior ao máximo permitido pelo tipo do código confuso.
        /// </exception>
        public KarRabHashString64(ulong seqNum, ulong bits)
        {
            if (bits > maxBits)
            {
                throw new ArgumentException(string.Format(
                    "The number of bits must no exceed {0}.",
                    maxBits));
            }
            else
            {
                this.count = 0;
                this.seqNumber = seqNum;
                this.hashMask = this.MaskFunc((int)bits);
                this.InitializeHashValues();
                this.bton = 1UL;
                for (var i = 0UL; i < seqNum; ++i)
                {
                    this.bton *= prmNumb;
                    this.bton &= this.hashMask;
                }
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public ulong HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 64 bits.
        /// </summary>
        public ulong GetHash64(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                var result = 0UL;
                var len = obj.Length;
                for (var i = 0; i < len; ++i)
                {
                    var x = 1UL;
                    for (var j = 0; j < len - i - 1; ++j)
                    {
                        x = (x * prmNumb) & this.hashMask;
                    }

                    x = (x * this.hashValues[obj[i]]) & this.hashMask;
                    result = (result + x) & this.hashMask;
                }

                return result;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(char obj)
        {
            if (this.count == (int)this.seqNumber)
            {
                throw new ArgumentException();
            }
            else
            {
                ++this.count;
                this.hashValue = (prmNumb * this.hashValue + this.hashValues[obj]) &
                    this.hashMask;
            }
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(char inObj, char outObj)
        {
            this.hashValue = (prmNumb * this.hashValue + this.hashValues[inObj]
                - this.bton * this.hashValues[outObj]) & this.hashMask;
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen1 = new MTRand();
            var randValue1 = (uint)(this.hashMask >> 32);
            if (randValue1 == 0U)
            {
                for (var i = 0UL; i < 256; ++i)
                {
                    this.hashValues[i] = randomGen1.RandInt((uint)this.hashMask);
                }
            }
            else
            {
                var randomGen2 = new MTRand();
                for (var i = 0UL; i < 256; ++i)
                {
                    var temp = ((ulong)randomGen1.RandInt(randValue1) << 32) |
                        ((ulong)randomGen2.RandInt(0xFFFFFFFFU));

                    this.hashValues[i] = temp;
                }
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private ulong MaskFunc(int bits)
        {
            var result = 1UL << (bits - 1);
            return result ^ (result - 1);
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class GenRollHashByte32 :
        IRollingHash32<byte>,
        IHash32<byte[]>
    {
        /// <summary>
        /// Valor associado ao polinómio irredutível.
        /// </summary>
        private uint irreduciblePoly;

        /// <summary>
        /// O número de carácteres na sequência.
        /// </summary>
        private int seqNum;

        /// <summary>
        /// O número de bits no resultado.
        /// </summary>
        private int bits;

        /// <summary>
        /// Valor com o último bit atribuído.
        /// </summary>
        private uint lastBit;

        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private uint hashValue;

        /// <summary>
        /// Mantém o valor da máscara de bits.
        /// </summary>
        private uint hashMask;

        /// <summary>
        /// Mantém valores pré-computados.
        /// </summary>
        private uint[] precomputedShift;

        /// <summary>
        /// Mantém um vector com os valores dos códigos confusos.
        /// </summary>
        private uint[] hashValues = new uint[256];

        /// <summary>
        /// O tipo de pré-computação.
        /// </summary>
        PrecomputationType precomputationType;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenRollHashByte32"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="compType">O tipo de pré-computação.</param>
        public GenRollHashByte32(
            int seqNum,
            int bits = 19,
            PrecomputationType compType = PrecomputationType.NOPRECOMP)
        {
            if (bits == 19)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                        "The sequence number {0} exceeded the bits number, {1},",
                        seqNum,
                        bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 1) + (1U << 2) + (1U << 5)
                                   + (1U << 19);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);

                }
            }
            else if (bits == 9)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                           "The sequence number {0} exceeded the bits number, {1},",
                           seqNum,
                           bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 2) + (1U << 3) + (1U << 5) + (1U << 9);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);
                }
            }
            else
            {
                throw new ArgumentException("The hasher only supports 19 or 9 bits length.");
            }

        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public uint HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            this.hashValue = this.FastLeftShift(this.hashValue, 1);
            this.hashValue ^= this.hashValues[obj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            this.hashValue <<= 1;
            if ((this.hashValue & this.lastBit) == this.lastBit)
            {
                this.hashValue ^= this.irreduciblePoly;
            }

            var z = this.hashValues[outObj];
            if (this.precomputationType == PrecomputationType.FULLPRECOMP)
            {
                z = this.FastLeftShiftN(z);
            }
            else
            {
                z = this.FastLeftShift(z, (int)this.seqNum);
            }

            this.hashValue ^= z ^ this.hashValues[inObj];
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var result = 0U;
                var len = obj.Length;
                for (var k = 0U; k < len; ++k)
                {
                    result = this.FastLeftShift(result, 1);
                    result ^= this.hashValues[obj[k]];
                }

                return result;
            }
        }

        /// <summary>
        /// Inicializa os parâmetros do objecto.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="type">O tipo de pré-computação.</param>
        private void InitializeParameters(
            int seqNum,
            int bits,
            PrecomputationType type)
        {
            this.seqNum = seqNum;
            this.bits = bits;
            this.lastBit = 1U << bits;
            this.hashMask = this.MaskFunc(bits);
            this.InitializeHashValues();
            this.precomputationType = type;
            if (type == PrecomputationType.FULLPRECOMP)
            {
                var len = 1 << (int)seqNum;
                this.precomputedShift = new uint[len];

                var leftExp = bits - seqNum;
                if (leftExp == 0)
                {
                    for (var i = 0U; i < len; ++i)
                    {
                        var leftOver = this.FastLeftShift(
                            i,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
                else
                {
                    for (var i = 0U; i < len; ++i)
                    {
                        var leftOver = i << leftExp;
                        leftOver = this.FastLeftShift(
                            leftOver,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
            }
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen = new MTRand();
            for (var i = 0; i < 256; ++i)
            {
                this.hashValues[i] = randomGen.RandInt(this.hashMask);
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private uint MaskFunc(int bits)
        {
            var result = 1U << (bits - 1);
            return result ^ (result - 1);
        }

        /// <summary>
        /// Efectua o deslocamento à esquerda, misturando os valores
        /// com o valor do polinómio irredutível.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <param name="r">O número de deslocamentos.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private uint FastLeftShift(
            uint x,
            int r)
        {
            var result = x;
            for (var i = 0; i < r; ++i)
            {
                result <<= 1;
                if ((result & lastBit) == lastBit)
                {
                    result ^= this.irreduciblePoly;
                }
            }

            return result;
        }

        /// <summary>
        /// Efectua um deslocamento à esquerda.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private uint FastLeftShiftN(uint x)
        {
            var leftExp = this.bits - this.seqNum;
            var result = this.precomputedShift[(x >> leftExp)] ^
                ((x << (int)this.seqNum) & (this.lastBit - 1));
            return result;
        }

        /// <summary>
        /// Estabelece o tipo de pré-computação.
        /// </summary>
        public enum PrecomputationType
        {
            /// <summary>
            /// Pré-computação completa.
            /// </summary>
            FULLPRECOMP,

            /// <summary>
            /// Nenhhuma pré-computação.
            /// </summary>
            NOPRECOMP
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class GenRollHashString32 :
        IRollingHash32<char>,
        IHash32<string>
    {
        /// <summary>
        /// Valor associado ao polinómio irredutível.
        /// </summary>
        private uint irreduciblePoly;

        /// <summary>
        /// O número de carácteres na sequência.
        /// </summary>
        private int seqNum;

        /// <summary>
        /// O número de bits no resultado.
        /// </summary>
        private int bits;

        /// <summary>
        /// Valor com o último bit atribuído.
        /// </summary>
        private uint lastBit;

        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private uint hashValue;

        /// <summary>
        /// Mantém o valor da máscara de bits.
        /// </summary>
        private uint hashMask;

        /// <summary>
        /// Mantém valores pré-computados.
        /// </summary>
        private uint[] precomputedShift;

        /// <summary>
        /// Mantém um vector com os valores dos códigos confusos.
        /// </summary>
        private uint[] hashValues = new uint[256];

        /// <summary>
        /// O tipo de pré-computação.
        /// </summary>
        PrecomputationType precomputationType;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenRollHashString32"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="compType">O tipo de pré-computação.</param>
        public GenRollHashString32(
            int seqNum,
            int bits = 19,
            PrecomputationType compType = PrecomputationType.NOPRECOMP)
        {
            if (bits == 19)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                        "The sequence number {0} exceeded the bits number, {1},",
                        seqNum,
                        bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 1) + (1U << 2) + (1U << 5)
                                   + (1U << 19);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);

                }
            }
            else if (bits == 9)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                           "The sequence number {0} exceeded the bits number, {1},",
                           seqNum,
                           bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 2) + (1U << 3) + (1U << 5) + (1U << 9);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);
                }
            }
            else
            {
                throw new ArgumentException("The hasher only supports 19 or 9 bits length.");
            }

        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public uint HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(char obj)
        {
            this.hashValue = this.FastLeftShift(this.hashValue, 1);
            this.hashValue ^= this.hashValues[obj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(char inObj, char outObj)
        {
            this.hashValue <<= 1;
            if ((this.hashValue & this.lastBit) == this.lastBit)
            {
                this.hashValue ^= this.irreduciblePoly;
            }

            var z = this.hashValues[outObj];
            if (this.precomputationType == PrecomputationType.FULLPRECOMP)
            {
                z = this.FastLeftShiftN(z);
            }
            else
            {
                z = this.FastLeftShift(z, (int)this.seqNum);
            }

            this.hashValue ^= z ^ this.hashValues[inObj];
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(string obj)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var result = 0U;
                var len = obj.Length;
                for (var k = 0; k < len; ++k)
                {
                    result = this.FastLeftShift(result, 1);
                    result ^= this.hashValues[obj[k]];
                }

                return result;
            }
        }

        /// <summary>
        /// Inicializa os parâmetros do objecto.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="type">O tipo de pré-computação.</param>
        private void InitializeParameters(
            int seqNum,
            int bits,
            PrecomputationType type)
        {
            this.seqNum = seqNum;
            this.bits = bits;
            this.lastBit = 1U << bits;
            this.hashMask = this.MaskFunc(bits);
            this.InitializeHashValues();
            this.precomputationType = type;
            if (type == PrecomputationType.FULLPRECOMP)
            {
                var len = 1 << (int)seqNum;
                this.precomputedShift = new uint[len];

                var leftExp = bits - seqNum;
                if (leftExp == 0)
                {
                    for (var i = 0U; i < len; ++i)
                    {
                        var leftOver = this.FastLeftShift(
                            i,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
                else
                {
                    for (var i = 0U; i < len; ++i)
                    {
                        var leftOver = i << leftExp;
                        leftOver = this.FastLeftShift(
                            leftOver,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
            }
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen = new MTRand();
            for (var i = 0; i < 256; ++i)
            {
                this.hashValues[i] = randomGen.RandInt(this.hashMask);
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private uint MaskFunc(int bits)
        {
            var result = 1U << (bits - 1);
            return result ^ (result - 1);
        }

        /// <summary>
        /// Efectua o deslocamento à esquerda, misturando os valores
        /// com o valor do polinómio irredutível.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <param name="r">O número de deslocamentos.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private uint FastLeftShift(
            uint x,
            int r)
        {
            var result = x;
            for (var i = 0; i < r; ++i)
            {
                result <<= 1;
                if ((result & lastBit) == lastBit)
                {
                    result ^= this.irreduciblePoly;
                }
            }

            return result;
        }

        /// <summary>
        /// Efectua um deslocamento à esquerda.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private uint FastLeftShiftN(uint x)
        {
            var leftExp = this.bits - this.seqNum;
            var result = this.precomputedShift[(x >> leftExp)] ^
                ((x << (int)this.seqNum) & (this.lastBit - 1));
            return result;
        }

        /// <summary>
        /// Estabelece o tipo de pré-computação.
        /// </summary>
        public enum PrecomputationType
        {
            /// <summary>
            /// Pré-computação completa.
            /// </summary>
            FULLPRECOMP,

            /// <summary>
            /// Nenhhuma pré-computação.
            /// </summary>
            NOPRECOMP
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class GenRollHashByte64 :
        IRollingHash64<byte>,
        IHash64<byte[]>
    {
        /// <summary>
        /// Valor associado ao polinómio irredutível.
        /// </summary>
        private uint irreduciblePoly;

        /// <summary>
        /// O número de carácteres na sequência.
        /// </summary>
        private int seqNum;

        /// <summary>
        /// O número de bits no resultado.
        /// </summary>
        private int bits;

        /// <summary>
        /// Valor com o último bit atribuído.
        /// </summary>
        private ulong lastBit;

        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private ulong hashValue;

        /// <summary>
        /// Mantém o valor da máscara de bits.
        /// </summary>
        private ulong hashMask;

        /// <summary>
        /// Mantém valores pré-computados.
        /// </summary>
        private ulong[] precomputedShift;

        /// <summary>
        /// Mantém um vector com os valores dos códigos confusos.
        /// </summary>
        private ulong[] hashValues = new ulong[256];

        /// <summary>
        /// O tipo de pré-computação.
        /// </summary>
        PrecomputationType precomputationType;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenRollHashByte64"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="compType">O tipo de pré-computação.</param>
        public GenRollHashByte64(
            int seqNum,
            int bits = 19,
            PrecomputationType compType = PrecomputationType.NOPRECOMP)
        {
            if (bits == 19)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                        "The sequence number {0} exceeded the bits number, {1},",
                        seqNum,
                        bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 1) + (1U << 2) + (1U << 5)
                                   + (1U << 19);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);

                }
            }
            else if (bits == 9)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                           "The sequence number {0} exceeded the bits number, {1},",
                           seqNum,
                           bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 2) + (1U << 3) + (1U << 5) + (1U << 9);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);
                }
            }
            else
            {
                throw new ArgumentException("The hasher only supports 19 or 9 bits length.");
            }

        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public ulong HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            this.hashValue = this.FastLeftShift(this.hashValue, 1);
            this.hashValue ^= this.hashValues[obj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            this.hashValue <<= 1;
            if ((this.hashValue & this.lastBit) == this.lastBit)
            {
                this.hashValue ^= this.irreduciblePoly;
            }

            var z = this.hashValues[outObj];
            if (this.precomputationType == PrecomputationType.FULLPRECOMP)
            {
                z = this.FastLeftShiftN(z);
            }
            else
            {
                z = this.FastLeftShift(z, (int)this.seqNum);
            }

            this.hashValue ^= z ^ this.hashValues[inObj];
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public ulong GetHash64(byte[] obj)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var result = 0UL;
                var len = (ulong)obj.Length;
                for (var k = 0UL; k < len; ++k)
                {
                    result = this.FastLeftShift(result, 1);
                    result ^= this.hashValues[obj[k]];
                }

                return result;
            }
        }

        /// <summary>
        /// Inicializa os parâmetros do objecto.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="type">O tipo de pré-computação.</param>
        private void InitializeParameters(
            int seqNum,
            int bits,
            PrecomputationType type)
        {
            this.seqNum = seqNum;
            this.bits = bits;
            this.lastBit = 1U << bits;
            this.hashMask = this.MaskFunc(bits);
            this.InitializeHashValues();
            this.precomputationType = type;
            if (type == PrecomputationType.FULLPRECOMP)
            {
                var len = (ulong)(1 << (int)seqNum);
                this.precomputedShift = new ulong[len];

                var leftExp = bits - seqNum;
                if (leftExp == 0)
                {
                    for (var i = 0UL; i < len; ++i)
                    {
                        var leftOver = this.FastLeftShift(
                            i,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
                else
                {
                    for (var i = 0UL; i < len; ++i)
                    {
                        var leftOver = i << leftExp;
                        leftOver = this.FastLeftShift(
                            leftOver,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
            }
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen = new MTRand();
            for (var i = 0; i < 256; ++i)
            {
                this.hashValues[i] = randomGen.RandInt((uint)this.hashMask);
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private ulong MaskFunc(int bits)
        {
            var result = 1UL << (bits - 1);
            return result ^ (result - 1);
        }

        /// <summary>
        /// Efectua o deslocamento à esquerda, misturando os valores
        /// com o valor do polinómio irredutível.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <param name="r">O número de deslocamentos.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private ulong FastLeftShift(
            ulong x,
            int r)
        {
            var result = x;
            for (var i = 0; i < r; ++i)
            {
                result <<= 1;
                if ((result & lastBit) == lastBit)
                {
                    result ^= this.irreduciblePoly;
                }
            }

            return result;
        }

        /// <summary>
        /// Efectua um deslocamento à esquerda.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private ulong FastLeftShiftN(ulong x)
        {
            var leftExp = this.bits - this.seqNum;
            var result = this.precomputedShift[(x >> leftExp)] ^
                ((x << (int)this.seqNum) & (this.lastBit - 1));
            return result;
        }

        /// <summary>
        /// Estabelece o tipo de pré-computação.
        /// </summary>
        public enum PrecomputationType
        {
            /// <summary>
            /// Pré-computação completa.
            /// </summary>
            FULLPRECOMP,

            /// <summary>
            /// Nenhhuma pré-computação.
            /// </summary>
            NOPRECOMP
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class GenRollHashString64 :
        IRollingHash64<char>,
        IHash64<string>
    {
        /// <summary>
        /// Valor associado ao polinómio irredutível.
        /// </summary>
        private uint irreduciblePoly;

        /// <summary>
        /// O número de carácteres na sequência.
        /// </summary>
        private int seqNum;

        /// <summary>
        /// O número de bits no resultado.
        /// </summary>
        private int bits;

        /// <summary>
        /// Valor com o último bit atribuído.
        /// </summary>
        private ulong lastBit;

        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private ulong hashValue;

        /// <summary>
        /// Mantém o valor da máscara de bits.
        /// </summary>
        private ulong hashMask;

        /// <summary>
        /// Mantém valores pré-computados.
        /// </summary>
        private ulong[] precomputedShift;

        /// <summary>
        /// Mantém um vector com os valores dos códigos confusos.
        /// </summary>
        private ulong[] hashValues = new ulong[256];

        /// <summary>
        /// O tipo de pré-computação.
        /// </summary>
        PrecomputationType precomputationType;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="GenRollHashString64"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="compType">O tipo de pré-computação.</param>
        public GenRollHashString64(
            int seqNum,
            int bits = 19,
            PrecomputationType compType = PrecomputationType.NOPRECOMP)
        {
            if (bits == 19)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                        "The sequence number {0} exceeded the bits number, {1},",
                        seqNum,
                        bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 1) + (1U << 2) + (1U << 5)
                                   + (1U << 19);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);

                }
            }
            else if (bits == 9)
            {
                if (seqNum < 0 || seqNum > bits)
                {
                    throw new ArgumentException(string.Format(
                           "The sequence number {0} exceeded the bits number, {1},",
                           seqNum,
                           bits));
                }
                else
                {
                    this.irreduciblePoly = 1U + (1U << 2) + (1U << 3) + (1U << 5) + (1U << 9);
                    this.InitializeParameters(
                        seqNum,
                        bits,
                        compType);
                }
            }
            else
            {
                throw new ArgumentException("The hasher only supports 19 or 9 bits length.");
            }

        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public ulong HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(char obj)
        {
            this.hashValue = this.FastLeftShift(this.hashValue, 1);
            this.hashValue ^= this.hashValues[obj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(char inObj, char outObj)
        {
            this.hashValue <<= 1;
            if ((this.hashValue & this.lastBit) == this.lastBit)
            {
                this.hashValue ^= this.irreduciblePoly;
            }

            var z = this.hashValues[outObj];
            if (this.precomputationType == PrecomputationType.FULLPRECOMP)
            {
                z = this.FastLeftShiftN(z);
            }
            else
            {
                z = this.FastLeftShift(z, (int)this.seqNum);
            }

            this.hashValue ^= z ^ this.hashValues[inObj];
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 64 bits.
        /// </summary>
        public ulong GetHash64(string obj)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var result = 0UL;
                var len = obj.Length;
                for (var k = 0; k < len; ++k)
                {
                    result = this.FastLeftShift(result, 1);
                    result ^= this.hashValues[obj[k]];
                }

                return result;
            }
        }

        /// <summary>
        /// Inicializa os parâmetros do objecto.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits do resultado.</param>
        /// <param name="type">O tipo de pré-computação.</param>
        private void InitializeParameters(
            int seqNum,
            int bits,
            PrecomputationType type)
        {
            this.seqNum = seqNum;
            this.bits = bits;
            this.lastBit = 1U << bits;
            this.hashMask = this.MaskFunc(bits);
            this.InitializeHashValues();
            this.precomputationType = type;
            if (type == PrecomputationType.FULLPRECOMP)
            {
                var len = (ulong)(1 << (int)seqNum);
                this.precomputedShift = new ulong[len];

                var leftExp = bits - seqNum;
                if (leftExp == 0)
                {
                    for (var i = 0UL; i < len; ++i)
                    {
                        var leftOver = this.FastLeftShift(
                            i,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
                else
                {
                    for (var i = 0UL; i < len; ++i)
                    {
                        var leftOver = i << leftExp;
                        leftOver = this.FastLeftShift(
                            leftOver,
                            (int)seqNum);
                        this.precomputedShift[i] = leftOver;
                    }
                }
            }
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen = new MTRand();
            for (var i = 0; i < 256; ++i)
            {
                this.hashValues[i] = randomGen.RandInt((uint)this.hashMask);
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private ulong MaskFunc(int bits)
        {
            var result = 1UL << (bits - 1);
            return result ^ (result - 1);
        }

        /// <summary>
        /// Efectua o deslocamento à esquerda, misturando os valores
        /// com o valor do polinómio irredutível.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <param name="r">O número de deslocamentos.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private ulong FastLeftShift(
            ulong x,
            int r)
        {
            var result = x;
            for (var i = 0; i < r; ++i)
            {
                result <<= 1;
                if ((result & lastBit) == lastBit)
                {
                    result ^= this.irreduciblePoly;
                }
            }

            return result;
        }

        /// <summary>
        /// Efectua um deslocamento à esquerda.
        /// </summary>
        /// <param name="x">O valor a ser deslocado.</param>
        /// <returns>O resultado do deslocamento.</returns>
        private ulong FastLeftShiftN(ulong x)
        {
            var leftExp = this.bits - this.seqNum;
            var result = this.precomputedShift[(x >> leftExp)] ^
                ((x << (int)this.seqNum) & (this.lastBit - 1));
            return result;
        }

        /// <summary>
        /// Estabelece o tipo de pré-computação.
        /// </summary>
        public enum PrecomputationType
        {
            /// <summary>
            /// Pré-computação completa.
            /// </summary>
            FULLPRECOMP,

            /// <summary>
            /// Nenhhuma pré-computação.
            /// </summary>
            NOPRECOMP
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class CyclicHashByte32 :
        IRollingHash32<byte>,
        IHash32<byte[]>
    {
        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private uint hashValue = 0U;

        /// <summary>
        /// Máscara relativa ao número de bits decrescido 
        /// em uma unidade.
        /// </summary>
        private uint mask1;

        /// <summary>
        /// Máscara relativa ao número de bits.
        /// </summary>
        private uint maskn;

        /// <summary>
        /// Mantém o número de bits.
        /// </summary>
        private int bits;

        /// <summary>
        /// Mantém o resto da divisão do número de carácteres
        /// pelo número de bits.
        /// </summary>
        private int rem;

        /// <summary>
        /// Mantém códigos confusos para cada carácter.
        /// </summary>
        private uint[] hashValues = new uint[256];

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CyclicHashByte32"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequeência.</param>
        /// <param name="bits">O número de bits a ser considerado.</param>
        public CyclicHashByte32(int seqNum, int bits = 19)
        {
            if (seqNum > 0)
            {
                if (bits <= 0 || bits > 32)
                {
                    throw new ArgumentException("The number of bits must be a value between 1 and 32.");
                }
                else
                {
                    this.bits = bits;
                    this.rem = seqNum % bits;
                    this.mask1 = this.MaskFunc(bits - 1);
                    this.maskn = this.MaskFunc(bits - (int)this.rem);
                    this.InitializeHashValues();
                }
            }
            else
            {
                throw new ArgumentException("The number of characters in sequence must be a positive value.");
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public uint HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            this.hashValue = this.FastLeftShif1(this.hashValue);
            this.hashValue ^= this.hashValues[obj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            var z = this.hashValues[outObj];
            z = this.FastLeftShiftN(z);
            this.hashValue = this.FastLeftShif1(this.hashValue) ^
                z ^
                this.hashValues[inObj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso assumindo que o carácter
        /// de entrada é o primeiro e o de saída é o último.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void ReverseUpdate(byte inObj, byte outObj)
        {
            var z = this.hashValues[inObj];
            z = this.FastLeftShiftN(z);
            this.hashValue ^= z ^ this.hashValues[outObj];
            this.hashValue = this.FastRightShift1(this.hashValue);
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                var result = 0U;
                var len = obj.Length;
                for (var k = 0; k < len; ++k)
                {
                    result = this.FastLeftShif1(result);
                    result ^= this.hashValues[obj[k]];
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um código confuso que resulta do deslocamento do código
        /// confuso associado ao carácter proporcionado.
        /// </summary>
        /// <param name="outObj">O carácter.</param>
        /// <param name="n">O valor do deslocamento.</param>
        /// <returns>O código confuso calculado.</returns>
        public uint GetHashZ(char outObj, uint n)
        {
            var result = this.hashValues[outObj];
            for (var k = 0; k < n; ++k)
            {
                result = this.FastLeftShif1(result);
            }

            return result;
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen = new MTRand();
            for (var i = 0; i < 256; ++i)
            {
                this.hashValues[i] = randomGen.RandInt(this.maskn);
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private uint MaskFunc(int bits)
        {
            var result = 1U << (bits - 1);
            return result ^ (result - 1);
        }

        /// <summary>
        /// Roda os bits para a esquerda dentro da máscara um número de vezes
        /// igual ao resto da divisão.
        /// </summary>
        /// <param name="x">O valor a ser rodado.</param>
        /// <returns>O resultado da rotação.</returns>
        private uint FastLeftShiftN(uint x)
        {
            return ((x & this.maskn) << this.rem) | (x >> (this.bits - this.rem));
        }

        /// <summary>
        /// Roda os bits para a esquerda dentro da máscara em uma unidade.
        /// </summary>
        /// <param name="x">O valor a ser rodado.</param>
        /// <returns>O resultado da rotação.</returns>
        private uint FastLeftShif1(uint x)
        {
            return x = ((x & this.mask1) << 1) | (x >> (this.bits - 1));
        }

        /// <summary>
        /// Roda os bits para a direita dentro da máscara em uma unidade.
        /// </summary>
        /// <param name="x">O valor a ser rodado.</param>
        /// <returns>O resultado da rotação.</returns>
        private uint FastRightShift1(uint x)
        {
            return (x >> 1) | ((x & 1) << (this.bits - 1));
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class CyclicHashByte64 :
        IRollingHash64<byte>,
        IHash64<byte[]>
    {
        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private ulong hashValue = 0UL;

        /// <summary>
        /// Máscara relativa ao número de bits decrescido 
        /// em uma unidade.
        /// </summary>
        private ulong mask1;

        /// <summary>
        /// Máscara relativa ao número de bits.
        /// </summary>
        private ulong maskn;

        /// <summary>
        /// Mantém o número de bits.
        /// </summary>
        private int bits;

        /// <summary>
        /// Mantém o resto da divisão do número de carácteres
        /// pelo número de bits.
        /// </summary>
        private int rem;

        /// <summary>
        /// Mantém códigos confusos para cada carácter.
        /// </summary>
        private ulong[] hashValues = new ulong[256];

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CyclicHashByte64"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequeência.</param>
        /// <param name="bits">O número de bits a ser considerado.</param>
        public CyclicHashByte64(int seqNum, int bits = 19)
        {
            if (seqNum > 0)
            {
                if (bits <= 0 || bits > 64)
                {
                    throw new ArgumentException("The number of bits must be a value between 1 and 64.");
                }
                else
                {
                    this.bits = bits;
                    this.rem = seqNum % bits;
                    this.mask1 = this.MaskFunc(bits - 1);
                    this.maskn = this.MaskFunc(bits - (int)this.rem);
                    this.InitializeHashValues();
                }
            }
            else
            {
                throw new ArgumentException("The number of characters in sequence must be a positive value.");
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public ulong HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            this.hashValue = this.FastLeftShif1(this.hashValue);
            this.hashValue ^= this.hashValues[obj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            var z = this.hashValues[outObj];
            z = this.FastLeftShiftN(z);
            this.hashValue = this.FastLeftShif1(this.hashValue) ^
                z ^
                this.hashValues[inObj];
        }

        /// <summary>
        /// Actualiza o valor do código confuso assumindo que o carácter
        /// de entrada é o primeiro e o de saída é o último.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void ReverseUpdate(byte inObj, byte outObj)
        {
            var z = this.hashValues[inObj];
            z = this.FastLeftShiftN(z);
            this.hashValue ^= z ^ this.hashValues[outObj];
            this.hashValue = this.FastRightShift1(this.hashValue);
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 64 bits.
        /// </summary>
        public ulong GetHash64(byte[] obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            else
            {
                var result = 0UL;
                var len = obj.Length;
                for (var k = 0; k < len; ++k)
                {
                    result = this.FastLeftShif1(result);
                    result ^= this.hashValues[obj[k]];
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um código confuso que resulta do deslocamento do código
        /// confuso associado ao carácter proporcionado.
        /// </summary>
        /// <param name="outObj">O carácter.</param>
        /// <param name="n">O valor do deslocamento.</param>
        /// <returns>O código confuso calculado.</returns>
        public ulong GetHashZ(char outObj, uint n)
        {
            var result = this.hashValues[outObj];
            for (var k = 0; k < n; ++k)
            {
                result = this.FastLeftShif1(result);
            }

            return result;
        }

        /// <summary>
        /// Inicializa os valores do código confuso.
        /// </summary>
        private void InitializeHashValues()
        {
            var randomGen1 = new MTRand();
            var randValue1 = (uint)(this.maskn >> 32);
            if (randValue1 == 0U)
            {
                for (var i = 0UL; i < 256; ++i)
                {
                    this.hashValues[i] = randomGen1.RandInt((uint)this.maskn);
                }
            }
            else
            {
                var randomGen2 = new MTRand();
                for (var i = 0UL; i < 256; ++i)
                {
                    var temp = ((ulong)randomGen1.RandInt(randValue1) << 32) |
                        ((ulong)randomGen2.RandInt(0xFFFFFFFFU));

                    this.hashValues[i] = temp;
                }
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private ulong MaskFunc(int bits)
        {
            var result = 1UL << (bits - 1);
            return result ^ (result - 1);
        }

        /// <summary>
        /// Roda os bits para a esquerda dentro da máscara um número de vezes
        /// igual ao resto da divisão.
        /// </summary>
        /// <param name="x">O valor a ser rodado.</param>
        /// <returns>O resultado da rotação.</returns>
        private ulong FastLeftShiftN(ulong x)
        {
            return ((x & this.maskn) << this.rem) | (x >> (this.bits - this.rem));
        }

        /// <summary>
        /// Roda os bits para a esquerda dentro da máscara em uma unidade.
        /// </summary>
        /// <param name="x">O valor a ser rodado.</param>
        /// <returns>O resultado da rotação.</returns>
        private ulong FastLeftShif1(ulong x)
        {
            return x = ((x & this.mask1) << 1) | (x >> (this.bits - 1));
        }

        /// <summary>
        /// Roda os bits para a direita dentro da máscara em uma unidade.
        /// </summary>
        /// <param name="x">O valor a ser rodado.</param>
        /// <returns>O resultado da rotação.</returns>
        private ulong FastRightShift1(ulong x)
        {
            return (x >> 1) | ((x & 1) << (this.bits - 1));
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class ThreeWiseHash32 :
        IRollingHash32<byte>,
        IHash32<byte[]>
    {
        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private uint hashValue = 0U;

        /// <summary>
        /// Mantém uma matriz de códigos confusos.
        /// </summary>
        private uint[,] hashers;

        /// <summary>
        /// A máscara para código confuso.
        /// </summary>
        private uint maskn;

        /// <summary>
        /// Pilha de dupla entrada com os carácteres.
        /// </summary>
        private Deque<byte> ngram;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ThreeWiseHash32"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits no código confuso.</param>
        public ThreeWiseHash32(int seqNum, int bits = 19)
        {
            if (seqNum <= 0)
            {
                throw new ArgumentException("The number of characters in sequence must be a positive number.");
            }
            else if (bits <= 0 || bits > 32)
            {
                throw new ArgumentException("The number of bits must be a value between 1 and 32.");
            }
            else
            {
                this.ngram = new Deque<byte>(seqNum);
                this.InitializeHashes(seqNum, bits);
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public uint HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            this.ngram.EnqueueBack(obj);
            this.UpdateHashValue();
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            this.ngram.EnqueueBack(inObj);
            this.ngram.DequeueFront();
            this.UpdateHashValue();
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var result = 0U;
                var len = this.ngram.Capacity;
                for (var i = 0; i < len; ++i)
                {
                    result ^= this.hashers[i, obj[i]];
                }

                return result;
            }
        }

        /// <summary>
        /// Actualiza o valor corrente do código confuso.
        /// </summary>
        private void UpdateHashValue()
        {
            this.hashValue = 0U;
            var count = this.ngram.Count;
            for (var i = 0; i < count; ++i)
            {
                this.hashValue ^= this.hashers[i, this.ngram[i]];
            }
        }

        /// <summary>
        /// Inicializa os códigos confusos.
        /// </summary>
        /// <param name="seqNum">O número de carácters na sequência.</param>
        /// <param name="bits">O número de bits.</param>
        private void InitializeHashes(int seqNum, int bits)
        {
            this.maskn = this.MaskFunc(bits);
            this.hashers = new uint[seqNum, 256];
            var randomGen = new MTRand();
            for (var i = 0; i < seqNum; ++i)
            {
                for (var j = 0; j < 256; ++j)
                {
                    this.hashers[i, j] = randomGen.RandInt(this.maskn);
                }
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private uint MaskFunc(int bits)
        {
            var result = 1U << (bits - 1);
            return result ^ (result - 1);
        }
    }

    /// <summary>
    /// Implementa um código confuso de rotação.
    /// </summary>
    public class ThreeWiseHash64 :
        IRollingHash64<byte>,
        IHash64<byte[]>
    {
        /// <summary>
        /// Mantém o valor actual do código confuso.
        /// </summary>
        private ulong hashValue = 0UL;

        /// <summary>
        /// Mantém uma matriz de códigos confusos.
        /// </summary>
        private ulong[,] hashers;

        /// <summary>
        /// A máscara para código confuso.
        /// </summary>
        private ulong maskn;

        /// <summary>
        /// Pilha de dupla entrada com os carácteres.
        /// </summary>
        private Deque<byte> ngram;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ThreeWiseHash32"/>.
        /// </summary>
        /// <param name="seqNum">O número de carácteres na sequência.</param>
        /// <param name="bits">O número de bits no código confuso.</param>
        public ThreeWiseHash64(int seqNum, int bits = 19)
        {
            if (seqNum <= 0)
            {
                throw new ArgumentException("The number of characters in sequence must be a positive number.");
            }
            else if (bits <= 0 || bits > 64)
            {
                throw new ArgumentException("The number of bits must be a value between 1 and 32.");
            }
            else
            {
                this.ngram = new Deque<byte>(seqNum);
                this.InitializeHashes(seqNum, bits);
            }
        }

        /// <summary>
        /// Obtém o valor corrente do código confuso.
        /// </summary>
        public ulong HashValue
        {
            get
            {
                return this.hashValue;
            }
        }

        /// <summary>
        /// Adiciona ao objecto o código confuso.
        /// </summary>
        /// <param name="obj">O objecto a ser adicionado.</param>
        public void Eat(byte obj)
        {
            this.ngram.EnqueueBack(obj);
            this.UpdateHashValue();
        }

        /// <summary>
        /// Actualiza o valor do código confuso, indicando o objecto de entrada
        /// e o de saída.
        /// </summary>
        /// <param name="inObj">O objecto de entrada.</param>
        /// <param name="outObj">O objecto de saída.</param>
        public void Update(byte inObj, byte outObj)
        {
            this.ngram.EnqueueBack(inObj);
            this.ngram.DequeueFront();
            this.UpdateHashValue();
        }

        /// <summary>
        /// Define uma função de confusão de um objecto com 32 bits.
        /// </summary>
        public ulong GetHash64(byte[] obj)
        {
            if (obj == null)
            {
                return 0UL;
            }
            else
            {
                var result = 0UL;
                var len = this.ngram.Capacity;
                for (var i = 0; i < len; ++i)
                {
                    result ^= this.hashers[i, obj[i]];
                }

                return result;
            }
        }

        /// <summary>
        /// Actualiza o valor corrente do código confuso.
        /// </summary>
        private void UpdateHashValue()
        {
            this.hashValue = 0U;
            var count = this.ngram.Count;
            for (var i = 0; i < count; ++i)
            {
                this.hashValue ^= this.hashers[i, this.ngram[i]];
            }
        }

        /// <summary>
        /// Inicializa os códigos confusos.
        /// </summary>
        /// <param name="seqNum">O número de carácters na sequência.</param>
        /// <param name="bits">O número de bits.</param>
        private void InitializeHashes(int seqNum, int bits)
        {
            this.maskn = this.MaskFunc(bits);
            this.hashers = new ulong[seqNum, 256];
            var randomGen1 = new MTRand();
            var randValue1 = (uint)(this.maskn >> 32);
            if (randValue1 == 0U)
            {
                for (var i = 0; i < seqNum; ++i)
                {
                    for (var j = 0; i < 256; ++j)
                    {
                        this.hashers[i, j] = randomGen1.RandInt((uint)this.maskn);
                    }
                }
            }
            else
            {
                var randomGen2 = new MTRand();
                for (var i = 0; i < seqNum; ++i)
                {
                    for (var j = 0; j < 256; ++j)
                    {
                        var temp = ((ulong)randomGen1.RandInt(randValue1) << 32) |
                        ((ulong)randomGen2.RandInt(0xFFFFFFFFU));
                        this.hashers[i, j] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém uma máscara para o número de bits.
        /// </summary>
        /// <param name="bits">O número de bits.</param>
        /// <returns>A máscara.</returns>
        private ulong MaskFunc(int bits)
        {
            var result = 1UL << (bits - 1);
            return result ^ (result - 1);
        }
    }

    /// <summary>
    /// Implementa o algoritmo Farm Hash.
    /// </summary>
    public class FarmHash :
        IHash32<byte[]>,
        IHash64<byte[]>,
        IHashN<byte[]>
    {
        #region Campos privados

        /// <summary>
        /// Número primo compreendido entre 2^63 e 2^64
        /// </summary>
        private const ulong k0 = 0xc3a5c85c97cb3127UL;

        /// <summary>
        /// Número primo compreendido entre 2^63 e 2^64
        /// </summary>
        private const ulong k1 = 0xb492b66fbe98f273UL;

        /// <summary>
        /// Número primo compreendido entre 2^63 e 2^64
        /// </summary>
        private const ulong k2 = 0x9ae16a3b2f90404fUL;

        /// <summary>
        /// O primeiro parâmetro constante do algoritmo.
        /// </summary>
        private const uint c1 = 0xcc9e2d51;

        /// <summary>
        /// O segundo parâmetro constante do algoritmo.
        /// </summary>
        private const uint c2 = 0x1b873593;

        #endregion Campos privados

        #region Funções públicas

        /// <summary>
        /// Obtém o código confuso de 32 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>O código confuso.</returns>
        public uint GetHash32(byte[] obj)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                return this.Hash32(obj, 0, (uint)obj.Length);
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
                return 0UL;
            }
            else
            {
                return this.Hash64(obj, 0, (ulong)obj.LongLength);
            }
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto com valores iniciais.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="seed0">O primeiro valor inicial.</param>
        /// <param name="seed1">O segundo valor inicial.</param>
        /// <returns>O código confuso.</returns>
        public ulong GetHash64WithSeeds(
            byte[] s,
            ulong seed0,
            ulong seed1)
        {
            if (s == null)
            {
                return 0;
            }
            else
            {
                return this.Hash64WithSeeds(
                    s,
                    0,
                    (ulong)s.Length,
                    seed0,
                    seed1);
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
            else
            {
                throw new NotSupportedException("The number of bytes is not supported.");
            }
        }

        #endregion Funções públicas

        #region Funções internas

        /// <summary>
        /// Obtém o código confuso de 32 bit.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se pretende determinar o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        internal uint InternalHash32(byte[] s, int index, uint len)
        {
            return this.Hash32(s, index, len);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bit.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se pretende determinar o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        internal ulong InternalHash64(byte[] s, int index, ulong len)
        {
            return this.Hash64(s, index, len);
        }

        #endregion Funções internas

        #region Funções privadas

        /// <summary>
        /// Aplica uma permutação cíclica às variáveis.
        /// </summary>
        /// <param name="a">A primeira variável.</param>
        /// <param name="b">A segunda variável.</param>
        /// <param name="c">A terceira variável.</param>
        private void Permute3(
            ref uint a,
            ref uint b,
            ref uint c)
        {
            var t = a;
            a = b;
            b = c;
            c = t;
        }

        /// <summary>
        /// Efectua a mistura dos bits da variável.
        /// </summary>
        /// <param name="h">A variável da qual se pretende obter a mistura.</param>
        /// <returns>O resultado da mistura.</returns>
        private uint fmix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
            return h;
        }

        /// <summary>
        /// Aplica uma função de combinação entre duas variáveis.
        /// </summary>
        /// <param name="a">A primeira variável.</param>
        /// <param name="h">A segunda variável.</param>
        /// <returns>O resultado da combinação.</returns>
        private uint Mur(uint a, uint h)
        {
            a *= c1;
            a = Utils.RotateRight(a, 17);
            a *= c2;
            h ^= a;
            h = Utils.RotateRight(h, 19);
            return h * 5 + 0xe6546b64;
        }

        /// <summary>
        /// Determina o código confuso para objectos com comprimento compreendido
        /// entre 13 e 24.
        /// </summary>
        /// <param name="s">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice a partir do qual se pretende determinar o código confuso.</param>
        /// <param name="len">O comprimento da parte do objecto.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private uint Hash32Len13to24(byte[] s, int index, uint len, uint seed = 0)
        {
            var a = BitConverter.ToUInt32(s, index - 4 + ((int)len >> 1));
            var b = BitConverter.ToUInt32(s, index + 4);
            var c = BitConverter.ToUInt32(s, index + (int)len - 8);
            var d = BitConverter.ToUInt32(s, index + ((int)len >> 1));
            var e = BitConverter.ToUInt32(s, index);
            var f = BitConverter.ToUInt32(s, index + (int)len - 4);
            var h = d * c1 + len + seed;
            a = Utils.RotateRight(a, 12) + f;
            h = Mur(c, h) + a;
            a = Utils.RotateRight(a, 3) + c;
            h = this.Mur(e, h) + a;
            a = Utils.RotateRight(a + f, 12) + d;
            h = this.Mur(b ^ seed, h) + a;
            return this.fmix(h);
        }

        /// <summary>
        /// Determina o código confuso para objectos com comprimento compreendido
        /// entre 0 e 4.
        /// </summary>
        /// <param name="s">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice a partir do qual se pretende determinar o código confuso.</param>
        /// <param name="len">O comprimento da parte do objecto.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private uint Hash32Len0to4(byte[] s, int index, uint len, uint seed = 0U)
        {
            uint b = seed;
            uint c = 9;
            for (var i = index; i < len; i++)
            {
                var v = s[i];
                b = b * c1 + v;
                c ^= b;
            }

            return this.fmix(this.Mur(b, this.Mur(len, c)));
        }

        /// <summary>
        /// Determina o código confuso para objectos com comprimentos compreendidos
        /// entre 5 e 12.
        /// </summary>
        /// <param name="s">O obejcto do qual se pretende determianr o código confuso.</param>
        /// <param name="index">O índice a partir do qual se pretende determianr o código confuso.</param>
        /// <param name="len">O comprimento da parte do objecto.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private uint Hash32Len5to12(byte[] s, int index, uint len, uint seed = 0U)
        {
            uint a = len, b = len * 5, c = 9, d = b + seed;
            a += BitConverter.ToUInt32(s, index);
            b += BitConverter.ToUInt32(s, index + (int)len - 4);
            c += BitConverter.ToUInt32(s, index + (((int)len >> 1) & 4));
            return this.fmix(seed ^ this.Mur(c, this.Mur(b, this.Mur(a, d))));
        }

        /// <summary>
        /// Obtém o código de 32 bit do objecto.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se pretende obter o código confuso.</param>
        /// <param name="len">O comprimento da parte do objecto.</param>
        /// <returns>O valor do código confuso.</returns>
        private uint Hash32(byte[] s, int index, uint len)
        {
            if (len <= 24)
            {
                return len <= 12 ?
                    (len <= 4 ? Hash32Len0to4(s, index, len) : Hash32Len5to12(s, index, len)) :
                    Hash32Len13to24(s, index, len);
            }
            else
            {
                var p = index;
                uint h = len, g = c1 * len, f = g;
                uint a0 = Utils.RotateRight(
                    BitConverter.ToUInt32(s, p + (int)len - 4) * c1, 17) * c2;
                uint a1 = Utils.RotateRight(BitConverter.ToUInt32(s, p + (int)len - 8) * c1, 17) * c2;
                uint a2 = Utils.RotateRight(BitConverter.ToUInt32(s, p + (int)len - 16) * c1, 17) * c2;
                uint a3 = Utils.RotateRight(BitConverter.ToUInt32(s, p + (int)len - 12) * c1, 17) * c2;
                uint a4 = Utils.RotateRight(BitConverter.ToUInt32(s, p + (int)len - 20) * c1, 17) * c2;
                h ^= a0;
                h = Utils.RotateRight(h, 19);
                h = h * 5 + 0xe6546b64;
                h ^= a2;
                h = Utils.RotateRight(h, 19);
                h = h * 5 + 0xe6546b64;
                g ^= a1;
                g = Utils.RotateRight(g, 19);
                g = g * 5 + 0xe6546b64;
                g ^= a3;
                g = Utils.RotateRight(g, 19);
                g = g * 5 + 0xe6546b64;
                f += a4;
                f = Utils.RotateRight(f, 19) + 113;
                var iters = (len - 1) / 20;
                do
                {
                    var a = BitConverter.ToUInt32(s, p);
                    var b = BitConverter.ToUInt32(s, p + 4);
                    var c = BitConverter.ToUInt32(s, p + 8);
                    var d = BitConverter.ToUInt32(s, p + 12);
                    var e = BitConverter.ToUInt32(s, p + 16);
                    h += a;
                    g += b;
                    f += c;
                    h = Mur(d, h) + e;
                    g = Mur(c, g) + a;
                    f = Mur(b + e * c1, f) + d;
                    f += g;
                    g += f;
                    p += 20;
                } while (--iters != 0);
                g = Utils.RotateRight(g, 11) * c1;
                g = Utils.RotateRight(g, 17) * c1;
                f = Utils.RotateRight(f, 11) * c1;
                f = Utils.RotateRight(f, 17) * c1;
                h = Utils.RotateRight(h + g, 19);
                h = h * 5 + 0xe6546b64;
                h = Utils.RotateRight(h, 17) * c1;
                h = Utils.RotateRight(h + f, 19);
                h = h * 5 + 0xe6546b64;
                h = Utils.RotateRight(h, 17) * c1;
                return h;
            }
        }

        /// <summary>
        /// Efectua um deslocamento e mistura sobre um valor.
        /// </summary>
        /// <param name="val">O valor.</param>
        /// <returns>O resultado do procedimento.</returns>
        private ulong ShiftMix(ulong val)
        {
            return val ^ (val >> 47);
        }

        /// <summary>
        /// Obtém o código confuso de um inteiro de 128 bit.
        /// </summary>
        /// <param name="low">O valor baixo do número.</param>
        /// <param name="high">O valor alto do número.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash128to64(ulong low, ulong high)
        {
            var kMul = 0x9ddfea08eb382d69UL;
            var a = (low ^ high) * kMul;
            a ^= (a >> 47);
            var b = (high ^ a) * kMul;
            b ^= (b >> 47);
            b *= kMul;
            return b;
        }

        /// <summary>
        /// Obtém o código confuso de um inteiro de 128 bit.
        /// </summary>
        /// <param name="u">O valor baixo do número.</param>
        /// <param name="v">O valor alto do número.</param>
        /// <param name="mul">Um valor multiplicativo.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen16NA(ulong u, ulong v, ulong mul)
        {
            var a = (u ^ v) * mul;
            a ^= (a >> 47);
            var b = (v ^ a) * mul;
            b ^= (b >> 47);
            b *= mul;
            return b;
        }

        /// <summary>
        /// Obtém o código confuso de um inteiro de 128 bit.
        /// </summary>
        /// <param name="u">O valor baixo do número.</param>
        /// <param name="v">O valor alto do número.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen16NA(ulong u, ulong v)
        {
            return this.Hash128to64(u, v);
        }

        /// <summary>
        /// Determina o código confuso de um objecto com comprimento compreendido
        /// entre 0 e 16.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se pretende calcular o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen0to16NA(byte[] s, int index, ulong len)
        {
            if (len >= 8)
            {
                var mul = k2 + len * 2;
                var a = BitConverter.ToUInt64(s, index) + k2;
                var b = BitConverter.ToUInt64(s, index + (int)len - 8);
                var c = Utils.RotateRight(b, 37) * mul + a;
                var d = (Utils.RotateRight(a, 25) + b) * mul;
                return this.HashLen16NA(c, d, mul);
            }
            if (len >= 4)
            {
                var mul = k2 + len * 2;
                var a = BitConverter.ToUInt32(s, index);
                return this.HashLen16NA(len + (a << 3), BitConverter.ToUInt32(s, index + (int)len - 4), mul);
            }
            if (len > 0)
            {
                var a = s[0];
                var b = s[len >> 1];
                var c = s[len - 1];
                var y = (uint)a + ((uint)b << 8);
                var z = len + ((uint)c << 2);
                return this.ShiftMix(y * k2 ^ z * k0) * k2;
            }
            return k2;
        }

        /// <summary>
        /// Determina o código confuso de um objecto com comprimento compreendido
        /// entre 17 e 32.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se pretende calcular o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen17to32NA(byte[] s, int index, ulong len)
        {
            var mul = k2 + len * 2;
            var a = BitConverter.ToUInt64(s, index) * k1;
            var b = BitConverter.ToUInt64(s, index + 8);
            var c = BitConverter.ToUInt64(s, index + (int)len - 8) * mul;
            var d = BitConverter.ToUInt64(s, index + (int)len - 16) * k2;
            return this.HashLen16NA(Utils.RotateRight(a + b, 43) + Utils.RotateRight(c, 30) + d,
                             a + Utils.RotateRight(b + k2, 18) + c, mul);
        }

        /// <summary>
        /// Determina o código confuso de um objecto com comprimento compreendido
        /// entre 33 e 64.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se pretende calcular o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen33to64NA(byte[] s, int index, ulong len)
        {
            var mul = k2 + len * 2;
            var a = BitConverter.ToUInt64(s, index) * k2;
            var b = BitConverter.ToUInt64(s, index + 8);
            var c = BitConverter.ToUInt64(s, index + (int)len - 8) * mul;
            var d = BitConverter.ToUInt64(s, index + (int)len - 16) * k2;
            var y = Utils.RotateRight(a + b, 43) + Utils.RotateRight(c, 30) + d;
            var z = this.HashLen16NA(y, a + Utils.RotateRight(b + k2, 18) + c, mul);
            var e = BitConverter.ToUInt64(s, index + 16) * mul;
            var f = BitConverter.ToUInt64(s, index + 24);
            var g = (y + BitConverter.ToUInt64(s, index + (int)len - 32)) * mul;
            var h = (z + BitConverter.ToUInt64(s, index + (int)len - 24)) * mul;
            return this.HashLen16NA(Utils.RotateRight(e + f, 43) + Utils.RotateRight(g, 30) + h,
                             e + Utils.RotateRight(f + a, 18) + g, mul);
        }

        /// <summary>
        /// Determina uma forma de código confuso a partir de valores iniciais.
        /// </summary>
        /// <param name="w">Valor inicial.</param>
        /// <param name="x">Valor inicial.</param>
        /// <param name="y">Valor inicial.</param>
        /// <param name="z">Valor inicial.</param>
        /// <param name="a">Valor inicial.</param>
        /// <param name="b">Valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private MutableTuple<ulong, ulong> WeakHashLen32WithSeeds(
            ulong w,
            ulong x,
            ulong y,
            ulong z,
            ulong a,
            ulong b)
        {
            a += w;
            b = Utils.RotateRight(b + a + z, 21);
            var c = a;
            a += x;
            a += y;
            b += Utils.RotateRight(a, 44);
            return MutableTuple.Create(a + z, b + c);
        }

        /// <summary>
        /// Determina uma forma de código confuso dados valores iniciais.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice do objecto a partir do qual se calcula o código confuso.</param>
        /// <param name="a">O primeiro valor inicial.</param>
        /// <param name="b">O segundo valor inicial.</param>
        /// <returns>O resultado do código confuso.</returns>
        private MutableTuple<ulong, ulong> WeakHashLen32WithSeeds(
            byte[] s,
            int index,
            ulong a,
            ulong b)
        {
            return this.WeakHashLen32WithSeeds(BitConverter.ToUInt64(s, index),
                                          BitConverter.ToUInt64(s, index + 8),
                                          BitConverter.ToUInt64(s, index + 16),
                                          BitConverter.ToUInt64(s, index + 24),
                                          a,
                                          b);
        }

        /// <summary>
        /// Determina o código confuso de 64 bit.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se calcula o código confuso.</param>
        /// <param name="len">O comprimento a ser considerado no código confuso.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash64NA(byte[] s, int index, ulong len)
        {
            var seed = 81UL;
            if (len <= 32)
            {
                if (len <= 16)
                {
                    return this.HashLen0to16NA(s, index, len);
                }
                else
                {
                    return this.HashLen17to32NA(s, index, len);
                }
            }
            else if (len <= 64)
            {
                return this.HashLen33to64NA(s, index, len);
            }
            else
            {
                var x = seed;
                var y = seed * k1 + 113;
                var z = this.ShiftMix(y * k2 + 113) * k2;
                var v = MutableTuple.Create(0UL, 0UL);
                var w = MutableTuple.Create(0UL, 0UL);
                x = x * k2 + BitConverter.ToUInt64(s, index);

                var end = index + (((int)len - 1) / 64) * 64;
                var last64 = end + (((int)len - 1) & 63) - 63;
                System.Diagnostics.Debug.Assert(index + (int)len - 64 == last64);

                var p = index;
                do
                {
                    x = Utils.RotateRight(x + y + v.Item1 + BitConverter.ToUInt64(s, p + 8), 37) * k1;
                    y = Utils.RotateRight(y + v.Item2 + BitConverter.ToUInt64(s, p + 48), 42) * k1;
                    x ^= w.Item2;
                    y += v.Item1 + BitConverter.ToUInt64(s, p + 40);
                    z = Utils.RotateRight(z + w.Item1, 33) * k1;
                    v = this.WeakHashLen32WithSeeds(s, p, v.Item2 * k1, x + w.Item1);
                    w = this.WeakHashLen32WithSeeds(s, p + 32, z + w.Item2, y + BitConverter.ToUInt64(s, p + 16));

                    var t = z;
                    z = x;
                    x = t;

                    p += 64;
                } while (p != end);

                var mul = k1 + ((z & 0xff) << 1);
                p = last64;
                w.Item1 += ((len - 1) & 63);
                v.Item1 += w.Item1;
                w.Item1 += v.Item1;
                x = Utils.RotateRight(x + y + v.Item1 + BitConverter.ToUInt64(s, p + 8), 37) * mul;
                y = Utils.RotateRight(y + v.Item2 + BitConverter.ToUInt64(s, p + 48), 42) * mul;
                x ^= w.Item2 * 9;
                y += v.Item1 * 9 + BitConverter.ToUInt64(s, p + 40);
                z = Utils.RotateRight(z + w.Item1, 33) * mul;
                v = this.WeakHashLen32WithSeeds(s, p, v.Item2 * mul, x + w.Item1);
                w = this.WeakHashLen32WithSeeds(s, p + 32, z + w.Item2, y + BitConverter.ToUInt64(s, p + 16));
                var temp = z;
                z = x;
                x = temp;
                return this.HashLen16NA(this.HashLen16NA(v.Item1, w.Item1, mul) + this.ShiftMix(y) * k0 + z,
                                 this.HashLen16NA(v.Item2, w.Item2, mul) + x,
                                 mul);
            }
        }

        /// <summary>
        /// Determina o código confuso de 64 bit com valores iniciais.
        /// </summary>
        /// <param name="s">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice a partir do qual é calculado o código confuso.</param>
        /// <param name="len">O comprimento a ser considerado para o código confuso.</param>
        /// <param name="seed0">O primeiro valor inicial.</param>
        /// <param name="seed1">O segundo valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash64WithSeedsNA(
            byte[] s,
            int index,
            ulong len,
            ulong seed0,
            ulong seed1)
        {
            return this.HashLen16NA(this.Hash64NA(s, index, len) - seed0, seed1);
        }

        /// <summary>
        /// Determina o código confuso de 64 bit com valor inicial.
        /// </summary>
        /// <param name="s">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice a partir do qual é calculado o código confuso.</param>
        /// <param name="len">O comprimento a ser considerado para o código confuso.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash64WithSeedNA(byte[] s, int index, ulong len, ulong seed)
        {
            return this.Hash64WithSeedsNA(s, index, len, k2, seed);
        }

        /// <summary>
        /// Função auxiliar na determinação do código confuso de 64 bit.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual a função é aplicada.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <param name="mul">Um factor multiplicativo.</param>
        /// <param name="seed0">Valor inicial.</param>
        /// <param name="seed1">Valor inicial.</param>
        /// <returns>O resultado.</returns>
        private ulong H32(
            byte[] s,
            int index,
            ulong len,
            ulong mul,
            ulong seed0 = 0UL,
            ulong seed1 = 0UL)
        {
            var a = BitConverter.ToUInt64(s, index) * k1;
            var b = BitConverter.ToUInt64(s, index + 8);
            var c = BitConverter.ToUInt64(s, index + (int)len - 8) * mul;
            var d = BitConverter.ToUInt64(s, index + (int)len - 16) * k2;
            var u = Utils.RotateRight(a + b, 43) + Utils.RotateRight(c, 30) + d + seed0;
            var v = a + Utils.RotateRight(b + k2, 18) + c + seed1;
            a = this.ShiftMix((u ^ v) * mul);
            b = this.ShiftMix((v ^ a) * mul);
            return b;
        }

        /// <summary>
        /// Determina o código confuso de objectos com comprimento compreendido
        /// entre 33 e 64.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual é calculado o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen33to64(byte[] s, int index, ulong len)
        {
            var mul0 = k2 - 30;
            var mul1 = k2 - 30 + 2 * len;
            var h0 = this.H32(s, index, 32, mul0);
            var h1 = this.H32(s, index + (int)len - 32, 32, mul1);
            return ((h1 * mul1) + h0) * mul1;
        }

        /// <summary>
        /// Determina o código confuso de objectos com comprimento compreendido
        /// entre 65 e 96.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual é calculado o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong HashLen65to96(byte[] s, int index, ulong len)
        {
            var mul0 = k2 - 114;
            var mul1 = k2 - 114 + 2 * len;
            var h0 = this.H32(s, index, 32, mul0);
            var h1 = this.H32(s, index + 32, 32, mul1);
            var h2 = this.H32(s, index + (int)len - 32, 32, mul1, h0, h1);
            return (h2 * 9 + (h0 >> 17) + (h1 >> 21)) * mul1;
        }

        /// <summary>
        /// Função auxiliar de mistura para o cálculo do código confuso.
        /// </summary>
        /// <param name="x">O primeiro valor a ser misturado.</param>
        /// <param name="y">O segundo valor a ser misturado.</param>
        /// <param name="mul">Um factor multiplicativo.</param>
        /// <param name="r">O parâmetro de rotação.</param>
        /// <returns>O resultado.</returns>
        private ulong AuxH(ulong x, ulong y, ulong mul, int r)
        {
            var a = (x ^ y) * mul;
            a ^= (a >> 47);
            var b = (y ^ a) * mul;
            return Utils.RotateRight(b, r) * mul;
        }

        /// <summary>
        /// Determina o código confuso de 64 bit com valores iniciais.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual se calcula o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <param name="seed0">O primeiro valor inicial.</param>
        /// <param name="seed1">O segundo valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash64WithSeeds(
            byte[] s,
            int index,
            ulong len,
            ulong seed0,
            ulong seed1)
        {
            if (len <= 64)
            {
                return this.Hash64WithSeedsNA(s, index, len, seed0, seed1);
            }
            else
            {
                var x = seed0;
                var y = seed1 * k2 + 113;
                var z = this.ShiftMix(y * k2) * k2;
                var v = MutableTuple.Create(seed0, seed1);
                var w = MutableTuple.Create(0UL, 0UL);
                var u = x - z;
                x *= k2;
                var mul = k2 + (u & 0x82);

                var p = index;
                var end = p + (((int)len - 1) / 64) * 64;
                var last64 = end + (((int)len - 1) & 63) - 63;
                System.Diagnostics.Debug.Assert(index + (int)len - 64 == last64);
                do
                {
                    var a0 = BitConverter.ToUInt64(s, p);
                    var a1 = BitConverter.ToUInt64(s, p + 8);
                    var a2 = BitConverter.ToUInt64(s, p + 16);
                    var a3 = BitConverter.ToUInt64(s, p + 24);
                    var a4 = BitConverter.ToUInt64(s, p + 32);
                    var a5 = BitConverter.ToUInt64(s, p + 40);
                    var a6 = BitConverter.ToUInt64(s, p + 48);
                    var a7 = BitConverter.ToUInt64(s, p + 56);
                    x += a0 + a1;
                    y += a2;
                    z += a3;
                    v.Item1 += a4;
                    v.Item2 += a5 + a1;
                    w.Item1 += a6;
                    w.Item2 += a7;

                    x = Utils.RotateRight(x, 26);
                    x *= 9;
                    y = Utils.RotateRight(y, 29);
                    z *= mul;
                    v.Item1 = Utils.RotateRight(v.Item1, 33);
                    v.Item2 = Utils.RotateRight(v.Item2, 30);
                    w.Item1 ^= x;
                    w.Item1 *= 9;
                    z = Utils.RotateRight(z, 32);
                    z += w.Item2;
                    w.Item2 += z;
                    z *= 9;
                    var t = u;
                    u = y;
                    y = t;

                    z += a0 + a6;
                    v.Item1 += a2;
                    v.Item2 += a3;
                    w.Item1 += a4;
                    w.Item2 += a5 + a6;
                    x += a1;
                    y += a7;

                    y += v.Item1;
                    v.Item1 += x - y;
                    v.Item2 += w.Item1;
                    w.Item1 += v.Item2;
                    w.Item2 += x - y;
                    x += w.Item2;
                    w.Item2 = Utils.RotateRight(w.Item2, 34);

                    t = u;
                    u = z;
                    z = t;
                    p += 64;
                } while (p != end);
                // Make s point to the last 64 bytes of input.
                p = last64;
                u *= 9;
                v.Item2 = Utils.RotateRight(v.Item2, 28);
                v.Item1 = Utils.RotateRight(v.Item1, 20);
                w.Item1 += ((len - 1) & 63);
                u += y;
                y += u;
                x = Utils.RotateRight(y - x + v.Item1 + BitConverter.ToUInt64(s, p + 8), 37) * mul;
                y = Utils.RotateRight(y ^ v.Item2 ^ BitConverter.ToUInt64(s, p + 48), 42) * mul;
                x ^= w.Item2 * 9;
                y += v.Item1 + BitConverter.ToUInt64(s, p + 40);
                z = Utils.RotateRight(z + w.Item1, 33) * mul;
                v = this.WeakHashLen32WithSeeds(s, p, v.Item2 * mul, x + w.Item1);
                w = this.WeakHashLen32WithSeeds(s, p + 32, z + w.Item2, y + BitConverter.ToUInt64(s, p + 16));
                return this.AuxH(this.HashLen16NA(v.Item1 + x, w.Item1 ^ y, mul) + z - u,
                         this.AuxH(v.Item2 + y, w.Item2 + z, k2, 30) ^ x,
                         k2,
                         31);
            }
        }

        /// <summary>
        /// Determina o código confuso de 64 bit com valor inicial.
        /// </summary>
        /// <param name="s">O objecto do qual se pretende obter o código confuso.</param>
        /// <param name="index">O índice a partir do qual é calculado o código confuso.</param>
        /// <param name="len">O comprimento a ser considerado para o código confuso.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash64WithSeed(byte[] s, int index, ulong len, ulong seed)
        {
            return len <= 64 ? this.Hash64WithSeedNA(s, index, len, seed) :
                this.Hash64WithSeeds(s, index, len, 0, seed);
        }

        /// <summary>
        /// Determina o código confuso de 64 bit.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual é calculado o código confuso.</param>
        /// <param name="len">O comprimento a ser considerado para o código confuso.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash64UO(byte[] s, int index, ulong len)
        {
            return len <= 64 ? this.Hash64NA(s, index, len) :
                this.Hash64WithSeeds(s, index, len, 81, 0);
        }

        /// <summary>
        /// Determina o código confuso de 64 bit.
        /// </summary>
        /// <param name="s">O objecto.</param>
        /// <param name="index">O índice a partir do qual é calculado o código confuso.</param>
        /// <param name="len">O comprimento considerado.</param>
        /// <returns>O código confuso.</returns>
        private ulong Hash64(byte[] s, int index, ulong len)
        {
            if (len <= 32)
            {
                if (len <= 16)
                {
                    return this.HashLen0to16NA(s, index, len);
                }
                else
                {
                    return HashLen17to32NA(s, index, len);
                }
            }
            else if (len <= 64)
            {
                return this.HashLen33to64(s, index, len);
            }
            else if (len <= 96)
            {
                return this.HashLen65to96(s, index, len);
            }
            else if (len <= 256)
            {
                return this.Hash64NA(s, index, len);
            }
            else
            {
                return this.Hash64UO(s, index, len);
            }
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Define funções que permitem calcular códigos confusos simples.
    /// </summary>
    public static class SimpleHashFunctions
    {
        /// <summary>
        /// Mantém o número de bits num inteiro se sinal.
        /// </summary>
        private static int numberOfBitsInUint = (sizeof(uint) * 8);

        /// <summary>
        /// Três quartos do número de bits.
        /// </summary>
        private static int threeQuarters = (numberOfBitsInUint * 3) / 4;

        /// <summary>
        /// Um oitavo do número de bits.
        /// </summary>
        private static int oneEight = sizeof(uint);

        /// <summary>
        /// O valor dos bits mais elevados.
        /// </summary>
        private static uint highBits = 0xFFFFFFFFU << (numberOfBitsInUint - oneEight);

        /// <summary>
        /// Tabela com números primos.
        /// </summary>
        private static int[] primes = new[]{
            3, 5, 7, 11, 13, 17, 19, 23, 29,
            31, 37, 41, 43, 47, 53, 59, 61,
            67, 71, 73, 79, 83, 89, 97, 101,
            103, 107, 109, 113};

        /// <summary>
        /// Implementa um algoritmo rápido para gerar códigos confusos.
        /// </summary>
        /// <param name="obj">A chave.</param>
        /// <param name="start">A posição na chave onde o algoritmo deve ser iniciado.</param>
        /// <param name="length">O comprimento válido.</param>
        /// <returns>O valor do código confuso.</returns>
        public static uint SuperFastHash(byte[] obj, int start, int length)
        {
            if (length <= 0 || obj == null)
            {
                return 0;
            }
            else if (start + length > obj.Length)
            {
                throw new ArgumentException("Length value is greater than object's length.");
            }
            else
            {
                var hash = (uint)length;
                var rem = length & 3;
                length >>= 2;

                var pointer = start;
                for (; length > 0; length--)
                {
                    hash += (uint)BitConverter.ToUInt16(obj, pointer);
                    var tmp = (uint)(BitConverter.ToUInt16(obj, pointer + 2) << 11) ^ hash;
                    hash = (hash << 16) ^ tmp;
                    pointer += 4;
                    hash += hash >> 11;
                }

                switch (rem)
                {
                    case 3:
                        hash += (uint)BitConverter.ToUInt16(obj, pointer);
                        hash ^= hash << 16;
                        var t = (sbyte)obj[pointer + 2];
                        hash ^= (uint)(t << 18);
                        hash += hash >> 11;
                        break;
                    case 2: hash += (uint)BitConverter.ToUInt16(obj, pointer);
                        hash ^= hash << 11;
                        hash += hash >> 17;
                        break;
                    case 1:
                        t = (sbyte)obj[pointer];
                        if (t < 0)
                        {
                            hash -= (uint)(~t + 1);
                        }
                        else
                        {
                            hash += (uint)t;
                        }
                        hash ^= hash << 10;
                        hash += hash >> 1;
                        break;
                }

                /* Force "avalanching" of final 127 bits */
                hash ^= hash << 3;
                hash += hash >> 5;
                hash ^= hash << 4;
                hash += hash >> 17;
                hash ^= hash << 25;
                hash += hash >> 6;

                return hash;
            }
        }

        /// <summary>
        /// Implementa um algoritmo simples para código confuso baseado na representação
        /// inteira dos dados na base b.
        /// </summary>
        /// <remarks>
        /// A linguagem java implementa este algoritmo com b = 31.
        /// </remarks>
        /// <param name="obj">A mensagem sobre a qual se aplica o algoritmo.</param>
        /// <param name="start">O início da mensagem onde se pretende aplicar o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem a aplicar o algoritmo.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <param name="b">A base.</param>
        /// <returns>O valor do código confuso.</returns>
        public static uint PowerHash32(
            byte[] obj,
            int start,
            int length,
            uint seed,
            uint b = 31)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var h = seed;
                    if (obj != null && obj.Length > 0)
                    {
                        for (int i = 0; i < obj.Length; i++)
                        {
                            h = 31 * h + obj[i];
                        }
                    }

                    return h;
                }
            }
        }

        /// <summary>
        /// Implementa um algoritmo simples para código confuso baseado na representação
        /// inteira dos dados na base b.
        /// </summary>
        /// <remarks>
        /// A linguagem java implementa este algoritmo com b = 31.
        /// </remarks>
        /// <param name="obj">A mensagem sobre a qual se aplica o algoritmo.</param>
        /// <param name="start">O início da mensagem onde se pretende aplicar o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem a aplicar o algoritmo.</param>
        /// <param name="seed">O valor inicial.</param>
        /// <param name="b">A base.</param>
        /// <returns>O valor do código confuso.</returns>
        public static ulong PowerHash64(
            byte[] obj,
            int start,
            int length,
            ulong seed,
            ulong b = 31)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var h = seed;
                    if (obj != null && obj.Length > 0)
                    {
                        for (int i = start; i < final; i++)
                        {
                            h = 31 * h + obj[i];
                        }
                    }

                    return h;
                }
            }
        }

        /// <summary>
        /// Implementa o algoritmo de código confuso PJW.
        /// </summary>
        /// <param name="obj">A mensgem sobre a qual aplicar o algoritmo.</param>
        /// <param name="start">O início da mensagem onde se pretende aplicar o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem a aplicar o algoritmo.</param>
        /// <returns>O valor do código confuso.</returns>
        public static uint PJWHash(
            byte[] obj,
            int start,
            int length)
        {
            if (obj == null)
            {
                return 0U;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var hash = 0U;
                    for (var i = start; i < final; ++i)
                    {
                        hash = (hash << oneEight) + obj[i];
                        var test = hash & highBits;
                        if (test != 0)
                        {
                            hash = ((hash ^ (test >> threeQuarters)) & (~highBits));
                        }
                    }

                    return hash;
                }
            }
        }

        /// <summary>
        /// Implementa o algoritmo de código confuso ELF.
        /// </summary>
        /// <param name="obj">A mensgem sobre a qual aplicar o algoritmo.</param>
        /// <param name="start">O início da mensagem onde se pretende aplicar o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem a aplicar o algoritmo.</param>
        /// <returns>O valor do código confuso.</returns>
        public static uint ELFHash(byte[] obj, int start, int length)
        {
            var final = start + length;
            if (final > obj.Length)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
            else
            {
                var hash = 0U;

                for (var i = 0; i < length; i++)
                {
                    hash = (hash << 4) + obj[i];
                    var x = hash & 0xF0000000U;
                    if (x != 0)
                    {
                        hash ^= (x >> 24);
                    }

                    hash &= ~x;
                }

                return hash;
            }
        }

        /// <summary>
        /// Implementa o algoritmo de código confuso NHash de 32 bits.
        /// </summary>
        /// <param name="obj">A mensgem sobre a qual aplicar o algoritmo.</param>
        /// <param name="start">O início da mensagem onde se pretende aplicar o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem a aplicar o algoritmo.</param>
        /// <returns>O valor do código confuso.</returns>
        public static uint NumericHash32(byte[] obj, int start, int length)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var hash = 0U;
                    var primesPointer = 0;
                    for (var i = start; i < final; ++i)
                    {
                        primesPointer += 28;
                        primesPointer %= 29;
                        hash += (uint)primes[primesPointer] * obj[i];
                    }

                    return hash;
                }
            }
        }

        /// <summary>
        /// Implementa o algoritmo de código confuso NHash de 64 bits.
        /// </summary>
        /// <param name="obj">A mensgem sobre a qual aplicar o algoritmo.</param>
        /// <param name="start">O início da mensagem onde se pretende aplicar o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem a aplicar o algoritmo.</param>
        /// <returns>O valor do código confuso.</returns>
        public static ulong NumericHash64(byte[] obj, int start, int length)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var hash = 0UL;
                    var primesPointer = 0;
                    for (var i = start; i < final; ++i)
                    {
                        primesPointer += 28;
                        primesPointer %= 29;
                        hash += (ulong)primes[primesPointer] * obj[i];
                    }

                    return hash;
                }
            }
        }

        /// <summary>
        /// Implementa o algoritmo de código confuso NHash de 32 bits.
        /// </summary>
        /// <remarks>
        /// O desempenho deste algoritmo pode ser melhorado evitando o cálculo da função
        /// de potência em cada chamada da função. Tal melhoria pode ser conseguida, implementando
        /// o algoritmo numa classe independente. Neste ponto parece não compensar.
        /// </remarks>
        /// <param name="obj">A mensgem sobre a qual aplicar o algoritmo.</param>
        /// <param name="start">O início da mensagem onde se pretende aplicar o algoritmo.</param>
        /// <param name="length">O tamanho da mensagem a aplicar o algoritmo.</param>
        /// <param name="bits">O número de bits.</param>
        /// <returns>O valor do código confuso.</returns>
        public static BigInteger NumericHashN(byte[] obj, int start, int length, int bits)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var final = start + length;
                if (final > obj.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    var bitsPow = BigInteger.Pow(2, bits);
                    var hash = BigInteger.Zero;
                    var primesPointer = 0;
                    for (var i = start; i < final; ++i)
                    {
                        primesPointer += 28;
                        primesPointer %= 29;
                        hash += primes[primesPointer] * obj[i];
                        BigInteger.DivRem(hash, bitsPow, out hash);
                    }

                    return hash;
                }
            }
        }
    }
}
