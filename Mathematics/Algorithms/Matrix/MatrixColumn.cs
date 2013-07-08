using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    internal class MatrixColumn<ColumnType, T> : IMatrixColumn<ColumnType, T>
    {
        private ColumnType column;

        private T value;

        internal MatrixColumn(ColumnType column, T value)
        {
            this.column = column;
            this.value = value;
        }

        public ColumnType Column
        {
            get
            {
                return this.column;
            }
        }

        public T Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
