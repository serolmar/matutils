namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite realizar a leitura de um vector.
    /// </summary>
    /// <typeparam name="T">O tipo de elementos do vector.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">O tipo dos identificadores dos símbolos.</typeparam>
    /// <typeparam name="InputReader">O leitor para os dados de entrada.</typeparam>
    public class ConfigVectorReader<T, SymbValue, SymbType, InputReader>
    {
        private ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader;

        private IVectorFactory<T> vectorFactory;

        public ConfigVectorReader(int lines, IVectorFactory<T> vectorFactory)
        {
            if (vectorFactory == null)
            {
                throw new ArgumentNullException("vectorFactory");
            }
            else
            {
                this.vectorFactory = vectorFactory;
                this.rangeReader = new RangeConfigReader<T, SymbValue, SymbType, InputReader>(
                    new int[] { lines});
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

        public bool TryParseVector(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            out IVector<T> vector)
        {
            return this.TryParseVector(reader, parser, null, out vector);
        }

        public bool TryParseVector(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            List<string> errors,
            out IVector<T> vector)
        {
            vector = default(IVector<T>);
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
                var configurationEnumerator = this.rangeReader.Configuration.GetEnumerator();
                if (configurationEnumerator.MoveNext())
                {
                    lines = configurationEnumerator.Current;
                    if (configurationEnumerator.MoveNext())
                    {
                        lines = configurationEnumerator.Current;
                    }
                }

                vector = this.vectorFactory.CreateVector(lines);
                this.SetupResultVector(vector, lines, this.rangeReader.Elements);
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

        private void SetupResultVector(IVector<T> vector, int lines, ReadOnlyCollection<T> elements)
        {
            var currentLine = -1;
            for (int i = 0; i < lines; ++i)
            {
                ++currentLine;
                vector[currentLine] = elements[i];
            }
        }
    }
}
