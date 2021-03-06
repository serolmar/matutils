﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define as operações de domínio sobre um polinómio univariável na forma normal com grau genérico.
    /// </summary>
    /// <remarks>
    /// Uma definição comum de polinómios que se adapta à ciência de computadores consiste em considerá-los
    /// como sendo funçlões do conjunto de inteiros (graus) num conjunto de valores (coeficientes). Porém, esta
    /// definição admite uma generalização óbvia que consiste em considerá-los como sendo funções de um conjunto de
    /// objectos (grau) sobre o qual está definida uma estrutura de monóide sobre um conjunto de valores (coeficientes).
    /// No presente caso, considera-se que essa estrutura seja a de um número inteiro por ser mais útil na
    /// implementação dos vários algoritmos.
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo dos coeficientes do polinómio.</typeparam>
    /// <typeparam name="DegreeType">O tipo do grau do polinómio.</typeparam>
    public class GeneralDegUnivarPolynomEuclideanDomain<CoeffType, DegreeType>
        : GeneralDegUnivarPolynomRing<CoeffType, DegreeType>,
        IEuclidenDomain<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>>
    {
        /// <summary>
        /// O corpo responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IField<CoeffType> field;

        /// <summary>
        /// A unidade do polinómio geral na forma normal.
        /// </summary>
        private GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> unit;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="GeneralDegUnivarPolynomEuclideanDomain{CoeffType, DegreeType}"/>.
        /// </summary>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os coeficiente.</param>
        /// <param name="integerNumber">O objecto que é responsável pelas operações sobre os graus.</param>
        public GeneralDegUnivarPolynomEuclideanDomain(
            string variableName,
            IField<CoeffType> field,
            IIntegerNumber<DegreeType> integerNumber)
            : base(variableName, field, integerNumber)
        {
            this.field = field;
            this.unit = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                     this.field.MultiplicativeUnity,
                     this.integerNumber.MultiplicativeUnity,
                     this.variableName,
                     this.field,
                     this.integerNumber);
        }

        /// <summary>
        /// Obtém o corpo responsável pelas operações sobre os coeficientes do polinómio.
        /// </summary>
        /// <value>
        /// O corpo responsável pelas operações sobre os coeficientes do polinómio.
        /// </value>
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

        /// <summary>
        /// Obtém o número de unidades.
        /// </summary>
        /// <value>
        /// O número de unidades.
        /// </value>
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
        /// <exception cref="ArgumentNullException">Caso algum dos argumentos seja nulo.</exception>
        /// <exception cref="ArgumentException">Se os polinómios contiverem variáveis cujos nomes são diferentes.</exception>
        /// <exception cref="DivideByZeroException">Se o divisor for uma unidade aditiva.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Quo(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> dividend,
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> divisor)
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
                else if (this.integerNumber.Compare(divisor.Degree, dividend.Degree) > 0)
                {
                    return new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                        this.variableName,
                        this.integerNumber);
                }
                else
                {
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(this.integerNumber);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(this.integerNumber);
                    var quotientCoeffs = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                        this.variableName,
                        this.integerNumber);

                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (this.integerNumber.Compare(remainderLeadingDegree, divisorLeadingDegree) >= 0 &&
                        remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = this.integerNumber.Add(
                            remainderLeadingDegree,
                            this.integerNumber.AdditiveInverse(divisorLeadingDegree));
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
                            currentDivisorDegree = this.integerNumber.Add(currentDivisorDegree, differenceDegree);
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
                            remainderLeadingDegree = this.integerNumber.AdditiveUnity;
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
        /// <exception cref="ArgumentNullException">Caso algum dos argumentos seja nulo.</exception>
        /// <exception cref="ArgumentException">Se os polinómios contiverem variáveis cujos nomes são diferentes.</exception>
        /// <exception cref="DivideByZeroException">Se o divisor for uma unidade aditiva.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Rem(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> dividend,
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> divisor)
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
                else if (this.integerNumber.Compare(divisor.Degree, dividend.Degree) > 0)
                {
                    return dividend.Clone();
                }
                else
                {
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(this.integerNumber);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(this.integerNumber);
                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (this.integerNumber.Compare(remainderLeadingDegree, divisorLeadingDegree) >= 0 &&
                        remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = this.integerNumber.Add(
                            remainderLeadingDegree,
                            this.integerNumber.AdditiveInverse(divisorLeadingDegree));
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
                            currentDivisorDegree = this.integerNumber.Add(currentDivisorDegree, differenceDegree);
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
                            remainderLeadingDegree = this.integerNumber.AdditiveUnity;
                        }
                    }

                    var remainder = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                        remainderSortedCoeffs,
                        this.variableName,
                        this.field,
                        this.integerNumber);
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
        /// <exception cref="ArgumentNullException">Caso algum dos argumentos seja nulo.</exception>
        /// <exception cref="ArgumentException">Se os polinómios contiverem variáveis cujos nomes são diferentes.</exception>
        /// <exception cref="DivideByZeroException">Se o divisor for uma unidade aditiva.</exception>
        public DomainResult<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>> GetQuotientAndRemainder(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> dividend,
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> divisor)
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
                    return new DomainResult<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>>(
                        this.AdditiveUnity,
                        this.AdditiveUnity);
                }
                else if (this.integerNumber.Compare(divisor.Degree, dividend.Degree) > 0)
                {
                    return new DomainResult<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>>(
                        new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                            this.variableName,
                            this.integerNumber),
                        dividend);
                }
                else
                {
                    var remainderSortedCoeffs = dividend.GetOrderedCoefficients(this.integerNumber);
                    var divisorSorteCoeffs = divisor.GetOrderedCoefficients(this.integerNumber);
                    var quotientCoeffs = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                        this.variableName,
                        this.integerNumber);

                    var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                    var inverseDivisorLeadingCoeff = this.field.MultiplicativeInverse(divisorSorteCoeffs[divisorLeadingDegree]);
                    while (this.integerNumber.Compare(remainderLeadingDegree, divisorLeadingDegree) >= 0 &&
                        remainderSortedCoeffs.Count > 0)
                    {
                        var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                        var differenceDegree = this.integerNumber.Add(remainderLeadingDegree,
                            this.integerNumber.AdditiveInverse(divisorLeadingDegree));
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
                            currentDivisorDegree = this.integerNumber.Add(currentDivisorDegree, differenceDegree);
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
                            remainderLeadingDegree = this.integerNumber.AdditiveUnity;
                        }
                    }

                    var remainder = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                        remainderSortedCoeffs,
                        this.variableName,
                        this.field,
                        this.integerNumber);
                    return new DomainResult<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>>(
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
        /// <exception cref="ArgumentNullException">Se o argumento for nulo.</exception>
        public uint Degree(GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return (uint)(object)value.Degree;
            }
        }

        /// <summary>
        /// Obtém um iterador para as unidades.
        /// </summary>
        /// <value>
        /// O interador para as unidades.
        /// </value>
        public IEnumerable<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>> Units
        {
            get
            {
                yield return this.unit;
            }
        }
    }
}
