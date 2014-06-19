namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de valores de precisão dupla.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public class DoubleParser<SymbType> : IParse<double, string, SymbType>, IParse<object, string, SymbType>
    {
        /// <summary>
        /// O estilo dos números.
        /// </summary>
        private NumberStyles numberStyles;

        /// <summary>
        /// O provedor de formatos.
        /// </summary>
        private IFormatProvider formatProvider;

        /// <summary>
        /// Instancia um novo objeto do tipo <see cref="DoubleParser{SymbType}"/>.
        /// </summary>
        public DoubleParser()
            : this(NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat)
        {
        }

        /// <summary>
        /// Instancia um novo objeto do tipo <see cref="DoubleParser{SymbType}"/>.
        /// </summary>
        /// <param name="numberStyles">Os estilos dos números.</param>
        /// <param name="formatProvider">O provedor de formatos.</param>
        public DoubleParser(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                throw new UtilitiesDataException("Expecting a non null format provider.");
            }

            this.numberStyles = numberStyles;
            this.formatProvider = formatProvider;
        }

        /// <summary>
        /// Tenta efectuar a leitura.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
        /// <param name="value">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbol<string, SymbType>[] symbolListToParse, out double value)
        {
            if (symbolListToParse.Length > 1)
            {
                value = 0.0;
                return false;
            }
            else
            {
                var firstSymbol = symbolListToParse[0];
                return double.TryParse(firstSymbol.SymbolValue,
                    this.numberStyles,
                    this.formatProvider,
                    out value);
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
            var temp = default(double);
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
