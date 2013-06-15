using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class BooleanConditionMathematicsObject : AMathematicsConditionObject
    {
        private AMathematicsObject leftObject;

        private AMathematicsObject rightObject;

        public BooleanConditionMathematicsObject(EMathematicsConditionType conditionType)
            : base(conditionType, EMathematicsType.CONDITION, false)
        {
            if (conditionType == EMathematicsConditionType.BOOLEAN_VALUE)
            {
                throw new ExpressionInterpreterException("Condition must be binary.");
            }
        }

        public AMathematicsObject LeftObject
        {
            get
            {
                return this.leftObject;
            }
        }

        public AMathematicsObject RightObject
        {
            get
            {
                return this.rightObject;
            }
        }

        public override bool CanConvertTo(EMathematicsType mathematicsType)
        {
            if (this.mathematicsType.Equals(mathematicsType))
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
            if (!this.mathematicsType.Equals(mathematicsType))
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
            else
            {
                return this;
            }
        }

        public override bool AssertCondition()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a string representation of boolean condition object.
        /// </summary>
        /// <returns>The boolean condition object representation.</returns>
        public override string ToString()
        {
            var conditionSymbol = string.Empty;
            if (this.mathematicsConditionType == EMathematicsConditionType.EQUAL)
            {
                conditionSymbol = "==";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.GREAT_OR_EQUAL_THAN)
            {
                conditionSymbol = ">=";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.GREAT_THAN)
            {
                conditionSymbol = ">";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.LESS_OR_EQUAL_THAN)
            {
                conditionSymbol = "<=";
            }
            else if (this.mathematicsConditionType == EMathematicsConditionType.LESS_THAN)
            {
                conditionSymbol = "<";
            }
            else
            {
                return "Invalid condition";
            }

            return string.Format("{0} {1} {2}", this.leftObject.ToString(), conditionSymbol, this.rightObject.ToString());
        }
    }
}
