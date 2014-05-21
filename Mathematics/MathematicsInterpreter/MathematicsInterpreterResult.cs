namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa o resultado de uma interpretação.
    /// </summary>
    public class MathematicsInterpreterResult
    {
        /// <summary>
        /// A mensagem.
        /// </summary>
        private string interpreterResultMessage;

        /// <summary>
        /// O estado do resultado da interpretação.
        /// </summary>
        private EMathematicsInterpreterStatus interpreterMessageStatus;

        /// <summary>
        /// Obtém a mensagem.
        /// </summary>
        /// <value>A mensagem.</value>
        public string InterpreterResultMessage
        {
            get
            {
                return this.interpreterResultMessage;
            }
            internal set
            {
                this.interpreterResultMessage = value;
            }
        }

        /// <summary>
        /// Obtém o estado do resultado.
        /// </summary>
        /// <value>O estado do resultado.</value>
        public EMathematicsInterpreterStatus InterpreterMessageStatus
        {
            get
            {
                return this.interpreterMessageStatus;
            }
            internal set
            {
                this.interpreterMessageStatus = value;
            }
        }
    }
}
