﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class ConfigMatrixParser<T> :
        IParse<IMatrix<T>, string, string>
    {
        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private ConfigMatrixReader<T, string, string, ISymbol<string, string>[]> arrayMatrixReader;

        /// <summary>
        /// O leitor dos elmentos contidos na matriz multidimensional.
        /// </summary>
        private IParse<T, string, string> elementsParser;

        public string SeparatorSymbType
        {
            get
            {
                return this.arrayMatrixReader.SeparatorSymbType;
            }
            set
            {
                this.arrayMatrixReader.SeparatorSymbType = value;
            }
        }

        public ConfigMatrixParser(
            IParse<T, string, string> elementsParser, 
            int numberOfLines, int numberOfColumns, 
            IMatrixFactory<T> matrixFactory)
        {
            if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else if (numberOfLines < 0)
            {
                throw new ArgumentOutOfRangeException("numberOfLines");
            }
            else if (numberOfColumns < 0)
            {
                throw new ArgumentOutOfRangeException("numberOfColumns");
            }
            else
            {
                this.arrayMatrixReader = new ConfigMatrixReader<T, string, string, ISymbol<string, string>[]>(
                    numberOfLines,
                    numberOfColumns,
                    matrixFactory);
                this.elementsParser = elementsParser;
            }
        }

        public bool TryParse(ISymbol<string, string>[] symbolListToParse, out IMatrix<T> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.arrayMatrixReader.TryParseMatrix(arrayReader, this.elementsParser, out value);
        }

        public void MapInternalDelimiters(string openSymbolType, string closeSymbType)
        {
            this.arrayMatrixReader.MapInternalDelimiters(openSymbolType, closeSymbType);
        }

        public void MapExternalDelimiters(string openSymbType, string closeSymbType)
        {
            this.arrayMatrixReader.MapExternalDelimiters(openSymbType, closeSymbType);
        }

        public void AddBlanckSymbolType(string symbolType)
        {
            this.arrayMatrixReader.AddBlanckSymbolType(symbolType);
        }

        public void RemoveBlanckSymbolType(string symbolType)
        {
            this.arrayMatrixReader.RemoveBlanckSymbolType(symbolType);
        }

        public void ClearBlanckSymbols()
        {
            this.arrayMatrixReader.ClearBlanckSymbols();
        }
    }
}
