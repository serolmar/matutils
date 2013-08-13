using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;
using Mathematics.Algorithms;
using System.IO;

namespace Mathematics
{
    public class UnivariatePolynomialReader<T, RingType, InputReader>
        where RingType : IRing<T>
    {
        /// <summary>
        /// O leitor de coeficientes.
        /// </summary>
        private IParse<T, string, string> coeffParser;

        /// <summary>
        /// O anel responsável pelas operações.
        /// </summary>
        private RingType ring;

        /// <summary>
        /// O nome da variável.
        /// </summary>
        private string variable;

        /// <summary>
        /// Maps the external delimiters. External delimiters bounds entire elementary subexpressions.
        /// </summary>
        private Dictionary<string, List<string>> externalDelimitersTypes = new Dictionary<string, List<string>>();

        /// <summary>
        /// O domínio que contém as funções sobre inteiros.
        /// </summary>
        private IntegerDomain integerRing = new IntegerDomain();

        public UnivariatePolynomialReader(string variable, IParse<T, string, string> coeffParser, RingType ring)
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
            }
        }
        /// <summary>
        /// Efectua a leitura de um polinómio.
        /// </summary>
        /// <param name="polynomialReader">A cadeia de carácteres que contém o polinómimo.</param>
        /// <param name="resultPolynomial">O polinómio lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        public bool TryParsePolynomial(MementoSymbolReader<InputReader, string, string> polynomialReader, out UnivariatePolynomialNormalForm<T, RingType> resultPolynomial)
        {
            return this.TryParsePolynomial(polynomialReader, null, out resultPolynomial);
        }

        /// <summary>
        /// Efectua a leitura de um polinómio.
        /// </summary>
        /// <param name="polynomial">A cadeia de carácteres que contém o polinómimo.</param>
        /// <param name="errors">A lista de errros encontrados.</param>
        /// <param name="resultPolynomial">O polinómio lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        public bool TryParsePolynomial(MementoSymbolReader<InputReader, string, string> polynomialReader, List<string> errors, out UnivariatePolynomialNormalForm<T, RingType> resultPolynomial)
        {
            if (polynomialReader == null)
            {
                throw new ArgumentNullException("polynomialReader");
            }

            resultPolynomial = default(UnivariatePolynomialNormalForm<T, RingType>);
            var expressionReader = new ExpressionReader<ParseUnivarPolynomNormalFormItem<T, RingType>, InputReader>(new SimpleUnivarPolynomNormalFormReader<T, RingType>(this.coeffParser, this.ring));
            expressionReader.RegisterBinaryOperator("plus", Add, 0);
            expressionReader.RegisterBinaryOperator("times", Multiply, 1);
            expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
            expressionReader.RegisterBinaryOperator("hat", Power, 2);
            expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
            expressionReader.RegisterBinaryOperator("over", Divide, 1);

            expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            foreach (var kvp in this.externalDelimitersTypes)
            {
                foreach (var delimiter in kvp.Value)
                {
                    expressionReader.RegisterExternalDelimiterTypes(kvp.Key, delimiter);
                }
            }

            expressionReader.AddVoid("space");
            expressionReader.AddVoid("carriage_return");
            expressionReader.AddVoid("new_line");

            var expressionResult = default(ParseUnivarPolynomNormalFormItem<T, RingType>);
            if (expressionReader.TryParse(polynomialReader, errors, out expressionResult))
            {
                if (expressionResult.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    resultPolynomial = new UnivariatePolynomialNormalForm<T, RingType>(expressionResult.Coeff, 0, this.variable, this.ring);
                    return true;
                }
                else if (expressionResult.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(T).IsAssignableFrom(typeof(int)))
                    {
                        resultPolynomial = new UnivariatePolynomialNormalForm<T, RingType>((T)(object)expressionResult.Degree, 0, this.variable, this.ring);
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
                        errors.Add("Severe error.");
                    }

                    return false;
                }
            }
            else
            {
                return false;
            }
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
                List<string> temporary = new List<string>();
                temporary.Add(closeDelimiter);
                this.externalDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        /// <summary>
        /// Adiciona dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio a adicionar.</param>
        /// <param name="right">O segundo polinómio a adicionar.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParseUnivarPolynomNormalFormItem<T, RingType> Add(ParseUnivarPolynomNormalFormItem<T, RingType> left, ParseUnivarPolynomNormalFormItem<T, RingType> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T, RingType>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Coeff = this.ring.Add(left.Coeff, right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Degree = (int)(object)left.Coeff + right.Degree;
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = right.Polynomial.Add(left.Coeff);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Degree = left.Degree + (int)(object)right.Coeff;
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = left.Degree + right.Degree;
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Polynomial = right.Polynomial.Add((T)(object)left.Degree);
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Polynomial = left.Polynomial.Add(right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Polynomial = left.Polynomial.Add((T)(object)right.Degree);
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = left.Polynomial.Add(right.Polynomial);
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
        protected virtual ParseUnivarPolynomNormalFormItem<T, RingType> Multiply(ParseUnivarPolynomNormalFormItem<T, RingType> left, ParseUnivarPolynomNormalFormItem<T, RingType> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T, RingType>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Coeff = this.ring.Multiply(left.Coeff, right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Degree = (int)(object)left.Coeff * right.Degree;
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = right.Polynomial.Multiply(left.Coeff);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Degree = left.Degree * (int)(object)right.Coeff;
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = left.Degree * right.Degree;
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Polynomial = right.Polynomial.Multiply((T)(object)left.Degree);
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Polynomial = left.Polynomial.Multiply(right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Polynomial = left.Polynomial.Multiply((T)(object)right.Degree);
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = left.Polynomial.Multiply(right.Polynomial);
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
        protected virtual ParseUnivarPolynomNormalFormItem<T, RingType> Subtract(ParseUnivarPolynomNormalFormItem<T, RingType> left, ParseUnivarPolynomNormalFormItem<T, RingType> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T, RingType>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Coeff = this.ring.Add(left.Coeff, this.ring.AdditiveInverse(right.Coeff));
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Degree = (int)(object)left.Coeff - right.Degree;
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = right.Polynomial.Subtract(left.Coeff);
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Degree = left.Degree - (int)(object)right.Coeff;
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = left.Degree - right.Degree;
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Polynomial = right.Polynomial.Subtract((T)(object)left.Degree);
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    result.Polynomial = left.Polynomial.Subtract(right.Coeff);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Polynomial = left.Polynomial.Subtract((T)(object)right.Degree);
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    result.Polynomial = left.Polynomial.Subtract(right.Polynomial);
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
        protected virtual ParseUnivarPolynomNormalFormItem<T, RingType> Divide(ParseUnivarPolynomNormalFormItem<T, RingType> left, ParseUnivarPolynomNormalFormItem<T, RingType> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T, RingType>();
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
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        if (right.Degree == 0)
                        {
                            throw new DivideByZeroException();
                        }
                        else
                        {
                            result.Degree = (int)(object)left.Coeff / right.Degree;
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var rightCoeff = right.Polynomial.GetAsValue();
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
                                result.Coeff = field.Multiply(left.Coeff, field.MultiplicativeInverse(rightCoeff));
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
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        var rightValue = (int)(object)right.Coeff;
                        if (rightValue == 0)
                        {
                            throw new DivideByZeroException();
                        }
                        else
                        {
                            result.Degree = left.Degree / rightValue;
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = left.Degree * right.Degree;
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        if (typeof(int).IsAssignableFrom(typeof(T)))
                        {
                            var value = (int)(object)right.Polynomial.GetAsValue();
                            if (value == 0)
                            {
                                throw new DivideByZeroException();
                            }
                            else
                            {
                                result.Degree = left.Degree / value;
                            }
                        }
                        else
                        {
                            throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
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
                        result.Polynomial = left.Polynomial.Multiply(field.MultiplicativeInverse(right.Coeff));
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        if (right.Degree == 0)
                        {
                            throw new DivideByZeroException();
                        }
                        else
                        {
                            result.Polynomial = left.Polynomial.Multiply((T)(object)(1 / right.Degree));
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't convert an integer value to polynomial coefficient type.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var rightValue = right.Polynomial.GetAsValue();
                        if (this.ring.IsAdditiveUnity(rightValue))
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
                                result.Polynomial = left.Polynomial.Multiply(field.MultiplicativeInverse(rightValue));
                            }
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
        protected virtual ParseUnivarPolynomNormalFormItem<T, RingType> Symmetric(ParseUnivarPolynomNormalFormItem<T, RingType> pol)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T, RingType>();
            if (pol.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                result.Coeff = this.ring.AdditiveInverse(pol.Coeff);
            }
            else if (pol.ValueType == EParsePolynomialValueType.INTEGER)
            {
                result.Degree = -pol.Degree;
            }
            else if (pol.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                result.Polynomial = pol.Polynomial.GetSymmetric();
            }

            return result;
        }

        /// <summary>
        /// Determina a potência do polinómio.
        /// </summary>
        /// <param name="left">O polinómio.</param>
        /// <param name="right">O expoente.</param>
        /// <returns>O resultado da potência.</returns>
        protected virtual ParseUnivarPolynomNormalFormItem<T, RingType> Power(ParseUnivarPolynomNormalFormItem<T, RingType> left, ParseUnivarPolynomNormalFormItem<T, RingType> right)
        {
            var result = new ParseUnivarPolynomNormalFormItem<T, RingType>();
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        var exponent = (int)(object)right.Coeff;
                        result.Coeff = MathFunctions.Power(left.Coeff, exponent, this.ring);
                    }
                    else
                    {
                        throw new MathematicsException("Can't compute power of non integer values.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        result.Coeff = MathFunctions.Power(left.Coeff, right.Degree, this.ring);
                    }
                    else
                    {
                        throw new MathematicsException("Can't compute power of non integer values.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        if (right.Polynomial.IsValue)
                        {
                            var exponent = (int)(object)right.Polynomial.GetAsValue();
                            result.Coeff = MathFunctions.Power(left.Coeff, exponent, this.ring);
                        }
                        else
                        {
                            throw new MathematicsException("Polynomial exponents aren't allowed.");
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't compute power of non integer values.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        var exponent = (int)(object)right.Coeff;
                        result.Degree = MathFunctions.Power(left.Degree, exponent, this.integerRing);
                    }
                    else
                    {
                        throw new MathematicsException("Can't compute power of non integer values.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Degree = MathFunctions.Power(left.Degree, right.Degree, this.integerRing);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        if (right.Polynomial.IsValue)
                        {
                            var exponent = (int)(object)right.Polynomial.GetAsValue();
                            result.Degree = MathFunctions.Power(left.Degree, exponent, this.integerRing);
                        }
                        else
                        {
                            throw new MathematicsException("Polynomial exponents aren't allowed.");
                        }
                    }
                    else
                    {
                        throw new MathematicsException("Can't compute power of non integer values.");
                    }
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        var exponent = (int)(object)right.Coeff;
                        result.Polynomial = MathFunctions.Power(left.Polynomial,
                            exponent,
                            new UnivarPolynomRing<T, RingType>(left.Polynomial.VariableName, this.ring));
                    }
                    else
                    {
                        throw new MathematicsException("Can't compute power of non integer values.");
                    }
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Polynomial = MathFunctions.Power(left.Polynomial,
                            right.Degree,
                            new UnivarPolynomRing<T, RingType>(left.Polynomial.VariableName, this.ring));
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (typeof(int).IsAssignableFrom(typeof(T)))
                    {
                        var exponent = (int)(object)right.Polynomial.GetAsValue();
                        result.Polynomial = MathFunctions.Power(left.Polynomial,
                            exponent,
                            new UnivarPolynomRing<T, RingType>(left.Polynomial.VariableName, this.ring));
                    }
                    else
                    {
                        throw new MathematicsException("Can't compute power of non integer values.");
                    }
                }
            }

            return result;
        }
    }
}
