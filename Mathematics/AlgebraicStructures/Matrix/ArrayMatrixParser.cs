namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    class ArrayMatrixParser<T> :
        IParse<ArrayMatrix<T>, string, string>
    {
        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private ArrayMatrixReader<T, string, string, ISymbol<string,string>[]> arrayMatrixReader;

        /// <summary>
        /// O leitor dos elmentos contidos na matriz multidimensional.
        /// </summary>
        private IParse<T, string, string> elementsParser;

        /// <summary>
        /// Número de linhas da matriz.
        /// </summary>
        private int numberOfLines;

        /// <summary>
        /// Número de colunas da matriz.
        /// </summary>
        private int numberOfColumns;

        public ArrayMatrixParser(IParse<T, string, string> elementsParser, int numberOfLines, int numberOfColumns)
        {
            if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else if(numberOfLines < 0){
            }
            else if(numberOfColumns<0){
            }
            else
            {
                this.arrayMatrixReader = new ArrayMatrixReader<T, string, string, ISymbol<string, string>[]>(
                    this.numberOfLines,
                    this.numberOfColumns);
                this.elementsParser = elementsParser;
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out ArrayMatrix<T> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.arrayMatrixReader.TryParseMatrix(arrayReader, this.elementsParser, out value);
        }
    }
}
