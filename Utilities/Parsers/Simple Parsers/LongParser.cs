namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite efectuar a leitura de inteiros com precisão dupla.
    /// </summary>
    public class LongParser<SymbType> : IParse<long, string, SymbType>, IParse<object, string, SymbType>
    {
        /// <summary>
        /// Implementa a leitura de inteiros com precisão dupla.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ser interpretada.</param>
        /// <param name="value">O valor interpretado.</param>
        /// <returns>Verdadeiro caso a leitura seja realizada com sucesso e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out long value)
        {
            if (symbolListToParse.Length > 1)
            {
                value = 0;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return long.TryParse(firstSymbol.SymbolValue, out value);
            }
        }

        /// <summary>
        /// Implementa a leitura de inteiros com precisão dupla.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ser interpretada.</param>
        /// <param name="value">O valor interpretado.</param>
        /// <returns>Verdadeiro caso a leitura seja realizada com sucesso e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out object value)
        {
            var temp = default(long);
            if (this.TryParse(symbolListToParse, out temp))
            {
                value = temp;
                return true;
            }
            else
            {
                value = default(object);
                return false;
            }
        }
    }
}
