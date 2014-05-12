namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um espaço normal sobre números de precisão dupla.
    /// </summary>
    public class DoubleNormSpace : INormSpace<double,double>
    {
        /// <summary>
        /// Obtém o módulo de um número decimal.
        /// </summary>
        /// <param name="value">O número decimal.</param>
        /// <returns>O módulo do número.</returns>
        public double GetNorm(double value)
        {
            return Math.Abs(value);
        }

        /// <summary>
        /// Compara dois números decimais.
        /// </summary>
        /// <param name="x">O primeiro número a ser compardao.</param>
        /// <param name="y">O segundo número a ser comparado.</param>
        /// <returns>
        /// O valor 1 caso o primeiro seja superior ao segundo, 0 csao sejam iguais e -1 caso contrário.
        /// </returns>
        public int Compare(double x, double y)
        {
            return Comparer<double>.Default.Compare(x, y);
        }
    }
}
