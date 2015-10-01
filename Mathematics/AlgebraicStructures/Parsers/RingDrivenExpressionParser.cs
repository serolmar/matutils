namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite construir um leitor de valores com base num anel.
    /// </summary>
    /// <example>
    /// Leitura de expressões como 2^3+5*(1+4)^5 com base num domínio sobre inteiros, o qual
    /// define todas as operações de anel.
    /// </example>
    /// <typeparam name="ObjectType">O tipo de valor a ser lido.</typeparam>
    public class RingDrivenExpressionParser<ObjectType>
        : IParse<ObjectType, string, string>
    {
        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        protected RingDrivenExpressionReader<ObjectType, ISymbol<string, string>[]> expressionReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="RingDrivenExpressionParser{ObjectType}"/>.
        /// </summary>
        protected RingDrivenExpressionParser()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="RingDrivenExpressionParser{ObjectType}"/>.
        /// </summary>
        /// <param name="elementsParser">O leitor de valores.</param>
        /// <param name="ring">O anel responsável pela leitura dos valores.</param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre representantes de números inteiros.</param>
        public RingDrivenExpressionParser(
            IParse<ObjectType, string, string> elementsParser,
            IRing<ObjectType> ring,
            IIntegerNumber<ObjectType> integerNumber = null)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.expressionReader = new RingDrivenExpressionReader<ObjectType, ISymbol<string, string>[]>(
                    elementsParser,
                    ring,
                    integerNumber);
            }
        }

        /// <summary>
        /// Realiza a leitura.
        /// </summary>
        /// <remarks>
        /// Se a leitura não for bem-sucedida, os erros de leitura serão registados no diário
        /// e será retornado o objecto por defeito.
        /// </remarks>
        /// <param name="symbolListToParse">O vector de símbolos a ser lido.</param>
        /// <param name="errorLogs">O objecto que irá manter o registo do diário da leitura.</param>
        /// <returns>O valor lido.</returns>
        public virtual ObjectType Parse(
            ISymbol<string, string>[] symbolListToParse, 
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var arrayReader = new ArraySymbolReader<string, string>(
                symbolListToParse, 
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            var value = default(ObjectType);
            this.expressionReader.TryParsePolynomial(
                arrayReader, 
                errorLogs, 
                out value);
            return value;
        }

        /// <summary>
        /// Permite registar delimitadores de precedência na expressão.
        /// </summary>
        /// <remarks>
        /// Caso nenhum delimitador de expressão se encontre registado, serão utilizados os parêntesis
        /// de abertura e fecho por defeito.
        /// </remarks>
        /// <param name="openDelimiterType">O tipo do delimitador de abertura.</param>
        /// <param name="closeDelimiterType">O tiop do delimitador de fecho.</param>
        public virtual void RegisterExpressionDelimitersTyes(string openDelimiterType, string closeDelimiterType)
        {
            this.expressionReader.RegisterExpressionDelimiterTypes(openDelimiterType, closeDelimiterType);
        }

        /// <summary>
        /// Elimmina todos os mapeamentos de expressão previamente registados.
        /// </summary>
        public virtual void ClearExpressionDelimitersTypes()
        {
            this.expressionReader.ClearExpressionDelimitersMappings();
        }

        /// <summary>
        /// Permite associar tipos de delimitadores externos.
        /// </summary>
        /// <param name="openDelimiterType">O tiop de delimitador externo de abertura.</param>
        /// <param name="closeDelimiterType">O tipo de delimitador externo de fecho.</param>
        public virtual void RegisterExternalDelimitersTypes(string openDelimiterType, string closeDelimiterType)
        {
            this.expressionReader.RegisterExternalDelimiterTypes(openDelimiterType, closeDelimiterType);
        }

        /// <summary>
        /// Elimina todos os mapeamentos externos previamente registados.
        /// </summary>
        public virtual void ClearExternalDelmitersTypes()
        {
            this.expressionReader.ClearExternalDelimitersMappings();
        }
    }
}
