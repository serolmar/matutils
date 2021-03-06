﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;
    using Mathematics.Algorithms;

    /// <summary>
    /// Representa um polinómio simétrico.
    /// </summary>
    /// <remarks>
    /// Dada a natureza particular dos polinómios simétricos, estes podem ser adicionados
    /// e multiplicados entre si de uma forma mais rápida do que no caso em que são considerados
    /// como polinómios habituais.
    /// </remarks>
    /// <typeparam name="T">O tipo de coeficiente no polinómio.</typeparam>
    public class SymmetricPolynomial<T>
    {
        /// <summary>
        /// O comparador de graus para o polinómio simétrico.
        /// </summary>
        private SymmetricPolynomialDegreeEqualityComparer degreeComparer = 
            new SymmetricPolynomialDegreeEqualityComparer();

        /// <summary>
        /// Mantém a lista das variáveis.
        /// </summary>
        private List<string> variables = new List<string>();

        /// <summary>
        /// Mantém a lista dos termos do polinómio simétrico.
        /// </summary>
        private Dictionary<Dictionary<int, int>, T> polynomialTerms;

        /// <summary>
        /// Inibe a instanciação por defeito de objectos do tipo <see cref="SymmetricPolynomial{T}"/>.
        /// </summary>
        private SymmetricPolynomial()
        {
            this.polynomialTerms = new Dictionary<Dictionary<int, int>, T>(this.degreeComparer);
        }

        /// <summary>
        /// Permite construir um polinómio simétrico nulo com base numa lista de variáveis.
        /// </summary>
        /// <param name="variables">A lista de variáveis.</param>
        /// <exception cref="ArgumentException">
        /// Se a lista de variáveis for nula ou vazia ou existirem variáveis repetidas.</exception>
        public SymmetricPolynomial(List<string> variables)
        {
            if (variables == null || variables.Count == 0)
            {
                throw new ArgumentException("No variable was provided for symmetric polynomial.");
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

            this.polynomialTerms = new Dictionary<Dictionary<int, int>, T>(this.degreeComparer);
            this.variables.AddRange(variables);
        }

        /// <summary>
        /// Permite construir o polinómio simétrico elementar com base no respectivo índice.
        /// </summary>
        /// <remarks>
        /// O índice de cada polinómio simétrico elementar varia entre zero e o número de itens na lista
        /// de variáveis. Por exemplo, s[0] corresponde à unidade, s[1] corresponde à soma das variáveis e,
        /// em geral, s[i] corresponde à soma das combinações de i variáveis. Neste caso, i designa o índice
        /// do polinómio simétrico elementar.
        /// </remarks>
        /// <param name="coefficient">O coeficiente multiplicativo do polinómio simétrico elementar.</param>
        /// <param name="variables">A lista de variáveis.</param>
        /// <param name="elementarySymmIndex">O índice do polinómio simétrico elemnetar.</param>
        /// <param name="monoid">O monóide responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// Se o índice para as funções simétricas elementares não definir nenhuma destas funções.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Se o coeficiente ou o monóide forem nulos.
        /// </exception>
        /// <exception cref="ArgumentException">Se existirem variáveis repetidas.</exception>
        public SymmetricPolynomial(
            T coefficient, 
            List<string> variables, 
            int elementarySymmIndex,
            IMonoid<T> monoid)
            : this(variables)
        {
            if (elementarySymmIndex < 0 || elementarySymmIndex > variables.Count)
            {
                throw new IndexOutOfRangeException(
                    "The specified elementary symmetric index must be between zero and the number of variables.");
            }
            else if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (coefficient == null)
            {
                throw new ArgumentNullException("coefficient");
            }
            else
            {
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

                this.polynomialTerms = new Dictionary<Dictionary<int, int>, T>(this.degreeComparer);
                this.variables.AddRange(variables);

                if (!monoid.IsAdditiveUnity(coefficient))
                {
                    var totalDegree = variables.Count;
                    var degreeDictionary = new Dictionary<int, int>();
                    if (elementarySymmIndex == 0) // O valor uniário.
                    {
                        degreeDictionary.Add(0, totalDegree);
                    }
                    else if (elementarySymmIndex == totalDegree)
                    {
                        degreeDictionary.Add(1, elementarySymmIndex);
                    }
                    else
                    {
                        degreeDictionary.Add(0, totalDegree - elementarySymmIndex);
                        degreeDictionary.Add(1, elementarySymmIndex);
                    }

                    this.polynomialTerms.Add(degreeDictionary, coefficient);
                }
            }
        }

        /// <summary>
        /// Permite constuir um monómio simétrico providenciando os graus das variáveis nas várias combinações.
        /// </summary>
        /// <remarks>
        /// A título de exemplo, nas variáveis x e y, o polinómio simétrico associado à lista [1,2] é da forma
        /// xy^2+x^2y onde é construído, em primeiro lugar, o monómio xy^2 e de seguida são aplicadas todas as
        /// permutações relevantes sobre as variáveis.
        /// </remarks>
        /// <param name="variables">A lista de variáveis que consituem o polinómio simétrico.</param>
        /// <param name="degree">A lista dos graus.</param>
        /// <param name="coeff">O coeficientes multiplicativo do monómio simétrico.</param>
        /// <param name="monoid">O anel responsável pela determinação da nulidade dos coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Se o coeficiente ou o monóide forem nulos.
        /// </exception>
        /// <exception cref="ArgumentException">Se existirem variáveis repetidas.</exception>
        public SymmetricPolynomial(List<string> variables, List<int> degree, T coeff, IMonoid<T> monoid)
            : this(variables)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
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

                var innerDegree = this.GetSimplifiedDegree(degree);
                if (degree.Count > this.variables.Count)
                {
                    throw new ArgumentException("The number elements on degree must not surpass the number of defined variables.");
                }

                if (!monoid.IsAdditiveUnity(coeff))
                {
                    this.polynomialTerms.Add(innerDegree, coeff);
                }
            }
        }

        /// <summary>
        /// Permite construir um monómio simétrico com base numa lista de variáveis e num descritor de graus.
        /// </summary>
        /// <remarks>
        /// Considerando a lista de variáveis [x,y,z], o monómio simétrico associado à lista de graus [1,2,3]
        /// é da forma xy^2z^3+xy^3z^2+..., onde os restantes termos são dados a partir das permutações 
        /// pertinentes das variáveis. No entanto, a maior parte da lista de graus contém elementos repetidos,
        /// tais como [0,0,0,1,1]. Assim, é útil passar um dicionário que contenha os graus e a respectiva
        /// contagem.
        /// </remarks>
        /// <param name="variables">O conjunto de variáveis.</param>
        /// <param name="degree">O dicionário que contém a contagem dos graus.</param>
        /// <param name="coeff">O coeficiente multiplicativo.</param>
        /// <param name="monoid">O monóide responsável pela determinação da nulidade dos coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Se o coeficiente ou o monóide forem nulos.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Se existirem variáveis repetidas ou se algum grau for negativo.
        /// </exception>
        public SymmetricPolynomial(List<string> variables, Dictionary<int, int> degree, T coeff, IMonoid<T> monoid)
            : this(variables)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }
            else if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
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

                if (degree != null && !monoid.IsAdditiveUnity(coeff))
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
                            degreeCount += kvp.Value;
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
        }

        /// <summary>
        /// Obtém a lista de variáveis.
        /// </summary>
        /// <value>A lista de variáveis.</value>
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
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>A soma de ambos os polinómios.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="ArgumentException">Se as variáveis não conicidirem.</exception>
        public SymmetricPolynomial<T> Add(SymmetricPolynomial<T> right, IMonoid<T> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            foreach (var variable in this.variables)
            {
                if (!right.variables.Any(v => v == variable))
                {
                    throw new ArgumentException(
                        "The polynomials must have the same variables in order to be added.");
                }
            }

            var result = new SymmetricPolynomial<T>();
            result.variables.AddRange(this.variables);
            foreach (var term in this.polynomialTerms)
            {
                result.polynomialTerms.Add(term.Key, term.Value);
            }

            foreach (var term in right.polynomialTerms)
            {
                var coeff = default(T);
                if (result.polynomialTerms.TryGetValue(term.Key, out coeff))
                {
                    coeff = monoid.Add(coeff, term.Value);
                    if (monoid.IsAdditiveUnity(coeff))
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
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>A diferença entre ambos os polinómios.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="ArgumentException">Se as variáveis não conicidirem.</exception>
        public SymmetricPolynomial<T> Subtract(SymmetricPolynomial<T> right, IGroup<T> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("monoid");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            foreach (var variable in this.variables)
            {
                if (!right.variables.Any(v => v == variable))
                {
                    throw new ArgumentException(
                        "The polynomials must have the same variables in order to be added.");
                }
            }

            var result = new SymmetricPolynomial<T>();
            result.variables.AddRange(this.variables);
            foreach (var term in this.polynomialTerms)
            {
                result.polynomialTerms.Add(term.Key, term.Value);
            }

            foreach (var term in right.polynomialTerms)
            {
                var coeff = default(T);
                if (result.polynomialTerms.TryGetValue(term.Key, out coeff))
                {
                    var subtractCoeff = group.AdditiveInverse(term.Value);
                    coeff = group.Add(coeff, subtractCoeff);
                    if (group.IsAdditiveUnity(coeff))
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
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O produto de ambos os polinómios.</returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="ArgumentException">Se as variáveis não conicidirem.</exception>
        public SymmetricPolynomial<T> Multiply(SymmetricPolynomial<T> right, IRing<T> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }

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

            var result = new SymmetricPolynomial<T>();
            result.variables.AddRange(this.variables);

            foreach (var thisTerm in this.polynomialTerms)
            {
                foreach (var rightTerm in right.polynomialTerms)
                {
                    var factors = this.MultiplyMonomials(
                        thisTerm.Value,
                        thisTerm.Key,
                        rightTerm.Value,
                        rightTerm.Key,
                        ring);
                    foreach (var factorKvp in factors)
                    {
                        var coeff = default(T);
                        if (result.polynomialTerms.TryGetValue(factorKvp.Key, out coeff))
                        {
                            coeff = ring.Add(coeff, factorKvp.Value);
                            if (ring.IsAdditiveUnity(coeff))
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
        /// Obtém a representação do polinómio simétrico como combinação polinomial dos polinómios simétricos 
        /// elementares.
        /// </summary>
        /// <param name="elementarySymmetricVarDefs">
        /// Um dicionário que permite mapear entre o n-ésimo polinómios simétrico e o seu nome, caso a primeira 
        /// entrada do tuplo se encontre a falso e o seu valor caso esta se encontre a verdadeiro.
        /// </param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>
        /// A respresentação polinomial do polinómio simétrico como combinação dos polinómios simétricos elementares.
        /// </returns>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        /// <exception cref="MathematicsException">Caso algum erro interno tenha ocorrido.</exception>
        public Polynomial<T> GetElementarySymmetricRepresentation(
            Dictionary<int, Tuple<bool, string, T>> elementarySymmetricVarDefs,
            IRing<T> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }

            if (elementarySymmetricVarDefs == null)
            {
                throw new ArgumentNullException("elementarySymmetricVarDefs");
            }

            var symmetricTermsStack = new Stack<Dictionary<Dictionary<int, int>, T>>();
            var resultStack = new Stack<Polynomial<T>>();
            symmetricTermsStack.Push(this.polynomialTerms);
            resultStack.Push(new Polynomial<T>(ring.AdditiveUnity, ring));
            resultStack.Push(new Polynomial<T>(ring.MultiplicativeUnity, ring));
            resultStack.Push(new Polynomial<T>(ring.AdditiveUnity, ring));
            while (symmetricTermsStack.Count > 0)
            {
                var topMonomials = symmetricTermsStack.Pop();
                var topEnumerator = topMonomials.GetEnumerator();
                if (topEnumerator.MoveNext())
                {
                    var topMonomial = topEnumerator.Current;
                    var index = this.GetelementarySymmetricIndex(topMonomial.Key);
                    if (index == -1)
                    {
                        var degreeDivisor = this.GetDivisor(topMonomial.Key);
                        var maximumSymmetric = this.GetMaximumSymmetric(topMonomial.Key);
                        var multTerms = this.MultiplyMonomials(
                            ring.MultiplicativeUnity,
                            degreeDivisor,
                            ring.AdditiveInverse(topMonomial.Value),
                            maximumSymmetric,
                            ring);

                        var newTopMonomial = new Dictionary<Dictionary<int, int>, T>();
                        multTerms.Remove(topMonomial.Key);
                        while (topEnumerator.MoveNext())
                        {
                            T value = default(T);
                            if (multTerms.TryGetValue(topEnumerator.Current.Key, out value))
                            {
                                value = ring.Add(value, topEnumerator.Current.Value);
                                multTerms.Remove(topEnumerator.Current.Key);
                            }
                            else
                            {
                                value = topEnumerator.Current.Value;
                            }

                            newTopMonomial.Add(topEnumerator.Current.Key, value);
                        }

                        foreach (var multTerm in multTerms)
                        {
                            newTopMonomial.Add(multTerm.Key, multTerm.Value);
                        }

                        symmetricTermsStack.Push(newTopMonomial);

                        Tuple<bool, string, T> varDefs = null;
                        index = this.GetelementarySymmetricIndex(maximumSymmetric);
                        if (elementarySymmetricVarDefs.TryGetValue(index, out varDefs))
                        {
                            if (varDefs.Item1)
                            {
                                var variableCoeff = varDefs.Item3;
                                if (!ring.IsAdditiveUnity(variableCoeff))
                                {
                                    var newSymmetricPol = new Dictionary<Dictionary<int, int>, T>();
                                    newSymmetricPol.Add(degreeDivisor, ring.MultiplicativeUnity);
                                    symmetricTermsStack.Push(newSymmetricPol);

                                    resultStack.Push(new Polynomial<T>(variableCoeff, ring));
                                    resultStack.Push(new Polynomial<T>(ring.AdditiveUnity, ring));

                                }
                            }
                            else
                            {
                                var newSymmetricPol = new Dictionary<Dictionary<int, int>, T>();
                                newSymmetricPol.Add(degreeDivisor, ring.MultiplicativeUnity);
                                symmetricTermsStack.Push(newSymmetricPol);

                                resultStack.Push(new Polynomial<T>(topMonomial.Value, varDefs.Item2, ring));
                                resultStack.Push(new Polynomial<T>(ring.AdditiveUnity, ring));

                            }
                        }
                        else
                        {
                            var newSymmetricPol = new Dictionary<Dictionary<int, int>, T>();
                            newSymmetricPol.Add(degreeDivisor, ring.MultiplicativeUnity);
                            symmetricTermsStack.Push(newSymmetricPol);

                            var defaultSymmetricName = this.GetElementarySymmDefaultName(index);

                            resultStack.Push(new Polynomial<T>(topMonomial.Value, defaultSymmetricName, ring));
                            resultStack.Push(new Polynomial<T>(ring.AdditiveUnity, ring));

                        }
                    }
                    else
                    {
                        Tuple<bool, string, T> varDefs = null;
                        if (elementarySymmetricVarDefs.TryGetValue(index, out varDefs))
                        {
                            if (varDefs.Item1)
                            {
                                var variableCoeff = varDefs.Item3;
                                if (!ring.IsAdditiveUnity(variableCoeff))
                                {
                                    var resultTop = resultStack.Pop();
                                    resultStack.Push(resultTop.Add(new Polynomial<T>(variableCoeff, ring), ring));
                                }
                            }
                            else
                            {
                                var resultTop = resultStack.Pop();
                                resultStack.Push(resultTop.Add(new Polynomial<T>(
                                    topMonomial.Value, 
                                    varDefs.Item2, 
                                    ring), 
                                    ring));
                            }
                        }
                        else
                        {
                            var defaultSymmetricName = this.GetElementarySymmDefaultName(index);

                            var resultTop = resultStack.Pop();
                            resultStack.Push(resultTop.Add(new Polynomial<T>(
                                topMonomial.Value, 
                                defaultSymmetricName, 
                                ring),
                                ring));
                        }

                        topMonomials.Remove(topMonomial.Key);
                        symmetricTermsStack.Push(topMonomials);
                    }
                }
                else
                {
                    var resultTop = resultStack.Pop();
                    var toMultiply = resultStack.Pop();
                    var toAdd = resultStack.Pop();
                    resultTop = resultTop.Multiply(toMultiply, ring);
                    resultTop = resultTop.Add(toAdd, ring);
                    resultStack.Push(resultTop);
                }
            }

            if (resultStack.Count != 1)
            {
                throw new MathematicsException("A severe error has occured.");
            }
            else
            {
                var result = resultStack.Pop();
                return result;
            }
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
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>A representação polinomial.</returns>
        private Polynomial<T> GetElementarySymmetricMonomialRepresentation(
            Dictionary<int, int> monomialDegree,
            T coeff,
            Dictionary<int, Tuple<bool, string, T>> elementarySymmetricVarDefs,
            IRing<T> ring)
        {
            var monomialStack = new Stack<KeyValuePair<Dictionary<int, int>, T>>();
            var resultStack = new Stack<Polynomial<T>>();
            monomialStack.Push(new KeyValuePair<Dictionary<int, int>, T>(monomialDegree, coeff));
            resultStack.Push(new Polynomial<T>(ring.MultiplicativeUnity, ring));
            while (monomialStack.Count != 0)
            {
                var topMonomial = monomialStack.Pop();
                var resultTop = resultStack.Pop();
                var index = this.GetelementarySymmetricIndex(topMonomial.Key);
                if (index == -1)
                {
                    var degreeDivisor = this.GetDivisor(topMonomial.Key);
                    var maximumSymmetric = this.GetMaximumSymmetric(topMonomial.Key);
                    var multTerms = this.MultiplyMonomials(
                        ring.MultiplicativeUnity,
                        degreeDivisor,
                        ring.AdditiveInverse(topMonomial.Value),
                        maximumSymmetric,
                        ring);

                    multTerms.Remove(topMonomial.Key);
                    foreach (var kvp in multTerms)
                    {
                        monomialStack.Push(kvp);
                        resultStack.Push(new Polynomial<T>(ring.MultiplicativeUnity, ring));
                    }

                    Tuple<bool, string, T> varDefs = null;
                    index = this.GetelementarySymmetricIndex(maximumSymmetric);
                    if (elementarySymmetricVarDefs.TryGetValue(index, out varDefs))
                    {
                        if (varDefs.Item1)
                        {
                            var variableCoeff = varDefs.Item3;
                            if (!ring.IsAdditiveUnity(variableCoeff))
                            {
                                resultTop = resultTop.Multiply(new Polynomial<T>(variableCoeff, ring), ring);
                                monomialStack.Push(new KeyValuePair<Dictionary<int, int>, T>(
                                    degreeDivisor, 
                                    ring.MultiplicativeUnity));
                                resultStack.Push(resultTop);
                            }
                        }
                        else
                        {
                            resultTop = resultTop.Multiply(new Polynomial<T>(
                                topMonomial.Value, 
                                varDefs.Item2, 
                                ring),
                                ring);
                        }
                    }
                    else
                    {
                        var defaultSymmetricName = this.GetElementarySymmDefaultName(index);
                        var symPolRes = new Polynomial<T>(topMonomial.Value, defaultSymmetricName, ring);
                        symPolRes = symPolRes.Multiply(resultTop, ring);
                        if (resultStack.Count == 0)
                        {
                            return symPolRes;
                        }
                        else
                        {
                            var otherTop = resultStack.Pop();
                            resultStack.Push(symPolRes.Add(otherTop, ring));
                        }
                    }
                }
                else
                {
                    Tuple<bool, string, T> varDefs = null;
                    if (elementarySymmetricVarDefs.TryGetValue(index, out varDefs))
                    {
                        if (varDefs.Item1)
                        {
                            var variableCoeff = varDefs.Item3;
                            if (!ring.IsAdditiveUnity(variableCoeff))
                            {
                                variableCoeff = ring.Multiply(variableCoeff, topMonomial.Value);
                                var symPolRes = new Polynomial<T>(variableCoeff, ring);
                                symPolRes = symPolRes.Multiply(resultTop,ring);
                                if (resultStack.Count == 0)
                                {
                                    return symPolRes;
                                }
                                else
                                {
                                    var otherTop = resultStack.Pop();
                                    resultStack.Push(symPolRes.Add(otherTop,ring));
                                }
                            }
                        }
                        else
                        {
                            var symPolRes = new Polynomial<T>(topMonomial.Value, varDefs.Item2, ring);
                            symPolRes = symPolRes.Multiply(resultTop,ring);
                            if (resultStack.Count == 0)
                            {
                                return symPolRes;
                            }
                            else
                            {
                                var otherTop = resultStack.Pop();
                                resultStack.Push(symPolRes.Add(otherTop,ring));
                            }
                        }
                    }
                    else
                    {
                        var defaultSymmetricName = this.GetElementarySymmDefaultName(index);
                        var symPolRes = new Polynomial<T>(topMonomial.Value, defaultSymmetricName, ring);
                        symPolRes = symPolRes.Multiply(resultTop,ring);
                        if (resultStack.Count == 0)
                        {
                            return symPolRes;
                        }
                        else
                        {
                            var otherTop = resultStack.Pop();
                            resultStack.Push(symPolRes.Add(otherTop,ring));
                        }
                    }
                }
            }

            return new Polynomial<T>(ring.AdditiveUnity, ring);
        }

        /// <summary>
        /// Obtém o divisor do grau tendo como quociente o polinómio simétrico elementar
        /// especificado pelo índice.
        /// </summary>
        /// <param name="degree">O grau do polinómio a dividir.</param>
        /// <returns>O resultado da divisão.</returns>
        private Dictionary<int, int> GetDivisor(Dictionary<int, int> degree)
        {
            var result = new Dictionary<int, int>();
            var zeroCount = 0;
            foreach (var kvp in degree)
            {
                if (kvp.Key == 0)
                {
                    zeroCount += kvp.Value;
                }
                else
                {
                    var keyDifference = kvp.Key - 1;
                    if (keyDifference == 0)
                    {
                        zeroCount += kvp.Value;
                    }
                    else
                    {
                        result.Add(keyDifference, kvp.Value);
                    }
                }
            }

            if (result.Count != 0 && zeroCount > 0)
            {
                result.Add(0, zeroCount);
            }

            return result;
        }

        /// <summary>
        /// Obtém o máximo polinómio simétrico elementar que divide o grau.
        /// </summary>
        /// <param name="degree">O grau.</param>
        /// <returns>O polinómio simétrico elmentar.</returns>
        private Dictionary<int, int> GetMaximumSymmetric(Dictionary<int, int> degree)
        {
            var result = new Dictionary<int, int>();
            var nonZeroCount = 0;
            foreach (var kvp in degree)
            {
                if (kvp.Key == 0)
                {
                    result.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    nonZeroCount += kvp.Value;
                }
            }

            result.Add(1, nonZeroCount);
            return result;
        }

        /// <summary>
        /// Obtém o nome por defeito do simétrico elementar.
        /// </summary>
        /// <param name="index">O índice do simétrico elementar.</param>
        /// <returns>O nome por defeito.</returns>
        private string GetElementarySymmDefaultName(int index)
        {
            return string.Format("s{0}", index);
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
        /// <param name="ring">O anel responsável pelas as operações.</param>
        /// <returns>O produto dos monómios.</returns>
        private Dictionary<Dictionary<int, int>, T> MultiplyMonomials(
            T firstMonomialCoeff,
            Dictionary<int, int> firstMonomialDegree,
            T secondMonomialCoeff,
            Dictionary<int, int> secondMonomialDegree,
            IRing<T> ring)
        {
            var firstDegreeNumber = this.GetDegreeTermsNumber(firstMonomialDegree);
            var secondDegreeNumber = this.GetDegreeTermsNumber(secondMonomialDegree);
            var firstDegreeNumberNumerator = firstDegreeNumber.Numerator;
            var secondDegreeNumberNumerator = secondDegreeNumber.Numerator;

            int[] permutationDegree;
            int[] fixedDegree;
            Dictionary<int, int> fixedCompactDegree;
            Dictionary<int, int> permDegree;
            if (firstDegreeNumberNumerator < secondDegreeNumberNumerator)
            {
                permutationDegree = this.ExpandDegree(firstMonomialDegree);
                fixedDegree = this.ExpandDegree(secondMonomialDegree);
                fixedCompactDegree = secondMonomialDegree;
                permDegree = firstMonomialDegree;
            }
            else
            {
                permutationDegree = this.ExpandDegree(secondMonomialDegree);
                fixedDegree = this.ExpandDegree(firstMonomialDegree);
                fixedCompactDegree = firstMonomialDegree;
                permDegree = secondMonomialDegree;
            }

            var result = new Dictionary<Dictionary<int, int>, T>(this.degreeComparer);

            // Conta o número de graus que resultam da multiplicação do monómios para aplicar o factor
            var degreesCount = new Dictionary<Dictionary<int, int>, int>(this.degreeComparer);

            var permutationDegreeStructure = new int[permutationDegree.Length][];
            for (int i = 0; i < permutationDegree.Length; ++i)
            {
                permutationDegreeStructure[i] = permutationDegree;
            }

            var structurePermutationAffector = new StructureAffector(permutationDegreeStructure, permDegree);
            foreach (var degreeAffectation in structurePermutationAffector)
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

            var coeffProd = ring.Multiply(firstMonomialCoeff, secondMonomialCoeff);
            foreach (var degreeCountKvp in degreesCount)
            {
                var scaleFactor = this.GetFactorScale(degreeCountKvp.Key, fixedCompactDegree);
                scaleFactor.NumeratorNumberMultiply(degreeCountKvp.Value);
                var auxCoeffProd = ring.AddRepeated(coeffProd, scaleFactor.Numerator);
                result.Add(degreeCountKvp.Key, auxCoeffProd);
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
                            result = count;
                            if (degreeEnumerator.MoveNext())
                            {
                                result = -1;
                            }
                        }
                        else
                        {
                            result = -1;
                        }
                    }
                    else
                    {
                        result = count;
                    }
                }
                else if (degreeEnumerator.Current.Key == 0)
                {
                    if (degreeEnumerator.MoveNext())
                    {
                        if (degreeEnumerator.Current.Key == 1)
                        {
                            result = degreeEnumerator.Current.Value;
                            if (degreeEnumerator.MoveNext())
                            {
                                result = -1;
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
                    result[degreeItem] = degreeCount + 1;
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
