namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringDataReader : IDataReader<string>
    {
        public Type ObjectType
        {
            get
            {
                return typeof(string);
            }
        }

        public bool TryRead(string conversionObject, out object value)
        {
            value = conversionObject;
            return true;
        }
    }
}
