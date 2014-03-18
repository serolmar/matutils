namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o comparador de inteiros invertido.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente a ser comparado.</typeparam>
    public class InverseComparer<CoeffType> : Comparer<CoeffType>
    {
        /// <summary>
        /// O coeficiente a ser comparado.
        /// </summary>
        private IComparer<CoeffType> coeffsComparer;

        public InverseComparer(IComparer<CoeffType> coeffsComparer)
        {
            if (coeffsComparer == null)
            {
                throw new ArgumentNullException("coeffsComparer");
            }
            else
            {
                this.coeffsComparer = coeffsComparer;
            }
        }

        /// <summary>
        /// Compara dois inteiros de forma inversa.
        /// </summary>
        /// <param name="x">O primeiro elemento a ser comparado.</param>
        /// <param name="y">O segundo elemento a ser comparado.</param>
        /// <returns>Retorna -1 caso x seja superior a y, 0 caso sejam iguais e 1 se x for inferior a y.</returns>
        public override int Compare(CoeffType x, CoeffType y)
        {
            return -this.coeffsComparer.Compare(x, y);
        }
    }
}
