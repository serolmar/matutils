namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// Identifica os tiops de palavras utilizadas pelo construtor de expressões.
    /// </summary>
    public enum ELambdaExpressionWordType
    {
        /// <summary>
        /// Valor numérico.
        /// </summary>
        NUMERIC = 0,

        /// <summary>
        /// Valor alfabético.
        /// </summary>
        ALPHA = 1,

        /// <summary>
        /// Espaço.
        /// </summary>
        SPACE = 2,

        /// <summary>
        /// Parêntesis de abertura.
        /// </summary>
        OPEN_PARENTHESIS = 3,

        /// <summary>
        /// Parêntesis de fecho.
        /// </summary>
        CLOSE_PARENTHESIS = 4,

        /// <summary>
        /// Delimitador de abertura.
        /// </summary>
        DELIMITER = 5,

        /// <summary>
        /// A vírgula.
        /// </summary>
        COMMA = 6,

        /// <summary>
        /// Outro tipo de elementos.
        /// </summary>
        OTHER = 7,

        /// <summary>
        /// O final de ficheiro.
        /// </summary>
        EOF = 8
    }

    /// <summary>
    /// Permite construir uma expressão lambda com base num filtro escrito numa linguagem
    /// próxima do natural.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objecto.</typeparam>
    public class SmartFilterLambdaBuilder<ObjectType>
    {
        /// <summary>
        /// O conjunto de carácteres especiais que não podem fazer parte do nome
        ///  de um operador.
        /// </summary>
        private static char[] specialChars = new[] { '(', '"', ')', ',', '.', '[', ']', ' ', '\n', '\r' };

        /// <summary>
        /// O operador de disjunção.
        /// </summary>
        private string disjunctionOperatorName;

        /// <summary>
        /// O operador de conjunção.
        /// </summary>
        private string conjunctionOperatorName;

        /// <summary>
        /// O operador de negação.
        /// </summary>
        private string negationOperatorName;

        /// <summary>
        /// O registo de todos os operadores internos.
        /// </summary>
        private Dictionary<string, Func<Object, bool>> internalOperators;

        /// <summary>
        /// Mantém a lista dos estados utilizados pela máquina de estados.
        /// </summary>
        private List<IState<string, ELambdaExpressionWordType>> stateList;

        /// <summary>
        /// Mantém a pilha das sub-expressões.
        /// </summary>
        private Stack<Expression<Func<ObjectType, bool>>> expressionStack;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SmartFilterLambdaBuilder{ObjectType}"/>.
        /// </summary>
        /// <param name="disjunctionOperatorName">O nome do operador de disjunção.</param>
        /// <param name="conjunctionOperatorName">O nome do operador de conjunção.</param>
        /// <param name="negationOperatorName">O nome do operador de negação.</param>
        public SmartFilterLambdaBuilder(
            string disjunctionOperatorName,
            string conjunctionOperatorName,
            string negationOperatorName)
        {
            if (string.IsNullOrWhiteSpace(disjunctionOperatorName))
            {
                throw new ArgumentException("The disjunction operator name must not be empty.");
            }
            else if (string.IsNullOrWhiteSpace(conjunctionOperatorName))
            {
                throw new ArgumentException("The conjunction operator name must not be empty.");
            }
            else if (string.IsNullOrWhiteSpace(negationOperatorName))
            {
                throw new ArgumentException("The negation operator name must not be empty.");
            }
            else
            {
                this.ValidateOperatorName(disjunctionOperatorName);
                this.ValidateOperatorName(conjunctionOperatorName);
                this.ValidateOperatorName(negationOperatorName);
                this.disjunctionOperatorName = disjunctionOperatorName;
                this.conjunctionOperatorName = conjunctionOperatorName;
                this.negationOperatorName = negationOperatorName;

                this.internalOperators = new Dictionary<string, Func<object, bool>>();
                this.expressionStack = new Stack<Expression<Func<ObjectType, bool>>>();
            }
        }

        /// <summary>
        /// Costrói uma expressão que actua sobre o objecto.
        /// </summary>
        /// <param name="pattern">O filtro de pesquisa.</param>
        /// <returns>A expressão resultante.</returns>
        public Expression<Func<ObjectType, bool>> BuildExpressionTree(string pattern)
        {
            var patternReader = this.GetPatternReader(pattern);
            var stateMachine = new StateMachine<string, ELambdaExpressionWordType>(
                this.stateList[0],
                this.stateList[1]);
            stateMachine.RunMachine(patternReader);
            if (this.expressionStack.Count == 0)
            {
                // Retorna a expressão vazia
                var parameterExpression = LambdaExpression.Parameter(
                    typeof(ObjectType));
                var body = LambdaExpression.Constant(true);
                var result = LambdaExpression.Lambda<Func<ObjectType, bool>>(
                    body,
                    new[] { parameterExpression });
                return result;
            }
            else
            {
                var top = this.expressionStack.Pop();
                var parameterExpression = LambdaExpression.Parameter(
                    typeof(ObjectType));
                var result = LambdaExpression.Lambda<Func<ObjectType, bool>>(
                    top,
                    new[] { parameterExpression });
                return result;
            }
        }

        /// <summary>
        /// Regista os operadores internos.
        /// </summary>
        /// <param name="operatorName">O nome do operador.</param>
        /// <param name="optr">O operador.</param>
        public void RegisterInternalOperator(string operatorName, Func<Object, bool> optr)
        {
            if (optr == null)
            {
                throw new ArgumentNullException("optr");
            }
            else
            {
                this.ValidateOperatorName(operatorName);
                if (this.internalOperators.ContainsKey(operatorName))
                {
                    this.internalOperators[operatorName] = optr;
                }
                else
                {
                    this.internalOperators.Add(operatorName, optr);
                }
            }
        }

        /// <summary>
        /// Remove o registo do operador interno especificado.
        /// </summary>
        /// <param name="operatorName">O nome do operador interno a ser removido.</param>
        public void UnregisterInternalOperator(string operatorName)
        {
            this.internalOperators.Remove(operatorName);
        }

        /// <summary>
        /// Remove todos os operadores internos registados.
        /// </summary>
        public void ClearInternalOperators()
        {
            this.internalOperators.Clear();
        }

        /// <summary>
        /// Inicializa os estados relativos à máquina de estados.
        /// </summary>
        private void InitStates()
        {
            this.stateList = new List<IState<string, ELambdaExpressionWordType>>();
            this.stateList.Add(new DelegateDrivenState<string, ELambdaExpressionWordType>(
                0, 
                "StartTransition", 
                this.StartTransition));
            this.stateList.Add(new DelegateDrivenState<string, ELambdaExpressionWordType>(
                1, 
                "EndTransition", 
                this.EndTransition));


            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém um leitor de símbolos a partir do texto que representa o padrão.
        /// </summary>
        /// <param name="pattern">O padrão.</param>
        /// <returns>O leitor de símbolos.</returns>
        private SimpleTextSymbolReader<ELambdaExpressionWordType> GetPatternReader(
            string pattern)
        {
            var innerPattern = pattern == null ? string.Empty : pattern;
            var reader = new StringReader(pattern);
            var charSymbolReader = new CharSymbolReader<ELambdaExpressionWordType>(
                reader,
                ELambdaExpressionWordType.OTHER,
                ELambdaExpressionWordType.EOF);
            charSymbolReader.RegisterCharRangeType('a', 'z', ELambdaExpressionWordType.ALPHA);
            charSymbolReader.RegisterCharRangeType('A', 'Z', ELambdaExpressionWordType.ALPHA);
            charSymbolReader.RegisterCharRangeType('0', '9', ELambdaExpressionWordType.NUMERIC);
            charSymbolReader.RegisterCharType('(', ELambdaExpressionWordType.OPEN_PARENTHESIS);
            charSymbolReader.RegisterCharType('"', ELambdaExpressionWordType.DELIMITER);
            charSymbolReader.RegisterCharType(')', ELambdaExpressionWordType.CLOSE_PARENTHESIS);
            charSymbolReader.RegisterCharType(',', ELambdaExpressionWordType.COMMA);
            charSymbolReader.RegisterCharType(' ', ELambdaExpressionWordType.SPACE);
            charSymbolReader.RegisterCharType('\r', ELambdaExpressionWordType.SPACE);
            charSymbolReader.RegisterCharType('\n', ELambdaExpressionWordType.SPACE);
            var result = new SimpleTextSymbolReader<ELambdaExpressionWordType>(
                charSymbolReader,
                ELambdaExpressionWordType.EOF);

            result.SetGroupCount(ELambdaExpressionWordType.DELIMITER, 1);
            result.SetGroupCount(ELambdaExpressionWordType.OPEN_PARENTHESIS, 1);
            result.SetGroupCount(ELambdaExpressionWordType.CLOSE_PARENTHESIS, 1);
            result.SetGroupCount(ELambdaExpressionWordType.COMMA, 1);

            return result;
        }

        /// <summary>
        /// Valida o nome atribuído a um operador.
        /// </summary>
        /// <param name="operatorName">O nome do operador a ser validado.</param>
        private void ValidateOperatorName(string operatorName)
        {
            if (string.IsNullOrWhiteSpace(operatorName))
            {
                throw new UtilitiesException("A non empty operator name must be provided.");
            }
            else if (operatorName.Any(c => specialChars.Contains(c)))
            {
                var name = "character";
                var verb = "isn't";
                if (specialChars.Length > 1)
                {
                    name = "characters";
                    verb = "aren't";
                }

                var allChars = specialChars[0].ToString();
                var last = specialChars.Length - 1;
                for (int i = 1; i < last; ++i)
                {
                    allChars += ", " + specialChars[i];
                }

                allChars += " and " + specialChars[last];
                throw new UtilitiesException(string.Format(
                    "The {0} {1} {2} valid for operator name.",
                    name,
                    allChars,
                    verb));
            }
        }

        #region Funções de transição

        /// <summary>
        /// A funçáo correspondente à transição inicial - estado 0.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, ELambdaExpressionWordType> StartTransition(
            ISymbolReader<string, ELambdaExpressionWordType> reader)
        {
            if (reader.IsAtEOF())
            {
            }
            else
            {
                var readed = reader.Get();
                switch (readed.SymbolType)
                {
                    case ELambdaExpressionWordType.ALPHA:
                        break;
                    case ELambdaExpressionWordType.DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.OTHER:
                        break;
                    case ELambdaExpressionWordType.SPACE:
                        break;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// A função correspondente à transição final - esatado 1.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, ELambdaExpressionWordType> EndTransition(
            ISymbolReader<string, ELambdaExpressionWordType> reader)
        {
            return null;
        }

        /// <summary>
        /// A funçáo correspondente à transição de campo - estado 2.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, ELambdaExpressionWordType> FieldTransition(
            ISymbolReader<string, ELambdaExpressionWordType> reader)
        {
            if (reader.IsAtEOF())
            {
            }
            else
            {
                var readed = reader.Get();
                switch (readed.SymbolType)
                {
                    case ELambdaExpressionWordType.ALPHA:
                        break;
                    case ELambdaExpressionWordType.DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.OTHER:
                        break;
                    case ELambdaExpressionWordType.SPACE:
                        break;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// A funçáo correspondente à transição de operador interno - estado 3.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, ELambdaExpressionWordType> InternalOperatorTransition(
            ISymbolReader<string, ELambdaExpressionWordType> reader)
        {
            if (reader.IsAtEOF())
            {
            }
            else
            {
                var readed = reader.Get();
                switch (readed.SymbolType)
                {
                    case ELambdaExpressionWordType.ALPHA:
                        break;
                    case ELambdaExpressionWordType.DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.OTHER:
                        break;
                    case ELambdaExpressionWordType.SPACE:
                        break;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// A funçáo correspondente à transição de valor - estado 4.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, ELambdaExpressionWordType> ValueTransition(
            ISymbolReader<string, ELambdaExpressionWordType> reader)
        {
            if (reader.IsAtEOF())
            {
            }
            else
            {
                var readed = reader.Get();
                switch (readed.SymbolType)
                {
                    case ELambdaExpressionWordType.ALPHA:
                        break;
                    case ELambdaExpressionWordType.DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.OTHER:
                        break;
                    case ELambdaExpressionWordType.SPACE:
                        break;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// A funçáo correspondente à transição de operador lógico - estado 5.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, ELambdaExpressionWordType> LogicNumberTransition(
            ISymbolReader<string, ELambdaExpressionWordType> reader)
        {
            if (reader.IsAtEOF())
            {
            }
            else
            {
                var readed = reader.Get();
                switch (readed.SymbolType)
                {
                    case ELambdaExpressionWordType.ALPHA:
                        break;
                    case ELambdaExpressionWordType.DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.OTHER:
                        break;
                    case ELambdaExpressionWordType.SPACE:
                        break;
                }
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// A funçáo correspondente à transição de vírgula - estado 6.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, ELambdaExpressionWordType> CommaTransition(
            ISymbolReader<string, ELambdaExpressionWordType> reader)
        {
            if (reader.IsAtEOF())
            {
            }
            else
            {
                var readed = reader.Get();
                switch (readed.SymbolType)
                {
                    case ELambdaExpressionWordType.ALPHA:
                        break;
                    case ELambdaExpressionWordType.DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.OTHER:
                        break;
                    case ELambdaExpressionWordType.SPACE:
                        break;
                }
            }

            throw new NotImplementedException();
        }

        #endregion Funções de transição
    }
}
