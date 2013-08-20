
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

    public class ExpressionReader<ObjType, SymbValue, SymbType, InputReader>
    {
        /// <summary>
        /// Parse the objects from readed elements.
        /// </summary>
        private IParse<ObjType, SymbValue, SymbType> parser;

        /// <summary>
        /// The list of available states.
        /// </summary>
        private List<IState<InputReader, SymbValue, SymbType>> stateList = new List<IState<InputReader, SymbValue, SymbType>>();

        /// <summary>
        /// A list with the binary operators and precedences.
        /// </summary>
        private Dictionary<SymbType, SOperatorPrecedence<BinaryOperator<ObjType>>> binaryOperators;

        /// <summary>
        /// A list with unary operators and precedences.
        /// </summary>
        private Dictionary<SymbType, SOperatorPrecedence<UnaryOperator<ObjType>>> unaryOperators;

        /// <summary>
        /// The mapped expression delimiters. An expression delimiter alter the operators precedences.
        /// </summary>
        private Dictionary<SymbType, List<ExpressionCompoundDelimiter<ObjType, SymbType>>> expressionDelimitersTypes = 
            new Dictionary<SymbType, List<ExpressionCompoundDelimiter<ObjType, SymbType>>>();

        /// <summary>
        /// Maps the external delimiters. External delimiters bounds entire elementary subexpressions.
        /// </summary>
        private Dictionary<SymbType, List<SymbType>> externalDelimitersTypes = new Dictionary<SymbType, List<SymbType>>();

        /// <summary>
        /// Maps sequence delimiters. Sequence delimiters bounds function expressions.
        /// </summary>
        private Dictionary<SymbType, List<SymbType>> sequenceDelimitersTypes = new Dictionary<SymbType, List<SymbType>>();

        /// <summary>
        /// A list with all items in expression that will be ignored like blanck spaces or changes of line.
        /// </summary>
        private List<SymbType> expressionVoids = new List<SymbType>();

        /// <summary>
        /// The operators stack.
        /// </summary>
        private Stack<OperatorDefinition<SymbType>> operatorStack = new Stack<OperatorDefinition<SymbType>>();

        /// <summary>
        /// The elements stack.
        /// </summary>
        private Stack<ObjType> elementStack = new Stack<ObjType>();

        /// <summary>
        /// The current readed value.
        /// </summary>
        private List<ISymbol<SymbValue, SymbType>> currentSymbolValues = new List<ISymbol<SymbValue, SymbType>>();

        /// <summary>
        /// Mensagens de erro.
        /// </summary>
        private List<string> errorMessages = new List<string>();

        /// <summary>
        /// Intantiates a new instance of the <see cref="ExpressionReader"/> class.
        /// </summary>
        /// <param name="parser">The expression element's parser.</param>
        public ExpressionReader(IParse<ObjType, SymbValue, SymbType> parser)
        {
            this.parser = parser;
            this.binaryOperators = new Dictionary<SymbType, SOperatorPrecedence<BinaryOperator<ObjType>>>();
            this.unaryOperators = new Dictionary<SymbType, SOperatorPrecedence<UnaryOperator<ObjType>>>();
            this.stateList.Add(new DelegateDrivenState<InputReader, SymbValue, SymbType>(0, "start", this.StartTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, SymbValue, SymbType>(1, "end", this.EndTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, SymbValue, SymbType>(2, "element", this.ElementTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, SymbValue, SymbType>(3, "operator", this.OperatorTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, SymbValue, SymbType>(4, "external delimiters", this.InsideExternalDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, SymbValue, SymbType>(5, "sequence peek", this.SequencePeekDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<InputReader, SymbValue, SymbType>(6, "end", this.InsideSequenceDelimitersTransition));
        }

        #region Public Methods
        /// <summary>
        /// Parses the expresssion provided in a symbol reader.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The parsed object.</returns>
        public ObjType Parse(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            this.errorMessages.Clear();
            StateMachine<InputReader, SymbValue, SymbType> stateMchine = new StateMachine<InputReader, SymbValue, SymbType>(
                this.stateList[0], 
                this.stateList[1]);
            stateMchine.RunMachine(reader);
            if (this.errorMessages.Count > 0)
            {
                var errorBuilder = new StringBuilder();
                errorBuilder.AppendLine("Found the following errors while reading the expression:");
                foreach (var message in this.errorMessages)
                {
                    errorBuilder.AppendLine(message);
                }

                throw new ExpressionReaderException(errorBuilder.ToString());
            }
            else
            {
                if (this.elementStack.Count != 0)
                {
                    return this.elementStack.Pop();
                }
                else
                {
                    throw new ExpressionReaderException("Empty value.");
                }
            }
        }

        public bool TryParse(SymbolReader<InputReader, SymbValue, SymbType> reader, out ObjType result)
        {
            return this.TryParse(reader, null, out result);
        }

        public bool TryParse(SymbolReader<InputReader, SymbValue, SymbType> reader, List<string> errors, out ObjType result)
        {
            result = default(ObjType);
            this.errorMessages.Clear();
            StateMachine<InputReader, SymbValue, SymbType> stateMchine = new StateMachine<InputReader, SymbValue, SymbType>(this.stateList[0], this.stateList[1]);
            stateMchine.RunMachine(reader);
            if (this.errorMessages.Count > 0)
            {
                if (errors != null)
                {
                    foreach (var message in this.errorMessages)
                    {
                        errors.Add(message);
                    }
                }

                return false;
            }
            else
            {
                if (this.elementStack.Count != 0)
                {
                    result = this.elementStack.Pop();
                    return true;
                }
                else
                {
                    if (errors != null)
                    {
                        errors.Add("Empty value.");
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// Registers a binary operator.
        /// </summary>
        /// <param name="opDesignation">The operator name.</param>
        /// <param name="functionOperator">The operator function delegate.</param>
        /// <param name="precedence">The operator's precedence.</param>
        public void RegisterBinaryOperator(SymbType opDesignation, BinaryOperator<ObjType> functionOperator, int precedence)
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
        public void RegisterUnaryOperator(SymbType opDesignation, UnaryOperator<ObjType> functionOperator, int precedence)
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

        /// <summary>
        /// Regista possíveis delimitadores externos.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O respectivo delimitador de fecho.</param>
        public void RegisterExternalDelimiterTypes(SymbType openDelimiter, SymbType closeDelimiter)
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
                var temporary = new List<SymbType>();
                temporary.Add(closeDelimiter);
                this.externalDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        /// <summary>
        /// Regista delimitadores de sequência.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        public void RegisterSequenceDelimiterTypes(SymbType openDelimiter, SymbType closeDelimiter)
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
                var temporary = new List<SymbType>();
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
        public void RegisterExpressionDelimiterTypes(SymbType openDelimiter, SymbType closeDelimiter, UnaryOperator<ObjType> unaryOp)
        {
            if (this.externalDelimitersTypes.ContainsKey(openDelimiter))
            {
                throw new ExpressionReaderException("The specified expression open delimiter was already setup for an external open delimiter.");
            }

            ExpressionCompoundDelimiter<ObjType, SymbType> compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = closeDelimiter, DelimiterOperator = unaryOp };
            if (this.expressionDelimitersTypes.ContainsKey(openDelimiter))
            {
                if (!this.expressionDelimitersTypes[openDelimiter].Contains(compound))
                {
                    this.expressionDelimitersTypes[openDelimiter].Add(compound);
                }
            }
            else
            {
                List<ExpressionCompoundDelimiter<ObjType, SymbType>> temporary = new List<ExpressionCompoundDelimiter<ObjType, SymbType>>();
                temporary.Add(compound);
                this.expressionDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        /// <summary>
        /// Regista delimitadores de expressão que não correspondem a um operador específico.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        public void RegisterExpressionDelimiterTypes(SymbType openDelimiter, SymbType closeDelimiter)
        {
            this.RegisterExpressionDelimiterTypes(openDelimiter, closeDelimiter, null);
        }

        /// <summary>
        /// Adds symbol types that are to be ignored. If an operator is added as a void than it will be ignored.
        /// </summary>
        /// <param name="voidType">The symbol type.</param>
        public void AddVoid(SymbType voidType)
        {
            this.expressionVoids.Add(voidType);
        }
        #endregion

        private bool IsBinaryOperator(SymbType operatorType)
        {
            return this.binaryOperators.ContainsKey(operatorType);
        }

        private bool IsUnaryOperator(SymbType operatorType)
        {
            return this.unaryOperators.ContainsKey(operatorType);
        }

        private bool IsExpressionOpenDelimiter(SymbType operatorType)
        {
            return this.expressionDelimitersTypes.ContainsKey(operatorType);
        }

        private bool IsExpressionCloseDelimiter(SymbType operatorType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = operatorType };
            return (from pair in this.expressionDelimitersTypes
                    where pair.Value.Contains(compound)
                    select pair).Any();
        }

        private bool IsExpressionOpenDelimiterFor(SymbType closeDelimiter, SymbType openDelimiter)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = closeDelimiter };
            return (from res in this.expressionDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(compound)
                    select res).Any();
        }

        private ExpressionCompoundDelimiter<ObjType, SymbType> GetDelegateCompoundForPair(SymbType openDelimiterType, SymbType closeDelimiterType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = closeDelimiterType };
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

        private bool MapOpenDelimiterType(SymbType operatorTypeToMatch, SymbType openDelimiterType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = openDelimiterType };
            if (!this.expressionDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.expressionDelimitersTypes[operatorTypeToMatch].Contains(compound);
        }

        private bool IsExternalOpenDelimiter(SymbType operatorType)
        {
            return this.externalDelimitersTypes.ContainsKey(operatorType);
        }

        private bool IsExternalCloseDelimiter(SymbType operatorType)
        {
            return (from pair in this.externalDelimitersTypes
                    where pair.Value.Contains(operatorType)
                    select pair).Any();
        }

        private bool IsExternalCloseDelimiterFor(SymbType closeDelimiter, SymbType openDelimiter)
        {
            return (from res in this.externalDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(closeDelimiter)
                    select res).Any();
        }

        private bool MapOpenExternalDelimiterType(SymbType operatorTypeToMatch, SymbType openExternalDelimiterType)
        {
            if (!this.externalDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.externalDelimitersTypes[operatorTypeToMatch].Contains(openExternalDelimiterType);
        }

        private bool IsSequenceOpenDelimiter(SymbType operatorType)
        {
            return this.sequenceDelimitersTypes.ContainsKey(operatorType);
        }

        private bool IsSequenceCloseDelimiter(SymbType operatorType)
        {
            return (from pair in this.sequenceDelimitersTypes
                    where pair.Value.Contains(operatorType)
                    select pair).Any();
        }

        private bool IsSequenceCloseDelimiterFor(SymbType closeDelimiter, SymbType openDelimiter)
        {
            return (from res in this.sequenceDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(closeDelimiter)
                    select res).Any();
        }

        private bool MapOpenSequenceDelimiterType(SymbType operatorTypeToMatch, SymbType openExternalDelimiterType)
        {
            if (!this.sequenceDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.externalDelimitersTypes[operatorTypeToMatch].Contains(openExternalDelimiterType);
        }

        private List<ExpressionCompoundDelimiter<ObjType, SymbType>> GetExpressionCloseMatches(SymbType openType)
        {
            try
            {
                return this.expressionDelimitersTypes[openType];
            }
            catch (Exception)
            {
                return new List<ExpressionCompoundDelimiter<ObjType, SymbType>>();
            }
        }

        private List<SymbType> GetExternalCloseMatchcs(SymbType openType)
        {
            try
            {
                return this.externalDelimitersTypes[openType];
            }
            catch (Exception)
            {
                return new List<SymbType>();
            }
        }

        private void IgnoreVoids(SymbolReader<InputReader, SymbValue, SymbType> symbolReader)
        {
            var symbol = symbolReader.Peek();
            while (this.expressionVoids.Contains(symbol.SymbolType))
            {
                symbolReader.Get();
                symbol = symbolReader.Peek();
            }
        }

        private ObjType InvokeBinaryOperator(SymbType operatorType, ObjType left, ObjType right)
        {
            return this.binaryOperators[operatorType].Op.Invoke(left, right);
        }

        private ObjType InvokeUnaryOperator(SymbType operatorType, ObjType obj)
        {
            return this.unaryOperators[operatorType].Op.Invoke(obj);
        }

        /// <summary>
        /// Avalia os elementos contidos na pilha.
        /// </summary>
        /// <param name="closeDelimiterType">O delimitador de fecho anterior à chamada.</param>
        /// <param name="precedence">A precedência de paragem.</param>
        /// <param name="ignorePrecedence">Recebe um valor verdadeiro caso seja para avaliar enquanto existirem
        /// elementos na pilha.
        /// </param>
        /// <returns>Um valor que determina se a avaliação foi bem sucedida.</returns>
        private bool Eval(int precedence, bool ignorePrecedence)
        {
            while (!(this.operatorStack.Count == 0))
            {
                var topOperator = this.operatorStack.Pop();

                if (topOperator.OperatorType == EOperatorType.UNARY)
                {
                    if (this.unaryOperators[topOperator.Symbol].Precedence < precedence && !ignorePrecedence)
                    {
                        this.operatorStack.Push(topOperator);
                        return true;
                    }
                    if (this.elementStack.Count == 0)
                    {
                        this.errorMessages.Add("Internal error.");
                        return false;
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
                        return true;
                    }
                    if (this.elementStack.Count < 2)
                    {
                        this.errorMessages.Add("Internal error.");
                        return false;
                    }

                    ObjType b = this.elementStack.Pop();
                    ObjType a = this.elementStack.Pop();
                    ObjType res = this.InvokeBinaryOperator(topOperator.Symbol, a, b);
                    this.elementStack.Push(res);
                }
                else if (topOperator.OperatorType == EOperatorType.INTERNAL_DELIMITER)
                {
                    this.operatorStack.Push(topOperator);
                    return true;
                }
                else
                {
                    this.errorMessages.Add("Internal error.");
                    return false;
                }
            }

            return true;
        }

        #region State Functions
        /// <summary>
        /// Start transition function - state 0.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, SymbValue, SymbType> StartTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            this.IgnoreVoids(reader);
            if (reader.IsAtEOF())
            {
                return this.stateList[1];
            }

            var readedSymbol = reader.Peek();
            if (this.IsExpressionOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.INTERNAL_DELIMITER));
                reader.Get();
                return this.stateList[0];
            }
            if (this.IsExternalOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.EXTERNAL_DELIMITER));
                this.currentSymbolValues.Add(readedSymbol);
                reader.Get();
                return this.stateList[4];
            }
            if (this.IsExpressionCloseDelimiter(readedSymbol.SymbolType) || this.IsExternalCloseDelimiter(readedSymbol.SymbolType))
            {
                this.errorMessages.Add(string.Format(
                    "Unexpected {0} in expression.",
                    readedSymbol.SymbolValue));
                return this.stateList[1];
            }
            if (this.IsUnaryOperator(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.UNARY));
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
        private IState<InputReader, SymbValue, SymbType> EndTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            return null;
        }

        /// <summary>
        /// Element transition function - state 2.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, SymbValue, SymbType> ElementTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            this.IgnoreVoids(reader);
            if (reader.IsAtEOF())
            {
                this.errorMessages.Add("Unexpected end of expression.");
                return this.stateList[1];
            }

            var readedSymbol = reader.Peek();
            this.currentSymbolValues.Clear();
            if (this.IsExpressionOpenDelimiter(readedSymbol.SymbolType))
            {
                return this.stateList[0];
            }

            if (this.IsExternalOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.EXTERNAL_DELIMITER));
                this.currentSymbolValues.Add(readedSymbol);
                reader.Get();
                return this.stateList[4];
            }

            if (this.IsUnaryOperator(readedSymbol.SymbolType))
            {
                // TODO: verificar no topo da pilha dos operadores se existe algum que esteja marcado como admitindo um sinal unário
                this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.UNARY));
                reader.Get();
                return this.stateList[2];
            }

            if (this.IsBinaryOperator(readedSymbol.SymbolType))
            {
                this.errorMessages.Add("Unexpected binary operator. Binary operators are forbidden after other operators.");
                return this.stateList[1];
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
        private IState<InputReader, SymbValue, SymbType> OperatorTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            this.IgnoreVoids(reader);
            if (reader.IsAtEOF())
            {
                if (this.Eval(0, true))
                {
                    if (this.operatorStack.Count > 0)
                    {
                        this.errorMessages.Add("There are unmatched expression delimiters.");
                    }

                    return this.stateList[1];
                }
                else
                {
                    return this.stateList[1];
                }
            }

            var readedSymbol = reader.Peek();
            if (this.IsBinaryOperator(readedSymbol.SymbolType))
            {
                if (this.Eval(this.binaryOperators[readedSymbol.SymbolType].Precedence, false))
                {
                    this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.BINARY));
                    reader.Get();
                    return this.stateList[2];
                }
                else
                {
                    return this.stateList[1];
                }
            }
            if (this.IsUnaryOperator(readedSymbol.SymbolType))
            {
                this.errorMessages.Add("Unexpected unary operator.");
                return this.stateList[1];
            }
            if (this.IsExpressionCloseDelimiter(readedSymbol.SymbolType))
            {
                if (this.Eval(0, true))
                {
                    if (this.operatorStack.Count == 0)
                    {
                        this.errorMessages.Add(string.Format(
                            "Found close delimiter {0} but no open delimiter is matched.",
                            readedSymbol.SymbolType));
                        return this.stateList[1];
                    }

                    var delimiterType = this.operatorStack.Pop().Symbol;
                    if (!this.IsExpressionOpenDelimiterFor(readedSymbol.SymbolType, delimiterType))
                    {
                        this.errorMessages.Add(string.Format(
                            "Found close delimiter {0} that matches with unmtchable {1} open delimiter",
                            readedSymbol.SymbolType,
                            delimiterType));
                        return this.stateList[1];
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
                else
                {
                    return this.stateList[1];
                }
            }

            this.errorMessages.Add(string.Format(
                "Expected operator but received {0}",
                readedSymbol.SymbolValue));
            return this.stateList[1];
        }

        /// <summary>
        /// Inside external delimiters transition function - state 4.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, SymbValue, SymbType> InsideExternalDelimitersTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            var readedSymbol = reader.Peek();
            if (reader.IsAtEOFSymbol(readedSymbol))
            {
                this.errorMessages.Add("An external delimiter was opened but wasn't closed.");
                return this.stateList[1];
            }
            else if (this.IsExternalOpenDelimiter(readedSymbol.SymbolType))
            {
                this.currentSymbolValues.Add(readedSymbol);
                this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.EXTERNAL_DELIMITER));
            }
            else if (this.IsExternalCloseDelimiter(readedSymbol.SymbolType))
            {
                if (this.operatorStack.Count == 0)
                {
                    this.errorMessages.Add("External delimiters mismatch.");
                    return this.stateList[1];
                }
                else
                {
                    var delimiter = this.operatorStack.Pop();
                    if (delimiter.OperatorType == EOperatorType.INTERNAL_DELIMITER)
                    {
                        this.errorMessages.Add("Delimiters mismatch or confusion with internal delimiters.");
                        return this.stateList[1];
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
                                    this.errorMessages.Add("Can't parse expression.");
                                    return this.stateList[1];
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
                                        this.errorMessages.Add("Can't parse expression.");
                                        return this.stateList[1];
                                    }

                                    return this.stateList[3];
                                }
                            }
                        }
                        else
                        {
                            this.errorMessages.Add("External delimiters mismatch.");
                            return this.stateList[1];
                        }
                    }
                    else
                    {
                        this.errorMessages.Add("External delimiters mismatch.");
                        return this.stateList[1];
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
        private IState<InputReader, SymbValue, SymbType> SequencePeekDelimitersTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            this.IgnoreVoids(reader);
            var readedSymbol = reader.Peek();
            if (this.IsSequenceOpenDelimiter(readedSymbol.SymbolType))
            {
                this.operatorStack.Push(new OperatorDefinition<SymbType>(readedSymbol.SymbolType, EOperatorType.SEQUENCE_DELIMITER));
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
                    this.errorMessages.Add("Can't parse expression.");
                    return this.stateList[1];
                }

                return this.stateList[3];
            }
        }

        /// <summary>
        /// Inside sequence delimiters transition function - state 6.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<InputReader, SymbValue, SymbType> InsideSequenceDelimitersTransition(SymbolReader<InputReader, SymbValue, SymbType> reader)
        {
            var readedSymbol = reader.Peek();
            var delimiterType = this.operatorStack.Pop().Symbol;
            while (!this.IsSequenceCloseDelimiterFor(readedSymbol.SymbolType, delimiterType) && !reader.IsAtEOF())
            {
                readedSymbol = reader.Get();
                this.currentSymbolValues.Add(readedSymbol);
                readedSymbol = reader.Peek();
            }

            if (reader.IsAtEOF())
            {
                this.errorMessages.Add("A sequence delimiter was opened but wasn't closed.");
                return this.stateList[1];
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
                    this.errorMessages.Add("Can't parse expression.");
                    return this.stateList[1];
                }

                return this.stateList[3];
            }
        }
        #endregion
    }
}
