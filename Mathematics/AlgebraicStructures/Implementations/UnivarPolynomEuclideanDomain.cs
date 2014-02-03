namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Permite implementar as operações de divisão de polinómios.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes do polinómio.</typeparam>
    public class UnivarPolynomEuclideanDomain<CoeffType> :
        UnivarPolynomRing<CoeffType>,
        IEuclidenDomain<UnivariatePolynomialNormalForm<CoeffType>>
    {
        private IField<CoeffType> field;

        private UnivariatePolynomialNormalForm<CoeffType> unit;

        public UnivarPolynomEuclideanDomain(string variableName, IField<CoeffType> field)
            : base(variableName, field)
        {
            this.unit = new UnivariatePolynomialNormalForm<CoeffType>(
                     this.field.MultiplicativeUnity,
                     1,
                     this.variableName,
                     this.field);
            this.field = field;
        }

        /// <summary>
        /// Obtém o corpo sobre os coeficientes do polinómio.
        /// </summary>
        public IField<CoeffType> Field
        {
            get
            {
                return this.field;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("A polynomial coefficients field must be provided.");
                }
                else
                {
                    this.field = value;
                    this.ring = value;
                }
            }
        }

        public int UnitsCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Calcula o quociente entre dois polinómios.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O quociente.</returns>
        public UnivariatePolynomialNormalForm<CoeffType> Quo(
            UnivariatePolynomialNormalForm<CoeffType> dividend,
            UnivariatePolynomialNormalForm<CoeffType> divisor)
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
                if (this.IsAdditiveUnity(divisor))
                {
                    throw new DivideByZeroException("Can't divide by the null polynomial.");
                }
                else if (this.IsAdditiveUnity(dividend))
                {
                    return this.AdditiveUnity;
                }
                else if (divisor.Degree > dividend.Degree)
                {
                    return new UnivariatePolynomialNormalForm<CoeffType>(
                        this.variableName);
                }
                else
                {
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(Comparer<int>.Default);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(Comparer<int>.Default);
                    var quotientCoeffs = new UnivariatePolynomialNormalForm<CoeffType>(this.variableName);
                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (remainderLeadingDegree >= divisorLeadingDegree && remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.field.Multiply(
                            remainderLeadingCoeff,
                            inverseDivisorLeadingCoeff);
                        quotientCoeffs = quotientCoeffs.Add(factor, differenceDegree, this.field);
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

                        if (remainderSortedCoeffs.Count > 0)
                        {
                            remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                        }
                        else
                        {
                            remainderLeadingDegree = 0;
                        }
                    }

                    return quotientCoeffs;
                }
            }
        }

        /// <summary>
        /// Calcula o resto da divisão entre dois polinómios.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O resto.</returns>
        public UnivariatePolynomialNormalForm<CoeffType> Rem(
            UnivariatePolynomialNormalForm<CoeffType> dividend,
            UnivariatePolynomialNormalForm<CoeffType> divisor)
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
                if (this.IsAdditiveUnity(divisor))
                {
                    throw new DivideByZeroException("Can't divide by the null polynomial.");
                }
                else if (this.IsAdditiveUnity(dividend))
                {
                    return this.AdditiveUnity;
                }
                else if (divisor.Degree > dividend.Degree)
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
                    while (remainderLeadingDegree >= divisorLeadingDegree && remainderSortedCoeffs.Count > 0)
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

                        if (remainderSortedCoeffs.Count > 0)
                        {
                            remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                        }
                        else
                        {
                            remainderLeadingDegree = 0;
                        }
                    }

                    var remainder = new UnivariatePolynomialNormalForm<CoeffType>(
                        remainderSortedCoeffs,
                        this.variableName,
                        this.field);
                    return remainder;
                }
            }
        }

        /// <summary>
        /// Obtém o quociente e o resto da divisão entre dois polinómios.
        /// </summary>
        /// <param name="dividend">O dividendo.</param>
        /// <param name="divisor">O divisor.</param>
        /// <returns>O quociente e o resto.</returns>
        public DomainResult<UnivariatePolynomialNormalForm<CoeffType>> GetQuotientAndRemainder(
            UnivariatePolynomialNormalForm<CoeffType> dividend,
            UnivariatePolynomialNormalForm<CoeffType> divisor)
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
                if (this.IsAdditiveUnity(divisor))
                {
                    throw new DivideByZeroException("Can't divide by the null polynomial.");
                }
                else if (this.IsAdditiveUnity(dividend))
                {
                    return new DomainResult<UnivariatePolynomialNormalForm<CoeffType>>(
                        this.AdditiveUnity,
                        this.AdditiveUnity);
                }
                else if (divisor.Degree > dividend.Degree)
                {
                    return new DomainResult<UnivariatePolynomialNormalForm<CoeffType>>(
                        new UnivariatePolynomialNormalForm<CoeffType>(this.variableName),
                        dividend);
                }
                else
                {
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(Comparer<int>.Default);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(Comparer<int>.Default);
                    var quotientCoeffs = new UnivariatePolynomialNormalForm<CoeffType>(this.variableName);
                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (remainderLeadingDegree >= divisorLeadingDegree && remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.field.Multiply(
                            remainderLeadingCoeff,
                            inverseDivisorLeadingCoeff);
                        quotientCoeffs = quotientCoeffs.Add(factor, differenceDegree, this.field);
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

                        if (remainderSortedCoeffs.Count > 0)
                        {
                            remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                        }
                        else
                        {
                            remainderLeadingDegree = 0;
                        }
                    }

                    var remainder = new UnivariatePolynomialNormalForm<CoeffType>(
                        remainderSortedCoeffs,
                        this.variableName,
                        this.field);
                    return new DomainResult<UnivariatePolynomialNormalForm<CoeffType>>(
                        quotientCoeffs,
                        remainder);
                }
            }
        }

        /// <summary>
        /// Obtém o grau do polinómio.
        /// </summary>
        /// <param name="value">O polinómio do qual se pretende obter o grau.</param>
        /// <returns>O valor do grau.</returns>
        public uint Degree(UnivariatePolynomialNormalForm<CoeffType> value)
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

        public IEnumerable<UnivariatePolynomialNormalForm<CoeffType>> Units
        {
            get
            {
                yield return this.unit;
            }
        }
    }
}
