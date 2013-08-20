using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities.ExpressionBuilders;
using Utilities.Parsers;

namespace Utilities.Parsers
{
    public class StringSymbolReader : MementoSymbolReader<CharSymbolReader<string>,string, string>
    {
        private static string endOfFile = "eof";

        private ISymbol<string,string> currentSymbol = new StringSymbol<string>();
        private List<IState<TextReader, string, string>> stateList = new List<IState<TextReader, string, string>>();
        private Dictionary<string, string> keyWords = new Dictionary<string, string>();
        private bool readNegativeNumbers = true;

        public StringSymbolReader(TextReader reader, bool isToReadNegativeNumbers)
            : base(new CppCompliantCharSymbolReaderBuilder().BuildReader(reader) as CharSymbolReader<string>)
        {
            this.readNegativeNumbers = isToReadNegativeNumbers;
            this.currentSymbol.SymbolType = "any";
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(0, "start", this.StartTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(1, "string", this.StringTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(2, "number", this.NumberTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(3, "equal", this.EqualTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(4, "greater", this.GreaterTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(5, "lesser", this.LesserTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(6, "or", this.OrTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(7, "and", this.AndTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(8, "colon", this.ColonTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(9, "plus", this.PlusTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(10, "minus", this.MinusTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(11, "times", this.TimesTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(12, "over", this.OverTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(13, "point", this.PointTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(14, "blancks", this.BlanckTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(15, "exponential", this.ExponentialTransition));
            stateList.Add(new DelegateDrivenState<TextReader, string, string>(16, "end", this.EndTransition));
        }

        public override ISymbol<string,string> Peek()
        {
            if (!this.started)
            {
                this.started = true;
            }
            if (this.bufferPointer == this.symbolBuffer.Count)
            {
                this.AddNextSymbolFromStream();
            }
            StringSymbol<string> result = new StringSymbol<string>()
            {
                SymbolType = this.symbolBuffer[this.bufferPointer].SymbolType,
                SymbolValue = this.symbolBuffer[this.bufferPointer].SymbolValue
            };
            return result;
        }

        public override ISymbol<string,string> Get()
        {
            if (!this.started)
            {
                this.started = true;
            }
            ISymbol<string,string> result = this.Peek();
            if (this.bufferPointer < this.symbolBuffer.Count)
            {
                ++this.bufferPointer;
            }
            return result;
        }

        public override void UnGet()
        {
            if (bufferPointer > 0)
            {
                --this.bufferPointer;
            }
        }

        public override bool IsAtEOF()
        {
            return this.Peek().SymbolType.Equals(StringSymbolReader.endOfFile);
        }

        public override bool IsAtEOFSymbol(ISymbol<string, string> symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            else
            {
                return symbol.SymbolType.Equals(StringSymbolReader.endOfFile);
            }
        }

        public void RegisterKeyWordType(string key, string type)
        {
            if (this.keyWords.ContainsKey(key))
            {
                this.keyWords[key] = type;
            }
            else
            {
                this.keyWords.Add(key, type);
            }
        }

        private void AddNextSymbolFromStream()
        {
            this.currentSymbol = new StringSymbol<string>() { SymbolType = string.Empty, SymbolValue = string.Empty };
            StateMachine<TextReader, string, string> machine = new StateMachine<TextReader, string,string>(
                this.stateList[0],
                this.stateList[16]);
            machine.RunMachine(this.inputStream);
            var result = new StringSymbol<string>();
            result.SymbolType = this.currentSymbol.SymbolType;
            result.SymbolValue = this.currentSymbol.SymbolValue;
            this.symbolBuffer.Add(result);
        }

        #region transition functions
        private IState<TextReader, string, string> StartTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string,string> peeked = reader.Peek();
            switch (peeked.SymbolType)
            {
                case "alpha":
                case "underscore":
                    return this.stateList[1];
                case "algarism":
                    return this.stateList[2];
                case "equal":
                    return this.stateList[3];
                case "great_than":
                    return this.stateList[4];
                case "less_than":
                    return this.stateList[5];
                case "bitwise_or":
                    return this.stateList[6];
                case "bitwise_and":
                    return this.stateList[7];
                case "colon":
                    return this.stateList[8];
                case "plus":
                    return this.stateList[9];
                case "minus":
                    return this.stateList[10];
                case "times":
                    return this.stateList[11];
                case "right_bar":
                    return this.stateList[12];
                case "point":
                    return this.stateList[13];
                case "space":
                case "new_line":
                case "carriage_return":
                case "tab":
                    return this.stateList[14];
                default:
                    this.currentSymbol = peeked;
                    reader.Get();
                    break;
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> StringTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (!this.currentSymbol.SymbolType.Equals("string"))
            {
                this.currentSymbol.SymbolType = "string";
            }
            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            switch (symbol.SymbolType)
            {
                case "alpha":
                case "underscore":
                case "algarism":
                    return this.stateList[1];
            }
            if (this.keyWords.ContainsKey(this.currentSymbol.SymbolValue))
            {
                this.currentSymbol.SymbolType = this.keyWords[this.currentSymbol.SymbolValue];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> NumberTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (
                this.currentSymbol.SymbolType.Equals("any") ||
                this.currentSymbol.SymbolType.Equals(string.Empty) ||
                this.currentSymbol.SymbolType.Equals("minus"))
            {
                this.currentSymbol.SymbolType = "integer";
            }
            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("algarism"))
            {
                return this.stateList[2];
            }
            else if (symbol.SymbolType.Equals("point") && this.currentSymbol.SymbolType.Equals("integer"))
            {
                return this.stateList[13];
            }
            else if (symbol.SymbolType.Equals("alpha") && (symbol.SymbolValue.Equals("e") || symbol.SymbolValue.Equals("E")))
            {
                return this.stateList[15];
            }
            else
            {
                if (this.currentSymbol.SymbolType.Equals("double_exponential"))
                {
                    this.currentSymbol.SymbolType = "double";
                }
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> EqualTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "equal":
                    this.currentSymbol.SymbolType = "double_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "plus":
                    this.currentSymbol.SymbolType = "plus_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "minus":
                    this.currentSymbol.SymbolType = "minus_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "times":
                    this.currentSymbol.SymbolType = "times_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "over":
                    this.currentSymbol.SymbolType = "over_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "or":
                    this.currentSymbol.SymbolType = "or_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "and":
                    this.currentSymbol.SymbolType = "and_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "great_than":
                    this.currentSymbol.SymbolType = "great_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "less_than":
                    this.currentSymbol.SymbolType = "less_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> GreaterTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "great_than":
                    this.currentSymbol.SymbolType = "double_great";
                    this.currentSymbol.SymbolValue = ">>";
                    break;
                case "double_great":
                    this.currentSymbol.SymbolType = "triple_great";
                    this.currentSymbol.SymbolValue = ">>>";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("great_than") &&
                (this.currentSymbol.SymbolType.Equals("great_than") || this.currentSymbol.SymbolType.Equals("double_great")))
            {
                return this.stateList[4];
            }
            else if (symbol.SymbolType.Equals("equal") && this.currentSymbol.SymbolType.Equals("great_than"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> LesserTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "less_than":
                    this.currentSymbol.SymbolType = "double_less";
                    this.currentSymbol.SymbolValue = "<<";
                    break;
                case "double_less":
                    this.currentSymbol.SymbolType = "triple_less";
                    this.currentSymbol.SymbolValue = "<<<";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("less_than") &&
                (this.currentSymbol.SymbolType.Equals("less_than") || this.currentSymbol.SymbolType.Equals("double_less")))
            {
                return this.stateList[5];
            }
            else if (symbol.SymbolType.Equals("equal") && this.currentSymbol.SymbolType.Equals("less_than"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> OrTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "bitwise_or":
                    this.currentSymbol.SymbolType = "double_or";
                    this.currentSymbol.SymbolValue = "||";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("bitwise_or") && this.currentSymbol.SymbolType.Equals("bitwise_or"))
            {
                return this.stateList[6];
            }
            else if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> AndTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "bitwise_and":
                    this.currentSymbol.SymbolType = "double_and";
                    this.currentSymbol.SymbolValue = "&&";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("bitwise_and") && this.currentSymbol.SymbolType.Equals("bitwise_and"))
            {
                return this.stateList[7];
            }
            else if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string,string> ColonTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "colon":
                    this.currentSymbol.SymbolType = "double_colon";
                    this.currentSymbol.SymbolValue = "::";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("colon") && this.currentSymbol.SymbolType.Equals("colon"))
            {
                return this.stateList[8];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> PlusTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "plus":
                    this.currentSymbol.SymbolType = "double_plus";
                    this.currentSymbol.SymbolValue = "++";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("plus") && this.currentSymbol.SymbolType.Equals("plus"))
            {
                return this.stateList[9];
            }
            else if (symbol.SymbolType.Equals("equal") && !this.currentSymbol.SymbolType.Equals("double_plus"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> MinusTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "minus":
                    this.currentSymbol.SymbolType = "double_minus";
                    this.currentSymbol.SymbolValue = "--";
                    break;
                case "double":
                    this.currentSymbol.SymbolValue += "-";
                    break;
                case "double_exponential":
                    this.currentSymbol.SymbolValue += "-";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("minus") && this.currentSymbol.SymbolType.Equals("minus"))
            {
                return this.stateList[10];
            }
            else if (symbol.SymbolType.Equals("equal") && this.currentSymbol.SymbolType.Equals("minus"))
            {
                return this.stateList[3];
            }
            else if (this.currentSymbol.SymbolType.Equals("double_minus"))
            {
                return this.stateList[16];
            }
            else if (this.currentSymbol.SymbolType.Equals("exponential_double"))
            {
                if (symbol.SymbolType.Equals("algarism"))
                {
                    return this.stateList[2];
                }
                else
                {
                    this.currentSymbol.SymbolType = "double";
                    this.currentSymbol.SymbolValue = this.currentSymbol.SymbolValue.Substring(0, this.currentSymbol.SymbolValue.Length - 2);
                    reader.UnGet();
                    reader.UnGet();
                }
            }
            else
            {
                if (symbol.SymbolType.Equals("algarism") && this.readNegativeNumbers)
                {
                    return this.stateList[2];
                }
                else if (symbol.SymbolType.Equals("point"))
                {
                    return this.stateList[13];
                }
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> TimesTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "times":
                    this.currentSymbol.SymbolType = "double_times";
                    this.currentSymbol.SymbolValue = "**";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("times") && this.currentSymbol.SymbolType.Equals("times"))
            {
                return this.stateList[11];
            }
            else if (symbol.SymbolType.Equals("equal") && !this.currentSymbol.SymbolType.Equals("double_times"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> OverTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            this.currentSymbol = symbol;
            this.currentSymbol.SymbolType = "over";
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> PointTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "minus":
                    this.currentSymbol.SymbolType = "minus_point";
                    this.currentSymbol.SymbolValue += symbol.SymbolValue;
                    break;
                case "integer":
                    this.currentSymbol.SymbolValue += symbol.SymbolValue;
                    this.currentSymbol.SymbolType = "double";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("algarism"))
            {
                this.currentSymbol.SymbolType = "double";
                return this.stateList[2];
            }
            else
            {
                if (this.currentSymbol.SymbolType.Equals("minus_point"))
                {
                    this.currentSymbol.SymbolType = "minus";
                    this.currentSymbol.SymbolValue = this.currentSymbol.SymbolValue.Substring(0, this.currentSymbol.SymbolValue.Length - 1);
                    reader.UnGet();
                }
                else if (this.currentSymbol.SymbolType.Equals("double"))
                {
                    this.currentSymbol.SymbolType = "integer";
                    this.currentSymbol.SymbolValue = this.currentSymbol.SymbolValue.Substring(0, this.currentSymbol.SymbolValue.Length - 1);
                    reader.UnGet();
                }
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> BlanckTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (!this.currentSymbol.SymbolType.Equals("blancks"))
            {
                this.currentSymbol.SymbolType = "blancks";
            }
            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            switch (symbol.SymbolType)
            {
                case "space":
                case "new_line":
                case "carriage_return":
                case "tab":
                    return this.stateList[14];
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> ExponentialTransition(SymbolReader<TextReader, string, string> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (!this.currentSymbol.SymbolType.Equals("double_exponential_minus"))
            {
                this.currentSymbol.SymbolType = "double_exponential";
            }
            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("minus") && this.currentSymbol.SymbolType.Equals("double_exponential"))
            {

                this.currentSymbol.SymbolType = "double_exponential_minus";
                return this.stateList[15];
            }
            else if (symbol.SymbolType.Equals("algarism"))
            {
                this.currentSymbol.SymbolType = "double_exponential";
                return this.stateList[2];
            }
            else
            {
                reader.UnGet();
                string val = this.currentSymbol.SymbolValue;
                if (this.currentSymbol.SymbolType.Equals("double_exponential"))
                {
                    val = val.Substring(0, val.Length - 1);
                }
                else
                {
                    val = val.Substring(0, val.Length - 2);
                    reader.UnGet();
                }
                this.currentSymbol = new StringSymbol<string>()
                {
                    SymbolType = this.GetTypeFromNumberRepresentation(val),
                    SymbolValue = val
                };
            }
            return this.stateList[16];
        }

        private IState<TextReader, string, string> EndTransition(SymbolReader<TextReader, string, string> reader)
        {
            return null;
        }

        #endregion

        private string GetTypeFromNumberRepresentation(string number)
        {
            if (number.Contains('.'))
            {
                return "double";
            }
            return "integer";
        }
    }
}
