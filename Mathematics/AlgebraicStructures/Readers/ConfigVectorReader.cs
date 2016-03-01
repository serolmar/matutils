namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite realizar a leitura de um vector.
    /// </summary>
    /// <typeparam name="T">O tipo de elementos do vector.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">O tipo dos identificadores dos símbolos.</typeparam>
    /// <typeparam name="InputReader">O leitor para os dados de entrada.</typeparam>
    public class ConfigVectorReader<T, SymbValue, SymbType, InputReader>
    {
        /// <summary>
        /// O leitor de alcances multidimensionais.
        /// </summary>
        private ARangeReader<T, SymbValue, SymbType> rangeReader;

        /// <summary>
        /// A fábrica responsável pela criação de vectores.
        /// </summary>
        private IVectorFactory<T> vectorFactory;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ConfigVectorReader{T, SymbValue, SymbType, InputReader}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="vectorFactory">A fábrica responsável pela criação de vectores.</param>
        /// <exception cref="ArgumentNullException">Se a fábrica de vectores for nula.</exception>
        public ConfigVectorReader(int lines, IVectorFactory<T> vectorFactory)
        {
            if (vectorFactory == null)
            {
                throw new ArgumentNullException("vectorFactory");
            }
            else
            {
                this.vectorFactory = vectorFactory;
                this.rangeReader = new RangeConfigReader<T, SymbValue, SymbType>(
                    new int[] { lines});
            }
        }

        /// <summary>
        /// Obtém e atribui o tipo de símbolo que representa um separador de objectos.
        /// </summary>
        /// <value>
        /// O tipo de separador.
        /// </value>
        public SymbType SeparatorSymbType
        {
            get
            {
                return this.rangeReader.SeparatorSymbType;
            }
            set
            {
                this.rangeReader.SeparatorSymbType = value;
            }
        }

        /// <summary>
        /// Tenta ler o vector a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador do vector.</param>
        /// <param name="vector">Estabelece vector lido.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseVector(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            out IMathVector<T> vector)
        {
            return this.TryParseVector(reader, parser, null, out vector);
        }

        /// <summary>
        /// Tenta ler o vector a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador do vector.</param>
        /// <param name="errors">A lista de erros.</param>
        /// <param name="vector">Estabelece o vector lido.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseVector(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            List<string> errors,
            out IMathVector<T> vector)
        {
            vector = default(IMathVector<T>);
            this.rangeReader.ReadRangeValues(reader, parser);
            if (this.rangeReader.HasErrors)
            {
                if (errors != null)
                {
                    foreach (var message in this.rangeReader.ErrorMessages)
                    {
                        errors.Add(message);
                    }
                }

                return false;
            }
            else
            {
                var lines = -1;
                var configurationEnumerator = this.rangeReader.Configuration.GetEnumerator();
                if (configurationEnumerator.MoveNext())
                {
                    lines = configurationEnumerator.Current;
                    if (configurationEnumerator.MoveNext())
                    {
                        lines = configurationEnumerator.Current;
                    }
                }

                vector = this.vectorFactory.CreateVector(lines);
                this.SetupResultVector(vector, lines, this.rangeReader.Elements);
                return true;
            }
        }

        /// <summary>
        /// Mapeia um símbolo interno de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbolType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        public void MapInternalDelimiters(SymbType openSymbolType, SymbType closeSymbType)
        {
            this.rangeReader.MapInternalDelimiters(openSymbolType, closeSymbType);
        }

        /// <summary>
        /// Mapeia um símbolo externo de fecho a um símbolo de abertura.
        /// </summary>
        /// <param name="openSymbType">O tipo de símbolo que representa um delimitador de abertura.</param>
        /// <param name="closeSymbType">O tipo de símbolo que representa um delimitador de fecho.</param>
        public void MapExternalDelimiters(SymbType openSymbType, SymbType closeSymbType)
        {
            this.rangeReader.MapExternalDelimiters(openSymbType, closeSymbType);
        }

        /// <summary>
        /// Marca um tipo de símbolo como sendo vazio.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void AddBlanckSymbolType(SymbType symbolType)
        {
            this.rangeReader.AddBlanckSymbolType(symbolType);
        }

        /// <summary>
        /// Remove o tipo de símbolo especificado da lista de símbolos vazios.
        /// </summary>
        /// <param name="symbolType">O tipo de símbolo.</param>
        public void RemoveBlanckSymbolType(SymbType symbolType)
        {
            this.rangeReader.RemoveBlanckSymbolType(symbolType);
        }

        /// <summary>
        /// Desmarca todos os símbolos vazios.
        /// </summary>
        public void ClearBlanckSymbols()
        {
            this.rangeReader.ClearBlanckSymbols();
        }

        /// <summary>
        /// Constrói o vector após a leitura.
        /// </summary>
        /// <param name="vector">O vector.</param>
        /// <param name="lines">O número de linhas lidas.</param>
        /// <param name="elements">A lista dos elementos.</param>
        private void SetupResultVector(IMathVector<T> vector, int lines, ReadOnlyCollection<T> elements)
        {
            var currentLine = -1;
            for (int i = 0; i < lines; ++i)
            {
                ++currentLine;
                vector[currentLine] = elements[i];
            }
        }
    }
}
