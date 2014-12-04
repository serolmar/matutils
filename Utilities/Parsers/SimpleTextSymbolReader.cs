namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

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
}
