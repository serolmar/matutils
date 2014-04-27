namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Uma versão mais rápida que permite calcular o logaritmo binário (base 2) de um número.
    /// </summary>
    public class FastBinaryLogIntegerPartAlg : IAlgorithm<int, int>
    {
        /// <summary>
        /// Permite calcular a parte inteira do logaritmo binário de um número.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>A parte inteira do logaritmo.</returns>
        public int Run(int data)
        {
            if (data <= 0)
            {
                throw new ArgumentException("Can only compute logarithms of positive numbers.");
            }
            else
            {
                var b = 1;
                var r = 0;
                var pr = 1;
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
