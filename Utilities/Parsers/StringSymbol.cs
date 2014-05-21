namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um símbolo cujo valor é texual.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolo.</typeparam>
    class StringSymbol<SymbType> : ISymbol<string, SymbType>
    {
        #region ISymbol Members

        /// <summary>
        /// Obtém ou atribui o valor.
        /// </summary>
        /// <value>O valor.</value>
        public string SymbolValue
        {
            get;
            set;
        }

        /// <summary>
        /// Obtém e atribui o tipo de símbolo.
        /// </summary>
        /// <value>O tipo de símbolo.</value>
        public SymbType SymbolType
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Constrói uma representação textual do símbolo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", this.SymbolValue, this.SymbolType);
        }
    }
}
