using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class CollectionsException : UtilitiesException
    {
        public CollectionsException() : base() { }

        public CollectionsException(string message) : base(message) { }

        public CollectionsException(string message, Exception innerException) : base(message, innerException) { }

        public CollectionsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }
    }
}
