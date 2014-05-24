using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Utilities.Collections;
using System.Collections.ObjectModel;

namespace Mathematics
{
    /// <summary>
    /// Implementa a base para uma leituro de alcances multidimensionais.
    /// </summary>
    /// <remarks>
    /// A leitura de alcances multidimensionais é realizada sobre um leitor de símbolos que, por sua vez,
    /// assenta sobre um leitor de objectos arbitrários. O leitor de símbolos consiste, portanto, um classficador
    /// e aglomerador de objectos.
    /// </remarks>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    /// <typeparam name="InputReader">O tipo do leitor de entrada..</typeparam>
    public abstract class ARangeReader<T, SymbValue, SymbType, InputReader>
    {
        #region Fields

        /// <summary>
        /// Um contentor de símbolos que representam os valores que vão sendo lidos.
        /// </summary>
        protected List<ISymbol<SymbValue, SymbType>> currentElementSymbols = new List<ISymbol<SymbValue, SymbType>>();

        /// <summary>
        /// Contentor para os mapeamentos de símbols de fecho internos a símbolos de abertura.
        /// </summary>
        protected GeneralMapper<SymbType, SymbType> mapInternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        /// <summary>
        /// Contentor para os mapeamentos de símbolos de fecho que externos e símbols de abertura.
        /// </summary>
        protected GeneralMapper<SymbType, SymbType> mapExternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        /// <summary>
        /// O tipo de símbolo que representa um separador.
        /// </summary>
        protected SymbType separatorSymb;

        /// <summary>
        /// Valor que indica se a leitura foi iniciada.
        /// </summary>
        protected bool hasStarted;

        /// <summary>
        /// Valor que indica se a leitura foi bem sucedida ou não.
        /// </summary>
        protected bool hasErrors;

        /// <summary>
        /// O conjunto de mensagens enviadas.
        /// </summary>
        protected List<string> errorMessages;

        /// <summary>
        /// Mantém a lista de símbolos que são ignorados.
        /// </summary>
        protected List<SymbType> blancks = new List<SymbType>();

        #endregion

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ARangeReader{T, SymbValue, SymbType, InputReader}"/>.
        /// </summary>
        public ARangeReader()
        {
            this.hasErrors = false;
            this.errorMessages = new List<string>();
        }

        /// <summary>
        /// Obtém a configuração lida.
        /// </summary>
        /// <value>
        /// A configuração lida.
        /// </value>
        public IEnumerable<int> Configuration
        {
            get
            {
                return this.GetFinalCofiguration();
            }
        }

        /// <summary>
        /// Obtém os elementos lidos.
        /// </summary>
        /// <value>
        /// Os elementos lidos.
        /// </value>
        public ReadOnlyCollection<T> Elements
        {
            get
            {
                return this.GetElements();
            }
        }

        /// <summary>
        /// Obtém um valor que indica se foram encontrados erros na leitura.
        /// </summary>
        /// <value>
        /// Verdadeiro caso tenham sido encontrados erros e falso caso contrário.
        /// </value>
        public bool HasErrors
        {
            get
            {
                return this.hasErrors;
            }
        }

        /// <summary>
        /// Obtém as mensagens de erro.
        /// </summary>
        /// <value>
        /// As mensagens de erro.
        /// </value>
        public ReadOnlyCollection<string> ErrorMessages
        {
            get
            {
                return this.errorMessages.AsReadOnly();
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o leitor foi iniciado.
        /// </summary>
        /// <value>
        /// Verdadeiro caso o leitor tenha sido iniciado e falso caso contrário.
        /// </value>
        public bool HasStarted
        {
            get
            {
                return this.hasStarted;
            }
        }

        /// <summary>
        /// Obtém e atribui o tipo de símbolo que representa um separador de objectos.
        /// </summary>
        /// <value>
        /// O tipo de separador.
        /// </value>
        /// <exception cref="ArgumentException">Se o tipo de símbolo estiver marcado como vazio.</exception>
        public SymbType SeparatorSymbType
        {
            get
            {
                return this.separatorSymb;
            }
            set
            {
                if (this.blancks.Contains(value))
                {
                    throw new ArgumentException("Can't use a blanck symbol as a separator.");
                }
                else
                {
                    this.separatorSymb = value;
                }
            }
        }

        /// <summary>
        /// Efectua a leitura dos valores.
        /// </summary>
        /// <remarks>
        /// O controlo da leitura dos valores é passado para o leitor de objectos sempre que uma sequência
        /// de símbolos nestas condições é encontrada.
        /// </remarks>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O leitor responsável pela leitura dos valores.</param>
        /// <exception cref="MathematicsException">
        /// Se nenhum leitor de objectos for providenciado, nenhum delimitador interno estiver definido ou se nenhum
        /// separador de objectos estiver definido.
        /// </exception>
        public void ReadRangeValues(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser)
        {
            if (this.hasStarted)
            {
                throw new MathematicsException("Reader has already been started.");
            }
            else
            {
                try
                {
                    if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.Objects.Count == 0)
                    {
                        throw new MathematicsException("No internal delimiter symbols were provided.");
                    }
                    else if (this.separatorSymb == null)
                    {
                        throw new MathematicsException("No separator symbol was provided.");
                    }
                    else
                    {
                        this.hasStarted = true;
                        this.errorMessages.Clear();
                        this.hasErrors = false;
                        this.InnerReadRangeValues(reader, parser);
                        this.hasStarted = false;
                    }
                }
                catch (Exception exception)
                {
                    this.hasStarted = false;
                    this.hasErrors = true;
                    throw exception;
                }
            }
        }

        #region Public Methods

        /// <summary>
        /// Mapeia um símbolo interno de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbolType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        /// <exception cref="ExpressionReaderException">
        /// Se algum dos tipos de símbolos proporcionados forem marcados como símbolo vazio.
        /// </exception>
        public void MapInternalDelimiters(SymbType openSymbolType, SymbType closeSymbType)
        {
            if (this.blancks.Contains(openSymbolType) || this.blancks.Contains(closeSymbType))
            {
                throw new ExpressionReaderException(
                    "Can't mark a blanck symbol as a delimiter type. Please remove symbol from blancks before mark it as a delimiter.");
            }
            else
            {
                this.mapInternalOpenDelimitersToCloseDelimitersTypes.Add(openSymbolType, closeSymbType);
            }
        }

        /// <summary>
        /// Mapeia um símbolo externo de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        /// <exception cref="ExpressionReaderException">
        /// Se algum dos tipos de símbolos proporcionados forem marcados como símbolo vazio.
        /// </exception>
        public void MapExternalDelimiters(SymbType openSymbType, SymbType closeSymbType)
        {
            if (this.blancks.Contains(openSymbType) || this.blancks.Contains(closeSymbType))
            {
                throw new ExpressionReaderException("Can't mark a blanck symbol as a delimiter type. Please remove symbol from blancks before mark it as a delimiter.");
            }
            else
            {
                this.mapExternalOpenDelimitersToCloseDelimitersTypes.Add(openSymbType, closeSymbType);
            }
        }

        /// <summary>
        /// Marca um tipo de símbolo como sendo vazio.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        /// <exception cref="ExpressionReaderException">Se o símbolo for um delimitador ou um separador.</exception>
        public void AddBlanckSymbolType(SymbType symbolType)
        {
            if (symbolType != null)
            {
                if (symbolType.Equals(this.separatorSymb))
                {
                    throw new ExpressionReaderException("Can't mark the separator as a blank symbol.");
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(symbolType) || this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(symbolType))
                {
                    throw new ExpressionReaderException("Can't mark a delimiter as a blank symbol.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(symbolType) || this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(symbolType))
                {
                    throw new ExpressionReaderException("Can't mark a delimiter as a blank symbol.");
                }

                if (!this.blancks.Contains(symbolType))
                {
                    this.blancks.Add(symbolType);
                }
            }
        }

        /// <summary>
        /// Remove o tipo de símbolo especificado da lista de símbolos vazios.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void RemoveBlanckSymbolType(SymbType symbolType)
        {
            this.blancks.Remove(symbolType);
        }

        /// <summary>
        /// Desmarca todos os símbolos vazios.
        /// </summary>
        public void ClearBlanckSymbols()
        {
            this.blancks.Clear();
        }
        #endregion

        /// <summary>
        /// Efectua leitura do alcance multidimensional.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O leitor de objectos.</param>
        protected abstract void InnerReadRangeValues(MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser);

        /// <summary>
        /// Obtém a configuração após uma leitura.
        /// </summary>
        /// <returns>A configuração.</returns>
        protected abstract IEnumerable<int> GetFinalCofiguration();

        /// <summary>
        /// Obtém os elmentos lidos após a leitura.
        /// </summary>
        /// <returns>Os elementos lidos.</returns>
        protected abstract ReadOnlyCollection<T> GetElements();
    }
}
