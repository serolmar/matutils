using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Utilities.Parsers
{
    class StringSymbol : ISymbol<string,string>
    {
        #region ISymbol Members

        public string SymbolValue
        {
            get;
            set;
        }

        public string SymbolType
        {
            get;
            set;
        }
        #endregion
        public override string ToString()
        {
            return string.Format("{0}:{1}", this.SymbolValue, this.SymbolType);
        }
    }
}
