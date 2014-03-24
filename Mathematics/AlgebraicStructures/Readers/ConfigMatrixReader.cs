namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class ConfigMatrixReader<T, SymbValue, SymbType, InputReader>
    {
        private ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader;

        private IMatrixFactory<T> matrixFactory;

        public ConfigMatrixReader(int lines, int columns, IMatrixFactory<T> matrixFactory)
        {
            if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                this.matrixFactory = matrixFactory;
                this.rangeReader = new RangeConfigReader<T, SymbValue, SymbType, InputReader>(
                    new int[] { lines, columns });
            }
        }

        public SymbType SeparatorSymbType
        {
            get
            {
                return this.rangeReader.SeparatorSymbType;
            }
            set
            {
                this.rangeReader.SeparatorSymbType = value;
            }
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            out IMatrix<T> matrix)
        {
            return this.TryParseMatrix(reader, parser, null, out matrix);
        }

        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            T defaultValue,
            out IMatrix<T> matrix)
        {
            return this.TryParseMatrix(reader, parser, null, defaultValue, out matrix);
        }

        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            List<string> errors,
            T defaultValue,
            out IMatrix<T> matrix)
        {
            matrix = default(ArrayMatrix<T>);
            this.rangeReader.ReadRangeValues(reader, parser);
            if (this.rangeReader.HasErrors)
            {
                if (errors != null)
                {
                    foreach (var message in this.rangeReader.ErrorMessages)
                    {
                        errors.Add(message);
                    }
                }

                return false;
            }
            else
            {
                var lines = -1;
                var columns = -1;
                var configurationEnumerator = this.rangeReader.Configuration.GetEnumerator();
                if (configurationEnumerator.MoveNext())
                {
                    lines = configurationEnumerator.Current;
                    if (configurationEnumerator.MoveNext())
                    {
                        columns = configurationEnumerator.Current;
                    }
                }

                matrix = this.matrixFactory.CreateMatrix(lines, columns, defaultValue);
                this.SetupResultMatrix(matrix, new int[] { lines, columns }, this.rangeReader.Elements);
                return true;
            }
        }

        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            List<string> errors,
            out IMatrix<T> matrix)
        {
            matrix = default(ArrayMatrix<T>);
            this.rangeReader.ReadRangeValues(reader, parser);
            if (this.rangeReader.HasErrors)
            {
                if (errors != null)
                {
                    foreach (var message in this.rangeReader.ErrorMessages)
                    {
                        errors.Add(message);
                    }
                }

                return false;
            }
            else
            {
                var lines = -1;
                var columns = -1;
                var configurationEnumerator = this.rangeReader.Configuration.GetEnumerator();
                if (configurationEnumerator.MoveNext())
                {
                    lines = configurationEnumerator.Current;
                    if (configurationEnumerator.MoveNext())
                    {
                        columns = configurationEnumerator.Current;
                    }
                }

                matrix = this.matrixFactory.CreateMatrix(lines, columns);
                this.SetupResultMatrix(matrix, new int[] { lines, columns }, this.rangeReader.Elements);
                return true;
            }
        }

        public void MapInternalDelimiters(SymbType openSymbolType, SymbType closeSymbType)
        {
            this.rangeReader.MapInternalDelimiters(openSymbolType, closeSymbType);
        }

        public void MapExternalDelimiters(SymbType openSymbType, SymbType closeSymbType)
        {
            this.rangeReader.MapExternalDelimiters(openSymbType, closeSymbType);
        }

        public void AddBlanckSymbolType(SymbType symbolType)
        {
            this.rangeReader.AddBlanckSymbolType(symbolType);
        }

        public void RemoveBlanckSymbolType(SymbType symbolType)
        {
            this.rangeReader.RemoveBlanckSymbolType(symbolType);
        }

        public void ClearBlanckSymbols()
        {
            this.rangeReader.ClearBlanckSymbols();
        }

        /// <summary>
        /// Atribui os dados lidos à matriz.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <param name="configuration">A configuração dos elementos.</param>
        /// <param name="elements">Os elementos lidos.</param>
        private void SetupResultMatrix(IMatrix<T> matrix, int[] configuration, ReadOnlyCollection<T> elements)
        {
            var currentLine = -1;
            var currentColumn = 0;
            for (int i = 0; i < elements.Count; ++i)
            {
                ++currentLine;
                if (currentLine >= configuration[0])
                {
                    currentLine = 0;
                    ++currentColumn;
                }

                matrix[currentLine, currentColumn] = elements[i];
            }
        }
    }
}
