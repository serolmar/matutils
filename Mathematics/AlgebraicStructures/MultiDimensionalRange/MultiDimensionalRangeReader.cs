using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    public class MultiDimensionalRangeReader<T, SymbValue, SymbType, InputReader>
    {
        private ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader;

        public MultiDimensionalRangeReader(ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader)
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

        public bool TryParseRange(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            out MultiDimensionalRange<T> result)
        {
            return this.TryParseRange(reader, parser, null, out result);
        }

        public bool TryParseRange(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            List<string> errors,
            out MultiDimensionalRange<T> result)
        {
            result = default(MultiDimensionalRange<T>);
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
                result = new MultiDimensionalRange<T>(this.rangeReader.Configuration);
                result.InternalElements = this.rangeReader.Elements.ToArray();
                return true;
            }
        }
    }
}
