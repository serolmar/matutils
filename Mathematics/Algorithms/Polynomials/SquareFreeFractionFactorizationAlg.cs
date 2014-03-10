namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite aplicar o algoritmo de factorização livre de quadrados a um polinómio cujos coeficientes
    /// são fracções inteiras.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class SquareFreeFractionFactorizationAlg<CoeffType>
        : IAlgorithm<UnivariatePolynomialNormalForm<Fraction<CoeffType, IEuclidenDomain<CoeffType>>>,
                     SquareFreeFactorizationResult<Fraction<CoeffType, IEuclidenDomain<CoeffType>>, CoeffType>>
    {
        /// <summary>
        /// O objecto responsável pelas operações sobre inteiros.
        /// </summary>
        private IIntegerNumber<CoeffType> integerNumber;

        /// <summary>
        /// O corpo responsável pelas operações sobre as fracções.
        /// </summary>
        private IField<Fraction<CoeffType, IEuclidenDomain<CoeffType>>> fractionField;

        public SquareFreeFractionFactorizationAlg(
            IField<Fraction<CoeffType, IEuclidenDomain<CoeffType>>> fractionField,
            IIntegerNumber<CoeffType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (fractionField == null)
            {
                throw new ArgumentNullException("fractionField");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.fractionField = fractionField;
            }
        }

        /// <summary>
        /// Executa o algoritmo que permite obter uma factorização livre de quadrados.
        /// </summary>
        /// <param name="polynomial">O polinómio de entrada.</param>
        /// <returns>A factorização livre de quadrados.</returns>
        public SquareFreeFactorizationResult<Fraction<CoeffType, IEuclidenDomain<CoeffType>>, CoeffType> Run(
            UnivariatePolynomialNormalForm<Fraction<CoeffType, IEuclidenDomain<CoeffType>>> polynomial)
        {
            if (polynomial == null)
            {
                throw new ArgumentNullException("polynomial");
            }
            else
            {
                var independentCoeff = this.fractionField.MultiplicativeUnity;
                var result = new Dictionary<int, UnivariatePolynomialNormalForm<CoeffType>>();
                var currentDegree = 1;
                var polynomDomain = new UnivarPolynomEuclideanDomain<Fraction<CoeffType, IEuclidenDomain<CoeffType>>>(
                    polynomial.VariableName, 
                    this.fractionField);
                var dataDerivative = polynomial.GetPolynomialDerivative(this.fractionField);
                var gcd = MathFunctions.GreatCommonDivisor(polynomial, dataDerivative, polynomDomain);
                if (polynomDomain.IsMultiplicativeUnity(gcd))
                {
                    //result.Add(currentDegree, polynomial.Clone());
                }
                else
                {
                    var polyCoffactor = polynomDomain.Quo(polynomial, gcd);
                    var nextGcd = MathFunctions.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain);
                    var squareFreeFactor = polynomDomain.Quo(polyCoffactor, nextGcd);
                    polyCoffactor = gcd;
                    gcd = nextGcd;
                    if (squareFreeFactor.Degree > 0)
                    {
                        //result.Add(currentDegree, squareFreeFactor);
                    }

                    ++currentDegree;
                    while (gcd.Degree > 0)
                    {
                        polyCoffactor = polynomDomain.Quo(polyCoffactor, gcd);
                        nextGcd = MathFunctions.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain);
                        squareFreeFactor = polynomDomain.Quo(gcd, nextGcd);
                        gcd = nextGcd;
                        if (squareFreeFactor.Degree > 0)
                        {
                            //result.Add(currentDegree, squareFreeFactor);
                        }

                        ++currentDegree;
                    }
                }

                return new SquareFreeFactorizationResult<Fraction<CoeffType, IEuclidenDomain<CoeffType>>, CoeffType>(
                    independentCoeff,
                    result);
            }
        }
    }
}
