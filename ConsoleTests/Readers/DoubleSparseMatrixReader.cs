namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;

    public class DoubleSparseMatrixReader
    {
        /// <summary>
        /// Faz a leitura de uma matriz de valores numéricos.
        /// </summary>
        /// <param name="lines">The number of lines.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="arrayString">O texto que representa a matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <returns>A matriz lida.</returns>
        public SparseDictionaryMathMatrix<double> ReadArray(
            int lines, 
            int columns, 
            string arrayString, 
            double defaultValue = 0)
        {
            var expressionParser = new DoubleExpressionParser();
            var reader = new StringReader(arrayString);
            var stringSymbolReader = new StringSymbolReader(reader, false);
            var arrayMatrixFactory = new SparseDictionaryMatrixFactory<double>();
            var arrayMatrixReader = new ConfigMatrixReader<double, IMathMatrix<double>, string, string>(
                lines, 
                columns);
            arrayMatrixReader.MapInternalDelimiters("left_bracket", "right_bracket");
            arrayMatrixReader.AddBlanckSymbolType("blancks");
            arrayMatrixReader.SeparatorSymbType = "comma";

            var matrix = default(IMathMatrix<double>);
            if (arrayMatrixReader.TryParseMatrix(
                stringSymbolReader, 
                expressionParser, 
                (i,j)=>arrayMatrixFactory.CreateMatrix(i,j,defaultValue), 
                out matrix))
            {
                return matrix as SparseDictionaryMathMatrix<double>;
            }
            else
            {
                throw new ArgumentException("Can't read the specified matrix.");
            }
        }
    }
}
