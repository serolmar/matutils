namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma condição.
    /// </summary>
    public abstract class AMathematicsConditionObject : AMathematicsObject
    {
        /// <summary>
        /// O tipo de condição.
        /// </summary>
        protected EMathematicsConditionType mathematicsConditionType;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="AMathematicsConditionObject"/>.
        /// </summary>
        /// <param name="conditionType">O tipo de condição.</param>
        /// <param name="mathematicsType">O tipo do objecto.</param>
        /// <param name="isFunctionObject">Um valor que indica se se trata de uma função.</param>
        public AMathematicsConditionObject(
            EMathematicsConditionType conditionType, 
            EMathematicsType mathematicsType, bool isFunctionObject)
            : base(mathematicsType, false)
        {
        }

        /// <summary>
        /// Avalia a condição.
        /// </summary>
        /// <returns>O resultado da avaliação.</returns>
        public abstract bool AssertCondition();
    }
}
