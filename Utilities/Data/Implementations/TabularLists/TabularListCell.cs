namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class TabularListCell : ITabularCell
    {
        /// <summary>
        /// O item tabular.
        /// </summary>
        protected TabularListsItem parent;

        /// <summary>
        /// O número da linha.
        /// </summary>
        private int rowNumber;

        /// <summary>
        /// O número da coluna.
        /// </summary>
        private int columnNumber;

        public TabularListCell(int rowNumber, int columnNumber, TabularListsItem parent)
        {
            this.rowNumber = rowNumber;
            this.columnNumber = columnNumber;
            this.parent = parent;
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

        public int ColumnNumber
        {
            get
            {
                return this.columnNumber;
            }
            set
            {
                this.columnNumber = value;
            }
        }

        public bool NullOrEmpty
        {
            get {
                var value = this.parent.GetCellValue(this.rowNumber, this.columnNumber);
                if (value == null)
                {
                    return true;
                }
                else if (typeof(string).IsAssignableFrom(value.GetType()))
                {
                    return string.IsNullOrEmpty(value.ToString());
                }
                else
                {
                    return false;
                }
            }
        }

        public Type ValueType
        {
            get {
                var value = this.parent.GetCellValue(this.rowNumber, this.columnNumber);
                if (value == null){
                    throw new UtilitiesDataException("Can't infer type from null value.");
                }
                else
                {
                    return value.GetType();
                }
            }
        }

        public void SetCellValue<T>(T value)
        {
            this.parent.SetValue(this.rowNumber, this.columnNumber, value);
        }

        public T GetCellValue<T>()
        {
            var value = this.parent.GetCellValue(this.rowNumber, this.columnNumber);
            if (value == null)
            {
                if (typeof(T).IsClass)
                {
                    return (T)value;
                }
                else
                {
                    throw new UtilitiesDataException("Can't convert cell's value to the specified type.");
                }
            }
            else if (typeof(T).IsAssignableFrom(value.GetType()))
            {
                return (T)value;
            }
            else
            {
                throw new UtilitiesDataException("Can't convert cell's value to the specified type.");
            }
        }

        public string GetAsText()
        {
            var value = this.parent.GetCellValue(this.rowNumber, this.columnNumber);
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
