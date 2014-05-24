namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite converter de um número de precisão dupla para um inteiro 
    /// <see cref="int"/>.
    /// </summary>
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

        /// <summary>
        /// Indica se é possível converter o número de precisão dupla para inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número em análise.</param>
        /// <returns>Verdadeiro caso a conversão seja possível e falso caso contrário.</returns>
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

        /// <summary>
        /// Indica se é possível converter um número inteiro para um número de precisão dupla.
        /// </summary>
        /// <remarks>
        /// Esta conversão é sempre possível.
        /// </remarks>
        /// <param name="objectToConvert">O número inteiro.</param>
        /// <returns>Verdadeiro.</returns>
        public bool CanApplyInverseConversion(int objectToConvert)
        {
            return true;
        }

        /// <summary>
        /// Obtém o resultado da conversão de um número de precisão dupla num inteiro.
        /// </summary>
        /// <param name="objectToConvert">O número de precisão dupla.</param>
        /// <returns>O inteiro convertido.</returns>
        /// <exception cref="MathematicsException">Caso o número de precisão dupla não represente um valor inteiro.</exception>
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

        /// <summary>
        /// Converte um número inteiro num número de precisão dupla.
        /// </summary>
        /// <param name="objectToConvert">O número inteiro a ser convertido.</param>
        /// <returns>O número de precisão dupla.</returns>
        public double InverseConversion(int objectToConvert)
        {
            return objectToConvert;
        }
    }
}
