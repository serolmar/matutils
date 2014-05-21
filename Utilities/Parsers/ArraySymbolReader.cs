namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite a leitura organizada a partir de um vector de símbolos.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo de objectos que constituem os valores.</typeparam>
    /// <typeparam name="TSymbType">O tipo de objectos que constituem os tipos de símbolos.</typeparam>
    public class ArraySymbolReader<TSymbVal, TSymbType> : MementoSymbolReader<ISymbol<TSymbVal, TSymbType>[], TSymbVal, TSymbType>
    {
        /// <summary>
        /// O símbolo correspondente ao fim do ficheiro.
        /// </summary>
        private TSymbType endOfFileSymbolType;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ArraySymbolReader{TSymbVal, TSymbType}"/>.
        /// </summary>
        /// <param name="arrayOfSymbols">O vector de símbolos.</param>
        /// <param name="endOfFileSymbolType">O tipo de símbolo que corresponde ao final do ficheiro.</param>
        public ArraySymbolReader(ISymbol<TSymbVal, TSymbType>[] arrayOfSymbols, TSymbType endOfFileSymbolType)
            : base(arrayOfSymbols)
        {
            this.bufferPointer = -1;
            this.symbolBuffer.AddRange(arrayOfSymbols);
            this.endOfFileSymbolType = endOfFileSymbolType;
        }

        /// <summary>
        /// Obtém o próximo símbolo mas não avança o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
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

        /// <summary>
        /// Lê o próximo símbolo avançando o cursor.
        /// </summary>
        /// <returns>O símbolo lido.</returns>
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

        /// <summary>
        /// Retrocede o cursor.
        /// </summary>
        public override void UnGet()
        {
            if (this.bufferPointer > -1)
            {
                --this.bufferPointer;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o leitor se encontra no final.
        /// </summary>
        /// <returns>Verdadeiro caso o leitor se encontre no final e falso caso contrário.</returns>
        public override bool IsAtEOF()
        {
            return this.bufferPointer >= this.symbolBuffer.Count - 1;
        }

        /// <summary>
        /// Obtém um valor que indica se o símbolo proporcionado correspode ao fim de ficheiro.
        /// </summary>
        /// <param name="symbol">O símbolo a ser verificado.</param>
        /// <returns>Verdadeiro caso o símbolo seja final de ficheiro e falso caso contrário.</returns>
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

        /// <summary>
        /// Impelmenta um símbolo lido de um vector de símbolos.
        /// </summary>
        private class ArraySymbol : ISymbol<TSymbVal, TSymbType>
        {
            /// <summary>
            /// O valor do símbolo.
            /// </summary>
            private TSymbVal symbolValue;

            /// <summary>
            /// O tipo do símbolo.
            /// </summary>
            private TSymbType symbolType;

            /// <summary>
            /// Obtém ou atribui o valor do símbolo.
            /// </summary>
            /// <value>
            /// O valor do símbolo.
            /// </value>
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

            /// <summary>
            /// Obtém ou atribui o tipo de símbolo.
            /// </summary>
            /// <value>
            /// O tipo de símbolo.
            /// </value>
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
