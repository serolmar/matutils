namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    public class CsvFileParser<InputType, SymbValue, SymbType>
    {
        /// <summary>
        /// O separador de linha.
        /// </summary>
        private SymbType lineSeparator;

        /// <summary>
        /// O separador de coluna.
        /// </summary>
        private SymbType columnSeparator;

        /// <summary>
        /// Os delimitadores.
        /// </summary>
        private Dictionary<SymbType, List<SymbType>> delimiters = new Dictionary<SymbType, List<SymbType>>();

        /// <summary>
        /// Lista com todos os estados necessários para processar o csv.
        /// </summary>
        private List<IState<InputType, SymbValue, SymbType>> states;

        /// <summary>
        /// Referência para a tabela a ser carregada.
        /// </summary>
        private ITabularItem currentTable;

        /// <summary>
        /// A linha corrente.
        /// </summary>
        private List<object> currentRow = new List<object>();

        /// <summary>
        /// O elemento corrente.
        /// </summary>
        private string currentElement = string.Empty;

        /// <summary>
        /// A linha actual.
        /// </summary>
        private int currentRowNumber = 0;

        /// <summary>
        /// A coluna actual.
        /// </summary>
        private int currentColumnNumber = 0;

        /// <summary>
        /// O valor actual lido.
        /// </summary>
        private string currentReadedValue = string.Empty;

        public CsvFileParser(
            SymbType lineSeparator,
            SymbType columnSeparator)
        {
            this.lineSeparator = lineSeparator;
            this.columnSeparator = columnSeparator;
            this.InitStates();
        }

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

        public void ClearDelimiters()
        {
            this.delimiters.Clear();
        }

        public ITabularItem Parse(SymbolReader<InputType, SymbValue, SymbType> reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else
            {
                this.currentTable = new TabularListsItem();
                this.currentRow.Clear();
                this.currentElement = string.Empty;

                var machine =
                    new StateMachine<InputType, SymbValue, SymbType>(
                    this.states[0],
                    this.states[1]);
                machine.RunMachine(reader);

                return this.currentTable;
            }
        }

        /// <summary>
        /// Inicia os estados associados às transições.
        /// </summary>
        private void InitStates()
        {
            this.states = new List<IState<InputType, SymbValue, SymbType>>();
            this.states.Add(new DelegateStateImplementation<InputType, SymbValue, SymbType>(0, "start", this.StartTransition));
            this.states.Add(new DelegateStateImplementation<InputType, SymbValue, SymbType>(1, "end", this.EndTransition));
            this.states.Add(new DelegateStateImplementation<InputType, SymbValue, SymbType>(2, "reading", this.ReadingTransition));
            this.states.Add(new DelegateStateImplementation<InputType, SymbValue, SymbType>(3, "delimiters", this.InsideDelimitersTransition));
        }

        private IState<InputType, SymbValue, SymbType> StartTransition(SymbolReader<InputType, SymbValue, SymbType> reader)
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
                    this.currentTable.Add(this.currentRow);
                    ++this.currentRowNumber;
                    return this.states[2];
                }
                else if (symbol.SymbolType.Equals(this.columnSeparator))
                {
                    this.currentRow.Add(currentElement);
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
                    this.currentElement += symbol.SymbolValue;
                    return this.states[2];
                }
            }
        }

        private IState<InputType, SymbValue, SymbType> EndTransition(SymbolReader<InputType, SymbValue, SymbType> reader)
        {
            return null;
        }

        private IState<InputType, SymbValue, SymbType> ReadingTransition(SymbolReader<InputType, SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.currentRow.Add(this.currentElement);
                this.currentTable.Add(this.currentRow);
                return this.states[1];
            }
            else
            {
                var symbol = reader.Get();
                if (symbol.SymbolType.Equals(this.lineSeparator))
                {
                    this.currentRow.Add(this.currentElement);
                    this.currentElement = string.Empty;
                    this.currentTable.Add(this.currentRow);
                    this.currentRow.Clear();
                    ++this.currentRowNumber;
                    this.currentColumnNumber = 0;
                    return this.states[2];
                }
                else if (symbol.SymbolType.Equals(this.columnSeparator))
                {
                    this.currentRow.Add(currentElement);
                    this.currentElement = string.Empty;
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
                    this.currentElement += symbol.SymbolValue;
                    return this.states[2];
                }
            }
        }

        private IState<InputType, SymbValue, SymbType> InsideDelimitersTransition(SymbolReader<InputType, SymbValue, SymbType> reader)
        {
            var symbolsStack = new Stack<SymbType>();
            var symbol = reader.Get();
            symbolsStack.Push(symbol.SymbolType);
            this.currentElement += symbol.SymbolValue;
            while (symbolsStack.Count > 0)
            {
                var topValue = symbolsStack.Pop();
                if (reader.IsAtEOF())
                {
                    throw new NotImplementedException();
                }
                else
                {
                    symbol = reader.Get();
                    var close = this.delimiters[topValue];
                    this.currentElement += symbol.SymbolValue;
                    if (!close.Contains(symbol.SymbolType))
                    {
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
