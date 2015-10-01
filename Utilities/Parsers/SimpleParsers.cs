namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Implementa um leitor de valores inteiros.
    /// </summary>
    /// <typeparam name="SymbType">O tipos dos objectos que constituem os tipos de símbolos.</typeparam>
    public class IntegerParser<SymbType> : IParse<int, string, SymbType>
    {
        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public int Parse(
            ISymbol<string, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            if (errorLogs == null)
            {
                throw new ArgumentNullException("errorLogs");
            }
            else
            {
                var value = default(int);
                if (symbolListToParse == null)
                {
                    errorLogs.AddLog(
                        "Null values for integer aren't allowed.",
                        EParseErrorLevel.ERROR);
                }
                else if (symbolListToParse.Length == 0)
                {
                    errorLogs.AddLog(
                        "No symbol was provided for reading.",
                        EParseErrorLevel.ERROR);
                }
                else
                {
                    var firstSymbol = symbolListToParse[0];
                    if (!int.TryParse(firstSymbol.SymbolValue, out value))
                    {
                        errorLogs.AddLog(
                            string.Format("Invalid integer symbol: {0}", firstSymbol.SymbolValue),
                            EParseErrorLevel.ERROR);
                    }
                }

                return value;
            }
        }
    }

    /// <summary>
    /// Implementa um leitor de valores de precisão dupla.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public class DoubleParser<SymbType> : IParse<double, string, SymbType>
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
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public double Parse(
            ISymbol<string, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            if (errorLogs == null)
            {
                throw new ArgumentNullException("errorLogs");
            }
            else
            {
                var value = default(double);
                if (symbolListToParse == null)
                {
                    errorLogs.AddLog(
                        "Null values for double aren't allowed.",
                        EParseErrorLevel.ERROR);
                }
                else if (symbolListToParse.Length == 0)
                {
                    errorLogs.AddLog(
                        "No symbol value was provided for reading.",
                        EParseErrorLevel.ERROR);
                }
                else
                {
                    var firstValue = symbolListToParse[0];
                    if (!double.TryParse(
                        firstValue.SymbolValue,
                        this.numberStyles,
                        this.formatProvider,
                        out value))
                    {
                        errorLogs.AddLog(
                            string.Format("Invalid double symbol value: {0}", firstValue.SymbolValue),
                            EParseErrorLevel.ERROR);
                    }
                }

                return value;
            }
        }
    }

    /// <summary>
    /// Permite efectuar a leitura de inteiros com precisão dupla.
    /// </summary>
    /// <typeparam name="SymbType">O tipo de objectos que constituem os tipos de símbolos.</typeparam>
    public class LongParser<SymbType> : IParse<long, string, SymbType>
    {
        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public long Parse(
            ISymbol<string, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            if (errorLogs == null)
            {
                throw new ArgumentNullException("errorLogs");
            }
            else
            {
                var value = default(long);
                if (symbolListToParse == null)
                {
                    errorLogs.AddLog(
                        "Null values for integer aren't allowed.",
                        EParseErrorLevel.ERROR);
                }
                else if (symbolListToParse.Length == 0)
                {
                    errorLogs.AddLog(
                        "No symbol was provided for reading.",
                        EParseErrorLevel.ERROR);
                }
                else
                {
                    var firstSymbol = symbolListToParse[0];
                    if (!long.TryParse(firstSymbol.SymbolValue, out value))
                    {
                        errorLogs.AddLog(
                            string.Format("Invalid integer symbol: {0}", firstSymbol.SymbolValue),
                            EParseErrorLevel.ERROR);
                    }
                }

                return value;
            }
        }
    }

    /// <summary>
    /// Implementa um leitor de valores lógicos.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public class BoolParser<SymbType> : IParse<bool, string, SymbType>
    {
        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public bool Parse(
            ISymbol<string, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            if (errorLogs == null)
            {
                throw new ArgumentNullException("errorLogs");
            }
            else
            {
                var value = default(bool);
                if (symbolListToParse == null)
                {
                    errorLogs.AddLog(
                        "Null values for integer aren't allowed.",
                        EParseErrorLevel.ERROR);
                }
                else if (symbolListToParse.Length == 0)
                {
                    errorLogs.AddLog(
                        "No symbol was provided for reading.",
                        EParseErrorLevel.ERROR);
                }
                else
                {
                    var firstSymbol = symbolListToParse[0];
                    if (!bool.TryParse(firstSymbol.SymbolValue, out value))
                    {
                        errorLogs.AddLog(
                            string.Format("Invalid integer symbol: {0}", firstSymbol.SymbolValue),
                            EParseErrorLevel.ERROR);
                    }
                }

                return value;
            }
        }
    }

    /// <summary>
    /// Implementa um leitor de valores inteiros de precisão arbitrária.
    /// </summary>
    /// <typeparam name="SymbType">O tipo de objectos que constituem os tipos de símbolos.</typeparam>
    public class BigIntegerParser<SymbType> : IParse<BigInteger, string, SymbType>
    {

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public BigInteger Parse(
            ISymbol<string, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            if (errorLogs == null)
            {
                throw new ArgumentNullException("errorLogs");
            }
            else
            {
                var value = default(BigInteger);
                if (symbolListToParse == null)
                {
                    errorLogs.AddLog(
                        "Null values for integer aren't allowed.",
                        EParseErrorLevel.ERROR);
                }
                else if (symbolListToParse.Length == 0)
                {
                    errorLogs.AddLog(
                        "No symbol was provided for reading.",
                        EParseErrorLevel.ERROR);
                }
                else
                {
                    var firstSymbol = symbolListToParse[0];
                    if (!BigInteger.TryParse(firstSymbol.SymbolValue, out value))
                    {
                        errorLogs.AddLog(
                            string.Format("Invalid integer symbol: {0}", firstSymbol.SymbolValue),
                            EParseErrorLevel.ERROR);
                    }
                }

                return value;
            }
        }
    }

    /// <summary>
    /// Implementa um leitor de expressões que definem decimais.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public class DecimalParser<SymbType> : IParse<decimal, string, SymbType>
    {
        /// <summary>
        /// O estilo do número.
        /// </summary>
        private NumberStyles numberStyles;

        /// <summary>
        /// O provedor de formatos.
        /// </summary>
        private IFormatProvider formatProvider;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DecimalParser{SymbType}"/>.
        /// </summary>
        public DecimalParser()
            : this(NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DecimalParser{SymbType}"/>.
        /// </summary>
        /// <param name="numberStyles">Os estilos do número.</param>
        /// <param name="formatProvider">O provedor de formatos.</param>
        public DecimalParser(NumberStyles numberStyles, IFormatProvider formatProvider)
        {
            if (formatProvider == null)
            {
                throw new UtilitiesDataException("Expecting a non null format provider.");
            }

            this.numberStyles = numberStyles;
            this.formatProvider = formatProvider;
        }

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public decimal Parse(
            ISymbol<string, SymbType>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            if (errorLogs == null)
            {
                throw new ArgumentNullException("errorLogs");
            }
            else
            {
                var value = default(decimal);
                if (symbolListToParse == null)
                {
                    errorLogs.AddLog(
                        "Null values for integer aren't allowed.",
                        EParseErrorLevel.ERROR);
                }
                else if (symbolListToParse.Length == 0)
                {
                    errorLogs.AddLog(
                        "No symbol was provided for reading.",
                        EParseErrorLevel.ERROR);
                }
                else
                {
                    var firstSymbol = symbolListToParse[0];
                    if (!decimal.TryParse(
                        firstSymbol.SymbolValue,
                        this.numberStyles,
                        this.formatProvider,
                        out value))
                    {
                        errorLogs.AddLog(
                            string.Format("Invalid integer symbol: {0}", firstSymbol.SymbolValue),
                            EParseErrorLevel.ERROR);
                    }
                }

                return value;
            }
        }
    }
}
