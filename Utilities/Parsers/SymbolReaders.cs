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

    /// <summary>
    /// Implementa um leitor de símbolos a partir de um leitor de carácteres.
    /// </summary>
    /// <remarks>
    /// O leitor de símbolos permite identificar algumas sequências de carácteres que ocorrem
    /// com frequência na linguagem C++.
    /// </remarks>
    public class StringSymbolReader : MementoSymbolReader<CharSymbolReader<string>, string, string>
    {
        /// <summary>
        /// O tipo de símbolo correspondente ao final do ficheiro.
        /// </summary>
        private static string endOfFile = "eof";

        /// <summary>
        /// O símbolo actual.
        /// </summary>
        private ISymbol<string, string> currentSymbol = new StringSymbol<string>();

        /// <summary>
        /// O conjunto de estados.
        /// </summary>
        private List<IState<string, string>> stateList = new List<IState<string, string>>();

        /// <summary>
        /// Um mapeamento de texto para tipos de símbolos.
        /// </summary>
        private Dictionary<string, string> keyWords = new Dictionary<string, string>();

        /// <summary>
        /// Valor que indica se os número negativos são lidos com o sinal.
        /// </summary>
        private bool readNegativeNumbers = true;

        /// <summary>
        /// Valor que indica se os símbolos consecutivos marcados como vazios são para considerar como 
        /// um único símbolo.
        /// </summary>
        private bool joinBlancks = true;

        /// <summary>
        /// Instancia um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <param name="isToReadNegativeNumbers">
        /// Veradeiro caso seja para ler números negativos e falso caso seja para ler o sinal
        /// e posteriormente o valor.
        /// </param>
        /// <param name="joinBlancks">
        /// Verdadeiro caso seja para condensar os vazios como espaços, mudanças de linha ou tabulações
        /// e falso se for para ler cada elemento independentemente.
        /// </param>
        public StringSymbolReader(TextReader reader, bool isToReadNegativeNumbers, bool joinBlancks = true)
            : base(new CppCompliantCharSymbolReaderBuilder().BuildReader(reader) as CharSymbolReader<string>)
        {
            this.readNegativeNumbers = isToReadNegativeNumbers;
            this.joinBlancks = joinBlancks;
            this.currentSymbol.SymbolType = "any";
            stateList.Add(new DelegateDrivenState<string, string>(0, "start", this.StartTransition));
            stateList.Add(new DelegateDrivenState<string, string>(1, "string", this.StringTransition));
            stateList.Add(new DelegateDrivenState<string, string>(2, "number", this.NumberTransition));
            stateList.Add(new DelegateDrivenState<string, string>(3, "equal", this.EqualTransition));
            stateList.Add(new DelegateDrivenState<string, string>(4, "greater", this.GreaterTransition));
            stateList.Add(new DelegateDrivenState<string, string>(5, "lesser", this.LesserTransition));
            stateList.Add(new DelegateDrivenState<string, string>(6, "or", this.OrTransition));
            stateList.Add(new DelegateDrivenState<string, string>(7, "and", this.AndTransition));
            stateList.Add(new DelegateDrivenState<string, string>(8, "colon", this.ColonTransition));
            stateList.Add(new DelegateDrivenState<string, string>(9, "plus", this.PlusTransition));
            stateList.Add(new DelegateDrivenState<string, string>(10, "minus", this.MinusTransition));
            stateList.Add(new DelegateDrivenState<string, string>(11, "times", this.TimesTransition));
            stateList.Add(new DelegateDrivenState<string, string>(12, "over", this.OverTransition));
            stateList.Add(new DelegateDrivenState<string, string>(13, "point", this.PointTransition));
            stateList.Add(new DelegateDrivenState<string, string>(14, "blancks", this.BlanckTransition));
            stateList.Add(new DelegateDrivenState<string, string>(15, "exponential", this.ExponentialTransition));
            stateList.Add(new DelegateDrivenState<string, string>(16, "end", this.EndTransition));
        }

        /// <summary>
        /// Lê o próximo símbolo sem avançar o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
        public override ISymbol<string, string> Peek()
        {
            if (!this.started)
            {
                this.started = true;
            }

            if (this.bufferPointer == this.symbolBuffer.Count)
            {
                this.AddNextSymbolFromStream();
            }

            StringSymbol<string> result = new StringSymbol<string>()
            {
                SymbolType = this.symbolBuffer[this.bufferPointer].SymbolType,
                SymbolValue = this.symbolBuffer[this.bufferPointer].SymbolValue
            };

            return result;
        }

        /// <summary>
        /// Lê o próximo símbolo avançando o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
        public override ISymbol<string, string> Get()
        {
            if (!this.started)
            {
                this.started = true;
            }

            ISymbol<string, string> result = this.Peek();
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
            if (this.bufferPointer > 0)
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
            return this.Peek().SymbolType.Equals(StringSymbolReader.endOfFile);
        }

        /// <summary>
        /// Indica se o símbolo constitui um final de ficheiro.
        /// </summary>
        /// <param name="symbol">O símbolo.</param>
        /// <returns>Verdadeiro caso o símbolo seja um final de ficheiro e falso caso contrário.</returns>
        /// <exception cref="ArgumentNullException">O símbolo.</exception>
        public override bool IsAtEOFSymbol(ISymbol<string, string> symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            else
            {
                return symbol.SymbolType.Equals(StringSymbolReader.endOfFile);
            }
        }

        /// <summary>
        /// Reserva um tipo de símbolo para uma palavra.
        /// </summary>
        /// <param name="key">A palavra.</param>
        /// <param name="type">O tipo de símbolo.</param>
        public void RegisterKeyWordType(string key, string type)
        {
            if (this.keyWords.ContainsKey(key))
            {
                this.keyWords[key] = type;
            }
            else
            {
                this.keyWords.Add(key, type);
            }
        }

        /// <summary>
        /// Lê o próximo símbolo do leitor de carácters.
        /// </summary>
        private void AddNextSymbolFromStream()
        {
            this.currentSymbol = new StringSymbol<string>() { SymbolType = string.Empty, SymbolValue = string.Empty };
            StateMachine<string, string> machine = new StateMachine<string, string>(
                this.stateList[0],
                this.stateList[16]);
            machine.RunMachine(this.inputStream);
            var result = new StringSymbol<string>();
            result.SymbolType = this.currentSymbol.SymbolType;
            result.SymbolValue = this.currentSymbol.SymbolValue;
            this.symbolBuffer.Add(result);
        }

        #region transition functions

        /// <summary>
        /// Define a função de transição inicial.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> StartTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> peeked = reader.Peek();
            switch (peeked.SymbolType)
            {
                case "alpha":
                case "underscore":
                    return this.stateList[1];
                case "algarism":
                    return this.stateList[2];
                case "equal":
                    return this.stateList[3];
                case "great_than":
                    return this.stateList[4];
                case "less_than":
                    return this.stateList[5];
                case "bitwise_or":
                    return this.stateList[6];
                case "bitwise_and":
                    return this.stateList[7];
                case "colon":
                    return this.stateList[8];
                case "plus":
                    return this.stateList[9];
                case "minus":
                    return this.stateList[10];
                case "times":
                    return this.stateList[11];
                case "right_bar":
                    return this.stateList[12];
                case "point":
                    return this.stateList[13];
                case "space":
                case "new_line":
                case "carriage_return":
                case "tab":
                    if (this.joinBlancks)
                    {
                        return this.stateList[14];
                    }
                    else
                    {
                        this.currentSymbol = peeked;
                        reader.Get();
                    }

                    break;
                default:
                    this.currentSymbol = peeked;
                    reader.Get();
                    break;
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de texto.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> StringTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (!this.currentSymbol.SymbolType.Equals("string"))
            {
                this.currentSymbol.SymbolType = "string";
            }
            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            switch (symbol.SymbolType)
            {
                case "alpha":
                case "underscore":
                case "algarism":
                    return this.stateList[1];
            }
            if (this.keyWords.ContainsKey(this.currentSymbol.SymbolValue))
            {
                this.currentSymbol.SymbolType = this.keyWords[this.currentSymbol.SymbolValue];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de número.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> NumberTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (
                this.currentSymbol.SymbolType.Equals("any") ||
                this.currentSymbol.SymbolType.Equals(string.Empty) ||
                this.currentSymbol.SymbolType.Equals("minus"))
            {
                this.currentSymbol.SymbolType = "integer";
            }
            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("algarism"))
            {
                return this.stateList[2];
            }
            else if (symbol.SymbolType.Equals("point") && this.currentSymbol.SymbolType.Equals("integer"))
            {
                return this.stateList[13];
            }
            else if (symbol.SymbolType.Equals("alpha") && (symbol.SymbolValue.Equals("e") || symbol.SymbolValue.Equals("E")))
            {
                return this.stateList[15];
            }
            else
            {
                if (this.currentSymbol.SymbolType.Equals("double_exponential"))
                {
                    this.currentSymbol.SymbolType = "double";
                }
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de igual.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> EqualTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "equal":
                    this.currentSymbol.SymbolType = "double_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "plus":
                    this.currentSymbol.SymbolType = "plus_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "minus":
                    this.currentSymbol.SymbolType = "minus_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "times":
                    this.currentSymbol.SymbolType = "times_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "over":
                    this.currentSymbol.SymbolType = "over_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "or":
                    this.currentSymbol.SymbolType = "or_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "and":
                    this.currentSymbol.SymbolType = "and_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "great_than":
                    this.currentSymbol.SymbolType = "great_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                case "less_than":
                    this.currentSymbol.SymbolType = "less_equal";
                    this.currentSymbol.SymbolValue += "=";
                    return this.stateList[16];
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de maior.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> GreaterTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "great_than":
                    this.currentSymbol.SymbolType = "double_great";
                    this.currentSymbol.SymbolValue = ">>";
                    break;
                case "double_great":
                    this.currentSymbol.SymbolType = "triple_great";
                    this.currentSymbol.SymbolValue = ">>>";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("great_than") &&
                (this.currentSymbol.SymbolType.Equals("great_than") || this.currentSymbol.SymbolType.Equals("double_great")))
            {
                return this.stateList[4];
            }
            else if (symbol.SymbolType.Equals("equal") && this.currentSymbol.SymbolType.Equals("great_than"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de menor.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> LesserTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "less_than":
                    this.currentSymbol.SymbolType = "double_less";
                    this.currentSymbol.SymbolValue = "<<";
                    break;
                case "double_less":
                    this.currentSymbol.SymbolType = "triple_less";
                    this.currentSymbol.SymbolValue = "<<<";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("less_than") &&
                (this.currentSymbol.SymbolType.Equals("less_than") || this.currentSymbol.SymbolType.Equals("double_less")))
            {
                return this.stateList[5];
            }
            else if (symbol.SymbolType.Equals("equal") && this.currentSymbol.SymbolType.Equals("less_than"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de ou lógico.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> OrTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "bitwise_or":
                    this.currentSymbol.SymbolType = "double_or";
                    this.currentSymbol.SymbolValue = "||";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("bitwise_or") && this.currentSymbol.SymbolType.Equals("bitwise_or"))
            {
                return this.stateList[6];
            }
            else if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de e lógico.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> AndTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "bitwise_and":
                    this.currentSymbol.SymbolType = "double_and";
                    this.currentSymbol.SymbolValue = "&&";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("bitwise_and") && this.currentSymbol.SymbolType.Equals("bitwise_and"))
            {
                return this.stateList[7];
            }
            else if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de dois pontos.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> ColonTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "colon":
                    this.currentSymbol.SymbolType = "double_colon";
                    this.currentSymbol.SymbolValue = "::";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("colon") && this.currentSymbol.SymbolType.Equals("colon"))
            {
                return this.stateList[8];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de mais.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> PlusTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "plus":
                    this.currentSymbol.SymbolType = "double_plus";
                    this.currentSymbol.SymbolValue = "++";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("plus") && this.currentSymbol.SymbolType.Equals("plus"))
            {
                return this.stateList[9];
            }
            else if (symbol.SymbolType.Equals("equal") && !this.currentSymbol.SymbolType.Equals("double_plus"))
            {
                return this.stateList[3];
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de menos.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> MinusTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "minus":
                    this.currentSymbol.SymbolType = "double_minus";
                    this.currentSymbol.SymbolValue = "--";
                    break;
                case "double":
                    this.currentSymbol.SymbolValue += "-";
                    break;
                case "double_exponential":
                    this.currentSymbol.SymbolValue += "-";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("minus") && this.currentSymbol.SymbolType.Equals("minus"))
            {
                return this.stateList[10];
            }
            else if (symbol.SymbolType.Equals("equal") && this.currentSymbol.SymbolType.Equals("minus"))
            {
                return this.stateList[3];
            }
            else if (this.currentSymbol.SymbolType.Equals("double_minus"))
            {
                return this.stateList[16];
            }
            else if (this.currentSymbol.SymbolType.Equals("exponential_double"))
            {
                if (symbol.SymbolType.Equals("algarism"))
                {
                    return this.stateList[2];
                }
                else
                {
                    this.currentSymbol.SymbolType = "double";
                    this.currentSymbol.SymbolValue = this.currentSymbol.SymbolValue.Substring(0, this.currentSymbol.SymbolValue.Length - 2);
                    reader.UnGet();
                    reader.UnGet();
                }
            }
            else
            {
                if (symbol.SymbolType.Equals("algarism") && this.readNegativeNumbers)
                {
                    return this.stateList[2];
                }
                else if (symbol.SymbolType.Equals("point"))
                {
                    return this.stateList[13];
                }
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de vezes.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> TimesTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "times":
                    this.currentSymbol.SymbolType = "double_times";
                    this.currentSymbol.SymbolValue = "**";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }

            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("times") && this.currentSymbol.SymbolType.Equals("times"))
            {
                return this.stateList[11];
            }
            else if (symbol.SymbolType.Equals("equal") && !this.currentSymbol.SymbolType.Equals("double_times"))
            {
                return this.stateList[3];
            }
            else if (symbol.SymbolType.Equals("right_bar"))
            {
                this.currentSymbol.SymbolType = "end_comment";
                this.currentSymbol.SymbolValue = "*/";
                reader.Get();
            }

            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de dividir.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> OverTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            this.currentSymbol = symbol;
            this.currentSymbol.SymbolType = "over";
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("equal"))
            {
                return this.stateList[3];
            }
            else if (symbol.SymbolType.Equals("times"))
            {
                reader.Get();
                this.currentSymbol.SymbolValue = "/*";
                this.currentSymbol.SymbolType = "start_comment";
                return this.stateList[16];
            }
            else if (symbol.SymbolType.Equals("over"))
            {
                reader.Get();
                this.currentSymbol.SymbolValue = "//";
                this.currentSymbol.SymbolType = "line_comment";
                return this.stateList[16];
            }
            else
            {
                return this.stateList[16];
            }
        }

        /// <summary>
        /// Define a função de transição de ponto.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> PointTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            switch (this.currentSymbol.SymbolType)
            {
                case "minus":
                    this.currentSymbol.SymbolType = "minus_point";
                    this.currentSymbol.SymbolValue += symbol.SymbolValue;
                    break;
                case "integer":
                    this.currentSymbol.SymbolValue += symbol.SymbolValue;
                    this.currentSymbol.SymbolType = "double";
                    break;
                default:
                    this.currentSymbol = symbol;
                    break;
            }

            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("algarism"))
            {
                this.currentSymbol.SymbolType = "double";
                return this.stateList[2];
            }
            else
            {
                if (this.currentSymbol.SymbolType.Equals("minus_point"))
                {
                    this.currentSymbol.SymbolType = "minus";
                    this.currentSymbol.SymbolValue = this.currentSymbol.SymbolValue.Substring(0, this.currentSymbol.SymbolValue.Length - 1);
                    reader.UnGet();
                }
                else if (this.currentSymbol.SymbolType.Equals("double"))
                {
                    this.currentSymbol.SymbolType = "integer";
                    this.currentSymbol.SymbolValue = this.currentSymbol.SymbolValue.Substring(0, this.currentSymbol.SymbolValue.Length - 1);
                    reader.UnGet();
                }
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de vazios.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> BlanckTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (!this.currentSymbol.SymbolType.Equals("blancks"))
            {
                this.currentSymbol.SymbolType = "blancks";
            }

            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            switch (symbol.SymbolType)
            {
                case "space":
                case "new_line":
                case "carriage_return":
                case "tab":
                    return this.stateList[14];
            }

            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição de exponencial.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>O próximo estado.</returns>
        private IState<string, string> ExponentialTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            ISymbol<string, string> symbol = reader.Get();
            if (!this.currentSymbol.SymbolType.Equals("double_exponential_minus"))
            {
                this.currentSymbol.SymbolType = "double_exponential";
            }
            this.currentSymbol.SymbolValue += symbol.SymbolValue;
            symbol = reader.Peek();
            if (symbol.SymbolType.Equals("minus") && this.currentSymbol.SymbolType.Equals("double_exponential"))
            {

                this.currentSymbol.SymbolType = "double_exponential_minus";
                return this.stateList[15];
            }
            else if (symbol.SymbolType.Equals("algarism"))
            {
                this.currentSymbol.SymbolType = "double_exponential";
                return this.stateList[2];
            }
            else
            {
                reader.UnGet();
                string val = this.currentSymbol.SymbolValue;
                if (this.currentSymbol.SymbolType.Equals("double_exponential"))
                {
                    val = val.Substring(0, val.Length - 1);
                }
                else
                {
                    val = val.Substring(0, val.Length - 2);
                    reader.UnGet();
                }
                this.currentSymbol = new StringSymbol<string>()
                {
                    SymbolType = this.GetTypeFromNumberRepresentation(val),
                    SymbolValue = val
                };
            }
            return this.stateList[16];
        }

        /// <summary>
        /// Define a função de transição final.
        /// </summary>
        /// <param name="reader">O leitor de carácteres.</param>
        /// <returns>Nulo.</returns>
        private IState<string, string> EndTransition(IObjectReader<ISymbol<string, string>> reader)
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Determina o tipo de símbolo a partir de um número.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O tipo de símbolo.</returns>
        private string GetTypeFromNumberRepresentation(string number)
        {
            if (number.Contains('.'))
            {
                return "double";
            }
            return "integer";
        }
    }

    /// <summary>
    /// Leitor que permite agrupar os carácters em palavras da mesma espécie às quais
    /// estão associados tipos de símbolos.
    /// </summary>
    /// <remarks>
    /// A título de exemplo, é possível atribuir o tipo ALPHA a todas as letras e o tipo
    /// NUM a todos os algarismos e ANY aos restantes de modo a que o leitor reconheça palavras
    /// vulgares, às quais atribui o tipo de símbolo ALPHA, números inteiros aos quais atribui o tipo
    /// NUM e cadeias constituídas por todos os outros carácteres aos quais atribui o tipo ANY.
    /// </remarks>
    /// <typeparam name="SymbType">O tipo de símbolo.</typeparam>
    public class SimpleTextSymbolReader<SymbType>
        : MementoSymbolReader<CharSymbolReader<SymbType>, string, SymbType>
    {
        /// <summary>
        /// O comparador de símbolos.
        /// </summary>
        private IEqualityComparer<SymbType> symbolTypeEqualityComparer;

        /// <summary>
        /// Define o número de carácteres do mesmo tipo que podem ser agrupados.
        /// </summary>
        private Dictionary<SymbType, int> maxGroupCount;

        /// <summary>
        /// O tipo de símbolo a ser atribuído ao final de ficheiro.
        /// </summary>
        private SymbType endOfFileSymbType;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimpleTextSymbolReader{SymbType}"/>.
        /// </summary>
        /// <param name="reader">O leitor do qual são efectuadas as leituras dos símbolos.</param>
        /// <param name="endOfFileSymbType">O símbolo que será retornado aquando do final de ficheiro.</param>
        public SimpleTextSymbolReader(
            CharSymbolReader<SymbType> reader,
            SymbType endOfFileSymbType)
            : base(reader)
        {
            if (endOfFileSymbType == null)
            {
                throw new ArgumentNullException("endOfFileSymbType");
            }
            else
            {
                this.endOfFileSymbType = endOfFileSymbType;
                this.symbolTypeEqualityComparer = EqualityComparer<SymbType>.Default;
                this.maxGroupCount = new Dictionary<SymbType, int>(
                    EqualityComparer<SymbType>.Default);
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimpleTextSymbolReader{SymbType}"/>.
        /// </summary>
        /// <param name="reader">O leitor do qual são efectuadas as leituras dos símbolos.</param>
        /// <param name="endOfFileSymbType">O símbolo que será retornado aquando do final de ficheiro.</param>
        /// <param name="symbolTypeEqualityComparer">O comparador de símbolos.</param>
        public SimpleTextSymbolReader(
            CharSymbolReader<SymbType> reader,
            SymbType endOfFileSymbType,
            IEqualityComparer<SymbType> symbolTypeEqualityComparer)
            : base(reader)
        {
            if (endOfFileSymbType == null)
            {
                throw new ArgumentNullException("endOfFileSymbType");
            }
            else if (symbolTypeEqualityComparer == null)
            {
                this.endOfFileSymbType = endOfFileSymbType;
                this.symbolTypeEqualityComparer = EqualityComparer<SymbType>.Default;
            }
            else
            {
                this.endOfFileSymbType = endOfFileSymbType;
                this.symbolTypeEqualityComparer = symbolTypeEqualityComparer;
                this.maxGroupCount = new Dictionary<SymbType, int>(symbolTypeEqualityComparer);
            }
        }

        /// <summary>
        /// Obtém ou atribui o símbolo de final de ficheiro.
        /// </summary>
        /// <exception cref="UtilitiesException">
        /// O valor atribuído é nulo.
        /// </exception>
        public SymbType EndOfFileSymbType
        {
            get
            {
                return this.endOfFileSymbType;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("The end of file symbol can't be null.");
                }
                else
                {
                    this.endOfFileSymbType = value;
                }
            }
        }

        /// <summary>
        /// Estabelece o número máximo de carácteres de um determinado tipo
        /// que podem ser agrupados.
        /// </summary>
        /// <param name="symbType">O tipo de símbolo ao qual o carácter pertence.</param>
        /// <param name="count">O número máximo de símbolos que podem ser agrupados.</param>
        public void SetGroupCount(SymbType symbType, int count)
        {
            if (symbType == null)
            {
                throw new ArgumentNullException("symbType");
            }
            else if (count > 0)
            {
                if (this.maxGroupCount.ContainsKey(symbType))
                {
                    this.maxGroupCount[symbType] = count;
                }
                else
                {
                    this.maxGroupCount.Add(symbType, count);
                }
            }
            else
            {
                throw new ArgumentException("The number of grouped items must be greater than zero.");
            }
        }

        /// <summary>
        /// Elimina qualquer limite de contagem que possa ter sido atribuído ao
        /// tipo de símbolo.
        /// </summary>
        /// <param name="symbType">O tipo de símbolo.</param>
        public void SetUnlimitedCount(SymbType symbType)
        {
            if (symbType != null)
            {
                this.maxGroupCount.Remove(symbType);
            }
        }

        /// <summary>
        /// Remove o limite de contagem de qualquer símbolo que tenha sido
        /// previamente atrbiuído.
        /// </summary>
        public void SetAllUnlimited()
        {
            this.maxGroupCount.Clear();
        }

        /// <summary>
        /// Efectua a leitura do próximo símbolo sem avançar o cursor.
        /// </summary>
        /// <returns>O próximo símbolo no leitor.</returns>
        public override ISymbol<string, SymbType> Peek()
        {
            this.started = true;
            if (this.bufferPointer == this.symbolBuffer.Count)
            {
                if (this.inputStream.IsAtEOF())
                {
                    var result = new GeneralSymbol<string, SymbType>()
                    {
                        SymbolType = this.endOfFileSymbType,
                        SymbolValue = string.Empty
                    };

                    return result;
                }
                else
                {
                    this.AddNextSymbolFromStream();
                    var result = new GeneralSymbol<string, SymbType>()
                    {
                        SymbolType = this.symbolBuffer[this.bufferPointer].SymbolType,
                        SymbolValue = this.symbolBuffer[this.bufferPointer].SymbolValue
                    };

                    return result;
                }
            }
            else
            {
                var result = new GeneralSymbol<string, SymbType>()
                {
                    SymbolType = this.symbolBuffer[this.bufferPointer].SymbolType,
                    SymbolValue = this.symbolBuffer[this.bufferPointer].SymbolValue
                };

                return result;
            }
        }

        /// <summary>
        /// Efectua a leitura do próximo símbolo e avança o cursor.
        /// </summary>
        /// <returns>O próximo símbolo no leitor.</returns>
        public override ISymbol<string, SymbType> Get()
        {
            this.started = true;
            var result = this.Peek();
            if (this.bufferPointer < this.symbolBuffer.Count)
            {
                ++this.bufferPointer;
            }

            return result;
        }

        /// <summary>
        /// Repõe o símbolo correspondente ao cursor.
        /// </summary>
        public override void UnGet()
        {
            if (this.bufferPointer > 0)
            {
                --this.bufferPointer;
            }
        }

        /// <summary>
        /// Verifica se o cursor se encontra no final do cursor.
        /// </summary>
        /// <returns>Verdadeiro caso o cursor se encontre no final do cursor e falso caso contrário.</returns>
        public override bool IsAtEOF()
        {
            return this.bufferPointer == this.symbolBuffer.Count
                && this.inputStream.IsAtEOF();
        }

        /// <summary>
        /// Verifica se um determinado símbolo corresponde ao símbolo retornado quando o
        /// cursor se encontra no final do leitor.
        /// </summary>
        /// <param name="symbol">O símbolo.</param>
        /// <returns>Verdadeiro caso o símbolo marque o final do leitor e falso caso contrário.</returns>
        public override bool IsAtEOFSymbol(ISymbol<string, SymbType> symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            else
            {
                return this.symbolTypeEqualityComparer.Equals(
                    this.endOfFileSymbType,
                    symbol.SymbolType);
            }
        }

        /// <summary>
        /// Lê o próximo símbolo do leitor de carácters.
        /// </summary>
        private void AddNextSymbolFromStream()
        {
            var readed = this.inputStream.Get();

            var symbType = readed.SymbolType;
            var symbValue = readed.SymbolValue;
            var count = 1;

            if (!this.inputStream.IsAtEOF())
            {
                var peeked = this.inputStream.Peek();
                var state = this.symbolTypeEqualityComparer.Equals(
                    readed.SymbolType,
                    peeked.SymbolType);

                var maxCount = default(int);
                if (this.maxGroupCount.TryGetValue(peeked.SymbolType, out maxCount))
                {
                    state &= count < maxCount;
                    ++count;
                }

                while (state)
                {
                    readed = this.inputStream.Get();
                    symbValue += readed.SymbolValue;
                    if (this.inputStream.IsAtEOF())
                    {
                        state = false;
                    }
                    else
                    {
                        peeked = this.inputStream.Peek();
                        state = this.symbolTypeEqualityComparer.Equals(
                            readed.SymbolType,
                            peeked.SymbolType);
                        if (this.maxGroupCount.TryGetValue(peeked.SymbolType, out maxCount))
                        {
                            state &= count < maxCount;
                            ++count;
                        }
                    }
                }
            }

            // Adiciona o símbolo ao contentor.
            var symbol = new GeneralSymbol<string, SymbType>()
            {
                SymbolValue = symbValue,
                SymbolType = symbType
            };

            this.symbolBuffer.Add(symbol);
        }

        /// <summary>
        /// O delegado reponsável pela determinação do tipo de símbolo a partir de um carácter.
        /// </summary>
        /// <param name="c">O carácter.</param>
        /// <returns>O tipo de símbolo ao qual pertence.</returns>
        public delegate SymbType GetSymbolFromCharacter(char c);
    }

    /// <summary>
    /// Permite a leitura organizada a partir de um vector de símbolos.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo de objectos que constituem os valores.</typeparam>
    /// <typeparam name="TSymbType">O tipo de objectos que constituem os tipos de símbolos.</typeparam>
    public class ArraySymbolReader<TSymbVal, TSymbType> : MementoSymbolReader<ISymbol<TSymbVal, TSymbType>[], TSymbVal, TSymbType>
    {
        /// <summary>
        /// O símbolo correspondente ao fim do ficheiro.
        /// </summary>
        private TSymbType endOfFileSymbolType;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ArraySymbolReader{TSymbVal, TSymbType}"/>.
        /// </summary>
        /// <param name="arrayOfSymbols">O vector de símbolos.</param>
        /// <param name="endOfFileSymbolType">O tipo de símbolo que corresponde ao final do ficheiro.</param>
        public ArraySymbolReader(ISymbol<TSymbVal, TSymbType>[] arrayOfSymbols, TSymbType endOfFileSymbolType)
            : base(arrayOfSymbols)
        {
            this.bufferPointer = -1;
            this.symbolBuffer.AddRange(arrayOfSymbols);
            this.endOfFileSymbolType = endOfFileSymbolType;
        }

        /// <summary>
        /// Obtém o próximo símbolo mas não avança o cursor.
        /// </summary>
        /// <returns>O símbolo.</returns>
        public override ISymbol<TSymbVal, TSymbType> Peek()
        {
            var nextPointer = this.bufferPointer + 1;
            if (nextPointer >= this.symbolBuffer.Count)
            {
                return new ArraySymbol() { SymbolValue = default(TSymbVal), SymbolType = this.endOfFileSymbolType };
            }
            else
            {
                return this.symbolBuffer[nextPointer];
            }
        }

        /// <summary>
        /// Lê o próximo símbolo avançando o cursor.
        /// </summary>
        /// <returns>O símbolo lido.</returns>
        public override ISymbol<TSymbVal, TSymbType> Get()
        {
            if (this.bufferPointer < this.symbolBuffer.Count)
            {
                ++this.bufferPointer;
                if (this.bufferPointer < this.symbolBuffer.Count)
                {
                    return this.symbolBuffer[this.bufferPointer];
                }
                else
                {
                    return new ArraySymbol() { SymbolValue = default(TSymbVal), SymbolType = this.endOfFileSymbolType };
                }
            }
            else
            {
                return new ArraySymbol() { SymbolValue = default(TSymbVal), SymbolType = this.endOfFileSymbolType };
            }
        }

        /// <summary>
        /// Retrocede o cursor.
        /// </summary>
        public override void UnGet()
        {
            if (this.bufferPointer > -1)
            {
                --this.bufferPointer;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o leitor se encontra no final.
        /// </summary>
        /// <returns>Verdadeiro caso o leitor se encontre no final e falso caso contrário.</returns>
        public override bool IsAtEOF()
        {
            return this.bufferPointer >= this.symbolBuffer.Count - 1;
        }

        /// <summary>
        /// Obtém um valor que indica se o símbolo proporcionado correspode ao fim de ficheiro.
        /// </summary>
        /// <param name="symbol">O símbolo a ser verificado.</param>
        /// <returns>Verdadeiro caso o símbolo seja final de ficheiro e falso caso contrário.</returns>
        public override bool IsAtEOFSymbol(ISymbol<TSymbVal, TSymbType> symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }
            else
            {
                return this.endOfFileSymbolType.Equals(symbol.SymbolType);
            }
        }

        /// <summary>
        /// Impelmenta um símbolo lido de um vector de símbolos.
        /// </summary>
        private class ArraySymbol : ISymbol<TSymbVal, TSymbType>
        {
            /// <summary>
            /// O valor do símbolo.
            /// </summary>
            private TSymbVal symbolValue;

            /// <summary>
            /// O tipo do símbolo.
            /// </summary>
            private TSymbType symbolType;

            /// <summary>
            /// Obtém ou atribui o valor do símbolo.
            /// </summary>
            /// <value>
            /// O valor do símbolo.
            /// </value>
            public TSymbVal SymbolValue
            {
                get
                {
                    return this.symbolValue;
                }
                set
                {
                    this.symbolValue = value;
                }
            }

            /// <summary>
            /// Obtém ou atribui o tipo de símbolo.
            /// </summary>
            /// <value>
            /// O tipo de símbolo.
            /// </value>
            public TSymbType SymbolType
            {
                get
                {
                    return this.symbolType;
                }
                set
                {
                    this.symbolType = value;
                }
            }
        }
    }
}
