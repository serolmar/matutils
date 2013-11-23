namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
