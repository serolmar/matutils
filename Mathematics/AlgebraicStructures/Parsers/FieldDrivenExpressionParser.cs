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
    /// <typeparam name="SymbValue">O tipo de valor associado ao símbolo.</typeparam>
    /// <typeparam name="SymbType">O tipo de dados associado ao tipo de símbolo.</typeparam>
    public class FieldDrivenExpressionParser<ObjectType>
        : RingDrivenExpressionParser<ObjectType>
    {
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
