using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    public class RangeConfigReader<T, SymbValue, SymbType, InputReader> : ARangeReader<T, SymbValue, SymbType, InputReader>
    {
        /// <summary>
        /// The element parser.
        /// </summary>
        private IParse<T, SymbValue, SymbType> parser;

        private List<int> currentReadedConfiguration = new List<int>();

        private List<T> readedElements;

        private int[] finalConfiguration;

        private int level;

        private Stack<SymbType> opsStack = new Stack<SymbType>();

        private List<IState<InputReader, SymbValue, SymbType>> stateList = new List<IState<InputReader, SymbValue, SymbType>>();

        public RangeConfigReader(int[] finalConfiguration)
        {
            if (finalConfiguration == null)
            {
                throw new UtilitiesDataException("Parameter range can't be null.");
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
                this.RunStateMchine(reader);
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

        #region Transition Functions
        private IState<InputReader, SymbValue, SymbType> StartTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
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
