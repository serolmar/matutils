namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;

    #region Leitor configurado

    /// <summary>
    /// Implementa um leitor de matrizes.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="M">O tipo da matriz.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    public class ConfigMatrixReader<T, M, SymbValue, SymbType>
        where M : IMatrix<T>
    {
        /// <summary>
        /// O leitor de alcances multidimensionais.
        /// </summary>
        private ARangeReader<T, SymbValue, SymbType> rangeReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ConfigMatrixReader{T, M, SymbValue, SymbType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        public ConfigMatrixReader(int lines, int columns)
        {
            this.rangeReader = new RangeConfigReader<T, SymbValue, SymbType>(
                    new int[] { lines, columns });
        }

        /// <summary>
        /// Obtém e atribui o tipo de símbolo que representa um separador de objectos.
        /// </summary>
        /// <value>
        /// O tipo de separador.
        /// </value>
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
        /// <param name="matrixFactory">A função responsável pela criação das matrizes.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            Func<int, int, M> matrixFactory,
            out M matrix)
        {
            return this.TryParseMatrix(reader, parser, null, matrixFactory, out matrix);
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos estabelecendo um valor por defeito.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="errors">Os erros da leitura.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="matrixFactory">A função responsável pela criação de matrizes.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            ILogStatus<string, EParseErrorLevel> errors,
            T defaultValue,
            Func<int, int, T, M> matrixFactory,
            out M matrix)
        {
            if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                matrix = default(M);
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

                    matrix = matrixFactory.Invoke(lines, columns, defaultValue);
                    this.SetupResultMatrix(matrix, new int[] { lines, columns }, this.rangeReader.Elements);
                    return true;
                }
            }
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="errors">Os erros da leitura.</param>
        /// <param name="matrixFactory">A função responsável pela criação de matrizes.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            ILogStatus<string, EParseErrorLevel> errors,
            Func<int, int, M> matrixFactory,
            out M matrix)
        {
            matrix = default(M);
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

                matrix = matrixFactory.Invoke(lines, columns);
                this.SetupResultMatrix(matrix, new int[] { lines, columns }, this.rangeReader.Elements);
                return true;
            }
        }

        /// <summary>
        /// Mapeia um símbolo interno de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbolType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        public void MapInternalDelimiters(SymbType openSymbolType, SymbType closeSymbType)
        {
            this.rangeReader.MapInternalDelimiters(openSymbolType, closeSymbType);
        }

        /// <summary>
        /// Mapeia um símbolo externo de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        public void MapExternalDelimiters(SymbType openSymbType, SymbType closeSymbType)
        {
            this.rangeReader.MapExternalDelimiters(openSymbType, closeSymbType);
        }

        /// <summary>
        /// Marca um tipo de símbolo como sendo vazio.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void AddBlanckSymbolType(SymbType symbolType)
        {
            this.rangeReader.AddBlanckSymbolType(symbolType);
        }

        /// <summary>
        /// Remove o tipo de símbolo especificado da lista de símbolos vazios.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void RemoveBlanckSymbolType(SymbType symbolType)
        {
            this.rangeReader.RemoveBlanckSymbolType(symbolType);
        }

        /// <summary>
        /// Desmarca todos os símbolos vazios.
        /// </summary>
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
        private void SetupResultMatrix(M matrix, int[] configuration, ReadOnlyCollection<T> elements)
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
        private ConfigMatrixReader<T, M, string, string> arrayMatrixReader;

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

    #endregion Leitor Configurado

    #region Leitor MatrixMarket tipo coordenadas

    /// <summary>
    /// Permite efectuar a leitura de uma matriz cujos índices são generalizados.
    /// </summary>
    /// <typeparam name="C1">O tipo de objectos que constituem a primeira coordenada.</typeparam>
    /// <typeparam name="C2">O tipo de objectos que consituem a segunda coordenada.</typeparam>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da matriz.</typeparam>
    /// <typeparam name="SymbVal">O tipo de objecto que representa o valor do símbolo lido.</typeparam>
    /// <typeparam name="SymbType">O tipo de objecto que representa o tipo do símbolo lido.</typeparam>
    public class CoordinatesListReader<C1, C2, T, SymbVal, SymbType>
    {
        /// <summary>
        /// O leitor da primeira coordenada.
        /// </summary>
        private IParse<C1, SymbVal, SymbType> firstCoordParser;

        /// <summary>
        /// O leitor da segunda coordenada.
        /// </summary>
        private IParse<C2, SymbVal, SymbType> secondCoordParser;

        /// <summary>
        /// O leitor da terceira coordenada.
        /// </summary>
        private IParse<T, SymbVal, SymbType> elementParser;

        /// <summary>
        /// Mantém o tipo de símbolo que separa cada grupo de valores.
        /// </summary>
        /// <remarks>
        /// Um grupo de valores é constituído pelas coordenadas e elemento associado.
        /// </remarks>
        private SymbType groupSymbolSeparator;

        /// <summary>
        /// Mantém o tipo de símbolo que separa cada item num grupo de valores.
        /// </summary>
        private SymbType itemSymbolSeparator;

        /// <summary>
        /// Define os delimitadores de comentários.
        /// </summary>
        private GeneralMapper<SymbType, SymbType> commentSymbols;

        /// <summary>
        /// Define os delimitadores de valores.
        /// </summary>
        private GeneralMapper<SymbType, SymbType> delimiters;

        /// <summary>
        /// Inclui os símbolos que não são considerados na leitura.
        /// </summary>
        /// <remarks>
        /// Os símbolos não serão ignorados quando se encontrarem entre delimitadores
        /// ou comentários.
        /// </remarks>
        private List<SymbType> blancks;

        /// <summary>
        /// Mantém o leitor de símbolos.
        /// </summary>
        private ISymbolReader<SymbVal, SymbType> reader;

        /// <summary>
        /// Mantém os elementos lidos.
        /// </summary>
        private CoordItem current;

        /// <summary>
        /// Valor que indica se o leitor foi iniciado.
        /// </summary>
        private bool started;

        /// <summary>
        /// Valor que indica se o leitor foi terminado.
        /// </summary>
        private bool ended;

        /// <summary>
        /// Lista que contém a leitura corrente.
        /// </summary>
        private List<ISymbol<SymbVal, SymbType>> buffer;

        /// <summary>
        /// Os símbolos de fecho associados a um símbolo de leitura.
        /// </summary>
        private ReadOnlyCollection<SymbType> currentMatches;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinatesListReader{C1, C2, T, SymbVal, SymbType}"/>.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="firstCoordParser">O leitor da primeira coordenada.</param>
        /// <param name="secondCoordParser">O leitor da segunda coordenada.</param>
        /// <param name="elementParser">O leitor dos elementos.</param>
        /// <param name="groupSymbolSeparator">
        /// O separador de groupos definido pelas coordenadas e valor do elemento.
        /// </param>
        /// <param name="itemSymbolSeparator">O separador dos itens num grupo.</param>
        public CoordinatesListReader(
            IParse<C1, SymbVal, SymbType> firstCoordParser,
            IParse<C2, SymbVal, SymbType> secondCoordParser,
            IParse<T, SymbVal, SymbType> elementParser,
            SymbType groupSymbolSeparator,
            SymbType itemSymbolSeparator,
            ISymbolReader<SymbVal, SymbType> reader)
            : this(
                reader,
                firstCoordParser,
                secondCoordParser,
                elementParser,
                groupSymbolSeparator,
                itemSymbolSeparator,
                EqualityComparer<SymbType>.Default) { }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordinatesListReader{C1, C2, T, SymbVal, SymbType}"/>.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="firstCoordParser">O leitor da primeira coordenada.</param>
        /// <param name="secondCoordParser">O leitor da segunda coordenada.</param>
        /// <param name="elementParser">O leitor dos elementos.</param>
        /// <param name="groupSymbolSeparator">
        /// O separador de groupos definido pelas coordenadas e valor do elemento.
        /// </param>
        /// <param name="itemSymbolSeparator">O separador dos itens num grupo.</param>
        /// <param name="symbolComparer">O comparador de igualdade de símbolos.</param>
        public CoordinatesListReader(
            ISymbolReader<SymbVal, SymbType> reader,
            IParse<C1, SymbVal, SymbType> firstCoordParser,
            IParse<C2, SymbVal, SymbType> secondCoordParser,
            IParse<T, SymbVal, SymbType> elementParser,
            SymbType groupSymbolSeparator,
            SymbType itemSymbolSeparator,
            IEqualityComparer<SymbType> symbolComparer)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (firstCoordParser == null)
            {
                throw new ArgumentNullException("firstCoordParser");
            }
            else if (secondCoordParser == null)
            {
                throw new ArgumentNullException("secondCoordParser");
            }
            else if (elementParser == null)
            {
                throw new ArgumentNullException("elementParser");
            }
            else if (groupSymbolSeparator == null)
            {
                throw new ArgumentNullException("groupSymbolSeparator");
            }
            else if (itemSymbolSeparator == null)
            {
                throw new ArgumentNullException("itemSymbolSeparator");
            }
            else if (symbolComparer == null)
            {
                throw new ArgumentNullException("symbolComparer");
            }
            else
            {
                this.reader = reader;
                this.firstCoordParser = firstCoordParser;
                this.secondCoordParser = secondCoordParser;
                this.elementParser = elementParser;
                this.groupSymbolSeparator = groupSymbolSeparator;
                this.itemSymbolSeparator = itemSymbolSeparator;
                this.commentSymbols = new GeneralMapper<SymbType, SymbType>(
                    symbolComparer,
                    symbolComparer);
                this.delimiters = new GeneralMapper<SymbType, SymbType>(
                    symbolComparer,
                    symbolComparer);
                this.blancks = new List<SymbType>();
                this.started = false;
                this.ended = false;
                this.current = null;
                this.buffer = new List<ISymbol<SymbVal, SymbType>>();
            }
        }

        /// <summary>
        /// Obtém ou atribui o leitor de símbolos.
        /// </summary>
        /// <remarks>
        /// A atribuição de um novo leitor de símbolos irá reinicializar as variáveis e só
        /// é possível caso o leitor de coordenadas se encontre inicializado ou finalizado.
        /// </remarks>
        public ISymbolReader<SymbVal, SymbType> Reader
        {
            get
            {
                return this.reader;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Value can't be null.");
                }
                else if (this.started)
                {
                    throw new UtilitiesException("Reader was already started.");
                }
                else
                {
                    this.reader = value;
                    this.started = false;
                    this.ended = false;
                    this.current = null;
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o leitor foi inicializado.
        /// </summary>
        public bool Started
        {
            get
            {
                return this.started;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o leitor foi finalizado.
        /// </summary>
        public bool Ended
        {
            get
            {
                return this.ended;
            }
        }

        /// <summary>
        /// Obtém o elemento corrente do leitor.
        /// </summary>
        public ACoordItem Current
        {
            get
            {
                if (this.started)
                {
                    if (this.ended)
                    {
                        throw new UtilitiesException("The reader was alaready ended.");
                    }
                    else
                    {
                        return this.current;
                    }
                }
                else
                {
                    throw new UtilitiesException("The reader hasn't been started.");
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o tipo de símbolo que separa cada grupo de valores.
        /// </summary>
        /// <remarks>
        /// Um grupo de valores é constituído pelas coordenadas e elemento associado.
        /// </remarks>
        public SymbType GroupSymbolSeparator
        {
            get
            {
                return this.groupSymbolSeparator;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Value cant' be null.");
                }
                else if (this.started)
                {
                    throw new UtilitiesException("The reader was already started.");
                }
                else
                {
                    this.groupSymbolSeparator = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o tipo de símbolo que separa cada item num grupo de valores.
        /// </summary>
        public SymbType ItemSymbolSeparator
        {
            get
            {
                return this.itemSymbolSeparator;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("Value can't be null.");
                }
                else if (this.started)
                {
                    throw new UtilitiesException("The reader was already started.");
                }
                else
                {
                    this.itemSymbolSeparator = value;
                }
            }
        }

        /// <summary>
        /// Regista delimitadores de expressões.
        /// </summary>
        /// <param name="open">O delimitador de abertura.</param>
        /// <param name="close">O delimitador de fecho.</param>
        public void RegisterDelimiter(SymbType open, SymbType close)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }
            else if (close == null)
            {
                throw new ArgumentNullException("close");
            }
            else
            {
                this.delimiters.Add(open, close);
            }
        }

        /// <summary>
        /// Elimina o mapeamento de delimtitadores da colecção.
        /// </summary>
        /// <param name="open">O delimitador de abertura.</param>
        /// <param name="close">O delimitador de fecho.</param>
        public void UnregisterDelimiter(SymbType open, SymbType close)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }
            else if (close == null)
            {
                throw new ArgumentNullException("close");
            }
            else if (this.started)
            {
                throw new UtilitiesException("The reader was already started.");
            }
            else
            {
                this.delimiters.RemoveMap(open, close);
            }
        }

        /// <summary>
        /// Regista delimitadores de comentários.
        /// </summary>
        /// <param name="open">O delimitador de abertura.</param>
        /// <param name="close">O delimitador de fecho.</param>
        public void RegisterCommentDelimiter(SymbType open, SymbType close)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }
            else if (close == null)
            {
                throw new ArgumentNullException("close");
            }
            else if (this.started)
            {
                throw new UtilitiesException("The reader was already started.");
            }
            else
            {
                this.commentSymbols.Add(open, close);
            }
        }

        /// <summary>
        /// Elimina o mapeamento de delimitadores de comentário.
        /// </summary>
        /// <param name="open">O delimitador de abertura.</param>
        /// <param name="close">O delimitador de fecho.</param>
        public void UnregisterCommentDelimiter(SymbType open, SymbType close)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }
            else if (close == null)
            {
                throw new ArgumentNullException("close");
            }
            else if (this.started)
            {
                throw new UtilitiesException("The reader was already started.");
            }
            else
            {
                this.commentSymbols.RemoveMap(open, close);
            }
        }

        /// <summary>
        /// Adiciona um símbolo que será ignorado no decurso da leitura.
        /// </summary>
        /// <remarks>
        /// O símbolo não será ignorado se se encontrar entre delimitadores.
        /// </remarks>
        /// <param name="symbType">O tipo de símbolo que será ignorado.</param>
        public void AddBlanck(SymbType symbType)
        {
            if (symbType == null)
            {
                throw new ArgumentNullException("symbType");
            }
            else
            {
                if (!this.blancks.Contains(symbType, this.delimiters.ObjectComparer))
                {
                    this.blancks.Add(symbType);
                }
            }
        }

        /// <summary>
        /// Remove o tipo de símbolo entre os que estão marcados
        /// para serem ignorados.
        /// </summary>
        /// <param name="symbType">O tipo de símbolo a remover.</param>
        public void RemoveBlanck(SymbType symbType)
        {
            if (symbType != null)
            {
                var foundSymbol = this.blancks.Find(
                    s => this.delimiters.ObjectComparer.Equals(s, symbType));
                if (foundSymbol != null)
                {
                    this.blancks.Remove(foundSymbol);
                }
            }
        }

        /// <summary>
        /// Elimina todos os símbolos marcados para serem ignorados.
        /// </summary>
        public void ClearBlancks()
        {
            this.blancks.Clear();
        }

        /// <summary>
        /// Reinicia o leitor.
        /// </summary>
        public void Reset()
        {
            this.buffer.Clear();
            this.current = null;
            this.ended = false;
        }

        /// <summary>
        /// Efectua a leitura do próximo terno de coordenadas.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso a leitura seja bem-sucedida e falso caso o leitor se
        /// encontre no final.
        /// </returns>
        public bool MoveNext()
        {
            if (this.ended)
            {
                return false;
            }
            else
            {
                this.started = true;
                var result = this.AuxMoveNext();
                if (!result)
                {
                    this.ended = true;
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a leitura do próximo triplo de coordenadas e elemento.
        /// </summary>
        /// <returns>Veraddeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        private bool AuxMoveNext()
        {
            this.current = null;
            var firstCoord = default(C1);
            var secondCoord = default(C2);
            var parseLogs = new LogStatus<string, EParseErrorLevel>();
            var foundError = false;
            var comparer = this.delimiters.ObjectComparer;
            var state = 0;
            var coordState = 0;
            while (state != -1)
            {
                if (state == 0) // Primeira leitura
                {
                    if (this.reader.IsAtEOF())
                    {
                        return false;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (!comparer.Equals(readed.SymbolType, this.groupSymbolSeparator))
                        {
                            if (!comparer.Equals(readed.SymbolType, this.itemSymbolSeparator))
                            {
                                if (this.delimiters.ContainsObject(readed.SymbolType))
                                {
                                    this.currentMatches = this.delimiters.TargetFor(readed.SymbolType);
                                    this.buffer.Add(readed);
                                    coordState = 1;
                                    state = 6;
                                }
                                else if (this.commentSymbols.ContainsObject(readed.SymbolType))
                                {
                                    this.currentMatches = this.commentSymbols.TargetFor(readed.SymbolType);
                                    coordState = 0;
                                    state = 7;
                                }
                                else if (!this.blancks.Contains(readed.SymbolType, comparer))
                                {
                                    this.buffer.Add(readed);
                                    state = 1;
                                }
                            }
                        }
                    }
                }
                else if (state == 1) // Primeira coordenada
                {

                    if (this.reader.IsAtEOF())
                    {
                        parseLogs.AddLog(
                            "Found end of file after first coordinate.",
                            EParseErrorLevel.ERROR);
                        this.buffer.Clear();
                        this.current = new CoordItem()
                        {
                            HasError = true,
                            ErrorLog = parseLogs
                        };

                        return true;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (comparer.Equals(readed.SymbolType, this.groupSymbolSeparator))
                        {
                            parseLogs.AddLog(
                                "Found group separator after first coordinate.",
                                EParseErrorLevel.ERROR);
                            this.buffer.Clear();
                            this.current = new CoordItem()
                            {
                                HasError = true,
                                ErrorLog = parseLogs
                            };

                            return true;
                        }
                        else if (comparer.Equals(readed.SymbolType, this.itemSymbolSeparator))
                        {
                            firstCoord = this.firstCoordParser.Parse(
                                this.buffer.ToArray(),
                                parseLogs);
                            if (parseLogs.HasLogs(EParseErrorLevel.ERROR))
                            {
                                foundError = true;
                            }

                            this.buffer.Clear();
                            state = 2;
                        }
                        else if (this.delimiters.ContainsObject(readed.SymbolType))
                        {
                            this.currentMatches = this.delimiters.TargetFor(readed.SymbolType);
                            coordState = 1;
                            this.buffer.Add(readed);
                            state = 6;
                        }
                        else if (this.commentSymbols.ContainsObject(readed.SymbolType))
                        {
                            this.currentMatches = this.commentSymbols.TargetFor(readed.SymbolType);
                            coordState = 1;
                            state = 7;
                        }
                        else if (!this.blancks.Contains(readed.SymbolType, comparer))
                        {
                            this.buffer.Add(readed);
                        }
                    }
                }
                else if (state == 2) // Segunda coordenada (primeira leitura)
                {
                    if (this.reader.IsAtEOF())
                    {
                        parseLogs.AddLog(
                                "Found end of file before second coordinate.",
                                EParseErrorLevel.ERROR);
                        this.buffer.Clear();
                        this.current = new CoordItem()
                        {
                            ErrorLog = parseLogs,
                            HasError = true
                        };

                        return true;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (comparer.Equals(readed.SymbolType, this.groupSymbolSeparator))
                        {
                            parseLogs.AddLog(
                                "Found group separator before second coordinate.",
                                EParseErrorLevel.ERROR);
                            this.buffer.Clear();
                            this.current = new CoordItem()
                            {
                                ErrorLog = parseLogs,
                                HasError = true
                            };

                            return true;
                        }
                        else if (!comparer.Equals(readed.SymbolType, this.itemSymbolSeparator))
                        {
                            if (this.delimiters.ContainsObject(readed.SymbolType))
                            {
                                this.currentMatches = this.delimiters.TargetFor(readed.SymbolType);
                                coordState = 3;
                                this.buffer.Add(readed);
                                state = 6;
                            }
                            else if (this.commentSymbols.ContainsObject(readed.SymbolType))
                            {
                                this.currentMatches = this.commentSymbols.TargetFor(readed.SymbolType);
                                coordState = 2;
                                state = 7;
                            }
                            else if (!this.blancks.Contains(readed.SymbolType, comparer))
                            {
                                this.buffer.Add(readed);
                                state = 3;
                            }
                        }
                    }
                }
                else if (state == 3) // Segunda coordenada (segunda leitura)
                {
                    if (this.reader.IsAtEOF())
                    {
                        parseLogs.AddLog(
                                "Found end of file after second coordinate.",
                                EParseErrorLevel.ERROR);
                        this.buffer.Clear();
                        this.current = new CoordItem()
                        {
                            ErrorLog = parseLogs,
                            HasError = true
                        };

                        return true;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (comparer.Equals(readed.SymbolType, this.groupSymbolSeparator))
                        {
                            parseLogs.AddLog(
                                "Found group separator after second coordinate.",
                                EParseErrorLevel.ERROR);
                            this.buffer.Clear();
                            this.current = new CoordItem()
                            {
                                ErrorLog = parseLogs,
                                HasError = true
                            };

                            return true;
                        }
                        else if (comparer.Equals(readed.SymbolType, this.itemSymbolSeparator))
                        {
                            secondCoord = secondCoordParser.Parse(
                                this.buffer.ToArray(),
                                parseLogs);
                            if (parseLogs.HasLogs(EParseErrorLevel.ERROR))
                            {
                                foundError = true;
                            }

                            this.buffer.Clear();
                            state = 4;
                        }
                        else if (this.delimiters.ContainsObject(readed.SymbolType))
                        {
                            this.currentMatches = this.delimiters.TargetFor(readed.SymbolType);
                            coordState = 3;
                            this.buffer.Add(readed);
                            state = 6;
                        }
                        else if (this.commentSymbols.ContainsObject(readed.SymbolType))
                        {
                            this.currentMatches = this.commentSymbols.TargetFor(readed.SymbolType);
                            coordState = 3;
                            state = 7;
                        }
                        else if (!this.blancks.Contains(readed.SymbolType, comparer))
                        {
                            this.buffer.Add(readed);
                        }
                    }
                }
                else if (state == 4) // Elemento (primeira leitura)
                {
                    if (this.reader.IsAtEOF())
                    {
                        parseLogs.AddLog(
                                "Found end of file before element.",
                                EParseErrorLevel.ERROR);
                        this.buffer.Clear();
                        this.current = new CoordItem()
                        {
                            ErrorLog = parseLogs,
                            HasError = true
                        };

                        return true;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (comparer.Equals(readed.SymbolType, this.groupSymbolSeparator))
                        {
                            parseLogs.AddLog(
                                "Found group separator before second coordinate.",
                                EParseErrorLevel.ERROR);
                            this.buffer.Clear();
                            this.current = new CoordItem()
                            {
                                ErrorLog = parseLogs,
                                HasError = true
                            };

                            return true;
                        }
                        else if (!comparer.Equals(readed.SymbolType, this.itemSymbolSeparator))
                        {
                            if (this.delimiters.ContainsObject(readed.SymbolType))
                            {
                                this.currentMatches = this.delimiters.TargetFor(readed.SymbolType);
                                coordState = 5;
                                this.buffer.Add(readed);
                                state = 6;
                            }
                            else if (this.commentSymbols.ContainsObject(readed.SymbolType))
                            {
                                this.currentMatches = this.commentSymbols.TargetFor(readed.SymbolType);
                                coordState = 4;
                                state = 7;
                            }
                            else if (!this.blancks.Contains(readed.SymbolType, comparer))
                            {
                                this.buffer.Add(readed);
                                state = 5;
                            }
                        }
                    }
                }
                else if (state == 5) // Elemento (segunda leitura)
                {
                    if (this.reader.IsAtEOF())
                    {
                        var readedElement = this.elementParser.Parse(
                                this.buffer.ToArray(),
                                parseLogs);
                        if (parseLogs.HasLogs(EParseErrorLevel.ERROR))
                        {
                            foundError = true;
                        }

                        this.buffer.Clear();
                        this.current = new CoordItem()
                        {
                            Line = firstCoord,
                            Column = secondCoord,
                            Element = readedElement,
                            HasError = foundError,
                            ErrorLog = parseLogs
                        };

                        return true;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (comparer.Equals(readed.SymbolType, this.groupSymbolSeparator))
                        {
                            var readedElement = this.elementParser.Parse(
                                this.buffer.ToArray(),
                                parseLogs);
                            if (parseLogs.HasLogs(EParseErrorLevel.ERROR))
                            {
                                foundError = true;
                            }

                            this.buffer.Clear();
                            this.current = new CoordItem()
                            {
                                Line = firstCoord,
                                Column = secondCoord,
                                Element = readedElement,
                                HasError = foundError,
                                ErrorLog = parseLogs
                            };

                            return true;
                        }
                        else
                        {
                            if (comparer.Equals(readed.SymbolType, this.itemSymbolSeparator))
                            {
                                var readedElement = this.elementParser.Parse(
                                this.buffer.ToArray(),
                                parseLogs);
                                if (parseLogs.HasLogs(EParseErrorLevel.ERROR))
                                {
                                    foundError = true;
                                }

                                this.buffer.Clear();
                                this.current = new CoordItem()
                                {
                                    Line = firstCoord,
                                    Column = secondCoord,
                                    Element = readedElement,
                                    HasError = foundError,
                                    ErrorLog = parseLogs
                                };

                                // Verifica se não existem mais objectos a serem lidos.
                                state = 8;
                            }
                            else if (this.delimiters.ContainsObject(readed.SymbolType))
                            {
                                this.currentMatches = this.delimiters.TargetFor(readed.SymbolType);
                                coordState = 5;
                                this.buffer.Add(readed);
                                state = 6;
                            }
                            else if (this.commentSymbols.ContainsObject(readed.SymbolType))
                            {
                                this.currentMatches = this.commentSymbols.TargetFor(readed.SymbolType);
                                var readedElement = this.elementParser.Parse(
                                this.buffer.ToArray(),
                                parseLogs);
                                if (parseLogs.HasLogs(EParseErrorLevel.ERROR))
                                {
                                    foundError = true;
                                }

                                this.buffer.Clear();
                                this.current = this.current = new CoordItem()
                                {
                                    Line = firstCoord,
                                    Column = secondCoord,
                                    Element = readedElement,
                                    HasError = foundError,
                                    ErrorLog = parseLogs
                                };

                                coordState = 8;
                                state = 7;
                            }
                            else if (!this.blancks.Contains(readed.SymbolType, comparer))
                            {
                                this.buffer.Add(readed);
                            }
                        }
                    }
                }
                else if (state == 6) // Delimitadores
                {
                    if (this.reader.IsAtEOF())
                    {
                        parseLogs.AddLog(
                                "Found no matching close delimiter before end of file.",
                                EParseErrorLevel.ERROR);
                        this.buffer.Clear();
                        this.current = new CoordItem()
                        {
                            HasError = true,
                            ErrorLog = parseLogs
                        };

                        return true;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        buffer.Add(readed);
                        if (this.currentMatches.Contains(readed.SymbolType, comparer))
                        {
                            state = coordState;
                        }
                    }
                }
                else if (state == 7) // Comentários
                {
                    if (this.reader.IsAtEOF())
                    {
                        if (coordState == 5)
                        {
                            var readedElement = this.elementParser.Parse(
                                   this.buffer.ToArray(),
                                   parseLogs);
                            if (parseLogs.HasLogs(EParseErrorLevel.ERROR))
                            {
                                foundError = true;
                            }

                            this.buffer.Clear();
                            this.current = new CoordItem()
                            {
                                Line = firstCoord,
                                Column = secondCoord,
                                Element = readedElement,
                                HasError = foundError,
                                ErrorLog = parseLogs
                            };

                            return true;
                        }
                        else
                        {
                            parseLogs.AddLog(
                                "Unexpected end of file.",
                                EParseErrorLevel.ERROR);
                            this.buffer.Clear();
                            this.current = new CoordItem()
                            {
                                HasError = true,
                                ErrorLog = parseLogs
                            };

                            return true;
                        }
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (this.currentMatches.Contains(readed.SymbolType, comparer))
                        {
                            state = coordState;
                        }
                    }
                }
                else if (state == 8) // Separador de itens no final do elemento
                {
                    if (this.reader.IsAtEOF())
                    {
                        this.buffer.Clear();
                        return true;
                    }
                    else
                    {
                        var readed = this.reader.Get();
                        if (comparer.Equals(this.groupSymbolSeparator, readed.SymbolType))
                        {
                            this.buffer.Clear();
                            return true;
                        }
                        else if (this.commentSymbols.ContainsObject(readed.SymbolType))
                        {
                            coordState = 8;
                            state = 7;
                        }
                        else
                        {
                            if (!comparer.Equals(readed.SymbolType, this.itemSymbolSeparator)
                                && !this.blancks.Contains(readed.SymbolType, comparer))
                            {
                                parseLogs.AddLog(
                                    string.Format(
                                        "Found extra data after element: symb_val: {0}\n symb_type: {1}",
                                        readed.SymbolValue,
                                        readed.SymbolType),
                                    EParseErrorLevel.ERROR);
                                this.current.HasError = true;

                                // Avança até ao próximo separador de grupo.
                                while (!this.reader.IsAtEOF())
                                {
                                    readed = this.reader.Get();
                                    if (comparer.Equals(this.groupSymbolSeparator, readed.SymbolType))
                                    {
                                        this.buffer.Clear();
                                        return true;
                                    }
                                }

                                this.buffer.Clear();
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Representa um item lido.
        /// </summary>
        public abstract class ACoordItem
        {
            /// <summary>
            /// Número da linha.
            /// </summary>
            protected C1 line;

            /// <summary>
            /// Número da coluna.
            /// </summary>
            protected C2 column;

            /// <summary>
            /// O valor do elemento.
            /// </summary>
            protected T element;

            /// <summary>
            /// Os diários da leitura.
            /// </summary>
            protected LogStatus<string, EParseErrorLevel> errorLog;

            /// <summary>
            /// Valor que indica se um error foi encontrado.
            /// </summary>
            protected bool hasError;

            /// <summary>
            /// Obtém o número da linha.
            /// </summary>
            public C1 Line
            {
                get
                {
                    if (this.hasError)
                    {
                        throw new UtilitiesException("The current element has an error.");
                    }
                    else
                    {
                        return this.line;
                    }
                }
            }

            /// <summary>
            /// Obtém o número da coluna.
            /// </summary>
            public C2 Column
            {
                get
                {
                    if (this.hasError)
                    {
                        throw new UtilitiesException("The current element has an error.");
                    }
                    else
                    {
                        return this.column;
                    }
                }
            }

            /// <summary>
            /// Obtém o valor do elemento.
            /// </summary>
            public T Element
            {
                get
                {
                    if (this.hasError)
                    {
                        throw new UtilitiesException("The current element has an error.");
                    }
                    else
                    {
                        return this.element;
                    }
                }
            }

            /// <summary>
            /// Obtém o diário dos erros ou avisos.
            /// </summary>
            public ILogStatus<string, EParseErrorLevel> ErrorLog
            {
                get
                {
                    return this.errorLog;
                }
            }

            /// <summary>
            /// Obtém um valor que indica se o item contém erros.
            /// </summary>
            public bool HasError
            {
                get
                {
                    return this.hasError;
                }
            }
        }

        /// <summary>
        /// Representa um item lido.
        /// </summary>
        protected class CoordItem : ACoordItem
        {
            /// <summary>
            /// Atribui o número da linha.
            /// </summary>
            public new C1 Line
            {
                get
                {
                    return base.line;
                }
                set
                {
                    this.line = value;
                }
            }

            /// <summary>
            /// Atribui o número da coluna.
            /// </summary>
            public new C2 Column
            {
                get
                {
                    return this.column;
                }
                set
                {
                    this.column = value;
                }
            }

            /// <summary>
            /// Atribui o valor do elemento lido.
            /// </summary>
            public new T Element
            {
                get
                {
                    return this.element;
                }
                set
                {
                    this.element = value;
                }
            }

            /// <summary>
            /// Atribui o diário de erros.
            /// </summary>
            public new LogStatus<string, EParseErrorLevel> ErrorLog
            {
                get
                {
                    return this.errorLog;
                }
                set
                {
                    this.errorLog = value;
                }
            }

            /// <summary>
            /// Atribui o valor que indica se o item contém erros.
            /// </summary>
            public new bool HasError
            {
                get
                {
                    return this.hasError;
                }
                set
                {
                    this.hasError = value;
                }
            }
        }
    }

    /// <summary>
    /// Efectua a leitura de uma matriz MatrixMarket com formato de lista
    /// de coordenadas e valores.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem as entradas da matriz.</typeparam>
    public class CoordMatrixMarketReader<T>
    {
        /// <summary>
        /// Função a ser executada aquando da leitura das dimensões da matriz, nomeadamente,
        /// o número de linhas, o número de colunas e o número de entradas não nulas.
        /// </summary>
        protected Action<long, long, long> dimensionsAction;

        /// <summary>
        /// Função a ser executada aquando da leitura de um novo terno da matriz.
        /// </summary>
        protected Action<long, long, T> elementsAction;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CoordMatrixMarketReader{T}"/>
        /// </summary>
        /// <param name="dimensionsAction">Acção aplicada aquando da determinação do número de linhas,
        /// colunas e entradas não nulas da matriz.
        /// </param>
        /// <param name="elementsAction">
        /// Acção aplicada aos ternos de coordenadas e elementos obtidos da leitura.
        /// </param>
        public CoordMatrixMarketReader(
            Action<long, long, long> dimensionsAction,
            Action<long, long, T> elementsAction)
        {
            if (dimensionsAction == null)
            {
                throw new ArgumentNullException("dimensionsAction");
            }
            else if (elementsAction == null)
            {
                throw new ArgumentNullException("elementsAction");
            }
            else
            {
                this.dimensionsAction = dimensionsAction;
                this.elementsAction = elementsAction;
            }
        }

        /// <summary>
        /// Efectua a leitura dos elementos da matriz.
        /// </summary>
        /// <param name="reader">O leitor de informação.</param>
        /// <param name="coordinatesParser">O leitor dos valores inteiros.</param>
        /// <param name="elementsParser">O leitor dos elementos.</param>
        /// <param name="delimiters">O mapeamento para os delimitadores.</param>
        /// <returns>
        /// O diário que indica o resultado da leitura.
        /// </returns>
        public ILogStatus<string, EParseErrorLevel> Parse(
            StringSymbolReader reader,
            IParse<long, string, string> coordinatesParser,
            IParse<T, string, string> elementsParser,
            GeneralMapper<string, string> delimiters)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (coordinatesParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else if (delimiters == null)
            {
                throw new ArgumentNullException("delimiters");
            }
            else
            {
                var result = new LogStatus<string, EParseErrorLevel>();
                var state = 0;
                var buffer = new List<ISymbol<string, string>>();
                var dimensions = new MutableTuple<long, long, long>();
                while (state != -1)
                {
                    if (reader.IsAtEOF())
                    {
                        state = -1;
                    }
                    else
                    {
                        var readed = reader.Get();
                        var symbType = readed.SymbolType;
                        if (state == 0)
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.MOD))
                            {
                                state = 1;
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.INTEGER))
                            {
                                buffer.Add(readed);
                                state = 2;
                            }
                            else if (
                                symbType != Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE)
                                && symbType != Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE)
                                && symbType != Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN))
                            {
                                result.AddLog(string.Format(
                                    "Unexpected symbol at the begining of an header line: {0}=>{1}",
                                    readed.SymbolValue,
                                    readed.SymbolType),
                                    EParseErrorLevel.ERROR);
                                state = -1;
                            }
                        }
                        else if (state == 1) // Comentário
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE))
                            {
                                state = 0;
                            }
                        }
                        else if (state == 2) // Número de linhas
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.INTEGER))
                            {
                                buffer.Add(readed);
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE))
                            {
                                result.AddLog("Unexpected change of line after the value setting the number of lines.",
                                    EParseErrorLevel.ERROR);
                                state = -1;
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE))
                            {
                                var parsed = coordinatesParser.Parse(
                                    buffer.ToArray(),
                                    result);
                                dimensions.Item1 = parsed;
                                state = 3;
                            }
                            else
                            {
                                result.AddLog(string.Format(
                                    "Unexpected symbol after the value setting the number of lines: {0}=>{1}",
                                    readed.SymbolValue,
                                    readed.SymbolValue),
                                    EParseErrorLevel.ERROR);
                                state = -1;
                            }
                        }
                        else if (state == 3) // Número de colunas - início.
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.INTEGER))
                            {
                                buffer.Clear();
                                buffer.Add(readed);
                                state = 4;
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE))
                            {
                                result.AddLog(
                                    "Unexpected end of line after the value setting the number of lines.",
                                    EParseErrorLevel.ERROR);
                                state = -1;
                            }
                            else if (symbType != Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE))
                            {
                                result.AddLog(string.Format(
                                    "Unexpected symbol after the value setting the number of lines: {0}=>{1}",
                                    readed.SymbolValue,
                                    readed.SymbolType),
                                    EParseErrorLevel.ERROR);
                            }
                        }
                        else if (state == 4) // Número de colunas - leitura.
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE))
                            {
                                result.AddLog(
                                       "Unexpected end of line after the value setting the number of columns.",
                                       EParseErrorLevel.ERROR);
                                state = -1;
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE))
                            {
                                var parsed = coordinatesParser.Parse(
                                       buffer.ToArray(),
                                       result);
                                dimensions.Item2 = parsed;
                                state = 5;
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.INTEGER))
                            {
                                buffer.Add(readed);
                            }
                            else
                            {
                                result.AddLog(string.Format(
                                       "Unexpected symbol after the value setting the number of columns: {0}=>{1}",
                                       readed.SymbolValue,
                                       readed.SymbolValue),
                                       EParseErrorLevel.ERROR);
                                state = -1;
                            }
                        }
                        else if (state == 5) // Número de elementos - início.
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.INTEGER))
                            {
                                buffer.Clear();
                                buffer.Add(readed);
                                state = 6;
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE))
                            {
                                result.AddLog(
                                    "Unexpected end of line after the value setting the number of columns.",
                                    EParseErrorLevel.ERROR);
                                state = -1;
                            }
                            else if (symbType != Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE))
                            {
                                result.AddLog(string.Format(
                                    "Unexpected symbol after the value setting the number of lines: {0}=>{1}",
                                    readed.SymbolValue,
                                    readed.SymbolType),
                                    EParseErrorLevel.ERROR);
                            }
                        }
                        else if (state == 6) // Número de elementos - leitura.
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE))
                            {
                                var parsed = coordinatesParser.Parse(
                                       buffer.ToArray(),
                                       result);
                                dimensions.Item3 = parsed;
                                this.dimensionsAction.Invoke(dimensions.Item1, dimensions.Item2, dimensions.Item3);

                                // Leitura das entradas da matriz
                                this.ReadCoordinates(
                                    reader,
                                    coordinatesParser,
                                    elementsParser,
                                    delimiters,
                                    result);
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE)
                                || symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN))
                            {
                                state = 7;
                            }
                            else if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.INTEGER))
                            {
                                buffer.Add(readed);
                            }
                            else
                            {
                                result.AddLog(string.Format(
                                       "Unexpected symbol after the value setting the number of columns: {0}=>{1}",
                                       readed.SymbolValue,
                                       readed.SymbolValue),
                                       EParseErrorLevel.ERROR);
                                state = -1;
                            }
                        }
                        else if (state == 7)
                        {
                            if (symbType == Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE))
                            {
                                var parsed = coordinatesParser.Parse(
                                       buffer.ToArray(),
                                       result);
                                dimensions.Item2 = parsed;
                                this.dimensionsAction.Invoke(dimensions.Item1, dimensions.Item2, dimensions.Item3);

                                // Leitura das entradas da matriz
                                this.ReadCoordinates(
                                    reader,
                                    coordinatesParser,
                                    elementsParser,
                                    delimiters,
                                    result);
                                state = -1;
                            }
                            else if (symbType != Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE)
                               && symbType != Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN))
                            {
                                result.AddLog(string.Format(
                                       "Unexpected symbol after the value setting the number of columns: {0}=>{1}",
                                       readed.SymbolValue,
                                       readed.SymbolValue),
                                       EParseErrorLevel.ERROR);

                                state = -1;
                            }
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Efectua a leitura das coordenadas.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="coordinatesParser">O leitor das coordenadas.</param>
        /// <param name="elementsParser">O leitor dos elementos.</param>
        /// <param name="delimiters">O conjunto de delimitadores.</param>
        /// <param name="log">O diário de erros.</param>
        private void ReadCoordinates(
            StringSymbolReader reader,
            IParse<long, string, string> coordinatesParser,
            IParse<T, string, string> elementsParser,
            GeneralMapper<string, string> delimiters,
            LogStatus<string, EParseErrorLevel> log)
        {
            var coordsReader = new CoordinatesListReader<long, long, T, string, string>(
                coordinatesParser,
                coordinatesParser,
                elementsParser,
                Utils.GetStringSymbolType(EStringSymbolReaderType.NEW_LINE),
                Utils.GetStringSymbolType(EStringSymbolReaderType.SPACE),
                reader);
            coordsReader.AddBlanck(Utils.GetStringSymbolType(EStringSymbolReaderType.CARRIAGE_RETURN));
            foreach (var kvp in delimiters)
            {
                coordsReader.RegisterDelimiter(kvp.ObjectValue, kvp.TargetValue);
            }

            while (coordsReader.MoveNext())
            {
                var current = coordsReader.Current;
                if (current.HasError)
                {
                    var currentLog = current.ErrorLog;
                    foreach (var errLog in currentLog.GetLogs())
                    {
                        log.AddLog(errLog.Value, errLog.Key);
                    }
                }
                else
                {
                    this.elementsAction.Invoke(current.Line, current.Column, current.Element);
                }
            }
        }

    }

    #endregion Leitor MatrixMarket tipo coordenadas
}
