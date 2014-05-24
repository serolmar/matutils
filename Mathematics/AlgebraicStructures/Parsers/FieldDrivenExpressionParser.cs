namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite construir um leitor de valores com base num corpo.
    /// </summary>
    /// <example>
    /// Leitura de expressões como (2/3)^3+5*(1/2+4/5)^5 com base num domínio sobre inteiros, o qual
    /// define todas as operações de anel.
    /// </example>
    /// <typeparam name="ObjectType">O tipo de valor a ser lido.</typeparam>
    public class FieldDrivenExpressionParser<ObjectType>
        : RingDrivenExpressionParser<ObjectType>
    {
        /// <summary>
        /// Instancia uma novo objecto do tipo <see cref="FieldDrivenExpressionParser{ObjectType}"/>.
        /// </summary>
        /// <param name="elementsParser">O leitor dos valores.</param>
        /// <param name="field">O corpo responsável pelas operações sobre os valores.</param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre os representantes de inteiros.</param>
        /// <exception cref="ArgumentNullException">Se o leitor dos valores ou o corpo responsável pelas suas operações forem nulos.</exception>
        public FieldDrivenExpressionParser(
            IParse<ObjectType, string, string> elementsParser,
            IField<ObjectType> field,
            IIntegerNumber<ObjectType> integerNumber = null)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            else if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.expressionReader = new FieldDrivenExpressionReader<ObjectType, ISymbol<string, string>[]>(
                    elementsParser,
                    field,
                    integerNumber);
            }
        }
    }
}
