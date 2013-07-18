using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Utilities.Collections;

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

        public SymmetricPolynomial(List<string> variables, List<int> degree, T coeff, R ring)
            : this(variables, ring)
        {
            var innerDegree = this.GetSimplifiedDegree(degree);
            if (degree.Count > this.variables.Count)
            {
                throw new ArgumentException("The number of non zero elements on degree must not surpass the number of defined variables.");
            }

            if (!this.ring.IsAdditiveUnity(coeff))
            {
                this.polynomialTerms.Add(innerDegree, coeff);
            }
        }

        public SymmetricPolynomial(List<string> variables, List<int> degree, T coeff, R ring)
            : this(variables, ring)
        {
            var innerDegree = this.GetSimplifiedDegree(degree);
            if (!this.ring.IsAdditiveUnity(coeff))
            {
                this.polynomialTerms.Add(innerDegree, coeff);
            }
        }

        public SymmetricPolynomial(List<string> variables, Dictionary<int, int> degree, T coeff, R ring)
            : this(variables, ring)
        {
            throw new NotImplementedException();
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

                }
            }

            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
                        throw new ArgumentException("The degree in symmetric polynomial can't be zero.");
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
            IntegerFactorialFraction permutationDegreeNumber;
            if (firstDegreeNumberNumerator < secondDegreeNumberNumerator)
            {
                permutationDegree = this.ExpandDegree(firstMonomialDegree);
                fixedDegree = this.ExpandDegree(secondMonomialDegree);
                permutationDegreeNumber = firstDegreeNumber;
            }
            else
            {
                permutationDegree = this.ExpandDegree(secondMonomialDegree);
                fixedDegree = this.ExpandDegree(firstMonomialDegree);
                permutationDegreeNumber = secondDegreeNumber;
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
                // TODO: somar o grau à variável degrees count e, fora do ciclo, proceder ao respectivo processamento.
            }

            throw new NotImplementedException();
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
    }
}
