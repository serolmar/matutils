namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BooleanCombination
    {
        /// <summary>
        /// O valor da entrada lógica.
        /// </summary>
        private BitArray logicInput;

        /// <summary>
        /// O valor da saída lógica.
        /// </summary>
        private EBooleanMinimalFormOutStatus logicOutput;

        internal BooleanCombination(BitArray logicInput, EBooleanMinimalFormOutStatus logicOutput)
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
        /// O valor da entrada lógica.
        /// </summary>
        public BitArray LogicInput
        {
            get
            {
                return this.logicInput;
            }
        }

        /// <summary>
        /// O valor da saída lógica.
        /// </summary>
        public EBooleanMinimalFormOutStatus LogicOutput
        {
            get
            {
                return this.logicOutput;
            }
        }
    }
}
