﻿namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using Parsers;

    public class CsvFileReaderWriter
    {
        /// <summary>
        /// Mantém uma referência para a cultura existente por defeito.
        /// </summary>
        private CultureInfo currentCulture = CultureInfo.CurrentCulture;

        /// <summary>
        /// O separador de colunas no csv.
        /// </summary>
        private char columnSeparatorType = ';';

        /// <summary>
        /// O separador de linhas no csv.
        /// </summary>
        private char lineSeparatorType = '\n';

        public CsvFileReaderWriter()
        {
        }

        public CsvFileReaderWriter(char lineSeparatorType, char columnSeparatorType)
        {
            if (char.IsLetterOrDigit(lineSeparatorType))
            {
                throw new UtilitiesDataException("Line separator can't be a letter or digit.");
            }
            else if (char.IsLetterOrDigit(columnSeparatorType))
            {
                throw new UtilitiesDataException("Column separator can't be a letter or digit.");
            }
            else
            {
                this.columnSeparatorType = columnSeparatorType;
                this.lineSeparatorType = lineSeparatorType;
            }
        }

        public CsvFileReaderWriter(
            char lineSeparatorType,
            char columnSeparatorType,
            CultureInfo cultureInfo)
            : this(lineSeparatorType, columnSeparatorType)
        {
            if (cultureInfo != null)
            {
                this.currentCulture = cultureInfo;
            }
        }

        /// <summary>
        /// Atribui e obtém o carácter que serve de separador das linhas do csv.
        /// </summary>
        public char LineSeparatorType
        {
            get
            {
                return this.lineSeparatorType;
            }
            set
            {
                if (char.IsLetterOrDigit(value))
                {
                    throw new UtilitiesDataException("Line separator can't be a letter or digit.");
                }
                else
                {
                    this.lineSeparatorType = value;
                }
            }
        }

        /// <summary>
        /// Atribui e obtém o carácter que serve de separador das colunas do csv.
        /// </summary>
        public char ColumnSeparatorType
        {
            get
            {
                return this.columnSeparatorType;
            }
            set
            {
                if (char.IsLetterOrDigit(value))
                {
                    throw new UtilitiesDataException("Column separator can't be a letter or digit.");
                }
                else
                {
                    this.columnSeparatorType = value;
                }
            }
        }

        public ITabularItem Read(Stream stream)
        {
            return this.Read(stream, Encoding.ASCII);
        }

        public ITabularItem Read(Stream stream, List<IDataValidation<int, object>> validations)
        {
            throw new NotImplementedException();
        }

        public ITabularItem Read(Stream stream, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentException("stream");
            }
            else
            {
                var innerEncoding = encoding;
                if (encoding == null)
                {
                    innerEncoding = Encoding.ASCII;
                }

                var textReader = new StreamReader(stream, innerEncoding);
                return this.Read(textReader);
            }
        }

        public ITabularItem Read(
            Stream stream,
            Encoding encoding,
            List<IDataValidation<int, object>> validations)
        {
            if (stream == null)
            {
                throw new ArgumentException("stream");
            }
            else
            {
                var innerEncoding = encoding;
                if (encoding == null)
                {
                    innerEncoding = Encoding.ASCII;
                }

                var textReader = new StreamReader(stream, innerEncoding);
                return this.Read(textReader, validations);
            }
        }

        /// <summary>
        /// Faz a leitura do csv.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O item tabular que representa o csv.</returns>
        public ITabularItem Read(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else
            {
                var result = new TabularListsItem();
                this.ReadCsv(reader, result);
                return result;
            }
        }

        /// <summary>
        /// Faz a leitura do csv.
        /// </summary>
        /// <param name="reader">O leitor.</param>
        /// <returns>O item tabular que representa o csv.</returns>
        public ITabularItem Read(TextReader reader, List<IDataValidation<int, object>> validations)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else
            {
                var result = new TabularListsItem();
                this.ReadCsv(reader, result);
                if (validations != null)
                {
                    foreach (var validation in validations)
                    {
                        result.AddValidation(validation);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Escreve o item tabular no formato csv.
        /// </summary>
        /// <param name="stream">O controlo da cadeia de escrita do csv.</param>
        /// <param name="tabularItem">O item tabular.</param>
        private void Write(Stream stream, ITabularItem tabularItem)
        {
            this.Write(stream, tabularItem, Encoding.ASCII);
        }

        /// <summary>
        /// Escreve o item tabular no formato csv.
        /// </summary>
        /// <param name="stream">O controlo da cadeia de escrita do csv.</param>
        /// <param name="tabularItem">O item tabular.</param>
        /// <param name="encoding">A codificação de escrita.</param>
        private void Write(Stream stream, ITabularItem tabularItem, Encoding encoding)
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
        private void Write(TextWriter writer, ITabularItem tabularItem)
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
                            writer.Write(this.columnSeparatorType);
                            writer.Write(columnEnumerator.Current.GetCellValue<object>());
                        }
                    }

                    while (lineEnumerator.MoveNext())
                    {
                        writer.Write(this.lineSeparatorType);
                        columnEnumerator = currentLine.GetEnumerator();
                        if (columnEnumerator.MoveNext())
                        {
                            writer.Write(columnEnumerator.Current.GetCellValue<object>());
                            while (columnEnumerator.MoveNext())
                            {
                                writer.Write(this.columnSeparatorType);
                                writer.Write(columnEnumerator.Current.GetCellValue<object>());
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Completa a leitura do csv.
        /// </summary>
        /// <param name="reader">O leitor de texto.</param>
        /// <param name="result">O contentor da leitura.</param>
        private void ReadCsv(TextReader reader, TabularListsItem result)
        {
            var elements = new List<object>();
            var currentItem = string.Empty;
            var readed = reader.Read();
            if (readed != -1)
            {
                var readedChar = (char)readed;
                if (readedChar == this.lineSeparatorType)
                {
                    elements.Add(this.GetValues(currentItem));
                    result.Add(elements);
                    currentItem = string.Empty;
                    elements.Clear();
                }
                else if (readedChar == this.columnSeparatorType)
                {
                    elements.Add(this.GetValues(currentItem));
                    currentItem = string.Empty;
                }
                else
                {
                    currentItem += readedChar;
                }

                readed = reader.Read();
                while (readed != -1)
                {
                    readedChar = (char)readed;
                    if (readedChar == this.lineSeparatorType)
                    {
                        elements.Add(this.GetValues(currentItem));
                        result.Add(elements);
                        currentItem = string.Empty;
                        elements.Clear();
                    }
                    else if (readedChar == this.columnSeparatorType)
                    {
                        elements.Add(this.GetValues(currentItem));
                        currentItem = string.Empty;
                    }
                    else
                    {
                        currentItem += readedChar;
                    }

                    readed = reader.Read();
                }

                elements.Add(this.GetValues(currentItem));
                result.Add(elements);
            }
        }

        /// <summary>
        /// Tenta ler valores a partir do texto.
        /// </summary>
        /// <param name="text">O texto.</param>
        /// <returns>Os valores.</returns>
        private object GetValues(string text)
        {
            var integerValue = 0;
            if (int.TryParse(text, out integerValue))
            {
                return integerValue;
            }
            else
            {
                var longValue = 0L;
                if (long.TryParse(text, out longValue))
                {
                    return longValue;
                }
                else
                {
                    var bigInteger = default(BigInteger);
                    if (BigInteger.TryParse(text, out bigInteger))
                    {
                        return bigInteger;
                    }
                    else
                    {
                        var doubleValue = 0.0;
                        if (double.TryParse(text, out doubleValue))
                        {
                            return doubleValue;
                        }
                        else
                        {
                            var decimalValue = 0.0M;
                            if (decimal.TryParse(text, out decimalValue))
                            {
                                return decimalValue;
                            }
                            else
                            {
                                var dateTime = default(DateTime);
                                if (DateTime.TryParse(
                                    text,
                                    currentCulture.DateTimeFormat,
                                    DateTimeStyles.AllowWhiteSpaces,
                                    out dateTime))
                                {
                                    return dateTime;
                                }
                                else
                                {
                                    return text;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
