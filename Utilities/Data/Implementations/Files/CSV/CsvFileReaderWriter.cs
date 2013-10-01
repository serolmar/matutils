namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class CsvFileReaderWriter
    {
        /// <summary>
        /// O separador de colunas no csv.
        /// </summary>
        private string columnSeparatorType;

        /// <summary>
        /// O separador de linhas no csv.
        /// </summary>
        private string lineSeparatorType;

        /// <summary>
        /// A lista com os delimitadores.
        /// </summary>
        private Dictionary<string, List<string>> delimiters = new Dictionary<string, List<string>>();

        public CsvFileReaderWriter()
        {
            this.columnSeparatorType = "semi_colon";
            this.lineSeparatorType = "new_line";
        }

        public CsvFileReaderWriter(string separatorType, string lineSeparatorType)
        {
            this.columnSeparatorType = separatorType;
            this.lineSeparatorType = lineSeparatorType;
        }

        /// <summary>
        /// Atribui e obtém o carácter que serve de separador do csv.
        /// </summary>
        public string SeparatorType
        {
            get
            {
                return this.columnSeparatorType;
            }
            set
            {
                this.columnSeparatorType = value;
            }
        }

        public void MapDelimiters(string openDelimiter, string closeDelimiter)
        {
            if (string.IsNullOrWhiteSpace(openDelimiter))
            {
                throw new ArgumentException("An open delimiter must be provided.");
            }
            else if (string.IsNullOrWhiteSpace(closeDelimiter))
            {
                throw new ArgumentException("A close delimiter must be provided.");
            }
            else
            {
                var closedDelimiteres = default(List<string>);
                if (this.delimiters.TryGetValue(openDelimiter, out closedDelimiteres))
                {
                    if (!closedDelimiteres.Contains(closeDelimiter))
                    {
                        closedDelimiteres.Add(closeDelimiter);
                    }
                }
                else
                {
                    this.delimiters.Add(openDelimiter, new List<string>() { closeDelimiter });
                }
            }
        }

        public void UnmapDelimiters(string openDelimiter, string closeDelimiter)
        {
            if (!string.IsNullOrWhiteSpace(openDelimiter) &&
                !string.IsNullOrWhiteSpace(closeDelimiter))
            {
                var closedDelimiteres = default(List<string>);
                if (this.delimiters.TryGetValue(openDelimiter, out closedDelimiteres))
                {
                    closedDelimiteres.Remove(closeDelimiter);
                    if (closedDelimiteres.Count == 0)
                    {
                        this.delimiters.Remove(openDelimiter);
                    }
                }
            }
        }

        public void ClearDelimiters()
        {
            this.delimiters.Clear();
        }

        /// <summary>
        /// Lê o csv dado o caminho do ficheiro.
        /// </summary>
        /// <param name="fileName">O caminho do ficheiro.</param>
        /// <returns>O item tabular.</returns>
        public ITabularItem Read(string fileName)
        {
            return this.Read(fileName, Encoding.ASCII);
        }

        public ITabularItem Read(string fileName, Encoding encoding)
        {
            var innerEncoding = encoding;
            if (encoding == null)
            {
                innerEncoding = Encoding.ASCII;
            }

            var fileInfo = new FileInfo(fileName);
            return this.Read(fileInfo, innerEncoding);
        }

        public ITabularItem Read(FileInfo fileInfo)
        {
            return this.Read(fileInfo, Encoding.ASCII);
        }

        public ITabularItem Read(FileInfo fileInfo, Encoding encoding)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException("fileInfo");
            }
            else
            {
                var innerEncoding = encoding;
                if (encoding == null)
                {
                    innerEncoding = Encoding.ASCII;
                }

                var stream = fileInfo.OpenRead();
                return this.Read(stream, innerEncoding);
            }
        }

        public ITabularItem Read(Stream stream)
        {
            return this.Read(stream, Encoding.ASCII);
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

        public ITabularItem Read(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
