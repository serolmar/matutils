using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    public abstract class SymbolReader<InputReader, TSymbVal, TSymbType>
    {
        protected InputReader inputStream;
        protected List<ISymbol<TSymbVal, TSymbType>> symbolBuffer = new List<ISymbol<TSymbVal, TSymbType>>();
        protected int bufferPointer = 0;
        protected bool started = false;

        public SymbolReader(InputReader inputTextStream)
        {
            if (inputTextStream == null)
            {
                throw new ArgumentNullException("An input reader must be provided.");
            }

            this.inputStream = inputTextStream;
        }

        public bool Started
        {
            get
            {
                return this.started;
            }
        }

        public abstract ISymbol<TSymbVal, TSymbType> Peek();
        public abstract ISymbol<TSymbVal, TSymbType> Get();
        public abstract void UnGet();
        public abstract bool IsAtEOF();
        public abstract bool IsAtEOFSymbol(ISymbol<TSymbVal, TSymbType> symbol);
    }
}
