namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class CharSymbolReader<SymbType> : MementoSymbolReader<TextReader, string, SymbType>
    {
        public delegate SymbType TypeOfReadedChar(char c);

        /// <summary>
        /// O tipo por defeito.
        /// </summary>
        private SymbType genericType;

        /// <summary>
        /// O tipo associado ao fim de ficheiro.
        /// </summary>
        private SymbType endOfFileType;

        private TypeOfReadedChar deciderFunction = null;

        private Dictionary<char, SymbType> charTypes = new Dictionary<char, SymbType>();
        List<StructRangeType<SymbType>> registeredRanges = new List<StructRangeType<SymbType>>();

        public TypeOfReadedChar DeciderFunction
        {
            get
            {
                return this.deciderFunction;
            }
            set
            {
                if (this.started)
                {
                    throw new Exception("Reader has already been started.");
                }
                this.deciderFunction = value;
            }
        }

        public SymbType GenericType
        {
            get
            {
                return this.genericType;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Type can't be null.");
                }
                else
                {
                    this.genericType = value;
                }
            }
        }

        public SymbType EndOfFileType
        {
            get
            {
                return this.endOfFileType;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Type can't be null.");
                }
                else
                {
                    this.endOfFileType = value;
                }
            }
        }

        public CharSymbolReader(TextReader inputReader) : base(inputReader) { }

        public void RegisterCharType(char charToRegister, SymbType typeOfChar)
        {
            if (this.started)
            {
                throw new Exception("Reader has already been started.");
            }
            if (charTypes.ContainsKey(charToRegister))
            {
                this.charTypes[charToRegister] = typeOfChar;
            }
            else
            {
                this.charTypes.Add(charToRegister, typeOfChar);
            }
        }

        public void UnRegisterCharType(char charToRegister)
        {
            if (this.started)
            {
                throw new Exception("Reader has already been started.");
            }
            if (this.charTypes.ContainsKey(charToRegister))
            {
                this.charTypes.Remove(charToRegister);
            }
        }

        public void RegisterCharRangeType(char charOne, char charTwo, SymbType type)
        {
            if (this.started)
            {
                throw new Exception("Reader has already been started.");
            }

            CharRange range = new CharRange(charOne, charTwo);
            if (!range.IsEmptyRange() && !type.Equals(string.Empty))
            {
                this.registeredRanges.Add(new StructRangeType<SymbType>() { Range = range, Type = type });
            }
        }

        public void UnRegisterAll()
        {
            if (this.started)
            {
                throw new Exception("Reader has already been started.");
            }
            this.charTypes.Clear();
            this.registeredRanges.Clear();
        }

        public override ISymbol<string, SymbType> Peek()
        {
            if (!this.started)
            {
                this.started = true;
            }
            if (this.bufferPointer == this.symbolBuffer.Count)
            {
                this.AddNextSymbolFromStream();
            }

            var result = new StringSymbol<SymbType>()
            {
                SymbolType = this.symbolBuffer[this.bufferPointer].SymbolType,
                SymbolValue = this.symbolBuffer[this.bufferPointer].SymbolValue
            };

            return result;
        }

        //private void ProcessInsertCharRange(CharRange inputRange, string type)
        //{
        //    CharRange remainingRange = inputRange;
        //    Dictionary<CharRange, string> tempDictionary = new Dictionary<CharRange, string>();
        //    foreach (var pair in this.charRangeTypes)
        //    {
        //        if (!inputRange.Contains(pair.Key))
        //        {
        //            CharRange intersection = remainingRange.Intersection(pair.Key);
        //            if (!intersection.IsEmptyRange())
        //            {
        //                if (pair.Key.StartChar < intersection.StartChar)
        //                {

        //                }
        //            }
        //        }
        //    }
        //}

        public override ISymbol<string,SymbType> Get()
        {
            if (!this.started)
            {
                this.started = true;
            }

            var result = this.Peek();
            if (result.SymbolType.Equals(endOfFileType))
            {
                return result;
            }
            if (this.bufferPointer < this.symbolBuffer.Count)
            {
                ++this.bufferPointer;
            }
            return result;
        }

        public override void UnGet()
        {
            if (bufferPointer > 0)
            {
                --this.bufferPointer;
            }
        }

        public override bool IsAtEOF()
        {
            return this.Peek().SymbolType.Equals(this.endOfFileType);
        }

        public override bool IsAtEOFSymbol(ISymbol<string, SymbType> symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            else
            {
                return symbol.SymbolType.Equals(this.endOfFileType);
            }
        }

        /// <summary>
        /// Adiciona o próximo símbolo lido para o contentor temporário.
        /// </summary>
        private void AddNextSymbolFromStream()
        {
            var result = new StringSymbol<SymbType>();
            int readedSymbol = this.inputStream.Read();
            if (readedSymbol == -1)
            {
                result.SymbolType = this.endOfFileType;
            }
            else
            {
                char readedChar = (char)readedSymbol;
                result.SymbolValue = readedChar.ToString();
                if (this.deciderFunction != null)
                {
                    result.SymbolType = this.deciderFunction.Invoke(readedChar);
                }

                var tempSymbol = this.GetCharTypeFromRanges(readedSymbol);
                if (tempSymbol != null)
                {
                    result.SymbolType = tempSymbol;
                }

                if (this.charTypes.ContainsKey(readedChar))
                {
                    result.SymbolType = this.charTypes[readedChar];
                }
                else if (result.SymbolType == null)
                {
                    result.SymbolType = this.genericType;
                }
            }

            this.symbolBuffer.Add(result);
        }

        private SymbType GetCharTypeFromRanges(int c)
        {
            var result = default(SymbType);
            foreach (var range in this.registeredRanges)
            {
                if (range.Range.HasChar(c))
                {
                    result = range.Type;
                }
            }

            return result;
        }

        private class CharRange
        {
            private int startChar;
            private int endChar;

            public CharRange()
            {
                this.SetEmpty();
            }

            public CharRange(int charOne, int charTwo)
            {
                this.SetRange(charOne, charTwo);
            }

            public int StartChar
            {
                get { return startChar; }
            }

            public int EndChar
            {
                get { return endChar; }
            }

            public int Length
            {
                get { return (int)this.endChar - (int)this.startChar + 1; }
            }

            public void SetRange(int charOne, int charTwo)
            {
                if (charOne < charTwo)
                {
                    this.startChar = charOne;
                    this.endChar = charTwo;
                }
                else
                {
                    this.startChar = charTwo;
                    this.endChar = charOne;
                }
            }

            public void SetEmpty()
            {
                this.startChar = -1;
                this.endChar = -1;
            }

            public bool HasChar(int c)
            {
                return c >= this.startChar && c <= this.endChar;
            }

            public bool IsEmptyRange()
            {
                return this.startChar < 0 || this.endChar < 0;
            }

            public bool Contains(CharRange otherCharRange)
            {
                return this.HasChar(otherCharRange.startChar) && this.HasChar(otherCharRange.endChar);
            }

            public bool IsContained(CharRange otherCharRange)
            {
                return otherCharRange.HasChar(this.startChar) && otherCharRange.HasChar(this.endChar);
            }

            public CharRange Intersection(CharRange otherCharRange)
            {
                CharRange result = new CharRange();
                if (this.startChar >= otherCharRange.startChar && this.startChar <= otherCharRange.endChar)
                {
                    result.startChar = this.startChar;
                    if (this.endChar <= otherCharRange.endChar)
                    {
                        result.endChar = this.endChar;
                    }
                    else
                    {
                        result.endChar = otherCharRange.endChar;
                    }
                }
                else if (this.startChar < otherCharRange.startChar)
                {
                    result.startChar = otherCharRange.startChar;
                    if (this.endChar >= startChar && this.endChar <= otherCharRange.endChar)
                    {
                        result.endChar = this.endChar;
                    }
                    else if (this.endChar > otherCharRange.endChar)
                    {
                        result.endChar = otherCharRange.endChar;
                    }
                }
                return result;
            }

            public override bool Equals(object obj)
            {
                CharRange rightHandSide = obj as CharRange;
                if (rightHandSide == null)
                {
                    return false;
                }
                return this.startChar.Equals(rightHandSide.startChar) && this.endChar.Equals(rightHandSide.endChar);
            }

            public override int GetHashCode()
            {
                return this.startChar ^ this.endChar;
            }

            public override string ToString()
            {
                if (this.IsEmptyRange())
                {
                    return "Range(empty)";
                }
                return string.Format("Range({0},{1})", this.startChar, this.endChar);
            }
        }

        private struct StructRangeType<Symb>
        {
            public CharRange Range { get; set; }
            public Symb Type { get; set; }
        }
    }
}
