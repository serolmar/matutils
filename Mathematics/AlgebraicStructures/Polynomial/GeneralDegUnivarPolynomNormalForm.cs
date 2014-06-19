namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Representa um polinómio geral na forma normal.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de dados associado aos coeficientes.</typeparam>
    /// <typeparam name="DegreeType">O tipo de dados associado ao grau.</typeparam>
    public class GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> 
        : IEnumerable<KeyValuePair<DegreeType, CoeffType>>
    {
        /// <summary>
        /// Objecto responsável pelas operações sobre o grau.
        /// </summary>
        private IIntegerNumber<DegreeType> degreeNumber;

        /// <summary>
        /// Os termos do polinómio, mapeando o grau ao coeficiente correspondente.
        /// </summary>
        private SortedList<DegreeType, CoeffType> terms;

        /// <summary>
        /// O nome da variável associada ao polinómio.
        /// </summary>
        private string variableName;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GeneralDegUnivarPolynomNormalForm{CoeffType, DegreeType}"/>.
        /// </summary>
        /// <param name="degreeNumber">O objecto responsável pelas operações sobre os graus.</param>
        /// <exception cref="ArgumentNullException">Se o argumento for nulo.</exception>
        private GeneralDegUnivarPolynomNormalForm(IIntegerNumber<DegreeType> degreeNumber)
        {
            if (degreeNumber == null)
            {
                throw new ArgumentNullException("degreeNumber");
            }
            else
            {
                this.degreeNumber = degreeNumber;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GeneralDegUnivarPolynomNormalForm{CoeffType, DegreeType}"/>.
        /// </summary>
        /// <param name="variable">A variável assoiada ao polinómio.</param>
        /// <param name="degreeNumber">O objecto responsável pelas operações sobre os graus.</param>
        /// <exception cref="ArgumentNullException">Se a variável for nula ou vazia.</exception>
        public GeneralDegUnivarPolynomNormalForm(string variable, IIntegerNumber<DegreeType> degreeNumber)
            : this(degreeNumber)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                //this.ring = ring;
                this.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(degreeNumber));
                this.variableName = variable;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GeneralDegUnivarPolynomNormalForm{CoeffType, DegreeType}"/>.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="degree">O grau do coeficiente.</param>
        /// <param name="variable">A variável assoiada ao polinómio.</param>
        /// <param name="monoid">O monóide responsável pelas operações sobre os coeficientes.</param>
        /// <param name="degreeNumber">O objecto responsável pelas operações sobre os graus.</param>
        /// <exception cref="ArgumentNullException">Se o monóide ou o coeficiente forem nulos.</exception>
        /// <exception cref="ArgumentException">Se o grau for negativo.</exception>
        public GeneralDegUnivarPolynomNormalForm(
            CoeffType coeff,
            DegreeType degree,
            string variable,
            IMonoid<CoeffType> monoid,
            IIntegerNumber<DegreeType> degreeNumber) :
            this(variable, degreeNumber)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else if (this.degreeNumber.Compare(degree, this.degreeNumber.AdditiveUnity) < 0)
            {
                throw new ArgumentException("Negative degrees aren't allowed.");
            }
            else if (!monoid.IsAdditiveUnity(coeff))
            {
                this.terms.Add(degree, coeff);
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GeneralDegUnivarPolynomNormalForm{CoeffType, DegreeType}"/>.
        /// </summary>
        /// <param name="terms">O conjunto de termos do polinómio.</param>
        /// <param name="variable">A variável assoiada ao polinómio.</param>
        /// <param name="ring">O anel reponsável pelas operações sobre os coeficientes.</param>
        /// <param name="degreeNumber">O objecto responsável pelas operações sobre os graus.</param>
        /// <exception cref="ArgumentNullException">Se o anel ou o conjunto de termos forem nulos.</exception>
        /// <exception cref="ArgumentException">Se o grau for negativo.</exception>
        public GeneralDegUnivarPolynomNormalForm(
            IDictionary<DegreeType, CoeffType> terms,
            string variable,
            IRing<CoeffType> ring,
            IIntegerNumber<DegreeType> degreeNumber)
            : this(variable, degreeNumber)
        {
            if (terms == null)
            {
                throw new ArgumentNullException("terms");
            }
            else
            {
                foreach (var kvp in terms)
                {
                    if (this.degreeNumber.Compare(kvp.Key, this.degreeNumber.AdditiveUnity) < 0)
                    {
                        throw new ArgumentException("Negative degrees aren't allowed.");
                    }
                    else if (!ring.IsAdditiveUnity(kvp.Value))
                    {
                        this.terms.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o nome da variável.
        /// </summary>
        /// <value>O nome da variável.</value>
        public string VariableName
        {
            get
            {
                return this.variableName;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se se trata de um monómio.
        /// </summary>
        /// <value>Valor que indica se se trata de um monómio.</value>
        public bool IsMonomial
        {
            get
            {
                return this.terms.Count < 2;
            }
        }

        /// <summary>
        /// Obtém um valor que verifica se o polinómio é nulo.
        /// </summary>
        /// <value>Valor que indica se o polinómio é nulo.</value>
        public bool IsZero
        {
            get
            {
                return this.terms.Count == 0;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o polinómio contém apenas um valor.
        /// </summary>
        /// <value>Valor que indica se o polinómio é um valor.</value>
        public bool IsValue
        {
            get
            {
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var currentTerm = termsEnumerator.Current;
                    if (termsEnumerator.MoveNext())
                    {
                        return false;
                    }
                    else
                    {
                        return this.degreeNumber.IsAdditiveUnity(currentTerm.Key);
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Obtém o grau do polinómio.
        /// </summary>
        /// <value>O grau do polinómio.</value>
        public DegreeType Degree
        {
            get
            {
                var result = this.degreeNumber.AdditiveUnity;
                foreach (var kvp in terms)
                {
                    if (this.degreeNumber.Compare(kvp.Key, result) > 0)
                    {
                        result = kvp.Key;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém os termos do polinómio.
        /// </summary>
        /// <value>Os termos do polinómio.</value>
        internal SortedList<DegreeType, CoeffType> Terms
        {
            get
            {
                return this.terms;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o polinómio é unitário.
        /// </summary>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>Verdadeiro caso o argumento  seja uma unidade e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide for nulo.</exception>
        public bool IsUnity(IRing<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var currentValue = termsEnumerator.Current;
                    if (this.degreeNumber.IsAdditiveUnity(currentValue.Key))
                    {
                        if (termsEnumerator.MoveNext())
                        {
                            return false;
                        }
                        else if (monoid.IsMultiplicativeUnity(currentValue.Value))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o polinómio como sendo um valor.
        /// </summary>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>O polinómio.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide for nulo.</exception>
        public CoeffType GetAsValue(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var currentTerm = termsEnumerator.Current;
                    if (termsEnumerator.MoveNext() || !this.degreeNumber.IsAdditiveUnity(currentTerm.Key))
                    {
                        throw new MathematicsException("Polynomail can't be converted to a value.");
                    }
                    else
                    {
                        return currentTerm.Value;
                    }
                }
                else
                {
                    return monoid.AdditiveUnity;
                }
            }
        }

        /// <summary>
        /// Obtém uma cópia do polinómio corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Clone()
        {
            var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
            result.variableName = this.variableName;
            result.terms = new SortedList<DegreeType, CoeffType>(this.terms.Comparer);
            foreach (var kvp in this.terms)
            {
                result.terms.Add(kvp.Key, kvp.Value);
            }

            return result;
        }

        /// <summary>
        /// Obtém o monómio com o maior grau.
        /// </summary>
        /// <returns>O monómio.</returns>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> GetLeadingMonomial()
        {
            var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
            result.variableName = this.variableName;
            if (this.terms.Count != 0)
            {
                var degree = this.terms.Keys[0];
                var coeff = this.terms[degree];
                result.terms.Add(degree, coeff);
            }

            return result;
        }

        /// <summary>
        /// Obtém o coeficiente do monómio com o maior grau.
        /// </summary>
        /// <param name="monoid">O monóide responsável pela determinação da unidade aditiva.</param>
        /// <returns>O coeficiente.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide for nulo.</exception>
        public CoeffType GetLeadingCoefficient(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (this.terms.Count == 0)
            {
                return monoid.AdditiveUnity;
            }
            else
            {
                return this.terms[this.terms.Keys[0]];
            }
        }

        /// <summary>
        /// Obtém o monómio com o menor grau.
        /// </summary>
        /// <returns>O monómio.</returns>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> GetTailMonomial()
        {
            var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
            result.variableName = this.variableName;
            if (this.terms.Count != 0)
            {
                var degree = this.terms.Keys[this.terms.Count - 1];
                var coeff = this.terms[degree];
                result.terms.Add(degree, coeff);
            }

            return result;
        }

        /// <summary>
        /// Obtém o coeficiente do monómio com o menor grau.
        /// </summary>
        /// <param name="monoid">O monóide responsável pela determinação da unidade aditiva.</param>
        /// <returns>O coeficiente.</returns>
        /// <exception cref="ArgumentNullException">Se o monóide for nulo.</exception>
        public CoeffType GetTailCoefficient(IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (this.terms.Count == 0)
            {
                return monoid.AdditiveUnity;
            }
            else
            {
                return this.terms[this.terms.Keys[this.terms.Count - 1]];
            }
        }

        /// <summary>
        /// Obtém a derivada formal do polinómio corrente.
        /// </summary>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>A derivada.</returns>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> GetPolynomialDerivative(
            IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(this.terms.Comparer);
                foreach (var termKvp in this.terms)
                {
                    if (this.degreeNumber.Compare(termKvp.Key, this.degreeNumber.AdditiveUnity) > 0)
                    {
                        var elementsToAdd = MathFunctions.AddPower(
                            termKvp.Value, 
                            termKvp.Key, 
                            ring, 
                            this.degreeNumber);
                        if (!ring.IsAdditiveUnity(elementsToAdd))
                        {
                            result.terms.Add(this.degreeNumber.Predecessor(termKvp.Key), elementsToAdd);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um vector com a soma das potências, desde 1 até ao grau do polinómio, das raízes.
        /// </summary>
        /// <param name="field">O corpo responsável pelas operações.</param>
        /// <param name="conversion">O conversor que permite converter entre o grau e os números inteiros.</param>
        /// <returns>O vector com o valor da soma das potências.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public IMatrix<CoeffType> GetRootPowerSums(
            IField<CoeffType> field,
            IConversion<int, DegreeType> conversion)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }
            else if (!conversion.CanApplyDirectConversion(this.Degree))
            {
                throw new MathematicsException("Degree is too long to get all power sums entries.");
            }
            else
            {
                var result = new ArrayMatrix<CoeffType>(
                    conversion.DirectConversion(this.Degree), 1, field.AdditiveUnity);
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var topTerm = termsEnumerator.Current.Value;
                    var topDegree = conversion.DirectConversion(termsEnumerator.Current.Key);
                    if (topDegree == 0)
                    {
                        return new ArrayMatrix<CoeffType>(0, 0, field.AdditiveUnity);
                    }
                    else if (termsEnumerator.MoveNext())
                    {
                        var currentDegree = conversion.DirectConversion(termsEnumerator.Current.Key);
                        var difference = topDegree - currentDegree - 1;

                        var value = field.AdditiveInverse(termsEnumerator.Current.Value);
                        value = field.AddRepeated(value, difference + 1);
                        result[difference, 0] = field.Multiply(value, field.MultiplicativeInverse(topTerm));
                        var control = difference - 1;
                        for (var i = difference + 1; i < topDegree; ++i)
                        {
                            termsEnumerator = this.terms.GetEnumerator();
                            termsEnumerator.MoveNext();
                            var state = termsEnumerator.MoveNext();
                            var currentIteration = i - 1;
                            var compareDegree = topDegree - 1;
                            while (state && currentIteration > control)
                            {
                                currentDegree = conversion.DirectConversion(termsEnumerator.Current.Key);
                                if (compareDegree == currentDegree)
                                {
                                    value = field.AdditiveInverse(termsEnumerator.Current.Value);
                                    value = field.Multiply(value, result[currentIteration, 0]);
                                    result[i, 0] = field.Add(result[i, 0], value);
                                    state = termsEnumerator.MoveNext();
                                }

                                --compareDegree;
                                --currentIteration;
                            }

                            while (state && control >= 0)
                            {
                                --control;
                                currentDegree = conversion.DirectConversion(termsEnumerator.Current.Key);
                                if (compareDegree == currentDegree)
                                {
                                    state = termsEnumerator.MoveNext();
                                    --compareDegree;
                                }
                            }

                            if (state)
                            {
                                currentDegree = conversion.DirectConversion(termsEnumerator.Current.Key);
                                if (compareDegree == currentDegree)
                                {
                                    value = field.AdditiveInverse(termsEnumerator.Current.Value);
                                    value = field.AddRepeated(value, i + 1);
                                    result[i, 0] = field.Add(result[i, 0], value);
                                }
                            }

                            result[i, 0] = field.Multiply(result[i, 0], field.MultiplicativeInverse(topTerm));
                        }

                        return result;
                    }
                }
                else
                {
                    return new ArrayMatrix<CoeffType>(0, 0, field.AdditiveUnity);
                }

                return result;
            }
        }

        #region Operações
        /// <summary>
        /// Obtém a soma do polinómio corrente com o polinómio providenciado.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>A soma.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="MathematicsException">Se o nome das variáveis não coincidirem.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Add(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> right,
            IMonoid<CoeffType> monoid)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (right.variableName != this.variableName)
            {
                throw new MathematicsException(
                    "Can't multiply two univariate polynomials with different variable names.");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                foreach (var term in this.terms)
                {
                    result.terms.Add(term.Key, term.Value);
                }

                foreach (var term in right.terms)
                {
                    var coeff = default(CoeffType);
                    if (result.terms.TryGetValue(term.Key, out coeff))
                    {
                        coeff = monoid.Add(coeff, term.Value);
                        if (monoid.IsAdditiveUnity(coeff))
                        {
                            result.terms.Remove(term.Key);
                        }
                        else
                        {
                            result.terms[term.Key] = coeff;
                        }
                    }
                    else
                    {
                        result.terms.Add(term.Key, term.Value);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a soma do polinómio corrente com um termo constante.
        /// </summary>
        /// <param name="coeff">O termo constante.</param>
        /// <param name="monoid">O monóide responsável pelas operações sobre os coeficientes.</param>
        /// <returns>A soma.</returns>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Add(
            CoeffType coeff,
            IMonoid<CoeffType> monoid)
        {
            return this.Add(coeff, this.degreeNumber.AdditiveUnity, monoid);
        }

        /// <summary>
        /// Obtém a soma do polinómio corrente com um monómio.
        /// </summary>
        /// <param name="coeff">Os elementos do monómio.</param>
        /// <param name="degree">O grau do coeficiente.</param>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>A soma.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Add(
            CoeffType coeff,
            DegreeType degree,
            IMonoid<CoeffType> monoid)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else if (degree == null)
            {
                throw new ArgumentNullException("degree");
            }
            else if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                var degreeCoeff = coeff;
                foreach (var kvp in this.terms)
                {
                    if (this.degreeNumber.Equals(kvp.Key, degree))
                    {
                        degreeCoeff = monoid.Add(degreeCoeff, kvp.Value);
                    }
                    else
                    {
                        result.terms.Add(kvp.Key, kvp.Value);
                    }
                }

                if (!monoid.IsAdditiveUnity(degreeCoeff))
                {
                    result.terms.Add(degree, degreeCoeff);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a diferença entre o polinómio corrente e o polinómio providenciado.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>A diferença.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="MathematicsException">Se o nome das variáveis não coincidirem.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Subtract(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> right,
            IGroup<CoeffType> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (right.variableName != this.variableName)
            {
                throw new MathematicsException(
                    "Can't multiply two univariate polynomials with different variable names.");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                foreach (var term in this.terms)
                {
                    result.terms.Add(term.Key, term.Value);
                }

                foreach (var term in right.terms)
                {
                    var coeff = default(CoeffType);
                    if (result.terms.TryGetValue(term.Key, out coeff))
                    {
                        coeff = group.Add(coeff, group.AdditiveInverse(term.Value));
                        if (group.IsAdditiveUnity(coeff))
                        {
                            result.terms.Remove(term.Key);
                        }
                        else
                        {
                            result.terms[term.Key] = coeff;
                        }
                    }
                    else
                    {
                        result.terms.Add(term.Key, group.AdditiveInverse(term.Value));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a diferença entre o polinómio corrente e um termo constante.
        /// </summary>
        /// <param name="coeff">O coeficiente a ser strubtraído.</param>
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>A diferença.</returns>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Subtract(
            CoeffType coeff,
            IGroup<CoeffType> group)
        {
            return this.Subtract(coeff, this.degreeNumber.AdditiveUnity, group);
        }

        /// <summary>
        /// Obtém a diferença entre o polinómio corrente e um monómio.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="degree">O grau.</param>
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>A diferença.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Subtract(
            CoeffType coeff,
            DegreeType degree,
            IGroup<CoeffType> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else if (degree == null)
            {
                throw new ArgumentNullException("degree");
            }
            else if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                var degreeCoeff = group.AdditiveInverse(coeff);
                foreach (var kvp in this.terms)
                {
                    if (this.degreeNumber.Equals(kvp.Key, degree))
                    {
                        degreeCoeff = group.Add(degreeCoeff, kvp.Value);
                    }
                    else
                    {
                        result.terms.Add(kvp.Key, kvp.Value);
                    }
                }

                if (!group.IsAdditiveUnity(degreeCoeff))
                {
                    result.terms.Add(degree, degreeCoeff);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém produto do polinómio corrente com o polinómio providenciado.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O produto.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="MathematicsException">Se o nome das variáveis não coincidirem.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Multiply(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> right,
            IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (right.variableName != this.variableName)
            {
                throw new MathematicsException(
                    "Can't multiply two univariate polynomials with different variable names.");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                foreach (var thisTerm in this.terms)
                {
                    if (!ring.IsAdditiveUnity(thisTerm.Value))
                    {
                        foreach (var rightTerm in right.terms)
                        {
                            if (!ring.IsAdditiveUnity(rightTerm.Value))
                            {
                                var totalDegree = this.degreeNumber.Add(thisTerm.Key, rightTerm.Key);
                                var totalCoeff = ring.Multiply(thisTerm.Value, rightTerm.Value);
                                var sumCoeff = default(CoeffType);
                                if (result.terms.TryGetValue(totalDegree, out sumCoeff))
                                {
                                    sumCoeff = ring.Add(sumCoeff, totalCoeff);
                                    if (ring.IsAdditiveUnity(sumCoeff))
                                    {
                                        result.terms.Remove(totalDegree);
                                    }
                                    else
                                    {
                                        result.terms[totalDegree] = sumCoeff;
                                    }
                                }
                                else if (!ring.IsAdditiveUnity(totalCoeff))
                                {
                                    result.terms.Add(totalDegree, totalCoeff);
                                }
                            }
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém produto do polinómio corrente com um termo constante.
        /// </summary>
        /// <param name="coeff">O coeficiente a ser multiplicado.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O produto.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Multiply(
            CoeffType coeff,
            IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                if (!ring.IsAdditiveUnity(coeff))
                {
                    foreach (var thisTerm in this.terms)
                    {
                        result.terms.Add(thisTerm.Key, ring.Multiply(thisTerm.Value, coeff));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o polinómio simétrico do actual.
        /// </summary>
        /// <param name="group">O grupo responsável pela determinação da inversa.</param>
        /// <returns>O polinómio simétrico do actual.</returns>
        /// <exception cref="ArgumentNullException">Se o grupo for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> GetSymmetric(IGroup<CoeffType> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = this.variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                foreach (var kvp in this.terms)
                {
                    result.terms.Add(kvp.Key, group.AdditiveInverse(kvp.Value));
                }

                return result;
            }
        }

        /// <summary>
        /// Substitui a variável pelo coeficiente especificado e calcula o resultado.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O resultado.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public CoeffType Replace(CoeffType coeff, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var result = termsEnumerator.Current.Value;
                    var previousDegree = termsEnumerator.Current.Key;
                    while (termsEnumerator.MoveNext())
                    {
                        var currentDegree = termsEnumerator.Current.Key;
                        var power = MathFunctions.Power(
                            coeff,
                            this.degreeNumber.Add(previousDegree, this.degreeNumber.AdditiveInverse(currentDegree)),
                            ring, this.degreeNumber);
                        result = ring.Multiply(result, power);
                        result = ring.Add(result, termsEnumerator.Current.Value);
                        previousDegree = currentDegree;
                    }

                    var lastPower = MathFunctions.Power(coeff, previousDegree, ring, this.degreeNumber);
                    result = ring.Multiply(result, lastPower);
                    return result;
                }
                else
                {
                    return ring.AdditiveUnity;
                }
            }
        }

        /// <summary>
        /// Determina o valor da substituição da variável por um elemento de uma álgebra.
        /// </summary>
        /// <typeparam name="ResultType">O tipo do elemento da álgebra.</typeparam>
        /// <param name="value">O valor do elemento.</param>
        /// <param name="algebra">A álgebra.</param>
        /// <returns>O resultado da substituição.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public ResultType Replace<ResultType>(ResultType value, IAlgebra<CoeffType, ResultType> algebra)
        {
            if (algebra == null)
            {
                throw new ArgumentNullException("algebra");
            }
            else if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var result = algebra.MultiplyScalar(
                        termsEnumerator.Current.Value,
                        algebra.MultiplicativeUnity);
                    var previousDegree = termsEnumerator.Current.Key;
                    while (termsEnumerator.MoveNext())
                    {
                        var currentDegree = termsEnumerator.Current.Key;
                        var power = MathFunctions.Power(
                            value,
                            this.degreeNumber.Add(previousDegree, this.degreeNumber.AdditiveInverse(currentDegree)),
                            algebra,
                            this.degreeNumber);
                        result = algebra.Multiply(result, power);
                        result = algebra.Add(
                            result,
                            algebra.MultiplyScalar(termsEnumerator.Current.Value, algebra.MultiplicativeUnity));
                        previousDegree = currentDegree;
                    }

                    var lastPower = MathFunctions.Power(value, previousDegree, algebra, this.degreeNumber);
                    result = algebra.Multiply(result, lastPower);
                    return result;
                }
                else
                {
                    return algebra.AdditiveUnity;
                }
            }
        }

        /// <summary>
        /// Substitui a variável corrente por uma outra.
        /// </summary>
        /// <param name="variableName">O nome da variável.</param>
        /// <returns>O polinómio com a variável substituída.</returns>
        /// <exception cref="ArgumentException">Se a variável for nula ou vazia.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Replace(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(this.degreeNumber);
                result.variableName = variableName;
                result.terms = new SortedList<DegreeType, CoeffType>(
                    new InverseComparer<DegreeType>(this.degreeNumber));
                foreach (var kvp in this.terms)
                {
                    result.terms.Add(kvp.Key, kvp.Value);
                }

                return result;
            }
        }

        /// <summary>
        /// Substitui a variável pelo polinómio especificado e calcula o resultado.
        /// </summary>
        /// <param name="other">O polinómio a substituir.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="integerNumber">O número inteiro.</param>
        /// <returns>O resultado da substituição.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Replace(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> other,
            IRing<CoeffType> ring,
            IIntegerNumber<DegreeType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else
            {
                var polynomialRing = new GeneralDegUnivarPolynomRing<CoeffType, DegreeType>(
                    this.variableName, 
                    ring,
                    integerNumber);
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                        termsEnumerator.Current.Value,
                        this.degreeNumber.AdditiveUnity,
                        this.variableName,
                        ring,
                        this.degreeNumber);
                    var previousDegree = termsEnumerator.Current.Key;
                    while (termsEnumerator.MoveNext())
                    {
                        var currentDegree = termsEnumerator.Current.Key;
                        var power = MathFunctions.Power(
                            other,
                            this.degreeNumber.Add(previousDegree, this.degreeNumber.AdditiveInverse(currentDegree)),
                            polynomialRing,
                            this.degreeNumber);
                        result = result.Multiply(power, ring);
                        result = result.Add(termsEnumerator.Current.Value, ring);
                        previousDegree = currentDegree;
                    }

                    var lastPower = MathFunctions.Power(
                        other,
                        previousDegree,
                        polynomialRing,
                        this.degreeNumber);
                    result = result.Multiply(lastPower, ring);
                    return result;
                }
                else
                {
                    return polynomialRing.AdditiveUnity;
                }
            }
        }

        #endregion Operações

        /// <summary>
        /// Determina se o objecto especificado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto a comparar.</param>
        /// <returns>
        /// Verdadeiro se as instâncias forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                if (this.variableName != innerObj.variableName)
                {
                    return false;
                }
                else
                {
                    if (this.terms.Count != innerObj.terms.Count)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (var term in this.terms)
                        {
                            var otherTerm = default(CoeffType);
                            if (innerObj.terms.TryGetValue(term.Key, out otherTerm))
                            {
                                return term.Value.Equals(otherTerm);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Retorna um código confuso para a instância corrente.
        /// </summary>
        /// <returns>
        /// Um código confuso para a instância corrente que pode ser usado em vários algoritmos.
        /// </returns>
        public override int GetHashCode()
        {
            var result = this.variableName.GetHashCode();
            foreach (var term in this.terms)
            {
                result ^= term.Key.GetHashCode();
                result ^= term.Value.GetHashCode();
            }

            return result;
        }

        /// <summary>
        /// Obtém uma representação textual do polinómio.
        /// </summary>
        /// <returns>A repesentação textual do polinómio.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            if (terms.Count == 0)
            {
                resultBuilder.Append("zero");
            }
            else
            {
                var termsEnum = this.terms.GetEnumerator();
                if (termsEnum.MoveNext())
                {
                    var currentDegree = termsEnum.Current.Key;
                    var currentValue = termsEnum.Current.Value;
                    resultBuilder.AppendFormat("{0}", currentValue);
                    if (this.degreeNumber.IsMultiplicativeUnity(currentDegree))
                    {
                        resultBuilder.AppendFormat("*{0}", this.variableName);
                    }
                    else if (this.degreeNumber.Compare(currentDegree, this.degreeNumber.MultiplicativeUnity) > 0)
                    {
                        resultBuilder.AppendFormat("*{0}^{1}", this.variableName, currentDegree);
                    }

                    while (termsEnum.MoveNext())
                    {
                        resultBuilder.Append("+");
                        currentDegree = termsEnum.Current.Key;
                        currentValue = termsEnum.Current.Value;
                        resultBuilder.AppendFormat("{0}", currentValue);
                        if (this.degreeNumber.IsMultiplicativeUnity(currentDegree))
                        {
                            resultBuilder.AppendFormat("*{0}", this.variableName);
                        }
                        else if (this.degreeNumber.Compare(currentDegree, this.degreeNumber.MultiplicativeUnity) > 0)
                        {
                            resultBuilder.AppendFormat("*{0}^{1}", this.variableName, currentDegree);
                        }
                    }
                }
            }

            return resultBuilder.ToString();
        }

        /// <summary>
        /// Obtém todos os coeficientes ordenados pelo grau de acordo com o comparador especificado.
        /// </summary>
        /// <param name="degreeComparer">O comparador.</param>
        /// <returns>Uma colecção ordenada com os coeficientes.</returns>
        public SortedList<DegreeType, CoeffType> GetOrderedCoefficients(
            IComparer<DegreeType> degreeComparer)
        {
            var result = new SortedList<DegreeType, CoeffType>(degreeComparer);
            foreach (var kvp in this.terms)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }

        /// <summary>
        /// Determina o polinómio que resulta da aplicação da função quociente a todos
        /// os coeficientes do polinómio.
        /// </summary>
        /// <param name="coeff">O coeficiente que serve de quociente.</param>
        /// <param name="domain">O domínio responsável pelas operações.</param>
        /// <returns>O polinómio cujos coeficientes são o resultado do quociente respectivo.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> ApplyQuo(
            CoeffType coeff, 
            IEuclidenDomain<CoeffType> domain)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.degreeNumber);
                foreach (var kvp in this.terms)
                {
                    var quo = domain.Quo(kvp.Value, coeff);
                    if (!domain.IsAdditiveUnity(quo))
                    {
                        result.terms.Add(kvp.Key, quo);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Determina o polinómio que resulta da aplicação da função resto a todos
        /// os coeficientes do polinómio.
        /// </summary>
        /// <param name="coeff">O coeficiente que serve de quociente.</param>
        /// <param name="domain">O domínio responsável pelas operações.</param>
        /// <returns>O polinómio cujos coeficientes são o resultado do resto respectivo.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> ApplyRem(CoeffType coeff, IEuclidenDomain<CoeffType> domain)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.degreeNumber);
                foreach (var kvp in this.terms)
                {
                    var rem = domain.Rem(kvp.Value, coeff);
                    if (!domain.IsAdditiveUnity(rem))
                    {
                        result.terms.Add(kvp.Key, rem);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Permite construir o polinómio que resulta do polinómio actual, aplicando
        /// uma função a cada um dos coeficientes.
        /// </summary>
        /// <param name="func">A função a ser aplicada.</param>
        /// <param name="monoid">O monóide que irá permitir eliminar os elementos nulos.</param>
        /// <returns>O polinómio.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> ApplyFunction(
            Func<CoeffType, CoeffType> func,
            IMonoid<CoeffType> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.degreeNumber);
                foreach (var kvp in this.terms)
                {
                    var funcRes = func.Invoke(kvp.Value);
                    if (!monoid.IsAdditiveUnity(funcRes))
                    {
                        result.terms.Add(kvp.Key, funcRes);
                    }
                }

                return result;
            }
        }

        /// <summary>
        ///Determina o conteúdo do polinómio, isto é, o máximo divisor comum entre todos os seus coeficientes.
        /// </summary>
        /// <param name="domain">O domínio responsável pelas operações.</param>
        /// <returns>O conteúdo do polinómio.</returns>
        /// <exception cref="ArgumentNullException">Se o domínio for nulo.</exception>
        public CoeffType GetContent(IEuclidenDomain<CoeffType> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var result = domain.MultiplicativeUnity;
                var enumerator = this.terms.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    result = enumerator.Current.Value;
                    while (enumerator.MoveNext())
                    {
                        result = MathFunctions.GreatCommonDivisor(result, enumerator.Current.Value, domain);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém uma cópia do polinóimio corrente onde o coeficiente correspondente ao termo de maior grau
        /// é substituído por um outro.
        /// </summary>
        /// <param name="coeff">O coeficiente a substituir.</param>
        /// <param name="monoid">O monóide responsável por determinar se se está na presença de uma unidade
        /// aditiva.
        /// </param>
        /// <returns>O polinómio resultante da substituição.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> ReplaceLeadingCoeff(CoeffType coeff, IMonoid<CoeffType> monoid)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.degreeNumber);
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    if (!monoid.IsAdditiveUnity(coeff))
                    {
                        result.terms.Add(termsEnumerator.Current.Key, coeff);
                    }

                    while (termsEnumerator.MoveNext())
                    {
                        result.terms.Add(termsEnumerator.Current.Key, termsEnumerator.Current.Value);
                    }
                }
                else if (!monoid.IsAdditiveUnity(coeff))
                {
                    result.terms.Add(this.degreeNumber.AdditiveUnity, coeff);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um enumerador para os termos do polinómio como par chave/valor na qual a chave
        /// contém o grau e o valor contém o respectivo coeficiente.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<KeyValuePair<DegreeType, CoeffType>> GetEnumerator()
        {
            return this.terms.GetEnumerator();
        }

        /// <summary>
        /// Obtém o enumerador genérico para os termos do polinómio.
        /// </summary>
        /// <returns>O enumerador gen+erico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
