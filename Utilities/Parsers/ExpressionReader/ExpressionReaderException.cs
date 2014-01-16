namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ExpressionReaderException : Exception
    {
        public ExpressionReaderException() : base() { }

        public ExpressionReaderException(string message) : base(message) { }

        public ExpressionReaderException(string message, Exception innerException) : base(message, innerException) { }

        public ExpressionReaderException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }

    }
}
