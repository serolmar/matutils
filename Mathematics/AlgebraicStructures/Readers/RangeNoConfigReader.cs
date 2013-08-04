using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;
using System.Collections.ObjectModel;

namespace Mathematics
{
    public class RangeNoConfigReader<T, SymbValue, SymbType, InputReader> : ARangeReader<T, SymbValue, SymbType, InputReader>
    {
        /// <summary>
        /// The element parser.
        /// </summary>
        private IParse<T, SymbValue, SymbType> parser;

        private List<int> currentReadedConfiguration = new List<int>();

        private List<T> readedElements;

        private List<int> finalConfiguration = new List<int>();

        private int level;

        private Stack<SymbType> opsStack = new Stack<SymbType>();

        private Stack<RangeReaderMementoManager> mementoStack = new Stack<RangeReaderMementoManager>();

        private List<IState<InputReader, SymbValue, SymbType>> stateList = new List<IState<InputReader, SymbValue, SymbType>>();

        public RangeNoConfigReader()
        {
            this.InitStates();
        }

        /// <summary>
        /// Parses the reader without having the configuration defined.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>The result.</returns>
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
                this.finalConfiguration.Clear();
                this.readedElements = new List<T>();
                this.RunStateMchine(reader);
                this.finalConfiguration.RemoveAt(this.finalConfiguration.Count - 1);
            }
        }

        protected override IEnumerable<int> GetFinalCofiguration()
        {
            return this.finalConfiguration;
        }

        protected override ReadOnlyCollection<T> GetElements()
        {
            return this.readedElements.AsReadOnly();
        }

        private void RunStateMchine(MementoSymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            var stateMchine = new StateMachine<InputReader, SymbValue, SymbType>(stateList[0], stateList[1]);
            stateMchine.RunMachine(reader);
        }

        private void Reset()
        {
            this.opsStack.Clear();
        }

        private void InitStates()
        {
            this.stateList.Clear();
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(0, "start", this.StartTransition));
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(1, "end", this.EndTransition));
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(2, "sequence", this.SequenceTransition));
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(3, "element", this.ElementTransition));
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(4, "inside", this.InsideElementDelimitersTransition));
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(5, "operator", this.OperatorTransition));
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(6, "resume sequence", this.ResumeSequenceTransition));
            this.stateList.Add(new DelegateStateImplementation<InputReader, SymbValue, SymbType>(7, "after start", this.AfterStartTransition));
        }

        private IState<InputReader, SymbValue, SymbType> StartTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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

        private IState<InputReader, SymbValue, SymbType> EndTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            return null;
        }

        private IState<InputReader, SymbValue, SymbType> SequenceTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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

        private IState<InputReader, SymbValue, SymbType> ElementTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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

                                (reader as MementoSymbolReader<InputReader, SymbValue, SymbType>).RestoreToMemento(topMemento.Memento);
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
                    throw new ExpressionReaderException("");
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

                            (reader as MementoSymbolReader<InputReader, SymbValue, SymbType>).RestoreToMemento(topMemento.Memento);
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

        private IState<InputReader, SymbValue, SymbType> InsideElementDelimitersTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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

        private IState<InputReader, SymbValue, SymbType> OperatorTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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

                            (reader as MementoSymbolReader<InputReader, SymbValue, SymbType>).RestoreToMemento(topMemento.Memento);
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

        private IState<InputReader, SymbValue, SymbType> ResumeSequenceTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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

        private IState<InputReader, SymbValue, SymbType> AfterStartTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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
                        Memento = (reader as MementoSymbolReader<InputReader, SymbValue, SymbType>).SaveToMemento(),
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

        private void IgnoreVoids(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            var symbol = reader.Peek();
            while (this.blancks.Contains(symbol.SymbolType))
            {
                reader.Get();
                symbol = reader.Peek();
            }
        }
    }
}
