using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class EmptyMathematicsObject : AMathematicsObject
    {
        public EmptyMathematicsObject()
            : base(EMathematicsType.EMPTY, false)
        {
        }

        public override bool CanConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == this.mathematicsType)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == this.mathematicsType)
            {
                return this;
            }
            else
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
        }

        public override string ToString()
        {
            return "Empty expression";
        }
    }
}
