namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define operação que permite determinar a norma de um obecto.
    /// </summary>
    /// <typeparam name="CoeffValueType">O tipo de valor que representa o resultado da norma.</typeparam>
    /// <typeparam name="CoeffNormType">
    /// O tipo de valor que representa o objecto do qual se pretende obter a norma.
    /// </typeparam>
    public interface INormSpace<CoeffValueType, CoeffNormType> 
        : IComparer<CoeffNormType>
    {
        /// <summary>
        /// Obtém a norma do valor especificado.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <returns>A norma.</returns>
        CoeffNormType GetNorm(CoeffValueType value);
    }
}
