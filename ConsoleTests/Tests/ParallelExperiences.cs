namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ParallelExperiences
    {
        /// <summary>
        /// Permite aplicar uma função de agregação a um vector de valores.
        /// </summary>
        /// <remarks>
        /// O resultado da agregação será colocado na primeira posição do vector.
        /// </remarks>
        /// <typeparam name="T">O tipo de elementos que constituem as entradas a serem agregadas.</typeparam>
        /// <param name="values">Os valores a serem agregados.</param>
        /// <param name="aggregateFunc">A função de agregação.</param>
        /// <param name="processes">O número de processos.</param>
        public static void UpdateAgregate<T>(T[] values, Func<T, T, T> aggregateFunc)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("Nada para agregar.");
            }
            else
            {
                var length = values.Length;
                while (length > 1)
                {
                    if ((length & 1) == 0)
                    {
                        // Número par de elementos
                        length >>= 1;
                        Parallel.For(0, length, i =>
                        {
                        });

                        for (int i = 0; i < length; ++i)
                        {
                            values[i] = aggregateFunc.Invoke(values[i], values[length + i]);
                        }
                    }
                    else
                    {
                        // Número ímpar de elementos
                        length = (length + 1) >> 1;
                        var prevLength = length - 1;
                        for (int i = 0; i < prevLength; ++i)
                        {
                            values[i] = aggregateFunc.Invoke(values[i], values[length + i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Permite aplicar uma função de agregação a um vector de valores cujo comprimento seja igual
        /// a uma potência de dois.
        /// </summary>
        /// <remarks>
        /// O resultado da agregação será colocado na primeira posição do vector.
        /// </remarks>
        /// <typeparam name="T">O tipo de elementos que constituem as entradas a serem agregadas.</typeparam>
        /// <param name="values">Os valores a serem agregados.</param>
        /// <param name="aggregateFunc">A função de agregação.</param>
        /// <param name="processes">O número de processos.</param>
        public static void UpdateAgregatePowerOfTwo<T>(T[] values, Func<T, T, T> aggregateFunc)
        {
            if (values == null || values.Length == 0)
            {
                throw new ArgumentException("Nada para agregar.");
            }
            else
            {
                var length = values.Length;
                while (length > 0)
                {
                    for (int i = 0; i < length; ++i)
                    {
                        values[i] = aggregateFunc.Invoke(values[i], values[length + i]);
                    }

                    length >>= 1;
                }
            }
        }
    }
}
