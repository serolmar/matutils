namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um leitor que permite a caracterização de carácters.
    /// </summary>
    /// <typeparam name="SymbType">O tipo dos objectos que constituem os símbolos.</typeparam>
    public class CharSymbolReader<SymbType> : MementoSymbolReader<TextReader, string, SymbType>
    {
        /// <summary>
        /// Define o delegado que permite incluir funções de classificação de carácteres.
        /// </summary>
        /// <param name="c">O caráter a ser classificado.</param>
        /// <returns>O tipo de símbolo associado ao carácter.</returns>
        public delegate SymbType TypeOfReadedChar(char c);

        /// <summary>
        /// O tipo por defeito.
        /// </summary>
        private SymbType genericType;

        /// <summary>
        /// O tipo associado ao fim de ficheiro.
        /// </summary>
        private SymbType endOfFileType;

        /// <summary>
        /// Função que permite atribuir um tipo de símbolo a um carácter.
        /// </summary>
        private TypeOfReadedChar deciderFunction = null;

        /// <summary>
        /// Um mapeamento entre carácteres e respectivos tipos de símbolos.
        /// </summary>
        private Dictionary<char, SymbType> charTypes = new Dictionary<char, SymbType>();

        /// <summary>
        /// Uma lista de limites de carácteres e os seus tipos de símbolos.
        /// </summary>
        List<StructRangeType<SymbType>> registeredRanges = new List<StructRangeType<SymbType>>();

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CharSymbolReader{SymbType}"/>.
        /// </summary>
        /// <param name="inputReader">O leitor de texto.</param>
        /// <param name="genericType">O tipo genérico.</param>
        /// <param name="endOfFileType">O marcador de final de ficheiro.</param>
        public CharSymbolReader(
            TextReader inputReader,
            SymbType genericType,
            SymbType endOfFileType)
            : base(inputReader)
        {
            if (endOfFileType == null)
            {
                throw new ArgumentNullException("endOfFileType");
            }
            if (genericType == null)
            {
                throw new ArgumentNullException("genericType");
            }
            else
            {
                this.genericType = genericType;
                this.endOfFileType = endOfFileType;
            }
        }

        /// <summary>
        /// Obtém ou atribui a função que permite classificar carácteres.
        /// </summary>
        /// <value>A função que permite classificar carácteres.</value>
        /// <exception cref="UtilitiesException">Se o leitor tiver sido inicializado.</exception>
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
                    throw new UtilitiesException("Reader has already been started.");
                }
                else
                {
                    this.deciderFunction = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui os tipo de símbolo que é atribuído aos carácteres não classificados.
        /// </summary>
        /// <value>O tipo de símbolo.</value>
        /// <exception cref="UtilitiesException">
        /// Se o valor a atribuir for nulo ou se o leitor tiver sido iniciado.
        /// </exception>
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
                    throw new UtilitiesException("Type can't be null.");
                }
                else if (this.started)
                {
                    throw new UtilitiesException("Reader has already been started.");
                }
                else
                {
                    this.genericType = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o tipo de símbolo correspondente ao final de ficheiro.
        /// </summary>
        /// <value>O tipo de símbolo.</value>
        /// <exception cref="UtilitiesException">
        /// Se o valor a atribuir for nulo ou se o leitor tiver sido iniciado.
        /// </exception>
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
                    throw new UtilitiesException("Type can't be null.");
                }
                else if (this.started)
                {
                    throw new UtilitiesException("Reader has already been started.");
                }
                else
                {
                    this.endOfFileType = value;
                }
            }
        }

        /// <summary>
        /// Regista um carácter com um determinado tipo de símbolo.
        /// </summary>
        /// <param name="charToRegister">O carácter.</param>
        /// <param name="typeOfChar">O tipo do símbolo.</param>
        /// <exception cref="UtilitiesException">Se o leitor foi iniciado.</exception>
        public void RegisterCharType(char charToRegister, SymbType typeOfChar)
        {
            if (this.started)
            {
                throw new UtilitiesException("Reader has already been started.");
            }
            else if (charTypes.ContainsKey(charToRegister))
            {
                this.charTypes[charToRegister] = typeOfChar;
            }
            else
            {
                this.charTypes.Add(charToRegister, typeOfChar);
            }
        }

        /// <summary>
        /// Elimina o registo do carácter desassociando-o do seu tipo de símbolo.
        /// </summary>
        /// <param name="charToRegister">O carácter a eliminar.</param>
        /// <exception cref="UtilitiesException">Se o leitor foi iniciado.</exception>
        public void UnRegisterCharType(char charToRegister)
        {
            if (this.started)
            {
                throw new UtilitiesException("Reader has already been started.");
            }
            else if (this.charTypes.ContainsKey(charToRegister))
            {
                this.charTypes.Remove(charToRegister);
            }
        }

        /// <summary>
        /// Regista um intervalo de carácteres com o mesmo tipo de símbolo.
        /// </summary>
        /// <param name="charOne">O carácter que define uma extremidade do intervalo.</param>
        /// <param name="charTwo">O carácter que define a outra extremidade do intervalo.</param>
        /// <param name="type">O tipo de símbolo.</param>
        /// <exception cref="UtilitiesException">Se o leitor foi iniciado.</exception>
        public void RegisterCharRangeType(char charOne, char charTwo, SymbType type)
        {
            if (this.started)
            {
                throw new UtilitiesException("Reader has already been started.");
            }
            else
            {
                CharRange range = new CharRange(charOne, charTwo);
                if (!range.IsEmptyRange() && !type.Equals(string.Empty))
                {
                    this.registeredRanges.Add(new StructRangeType<SymbType>() { Range = range, Type = type });
                }
            }
        }

        /// <summary>
        /// Elmina todos os registos de carácteres.
        /// </summary>
        /// <exception cref="UtilitiesException">Se o leitor foi iniciado.</exception>
        public void UnRegisterAll()
        {
            if (this.started)
            {
                throw new UtilitiesException("Reader has already been started.");
            }
            else
            {
                this.charTypes.Clear();
                this.registeredRanges.Clear();
            }
        }

        /// <summary>
        /// Lê o próximo símbolo sem avançar o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
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

        /// <summary>
        /// Lê o próximo símbolo avançando o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
        public override ISymbol<string, SymbType> Get()
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

        /// <summary>
        /// Retrocede o cursor.
        /// </summary>
        public override void UnGet()
        {
            if (bufferPointer > 0)
            {
                --this.bufferPointer;
            }
        }

        /// <summary>
        /// Indica se o leitor encontrou o final de ficheiro.
        /// </summary>
        /// <returns>Verdadeiro caso o leitor tenha encontrado o final de ficheiro e falso caso contrário.</returns>
        public override bool IsAtEOF()
        {
            return this.Peek().SymbolType.Equals(this.endOfFileType);
        }

        /// <summary>
        /// Indica se o símbolo constitui um final de ficheiro.
        /// </summary>
        /// <param name="symbol">O símbolo.</param>
        /// <returns>Verdadeiro caso o símbolo seja um final de ficheiro e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">Se o símbolo for nulo.</exception>
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

        /// <summary>
        /// Obtém o tipo a partir do conjunto de intervalos.
        /// </summary>
        /// <param name="c">O carácter.</param>
        /// <returns>O tipo de símbolo reultante.</returns>
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

        /// <summary>
        /// Deinie um intervalo.
        /// </summary>
        private class CharRange
        {
            /// <summary>
            /// O carácter de início.
            /// </summary>
            private int startChar;

            /// <summary>
            /// O carácter de fim.
            /// </summary>
            private int endChar;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="CharRange"/>.
            /// </summary>
            public CharRange()
            {
                this.SetEmpty();
            }

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="CharRange"/>.
            /// </summary>
            /// <param name="charOne">O primeiro carácter.</param>
            /// <param name="charTwo">O segundo carácter.</param>
            public CharRange(int charOne, int charTwo)
            {
                this.SetRange(charOne, charTwo);
            }

            /// <summary>
            /// Obtém o primeiro carácter.
            /// </summary>
            /// <value>
            /// O primeiro carácter.
            /// </value>
            public int StartChar
            {
                get
                {
                    return startChar;
                }
            }

            /// <summary>
            /// Obtém o segundo carácter.
            /// </summary>
            /// <value>
            /// O segundo carácter.
            /// </value>
            public int EndChar
            {
                get
                {
                    return endChar;
                }
            }

            /// <summary>
            /// Obtém o tamanho do intervalo.
            /// </summary>
            /// <value>
            /// O tamanho do intervalo.
            /// </value>
            public int Length
            {
                get
                {
                    return (int)this.endChar - (int)this.startChar + 1;
                }
            }

            /// <summary>
            /// Estabelece os valores do intevalo.
            /// </summary>
            /// <param name="charOne">O primeiro carácter.</param>
            /// <param name="charTwo">O segundo carácter.</param>
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

            /// <summary>
            /// Estabelece o intervalo vazio.
            /// </summary>
            public void SetEmpty()
            {
                this.startChar = -1;
                this.endChar = -1;
            }

            /// <summary>
            /// Determina se o carácter pertence ao intervalo.
            /// </summary>
            /// <param name="c">O carácter.</param>
            /// <returns>Verdadeiro caso o carácter pertença ao intervalo e falos caso contrário.</returns>
            public bool HasChar(int c)
            {
                return c >= this.startChar && c <= this.endChar;
            }

            /// <summary>
            /// Determina se o intervalo é vazio.
            /// </summary>
            /// <returns>Veradeiro caso o intervalo seja vazio e falso caso contrário.</returns>
            public bool IsEmptyRange()
            {
                return this.startChar < 0 || this.endChar < 0;
            }

            /// <summary>
            /// Vericia se o intervalo contém outro intervalo.
            /// </summary>
            /// <param name="otherCharRange">O intervalo a ser analisado.</param>
            /// <returns>Verdadeiro caso o intervalo contenha o outro e falso caso contrário.</returns>
            public bool Contains(CharRange otherCharRange)
            {
                return this.HasChar(otherCharRange.startChar) && this.HasChar(otherCharRange.endChar);
            }

            /// <summary>
            /// Verifica se o intervalo corrente está contido em outro intervalo.
            /// </summary>
            /// <param name="otherCharRange">O intervalo a ser analisado.</param>
            /// <returns>
            /// Verdadeiro se o intervalo corrente contém o outro intervalo e falso caso contrário.
            /// </returns>
            public bool IsContained(CharRange otherCharRange)
            {
                return otherCharRange.HasChar(this.startChar) && otherCharRange.HasChar(this.endChar);
            }

            /// <summary>
            /// Retorna a intersecção do intervalo corrente com outro intervalo.
            /// </summary>
            /// <param name="otherCharRange">O intervalo.</param>
            /// <returns>A intersecção.</returns>
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

            /// <summary>
            /// Determina se o objecto é igual à instância corrente.
            /// </summary>
            /// <param name="obj">O objecto.</param>
            /// <returns>
            /// Verdadeiro se o objecto for igual à instância corrente e falso caso contrário.
            /// </returns>
            public override bool Equals(object obj)
            {
                CharRange rightHandSide = obj as CharRange;
                if (rightHandSide == null)
                {
                    return false;
                }

                return this.startChar.Equals(rightHandSide.startChar) && this.endChar.Equals(rightHandSide.endChar);
            }

            /// <summary>
            /// Retorna um código confuso para a instância corrente.
            /// </summary>
            /// <returns>
            /// O código confuso para a instância corrente que pode ser utilizado em alguns algoritmos.
            /// </returns>
            public override int GetHashCode()
            {
                return this.startChar ^ this.endChar;
            }

            /// <summary>
            /// Constrói uma representação textual da instância corrente.
            /// </summary>
            /// <returns>A representação textual.</returns>
            public override string ToString()
            {
                if (this.IsEmptyRange())
                {
                    return "Range(empty)";
                }

                return string.Format("Range({0},{1})", this.startChar, this.endChar);
            }
        }

        /// <summary>
        /// Estrutura que permite mapear o interavalo ao seu tipo.
        /// </summary>
        /// <typeparam name="Symb">O tipo dos objectos que constituem os tipos de símbolos.</typeparam>
        private struct StructRangeType<Symb>
        {
            /// <summary>
            /// Obtém ou atribui o intervalo.
            /// </summary>
            /// <value>O interavalo.</value>
            public CharRange Range { get; set; }

            /// <summary>
            /// Obtém ou atribui o tipo de símbolo.
            /// </summary>
            /// <value>
            /// O tipo de símbolo.
            /// </value>
            public Symb Type { get; set; }
        }
    }
}
