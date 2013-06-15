using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    public class ExpressionInterpreterException : Exception
    {
        public ExpressionInterpreterException() : base() { }

        public ExpressionInterpreterException(string message) : base(message) { }

        public ExpressionInterpreterException(string message, Exception innerException) : base(message, innerException) { }

        public ExpressionInterpreterException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }
    }
}
