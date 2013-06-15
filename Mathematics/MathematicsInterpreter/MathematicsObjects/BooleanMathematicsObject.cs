using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class BooleanMathematicsObject : AMathematicsConditionObject
    {
        private bool value;

        public BooleanMathematicsObject()
            : base(EMathematicsConditionType.BOOLEAN_VALUE, EMathematicsType.BOOLEAN_VALUE, false)
        {
        }

        public bool Value
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
            if (mathematicsType == EMathematicsType.BOOLEAN_VALUE || mathematicsType == EMathematicsType.CONDITION)
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
            if (mathematicsType == EMathematicsType.BOOLEAN_VALUE || mathematicsType == EMathematicsType.CONDITION)
            {
                return this;
            }
            else
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
        }

        public override bool AssertCondition()
        {
            return this.value;
        }

        /// <summary>
        /// Gets a string representation of boolean object.
        /// </summary>
        /// <returns>The boolean object representation.</returns>
        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
