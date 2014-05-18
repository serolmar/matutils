namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Enumeração dos tipos de delimitadores usados no leitor de listas.
    /// </summary>
    public enum LispDelimiterType
    {
        /// <summary>
        /// Parênteisis curvos ().
        /// </summary>
        PARENTHESIS,

        /// <summary>
        /// Parêntesis rectos [].
        /// </summary>
        BRACKETS,

        /// <summary>
        /// Chavetas {}.
        /// </summary>
        BRACES,

        /// <summary>
        /// Parêntesis angulares &lt;&gt;.
        /// </summary>
        LESSER_GREATER,

        /// <summary>
        /// Barra - parênteisis angulares |>.
        /// </summary>
        BAR_GREATER
    }

    /// <summary>
    /// Implementa uma lista ao estilo da linguagem LISP.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos contidos na lista.</typeparam>
    public class LispStyleList<T> : IEnumerable<LispStyleList<T>>
    {
        /// <summary>
        /// O contentor da lista.
        /// </summary>
        private List<ElementList<T>> values = new List<LispStyleList<T>.ElementList<T>>();

        #region Construtores

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LispStyleList{T}"/>.
        /// </summary>
        public LispStyleList() { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LispStyleList{T}"/>.
        /// </summary>
        /// <param name="val">O valor inicial.</param>
        public LispStyleList(T val)
        {
            this.values.Add(new ElementList<T>() { Element = val, Elements = null });
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LispStyleList{T}"/>.
        /// </summary>
        /// <param name="initialList">A lista inicial.</param>
        public LispStyleList(List<T> initialList)
        {
            if (initialList != null)
            {
                foreach (var elem in initialList)
                {
                    this.values.Add(new ElementList<T>() { Element = elem, Elements = null });
                }
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="LispStyleList{T}"/>.
        /// </summary>
        /// <param name="copy">A lista LISP a ser copiada.</param>
        public LispStyleList(LispStyleList<T> copy)
        {
            foreach (var item in copy.values)
            {
                this.values.Add(item.Clone());
            }
        }

        #endregion Construtores

        /// <summary>
        /// Obtém ou atribui a lista LISP especificada pelo índice.
        /// </summary>
        /// <value>
        /// A lista.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Se o índice não se encontrar nos limites da lista.
        /// </exception>
        public LispStyleList<T> this[int index]
        {
            get
            {
                if (index >= this.values.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                LispStyleList<T> result = new LispStyleList<T>();
                if (this.values[index].Elements == null)
                {
                    result.values.Add(new ElementList<T>() { Element = this.values[index].Element, Elements = null });
                }
                else
                {
                    result.values.AddRange(this.values[index].Elements);
                }
                return result;
            }
            set
            {
                if (index >= this.values.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if (value.values.Count == 0)
                {
                    this.values.RemoveAt(index);
                }
                else if (value.values.Count == 1 && value.values[0].Elements == null)
                {
                    this.values[index].Element = value.values[0].Element;
                    this.values[index].Elements = null;
                }
                else
                {
                    if (this.values[index].Elements == null)
                    {
                        this.values[index].Elements = new List<ElementList<T>>();
                    }
                    List<ElementList<T>> temp = new List<ElementList<T>>();
                    foreach (var elem in value.values)
                    {
                        temp.Add(elem.Clone());
                    }
                    this.values[index].Element = default(T);
                    this.values[index].Elements.Clear();
                    this.values[index].Elements.AddRange(temp);
                }
            }
        }

        /// <summary>
        /// Obtém o número de elementos no nível actual da lista.
        /// </summary>
        /// <value>
        /// O número de elementos.
        /// </value>
        public int Count
        {
            get
            {
                return this.values.Count;
            }
        }

        #region Métodos

        /// <summary>
        /// Concatena a lista especificada.
        /// </summary>
        /// <param name="toAppend">A lista a ser concatenada.</param>
        public void Concatenate(LispStyleList<T> toAppend)
        {
            List<ElementList<T>> temp = new List<ElementList<T>>();
            foreach (var app in toAppend.values)
            {
                ElementList<T> element = app.Clone();
                temp.Add(element);
            }
            this.values.AddRange(temp);
        }

        /// <summary>
        /// Injecta um valor no final da lista.
        /// </summary>
        /// <param name="val">O valor a ser injectado.</param>
        public void Push(LispStyleList<T> val)
        {
            if (val.values.Count == 1)
            {
                this.values.Add(new ElementList<T>() { Element = val.values[0].Element, Elements = null });
            }
            else if (val.values.Count > 1)
            {
                this.values.Add(new ElementList<T>() { Element = default(T), Elements = val.values });
            }
        }

        /// <summary>
        /// Obtém o primeiro elemento da lista.
        /// </summary>
        /// <returns>O primeiro elemento da lista.</returns>
        public LispStyleList<T> Header()
        {
            LispStyleList<T> result = new LispStyleList<T>();
            if (this.values.Count > 0)
            {
                result.values.Add(this.values[0]);
            }

            return result;
        }

        /// <summary>
        /// Obtém o último elemento da lista.
        /// </summary>
        /// <returns>O último elemento da lista.</returns>
        public LispStyleList<T> Tail()
        {
            LispStyleList<T> result = new LispStyleList<T>();
            if (this.values.Count > 0)
            {
                result.values.Add(this.values[this.values.Count - 1]);
            }

            return result;
        }

        /// <summary>
        /// Obtém um enumerador para o primeiro nível da lista.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<LispStyleList<T>> GetEnumerator()
        {
            return new LispStyleEnumerator<T>(this.values);
        }

        /// <summary>
        /// Obtém um enumerador não genérico para o primeiro nível da lista.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Gets a <see cref="IListStyleParser"/> parser for list style list using
        /// the parenthesis as the default delimiters.
        /// </summary>
        /// <param name="parserForT">The parser for list objects.</param>
        /// <returns>The parser.</returns>
        public static ILispStyleListParser<T> GetParser(IParse<T, string, string> parserForT)
        {
            return new LispStyleParser<T>(parserForT);
        }

        /// <summary>
        /// Gets a <see cref="IListStyleParser"/> parser for list style list using
        /// the specified delimiters types.
        /// </summary>
        /// <param name="parserForT">The parser for list objects.</param>
        /// <param name="type">The type of delimiters to be used during the parsing.</param>
        /// <returns>The parser.</returns>
        public static ILispStyleListParser<T> GetParser(
            IParse<T, string, string> parserForT, 
            LispDelimiterType type)
        {
            return new LispStyleParser<T>(parserForT, type);
        }

        /// <summary>
        /// Constrói uma representação textual da lista.
        /// </summary>
        /// <returns>A representação textual da lista.</returns>
        public override string ToString()
        {
            return this.ToString(LispDelimiterType.PARENTHESIS);
        }

        /// <summary>
        /// Constrói uma representação textual da lista.
        /// </summary>
        /// <param name="type">O delimitador.</param>
        /// <returns>A representação textual da lista.</returns>
        public string ToString(LispDelimiterType type)
        {
            var openType = string.Empty;
            var closeType = string.Empty;
            switch (type)
            {
                case LispDelimiterType.BAR_GREATER:
                    openType = "|";
                    closeType = ">";
                    break;
                case LispDelimiterType.BRACES:
                    openType = "{";
                    closeType = "}";
                    break;
                case LispDelimiterType.BRACKETS:
                    openType = "[";
                    closeType = "]";
                    break;
                case LispDelimiterType.LESSER_GREATER:
                    openType = "<";
                    closeType = ">";
                    break;
                case LispDelimiterType.PARENTHESIS:
                    openType = "(";
                    closeType = ")";
                    break;
                default:
                    throw new CollectionsException("Delimiter types not yet implemented.");
            }

            string result = openType;
            if (this.values.Count > 0)
            {
                result += this.PrintElement(this.values[0], openType, closeType);
                for (int i = 1; i < this.values.Count; ++i)
                {
                    result += ",";
                    result += this.PrintElement(this.values[i], openType, closeType);
                }
            }
            result += closeType;
            return result;
        }

        /// <summary>
        /// Imprime um determinado elemento da lista.
        /// </summary>
        /// <param name="elementToPrint">O elemento a ser imprimido.</param>
        /// <param name="openDelimiter">O delimitador de abertura.</param>
        /// <param name="closeDelimiter">O delimitador de fecho.</param>
        /// <returns>O resultado.</returns>
        private string PrintElement(ElementList<T> elementToPrint, string openDelimiter, string closeDelimiter)
        {
            string result = string.Empty;
            if (elementToPrint.Elements == null)
            {
                result += elementToPrint.Element.ToString();
            }
            else if (elementToPrint.Elements.Count == 0)
            {
                result += elementToPrint.Element.ToString();
            }
            else
            {
                result += openDelimiter;
                result += this.PrintElement(elementToPrint.Elements[0], openDelimiter, closeDelimiter);
                for (int i = 1; i < elementToPrint.Elements.Count; ++i)
                {
                    result += ",";
                    result += this.PrintElement(elementToPrint.Elements[i], openDelimiter, closeDelimiter);
                }
                result += closeDelimiter;
            }
            return result;
        }

        #endregion Métodos

        /// <summary>
        /// Representa um elemento da lista.
        /// </summary>
        /// <typeparam name="R">O tipo de objectos que o elemento contém.</typeparam>
        private class ElementList<R>
        {
            /// <summary>
            /// Obtém ou atribui o elemento.
            /// </summary>
            /// <value>
            /// O elemento.
            /// </value>
            public R Element { get; set; }

            /// <summary>
            /// Obtém ou atribui uma lista de elementos.
            /// </summary>
            /// <value>
            /// A lista de elementos.
            /// </value>
            public List<ElementList<R>> Elements { get; set; }

            /// <summary>
            /// Copia um elemento.
            /// </summary>
            /// <returns>A cópia.</returns>
            public ElementList<R> Clone()
            {
                ElementList<R> result = new ElementList<R>();
                result.Element = this.Element;
                if (this.Elements != null)
                {
                    List<ElementList<R>> temp = new List<ElementList<R>>();
                    foreach (var item in this.Elements)
                    {
                        temp.Add(item.Clone() as ElementList<R>);
                    }
                    result.Elements = temp;
                }
                return result;
            }
        }

        /// <summary>
        /// Define um enumerador para o primeiro nível da lista.
        /// </summary>
        /// <typeparam name="R">O tipo de objectos que a lista contém.</typeparam>
        private class LispStyleEnumerator<R> : IEnumerator<LispStyleList<R>>
        {
            /// <summary>
            /// O enumerador para os elementos da lista.
            /// </summary>
            IEnumerator<LispStyleList<R>.ElementList<R>> enumerator;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="LispStyleEnumerator`1"/>.
            /// </summary>
            /// <param name="elements">Os elementos.</param>
            public LispStyleEnumerator(List<LispStyleList<R>.ElementList<R>> elements)
            {
                this.enumerator = elements.GetEnumerator();
            }

            /// <summary>
            /// Obtém a lista LIST com o elemento apontado pelo enumerador.
            /// </summary>
            /// <returns>A lista LISP com o elemento apontado pelo enumerador.</returns>
            public LispStyleList<R> Current
            {
                get
                {
                    LispStyleList<R> result = new LispStyleList<R>();
                    result.values.Add(this.enumerator.Current);
                    return result;
                }
            }

            /// <summary>
            /// Descarta o enumerador.
            /// </summary>
            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            /// <summary>
            /// Obtém a lista LIST com o elemento apontado pelo enumerador.
            /// </summary>
            /// <returns>A lista LISP com o elemento apontado pelo enumerador.</returns>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            /// <summary>
            /// Avança o enumerador para o próximo elemento da lista LISP.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso o enumerador avance e falso caso contrário.
            /// </returns>
            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            /// <summary>
            /// Incializa o enumerador.
            /// </summary>
            public void Reset()
            {
                this.enumerator.Reset();
            }
        }

        /// <summary>
        /// Implementa um leitor de listas LISP.
        /// </summary>
        /// <typeparam name="R">O tipo de objectos contidos na lista.</typeparam>
        private class LispStyleParser<R> : ILispStyleListParser<R>
        {
            /// <summary>
            /// O leitor de expressões.
            /// </summary>
            private ExpressionReader<LispStyleList<R>.ElementList<R>, string, string> expressionReader;

            /// <summary>
            /// Os delimitadores.
            /// </summary>
            private LispDelimiterType delimiters;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="LispStyleParser`1"/>.
            /// </summary>
            /// <param name="parserForT">O leitor de elementos.</param>
            public LispStyleParser(IParse<R, string, string> parserForT)
                : this(parserForT, LispDelimiterType.PARENTHESIS)
            {
            }

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="LispStyleParser`1"/>..
            /// </summary>
            /// <param name="parserForT">O leitor de elementos.</param>
            /// <param name="type">O tipo de delimitador.</param>
            /// <exception cref="CollectionsException">Se o delimitador não for suportado.</exception>
            public LispStyleParser(IParse<R, string, string> parserForT, LispDelimiterType type)
            {
                string openType = string.Empty;
                string closeType = string.Empty;
                switch (type)
                {
                    case LispDelimiterType.BAR_GREATER:
                        openType = "bitwise_or";
                        closeType = "great_than";
                        break;
                    case LispDelimiterType.BRACES:
                        openType = "left_brace";
                        closeType = "right_brace";
                        break;
                    case LispDelimiterType.BRACKETS:
                        openType = "left_bracket";
                        closeType = "right_bracket";
                        break;
                    case LispDelimiterType.LESSER_GREATER:
                        openType = "less_than";
                        closeType = "great_than";
                        break;
                    case LispDelimiterType.PARENTHESIS:
                        openType = "left_parenthesis";
                        closeType = "right_parenthesis";
                        break;
                    default:
                        throw new CollectionsException("Delimiter types not yet implemented.");
                }

                this.expressionReader = new ExpressionReader<LispStyleList<R>.ElementList<R>, string, string>(
                    new ElementParser<R>(parserForT));
                this.expressionReader.RegisterExpressionDelimiterTypes(openType, closeType, this.Parenthesis);
                this.expressionReader.RegisterBinaryOperator("comma", this.Concatenate, 0);
                this.delimiters = type;
            }

            /// <summary>
            /// Tenta realizar a leitura.
            /// </summary>
            /// <param name="strToParse">O texto a ler.</param>
            /// <param name="value">O valor para onde a lista será lida.</param>
            /// <returns>Verdadeiro se a leitura for bem-sucedida e falso caso contrário.</returns>
            /// <exception cref="System.FormatException">
            /// Se o texto contiver erros.
            /// </exception>
            /// <exception cref="CollectionsException">Em caso de erro interno.</exception>
            public bool TryParse(ISymbol<string,string>[] strToParse, out LispStyleList<R> value)
            {
                var strValue = strToParse[0];
                string clean = strValue.SymbolValue.Trim();
                if (clean.Equals(string.Empty))
                {
                    throw new FormatException("Can't construct a lisp style list from empty strings.");
                }

                if (this.delimiters == LispDelimiterType.PARENTHESIS)
                {
                    if (clean[0] != '(' || clean[clean.Length - 1] != ')')
                    {
                        throw new FormatException("A lisp style list must begin with '(' and finish with ')'");
                    }
                }
                else if (this.delimiters == LispDelimiterType.BAR_GREATER)
                {
                    if (clean[0] != '|' || clean[clean.Length - 1] != '>')
                    {
                        throw new FormatException("A lisp style list must begin with '|' and finish with '>'");
                    }
                }
                else if (this.delimiters == LispDelimiterType.BRACES)
                {
                    if (clean[0] != '{' || clean[clean.Length - 1] != '}')
                    {
                        throw new FormatException("A lisp style list must begin with '{' and finish with '}'");
                    }
                }
                else if (this.delimiters == LispDelimiterType.BRACKETS)
                {
                    if (clean[0] != '[' || clean[clean.Length - 1] != ']')
                    {
                        throw new FormatException("A lisp style list must begin with '[' and finish with ']'");
                    }
                }
                else if (this.delimiters == LispDelimiterType.LESSER_GREATER)
                {
                    if (clean[0] != '<' || clean[clean.Length - 1] != '>')
                    {
                        throw new FormatException("A lisp style list must begin with '<' and finish with '>'");
                    }
                }
                else
                {
                    throw new CollectionsException("An internal error has occured.");
                }

                LispStyleList<R>.ElementList<R> elementList = this.expressionReader.Parse(
                    new StringSymbolReader(new StringReader(clean), false));

                LispStyleList<R> result = new LispStyleList<R>();
                if (elementList.Elements != null)
                {
                    result.values.AddRange(elementList.Elements);
                }
                else
                {
                    result.values.Add(elementList);
                }

                value = result;
                return true;
            }

            /// <summary>
            /// Mapeia delimitadores externos que permitem isolar o texto dos objectos.
            /// </summary>
            /// <param name="openDelimiterType">O tipo de delimitador de abertura.</param>
            /// <param name="closeDelimiterType">O tipo de delimitador de fecho.</param>
            public void RegisterValueDelimiterType(string openDelimiterType, string closeDelimiterType)
            {
                this.expressionReader.RegisterExternalDelimiterTypes(openDelimiterType, closeDelimiterType);
            }

            /// <summary>
            /// Executa a função aquando da identificação de parêntesis.
            /// </summary>
            /// <param name="arg">O argumento.</param>
            /// <returns>A lista.</returns>
            private LispStyleList<R>.ElementList<R> Parenthesis(LispStyleList<R>.ElementList<R> arg)
            {
                LispStyleList<R>.ElementList<R> result = new LispStyleList<R>.ElementList<R>();
                result.Elements = new List<LispStyleList<R>.ElementList<R>>();
                result.Elements.Add(arg);
                return result;
            }

            /// <summary>
            /// Concatena duas listas.
            /// </summary>
            /// <param name="left">A primeira lisa a ser concatenada.</param>
            /// <param name="right">A segunda lista a ser concatenada.</param>
            /// <returns>O resultado da concatenação.</returns>
            private LispStyleList<R>.ElementList<R> Concatenate(
                LispStyleList<R>.ElementList<R> left, 
                LispStyleList<R>.ElementList<R> right)
            {
                LispStyleList<R>.ElementList<R> result = left.Clone();
                if (result.Elements != null)
                {
                    if (right.Elements != null)
                    {
                        foreach (var item in right.Elements)
                        {
                            result.Elements.Add(item.Clone());
                        }
                    }
                    else
                    {
                        result.Elements.Add(new LispStyleList<R>.ElementList<R>() { Element = right.Element });
                    }
                }
                else
                {
                    result.Elements = new List<LispStyleList<R>.ElementList<R>>();
                    result.Elements.Add(new LispStyleList<R>.ElementList<R>() { Element = left.Element });
                    if (right.Elements != null)
                    {
                        foreach (var item in right.Elements)
                        {
                            result.Elements.Add(item.Clone());
                        }
                    }
                    else
                    {
                        result.Elements.Add(new LispStyleList<R>.ElementList<R>() { Element = right.Element });
                    }
                }
                return result;
            }

            /// <summary>
            /// Implementa um leitor de elementos.
            /// </summary>
            /// <typeparam name="Q">O tipo de objectos nos elementos.</typeparam>
            private class ElementParser<Q> : IParse<LispStyleList<Q>.ElementList<Q>, string, string>
            {
                /// <summary>
                /// O leitor para cada elemento individual.
                /// </summary>
                private IParse<Q, string, string> parserForT;

                /// <summary>
                /// Instancia um novo objecto do tipo <see cref="ElementParser`1"/>.
                /// </summary>
                /// <param name="parser">O leitor de elementos.</param>
                public ElementParser(IParse<Q, string, string> parser)
                {
                    this.parserForT = parser;
                }

                /// <summary>
                /// Tenta realizar a leitura do elemento.
                /// </summary>
                /// <param name="symbolListToParse">A lista de símbolos a ler.</param>
                /// <param name="value">O valor que conterá a leitura.</param>
                /// <returns>Verdadeiro caso a leitura seja bem-sucedida e falso caso contrário.</returns>
                public bool TryParse(
                    ISymbol<string, string>[] symbolListToParse, 
                    out LispStyleList<Q>.ElementList<Q> value)
                {
                    Q tempVal = default(Q);
                    if (this.parserForT.TryParse(symbolListToParse, out tempVal))
                    {
                        List<LispStyleList<Q>.ElementList<Q>> tempList = new List<LispStyleList<Q>.ElementList<Q>>();
                        tempList.Add(new LispStyleList<Q>.ElementList<Q>() { Element = tempVal, Elements = null });
                        value = new LispStyleList<Q>.ElementList<Q>()
                        {
                            Element = default(Q),
                            Elements = tempList
                        };

                        return true;
                    }
                    else
                    {
                        value = null;
                        return false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Implementa um leitor de listas LISP.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos contidos na lista.</typeparam>
    public interface ILispStyleListParser<T> : IParse<LispStyleList<T>, string, string>
    {
        void RegisterValueDelimiterType(string openDelimiterType, string closeDelimiterType);
    }
}
