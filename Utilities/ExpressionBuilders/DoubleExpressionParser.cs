using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Utilities.ExpressionBuilders
{
    public class DoubleExpressionParser
    {
        public double Parse(TextReader reader)
        {
            var symbolReader = new StringSymbolReader(reader, false);
            ExpressionReader<double, string, string, CharSymbolReader> result = new ExpressionReader<double, string, string, CharSymbolReader>(new DoubleParser());
            result.RegisterBinaryOperator("plus", Add, 0);
            result.RegisterBinaryOperator("times", Multiply, 1);
            result.RegisterBinaryOperator("minus", Subtract, 0);
            result.RegisterUnaryOperator("minus", Symmetric, 0);
            result.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            result.AddVoid("space");
            result.AddVoid("carriage_return");
            result.AddVoid("new_line");
            return result.Parse(symbolReader);
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
    }
}
