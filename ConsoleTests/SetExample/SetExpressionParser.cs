namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    class SetExpressionParser<ObjectType>
    {
        private ExpressionReader<HashSet<ObjectType>, string, ESymbolSetType> expressionParser;

        public SetExpressionParser(IParse<ObjectType, string, ESymbolSetType> elementParser)
        {
            if (elementParser == null)
            {
                throw new ArgumentNullException("elementParser");
            }
            else
            {
                this.expressionParser = new ExpressionReader<HashSet<ObjectType>, string, ESymbolSetType>(
                    new SetParser<ObjectType>(elementParser));
                this.expressionParser.RegisterExpressionDelimiterTypes(ESymbolSetType.OPAR, ESymbolSetType.CPAR);
                this.expressionParser.RegisterExternalDelimiterTypes(ESymbolSetType.LBRACE, ESymbolSetType.RBRACE);

                this.expressionParser.RegisterBinaryOperator(ESymbolSetType.UNION, this.Union, 0);
                this.expressionParser.RegisterBinaryOperator(ESymbolSetType.INTERSECTION, this.Intersection, 1);

                this.expressionParser.AddVoid(ESymbolSetType.SPACE);
                this.expressionParser.AddVoid(ESymbolSetType.CHANGE_LINE);
            }
        }

        public bool TryParse(SymbolReader<CharSymbolReader<ESymbolSetType>, string, ESymbolSetType> reader, out HashSet<ObjectType> value)
        {
            return this.expressionParser.TryParse(reader, out value);
        }

        /// <summary>
        /// Determina a reunião de dois conjuntos.
        /// </summary>
        /// <param name="left">O primeiro conjunto.</param>
        /// <param name="right">O segundo conjunto.</param>
        /// <returns>A união.</returns>
        private HashSet<ObjectType> Union(HashSet<ObjectType> left, HashSet<ObjectType> right)
        {
            var result = new HashSet<ObjectType>(left);
            result.UnionWith(right);
            return result;
        }

        /// <summary>
        /// Determina a intersecção de dois conjuntos.
        /// </summary>
        /// <param name="left">O primeiro conjunto.</param>
        /// <param name="right">O segundo conjunto.</param>
        /// <returns>A intersecção.</returns>
        private HashSet<ObjectType> Intersection(HashSet<ObjectType> left, HashSet<ObjectType> right)
        {
            var result = new HashSet<ObjectType>(left);
            result.IntersectWith(right);
            return result;
        }
    }
}
