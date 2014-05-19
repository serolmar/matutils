namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// O delegado para operadores binários.
    /// </summary>
    /// <typeparam name="ObjType">O tipo do objecto.</typeparam>
    /// <param name="left">O primeiro argumento do operador.</param>
    /// <param name="right">O segundo argumento do operador.</param>
    /// <returns>O resultado da operação.</returns>
    public delegate ObjType BinaryOperator<ObjType>(ObjType left, ObjType right);

    /// <summary>
    /// O delegado para operadores unários.
    /// </summary>
    /// <typeparam name="ObjType">O tipo do objecto.</typeparam>
    /// <param name="operand">O primeiro argumento do operador.</param>
    /// <returns>O resultado da operação.</returns>
    public delegate ObjType UnaryOperator<ObjType>(ObjType operand);

    /// <summary>
    /// Implementa um leitor de expressões generalizado.
    /// </summary>
    /// <remarks>
    /// O leitor permite o registo de operadores binários e unários, incluindo as precedências. É possível
    /// registar vários tipos de delimitadores que farão parte da expressão e delimitadores, designados por
    /// externos, que envolvem completamente os objectos a serem lidos. Também se encontra definida a noção de
    /// sequência.
    /// </remarks>
    /// <typeparam name="ObjType">O tipo dos objectos que são interpretados.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que constituem as classificações dos símbolos.</typeparam>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os valores dos símbolos.</typeparam>
    public class ExpressionReader<ObjType, SymbValue, SymbType>
    {
        /// <summary>
        /// O leitor de objectos a partir de uma lista de símbolos.
        /// </summary>
        private IParse<ObjType, SymbValue, SymbType> parser;

        /// <summary>
        /// A lista dos estados disponíveis.
        /// </summary>
        private List<IState<SymbValue, SymbType>> stateList = new List<IState<SymbValue, SymbType>>();

        /// <summary>
        /// A lista com os operadores binários e respectivas precedências.
        /// </summary>
        private Dictionary<SymbType, SOperatorPrecedence<BinaryOperator<ObjType>>> binaryOperators;

        /// <summary>
        /// A lista com os operadores unários e respectivas precedências.
        /// </summary>
        private Dictionary<SymbType, SOperatorPrecedence<UnaryOperator<ObjType>>> unaryOperators;

        /// <summary>
        /// O mapeamento dos delimitadores internos.
        /// </summary>
        private Dictionary<SymbType, List<ExpressionCompoundDelimiter<ObjType, SymbType>>> expressionDelimitersTypes = 
            new Dictionary<SymbType, List<ExpressionCompoundDelimiter<ObjType, SymbType>>>();

        /// <summary>
        /// Os mapeamentos dos delimitadores externos.
        /// </summary>
        private Dictionary<SymbType, List<SymbType>> externalDelimitersTypes = new Dictionary<SymbType, List<SymbType>>();

        /// <summary>
        /// Os mapeadores de delimitadores de sequência.
        /// </summary>
        private Dictionary<SymbType, List<SymbType>> sequenceDelimitersTypes = new Dictionary<SymbType, List<SymbType>>();

        /// <summary>
        /// Uma lista de símbolos que serão ignorados.
        /// </summary>
        private List<SymbType> expressionVoids = new List<SymbType>();

        /// <summary>
        /// A pilha de operadores.
        /// </summary>
        private Stack<OperatorDefinition<SymbType>> operatorStack = new Stack<OperatorDefinition<SymbType>>();

        /// <summary>
        /// A pilha de elementos.
        /// </summary>
        private Stack<ObjType> elementStack = new Stack<ObjType>();

        /// <summary>
        /// O valor que é lido.
        /// </summary>
        private List<ISymbol<SymbValue, SymbType>> currentSymbolValues = new List<ISymbol<SymbValue, SymbType>>();

        /// <summary>
        /// Mensagens de erro.
        /// </summary>
        private List<string> errorMessages = new List<string>();

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ExpressionReader"/>.
        /// </summary>
        /// <param name="parser">O leitor responsável pela leitura de elementos.</param>
        public ExpressionReader(IParse<ObjType, SymbValue, SymbType> parser)
        {
            this.parser = parser;
            this.binaryOperators = new Dictionary<SymbType, SOperatorPrecedence<BinaryOperator<ObjType>>>();
            this.unaryOperators = new Dictionary<SymbType, SOperatorPrecedence<UnaryOperator<ObjType>>>();
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(0, "start", this.StartTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(1, "end", this.EndTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(2, "element", this.ElementTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(3, "operator", this.OperatorTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(4, "external delimiters", this.InsideExternalDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(5, "sequence peek", this.SequencePeekDelimitersTransition));
            this.stateList.Add(new DelegateDrivenState<SymbValue, SymbType>(6, "end", this.InsideSequenceDelimitersTransition));
        }

        #region Public Methods

        /// <summary>
        /// Efectua a leitura da expressão a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O objecto lido.</returns>
        /// <exception cref="ExpressionReaderException">Em caso de erro na expressão.</exception>
        public ObjType Parse(ISymbolReader<SymbValue, SymbType> reader)
        {
            this.errorMessages.Clear();
            StateMachine<SymbValue, SymbType> stateMchine = new StateMachine<SymbValue, SymbType>(
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

        /// <summary>
        /// Tenta efectuar a leitura.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="result">A variável que contém o resultado da leitura.</param>
        /// <returns>Verdadeiro se a leitura for bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbolReader<SymbValue, SymbType> reader, out ObjType result)
        {
            return this.TryParse(reader, null, out result);
        }

        /// <summary>
        /// Tenta efectuar a leitura.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="errors">A lista que contém os erros de leitura.</param>
        /// <param name="result">A variável que contém o resultado da leitura.</param>
        /// <returns>Verdadeiro se a leitura for bem-sucedida e falso caso contrário.</returns>
        public bool TryParse(ISymbolReader<SymbValue, SymbType> reader, List<string> errors, out ObjType result)
        {
            result = default(ObjType);
            this.errorMessages.Clear();
            StateMachine<SymbValue, SymbType> stateMchine = new StateMachine<SymbValue, SymbType>(this.stateList[0], this.stateList[1]);
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
        /// Regista um operador binário.
        /// </summary>
        /// <param name="opDesignation">O nome do operador.</param>
        /// <param name="functionOperator">O delegado para a função do operador.</param>
        /// <param name="precedence">A precedência do operador.</param>
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
        /// Regista um operador unário.
        /// </summary>
        /// <param name="opDesignation">O nome do operador.</param>
        /// <param name="functionOperator">O delegado para a função do operador.</param>
        /// <param name="precedence">A precedência do operador.</param>
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
        /// <exception cref="ExpressionReaderException">
        /// Se o delimitador externo estiver a ser utilizado em outros cenários.
        /// </exception>
        public void RegisterExternalDelimiterTypes(SymbType openDelimiter, SymbType closeDelimiter)
        {
            if (this.expressionDelimitersTypes.ContainsKey(openDelimiter))
            {
                throw new ExpressionReaderException(
                    "The specified external open delimiter was already setup for an expression open delimiter.");
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
        /// Regista delimitadores internos â expressão.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        /// <exception cref="ExpressionReaderException">
        /// Se o delimitador se encontra a ser utilizado em outros cenários.
        /// </exception>
        public void RegisterExpressionDelimiterTypes(SymbType openDelimiter, SymbType closeDelimiter, UnaryOperator<ObjType> unaryOp)
        {
            if (this.externalDelimitersTypes.ContainsKey(openDelimiter))
            {
                throw new ExpressionReaderException(
                    "The specified expression open delimiter was already setup for an external open delimiter.");
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
        /// Adiciona tipos de símbolos que serão ignorados.
        /// </summary>
        /// <param name="voidType">O tipo dos símbolos.</param>
        public void AddVoid(SymbType voidType)
        {
            this.expressionVoids.Add(voidType);
        }

        #endregion

        /// <summary>
        /// Determina se o tipo de símbolo se refere a um operador binário.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <returns>
        /// Verdadeiro caso o tipo do símbolo represente um operador binário e falso caso contrário.
        /// </returns>
        private bool IsBinaryOperator(SymbType operatorType)
        {
            return this.binaryOperators.ContainsKey(operatorType);
        }

        /// <summary>
        /// Determina se o tipo de símbolo se refere a um operador unário.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <returns>
        /// Verdadeiro caso o tipo do símbolo represente um operador unário e falso caso contrário.
        /// </returns>
        private bool IsUnaryOperator(SymbType operatorType)
        {
            return this.unaryOperators.ContainsKey(operatorType);
        }

        /// <summary>
        /// Determina se o tipo de símbolo se refere a um delimitador interno de abertura.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <returns>
        /// Verdadeiro caso o tipo do símbolo represente um delimitador interno de abertura e falso caso contrário.
        /// </returns>
        private bool IsExpressionOpenDelimiter(SymbType operatorType)
        {
            return this.expressionDelimitersTypes.ContainsKey(operatorType);
        }

        /// <summary>
        /// Determina se o tipo de símbolo se refere a um delimitador interno de fecho.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <returns>
        /// Verdadeiro caso o tipo do símbolo represente um delimitador interno de fecho e falso caso contrário.
        /// </returns>
        private bool IsExpressionCloseDelimiter(SymbType operatorType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = operatorType };
            return (from pair in this.expressionDelimitersTypes
                    where pair.Value.Contains(compound)
                    select pair).Any();
        }


        /// <summary>
        /// Determina se um delimitador interno de fecho corresponde a um delimitador de abertura.
        /// </summary>
        /// <param name="closeDelimiter">O delimitador interno de fecho.</param>
        /// <param name="openDelimiter">O delimitador interno de abertura.</param>
        /// <returns>
        /// Verdadeiro caso o delimitador interno de fecho corresponda ao de abertura e falso caso contrário.
        /// </returns>
        private bool IsExpressionOpenDelimiterFor(SymbType closeDelimiter, SymbType openDelimiter)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = closeDelimiter };
            return (from res in this.expressionDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(compound)
                    select res).Any();
        }

        /// <summary>
        /// Obtém o delegado para a função que permite lidar com um par de delimitadores.
        /// </summary>
        /// <param name="openDelimiterType">O delimitador de abertura.</param>
        /// <param name="closeDelimiterType">O delimitador de fecho.</param>
        /// <returns>O delegado.</returns>
        private ExpressionCompoundDelimiter<ObjType, SymbType> GetDelegateCompoundForPair(
            SymbType openDelimiterType, 
            SymbType closeDelimiterType)
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

        /// <summary>
        /// Determina se um operador é mapeado por um delimitador de abertura.
        /// </summary>
        /// <param name="operatorTypeToMatch">O operador.</param>
        /// <param name="openDelimiterType">O delimitador de abertura.</param>
        /// <returns>Verdadeiro caso o mapeamento exista e falso caso contrário.</returns>
        private bool MapOpenDelimiterType(SymbType operatorTypeToMatch, SymbType openDelimiterType)
        {
            var compound = new ExpressionCompoundDelimiter<ObjType, SymbType>() { DelimiterType = openDelimiterType };
            if (!this.expressionDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.expressionDelimitersTypes[operatorTypeToMatch].Contains(compound);
        }

        /// <summary>
        /// Determina se o tipo de símbolo se refere a um delimitador externo de abertura.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <returns>
        /// Verdadeiro caso o tipo do símbolo represente um delimitador externo de abertura e falso caso contrário.
        /// </returns>
        private bool IsExternalOpenDelimiter(SymbType operatorType)
        {
            return this.externalDelimitersTypes.ContainsKey(operatorType);
        }

        /// <summary>
        /// Determina se o tipo de símbolo se refere a um delimitador externo de fecho.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <returns>
        /// Verdadeiro caso o tipo do símbolo represente um delimitador externo de fecho e falso caso contrário.
        /// </returns>
        private bool IsExternalCloseDelimiter(SymbType operatorType)
        {
            return (from pair in this.externalDelimitersTypes
                    where pair.Value.Contains(operatorType)
                    select pair).Any();
        }

        /// <summary>
        /// Determina se um delimitador externo de fecho corresponde a um delimitador de abertura.
        /// </summary>
        /// <param name="closeDelimiter">O delimitador externo de fecho.</param>
        /// <param name="openDelimiter">O delimitador externo de abertura.</param>
        /// <returns>
        /// Verdadeiro caso o delimitador externo de fecho corresponda ao de abertura e falso caso contrário.
        /// </returns>
        private bool IsExternalCloseDelimiterFor(SymbType closeDelimiter, SymbType openDelimiter)
        {
            return (from res in this.externalDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(closeDelimiter)
                    select res).Any();
        }

        /// <summary>
        /// Determina se um operador é mapeado por um delimitador externo de abertura.
        /// </summary>
        /// <param name="operatorTypeToMatch">O operador.</param>
        /// <param name="openDelimiterType">O delimitador externo de abertura.</param>
        /// <returns>Verdadeiro caso o mapeamento exista e falso caso contrário.</returns>
        private bool MapOpenExternalDelimiterType(SymbType operatorTypeToMatch, SymbType openExternalDelimiterType)
        {
            if (!this.externalDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.externalDelimitersTypes[operatorTypeToMatch].Contains(openExternalDelimiterType);
        }

        /// <summary>
        /// Determina se se trata de um operador de abertura sequência.
        /// </summary>
        /// <param name="operatorType">O tipo de operador.</param>
        /// <returns>Verdadeiro caso se trate de um operador de abertura de sequência e falso caso contrário.</returns>
        private bool IsSequenceOpenDelimiter(SymbType operatorType)
        {
            return this.sequenceDelimitersTypes.ContainsKey(operatorType);
        }

        /// <summary>
        /// Determina se se trata de um operador de fecho de sequência.
        /// </summary>
        /// <param name="operatorType">O tipo de operador.</param>
        /// <returns>Verdadeiro caso se trate de um operador de fecho de sequência e falso caso contrário.</returns>
        private bool IsSequenceCloseDelimiter(SymbType operatorType)
        {
            return (from pair in this.sequenceDelimitersTypes
                    where pair.Value.Contains(operatorType)
                    select pair).Any();
        }

        /// <summary>
        /// Determina se o operador de sequência de fecho é mapeado pelo delimitador de sequência de abertura.
        /// </summary>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <returns>Verdadeiro caso o mapeamento exista e falso caso contrário.</returns>
        private bool IsSequenceCloseDelimiterFor(SymbType closeDelimiter, SymbType openDelimiter)
        {
            return (from res in this.sequenceDelimitersTypes
                    where res.Key.Equals(openDelimiter) && res.Value.Contains(closeDelimiter)
                    select res).Any();
        }

        /// <summary>
        /// Derifica se o operador mapeia um delimitador de sequência de abertura.
        /// </summary>
        /// <param name="operatorTypeToMatch">O operador.</param>
        /// <param name="openExternalDelimiterType">O delimitador de sequência de abertura.</param>
        /// <returns>Verdadeiro caso o mapeamento exista e falso caso contrário.</returns>
        private bool MapOpenSequenceDelimiterType(SymbType operatorTypeToMatch, SymbType openExternalDelimiterType)
        {
            if (!this.sequenceDelimitersTypes.ContainsKey(operatorTypeToMatch))
            {
                return false;
            }
            return this.externalDelimitersTypes[operatorTypeToMatch].Contains(openExternalDelimiterType);
        }

        /// <summary>
        /// Determina todos os delimitadores internos de fecho para o delimitador interno de abertura.
        /// </summary>
        /// <param name="openType">O delimitador interno de abertura.</param>
        /// <returns>Os delimitadores internos de fecho.</returns>
        private List<ExpressionCompoundDelimiter<ObjType, SymbType>> GetExpressionCloseMatches(SymbType openType)
        {
            var result = default(List<ExpressionCompoundDelimiter<ObjType, SymbType>>);
            if (!this.expressionDelimitersTypes.TryGetValue(openType, out result))
            {
                result = new List<ExpressionCompoundDelimiter<ObjType, SymbType>>();
            }

            return result;
        }

        /// <summary>
        /// Determina todos os delimitadores externos de fecho para o delimitador externo de abertura.
        /// </summary>
        /// <param name="openType">O delimitador externo de abertura.</param>
        /// <returns>Os delimitadores externos de fecho.</returns>
        private List<SymbType> GetExternalCloseMatchcs(SymbType openType)
        {
            var result = default(List<SymbType>);
            if (!this.externalDelimitersTypes.TryGetValue(openType, out result))
            {
                result = new List<SymbType>();
            }

            return result;
        }

        /// <summary>
        /// Ignora todos os símbolos que estão marcados como vazios.
        /// </summary>
        /// <param name="symbolReader">O leitor de símbolos.</param>
        private void IgnoreVoids(ISymbolReader<SymbValue, SymbType> symbolReader)
        {
            var symbol = symbolReader.Peek();
            while (this.expressionVoids.Contains(symbol.SymbolType))
            {
                symbolReader.Get();
                symbol = symbolReader.Peek();
            }
        }

        /// <summary>
        /// Inovca o operador binário.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <param name="left">O primeiro argumento.</param>
        /// <param name="right">O segundo argumento.</param>
        /// <returns>O resultado da operação.</returns>
        private ObjType InvokeBinaryOperator(SymbType operatorType, ObjType left, ObjType right)
        {
            return this.binaryOperators[operatorType].Op.Invoke(left, right);
        }

        /// <summary>
        /// Inovca o operador unário.
        /// </summary>
        /// <param name="operatorType">O tipo do operador.</param>
        /// <param name="left">O argumento.</param>
        /// <returns>O resultado da operação.</returns>
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
        /// A função correspondente à transição inicial - estado 0.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState< SymbValue, SymbType> StartTransition(ISymbolReader<SymbValue, SymbType> reader)
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
        /// A função correspondente à transição final - esatado 1.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState< SymbValue, SymbType> EndTransition(ISymbolReader<SymbValue, SymbType> reader)
        {
            return null;
        }

        /// <summary>
        /// A função correspondente à transição de elemento - estado 2.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState< SymbValue, SymbType> ElementTransition(ISymbolReader<SymbValue, SymbType> reader)
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
        /// A função correspondente à transição de operador - estado 3.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState< SymbValue, SymbType> OperatorTransition(ISymbolReader<SymbValue, SymbType> reader)
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
        /// A função correspondente à transição de delimitadores externos - estado 4.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState< SymbValue, SymbType> InsideExternalDelimitersTransition(ISymbolReader<SymbValue, SymbType> reader)
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
        /// A função correspondente à transição de sequência - estado 5.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState< SymbValue, SymbType> SequencePeekDelimitersTransition(ISymbolReader<SymbValue, SymbType> reader)
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
        /// A função correspondente à transição no interior de delimitadores - estado 6.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState< SymbValue, SymbType> InsideSequenceDelimitersTransition(ISymbolReader<SymbValue, SymbType> reader)
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
