namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Utilities.Parsers;

    class SetParser<ObjectType> : IParse<HashSet<ObjectType>, string, ESymbolSetType>
    {
        private ExpressionReader<HashSet<ObjectType>, string, ESymbolSetType, ISymbol<string, ESymbolSetType>[]> expressionReader;

        public SetParser(IParse<ObjectType, string, ESymbolSetType> elementsParser)
        {
            if (elementsParser == null)
            {
                throw new ArgumentNullException("elementsParser");
            }
            else
            {
                this.expressionReader = new ExpressionReader<HashSet<ObjectType>, string, ESymbolSetType, ISymbol<string, ESymbolSetType>[]>(
                    new HashSetParser(elementsParser));
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.OPAR, ESymbolSetType.CPAR);
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.LBRACK, ESymbolSetType.RBRACK);
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.LANGLE, ESymbolSetType.RANGLE);
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.LBRACK, ESymbolSetType.RANGLE);
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.LANGLE, ESymbolSetType.RBRACK);
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.VBAR, ESymbolSetType.VBAR);
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.VBAR, ESymbolSetType.RANGLE);
                this.expressionReader.RegisterExternalDelimiterTypes(ESymbolSetType.LANGLE, ESymbolSetType.VBAR);

                this.expressionReader.RegisterBinaryOperator(ESymbolSetType.COMMA, this.Concatenate, 0);

                this.expressionReader.AddVoid(ESymbolSetType.SPACE);
                this.expressionReader.AddVoid(ESymbolSetType.CHANGE_LINE);
            }
        }

        public bool TryParse(
            ISymbol<string, ESymbolSetType>[] symbolListToParse, 
            out HashSet<ObjectType> value)
        {
            var openSymbol = -1;
            for (int i = 0; i < symbolListToParse.Length; ++i)
            {
                var currentSymbol = symbolListToParse[i];
                if (currentSymbol.SymbolType == ESymbolSetType.LBRACE)
                {
                    openSymbol = i;
                    i = symbolListToParse.Length;
                }
                else if (currentSymbol.SymbolType != ESymbolSetType.SPACE && currentSymbol.SymbolType != ESymbolSetType.CHANGE_LINE)
                {
                    i = symbolListToParse.Length;
                }
            }

            if (openSymbol == -1)
            {
                value = default(HashSet<ObjectType>);
                return false;
            }
            else
            {
                var closeSymbol = -1;
                for (int i = symbolListToParse.Length - 1; i > openSymbol; --i)
                {
                    var currentSymbol = symbolListToParse[i];
                    if (currentSymbol.SymbolType == ESymbolSetType.RBRACE)
                    {
                        closeSymbol = i;
                        i = openSymbol;
                    }
                    else if (currentSymbol.SymbolType != ESymbolSetType.SPACE && currentSymbol.SymbolType != ESymbolSetType.CHANGE_LINE)
                    {
                        i = openSymbol;
                    }
                }

                if (closeSymbol == -1)
                {
                    value = default(HashSet<ObjectType>);
                    return false;
                }
                else
                {
                    var elementsNumber = closeSymbol - openSymbol - 1;
                    var elementsArray = new ISymbol<string, ESymbolSetType>[elementsNumber];
                    Array.Copy(symbolListToParse, openSymbol + 1, elementsArray, 0, elementsNumber);
                    var arraySymbolReader = new ArraySymbolReader<string, ESymbolSetType>(elementsArray, ESymbolSetType.EOF);
                    return this.expressionReader.TryParse(arraySymbolReader, out value);
                }
            }
        }

        /// <summary>
        /// Concatena dois conjuntos, isto é, determina a respectiva união.
        /// </summary>
        /// <param name="left">O primeiro conjunto.</param>
        /// <param name="right">O segundo conjunto.</param>
        /// <returns>O resultado da união.</returns>
        private HashSet<ObjectType> Concatenate(HashSet<ObjectType> left, HashSet<ObjectType> right)
        {
            var result = new HashSet<ObjectType>(left);
            result.UnionWith(right);
            return result;
        }

        private class HashSetParser : IParse<HashSet<ObjectType>, string, ESymbolSetType>
        {
            private IParse<ObjectType, string, ESymbolSetType> elementsParser;

            public HashSetParser(IParse<ObjectType, string, ESymbolSetType> elementsParser)
            {
                this.elementsParser = elementsParser;
            }

            /// <summary>
            /// Obtém o leitor dos elementos.
            /// </summary>
            public IParse<ObjectType, string, ESymbolSetType> ElementsParser
            {
                get
                {
                    return this.elementsParser;
                }
            }

            /// <summary>
            /// Tenta efectuar a leitura de um elemento e encapsulá-lo num conjunto.
            /// </summary>
            /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
            /// <param name="value">O valor.</param>
            /// <returns>Verdadeiro caso a leitura seja bem sucedida e falso no caso contrário.</returns>
            public bool TryParse(ISymbol<string, ESymbolSetType>[] symbolListToParse, out HashSet<ObjectType> value)
            {
                var parsedObject = default(ObjectType);
                if (this.elementsParser.TryParse(symbolListToParse, out parsedObject))
                {
                    value = new HashSet<ObjectType>();
                    value.Add(parsedObject);
                    return true;
                }
                else
                {
                    value = default(HashSet<ObjectType>);
                    return false;
                }
            }
        }
    }
}
