using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    public class ArrayMatrixReader<T, SymbValue, SymbType, InputReader>
    {
        private ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader;

        public ArrayMatrixReader(int lines, int columns)
        {
            this.rangeReader = new RangeConfigReader<T, SymbValue, SymbType, InputReader>(
                new int[] { lines, columns });
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

        public ArrayMatrix<T> ParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser)
        {
            this.rangeReader.ReadRangeValues(reader, parser);
            if (this.rangeReader.HasErrors)
            {
                var messageBuilder = new StringBuilder();
                messageBuilder.AppendLine("Found some errors while reading the range:");
                foreach (var message in this.rangeReader.ErrorMessages)
                {
                    messageBuilder.AppendLine(message);
                }

                throw new MathematicsException(messageBuilder.ToString());
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

                var result = new ArrayMatrix<T>(lines, columns);
                this.SetupResultMatrix(result, new int[] { lines, columns }, this.rangeReader.Elements);
                return result;
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

        private void SetupResultMatrix(ArrayMatrix<T> matrix, int[] configuration, ReadOnlyCollection<T> elements)
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
