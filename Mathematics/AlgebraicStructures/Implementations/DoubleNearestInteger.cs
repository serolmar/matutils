namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite obter o inteiro mais próximo de um número decimal.
    /// </summary>
    public class DoubleNearestInteger : INearest<double, int>
    {
        /// <summary>
        /// Obtém o inteiro que mais se aproxima do número especificado.
        /// </summary>
        /// <param name="source">O número.</param>
        /// <returns>O inteiro.</returns>
        public int GetNearest(double source)
        {
            return (int)Math.Round(source);
        }
    }
}
