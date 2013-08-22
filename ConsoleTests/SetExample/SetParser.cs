namespace ConsoleTests.SetExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    class SetParser<ObjectType> : IParse<HashSet<ObjectType>, string, ESymbolSetType>
    {
        public bool TryParse(
            ISymbol<string, ESymbolSetType>[] symbolListToParse, 
            out HashSet<ObjectType> value)
        {
            throw new NotImplementedException();
        }
    }
}
