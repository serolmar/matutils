namespace Utilities.Lambda
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// Permite construir uma expressão lambda com base num filtro escrito numa linguagem
    /// próxima do natural.
    /// </summary>
    public class SmartFilterLambdaBuilder
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
        /// Instancia uma nova instância de objectos do tipo <see cref="SmartFilterLambdaBuilder"/>.
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
            }
        }

        /// <summary>
        /// Costrói uma expressão que actua sobre o objecto.
        /// </summary>
        /// <typeparam name="ObjectType">O tipo de objecto.</typeparam>
        /// <param name="pattern">O filtro de pesquisa.</param>
        /// <returns>A expressão resultante.</returns>
        public Expression<Func<ObjectType, bool>> BuildExpressionTree<ObjectType>(string pattern)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constrói uma expressão que actua sobre uma propriedade pré-especificada do objecto.
        /// </summary>
        /// <typeparam name="ObjectType">O tipo de objecto.</typeparam>
        /// <typeparam name="PropertyType">O tipo de propriedade.</typeparam>
        /// <param name="selector">O selector da propriedade do objecto.</param>
        /// <param name="pattern">O filtro de pesquisa.</param>
        /// <returns>A expressão resultante.</returns>
        public Expression<Func<ObjectType, bool>> BuildExpressionTree<ObjectType, PropertyType>(
            Expression<Func<ObjectType, PropertyType>> selector,
            string pattern)
        {
            throw new NotImplementedException();
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
            charSymbolReader.RegisterCharType('"', ELambdaExpressionWordType.OPEN_DELIMITER);
            charSymbolReader.RegisterCharType(')', ELambdaExpressionWordType.CLOSE_PARENTHESIS);
            charSymbolReader.RegisterCharType('"', ELambdaExpressionWordType.CLOSE_DELIMITER);
            charSymbolReader.RegisterCharType(',', ELambdaExpressionWordType.COMMA);
            charSymbolReader.RegisterCharType(' ', ELambdaExpressionWordType.SPACE);
            charSymbolReader.RegisterCharType('\r', ELambdaExpressionWordType.SPACE);
            charSymbolReader.RegisterCharType('\n', ELambdaExpressionWordType.SPACE);
            var result = new SimpleTextSymbolReader<ELambdaExpressionWordType>(
                charSymbolReader,
                ELambdaExpressionWordType.EOF);

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
                    "The {0} {1} {2} aren't valid for operator name.",
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
                    case ELambdaExpressionWordType.CLOSE_DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_DELIMITER:
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
                    case ELambdaExpressionWordType.CLOSE_DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_DELIMITER:
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
                    case ELambdaExpressionWordType.CLOSE_DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_DELIMITER:
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
                    case ELambdaExpressionWordType.CLOSE_DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_DELIMITER:
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
                    case ELambdaExpressionWordType.CLOSE_DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_DELIMITER:
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
                    case ELambdaExpressionWordType.CLOSE_DELIMITER:
                        break;
                    case ELambdaExpressionWordType.CLOSE_PARENTHESIS:
                        break;
                    case ELambdaExpressionWordType.COMMA:
                        break;
                    case ELambdaExpressionWordType.NUMERIC:
                        break;
                    case ELambdaExpressionWordType.OPEN_DELIMITER:
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
