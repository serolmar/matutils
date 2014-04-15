namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class OdmpLabelsMatrixColumn : IOdmpMatrixColumn<int, double>
    {
        private int column;

        private double value;

        public OdmpLabelsMatrixColumn(int column, double value)
        {
            this.column = column;
            this.value = value;
        }

        /// <summary>
        /// Obtém o número da coluna actual.
        /// </summary>
        public int Column
        {
            get
            {
                return this.column;
            }
        }

        /// <summary>
        /// Obtém o valor contido na coluna.
        /// </summary>
        public double Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
