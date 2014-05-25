namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;
    using Utilities.Collections;
    using System.Globalization;

    /// <summary>
    /// Implementa um leitor de objectos matemáticos.
    /// </summary>
    class MathematicsObjectParser : IParse<AMathematicsObject, string, string>
    {
        /// <summary>
        /// Os símbolos vazios.
        /// </summary>
        private string[] voidSymbols = {"blancks"};

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
        /// Tenta fazer a leitura.
        /// </summary>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="mathematicsObject">O valor que contém a leitura.</param>
        /// <returns>Verdadeiro se a leitura for bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(
            ISymbol<string, string>[] symbolListToParse, 
            out AMathematicsObject mathematicsObject)
        {
            var valueReader = new StringReader(symbolListToParse[0].SymbolValue);
            var symbolReader = new StringSymbolReader(valueReader, true);
            this.IgnoreVoids(symbolReader);

            var readed = symbolReader.Peek();
            if (readed.SymbolType == "eof")
            {
                mathematicsObject = new EmptyMathematicsObject();
                return true;
            }
            else if (readed.SymbolType == "left_parenthesis")
            {
            }
            else if (readed.SymbolType == "left_bracket")
            {
            }
            else if (readed.SymbolType == "integer")
            {
                mathematicsObject = new IntegerMathematicsObject() { Value = int.Parse(symbolListToParse[0].SymbolValue) };
                return true;
            }
            else if (readed.SymbolType == "double")
            {
                mathematicsObject = new DoubleMathematicsObject() { Value = double.Parse(symbolListToParse[0].SymbolValue, CultureInfo.InvariantCulture.NumberFormat) };
                return true;
            }
            else if (readed.SymbolType == "string")
            {
                mathematicsObject = new NameMathematicsObject(symbolListToParse[0].SymbolValue, this.mediator);
                return true;
            }
            else if (readed.SymbolType == "double_quote")
            {
                mathematicsObject = new StringMathematicsObject() { Value = symbolListToParse[0].SymbolValue };
                return true;
            }
            else if (readed.SymbolType == "boolean")
            {
                mathematicsObject = new BooleanMathematicsObject() { Value = bool.Parse(symbolListToParse[0].SymbolValue) };
                return true;
            }
            else
            {
                mathematicsObject = null;
                return false;
            }

            var lispStyleListParser = LispStyleList<AMathematicsObject>.GetParser(this);

            var rangeReader = new RangeNoConfigReader<AMathematicsObject, string, string, CharSymbolReader<string>>();
            var multidimensionalRangeParser = new MultiDimensionalRangeReader<AMathematicsObject, string, string, CharSymbolReader<string>>(rangeReader);
            var multidimensionalRange = default(MultiDimensionalRange<AMathematicsObject>);
            var parsed = multidimensionalRangeParser.TryParseRange(symbolReader, this, out multidimensionalRange);
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
