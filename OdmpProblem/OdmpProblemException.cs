using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdmpProblem
{
    /// <summary>
    /// Qualquer erro relacionado com o problema do ODMP.
    /// </summary>
    public class OdmpProblemException : Exception
    {
        public OdmpProblemException() : base() { }

        public OdmpProblemException(string message) : base(message) { }

        public OdmpProblemException(string message, Exception innerException) : base(message, innerException) { }

        public OdmpProblemException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }
    }
}
