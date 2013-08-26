namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo AKS cujo propósito consiste em averiguar
    /// se um número é primo.
    /// </summary>
    public class AksPrimalityTest : IAlgorithm<int, bool>
    {
        /// <summary>
        /// Averigua se o número especificado é primo.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>Verdadeiro caso o número seja primo e falso caso este seja composto.</returns>
        public bool Run(int data)
        {
            if (data == 0)
            {
                return false;
            }
            else if (data == 1)
            {
                return false;
            }
            else
            {
                var innerData = Math.Abs(data);
                var sqrt = (int)Math.Floor(Math.Sqrt(innerData));
                if (sqrt * sqrt == innerData)
                {
                    return false;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            throw new NotImplementedException();
        }
    }
}
