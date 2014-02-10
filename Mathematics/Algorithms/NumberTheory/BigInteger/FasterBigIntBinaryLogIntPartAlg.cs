namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Forma muito rápida para calcular a parte inteira do logaritmo de um número grande.
    /// </summary>
    /// <remarks>
    /// Ver: http://stackoverflow.com/questions/12003719/log-of-a-very-large-number
    /// </remarks>
    public class FasterBigIntBinaryLogIntPartAlg : IAlgorithm<BigInteger, BigInteger>
    {
        /// <summary>
        /// Os valores calculados do primeiro "byte".
        /// </summary>
        static int[] preCalc = new int[] { 
            8, 7, 6, 6, 5, 5, 5, 5, 4, 4, 4, 4, 4, 4, 4, 4, 
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
        
        /// <summary>
        /// Calcula a parte inteira do logaritmo do argumento de forma muito rápida.
        /// </summary>
        /// <param name="data">O argumento.</param>
        /// <returns>O logaritmo do número.</returns>
        public BigInteger Run(BigInteger data)
        {
            if (data <= 0)
            {
                throw new ArgumentException("Can only compute logarithms of positive numbers.");
            }
            else
            {
                byte[] buf = data.ToByteArray();
                int len = buf.Length;
                return (len << 3) - preCalc[buf[len - 1]] - 1;
            }
        }
    }
}
