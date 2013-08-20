using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Utilities.ExpressionBuilders
{
    public class IntegerExpressionParser : IParse<int, string, string>
    {
        private ExpressionReader<int, string, string, ISymbol<string, string>[]> expressionReader;

        public IntegerExpressionParser()
        {
            this.expressionReader = new ExpressionReader<int, string, string, ISymbol<string, string>[]>(
                new IntegerParser<string>());
            this.expressionReader.RegisterBinaryOperator("plus", Add, 0);
            this.expressionReader.RegisterBinaryOperator("times", Multiply, 1);
            this.expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
            this.expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
            this.expressionReader.RegisterBinaryOperator("over", Divide, 1);
            this.expressionReader.RegisterBinaryOperator("mod", Remainder, 1);
            this.expressionReader.RegisterBinaryOperator("hat", Power, 2);
            this.expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.AddVoid("blancks");
            this.expressionReader.AddVoid("space");
            this.expressionReader.AddVoid("carriage_return");
            this.expressionReader.AddVoid("new_line");
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out int value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParse(arrayReader, out value);
        }

        private int Add(int i, int j)
        {
            return i + j;
        }

        private int Subtract(int i, int j)
        {
            return i - j;
        }

        private int Multiply(int i, int j)
        {
            return i * j;
        }

        private int Divide(int i, int j)
        {
            if (j == 0)
            {
                throw new ExpressionReaderException("Error: division by zero.");
            }

            return i / j;
        }

        private int Symmetric(int i)
        {
            return -i;
        }

        private int Remainder(int dividend, int divisor)
        {
            return dividend % divisor;
        }

        private int Power(int value, int exponent)
        {
            if (exponent < 0)
            {
                throw new ExpressionReaderException("Negative exponents aren't allowed.");
            }
            else
            {
                return (int)Math.Pow(value, exponent);
            }
        }
    }
}
