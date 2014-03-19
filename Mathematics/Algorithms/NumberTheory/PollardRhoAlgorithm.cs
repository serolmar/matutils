namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo rho de Pollard para factorizar um número.
    /// </summary>
    public class PollardRhoAlgorithm<NumberType> :
        IAlgorithm<NumberType, Tuple<NumberType, NumberType>>,
        IAlgorithm<NumberType, NumberType, Tuple<NumberType, NumberType>>
    {
        /// <summary>
        /// Mantém uma lista de polinómios a serem considerados no algoritmo.
        /// </summary>
        private List<UnivariatePolynomialNormalForm<NumberType>> polynomialsList;

        /// <summary>
        /// O domínio responsável pelas operações sobre os números inteiros.
        /// </summary>
        private IIntegerNumber<NumberType> integerNumber;

        /// <summary>
        /// Permite criar instâncias de um corpo modular.
        /// </summary>
        private IModularFieldFactory<NumberType> modularFieldFactory;

        /// <summary>
        /// O valor inicial para a execução do algoritmo.
        /// </summary>
        private NumberType startValue;

        /// <summary>
        /// Permite criar uma instância dum objecto responsável pela obtenção de factores de um número.
        /// </summary>
        /// <param name="modularFieldFactory">O objecto responsável pelas operações modulares.</param>
        /// <param name="integerDomain">O objecto responsável pelas operações sobre números inteiros.</param>
        public PollardRhoAlgorithm(
            IModularFieldFactory<NumberType> modularFieldFactory,
            IIntegerNumber<NumberType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (modularFieldFactory == null)
            {
                throw new ArgumentNullException("modularFieldFactory");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.modularFieldFactory = modularFieldFactory;
                this.startValue = this.integerNumber.MapFrom(19);
                this.SetupPolynomialList();
            }
        }

        public PollardRhoAlgorithm(
            List<UnivariatePolynomialNormalForm<NumberType>> testPolynomials,
            IModularFieldFactory<NumberType> modularFieldFactory,
            IIntegerNumber<NumberType> integerNumber) : this(modularFieldFactory, integerNumber)
        {
            if (testPolynomials == null || testPolynomials.Count == 0)
            {
                this.SetupPolynomialList();
            }
            else
            {
                this.polynomialsList = new List<UnivariatePolynomialNormalForm<NumberType>>();
                this.polynomialsList.AddRange(testPolynomials);
            }
        }

        /// <summary>
        /// Obtém e atribui o valor inicial para o algoritmo.
        /// </summary>
        public NumberType StartValue
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
        public Tuple<NumberType, NumberType> Run(NumberType number)
        {
            var innerNumber = this.integerNumber.GetNorm(number);
            if (this.integerNumber.IsAdditiveUnity(innerNumber))
            {
                throw new MathematicsException("Zero has no factors.");
            }
            else if (this.integerNumber.IsMultiplicativeUnity(innerNumber))
            {
                return Tuple.Create(
                    this.integerNumber.MultiplicativeUnity,
                    this.integerNumber.MultiplicativeUnity);
            }
            else
            {
                var modularRing = this.modularFieldFactory.CreateInstance(number);
                var innerStartValue = modularRing.GetReduced(this.startValue);

                foreach (var polynomial in this.polynomialsList)
                {
                    var firstSequenceValue = modularRing.GetReduced(this.startValue);
                    var secondSequenceValue = modularRing.GetReduced(this.startValue);
                    var gcd = this.integerNumber.MultiplicativeUnity;
                    do
                    {
                        firstSequenceValue = polynomial.Replace(firstSequenceValue, modularRing);
                        var secondSequenceTemp = polynomial.Replace(secondSequenceValue, modularRing);
                        secondSequenceValue = polynomial.Replace(secondSequenceTemp, modularRing);
                        if (this.integerNumber.Equals(firstSequenceValue , secondSequenceValue))
                        {
                            gcd = this.integerNumber.AdditiveUnity;
                        }
                        else
                        {
                            gcd = MathFunctions.GreatCommonDivisor(
                                innerNumber,
                                this.integerNumber.GetNorm(
                                this.integerNumber.Add(
                                firstSequenceValue,
                                this.integerNumber.AdditiveInverse(secondSequenceValue))),
                                this.integerNumber);
                        }
                    } while (this.integerNumber.IsMultiplicativeUnity(gcd));

                    if (!this.integerNumber.IsAdditiveUnity(gcd))
                    {
                        return Tuple.Create(gcd, this.integerNumber.Quo(innerNumber, gcd));
                    }
                }

                return Tuple.Create(innerNumber, this.integerNumber.MultiplicativeUnity);
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
        public Tuple<NumberType, NumberType> Run(NumberType number, NumberType block)
        {
            var innerNumber = this.integerNumber.GetNorm(number);
            if (this.integerNumber.Compare(block, this.integerNumber.AdditiveUnity) <= 0)
            {
                throw new ArgumentException("Blocks number must be a positive integer.");
            }
            else if (this.integerNumber.IsAdditiveUnity(innerNumber))
            {
                throw new MathematicsException("Zero has no factors.");
            }
            else if (this.integerNumber.IsMultiplicativeUnity(innerNumber))
            {
                return Tuple.Create(
                    this.integerNumber.MultiplicativeUnity,
                    this.integerNumber.MultiplicativeUnity);
            }
            else
            {
                var modularRing = this.modularFieldFactory.CreateInstance(innerNumber);
                var innerStartValue = modularRing.GetReduced(this.startValue);

                foreach (var polynomial in this.polynomialsList)
                {
                    var firstSequenceValue = modularRing.GetReduced(this.startValue);
                    var secondSequenceValue = modularRing.GetReduced(this.startValue);
                    var gcd = this.integerNumber.MultiplicativeUnity;
                    do
                    {
                        var initialFirstSequence = firstSequenceValue;
                        var initialSecondSequence = secondSequenceValue;
                        var blocksNumber = this.integerNumber.AdditiveUnity;
                        var blockProduct = this.integerNumber.MultiplicativeUnity;
                        while (this.integerNumber.Compare(blocksNumber,block )<0 && 
                            !this.integerNumber.IsAdditiveUnity(blockProduct ))
                        {
                            firstSequenceValue = polynomial.Replace(firstSequenceValue, modularRing);
                            var secondSequenceTemp = polynomial.Replace(secondSequenceValue, modularRing);
                            secondSequenceValue = polynomial.Replace(secondSequenceTemp, modularRing);
                            var temp = this.integerNumber.GetNorm(
                                this.integerNumber.Add(
                                firstSequenceValue,
                                this.integerNumber.AdditiveInverse(secondSequenceValue)));
                            blockProduct = modularRing.Multiply(blockProduct, temp);
                            blocksNumber = this.integerNumber.Successor(blocksNumber);
                        }

                        if (this.integerNumber.IsAdditiveUnity(blockProduct))
                        {
                            gcd = this.integerNumber.AdditiveUnity;
                        }
                        else
                        {
                            gcd = MathFunctions.GreatCommonDivisor(blockProduct, innerNumber, this.integerNumber);
                        }

                        if (this.integerNumber.IsAdditiveUnity(gcd))
                        {
                            gcd = this.integerNumber.MultiplicativeUnity;
                            while (!this.integerNumber.Equals(initialFirstSequence, firstSequenceValue) &&
                                !this.integerNumber.Equals(initialSecondSequence, secondSequenceValue) &&
                                this.integerNumber.IsMultiplicativeUnity(gcd))
                            {
                                initialFirstSequence = polynomial.Replace(initialFirstSequence, modularRing);
                                var secondSequenceTemp = polynomial.Replace(initialSecondSequence, modularRing);
                                initialSecondSequence = polynomial.Replace(secondSequenceTemp, modularRing);
                                if (this.integerNumber.Equals(initialFirstSequence , initialSecondSequence))
                                {
                                    gcd = this.integerNumber.AdditiveUnity;
                                }
                                else
                                {
                                    gcd = MathFunctions.GreatCommonDivisor(
                                        innerNumber,
                                        this.integerNumber.GetNorm(
                                            this.integerNumber.Add(
                                                firstSequenceValue,
                                                this.integerNumber.AdditiveInverse(secondSequenceValue))),
                                        this.integerNumber);
                                }
                            }
                        }

                    } while (this.integerNumber.IsAdditiveUnity(gcd));

                    if (!this.integerNumber.Equals(gcd, innerNumber) && !this.integerNumber.IsAdditiveUnity(gcd))
                    {
                        return Tuple.Create(gcd, this.integerNumber.Quo(innerNumber, gcd));
                    }
                }
            }

            return Tuple.Create(innerNumber, this.integerNumber.MultiplicativeUnity);
        }

        /// <summary>
        /// Estabelece o polinómio de teste que será usado por defeito.
        /// </summary>
        private void SetupPolynomialList()
        {
            this.polynomialsList = new List<UnivariatePolynomialNormalForm<NumberType>>();
            var polynomial = new UnivariatePolynomialNormalForm<NumberType>(
                this.integerNumber.MultiplicativeUnity,
                0,
                "x",
                this.integerNumber);
            polynomial = polynomial.Add(new UnivariatePolynomialNormalForm<NumberType>(
                this.integerNumber.MultiplicativeUnity,
                2,
                "x",
                this.integerNumber), this.integerNumber);
            this.polynomialsList.Add(polynomial);
        }
    }
}
