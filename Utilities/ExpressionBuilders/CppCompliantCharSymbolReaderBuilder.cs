namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    public interface ReaderBuilder
    {
        SymbolReader<TextReader,string, string> BuildReader(TextReader input);
    }

    class CppCompliantCharSymbolReaderBuilder : ReaderBuilder
    {
        public SymbolReader<TextReader,string, string> BuildReader(TextReader input)
        {
            CharSymbolReader<string> result = new CharSymbolReader<string>(input);
            result.RegisterCharType('_', "underscore");
            result.RegisterCharType('(', "left_parenthesis");
            result.RegisterCharType(')', "right_parenthesis");
            result.RegisterCharType('[', "left_bracket");
            result.RegisterCharType(']', "right_bracket");
            result.RegisterCharType('"', "double_quote");
            result.RegisterCharType('\'', "quote");
            result.RegisterCharType('+', "plus");
            result.RegisterCharType('-', "minus");
            result.RegisterCharType('*', "times");
            result.RegisterCharType('/', "right_bar");
            result.RegisterCharType('\\', "left_bar");
            result.RegisterCharType('.', "point");
            result.RegisterCharType(':', "colon");
            result.RegisterCharType(';', "semi_colon");
            result.RegisterCharType(',', "comma");
            result.RegisterCharType('~', "tild");
            result.RegisterCharType('^', "hat");
            result.RegisterCharType('?', "question_mark");
            result.RegisterCharType('!', "exclamation_mark");
            result.RegisterCharType('<', "less_than");
            result.RegisterCharType('>', "great_than");
            result.RegisterCharType('|', "bitwise_or");
            result.RegisterCharType('&', "bitwise_and");
            result.RegisterCharType(' ', "space");
            result.RegisterCharType('\n', "new_line");
            result.RegisterCharType('\t', "tab");
            result.RegisterCharType('\r', "carriage_return");
            result.RegisterCharType('=', "equal");
            result.RegisterCharType('{', "left_brace");
            result.RegisterCharType('}', "right_brace");
            result.RegisterCharType('@', "at");
            result.RegisterCharType('#', "cardinal");
            result.RegisterCharType('$', "dollar");
            result.RegisterCharType('%', "mod");
            result.RegisterCharType('£', "pound");
            result.RegisterCharType('§', "chapter");
            result.RegisterCharType('€', "euro");
            result.DeciderFunction = this.DeciderFunction;
            result.GenericType = "any";
            result.EndOfFileType = "eof";
            return result;
        }

        private string DeciderFunction(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) return "alpha";
            if (c >= '0' && c <= '9') return "algarism";
            return "any";
        }
    }
}
