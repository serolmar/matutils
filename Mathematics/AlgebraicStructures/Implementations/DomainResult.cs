using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class DomainResult<ObjectType>
    {
        private ObjectType quotient;

        private ObjectType remainder;

        public DomainResult(ObjectType quotient, ObjectType remainder)
        {
            this.quotient = quotient;
            this.remainder = remainder;
        }

        public ObjectType Quotient
        {
            get
            {
                return this.quotient;
            }
        }

        public ObjectType Remainder
        {
            get
            {
                return this.remainder;
            }
        }
    }
}
