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
        /// Estado ligado.
        /// </summary>
        ON,

        /// <summary>
        /// Estado desligado.
        /// </summary>
        OFF,

        /// <summary>
        /// Estado indiferente.
        /// </summary>
        DONT_CARE
    }
}
