namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um conjunto de combinações lógicas.
    /// </summary>
    /// <remarks>
    /// O conjunto de combinações lógicas permite representar uma expressão lógica na forma normal.
    /// </remarks>
    public class BooleanCombination
    {
        /// <summary>
        /// O valor da entrada lógica.
        /// </summary>
        private LogicCombinationBitArray logicInput;

        /// <summary>
        /// O valor da saída lógica.
        /// </summary>
        private EBooleanMinimalFormOutStatus logicOutput;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="BooleanCombination"/>.
        /// </summary>
        /// <param name="logicInput">A combinação lógica de entrada.</param>
        /// <param name="logicOutput">The logic output.</param>
        /// <exception cref="ArgumentNullException">logicInput</exception>
        internal BooleanCombination(LogicCombinationBitArray logicInput, EBooleanMinimalFormOutStatus logicOutput)
        {
            if (logicInput == null)
            {
                throw new ArgumentNullException("logicInput");
            }
            else
            {
                this.logicInput = logicInput;
                this.logicOutput = logicOutput;
            }
        }

        /// <summary>
        /// Obtém o valor da entrada lógica.
        /// </summary>
        /// <value>O valor da entrada lógica.</value>
        public LogicCombinationBitArray LogicInput
        {
            get
            {
                return this.logicInput;
            }
        }

        /// <summary>
        /// Obtém o valor da saída lógica.
        /// </summary>
        /// <value>
        /// O valor da saída lógica.
        /// </value>
        public EBooleanMinimalFormOutStatus LogicOutput
        {
            get
            {
                return this.logicOutput;
            }
        }
    }
}
