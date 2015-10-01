namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Numerics;
    using System.Text;

    /// <summary>
    /// Implementa um leitor e escritor de ficheiros CSV.
    /// </summary>
    public class CsvFileReaderWriter
    {
        /// <summary>
        /// O separador de colunas no csv.
        /// </summary>
        private char columnSeparatorChar = ';';

        /// <summary>
        /// O separador de linhas no csv.
        /// </summary>
        private char lineSeparatorChar = '\n';

        /// <summary>
        /// Define o delimitador.
        /// </summary>
        private char delimiter = '"';

        /// <summary>
        /// A função que permite efectuar a leitura dos campos.
        /// </summary>
        private Func<int, int, string, object> fieldReader;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CsvFileReaderWriter"/>.
        /// </summary>
        public CsvFileReaderWriter()
        {
            // A função por defeito retorna sempre texto.
            this.fieldReader = (i, j, t) => t;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CsvFileReaderWriter"/>.
        /// </summary>
        /// <param name="fieldReader">O leitor dos campos.</param>
        public CsvFileReaderWriter(Func<int, int, string, object> fieldReader)
        {
            if (fieldReader == null)
            {
                throw new ArgumentNullException("fieldReader");
            }
            else
            {
                this.fieldReader = fieldReader;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CsvFileReaderWriter"/>.
        /// </summary>
        /// <param name="lineSeparatorChar">O carácter que representa o separador de linhas.</param>
        /// <param name="columnSeparatorChar">O carácter que representa o separador de colunas.</param>
        /// <param name="delimiter">O carácter delimitador.</param>
        /// <exception cref="UtilitiesDataException">Se os carácteres separadores forem letras ou dígitos.</exception>
        public CsvFileReaderWriter(char lineSeparatorChar, char columnSeparatorChar, char delimiter)
            : this()
        {
            if (char.IsLetterOrDigit(lineSeparatorChar))
            {
                throw new UtilitiesDataException("Line separator can't be a letter nor a digit.");
            }
            else if (char.IsLetterOrDigit(columnSeparatorChar))
            {
                throw new UtilitiesDataException("Column separator can't be a letter nor a digit.");
            }
            else if (char.IsLetterOrDigit(delimiter))
            {
                throw new UtilitiesDataException("Delimtier can't be a letter nor a digit.");
            }
            else
            {
                this.delimiter = delimiter;
                this.columnSeparatorChar = columnSeparatorChar;
                this.lineSeparatorChar = lineSeparatorChar;
            }
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CsvFileReaderWriter"/>.
        /// </summary>
        /// <param name="lineSeparatorChar">O carácter que representa o separador de linhas.</param>
        /// <param name="columnSeparatorChar">O carácter que representa o separador de colunas.</param>
        /// <param name="delimiter">O carácter delimitador.</param>
        /// <param name="fieldReader">O leitor de campos.</param>
        public CsvFileReaderWriter(
            char lineSeparatorChar,
            char columnSeparatorChar,
            char delimiter,
            Func<int, int, string, object> fieldReader)
        {
            if (fieldReader == null)
            {
                throw new ArgumentNullException("fieldReader");
            }
            else if (char.IsLetterOrDigit(lineSeparatorChar))
            {
                throw new UtilitiesDataException("Line separator can't be a letter nor a digit.");
            }
            else if (char.IsLetterOrDigit(columnSeparatorChar))
            {
                throw new UtilitiesDataException("Column separator can't be a letter nor a digit.");
            }
            else if (char.IsLetterOrDigit(delimiter))
            {
                throw new UtilitiesDataException("Delimiter can't be a letter nor a digit.");
            }
            else
            {
                this.columnSeparatorChar = columnSeparatorChar;
                this.lineSeparatorChar = lineSeparatorChar;
                this.fieldReader = fieldReader;
            }
        }

        /// <summary>
        /// Atribui e obtém o carácter que serve de separador das linhas do csv.
        /// </summary>
        /// <value>O carácter.</value>
        public char LineSeparatorChar
        {
            get
            {
                return this.lineSeparatorChar;
            }
            set
            {
                if (char.IsLetterOrDigit(value))
                {
                    throw new UtilitiesDataException("Line separator can't be a letter or digit.");
                }
                else
                {
                    this.lineSeparatorChar = value;
                }
            }
        }

        /// <summary>
        /// Atribui e obtém o carácter que serve de separador das colunas do csv.
        /// </summary>
        /// <value>O carácter.</value>
        public char ColumnSeparatorChar
        {
            get
            {
                return this.columnSeparatorChar;
            }
            set
            {
                if (char.IsLetterOrDigit(value))
                {
                    throw new UtilitiesDataException("Column separator can't be a letter or digit.");
                }
                else
                {
                    this.columnSeparatorChar = value;
                }
            }
        }

        /// <summary>
        /// Obtém o carácter que funciona como delimitador.
        /// </summary>
        public char Delimiter
        {
            get
            {
                return this.delimiter;
            }
            set
            {
                if (char.IsLetterOrDigit(value))
                {
                    throw new UtilitiesDataException("Delimiter separator can't be a letter or digit.");
                }
                else
                {
                    this.delimiter = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o leitor de campos.
        /// </summary>
        public Func<int, int, string, object> FieldReader
        {
            get
            {
                return this.fieldReader;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("A non null value must be provided.");
                }
                else
                {
                    this.fieldReader = value;
                }
            }
        }

        /// <summary>
        /// Realiza a leitura de um csv.
        /// </summary>
        /// <param name="tabularItem">O item tabular a ser lido.</param>
        /// <param name="stream">O leitor de informação.</param>
        public void Read(ITabularItem tabularItem, Stream stream)
        {
            this.Read(tabularItem, stream, Encoding.ASCII);
        }

        /// <summary>
        /// Realiza a leitura de um csv providenciando a codificação dos carácteres.
        /// </summary>
        /// <param name="tabularItem">O item tabular a ser lido.</param>
        /// <param name="stream">O leitor de informação.</param>
        /// <param name="encoding">A codificação.</param>
        /// <exception cref="ArgumentNullException">Se o leitor de informação for nulo.</exception>
        public void Read(ITabularItem tabularItem, Stream stream, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else
            {
                var innerEncoding = encoding;
                if (encoding == null)
                {
                    innerEncoding = Encoding.ASCII;
                }

                var textReader = new StreamReader(stream, innerEncoding);
                this.Read(tabularItem, textReader);
            }
        }

        /// <summary>
        /// Faz a leitura do csv.
        /// </summary>
        /// <param name="tabularItem">O item tabular.</param>
        /// <param name="reader">O leitor.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public void Read(ITabularItem tabularItem, TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else if (tabularItem == null)
            {
                throw new ArgumentNullException("tabularItem");
            }
            else
            {
                this.ReadCsv(tabularItem, reader);
            }
        }

        /// <summary>
        /// Escreve o item tabular no formato csv.
        /// </summary>
        /// <param name="stream">O controlo da cadeia de escrita do csv.</param>
        /// <param name="tabularItem">O item tabular.</param>
        public void Write(Stream stream, ITabularItem tabularItem)
        {
            this.Write(stream, tabularItem, Encoding.ASCII);
        }

        /// <summary>
        /// Escreve o item tabular no formato csv.
        /// </summary>
        /// <param name="stream">O controlo da cadeia de escrita do csv.</param>
        /// <param name="tabularItem">O item tabular.</param>
        /// <param name="encoding">A codificação de escrita.</param>
        /// <exception cref="ArgumentNullException">Se o leitor ou ou item forem nulos.</exception>
        public void Write(Stream stream, ITabularItem tabularItem, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            else if (tabularItem == null)
            {
                throw new ArgumentNullException("tabularItem");
            }
            else
            {
                var writer = new StreamWriter(stream, encoding);
                this.Write(writer, tabularItem);
            }
        }

        /// <summary>
        /// Escreve o csv para um contentor de texto.
        /// </summary>
        /// <param name="writer">O contentor.</param>
        /// <param name="tabularItem">O item tabular.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public void Write(TextWriter writer, ITabularItem tabularItem)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            else if (tabularItem == null)
            {
                throw new ArgumentNullException("tabularItem");
            }
            else
            {
                var lineEnumerator = tabularItem.GetEnumerator();
                if (lineEnumerator.MoveNext())
                {
                    var currentLine = lineEnumerator.Current;
                    var columnEnumerator = currentLine.GetEnumerator();
                    if (columnEnumerator.MoveNext())
                    {
                        writer.Write(columnEnumerator.Current.GetCellValue<object>());
                        while (columnEnumerator.MoveNext())
                        {
                            writer.Write(this.columnSeparatorChar);
                            writer.Write(columnEnumerator.Current.GetCellValue<object>());
                        }
                    }

                    while (lineEnumerator.MoveNext())
                    {
                        currentLine = lineEnumerator.Current;
                        writer.Write(this.lineSeparatorChar);
                        columnEnumerator = currentLine.GetEnumerator();
                        if (columnEnumerator.MoveNext())
                        {
                            writer.Write(columnEnumerator.Current.GetCellValue<object>());
                            while (columnEnumerator.MoveNext())
                            {
                                writer.Write(this.columnSeparatorChar);
                                writer.Write(columnEnumerator.Current.GetCellValue<object>());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Completa a leitura do csv tendo em conta os leitores individuais de dados.
        /// </summary>
        /// <param name="result">O resultado da leitura.</param>
        /// <param name="reader">O leitor de texto.</param>
        /// <exception cref="UtilitiesDataException">Se a leitura falhar.</exception>
        private void ReadCsv(
            ITabularItem result,
            TextReader reader)
        {
            var elements = new List<object>();
            var currentItem = string.Empty;
            var currentLine = 0;
            var currentColumn = 0;
            var readed = reader.Read();
            if (readed != -1)
            {
                var readedChar = (char)readed;
                if (readedChar == this.lineSeparatorChar)
                {
                    ++currentLine;
                    var element = this.fieldReader.Invoke(currentLine, currentColumn, currentItem);
                    elements.Add(element);
                    result.Add(elements);
                    currentItem = string.Empty;
                    elements.Clear();
                }
                else if (readedChar == this.columnSeparatorChar)
                {
                    var element = this.fieldReader.Invoke(currentLine, currentColumn, currentItem);
                    elements.Add(element);
                    ++currentColumn;
                    currentItem = string.Empty;
                }
                else if (readedChar == this.delimiter)
                {
                    currentItem += readedChar;
                    currentItem += this.GetBetweenDelimiters(reader);
                }
                else
                {
                    currentItem += readedChar;
                }

                readed = reader.Read();
                while (readed != -1)
                {
                    readedChar = (char)readed;
                    if (readedChar == this.lineSeparatorChar)
                    {
                        ++currentLine;
                        var element = this.fieldReader.Invoke(currentLine, currentColumn, currentItem);
                        elements.Add(element);
                        result.Add(elements);
                        currentItem = string.Empty;
                        elements.Clear();
                    }
                    else if (readedChar == this.columnSeparatorChar)
                    {
                        var element = this.fieldReader.Invoke(currentLine, currentColumn, currentItem);
                        elements.Add(element);
                        ++currentColumn;
                        currentItem = string.Empty;
                    }
                    else if (readedChar == this.delimiter)
                    {
                        currentItem += readedChar;
                        currentItem += this.GetBetweenDelimiters(reader);
                    }
                    else
                    {
                        currentItem += readedChar;
                    }

                    readed = reader.Read();
                }

                var outerElement = this.fieldReader.Invoke(currentLine, currentColumn, currentItem);
                elements.Add(outerElement);
                currentItem = string.Empty;
                result.Add(elements);
            }
        }

        /// <summary>
        /// Efectua a leitura dos valores que se encontram no interior dos delimitadores.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <returns>O valor lido entre os delimitadores.</returns>
        private string GetBetweenDelimiters(TextReader reader)
        {
            var result = string.Empty;
            var state = true;
            while (state)
            {
                var readed = reader.Read();
                if (readed == -1)
                {
                    state = false;
                }
                else
                {
                    result += (char)readed;
                    if (readed == this.delimiter)
                    {
                        state = false;
                    }
                }
            }

            return result;
        }
    }
}
