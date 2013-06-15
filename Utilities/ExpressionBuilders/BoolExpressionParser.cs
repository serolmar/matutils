using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Utilities.ExpressionBuilders
{
    public class BoolExpressionParser
    {
        public bool Parse(TextReader reader)
        {
            var symbolReader = new StringSymbolReader(reader, false);
            ExpressionReader<bool, CharSymbolReader> result = new ExpressionReader<bool, CharSymbolReader>(new BoolParser());
            result.RegisterBinaryOperator("double_or", Or, 0);
            result.RegisterBinaryOperator("double_and", And, 1);
            result.RegisterUnaryOperator("not", Not, 0);
            result.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            result.AddVoid("space");
            result.AddVoid("carriage_return");
            result.AddVoid("new_line");
            return result.Parse(symbolReader);
        }

        private bool Or(bool i, bool j)
        {
            return i || j;
        }

        private bool And(bool i, bool j)
        {
            return i && j;
        }

        private bool Not(bool i)
        {
            return !i;
        }
    }
}
