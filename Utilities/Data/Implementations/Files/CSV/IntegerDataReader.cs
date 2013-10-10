namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class IntegerDataReader : IDataReader<string>
    {
        public Type ObjectType
        {
            get
            {
                return typeof(int);
            }
        }

        public bool TryRead(string text, out object value)
        {
            var innerValue = 0;
            if (int.TryParse(text, out innerValue))
            {
                value = innerValue;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
