namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Define as operações sobre um espaço normado de inteiros de precisão arbitrária.
    /// </summary>
    public class BigIntegerNormSpace : INormSpace<BigInteger, BigInteger>
    {
        /// <summary>
        /// Obtém o módulo de um número inteiro de precisão arbitrária.
        /// </summary>
        /// <param name="value">O número inteiro de precisão arbitrária.</param>
        /// <returns>O módulo do número.</returns>
        public BigInteger GetNorm(BigInteger value)
        {
            return BigInteger.Abs(value);
        }

        /// <summary>
        /// Compara dois números inteiros de precisão arbitrária.
        /// </summary>
        /// <param name="x">O primeiro número a ser compardao.</param>
        /// <param name="y">O segundo número a ser comparado.</param>
        /// <returns>
        /// O valor 1 caso o primeiro seja superior ao segundo, 0 csao sejam iguais e -1 caso contrário.
        /// </returns>
        public int Compare(BigInteger x, BigInteger y)
        {
            return BigInteger.Compare(x, y);
        }
    }
}
