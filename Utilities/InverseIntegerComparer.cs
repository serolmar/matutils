namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o comparador de inteiros invertido.
    /// </summary>
    public class InverseIntegerComparer : Comparer<int>
    {
        /// <summary>
        /// Compara dois inteiros de forma inversa.
        /// </summary>
        /// <param name="x">O primeiro elemento a ser comparado.</param>
        /// <param name="y">O segundo elemento a ser comparado.</param>
        /// <returns>Retorna -1 caso x seja superior a y, 0 caso sejam iguais e 1 se x for inferior a y.</returns>
        public override int Compare(int x, int y)
        {
            return -Comparer<int>.Default.Compare(x, y);
        }
    }
}
