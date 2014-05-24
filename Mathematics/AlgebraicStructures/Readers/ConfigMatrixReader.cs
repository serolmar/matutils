namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa um leitor de matrizes.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas dos alcances multidimensionais.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    /// <typeparam name="InputReader">O tipo do leitor de entrada..</typeparam>
    public class ConfigMatrixReader<T, SymbValue, SymbType, InputReader>
    {
        /// <summary>
        /// O leitor de alcances multidimensionais.
        /// </summary>
        private ARangeReader<T, SymbValue, SymbType, InputReader> rangeReader;

        /// <summary>
        /// A fábrica responsável pela criação de matrizes.
        /// </summary>
        private IMatrixFactory<T> matrixFactory;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ConfigMatrixReader{T, SymbValue, SymbType, InputReader}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        /// <param name="matrixFactory">A fábrica responsável pela criação de matrizes.</param>
        /// <exception cref="ArgumentNullException">Se a fábrica de matrizes for nula.</exception>
        public ConfigMatrixReader(int lines, int columns, IMatrixFactory<T> matrixFactory)
        {
            if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                this.matrixFactory = matrixFactory;
                this.rangeReader = new RangeConfigReader<T, SymbValue, SymbType, InputReader>(
                    new int[] { lines, columns });
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
        /// Tenta ler a matriz a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            out IMatrix<T> matrix)
        {
            return this.TryParseMatrix(reader, parser, null, out matrix);
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos estabelecendo um valor por defeito.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            T defaultValue,
            out IMatrix<T> matrix)
        {
            return this.TryParseMatrix(reader, parser, null, defaultValue, out matrix);
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos estabelecendo um valor por defeito.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="errors">Os erros da leitura.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            List<string> errors,
            T defaultValue,
            out IMatrix<T> matrix)
        {
            matrix = default(ArrayMatrix<T>);
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
                var columns = -1;
                var configurationEnumerator = this.rangeReader.Configuration.GetEnumerator();
                if (configurationEnumerator.MoveNext())
                {
                    lines = configurationEnumerator.Current;
                    if (configurationEnumerator.MoveNext())
                    {
                        columns = configurationEnumerator.Current;
                    }
                }

                matrix = this.matrixFactory.CreateMatrix(lines, columns, defaultValue);
                this.SetupResultMatrix(matrix, new int[] { lines, columns }, this.rangeReader.Elements);
                return true;
            }
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="errors">Os erros da leitura.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            List<string> errors,
            out IMatrix<T> matrix)
        {
            matrix = default(ArrayMatrix<T>);
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
                var columns = -1;
                var configurationEnumerator = this.rangeReader.Configuration.GetEnumerator();
                if (configurationEnumerator.MoveNext())
                {
                    lines = configurationEnumerator.Current;
                    if (configurationEnumerator.MoveNext())
                    {
                        columns = configurationEnumerator.Current;
                    }
                }

                matrix = this.matrixFactory.CreateMatrix(lines, columns);
                this.SetupResultMatrix(matrix, new int[] { lines, columns }, this.rangeReader.Elements);
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
        /// Atribui os dados lidos à matriz.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <param name="configuration">A configuração dos elementos.</param>
        /// <param name="elements">Os elementos lidos.</param>
        private void SetupResultMatrix(IMatrix<T> matrix, int[] configuration, ReadOnlyCollection<T> elements)
        {
            var currentLine = -1;
            var currentColumn = 0;
            for (int i = 0; i < elements.Count; ++i)
            {
                ++currentLine;
                if (currentLine >= configuration[0])
                {
                    currentLine = 0;
                    ++currentColumn;
                }

                matrix[currentLine, currentColumn] = elements[i];
            }
        }
    }
}
