namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DoubleToIntegerConversion : IConversion<int, double>
    {
        /// <summary>
        /// A precisão que se pretende considerar.
        /// </summary>
        private double precision;

        /// <summary>
        /// Permite instanciar um conversor de ponto flutuante para inteiro.
        /// </summary>
        /// <remarks>
        /// Um número será considerado inteiro caso o seu valor diferir de um valor inteiro em um valor inferior
        /// à precisão estabelecida.
        /// </remarks>
        /// <param name="precision">
        /// A precisão a ter em conta na comparação de valores. Será considerado o módulo do valor fornecido.
        /// </param>
        public DoubleToIntegerConversion(double precision = 0.0)
        {
            this.precision = Math.Abs(precision);
        }

        public bool CanApplyDirectConversion(double objectToConvert)
        {
            var value = Math.Round(objectToConvert);
            if (this.precision == 0 && value == objectToConvert)
            {
                return true;
            }
            else if (value < objectToConvert + this.precision && value > objectToConvert - this.precision)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        public int DirectConversion(double objectToConvert)
        {
            var value = Math.Round(objectToConvert);
            if (this.precision == 0 && value == objectToConvert)
            {
                return (int)value;
            }
            else if (value < objectToConvert + this.precision && value > objectToConvert - this.precision)
            {
                
                return (int)value;
            }
            else
            {
                throw new MathematicsException(string.Format("Can't convert value {0} to integer.", objectToConvert));
            }
        }

        public double InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }
}
