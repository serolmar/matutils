namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Implementa um leitor de alcances multidimensionais no qual está definida a configuração pretendida.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    public class RangeConfigReader<T, SymbValue, SymbType>
        : ARangeReader<T, SymbValue, SymbType>
    {
        /// <summary>
        /// O leitor de elementos.
        /// </summary>
        private IParse<T, SymbValue, SymbType> parser;

        /// <summary>
        /// A configuração actual.
        /// </summary>
        private List<int> currentReadedConfiguration = new List<int>();

        /// <summary>
        /// O contentor para os elementos lidos.
        /// </summary>
        private List<T> readedElements;

        /// <summary>
        /// A configuração final.
        /// </summary>
        private int[] finalConfiguration;

        /// <summary>
        /// O nível no qual se encontra o leitor.
        /// </summary>
        private int level;

        /// <summary>
        /// A pilha de operadores.
        /// </summary>
        private Stack<SymbType> opsStack = new Stack<SymbType>();

        /// <summary>
        /// A lista de estados definidos no leitor.
        /// </summary>
        private List<IState<SymbValue, SymbType>> stateList = new List<IState<SymbValue, SymbType>>();

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="RangeConfigReader{T, SymbValue, SymbType}"/>.
        /// </summary>
        /// <param name="finalConfiguration">A configuração pretendida.</param>
        /// <exception cref="ExpressionReaderException">Se a configuração final for nula.</exception>
        /// <exception cref="IndexOutOfRangeException">Se algum índice da configuração for negativo.</exception>
        public RangeConfigReader(int[] finalConfiguration)
        {
            if (finalConfiguration == null)
            {
                throw new ExpressionReaderException("Parameter range can't be null.");
            }

            for (int i = 0; i < finalConfiguration.Length; ++i)
            {
                if (finalConfiguration[i] < 0)
                {
                    throw new IndexOutOfRangeException("Configuration values must be non-negative.");
                }
            }

            this.finalConfiguration = finalConfiguration;
            this.InitStates();
        }

        /// <summary>
        /// Efectua leitura do alcance multidimensional.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O leitor de objectos.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        protected override void InnerReadRangeValues(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }
            else
            {
                this.parser = parser;
                this.readedElements = new List<T>();
                this.currentReadedConfiguration.Clear();
                this.RunStateMchine(reader);
            }
        }

        /// <summary>
        /// Obtém a configuração após uma leitura.
        /// </summary>
        /// <returns>A configuração.</returns>
        protected override IEnumerable<int> GetFinalCofiguration()
        {
            return this.finalConfiguration;
        }

        /// <summary>
        /// Obtém os elmentos lidos após a leitura.
        /// </summary>
        /// <returns>Os elementos lidos.</returns>
        protected override ReadOnlyCollection<T> GetElements()
        {
            return this.readedElements.AsReadOnly();
        }

        /// <summary>
        /// Inicia a máquina de estado responsável por realizar a leitura.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        private void RunStateMchine(IMementoSymbolReader<SymbValue, SymbType> reader)
        {
            var stateMchine = new StateMachine<SymbValue, SymbType>(stateList[0], stateList[1]);
            stateMchine.RunMachine(reader);
        }

        /// <summary>
        /// Remete o leitor ao estado inicial.
        /// </summary>
        private void Reset()
        {
            this.opsStack.Clear();
        }

        /// <summary>
        /// Inicializa os estados da máquina de estados.
        /// </summary>
        private void InitStates()
        {
            this.stateList.Clear();
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(0, "start", this.StartTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(1, "end", this.EndTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(2, "sequence", this.SequenceTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(3, "element", this.ElementTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(4, "inside", this.InsideElementDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(5, "operator", this.OperatorTransition));
        }

        /// <summary>
        /// Ignora todos os símbolos vazios consecutivos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        private void IgnoreVoids(ISymbolReader<SymbValue, SymbType> reader)
        {
            var symbol = reader.Peek();
            while (this.blancks.Contains(symbol.SymbolType))
            {
                reader.Get();
                symbol = reader.Peek();
            }
        }

        #region Transition Functions

        /// <summary>
        /// Define a transição inicial.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> StartTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.errorMessages.Add("Expecting open delimiter but found end of expression.");
                this.hasErrors = true;
                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Get();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.opsStack.Push(readedSymbol.SymbolType);
                    this.level = 0;
                    this.currentReadedConfiguration.Add(0);
                    return this.stateList[2];
                }
                else
                {
                    this.errorMessages.Add("Expecting open delimiter at the begining of expression.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
            }

        }

        /// <summary>
        /// Define a transição final.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> EndTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            return null;
        }

        /// <summary>
        /// Define a transição de sequência.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> SequenceTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                if (opsStack.Count != 0)
                {
                    this.errorMessages.Add("Delimiters mismatch.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }

                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Get();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    if (this.level == this.finalConfiguration.Length - 1)
                    {
                        if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                        {
                            return this.stateList[4];
                        }
                        else
                        {
                            this.errorMessages.Add("Expression doesn't match range dimensions.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                    }
                    else
                    {
                        ++this.level;
                        if (this.level < this.currentReadedConfiguration.Count)
                        {
                            this.currentReadedConfiguration[this.level] = 0;
                        }
                        else
                        {
                            this.currentReadedConfiguration.Add(0);
                        }

                        this.opsStack.Push(readedSymbol.SymbolType);
                        return this.stateList[2];
                    }
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.errorMessages.Add("Unexpected close delimiter.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    return this.stateList[4];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.errorMessages.Add("Unexpected close delimiter.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (readedSymbol.SymbolType.Equals(separatorSymb))
                {
                    this.errorMessages.Add("Unexpected separator symbol.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else
                {
                    if (this.level == this.finalConfiguration.Length - 1)
                    {
                        this.currentElementSymbols.Clear();
                        while (!this.separatorSymb.Equals(readedSymbol.SymbolType) &&
                            !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType) &&
                            !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                        {
                            this.currentElementSymbols.Add(readedSymbol);
                            readedSymbol = reader.Get();
                        }

                        var error = new LogStatus<string, EParseErrorLevel>();
                        var currentElement = this.parser.Parse(this.currentElementSymbols.ToArray(), error);
                        if (error.HasLogs(EParseErrorLevel.ERROR))
                        {
                            this.errorMessages.Add("Can't parse object value.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                        else
                        {
                            this.readedElements.Add(currentElement);
                            reader.UnGet();
                            return this.stateList[3];
                        }
                    }
                    else
                    {
                        this.errorMessages.Add("Expression doesn't match range dimensions.");
                        this.hasErrors = true;
                        return this.stateList[1];
                    }
                }
            }
        }

        /// <summary>
        /// Define a transição de elemento.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> ElementTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                if (this.opsStack.Count == 0)
                {
                    return this.stateList[1];
                }
                else
                {
                    this.errorMessages.Add("Parenthesis mismatch.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Get();
                if (this.opsStack.Count == 0)
                {
                    this.errorMessages.Add("Unexpected symbol after final close delimiter.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    var topOperator = this.opsStack.Pop();
                    if (!this.mapInternalOpenDelimitersToCloseDelimitersTypes.TargetFor(topOperator).Contains(
                        readedSymbol.SymbolType))
                    {
                        this.errorMessages.Add("Delimiters mismatch.");
                        this.hasErrors = true;
                        return this.stateList[1];
                    }
                    else
                    {
                        ++this.currentReadedConfiguration[this.level];
                        if (this.currentReadedConfiguration[this.level] != this.finalConfiguration[this.finalConfiguration.Length - this.level - 1])
                        {
                            this.errorMessages.Add("The number of readed elements in range doesn't match the initial configuration.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                        else
                        {
                            --this.level;
                            return this.stateList[3];
                        }
                    }
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.errorMessages.Add("Unexpected open delimiter.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.errorMessages.Add("Unexpected open delimiter.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.errorMessages.Add("Unexpected close delimiter.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (readedSymbol.SymbolType.Equals(this.separatorSymb))
                {
                    this.currentReadedConfiguration[this.level]++;
                    if (this.currentReadedConfiguration[this.level] > this.finalConfiguration[this.finalConfiguration.Length - this.level - 1])
                    {
                        this.errorMessages.Add("The number of readed elements in range doesn't match the initial configuration.");
                        this.hasErrors = true;
                        return this.stateList[1];
                    }
                    else
                    {
                        return this.stateList[5];
                    }
                }
                else
                {
                    this.errorMessages.Add("Parse error.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
            }
        }

        /// <summary>
        /// Define a transição no interior de delimitadores externos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> InsideElementDelimitersTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.errorMessages.Add("Expecting open delimiter but found end of expression.");
                this.hasErrors = true;
                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                this.currentElementSymbols.Clear();
                this.currentElementSymbols.Add(readedSymbol);
                var temporaryStack = new Stack<SymbType>();
                temporaryStack.Push(readedSymbol.SymbolType);
                while (temporaryStack.Count > 0)
                {
                    readedSymbol = reader.Get();
                    if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                    {
                        temporaryStack.Push(readedSymbol.SymbolType);
                    }
                    else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                    {
                        temporaryStack.Pop();
                    }

                    this.currentElementSymbols.Add(readedSymbol);
                }

                var error = new LogStatus<string, EParseErrorLevel>();
                var currentElement = this.parser.Parse(this.currentElementSymbols.ToArray(), error);
                if (error.HasLogs(EParseErrorLevel.ERROR))
                {
                    this.errorMessages.Add("Can't parse object value.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else
                {
                    this.readedElements.Add(currentElement);
                    this.currentElementSymbols.Clear();
                    return this.stateList[3];
                }
            }
        }

        /// <summary>
        /// Define a transição de operador.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> OperatorTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.errorMessages.Add("Expecting open delimiter but found end of expression.");
                this.hasErrors = true;
                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Get();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    if (this.level == this.finalConfiguration.Length - 1)
                    {
                        if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                        {
                            return this.stateList[4];
                        }
                        else
                        {
                            this.errorMessages.Add("Expression doesn't match range dimensions.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                    }
                    else
                    {
                        ++this.level;
                        if (this.level < this.currentReadedConfiguration.Count)
                        {
                            this.currentReadedConfiguration[this.level] = 0;
                        }
                        else
                        {
                            this.currentReadedConfiguration.Add(0);
                        }

                        this.opsStack.Push(readedSymbol.SymbolType);
                        return this.stateList[2];
                    }
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    if (this.level == this.finalConfiguration.Length - 1)
                    {
                        if (this.currentReadedConfiguration[this.level] == this.finalConfiguration[this.finalConfiguration.Length - this.level - 1])
                        {
                            this.errorMessages.Add("Invalid range dimensions.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                        else
                        {
                            return this.stateList[4];
                        }
                    }
                    else
                    {
                        this.errorMessages.Add("Unexpected open delimiter after operator within range dimensions.");
                        this.hasErrors = true;
                        return this.stateList[1];
                    }
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.errorMessages.Add("Unexpected close delimiter after separator.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.errorMessages.Add("Unexpected close delimiter after separator.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else if (readedSymbol.SymbolType.Equals(this.separatorSymb))
                {
                    this.errorMessages.Add("Unexpected separator after separator.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else
                {
                    if (this.currentReadedConfiguration[this.level] == this.finalConfiguration[this.finalConfiguration.Length - this.level - 1])
                    {
                        this.errorMessages.Add("Invalid range dimensions.");
                        this.hasErrors = true;
                        return this.stateList[1];
                    }
                    else
                    {
                        this.currentElementSymbols.Clear();
                        while (!this.separatorSymb.Equals(readedSymbol.SymbolType) &&
                            !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType) &&
                            !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                        {
                            this.currentElementSymbols.Add(readedSymbol);
                            readedSymbol = reader.Get();
                        }

                        var error = new LogStatus<string, EParseErrorLevel>();
                        var currentElement = this.parser.Parse(this.currentElementSymbols.ToArray(), error);
                        if (error.HasLogs(EParseErrorLevel.ERROR))
                        {
                            this.errorMessages.Add("Can't parse object value.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                        else
                        {
                            this.readedElements.Add(currentElement);
                            reader.UnGet();
                            return this.stateList[3];
                        }
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Implementa um leitor de alcances multidimensionais no qual não está definida a configuração pretendida.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    public class RangeNoConfigReader<T, SymbValue, SymbType> : ARangeReader<T, SymbValue, SymbType>
    {
        /// <summary>
        /// O leitor de elementos.
        /// </summary>
        private IParse<T, SymbValue, SymbType> parser;

        /// <summary>
        /// A configuração actual.
        /// </summary>
        private List<int> currentReadedConfiguration = new List<int>();

        /// <summary>
        /// O contentor para os elementos lidos.
        /// </summary>
        private List<T> readedElements;

        /// <summary>
        /// A configuração final.
        /// </summary>
        private List<int> finalConfiguration = new List<int>();

        /// <summary>
        /// O nível no qual se encontra o leitor.
        /// </summary>
        private int level;

        /// <summary>
        /// A pilha de operadores.
        /// </summary>
        private Stack<SymbType> opsStack = new Stack<SymbType>();

        /// <summary>
        /// A pilha de memorizadores.
        /// </summary>
        private Stack<RangeReaderMementoManager> mementoStack = new Stack<RangeReaderMementoManager>();

        /// <summary>
        /// A lista de estados definidos no leitor.
        /// </summary>
        private List<IState<SymbValue, SymbType>> stateList = new List<IState<SymbValue, SymbType>>();

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="RangeNoConfigReader{T, SymbValue, SymbType}"/>.
        /// </summary>
        public RangeNoConfigReader()
        {
            this.InitStates();
        }

        /// <summary>
        /// Efectua leitura do alcance multidimensional.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O leitor de objectos.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        protected override void InnerReadRangeValues(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }
            else
            {
                this.parser = parser;
                this.finalConfiguration.Clear();
                this.readedElements = new List<T>();
                this.RunStateMchine(reader);
                this.finalConfiguration.RemoveAt(this.finalConfiguration.Count - 1);
            }
        }

        /// <summary>
        /// Obtém a configuração após uma leitura.
        /// </summary>
        /// <returns>A configuração.</returns>
        protected override IEnumerable<int> GetFinalCofiguration()
        {
            return this.finalConfiguration;
        }

        /// <summary>
        /// Obtém os elmentos lidos após a leitura.
        /// </summary>
        /// <returns>Os elementos lidos.</returns>
        protected override ReadOnlyCollection<T> GetElements()
        {
            return this.readedElements.AsReadOnly();
        }

        /// <summary>
        /// Inicia a máquina de estado responsável por realizar a leitura.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        private void RunStateMchine(IMementoSymbolReader<SymbValue, SymbType> reader)
        {
            var stateMchine = new StateMachine<SymbValue, SymbType>(stateList[0], stateList[1]);
            stateMchine.RunMachine(reader);
        }

        /// <summary>
        /// Remete o leitor ao estado inicial.
        /// </summary>
        private void Reset()
        {
            this.opsStack.Clear();
        }

        /// <summary>
        /// Inicializa os estados da máquina de estados.
        /// </summary>
        private void InitStates()
        {
            this.stateList.Clear();
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(0, "start", this.StartTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(1, "end", this.EndTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(2, "sequence", this.SequenceTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(3, "element", this.ElementTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(4, "inside", this.InsideElementDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(5, "operator", this.OperatorTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(6, "resume sequence", this.ResumeSequenceTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(7, "after start", this.AfterStartTransition));
        }

        /// <summary>
        /// Define a transição inicial.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> StartTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.hasErrors = true;
                this.errorMessages.Add("Expecting open delimiter but found end of expression.");
                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Get();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.opsStack.Push(readedSymbol.SymbolType);
                    this.level = 0;
                    this.currentReadedConfiguration.Add(0);
                    this.finalConfiguration.Add(-1);
                    return this.stateList[7];
                }
                else
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Expecting open delimiter at the begining of expression.");
                    return this.stateList[1];
                }
            }

        }

        /// <summary>
        /// Define a transição final.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> EndTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            return null;
        }

        /// <summary>
        /// Define a transição de sequência.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> SequenceTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                if (opsStack.Count != 0)
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Delimiters mismatch.");
                    return this.stateList[1];
                }

                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    if (this.level == 0)
                    {
                        if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                        {
                            return this.stateList[4];
                        }
                        else
                        {
                            this.hasErrors = true;
                            this.errorMessages.Add("Expression doesn't match range dimensions.");
                            return this.stateList[1];
                        }
                    }
                    else
                    {
                        --this.level;
                        this.currentReadedConfiguration[this.level] = 0;

                        this.opsStack.Push(readedSymbol.SymbolType);
                        reader.Get();
                        return this.stateList[2];
                    }
                }
                else
                {
                    return this.stateList[6];
                }
            }
        }

        /// <summary>
        /// Define a transição de elemento.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> ElementTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                if (this.opsStack.Count == 0)
                {
                    return this.stateList[1];
                }
                else
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Parenthesis mismatch.");
                    return this.stateList[1];
                }
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                if (this.opsStack.Count == 0)
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected symbol after final close delimiter.");
                    return this.stateList[1];
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    var topOperator = this.opsStack.Pop();
                    if (!this.mapInternalOpenDelimitersToCloseDelimitersTypes.TargetFor(topOperator).Contains(
                        readedSymbol.SymbolType))
                    {
                        this.hasErrors = true;
                        this.errorMessages.Add("Delimiters mismatch.");
                        return this.stateList[1];
                    }
                    else
                    {
                        ++this.currentReadedConfiguration[this.level];
                        if (this.finalConfiguration[this.level] == -1)
                        {
                            this.finalConfiguration[this.level] = this.currentReadedConfiguration[this.level];
                            ++this.level;
                            if (this.level == this.currentReadedConfiguration.Count)
                            {
                                this.currentReadedConfiguration.Add(0);
                                this.finalConfiguration.Add(-1);
                            }

                            reader.Get();
                            return this.stateList[3];
                        }
                        else if (this.currentReadedConfiguration[this.level] != this.finalConfiguration[this.level])
                        {
                            if (this.mementoStack.Count > 0)
                            {
                                var topMemento = this.mementoStack.Pop();
                                var count = this.finalConfiguration.Count - 1;
                                while (this.currentReadedConfiguration.Count > topMemento.Level + 1)
                                {
                                    this.currentReadedConfiguration.RemoveAt(this.currentReadedConfiguration.Count - 1);
                                    this.finalConfiguration[count--] = -1;
                                }

                                while (this.readedElements.Count > topMemento.CurrentReadedElements)
                                {
                                    this.readedElements.RemoveAt(this.readedElements.Count - 1);
                                }

                                (reader as IMementoSymbolReader<SymbValue, SymbType>).RestoreToMemento(topMemento.Memento);
                                return this.stateList[6];
                            }
                            else
                            {
                                this.hasErrors = true;
                                this.errorMessages.Add("The number of readed elements in range doesn't match the initial configuration.");
                                return this.stateList[1];
                            }
                        }
                        else
                        {
                            ++this.level;
                            reader.Get();
                            return this.stateList[3];
                        }
                    }
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected open delimiter.");
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected open delimiter.");
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected close delimiter.");
                    return this.stateList[1];
                }
                else if (readedSymbol.SymbolType.Equals(this.separatorSymb))
                {
                    this.currentReadedConfiguration[this.level]++;
                    if (this.finalConfiguration[this.level] != -1 && this.currentReadedConfiguration[this.level] > this.finalConfiguration[this.level])
                    {
                        if (this.mementoStack.Count > 0)
                        {
                            var topMemento = this.mementoStack.Pop();
                            var count = this.finalConfiguration.Count - 1;
                            while (this.currentReadedConfiguration.Count > topMemento.Level + 1)
                            {
                                this.currentReadedConfiguration.RemoveAt(this.currentReadedConfiguration.Count - 1);
                                this.finalConfiguration[count--] = -1;
                            }

                            while (this.readedElements.Count > topMemento.CurrentReadedElements)
                            {
                                this.readedElements.RemoveAt(this.readedElements.Count - 1);
                            }

                            (reader as IMementoSymbolReader<SymbValue, SymbType>).RestoreToMemento(topMemento.Memento);
                            return this.stateList[6];
                        }
                        else
                        {
                            this.hasErrors = true;
                            this.errorMessages.Add("The number of readed elements in range doesn't match the current configuration configuration.");
                            return this.stateList[1];
                        }
                    }
                    else
                    {
                        reader.Get();
                        return this.stateList[5];
                    }
                }
                else
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Parse error.");
                    return this.stateList[1];
                }
            }
        }

        /// <summary>
        /// Define a transição no interior de delimitadores externos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> InsideElementDelimitersTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.errorMessages.Add("Expecting open delimiter but found end of expression.");
                this.hasErrors = true;
                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                this.currentElementSymbols.Clear();
                this.currentElementSymbols.Add(readedSymbol);
                var temporaryStack = new Stack<SymbType>();
                temporaryStack.Push(readedSymbol.SymbolType);
                while (temporaryStack.Count > 0)
                {
                    readedSymbol = reader.Get();
                    if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                    {
                        temporaryStack.Push(readedSymbol.SymbolType);
                    }
                    else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                    {
                        temporaryStack.Pop();
                    }

                    this.currentElementSymbols.Add(readedSymbol);
                }

                var error = new LogStatus<string, EParseErrorLevel>();
                var currentElement = this.parser.Parse(this.currentElementSymbols.ToArray(), error);
                if (error.HasLogs(EParseErrorLevel.ERROR))
                {
                    this.errorMessages.Add("Can't parse object value.");
                    this.hasErrors = true;
                    return this.stateList[1];
                }
                else
                {
                    this.readedElements.Add(currentElement);
                    this.currentElementSymbols.Clear();
                    return this.stateList[3];
                }
            }
        }

        /// <summary>
        /// Define a transição de operador.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> OperatorTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.hasErrors = true;
                this.errorMessages.Add("Expecting open delimiter but found end of expression.");
                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    if (this.level == 0)
                    {
                        if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                        {
                            return this.stateList[4];
                        }
                        else
                        {
                            this.hasErrors = true;
                            this.errorMessages.Add("Expression doesn't match range dimensions.");
                            return this.stateList[1];
                        }
                    }
                    else
                    {
                        --this.level;
                        this.currentReadedConfiguration[this.level] = 0;
                        this.opsStack.Push(readedSymbol.SymbolType);

                        reader.Get();
                        return this.stateList[2];
                    }
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    if (this.level == this.finalConfiguration.Count - 1)
                    {
                        if (this.currentReadedConfiguration[this.level] == this.finalConfiguration[this.level])
                        {
                            this.hasErrors = true;
                            this.errorMessages.Add("Invalid range dimensions.");
                            return this.stateList[1];
                        }
                        else
                        {
                            return this.stateList[4];
                        }
                    }
                    else
                    {
                        this.hasErrors = true;
                        this.errorMessages.Add("Unexpected open delimiter after operator within range dimensions.");
                        return this.stateList[1];
                    }
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected close delimiter after separator.");
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected close delimiter after separator.");
                    return this.stateList[1];
                }
                else if (readedSymbol.SymbolType.Equals(this.separatorSymb))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected separator after separator.");
                    return this.stateList[1];
                }
                else
                {
                    if (this.level != 0 || this.currentReadedConfiguration[this.level] == this.finalConfiguration[this.level])
                    {
                        if (this.mementoStack.Count > 0)
                        {
                            var topMemento = this.mementoStack.Pop();
                            var count = this.finalConfiguration.Count - 1;
                            while (this.currentReadedConfiguration.Count > topMemento.Level + 1)
                            {
                                this.currentReadedConfiguration.RemoveAt(this.currentReadedConfiguration.Count - 1);
                                this.finalConfiguration[count--] = -1;
                            }

                            while (this.readedElements.Count > topMemento.CurrentReadedElements)
                            {
                                this.readedElements.RemoveAt(this.readedElements.Count - 1);
                            }

                            (reader as IMementoSymbolReader<SymbValue, SymbType>).RestoreToMemento(topMemento.Memento);
                            return this.stateList[6];
                        }
                        else
                        {
                            this.hasErrors = true;
                            this.errorMessages.Add("Invalid range dimensions.");
                            return this.stateList[1];
                        }
                    }
                    else
                    {
                        this.currentElementSymbols.Clear();
                        readedSymbol = reader.Get();
                        while (!this.separatorSymb.Equals(readedSymbol.SymbolType) &&
                            !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType) &&
                            !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                        {
                            this.currentElementSymbols.Add(readedSymbol);
                            readedSymbol = reader.Get();
                        }

                        var error = new LogStatus<string, EParseErrorLevel>();
                        var currentElement = this.parser.Parse(this.currentElementSymbols.ToArray(), error);
                        if (error.HasLogs(EParseErrorLevel.ERROR))
                        {
                            this.errorMessages.Add("Can't parse object value.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                        else
                        {
                            this.readedElements.Add(currentElement);
                            reader.UnGet();
                            return this.stateList[3];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Define a transição de resumo de sequência.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> ResumeSequenceTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                if (opsStack.Count != 0)
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Delimiters mismatch.");
                }

                return this.stateList[1];
            }
            else
            {
                var readedSymbol = reader.Peek();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected close delimiter.");
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    return this.stateList[4];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected close delimiter.");
                    return this.stateList[1];
                }
                else if (readedSymbol.SymbolType.Equals(separatorSymb))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected separator symbol.");
                    return this.stateList[1];
                }
                else
                {
                    this.currentElementSymbols.Clear();
                    readedSymbol = reader.Get();
                    while (!this.separatorSymb.Equals(readedSymbol.SymbolType) &&
                        !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType) &&
                        !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                    {
                        this.currentElementSymbols.Add(readedSymbol);
                        readedSymbol = reader.Get();
                    }

                    var error = new LogStatus<string, EParseErrorLevel>();
                    var currentElement = this.parser.Parse(this.currentElementSymbols.ToArray(), error);
                    if (error.HasLogs(EParseErrorLevel.ERROR))
                    {
                        this.errorMessages.Add("Can't parse object value.");
                        this.hasErrors = true;
                        return this.stateList[1];
                    }
                    else
                    {
                        this.readedElements.Add(currentElement);
                        reader.UnGet();
                        return this.stateList[3];
                    }
                }
            }
        }

        /// <summary>
        /// Define a transição de após início.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado da máquina de estados.</returns>
        private IState<SymbValue, SymbType> AfterStartTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                this.hasErrors = true;
                this.errorMessages.Add("Expecting open delimiter but found end of expression.");
                return this.stateList[1];
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.opsStack.Push(readedSymbol.SymbolType);
                    var memento = new RangeReaderMementoManager()
                    {
                        Level = this.level,
                        Memento = (reader as IMementoSymbolReader<SymbValue, SymbType>).SaveToMemento(),
                        CurrentReadedElements = this.readedElements.Count
                    };

                    if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                    {
                        this.mementoStack.Push(memento);
                    }

                    reader.Get();
                    return this.stateList[7];
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected close delimiter.");
                    return this.stateList[1];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    return this.stateList[4];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected close delimiter.");
                    return this.stateList[1];
                }
                else if (readedSymbol.SymbolType.Equals(separatorSymb))
                {
                    this.hasErrors = true;
                    this.errorMessages.Add("Unexpected separator symbol.");
                    return this.stateList[1];
                }
                else
                {
                    this.currentElementSymbols.Clear();
                    readedSymbol = reader.Get();
                    while (!this.separatorSymb.Equals(readedSymbol.SymbolType) &&
                        !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType) &&
                        !this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                    {
                        this.currentElementSymbols.Add(readedSymbol);
                        readedSymbol = reader.Get();
                    }

                    var error = new LogStatus<string, EParseErrorLevel>();
                    var currentElement = this.parser.Parse(this.currentElementSymbols.ToArray(), error);
                    if (error.HasLogs(EParseErrorLevel.ERROR))
                    {
                        this.errorMessages.Add("Can't parse object value.");
                        this.hasErrors = true;
                        return this.stateList[1];
                    }
                    else
                    {
                        this.readedElements.Add(currentElement);
                        reader.UnGet();
                        return this.stateList[3];
                    }
                }
            }
        }

        /// <summary>
        /// Ignora todos os símbolos vazios consecutivos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        private void IgnoreVoids(ISymbolReader<SymbValue, SymbType> reader)
        {
            var symbol = reader.Peek();
            while (this.blancks.Contains(symbol.SymbolType))
            {
                reader.Get();
                symbol = reader.Peek();
            }
        }
    }

    /// <summary>
    /// Implementa a base para uma leituro de alcances multidimensionais.
    /// </summary>
    /// <remarks>
    /// A leitura de alcances multidimensionais é realizada sobre um leitor de símbolos que, por sua vez,
    /// assenta sobre um leitor de objectos arbitrários. O leitor de símbolos consiste, portanto, um classficador
    /// e aglomerador de objectos.
    /// </remarks>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    public abstract class ARangeReader<T, SymbValue, SymbType>
    {
        #region Fields

        /// <summary>
        /// Um contentor de símbolos que representam os valores que vão sendo lidos.
        /// </summary>
        protected List<ISymbol<SymbValue, SymbType>> currentElementSymbols = new List<ISymbol<SymbValue, SymbType>>();

        /// <summary>
        /// Contentor para os mapeamentos de símbols de fecho internos a símbolos de abertura.
        /// </summary>
        protected GeneralMapper<SymbType, SymbType> mapInternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        /// <summary>
        /// Contentor para os mapeamentos de símbolos de fecho que externos e símbols de abertura.
        /// </summary>
        protected GeneralMapper<SymbType, SymbType> mapExternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        /// <summary>
        /// O tipo de símbolo que representa um separador.
        /// </summary>
        protected SymbType separatorSymb;

        /// <summary>
        /// Valor que indica se a leitura foi iniciada.
        /// </summary>
        protected bool hasStarted;

        /// <summary>
        /// Valor que indica se a leitura foi bem sucedida ou não.
        /// </summary>
        protected bool hasErrors;

        /// <summary>
        /// O conjunto de mensagens enviadas.
        /// </summary>
        protected List<string> errorMessages;

        /// <summary>
        /// Mantém a lista de símbolos que são ignorados.
        /// </summary>
        protected List<SymbType> blancks = new List<SymbType>();

        #endregion

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ARangeReader{T, SymbValue, SymbType}"/>.
        /// </summary>
        public ARangeReader()
        {
            this.hasErrors = false;
            this.errorMessages = new List<string>();
        }

        /// <summary>
        /// Obtém a configuração lida.
        /// </summary>
        /// <value>
        /// A configuração lida.
        /// </value>
        public IEnumerable<int> Configuration
        {
            get
            {
                return this.GetFinalCofiguration();
            }
        }

        /// <summary>
        /// Obtém os elementos lidos.
        /// </summary>
        /// <value>
        /// Os elementos lidos.
        /// </value>
        public ReadOnlyCollection<T> Elements
        {
            get
            {
                return this.GetElements();
            }
        }

        /// <summary>
        /// Obtém um valor que indica se foram encontrados erros na leitura.
        /// </summary>
        /// <value>
        /// Verdadeiro caso tenham sido encontrados erros e falso caso contrário.
        /// </value>
        public bool HasErrors
        {
            get
            {
                return this.hasErrors;
            }
        }

        /// <summary>
        /// Obtém as mensagens de erro.
        /// </summary>
        /// <value>
        /// As mensagens de erro.
        /// </value>
        public ReadOnlyCollection<string> ErrorMessages
        {
            get
            {
                return this.errorMessages.AsReadOnly();
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o leitor foi iniciado.
        /// </summary>
        /// <value>
        /// Verdadeiro caso o leitor tenha sido iniciado e falso caso contrário.
        /// </value>
        public bool HasStarted
        {
            get
            {
                return this.hasStarted;
            }
        }

        /// <summary>
        /// Obtém e atribui o tipo de símbolo que representa um separador de objectos.
        /// </summary>
        /// <value>
        /// O tipo de separador.
        /// </value>
        /// <exception cref="ArgumentException">Se o tipo de símbolo estiver marcado como vazio.</exception>
        public SymbType SeparatorSymbType
        {
            get
            {
                return this.separatorSymb;
            }
            set
            {
                if (this.blancks.Contains(value))
                {
                    throw new ArgumentException("Can't use a blanck symbol as a separator.");
                }
                else
                {
                    this.separatorSymb = value;
                }
            }
        }

        /// <summary>
        /// Efectua a leitura dos valores.
        /// </summary>
        /// <remarks>
        /// O controlo da leitura dos valores é passado para o leitor de objectos sempre que uma sequência
        /// de símbolos nestas condições é encontrada.
        /// </remarks>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O leitor responsável pela leitura dos valores.</param>
        /// <exception cref="UtilitiesException">
        /// Se nenhum leitor de objectos for providenciado, nenhum delimitador interno estiver definido ou se nenhum
        /// separador de objectos estiver definido.
        /// </exception>
        public void ReadRangeValues(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser)
        {
            if (this.hasStarted)
            {
                throw new UtilitiesException("Reader has already been started.");
            }
            else
            {
                try
                {
                    if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.Objects.Count == 0)
                    {
                        throw new UtilitiesException("No internal delimiter symbols were provided.");
                    }
                    else if (this.separatorSymb == null)
                    {
                        throw new UtilitiesException("No separator symbol was provided.");
                    }
                    else
                    {
                        this.hasStarted = true;
                        this.errorMessages.Clear();
                        this.hasErrors = false;
                        this.InnerReadRangeValues(reader, parser);
                        this.hasStarted = false;
                    }
                }
                catch (Exception exception)
                {
                    this.hasStarted = false;
                    this.hasErrors = true;
                    throw exception;
                }
            }
        }

        #region Public Methods

        /// <summary>
        /// Mapeia um símbolo interno de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbolType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        /// <exception cref="ExpressionReaderException">
        /// Se algum dos tipos de símbolos proporcionados forem marcados como símbolo vazio.
        /// </exception>
        public void MapInternalDelimiters(SymbType openSymbolType, SymbType closeSymbType)
        {
            if (this.blancks.Contains(openSymbolType) || this.blancks.Contains(closeSymbType))
            {
                throw new ExpressionReaderException(
                    "Can't mark a blanck symbol as a delimiter type. Please remove symbol from blancks before mark it as a delimiter.");
            }
            else
            {
                this.mapInternalOpenDelimitersToCloseDelimitersTypes.Add(openSymbolType, closeSymbType);
            }
        }

        /// <summary>
        /// Mapeia um símbolo externo de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        /// <exception cref="ExpressionReaderException">
        /// Se algum dos tipos de símbolos proporcionados forem marcados como símbolo vazio.
        /// </exception>
        public void MapExternalDelimiters(SymbType openSymbType, SymbType closeSymbType)
        {
            if (this.blancks.Contains(openSymbType) || this.blancks.Contains(closeSymbType))
            {
                throw new ExpressionReaderException("Can't mark a blanck symbol as a delimiter type. Please remove symbol from blancks before mark it as a delimiter.");
            }
            else
            {
                this.mapExternalOpenDelimitersToCloseDelimitersTypes.Add(openSymbType, closeSymbType);
            }
        }

        /// <summary>
        /// Marca um tipo de símbolo como sendo vazio.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        /// <exception cref="ExpressionReaderException">Se o símbolo for um delimitador ou um separador.</exception>
        public void AddBlanckSymbolType(SymbType symbolType)
        {
            if (symbolType != null)
            {
                if (symbolType.Equals(this.separatorSymb))
                {
                    throw new ExpressionReaderException("Can't mark the separator as a blank symbol.");
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(symbolType) || this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(symbolType))
                {
                    throw new ExpressionReaderException("Can't mark a delimiter as a blank symbol.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(symbolType) || this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(symbolType))
                {
                    throw new ExpressionReaderException("Can't mark a delimiter as a blank symbol.");
                }

                if (!this.blancks.Contains(symbolType))
                {
                    this.blancks.Add(symbolType);
                }
            }
        }

        /// <summary>
        /// Remove o tipo de símbolo especificado da lista de símbolos vazios.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void RemoveBlanckSymbolType(SymbType symbolType)
        {
            this.blancks.Remove(symbolType);
        }

        /// <summary>
        /// Desmarca todos os símbolos vazios.
        /// </summary>
        public void ClearBlanckSymbols()
        {
            this.blancks.Clear();
        }
        #endregion

        /// <summary>
        /// Efectua leitura do alcance multidimensional.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O leitor de objectos.</param>
        protected abstract void InnerReadRangeValues(IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser);

        /// <summary>
        /// Obtém a configuração após uma leitura.
        /// </summary>
        /// <returns>A configuração.</returns>
        protected abstract IEnumerable<int> GetFinalCofiguration();

        /// <summary>
        /// Obtém os elmentos lidos após a leitura.
        /// </summary>
        /// <returns>Os elementos lidos.</returns>
        protected abstract ReadOnlyCollection<T> GetElements();
    }

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
            IMementoSymbolReader<SymbValue, SymbType> reader,
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
            ILogStatus<string, EParseErrorLevel> errors,
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

    #region Classes auxiliares

    /// <summary>
    /// Clase responsável por gerenciar os mementos aferentes à leitura
    /// arbitrária de matrizes.
    /// </summary>
    internal class RangeReaderMementoManager
    {
        /// <summary>
        /// O memorizador.
        /// </summary>
        private IMemento memento;

        /// <summary>
        /// O nível.
        /// </summary>
        private int level;

        /// <summary>
        /// O número de elementos lidos.
        /// </summary>
        private int currentReadedElements;

        /// <summary>
        /// Obtém ou atribui o memorizador.
        /// </summary>
        /// <value>
        /// O memorizador.
        /// </value>
        public IMemento Memento
        {
            get
            {
                return this.memento;
            }
            set
            {
                this.memento = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o nível.
        /// </summary>
        /// <value>
        /// O nível.
        /// </value>
        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o número de elementos lidos.
        /// </summary>
        /// <value>
        /// O número de elementos lidos.
        /// </value>
        public int CurrentReadedElements
        {
            get
            {
                return this.currentReadedElements;
            }
            set
            {
                this.currentReadedElements = value;
            }
        }
    }

    #endregion Classes auxiliares
}
