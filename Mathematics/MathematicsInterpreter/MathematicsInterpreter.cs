using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utilities;

namespace Mathematics.MathematicsInterpreter
{
    public class MathematicsInterpreter
    {
        private static string[] voids = new[] { "space", "new_line", "tab", "carriage_return" };

        /// <summary>
        /// The expression reader.
        /// </summary>
        private ExpressionReader<AMathematicsObject, string, string> expressionReader;

        /// <summary>
        /// The mathematis interpreter mediator.
        /// </summary>
        private MathematicsInterpreterMediator mediator;

        /// <summary>
        /// The list of available states.
        /// </summary>
        private List<IState<string, string>> stateList = new List<IState<string, string>>();

        /// <summary>
        /// The current state.
        /// </summary>
        private IState<string, string> currentState;

        /// <summary>
        /// The stack with the scope definition.
        /// </summary>
        private Stack<ScopeDefinition> scopeDefinitionStack = new Stack<ScopeDefinition>();

        /// <summary>
        /// The stack with the parenthesis.
        /// </summary>
        private Stack<string> openParenthesisStack = new Stack<string>();

        /// <summary>
        /// Maintains the current item readed.
        /// </summary>
        private string currentItem;

        /// <summary>
        /// Maintains the current result.
        /// </summary>
        private MathematicsInterpreterResult currentResult;

        /// <summary>
        /// Maintains a reference for the last expression result.
        /// </summary>
        private AMathematicsObject lastExpressionResult;

        public MathematicsInterpreter()
        {
            this.currentResult = new MathematicsInterpreterResult();
            this.mediator = new MathematicsInterpreterMediator();
            this.expressionReader = this.PrepareExpressionReader();
            this.InitStates();
            this.Reset();
        }

        public MathematicsInterpreterResult Interpret(TextReader reader, TextWriter outputWriter)
        {
            var symbolReader = this.PrepareSymbolReader(reader);
            var stateMachine = new StateMachine<string, string>(this.currentState, this.stateList[1]);
            stateMachine.RunMachine(symbolReader);
            return this.currentResult;
        }

        public void Reset()
        {
            this.currentState = this.stateList[0];
            this.openParenthesisStack.Clear();
            this.scopeDefinitionStack.Clear();
            this.scopeDefinitionStack.Push(new ScopeDefinition() { ScopeType = EScopeType.MAIN });
        }

        private ExpressionReader<AMathematicsObject, string, string> PrepareExpressionReader()
        {
            var result = new ExpressionReader<AMathematicsObject, string, string>(new MathematicsObjectParser(this.mediator));

            result.RegisterBinaryOperator("equal", this.Assign, 0);
            result.RegisterBinaryOperator("plus", this.Add, 2);
            result.RegisterBinaryOperator("times", this.Multiply, 3);
            result.RegisterBinaryOperator("minus", this.Subtract, 2);
            result.RegisterBinaryOperator("over", this.Divide, 3);
            result.RegisterUnaryOperator("minus", this.Symmetric, 2);
            result.RegisterBinaryOperator("hat", this.Exponentiate, 4);

            result.RegisterBinaryOperator("double_equal", this.AreEqual, 1);
            result.RegisterBinaryOperator("great_than", this.GreaterThan, 1);
            result.RegisterBinaryOperator("great_equal", this.GreaterOrEqualThan, 1);
            result.RegisterBinaryOperator("less_than", this.LesserThan, 1);
            result.RegisterBinaryOperator("less_equal", this.LesserOrEqualThan, 1);

            result.RegisterBinaryOperator("double_or", this.LogicalDisjunction, 2);
            result.RegisterBinaryOperator("double_and", this.LogicalConjunction, 3);

            result.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
            result.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
            result.RegisterSequenceDelimiterTypes("left_bracket", "right_bracket");
            result.RegisterExternalDelimiterTypes("left_bracket", "right_bracket");
            result.RegisterExternalDelimiterTypes("left_brace", "right_brace");
            result.RegisterExternalDelimiterTypes("double_quote", "double_quote");

            result.AddVoid("blancks");

            return result;
        }

        private StringSymbolReader PrepareSymbolReader(TextReader reader)
        {
            var result = new StringSymbolReader(reader, false);
            result.RegisterKeyWordType("while", "while");
            result.RegisterKeyWordType("if", "if");
            result.RegisterKeyWordType("for", "for");
            result.RegisterKeyWordType("else", "else");
            result.RegisterKeyWordType("true", "boolean");
            result.RegisterKeyWordType("false", "boolean");
            return result;
        }

        private void InitStates()
        {
            this.stateList.Add(new DelegateDrivenState<string, string>(1, "Start", this.StartTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(2, "End", this.EndTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(3, "While", this.WhileTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(4, "If", this.IfTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(5, "Else", this.ElseTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(6, "WhileIfCondition", this.WhileIfConditionTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(7, "For", this.ForTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(8, "FirstForCondition", this.FirstForConditionTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(9, "SecondForCondition", this.SecondForConditionTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(10, "ThirdForCondition", this.ThirdForConditionTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(11, "InsideWhileCondition", this.InsideWhileIfConditionTransition));
            this.stateList.Add(new DelegateDrivenState<string, string>(12, "InsideNonExecutingScope", this.InsideNonExecutingScope));
        }

        private void IgnoreVoids(ISymbolReader<string, string> symbolReader)
        {
            ISymbol<string, string> symbol = symbolReader.Peek();
            while (voids.Contains(symbol.SymbolType))
            {
                symbolReader.Get();
                symbol = symbolReader.Peek();
            }
        }

        private IState<string, string> SetupErrorAndQuit(string message)
        {
            this.currentResult.InterpreterMessageStatus = EMathematicsInterpreterStatus.ERROR;
            this.currentResult.InterpreterResultMessage = message;
            this.Reset();
            return this.stateList[1];
        }

        #region Mathematics Interpreter Operator
        private AMathematicsObject Assign(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            if (leftVariable.MathematicsType != EMathematicsType.NAME)
            {
                throw new ExpressionInterpreterException("All assigns must be attributed to a name.");
            }
            else
            {
                return new AssignMathematicsObject(leftVariable, rightValue, this.mediator);
            }
        }

        private AMathematicsObject Add(AMathematicsObject leftValue, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject Subtract(AMathematicsObject leftValue, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject Multiply(AMathematicsObject leftValue, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject Divide(AMathematicsObject leftValue, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject Exponentiate(AMathematicsObject leftValue, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject Symmetric(AMathematicsObject value)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject AreEqual(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject GreaterThan(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject GreaterOrEqualThan(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject LesserThan(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject LesserOrEqualThan(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject LogicalDisjunction(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }

        private AMathematicsObject LogicalConjunction(AMathematicsObject leftVariable, AMathematicsObject rightValue)
        {
            throw new NotImplementedException();
        }
        #endregion Mathematics Interpreter Operator

        #region State Transition Functions
        /// <summary>
        /// Função de transição inicial - estado 0.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> StartTransition(ISymbolReader<string, string> reader)
        {
            var readed = reader.Get();
            var topScopeDefinition = this.scopeDefinitionStack.Pop();
            if (readed.SymbolType == "eof")
            {
                if (!string.IsNullOrEmpty(this.currentItem))
                {
                    var symbolReader = new StringSymbolReader(new StringReader(this.currentItem), false);
                    this.lastExpressionResult = this.expressionReader.Parse(symbolReader);
                    this.currentItem = string.Empty;
                    if (this.lastExpressionResult.IsFunctionObject)
                    {
                        ((AMathematicsFunctionObject)this.lastExpressionResult).Evaulate();
                    }
                }

                if (topScopeDefinition.ScopeType != EScopeType.MAIN)
                {
                    this.currentResult.InterpreterMessageStatus = EMathematicsInterpreterStatus.WAITING;
                    this.currentResult.InterpreterResultMessage = "Sentence is incomplete. Scope delimiters don't match.";
                    if (topScopeDefinition.ScopeType == EScopeType.WHILE)
                    {
                        var whileCondition = topScopeDefinition.ConditionExpression;
                        if (whileCondition.AssertCondition())
                        {
                            (reader as StringSymbolReader).RestoreToMemento(topScopeDefinition.ScopeStartMemento);
                        }
                    }
                    else if (topScopeDefinition.ScopeType == EScopeType.FOR)
                    {
                        var updateExpression = topScopeDefinition.UpdateExpression;
                        updateExpression.Evaulate();
                        var forCondition = topScopeDefinition.ConditionExpression;
                        if (forCondition.AssertCondition())
                        {
                            (reader as StringSymbolReader).RestoreToMemento(topScopeDefinition.ScopeStartMemento);
                        }
                    }

                    this.scopeDefinitionStack.Push(topScopeDefinition);

                    return this.stateList[1];
                }
                else
                {
                    this.currentResult.InterpreterMessageStatus = EMathematicsInterpreterStatus.COMPLETED;
                    this.currentResult.InterpreterResultMessage = this.lastExpressionResult.ToString();
                    return this.stateList[1];
                }
            }
            else if (readed.SymbolType == "semi_colon")
            {
                var symbolReader = new StringSymbolReader(new StringReader(this.currentItem), false);
                this.lastExpressionResult = this.expressionReader.Parse(symbolReader);
                this.currentItem = string.Empty;
                if (this.lastExpressionResult.IsFunctionObject)
                {
                    ((AMathematicsFunctionObject)this.lastExpressionResult).Evaulate();
                }

                if (topScopeDefinition.ScopeType == EScopeType.WHILE)
                {
                    var whileCondition = topScopeDefinition.ConditionExpression;
                    if (whileCondition.AssertCondition())
                    {
                        (reader as StringSymbolReader).RestoreToMemento(topScopeDefinition.ScopeStartMemento);
                    }
                }
                else if (topScopeDefinition.ScopeType == EScopeType.FOR)
                {
                    var updateExpression = topScopeDefinition.UpdateExpression;
                    updateExpression.Evaulate();
                    var forCondition = topScopeDefinition.ConditionExpression;
                    if (forCondition.AssertCondition())
                    {
                        (reader as StringSymbolReader).RestoreToMemento(topScopeDefinition.ScopeStartMemento);
                    }
                }

                this.scopeDefinitionStack.Push(topScopeDefinition);
            }
            else if (readed.SymbolType == "left_brace")
            {
                var innerScope = new ScopeDefinition() { ScopeType = EScopeType.INNER };
                this.scopeDefinitionStack.Push(topScopeDefinition);
                this.scopeDefinitionStack.Push(innerScope);
            }
            else if (readed.SymbolType == "right_brace")
            {
                if (topScopeDefinition.ScopeType == EScopeType.MAIN)
                {
                    return this.SetupErrorAndQuit("Scope delimiters don't match.");
                }
                else if (topScopeDefinition.ScopeType == EScopeType.INNER)
                {
                    return this.stateList[0];
                }
                else if (topScopeDefinition.ScopeType == EScopeType.WHILE)
                {
                    var whileCondition = topScopeDefinition.ConditionExpression;
                    if (whileCondition.AssertCondition())
                    {
                        (reader as StringSymbolReader).RestoreToMemento(topScopeDefinition.ScopeStartMemento);
                        this.scopeDefinitionStack.Push(topScopeDefinition);
                    }
                }
                else if (topScopeDefinition.ScopeType == EScopeType.FOR)
                {
                    var updateExpression = topScopeDefinition.UpdateExpression;
                    updateExpression.Evaulate();
                    var forCondition = topScopeDefinition.ConditionExpression;
                    if (forCondition.AssertCondition())
                    {
                        (reader as StringSymbolReader).RestoreToMemento(topScopeDefinition.ScopeStartMemento);
                        this.scopeDefinitionStack.Push(topScopeDefinition);
                    }
                }
            }
            else if (readed.SymbolType == "while")
            {
                this.scopeDefinitionStack.Push(topScopeDefinition);
                this.scopeDefinitionStack.Push(new ScopeDefinition() { ScopeType = EScopeType.WHILE });
                return this.stateList[5];
            }
            else if (readed.SymbolType == "if")
            {
                this.scopeDefinitionStack.Push(topScopeDefinition);
                this.scopeDefinitionStack.Push(new ScopeDefinition() { ScopeType = EScopeType.IF_ELSE });
                return this.stateList[5];
            }
            else if (readed.SymbolType == "for")
            {
                this.scopeDefinitionStack.Push(topScopeDefinition);
                this.scopeDefinitionStack.Push(new ScopeDefinition() { ScopeType = EScopeType.FOR });
                return this.stateList[7];
            }
            else if (readed.SymbolType == "else")
            {
                this.scopeDefinitionStack.Push(topScopeDefinition);
                return this.SetupErrorAndQuit("Unexpected word else.");
            }
            else
            {
                this.scopeDefinitionStack.Push(topScopeDefinition);
                this.currentItem += readed.SymbolValue;
            }

            return this.stateList[0];
        }

        /// <summary>
        /// End transition function - state 1.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> EndTransition(ISymbolReader<string, string> reader)
        {
            return null;
        }

        /// <summary>
        /// While transition function - state 2.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> WhileTransition(ISymbolReader<string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Peek();
            var topScopeDefinition = this.scopeDefinitionStack.Pop();
            if (readed.SymbolType == "eof")
            {
                this.currentResult.InterpreterMessageStatus = EMathematicsInterpreterStatus.WAITING;
                this.currentResult.InterpreterResultMessage = "Incomplete expression.";
                return this.stateList[1];
            }
            else if (readed.SymbolType == "left_brace")
            {
                reader.Get();
                topScopeDefinition.ExecuteInScope = topScopeDefinition.ConditionExpression.AssertCondition();
                topScopeDefinition.HasBraces = true;
                if (topScopeDefinition.ExecuteInScope)
                {
                    topScopeDefinition.ScopeStartMemento = (reader as MementoSymbolReader<CharSymbolReader<string>, string, string>).SaveToMemento();
                    this.scopeDefinitionStack.Push(topScopeDefinition);
                }

                return this.stateList[0];
            }
            else if (readed.SymbolType == "semi_colon")
            {
                topScopeDefinition.ExecuteInScope = topScopeDefinition.ConditionExpression.AssertCondition();
                if (topScopeDefinition.ExecuteInScope)
                {
                    this.scopeDefinitionStack.Push(topScopeDefinition);
                    return this.stateList[2];
                }
                else
                {
                    return this.stateList[0];
                }
            }
            else
            {
                topScopeDefinition.ExecuteInScope = topScopeDefinition.ConditionExpression.AssertCondition();
                if (topScopeDefinition.ExecuteInScope)
                {
                    topScopeDefinition.ScopeStartMemento = (reader as MementoSymbolReader<CharSymbolReader<string>, string, string>).SaveToMemento();
                    this.scopeDefinitionStack.Push(topScopeDefinition);
                    return this.stateList[0];
                }
                else
                {
                    this.scopeDefinitionStack.Push(topScopeDefinition);
                    return this.stateList[11];
                }

            }
        }

        /// <summary>
        /// If transition function - state 3.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> IfTransition(ISymbolReader<string, string> reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Else transition function - state 4.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> ElseTransition(ISymbolReader<string, string> reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// While and If transition function - state 5.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> WhileIfConditionTransition(ISymbolReader<string, string> reader)
        {
            this.IgnoreVoids(reader);
            var readed = reader.Get();
            if (readed.SymbolType == "eof")
            {
                this.currentResult.InterpreterMessageStatus = EMathematicsInterpreterStatus.WAITING;
                this.currentResult.InterpreterResultMessage = "Sentence is incomplete.";
                return this.stateList[1];
            }
            else if (readed.SymbolType == "left_parenthesis")
            {
                return this.stateList[10];
            }
            else
            {
                return this.SetupErrorAndQuit("Expecting parenthesis after word while.");
            }
        }

        /// <summary>
        /// For transition function - state 6.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> ForTransition(ISymbolReader<string, string> reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// First For transition function - state 7.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> FirstForConditionTransition(ISymbolReader<string, string> reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Second For transition function - state 8.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> SecondForConditionTransition(ISymbolReader<string, string> reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Third For transition function - state 9.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> ThirdForConditionTransition(ISymbolReader<string, string> reader)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// End transition function - state 10.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> InsideWhileIfConditionTransition(ISymbolReader<string, string> reader)
        {
            var readed = reader.Get();
            if (readed.SymbolType == "eof")
            {
                this.currentResult.InterpreterMessageStatus = EMathematicsInterpreterStatus.WAITING;
                this.currentResult.InterpreterResultMessage = "Expecting expression.";
                return this.stateList[1];
            }
            else if (readed.SymbolType == "left_parenthesis")
            {
                this.openParenthesisStack.Push("left_parenthesis");
                this.currentItem += readed.SymbolValue;
            }
            else if (readed.SymbolType == "right_parenthesis")
            {
                if (this.openParenthesisStack.Count == 0)
                {
                    var topScopeDefinition = this.scopeDefinitionStack.Pop();
                    var readedExpression = this.expressionReader.Parse(new StringSymbolReader(new StringReader(this.currentItem), false));
                    this.currentItem = string.Empty;
                    if (readedExpression.MathematicsType == EMathematicsType.CONDITION)
                    {
                        topScopeDefinition.ConditionExpression = readedExpression as BooleanConditionMathematicsObject;
                        this.scopeDefinitionStack.Push(topScopeDefinition);
                        if (topScopeDefinition.ScopeType == EScopeType.WHILE)
                        {
                            return this.stateList[2];
                        }
                        else if (topScopeDefinition.ScopeType == EScopeType.IF_ELSE)
                        {
                            return this.stateList[3];
                        }
                    }
                    else
                    {
                        this.SetupErrorAndQuit("Expecting a condition expression after while.");
                        return this.stateList[1];
                    }
                }
                else
                {
                    var topOperator = this.openParenthesisStack.Pop();
                    if (topOperator != "left_parenthesis")
                    {
                        this.openParenthesisStack.Push(topOperator);
                    }

                    this.currentItem += readed.SymbolValue;
                }
            }
            else if (readed.SymbolType == "left_bracket")
            {
                this.currentItem += readed.SymbolValue;
                this.openParenthesisStack.Push("left_bracket");
            }
            else if (readed.SymbolType == "right_bracket")
            {
                var topOperator = this.openParenthesisStack.Pop();
                if (topOperator != "left_bracket")
                {
                    this.openParenthesisStack.Push(topOperator);
                }

                this.currentItem += readed.SymbolValue;
            }
            else if (readed.SymbolType == "left_brace")
            {
                this.currentItem += readed.SymbolValue;
                this.openParenthesisStack.Push("left_brace");
            }
            else if (readed.SymbolType == "right_brace")
            {
                var topOperator = this.openParenthesisStack.Pop();
                if (topOperator != "left_brace")
                {
                    this.openParenthesisStack.Push(topOperator);
                }

                this.currentItem += readed.SymbolValue;
            }
            else
            {
                this.currentItem += readed.SymbolValue;
            }

            return this.stateList[10];
        }

        /// <summary>
        /// Inside non executing scope transition function - state 11.
        /// </summary>
        /// <param name="reader">The symbol reader.</param>
        /// <returns>The next state.</returns>
        private IState<string, string> InsideNonExecutingScope(ISymbolReader<string, string> reader)
        {
            throw new NotImplementedException();
        }
        #endregion State Transition Functions
    }
}
