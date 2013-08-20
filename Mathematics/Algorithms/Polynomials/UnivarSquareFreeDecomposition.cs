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
                var gcd = MathFunctions.GreatCommonDivisor(data, dataDerivative, polynomDomain);
                if (polynomDomain.IsMultiplicativeUnity(gcd))
                {
                    result.Add(data.Clone());
                }
                else
                {
                    var polyCoffactor = polynomDomain.Quo(data, gcd);
                    gcd = MathFunctions.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain);
                    var squareFreeFactor = polynomDomain.Quo(polyCoffactor, gcd);
                    result.Add(squareFreeFactor);
                    while (gcd.Degree > 0)
                    {
                        polyCoffactor = polynomDomain.Quo(polyCoffactor, gcd);
                        var nextGcd = MathFunctions.GreatCommonDivisor(gcd, polyCoffactor, polynomDomain);
                        squareFreeFactor = polynomDomain.Quo(gcd, nextGcd);
                        gcd = nextGcd;
                        result.Add(squareFreeFactor);
                    }
                }

                return result;
            }
        }
    }
}
