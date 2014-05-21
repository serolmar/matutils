namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumera todos os estados do interpretador.
    /// </summary>
    public enum EMathematicsInterpreterStatus
    {
        /// <summary>
        /// If interpreter has a completed expression.
        /// </summary>
        COMPLETED,

        /// <summary>
        /// If interpreter detected an error.
        /// </summary>
        ERROR,

        /// <summary>
        /// If interpreter is waiting for user input.
        /// </summary>
        WAITING
    }
}
