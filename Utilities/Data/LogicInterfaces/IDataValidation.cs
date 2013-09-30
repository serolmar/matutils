namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IDataValidation<out ColumnType, in ObjectType>
    {
        /// <summary>
        /// Obtém o número das colunas que estarão envolvidas na validação.
        /// </summary>
        ColumnType[] Columns { get; }

        /// <summary>
        /// Valida o elemento especificado.
        /// </summary>
        /// <param name="element">O elemento a ser validado.</param>
        /// <returns>Veradeiro caso o elemento seja válido e falso caso contrário.</returns>
        bool Validate(ObjectType[] element);
    }
}
