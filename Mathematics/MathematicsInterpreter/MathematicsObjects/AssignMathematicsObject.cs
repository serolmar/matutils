using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class AssignMathematicsObject : AMathematicsFunctionObject
    {
        private AMathematicsObject leftObject;

        private AMathematicsObject rightObject;

        private MathematicsInterpreterMediator mediator;

        public AssignMathematicsObject(AMathematicsObject leftObject, AMathematicsObject rightObject, MathematicsInterpreterMediator mediator)
            : base(EMathematicsType.ASSIGN)
        {
            if (leftObject == null)
            {
                throw new ExpressionInterpreterException("Left object must be non null within an assignement.");
            }
            else if (rightObject == null)
            {
                throw new ExpressionInterpreterException("Right object must be non null within an assignement.");
            }
            else
            {
                this.leftObject = leftObject;
                this.rightObject = rightObject;
                this.mediator = mediator;
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
            return this.mathematicsType.Equals(mathematicsType);
        }

        public override AMathematicsObject ConvertTo(EMathematicsType mathematicsType)
        {
            if (!mathematicsType.Equals(this.mathematicsType))
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Evaluates all assignements and function calls.
        /// </summary>
        /// <returns>The evaluated object.</returns>
        public override void Evaulate()
        {
            this.mediator.Assign(this.leftObject as NameMathematicsObject, this.rightObject);
        }

        /// <summary>
        /// Gets a string representation of an assignement.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            return string.Format("{0} = {1}", this.leftObject.ToString(), this.rightObject.ToString());
        }
    }
}
