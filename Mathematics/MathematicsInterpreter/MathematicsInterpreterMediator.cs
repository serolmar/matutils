using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.MathematicsInterpreter
{
    class MathematicsInterpreterMediator
    {
        private Dictionary<NameMathematicsObject, AMathematicsObject> assignements = new Dictionary<NameMathematicsObject, AMathematicsObject>();

        public bool IsAssigned(NameMathematicsObject name)
        {
            return this.assignements.ContainsKey(name);
        }

        public void Assign(NameMathematicsObject name, AMathematicsObject value)
        {
            if (this.IsAssignementRecursive(name, value))
            {
                throw new ExpressionInterpreterException("Recursive assignment.");
            }
            else
            {
                if (this.assignements.ContainsKey(name))
                {
                    this.assignements[name] = value;
                }
                else
                {
                    this.assignements.Add(name, value);
                }
            }
        }

        public void Unassign(NameMathematicsObject name)
        {
            this.assignements.Remove(name);
        }

        public bool TryGetValue(NameMathematicsObject name, out AMathematicsObject value)
        {
            return this.assignements.TryGetValue(name, out value);
        }

        /// <summary>
        /// Checks if some assignement is recursive.
        /// </summary>
        /// <param name="name">The assingment name.</param>
        /// <param name="value">The assignement value.</param>
        /// <returns>True if assingment is recursive and false otherwise.</returns>
        private bool IsAssignementRecursive(NameMathematicsObject name, AMathematicsObject value)
        {
            if (value.MathematicsType == EMathematicsType.INTEGER_VALUE ||
                value.MathematicsType == EMathematicsType.DOUBLE_VALUE ||
                value.MathematicsType == EMathematicsType.BOOLEAN_VALUE)
            {
                return false;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
