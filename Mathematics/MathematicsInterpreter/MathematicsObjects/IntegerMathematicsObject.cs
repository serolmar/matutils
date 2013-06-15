using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class IntegerMathematicsObject : AMathematicsObject
    {
        private int value;

        public IntegerMathematicsObject()
            : base(EMathematicsType.INTEGER_VALUE, false)
        {
        }

        public int Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public override bool CanConvertTo(EMathematicsType mathematicsType)
        {
            throw new NotImplementedException();
        }

        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a string representation of integer object.
        /// </summary>
        /// <returns>The integer object.</returns>
        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
