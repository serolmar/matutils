using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;

namespace Mathematics
{
    public class MultiDimensionalRangeNoConfigParser<T, SymbValue, SymbType, InputReader> : AMultiDimensionalRangeParser<T, SymbValue, SymbType, InputReader>
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

        private Stack<MultidimensionalRangeParserMementoManager> mementoStack = new Stack<MultidimensionalRangeParserMementoManager>();

        private List<IState<InputReader, SymbValue, SymbType>> stateList = new List<IState<InputReader, SymbValue, SymbType>>();

        public MultiDimensionalRangeNoConfigParser()
        {
            this.InitStates();
        }

        public SymbType SeparatorSymbType
        {
            get
            {
                return this.separatorSymb;
            }
            set
            {
                this.separatorSymb = value;
            }
        }

        public override MultiDimensionalRange<T> ParseRange(MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser)
        {
            this.Reset();
            this.parser = parser;
            var range = new MultiDimensionalRange<T>();
            this.ParseNoConfig(range, reader);
            return range;
        }

        /// <summary>
        /// Parses the reader without having the configuration defined.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>The result.</returns>
        private void ParseNoConfig(
            MultiDimensionalRange<T> range,
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            this.finalConfiguration.Clear();
            this.readedElements = new List<T>();
            this.RunStateMchine(reader);
            range.InternalElements = this.readedElements.ToArray();
            var count = this.currentReadedConfiguration.Count - 1;
            range.InnerConfiguration = new int[count];
            for (int i = 0; i < count; ++i)
            {
                range.InnerConfiguration[i] = this.finalConfiguration[i];
            }
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
                throw new ExpressionReaderException("Expecting open delimiter but found end of expression.");
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
                    throw new ExpressionReaderException("Expecting open delimiter at the begining of expression.");
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
                    throw new ExpressionReaderException("Delimiters mismatch.");
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
                            this.opsStack.Push(readedSymbol.SymbolType);
                            this.currentElementSymbols.Add(readedSymbol);
                            reader.Get();
                            return this.stateList[4];
                        }
                        else
                        {
                            throw new ExpressionReaderException("Expression doesn't match range dimensions.");
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
                    throw new ExpressionReaderException("Parenthesis mismatch.");
                }
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                if (this.opsStack.Count == 0)
                {
                    throw new ExpressionReaderException("Unexpected symbol after final close delimiter.");
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    var topOperator = this.opsStack.Pop();
                    if (!this.mapInternalOpenDelimitersToCloseDelimitersTypes.TargetFor(topOperator).Contains(
                        readedSymbol.SymbolType))
                    {
                        throw new ExpressionReaderException("Delimiters mismatch.");
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
                                throw new ExpressionReaderException("The number of readed elements in range doesn't match the initial configuration.");
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
                    throw new ExpressionReaderException("Unexpected open delimiter.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    throw new ExpressionReaderException("Unexpected open delimiter.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    throw new ExpressionReaderException("Unexpected close delimiter.");
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
                            throw new ExpressionReaderException("The number of readed elements in range doesn't match the current configuration configuration.");
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
                    throw new ExpressionReaderException("Parse error.");
                }
            }
        }

        private IState<InputReader, SymbValue, SymbType> InsideElementDelimitersTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                throw new ExpressionReaderException("Expecting open delimiter but found end of expression.");
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Get();
                if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    var topOperator = this.opsStack.Pop();
                    if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ObjectFor(readedSymbol.SymbolType).Contains(
                        topOperator))
                    {
                        this.currentElementSymbols.Add(readedSymbol);
                        var readedElement = default(T);
                        if (this.parser.TryParse(this.currentElementSymbols.ToArray(), out readedElement))
                        {
                            this.readedElements.Add(readedElement);
                            this.currentElementSymbols.Clear();
                            return this.stateList[3];
                        }
                        else
                        {
                            throw new ExpressionReaderException("Can't parse object value.");
                        }
                    }
                    else
                    {
                        this.opsStack.Push(topOperator);
                        return this.stateList[4];
                    }
                }
                else
                {
                    this.currentElementSymbols.Add(readedSymbol);
                    return this.stateList[4];
                }
            }
        }

        private IState<InputReader, SymbValue, SymbType> OperatorTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                throw new ExpressionReaderException("Expecting open delimiter but found end of expression.");
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
                            this.opsStack.Push(readedSymbol.SymbolType);
                            this.currentElementSymbols.Add(readedSymbol);
                            reader.Get();
                            return this.stateList[4];
                        }
                        else
                        {
                            throw new ExpressionReaderException("Expression doesn't match range dimensions.");
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
                            throw new ExpressionReaderException("Invalid range dimensions.");
                        }
                        else
                        {
                            this.currentElementSymbols.Add(readedSymbol);
                            this.opsStack.Push(readedSymbol.SymbolType);

                            reader.Get();
                            return this.stateList[4];
                        }
                    }
                    else
                    {
                        throw new ExpressionReaderException("Unexpected open delimiter after operator within range dimensions.");
                    }
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    throw new ExpressionReaderException("Unexpected close delimiter after separator.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    throw new ExpressionReaderException("Unexpected close delimiter after separator.");
                }
                else if (readedSymbol.SymbolType.Equals(this.separatorSymb))
                {
                    throw new ExpressionReaderException("Unexpected separator after separator.");
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
                            throw new ExpressionReaderException("Invalid range dimensions.");
                        }
                    }
                    else
                    {
                        var readedElement = default(T);
                        if (this.parser.TryParse(new[] { readedSymbol }, out readedElement))
                        {
                            this.readedElements.Add(readedElement);
                            reader.Get();
                            return this.stateList[3];
                        }
                        else
                        {
                            throw new ExpressionReaderException("Can't parse object value.");
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
                    throw new ExpressionReaderException("Delimiters mismatch.");
                }

                return this.stateList[1];
            }
            else
            {
                var readedSymbol = reader.Peek();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    throw new ExpressionReaderException("Unexpected close delimiter.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.opsStack.Push(readedSymbol.SymbolType);
                    this.currentElementSymbols.Add(readedSymbol);

                    reader.Get();
                    return this.stateList[4];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    throw new ExpressionReaderException("Unexpected close delimiter.");
                }
                else if (readedSymbol.SymbolType.Equals(separatorSymb))
                {
                    throw new ExpressionReaderException("Unexpected separator symbol.");
                }
                else
                {
                    var currentElement = default(T);
                    if (this.parser.TryParse(new[] { readedSymbol }, out currentElement))
                    {
                        this.readedElements.Add(currentElement);
                        reader.Get();
                        return this.stateList[3];
                    }
                    else
                    {
                        throw new ExpressionReaderException("Can't parse object value.");
                    }
                }
            }
        }

        private IState<InputReader, SymbValue, SymbType> AfterStartTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            if (reader.IsAtEOF())
            {
                throw new ExpressionReaderException("Expecting open delimiter but found end of expression.");
            }
            else
            {
                this.IgnoreVoids(reader);
                var readedSymbol = reader.Peek();
                if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.opsStack.Push(readedSymbol.SymbolType);
                    var memento = new MultidimensionalRangeParserMementoManager()
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
                    throw new ExpressionReaderException("Unexpected close delimiter.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(readedSymbol.SymbolType))
                {
                    this.opsStack.Push(readedSymbol.SymbolType);
                    this.currentElementSymbols.Add(readedSymbol);

                    reader.Get();
                    return this.stateList[4];
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(readedSymbol.SymbolType))
                {
                    throw new ExpressionReaderException("Unexpected close delimiter.");
                }
                else if (readedSymbol.SymbolType.Equals(separatorSymb))
                {
                    throw new ExpressionReaderException("Unexpected separator symbol.");
                }
                else
                {
                    var currentElement = default(T);
                    if (this.parser.TryParse(new[] { readedSymbol }, out currentElement))
                    {
                        this.readedElements.Add(currentElement);
                        reader.Get();
                        return this.stateList[3];
                    }
                    else
                    {
                        throw new ExpressionReaderException("Can't parse object value.");
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
