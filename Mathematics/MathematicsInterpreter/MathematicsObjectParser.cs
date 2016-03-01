namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de objectos matemáticos.
    /// </summary>
    class MathematicsObjectParser : IParse<AMathematicsObject, string, string>
    {
        /// <summary>
        /// Os símbolos vazios.
        /// </summary>
        private string[] voidSymbols = { "blancks" };

        /// <summary>
        /// O mediador.
        /// </summary>
        private MathematicsInterpreterMediator mediator;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="MathematicsObjectParser"/>.
        /// </summary>
        /// <param name="mediator">O mediador.</param>
        public MathematicsObjectParser(MathematicsInterpreterMediator mediator)
        {
            this.mediator = mediator;
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
        public AMathematicsObject Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var valueReader = new StringReader(symbolListToParse[0].SymbolValue);
            var symbolReader = new StringSymbolReader(valueReader, true);
            this.IgnoreVoids(symbolReader);

            var readed = symbolReader.Peek();
            if (readed.SymbolType == Utils.GetStringSymbolType(EStringSymbolReaderType.EOF))
            {
                return new EmptyMathematicsObject();
            }
            else if (readed.SymbolType == Utils.GetStringSymbolType(EStringSymbolReaderType.LEFT_PARENTHESIS))
            {
            }
            else if (readed.SymbolType == Utils.GetStringSymbolType(EStringSymbolReaderType.LEFT_BRACKET))
            {
            }
            else if (readed.SymbolType == Utils.GetStringSymbolType(EStringSymbolReaderType.INTEGER))
            {
                return new IntegerMathematicsObject() { Value = int.Parse(symbolListToParse[0].SymbolValue) };
            }
            else if (readed.SymbolType == Utils.GetStringSymbolType(EStringSymbolReaderType.DOUBLE))
            {
                return new DoubleMathematicsObject()
                {
                    Value = double.Parse(
                        symbolListToParse[0].SymbolValue,
                        CultureInfo.InvariantCulture.NumberFormat)
                };
            }
            else if (readed.SymbolType == Utils.GetStringSymbolType(EStringSymbolReaderType.STRING))
            {
                return new NameMathematicsObject(symbolListToParse[0].SymbolValue, this.mediator);
            }
            else if (readed.SymbolType == Utils.GetStringSymbolType(EStringSymbolReaderType.DOUBLE_QUOTE))
            {
                return new StringMathematicsObject() { Value = symbolListToParse[0].SymbolValue };
            }
            else if (readed.SymbolType == "boolean")
            {
                return new BooleanMathematicsObject() { Value = bool.Parse(symbolListToParse[0].SymbolValue) };
            }
            else
            {
                return null;
            }

            var lispStyleListParser = LispStyleList<AMathematicsObject>.GetParser(this);

            var rangeReader = new RangeNoConfigReader<AMathematicsObject, string, string>();
            var multidimensionalRangeParser = new MultiDimensionalRangeReader<AMathematicsObject, string, string>(rangeReader);
            var multidimensionalRange = default(MultiDimensionalRange<AMathematicsObject>);
            var parsed = multidimensionalRangeParser.TryParseRange(
                symbolReader, 
                this, 
                out multidimensionalRange);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ignora os símbolos vazios.
        /// </summary>
        /// <param name="symbolReader">O leitor de símbolos.</param>
        private void IgnoreVoids(SymbolReader<CharSymbolReader<string>, string, string> symbolReader)
        {
            ISymbol<string, string> symbol = symbolReader.Peek();
            while (this.voidSymbols.Contains(symbol.SymbolType))
            {
                symbolReader.Get();
                symbol = symbolReader.Peek();
            }
        }
    }
}
