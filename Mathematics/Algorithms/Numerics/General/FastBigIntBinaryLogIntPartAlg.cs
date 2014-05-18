namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Uma versão mais rápida que permite calcular o logaritmo binário (base 2) de um número.
    /// </summary>
    public class FastBigIntBinaryLogIntPartAlg : IAlgorithm<BigInteger, BigInteger>
    {
        /// <summary>
        /// Permite calcular a parte inteira do logaritmo binário de um número.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>A parte inteira do logaritmo.</returns>
        /// <exception cref="ArgumentException">Se o número não for positivo.</exception>
        public BigInteger Run(BigInteger data)
        {
            if (data <= 0)
            {
                throw new ArgumentException("Can only compute logarithms of positive numbers.");
            }
            else
            {
                var b = 1;
                var r = BigInteger.Zero;
                var pr = BigInteger.One;
                while (b > 0)
                {
                    var sr = r;
                    var spr = pr;

                    r += b;
                    pr = pr << b;
                    if (pr > data)
                    {
                        r = sr;
                        pr = spr;
                        b = b >> 1;
                    }
                    else
                    {
                        b = b << 1;
                    }
                }

                return r;
            }
        }
    }
}
