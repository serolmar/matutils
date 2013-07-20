using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Utilities.Collections;
using Mathematics.Algorithms;

namespace Mathematics.AlgebraicStructures.Polynomial
{
    /// <summary>
    /// Representa um polinómio simétrico.
    /// </summary>
    /// <remarks>
    /// Dada a natureza particular dos polinómios simétricos, estes podem ser adicionados
    /// e multiplicados entre si de uma forma mais rápida do que no caso em que são considerados
    /// como polinómios habituais.
    /// </remarks>
    /// <typeparam name="T">O tipo de coeficiente no polinómio.</typeparam>
    /// <typeparam name="R">O anel que permite adicionar e multiplicar os respectivos coeficientes</typeparam>
    public class SymmetricPolynomial<T, R>
        where R : IRing<T>
    {
        /// <summary>
        /// O anel correspondente ao polinómio simétrico.
        /// </summary>
        R ring;

        /// <summary>
        /// O comparador de graus para o polinómio simétrico.
        /// </summary>
        private SymmetricPolynomialDegreeEqualityComparer degreeComparer = new SymmetricPolynomialDegreeEqualityComparer();

        /// <summary>
        /// Mantém a lista das variáveis.
        /// </summary>
        private List<string> variables = new List<string>();

        /// <summary>
        /// Mantém a lista dos termos do polinómio simétrico.
        /// </summary>
        private Dictionary<Dictionary<int, int>, T> polynomialTerms;

        private SymmetricPolynomial()
        {
            this.polynomialTerms = new Dictionary<Dictionary<int, int>, T>(this.degreeComparer);
        }

        public SymmetricPolynomial(List<string> variables, R ring)
        {
            if (variables == null || variables.Count == 0)
            {
                throw new ArgumentException("No variable was provided for symmetric polynomial.");
            }

            if (ring == null)
            {
                throw new MathematicsException("A ring must be provided for symmetric polynomial.");
            }

            var containsVariable = new Dictionary<string, bool>();
            foreach (var variable in variables)
            {
                if (containsVariable.ContainsKey(variable))
                {
                    throw new ArgumentException("Repeated variables in variables list aren't allowed.");
                }
                else
                {
                    containsVariable.Add(variable, true);
                }
            }

            this.ring = ring;
            this.polynomialTerms = new Dictionary<Dictionary<int, int>, T>(this.degreeComparer);
            this.variables.AddRange(variables);
        }

        public SymmetricPolynomial(List<string> variables, int elementarySymmIndex, R ring)
            : this(variables, ring)
        {
            if (elementarySymmIndex < 0 || elementarySymmIndex > variables.Count)
            {
                throw new IndexOutOfRangeException("The specified elementary symmetric index must be between zero and the number of variables.");
            }
            else
            {

            }
        }

        public SymmetricPolynomial(List<string> variables, List<int> degree, T coeff, R ring)
            : this(variables, ring)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }

            var innerDegree = this.GetSimplifiedDegree(degree);
            if (degree.Count > this.variables.Count)
            {
                throw new ArgumentException("The number elements on degree must not surpass the number of defined variables.");
            }

            if (!this.ring.IsAdditiveUnity(coeff))
            {
                this.polynomialTerms.Add(innerDegree, coeff);
            }
        }

        public SymmetricPolynomial(List<string> variables, Dictionary<int, int> degree, T coeff, R ring)
            : this(variables, ring)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }

            if (degree != null && !ring.IsAdditiveUnity(coeff))
            {
                var innerDegree = new Dictionary<int, int>();
                var degreeCount = 0;
                foreach (var kvp in degree)
                {
                    if (kvp.Key < 0)
                    {
                        throw new ArgumentException("Every degree in symmetric polynomial can't be negative.");
                    }
                    else if (kvp.Value <= 0)
                    {
                        throw new ArgumentException("Every degree count in symmetric polynomial must be non-negative.");
                    }
                    else
                    {
                        ++degreeCount;
                        innerDegree.Add(kvp.Key, kvp.Value);
                    }
                }

                if (degreeCount != variables.Count)
                {
                    throw new ArgumentException("The number elements on degree must not surpass the number of defined variables.");
                }
                else
                {
                    this.polynomialTerms.Add(innerDegree, coeff);
                }
            }
        }

        /// <summary>
        /// Obtém a lista de variáveis.
        /// </summary>
        public ReadOnlyCollection<string> Variables
        {
            get
            {
                return this.variables.AsReadOnly();
            }
        }

        /// <summary>
        /// Retorna o resultado da adição do polinómio corrente com o polinómio proporcionado.
        /// </summary>
        /// <param name="right">O polinómio proporcionado.</param>
        /// <returns>A soma de ambos os polinómios.</returns>
        public SymmetricPolynomial<T, R> Add(SymmetricPolynomial<T, R> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            foreach (var variable in this.variables)
            {
                if (!right.variables.Any(v => v == variable))
                {
                    throw new ArgumentException("The polynomials must have the same variables in order to be added.");
                }
            }

            var result = new SymmetricPolynomial<T, R>();
            result.variables.AddRange(this.variables);
            result.ring = this.ring;
            foreach (var term in this.polynomialTerms)
            {
                result.polynomialTerms.Add(term.Key, term.Value);
            }

            foreach (var term in right.polynomialTerms)
            {
                var coeff = default(T);
                if (result.polynomialTerms.TryGetValue(term.Key, out coeff))
                {
                    coeff = result.ring.Add(coeff, term.Value);
                    if (this.ring.IsAdditiveUnity(coeff))
                    {
                        result.polynomialTerms.Remove(term.Key);
                    }
                    else
                    {
                        result.polynomialTerms[term.Key] = coeff;
                    }
                }
                else
                {
                    result.polynomialTerms.Add(term.Key, term.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Retorna o resultado da subtracção do polinómio corrente com o polinómio proporcionado.
        /// </summary>
        /// <param name="right">O polinómio proporcionado.</param>
        /// <returns>A diferença entre ambos os polinómios.</returns>
        public SymmetricPolynomial<T, R> Subtract(SymmetricPolynomial<T, R> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            foreach (var variable in this.variables)
            {
                if (!right.variables.Any(v => v == variable))
                {
                    throw new ArgumentException("The polynomials must have the same variables in order to be added.");
                }
            }

            var result = new SymmetricPolynomial<T, R>();
            result.variables.AddRange(this.variables);
            result.ring = this.ring;
            foreach (var term in this.polynomialTerms)
            {
                result.polynomialTerms.Add(term.Key, term.Value);
            }

            foreach (var term in right.polynomialTerms)
            {
                var coeff = default(T);
                if (result.polynomialTerms.TryGetValue(term.Key, out coeff))
                {
                    var subtractCoeff = this.ring.AdditiveInverse(term.Value);
                    coeff = result.ring.Add(coeff, subtractCoeff);
                    if (this.ring.IsAdditiveUnity(coeff))
                    {
                        result.polynomialTerms.Remove(term.Key);
                    }
                    else
                    {
                        result.polynomialTerms[term.Key] = coeff;
                    }
                }
                else
                {
                    result.polynomialTerms.Add(term.Key, term.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Retorna o resultado da multiplicação do polinómio corrente com o polinómio proporcionado.
        /// </summary>
        /// <param name="right">O polinómio proporcionado.</param>
        /// <returns>O produto de ambos os polinómios.</returns>
        public SymmetricPolynomial<T, R> Multiply(SymmetricPolynomial<T, R> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            foreach (var variable in this.variables)
            {
                if (!right.variables.Any(v => v == variable))
                {
                    throw new ArgumentException("The polynomials must have the same variables in order to be added.");
                }
            }

            var result = new SymmetricPolynomial<T, R>();
            result.variables.AddRange(this.variables);
            result.ring = this.ring;

            foreach (var thisTerm in this.polynomialTerms)
            {
                foreach (var rightTerm in right.polynomialTerms)
                {
                    var factors = this.MultiplyMonomials(
                        thisTerm.Value,
                        thisTerm.Key,
                        rightTerm.Value,
                        rightTerm.Key);
                    foreach (var factorKvp in factors)
                    {
                        var coeff = default(T);
                        if (result.polynomialTerms.TryGetValue(factorKvp.Key, out coeff))
                        {
                            coeff = result.ring.Add(coeff, factorKvp.Value);
                            if (this.ring.IsAdditiveUnity(coeff))
                            {
                                result.polynomialTerms.Remove(factorKvp.Key);
                            }
                            else
                            {
                                result.polynomialTerms[factorKvp.Key] = coeff;
                            }
                        }
                        else
                        {
                            result.polynomialTerms.Add(factorKvp.Key, factorKvp.Value);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a representação do polinómio simétrico como combinação polinomial dos polinómios simétricos elementares.
        /// </summary>
        /// <param name="elementarySymmetricVarDefs">
        /// Um dicionário que permite mapear entre o n-ésimo polinómios simétrico e o seu nome, caso a primeira entrada do tuplo
        /// se encontre a falso e o seu valor caso esta se encontre a verdadeiro.
        /// </param>
        /// <returns>A respresentação polinomial do polinómio simétrico como combinação dos polinómios simétricos elementares.</returns>
        public Polynomial<T, R> GetElementarySymmetricRepresentation(Dictionary<int, Tuple<bool, string, T>> elementarySymmetricVarDefs)
        {
            var result = new Polynomial<T, R>(this.ring);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém uma representação do monómio correspondente em termos dos polinómios simétricos elementares.
        /// </summary>
        /// <param name="monomialDegree">O grau do monómio.</param>
        /// <param name="coeff">O coeficiente do monómio.</param>
        /// <param name="elementarySymmetricVarDefs">
        /// Um dicionário que permite mapear entre o n-ésimo polinómios simétrico e o seu nome, caso a primeira entrada do tuplo
        /// se encontre a falso e o seu valor caso esta se encontre a verdadeiro.
        /// </param>
        /// <returns>A representação polinomial.</returns>
        private Polynomial<T, R> GetElementarySymmetricMonomialRepresentation(
            Dictionary<int, int> monomialDegree,
            T coeff,
            Dictionary<int, Tuple<bool, string, T>> elementarySymmetricVarDefs)
        {
            var result = new Polynomial<T, R>(this.ring.AdditiveUnity, this.ring);
            var monomialStack = new Stack<KeyValuePair<Dictionary<int, int>, T>>();
            var operationTypeStack = new Stack<OperationType>();
            monomialStack.Push(new KeyValuePair<Dictionary<int, int>, T>(monomialDegree, coeff));
            operationTypeStack.Push(OperationType.ADD);
            while (monomialStack.Count != 0)
            {
                var topMonomial = monomialStack.Pop();
                var topOperation = operationTypeStack.Pop();
                var index = this.GetelementarySymmetricIndex(topMonomial.Key);
                if (index == -1)
                {
                    // TODO: completar esta parte da função
                }
                else
                {
                    Polynomial<T, R> symPolRes;
                    Tuple<bool, string, T> varDefs = null;
                    if (elementarySymmetricVarDefs.TryGetValue(index, out varDefs))
                    {
                        if (varDefs.Item1)
                        {
                            var variableCoeff = varDefs.Item3;
                            if (this.ring.IsAdditiveUnity(variableCoeff))
                            {
                                symPolRes = new Polynomial<T, R>(variableCoeff, this.ring);
                            }
                            else
                            {
                                variableCoeff = this.ring.Multiply(variableCoeff, topMonomial.Value);
                                symPolRes = new Polynomial<T, R>(variableCoeff, this.ring);
                            }
                        }
                        else
                        {
                            symPolRes = new Polynomial<T, R>(topMonomial.Value, varDefs.Item2, this.ring);
                        }
                    }
                    else
                    {
                        var defaultSymmetricName = this.GetElementarySymmDefaultName(index);
                        symPolRes = new Polynomial<T, R>(topMonomial.Value, defaultSymmetricName, this.ring);
                    }

                    if (topOperation == OperationType.ADD)
                    {
                        result = result.Add(symPolRes);
                    }
                    else if (topOperation == OperationType.MULTIPLY)
                    {
                        result = result.Multiply(symPolRes);
                    }
                    else
                    {
                        throw new NotImplementedException("Unknown operation type.");
                    }
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o nome por defeito do simétrico elementar.
        /// </summary>
        /// <param name="index">O índice do simétrico elementar.</param>
        /// <returns>O nome por defeito.</returns>
        private string GetElementarySymmDefaultName(int index)
        {
            return string.Format("s[{0}]", index);
        }

        /// <summary>
        /// Obtém o grau simplificado.
        /// </summary>
        /// <remarks>
        /// O grau de um polinómio simétrico consiste numa lista de inteiros,
        /// cada qual representado um termo.
        /// </remarks>
        /// <param name="degree">O grau a ser simplificado.</param>
        /// <returns>O grau simplificado.</returns>
        private Dictionary<int, int> GetSimplifiedDegree(List<int> degree)
        {
            if (degree == null)
            {
                return new Dictionary<int, int>();
            }
            else if (degree.Count > this.variables.Count)
            {
                throw new ArgumentException("The number of degrees must be the same as the number of variables.");
            }
            else
            {
                var result = new Dictionary<int, int>();
                foreach (var degreeItem in degree)
                {
                    if (degreeItem < 0)
                    {
                        throw new ArgumentException("Every degree in symmetric polynomial can't be negative.");
                    }
                    else
                    {
                        var degreeCount = 0;
                        if (result.TryGetValue(degreeItem, out degreeCount))
                        {
                            result[degreeItem] = degreeItem + 1;
                        }
                        else
                        {
                            result.Add(degreeItem, 1);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Verifica a integridade do grau.
        /// </summary>
        /// <param name="degree">O grau a ser verificado.</param>
        /// <returns>O grau.</returns>
        private Dictionary<int, int> VerifyDegree(Dictionary<int, int> degree)
        {
            if (degree == null)
            {
                return new Dictionary<int, int>();
            }
            else
            {
                var count = 0;
                foreach (var kvp in degree)
                {
                    if (kvp.Key < 0)
                    {
                        throw new ArgumentException("Negative degrees aren't allowed.");
                    }

                    count += kvp.Value;
                }

                if (count != this.variables.Count)
                {
                    throw new ArgumentException("The number of degrees must be the same as the number of variables.");
                }
                else
                {
                    return degree;
                }
            }
        }

        /// <summary>
        /// Multiplica dos monómios simétricos.
        /// </summary>
        /// <param name="firstMonomialCoeff">O coeficiente do primeiro monómio a multiplicar.</param>
        /// <param name="firstMonomialDegree">O grau do primeiro monómio a multiplicar.</param>
        /// <param name="secondMonomialCoeff">O coeficiente do segundo monómio a multiplicar.</param>
        /// <param name="secondMonomialDegree">O grau do segundo monómio a multiplicar.</param>
        /// <returns></returns>
        private Dictionary<Dictionary<int, int>, T> MultiplyMonomials(
            T firstMonomialCoeff,
            Dictionary<int, int> firstMonomialDegree,
            T secondMonomialCoeff,
            Dictionary<int, int> secondMonomialDegree)
        {
            var firstDegreeNumber = this.GetDegreeTermsNumber(firstMonomialDegree);
            var secondDegreeNumber = this.GetDegreeTermsNumber(secondMonomialDegree);
            var firstDegreeNumberNumerator = firstDegreeNumber.Numerator;
            var secondDegreeNumberNumerator = secondDegreeNumber.Numerator;

            int[] permutationDegree;
            int[] fixedDegree;
            Dictionary<int, int> fixedCompactDegree;
            if (firstDegreeNumberNumerator < secondDegreeNumberNumerator)
            {
                permutationDegree = this.ExpandDegree(firstMonomialDegree);
                fixedDegree = this.ExpandDegree(secondMonomialDegree);
                fixedCompactDegree = secondMonomialDegree;
            }
            else
            {
                permutationDegree = this.ExpandDegree(secondMonomialDegree);
                fixedDegree = this.ExpandDegree(firstMonomialDegree);
                fixedCompactDegree = firstMonomialDegree;
            }

            var result = new Dictionary<Dictionary<int, int>, T>(this.degreeComparer);

            // Conta o número de graus que resultam da multiplicação do monómios para aplicar o factor
            var degreesCount = new Dictionary<Dictionary<int, int>, int>(this.degreeComparer);

            var boxDegreeAffector = new PermutationBoxAffector(permutationDegree);
            foreach (var degreeAffectation in boxDegreeAffector)
            {
                var sumDegree = new int[fixedDegree.Length];
                for (int i = 0; i < sumDegree.Length; ++i)
                {
                    sumDegree[i] = fixedDegree[i] + degreeAffectation[i];
                }

                var compactSum = this.CompactDegree(sumDegree);
                var degreeCount = 0;
                if (degreesCount.TryGetValue(compactSum, out degreeCount))
                {
                    degreesCount[compactSum] = degreeCount + 1;
                }
                else
                {
                    degreesCount.Add(compactSum, 1);
                }
            }

            var coeffProd = this.ring.Multiply(firstMonomialCoeff, secondMonomialCoeff);
            foreach (var degreeCountKvp in degreesCount)
            {
                var scaleFactor = this.GetFactorScale(fixedCompactDegree, degreeCountKvp.Key);
                scaleFactor.NumeratorNumberMultiply(degreeCountKvp.Value);
                coeffProd = MathFunctions.AddPower(coeffProd, scaleFactor.Numerator, this.ring);
                result.Add(degreeCountKvp.Key, coeffProd);
            }

            return result;
        }

        /// <summary>
        /// Obtém o grau corresopndente ao simétrico elementar especificado pelo índice respectivo.
        /// </summary>
        /// <param name="elementaryIndex">O índice do polinómio simétrico elementar.</param>
        /// <returns>O grau do polinómio simétrico elementar.</returns>
        private Dictionary<int, int> GetElementarySymmetric(int elementaryIndex)
        {
            var result = new Dictionary<int, int>();
            if (elementaryIndex == 0)
            {
                result.Add(0, this.variables.Count);
            }
            else if (this.variables.Count == 0)
            {
                result.Add(1, elementaryIndex);
            }
            else
            {
                result.Add(1, elementaryIndex);
                result.Add(0, this.variables.Count - elementaryIndex);
            }

            return result;
        }

        /// <summary>
        /// Obtém o índice do polinómio simétrico elementar.
        /// </summary>
        /// <param name="degree">O grau do polinómio simétrico elementar.</param>
        /// <returns>O índice do polinómio simétrico elementar. Retorna -1 se não se trata de um simétrico elementar.</returns>
        private int GetelementarySymmetricIndex(Dictionary<int, int> degree)
        {
            var result = -1;
            var degreeEnumerator = degree.GetEnumerator();
            if (degreeEnumerator.MoveNext())
            {
                if (degreeEnumerator.Current.Key == 1)
                {
                    var count = degreeEnumerator.Current.Value;
                    if (degreeEnumerator.MoveNext())
                    {
                        if (degreeEnumerator.Current.Key == 0)
                        {
                            if (degreeEnumerator.MoveNext())
                            {
                                result = -1;
                            }
                            else
                            {
                                result = count;
                            }
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    else
                    {
                        result = degreeEnumerator.Current.Value;
                    }
                }
                else if (degreeEnumerator.Current.Key == 0)
                {
                    if (degreeEnumerator.MoveNext())
                    {
                        if (degreeEnumerator.Current.Key == 1)
                        {
                            if (degreeEnumerator.MoveNext())
                            {
                                result = -1;
                            }
                            else
                            {
                                result = degreeEnumerator.Current.Value;
                            }
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else
                {
                    result = -1;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o maior simétrico elementar que divide o grau considerado.
        /// </summary>
        /// <param name="degree">O grau considerado.</param>
        /// <returns>O simétrico elementar.</returns>
        private Dictionary<int, int> GetMaximumSymmetricDivisor(Dictionary<int, int> degree)
        {
            var zeroCount = 0;
            var nonZeroCount = 0;
            foreach (var degreeKvp in degree)
            {
                if (degreeKvp.Key == 0)
                {
                    ++zeroCount;
                }
                else
                {
                    ++nonZeroCount;
                }
            }

            var result = new Dictionary<int, int>();
            if (zeroCount == 0)
            {
                result.Add(1, nonZeroCount);
            }
            else if (nonZeroCount == 0)
            {
                result.Add(0, zeroCount);
            }
            else
            {
                result.Add(1, nonZeroCount);
                result.Add(0, nonZeroCount);
            }

            return result;
        }

        /// <summary>
        /// Obtém o número de termos do polinómio caso este fosse expandido.
        /// </summary>
        /// <param name="degree">O grau.</param>
        /// <returns>O número correspondente de termos.</returns>
        private IntegerFactorialFraction GetDegreeTermsNumber(Dictionary<int, int> degree)
        {
            var result = new IntegerFactorialFraction();
            var count = 0;
            foreach (var kvp in degree)
            {
                count += kvp.Value;
                result.MultiplyDenominator(kvp.Value);
            }

            result.MultiplyNumerator(count);
            return result;
        }

        /// <summary>
        /// Obtém o factor de escala a utilizar durante a multiplicação de monómios simétricos.
        /// </summary>
        /// <param name="firstMonomialDegree">O grau do primeiro monómio.</param>
        /// <param name="secondMonomialDegree">O grau do segundo monómio.</param>
        /// <returns></returns>
        private IntegerFactorialFraction GetFactorScale(Dictionary<int, int> firstMonomialDegree, Dictionary<int, int> secondMonomialDegree)
        {
            var result = new IntegerFactorialFraction();
            foreach (var kvp in firstMonomialDegree)
            {
                result.MultiplyNumerator(kvp.Value);
            }

            foreach (var kvp in secondMonomialDegree)
            {
                result.MultiplyDenominator(kvp.Value);
            }

            return result;
        }

        /// <summary>
        /// Expande o grau.
        /// </summary>
        /// <param name="expandDegree">O grau a expandir.</param>
        /// <returns>O grau expandido.</returns>
        private int[] ExpandDegree(Dictionary<int, int> expandDegree)
        {
            var result = new List<int>();
            foreach (var kvp in expandDegree)
            {
                for (int i = 0; i < kvp.Value; ++i)
                {
                    result.Add(kvp.Key);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Compacta o grau.
        /// </summary>
        /// <param name="compactDegree">O grau a compactar.</param>
        /// <returns>O grau compactado.</returns>
        private Dictionary<int, int> CompactDegree(IEnumerable<int> compactDegree)
        {
            var result = new Dictionary<int, int>();
            foreach (var degreeItem in compactDegree)
            {
                var degreeCount = 0;
                if (result.TryGetValue(degreeItem, out degreeCount))
                {
                    result[degreeItem] = degreeItem + 1;
                }
                else
                {
                    result.Add(degreeItem, 1);
                }
            }

            return result;
        }

        /// <summary>
        /// Serve para identificar o tipo de operação que é realizada nas etapas da determinação
        /// de uma representação do polinómio simétrico em termos dos polinómios simétricos elementares.
        /// </summary>
        private enum OperationType
        {
            ADD,
            MULTIPLY
        }
    }
}
