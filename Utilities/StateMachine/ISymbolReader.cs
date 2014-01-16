namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISymbolReader<TSymbVal, TSymbType> : IObjectReader<ISymbol<TSymbVal, TSymbType>>
    {
        bool IsAtEOFSymbol(ISymbol<TSymbVal, TSymbType> symbol);
    }
}
