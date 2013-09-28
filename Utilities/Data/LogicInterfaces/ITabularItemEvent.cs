namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal interface ITabularItemEvent
    {
        /// <summary>
        /// Define o evento que é despoletado antes do valor duma célula ser alterado.
        /// </summary>
        event UpdateEventHandler<ITabularCell, object> BeforeSetEvent;

        /// <summary>
        /// Define o evento que é despoletado depois do valor duma célula ser alterado.
        /// </summary>
        event UpdateEventHandler<ITabularCell, object> AfterSetEvent;

        event UpdateEventHandler<List<List<ITabularCell>>, List<object>> BeforeUpdateEvent;

        event UpdateEventHandler<List<List<ITabularCell>>, List<object>> AfterUpdateEvent;

        /// <summary>
        /// Define o evento que é despoletado aquando da adição de uma enumeração de elementos.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<object>> BeforeAddEvent;

        /// <summary>
        /// Define o evento que é despoletado aquando do final da adição de uma enumeração de elementos.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<object>> AfterAddEvent;

        /// <summary>
        /// Define o evento que é despoletado aquando da adição de uma colecção de valores mapeados.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<KeyValuePair<int, object>>> BeforeAddKeyedValuesEvent;

        event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> BeforeAddRangeEvent;

        event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> AfterAddRangeEvent;

        event AddDeleteEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>> 
            BeforeKeyedValuesAddRangeEvent;

        event AddDeleteEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>> 
            AfterKeyedValuesAddRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado aquando do final da adição de uma colecção de valores mapeados.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<KeyValuePair<int, object>>> AfterAddKeyedValuesEvent;

        /// <summary>
        /// Define o evento que é despoletado quando da inserção de uma colecção de valores.
        /// </summary>
        event InsertEventHandler<IEnumerable<object>> BeforeInsertEvent;

        /// <summary>
        /// Define o evento que é despoletado no final da inserção de uma colecção de valores.
        /// </summary>
        event InsertEventHandler<IEnumerable<object>> AfterInsertEvent;

        event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> BeforeKeyedValuesInsertEvent;

        event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> AfterKeyedValuesInsertEvent;

        event InsertEventHandler<IEnumerable<IEnumerable<object>>> BeforeInsertRangeEvent;

        event InsertEventHandler<IEnumerable<IEnumerable<object>>> AfterInsertRangeEvent;

        event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>> 
            BeforeKeyedValuesInsertRangeEvent;

        event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>> 
            AfterKeyedValuesInsertRangeEvent;

        event AddDeleteEventHandler<ITabularRow> BeforeDeleteEvent;

        event AddDeleteEventHandler<ITabularRow> AfterDeleteEvent;

        event AddDeleteEventHandler<IEnumerable<ITabularRow>> BeforeDeleteRange;

        event AddDeleteEventHandler<IEnumerable<ITabularRow>> AfterDeleteRange;
    }
}
