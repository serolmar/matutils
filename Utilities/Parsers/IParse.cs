namespace Utilities.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IParse<T, SymbValue, SymbType>
    {
        T Parse(ISymbol<SymbValue, SymbType>[] symbolListToParse);
    }
}
