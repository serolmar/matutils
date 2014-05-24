namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Obtém o objecto do tipo alvo mais próximo do tipo fonte.
    /// </summary>
    /// <remarks>
    /// Um possível exemplo consite na determinação do inteiro que se encontra mais próximo de uma fracção ou
    /// de um decimal.
    /// </remarks>
    public class DecimalNearestInteger : INearest<Decimal, Decimal>
    {
        /// <summary>
        /// Otbém o objecto mais próximo da fote especificada.
        /// </summary>
        /// <param name="source">A fonte.</param>
        /// <returns>O objecto mais próximo.</returns>
        public decimal GetNearest(decimal source)
        {
            return Math.Round(source);
        }
    }
}
