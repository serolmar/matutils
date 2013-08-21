namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnivarSquareFreeDecomposition<CoeffType, FieldType> :
        IAlgorithm<UnivariatePolynomialNormalForm<CoeffType, FieldType>,
        List<UnivariatePolynomialNormalForm<CoeffType, FieldType>>>
        where FieldType : IField<CoeffType>
    {
        public List<UnivariatePolynomialNormalForm<CoeffType, FieldType>> Run(
            UnivariatePolynomialNormalForm<CoeffType, FieldType> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var result = new List<UnivariatePolynomialNormalForm<CoeffType, FieldType>>();
                var polynomDomain = new UnivarPolynomEuclideanDomain<CoeffType, FieldType>(data.VariableName, data.Ring);
                var dataDerivative = data.GetPolynomialDerivative();
                var gcd = this.GreatCommonDivisor(data, dataDerivative, polynomDomain);
                if (polynomDomain.IsMultiplicativeUnity(gcd))
                {
                    result.Add(data.Clone());
                }
                else
                {
                    var polyCoffactor = polynomDomain.Quo(data, gcd);
                    gcd = this.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain);
                    var squareFreeFactor = polynomDomain.Quo(polyCoffactor, gcd);
                    result.Add(squareFreeFactor);
                    while (gcd.Degree > 0)
                    {
                        polyCoffactor = polynomDomain.Quo(polyCoffactor, gcd);
                        var nextGcd = this.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain);
                        squareFreeFactor = polynomDomain.Quo(gcd, nextGcd);
                        gcd = nextGcd;
                        result.Add(squareFreeFactor);
                    }
                }

                return result;
            }
        }

        private UnivariatePolynomialNormalForm<CoeffType, FieldType> GreatCommonDivisor(
            UnivariatePolynomialNormalForm<CoeffType, FieldType> first,
            UnivariatePolynomialNormalForm<CoeffType, FieldType> second,
            UnivarPolynomEuclideanDomain<CoeffType, FieldType> polynomDomain
            )
        {
            var result = MathFunctions.GreatCommonDivisor(first, second, polynomDomain);
            var leadingCoeff = result.GetLeadingCoefficient();
            result = result.Multiply(result.Ring.MultiplicativeInverse(leadingCoeff));
            return result;
        }
    }
}
