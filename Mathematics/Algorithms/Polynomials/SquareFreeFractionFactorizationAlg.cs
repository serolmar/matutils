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
        : IAlgorithm<UnivariatePolynomialNormalForm<Fraction<CoeffType, IIntegerNumber<CoeffType>>>,
                     SquareFreeFactorizationResult<Fraction<CoeffType, IIntegerNumber<CoeffType>>, CoeffType>>
    {
        /// <summary>
        /// O objecto responsável pelas operações sobre inteiros.
        /// </summary>
        private IIntegerNumber<CoeffType> integerNumber;

        /// <summary>
        /// O corpo responsável pelas operações sobre as fracções.
        /// </summary>
        private IField<Fraction<CoeffType, IIntegerNumber<CoeffType>>> fractionField;

        public SquareFreeFractionFactorizationAlg(
            IIntegerNumber<CoeffType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.fractionField = new FractionField<CoeffType, IIntegerNumber<CoeffType>>(this.integerNumber);
            }
        }

        /// <summary>
        /// Executa o algoritmo que permite obter uma factorização livre de quadrados.
        /// </summary>
        /// <param name="polynomial">O polinómio de entrada.</param>
        /// <returns>A factorização livre de quadrados.</returns>
        public SquareFreeFactorizationResult<Fraction<CoeffType, IIntegerNumber<CoeffType>>, CoeffType> Run(
            UnivariatePolynomialNormalForm<Fraction<CoeffType, IIntegerNumber<CoeffType>>> polynomial)
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
                var polynomDomain = new UnivarPolynomEuclideanDomain<Fraction<CoeffType, IIntegerNumber<CoeffType>>>(
                    polynomial.VariableName, 
                    this.fractionField);

                var lagAlg = new LagrangeAlgorithm<CoeffType>(this.integerNumber);
                var dataDerivative = polynomial.GetPolynomialDerivative(this.fractionField);
                var gcd = this.GreatCommonDivisor(polynomial, dataDerivative, polynomDomain);
                if (gcd.Degree == 0)
                {
                    var lcm = this.GetDenominatorLcm(polynomial, lagAlg);
                    if (this.integerNumber.IsMultiplicativeUnity(lcm))
                    {
                        var integerPol = this.GetIntegerPol(polynomial);
                        result.Add(currentDegree, integerPol);
                    }
                    else
                    {
                        independentCoeff = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                            this.integerNumber.MultiplicativeUnity,
                            lcm,
                            this.integerNumber);
                        var multipliable = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                            lcm,
                            this.integerNumber.MultiplicativeUnity,
                            this.integerNumber);
                        var multipliedPol = polynomial.Multiply(independentCoeff, this.fractionField);
                        var integerPol = this.GetIntegerPol(multipliedPol);
                        result.Add(currentDegree, integerPol);
                    }
                }
                else
                {
                    var polyCoffactor = polynomDomain.Quo(polynomial, gcd);
                    var nextGcd = this.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain);
                    var squareFreeFactor = polynomDomain.Quo(polyCoffactor, nextGcd);
                    polyCoffactor = gcd;
                    gcd = nextGcd;
                    if (squareFreeFactor.Degree > 0)
                    {
                        var lcm = this.GetDenominatorLcm(squareFreeFactor, lagAlg);
                        if (this.integerNumber.IsMultiplicativeUnity(lcm))
                        {
                            var integerPol = this.GetIntegerPol(squareFreeFactor);
                            result.Add(currentDegree, integerPol);
                        }
                        else if (this.fractionField.IsAdditiveUnity(independentCoeff))
                        {
                            independentCoeff = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                            this.integerNumber.MultiplicativeUnity,
                            MathFunctions.Power(lcm, currentDegree, this.integerNumber),
                            this.integerNumber);
                            var multipliable = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                                lcm,
                                this.integerNumber.MultiplicativeUnity,
                                this.integerNumber);
                            var multipliedPol = squareFreeFactor.Multiply(independentCoeff, this.fractionField);
                            var integerPol = this.GetIntegerPol(multipliedPol);
                            result.Add(currentDegree, integerPol);
                        }
                        else
                        {
                            var multiplicationCoeff = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                            this.integerNumber.MultiplicativeUnity,
                            MathFunctions.Power( lcm, currentDegree, this.integerNumber),
                            this.integerNumber);
                            independentCoeff = this.fractionField.Multiply(independentCoeff, multiplicationCoeff);
                            var multipliable = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                                lcm,
                                this.integerNumber.MultiplicativeUnity,
                                this.integerNumber);
                            var multipliedPol = squareFreeFactor.Multiply(independentCoeff, this.fractionField);
                            var integerPol = this.GetIntegerPol(multipliedPol);
                            result.Add(currentDegree, integerPol);
                        }
                    }
                    else
                    {
                        var value = squareFreeFactor.GetAsValue(this.fractionField);
                        if (!this.fractionField.IsMultiplicativeUnity(value))
                        {
                            if (this.fractionField.IsMultiplicativeUnity(independentCoeff))
                            {
                                independentCoeff = value;
                            }
                            else
                            {
                                independentCoeff = this.fractionField.Multiply(independentCoeff, value);
                            }
                        }
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
                            var lcm = this.GetDenominatorLcm(squareFreeFactor, lagAlg);
                            if (this.integerNumber.IsMultiplicativeUnity(lcm))
                            {
                                var integerPol = this.GetIntegerPol(squareFreeFactor);
                                result.Add(currentDegree, integerPol);
                            }
                            else if (this.fractionField.IsAdditiveUnity(independentCoeff))
                            {
                                independentCoeff = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                                this.integerNumber.MultiplicativeUnity,
                                MathFunctions.Power(lcm, currentDegree, this.integerNumber),
                                this.integerNumber);
                                var multipliable = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                                    lcm,
                                    this.integerNumber.MultiplicativeUnity,
                                    this.integerNumber);
                                var multipliedPol = squareFreeFactor.Multiply(independentCoeff, this.fractionField);
                                var integerPol = this.GetIntegerPol(multipliedPol);
                                result.Add(currentDegree, integerPol);
                            }
                            else
                            {
                                var multiplicationCoeff = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                                this.integerNumber.MultiplicativeUnity,
                                MathFunctions.Power(lcm, currentDegree, this.integerNumber),
                                this.integerNumber);
                                independentCoeff = this.fractionField.Multiply(independentCoeff, multiplicationCoeff);
                                var multipliable = new Fraction<CoeffType, IIntegerNumber<CoeffType>>(
                                    lcm,
                                    this.integerNumber.MultiplicativeUnity,
                                    this.integerNumber);
                                var multipliedPol = squareFreeFactor.Multiply(independentCoeff, this.fractionField);
                                var integerPol = this.GetIntegerPol(multipliedPol);
                                result.Add(currentDegree, integerPol);
                            }
                        }
                        else
                        {
                            var value = squareFreeFactor.GetAsValue(this.fractionField);
                            if (!this.fractionField.IsMultiplicativeUnity(value))
                            {
                                if (this.fractionField.IsMultiplicativeUnity(independentCoeff))
                                {
                                    independentCoeff = value;
                                }
                                else
                                {
                                    independentCoeff = this.fractionField.Multiply(independentCoeff, value);
                                }
                            }
                        }

                        ++currentDegree;
                    }

                    var cofactorValue = polyCoffactor.GetAsValue(this.fractionField);
                    if (!this.fractionField.IsMultiplicativeUnity(cofactorValue))
                    {
                        if (this.fractionField.IsMultiplicativeUnity(independentCoeff))
                        {
                            independentCoeff = cofactorValue;
                        }
                        else
                        {
                            independentCoeff = this.fractionField.Multiply(independentCoeff, cofactorValue);
                        }
                    }
                }

                return new SquareFreeFactorizationResult<Fraction<CoeffType, IIntegerNumber<CoeffType>>, CoeffType>(
                    independentCoeff,
                    result);
            }
        }

        private UnivariatePolynomialNormalForm<Fraction<CoeffType, IIntegerNumber<CoeffType>>> GreatCommonDivisor(
            UnivariatePolynomialNormalForm<Fraction<CoeffType, IIntegerNumber<CoeffType>>> first,
            UnivariatePolynomialNormalForm<Fraction<CoeffType, IIntegerNumber<CoeffType>>> second,
            UnivarPolynomEuclideanDomain<Fraction<CoeffType, IIntegerNumber<CoeffType>>> polynomDomain
            )
        {
            var result = MathFunctions.GreatCommonDivisor(first, second, polynomDomain);
            var leadingCoeff = result.GetLeadingCoefficient(this.fractionField);
            result = result.Multiply(
                this.fractionField.MultiplicativeInverse(leadingCoeff), 
                this.fractionField);
            return result;
        }

        /// <summary>
        /// Obtém o mínimo múltiplo comum entre os denominadores do polinómio.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="gcdCAlg">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O valor do mínimo múltiplo comum.</returns>
        private CoeffType GetDenominatorLcm(
            UnivariatePolynomialNormalForm<Fraction<CoeffType, IIntegerNumber<CoeffType>>> polynom,
            IBachetBezoutAlgorithm<CoeffType> gcdCAlg)
        {
            var termsEnumerator = polynom.GetEnumerator();
            var state = termsEnumerator.MoveNext();
            if (state)
            {
                var coeff = termsEnumerator.Current.Value.Denominator;
                state = termsEnumerator.MoveNext();
                while (state && gcdCAlg.Domain.IsMultiplicativeUnity(coeff))
                {
                    coeff = termsEnumerator.Current.Value.Denominator;
                    state = termsEnumerator.MoveNext();
                }

                while (state)
                {
                    var current = termsEnumerator.Current.Value.Denominator;
                    if (!gcdCAlg.Domain.IsMultiplicativeUnity(current))
                    {
                        var status = gcdCAlg.Run(coeff, current);
                        coeff = gcdCAlg.Domain.Multiply(status.FirstItem, status.SecondCofactor);
                    }

                    state = termsEnumerator.MoveNext();
                }

                return coeff;
            }
            else
            {
                return gcdCAlg.Domain.MultiplicativeUnity;
            }
        }

        /// <summary>
        /// Obtém o polinómio convertido para um polinómio com coeficientes inteiros.
        /// </summary>
        /// <param name="polynomial">O polinómio a ser convertido.</param>
        /// <returns>O polinómio convertido.</returns>
        private UnivariatePolynomialNormalForm<CoeffType> GetIntegerPol(
            UnivariatePolynomialNormalForm<Fraction<CoeffType, IIntegerNumber<CoeffType>>> polynomial)
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType>(polynomial.VariableName);
            foreach (var term in polynomial)
            {
                result.Add(term.Value.Numerator, term.Key, this.integerNumber);
            }

            return result;
        }
    }
}
