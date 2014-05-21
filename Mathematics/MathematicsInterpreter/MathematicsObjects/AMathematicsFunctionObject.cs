namespace Mathematics.MathematicsInterpreter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma função.
    /// </summary>
    public abstract class AMathematicsFunctionObject : AMathematicsObject
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="AMathematicsFunctionObject"/>.
        /// </summary>
        /// <param name="mathematicsType">O tipo.</param>
        public AMathematicsFunctionObject(EMathematicsType mathematicsType)
            : base(mathematicsType, true)
        {
        }

        /// <summary>
        /// Avalia a função.
        /// </summary>
        public abstract void Evaulate();
    }
}
