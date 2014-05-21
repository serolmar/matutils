namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IActionSpecification<in ObjectType, out ControlType>
    {
        /// <summary>
        /// Aplica a acção ao objecto.
        /// </summary>
        /// <param name="element">O objecto.</param>
        /// <returns>A mensagem de controlo.</returns>
        ControlType Apply(ObjectType element);
    }
}
