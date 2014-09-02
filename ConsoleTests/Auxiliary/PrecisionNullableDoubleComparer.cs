namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implmenta um comparador de números de vírgula flutuante tendo em conta a precisão.
    /// </summary>
    /// <remarks>
    /// A precisão consiste no erro segundo o qual dois números são iguais.
    /// </remarks>
    public class PrecisionNullableDoubleComparer : Comparer<Nullable<double>>
    {
        private double precision;

        /// <summary>
        /// Instancia uma nova instância de um objecto do tipo <see cref="PrecisionNullableDoubleComparer"/>.
        /// </summary>
        /// <param name="precision">A precisão.</param>
        public PrecisionNullableDoubleComparer(double precision)
        {
            if (precision < 0.0)
            {
                throw new ArgumentException("The precision number must be non-negative.");
            }
            else
            {
                this.precision = precision;
            }
        }

        /// <summary>
        /// Determina se um número é maior, menor ou igual a outro.
        /// </summary>
        /// <param name="x">O primeiro número a ser comparado.</param>
        /// <param name="y">O segundo número a ser comparado.</param>
        /// <returns>
        /// O valor -1 caso o primeiro número seja inferior ao segundo, 0 caso sejam iguais e -1 caso o primeiro
        /// seja superior ao segundo.
        /// </returns>
        public override int Compare(Nullable<double> x, Nullable<double> y)
        {
            if (x.HasValue)
            {
                if (y.HasValue)
                {
                    var xValue = x.Value;
                    var yValue = y.Value;
                    if (xValue == yValue)
                    {
                        return 0;
                    }
                    else
                    {
                        var difference = Math.Abs(xValue - yValue);
                        if (difference < this.precision)
                        {
                            return 0;
                        }
                        else
                        {
                            return Comparer<double>.Default.Compare(xValue, yValue);
                        }
                    }
                }
                else
                {
                    return 1;
                }
            }
            else if (y.HasValue)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
