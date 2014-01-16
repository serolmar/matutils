namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    class StringSymbol<SymbType> : ISymbol<string, SymbType>
    {
        #region ISymbol Members

        public string SymbolValue
        {
            get;
            set;
        }

        public SymbType SymbolType
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
