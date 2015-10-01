namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma validação de células.
    /// </summary>
    /// <typeparam name="ColumnType">O tipo de objecto que representa as colunas que identificam as células.</typeparam>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem os elementos das células.</typeparam>
    public interface IDataValidation<out ColumnType, in ObjectType>
    {
        /// <summary>
        /// Obtém os índices das colunas que estarão envolvidas na validação.
        /// </summary>
        /// <value>Os índices das colunas.</value>
        IEnumerable<ColumnType> Columns { get; }

        /// <summary>
        /// Valida o elemento especificado.
        /// </summary>
        /// <remarks>
        /// O emitente das validações define as colunas que requerem validação. O utente das validações
        /// passa os valores das linhas correspondentes para a função de validação na ordem em que as
        /// colunas são definidas.
        /// </remarks>
        /// <param name="element">O elemento a ser validado.</param>
        /// <returns>Veradeiro caso o elemento seja válido e falso caso contrário.</returns>
        bool Validate(IEnumerable<ObjectType> element);
    }

    /// <summary>
    /// Define uma validação por linha.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que fazem parte da validação.</typeparam>
    public interface IDataRowValidation<ObjectType>
    {
        /// <summary>
        /// Valida uma linha completa.
        /// </summary>
        /// <param name="elements">Os elementos da linha a serem validados.</param>
        void Validate(IEnumerable<ObjectType> elements);
    }
}
