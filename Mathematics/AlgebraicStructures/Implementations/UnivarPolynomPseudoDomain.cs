namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Corresponde ao domínio relativo à pseudo-divisão de polinómios.
    /// </summary>
    /// <remarks>
    /// Mostra-se que é possível encontrar dois polinómios q e r tais que
    /// b^d*f=gq  + r onde f é o dividendo, g é o divisor, b corresponde ao coeficiente do termo de
    /// maior grau de f e d = grau(f) - grau(g) + 1 com a particularidade do grau de r ser inferior ao
    /// grau de g. Trata-se de uma espécie de divisão sem ter a necessidade de serem efecutadas operações
    /// que não estejam contempladas no anel.
    /// </remarks>
    public class UnivarPolynomPseudoDomain<CoeffType>
        : UnivarPolynomRing<CoeffType>,
        IEuclidenDomain<UnivariatePolynomialNormalForm<CoeffType>>
    {
        private IEuclidenDomain<CoeffType> coeffsDomain;

        public UnivarPolynomPseudoDomain(
            string variableName,
            IEuclidenDomain<CoeffType> coeffsDomain)
            : base(variableName, coeffsDomain)
        {
            this.coeffsDomain = coeffsDomain;
        }

        /// <summary>
        /// Obtém o domínios dos coeficientes que implementa as operações necessárias à execução das
        /// funções do pseudo-domínio polinomial corrente.
        /// </summary>
        public IEuclidenDomain<CoeffType> CoeffsDomain
        {
            get
            {
                return this.coeffsDomain;
            }
        }

        /// <summary>
        /// Obtém o número de unidades quando encarado como um domínio de factorização única.
        /// </summary>
        public int UnitsCount
        {
            get
            {
                return this.coeffsDomain.UnitsCount;
            }
        }

        /// <summary>
        /// Calcula o quociente da pseudo-divisão entre dois polinómios.
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
                if (divisor.Degree > dividend.Degree)
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
                    var divisorLeadingCoeff = divisorSorteCoeffs.Values[divisorSorteCoeffs.Values.Count - 1];
                    var multiplyNumber = MathFunctions.Power(
                        divisorLeadingCoeff,
                        remainderLeadingDegree - divisorLeadingDegree + 1,
                        this.ring);
                    var temporaryRemainderCoeffs = new SortedList<int, CoeffType>(Comparer<int>.Default);
                    foreach (var kvp in remainderSortedCoeffs)
                    {
                        temporaryRemainderCoeffs.Add(
                            kvp.Key,
                            this.ring.Multiply(kvp.Value, multiplyNumber));
                    }

                    remainderSortedCoeffs = temporaryRemainderCoeffs;
                    while (remainderLeadingDegree >= divisorLeadingDegree && remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.coeffsDomain.Quo(remainderLeadingCoeff, divisorLeadingCoeff);
                        quotientCoeffs = quotientCoeffs.Add(factor, differenceDegree, this.ring);
                        remainderSortedCoeffs.Remove(remainderLeadingDegree);
                        for (int i = 0; i < divisorSorteCoeffs.Keys.Count - 1; ++i)
                        {
                            var currentDivisorDegree = divisorSorteCoeffs.Keys[i];
                            var currentCoeff = this.ring.Multiply(
                                divisorSorteCoeffs[currentDivisorDegree],
                                factor);
                            currentDivisorDegree += differenceDegree;
                            var addCoeff = default(CoeffType);
                            if (remainderSortedCoeffs.TryGetValue(currentDivisorDegree, out addCoeff))
                            {
                                addCoeff = this.ring.Add(
                                    addCoeff,
                                    this.ring.AdditiveInverse(currentCoeff));
                                if (this.ring.IsAdditiveUnity(addCoeff))
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
                                    this.ring.AdditiveInverse(currentCoeff));
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
        /// Calcula o resto da pseudo-divisão entre dois polinómios.
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
                    var divisorLeadingCoeff = divisorSorteCoeffs.Values[divisorSorteCoeffs.Values.Count - 1];
                    var multiplyNumber = MathFunctions.Power(
                        divisorLeadingCoeff,
                        remainderLeadingDegree - divisorLeadingDegree + 1,
                        this.ring);
                    var temporaryRemainderCoeffs = new SortedList<int, CoeffType>(Comparer<int>.Default);
                    foreach (var kvp in remainderSortedCoeffs)
                    {
                        temporaryRemainderCoeffs.Add(
                            kvp.Key,
                            this.ring.Multiply(kvp.Value, multiplyNumber));
                    }

                    remainderSortedCoeffs = temporaryRemainderCoeffs;
                    while (remainderLeadingDegree >= divisorLeadingDegree && remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.coeffsDomain.Quo(remainderLeadingCoeff, divisorLeadingCoeff);
                        remainderSortedCoeffs.Remove(remainderLeadingDegree);
                        for (int i = 0; i < divisorSorteCoeffs.Keys.Count - 1; ++i)
                        {
                            var currentDivisorDegree = divisorSorteCoeffs.Keys[i];
                            var currentCoeff = this.ring.Multiply(
                                divisorSorteCoeffs[currentDivisorDegree],
                                factor);
                            currentDivisorDegree += differenceDegree;
                            var addCoeff = default(CoeffType);
                            if (remainderSortedCoeffs.TryGetValue(currentDivisorDegree, out addCoeff))
                            {
                                addCoeff = this.ring.Add(
                                    addCoeff,
                                    this.ring.AdditiveInverse(currentCoeff));
                                if (this.ring.IsAdditiveUnity(addCoeff))
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
                                    this.ring.AdditiveInverse(currentCoeff));
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
                        this.ring);
                    return remainder;
                }
            }
        }

        /// <summary>
        /// Obtém o quociente e o resto da pseudo-divisão entre dois polinómios.
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
                if (divisor.Degree > dividend.Degree)
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
                    var divisorLeadingCoeff = divisorSorteCoeffs.Values[divisorSorteCoeffs.Values.Count - 1];
                    var multiplyNumber = MathFunctions.Power(
                        divisorLeadingCoeff,
                        remainderLeadingDegree - divisorLeadingDegree + 1,
                        this.ring);
                    var temporaryRemainderCoeffs = new SortedList<int, CoeffType>(Comparer<int>.Default);
                    foreach (var kvp in remainderSortedCoeffs)
                    {
                        temporaryRemainderCoeffs.Add(
                            kvp.Key,
                            this.ring.Multiply(kvp.Value, multiplyNumber));
                    }

                    remainderSortedCoeffs = temporaryRemainderCoeffs;
                    while (remainderLeadingDegree >= divisorLeadingDegree && remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                        var factor = this.coeffsDomain.Quo(remainderLeadingCoeff, divisorLeadingCoeff);
                        quotientCoeffs = quotientCoeffs.Add(factor, differenceDegree, this.ring);
                        remainderSortedCoeffs.Remove(remainderLeadingDegree);
                        for (int i = 0; i < divisorSorteCoeffs.Keys.Count - 1; ++i)
                        {
                            var currentDivisorDegree = divisorSorteCoeffs.Keys[i];
                            var currentCoeff = this.ring.Multiply(
                                divisorSorteCoeffs[currentDivisorDegree],
                                factor);
                            currentDivisorDegree += differenceDegree;
                            var addCoeff = default(CoeffType);
                            if (remainderSortedCoeffs.TryGetValue(currentDivisorDegree, out addCoeff))
                            {
                                addCoeff = this.ring.Add(
                                    addCoeff,
                                    this.ring.AdditiveInverse(currentCoeff));
                                if (this.ring.IsAdditiveUnity(addCoeff))
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
                                    this.ring.AdditiveInverse(currentCoeff));
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
                        this.ring);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém as unidades quando encarado como um domínio da factorização única.
        /// </summary>
        public IEnumerable<UnivariatePolynomialNormalForm<CoeffType>> Units
        {
            get
            {
                foreach (var unit in this.coeffsDomain.Units)
                {
                    yield return new UnivariatePolynomialNormalForm<CoeffType>(
                        unit,
                        1,
                        this.variableName,
                        this.coeffsDomain);
                }
            }
        }
    }
}
