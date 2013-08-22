namespace ConsoleTests.SetExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    class SetSymbol : ISymbol<string, ESymbolSetType>
    {
        private string symbolValue;

        private ESymbolSetType symbolType;

        public string SymbolValue
        {
            get
            {
                return this.symbolValue;
            }
            set
            {
                this.symbolValue = value;
            }
        }

        public ESymbolSetType SymbolType
        {
            get
            {
                return this.symbolType;
            }
            set
            {
                this.symbolType = value;
            }
        }
    }
}
