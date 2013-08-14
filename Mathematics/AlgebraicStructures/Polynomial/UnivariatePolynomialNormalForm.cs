using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mathematics.Algorithms;
using Utilities.Collections;

namespace Mathematics
{
    /// <summary>
    /// Representa um polinómio com apenas uma variável escrito na sua forma normal.
    /// </summary>
    /// <remarks>
    /// O modo de funcionamento deste tipo de polinómios é em tudo semelhante ao dos polinómios gerais diferindo
    /// apenas em questões de desempenho.
    /// </remarks>
    /// <typeparam name="ObjectType">O tipo de dados dos coeficientes.</typeparam>
    /// <typeparam name="RingType">O tipo de dados do anel responsável pelas respectivas operações.</typeparam>
    public class UnivariatePolynomialNormalForm<CoeffType, RingType>
        where RingType : IRing<CoeffType>
    {
        RingType ring;

        private Dictionary<int, CoeffType> terms;

        private string variableName;

        private UnivariatePolynomialNormalForm(RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }

            this.ring = ring;
        }

        public UnivariatePolynomialNormalForm(string variable, RingType ring)
            : this(ring)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                this.ring = ring;
                this.terms = new Dictionary<int, CoeffType>();
                this.variableName = variable;
            }
        }

        public UnivariatePolynomialNormalForm(CoeffType coeff, int degree, string variable, RingType ring) :
            this(variable, ring)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else if (degree < 0)
            {
                throw new ArgumentException("Negative degrees aren't allowed.");
            }
            else if (!ring.IsAdditiveUnity(coeff))
            {
                this.terms.Add(degree, coeff);
            }
        }


        public UnivariatePolynomialNormalForm(IDictionary<int, CoeffType> terms, string variable, RingType ring)
            : this(variable, ring)
        {
            if (terms == null)
            {
                throw new ArgumentNullException("terms");
            }
            else
            {
                foreach (var kvp in terms)
                {
                    if (kvp.Key < 0)
                    {
                        throw new ArgumentException("Negative degrees aren't allowed.");
                    }
                    else if (!this.ring.IsAdditiveUnity(kvp.Value))
                    {
                        this.terms.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o nome da variável.
        /// </summary>
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
                        return currentTerm.Key == 0;
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
        public int Degree
        {
            get
            {
                var result = 0;
                foreach (var kvp in terms)
                {
                    if (kvp.Key > result)
                    {
                        result = kvp.Key;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o polinómio é unitário.
        /// </summary>
        public bool IsUnity
        {
            get
            {
                var termsEnumerator = this.terms.GetEnumerator();
                if (termsEnumerator.MoveNext())
                {
                    var currentValue = termsEnumerator.Current;
                    if (currentValue.Key == 0)
                    {
                        if (termsEnumerator.MoveNext())
                        {
                            return false;
                        }
                        else if (this.ring.IsMultiplicativeUnity(currentValue.Value))
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
        /// <returns>O polinómio.</returns>
        public CoeffType GetAsValue()
        {
            var termsEnumerator = this.terms.GetEnumerator();
            if (termsEnumerator.MoveNext())
            {
                var currentTerm = termsEnumerator.Current;
                if (termsEnumerator.MoveNext() || currentTerm.Key != 0)
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
                return this.ring.AdditiveUnity;
            }
        }

        /// <summary>
        /// Obtém uma cópia do polinómio corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Clone()
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
            result.variableName = this.variableName;
            result.terms = new Dictionary<int, CoeffType>();
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
        public UnivariatePolynomialNormalForm<CoeffType, RingType> GetLeadingMonomial()
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
            result.variableName = this.variableName;
            var greatestDegree = 0;
            var greatestCoeff = this.ring.AdditiveUnity;
            var termsEnum = this.terms.GetEnumerator();
            if (termsEnum.MoveNext())
            {
                greatestDegree = termsEnum.Current.Key;
                greatestCoeff = termsEnum.Current.Value;
                while (termsEnum.MoveNext())
                {
                    if (greatestDegree < termsEnum.Current.Key)
                    {
                        greatestDegree = termsEnum.Current.Key;
                        greatestCoeff = termsEnum.Current.Value;
                    }
                }
            }

            result.terms = new Dictionary<int, CoeffType>();
            result.terms.Add(greatestDegree, greatestCoeff);
            return result;
        }

        /// <summary>
        /// Obtém o monómio com o menor grau.
        /// </summary>
        /// <returns>O monómio.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> GetTailMonomial()
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
            result.variableName = this.variableName;
            var greatestDegree = 0;
            var greatestCoeff = this.ring.AdditiveUnity;
            var termsEnum = this.terms.GetEnumerator();
            if (termsEnum.MoveNext())
            {
                greatestDegree = termsEnum.Current.Key;
                greatestCoeff = termsEnum.Current.Value;
                while (termsEnum.MoveNext())
                {
                    if (greatestDegree > termsEnum.Current.Key)
                    {
                        greatestDegree = termsEnum.Current.Key;
                        greatestCoeff = termsEnum.Current.Value;
                    }
                }
            }

            result.terms = new Dictionary<int, CoeffType>();
            result.terms.Add(greatestDegree, greatestCoeff);
            return result;
        }

        /// <summary>
        /// Obtém a derivada do polinómio corrente.
        /// </summary>
        /// <returns>A derivada.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> GetPolynomialDerivative()
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
            result.variableName = this.variableName;
            foreach (var termKvp in this.terms)
            {
                if (termKvp.Key > 0)
                {

                }
            }

            throw new NotImplementedException();
        }

        #region Operações
        /// <summary>
        /// Obtém a soma do polinómio corrente com o polinómio providenciado.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <returns>A soma.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Add(
            UnivariatePolynomialNormalForm<CoeffType, RingType> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (right.variableName != this.variableName)
            {
                throw new MathematicsException("Can't multiply two univariate polynomials with different variable names.");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = this.variableName;
                result.terms = new Dictionary<int, CoeffType>();
                foreach (var term in this.terms)
                {
                    result.terms.Add(term.Key, term.Value);
                }

                foreach (var term in right.terms)
                {
                    var coeff = default(CoeffType);
                    if (result.terms.TryGetValue(term.Key, out coeff))
                    {
                        coeff = this.ring.Add(coeff, term.Value);
                        if (this.ring.IsAdditiveUnity(coeff))
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
        /// <param name="right">O termo constante.</param>
        /// <returns>A soma.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Add(CoeffType coeff)
        {
            return this.Add(coeff, 0);
        }

        /// <summary>
        /// Obtém a soma do polinómio corrente com um monómio.
        /// </summary>
        /// <param name="right">Os elementos do monómio.</param>
        /// <param name="degree">O grau do coeficiente.</param>
        /// <returns>A soma.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Add(CoeffType coeff, int degree)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = this.variableName;
                result.terms = new Dictionary<int, CoeffType>();
                var degreeCoeff = coeff;
                foreach (var kvp in this.terms)
                {
                    if (kvp.Key == degree)
                    {
                        degreeCoeff = this.ring.Add(degreeCoeff, kvp.Value);
                    }
                    else
                    {
                        result.terms.Add(kvp.Key, kvp.Value);
                    }
                }

                if (!this.ring.IsAdditiveUnity(degreeCoeff))
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
        /// <returns>A diferença.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Subtract(
            UnivariatePolynomialNormalForm<CoeffType, RingType> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (right.variableName != this.variableName)
            {
                throw new MathematicsException("Can't multiply two univariate polynomials with different variable names.");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = this.variableName;
                result.terms = new Dictionary<int, CoeffType>();
                foreach (var term in this.terms)
                {
                    result.terms.Add(term.Key, term.Value);
                }

                foreach (var term in right.terms)
                {
                    var coeff = default(CoeffType);
                    if (result.terms.TryGetValue(term.Key, out coeff))
                    {
                        coeff = this.ring.Add(coeff, this.ring.AdditiveInverse(term.Value));
                        if (this.ring.IsAdditiveUnity(coeff))
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
                        result.terms.Add(term.Key, this.ring.AdditiveInverse(term.Value));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a diferença entre o polinómio corrente e um termo constante.
        /// </summary>
        /// <param name="right">O termo constante.</param>
        /// <returns>A diferença.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Subtract(CoeffType coeff)
        {
            return this.Subtract(coeff, 0);
        }

        /// <summary>
        /// Obtém a diferença entre o polinómio corrente e um monómio.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="degree">O grau.</param>
        /// <returns>A diferença.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Subtract(CoeffType coeff, int degree)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = this.variableName;
                result.terms = new Dictionary<int, CoeffType>();
                var degreeCoeff = this.ring.AdditiveInverse(coeff);
                foreach (var kvp in this.terms)
                {
                    if (kvp.Key == degree)
                    {
                        degreeCoeff = this.ring.Add(degreeCoeff, kvp.Value);
                    }
                    else
                    {
                        result.terms.Add(kvp.Key, kvp.Value);
                    }
                }

                if (!this.ring.IsAdditiveUnity(degreeCoeff))
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
        /// <returns>O produto.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Multiply(
            UnivariatePolynomialNormalForm<CoeffType, RingType> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (right.variableName != this.variableName)
            {
                throw new MathematicsException("Can't multiply two univariate polynomials with different variable names.");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = this.variableName;
                result.terms = new Dictionary<int, CoeffType>();
                foreach (var thisTerm in this.terms)
                {
                    if (!this.ring.IsAdditiveUnity(thisTerm.Value))
                    {
                        foreach (var rightTerm in right.terms)
                        {
                            if (!this.ring.IsAdditiveUnity(rightTerm.Value))
                            {
                                var totalDegree = thisTerm.Key + rightTerm.Key;
                                var totalCoeff = this.ring.Multiply(thisTerm.Value, rightTerm.Value);
                                var sumCoeff = default(CoeffType);
                                if (result.terms.TryGetValue(totalDegree, out sumCoeff))
                                {
                                    sumCoeff = this.ring.Add(sumCoeff, totalCoeff);
                                    if (this.ring.IsAdditiveUnity(sumCoeff))
                                    {
                                        result.terms.Remove(totalDegree);
                                    }
                                    else
                                    {
                                        result.terms[totalDegree] = sumCoeff;
                                    }
                                }
                                else
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
        /// <param name="right">O termo constante.</param>
        /// <returns>O produto.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Multiply(CoeffType coeff)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = this.variableName;
                result.terms = new Dictionary<int, CoeffType>();
                if (!this.ring.IsAdditiveUnity(coeff))
                {
                    foreach (var thisTerm in this.terms)
                    {
                        result.terms.Add(thisTerm.Key, this.ring.Multiply(thisTerm.Value, coeff));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o polinómio simétrico do actual.
        /// </summary>
        /// <returns>O polinómio simétrico do actual.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> GetSymmetric()
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
            result.variableName = this.variableName;
            result.terms = new Dictionary<int, CoeffType>();
            foreach (var kvp in this.terms)
            {
                result.terms.Add(kvp.Key, this.ring.AdditiveInverse(kvp.Value));
            }

            return result;
        }

        /// <summary>
        /// Substitui a variável pelo coeficiente especificado e calcula o resultado.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <returns>O resultado.</returns>
        public CoeffType Replace(CoeffType coeff)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var ordered = new InsertionSortedCollection<int>(Comparer<int>.Default);
                foreach (var term in this.terms)
                {
                    ordered.InsertSortElement(term.Key);
                }

                if (ordered.Count > 0)
                {
                    var result = this.terms[ordered.Last];
                    var previousDegree = ordered.Last;
                    ordered.RemoveElement(previousDegree);
                    while (ordered.Count > 0)
                    {
                        var power = MathFunctions.Power(coeff, previousDegree - ordered.Last, this.ring);
                        result = this.ring.Multiply(result, power);
                        result = this.ring.Add(result, this.terms[ordered.Last]);
                        previousDegree = ordered.Last;
                        ordered.RemoveElement(previousDegree);
                    }

                    var lastPower = MathFunctions.Power(coeff, previousDegree, this.ring);
                    result = this.ring.Multiply(result, lastPower);
                    return result;
                }
                else
                {
                    return this.ring.AdditiveUnity;
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
                var ordered = new InsertionSortedCollection<int>(Comparer<int>.Default);
                foreach (var term in this.terms)
                {
                    ordered.InsertSortElement(term.Key);
                }

                if (ordered.Count > 0)
                {
                    var result = algebra.MultiplyScalar(this.terms[ordered.Last], algebra.MultiplicativeUnity);
                    var previousDegree = ordered.Last;
                    ordered.RemoveElement(previousDegree);
                    while (ordered.Count > 0)
                    {
                        var power = MathFunctions.Power(value, previousDegree - ordered.Last, algebra);
                        result = algebra.Multiply(result, power);
                        result = algebra.Add(result, algebra.MultiplyScalar(this.terms[ordered.Last], algebra.MultiplicativeUnity));
                        previousDegree = ordered.Last;
                        ordered.RemoveElement(previousDegree);
                    }

                    var lastPower = MathFunctions.Power(value, previousDegree, algebra);
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
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Replace(string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = variableName;
                result.terms = new Dictionary<int, CoeffType>();
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
        /// <returns>O resultado da substituição.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Replace(
            UnivariatePolynomialNormalForm<CoeffType, RingType> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            else
            {
                var polynomialRing = new UnivarPolynomRing<CoeffType, RingType>(this.variableName, this.ring);
                var ordered = new InsertionSortedCollection<int>(Comparer<int>.Default);
                foreach (var term in this.terms)
                {
                    ordered.InsertSortElement(term.Key);
                }

                if (ordered.Count > 0)
                {
                    var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.terms[ordered.Last], 0, this.variableName, this.ring);
                    var previousDegree = ordered.Last;
                    ordered.RemoveElement(previousDegree);
                    while (ordered.Count > 0)
                    {
                        var power = MathFunctions.Power(other, previousDegree - ordered.Last, polynomialRing);
                        result = result.Multiply(power);
                        result = result.Add(this.terms[ordered.Last]);
                        previousDegree = ordered.Last;
                        ordered.RemoveElement(previousDegree);
                    }

                    var lastPower = MathFunctions.Power(other, previousDegree, polynomialRing);
                    result = result.Multiply(lastPower);
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
        /// Obtém uma representação textual do polinómio.
        /// </summary>
        /// <returns>A repesentação textual do polinómio.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            if (terms.Count == 0)
            {
                resultBuilder.Append(this.ring.AdditiveUnity);
            }
            else
            {
                var insertedSortedCollection = new InsertionSortedCollection<int>(
                    Comparer<int>.Default,
                    true);
                foreach (var term in this.terms)
                {
                    insertedSortedCollection.InsertSortElement(term.Key);
                }

                var invertedEnum = insertedSortedCollection.GetReversedEnumerator();
                if (invertedEnum.MoveNext())
                {
                    var currentDegree = invertedEnum.Current;
                    var currentValue = this.terms[invertedEnum.Current];
                    resultBuilder.AppendFormat("{0}", currentValue);
                    if (currentDegree == 1)
                    {
                        resultBuilder.AppendFormat("*{0}", this.variableName);
                    }
                    else if (currentDegree > 1)
                    {
                        resultBuilder.AppendFormat("*{0}^{1}", this.variableName, currentDegree);
                    }

                    while (invertedEnum.MoveNext())
                    {
                        resultBuilder.Append("+");
                        currentDegree = invertedEnum.Current;
                        currentValue = this.terms[invertedEnum.Current];
                        resultBuilder.AppendFormat("{0}", currentValue);
                        if (currentDegree == 1)
                        {
                            resultBuilder.AppendFormat("*{0}", this.variableName);
                        }
                        else if (currentDegree > 1)
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
        public SortedList<int, CoeffType> GetOrderedCoefficients(
            IComparer<int> degreeComparer)
        {
            var result = new SortedList<int, CoeffType>(degreeComparer);
            foreach (var kvp in this.terms)
            {
                result.Add(kvp.Key, kvp.Value);
            }

            return result;
        }
    }
}
