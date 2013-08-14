namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

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
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(Comparer<int>.Default);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(Comparer<int>.Default);
                    var quotientCoeffs = new UnivariatePolynomialNormalForm<CoeffType, FieldType>(this.variableName, this.field);
                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (remainderLeadingDegree >= divisorLeadingDegree)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.field.Multiply(
                            remainderLeadingCoeff,
                            inverseDivisorLeadingCoeff);
                        quotientCoeffs = quotientCoeffs.Add(factor, differenceDegree);
                        remainderSortedCoeffs.Remove(remainderLeadingDegree);
                        for (int i = 0; i < divisorSorteCoeffs.Keys.Count - 1; ++i)
                        {
                            var currentDivisorDegree = divisorSorteCoeffs.Keys[i];
                            var currentCoeff = this.field.Multiply(
                                divisorSorteCoeffs[currentDivisorDegree],
                                factor);
                            currentDivisorDegree += differenceDegree;
                            var addCoeff = default(CoeffType);
                            if (remainderSortedCoeffs.TryGetValue(currentDivisorDegree, out addCoeff))
                            {
                                addCoeff = this.field.Add(
                                    addCoeff,
                                    this.field.AdditiveInverse(currentCoeff));
                                if (this.field.IsAdditiveUnity(addCoeff))
                                {
                                    remainderSortedCoeffs.Remove(currentDivisorDegree);
                                }
                                else
                                {
                                    remainderSortedCoeffs[currentDivisorDegree] = addCoeff;
                                }
                            }
                            else
                            {
                                remainderSortedCoeffs.Add(
                                    currentDivisorDegree,
                                    this.field.AdditiveInverse(currentCoeff));
                            }
                        }

                        remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    }

                    return quotientCoeffs;
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
                if (divisor.Degree > dividend.Degree)
                {
                    return dividend.Clone();
                }
                else
                {
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(Comparer<int>.Default);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(Comparer<int>.Default);
                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (remainderLeadingDegree >= divisorLeadingDegree)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.field.Multiply(
                            remainderLeadingCoeff,
                            inverseDivisorLeadingCoeff);
                        remainderSortedCoeffs.Remove(remainderLeadingDegree);
                        for (int i = 0; i < divisorSorteCoeffs.Keys.Count - 1; ++i)
                        {
                            var currentDivisorDegree = divisorSorteCoeffs.Keys[i];
                            var currentCoeff = this.field.Multiply(
                                divisorSorteCoeffs[currentDivisorDegree],
                                factor);
                            currentDivisorDegree += differenceDegree;
                            var addCoeff = default(CoeffType);
                            if (remainderSortedCoeffs.TryGetValue(currentDivisorDegree, out addCoeff))
                            {
                                addCoeff = this.field.Add(
                                    addCoeff,
                                    this.field.AdditiveInverse(currentCoeff));
                                if (this.field.IsAdditiveUnity(addCoeff))
                                {
                                    remainderSortedCoeffs.Remove(currentDivisorDegree);
                                }
                                else
                                {
                                    remainderSortedCoeffs[currentDivisorDegree] = addCoeff;
                                }
                            }
                            else
                            {
                                remainderSortedCoeffs.Add(
                                    currentDivisorDegree,
                                    this.field.AdditiveInverse(currentCoeff));
                            }
                        }

                        remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    }

                    var remainder = new UnivariatePolynomialNormalForm<CoeffType, FieldType>(remainderSortedCoeffs, this.variableName, this.field);
                    return remainder;
                }
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
                if (divisor.Degree > dividend.Degree)
                {
                    return new DomainResult<UnivariatePolynomialNormalForm<CoeffType, FieldType>>(
                        new UnivariatePolynomialNormalForm<CoeffType, FieldType>(this.variableName, this.field),
                        dividend);
                }
                else
                {
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(Comparer<int>.Default);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(Comparer<int>.Default);
                    var quotientCoeffs = new UnivariatePolynomialNormalForm<CoeffType, FieldType>(this.variableName, this.field);
                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (remainderLeadingDegree >= divisorLeadingDegree)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.field.Multiply(
                            remainderLeadingCoeff,
                            inverseDivisorLeadingCoeff);
                        quotientCoeffs = quotientCoeffs.Add(factor, differenceDegree);
                        remainderSortedCoeffs.Remove(remainderLeadingDegree);
                        for (int i = 0; i < divisorSorteCoeffs.Keys.Count - 1; ++i)
                        {
                            var currentDivisorDegree = divisorSorteCoeffs.Keys[i];
                            var currentCoeff = this.field.Multiply(
                                divisorSorteCoeffs[currentDivisorDegree],
                                factor);
                            currentDivisorDegree += differenceDegree;
                            var addCoeff = default(CoeffType);
                            if (remainderSortedCoeffs.TryGetValue(currentDivisorDegree, out addCoeff))
                            {
                                addCoeff = this.field.Add(
                                    addCoeff,
                                    this.field.AdditiveInverse(currentCoeff));
                                if (this.field.IsAdditiveUnity(addCoeff))
                                {
                                    remainderSortedCoeffs.Remove(currentDivisorDegree);
                                }
                                else
                                {
                                    remainderSortedCoeffs[currentDivisorDegree] = addCoeff;
                                }
                            }
                            else
                            {
                                remainderSortedCoeffs.Add(
                                    currentDivisorDegree,
                                    this.field.AdditiveInverse(currentCoeff));
                            }
                        }

                        remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    }

                    var remainder = new UnivariatePolynomialNormalForm<CoeffType, FieldType>(remainderSortedCoeffs, this.variableName, this.field);
                    return new DomainResult<UnivariatePolynomialNormalForm<CoeffType,FieldType>>(
                        quotientCoeffs,
                        remainder);
                }
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
