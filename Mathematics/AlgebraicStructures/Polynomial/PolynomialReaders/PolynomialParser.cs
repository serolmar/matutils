namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    public class PolynomialParser<T, RingType>
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
        /// Maps the external delimiters. External delimiters bounds entire elementary subexpressions.
        /// </summary>
        private Dictionary<string, List<string>> externalDelimitersTypes = new Dictionary<string, List<string>>();

        public PolynomialParser(IParse<T, string, string> coeffParser, RingType ring)
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
        /// <param name="polynomial">A cadeia de carácteres que contém o polinómimo.</param>
        /// <returns>O polinómio lido.</returns>
        public Polynomial<T, RingType> Parse(string polynomial)
        {
            if (string.IsNullOrWhiteSpace(polynomial))
            {
                throw new MathematicsException("Empty string for polynomial.");
            }

            var stringSymbolReader = new StringSymbolReader(new StringReader(polynomial), false);
            var expressionReader = new ExpressionReader<ParsePolynomialItem<T, RingType>, CharSymbolReader>(new SimplePolynomialParser<T, RingType>(this.coeffParser, this.ring));
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

            var expressionResult = expressionReader.Parse(stringSymbolReader);
            return expressionResult.Polynomial;
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
        protected virtual ParsePolynomialItem<T, RingType> Add(ParsePolynomialItem<T, RingType> left, ParsePolynomialItem<T, RingType> right)
        {
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }

            throw new NotImplementedException("Something went very wrong.");
        }

        /// <summary>
        /// Multiplica dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParsePolynomialItem<T, RingType> Multiply(ParsePolynomialItem<T, RingType> left, ParsePolynomialItem<T, RingType> right)
        {
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }

            throw new NotImplementedException("Something went very wrong.");
        }

        /// <summary>
        /// Subtrai dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParsePolynomialItem<T, RingType> Subtract(ParsePolynomialItem<T, RingType> left, ParsePolynomialItem<T, RingType> right)
        {
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }

            throw new NotImplementedException("Something went very wrong.");
        }

        /// <summary>
        /// Divide dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParsePolynomialItem<T, RingType> Divide(ParsePolynomialItem<T, RingType> left, ParsePolynomialItem<T, RingType> right)
        {
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }

            throw new NotImplementedException("Something went very wrong.");
            //if (right.IsValue)
            //{
            //    var value = right.GetAsValue();
            //    if (this.ring.IsAdditiveUnity(value))
            //    {
            //        throw new MathematicsException("Division by zero or null.");
            //    }
            //    else
            //    {
            //        var field = right as IField<T>;
            //        if (field == null)
            //        {
            //            throw new MathematicsException("The provided ring isn't a field.");
            //        }
            //        else
            //        {
            //            var inversedValue = field.MultiplicativeInverse(value);
            //            return left.Multiply(new Polynomial<T, RingType>(inversedValue, this.ring));
            //        }
            //    }
            //}
            //else
            //{
            //    throw new MathematicsException("Can't divide two polynomials.");
            //}
        }

        /// <summary>
        /// Obtém o simétrico de um polinómio.
        /// </summary>
        /// <param name="pol">O polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual ParsePolynomialItem<T, RingType> Symmetric(ParsePolynomialItem<T, RingType> pol)
        {
            //var inversedUnity = new Polynomial<T, RingType>(this.ring.AdditiveInverse(this.ring.MultiplicativeUnity), this.ring);
            //return pol.Multiply(inversedUnity);
            if (pol.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
            }
            else if (pol.ValueType == EParsePolynomialValueType.INTEGER)
            {
            }
            else if (pol.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
            }

            throw new NotImplementedException("Something went very wrong.");
        }

        /// <summary>
        /// Determina a potência do polinómio.
        /// </summary>
        /// <param name="left">O polinómio.</param>
        /// <param name="right">O expoente.</param>
        /// <returns>O resultado da potência.</returns>
        protected virtual ParsePolynomialItem<T, RingType> Power(ParsePolynomialItem<T, RingType> left, ParsePolynomialItem<T, RingType> right)
        {
            if (left.ValueType == EParsePolynomialValueType.COEFFICIENT)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.INTEGER)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }
            else if (left.ValueType == EParsePolynomialValueType.POLYNOMIAL)
            {
                if (right.ValueType == EParsePolynomialValueType.COEFFICIENT)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.INTEGER)
                {
                }
                else if (right.ValueType == EParsePolynomialValueType.POLYNOMIAL)
                {
                }
            }

            throw new NotImplementedException("Something went very wrong.");
        }
    }
}
