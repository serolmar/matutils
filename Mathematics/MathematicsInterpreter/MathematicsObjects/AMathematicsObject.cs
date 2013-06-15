using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    public abstract class AMathematicsObject
    {
        protected EMathematicsType mathematicsType;

        protected bool isFunctionObject;

        public AMathematicsObject(EMathematicsType mathematicsType, bool isFunctionObject)
        {
            this.mathematicsType = mathematicsType;
            this.isFunctionObject = isFunctionObject;
        }

        public virtual EMathematicsType MathematicsType
        {
            get
            {
                return this.mathematicsType;
            }
        }

        public virtual bool IsFunctionObject
        {
            get
            {
                return this.isFunctionObject;
            }
        }

        public abstract bool CanConvertTo(EMathematicsType mathematicsType);

        public abstract AMathematicsObject ConvertTo(EMathematicsType mathematicsType);
    }
}
