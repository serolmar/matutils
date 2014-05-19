namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma linha da tabela.
    /// </summary>
    internal class TabularListRow : ITabularRow
    {
        /// <summary>
        /// A tabela que contém a linha.
        /// </summary>
        protected TabularListsItem parent;

        /// <summary>
        /// O número da linha.
        /// </summary>
        protected int rowNumber;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TabularListRow"/>.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="parent">A tabela que contém a linha.</param>
        public TabularListRow(int rowNumber, TabularListsItem parent)
        {
            this.parent = parent;
            this.rowNumber = rowNumber;
        }

        /// <summary>
        /// Obtém a célula especificada pelo índice.
        /// </summary>
        /// <value>A célula.</value>
        /// <param name="index">O índice.</param>
        /// <returns>A célula.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Se o índice for negativo.</exception>
        public ITabularCell this[int index]
        {
            get {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    return new TabularListCell(this.rowNumber, index, this.parent);
                }
            }
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
                return this.parent.GetRowCount(this.rowNumber);
            }
        }

        /// <summary>
        /// Obtém um enumerador para a linha.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<ITabularCell> GetEnumerator()
        {
            var parentRowCount = this.parent.GetRowCount(this.rowNumber);
            for (int i = 0; i < parentRowCount; ++i)
            {
                yield return new TabularListCell(this.rowNumber, i, this.parent);
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
    }
}
