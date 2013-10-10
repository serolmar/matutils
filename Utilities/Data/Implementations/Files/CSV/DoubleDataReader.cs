using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    class DoubleDataReader : IDataReader<string>
    {
        public Type ObjectType
        {
            get
            {
                return typeof(double);
            }
        }

        public bool TryRead(string text, out object value)
        {
            var innerValue = 0.0;
            if (double.TryParse(text, out innerValue))
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
