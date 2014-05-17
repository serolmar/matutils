namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de alcances multidimensionais no qual está definida a configuração pretendida.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    /// <typeparam name="InputReader">O tipo do leitor de entrada..</typeparam>
    public class RangeConfigReader<T, SymbValue, SymbType, InputReader> 
        : ARangeReader<T, SymbValue, SymbType, InputReader>
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
        /// Instancia um novo objecto do tipo <see cref="RangeConfigReader{T, SymbValue, SymbType, InputReader}"/>.
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
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
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
        private void RunStateMchine(MementoSymbolReader<InputReader, SymbValue, SymbType> reader)
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
            this.stateList.Add(new DelegateStateImplementation<SymbValue, SymbType>(0, "start", this.StartTransition));
            this.stateList.Add(new DelegateStateImplementation<SymbValue, SymbType>(1, "end", this.EndTransition));
            this.stateList.Add(new DelegateStateImplementation<SymbValue, SymbType>(2, "sequence", this.SequenceTransition));
            this.stateList.Add(new DelegateStateImplementation<SymbValue, SymbType>(3, "element", this.ElementTransition));
            this.stateList.Add(new DelegateStateImplementation<SymbValue, SymbType>(4, "inside", this.InsideElementDelimitersTransition));
            this.stateList.Add(new DelegateStateImplementation<SymbValue, SymbType>(5, "operator", this.OperatorTransition));
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

                        var currentElement = default(T);
                        if (this.parser.TryParse(this.currentElementSymbols.ToArray(), out currentElement))
                        {
                            this.readedElements.Add(currentElement);
                            reader.UnGet();
                            return this.stateList[3];
                        }
                        else
                        {
                            this.errorMessages.Add("Can't parse object value.");
                            this.hasErrors = true;
                            return this.stateList[1];
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

                var currentElement = default(T);
                if (this.parser.TryParse(this.currentElementSymbols.ToArray(), out currentElement))
                {
                    this.readedElements.Add(currentElement);
                    this.currentElementSymbols.Clear();
                    return this.stateList[3];
                }
                else
                {
                    this.errorMessages.Add("Can't parse object value.");
                    this.hasErrors = true;
                    return this.stateList[1];
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

                        var currentElement = default(T);
                        if (this.parser.TryParse(this.currentElementSymbols.ToArray(), out currentElement))
                        {
                            this.readedElements.Add(currentElement);
                            reader.UnGet();
                            return this.stateList[3];
                        }
                        else
                        {
                            this.errorMessages.Add("Can't parse object value.");
                            this.hasErrors = true;
                            return this.stateList[1];
                        }
                    }
                }
            }
        }

        #endregion
    }
}
