namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Algorithms;
    using Utilities;

    public class PolynomialReader<T, InputReader>
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
        /// Maps the external delimiters. External delimiters bounds entire elementary subexpressions.
        /// </summary>
        private Dictionary<string, List<string>> externalDelimitersTypes = new Dictionary<string, List<string>>();

        /// <summary>
        /// O domínio que contém as funções sobre inteiros.
        /// </summary>
        private IntegerDomain integerRing = new IntegerDomain();

        /// <summary>
        /// Mantém o conversor actual.
        /// </summary>
        private IConversion<int, T> conversion;

        public PolynomialReader(IParse<T, string, string> coeffParser, IRing<T> ring)
        {
            if (coeffParser == null)
            {
                throw new MathematicsException("A coefficient parser must be provided.");
            }

            if (ring == null)
            {
                throw new MathematicsException("A ring must be provided.");
            }

            this.coeffParser = coeffParser;
            this.ring = ring;
        }

        /// <summary>
        /// Efectua a leitura de um polinómio.
        /// </summary>
        /// <param name="polynomialReader">A cadeia de carácteres que contém o polinómimo.</param>
        /// <param name="resultPolynomial">O polinómio lido.</param>
        /// <param name="conversion">O conversor entre o tipo de coeficiente e um inteiro.</param>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        public bool TryParsePolynomial(
            MementoSymbolReader<InputReader, string, string> polynomialReader,
            IConversion<int, T> conversion,
            out Polynomial<T> resultPolynomial)
        {
            return this.TryParsePolynomial(polynomialReader, null, out resultPolynomial);
        }

        /// <summary>
        /// Efectua a leitura de um polinómio.
        /// </summary>
        /// <param name="polynomialReader">A cadeia de carácteres que contém o polinómimo.</param>
        /// <param name="conversion">O conversor entre o tipo de coeficiente e um inteiro.</param>
        /// <param name="errors">A lista de errros encontrados.</param>
        /// <param name="resultPolynomial">O polinómio lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso caso contrário.</returns>
        public bool TryParsePolynomial(
            MementoSymbolReader<InputReader, string, string> polynomialReader,
            IConversion<int, T> conversion,
            List<string> errors,
            out Polynomial<T> resultPolynomial)
        {
            if (polynomialReader == null)
            {
                throw new ArgumentNullException("polynomialReader");
            }

            if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }

            this.conversion = conversion;
            resultPolynomial = default(Polynomial<T>);
            var expressionReader = new ExpressionReader<ParsePolynomialItem<T>, string, string>(
                new SimplePolynomialReader<T>(this.coeffParser, this.ring));
            expressionReader.RegisterBinaryOperator("plus", Add, 0);
            expressionReader.RegisterBinaryOperator("times", Multiply, 1);
            expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
            expressionReader.RegisterBinaryOperator("hat", Power, 2);
            expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);

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

            var expressionResult = default(ParsePolynomialItem<T>);
            if (expressionReader.TryParse(polynomialReader, errors, out expressionResult))
            {
                if (expressionResult.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                    resultPolynomial = new Polynomial<T>(expressionResult.Coeff, this.ring);
                    return true;
                }
                else if (expressionResult.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    if (typeof(T).IsAssignableFrom(typeof(int)))
                    {
                        resultPolynomial = new Polynomial<T>((T)(object)expressionResult.Degree, this.ring);
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
        protected virtual ParsePolynomialItem<T> Add(ParsePolynomialItem<T> left, ParsePolynomialItem<T> right)
        {
            var result = new ParsePolynomialItem<T>();
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
        protected virtual ParsePolynomialItem<T> Multiply(ParsePolynomialItem<T> left, ParsePolynomialItem<T> right)
        {
            var result = new ParsePolynomialItem<T>();
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
        protected virtual ParsePolynomialItem<T> Subtract(ParsePolynomialItem<T> left, ParsePolynomialItem<T> right)
        {
            var result = new ParsePolynomialItem<T>();
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
        protected virtual ParsePolynomialItem<T> Divide(ParsePolynomialItem<T> left, ParsePolynomialItem<T> right)
        {
            var result = new ParsePolynomialItem<T>();
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
                        result.Polynomial.Multiply(field.MultiplicativeInverse(rightConverted), this.ring);
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
        protected virtual ParsePolynomialItem<T> Symmetric(ParsePolynomialItem<T> pol)
        {
            var result = new ParsePolynomialItem<T>();
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
        protected virtual ParsePolynomialItem<T> Power(ParsePolynomialItem<T> left, ParsePolynomialItem<T> right)
        {
            var result = new ParsePolynomialItem<T>();
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
                    result.Polynomial = left.Polynomial.Power(exponent, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                    result.Polynomial = left.Polynomial.Power(right.Degree, this.ring);
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                    if (right.Polynomial.IsValue)
                    {
                        var exponent = this.conversion.DirectConversion(right.Polynomial.GetAsValue(this.ring));
                        result.Polynomial = left.Polynomial.Power(exponent, this.ring);
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
