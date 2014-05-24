namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Algorithms;

    /// <summary>
    /// Classe que representa um polinómio.
    /// </summary>
    /// <remarks>
    /// O recurso ao tipo genérico constrangido ao invés de simplesmente atribuir
    /// o construtor permite limitar, em tempo de compilação, os tipos admitidos
    /// num algoritmo que possa recorrer ao polinómio.
    /// </remarks>
    /// <typeparam name="T">O tipo dos coeficientes no polinómio.</typeparam>
    public class Polynomial<T>
    {
        /// <summary>
        /// O número primo que auxilia na obtenção de um código confuso.
        /// </summary>
        private const int primeMultiplier = 53;

        /// <summary>
        /// O comparador de graus.
        /// </summary>
        private DegreeEqualityComparer degreeComparer = new DegreeEqualityComparer();

        /// <summary>
        /// A lista vazia.
        /// </summary>
        private List<int> emptyDegree = new List<int>();

        /// <summary>
        /// O contentor dos coeficientes e graus.
        /// </summary>
        private Dictionary<List<int>, T> coeffsMap;

        /// <summary>
        /// O contentor das variáveis.
        /// </summary>
        private List<PolynomialGeneralVariable<T>> variables = new List<PolynomialGeneralVariable<T>>();

        #region Construtores

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="Polynomial{T}"/>.
        /// </summary>
        public Polynomial()
        {
            this.coeffsMap = new Dictionary<List<int>, T>(this.degreeComparer);
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="Polynomial{T}"/>.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="coefficientRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se pelo menos um argumento for nulo.</exception>
        public Polynomial(T coeff, IRing<T> coefficientRing)
            : this()
        {
            if (coefficientRing == null)
            {
                throw new ArgumentNullException("coeffcientRing");
            }
            else if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var list = new List<int>();
                if (!coefficientRing.IsAdditiveUnity(coeff))
                {
                    this.coeffsMap.Add(list, coeff);
                }
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="Polynomial{T}"/>.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="degree">O grau.</param>
        /// <param name="var">O nome da variável.</param>
        /// <param name="coefficientRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se o coeficiente ou o anel forem nulos.</exception>
        /// <exception cref="MathematicsException">
        /// Se o grau for negativo ou a variável for nula ou vazia.
        /// </exception>
        public Polynomial(T coeff, int degree, string var, IRing<T> coefficientRing)
            : this()
        {
            if (degree < 0)
            {
                throw new MathematicsException("Negative degrees aren't allowed in polynomial.");
            }

            if (string.IsNullOrWhiteSpace(var))
            {
                throw new MathematicsException("Variables can't be null.");
            }

            var list = new List<int>();
            if (!coefficientRing.IsAdditiveUnity(coeff))
            {
                if (degree != 0)
                {
                    list.Add(degree);
                    this.variables.Add(new PolynomialGeneralVariable<T>(var));
                }

                this.coeffsMap.Add(list, coeff);
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="Polynomial{T}"/>.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="degree">O grau.</param>
        /// <param name="vars">A lista de variáveis.</param>
        /// <param name="coefficientRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Se o coeficiente, a lista da variáveis ou o anel forem nulos.
        /// </exception>
        /// <exception cref="MathematicsException">
        /// Se o número de graus não coincidir com o número de variáveis ou se existir algum grau negativo para alguma
        /// variável não nula e não vazia.
        /// </exception>
        public Polynomial(T coeff,
            IEnumerable<int> degree,
            IEnumerable<string> vars,
            IRing<T> coefficientRing)
            : this()
        {
            if (coefficientRing == null)
            {
                throw new ArgumentNullException("coefficientRing");
            }

            if (coeff == null)
            {
                throw new ArgumentNullException("Coef can't be null.");
            }

            if (vars == null)
            {
                throw new ArgumentNullException("vars");
            }

            var list = new List<int>();
            if (!coefficientRing.IsAdditiveUnity(coeff))
            {
                if (degree != null)
                {
                    var degreeEnumerator = degree.GetEnumerator();
                    var varsEnumerator = vars.GetEnumerator();
                    var degreeState = degreeEnumerator.MoveNext();
                    var varsState = varsEnumerator.MoveNext();

                    while (degreeState && varsState)
                    {
                        if (!string.IsNullOrEmpty(varsEnumerator.Current))
                        {
                            if (degreeEnumerator.Current < 0)
                            {
                                throw new MathematicsException("Negative degrees aren't allowed in polynomial.");
                            }

                            if (degreeEnumerator.Current != 0)
                            {
                                list.Add(degreeEnumerator.Current);
                                this.variables.Add(new PolynomialGeneralVariable<T>(varsEnumerator.Current));
                            }
                        }

                        degreeState = degreeEnumerator.MoveNext();
                        varsState = varsEnumerator.MoveNext();
                    }

                    while (degreeState)
                    {
                        if (degreeEnumerator.Current != 0)
                        {
                            throw new MathematicsException(
                                "Number of non-zero degrees must be lesser than the number of non empty variables.");
                        }

                        degreeState = degreeEnumerator.MoveNext();
                    }
                }

                this.coeffsMap.Add(list, coeff);
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="Polynomial{T}"/>.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="var">A variável.</param>
        /// <param name="coefficientRing">O anel responsável pelas operações sobre os coeficientes.</param>
        public Polynomial(T coeff, string var, IRing<T> coefficientRing)
            : this(coeff, 1, var, coefficientRing)
        {
        }

        #endregion Construtores

        /// <summary>
        /// Obtém um valor que indica se o polinómio é um coeficiente.
        /// </summary>
        /// <value>
        /// Verdadeiro caso o polinómio seja um coeficiente e falso caso contrário.
        /// </value>
        public bool IsValue
        {
            get
            {
                if (this.coeffsMap.Count == 0)
                {
                    return true;
                }

                if (this.coeffsMap.Count == 1)
                {
                    var coeffsDegree = this.coeffsMap.First().Key;
                    if (degreeComparer.Equals(emptyDegree, coeffsDegree))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o polinómio é um monómio.
        /// </summary>
        /// <value>
        /// Verdadeiro caso o polinómio seja um monómio e falso caso contrário.
        /// </value>
        public bool IsMonomial
        {
            get
            {
                return this.coeffsMap.Count < 2;
            }
        }

        /// <summary>
        /// Obtém um cópia do polinómio corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public Polynomial<T> Clone()
        {
            var res = new Polynomial<T>();
            var degreeList = new List<int>();
            foreach (var kvp in this.coeffsMap)
            {
                degreeList = new List<int>();
                degreeList.AddRange(kvp.Key);
                res.coeffsMap.Add(degreeList, kvp.Value);
            }

            foreach (var variable in this.variables)
            {
                res.variables.Add(variable.Clone());
            }

            return res;
        }

        /// <summary>
        /// Obtém o polinómio como sendo um valor.
        /// </summary>
        /// <param name="coefficientRing">O anel responsável pelas operações sobre os coficientes.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        /// <exception cref="MathematicsException">Se o polinómio não for um valor.</exception>
        public T GetAsValue(IRing<T> coefficientRing)
        {
            if (coefficientRing == null)
            {
                throw new ArgumentNullException("coefficientRing");
            }
            else if (!this.IsValue)
            {
                throw new MathematicsException("Can't convert polynomial to a value.");
            }
            else if (this.coeffsMap.Count == 0)
            {
                return coefficientRing.AdditiveUnity;
            }
            else
            {
                return this.coeffsMap.First().Value;
            }
        }

        /// <summary>
        /// Obtém o polinómio como sendo uma variável.
        /// </summary>
        /// <param name="coefficientRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>A variável.</returns>
        /// <exception cref="MathematicsException">Se o polinómio não constituir uma variável.</exception>
        public string GetAsVariable(IRing<T> coefficientRing)
        {
            if (this.IsVariable(coefficientRing))
            {
                var degree = this.coeffsMap.First().Key;
                for (int i = 0; i < degree.Count; ++i)
                {
                    if (degree[i] != 0)
                    {
                        return this.variables[i].GetVariable();
                    }
                }
            }

            throw new MathematicsException("Can't convert polynomial to a name.");
        }

        /// <summary>
        /// Determina se o polinómio corrente é uma variável.
        /// </summary>
        /// <param name="coefficientRing">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>Verdadeiro caso o polinómio seja uma variável e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        public bool IsVariable(IRing<T> coefficientRing)
        {
            if (coefficientRing == null)
            {
                throw new ArgumentNullException("coefficientRing");
            }
            else if (this.coeffsMap.Count == 1)
            {
                var sum = 0;
                var keyDegree = this.coeffsMap.First().Key;
                var keyValue = this.coeffsMap.First().Value;
                for (int i = 0; i < keyDegree.Count; ++i)
                {
                    if (keyDegree[i] != 0)
                    {
                        if (variables[i].IsPolynomial)
                        {
                            return false;
                        }

                        sum += keyDegree[i];
                    }
                }

                if (sum == 1 && coefficientRing.IsMultiplicativeUnity(keyValue))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Obtém todas as variáveis envolvidas no polinómio.
        /// </summary>
        /// <returns>O conjunto das variáveis.</returns>
        public List<string> GetVariables()
        {
            var result = new List<string>();
            var generalVarStack = new Stack<IEnumerator<PolynomialGeneralVariable<T>>>();
            var generalVarEnum = this.variables.GetEnumerator();
            generalVarStack.Push(generalVarEnum);
            while (generalVarStack.Count != 0)
            {
                var currentVarEnum = generalVarStack.Pop();
                if (currentVarEnum.MoveNext())
                {
                    var current = currentVarEnum.Current;
                    if (current.IsVariable)
                    {
                        var variable = current.GetVariable();
                        if (!result.Contains(variable))
                        {
                            result.Add(variable);
                        }
                    }
                    else if (current.IsPolynomial)
                    {
                        var polynomial = current.GetPolynomial();
                        generalVarEnum = polynomial.variables.GetEnumerator();
                        generalVarStack.Push(generalVarEnum);
                    }

                    generalVarStack.Push(currentVarEnum);
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o monómio principal quando lhe é proporcionado um comparador de graus.
        /// </summary>
        /// <param name="degreeComparable">O comparador de graus.</param>
        /// <returns>O maior monómio correspondente.</returns>
        public Polynomial<T> GetLeadingMonomial(IComparable<List<int>> degreeComparable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o monómio corresopndente ao menor grau.
        /// </summary>
        /// <param name="degreeComparable">O comparador de graus.</param>
        /// <returns>O monómio correspondente.</returns>
        public Polynomial<T> GetTailMonomial(IComparable<List<int>> degreeComparable)
        {
            throw new NotImplementedException();
        }

        #region Operações

        /// <summary>
        /// Obtém a soma do polinómio corrente com outro polinómio.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>O resultado da soma.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Add(Polynomial<T> right, IMonoid<T> monoid)
        {
            if (monoid == null)
            {
                throw new ArgumentNullException("monoid");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var result = new Polynomial<T>();
            var variableDic = new Dictionary<PolynomialGeneralVariable<T>, Tuple<int, int, int>>();
            var appendResultVars = new List<PolynomialGeneralVariable<T>>();
            foreach (var term in this.coeffsMap)
            {
                appendResultVars.Clear();
                var resultVarsCount = result.variables.Count;
                var resultDegree = new List<int>();
                var leftDegree = new List<int>();
                var rightDegree = new List<int>();
                var innerVariableDic = new Dictionary<PolynomialGeneralVariable<T>, Tuple<int, int, int>>();
                for (int i = 0; i < term.Key.Count; ++i)
                {
                    var currentDegree = term.Key[i];
                    if (currentDegree != 0)
                    {
                        Tuple<int, int, int> description = null;
                        if (!variableDic.TryGetValue(this.variables[i], out description))
                        {
                            if (!innerVariableDic.TryGetValue(this.variables[i], out description))
                            {
                                var rightPosition = right.variables.IndexOf(this.variables[i]);
                                description = Tuple.Create(i, rightPosition, resultVarsCount + appendResultVars.Count);
                                innerVariableDic.Add(this.variables[i], description);
                                appendResultVars.Add(this.variables[i]);
                            }
                        }

                        this.SetDegree(leftDegree, description.Item1, currentDegree);
                        this.SetDegree(rightDegree, description.Item2, currentDegree);
                        this.SetDegree(resultDegree, description.Item3, currentDegree);
                    }
                }

                if (leftDegree.Count == 0 && rightDegree.Count == 0)
                {
                    var rightCoeff = default(T);
                    if (right.coeffsMap.TryGetValue(rightDegree, out rightCoeff))
                    {
                        var resultCoeff = monoid.Add(term.Value, rightCoeff);
                        if (!monoid.IsAdditiveUnity(resultCoeff))
                        {
                            result.coeffsMap.Add(leftDegree, resultCoeff);
                        }
                    }
                }
                else if (leftDegree.Count != 0)
                {
                    if (rightDegree.Count != 0)
                    {
                        var rightCoeff = default(T);
                        if (right.coeffsMap.TryGetValue(rightDegree, out rightCoeff))
                        {
                            var resultCoeff = monoid.Add(term.Value, rightCoeff);
                            if (!monoid.IsAdditiveUnity(resultCoeff))
                            {
                                result.coeffsMap.Add(resultDegree, resultCoeff);
                                foreach (var variable in appendResultVars)
                                {
                                    result.variables.Add(variable.Clone());
                                }
                            }
                        }
                        else if (!monoid.IsAdditiveUnity(term.Value))
                        {
                            result.coeffsMap.Add(resultDegree, term.Value);
                            foreach (var variable in appendResultVars)
                            {
                                result.variables.Add(variable.Clone());
                            }
                        }
                    }
                    else
                    {
                        foreach (var variable in appendResultVars)
                        {
                            result.variables.Add(variable.Clone());
                        }

                        result.coeffsMap.Add(resultDegree, term.Value);
                    }
                }

                foreach (var kvp in innerVariableDic)
                {
                    variableDic.Add(kvp.Key, kvp.Value);
                }
            }

            foreach (var rightTerm in right.coeffsMap)
            {
                appendResultVars.Clear();
                var resultVarsCount = result.variables.Count;
                var resultDegree = new List<int>();
                var leftDegree = new List<int>();
                var rightDegree = new List<int>();
                var innerVariableDic = new Dictionary<PolynomialGeneralVariable<T>, Tuple<int, int, int>>();
                for (int i = 0; i < rightTerm.Key.Count; ++i)
                {
                    var currentDegree = rightTerm.Key[i];
                    if (currentDegree != 0)
                    {
                        Tuple<int, int, int> description = null;
                        if (!variableDic.TryGetValue(right.variables[i], out description))
                        {
                            if (!innerVariableDic.TryGetValue(right.variables[i], out description))
                            {
                                var leftPosition = this.variables.IndexOf(right.variables[i]);
                                description = Tuple.Create(leftPosition, i, resultVarsCount + appendResultVars.Count);
                                innerVariableDic.Add(right.variables[i], description);
                                appendResultVars.Add(right.variables[i]);
                            }
                        }

                        this.SetDegree(leftDegree, description.Item1, currentDegree);
                        this.SetDegree(rightDegree, description.Item2, currentDegree);
                        this.SetDegree(resultDegree, description.Item3, currentDegree);
                    }
                }

                if (rightDegree.Count != 0)
                {
                    if (leftDegree.Count == 0 || !this.coeffsMap.ContainsKey(leftDegree))
                    {
                        result.coeffsMap.Add(resultDegree, rightTerm.Value);
                        foreach (var variable in appendResultVars)
                        {
                            result.variables.Add(variable.Clone());
                        }
                    }
                }

                foreach (var kvp in innerVariableDic)
                {
                    variableDic.Add(kvp.Key, kvp.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Adiciona um coeficiente ao polinómio.
        /// </summary>
        /// <param name="coeff">O coefieciente a ser adicionado.</param>
        /// <param name="monoid">O monóide responsável pelas operações.</param>
        /// <returns>O resultado da adição.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Add(T coeff, IMonoid<T> monoid)
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
                var result = this.Clone();
                if (!monoid.IsAdditiveUnity(coeff))
                {
                    var degree = new List<int>();
                    var resultCoeff = default(T);
                    if (result.coeffsMap.TryGetValue(degree, out resultCoeff))
                    {
                        resultCoeff = monoid.Add(resultCoeff, coeff);
                        if (monoid.IsAdditiveUnity(resultCoeff))
                        {
                            result.coeffsMap.Remove(degree);
                        }
                        else
                        {
                            result.coeffsMap[degree] = resultCoeff;
                        }
                    }
                    else
                    {
                        result.coeffsMap.Add(degree, coeff);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a diferença entre polinómio corrente e outro polinómio.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>O resultado da diferença.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Subtract(Polynomial<T> right, IGroup<T> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new Polynomial<T>();
                var variableDic = new Dictionary<PolynomialGeneralVariable<T>, Tuple<int, int, int>>();
                var appendResultVars = new List<PolynomialGeneralVariable<T>>();
                foreach (var term in this.coeffsMap)
                {
                    appendResultVars.Clear();
                    var resultVarsCount = result.variables.Count;
                    var resultDegree = new List<int>();
                    var leftDegree = new List<int>();
                    var rightDegree = new List<int>();
                    var innerVariableDic = new Dictionary<PolynomialGeneralVariable<T>, Tuple<int, int, int>>();
                    for (int i = 0; i < term.Key.Count; ++i)
                    {
                        var currentDegree = term.Key[i];
                        if (currentDegree != 0)
                        {
                            Tuple<int, int, int> description = null;
                            if (!variableDic.TryGetValue(this.variables[i], out description))
                            {
                                if (!innerVariableDic.TryGetValue(this.variables[i], out description))
                                {
                                    var rightPosition = right.variables.IndexOf(this.variables[i]);
                                    description = Tuple.Create(i, rightPosition, resultVarsCount + appendResultVars.Count);
                                    innerVariableDic.Add(this.variables[i], description);
                                    appendResultVars.Add(this.variables[i]);
                                }
                            }

                            this.SetDegree(leftDegree, description.Item1, currentDegree);
                            this.SetDegree(rightDegree, description.Item2, currentDegree);
                            this.SetDegree(resultDegree, description.Item3, currentDegree);
                        }
                    }

                    if (leftDegree.Count == 0 && rightDegree.Count == 0)
                    {
                        var rightCoeff = default(T);
                        if (right.coeffsMap.TryGetValue(rightDegree, out rightCoeff))
                        {
                            var resultCoeff = group.Add(term.Value, rightCoeff);
                            if (!group.IsAdditiveUnity(resultCoeff))
                            {
                                result.coeffsMap.Add(leftDegree, resultCoeff);
                            }
                        }
                    }
                    else if (leftDegree.Count != 0)
                    {
                        if (rightDegree.Count != 0)
                        {
                            var rightCoeff = default(T);
                            if (right.coeffsMap.TryGetValue(rightDegree, out rightCoeff))
                            {
                                var resultCoeff = group.Add(term.Value, group.AdditiveInverse(rightCoeff));
                                if (!group.IsAdditiveUnity(resultCoeff))
                                {
                                    result.coeffsMap.Add(resultDegree, resultCoeff);
                                    foreach (var variable in appendResultVars)
                                    {
                                        result.variables.Add(variable.Clone());
                                    }

                                    result.coeffsMap.Add(resultDegree, resultCoeff);
                                }
                            }
                        }
                        else
                        {
                            foreach (var variable in appendResultVars)
                            {
                                result.variables.Add(variable.Clone());
                            }

                            result.coeffsMap.Add(resultDegree, term.Value);
                        }
                    }

                    foreach (var kvp in innerVariableDic)
                    {
                        variableDic.Add(kvp.Key, kvp.Value);
                    }
                }

                foreach (var rightTerm in right.coeffsMap)
                {
                    appendResultVars.Clear();
                    var resultVarsCount = result.variables.Count;
                    var resultDegree = new List<int>();
                    var leftDegree = new List<int>();
                    var rightDegree = new List<int>();
                    var innerVariableDic = new Dictionary<PolynomialGeneralVariable<T>, Tuple<int, int, int>>();
                    for (int i = 0; i < rightTerm.Key.Count; ++i)
                    {
                        var currentDegree = rightTerm.Key[i];
                        if (currentDegree != 0)
                        {
                            Tuple<int, int, int> description = null;
                            if (!variableDic.TryGetValue(right.variables[i], out description))
                            {
                                if (!innerVariableDic.TryGetValue(right.variables[i], out description))
                                {
                                    var leftPosition = this.variables.IndexOf(right.variables[i]);
                                    description = Tuple.Create(leftPosition, i, resultVarsCount + appendResultVars.Count);
                                    innerVariableDic.Add(right.variables[i], description);
                                    appendResultVars.Add(right.variables[i]);
                                }
                            }

                            this.SetDegree(leftDegree, description.Item1, currentDegree);
                            this.SetDegree(rightDegree, description.Item2, currentDegree);
                            this.SetDegree(resultDegree, description.Item3, currentDegree);
                        }
                    }

                    if (rightDegree.Count != 0)
                    {
                        if (leftDegree.Count == 0 || !this.coeffsMap.ContainsKey(leftDegree))
                        {
                            result.coeffsMap.Add(resultDegree, group.AdditiveInverse(rightTerm.Value));
                            foreach (var variable in appendResultVars)
                            {
                                result.variables.Add(variable.Clone());
                            }
                        }
                    }

                    foreach (var kvp in innerVariableDic)
                    {
                        variableDic.Add(kvp.Key, kvp.Value);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Subtrai um coeficiente ao polinómio.
        /// </summary>
        /// <param name="coeff">O coefieciente a ser subtraído.</param>
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>O resultado da subtracção.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Subtract(T coeff, IGroup<T> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = this.Clone();
                if (!group.IsAdditiveUnity(coeff))
                {
                    var degree = new List<int>();
                    var resultCoeff = default(T);
                    if (result.coeffsMap.TryGetValue(degree, out resultCoeff))
                    {
                        resultCoeff = group.Add(resultCoeff, group.AdditiveInverse(coeff));
                        if (group.IsAdditiveUnity(resultCoeff))
                        {
                            result.coeffsMap.Remove(degree);
                        }
                        else
                        {
                            result.coeffsMap[degree] = resultCoeff;
                        }
                    }
                    else
                    {
                        result.coeffsMap.Add(degree, group.AdditiveInverse(coeff));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o produto do polinómio corrente com outro polinómio.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O resultado do produto.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Multiply(Polynomial<T> right, IRing<T> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                var result = new Polynomial<T>();
                if (this.coeffsMap.Count == 0 || right.coeffsMap.Count == 0)
                {
                    return result;
                }

                var emptyDegree = new List<int>();
                if (this.IsValue)
                {
                    var coeff = this.coeffsMap[emptyDegree];
                    foreach (var kvp in right.coeffsMap)
                    {
                        result.coeffsMap.Add(
                            kvp.Key,
                            ring.Multiply(kvp.Value, coeff));
                    }

                    foreach (var vars in right.variables)
                    {
                        result.variables.Add(vars.Clone());
                    }
                }
                else if (right.IsValue)
                {
                    var coeff = right.coeffsMap[emptyDegree];
                    foreach (var kvp in this.coeffsMap)
                    {
                        result.coeffsMap.Add(
                            kvp.Key,
                            ring.Multiply(kvp.Value, coeff));
                    }

                    foreach (var vars in this.variables)
                    {
                        result.variables.Add(vars.Clone());
                    }
                }
                else if (this.coeffsMap.Count == 1 && right.coeffsMap.Count == 1)
                {
                    var firstDegree = new List<int>();
                    var secondDegree = new List<int>();
                    firstDegree.AddRange(this.coeffsMap.First().Key);
                    var firstCoeff = this.coeffsMap.First().Value;
                    secondDegree.AddRange(right.coeffsMap.First().Key);
                    var secondCoeff = right.coeffsMap.First().Value;

                    for (int i = 0; i < firstDegree.Count; ++i)
                    {
                        result.variables.Add(this.variables[i].Clone());
                    }

                    var appendVariables = new List<PolynomialGeneralVariable<T>>();
                    for (int i = 0; i < secondDegree.Count; ++i)
                    {
                        var index = -1;
                        for (int j = 0; j < result.variables.Count; ++j)
                        {
                            if (result.variables[j].Equals(right.variables[i]))
                            {
                                index = j;
                                j = secondDegree.Count;
                            }
                        }

                        if (index != -1)
                        {
                            firstDegree[index] += secondDegree[i];
                        }
                        else
                        {
                            if (secondDegree[i] != 0)
                            {
                                appendVariables.Add(right.variables[i]);
                                firstDegree.Add(secondDegree[i]);
                            }
                        }
                    }

                    result.variables.AddRange(appendVariables);
                    result.coeffsMap.Add(
                        firstDegree,
                        ring.Multiply(firstCoeff, secondCoeff));
                }
                else if (this.coeffsMap.Count == 1)
                {
                    var firstDegree = new List<int>();
                    firstDegree.AddRange(this.coeffsMap.First().Key);
                    var firstCoeff = this.coeffsMap.First().Value;

                    for (int i = 0; i < this.variables.Count; ++i)
                    {
                        result.variables.Add(this.variables[i].Clone());
                    }

                    firstDegree.Add(1);
                    result.variables.Add(new PolynomialGeneralVariable<T>(
                        right.Clone()));
                    result.coeffsMap.Add(firstDegree, firstCoeff);
                }
                else if (right.coeffsMap.Count == 1)
                {
                    var secondDegree = right.coeffsMap.First().Key;
                    var secondCoeff = right.coeffsMap.First().Value;

                    for (int i = 0; i < right.variables.Count; ++i)
                    {
                        result.variables.Add(right.variables[i].Clone());
                    }

                    secondDegree.Add(1);
                    result.variables.Add(new PolynomialGeneralVariable<T>(
                        this.Clone()));
                    result.coeffsMap.Add(secondDegree, secondCoeff);
                }
                else
                {
                    result.variables.Add(new PolynomialGeneralVariable<T>(this.Clone()));
                    result.variables.Add(new PolynomialGeneralVariable<T>(right.Clone()));
                    result.coeffsMap.Add(
                        new List<int>() { 1, 1 },
                        ring.MultiplicativeUnity);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o produto do polinómio por uma constante.
        /// </summary>
        /// <param name="coeff">A constante.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O produto.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Multiply(T coeff, IRing<T> ring)
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
                var result = new Polynomial<T>();
                foreach (var variable in this.variables)
                {
                    result.variables.Add(variable.Clone());
                }

                foreach (var kvp in this.coeffsMap)
                {
                    var product = ring.Multiply(kvp.Value, coeff);
                    result.coeffsMap.Add(kvp.Key, product);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o polinómio simétrico.
        /// </summary>
        /// <param name="group">O grupo responsável pelas operações.</param>
        /// <returns>O polinómio simétrico.</returns>
        /// <exception cref="ArgumentNullException">Se po grupo for nulo.</exception>
        public Polynomial<T> GetSymmetric(IGroup<T> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else
            {
                var result = new Polynomial<T>();
                foreach (var variable in this.variables)
                {
                    result.variables.Add(variable.Clone());
                }

                foreach (var kvp in this.coeffsMap)
                {
                    result.coeffsMap.Add(kvp.Key, group.AdditiveInverse(kvp.Value));
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a potência do polinómio actual.
        /// </summary>
        /// <param name="exponent">O expoente.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        /// <returns>O resultado da potência.</returns>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Se o expoente for um número negativo.</exception>
        public Polynomial<T> Power(int exponent, IRing<T> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (exponent < 0)
            {
                throw new ArgumentOutOfRangeException("Exponent can't be a negative number.");
            }
            else if (exponent == 1)
            {
                return this.Clone();
            }
            else
            {
                var result = new Polynomial<T>();
                var termsCount = this.coeffsMap.Count;
                if (termsCount > 1)
                {
                    var variable = new PolynomialGeneralVariable<T>(this.Clone());
                    result.variables.Add(variable);
                    result.coeffsMap.Add(new List<int>() { exponent }, ring.MultiplicativeUnity);
                }
                else if (termsCount == 1)
                {
                    var termsEnum = this.coeffsMap.GetEnumerator();
                    termsEnum.MoveNext();
                    var degree = termsEnum.Current.Key;
                    var coeff = termsEnum.Current.Value;
                    if (degree.Count == 0)
                    {
                        if (!ring.IsAdditiveUnity(coeff))
                        {
                            result.coeffsMap.Add(degree, MathFunctions.Power(coeff, exponent, ring));
                            foreach (var variable in this.variables)
                            {
                                result.variables.Add(variable.Clone());
                            }
                        }
                    }
                    else
                    {
                        var newDegree = new List<int>();
                        foreach (var deg in degree)
                        {
                            newDegree.Add(exponent * deg);
                        }

                        result.coeffsMap.Add(newDegree, MathFunctions.Power(coeff, exponent, ring));
                        foreach (var variable in this.variables)
                        {
                            result.variables.Add(variable.Clone());
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Substitui as variáveis pelos valores mapeados e efectua algumas simplificações.
        /// </summary>
        /// <param name="replace">O mapeamento das variáveis aos respectivos valores.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado da substituição.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Replace(Dictionary<string, T> replace, IRing<T> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (replace == null)
            {
                throw new ArgumentNullException("replace");
            }
            else
            {
                var result = new Polynomial<T>();
                var mapPositionToVariableCoefficient = new Dictionary<int, T>();
                var mapPositionToMonomial = new Dictionary<int, Polynomial<T>>();
                for (int i = 0; i < this.variables.Count; ++i)
                {
                    var variable = this.variables[i];
                    if (variable.IsVariable)
                    {
                        var variableName = variable.GetVariable();
                        if (replace.ContainsKey(variableName))
                        {
                            mapPositionToVariableCoefficient.Add(i, replace[variableName]);
                        }
                        else
                        {
                            result.variables.Add(variable.Clone());
                        }
                    }
                    else if (variable.IsPolynomial)
                    {
                        var replacedPol = variable.GetPolynomial().Replace(replace, ring);
                        if (replacedPol.IsValue)
                        {
                            mapPositionToVariableCoefficient.Add(i, replacedPol.GetAsValue(ring));
                        }
                        else if (replacedPol.coeffsMap.Count == 1)
                        {
                            mapPositionToMonomial.Add(i, replacedPol);
                        }
                        else
                        {
                            result.variables.Add(new PolynomialGeneralVariable<T>(replacedPol));
                        }
                    }
                }

                foreach (var kvp in this.coeffsMap)
                {
                    var updateDegree = new List<int>();
                    var coeff = kvp.Value;
                    for (int i = 0; i < kvp.Key.Count; ++i)
                    {
                        var degree = kvp.Key[i];
                        if (mapPositionToVariableCoefficient.ContainsKey(i) && degree != 0)
                        {
                            var replaceCoeff = MathFunctions.Power(
                                mapPositionToVariableCoefficient[i],
                                degree,
                                ring);
                            coeff = ring.Multiply(coeff, replaceCoeff);

                        }
                        else if (mapPositionToMonomial.ContainsKey(i) && degree != 0)
                        {
                            var monomial = mapPositionToMonomial[i];
                            var monomialCoeff = monomial.coeffsMap.First().Value;
                            var monomialDegree = monomial.coeffsMap.First().Key;

                            monomialCoeff = MathFunctions.Power(monomialCoeff, degree, ring);
                            coeff = ring.Multiply(coeff, monomialCoeff);
                            var multipliedDegree = new List<int>();
                            for (int j = 0; j < monomialDegree.Count; ++j)
                            {
                                multipliedDegree.Add(monomialDegree[j] * degree);
                            }

                            foreach (var item in monomial.variables)
                            {
                                if (!result.variables.Contains(item))
                                {
                                    result.variables.Add(item.Clone());
                                }
                            }

                            monomialDegree = result.GetDegreeFromRightPol(multipliedDegree, monomial);
                            updateDegree = this.GetDegreeSum(updateDegree, monomialDegree);
                        }
                        else
                        {
                            var index = result.variables.IndexOf(this.variables[i]);
                            if (index != -1 && degree != 0)
                            {
                                while (updateDegree.Count <= index)
                                {
                                    updateDegree.Add(0);
                                }

                                updateDegree[index] += degree;
                            }
                        }
                    }

                    if (result.coeffsMap.ContainsKey(updateDegree))
                    {
                        var existingCoeff = result.coeffsMap[updateDegree];
                        existingCoeff = ring.Add(existingCoeff, coeff);
                        if (ring.IsAdditiveUnity(existingCoeff))
                        {
                            result.coeffsMap.Remove(updateDegree);
                        }
                        else
                        {
                            result.coeffsMap[updateDegree] = existingCoeff;
                        }
                    }
                    else
                    {
                        if (!ring.IsAdditiveUnity(coeff))
                        {
                            result.coeffsMap.Add(updateDegree, coeff);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Substitui as variáveis por polinómios e efectua algumas simplificações.
        /// </summary>
        /// <param name="replace">O mapeamento das variáveis aos polinómios.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado da substituição.</returns>
        /// <exception cref="ArgumentNullException">Se pelo meno um dos argumentos for nulo.</exception>
        public Polynomial<T> Replace(Dictionary<string, Polynomial<T>> replace, IRing<T> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (replace == null)
            {
                throw new ArgumentNullException("replace");
            }
            else
            {
                var result = new Polynomial<T>();
                var mapPositionToVariableCoefficient = new Dictionary<int, T>();
                var mapPositionToMonomial = new Dictionary<int, Polynomial<T>>();
                for (int i = 0; i < this.variables.Count; ++i)
                {
                    var variable = this.variables[i];
                    if (variable.IsVariable)
                    {
                        var variableName = variable.GetVariable();
                        if (replace.ContainsKey(variableName))
                        {
                            var replacementPolynomial = replace[variableName];
                            if (replacementPolynomial.IsValue)
                            {
                                mapPositionToVariableCoefficient.Add(i, replacementPolynomial.GetAsValue(ring));
                            }
                            else if (replacementPolynomial.coeffsMap.Count == 1)
                            {
                                mapPositionToMonomial.Add(i, replacementPolynomial);
                            }
                            else
                            {
                                result.variables.Add(new PolynomialGeneralVariable<T>(replacementPolynomial));
                            }
                        }
                        else
                        {
                            result.variables.Add(variable.Clone());
                        }
                    }
                    else if (variable.IsPolynomial)
                    {
                        var replacedPol = variable.GetPolynomial().Replace(replace, ring);
                        if (replacedPol.IsValue)
                        {
                            mapPositionToVariableCoefficient.Add(i, replacedPol.GetAsValue(ring));
                        }
                        else if (replacedPol.coeffsMap.Count == 1)
                        {
                            mapPositionToMonomial.Add(i, replacedPol);
                        }
                        else
                        {
                            result.variables.Add(new PolynomialGeneralVariable<T>(replacedPol));
                        }
                    }
                }

                //O código restante deverá ser semelhante ao anterior - talvez seja digno de uma função
                foreach (var kvp in this.coeffsMap)
                {
                    var updateDegree = new List<int>();
                    var coeff = kvp.Value;
                    for (int i = 0; i < kvp.Key.Count; ++i)
                    {
                        var degree = kvp.Key[i];
                        if (mapPositionToVariableCoefficient.ContainsKey(i) && degree != 0)
                        {
                            var replaceCoeff = MathFunctions.Power(
                                mapPositionToVariableCoefficient[i],
                                degree,
                                ring);
                            coeff = ring.Multiply(coeff, replaceCoeff);

                        }
                        else if (mapPositionToMonomial.ContainsKey(i) && degree != 0)
                        {
                            var monomial = mapPositionToMonomial[i];
                            var monomialCoeff = monomial.coeffsMap.First().Value;
                            var monomialDegree = monomial.coeffsMap.First().Key;

                            monomialCoeff = MathFunctions.Power(monomialCoeff, degree, ring);
                            coeff = ring.Multiply(coeff, monomialCoeff);
                            var multipliedDegree = new List<int>();
                            for (int j = 0; j < monomialDegree.Count; ++j)
                            {
                                multipliedDegree.Add(monomialDegree[j] * degree);
                            }

                            foreach (var item in monomial.variables)
                            {
                                if (!result.variables.Contains(item))
                                {
                                    result.variables.Add(item.Clone());
                                }
                            }

                            monomialDegree = result.GetDegreeFromRightPol(multipliedDegree, monomial);
                            updateDegree = this.GetDegreeSum(updateDegree, monomialDegree);
                        }
                        else
                        {
                            var index = result.variables.IndexOf(this.variables[i]);
                            if (index != -1 && degree != 0)
                            {
                                while (updateDegree.Count <= index)
                                {
                                    updateDegree.Add(0);
                                }

                                updateDegree[index] += degree;
                            }
                        }
                    }

                    if (result.coeffsMap.ContainsKey(updateDegree))
                    {
                        var existingCoeff = result.coeffsMap[updateDegree];
                        existingCoeff = ring.Add(existingCoeff, coeff);
                        if (ring.IsAdditiveUnity(existingCoeff))
                        {
                            result.coeffsMap.Remove(updateDegree);
                        }
                        else
                        {
                            result.coeffsMap[updateDegree] = existingCoeff;
                        }
                    }
                    else
                    {
                        if (!ring.IsAdditiveUnity(coeff))
                        {
                            result.coeffsMap.Add(updateDegree, coeff);
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o polinómio corrente na forma expandida.
        /// </summary>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O polinómio expandido.</returns>
        /// <exception cref="ArgumentNullException">Se o anel for nulo.</exception>
        public Polynomial<T> GetExpanded(IRing<T> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                var res = new Polynomial<T>();
                foreach (var kvp in this.coeffsMap)
                {
                    var degree = kvp.Key;
                    var term = new Polynomial<T>(kvp.Value, ring);
                    for (int i = 0; i < degree.Count; ++i)
                    {
                        if (degree[i] != 0)
                        {
                            Polynomial<T> temporary = null;
                            if (this.variables[i].IsVariable)
                            {
                                temporary = new Polynomial<T>(
                                    ring.MultiplicativeUnity,
                                    degree[i],
                                    this.variables[i].GetVariable(),
                                    ring);
                            }
                            else if (this.variables[i].IsPolynomial)
                            {
                                temporary = this.variables[i].GetPolynomial().Clone();
                                temporary = this.Power(temporary.GetExpanded(ring), degree[i], ring);
                            }

                            term.MultiplyExpanded(temporary, ring);
                        }
                    }

                    res = res.Add(term, ring);
                }

                return res;
            }
        }

        #endregion Operações

        /// <summary>
        /// Determina se o objecto especificado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto a ser comparado.</param>
        /// <returns>
        /// Veradeiro se o objecto for igual e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // Um polinómio pode ser um coeficiente
            if (obj is T)
            {
                return false;
            }

            // Um polinómio pode ser uma variável
            if (obj is string)
            {
                return false;
            }

            var innerObj = obj as Polynomial<T>;
            if (innerObj == null)
            {
                return false;
            }

            if (this.coeffsMap.Count != innerObj.coeffsMap.Count)
            {
                return false;
            }

            foreach (var item in this.coeffsMap)
            {
                var rightDegree = this.GetDegreeFromRightPol(item.Key, innerObj);
                if (rightDegree == null)
                {
                    return false;
                }

                if (innerObj.coeffsMap.ContainsKey(rightDegree))
                {
                    if (!item.Value.Equals(innerObj.coeffsMap[rightDegree]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Retorna um código confuso para a instância corrente.
        /// </summary>
        /// <returns>
        /// O código confuso da instância corrente que pode ser utilizado em vários algoritmos.
        /// </returns>
        public override int GetHashCode()
        {
            var emptyHash = 0;
            foreach (var kvp in this.coeffsMap)
            {
                emptyHash ^= this.GetCurrentHash(kvp.Key);
                emptyHash ^= kvp.Value.GetHashCode();
            }

            return emptyHash;
        }

        /// <summary>
        /// Obtém uma representação textual do polinómio.
        /// </summary>
        /// <returns>A representação textual do polinómio.</returns>
        public override string ToString()
        {
            if (this.coeffsMap.Count == 0)
            {
                return "0";
            }

            var resultBuilder = new StringBuilder();
            var plusSignal = string.Empty;
            var emptyDegree = new List<int>();
            foreach (var kvp in this.coeffsMap)
            {
                resultBuilder.Append(plusSignal);
                var timesSignal = string.Empty;
                resultBuilder.Append(kvp.Value.ToString());
                timesSignal = "*";

                for (int i = 0; i < this.variables.Count; ++i)
                {
                    if (i < kvp.Key.Count && kvp.Key[i] != 0)
                    {
                        resultBuilder.Append(timesSignal);
                        resultBuilder.Append(variables[i].ToString());
                        if (kvp.Key[i] > 1)
                        {
                            resultBuilder.Append("^");
                            resultBuilder.Append(kvp.Key[i]);
                        }

                        timesSignal = "*";
                    }
                }

                plusSignal = "+";
            }

            return resultBuilder.ToString();
        }

        #region Private Methods

        /// <summary>
        /// Atribui o valor do grau a uma determinada posição.
        /// </summary>
        /// <param name="degrees">A lista dos graus.</param>
        /// <param name="position">A posição a ser atribuída.</param>
        /// <param name="value">O valor a ser atribuído.</param>
        private void SetDegree(List<int> degrees, int position, int value)
        {
            if (position >= 0)
            {
                while (degrees.Count <= position)
                {
                    degrees.Add(0);
                }

                degrees[position] = value;
            }
        }

        /// <summary>
        /// Gets the degree from right polynomial giving the current degree.
        /// </summary>
        /// <remarks>
        /// This function is used within equals. The right polynomial may be equal
        /// if the order isn't to be accounted.
        /// </remarks>
        /// <param name="leftDegree">The left degree.</param>
        /// <param name="right">The right polynomial.</param>
        /// <returns>The macthed right degree or null if match can't be attained.</returns>
        /// <exception cref="MathematicsException">Se acontecer algum erro imprevisto.</exception>
        private List<int> GetDegreeFromRightPol(List<int> leftDegree, Polynomial<T> right)
        {
            var res = new int[this.variables.Count];
            var numberOfZeroes = 0;
            for (int i = 0; i < leftDegree.Count; ++i)
            {
                if (leftDegree[i] != 0 && i < right.variables.Count)
                {
                    if (i >= this.variables.Count)
                    {
                        throw new MathematicsException("Internal error.");
                    }

                    for (int j = 0; j <= numberOfZeroes; ++j)
                    {
                        var currentVariable = right.variables[i - j];
                        var index = this.variables.IndexOf(currentVariable);
                        if (index < 0)
                        {
                            // Right polynomial hasn't current variable.
                            return null;
                        }

                        res[index] = leftDegree[i - j];
                    }

                    numberOfZeroes = 0;
                }
                else
                {
                    ++numberOfZeroes;
                }
            }

            var result = new List<int>();
            numberOfZeroes = 0;
            for (int i = 0; i < res.Length; ++i)
            {
                if (res[i] != 0)
                {
                    for (int j = 0; j < numberOfZeroes; ++j)
                    {
                        result.Add(0);
                    }

                    result.Add(res[i]);
                    numberOfZeroes = 0;
                }
                else
                {
                    ++numberOfZeroes;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the hash provided the current degree.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>The hash.</returns>
        private int GetCurrentHash(List<int> degree)
        {
            var res = 0;
            for (int i = 0; i < degree.Count; ++i)
            {
                if (degree[i] != 0)
                {
                    res ^= degree[i].GetHashCode() ^ this.variables[i].GetHashCode();
                }
            }

            return res;
        }

        /// <summary>
        /// Ontém o produto do polinómio especificado pelo polinómio corrente mantendo o resultado
        /// em forma expandida.
        /// </summary>
        /// <param name="right">O polinómio a ser multiplicado.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        private void MultiplyExpanded(Polynomial<T> right, IRing<T> ring)
        {
            foreach (var variable in right.variables)
            {
                if (!this.variables.Contains(variable))
                {
                    this.variables.Add(variable.Clone());
                }
            }

            var coeffMaps = new Dictionary<List<int>, T>(this.degreeComparer);
            foreach (var thisKvp in this.coeffsMap)
            {
                var coeff = thisKvp.Value;
                var degree = thisKvp.Key;
                foreach (var rightKvp in right.coeffsMap)
                {
                    var innerDegree = this.GetDegreeFromRightPol(rightKvp.Key, right);
                    var sum = this.GetDegreeSum(innerDegree, degree);
                    var newCoeff = ring.Multiply(coeff, rightKvp.Value);
                    if (coeffMaps.ContainsKey(sum))
                    {
                        var current = coeffMaps[sum];
                        current = ring.Add(current, newCoeff);
                        if (ring.IsAdditiveUnity(current))
                        {
                            coeffMaps.Remove(sum);
                        }
                        else
                        {
                            coeffMaps[sum] = current;
                        }
                    }
                    else
                    {
                        coeffMaps.Add(sum, newCoeff);
                    }
                }
            }

            this.coeffsMap = coeffMaps;
        }

        /// <summary>
        /// For expanded multiplication.
        /// </summary>
        /// <param name="mappedCoeffs">The mapped coefficients.</param>
        /// <param name="ring">O anel responsável pelas operações.</param>
        private void MultiplyExpanded(Dictionary<List<int>, T> mappedCoeffs, IRing<T> ring)
        {
            var newMappedCoeffs = new Dictionary<List<int>, T>();
            foreach (var leftgKvp in this.coeffsMap)
            {
                foreach (var rightKvp in mappedCoeffs)
                {
                    var temporaryDegree = new List<int>();
                    var leftDegEnum = leftgKvp.Key.GetEnumerator();
                    var rightDegEnum = rightKvp.Key.GetEnumerator();
                    var leftState = leftDegEnum.MoveNext();
                    var rightState = rightDegEnum.MoveNext();
                    while (leftState && rightState)
                    {
                        temporaryDegree.Add(leftDegEnum.Current + rightDegEnum.Current);
                        leftState = leftDegEnum.MoveNext();
                        rightState = rightDegEnum.MoveNext();
                    }

                    while (leftState)
                    {
                        temporaryDegree.Add(leftDegEnum.Current);
                        leftState = leftDegEnum.MoveNext();
                    }

                    while (rightState)
                    {
                        temporaryDegree.Add(rightDegEnum.Current);
                        rightState = rightDegEnum.MoveNext();
                    }

                    newMappedCoeffs.Add(
                        temporaryDegree,
                        ring.Multiply(leftgKvp.Value, rightKvp.Value)
                        );
                }
            }

            this.coeffsMap = newMappedCoeffs;
        }

        /// <summary>
        /// Determina uma potência do polinómio.
        /// </summary>
        /// <param name="pol">O polinómio.</param>
        /// <param name="exponent">O expoente.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O resultado da potência.</returns>
        private Polynomial<T> Power(Polynomial<T> pol, int exponent, IRing<T> ring)
        {
            if (exponent == 0)
            {
                return new Polynomial<T>(
                    ring.MultiplicativeUnity,
                    ring);
            }
            else
            {
                var result = pol;
                var innerExponent = MathFunctions.GetInversion(exponent);
                var rem = innerExponent % 2;
                innerExponent = innerExponent / 2;
                while (innerExponent > 0)
                {
                    result.MultiplyExpanded(result.coeffsMap, ring);
                    if (rem == 1)
                    {
                        result.MultiplyExpanded(pol.coeffsMap, ring);
                    }

                    rem = innerExponent % 2;
                    innerExponent = innerExponent / 2;
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a soma de duas listas de graus.
        /// </summary>
        /// <param name="first">A primeira lista a ser somada.</param>
        /// <param name="second">A segunda lista a ser somada.</param>
        /// <returns>O resultado da soma.</returns>
        private List<int> GetDegreeSum(List<int> first, List<int> second)
        {
            var result = new List<int>();
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();
            var firstState = firstEnumerator.MoveNext();
            var secondState = secondEnumerator.MoveNext();

            while (firstState && secondState)
            {
                result.Add(firstEnumerator.Current + secondEnumerator.Current);
                firstState = firstEnumerator.MoveNext();
                secondState = secondEnumerator.MoveNext();
            }

            while (firstState)
            {
                result.Add(firstEnumerator.Current);
                firstState = firstEnumerator.MoveNext();
            }

            while (secondState)
            {
                result.Add(secondEnumerator.Current);
                secondState = secondEnumerator.MoveNext();
            }

            return result;
        }
        #endregion
    }
}
