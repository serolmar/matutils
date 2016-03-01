﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de matrizes cuja configuração dimensional é previamente
    /// estabelecida.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem os coeficientes.</typeparam>
    /// <typeparam name="M">O tipo de objectos que constituem as matrizes.</typeparam>
    public class ConfigMatrixParser<T, M> :
        IParse<M, string, string>
        where M : IMatrix<T>
    {
        /// <summary>
        /// O leitor de matrizes.
        /// </summary>
        /// <remarks>
        /// O leitor actual efectua leituras de matrizes cujas dimensões são conhecidas.
        /// </remarks>
        private ConfigMatrixReader<T,M, string, string> arrayMatrixReader;

        /// <summary>
        /// O leitor dos elmentos contidos na matriz multidimensional.
        /// </summary>
        private IParse<T, string, string> elementsParser;

        /// <summary>
        /// O objecto responsável pela criação da matriz.
        /// </summary>
        Func<int, int, M> matrixFactory;

        /// <summary>
        /// Obtém e atribui o tipo de símbolo que separa os itens da matriz multidimensional.
        /// </summary>
        /// <value>O tipo de símbolo que separa os itens da matriz multidimensional.</value>
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

        /// <summary>
        /// Instancia um novo objecto que permite efectuar a leitura de matrizes.
        /// </summary>
        /// <param name="elementsParser">O leitor responsável pela leitura de cada obecto.</param>
        /// <param name="numberOfLines">O úmero de linhas da matriz.</param>
        /// <param name="numberOfColumns">O número de colunas da matriz.</param>
        /// <param name="matrixFactory">A fábrica responsável pela criação de matrizes.</param>
        /// <exception cref="ArgumentNullException">Se o leitor de objectos ou a fábrica de matrizes forem nulos.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o número de linhas ou o número de colunas for negativo.
        /// </exception>
        public ConfigMatrixParser(
            IParse<T, string, string> elementsParser, 
            int numberOfLines, 
            int numberOfColumns,
            Func<int, int, M> matrixFactory)
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
                this.arrayMatrixReader = new ConfigMatrixReader<T, M, string, string>(
                    numberOfLines,
                    numberOfColumns);
                this.matrixFactory = matrixFactory;
                this.elementsParser = elementsParser;
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
        public M Parse(
            ISymbol<string, string>[] symbolListToParse, 
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var arrayReader = new ArraySymbolReader<string, string>(
                symbolListToParse, 
                Utils.GetStringSymbolType(EStringSymbolReaderType.EOF));
            var value = default(M);
            this.arrayMatrixReader.TryParseMatrix(
                arrayReader,
                this.elementsParser,
                errorLogs, 
                this.matrixFactory,
                out value);
            return value;
        }

        /// <summary>
        /// Mapeia delimitadores de abertura com delimitadores de fecho que irão definir a matriz.
        /// </summary>
        /// <remarks>É possível definir mais do que um delimitador de fecho para o mesmo delimitador de abertura.</remarks>
        /// <param name="openSymbolType">O tipo de símbolo de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo de fecho.</param>
        public void MapInternalDelimiters(string openSymbolType, string closeSymbType)
        {
            this.arrayMatrixReader.MapInternalDelimiters(openSymbolType, closeSymbType);
        }

        /// <summary>
        /// Mapeia delimitadores externos de abertura com delimitadores externos de fecho.
        /// </summary>
        /// <remarks>
        /// Qualquer sequência que se encontre entre delimitadores externos é passada para os leitores
        /// de objectos mesmo que contenham separadores ou delimitadores internos.
        /// </remarks>
        /// <param name="openSymbType">O tipo de símbolo que representa o delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa o delimitador de fecho.</param>
        public void MapExternalDelimiters(string openSymbType, string closeSymbType)
        {
            this.arrayMatrixReader.MapExternalDelimiters(openSymbType, closeSymbType);
        }

        /// <summary>
        /// Adiciona um tipo de símbolo que será ignorado pelo leitor.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void AddBlanckSymbolType(string symbolType)
        {
            this.arrayMatrixReader.AddBlanckSymbolType(symbolType);
        }

        /// <summary>
        /// Remove um tipo de símbolo de modo a que deixe de ser ignorado pelo leitor.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void RemoveBlanckSymbolType(string symbolType)
        {
            this.arrayMatrixReader.RemoveBlanckSymbolType(symbolType);
        }

        /// <summary>
        /// Elimina todos os tipos de símbolos que serão ignorados pelo leitor.
        /// </summary>
        public void ClearBlanckSymbols()
        {
            this.arrayMatrixReader.ClearBlanckSymbols();
        }
    }
}
