﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mathematics.Algorithms;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de polinómio univariáveis na forma normal.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem os coeficientes.</typeparam>
    /// <typeparam name="InputReader">O leitor de valores.</typeparam>
    public class UnivariatePolynomialReader<T, InputReader>
    {
        /// <summary>
        /// O leitor de coeficientes.
        /// </summary>
        private IParse<T, string, string> coeffParser;

        /// <summary>
        /// O anel responsável pelas operações.
        /// </summary>
        private IRing<T> ring;

        /// <summary>
        /// O nome da variável.
        /// </summary>
        private string variable;

        /// <summary>
        /// Faz os mapeamentos dos delimitadores externos.
        /// </summary>
        private Dictionary<string, List<string>> externalDelimitersTypes = new Dictionary<string, List<string>>();

        /// <summary>
        /// Faz o mapeamento de delimitadores de expressões.
        /// </summary>
        private Dictionary<string, List<string>> expressionDelimiterTypes = new Dictionary<string, List<string>>();

        /// <summary>
        /// O domínio que contém as funções sobre inteiros.
        /// </summary>
        private IntegerDomain integerRing = new IntegerDomain();

        /// <summary>
        /// O anel responsável pelas operações sobre os polinómios.
        /// </summary>
        private UnivarPolynomRing<T> univarPolRing;

        /// <summary>
        /// Mantém o conversor actual.
        /// </summary>
        private IConversion<int, T> conversion;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnivariatePolynomialReader{T, InputReader}"/>.
        /// </summary>
        /// <param name="variable">A variável.</param>
        /// <param name="coeffParser">O leitor de coeficientes.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentException">Se a variável for nula ou vazia.</exception>
        /// <exception cref="MathematicsException">
        /// Se o anel ou o leitor de coeficientes não forem providenciados.
        /// </exception>
        public UnivariatePolynomialReader(string variable, IParse<T, string, string> coeffParser, IRing<T> ring)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentException("Variable must hava a non empty value.");
            }
            else if (coeffParser == null)
            {
                throw new MathematicsException("A coefficient parser must be provided.");
            }
            else if (ring == null)
            {
                throw new MathematicsException("A ring must be provided.");
            }
            else
            {
                this.variable = variable;
                this.coeffParser = coeffParser;
                this.ring = ring;
                this.univarPolRing = new UnivarPolynomRing<T>(variable, ring);
            }
        }

        /// <summary>
        /// Obtém e atribui o anel responsável pelas operações sobre o polinómio aquando
        /// da sua leitura.
        /// </summary>
        /// <value>O anel responsável pelas operações sobre o polinómio.</value>
        public IRing<T> Ring
        {
            get
            {
                return this.ring;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("A ring must be provided.");
                }
                else
                {
                    this.ring = value;
                }
            }
        }

        /// <summary>
        /// Efectua a leitura de um polinómio.
        /// </summary>
        /// <param name="polynomialReader">A cadeia de carácteres que contém o polinómimo.</param>
        /// <param name="conversion">O conversor entre o tipo de coeficiente e um inteiro.</param>
        /// <param name="resultPolynomial">O polinómio lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        public bool TryParsePolynomial(
            MementoSymbolReader<InputReader, string, string> polynomialReader,
            IConversion<int, T> conversion,
            out UnivariatePolynomialNormalForm<T> resultPolynomial)
        {
            var erros = new LogStatus<string, EParseErrorLevel>();
            return this.TryParsePolynomial(polynomialReader, conversion, erros, out resultPolynomial);
        }

        /// <summary>
        /// Efectua a leitura de um polinómio.
        /// </summary>
        /// <param name="polynomialReader">A cadeia de carácteres que contém o polinómimo.</param>
        /// <param name="conversion">O conversor entre o tipo de coeficiente e um inteiro.</param>
        /// <param name="errors">A lista de errros encontrados.</param>
        /// <param name="resultPolynomial">O polinómio lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o leitor de polinómios ou o conversor forem nulos.</exception>
        public bool TryParsePolynomial(
            MementoSymbolReader<InputReader, string, string> polynomialReader,
            IConversion<int, T> conversion,
            ILogStatus<string,EParseErrorLevel> errors,
            out UnivariatePolynomialNormalForm<T> resultPolynomial)
        {
            if (polynomialReader == null)
            {
                throw new ArgumentNullException("polynomialReader");
            }
            else if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }else if(errors == null)
            {
                throw new ArgumentNullException("errors");
            }
            else
            {
                this.conversion = conversion;
                resultPolynomial = default(UnivariatePolynomialNormalForm<T>);
                var expressionReader = new ExpressionReader<ParseUnivarPolynomNormalFormItem<T>, string, string>(
                    new SimpleUnivarPolynomNormalFormReader<T>(this.coeffParser, this.ring, this.variable));
                expressionReader.RegisterBinaryOperator("plus", Add, 0);
                expressionReader.RegisterBinaryOperator("times", Multiply, 1);
                expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
                expressionReader.RegisterBinaryOperator("hat", Power, 2);
                expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
                expressionReader.RegisterBinaryOperator("over", Divide, 1);

                if (this.expressionDelimiterTypes.Any())
                {
                    foreach (var expressionDelimiterKvp in this.expressionDelimiterTypes)
                    {
                        foreach (var closeDelimiter in expressionDelimiterKvp.Value)
                        {
                            expressionReader.RegisterExpressionDelimiterTypes(
                            expressionDelimiterKvp.Key,
                            closeDelimiter);
                        }
                    }
                }
                else
                {
                    expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
                }

                expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
                foreach (var kvp in this.externalDelimitersTypes)
                {
                    foreach (var delimiter in kvp.Value)
                    {
                        expressionReader.RegisterExternalDelimiterTypes(kvp.Key, delimiter);
                    }
                }

                expressionReader.AddVoid("blancks");
                expressionReader.AddVoid("space");
                expressionReader.AddVoid("carriage_return");
                expressionReader.AddVoid("new_line");

                var innerErrors = new LogStatus<string,EParseErrorLevel>();
                var expressionResult = expressionReader.Parse(polynomialReader, innerErrors);
                if (innerErrors.HasLogs(EParseErrorLevel.ERROR))
                {
                    foreach (var kvp in innerErrors.GetLogs())
                    {
                        errors.AddLog(kvp.Value, kvp.Key);
                    }
                    return false;
                }
                else
                {
                    if (expressionResult.ValueType == EParsePolynomialValueType.COEFFICIENT)
                    {
                        resultPolynomial = new UnivariatePolynomialNormalForm<T>(
                            expressionResult.Coeff,
                            0,
                            this.variable,
                            this.ring);
                        return true;
                    }
                    else if (expressionResult.ValueType == EParsePolynomialValueType.INTEGER)
                    {
                        if (typeof(T).IsAssignableFrom(typeof(int)))
                        {
                            resultPolynomial = new UnivariatePolynomialNormalForm<T>(
                                (T)(object)expressionResult.Degree,
                                0,
                                this.variable,
                                this.ring);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (expressionResult.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                    {
                        resultPolynomial = expressionResult.Polynomial;
                        return true;
                    }
                    else
                    {
                        if (errors != null)
                        {
                            errors.AddLog("Severe error.", EParseErrorLevel.ERROR);
                        }

                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Mapeia os delimitadores de expressão.
        /// </summary>
        /// <remarks>
        /// Caso não existam delimitadores de expressão, serão considerados os parêntesis
        /// de abertura e fecho por defeito.
        /// </remarks>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        /// <exception cref="ArgumentException">Se algum dos argumentos for nulo ou vazio.</exception>
        public void RegisterExpressionDelimiterTypes(string openDelimiter, string closeDelimiter)
        {
            if (string.IsNullOrWhiteSpace(openDelimiter))
            {
                throw new ArgumentException("An open delimiter must be provided.");
            }
            else if (string.IsNullOrWhiteSpace(closeDelimiter))
            {
                throw new ArgumentException("A close delimiter must be provided.");
            }
            else
            {
                if (this.expressionDelimiterTypes.ContainsKey(openDelimiter))
                {
                    if (!this.expressionDelimiterTypes[openDelimiter].Contains(closeDelimiter))
                    {
                        this.expressionDelimiterTypes[openDelimiter].Add(closeDelimiter);
                    }
                }
                else
                {
                    var temporary = new List<string>() { closeDelimiter };
                    this.expressionDelimiterTypes.Add(openDelimiter, temporary);
                }
            }
        }

        /// <summary>
        /// Elimina todos os mapeamentos de expressão.
        /// </summary>
        /// <remarks>
        /// Caso não existam delimitadores de expressão, serão considerados os parêntesis
        /// de abertura e fecho por defeito.
        /// </remarks>
        public void ClearExpressionDelimitersMappings()
        {
            this.expressionDelimiterTypes.Clear();
        }

        /// <summary>
        /// Regista os delimitadores externos.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        public void RegisterExternalDelimiterTypes(string openDelimiter, string closeDelimiter)
        {
            if (this.externalDelimitersTypes.ContainsKey(openDelimiter))
            {
                if (!this.externalDelimitersTypes[openDelimiter].Contains(closeDelimiter))
                {
                    this.externalDelimitersTypes[openDelimiter].Add(closeDelimiter);
                }
            }
            else
            {
                var temporary = new List<string>() { closeDelimiter };
                this.externalDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        /// <summary>
        /// Elimina todos os mapeamentos externos.
        /// </summary>
        public void ClearExternalDelimitersMappings()
        {
            this.externalDelimitersTypes.Clear();
        }

        /// <summary>
        /// Adiciona dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio a adicionar.</param>
        /// <param name="right">O segundo polinómio a adicionar.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParseUnivarPolynomNormalFormItem<T> Add(
            ParseUnivarPolynomNormalFormItem<T> left,
            ParseUnivarPolynomNormalFormItem<T> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Coeff = this.ring.Add(left.Coeff, right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var rightConversion = this.conversion.InverseConversion(right.Degree);
                    result.Coeff = this.ring.Add(left.Coeff, rightConversion);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = right.Polynomial.Add(left.Coeff, this.ring);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var leftConverted = this.conversion.InverseConversion(left.Degree);
                    result.Coeff = this.ring.Add(leftConverted, right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = this.integerRing.Add(left.Degree, right.Degree);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    var leftConverted = this.conversion.InverseConversion(left.Degree);
                    result.Polynomial.Add(leftConverted, this.ring);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Polynomial = left.Polynomial.Add(right.Coeff, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var rightConverted = this.conversion.InverseConversion(right.Degree);
                    result.Polynomial.Add(rightConverted, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = left.Polynomial.Add(right.Polynomial, this.ring);
                }
            }

            return result;
        }

        /// <summary>
        /// Multiplica dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParseUnivarPolynomNormalFormItem<T> Multiply(
            ParseUnivarPolynomNormalFormItem<T> left,
            ParseUnivarPolynomNormalFormItem<T> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Coeff = this.ring.Multiply(left.Coeff, right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var rightConversion = this.conversion.InverseConversion(right.Degree);
                    result.Coeff = this.ring.Multiply(left.Coeff, rightConversion);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = right.Polynomial.Multiply(left.Coeff, this.ring);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var leftConverted = this.conversion.InverseConversion(left.Degree);
                    result.Coeff = this.ring.Multiply(leftConverted, right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = this.integerRing.Multiply(left.Degree, right.Degree);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    var leftConverted = this.conversion.InverseConversion(left.Degree);
                    result.Polynomial.Multiply(leftConverted, this.ring);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Polynomial = left.Polynomial.Multiply(right.Coeff, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var rightConverted = this.conversion.InverseConversion(right.Degree);
                    result.Polynomial.Multiply(rightConverted, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = left.Polynomial.Multiply(right.Polynomial, this.ring);
                }
            }

            return result;
        }

        /// <summary>
        /// Subtrai dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParseUnivarPolynomNormalFormItem<T> Subtract(
            ParseUnivarPolynomNormalFormItem<T> left,
            ParseUnivarPolynomNormalFormItem<T> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Coeff = this.ring.Add(left.Coeff, this.ring.AdditiveInverse(right.Coeff));
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var rightConversion = this.conversion.InverseConversion(right.Degree);
                    result.Coeff = this.ring.Add(left.Coeff, this.ring.AdditiveInverse(rightConversion));
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = right.Polynomial.Subtract(left.Coeff, this.ring);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var leftConverted = this.conversion.InverseConversion(left.Degree);
                    result.Coeff = this.ring.Add(leftConverted, this.ring.AdditiveInverse(right.Coeff));
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = this.integerRing.Add(left.Degree, this.integerRing.AdditiveInverse(right.Degree));
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    var leftConverted = this.conversion.InverseConversion(left.Degree);
                    result.Polynomial.Subtract(leftConverted, this.ring);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Polynomial = left.Polynomial.Subtract(right.Coeff, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var rightConverted = this.conversion.InverseConversion(right.Degree);
                    result.Polynomial.Subtract(rightConverted, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = left.Polynomial.Subtract(right.Polynomial, this.ring);
                }
            }

            return result;
        }

        /// <summary>
        /// Divide dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        /// <exception cref="MathematicsException">Se a operação falhar.</exception>
        protected virtual ParseUnivarPolynomNormalFormItem<T> Divide(
            ParseUnivarPolynomNormalFormItem<T> left,
            ParseUnivarPolynomNormalFormItem<T> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var field = this.ring as IField<T>;
                    if (field == null)
                    {
                        throw new MathematicsException("The provided ring isn't a field.");
                    }
                    else
                    {
                        result.Coeff = field.Multiply(left.Coeff, field.MultiplicativeInverse(right.Coeff));
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var field = this.ring as IField<T>;
                    if (field == null)
                    {
                        throw new MathematicsException("The provided ring isn't a field.");
                    }
                    else
                    {
                        var rightConversion = this.conversion.InverseConversion(right.Degree);
                        result.Coeff = this.ring.Multiply(left.Coeff, field.MultiplicativeInverse(rightConversion));
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var rightCoeff = right.Polynomial.GetAsValue(this.ring);
                        if (this.ring.IsAdditiveUnity(rightCoeff))
                        {
                            throw new MathematicsException("Division by an additive unity.");
                        }
                        else
                        {
                            var field = this.ring as IField<T>;
                            if (field == null)
                            {
                                throw new MathematicsException("The provided ring isn't a field.");
                            }
                            else
                            {
                                result.Polynomial.Multiply(field.MultiplicativeInverse(left.Coeff), this.ring);
                            }
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't divide by a polynomial.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var field = this.ring as IField<T>;
                    if (field == null)
                    {
                        throw new MathematicsException("The provided ring isn't a field.");
                    }
                    else
                    {
                        var leftConverted = this.conversion.InverseConversion(left.Degree);
                        result.Coeff = this.ring.Multiply(leftConverted, field.MultiplicativeInverse(right.Coeff));
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (left.Degree % right.Degree == 0)
                    {
                        result.Degree = left.Degree / right.Degree;
                    }
                    else
                    {
                        throw new MathematicsException("Fractional powers aren't allowed.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var field = this.ring as IField<T>;
                        if (field == null)
                        {
                            throw new MathematicsException("The provided ring isn't a field.");
                        }
                        else
                        {
                            var rightValue = right.Polynomial.GetAsValue(this.ring);
                            var leftConverted = this.conversion.InverseConversion(left.Degree);
                            result.Coeff = this.ring.Multiply(leftConverted, field.MultiplicativeInverse(rightValue));
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't divide by a polynomial.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var field = this.ring as IField<T>;
                    if (field == null)
                    {
                        throw new MathematicsException("The provided ring isn't a field.");
                    }
                    else
                    {
                        result.Polynomial = left.Polynomial.Multiply(
                            field.MultiplicativeInverse(right.Coeff),
                            this.ring);
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    var field = this.ring as IField<T>;
                    if (field == null)
                    {
                        throw new MathematicsException("The provided ring isn't a field.");
                    }
                    else
                    {
                        var rightConverted = this.conversion.InverseConversion(right.Degree);
                        result.Polynomial.Multiply(
                            field.MultiplicativeInverse(rightConverted),
                            this.ring);
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var rightValue = right.Polynomial.GetAsValue(this.ring);
                        var field = this.ring as IField<T>;
                        if (field == null)
                        {
                            throw new MathematicsException("The provided ring isn't a field.");
                        }
                        else
                        {
                            result.Polynomial = left.Polynomial.Multiply(
                                field.MultiplicativeInverse(rightValue),
                                this.ring);
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't divide by a polynomial.");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o simétrico de um polinómio.
        /// </summary>
        /// <param name="pol">O polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParseUnivarPolynomNormalFormItem<T> Symmetric(
            ParseUnivarPolynomNormalFormItem<T> pol)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T>();
            if (pol.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                result.Coeff = this.ring.AdditiveInverse(pol.Coeff);
            }
            else if (pol.ValueType == EParsePolynomialValueType.INTEGER)
            {
                result.Degree = this.integerRing.AdditiveInverse(pol.Degree);
            }
            else if (pol.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                result.Polynomial = pol.Polynomial.GetSymmetric(this.ring);
            }

            return result;
        }

        /// <summary>
        /// Determina a potência do polinómio.
        /// </summary>
        /// <param name="left">O polinómio.</param>
        /// <param name="right">O expoente.</param>
        /// <returns>O resultado da potência.</returns>
        /// <exception cref="MathematicsException">Se a operação falhar.</exception>
        protected virtual ParseUnivarPolynomNormalFormItem<T> Power(
            ParseUnivarPolynomNormalFormItem<T> left,
            ParseUnivarPolynomNormalFormItem<T> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var degree = this.conversion.DirectConversion(right.Coeff);
                    result.Coeff = MathFunctions.Power(left.Coeff, degree, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Coeff = MathFunctions.Power(left.Coeff, right.Degree, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var exponent = this.conversion.DirectConversion(right.Polynomial.GetAsValue(this.ring));
                        result.Coeff = MathFunctions.Power(left.Coeff, exponent, this.ring);
                    }
                    else
                    {
                        throw new MathematicsException("Polynomial exponents aren't allowed.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var rightConversion = this.conversion.DirectConversion(right.Coeff);
                    result.Degree = MathFunctions.Power(left.Degree, rightConversion, this.integerRing);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = MathFunctions.Power(left.Degree, right.Degree, this.integerRing);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var exponent = this.conversion.DirectConversion(right.Polynomial.GetAsValue(this.ring));
                        result.Degree = MathFunctions.Power(left.Degree, exponent, this.integerRing);
                    }
                    else
                    {
                        throw new MathematicsException("Polynomial exponents aren't allowed.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    var exponent = this.conversion.DirectConversion(right.Coeff);
                    result.Polynomial = MathFunctions.Power(left.Polynomial, exponent, this.univarPolRing);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Polynomial = MathFunctions.Power(left.Polynomial, right.Degree, this.univarPolRing);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var exponent = this.conversion.DirectConversion(right.Polynomial.GetAsValue(this.ring));
                        result.Polynomial = MathFunctions.Power(left.Polynomial, exponent, this.univarPolRing);
                    }
                    else
                    {
                        throw new MathematicsException("Polynomial exponents aren't allowed.");
                    }
                }
            }

            return result;
        }
    }
}
