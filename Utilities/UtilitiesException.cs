namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UtilitiesException : Exception
    {
        public UtilitiesException() : base() { }

        public UtilitiesException(string message) : base(message) { }

        public UtilitiesException(string message, Exception innerException) : base(message, innerException) { }

        public UtilitiesException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }
    }
}
