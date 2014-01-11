namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;
    using Utilities.Parsers;
    using System.IO;

    public class IntegerArrayVectorReader
    {
        /// <summary>
        /// Permite fazer a leitura de um vector a partir de texto.
        /// </summary>
        /// <param name="lines">O tamanho do vector.</param>
        /// <param name="vectorString">O texto que representa o vector.</param>
        /// <returns>O vector.</returns>
        public IVector<int> ReadVector(int length, string vectorString)
        {
            var integerParser = new IntegerExpressionParser();
            var vectorFactory = new ArrayVectorFactory<int>();
            var reader = new StringReader(vectorString);
            var stringSymbolReader = new StringSymbolReader(reader, false);
            var vectorReader = new ConfigVectorReader<int, string, string, CharSymbolReader<string>>(
                length,
                vectorFactory);
            vectorReader.MapInternalDelimiters("left_bracket", "right_bracket");
            vectorReader.AddBlanckSymbolType("blancks");
            vectorReader.SeparatorSymbType = "comma";

            var vector = default(IVector<int>);
            if (vectorReader.TryParseVector(stringSymbolReader, integerParser, out vector))
            {
                return vector;
            }
            else
            {
                throw new Exception("An error has occured while reading integer vector.");
            }
        }
    }
}
