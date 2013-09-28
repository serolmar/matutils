using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class UtilitiesDataException : Exception
    {
        public UtilitiesDataException() : base() { }

        public UtilitiesDataException(string message) : base(message) { }

        public UtilitiesDataException(string message, Exception innerException) : base(message, innerException) { }

        public UtilitiesDataException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }

    }
}
