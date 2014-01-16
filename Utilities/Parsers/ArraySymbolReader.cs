namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite a leitura organizada a partir de um vector de símbolos.
    /// </summary>
    public class ArraySymbolReader<TSymbVal, TSymbType> : MementoSymbolReader<ISymbol<TSymbVal, TSymbType>[], TSymbVal, TSymbType>
    {
        /// <summary>
        /// O símbolo correspondente ao fim do ficheiro.
        /// </summary>
        private TSymbType endOfFileSymbolType;

        public ArraySymbolReader(ISymbol<TSymbVal, TSymbType>[] arrayOfSymbols, TSymbType endOfFileSymbolType)
            : base(arrayOfSymbols)
        {
            this.bufferPointer = -1;
            this.symbolBuffer.AddRange(arrayOfSymbols);
            this.endOfFileSymbolType = endOfFileSymbolType;
        }

        public override ISymbol<TSymbVal, TSymbType> Peek()
        {
            var nextPointer = this.bufferPointer + 1;
            if (nextPointer >= this.symbolBuffer.Count)
            {
                return new ArraySymbol() { SymbolValue = default(TSymbVal), SymbolType = this.endOfFileSymbolType };
            }
            else
            {
                return this.symbolBuffer[nextPointer];
            }
        }

        public override ISymbol<TSymbVal, TSymbType> Get()
        {
            if (this.bufferPointer < this.symbolBuffer.Count)
            {
                ++this.bufferPointer;
                if (this.bufferPointer < this.symbolBuffer.Count)
                {
                    return this.symbolBuffer[this.bufferPointer];
                }
                else
                {
                    return new ArraySymbol() { SymbolValue = default(TSymbVal), SymbolType = this.endOfFileSymbolType };
                }
            }
            else
            {
                return new ArraySymbol() { SymbolValue = default(TSymbVal), SymbolType = this.endOfFileSymbolType };
            }
        }

        public override void UnGet()
        {
            if (this.bufferPointer > -1)
            {
                --this.bufferPointer;
            }
        }

        public override bool IsAtEOF()
        {
            return this.bufferPointer >= this.symbolBuffer.Count - 1;
        }

        public override bool IsAtEOFSymbol(ISymbol<TSymbVal, TSymbType> symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            else
            {
                return this.endOfFileSymbolType.Equals(symbol.SymbolType);
            }
        }

        private class ArraySymbol : ISymbol<TSymbVal, TSymbType>
        {
            private TSymbVal symbolValue;

            private TSymbType symbolType;

            public TSymbVal SymbolValue
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

            public TSymbType SymbolType
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
}
