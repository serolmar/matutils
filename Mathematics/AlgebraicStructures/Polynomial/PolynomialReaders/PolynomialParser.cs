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
            var expressionReader = new ExpressionReader<Polynomial<T, RingType>, CharSymbolReader>(new SimplePolynomialParser<T, RingType>(this.coeffParser, this.ring));
            expressionReader.RegisterBinaryOperator("plus", Add, 0);
            expressionReader.RegisterBinaryOperator("times", Multiply, 1);
            expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
            expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
            expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            expressionReader.AddVoid("space");
            expressionReader.AddVoid("carriage_return");
            expressionReader.AddVoid("new_line");

            return expressionReader.Parse(stringSymbolReader);
        }

        /// <summary>
        /// Adiciona dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio a adicionar.</param>
        /// <param name="right">O segundo polinómio a adicionar.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual Polynomial<T, RingType> Add(Polynomial<T, RingType> left, Polynomial<T, RingType> right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Multiplica dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual Polynomial<T, RingType> Multiply(Polynomial<T, RingType> left, Polynomial<T, RingType> right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Subtrai dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual Polynomial<T, RingType> Subtract(Polynomial<T, RingType> left, Polynomial<T, RingType> right)
        {
            var inversedUnity = new Polynomial<T, RingType>(this.ring.AdditiveInverse(this.ring.MultiplicativeUnity), this.ring);
            return left.Add(right.Multiply(inversedUnity));
        }

        /// <summary>
        /// Divide dois polinómios.
        /// </summary>
        /// <param name="left">O primeiro polinómio.</param>
        /// <param name="right">O segundo polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        protected virtual Polynomial<T, RingType> Divide(Polynomial<T, RingType> left, Polynomial<T, RingType> right)
        {
            if (right.IsValue)
            {
                var value = right.GetAsValue();
                if (this.ring.IsAdditiveUnity(value))
                {
                    throw new MathematicsException("Division by zero or null.");
                }
                else
                {
                    var field = right as IField<T>;
                    if (field == null)
                    {
                        throw new MathematicsException("The provided ring isn't a field.");
                    }
                    else
                    {
                        var inversedValue = field.MultiplicativeInverse(value);
                        return left.Multiply(new Polynomial<T, RingType>(inversedValue, this.ring));
                    }
                }
            }
            else
            {
                throw new MathematicsException("Can't divide two polynomials.");
            }
        }

        /// <summary>
        /// Obtém o simétrico de um polinómio.
        /// </summary>
        /// <param name="pol">O polinómio.</param>
        /// <returns>O polinómio resultante.</returns>
        private Polynomial<T, RingType> Symmetric(Polynomial<T, RingType> pol)
        {
            var inversedUnity = new Polynomial<T, RingType>(this.ring.AdditiveInverse(this.ring.MultiplicativeUnity), this.ring);
            return pol.Multiply(inversedUnity);
        }
    }
}
