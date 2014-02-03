namespace Mathematics.Algorithms.NumberTheory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite calcular o valor inteiro da raiz quadrada de um número.
    /// </summary>
    public class IntegerSquareNumberAlgorithm : IAlgorithm<int,int>
    {
        /// <summary>
        /// Permite calcular o valor inteiro da raiz quadrada do número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O resultado da sua raiz inteira.</returns>
        public int Run(int number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Can't compute the square root of a negative number.");
            }
            else if (number == 0)
            {
                return 0;
            }
            else
            {
                return (int)Math.Floor(Math.Sqrt(number));
            }
        }
    }
}
