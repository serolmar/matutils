namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class TabularListRow : ITabularRow
    {
        protected TabularListsItem parent;

        protected int rowNumber;

        public TabularListRow(int rowNumber, TabularListsItem parent)
        {
            this.parent = parent;
            this.rowNumber = rowNumber;
        }

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

        public int Count
        {
            get
            {
                return this.parent.GetRowCount(this.rowNumber);
            }
        }

        public IEnumerator<ITabularCell> GetEnumerator()
        {
            var parentRowCount = this.parent.GetRowCount(this.rowNumber);
            for (int i = 0; i < parentRowCount; ++i)
            {
                yield return new TabularListCell(this.rowNumber, i, this.parent);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
