namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de polinómios univariáveis.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de obejctos que constituem os coeficientes dos polinómios.</typeparam>
    public class UnivarPolNormalFormParser<CoeffType> 
        : IParse<UnivariatePolynomialNormalForm<CoeffType>, string, string>
    {
        /// <summary>
        /// O leitor de matrizes multidimensionais.
        /// </summary>
        private UnivariatePolynomialReader<CoeffType, ISymbol<string, string>[]> polynomialReader;

        /// <summary>
        /// O conversor entre os coeficientes e os inteiros.
        /// </summary>
        private IConversion<int, CoeffType> conversion;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnivarPolNormalFormParser{CoeffType} "/>.
        /// </summary>
        /// <param name="variable">O nome da variável associada ao polinómio.</param>
        /// <param name="conversion">O objecto responsável pela conversão entre coeficientes e inteiros.</param>
        /// <param name="elementsParser">O leitor de coeficientes.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        public UnivarPolNormalFormParser(
            string variable, 
            IConversion<int, CoeffType> conversion,
            IParse<CoeffType, string, string> elementsParser, 
            IRing<CoeffType> ring)
        {
            if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentException("Variable must hava a non empty value.");
            }
            else if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.conversion = conversion;
                this.polynomialReader = new UnivariatePolynomialReader<CoeffType, ISymbol<string, string>[]>(
                    variable,
                    elementsParser,
                    ring);
            }
        }

        /// <summary>
        /// Experimenta a leitura de um polinómio univariável a partir de uma lista de símbolos.
        /// </summary>
        /// <param name="symbolListToParse">A lista de símbolos.</param>
        /// <param name="value">O valor que receberá o polinómio lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(
            ISymbol<string, string>[] symbolListToParse, 
            out UnivariatePolynomialNormalForm<CoeffType> value)
        {
            var arrayReader = new ArraySymbolReader<string, string>(symbolListToParse, "eof");
            return this.polynomialReader.TryParsePolynomial(arrayReader, this.conversion, out value);
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
        public UnivariatePolynomialNormalForm<CoeffType> Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var arrayReader = new ArraySymbolReader<string, string>(
                symbolListToParse, 
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            var value = default(UnivariatePolynomialNormalForm<CoeffType>);
            this.polynomialReader.TryParsePolynomial(
                arrayReader, 
                this.conversion, 
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
        public void RegisterExpressionDelimitersTyes(string openDelimiterType, string closeDelimiterType)
        {
            this.polynomialReader.RegisterExpressionDelimiterTypes(openDelimiterType, closeDelimiterType);
        }

        /// <summary>
        /// Elimmina todos os mapeamentos de expressão previamente registados.
        /// </summary>
        public void ClearExpressionDelimitersTypes()
        {
            this.polynomialReader.ClearExpressionDelimitersMappings();
        }

        /// <summary>
        /// Permite associar tipos de delimitadores externos.
        /// </summary>
        /// <param name="openDelimiterType">O tiop de delimitador externo de abertura.</param>
        /// <param name="closeDelimiterType">O tipo de delimitador externo de fecho.</param>
        public void RegisterExternalDelimitersTypes(string openDelimiterType, string closeDelimiterType)
        {
            this.polynomialReader.RegisterExternalDelimiterTypes(openDelimiterType, closeDelimiterType);
        }

        /// <summary>
        /// Elimina todos os mapeamentos externos previamente registados.
        /// </summary>
        public void ClearExternalDelmitersTypes()
        {
            this.polynomialReader.ClearExternalDelimitersMappings();
        }
    }
}
