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
        private Dictionary<List<int>, T> polynomialTerms;

        private SymmetricPolynomial()
        {
            this.polynomialTerms = new Dictionary<List<int>, T>(this.degreeComparer);
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
            this.polynomialTerms = new Dictionary<List<int>, T>(this.degreeComparer);
            this.variables.AddRange(variables);
        }

        public SymmetricPolynomial(List<string> variables, List<int> degree, T coeff, R ring) : this(variables, ring)
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
        private List<int> GetSimplifiedDegree(List<int> degree)
        {
            var insertedSortedCollection = new InsertionSortedCollection<int>(Comparer<int>.Default, false);
            foreach (var degreeItem in degree)
            {
                if (degreeItem > 0)
                {
                    insertedSortedCollection.InsertSortElement(degreeItem);
                }
                else if (degreeItem != 0)
                {
                    throw new MathematicsException("Negative degrees are not allowed.");
                }
            }

            var result = new List<int>();
            foreach (var element in insertedSortedCollection)
            {
                result.Insert(0, element);
            }

            return insertedSortedCollection.ToList();
        }
    }
}
