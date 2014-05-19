namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um fornecedor de leitor de tabelas a partir de CSV.
    /// </summary>
    /// <typeparam name="ReaderType">O tipo do leitor.</typeparam>
    public class DataReaderProvider<ReaderType> : IDataReaderProvider<ReaderType>
    {
        /// <summary>
        /// O leitor que se aplica à generalidade da tabela.
        /// </summary>
        private ReaderType mainDataReader;

        /// <summary>
        /// Os leitores associados às colunas da tabela.
        /// </summary>
        private Dictionary<int, ReaderType> columnDataReaders =
            new Dictionary<int, ReaderType>();

        /// <summary>
        /// Os leitores particularizados por cada célula específica da tabela.
        /// </summary>
        private Dictionary<Tuple<int, int>, ReaderType> cellDataReaders =
            new Dictionary<Tuple<int, int>, ReaderType>();

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DataReaderProvider{ReaderType}"/>.
        /// </summary>
        /// <param name="mainDataReader">O leitor.</param>
        public DataReaderProvider(ReaderType mainDataReader)
        {
            if (mainDataReader == null)
            {
                throw new ArgumentNullException("mainDataReader");
            }
            else
            {
                this.mainDataReader = mainDataReader;
            }
        }

        /// <summary>
        /// Obtém ou atribui o leitor.
        /// </summary>
        /// <value>O leitor.</value>
        /// <exception cref="UtilitiesDataException">Se o valor atribuído for nulo.</exception>
        public ReaderType MainDataReader
        {
            get
            {
                return this.mainDataReader;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("A data reader must be provided.");
                }
                else
                {
                    this.mainDataReader = value;
                }
            }
        }

        /// <summary>
        /// Regista um leitor por coluna.
        /// </summary>
        /// <param name="columnNumber">O número da coluna.</param>
        /// <param name="dataReader">O leitor.</param>
        /// <exception cref="IndexOutOfRangeException">Se o número da coluna for negativo.</exception>
        /// <exception cref="ArgumentNullException">Se o leitor for nulo.</exception>
        public void RegisterDataReader(int columnNumber, ReaderType dataReader)
        {
            if (columnNumber < 0)
            {
                throw new IndexOutOfRangeException("Column number can't be negative.");
            }
            else if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            else
            {
                if (this.columnDataReaders.ContainsKey(columnNumber))
                {
                    this.columnDataReaders[columnNumber] = dataReader;
                }
                else
                {
                    this.columnDataReaders.Add(columnNumber, dataReader);
                }
            }
        }

        /// <summary>
        /// Desprende o leirtor da coluna especificada.
        /// </summary>
        /// <param name="columnNumber">O número da coluna.</param>
        public void UnregisterDataReader(int columnNumber)
        {
            this.columnDataReaders.Remove(columnNumber);
        }

        /// <summary>
        /// Eliminda todos os leitores associados à tabela con excepção do leitor principal.
        /// </summary>
        public void ClearDataReaders()
        {
            this.columnDataReaders.Clear();
            this.cellDataReaders.Clear();
        }

        /// <summary>
        /// Regista um leitor e associa-o a uma célula específica.
        /// </summary>
        /// <param name="rowNumber">O número da linha que contém a célula.</param>
        /// <param name="columnNumber">O número da coluna que contém a célula.</param>
        /// <param name="dataReader">O leitor a ser associado.</param>
        /// <exception cref="IndexOutOfRangeException">Se o número da linha ou o número da coluna forem negativos.</exception>
        /// <exception cref="ArgumentNullException">Se o leitor for nulo.</exception>
        public void RegisterDataReader(
            int rowNumber,
            int columnNumber,
            ReaderType dataReader)
        {
            if (rowNumber < 0)
            {
                throw new IndexOutOfRangeException("Row number can't be negative.");
            }
            else if (columnNumber < 0)
            {
                throw new IndexOutOfRangeException("Column number can't be negative.");
            }
            else if (dataReader == null)
            {
                throw new ArgumentNullException("dataReader");
            }
            else
            {
                var coords = Tuple.Create(rowNumber, columnNumber);
                if (this.cellDataReaders.ContainsKey(coords))
                {
                    this.cellDataReaders[coords] = dataReader;
                }
                else
                {
                    this.cellDataReaders.Add(coords, dataReader);
                }
            }
        }

        /// <summary>
        /// Elimina o leitor associado à célula especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="columnNumber">O número da coluna.</param>
        public void UnregisterDataReader(int rowNumber, int columnNumber)
        {
            var coords = Tuple.Create(rowNumber, columnNumber);
            this.cellDataReaders.Remove(coords);
        }

        /// <summary>
        /// Tenta obter um leitor.
        /// </summary>
        /// <param name="rowNumber">O número da célula.</param>
        /// <param name="columnNumber">O número da coluna.</param>
        /// <param name="reader">O leitor.</param>
        /// <returns>Verdadeiro caso seja possível obter um leitor e falso caso contrário.</returns>
        public bool TryGetDataReader(
            int rowNumber,
            int columnNumber,
            out ReaderType reader)
        {
            if (rowNumber < 0)
            {
                reader = default(ReaderType);
                return false;
            }
            else if (columnNumber < 0)
            {
                reader = default(ReaderType);
                return false;
            }
            else
            {
                var coords = Tuple.Create(rowNumber, columnNumber);
                if (!this.cellDataReaders.TryGetValue(coords, out reader))
                {
                    if (!this.columnDataReaders.TryGetValue(columnNumber, out reader))
                    {
                        reader = this.mainDataReader;
                    }
                }

                return true;
            }
        }
    }
}
