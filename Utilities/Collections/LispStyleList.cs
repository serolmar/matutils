namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Enumeration of delimiter types within the lisp parser.
    /// </summary>
    public enum LispDelimiterType
    {
        PARENTHESIS,
        BRACKETS,
        BRACES,
        LESSER_GREATER,
        BAR_GREATER
    }

    public class LispStyleList<T> : IEnumerable<LispStyleList<T>>
    {
        /// <summary>
        /// Contains all the values of lisp style list.
        /// </summary>
        private List<ElementList<T>> values = new List<LispStyleList<T>.ElementList<T>>();

        #region constructors
        public LispStyleList() { }

        public LispStyleList(T val)
        {
            this.values.Add(new ElementList<T>() { Element = val, Elements = null });
        }

        public LispStyleList(List<T> initialList)
        {
            foreach (var elem in initialList)
            {
                this.values.Add(new ElementList<T>() { Element = elem, Elements = null });
            }
        }

        public LispStyleList(LispStyleList<T> copy)
        {
            foreach (var item in copy.values)
            {
                this.values.Add(item.Clone());
            }
        }
        #endregion

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

        public int Count
        {
            get
            {
                return this.values.Count;
            }
        }

        #region methods
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

        public LispStyleList<T> Header()
        {
            LispStyleList<T> result = new LispStyleList<T>();
            if (this.values.Count > 0)
            {
                result.values.Add(this.values[0]);
            }
            return result;
        }

        public LispStyleList<T> Tail()
        {
            LispStyleList<T> result = new LispStyleList<T>();
            if (this.values.Count > 0)
            {
                result.values.Add(this.values[this.values.Count - 1]);
            }
            return result;
        }

        public IEnumerator<LispStyleList<T>> GetEnumerator()
        {
            return new LispStyleEnumerator<T>(this.values);
        }

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
        public static ILispStyleListParser<T> GetParser(IParse<T, string, string> parserForT, LispDelimiterType type)
        {
            return new LispStyleParser<T>(parserForT, type);
        }

        public override string ToString()
        {
            return this.ToString(LispDelimiterType.PARENTHESIS);
        }

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
        #endregion

        private class ElementList<R>
        {
            public R Element { get; set; }
            public List<ElementList<R>> Elements { get; set; }

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

        private class LispStyleEnumerator<R> : IEnumerator<LispStyleList<R>>
        {
            IEnumerator<LispStyleList<R>.ElementList<R>> enumerator;
            public LispStyleEnumerator(List<LispStyleList<R>.ElementList<R>> elements)
            {
                this.enumerator = elements.GetEnumerator();
            }

            public LispStyleList<R> Current
            {
                get
                {
                    LispStyleList<R> result = new LispStyleList<R>();
                    result.values.Add(this.enumerator.Current);
                    return result;
                }
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }
        }

        private class LispStyleParser<R> : ILispStyleListParser<R>
        {
            private ExpressionReader<LispStyleList<R>.ElementList<R>, string, string> expressionReader;

            public LispStyleParser(IParse<R, string, string> parserForT)
                : this(parserForT, LispDelimiterType.PARENTHESIS)
            {
            }

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
            }

            public bool TryParse(ISymbol<string,string>[] strToParse, out LispStyleList<R> value)
            {
                var strValue = strToParse[0];
                string clean = strValue.SymbolValue.Trim();
                if (clean.Equals(string.Empty))
                {
                    throw new FormatException("Can't construct a lisp style list from empty strings.");
                }
                if (clean[0] != '(' || clean[clean.Length - 1] != ')')
                {
                    throw new FormatException("A lisp style list must begin with '(' and finish with ')'");
                }

                LispStyleList<R>.ElementList<R> elementList = this.expressionReader.Parse(new StringSymbolReader(new StringReader(clean), false));
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

            public void RegisterValueDelimiterType(string openDelimiterType, string closeDelimiterType)
            {
                this.expressionReader.RegisterExternalDelimiterTypes(openDelimiterType, closeDelimiterType);
            }

            private LispStyleList<R>.ElementList<R> Parenthesis(LispStyleList<R>.ElementList<R> arg)
            {
                LispStyleList<R>.ElementList<R> result = new LispStyleList<R>.ElementList<R>();
                result.Elements = new List<LispStyleList<R>.ElementList<R>>();
                result.Elements.Add(arg);
                return result;
            }

            private LispStyleList<R>.ElementList<R> Concatenate(LispStyleList<R>.ElementList<R> left, LispStyleList<R>.ElementList<R> right)
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

            private class ElementParser<Q> : IParse<LispStyleList<Q>.ElementList<Q>, string, string>
            {
                private IParse<Q, string, string> parserForT;

                public ElementParser(IParse<Q, string, string> parser)
                {
                    this.parserForT = parser;
                }

                public bool TryParse(ISymbol<string, string>[] symbolListToParse, out LispStyleList<Q>.ElementList<Q> value)
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

    public interface ILispStyleListParser<T> : IParse<LispStyleList<T>, string, string>
    {
        void RegisterValueDelimiterType(string openDelimiterType, string closeDelimiterType);
    }
}
