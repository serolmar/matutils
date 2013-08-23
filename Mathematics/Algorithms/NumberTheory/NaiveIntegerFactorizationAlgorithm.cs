namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo mais simples para factorizar um número.
    /// </summary>
    public class NaiveIntegerFactorizationAlgorithm : IAlgorithm<int, Dictionary<int, int>>
    {
        /// <summary>
        /// Obtém a factorização do número proporcionado.
        /// </summary>
        /// <param name="data">O número a ser factorizado.</param>
        /// <returns>Os factores primos afectos do respectivo grau.</returns>
        public Dictionary<int, int> Run(int data)
        {
            if (data == 0)
            {
                throw new ArgumentException("Zero has no factor.");
            }
            else if (data == 1)
            {
                var result = new Dictionary<int, int>();
                result.Add(1, 1);
                return result;
            }
            else if (data == -1) {
                var result = new Dictionary<int, int>();
                result.Add(-1, 1);
                return result;
            }
            else
            {
                var result = new Dictionary<int, int>();
                var innerData = (int)Math.Abs(data);
                var power = 0;
                while (innerData % 2 == 0)
                {
                    ++power;
                    innerData = innerData / 2;
                }

                if (power > 0)
                {
                    result.Add(2, power);
                }

                if (innerData != 1)
                {
                    var sqrt = (int)Math.Floor(Math.Sqrt(data));
                    var nextNumber = 3;
                    while (nextNumber <= sqrt && nextNumber <= innerData)
                    {
                        power = 0;
                        while (innerData % nextNumber == 0)
                        {
                            ++power;
                            innerData = innerData / nextNumber;
                        }

                        if (power > 0)
                        {
                            result.Add(nextNumber, power);
                        }

                        nextNumber += 2;
                    }

                    if (innerData != 1)
                    {
                        result.Add(innerData, 1);
                    }
                }

                // O factor que representa o sinal no caso de ser necessário.
                if (data < 0)
                {
                    result.Add(-1, 1);
                }

                return result;
            }
        }
    }
}
