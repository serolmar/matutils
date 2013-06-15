using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class NameMathematicsObject : AMathematicsObject
    {
        private MathematicsInterpreterMediator mediator;

        private string name;

        public NameMathematicsObject(string name, MathematicsInterpreterMediator mediator) : base(EMathematicsType.NAME, false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ExpressionInterpreterException("Empty names aren't allowed.");
            }

            this.name = name;
            this.mediator = mediator;
        }

        public override EMathematicsType MathematicsType
        {
            get
            {
                return this.mathematicsType;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public AMathematicsObject Value
        {
            get
            {
                AMathematicsObject value = null;
                if (this.mediator.TryGetValue(this, out value))
                {
                    return value;
                }
                else
                {
                    return this;
                }
            }
        }

        public override bool CanConvertTo(EMathematicsType mathematicsType)
        {
            if (mathematicsType == EMathematicsType.NAME || mathematicsType == EMathematicsType.POLYNOMIAL)
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
            if (mathematicsType == EMathematicsType.NAME)
            {
                return this;
            }
            else if (mathematicsType == EMathematicsType.POLYNOMIAL)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new ExpressionInterpreterException(string.Format("Can't convert from type {0} to type {1}.", this.mathematicsType, mathematicsType));
            }
        }

        /// <summary>
        /// Gets a string representation of name object.
        /// </summary>
        /// <returns>The name object.</returns>
        public override string ToString()
        {
            return this.name;
        }
    }
}
