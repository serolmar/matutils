namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de alcances multidimensionais.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    public class MultiDimensionalRangeReader<T, SymbValue, SymbType>
    {
        /// <summary>
        /// O leitor de alcances multidimensionais.
        /// </summary>
        private ARangeReader<T, SymbValue, SymbType> rangeReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="MultiDimensionalRangeReader{T, SymbValue, SymbType}"/>.
        /// </summary>
        /// <param name="rangeReader">O leitor de alcances multidimensionais.</param>
        /// <exception cref="ArgumentNullException">Se o leitor de alcances multidimensionais for nulo.</exception>
        public MultiDimensionalRangeReader(ARangeReader<T, SymbValue, SymbType> rangeReader)
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

        /// <summary>
        /// Tenta ler o alcance multidimensional a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador do alcance multidimensional.</param>
        /// <param name="result">Estabelece o alcance multidimensional lido.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseRange(
            IMementoSymbolReader< SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            out MultiDimensionalRange<T> result)
        {
            return this.TryParseRange(reader, parser, null, out result);
        }

        /// <summary>
        /// Tenta ler o alcance multidimensional a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador do alcance multidimensional.</param>
        /// <param name="errors">As mensagens de erro.</param>
        /// <param name="result">Estabelece o alcance multidimensional lido.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseRange(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            ILogStatus<string,EParseErrorLevel> errors,
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
                        errors.AddLog(message, EParseErrorLevel.ERROR);
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
