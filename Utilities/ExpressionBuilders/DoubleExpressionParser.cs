namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    public class DoubleExpressionParser : IParse<double, string, string>, IParse<object, string, string>
    {
        private ExpressionReader<double, string, string, ISymbol<string, string>[]> expressionReader;

        public DoubleExpressionParser()
        {
            this.expressionReader = new ExpressionReader<double, string, string, ISymbol<string, string>[]>(
                new DoubleParser<string>());
            this.expressionReader.RegisterBinaryOperator("plus", Add, 0);
            this.expressionReader.RegisterBinaryOperator("times", Multiply, 1);
            this.expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
            this.expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
            this.expressionReader.RegisterBinaryOperator("over", Divide, 1);
            this.expressionReader.RegisterBinaryOperator("hat", Power, 2);
            this.expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.AddVoid("blancks");
            this.expressionReader.AddVoid("space");
            this.expressionReader.AddVoid("carriage_return");
            this.expressionReader.AddVoid("new_line");
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out double value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParse(arrayReader, out value);
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out object value)
        {
            var temp = default(double);
            if (this.TryParse(symbolListToParse, out temp))
            {
                value = temp;
                return true;
            }
            else
            {
                value = default(object);
                return false;
            }
        }

        private double Add(double i, double j)
        {
            return i + j;
        }

        private double Subtract(double i, double j)
        {
            return i - j;
        }

        private double Multiply(double i, double j)
        {
            return i * j;
        }

        private double Divide(double i, double j)
        {
            if (j == 0)
            {
                throw new ExpressionReaderException("Error: division by zero.");
            }

            return i / j;
        }

        private double Symmetric(double i)
        {
            return -i;
        }

        private double Power(double value, double exponent)
        {
            return Math.Pow(value, exponent);
        }
    }
}
