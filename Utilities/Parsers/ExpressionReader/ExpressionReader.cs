
namespace Utilities.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Parsers;

    /// <summary>
    /// Binary operator delegate.
    /// </summary>
    /// <typeparam name="ObjType">The object type.</typeparam>
    /// <param name="left">The left hand side of operator object.</param>
    /// <param name="right">The right hand side of operator object.</param>
    /// <returns>The result of operation.</returns>
    public delegate ObjType BinaryOperator<ObjType>(ObjType left, ObjType right);

    /// <summary>
    /// Unary operator delegate.
    /// </summary>
    /// <typeparam name="ObjType">The object type.</typeparam>
    /// <param name="operand">The operand object.</param>
    /// <returns>The result of operation.</returns>
    public delegate ObjType UnaryOperator<ObjType>(ObjType operand);

    public class ExpressionReader<ObjType, InputReader>
    {
        /// <summary>
        /// Parse the objects from readed elements.
        /// </summary>
        private IParse<ObjType, string, string> parser;

        /// <summary>
        /// The list of available states.
        /// </summary>
        private List<IState<InputReader, string, string>> stateList = new List<IState<InputReader, string, string>>();

        /// <summary>
        /// A list with the binary operators and precedences.
        /// </summary>
        private Dictionary<string, SOperatorPrecedence<BinaryOperator<ObjType>>> binaryOperators;

        /// <summary>
        /// A list with unary operators and precedences.
        /// </summary>
        private Dictionary<string, SOperatorPrecedence<UnaryOperator<ObjType>>> unaryOperators;

        /// <summary>
        /// The mapped expression delimiters. An expression delimiter alter the operators precedences.
        /// </summary>
        private Dictionary<string, List<ExpressionCompoundDelimiter<ObjType>>> expressionDelimitersTypes = new Dictionary<string, List<ExpressionCompoundDelimiter<ObjType>>>();

        /// <summary>
        /// Maps the external delimiters. External delimiters bounds entire elementary subexpressions.
        /// </summary>
        private Dictionary<string, List<string>> externalDelimitersTypes = new Dictionary<string, List<string>>();

        /// <summary>
        /// Maps sequence delimiters. Sequence delimiters bounds function expressions.
        /// </summary>
        private Dictionary<string, List<string>> sequenceDelimitersTypes = new Dictionary<string, List<string>>();

        /// <summary>
        /// A list with all items in expression that will be ignored like blanck spaces or changes of line.
        /// </summary>
        private List<string> expressionVoids = new List<string>();

        /// <summary>
        /// The operators stack.
        /// </summary>
        private Stack<OperatorDefinition<string>> operatorStack = new Stack<OperatorDefinition<string>>();

        /// <summary>
        /// The elements stack.
        /// </summary>
        private Stack<ObjType> elementStack = new Stack<ObjType>();

        /// <summary>
        /// The current readed value.
        /// </summary>
        private List<ISymbol<string, string>> currentSymbolValues = new List<ISymbol<string,string>>();

        /// <summary>
        /// Intantiates a new instance of the <see cref="ExpressionReader"/> class.
        /// </summary>
        /// <param name="parser">The expression element's parser.</param>
        public ExpressionReader(IParse<ObjType, string, string> parser)
        {
            this.parser = parser;
            this.binaryOperators = new Dictionary<string, SOperatorPrecedence<BinaryOperator<ObjType>>>();
            this.unaryOperators = new Dictionary<string, SOperatorPrecedence<UnaryOperator<ObjType>>>();
            this.stateList.Add(new DelegateDrivenState<InputReader, string, string>(0, "start", this.StartTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, string, string>(1, "end", this.EndTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, string, string>(2, "element", this.ElementTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, string, string>(3, "operator", this.OperatorTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, string, string>(4, "external delimiters", this.InsideExternalDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, string, string>(5, "sequence peek", this.SequencePeekDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, string, string>(6, "end", this.InsideSequenceDelimitersTransition));
        }

        #region Public Methods
        /// <summary>
        /// Parses the expresssion provided in a symbol reader.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The parsed object.</returns>
        public ObjType Parse(SymbolReader<InputReader, string, string> reader)
        {
            StateMachine<InputReader, string, string> stateMchine = new StateMachine<InputReader, string, string>(this.stateList[0], this.stateList[1]);
            stateMchine.RunMachine(reader);
            if (this.elementStack.Count != 0)
            {
                return this.elementStack.Pop();
            }
            else
            {
                throw new ExpressionReaderException("Empty value.");
            }
        }

        /// <summary>
        /// Registers a binary operator.
        /// </summary>
        /// <param name="opDesignation">The operator name.</param>
        /// <param name="functionOperator">The operator function delegate.</param>
        /// <param name="precedence">The operator's precedence.</param>
        public void RegisterBinaryOperator(string opDesignation, BinaryOperator<ObjType> functionOperator, int precedence)
        {
            if (!this.binaryOperators.ContainsKey(opDesignation))
            {
                SOperatorPrecedence<BinaryOperator<ObjType>> precedencePair = new SOperatorPrecedence<BinaryOperator<ObjType>>()
                {
                    Op = functionOperator,
                    Precedence = precedence
                };

                this.binaryOperators.Add(opDesignation, precedencePair);
            }
            else
            {
                this.binaryOperators[opDesignation] = new SOperatorPrecedence<BinaryOperator<ObjType>>()
                {
                    Op = functionOperator,
                    Precedence = precedence
                };
            }
        }

        /// <summary>
        /// Registers an unary operator.
        /// </summary>
        /// <param name="opDesignation">The operator name.</param>
        /// <param name="functionOperator">The operator function delegate.</param>
        /// <param name="precedence">The operator's precedence.</param>
        public void RegisterUnaryOperator(string opDesignation, UnaryOperator<ObjType> functionOperator, int precedence)
        {
            if (this.unaryOperators.ContainsKey(opDesignation))
            {
                this.unaryOperators[opDesignation] = new SOperatorPrecedence<UnaryOperator<ObjType>>()
                {
                    Op = functionOperator,
                    Precedence = precedence
                };
            }
            else
            {
                SOperatorPrecedence<UnaryOperator<ObjType>> precedencePair = new SOperatorPrecedence<UnaryOperator<ObjType>>()
                {
                    Op = functionOperator,
                    Precedence = precedence
                };

                this.unaryOperators.Add(opDesignation, precedencePair);
            }
        }

        public void RegisterExternalDelimiterTypes(string openDelimiter, string closeDelimiter)
        {
            if (this.expressionDelimitersTypes.ContainsKey(openDelimiter))
            {
                throw new ExpressionReaderException("The specified external open delimiter was already setup for an expression open delimiter.");
            }

            if (this.externalDelimitersTypes.ContainsKey(openDelimiter))
            {
                if (!this.externalDelimitersTypes[openDelimiter].Contains(closeDelimiter))
                {
                    this.externalDelimitersTypes[openDelimiter].Add(closeDelimiter);
                }
            }
            else
            {
                List<string> temporary = new List<string>();
                temporary.Add(closeDelimiter);
                this.externalDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        public void RegisterSequenceDelimiterTypes(string openDelimiter, string closeDelimiter)
        {
            if (this.sequenceDelimitersTypes.ContainsKey(openDelimiter))
            {
                if (!this.sequenceDelimitersTypes[openDelimiter].Contains(closeDelimiter))
                {
                    this.sequenceDelimitersTypes[openDelimiter].Add(closeDelimiter);
                }
            }
            else
            {
                List<string> temporary = new List<string>();
                temporary.Add(closeDelimiter);
                this.sequenceDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        /// <summary>
        /// Registers the expression delimiter types. Some objects may have expressions wich subexpressions contains registered operators.
        /// In that case, all object expression must be enclosed by registered delimiters.
        /// </summary>
        /// <param name="openDelimiter">The open delimiter.</param>
        /// <param name="closeDelimiter">The close delimiter.</param>
        public void RegisterExpressionDelimiterTypes(string openDelimiter, string closeDelimiter, UnaryOperator<ObjType> unaryOp)
        {
            if (this.externalDelimitersTypes.ContainsKey(openDelimiter))
            {
                throw new ExpressionReaderException("The specified expression open delimiter was already setup for an external open delimiter.");
            }

            ExpressionCompoundDelimiter<ObjType> compound = new ExpressionCompoundDelimiter<ObjType>() { DelimiterType = closeDelimiter, DelimiterOperator = unaryOp };
            if (this.expressionDelimitersTypes.ContainsKey(openDelimiter))
            {
                if (!this.expressionDelimitersTypes[openDelimiter].Contains(compound))
                {
                    this.expressionDelimitersTypes[openDelimiter].Add(compound);
                }
            }
            else
            {
                List<ExpressionCompoundDelimiter<ObjType>> temporary = new List<ExpressionCompoundDelimiter<ObjType>>();
                temporary.Add(compound);
                this.expressionDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        public void RegisterExpressionDelimiterTypes(string openDelimiter, string closeDelimiter)
        {
            this.RegisterExpressionDelimiterTypes(openDelimiter, closeDelimiter, null);
        }

        /// <summary>
        /// Adds symbol types that are to be ignored. If an operator is added as a void than it will be ignored.
        /// </summary>
        /// <param name="voidType">The symbol type.</param>
        public void AddVoid(string voidType)
        {
            this.expressionVoids.Add(voidType);
        }
        #endregion

        private bool IsBinaryOperator(string operatorType)
        {
            return this.binaryOperators.ContainsKey(operatorType);
        }

        private bool IsUnaryOperator(string operatorType)
        {
            return this.unaryOperators.ContainsKey(operatorType);
        }

        private bool IsExpressionOpenDelimiter(string operatorType)
        {
            return this.expressionDelimitersTypes.ContainsKey(operatorType);
        }

        private bool IsExpressionCloseDelimiter(string operatorType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType>() { DelimiterType = operatorType };
            return (from pair in this.expressionDelimitersTypes
                    where pair.Value.Contains(compound)
                    select pair).Any();
        }

        private bool IsExpressionOpenDelimiterFor(string closeDelimiter, string openDelimiter)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType>() { DelimiterType = closeDelimiter };
            return (from res in this.expressionDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(compound)
                    select res).Any();
        }

        private ExpressionCompoundDelimiter<ObjType> GetDelegateCompoundForPair(string openDelimiterType, string closeDelimiterType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType>() { DelimiterType = closeDelimiterType };
            var q =
                       (from res in this.expressionDelimitersTypes
                        where res.Key.Equals(openDelimiterType)
                        select res.Value).FirstOrDefault();
            if (q == null)
            {
                return null;
            }

            return (from res in q
                    where res.DelimiterType.Equals(closeDelimiterType)
                    select res).FirstOrDefault();

        }

        private bool MapOpenDelimiterType(string operatorTypeToMatch, string openDelimiterType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType>() { DelimiterType = openDelimiterType };
            if (!this.expressionDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.expressionDelimitersTypes[operatorTypeToMatch].Contains(compound);
        }

        private bool IsExternalOpenDelimiter(string operatorType)
        {
            return this.externalDelimitersTypes.ContainsKey(operatorType);
        }

        private bool IsExternalCloseDelimiter(string operatorType)
        {
            return (from pair in this.externalDelimitersTypes
                    where pair.Value.Contains(operatorType)
                    select pair).Any();
        }

        private bool IsExternalCloseDelimiterFor(string closeDelimiter, string openDelimiter)
        {
            return (from res in this.externalDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(closeDelimiter)
                    select res).Any();
        }

        private bool MapOpenExternalDelimiterType(string operatorTypeToMatch, string openExternalDelimiterType)
        {
            if (!this.externalDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.externalDelimitersTypes[operatorTypeToMatch].Contains(openExternalDelimiterType);
        }

        private bool IsSequenceOpenDelimiter(string operatorType)
        {
            return this.sequenceDelimitersTypes.ContainsKey(operatorType);
        }

        private bool IsSequenceCloseDelimiter(string operatorType)
        {
            return (from pair in this.sequenceDelimitersTypes
                    where pair.Value.Contains(operatorType)
                    select pair).Any();
        }

        private bool IsSequenceCloseDelimiterFor(string closeDelimiter, string openDelimiter)
        {
            return (from res in this.sequenceDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(closeDelimiter)
                    select res).Any();
        }

        private bool MapOpenSequenceDelimiterType(string operatorTypeToMatch, string openExternalDelimiterType)
        {
            if (!this.sequenceDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.externalDelimitersTypes[operatorTypeToMatch].Contains(openExternalDelimiterType);
        }

        private List<ExpressionCompoundDelimiter<ObjType>> GetExpressionCloseMatches(string openType)
        {
            try
            {
                return this.expressionDelimitersTypes[openType];
            }
            catch (Exception)
            {
                return new List<ExpressionCompoundDelimiter<ObjType>>();
            }
        }

        private List<string> GetExternalCloseMatchcs(string openType)
        {
            try
            {
                return this.externalDelimitersTypes[openType];
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        private void IgnoreVoids(SymbolReader<InputReader, string, string> symbolReader)
        {
            ISymbol<string, string> symbol = symbolReader.Peek();
            while (this.expressionVoids.Contains(symbol.SymbolType))
            {
                symbolReader.Get();
                symbol = symbolReader.Peek();
            }
        }

        private ObjType InvokeBinaryOperator(string operatorType, ObjType left, ObjType right)
        {
            return this.binaryOperators[operatorType].Op.Invoke(left, right);
        }

        private ObjType InvokeUnaryOperator(string operatorType, ObjType obj)
        {
            return this.unaryOperators[operatorType].Op.Invoke(obj);
        }

        /// <summary>
        /// Evaluates the stacks and computes each value.
        /// </summary>
        /// <param name="closeDelimiterType">The close delimiter type before function calling.</param>
        /// <param name="precedence">The precedence of operator where to stop.</param>
        /// <param name="ignorePrecedence">True if evaluation is to be computed until operator stack is empty or to expression delimiters.</param>
        private void Eval(int precedence, bool ignorePrecedence)
        {
            while (!(this.operatorStack.Count == 0))
            {
                var topOperator = this.operatorStack.Pop();

                if (topOperator.OperatorType == EOperatorType.UNARY)
                {
                    if (this.unaryOperators[topOperator.Symbol].Precedence < precedence && !ignorePrecedence)
                    {
                        this.operatorStack.Push(topOperator);
                        return;
                    }
                    if (this.elementStack.Count == 0)
                    {
                        throw new ExpressionReaderException("Internal error.");
                    }
                    ObjType res = this.elementStack.Pop();
                    res = this.InvokeUnaryOperator(topOperator.Symbol, res);
                    this.elementStack.Push(res);
                }
                else if (topOperator.OperatorType == EOperatorType.BINARY)
                {
                    if (this.binaryOperators[topOperator.Symbol].Precedence < precedence && !ignorePrecedence)
                    {
                        this.operatorStack.Push(topOperator);
                        return;
                    }
                    if (this.elementStack.Count < 2)
                    {
                        throw new ExpressionReaderException("Internal error.");
                    }
                    ObjType b = this.elementStack.Pop();
                    ObjType a = this.elementStack.Pop();
                    ObjType res = this.InvokeBinaryOperator(topOperator.Symbol, a, b);
                    this.elementStack.Push(res);
                }
                else if (topOperator.OperatorType == EOperatorType.INTERNAL_DELIMITER)
                {
                    this.operatorStack.Push(topOperator);
                    return;
                }
                else
                {
                    throw new Exception("Internal error.");
                }
            }
        }

        #region State Functions
        /// <summary>
        /// Start transition function - state 0.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, string, string> StartTransition(SymbolReader<InputReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            if (reader.IsAtEOF())
            {
                return this.stateList[1];
            }

            ISymbol<string, string> readedSymbol = reader.Peek();
            if (this.IsExpressionOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.INTERNAL_DELIMITER));
                reader.Get();
                return this.stateList[0];
            }
            if (this.IsExternalOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.EXTERNAL_DELIMITER));
                this.currentSymbolValues.Add(readedSymbol);
                reader.Get();
                return this.stateList[4];
            }
            if (this.IsExpressionCloseDelimiter(readedSymbol.SymbolType) || this.IsExternalCloseDelimiter(readedSymbol.SymbolType))
            {
                throw new ExpressionReaderException(string.Format(
                    "Unexpected {0} in expression.",
                    readedSymbol.SymbolValue));
            }
            if (this.IsUnaryOperator(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.UNARY));
                reader.Get();
                return this.stateList[2];
            }
            if (this.IsBinaryOperator(readedSymbol.SymbolType))
            {
                return this.stateList[1];
            }
            return this.stateList[2];
        }

        /// <summary>
        /// End transition function - state 1.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, string, string> EndTransition(SymbolReader<InputReader, string, string> reader)
        {
            return null;
        }

        /// <summary>
        /// Element transition function - state 2.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, string, string> ElementTransition(SymbolReader<InputReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            if (reader.IsAtEOF())
            {
                throw new ExpressionReaderException("Unexpected end of expression.");
            }
            ISymbol<string, string> readedSymbol = reader.Peek();
            this.currentSymbolValues.Clear();
            if (this.IsExpressionOpenDelimiter(readedSymbol.SymbolType))
            {
                return this.stateList[0];
            }
            if (this.IsExternalOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.EXTERNAL_DELIMITER));
                this.currentSymbolValues.Add(readedSymbol);
                reader.Get();
                return this.stateList[4];
            }
            if (this.IsUnaryOperator(readedSymbol.SymbolType))
            {
                // TODO: verificar no topo da pilha dos operadores se existe algum que esteja marcado como admitindo um sinal unário
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.UNARY));
                reader.Get();
                return this.stateList[2];
            }
            if (this.IsBinaryOperator(readedSymbol.SymbolType))
            {
                throw new ExpressionReaderException("Unexpected binary operator. Binary operators are forbidden after other operators.");
            }

            this.currentSymbolValues.Add(readedSymbol);
            reader.Get();
            return this.stateList[5];
        }

        /// <summary>
        /// Operator transition function - state 3.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, string, string> OperatorTransition(SymbolReader<InputReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            if (reader.IsAtEOF())
            {
                this.Eval(0, true);
                if (this.operatorStack.Count > 0)
                {
                    throw new ExpressionReaderException("There are unmatched expression delimiters.");
                }
                return this.stateList[1];
            }
            ISymbol<string, string> readedSymbol = reader.Peek();
            if (this.IsBinaryOperator(readedSymbol.SymbolType))
            {
                this.Eval(this.binaryOperators[readedSymbol.SymbolType].Precedence, false);
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.BINARY));
                reader.Get();
                return this.stateList[2];
            }
            if (this.IsUnaryOperator(readedSymbol.SymbolType))
            {
                throw new ExpressionReaderException("Unexpected unary operator.");
            }
            if (this.IsExpressionCloseDelimiter(readedSymbol.SymbolType))
            {
                this.Eval(0, true);
                if (this.operatorStack.Count == 0)
                {
                    throw new ExpressionReaderException(string.Format(
                        "Found close delimiter {0} but no open delimiter is matched.",
                        readedSymbol.SymbolType));
                }

                var delimiterType = this.operatorStack.Pop().Symbol;
                if (!this.IsExpressionOpenDelimiterFor(readedSymbol.SymbolType, delimiterType))
                {
                    throw new ExpressionReaderException(string.Format(
                        "Found close delimiter {0} that matches with unmtchable {1} open delimiter",
                        readedSymbol.SymbolType,
                        delimiterType));
                }

                var compoundDelimiter = this.GetDelegateCompoundForPair(delimiterType, readedSymbol.SymbolType);

                if (compoundDelimiter.DelimiterOperator != null)
                {
                    if (this.elementStack.Count != 0)
                    {
                        ObjType res = this.elementStack.Pop();
                        res = compoundDelimiter.DelimiterOperator.Invoke(res);
                        this.elementStack.Push(res);
                    }
                }

                reader.Get();
                return this.stateList[3];
            }

            throw new ExpressionReaderException(string.Format(
                "Expected operator but received {0}",
                readedSymbol.SymbolValue));
        }

        /// <summary>
        /// Inside external delimiters transition function - state 4.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, string, string> InsideExternalDelimitersTransition(SymbolReader<InputReader, string, string> reader)
        {
            ISymbol<string, string> readedSymbol = reader.Peek();
            if (readedSymbol.SymbolType == "eof")
            {
                throw new ExpressionReaderException("An external delimiter was opened but wasn't closed.");
            }
            else if (this.IsExternalOpenDelimiter(readedSymbol.SymbolType))
            {
                this.currentSymbolValues.Add(readedSymbol);
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.EXTERNAL_DELIMITER));
            }
            else if (this.IsExternalCloseDelimiter(readedSymbol.SymbolType))
            {
                if (this.operatorStack.Count == 0)
                {
                    throw new ExpressionReaderException("External delimiters mismatch.");
                }
                else
                {
                    var delimiter = this.operatorStack.Pop();
                    if (delimiter.OperatorType == EOperatorType.INTERNAL_DELIMITER)
                    {
                        throw new ExpressionReaderException("Delimiters mismatch or confusion with internal delimiters.");
                    }
                    else if (delimiter.OperatorType == EOperatorType.EXTERNAL_DELIMITER)
                    {
                        if (this.IsExternalCloseDelimiterFor(readedSymbol.SymbolType, delimiter.Symbol))
                        {
                            this.currentSymbolValues.Add(readedSymbol);
                            if (this.operatorStack.Count == 0)
                            {
                                var parsed = default(ObjType);
                                if (this.parser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                                {
                                    this.elementStack.Push(parsed);
                                    reader.Get();
                                }
                                else
                                {
                                    throw new ExpressionReaderException("Can't parse expression.");
                                }

                                return this.stateList[3];
                            }
                            else
                            {
                                var nextStackedOperator = this.operatorStack.Pop();
                                this.operatorStack.Push(nextStackedOperator);
                                if (nextStackedOperator.OperatorType != EOperatorType.EXTERNAL_DELIMITER)
                                {
                                    var parsed = default(ObjType);
                                    if (this.parser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                                    {
                                        this.elementStack.Push(parsed);
                                        reader.Get();
                                    }
                                    else
                                    {
                                        throw new ExpressionReaderException("Can't parse expression.");
                                    }

                                    return this.stateList[3];
                                }
                            }
                        }
                        else
                        {
                            throw new ExpressionReaderException("External delimiters mismatch.");
                        }
                    }
                    else
                    {
                        throw new ExpressionReaderException("External delimiters mismatch.");
                    }
                }
            }
            else
            {
                this.currentSymbolValues.Add(readedSymbol);
            }

            reader.Get();
            return this.stateList[4];
        }

        /// <summary>
        /// Sequence peek transition function - state 5.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, string, string> SequencePeekDelimitersTransition(SymbolReader<InputReader, string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readedSymbol = reader.Peek();
            if (this.IsSequenceOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<string>(readedSymbol.SymbolType, EOperatorType.SEQUENCE_DELIMITER));
                this.currentSymbolValues.Add(readedSymbol);
                reader.Get();
                return this.stateList[6];
            }
            else
            {
                var parsed = default(ObjType);
                if (this.parser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                {
                    this.elementStack.Push(parsed);
                }
                else
                {
                    throw new ExpressionReaderException("Can't parse expression.");
                }

                return this.stateList[3];
            }
        }

        /// <summary>
        /// Inside sequence delimiters transition function - state 6.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, string, string> InsideSequenceDelimitersTransition(SymbolReader<InputReader, string, string> reader)
        {
            ISymbol<string, string> readedSymbol = reader.Peek();
            string delimiterType = this.operatorStack.Pop().Symbol;
            while (!this.IsSequenceCloseDelimiterFor(readedSymbol.SymbolType, delimiterType) && !reader.IsAtEOF())
            {
                readedSymbol = reader.Get();
                this.currentSymbolValues.Add(readedSymbol);
                readedSymbol = reader.Peek();
            }

            if (reader.IsAtEOF())
            {
                throw new ExpressionReaderException("A sequence delimiter was opened but wasn't closed.");
            }
            else
            {
                readedSymbol = reader.Get();
                this.currentSymbolValues.Add(readedSymbol);
                var parsed = default(ObjType);
                if (this.parser.TryParse(this.currentSymbolValues.ToArray(), out parsed))
                {
                    this.elementStack.Push(parsed);
                    reader.Get();
                }
                else
                {
                    throw new ExpressionReaderException("Can't parse expression.");
                }

                return this.stateList[3];
            }
        }
        #endregion
    }
}
