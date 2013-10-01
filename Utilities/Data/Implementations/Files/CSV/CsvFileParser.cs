namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    internal class CsvFileParser
    {
        /// <summary>
        /// O separador de linha.
        /// </summary>
        private string lineSeparator;

        /// <summary>
        /// O separador de coluna.
        /// </summary>
        private string columnSeparator;

        /// <summary>
        /// Os delimitadores.
        /// </summary>
        private Dictionary<string, string> delimiters;

        /// <summary>
        /// Lista com todos os estados necessários para processar o csv.
        /// </summary>
        private List<IState<CharSymbolReader<string>, string, string>> states;

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
            string lineSeparator,
            string columnSeparator,
            Dictionary<string, string> delimiters)
        {
            this.lineSeparator = lineSeparator;
            this.columnSeparator = columnSeparator;
            this.delimiters = delimiters;
            this.InitStates();
        }

        private ITabularItem Read(TextReader reader)
        {
            this.currentTable = new TabularListsItem();
            this.currentRow.Clear();
            this.currentElement = string.Empty;
            var stringSymbolReader = new StringSymbolReader(reader, false);

            StateMachine<CharSymbolReader<string>, string, string> machine = new StateMachine<CharSymbolReader<string>, string, string>(
                this.states[0],
                this.states[3]);
            machine.RunMachine(stringSymbolReader);

            return this.currentTable;
        }

        private void InitStates()
        {
            this.states = new List<IState<CharSymbolReader<string>, string, string>>();
            this.states.Add(new DelegateStateImplementation<CharSymbolReader<string>, string, string>(0, "start", this.StartTransition));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader<string>, string, string>(1, "end", this.EndTransition));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader<string>, string, string>(2, "reading", this.ReadingTransition));
            this.states.Add(new DelegateStateImplementation<CharSymbolReader<string>, string, string>(3, "delimiters", this.InsideDelimitersTransition));
        }

        private IState<CharSymbolReader<string>, string, string> StartTransition(SymbolReader<CharSymbolReader<string>, string, string> reader)
        {
            var symbol = reader.Get();
            if (symbol.SymbolType == "eof")
            {
                return this.states[1];
            }
            if (symbol.SymbolType == this.lineSeparator)
            {
                this.currentTable.Add(this.currentRow);
                return this.states[2];
            }
            else if (symbol.SymbolType == this.columnSeparator)
            {
                this.currentRow.Add(currentElement);
                return this.states[2];
            }
            else if (this.delimiters.ContainsKey(symbol.SymbolType))
            {
                return this.states[3];
            }
            else
            {
            }

            throw new NotImplementedException();
        }

        private IState<CharSymbolReader<string>, string, string> EndTransition(SymbolReader<CharSymbolReader<string>, string, string> reader)
        {
            return null;
        }

        private IState<CharSymbolReader<string>, string, string> ReadingTransition(SymbolReader<CharSymbolReader<string>, string, string> reader)
        {
            var symbol = reader.Get();
            throw new NotImplementedException(); 
        }

        private IState<CharSymbolReader<string>, string, string> InsideDelimitersTransition(SymbolReader<CharSymbolReader<string>, string, string> reader)
        {
            var symbolsStack = new Stack<string>();
            var symbol = reader.Peek();
            this.currentElement += symbol.SymbolValue;
            while (symbolsStack.Count > 0)
            {

            }

            throw new NotImplementedException();
        }
    }
}
