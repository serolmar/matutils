using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    public class MathematicsInterpreterResult
    {
        private string interpreterResultMessage;

        private EMathematicsInterpreterStatus interpreterMessageStatus;

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
