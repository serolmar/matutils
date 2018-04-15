// -----------------------------------------------------------------------
// <copyright file="RandomGenerators.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Gerador de números aleatórios de alto desempenho.
    /// </summary>
    /// <remarks>
    /// Ver: http://www.math.keio.ac.jp/~matumoto/emt.html.
    /// </remarks>
    public class MTRand
    {
        /// <summary>
        /// O tamanho do estado.
        /// </summary>
        protected const uint stateLen = 624;

        /// <summary>
        /// O tamanho do vector para gravação.
        /// </summary>
        protected const uint saveLen = stateLen + 1;

        /// <summary>
        /// O valor do período.
        /// </summary>
        protected const uint period = 397;

        /// <summary>
        /// Valor auxiliar a ser utilizado na função que gera códigos confusos
        /// para valores iniciais.
        /// </summary>
        private static uint hashDiff = 0U;

        /// <summary>
        /// Mantém os valores do estado.
        /// </summary>
        protected uint[] state = new uint[stateLen];

        /// <summary>
        /// Mantém o próximo índice do estado.
        /// </summary>
        protected uint nextStateIndex;

        /// <summary>
        /// Número de valores processados até que seja necessário o
        /// próximo carregamento.
        /// </summary>
        protected uint left;

        #region Construtuores

        /// <summary>
        /// Instancia uma nova instância de objectos do tiop <see cref="MTRand"/>.
        /// </summary>
        public MTRand()
        {
            this.Seed();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tiop <see cref="MTRand"/>.
        /// </summary>
        /// <param name="oneSeed">Um valor inicial único.</param>
        public MTRand(uint oneSeed)
        {
            this.Seed(oneSeed);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tiop <see cref="MTRand"/>.
        /// </summary>
        /// <param name="bigSeed">Um vector de valores iniciais.</param>
        /// <param name="seedLenght">O número de valores iniciais a serem considerados.</param>
        public MTRand(uint[] bigSeed, uint seedLenght)
        {
            this.Seed(bigSeed, seedLenght);
        }

        #endregion Construtores

        /// <summary>
        /// Obtém o tamanho do vector de estado.
        /// </summary>
        public static uint StateLen
        {
            get
            {
                return stateLen;
            }
        }

        /// <summary>
        /// Obtem o tamanho do vector para gravação.
        /// </summary>
        public static uint SaveLen
        {
            get
            {
                return saveLen;
            }
        }

        #region Funções públicas

        /// <summary>
        /// Gera um número real no intervalo [0,1].
        /// </summary>
        /// <returns>O número gerado.</returns>
        public double Rand()
        {
            return this.RandInt() * (1.0D / 4294967295.0D);
        }

        /// <summary>
        /// Gera um número real no intervalo [0,n].
        /// </summary>
        /// <param name="n">O limite superior do intervalo.</param>
        /// <returns>O número gerado.</returns>
        public double Rand(double n)
        {
            return this.Rand() * n;
        }

        /// <summary>
        /// Gera num número real no intervalo [0,1).
        /// </summary>
        /// <returns>O número gerado.</returns>
        public double RandExcept()
        {
            return this.RandInt() * (1.0D / 4294967296.0D);
        }

        /// <summary>
        /// Gera um número real no intervalo [0,n).
        /// </summary>
        /// <param name="n">O limite superior do intervalo.</param>
        /// <returns>O número gerado.</returns>
        public double RandExcept(double n)
        {
            return this.RandExcept() * n;
        }

        /// <summary>
        /// Gera um número real no intervalo (0,1).
        /// </summary>
        /// <returns>O número gerado.</returns>
        public double RandDoubleExcept()
        {
            return (this.RandInt() + 0.5D) * (1.0D / 4294967296.0D);
        }

        /// <summary>
        /// Gera um número no intervalo (0,n).
        /// </summary>
        /// <param name="n">O limite superior do intervalo.</param>
        /// <returns>O número gerado.</returns>
        public double RandDoubleExcept(uint n)
        {
            return this.RandDoubleExcept() * n;
        }

        /// <summary>
        /// Gera um número inteiro no intervalo [0,2^32-1].
        /// </summary>
        /// <returns>O número inteiro.</returns>
        public uint RandInt()
        {
            if (left == 0)
            {
                this.Reload();
            }

            --left;

            var s1 = 0U;
            s1 = this.state[this.nextStateIndex++];
            s1 ^= (s1 >> 11);
            s1 ^= (s1 << 7) & 0x9d2c5680U;
            s1 ^= (s1 << 15) & 0xefc60000U;
            return (s1 ^ (s1 >> 18));
        }

        /// <summary>
        /// Gera um número inteiro no intervalo [0,n].
        /// </summary>
        /// <param name="n">O limite superior do intervalo.</param>
        /// <returns>O número gerado.</returns>
        public uint RandInt(uint n)
        {
            var used = n;
            used |= used >> 1;
            used |= used >> 2;
            used |= used >> 4;
            used |= used >> 8;
            used |= used >> 16;

            var i = 0U;
            do
            {
                i = this.RandInt() & used;
            } while (i > n);

            return i;
        }

        /// <summary>
        /// Gera um número real com precisão de 53 bit no intervalo [0,1).
        /// </summary>
        /// <returns>O número gerado.</returns>
        public double Rand53()
        {
            var a = this.RandInt() >> 5;
            var b = this.RandInt() >> 6;
            return (a * 67108864.0D + b) * (1.0D / 9007199254740992.0D);
        }

        /// <summary>
        /// Gera um número real de acordo com a distribuição normal.
        /// </summary>
        /// <param name="mean">A média da distribuição.</param>
        /// <param name="variance">A variância da distribuição.</param>
        /// <returns>O número gerado.</returns>
        public double RandNorm(double mean = 0.0, double variance = 0.0)
        {
            double r = Math.Sqrt(-2.0 * Math.Log(1.0 - this.RandDoubleExcept())) * variance;
            double phi = 2.0 * 3.14159265358979323846264338328 * this.RandExcept();
            return mean + r * Math.Cos(phi);
        }

        /// <summary>
        /// Reatribui valores ao estado.
        /// </summary>
        /// <param name="oneSeed">O valor inicial.</param>
        public void Seed(uint oneSeed)
        {
            this.Initialize(oneSeed);
            this.Reload();
        }

        /// <summary>
        /// Reatribui valores ao estado.
        /// </summary>
        /// <param name="bigSeed">O vector de valores iniciais.</param>
        /// <param name="seedLength">O número de valore a serem considerados.</param>
        public void Seed(uint[] bigSeed, uint seedLength)
        {
            this.Initialize(19650218U);
            var i = 1;
            var j = 0U;
            var k = (stateLen > seedLength ? stateLen : seedLength);
            for (; k > 0; --k)
            {
                this.state[i] =
                    this.state[i] ^ ((state[i - 1] ^ (state[i - 1] >> 30)) * 1664525U);
                this.state[i] += (bigSeed[j] & 0xFFFFFFFFU) + j;
                this.state[i] &= 0xFFFFFFFFU;
                ++i;
                ++j;
                if (i >= stateLen)
                {
                    this.state[0] = this.state[stateLen - 1];
                    i = 1;
                }

                if (j >= seedLength)
                {
                    j = 0;
                }
            }

            for (k = stateLen - 1; k > 0; --k)
            {
                this.state[i] =
                    this.state[i] ^ ((this.state[i - 1] ^ (this.state[i - 1] >> 30)) * 1566083941U);
                this.state[i] -= (uint)i;
                this.state[i] &= 0xFFFFFFFFU;
                ++i;
                if (i >= stateLen)
                {
                    this.state[0] = this.state[stateLen - 1];
                    i = 1;
                }
            }

            this.state[0] = 0x80000000U;
            this.Reload();
        }

        /// <summary>
        /// Reatribui valores ao estado.
        /// </summary>
        public void Seed()
        {
            // TODO: incluir os valores gerados pelo sistema operativo no caso do Linux
            this.Seed(this.Hash(DateTime.Now));
        }

        /// <summary>
        /// Guarda o estado no vector.
        /// </summary>
        /// <param name="array">O vector.</param>
        public void Save(uint[] array)
        {
            var p = 0;
            var sp = 0;
            var i = stateLen;
            for (; i > 0; --i)
            {
                array[p++] = this.state[sp++];
            }

            array[p] = this.left;
        }

        /// <summary>
        /// Carrega o estado a partir de um vector.
        /// </summary>
        /// <param name="loadArray"></param>
        public void Load(uint[] loadArray)
        {
            var p = 0;
            var lp = 0;
            var i = stateLen;
            for (; i > 0; --i)
            {
                loadArray[lp++] = this.state[p++];
            }

            this.left = loadArray[lp];
            this.nextStateIndex = stateLen - this.left;
        }

        #endregion Funções públicas

        #region Funções protegidas

        /// <summary>
        /// Função de inicialização do estado.
        /// </summary>
        /// <param name="oneSeed">O valor inicial.</param>
        protected void Initialize(uint oneSeed)
        {
            var p1 = 0;
            var p2 = 0;
            var i = 1U;
            this.state[p1++] = 0xFFFFFFFFU;
            for (; i < stateLen; ++i)
            {
                this.state[p1++] =
                    (1812433253U * (this.state[p2] ^ (this.state[p2] >> 30)) + i) &
                    0xFFFFFFFFU;
                ++p2;
            }
        }

        /// <summary>
        /// Recarrega os valores do estado.
        /// </summary>
        protected void Reload()
        {
            var p = 0;
            var start = stateLen - period;
            for (var i = start; i > 0; --i, ++p)
            {
                this.state[p] = this.Twist(
                    this.state[start + p],
                    this.state[p],
                    this.state[p + 1]);
            }

            for (var i = period; i > 1; --i, ++p)
            {
                this.state[p] = this.Twist(
                    this.state[p - start],
                    this.state[p],
                    this.state[p + 1]);
            }

            this.state[p] = this.Twist(
                    this.state[p - start],
                    this.state[p],
                    this.state[0]);
            this.left = stateLen;
            this.nextStateIndex = 0;
        }

        /// <summary>
        /// Obtém o bit mais significativo de um número.
        /// </summary>
        /// <param name="u">O número.</param>
        /// <returns>O bit mais significativo.</returns>
        protected uint HighBit(uint u)
        {
            return u & 0x80000000U;
        }

        /// <summary>
        /// Obtém o bit menos significativo de um número.
        /// </summary>
        /// <param name="u">O número.</param>
        /// <returns>O bit menos significativo.</returns>
        protected uint LowBit(uint u)
        {
            return u & 0x00000001U;
        }

        /// <summary>
        /// Obtém um conjutno de bits menos significativos de 
        /// um número.
        /// </summary>
        /// <param name="u">O número.</param>
        /// <returns>Os bits menos significativos.</returns>
        protected uint LowBits(uint u)
        {
            return u & 0x7FFFFFFfU;
        }

        /// <summary>
        /// Mistura os bits de dois números.
        /// </summary>
        /// <param name="u">O primeiro número.</param>
        /// <param name="v">O segundo número.</param>
        /// <returns>O resultado da mistura.</returns>
        protected uint MixBits(uint u, uint v)
        {
            return this.HighBit(u) | this.LowBits(v);
        }

        /// <summary>
        /// Inflecte um número.
        /// </summary>
        /// <param name="m">O número a ser inflectido.</param>
        /// <param name="s0">O primeiro parâmetro da inflexão.</param>
        /// <param name="s1">O segundo parâmetro da inflexão.</param>
        /// <returns>O resultado da inflexão.</returns>
        protected uint Twist(uint m, uint s0, uint s1)
        {
            return m ^ (this.MixBits(s0, s1) >> 1) ^ (this.LowBit(s1) & 0x9908b0dfU);
        }

        /// <summary>
        /// Obtém um código confuso baseado na data.
        /// </summary>
        /// <param name="date">A data.</param>
        /// <returns>O código confuso.</returns>
        protected uint Hash(DateTime date)
        {
            var bytes = BitConverter.GetBytes(date.Ticks);
            var length = bytes.Length;
            var halfLength = length >> 1;
            var h1 = 0U;
            var i = 0;
            for (; i < halfLength; ++i)
            {
                h1 *= 0xFF + 2U;
                h1 += bytes[i];
            }

            var h2 = 0U;
            for (; i < length; ++i)
            {
                h2 *= 0xFF + 2U;
                h2 += bytes[i];
            }

            return (h1 + hashDiff++) ^ h2;
        }

        #endregion Funções protegidas
    }
}
