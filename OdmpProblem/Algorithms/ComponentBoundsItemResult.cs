namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ComponentBoundsItemResult
    {
        /// <summary>
        /// O menor valor de medianas escolhidas.
        /// </summary>
        private int lowerBound;

        /// <summary>
        /// O maior valor de medianas escolhidas.
        /// </summary>
        private int upperBound;

        public ComponentBoundsItemResult(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
            {
                throw new ArgumentOutOfRangeException("lowerBound");
            }
            else if(upperBound < 0){
                throw new ArgumentOutOfRangeException("upperBound");
            }
            else if (upperBound < lowerBound)
            {
                throw new ArgumentException("Lower bound can't be grater than upper bound.");
            }
            else
            {
                this.upperBound = upperBound;
                this.lowerBound = lowerBound;
            }
        }

        /// <summary>
        /// Obtém e atribui o menor valor de medianas escolhidas.
        /// </summary>
        public int LowerBound
        {
            get
            {
                return this.lowerBound;
            }
            set
            {
                if (value < 0)
                {
                    throw new OdmpProblemException("Lower bound can't be negative.");
                }
                else if (value > this.upperBound)
                {
                    throw new OdmpProblemException("Lower bound can't be grater than upper bound.");
                }
                else
                {
                    this.lowerBound = value;
                }
            }
        }

        /// <summary>
        /// Obtém e atribui o maior valor de medianas escolhidas.
        /// </summary>
        public int UpperBound
        {
            get
            {
                return this.upperBound;
            }
            set
            {
                if (value < 0)
                {
                    throw new OdmpProblemException("Upper bound can't be negative.");
                }
                else if (value < this.lowerBound)
                {
                    throw new OdmpProblemException("Lower bound can't be grater than upper bound.");
                }
                else
                {
                    this.upperBound = value;
                }
            }
        }
    }
}
