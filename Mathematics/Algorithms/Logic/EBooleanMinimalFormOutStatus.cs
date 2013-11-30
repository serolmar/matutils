namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumera os estados de saída de uma subexpressão lógica.
    /// </summary>
    public enum EBooleanMinimalFormOutStatus
    {

        /// <summary>
        /// Estado que não é utilizado.
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// Estado ligado.
        /// </summary>
        ON = 1,

        /// <summary>
        /// Estado desligado.
        /// </summary>
        OFF = 2,

        /// <summary>
        /// Estado indiferente.
        /// </summary>
        DONT_CARE = 3
    }
}
