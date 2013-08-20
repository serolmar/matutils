using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Utilities.ExpressionBuilders
{
    public class BoolExpressionParser : IParse<bool, string, string>
    {
        private ExpressionReader<bool, string, string, ISymbol<string, string>[]> expressionReader;

        public BoolExpressionParser()
        {
            this.expressionReader = new ExpressionReader<bool, string, string, ISymbol<string, string>[]>(
                new BoolParser<string>());
            this.expressionReader.RegisterBinaryOperator("double_or", Or, 0);
            this.expressionReader.RegisterBinaryOperator("double_and", And, 1);
            this.expressionReader.RegisterUnaryOperator("not", Not, 0);
            this.expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            this.expressionReader.AddVoid("blancks");
            this.expressionReader.AddVoid("space");
            this.expressionReader.AddVoid("carriage_return");
            this.expressionReader.AddVoid("new_line");
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out bool value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParse(arrayReader, out value);
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
