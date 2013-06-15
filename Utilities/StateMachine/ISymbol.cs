using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utilities.Parsers
{
    /// <summary>
    /// A symbol to be read.
    /// </summary>
    /// <remarks>
    /// A symbol contains the symbol value and its type.
    /// </remarks>
    /// <typeparam name="TSymbVal">The symbol value type.</typeparam>
    public interface ISymbol<TSymbVal, TSymbType>
    {
        /// <summary>
        /// Gets and sets the symbol value.
        /// </summary>
        TSymbVal SymbolValue { get; set; }

        /// <summary>
        /// Gets and sets the symbol type.
        /// </summary>
        TSymbType SymbolType { get; set; }
    }
}
