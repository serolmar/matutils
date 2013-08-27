namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo rho de Pollard para factorizar um número.
    /// </summary>
    public class PollardRhoAlgorithm :
        IAlgorithm<int, Tuple<int, int>>, IAlgorithm<int, int, Tuple<int, int>>
    {
        /// <summary>
        /// Mantém uma lista de polinómios a serem considerados no algoritmo.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<int, IntegerDomain>> polynomialsList;

        /// <summary>
        /// O domínio responsável pelas operações sobre os números inteiros.
        /// </summary>
        private IntegerDomain integerDomain = new IntegerDomain();

        /// <summary>
        /// O valor inicial para a execução do algoritmo.
        /// </summary>
        private int startValue = 19;

        public PollardRhoAlgorithm()
        {
            this.SetupPolynomialList();
        }

        public PollardRhoAlgorithm(List<UnivariatePolynomialNormalForm<int, IntegerDomain>> testPolynomials)
        {
            if (testPolynomials == null || testPolynomials.Count == 0)
            {
                this.SetupPolynomialList();
            }
            else
            {
                this.polynomialsList = new List<UnivariatePolynomialNormalForm<int, IntegerDomain>>();
                this.polynomialsList.AddRange(testPolynomials);
            }
        }

        /// <summary>
        /// Obtém e atribui o valor inicial para o algoritmo.
        /// </summary>
        public int StartValue
        {
            get
            {
                return this.startValue;
            }
            set
            {
                this.startValue = value;
            }
        }

        /// <summary>
        /// Obtém a decomposição do módulo do número especificado num produto de factores.
        /// </summary>
        /// <remarks>
        /// A decomposição poderá consistir no produto do número especificado pela unidade. Neste caso,
        /// não é garantida a primalidade do número. Normalmente, é escolhido outro polinómio como gerador da
        /// sequência pseudo-aleatória.
        /// </remarks>
        /// <param name="number">O número especificado.</param>
        /// <returns>A decomposição do produto.</returns>
        public Tuple<int, int> Run(int number)
        {
            var innerNumber = Math.Abs(number);
            if (innerNumber == 0)
            {
                throw new MathematicsException("Zero has no factors.");
            }
            else if (innerNumber == 1)
            {
                return Tuple.Create(1, 1);
            }
            else
            {
                var modularRing = new ModularIntegerField(number);
                var innerStartValue = modularRing.GetReduced(this.startValue);
                
                foreach (var polynomial in this.polynomialsList)
                {
                    var firstSequenceValue = modularRing.GetReduced(this.startValue);
                    var secondSequenceValue = modularRing.GetReduced(this.startValue);
                    var gcd = 1;
                    do
                    {
                        firstSequenceValue = polynomial.Replace(firstSequenceValue, modularRing);
                        var secondSequenceTemp = polynomial.Replace(secondSequenceValue, modularRing);
                        secondSequenceValue = polynomial.Replace(secondSequenceTemp, modularRing);
                        if (firstSequenceValue == secondSequenceValue)
                        {
                            gcd = 0;
                        }
                        else
                        {
                            gcd = MathFunctions.GreatCommonDivisor(
                                innerNumber,
                                Math.Abs(firstSequenceValue - secondSequenceValue),
                                this.integerDomain);
                        }
                    } while (gcd == 1);

                    if (gcd != 0)
                    {
                        return Tuple.Create(gcd, innerNumber / gcd);
                    }
                }

                return Tuple.Create(innerNumber, 1);
            }
        }

        /// <summary>
        /// Obtém a decomposição do módulo do número especificado num produto de factores.
        /// </summary>
        /// <remarks>
        /// A decomposição poderá consistir no produto do número especificado pela unidade. Neste caso,
        /// não é garantida a primalidade do número. Normalmente, é escolhido outro polinómio como gerador da
        /// sequência pseudo-aleatória.
        /// </remarks>
        /// <param name="number">O número especificado.</param>
        /// <param name="block">
        /// A quantidade de factores bloqueados antes de se proceder à determinação do
        /// máximo divisor comum.
        /// </param>
        /// <returns>A decomposição do número em factores.</returns>
        public Tuple<int, int> Run(int number, int block)
        {
            var innerNumber = Math.Abs(number);
            if (block <= 0)
            {
                throw new ArgumentException("Blocks number must be a positive integer.");
            }
            else if (innerNumber == 0)
            {
                throw new MathematicsException("Zero has no factors.");
            }
            else if (innerNumber == 1)
            {
                return Tuple.Create(1, 1);
            }
            else
            {
                var modularRing = new ModularIntegerField(number);
                var innerStartValue = modularRing.GetReduced(this.startValue);

                foreach (var polynomial in this.polynomialsList)
                {
                    var firstSequenceValue = modularRing.GetReduced(this.startValue);
                    var secondSequenceValue = modularRing.GetReduced(this.startValue);
                    var gcd = 1;
                    do
                    {
                        var initialFirstSequence = firstSequenceValue;
                        var initialSecondSequence = secondSequenceValue;
                        var blocksNumber = 0;
                        var blockProduct = 1;
                        while (blocksNumber < block && blockProduct != 0)
                        {
                            firstSequenceValue = polynomial.Replace(firstSequenceValue, modularRing);
                            var secondSequenceTemp = polynomial.Replace(secondSequenceValue, modularRing);
                            secondSequenceValue = polynomial.Replace(secondSequenceTemp, modularRing);
                            blockProduct = (blockProduct * Math.Abs(firstSequenceValue - secondSequenceValue)) %
                                innerNumber;
                        }

                        if (blockProduct == 0)
                        {
                            gcd = 0;
                        }
                        else
                        {
                            gcd = MathFunctions.GreatCommonDivisor(blockProduct, innerNumber, this.integerDomain);
                        }

                        if (gcd == 0)
                        {
                            gcd = 1;
                            while (initialFirstSequence != firstSequenceValue &&
                                initialSecondSequence != secondSequenceValue &&
                                gcd == 1)
                            {
                                initialFirstSequence = polynomial.Replace(initialFirstSequence, modularRing);
                                var secondSequenceTemp = polynomial.Replace(initialSecondSequence, modularRing);
                                initialSecondSequence = polynomial.Replace(secondSequenceTemp, modularRing);
                                if (initialFirstSequence == initialSecondSequence)
                                {
                                    gcd = 0;
                                }
                                else
                                {
                                    gcd = MathFunctions.GreatCommonDivisor(
                                        innerNumber,
                                        Math.Abs(initialFirstSequence - initialSecondSequence),
                                        this.integerDomain);
                                }
                            }
                        }

                    } while (gcd == 1);

                    if (gcd != innerNumber)
                    {
                        return Tuple.Create(gcd, innerNumber / gcd);
                    }
                }
            }

            return Tuple.Create(innerNumber, 1);
        }

        /// <summary>
        /// Estabelece o polinómio de teste que será usado por defeito.
        /// </summary>
        private void SetupPolynomialList()
        {
            this.polynomialsList = new List<UnivariatePolynomialNormalForm<int, IntegerDomain>>();
            var polynomial = new UnivariatePolynomialNormalForm<int, IntegerDomain>(1, 0, "x", this.integerDomain);
            polynomial = polynomial.Add(new UnivariatePolynomialNormalForm<int, IntegerDomain>(1, 2, "x", this.integerDomain));
            this.polynomialsList.Add(polynomial);
        }
    }
}
