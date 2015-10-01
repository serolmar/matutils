namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Data.SqlClient;

    /// <summary>
    /// Providencia um leitor de dados para CSV.
    /// </summary>
    /// <remarks>
    /// Trata-se de um leitor que permite flexibilizar a leitura dos vários
    /// elementos por coluna.
    /// </remarks>
    /// <typeparam name="ElementsType">O tipo de objectos que constituem os elementos a serem lidos.</typeparam>
    /// <typeparam name="SymbValue">O tipo de objectos que constituem os valores dos símbolos lidos</typeparam>
    /// <typeparam name="SymbType">O tipo de objectos que constituem os tipos dos símbolos lidos.</typeparam>
    public class CsvDataReader<ElementsType, SymbValue, SymbType> : IDataReader
    {
        /// <summary>
        /// Indica se o leitor finalizou a leitura.
        /// </summary>
        private bool hasEnded = false;

        /// <summary>
        /// Indica se o leitor se encontra a realizar a leitura.
        /// </summary>
        private int currentRow = -1;

        /// <summary>
        /// Define o leitor de símbolos.
        /// </summary>
        private ISymbolReader<SymbValue, SymbType> reader;

        /// <summary>
        /// O separador de linha.
        /// </summary>
        private SymbType lineSeparator;

        /// <summary>
        /// O separador de coluna.
        /// </summary>
        private SymbType columnSeparator;

        /// <summary>
        /// O símbolo que marca o final do CSV.
        /// </summary>
        private SymbType endOfCsvSymbol;

        /// <summary>
        /// Mantém o comparador de símbolos.
        /// </summary>
        private IEqualityComparer<SymbType> symbolTypesComparer;

        /// <summary>
        /// Os delimitadores.
        /// </summary>
        private Dictionary<SymbType, List<SymbType>> delimiters;

        /// <summary>
        /// A lista de tipos de símbolos a serem ignorados.
        /// </summary>
        private List<SymbType> ignoreSymbolTypes = new List<SymbType>();

        /// <summary>
        /// Mantém os leitores para as colunas.
        /// </summary>
        private Tuple<IParse<ElementsType, SymbValue, SymbType>, string>[] columnParsers;

        /// <summary>
        /// Mantém um indexador para as colunas.
        /// </summary>
        private Dictionary<string, int> namesIndexer;

        /// <summary>
        /// Mantém o registo actual.
        /// </summary>
        private ElementsType[] currentRecord;

        /// <summary>
        /// Mantém a lista dos erros actuais.
        /// </summary>
        private LogStatus<string, EParseErrorLevel> logStatus;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CsvDataReader{ElementsType, SymbValue, SymbType}"/>.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="columnParsers">O leitor de elementos das colunas.</param>
        /// <param name="lineSeparator">O tipo de dados que define o separador de linhas.</param>
        /// <param name="columnSeparator">O tipo de dados que define o separador de colunas.</param>
        /// <param name="endOfCsvSymbol">O símbolo que indica o término do CSV.</param>
        public CsvDataReader(
            ISymbolReader<SymbValue, SymbType> reader,
            Tuple<IParse<ElementsType, SymbValue, SymbType>, string>[] columnParsers,
            SymbType lineSeparator,
            SymbType columnSeparator,
            SymbType endOfCsvSymbol)
        {
            this.symbolTypesComparer = EqualityComparer<SymbType>.Default;
            this.ValidateSymbols(
                lineSeparator,
                columnSeparator,
                endOfCsvSymbol,
                this.symbolTypesComparer);
            this.ValidateAndSetFields(
                reader,
                columnParsers,
                lineSeparator,
                columnSeparator,
                endOfCsvSymbol);
            this.delimiters = new Dictionary<SymbType, List<SymbType>>(
                EqualityComparer<SymbType>.Default);
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CsvDataReader{ElementsType, SymbValue, SymbType}"/>.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="columnParsers">O leitor de elementos das colunas.</param>
        /// <param name="lineSeparator">O tipo de dados que define o separador de linhas.</param>
        /// <param name="columnSeparator">O tipo de dados que define o separador de colunas.</param>
        /// <param name="endOfCsvSymbol">O símbolo que indica o término do CSV.</param>
        /// <param name="symbolTypesComparer">O comaprador de tipos de símbolos.</param>
        public CsvDataReader(
            ISymbolReader<SymbValue, SymbType> reader,
            Tuple<IParse<ElementsType, SymbValue, SymbType>, string>[] columnParsers,
            SymbType lineSeparator,
            SymbType columnSeparator,
            SymbType endOfCsvSymbol,
            IEqualityComparer<SymbType> symbolTypesComparer)
        {
            if (symbolTypesComparer == null)
            {
                throw new ArgumentNullException("symbolTypesComparer");
            }
            else
            {
                this.symbolTypesComparer = symbolTypesComparer;
                this.ValidateSymbols(
                lineSeparator,
                columnSeparator,
                endOfCsvSymbol,
                this.symbolTypesComparer);
                this.ValidateAndSetFields(
                    reader,
                    columnParsers,
                    lineSeparator,
                    columnSeparator,
                    endOfCsvSymbol);
                this.delimiters = new Dictionary<SymbType, List<SymbType>>(
                    symbolTypesComparer);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna com o nome especificado.
        /// </summary>
        /// <param name="name">O nome da coluna.</param>
        /// <returns>O valor corrente associado à coluna.</returns>
        public object this[string name]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Column name can't be null nor empty.");
                }
                if (this.hasEnded)
                {
                    throw new UtilitiesDataException("The current data reader has already reached the end.");
                }
                else if (this.currentRow > -1)
                {
                    var trimmedName = name.Trim();
                    var index = default(int);
                    if (this.namesIndexer.TryGetValue(trimmedName, out index))
                    {
                        return this.currentRecord[index];
                    }
                    else
                    {
                        throw new KeyNotFoundException("Column with the specified name was not found.");
                    }
                }
                else
                {
                    throw new UtilitiesDataException("The current data reader hasn't started yet.");
                }
            }
        }

        /// <summary>
        /// Obtém o valor da coluna especificada pelo respectivo índice.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor corrent associada à coluna.</returns>
        public object this[int i]
        {
            get
            {
                if (i < 0 || i >= this.currentRecord.Length)
                {
                    throw new IndexOutOfRangeException("Index is out of range.");
                }
                else
                {
                    return this.currentRecord[i];
                }
            }
        }

        /// <summary>
        /// Obtém o nível de aninhamento para a linha corrente.
        /// </summary>
        public int Depth
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtém o número de colunas na linha corrente.
        /// </summary>
        public int FieldCount
        {
            get
            {
                return this.columnParsers.Length;
            }
        }

        /// <summary>
        /// Obtém o número de linhas alteradas pelo respectivo comando.
        /// </summary>
        public int RecordsAffected
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o leitor se encontra fechado.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return this.hasEnded;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a leitura da linha corrente contém erros.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                if (this.hasEnded)
                {
                    throw new UtilitiesDataException("The reader is already closed.");
                }
                else if (this.currentRow == -1)
                {
                    throw new UtilitiesDataException("The reader hasn't already been started.");
                }
                else
                {
                    return this.logStatus.HasLogs(EParseErrorLevel.ERROR);
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o separador de linhas.
        /// </summary>
        public SymbType LineSeparator
        {
            get
            {
                return this.lineSeparator;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Line sepatator value type can't be null.");
                }
                else
                {
                    if (this.currentRow == -1)
                    {
                        if (this.delimiters.ContainsKey(value))
                        {
                            throw new ArgumentException("Provided symbol is already being used as an open delimiter.");
                        }
                        else
                        {
                            this.lineSeparator = value;
                        }
                    }
                    else
                    {
                        throw new UtilitiesDataException("The data reader was already started.");
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o separador de colunas.
        /// </summary>
        public SymbType ColumnSeparator
        {
            get
            {
                return this.columnSeparator;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Column sepatator value type can't be null.");
                }
                else
                {
                    if (this.currentRow == -1)
                    {
                        if (this.delimiters.ContainsKey(value))
                        {
                            throw new ArgumentException("Provided symbol is already being used as an open delimiter.");
                        }
                        else
                        {
                            this.columnSeparator = value;
                        }
                    }
                    else
                    {
                        throw new UtilitiesDataException("The data reader was already started.");
                    }
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o símbolo de final de csv.
        /// </summary>
        public SymbType EndOdCsvSymbol
        {
            get
            {
                return this.endOfCsvSymbol;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("End of csv symbol value type can't be null.");
                }
                else
                {
                    if (this.currentRow == -1)
                    {
                        if (this.delimiters.ContainsKey(value))
                        {
                            throw new ArgumentException("Provided symbol is already being used as an open delimiter.");
                        }
                        else
                        {
                            this.endOfCsvSymbol = value;
                        }
                    }
                    else
                    {
                        throw new UtilitiesDataException("The data reader was already started.");
                    }
                }
            }
        }

        /// <summary>
        /// Mapeia o delimitador de abertura ao delimitador de fecho.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        /// <exception cref="ArgumentException">Se pelo menos um argumento for nulo.</exception>
        public void MapDelimiters(SymbType openDelimiter, SymbType closeDelimiter)
        {
            if (openDelimiter == null)
            {
                throw new ArgumentException("An open delimiter must be provided.");
            }
            else if (closeDelimiter == null)
            {
                throw new ArgumentException("A close delimiter must be provided.");
            }
            else if (this.symbolTypesComparer.Equals(
               openDelimiter,
               this.lineSeparator))
            {
                throw new ArgumentException("Open delimiter can't be equal to line separator symbol.");
            }
            else if (this.symbolTypesComparer.Equals(
              openDelimiter,
              this.columnSeparator))
            {
                throw new ArgumentException("Open delimiter can't be equal to column separator symbol.");
            }
            else if (this.symbolTypesComparer.Equals(
              openDelimiter,
              this.endOfCsvSymbol))
            {
                throw new ArgumentException("Open delimiter can't be equal to end of csv symbol.");
            }
            else
            {
                if (this.currentRow == -1)
                {
                    var closedDelimiteres = default(List<SymbType>);
                    if (this.delimiters.TryGetValue(openDelimiter, out closedDelimiteres))
                    {
                        if (!closedDelimiteres.Contains(closeDelimiter))
                        {
                            closedDelimiteres.Add(closeDelimiter);
                        }
                    }
                    else
                    {
                        this.delimiters.Add(openDelimiter, new List<SymbType>() { closeDelimiter });
                    }
                }
                else
                {
                    throw new UtilitiesDataException("The data reader was already started.");
                }
            }
        }

        /// <summary>
        /// Remove um mapeamento entre um delimitador de abertura e o delimitador de fecho.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimtiador de fecho.</param>
        public void UnmapDelimiters(SymbType openDelimiter, SymbType closeDelimiter)
        {

            if (openDelimiter != null &&
                closeDelimiter != null)
            {
                if (this.currentRow == -1)
                {
                    var closedDelimiteres = default(List<SymbType>);
                    if (this.delimiters.TryGetValue(openDelimiter, out closedDelimiteres))
                    {
                        closedDelimiteres.Remove(closeDelimiter);
                        if (closedDelimiteres.Count == 0)
                        {
                            this.delimiters.Remove(openDelimiter);
                        }
                    }
                }
                else
                {
                    throw new UtilitiesDataException("The data reader was already started.");
                }
            }
        }

        /// <summary>
        /// Remove todos os mapeamentos associados a um delimtiador de abertura.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        public void UnmapDelimiters(SymbType openDelimiter)
        {
            if (openDelimiter != null)
            {
                if (this.currentRow == -1)
                {
                    this.delimiters.Remove(openDelimiter);
                }
                else
                {
                    throw new UtilitiesDataException("The data reader was already started.");
                }
            }
        }

        /// <summary>
        /// Elimina todos os delimitadores.
        /// </summary>
        public void ClearDelimiters()
        {
            if (this.currentRow == -1)
            {
                this.delimiters.Clear();
            }
            else
            {
                throw new UtilitiesDataException("The data reader was already started.");
            }
        }

        /// <summary>
        /// Adiciona um tipo de símbolo que deverá ser ignorado durante a leitura.
        /// </summary>
        /// <param name="ignoreType">O tipo de símbolo a ser ignorado.</param>
        /// <exception cref="ArgumentNullException">Se o argumento for nulo.</exception>
        public void AddIgnoreType(SymbType ignoreType)
        {
            if (ignoreType == null)
            {
                throw new ArgumentNullException("ignoreType");
            }
            else
            {
                if (this.currentRow == -1)
                {
                    this.ignoreSymbolTypes.Add(ignoreType);
                }
                else
                {
                    throw new UtilitiesDataException("The data reader was already started.");
                }
            }
        }

        /// <summary>
        /// Remove o tipo de símbolo a ser ignorado.
        /// </summary>
        /// <param name="ignoreType">O tipo de símbolo a ser ignorado.</param>
        public void RemoveIgnoreType(SymbType ignoreType)
        {
            if (this.currentRow == -1)
            {
                this.ignoreSymbolTypes.Remove(ignoreType);
            }
            else
            {
                throw new UtilitiesDataException("The data reader was already started.");
            }
        }

        /// <summary>
        /// Elimina todos os tipos de símbolo a serem ignorados.
        /// </summary>
        public void ClearIgnoreTypes()
        {
            if (this.currentRow == -1)
            {
                this.ignoreSymbolTypes.Clear();
            }
            else
            {
                throw new UtilitiesDataException("The data reader was already started.");
            }
        }

        /// <summary>
        /// Obtém uma tabela que descreve os metadados das colunas, isto é,
        /// cada linha da tabela descreve cada coluna do leitor.
        /// </summary>
        /// <returns>A tabela.</returns>
        public DataTable GetSchemaTable()
        {
            var columnName = "ColumnName";
            var columnOrdinal = "ColumnOrdinal";
            var columnsParserType = "ColumnParserType";

            var metadataTable = new DataTable("SchemaTable");
            metadataTable.Columns.Add(columnName, typeof(string));
            metadataTable.Columns.Add(columnOrdinal, typeof(int));
            metadataTable.Columns.Add(columnsParserType, typeof(Type));

            // Adiciona os metadados associados às colunhas normais
            var parsersLength = this.columnParsers.Length;
            for (int i = 0; i < parsersLength; ++i)
            {
                var currentParser = this.columnParsers[i];
                var row = metadataTable.NewRow();
                row[columnName] = currentParser.Item2;
                row[columnOrdinal] = i;
                row[columnsParserType] = currentParser.Item1.GetType();
                metadataTable.Rows.Add(row);
            }

            var errorRow = metadataTable.NewRow();
            errorRow[columnOrdinal] = parsersLength;
            errorRow[columnsParserType] = null;
            metadataTable.Rows.Add(errorRow);

            return metadataTable;
        }

        /// <summary>
        /// Avança o leitor para o próximo resultado, quando o leitor se encontra
        /// associado a um conjunto de leituras (várias tabelas).
        /// </summary>
        /// <returns>
        /// Verdadeiro caso o leitor tenha sido movido e falso caso contrário.
        /// </returns>
        public bool NextResult()
        {
            throw new NotSupportedException("Only one result may be retrieved from a csv data reader.");
        }

        /// <summary>
        /// Efectua a leitura do próximo registo.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso o registo tenha sido efectuado com sucesso e falso caso contrário.
        /// </returns>
        public bool Read()
        {
            if (this.hasEnded)
            {
                return false;
            }
            else if (this.reader.IsAtEOF())
            {
                this.hasEnded = true;
                return false;
            }
            else
            {
                if (this.currentRow == -1)
                {
                    // Inicializa o leitor
                    this.currentRecord = new ElementsType[this.columnParsers.Length];
                    this.logStatus = new LogStatus<string, EParseErrorLevel>();
                }

                var currentReaded = new List<ISymbol<SymbValue, SymbType>>();
                var delimiterStack = new Stack<Tuple<SymbType, List<SymbType>>>();
                var innerStatus = new LogStatus<string, EParseErrorLevel>();
                var length = this.columnParsers.Length;
                var currentColumn = 0;

                // Leitura inicializada
                ++this.currentRow;

                var state = 0;
                var emptyLine = true;
                while (state != -1)
                {
                    if (currentColumn == length)
                    {
                        this.logStatus.AddLog(
                                        string.Format(
                                            "The number of readed columns is greater than the expected columns {0} at line {1}.",
                                            length,
                                            this.currentRow),
                                        EParseErrorLevel.ERROR);
                        var readed = this.reader.Get();
                        while (!this.symbolTypesComparer.Equals(
                            this.lineSeparator,
                            readed.SymbolType) &&
                            !this.symbolTypesComparer.Equals(
                            this.endOfCsvSymbol,
                            readed.SymbolType) &&
                            !this.reader.IsAtEOFSymbol(readed))
                        {
                            readed = this.reader.Get();
                        }

                        state = -1;
                    }
                    else if (state == 0)
                    {
                        var readed = this.reader.Get();
                        if (this.reader.IsAtEOFSymbol(readed))
                        {
                            if (emptyLine)
                            {
                                this.hasEnded = true;
                                return false;
                            }
                            else
                            {
                                innerStatus.ClearLogs();
                                this.SetColumn(currentColumn, currentReaded, innerStatus);
                                ++currentColumn;

                                // Copia os diários
                                this.logStatus.CopyFrom(innerStatus);
                                if (currentColumn < length)
                                {
                                    this.logStatus.AddLog(
                                        string.Format(
                                            "The number of readed columns {0} does not match the number of expected columns {1} at line {2}.",
                                            currentColumn,
                                            length,
                                            this.currentRow),
                                        EParseErrorLevel.ERROR);
                                }
                            }
                        }
                        else if (this.symbolTypesComparer.Equals(
                           readed.SymbolType,
                           this.endOfCsvSymbol))
                        {
                            if (emptyLine)
                            {
                                this.hasEnded = true;
                                return false;
                            }
                            else
                            {
                                innerStatus.ClearLogs();
                                this.SetColumn(currentColumn, currentReaded, innerStatus);
                                ++currentColumn;

                                // Copia os diários
                                this.logStatus.CopyFrom(innerStatus);
                                if (currentColumn < length)
                                {
                                    this.logStatus.AddLog(
                                        string.Format(
                                            "The number of readed columns {0} does not match the number of expected columns {1} at line {2}.",
                                            currentColumn,
                                            length,
                                            this.currentRow),
                                        EParseErrorLevel.ERROR);
                                }

                                state = -1;
                            }
                        }
                        else if (this.symbolTypesComparer.Equals(
                           readed.SymbolType,
                           this.lineSeparator))
                        {
                            if (emptyLine)
                            {
                                this.logStatus.AddLog(
                                    string.Format("Empty line: {0}", this.currentRow),
                                    EParseErrorLevel.ERROR);
                                return true;
                            }
                            else
                            {
                                if (currentColumn < length)
                                {
                                    innerStatus.ClearLogs();
                                    this.SetColumn(currentColumn, currentReaded, innerStatus);

                                    // Copia os diários
                                    this.logStatus.CopyFrom(innerStatus);
                                    if (currentColumn < length)
                                    {
                                        this.logStatus.AddLog(
                                            string.Format(
                                                "The number of readed columns {0} does not match the number of expected columns {1} at line {2}.",
                                                currentColumn,
                                                length,
                                                this.currentRow),
                                            EParseErrorLevel.ERROR);
                                    }
                                }
                                else
                                {
                                    this.logStatus.AddLog(
                                            string.Format(
                                                "The number of readed columns {0} does not match the number of expected columns {1} at line {2}.",
                                                currentColumn,
                                                length,
                                                this.currentRow),
                                            EParseErrorLevel.ERROR);
                                }

                                state = -1;
                            }
                        }
                        else if (this.symbolTypesComparer.Equals(
                           readed.SymbolType,
                           this.columnSeparator))
                        {
                            emptyLine = false;
                            innerStatus.ClearLogs();
                            this.SetColumn(currentColumn, currentReaded, innerStatus);
                            ++currentColumn;
                            this.logStatus.CopyFrom(innerStatus);
                            currentReaded.Clear();
                        }
                        else
                        {
                            var closeDelimiters = default(List<SymbType>);
                            if (this.delimiters.TryGetValue(readed.SymbolType, out closeDelimiters))
                            {
                                currentReaded.Add(readed);
                                delimiterStack.Push(Tuple.Create(readed.SymbolType, closeDelimiters));
                                emptyLine = false;
                                state = 1;
                            }
                            else if (!this.ignoreSymbolTypes.Contains(
                               readed.SymbolType,
                               this.symbolTypesComparer))
                            {
                                currentReaded.Add(readed);
                                emptyLine = false;
                            }
                        }
                    } // state == 0
                    else if (state == 1) // Encontra-se no meio da leitura entre delimitadores
                    {
                        var readed = this.reader.Get();
                        if (this.reader.IsAtEOFSymbol(readed))
                        {
                            innerStatus.ClearLogs();
                            this.SetColumn(currentColumn, currentReaded, innerStatus);
                            ++currentColumn;

                            // Copia os diários
                            this.logStatus.CopyFrom(innerStatus);
                            if (currentColumn < length)
                            {
                                this.logStatus.AddLog(
                                    string.Format(
                                        "The number of readed columns {0} does not match the number of expected columns {1} at line {2}.",
                                        currentColumn,
                                        length,
                                        this.currentRow),
                                    EParseErrorLevel.ERROR);
                            }
                        }
                        else
                        {
                            var top = delimiterStack.Pop();
                            if (top.Item2.Contains(readed.SymbolType, this.symbolTypesComparer))
                            {
                                currentReaded.Add(readed);
                                if (delimiterStack.Count == 0)
                                {
                                    state = 0;
                                }
                            }
                            else
                            {
                                delimiterStack.Push(top);
                                var closeDelimiters = default(List<SymbType>);
                                if (this.delimiters.TryGetValue(readed.SymbolType, out closeDelimiters))
                                {
                                    currentReaded.Add(readed);
                                    delimiterStack.Push(Tuple.Create(readed.SymbolType, closeDelimiters));
                                }
                                else if (!this.ignoreSymbolTypes.Contains(
                                   readed.SymbolType,
                                   this.symbolTypesComparer))
                                {
                                    currentReaded.Add(readed);
                                }
                            }
                        }
                    } // state == 1
                }

                return true;
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corrente como valor lógico.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor.</returns>
        public bool GetBoolean(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (bool)Convert.ChangeType(currentValue, TypeCode.Boolean);
            }
        }

        /// <summary>
        /// Otbém um "byte" de 8 "bits" a partir do registo corrente.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns></returns>
        public byte GetByte(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length )
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (byte)Convert.ChangeType(currentValue, TypeCode.Byte);
            }
        }

        /// <summary>
        /// Efectua a leItUra de um fluxo de "bytes" a partir de um deslocamento no valor
        /// da coluna e coloca o resultado no amortecedor, iniciando a escrita no
        /// deslocamento especificado.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <param name="fieldOffset">O deslocamento de leitura no valor da coluna.</param>
        /// <param name="buffer">O amortecedor de saída.</param>
        /// <param name="bufferoffset">O deslocamento de escrita no amortecedor de saída.</param>
        /// <param name="length">O número de "bytes" a serem lidos.</param>
        /// <returns>O número real de "bytes" lidos.</returns>
        public long GetBytes(
            int i,
            long fieldOffset,
            byte[] buffer,
            int bufferoffset,
            int length)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o valor da coluna como carácter do registo corrente.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O carácter.</returns>
        public char GetChar(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length - 1)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (char)Convert.ChangeType(currentValue, TypeCode.Char);
            }
        }

        /// <summary>
        /// Efectua a leitura de um fluxo de carácteres a partir de um deslocamento
        /// no valor da coluna e coloca o resultado num amortecedor, iniciando a escrita
        /// no deslocamento especificado.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <param name="fieldoffset">O deslocamento de leitura no valor da coluna.</param>
        /// <param name="buffer">O amortecedor de saída.</param>
        /// <param name="bufferoffset">O deslocamento de escrita no amortecedor de saída.</param>
        /// <param name="length">O número de carácteres a serem lidos.</param>
        /// <returns>O número de carácteres lidos.</returns>
        public long GetChars(
            int i,
            long fieldoffset,
            char[] buffer,
            int bufferoffset,
            int length)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém um <see cref="IDataReader"/> para a coluna espeficicada do registo corrente.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O <see cref="IDataReader"/>.</returns>
        public IDataReader GetData(int i)
        {
            throw new NotSupportedException("This feature isn't supported yet.");
        }

        /// <summary>
        /// Obtém a informação dos tipos para a coluna em questão do registo corrente.
        /// </summary>
        /// <param name="i">O índice do campo.</param>
        /// <returns>A informação do tipo.</returns>
        public string GetDataTypeName(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                if (currentValue == null)
                {
                    throw new UtilitiesDataException("Can't get type name from a null value.");
                }
                else
                {
                    return currentValue.GetType().Name;
                }
            }
        }

        /// <summary>
        /// Otbém o valor da coluna como data do registo corrente.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public DateTime GetDateTime(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (DateTime)Convert.ChangeType(currentValue, TypeCode.DateTime);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna como decimal do registo corrente.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public decimal GetDecimal(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (decimal)Convert.ChangeType(currentValue, TypeCode.Decimal);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corente como sendo um número
        /// de precisão dupla.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public double GetDouble(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (double)Convert.ChangeType(currentValue, TypeCode.Double);
            }
        }

        /// <summary>
        /// Obtém o tipo do valor da coluna especificada do registo corrente.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O tipo do valor da coluna.</returns>
        public Type GetFieldType(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                return typeof(ElementsType);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corente como sendo um número
        /// de precisão simples.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public float GetFloat(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (float)Convert.ChangeType(currentValue, TypeCode.Single);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corente como sendo um GUID.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public Guid GetGuid(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (Guid)Convert.ChangeType(currentValue, TypeCode.Object);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corrente como sendo um inteiro pequeno.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public short GetInt16(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (short)Convert.ChangeType(currentValue, TypeCode.Int16);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corrente como sendo um inteiro.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public int GetInt32(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (int)Convert.ChangeType(currentValue, TypeCode.Int32);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corrente como sendo um inteiro longo.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public long GetInt64(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (long)Convert.ChangeType(currentValue, TypeCode.Int64);
            }
        }

        /// <summary>
        /// Obtém o nome da coluna.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O nome da coluna.</returns>
        public string GetName(int i)
        {
            if (i < 0 || i >= this.columnParsers.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else
            {
                return this.columnParsers[i].Item2;
            }
        }

        /// <summary>
        /// Obtém o índice da coluna especificada pelo nome.
        /// </summary>
        /// <param name="name">O nome da coluna.</param>
        /// <returns>O índice da coluna.</returns>
        public int GetOrdinal(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Column name can't be null nor empty.");
            }
            else
            {
                var index = default(int);
                if (this.namesIndexer.TryGetValue(name.Trim(), out index))
                {
                    return index;
                }
                else
                {
                    throw new IndexOutOfRangeException("The name specified is not a valid column name.");
                }
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corrente como sendo texto.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public string GetString(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                return (string)Convert.ChangeType(currentValue, TypeCode.String);
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do registo corrente.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor da coluna.</returns>
        public object GetValue(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                return this.currentRecord[i];
            }
        }

        /// <summary>
        /// Obtém o valor da coluna do tipo especificado.
        /// </summary>
        /// <typeparam name="ObjVal">O tipo do valor.</typeparam>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>O valor.</returns>
        /// <exception cref="UtilitiesDataException">
        /// Se o valor da coluna não for convertível no tipo especificado.
        /// </exception>
        public ObjVal GetValue<ObjVal>(int i)
            where ObjVal : class
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var currentValue = this.currentRecord[i];
                if (typeof(ObjVal).IsAssignableFrom(currentValue.GetType()))
                {
                    return currentValue as ObjVal;
                }
                else
                {
                    throw new InvalidCastException("Can't convert value to the specified type.");
                }
                
            }
        }

        /// <summary>
        /// Popula um vector de objectos com os valores das colunas.
        /// </summary>
        /// <remarks>
        /// Em muitas aplicações, este método permite obter todos os objectos
        /// do registo de uma forma mais efeiciente do que iterar cada item indivisualmente.
        /// Se o tamanho do vector proporcionado não for suficiente para albergar os valores
        /// de todas as colunas, apenas serão consideradas as primeiras.
        /// </remarks>
        /// <param name="values">O vector de objectos a ser populado.</param>
        /// <returns>O número de instâncias de objectos no vector.</returns>
        public int GetValues(object[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                var length = Math.Min(this.currentRecord.Length, values.Length);
                Array.Copy(this.currentRecord, values, length);
                return length;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a coluna do registo corrente contém um valor nulo.
        /// </summary>
        /// <param name="i">O índice da coluna.</param>
        /// <returns>Verdadeiro caso a coluna contenha um valor nulo e falso caso contrário.</returns>
        public bool IsDBNull(int i)
        {
            if (i < 0 || i >= this.currentRecord.Length)
            {
                throw new ArgumentOutOfRangeException("i");
            }
            else if (this.hasEnded)
            {
                throw new UtilitiesDataException("The reader is already closed.");
            }
            else if (this.currentRow == -1)
            {
                throw new UtilitiesDataException("The reader hasn't already started.");
            }
            else
            {
                return this.currentRecord[i] == null;
            }
        }

        /// <summary>
        /// Obtém os diários associados à leitura da linha actual.
        /// </summary>
        /// <returns>Os diários.</returns>
        public ILogStatus<string, EParseErrorLevel> GetLogStatus()
        {
            return (ILogStatus<string, EParseErrorLevel>)this.logStatus.Clone();
        }

        /// <summary>
        /// Fecha o leitor.
        /// </summary>
        public void Close()
        {
            this.hasEnded = true;
        }

        /// <summary>
        /// Liberta recursos não geridos associados ao leitor.
        /// </summary>
        public void Dispose()
        {
        }

        #region Funções privadas

        /// <summary>
        /// Valida e estabelece os campos proporcionados no construtor.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="columnParsers">O leitor de elementos das colunas.</param>
        /// <param name="lineSeparator">O tipo de dados que define o separador de linhas.</param>
        /// <param name="columnSeparator">O tipo de dados que define o separador de colunas.</param>
        /// <param name="endOfCsvSymbol">O símbolo que indica o término do CSV.</param>
        private void ValidateAndSetFields(
            ISymbolReader<SymbValue, SymbType> reader,
            Tuple<IParse<ElementsType, SymbValue, SymbType>, string>[] columnParsers,
            SymbType lineSeparator,
            SymbType columnSeparator,
            SymbType endOfCsvSymbol)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (columnParsers == null)
            {
                throw new ArgumentNullException("columnParsers");
            }
            else if (lineSeparator == null)
            {
                throw new ArgumentNullException("lineSeparator");
            }
            else if (columnSeparator == null)
            {
                throw new ArgumentNullException("columnSeparator");
            }
            else if (endOfCsvSymbol == null)
            {
                throw new ArgumentNullException("endOfCsvSymbol");
            }
            else
            {
                // Validação do vector de leitores das colunas
                var parsersLength = columnParsers.Length;
                for (int i = 0; i < parsersLength; ++i)
                {
                    var current = columnParsers[i];
                    if (current.Item1 == null)
                    {
                        throw new UtilitiesDataException("Unexpected null value for column reader.");
                    }
                    else if (string.IsNullOrWhiteSpace(current.Item2))
                    {
                        throw new UtilitiesDataException("Column name can't be null nor empty.");
                    }
                }

                this.namesIndexer = new Dictionary<string, int>(
                                    StringComparer.InvariantCultureIgnoreCase);
                for (int i = 0; i < parsersLength; ++i)
                {
                    var current = columnParsers[i];
                    var currentName = current.Item2.Trim();
                    if (this.namesIndexer.ContainsKey(currentName))
                    {
                        throw new UtilitiesDataException("The column names must be unique in definition.");
                    }
                    else
                    {
                        this.namesIndexer.Add(currentName, i);
                    }
                }

                this.columnParsers =
                    new Tuple<IParse<ElementsType, SymbValue, SymbType>, string>[parsersLength];
                Array.Copy(columnParsers, this.columnParsers, columnParsers.Length);

                this.reader = reader;
                this.lineSeparator = lineSeparator;
                this.columnSeparator = columnSeparator;
                this.endOfCsvSymbol = endOfCsvSymbol;
            }
        }

        /// <summary>
        /// Valida os símbolos de separação.
        /// </summary>
        /// <param name="lineSeparator">O símbolo de separação de linha.</param>
        /// <param name="columnSeparator">O símbolo de separação de coluna.</param>
        /// <param name="endOfCsvSymbol">O símbolo de final de csv.</param>
        /// <param name="equalityComparer">O comparador de símbolos.</param>
        private void ValidateSymbols(
            SymbType lineSeparator,
            SymbType columnSeparator,
            SymbType endOfCsvSymbol,
            IEqualityComparer<SymbType> equalityComparer)
        {
            if (equalityComparer.Equals(lineSeparator, columnSeparator))
            {
                throw new ArgumentException("Column separator symbol must be different from line separator symbol");
            }
            else if (equalityComparer.Equals(lineSeparator, endOfCsvSymbol))
            {
                throw new ArgumentException("Line separator symbol must be different from end of csv separator symbo.");
            }
            else if (equalityComparer.Equals(columnSeparator, endOfCsvSymbol))
            {
                throw new ArgumentException("Column separator symbol must be different from end of csv separator symbol.");
            }
        }

        /// <summary>
        /// Estabelece o valor da coluna.
        /// </summary>
        /// <param name="columnNumber">O número da coluna.</param>
        /// <param name="readedSymbols">O conjunto lido de símbolos.</param>
        /// <param name="innerStatus">O estado interno.</param>
        private void SetColumn(
            int columnNumber,
            List<ISymbol<SymbValue, SymbType>> readedSymbols,
            ILogStatus<string, EParseErrorLevel> innerStatus)
        {
            var currentParser = this.columnParsers[columnNumber];
            this.currentRecord[columnNumber] = currentParser.Item1.Parse(
                readedSymbols.ToArray(),
                this.logStatus);
            if (innerStatus.HasLogs(EParseErrorLevel.ERROR))
            {
                this.logStatus.AddLog(
                    string.Format("Found error in row {0}, column {1}.", this.currentRow, columnNumber),
                    EParseErrorLevel.ERROR);
            }
        }

        #endregion Funções privadas
    }
}
