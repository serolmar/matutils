namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IDataValidation<out ColumnType, in ObjectType>
    {
        /// <summary>
        /// Obtém o índice das colunas que estarão envolvidas na validação.
        /// </summary>
        IEnumerable<ColumnType> Columns { get; }

        /// <summary>
        /// Valida o elemento especificado.
        /// </summary>
        /// <param name="element">O elemento a ser validado.</param>
        /// <returns>Veradeiro caso o elemento seja válido e falso caso contrário.</returns>
        bool Validate(IEnumerable<ObjectType> element);
    }
}
