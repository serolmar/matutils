namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de valores lógicos.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public class BoolParser<SymbType> : IParse<bool, string, SymbType>, IParse<object, string, SymbType>
    {
        /// <summary>
        /// Tenta efectuar a leitura.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out bool value)
        {
            value = false;
            if (symbolListToParse.Length > 1)
            {
                value = false;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return bool.TryParse(firstSymbol.SymbolValue, out value);
            }
        }

        /// <summary>
        /// Tenta efectuar a leitura.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out object value)
        {
            var temp = default(bool);
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
