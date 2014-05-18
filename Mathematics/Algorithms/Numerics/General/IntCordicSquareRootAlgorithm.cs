namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite calcular a raiz quadrada inteira de um número sem recorrer à livraria de matemática.
    /// </summary>
    public class IntCordicSquareRootAlgorithm : IAlgorithm<int, int>
    {
        /// <summary>
        /// Calcula a raiz quadrada inteira de um número.
        /// </summary>
        /// <param name="data">O número.</param>
        /// <returns>A raiz quadrada inteira.</returns>
        /// <exception cref="ArgumentException">Se o número for negativo.</exception>
        public int Run(int data)
        {
            if (data < 0)
            {
                throw new ArgumentException("Can't compute the square root of a negative number.");
            }
            else if (data == 0)
            {
                return data;
            }
            else
            {
                var b = 15;
                --b;
                var r = 0;
                var r2 = 0;
                while (b >= 0)
                {
                    var sr2 = r2;
                    var sr = r;

                    r2 += ((r << (1 + b)) + (1 << (b + b)));
                    r += (1 << b);
                    if (r2 > data)
                    {
                        r = sr;
                        r2 = sr2;
                    }

                    b--;
                }

                return r;
            }
        }
    }
}
