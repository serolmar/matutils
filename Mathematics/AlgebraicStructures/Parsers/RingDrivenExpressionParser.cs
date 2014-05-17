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
        /// Tenta fazer a leitura da expressão.
        /// </summary>
        /// <param name="symbolListToParse">O vector de símbolos que representa a expressão.</param>
        /// <param name="value">O valor lido.</param>
        /// <returns>Verdadeiro caso a leitura seja realizada com sucesso e falso caso contrário.</returns>
        public virtual bool TryParse(
            ISymbol<string, string>[] symbolListToParse, 
            out ObjectType value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.expressionReader.TryParsePolynomial(arrayReader, out value);
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
