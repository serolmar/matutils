using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class MathematicsException : Exception
    {
        public MathematicsException() : base() { }

        public MathematicsException(string message) : base(message) { }

        public MathematicsException(string message, Exception innerException) : base(message, innerException) { }

        public MathematicsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }
    }
}
