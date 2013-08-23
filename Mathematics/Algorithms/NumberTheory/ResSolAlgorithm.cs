namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo de Tonelli-Shanks.
    /// </summary>
    class ResSolAlgorithm : IAlgorithm<int, int, List<int>>
    {
        /// <summary>
        /// Determina o resíduo quadrático de um número módulo o número primo ímpar especificado.
        /// </summary>
        /// <remarks>
        /// Se o módulo não for um número primo ímpar, os resultados estarão errados. Esta verificação
        /// não é realizada sobre esse módulo.
        /// </remarks>
        /// <param name="number">O número.</param>
        /// <param name="primeModule">O número primo que servirá de módulo.</param>
        /// <returns>A lista com os dois resíduos.</returns>
        public List<int> Run(int number, int primeModule)
        {
            if (primeModule < 2)
            {
                throw new ArgumentException("The prime module must be a number greater than two.");
            }
            if (primeModule % 2 == 0)
            {
                throw new ArgumentException("The prime module must be an even number.");
            }
            else
            {
                var firstStepModule = primeModule - 1;
                var power = 0;
                while (firstStepModule % 2 == 0)
                {
                    ++power;
                    firstStepModule = firstStepModule / 2;
                }

                var modularIntegerField = new ModularIntegerField(primeModule);
                if (power == 1)
                {
                    var value = MathFunctions.Power(number, (primeModule + 1) / 4, modularIntegerField);
                    var result = new List<int>() { value, primeModule - value };
                    return result;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
