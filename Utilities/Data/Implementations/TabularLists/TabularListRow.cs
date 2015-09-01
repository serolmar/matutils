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
    internal class ReadonlyTabularListRow 
        : AGeneralTabularListRow<IReadonlyTabularCell>, IReadonlyTabularRow
    {
        /// <summary>
        /// A tabela que contém a linha.
        /// </summary>
        private ReadonlyTabularListItem parent;
        
        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ReadonlyTabularListRow"/>.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="parent">A tabela que contém a linha.</param>
        public ReadonlyTabularListRow(int rowNumber, ReadonlyTabularListItem parent)
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
            return this.parent.GetRowCount(rowNumber);
        }

        /// <summary>
        /// Obtém a célula especificada pelo índice.
        /// </summary>
        /// <param name="rowNumber">O número da linha que contém a célula.</param>
        /// <param name="columnNumber">O número da coluna que contém a célula.</param>
        /// <returns>A célula.</returns>
        protected override IReadonlyTabularCell GetCell(int rowNumber, int columnNumber)
        {
            return new ReadonlyTabularListCell(rowNumber, columnNumber);
        }
    }

    /// <summary>
    /// Implementa uma linha da tabela.
    /// </summary>
    internal class TabularListRow : AGeneralTabularListRow<ITabularCell>, ITabularRow
    {
        /// <summary>
        /// A tabela que contém a linha.
        /// </summary>
        protected TabularListsItem parent;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TabularListRow"/>.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="parent">A tabela que contém a linha.</param>
        public TabularListRow(int rowNumber, TabularListsItem parent)
            : base(rowNumber)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Obtém ou atribui a tabela à qual pertence a célula.
        /// </summary>
        /// <value>A tablea.</value>
        public TabularListsItem Parent
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
            return this.parent.GetRowCount(rowNumber);
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
