using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mathematics.Algorithms;

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

        public bool IsMonomial
        {
            get
            {
                return this.terms.Count < 2;
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
            else if(!this.ring.Equals(right.ring)){
                throw new MathematicsException("Can't add polynomials with coefficients in different rings.");
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
            else if (!this.ring.Equals(right.ring))
            {
                throw new MathematicsException("Can't add polynomials with coefficients in different rings.");
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
                        result.terms.Add(term.Key, term.Value);
                    }
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
            else if (!this.ring.Equals(right.ring))
            {
                throw new MathematicsException("Can't add polynomials with coefficients in different rings.");
            }
            else if (right.variableName != this.variableName)
            {
                throw new MathematicsException("Can't multiply two univariate polynomials with different variable names.");
            }
            else
            {
                var result = new UnivariatePolynomialNormalForm<CoeffType, RingType>(this.ring);
                result.variableName = this.variableName;
                result.terms = new Dictionary<int,CoeffType>();
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
                var result = this.ring.AdditiveUnity;
                foreach (var term in this.terms)
                {
                    var powered = MathFunctions.Power(coeff, term.Key, this.ring);
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Substitui a variável corrente por uma outra.
        /// </summary>
        /// <param name="variableName">O nome da variável.</param>
        /// <returns>O polinómio com a variável substituída.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Replace(string variableName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Substitui a variável pelo polinómio especificado e calcula o resultado.
        /// </summary>
        /// <param name="other">O polinómio a substituir.</param>
        /// <returns>O resultado da substituição.</returns>
        public UnivariatePolynomialNormalForm<CoeffType, RingType> Replace(
            UnivariatePolynomialNormalForm<CoeffType, RingType> other)
        {
            throw new NotImplementedException();
        }
        #endregion Operações
    }
}
