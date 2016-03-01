namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;

    public class DoubleArrayVectorReader
    {
        /// <summary>
        /// Permite fazer a leitura de um vector a partir de texto.
        /// </summary>
        /// <param name="lines">O tamanho do vector.</param>
        /// <param name="vectorString">O texto que representa o vector.</param>
        /// <returns>O vector.</returns>
        public IMathVector<double> ReadVector(int length, string vectorString)
        {
            var integerParser = new DoubleExpressionParser();
            var vectorFactory = new ArrayVectorFactory<double>();
            var reader = new StringReader(vectorString);
            var stringSymbolReader = new StringSymbolReader(reader, false);
            var vectorReader = new ConfigVectorReader<double, string, string, CharSymbolReader<string>>(
                length,
                vectorFactory);
            vectorReader.MapInternalDelimiters("left_bracket", "right_bracket");
            vectorReader.AddBlanckSymbolType("blancks");
            vectorReader.SeparatorSymbType = "comma";

            var vector = default(IMathVector<double>);
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
