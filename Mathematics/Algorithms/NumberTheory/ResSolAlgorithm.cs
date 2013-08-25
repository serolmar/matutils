namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo de Tonelli-Shanks.
    /// </summary>
    public class ResSolAlgorithm : IAlgorithm<int, int, List<int>>
    {
        private IAlgorithm<int, int, int> legendreJacobiSymAlg;

        /// <summary>
        /// Instancia um objecto do tipo <see cref="ResSolAlgorithm"/>.
        /// </summary>
        public ResSolAlgorithm() : this(null) { }

        /// <summary>
        /// Instancia um objecto do tipo <see cref="ResSolAlgorithm"/>.
        /// </summary>
        /// <param name="legendreJacobiSymbAlg">
        /// Um objecto que implemente o algoritmo que permite determinar o valor do símbolo
        /// de Legendre-Jacobi.
        /// </param>
        public ResSolAlgorithm(IAlgorithm<int, int, int> legendreJacobiSymbAlg)
        {
            if (legendreJacobiSymbAlg == null)
            {
                this.legendreJacobiSymAlg = new LegendreJacobiSymbolAlgorithm();
            }
            else
            {
                this.legendreJacobiSymAlg = legendreJacobiSymbAlg;
            }
        }

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
            else if (this.legendreJacobiSymAlg.Run(number, primeModule) != 1)
            {
                throw new ArgumentException("Solution doesn't exist.");
            }
            else
            {
                var innerNumber = number % primeModule;
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
                    var nonQuadraticResidue = this.FindNonQuadraticResidue(primeModule);
                    var poweredNonQuadraticResult = MathFunctions.Power(
                        nonQuadraticResidue,
                        firstStepModule,
                        modularIntegerField);
                    var result = MathFunctions.Power(innerNumber, (firstStepModule + 1) / 2, modularIntegerField);
                    var temp = MathFunctions.Power(innerNumber, firstStepModule, modularIntegerField);
                    while (temp != 1)
                    {
                        var lowestIndex = this.FindLowestIndex(temp, power, primeModule);
                        var aux = this.SquareValue(
                            poweredNonQuadraticResult,
                            power - lowestIndex - 1,
                            primeModule);
                        result = result * aux % primeModule;
                        aux = (aux * aux) % primeModule;
                        temp = (temp * aux) % primeModule;
                        poweredNonQuadraticResult = aux;
                        power = lowestIndex;
                    }

                    return new List<int>() { result, primeModule - result };
                }
            }
        }

        /// <summary>
        /// Encontra um não resíduo quadrático cuja existência é matematicamente garantida.
        /// </summary>
        /// <returns>O não resíduo quadrático.</returns>
        private int FindNonQuadraticResidue(int primeModule)
        {
            var result = 0;
            for (int i = 2; i < primeModule; ++i)
            {
                var legendreSymbol = this.legendreJacobiSymAlg.Run(i, primeModule);
                if (legendreSymbol == -1)
                {
                    result = i;
                    i = primeModule;
                }
            }

            return result;
        }

        /// <summary>
        /// Tenta encontrar o índice i tal que temp^(2^i) seja congruente com a unidade.
        /// </summary>
        /// <param name="temp">O valor de temp.</param>
        /// <param name="upperLimit">O valor do limite superior.</param>
        /// <param name="primeModule">O módulo.</param>
        /// <returns>O índice procurado.</returns>
        private int FindLowestIndex(int temp, int upperLimit, int primeModule)
        {
            var result = -1;
            var innerTemp = temp;
            for (int i = 0; i < upperLimit; ++i)
            {
                if (innerTemp == 1)
                {
                    result = i;
                    i = upperLimit;
                }
                else
                {
                    innerTemp = (innerTemp * innerTemp) % primeModule;
                }
            }

            return result;
        }

        /// <summary>
        /// Quadra um valor um número especificado de vezes.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <param name="times">O número de vezes.</param>
        /// <param name="primeModule">O módulo.</param>
        /// <returns>O resultado.</returns>
        private int SquareValue(int value, int times, int primeModule)
        {
            var result = value;
            for (int i = 0; i < times; ++i)
            {
                result = (result * result) % primeModule;
            }

            return result;
        }
    }
}
