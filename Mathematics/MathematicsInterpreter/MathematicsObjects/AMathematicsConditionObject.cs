using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    public abstract class AMathematicsConditionObject : AMathematicsObject
    {
        protected EMathematicsConditionType mathematicsConditionType;

        public AMathematicsConditionObject(EMathematicsConditionType conditionType, EMathematicsType mathematicsType, bool isFunctionObject)
            : base(mathematicsType, false)
        {
        }

        public abstract bool AssertCondition();
    }
}
