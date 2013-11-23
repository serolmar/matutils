namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerNormSpace : INormSpace<int, int>
    {
        /// <summary>
        /// Obtém o módulo de um número inteiro.
        /// </summary>
        /// <param name="value">O número inteiro.</param>
        /// <returns>O módulo do número.</returns>
        public int GetNorm(int value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Compara dois números inteiros.
        /// </summary>
        /// <param name="x">O primeiro número a ser compardao.</param>
        /// <param name="y">O segundo número a ser comparado.</param>
        /// <returns>
        /// O valor 1 caso o primeiro seja superior ao segundo, 0 csao sejam iguais e -1 caso contrário.
        /// </returns>
        public int Compare(int x, int y)
        {
            return Comparer<int>.Default.Compare(x, y);
        }
    }
}
