namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite aplicar o algoritmo da factorização livre de quadrados a um polinómio cujos coeficientes
    /// são elementos de um corpo arbitrário.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class UnivarSquareFreeDecomposition<CoeffType> :
        IAlgorithm<UnivariatePolynomialNormalForm<CoeffType>,
        IField<CoeffType>,
        SquareFreeFactorizationResult<CoeffType, CoeffType>>
    {
        /// <summary>
        /// Aplica o algoritmo de factorização livre de quadrados a um polinómio.
        /// </summary>
        /// <param name="data">O polinómio.</param>
        /// <param name="field">O corpos responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado da factorização livre de quadrados.</returns>
        /// <exception cref="System.ArgumentNullException">Caso algum dos argumentos seja nulo.</exception>
        public SquareFreeFactorizationResult<CoeffType, CoeffType> Run(
            UnivariatePolynomialNormalForm<CoeffType> data,
            IField<CoeffType> field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var independentCoeff = field.MultiplicativeUnity;
                var result = new Dictionary<int, UnivariatePolynomialNormalForm<CoeffType>>();
                var currentDegree = 1;
                var polynomDomain = new UnivarPolynomEuclideanDomain<CoeffType>(data.VariableName, field);
                var dataDerivative = data.GetPolynomialDerivative(field);
                var gcd = this.GreatCommonDivisor(data, dataDerivative, polynomDomain, field);
                if (polynomDomain.IsMultiplicativeUnity(gcd))
                {
                    result.Add(currentDegree, data.Clone());
                }
                else
                {
                    var polyCoffactor = polynomDomain.Quo(data, gcd);
                    var nextGcd = this.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain, field);
                    var squareFreeFactor = polynomDomain.Quo(polyCoffactor, nextGcd);
                    polyCoffactor = gcd;
                    gcd = nextGcd;
                    if (squareFreeFactor.Degree > 0)
                    {
                        result.Add(currentDegree, squareFreeFactor);
                    }
                    else
                    {
                        var value = squareFreeFactor.GetAsValue(field);
                        if (!field.IsMultiplicativeUnity(value))
                        {
                            if (field.IsMultiplicativeUnity(independentCoeff))
                            {
                                independentCoeff = value;
                            }
                            else
                            {
                                independentCoeff = field.Multiply(independentCoeff, value);
                            }
                        }
                    }

                    ++currentDegree;
                    while (gcd.Degree > 0)
                    {
                        polyCoffactor = polynomDomain.Quo(polyCoffactor, gcd);
                        nextGcd = this.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain, field);
                        squareFreeFactor = polynomDomain.Quo(gcd, nextGcd);
                        gcd = nextGcd;
                        if (squareFreeFactor.Degree > 0)
                        {
                            result.Add(currentDegree, squareFreeFactor);
                        }
                        else
                        {
                            var value = squareFreeFactor.GetAsValue(field);
                            if (!field.IsMultiplicativeUnity(value))
                            {
                                if (field.IsMultiplicativeUnity(independentCoeff))
                                {
                                    independentCoeff = value;
                                }
                                else
                                {
                                    independentCoeff = field.Multiply(independentCoeff, value);
                                }
                            }
                        }

                        ++currentDegree;
                    }

                    var cofactorValue = polyCoffactor.GetAsValue(field);
                    if (!field.IsMultiplicativeUnity(cofactorValue))
                    {
                        if (field.IsMultiplicativeUnity(independentCoeff))
                        {
                            independentCoeff = cofactorValue;
                        }
                        else
                        {
                            independentCoeff = field.Multiply(independentCoeff, cofactorValue);
                        }
                    }
                }

                return new SquareFreeFactorizationResult<CoeffType,CoeffType>(
                    independentCoeff,
                    result);
            }
        }

        /// <summary>
        /// Calcula o máximo divisor comum entre dois polinómios.
        /// </summary>
        /// <param name="first">O primeiro polinómio.</param>
        /// <param name="second">O segundo polinómio.</param>
        /// <param name="polynomDomain">O domínio responsável pelas operações sobre os polinómios.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O máximo divisor comum.</returns>
        private UnivariatePolynomialNormalForm<CoeffType> GreatCommonDivisor(
            UnivariatePolynomialNormalForm<CoeffType> first,
            UnivariatePolynomialNormalForm<CoeffType> second,
            UnivarPolynomEuclideanDomain<CoeffType> polynomDomain,
            IField<CoeffType> field
            )
        {
            var result = MathFunctions.GreatCommonDivisor(first, second, polynomDomain);
            var leadingCoeff = result.GetLeadingCoefficient(field);
            result = result.Multiply(field.MultiplicativeInverse(leadingCoeff), field);
            return result;
        }
    }
}
