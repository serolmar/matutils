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
    /// <typeparam name="M">O tipo da matriz.</typeparam>
    /// <typeparam name="SymbValue">O tipo dos objectos que costituem os valores dos símbolos.</typeparam>
    /// <typeparam name="SymbType">Os tipos de objectos que constituem os tipos de símbolos.</typeparam>
    public class ConfigMatrixReader<T, M, SymbValue, SymbType>
        where M : IMatrix<T>
    {
        /// <summary>
        /// O leitor de alcances multidimensionais.
        /// </summary>
        private ARangeReader<T, SymbValue, SymbType> rangeReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ConfigMatrixReader{T, M, SymbValue, SymbType}"/>.
        /// </summary>
        /// <param name="lines">O número de linhas.</param>
        /// <param name="columns">O número de colunas.</param>
        public ConfigMatrixReader(int lines, int columns)
        {
            this.rangeReader = new RangeConfigReader<T, SymbValue, SymbType>(
                    new int[] { lines, columns });
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
        /// <param name="matrixFactory">A função responsável pela criação das matrizes.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            Func<int,int,M> matrixFactory,
            out M matrix)
        {
            return this.TryParseMatrix(reader, parser, null, matrixFactory, out matrix);
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos estabelecendo um valor por defeito.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="errors">Os erros da leitura.</param>
        /// <param name="defaultValue">O valor por defeito.</param>
        /// <param name="matrixFactory">A função responsável pela criação de matrizes.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            ILogStatus<string, EParseErrorLevel> errors,
            T defaultValue,
            Func<int,int,T,M> matrixFactory,
            out M matrix)
        {
            if (matrixFactory == null)
            {
                throw new ArgumentNullException("matrixFactory");
            }
            else
            {
                matrix = default(M);
                this.rangeReader.ReadRangeValues(reader, parser);
                if (this.rangeReader.HasErrors)
                {
                    if (errors != null)
                    {
                        foreach (var message in this.rangeReader.ErrorMessages)
                        {
                            errors.AddLog(message, EParseErrorLevel.ERROR);
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

                    matrix = matrixFactory.Invoke(lines, columns, defaultValue);
                    this.SetupResultMatrix(matrix, new int[] { lines, columns }, this.rangeReader.Elements);
                    return true;
                }
            }
        }

        /// <summary>
        /// Tenta ler a matriz a partir de um leitor de símbolos.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <param name="parser">O interpretador da matriz.</param>
        /// <param name="errors">Os erros da leitura.</param>
        /// <param name="matrixFactory">A função responsável pela criação de matrizes.</param>
        /// <param name="matrix">Estabelece a matriz lida.</param>
        /// <returns>Verdadeiro caso a operação seja bem sucedida e falso caso contrário.</returns>
        public bool TryParseMatrix(
            IMementoSymbolReader<SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser,
            ILogStatus<string, EParseErrorLevel> errors,
            Func<int,int, M> matrixFactory,
            out M matrix)
        {
            matrix = default(M);
            this.rangeReader.ReadRangeValues(reader, parser);
            if (this.rangeReader.HasErrors)
            {
                if (errors != null)
                {
                    foreach (var message in this.rangeReader.ErrorMessages)
                    {
                        errors.AddLog(message, EParseErrorLevel.ERROR);
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

                matrix = matrixFactory.Invoke(lines, columns);
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
        private void SetupResultMatrix(M matrix, int[] configuration, ReadOnlyCollection<T> elements)
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
