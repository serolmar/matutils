﻿namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class CsvFileParser<MatrixType, ElementsType, InputType, SymbValue, SymbType>
    {
        /// <summary>
        /// O objecto responsável por fazer a sincronização dos processos.
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// Indica se o leitor se encontra a realizar a leitura de outra
        /// estrutura de dados.
        /// </summary>
        private bool hasStarted = false;

        /// <summary>
        /// O separador de linha.
        /// </summary>
        private SymbType lineSeparator;

        /// <summary>
        /// O separador de coluna.
        /// </summary>
        private SymbType columnSeparator;

        /// <summary>
        /// O providenciador de leitores.
        /// </summary>
        private IDataReaderProvider<IParse<ElementsType, SymbValue, SymbType>> provider;

        /// <summary>
        /// Os delimitadores.
        /// </summary>
        private Dictionary<SymbType, List<SymbType>> delimiters = new Dictionary<SymbType, List<SymbType>>();

        /// <summary>
        /// A lista de tipos de símbolos a serem ignorados.
        /// </summary>
        private List<SymbType> ignoreSymbolTypes = new List<SymbType>();

        /// <summary>
        /// Lista com todos os estados necessários para processar o csv.
        /// </summary>
        private List<IState<SymbValue, SymbType>> states;

        /// <summary>
        /// Referência para a tabela a ser carregada.
        /// </summary>
        private MatrixType currentTable;

        /// <summary>
        /// A linha corrente.
        /// </summary>
        private List<ElementsType> currentRow = new List<ElementsType>();

        /// <summary>
        /// O objecto responsável por adicionar um conjunto de elemntos a uma
        /// estrutura tabular de objectos.
        /// </summary>
        private IDataParseAdder<MatrixType, ElementsType> dataAdder;

        /// <summary>
        /// A linha actual.
        /// </summary>
        private int currentRowNumber = 0;

        /// <summary>
        /// A coluna actual.
        /// </summary>
        private int currentColumnNumber = 0;

        /// <summary>
        /// Os símbolos actualmente lidos.
        /// </summary>
        private List<ISymbol<SymbValue, SymbType>> currentSymbolValues = new List<ISymbol<SymbValue, SymbType>>();

        public CsvFileParser(
            SymbType lineSeparator,
            SymbType columnSeparator,
            IDataReaderProvider<IParse<ElementsType, SymbValue, SymbType>> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            else
            {
                this.provider = provider;
                this.lineSeparator = lineSeparator;
                this.columnSeparator = columnSeparator;
                this.InitStates();
            }
        }

        /// <summary>
        /// Mapeia o delimitador de abertura ao delimitador de fecho.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
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
            else
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
        }

        /// <summary>
        /// Remove todos os mapeamentos associados a um delimtiador de abertura.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        public void UnmapDelimiters(SymbType openDelimiter)
        {
            if (openDelimiter != null)
            {
                this.delimiters.Remove(openDelimiter);
            }
        }

        /// <summary>
        /// Elimina todos os delimitadores.
        /// </summary>
        public void ClearDelimiters()
        {
            this.delimiters.Clear();
        }

        /// <summary>
        /// Adiciona um tipo de símbolo que deverá ser ignorado durante a leitura.
        /// </summary>
        /// <param name="ignoreType">O tipo de símbolo a ser ignorado.</param>
        public void AddIgnoreType(SymbType ignoreType)
        {
            if (ignoreType == null)
            {
                throw new ArgumentNullException("ignoreType");
            }
            else
            {
                this.ignoreSymbolTypes.Add(ignoreType);
            }
        }

        /// <summary>
        /// Remove o tipo de símbolo a ser ignorado.
        /// </summary>
        /// <param name="ignoreType">O tipo de símbolo a ser ignorado.</param>
        public void RemoveIgnoreType(SymbType ignoreType)
        {
            this.ignoreSymbolTypes.Remove(ignoreType);
        }

        /// <summary>
        /// Elimina todos os tipos de símbolo a serem ignorados.
        /// </summary>
        public void ClearIgnoreTypes()
        {
            this.ignoreSymbolTypes.Clear();
        }

        public void Parse(
            SymbolReader<InputType, SymbValue, SymbType> reader,
            MatrixType matrix,
            IDataParseAdder<MatrixType, ElementsType> adder)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (adder == null)
            {
                throw new ArgumentNullException("adder");
            }
            else
            {
                lock (this.lockObject)
                {
                    if (this.hasStarted)
                    {
                        throw new UtilitiesDataException("Current parser has already been started.");
                    }
                    else
                    {
                        this.hasStarted = true;
                    }
                }

                this.currentSymbolValues.Clear();
                this.currentRow.Clear();
                this.currentRowNumber = 0;
                this.currentColumnNumber = 0;

                this.dataAdder = adder;
                this.currentTable = matrix;

                var stateMachine = new StateMachine<SymbValue, SymbType>(
                    this.states[0],
                    this.states[1]);
                stateMachine.RunMachine(reader);
                this.hasStarted = false;
            }
        }

        /// <summary>
        /// Inicia os estados associados às transições.
        /// </summary>
        private void InitStates()
        {
            this.states = new List<IState<SymbValue, SymbType>>();
            this.states.Add(new DelegateStateImplementation<SymbValue, SymbType>(0, "start", this.StartTransition));
            this.states.Add(new DelegateStateImplementation<SymbValue, SymbType>(1, "end", this.EndTransition));
            this.states.Add(new DelegateStateImplementation<SymbValue, SymbType>(2, "reading", this.ReadingTransition));
            this.states.Add(new DelegateStateImplementation<SymbValue, SymbType>(3, "delimiters", this.InsideDelimitersTransition));
        }

        private IState<SymbValue, SymbType> StartTransition(IObjectReader<ISymbol<SymbValue, SymbType>> reader)
        {
            if (reader.IsAtEOF())
            {
                return this.states[1];
            }
            else
            {
                var symbol = reader.Get();
                if (symbol.SymbolType.Equals(this.lineSeparator))
                {
                    this.dataAdder.Add(this.currentTable, this.currentRow);
                    ++this.currentRowNumber;
                    return this.states[2];
                }
                else if (symbol.SymbolType.Equals(this.columnSeparator))
                {
                    var dataParser = default(IParse<ElementsType, SymbValue, SymbType>);
                    if (!this.provider.TryGetDataReader(
                        this.currentRowNumber,
                        this.currentColumnNumber,
                        out dataParser))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "No data reader was provided for cell with coords ({0},{1}).",
                            this.currentRowNumber,
                            this.currentRowNumber));
                    }

                    var parsed = default(ElementsType);
                    if (!dataParser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "Can't parse value from cell with coords ({0},{1}) with the provided parser.",
                            this.currentRowNumber,
                            this.currentRowNumber));
                    }

                    this.currentRow.Add(parsed);
                    ++this.currentColumnNumber;
                    return this.states[2];
                }
                else if (this.delimiters.ContainsKey(symbol.SymbolType))
                {
                    reader.UnGet();
                    return this.states[3];
                }
                else
                {
                    if (!this.ignoreSymbolTypes.Contains(symbol.SymbolType))
                    {
                        this.currentSymbolValues.Add(symbol);
                    }

                    return this.states[2];
                }
            }
        }

        private IState<SymbValue, SymbType> EndTransition(IObjectReader<ISymbol<SymbValue, SymbType>> reader)
        {
            return null;
        }

        private IState<SymbValue, SymbType> ReadingTransition(IObjectReader<ISymbol<SymbValue, SymbType>> reader)
        {
            if (reader.IsAtEOF())
            {
                if (this.currentSymbolValues.Count > 0 || this.currentRow.Count > 0)
                {
                    var dataParser = default(IParse<ElementsType, SymbValue, SymbType>);
                    if (!this.provider.TryGetDataReader(
                        this.currentRowNumber,
                        this.currentColumnNumber,
                        out dataParser))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "No data reader was provided for cell with coords ({0},{1}).",
                            this.currentRowNumber,
                            this.currentRowNumber));
                    }

                    var parsed = default(ElementsType);
                    if (!dataParser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "Can't parse value from cell with coords ({0},{1}) with the provided parser.",
                            this.currentRowNumber,
                            this.currentRowNumber));
                    }

                    this.currentRow.Add(parsed);
                    this.dataAdder.Add(this.currentTable, this.currentRow);
                }

                return this.states[1];
            }
            else
            {
                var symbol = reader.Get();
                if (symbol.SymbolType.Equals(this.lineSeparator))
                {
                    var dataParser = default(IParse<ElementsType, SymbValue, SymbType>);
                    if (!this.provider.TryGetDataReader(
                        this.currentRowNumber,
                        this.currentColumnNumber,
                        out dataParser))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "No data reader was provided for cell with coords ({0},{1}).",
                            this.currentRowNumber,
                            this.currentRowNumber));
                    }

                    var parsed = default(ElementsType);
                    if (!dataParser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "Can't parse value from cell with coords ({0},{1}) with the provided parser.",
                            this.currentRowNumber,
                            this.currentColumnNumber));
                    }

                    this.currentRow.Add(parsed);
                    this.currentSymbolValues.Clear();
                    this.dataAdder.Add(this.currentTable, this.currentRow);
                    this.currentRow.Clear();
                    ++this.currentRowNumber;
                    this.currentColumnNumber = 0;
                    return this.states[2];
                }
                else if (symbol.SymbolType.Equals(this.columnSeparator))
                {
                    var dataParser = default(IParse<ElementsType, SymbValue, SymbType>);
                    if (!this.provider.TryGetDataReader(
                        this.currentRowNumber,
                        this.currentColumnNumber,
                        out dataParser))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "No data reader was provided for cell with coords ({0},{1}).",
                            this.currentRowNumber,
                            this.currentColumnNumber));
                    }

                    var parsed = default(ElementsType);
                    if (!dataParser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "Can't parse value from cell with coords ({0},{1}) with the provided parser.",
                            this.currentRowNumber,
                            this.currentColumnNumber));
                    }

                    this.currentRow.Add(parsed);
                    this.currentSymbolValues.Clear();
                    ++this.currentColumnNumber;
                    return this.states[2];
                }
                else if (this.delimiters.ContainsKey(symbol.SymbolType))
                {
                    reader.UnGet();
                    return this.states[3];
                }
                else
                {
                    if (!this.ignoreSymbolTypes.Contains(symbol.SymbolType))
                    {
                        this.currentSymbolValues.Add(symbol);
                    }

                    return this.states[2];
                }
            }
        }

        private IState<SymbValue, SymbType> InsideDelimitersTransition(IObjectReader<ISymbol<SymbValue, SymbType>> reader)
        {
            var symbolsStack = new Stack<SymbType>();
            var symbol = reader.Get();
            symbolsStack.Push(symbol.SymbolType);
            this.currentSymbolValues.Add(symbol);
            while (symbolsStack.Count > 0)
            {
                var topValue = symbolsStack.Pop();
                if (reader.IsAtEOF())
                {
                    var dataParser = default(IParse<ElementsType, SymbValue, SymbType>);
                    if (!this.provider.TryGetDataReader(
                        this.currentRowNumber,
                        this.currentColumnNumber,
                        out dataParser))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "No data reader was provided for cell with coords ({0},{1}).",
                            this.currentRowNumber,
                            this.currentColumnNumber));
                    }

                    var parsed = default(ElementsType);
                    if (!dataParser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                    {
                        throw new UtilitiesDataException(string.Format(
                            "Can't parse value from cell with coords ({0},{1}) with the provided parser.",
                            this.currentRowNumber,
                            this.currentColumnNumber));
                    }

                    this.currentRow.Add(parsed);
                    this.dataAdder.Add(this.currentTable, this.currentRow);
                    return this.states[1];
                }
                else
                {
                    symbol = reader.Get();
                    var close = this.delimiters[topValue];
                    if (close.Contains(symbol.SymbolType))
                    {
                        this.currentSymbolValues.Add(symbol);
                    }
                    else
                    {
                        if (!this.ignoreSymbolTypes.Contains(symbol.SymbolType))
                        {
                            this.currentSymbolValues.Add(symbol);
                        }

                        symbolsStack.Push(topValue);
                        if (this.delimiters.ContainsKey(symbol.SymbolType))
                        {
                            symbolsStack.Push(symbol.SymbolType);
                        }
                    }
                }
            }

            reader.UnGet();
            return this.states[2];
        }
    }
}
