using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class DoubleMathematicsObject : AMathematicsObject
    {
        private double value;

        public DoubleMathematicsObject()
            : base(EMathematicsType.DOUBLE_VALUE, false)
        {
        }

        public double Value
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
        /// Gets a string representation of double object.
        /// </summary>
        /// <returns>The double object.</returns>
        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
