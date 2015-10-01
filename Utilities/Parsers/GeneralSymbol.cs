namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um símbolo geral.
    /// </summary>
    /// <typeparam name="SymbValue">O tipo de dados que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">O tipo de dados que constituem os tipos dos símbolos.</typeparam>
    public class GeneralSymbol<SymbValue, SymbType> : ISymbol<SymbValue, SymbType>
    {
        /// <summary>
        /// O valor do símbolo.
        /// </summary>
        private SymbValue symbolValue;

        /// <summary>
        /// O tipo do símbolo.
        /// </summary>
        private SymbType symbolType;

        /// <summary>
        /// Obtém ou atribui o valor do símbolo.
        /// </summary>
        public SymbValue SymbolValue
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
        /// Obtém ou atribui o tipo do símbolo.
        /// </summary>
        public SymbType SymbolType
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

        /// <summary>
        /// Constrói uma representação textual do símbolo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", this.SymbolValue, this.SymbolType);
        }
    }

    /// <summary>
    /// Implementa um símbolo cujo valor é texual.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolo.</typeparam>
    class StringSymbol<SymbType> : GeneralSymbol<string, SymbType>
    {
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
