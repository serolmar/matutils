namespace ConsoleTests.SetExample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    class SetSymbolReader : SymbolReader<CharSymbolReader<ESymbolSetType>, string, ESymbolSetType>
    {
        public SetSymbolReader(TextReader inputReader) : 
            base(new CharSymbolReader<ESymbolSetType>(inputReader))
        {
            this.inputStream.EndOfFileType = ESymbolSetType.EOF;
            this.inputStream.GenericType = ESymbolSetType.ANY;
            this.inputStream.RegisterCharType('{', ESymbolSetType.LBRACE);
            this.inputStream.RegisterCharType('{', ESymbolSetType.LBRACE);
            this.inputStream.RegisterCharType('[', ESymbolSetType.LBRACK);
            this.inputStream.RegisterCharType(']', ESymbolSetType.RBRACK);
            this.inputStream.RegisterCharType('(', ESymbolSetType.OPAR);
            this.inputStream.RegisterCharType(')', ESymbolSetType.CPAR);
            this.inputStream.RegisterCharType('<', ESymbolSetType.LANGLE);
            this.inputStream.RegisterCharType('>', ESymbolSetType.RANGLE);
            this.inputStream.RegisterCharType('|', ESymbolSetType.VBAR);
            this.inputStream.RegisterCharType(',', ESymbolSetType.COMMA);
            this.inputStream.RegisterCharType('\n', ESymbolSetType.CHANGE_LINE);
            this.inputStream.RegisterCharType(' ', ESymbolSetType.SPACE);
        }

        public override ISymbol<string, ESymbolSetType> Peek()
        {
            if (!this.started)
            {
                this.started = true;
            }

            if (this.bufferPointer == this.symbolBuffer.Count)
            {
                this.AddNextSymbolFromStream();
            }

            var result = new SetSymbol()
            {
                SymbolType = this.symbolBuffer[this.bufferPointer].SymbolType,
                SymbolValue = this.symbolBuffer[this.bufferPointer].SymbolValue
            };

            return result;
        }

        public override ISymbol<string, ESymbolSetType> Get()
        {
            if (!this.started)
            {
                this.started = true;
            }

            var result = this.Peek();
            if (this.bufferPointer < this.symbolBuffer.Count)
            {
                ++this.bufferPointer;
            }

            return result;
        }

        public override void UnGet()
        {
            if (this.bufferPointer > 0)
            {
                --this.bufferPointer;
            }
        }

        public override bool IsAtEOF()
        {
            return this.Peek().SymbolType.Equals(ESymbolSetType.EOF);
        }

        public override bool IsAtEOFSymbol(ISymbol<string, ESymbolSetType> symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            else
            {
                return symbol.SymbolType.Equals(ESymbolSetType.EOF);
            }
        }

        private void AddNextSymbolFromStream()
        {
            var readed = string.Empty;
            var state = 0;
            while (state == 0)
            {
                var readedChar = this.inputStream.Peek();
                if (readedChar.SymbolType == ESymbolSetType.EOF)
                {
                    this.symbolBuffer.Add(readedChar);
                    state = -1;
                }
                else if (readedChar.SymbolType == ESymbolSetType.ANY)
                {
                    readed += readedChar.SymbolValue;
                    state = 1;
                }
                else
                {
                    this.symbolBuffer.Add(readedChar);
                }
            }

            if (state == 1)
            {
                var result = new SetSymbol() { SymbolValue = readed, SymbolType = ESymbolSetType.ANY };

                if (readed == "union")
                {
                    result.SymbolType = ESymbolSetType.UNION;
                }
                else if (readed == "intersection")
                {
                    result.SymbolType = ESymbolSetType.INTERSECTION;
                }
            }
        }
    }
}
