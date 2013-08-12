using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.AlgebraicStructures.Implementations
{
    public class UnivarPolynomEuclideanDomain<CoeffType, FieldType> : 
        UnivarPolynomRing<CoeffType, FieldType>, 
        IEuclidenDomain<UnivariatePolynomialNormalForm<CoeffType, FieldType>>
        where FieldType : IField<CoeffType>
    {
        private FieldType field;

        public UnivarPolynomEuclideanDomain(string variableName, FieldType field)
            : base(variableName, field)
        {
            this.field = field;
        }

        public UnivariatePolynomialNormalForm<CoeffType, FieldType> Quo(
            UnivariatePolynomialNormalForm<CoeffType, FieldType> dividend, 
            UnivariatePolynomialNormalForm<CoeffType, FieldType> divisor)
        {
            if (dividend == null)
            {
                throw new ArgumentNullException("dividend");
            }
            else if (divisor == null)
            {
                throw new ArgumentNullException("divisor");
            }
            else if (divisor.VariableName != divisor.VariableName)
            {
                throw new ArgumentException("Polynomials must share the same variable name in order to be operated.");
            }
            else
            {
                if (divisor.Degree > dividend.Degree)
                {
                    return new UnivariatePolynomialNormalForm<CoeffType, FieldType>(
                        this.variableName,
                        this.field);
                }
                else
                {
                    var dividendSortedCoeffs = dividend.GetOrderedCoefficients(Comparer<int>.Default);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(Comparer<int>.Default);
                    throw new NotImplementedException();
                }
            }
        }

        public UnivariatePolynomialNormalForm<CoeffType, FieldType> Rem(
            UnivariatePolynomialNormalForm<CoeffType, FieldType> dividend, 
            UnivariatePolynomialNormalForm<CoeffType, FieldType> divisor)
        {
            if (dividend == null)
            {
                throw new ArgumentNullException("dividend");
            }
            else if (divisor == null)
            {
                throw new ArgumentNullException("divisor");
            }
            else if (divisor.VariableName != divisor.VariableName)
            {
                throw new ArgumentException("Polynomials must share the same variable name in order to be operated.");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public DomainResult<UnivariatePolynomialNormalForm<CoeffType, FieldType>> GetQuotientAndRemainder(
            UnivariatePolynomialNormalForm<CoeffType, FieldType> dividend, 
            UnivariatePolynomialNormalForm<CoeffType, FieldType> divisor)
        {
            if (dividend == null)
            {
                throw new ArgumentNullException("dividend");
            }
            else if (divisor == null)
            {
                throw new ArgumentNullException("divisor");
            }
            else if (divisor.VariableName != divisor.VariableName)
            {
                throw new ArgumentException("Polynomials must share the same variable name in order to be operated.");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public uint Degree(UnivariatePolynomialNormalForm<CoeffType, FieldType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return (uint)value.Degree;
            }
        }
    }
}
