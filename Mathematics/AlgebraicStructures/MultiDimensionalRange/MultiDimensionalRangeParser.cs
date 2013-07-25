using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    public class MultiDimensionalRangeParser<T, SymbValue, SymbType, InputReader>
    {
        private ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader;

        public MultiDimensionalRangeParser(ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader)
        {
            if (rangeReader == null)
            {
                throw new ArgumentNullException("rangeReader");
            }
            else
            {
                this.rangeReader = rangeReader;
            }
        }

        public MultiDimensionalRange<T> ParseRange(
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
                var result = new MultiDimensionalRange<T>(this.rangeReader.Configuration);
                result.InternalElements = this.rangeReader.Elements.ToArray();
                return result;
            }
        }
    }
}
