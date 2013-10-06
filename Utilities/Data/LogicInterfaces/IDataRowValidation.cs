namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IDataRowValidation<ObjectType>
    {
        /// <summary>
        /// Valida uma linha completa.
        /// </summary>
        /// <param name="elements">Os elementos da linha a serem validados.</param>
        void Validate(IEnumerable<ObjectType> elements);
    }
}
