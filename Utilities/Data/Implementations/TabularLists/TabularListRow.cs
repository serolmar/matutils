namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal abstract class AGeneralTabularListRow<L> : IGeneralTabularRow<L>
        where L : IGeneralTabularCell
    {
        /// <summary>
        /// O número da linha.
        /// </summary>
        protected int rowNumber;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="AGeneralTabularListRow{L}"/>.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        public AGeneralTabularListRow(int rowNumber)
        {
            this.rowNumber = rowNumber;
        }

        /// <summary>
        /// Obtém a célula especificada pelo índice.
        /// </summary>
        /// <value>A célula.</value>
        /// <param name="index">O índice.</param>
        /// <returns>A célula.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o índice for negativo.</exception>
        public L this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    return this.GetCell(this.rowNumber, index);
                }
            }
        }

        /// <summary>
        /// Obtém e atribui o número da linha.
        /// </summary>
        /// <value>O número da linha.</value>
        public int RowNumber
        {
            get
            {
                return this.rowNumber;
            }
            set
            {
                this.rowNumber = value;
            }
        }

        /// <summary>
        /// Obtém e atribui o número de células na linha.
        /// </summary>
        /// <value>O número de células.</value>
        public int Count
        {
            get
            {
                return this.GetRowCount(this.rowNumber);
            }
        }

        /// <summary>
        /// O número da última coluna.
        /// </summary>
        public int LastColumnNumber
        {
            get
            {
                return this.GetLastColumnNumber(this.rowNumber);
            }
        }

        /// <summary>
        /// Obtém um enumerador para a linha.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<L> GetEnumerator()
        {
            var parentRowCount = this.GetRowCount(this.rowNumber);
            for (int i = 0; i < parentRowCount; ++i)
            {
                yield return this.GetCell(this.rowNumber, i);
            }
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a linha.
        /// </summary>
        /// <returns>O enumerador não genérico.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Obtém o número de colunas para a linha especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número de colunas na linha.</returns>
        protected abstract int GetRowCount(int rowNumber);

        /// <summary>
        /// Obtém o número da última coluna na linha actual.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número da última coluna.</returns>
        protected abstract int GetLastColumnNumber(int rowNumber);

        /// <summary>
        /// Obtém a célula especificada pelo índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha que contém a célula.</param>
        /// <param name="columnNumber">O número da coluna que contém a célula.</param>
        /// <returns>A célula.</returns>
        protected abstract L GetCell(int rowNumber, int columnNumber);
    }

    /// <summary>
    /// Implementa uma tabela só de leitura.
    /// </summary>
    /// <typeparam name="P">O tipo de objectos que constituem a tabela que contém a linha.</typeparam>
    /// <typeparam name="R">O tipo de objectos que definem as linhas da tabela.</typeparam>
    /// <typeparam name="L">O tipo de objectos que definem as colunas da tabela.</typeparam>
    internal class ReadonlyTabularListRow<P, R, L>
        : AGeneralTabularListRow<IReadonlyTabularCell>, IReadonlyTabularRow
        where P : IGeneralTabularItem<R>
        where R : IGeneralTabularRow<L>
        where L : IGeneralTabularCell
    {
        /// <summary>
        /// A tabela que contém a linha.
        /// </summary>
        private P parent;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ReadonlyTabularListRow{P, R, L}"/>.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="parent">A tabela que contém a linha.</param>
        public ReadonlyTabularListRow(int rowNumber, P parent)
            : base(rowNumber)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Obtém o número de colunas para a linha especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número de colunas na linha.</returns>
        protected override int GetRowCount(int rowNumber)
        {
            return this.parent.ColumnsCount(rowNumber);
        }

        /// <summary>
        /// Obtém o número da última coluna na linha actual.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número da última coluna.</returns>
        protected override int GetLastColumnNumber(int rowNumber)
        {
            return this.parent.GetLastColumnNumber(rowNumber);
        }

        /// <summary>
        /// Obtém a célula especificada pelo índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha que contém a célula.</param>
        /// <param name="columnNumber">O número da coluna que contém a célula.</param>
        /// <returns>A célula.</returns>
        protected override IReadonlyTabularCell GetCell(int rowNumber, int columnNumber)
        {
            return new ReadonlyTabularListCell<P,R,L>(
                rowNumber, 
                columnNumber, 
                this.parent);
        }
    }
    /// <summary>
    /// Implementa uma linha da tabela.
    /// </summary>
    internal class TabularListRow
        :AGeneralTabularListRow<ITabularCell>, ITabularRow
    {
        /// <summary>
        /// A tabela que contém a linha.
        /// </summary>
        protected ITabularItem parent;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TabularListRow"/>.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="parent">A tabela que contém a linha.</param>
        public TabularListRow(int rowNumber, ITabularItem parent)
            : base(rowNumber)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Obtém ou atribui a tabela à qual pertence a célula.
        /// </summary>
        /// <value>A tablea.</value>
        public ITabularItem Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        /// <summary>
        /// Obtém o número de colunas para a linha especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número de colunas na linha.</returns>
        protected override int GetRowCount(int rowNumber)
        {
            return this.parent.ColumnsCount(rowNumber);
        }

        /// <summary>
        /// Obtém o número da última coluna na linha actual.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número da última coluna.</returns>
        protected override int GetLastColumnNumber(int rowNumber)
        {
            return this.parent.GetLastColumnNumber(rowNumber);
        }

        /// <summary>
        /// Obtém a célula especificada pelo índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha que contém a célula.</param>
        /// <param name="columnNumber">O número da coluna que contém a célula.</param>
        /// <returns>A célula.</returns>
        protected override ITabularCell GetCell(int rowNumber, int columnNumber)
        {
            return new TabularListCell(this.rowNumber, columnNumber, this.parent);
        }
    }
}
