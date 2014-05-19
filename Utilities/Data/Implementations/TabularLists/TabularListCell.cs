namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma célula de uma tabela.
    /// </summary>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TabularListCell"/>.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="columnNumber">O número da coluna.</param>
        /// <param name="parent">A linha à qual pertence a célula.</param>
        public TabularListCell(int rowNumber, int columnNumber, TabularListsItem parent)
        {
            this.rowNumber = rowNumber;
            this.columnNumber = columnNumber;
            this.parent = parent;
        }

        /// <summary>
        /// Obtém ou atribui a linha à qual pertence a célula.
        /// </summary>
        /// <value>A linha.</value>
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
        /// Obtém e atribui o número da coluna.
        /// </summary>
        /// <value>O número da coluna.</value>
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

        /// <summary>
        /// Obtém um valor que indica se a célula é nula ou vazia.
        /// </summary>
        /// <value>Verdadeiro caso a célula se encontre nula ou vazia e falso caso contrário.</value>
        public bool NullOrEmpty
        {
            get
            {
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

        /// <summary>
        /// Obtém o tipo de dados contidos na célula.
        /// </summary>
        /// <value>O tipo de dados.</value>
        public Type ValueType
        {
            get
            {
                var value = this.parent.GetCellValue(this.rowNumber, this.columnNumber);
                if (value == null)
                {
                    return typeof(object);
                }
                else
                {
                    return value.GetType();
                }
            }
        }

        /// <summary>
        /// Atribui o valor à célula.
        /// </summary>
        /// <typeparam name="T">O tipo de dados do valor.</typeparam>
        /// <param name="value">O valor.</param>
        public void SetCellValue<T>(T value)
        {
            this.parent.SetValue(this.rowNumber, this.columnNumber, value);
        }

        /// <summary>
        /// Obtém o valor da célula.
        /// </summary>
        /// <typeparam name="T">O tipo de dados do valor.</typeparam>
        /// <returns>O valor.</returns>
        /// <exception cref="UtilitiesDataException">Se o valor da célula não for convertível no tipo proporcionado.</exception>
        public T GetCellValue<T>()
        {
            var value = this.parent.GetCellValue(this.rowNumber, this.columnNumber);
            if (value == null)
            {
                var type = typeof(T);
                if (typeof(T).IsClass)
                {
                    return (T)value;
                }
                else if (Nullable.GetUnderlyingType(typeof(T)) != null)
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

        /// <summary>
        /// Obtém o conteúdo da célula como sendo texto.
        /// </summary>
        /// <returns>O conteúdo da célula.</returns>
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
