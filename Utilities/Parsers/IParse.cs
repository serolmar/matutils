namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IParse<T, SymbValue, SymbType>
    {
        bool TryParse(ISymbol<SymbValue, SymbType>[] symbolListToParse, out T value);
    }
}
