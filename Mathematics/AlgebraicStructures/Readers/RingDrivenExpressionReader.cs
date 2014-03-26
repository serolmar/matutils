namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    public class RingDrivenExpressionReader<ObjectType, InputReader>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os elementos.
        /// </summary>
        private IRing<ObjectType> ring;

        /// <summary>
        /// O leitor responsável pela leitura individual de cada item.
        /// </summary>
        protected IParse<ObjectType, string, string> objectParser;

        /// <summary>
        /// Faz os mapeamentos dos delimitadores externos.
        /// </summary>
        protected Dictionary<string, List<string>> externalDelimitersTypes = new Dictionary<string, List<string>>();

        /// <summary>
        /// Faz o mapeamento de delimitadores de expressões.
        /// </summary>
        private Dictionary<string, List<string>> expressionDelimiterTypes = new Dictionary<string, List<string>>();

        public RingDrivenExpressionReader(
            IParse<ObjectType, string, string> objectParser, 
            IRing<ObjectType> ring)
        {
            if (objectParser == null)
            {
                throw new MathematicsException("An objet parser must be provided.");
            }
            else if (ring == null)
            {
                throw new MathematicsException("A ring must be provided.");
            }
            else
            {
                this.objectParser = objectParser;
                this.ring = ring;
            }
        }

        /// <summary>
        /// Obtém e atribui o anel responsável pelas operações sobre o leitor aquando
        /// da sua leitura.
        /// </summary>
        public IRing<ObjectType> Ring
        {
            get
            {
                return this.ring;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("A ring must be provided.");
                }
                else
                {
                    this.ring = value;
                }
            }
        }

        /// <summary>
        /// Tenta fazer a leitura da expressão.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="element">O valor lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParsePolynomial(
            MementoSymbolReader<InputReader, string, string> reader,
            out ObjectType element)
        {
            return this.TryParsePolynomial(reader, null, out element);
        }

        /// <summary>
        /// Tenta fazer a leitura da expressão.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="errors">A lista que irá receber os erros.</param>
        /// <param name="element">O elemento lido.</param>
        /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
        public bool TryParsePolynomial(
            MementoSymbolReader<InputReader, string, string> reader,
            List<string> errors,
            out ObjectType element)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else
            {
                var expressionReader = new ExpressionReader<ObjectType, string, string>(
                    this.objectParser);
                expressionReader.RegisterBinaryOperator("plus", Add, 0);
                expressionReader.RegisterBinaryOperator("times", Multiply, 1);
                expressionReader.RegisterBinaryOperator("minus", Subtract, 0);
                expressionReader.RegisterBinaryOperator("hat", Power, 2);
                expressionReader.RegisterUnaryOperator("minus", Symmetric, 0);
                expressionReader.RegisterBinaryOperator("over", Divide, 1);

                if (this.expressionDelimiterTypes.Any())
                {
                    foreach (var expressionDelimiterKvp in this.expressionDelimiterTypes)
                    {
                        foreach (var closeDelimiter in expressionDelimiterKvp.Value)
                        {
                            expressionReader.RegisterExpressionDelimiterTypes(
                            expressionDelimiterKvp.Key,
                            closeDelimiter);
                        }
                    }
                }
                else
                {
                    expressionReader.RegisterExpressionDelimiterTypes("left_parenthesis", "right_parenthesis");
                }

                expressionReader.RegisterSequenceDelimiterTypes("left_parenthesis", "right_parenthesis");
                foreach (var kvp in this.externalDelimitersTypes)
                {
                    foreach (var delimiter in kvp.Value)
                    {
                        expressionReader.RegisterExternalDelimiterTypes(kvp.Key, delimiter);
                    }
                }

                expressionReader.AddVoid("blancks");
                expressionReader.AddVoid("space");
                expressionReader.AddVoid("carriage_return");
                expressionReader.AddVoid("new_line");

                element = default(ObjectType);
                return expressionReader.TryParse(reader, errors, out element);
            }
        }

        /// <summary>
        /// Mapeia os delimitadores de expressão.
        /// </summary>
        /// <remarks>
        /// Caso não existam delimitadores de expressão, serão considerados os parêntesis
        /// de abertura e fecho por defeito.
        /// </remarks>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        public void RegisterExpressionDelimiterTypes(string openDelimiter, string closeDelimiter)
        {
            if (string.IsNullOrWhiteSpace(openDelimiter))
            {
                throw new ArgumentException("An open delimiter must be provided.");
            }
            else if (string.IsNullOrWhiteSpace(closeDelimiter))
            {
                throw new ArgumentException("A close delimiter must be provided.");
            }
            else
            {
                if (this.expressionDelimiterTypes.ContainsKey(openDelimiter))
                {
                    if (!this.expressionDelimiterTypes[openDelimiter].Contains(closeDelimiter))
                    {
                        this.expressionDelimiterTypes[openDelimiter].Add(closeDelimiter);
                    }
                }
                else
                {
                    var temporary = new List<string>() { closeDelimiter };
                    this.expressionDelimiterTypes.Add(openDelimiter, temporary);
                }
            }
        }

        /// <summary>
        /// Elimina todos os mapeamentos de expressão.
        /// </summary>
        /// <remarks>
        /// Caso não existam delimitadores de expressão, serão considerados os parêntesis
        /// de abertura e fecho por defeito.
        /// </remarks>
        public void ClearExpressionDelimitersMappings()
        {
            this.expressionDelimiterTypes.Clear();
        }

        /// <summary>
        /// Regista os delimitadores externos.
        /// </summary>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        public void RegisterExternalDelimiterTypes(string openDelimiter, string closeDelimiter)
        {
            if (this.externalDelimitersTypes.ContainsKey(openDelimiter))
            {
                if (!this.externalDelimitersTypes[openDelimiter].Contains(closeDelimiter))
                {
                    this.externalDelimitersTypes[openDelimiter].Add(closeDelimiter);
                }
            }
            else
            {
                var temporary = new List<string>() { closeDelimiter };
                this.externalDelimitersTypes.Add(openDelimiter, temporary);
            }
        }

        /// <summary>
        /// Elimina todos os mapeamentos externos.
        /// </summary>
        public void ClearExternalDelimitersMappings()
        {
            this.externalDelimitersTypes.Clear();
        }

        /// <summary>
        /// Adiciona dois elementos.
        /// </summary>
        /// <param name="left">O primeiro elemento a adicionar.</param>
        /// <param name="right">O segundo elemento a adicionar.</param>
        /// <returns>O elemento resultante.</returns>
        protected virtual ObjectType Add(
            ObjectType left,
            ObjectType right)
        {
            return this.ring.Add(left, right);
        }

        /// <summary>
        /// Multiplica dois elementos.
        /// </summary>
        /// <param name="left">O primeiro elemento.</param>
        /// <param name="right">O segundo elemento.</param>
        /// <returns>O elemento resultante.</returns>
        protected virtual ObjectType Multiply(
            ObjectType left,
            ObjectType right)
        {
            return this.ring.Multiply(left, right);
        }

        /// <summary>
        /// Subtrai dois elementos.
        /// </summary>
        /// <param name="left">O primeiro elemento.</param>
        /// <param name="right">O segundo elemento.</param>
        /// <returns>O elemento resultante.</returns>
        protected virtual ObjectType Subtract(
            ObjectType left,
            ObjectType right)
        {
            return this.ring.Add(
                left,
                this.ring.AdditiveInverse(right));
        }

        /// <summary>
        /// Divide dois elementos.
        /// </summary>
        /// <param name="left">O primeiro elemento.</param>
        /// <param name="right">O segundo elemento.</param>
        /// <returns>O elemento resultante.</returns>
        protected virtual ObjectType Divide(
            ObjectType left,
            ObjectType right)
        {
            throw new MathematicsException("Can't divide elements from a ring.");
        }

        /// <summary>
        /// Obtém o simétrico de um elemento.
        /// </summary>
        /// <param name="elem">O elemento.</param>
        /// <returns>O elemento resultante.</returns>
        protected virtual ObjectType Symmetric(
            ObjectType elem)
        {
            return this.ring.AdditiveInverse(elem);
        }

        /// <summary>
        /// Determina a potência do elemento.
        /// </summary>
        /// <param name="left">O elemento.</param>
        /// <param name="right">O expoente.</param>
        /// <returns>O resultado da potência.</returns>
        protected virtual ObjectType Power(
            ObjectType left,
            ObjectType right)
        {
            throw new NotImplementedException("The powers functionality isn't supported yet.");
        }
    }
}
