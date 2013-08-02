namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Algorithms;

    /// <summary>
    /// Class that represents a polynom.
    /// </summary>
    /// <typeparam name="T">The type of coefficients in polynom.</typeparam>
    /// <typeparam name="R">The type of ring evaluator.</typeparam>
    public class Polynomial<T, R>
        where R : IRing<T>
    {
        private const int primeMultiplier = 53;

        private DegreeEqualityComparer degreeComparer = new DegreeEqualityComparer();

        private List<int> emptyDegree = new List<int>();

        private R polynomialCoeffRing;

        private Dictionary<List<int>, T> coeffsMap;

        private List<PolynomialGeneralVariable<T, R>> variables = new List<PolynomialGeneralVariable<T, R>>();

        #region Constructors
        public Polynomial(R coefficientRing)
        {
            if (coefficientRing == null)
            {
                throw new MathematicsException("Parameter coefficientRing can't be null.");
            }

            this.polynomialCoeffRing = coefficientRing;
            this.coeffsMap = new Dictionary<List<int>, T>(this.degreeComparer);
        }

        public Polynomial(T coeff, R coefficientRing)
            : this(coefficientRing)
        {
            var list = new List<int>();
            if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
            {
                this.coeffsMap.Add(list, coeff);
            }
        }

        public Polynomial(T coeff, int degree, string var, R coefficientRing)
            : this(coefficientRing)
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
            if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
            {
                if (degree != 0)
                {
                    list.Add(degree);
                    this.variables.Add(new PolynomialGeneralVariable<T, R>(var));
                }

                this.coeffsMap.Add(list, coeff);
            }
        }

        public Polynomial(T coeff,
            IEnumerable<int> degree,
            IEnumerable<string> vars,
            R coefficientRing)
            : this(coefficientRing)
        {
            if (coeff == null)
            {
                throw new MathematicsException("Coef can't be null.");
            }

            var list = new List<int>();
            if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
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
                                this.variables.Add(new PolynomialGeneralVariable<T, R>(varsEnumerator.Current));
                            }
                        }

                        degreeState = degreeEnumerator.MoveNext();
                        varsState = varsEnumerator.MoveNext();
                    }

                    while (degreeState)
                    {
                        if (degreeEnumerator.Current != 0)
                        {
                            throw new MathematicsException("Number of non-zero degrees must be lesser than the number of non empty variables.");
                        }

                        degreeState = degreeEnumerator.MoveNext();
                    }
                }

                this.coeffsMap.Add(list, coeff);
            }
        }

        public Polynomial(T coeff, string var, R coefficientRing)
            : this(coeff, 1, var, coefficientRing)
        {
        }
        #endregion

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

        public bool IsVariable
        {
            get
            {
                if (this.coeffsMap.Count == 1)
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

                    if (sum == 1 && this.polynomialCoeffRing.IsMultiplicativeUnity(keyValue))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsMonomial
        {
            get
            {
                return this.coeffsMap.Count < 2;
            }
        }

        public Polynomial<T, R> Clone()
        {
            var res = new Polynomial<T, R>(this.polynomialCoeffRing);
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

        public T GetAsValue()
        {
            if (!this.IsValue)
            {
                throw new MathematicsException("Can't convert polynomial to a value.");
            }

            if (this.coeffsMap.Count == 0)
            {
                return this.polynomialCoeffRing.AdditiveUnity;
            }
            else
            {
                return this.coeffsMap.First().Value;
            }
        }

        public string GetAsVariable()
        {
            if (this.IsVariable)
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
        /// Obtém todas as variáveis envolvidas no polinómio.
        /// </summary>
        /// <returns>O conjunto das variáveis.</returns>
        public List<string> GetVariables()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o monómio principal quando lhe é proporcionado um comparador de graus.
        /// </summary>
        /// <param name="degreeComparable">O comparador de graus.</param>
        /// <returns>O maior monómio correspondente.</returns>
        public Polynomial<T, R> GetLeadingMonomial(IComparable<List<int>> degreeComparable)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o monómio corresopndente ao menor grau.
        /// </summary>
        /// <param name="degreeComparable">O comparador de graus.</param>
        /// <returns>O monómio correspondente.</returns>
        public Polynomial<T, R> GetTailMonomial(IComparable<List<int>> degreeComparable)
        {
            throw new NotImplementedException();
        }

        #region Operations
        /// <summary>
        /// Obtém a soma do polinómio corrente com outro polinómio.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <returns>O resultado da soma.</returns>
        public Polynomial<T, R> Add(Polynomial<T, R> right)
        {
            if (right == null)
            {
                throw new MathematicsException("Can't add a null polynomial.");
            }

            var result = new Polynomial<T, R>(this.polynomialCoeffRing);
            foreach (var kvp in this.coeffsMap)
            {
                result.coeffsMap.Add(kvp.Key, kvp.Value);
            }

            foreach (var vars in this.variables)
            {
                result.variables.Add(vars.Clone());
            }

            // TODO: speed up searches with dictionary
            foreach (var vars in right.variables)
            {
                if (!result.variables.Contains(vars))
                {
                    result.variables.Add(vars);
                }
            }

            foreach (var kvp in right.coeffsMap)
            {
                var resultDegree = result.GetDegreeFromRightPol(
                    kvp.Key,
                    right);

                if (result.coeffsMap.ContainsKey(resultDegree))
                {
                    var resultCoeff = this.polynomialCoeffRing.Add(
                        kvp.Value,
                        result.coeffsMap[resultDegree]);

                    if (this.polynomialCoeffRing.Equals(
                        this.polynomialCoeffRing.AdditiveUnity,
                        resultCoeff))
                    {
                        result.coeffsMap.Remove(resultDegree);
                    }
                    else
                    {
                        result.coeffsMap[resultDegree] = resultCoeff;
                    }
                }
                else
                {
                    result.coeffsMap.Add(resultDegree, kvp.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Adiciona um coeficiente ao polinómio.
        /// </summary>
        /// <param name="coeff">O coefieciente a ser adicionado.</param>
        /// <returns>O resultado da adição.</returns>
        public Polynomial<T, R> Add(T coeff)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = this.Clone();
                if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
                {
                    var degree = new List<int>();
                    var resultCoeff = default(T);
                    if (result.coeffsMap.TryGetValue(degree, out resultCoeff))
                    {
                        resultCoeff = this.polynomialCoeffRing.Add(resultCoeff, coeff);
                        if (this.polynomialCoeffRing.IsAdditiveUnity(resultCoeff))
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
        /// <returns>O resultado da diferença.</returns>
        public Polynomial<T, R> Subtract(Polynomial<T, R> right)
        {
            if (right == null)
            {
                throw new MathematicsException("Can't add a null polynomial.");
            }

            var result = new Polynomial<T, R>(this.polynomialCoeffRing);
            foreach (var kvp in this.coeffsMap)
            {
                result.coeffsMap.Add(kvp.Key, kvp.Value);
            }

            foreach (var vars in this.variables)
            {
                result.variables.Add(vars.Clone());
            }

            // TODO: speed up searches with dictionary
            foreach (var vars in right.variables)
            {
                if (!result.variables.Contains(vars))
                {
                    result.variables.Add(vars);
                }
            }

            foreach (var kvp in right.coeffsMap)
            {
                var resultDegree = result.GetDegreeFromRightPol(
                    kvp.Key,
                    right);

                if (result.coeffsMap.ContainsKey(resultDegree))
                {
                    var resultCoeff = this.polynomialCoeffRing.Add(
                        this.polynomialCoeffRing.AdditiveInverse(kvp.Value),
                        result.coeffsMap[resultDegree]);

                    if (this.polynomialCoeffRing.Equals(
                        this.polynomialCoeffRing.AdditiveUnity,
                        resultCoeff))
                    {
                        result.coeffsMap.Remove(resultDegree);
                    }
                    else
                    {
                        result.coeffsMap[resultDegree] = resultCoeff;
                    }
                }
                else
                {
                    result.coeffsMap.Add(resultDegree, this.polynomialCoeffRing.AdditiveInverse(kvp.Value));
                }
            }

            return result;
        }

        /// <summary>
        /// Subtrai um coeficiente ao polinómio.
        /// </summary>
        /// <param name="coeff">O coefieciente a ser subtraído.</param>
        /// <returns>O resultado da subtracção.</returns>
        public Polynomial<T, R> Subtract(T coeff)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = this.Clone();
                if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
                {
                    var degree = new List<int>();
                    var resultCoeff = default(T);
                    if (result.coeffsMap.TryGetValue(degree, out resultCoeff))
                    {
                        resultCoeff = this.polynomialCoeffRing.Add(resultCoeff, this.polynomialCoeffRing.AdditiveInverse(coeff));
                        if (this.polynomialCoeffRing.IsAdditiveUnity(resultCoeff))
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
                        result.coeffsMap.Add(degree, this.polynomialCoeffRing.AdditiveInverse(coeff));
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o produto do polinómio corrente com outro polinómio.
        /// </summary>
        /// <param name="right">O outro polinómio.</param>
        /// <returns>O resultado do produto.</returns>
        public Polynomial<T, R> Multiply(Polynomial<T, R> right)
        {
            var result = new Polynomial<T, R>(this.polynomialCoeffRing);
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
                        this.polynomialCoeffRing.Multiply(kvp.Value, coeff));
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
                        this.polynomialCoeffRing.Multiply(kvp.Value, coeff));
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

                var appendVariables = new List<PolynomialGeneralVariable<T, R>>();
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
                    this.polynomialCoeffRing.Multiply(firstCoeff, secondCoeff));
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
                result.variables.Add(new PolynomialGeneralVariable<T, R>(
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
                result.variables.Add(new PolynomialGeneralVariable<T, R>(
                    this.Clone()));
                result.coeffsMap.Add(secondDegree, secondCoeff);
            }
            else
            {
                result.variables.Add(new PolynomialGeneralVariable<T, R>(this.Clone()));
                result.variables.Add(new PolynomialGeneralVariable<T, R>(right.Clone()));
                result.coeffsMap.Add(
                    new List<int>() { 1, 1 },
                    this.polynomialCoeffRing.MultiplicativeUnity);
            }

            return result;
        }

        /// <summary>
        /// Obtém o produto do polinómio por uma constante.
        /// </summary>
        /// <param name="coeff">A constante.</param>
        /// <returns>O produto.</returns>
        public Polynomial<T, R> Multiply(T coeff)
        {
            if (coeff == null)
            {
                throw new ArgumentNullException("coeff");
            }
            else
            {
                var result = new Polynomial<T, R>(this.polynomialCoeffRing);
                foreach (var variable in this.variables)
                {
                    result.variables.Add(variable.Clone());
                }

                foreach (var kvp in this.coeffsMap)
                {
                    var product = this.polynomialCoeffRing.Multiply(kvp.Value, coeff);
                    result.coeffsMap.Add(kvp.Key, product);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém o polinómio simétrico.
        /// </summary>
        /// <returns>O polinómio simétrico.</returns>
        public Polynomial<T, R> GetSymmetric()
        {
            var result = new Polynomial<T, R>(this.polynomialCoeffRing);
            foreach (var variable in this.variables)
            {
                result.variables.Add(variable.Clone());
            }

            foreach (var kvp in this.coeffsMap)
            {
                var product = this.polynomialCoeffRing.Multiply(kvp.Value, this.polynomialCoeffRing.AdditiveInverse(kvp.Value));
                result.coeffsMap.Add(kvp.Key, product);
            }

            return result;
        }

        /// <summary>
        /// Obtém a potência do polinómio actual.
        /// </summary>
        /// <param name="exponent">O expoente.</param>
        /// <returns>O resultado da potência.</returns>
        public Polynomial<T, R> Power(int exponent)
        {
            if (exponent < 0)
            {
                throw new ArgumentOutOfRangeException("Exponent can't be a negative number.");
            }
            else if (exponent == 1)
            {
                return this.Clone();
            }
            else
            {
                var result = new Polynomial<T, R>(this.polynomialCoeffRing);
                var termsCount = this.coeffsMap.Count;
                if (termsCount > 1)
                {
                    var variable = new PolynomialGeneralVariable<T, R>(this.Clone());
                    result.variables.Add(variable);
                    result.coeffsMap.Add(new List<int>() { exponent }, this.polynomialCoeffRing.MultiplicativeUnity);
                }
                else if (termsCount == 1)
                {
                    var termsEnum = this.coeffsMap.GetEnumerator();
                    termsEnum.MoveNext();
                    var degree = termsEnum.Current.Key;
                    var coeff = termsEnum.Current.Value;
                    if (degree.Count == 0)
                    {
                        if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
                        {
                            result.coeffsMap.Add(degree, MathFunctions.Power(coeff, exponent, this.polynomialCoeffRing));
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

                        result.coeffsMap.Add(newDegree, MathFunctions.Power(coeff, exponent, this.polynomialCoeffRing));
                        foreach (var variable in this.variables)
                        {
                            result.variables.Add(variable.Clone());
                        }
                    }
                }

                return result;
            }
        }

        public Polynomial<T, R> Replace(Dictionary<string, T> replace)
        {
            var result = new Polynomial<T, R>(this.polynomialCoeffRing);
            var mapPositionToVariableCoefficient = new Dictionary<int, T>();
            var mapPositionToMonomial = new Dictionary<int, Polynomial<T, R>>();
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
                    var replacedPol = variable.GetPolynomial().Replace(replace);
                    if (replacedPol.IsValue)
                    {
                        mapPositionToVariableCoefficient.Add(i, replacedPol.GetAsValue());
                    }
                    else if (replacedPol.coeffsMap.Count == 1)
                    {
                        mapPositionToMonomial.Add(i, replacedPol);
                    }
                    else
                    {
                        result.variables.Add(new PolynomialGeneralVariable<T, R>(replacedPol));
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
                            this.polynomialCoeffRing);
                        coeff = this.polynomialCoeffRing.Multiply(coeff, replaceCoeff);

                    }
                    else if (mapPositionToMonomial.ContainsKey(i) && degree != 0)
                    {
                        var monomial = mapPositionToMonomial[i];
                        var monomialCoeff = monomial.coeffsMap.First().Value;
                        var monomialDegree = monomial.coeffsMap.First().Key;

                        monomialCoeff = MathFunctions.Power(monomialCoeff, degree, this.polynomialCoeffRing);
                        coeff = this.polynomialCoeffRing.Multiply(coeff, monomialCoeff);
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
                    existingCoeff = this.polynomialCoeffRing.Add(existingCoeff, coeff);
                    if (this.polynomialCoeffRing.IsAdditiveUnity(existingCoeff))
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
                    if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
                    {
                        result.coeffsMap.Add(updateDegree, coeff);
                    }
                }
            }

            return result;
        }

        public Polynomial<T, R> Replace(Dictionary<string, Polynomial<T, R>> replace)
        {
            var result = new Polynomial<T, R>(this.polynomialCoeffRing);
            var mapPositionToVariableCoefficient = new Dictionary<int, T>();
            var mapPositionToMonomial = new Dictionary<int, Polynomial<T, R>>();
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
                            mapPositionToVariableCoefficient.Add(i, replacementPolynomial.GetAsValue());
                        }
                        else if (replacementPolynomial.coeffsMap.Count == 1)
                        {
                            mapPositionToMonomial.Add(i, replacementPolynomial);
                        }
                        else
                        {
                            result.variables.Add(new PolynomialGeneralVariable<T, R>(replacementPolynomial));
                        }
                    }
                    else
                    {
                        result.variables.Add(variable.Clone());
                    }
                }
                else if (variable.IsPolynomial)
                {
                    var replacedPol = variable.GetPolynomial().Replace(replace);
                    if (replacedPol.IsValue)
                    {
                        mapPositionToVariableCoefficient.Add(i, replacedPol.GetAsValue());
                    }
                    else if (replacedPol.coeffsMap.Count == 1)
                    {
                        mapPositionToMonomial.Add(i, replacedPol);
                    }
                    else
                    {
                        result.variables.Add(new PolynomialGeneralVariable<T, R>(replacedPol));
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
                            this.polynomialCoeffRing);
                        coeff = this.polynomialCoeffRing.Multiply(coeff, replaceCoeff);

                    }
                    else if (mapPositionToMonomial.ContainsKey(i) && degree != 0)
                    {
                        var monomial = mapPositionToMonomial[i];
                        var monomialCoeff = monomial.coeffsMap.First().Value;
                        var monomialDegree = monomial.coeffsMap.First().Key;

                        monomialCoeff = MathFunctions.Power(monomialCoeff, degree, this.polynomialCoeffRing);
                        coeff = this.polynomialCoeffRing.Multiply(coeff, monomialCoeff);
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
                    existingCoeff = this.polynomialCoeffRing.Add(existingCoeff, coeff);
                    if (this.polynomialCoeffRing.IsAdditiveUnity(existingCoeff))
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
                    if (!this.polynomialCoeffRing.IsAdditiveUnity(coeff))
                    {
                        result.coeffsMap.Add(updateDegree, coeff);
                    }
                }
            }

            return result;
        }

        public Polynomial<T, R> GetExpanded()
        {
            var res = new Polynomial<T, R>(this.polynomialCoeffRing);
            foreach (var kvp in this.coeffsMap)
            {
                var degree = kvp.Key;
                var term = new Polynomial<T, R>(kvp.Value, this.polynomialCoeffRing);
                for (int i = 0; i < degree.Count; ++i)
                {
                    if (degree[i] != 0)
                    {
                        Polynomial<T, R> temporary = null;
                        if (this.variables[i].IsVariable)
                        {
                            temporary = new Polynomial<T, R>(
                                this.polynomialCoeffRing.MultiplicativeUnity,
                                degree[i],
                                this.variables[i].GetVariable(),
                                this.polynomialCoeffRing);
                        }
                        else if (this.variables[i].IsPolynomial)
                        {
                            temporary = this.variables[i].GetPolynomial().Clone();
                            temporary = this.Power(temporary.GetExpanded(), degree[i]);
                        }

                        term.MultiplyExpanded(temporary);
                    }
                }

                res = res.Add(term);
            }

            return res;
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // A polynomial can be a coefficient
            if (obj is T)
            {
                if (this.coeffsMap.Count == 0)
                {
                    return this.polynomialCoeffRing.IsAdditiveUnity((T)obj);

                }

                return false;
            }

            // A polynomial can be a variable
            if (obj is string)
            {
                if (this.coeffsMap.Count == 1)
                {
                    var coeffKvp = this.coeffsMap.First();
                    if (coeffKvp.Key.Sum() == 1)
                    {
                        return this.polynomialCoeffRing.IsMultiplicativeUnity(coeffKvp.Value);
                    }
                }

                return false;
            }

            var innerObj = obj as Polynomial<T, R>;
            if (innerObj == null)
            {
                return false;
            }

            foreach (var item in this.coeffsMap)
            {
                if (!this.polynomialCoeffRing.Equals(
                    this.polynomialCoeffRing.AdditiveUnity,
                    item.Value))
                {
                    var rightDegree = this.GetDegreeFromRightPol(item.Key, innerObj);
                    if (rightDegree == null)
                    {
                        return false;
                    }

                    if (innerObj.coeffsMap.ContainsKey(rightDegree))
                    {
                        return this.polynomialCoeffRing.Equals(
                            item.Value,
                            innerObj.coeffsMap[rightDegree]);
                    }
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            var emptyHash = 0;
            foreach (var kvp in this.coeffsMap)
            {
                if (!this.polynomialCoeffRing.Equals(
                    this.polynomialCoeffRing.AdditiveUnity,
                    kvp.Value))
                {
                    emptyHash ^= this.GetCurrentHash(kvp.Key);
                    if (!this.polynomialCoeffRing.Equals(
                        this.polynomialCoeffRing.MultiplicativeUnity,
                        kvp.Value) && kvp.Value != null)
                    {
                        emptyHash ^= kvp.Value.GetHashCode();
                    }
                }
            }

            return emptyHash;
        }

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
                if (!this.polynomialCoeffRing.Equals(
                    this.polynomialCoeffRing.MultiplicativeUnity,
                    kvp.Value) || this.degreeComparer.Equals(kvp.Key, emptyDegree))
                {
                    resultBuilder.Append(kvp.Value.ToString());
                    timesSignal = "*";
                }

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
        /// Gets the degree from right polynomial giving the current degree.
        /// </summary>
        /// <remarks>
        /// This function is used within equals. The right polynomial may be equal
        /// if the order isn't to be accounted.
        /// </remarks>
        /// <param name="leftDegree">The left degree.</param>
        /// <param name="right">The right polynomial.</param>
        /// <returns>The macthed right degree or null if match can't be attained.</returns>
        private List<int> GetDegreeFromRightPol(List<int> leftDegree, Polynomial<T, R> right)
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

        private void MultiplyExpanded(Polynomial<T, R> right)
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
                    var newCoeff = this.polynomialCoeffRing.Multiply(coeff, rightKvp.Value);
                    if (coeffMaps.ContainsKey(sum))
                    {
                        var current = coeffMaps[sum];
                        current = this.polynomialCoeffRing.Add(current, newCoeff);
                        if (this.polynomialCoeffRing.IsAdditiveUnity(current))
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
        private void MultiplyExpanded(Dictionary<List<int>, T> mappedCoeffs)
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
                        this.polynomialCoeffRing.Multiply(leftgKvp.Value, rightKvp.Value)
                        );
                }
            }

            this.coeffsMap = newMappedCoeffs;
        }

        private Polynomial<T, R> Power(Polynomial<T, R> pol, int exponent)
        {
            if (exponent == 0)
            {
                return new Polynomial<T, R>(
                    this.polynomialCoeffRing.MultiplicativeUnity,
                    this.polynomialCoeffRing);
            }
            else
            {
                var result = pol;
                var innerExponent = MathFunctions.GetInversion(exponent);
                var rem = innerExponent % 2;
                innerExponent = innerExponent / 2;
                while (innerExponent > 0)
                {
                    result.MultiplyExpanded(result.coeffsMap);
                    if (rem == 1)
                    {
                        result.MultiplyExpanded(pol.coeffsMap);
                    }

                    rem = innerExponent % 2;
                    innerExponent = innerExponent / 2;
                }

                return result;
            }
        }

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
