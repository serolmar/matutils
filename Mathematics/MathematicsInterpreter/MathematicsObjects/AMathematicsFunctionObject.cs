using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    public abstract class AMathematicsFunctionObject : AMathematicsObject
    {
        public AMathematicsFunctionObject(EMathematicsType mathematicsType)
            : base(mathematicsType, true)
        {
        }

        public abstract void Evaulate();
    }
}
