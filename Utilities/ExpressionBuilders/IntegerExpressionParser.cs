using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Utilities.ExpressionBuilders
{
    public class IntegerExpressionParser
    {
        public int Parse(string valueToParse)
        {
            if (string.IsNullOrWhiteSpace(valueToParse))
            {
                throw new ExpressionReaderException("Error: no value provided.");
            }

            var stringReader = new StringReader(valueToParse);
            return this.Parse(stringReader);
        }

        public int Parse(TextReader reader)
        {
            var symbolReader = new StringSymbolReader(reader, false);
            ExpressionReader<int, CharSymbolReader> result = new ExpressionReader<int, CharSymbolReader>(new IntegerParser());
            result.RegisterBinaryOperator("plus", Add, 0);
            result.RegisterBinaryOperator("times", Multiply, 1);
            result.RegisterBinaryOperator("minus", Subtract, 0);
            result.RegisterUnaryOperator("minus", Symmetric, 0);
            result.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            result.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            result.AddVoid("space");
            result.AddVoid("carriage_return");
            result.AddVoid("new_line");
            
            return result.Parse(symbolReader);
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
    }
}
