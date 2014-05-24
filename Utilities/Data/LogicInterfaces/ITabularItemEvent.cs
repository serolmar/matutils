namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um evento que pode ser associado a um item tabular.
    /// </summary>
    public interface ITabularItemEvent
    {
        /// <summary>
        /// Define o evento que é despoletado antes do valor duma célula ser alterado.
        /// </summary>
        event UpdateEventHandler<ITabularCell, object> BeforeSetEvent;

        /// <summary>
        /// Define o evento que é despoletado depois do valor duma célula ser alterado.
        /// </summary>
        event UpdateEventHandler<ITabularCell, object> AfterSetEvent;

        /// <summary>
        /// Define o evento que é despoletado antes da actualização de uma célula.
        /// </summary>
        event UpdateEventHandler<List<List<ITabularCell>>, List<object>> BeforeUpdateEvent;

        /// <summary>
        /// Define o evento que é despoletado depois da actualização de uma célula.
        /// </summary>
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

        /// <summary>
        /// Define o evento que é depoletado antes da adição de várias linhas.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> BeforeAddRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado depois da adição de várias linhas.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<IEnumerable<object>>> AfterAddRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado antes da adição de vários mapeamentos de coluna para valor.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>> 
            BeforeKeyedValuesAddRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado depois da adição de vários mapeamentos de coluna para valor.
        /// </summary>
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

        /// <summary>
        /// Define o evento que é despoletado antes da inserção de vários mapeamentos de coluna para valor.
        /// </summary>
        event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> BeforeKeyedValuesInsertEvent;

        /// <summary>
        /// Define o evento que é despoletado depois da inserção de vários mapeamentos de coluna para valor.
        /// </summary>
        event InsertEventHandler<IEnumerable<KeyValuePair<int, object>>> AfterKeyedValuesInsertEvent;

        /// <summary>
        /// Define o evento que é despoletado antes da inserção de várias linhas.
        /// </summary>
        event InsertEventHandler<IEnumerable<IEnumerable<object>>> BeforeInsertRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado depois da inserção de várias linhas.
        /// </summary>
        event InsertEventHandler<IEnumerable<IEnumerable<object>>> AfterInsertRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado antes da inserção de vários mapeamentos de coluna para valor.
        /// </summary>
        event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>> 
            BeforeKeyedValuesInsertRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado depois da inserção de vários mapeamentos de coluna para valor.
        /// </summary>
        event InsertEventHandler<IEnumerable<IEnumerable<KeyValuePair<int, object>>>> 
            AfterKeyedValuesInsertRangeEvent;

        /// <summary>
        /// Define o evento que é despoletado antes da remoção de uma linha.
        /// </summary>
        event AddDeleteEventHandler<ITabularRow> BeforeDeleteEvent;

        /// <summary>
        /// Define o evento que é despoletado depois da remoção de uma linha.
        /// </summary>
        event AddDeleteEventHandler<ITabularRow> AfterDeleteEvent;

        /// <summary>
        /// Define o evento que é despoletado antes da remoção de várias linhas.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<ITabularRow>> BeforeDeleteRange;

        /// <summary>
        /// Define o evento que é despoletado depois da remoção de várias linhas.
        /// </summary>
        event AddDeleteEventHandler<IEnumerable<ITabularRow>> AfterDeleteRange;
    }
}
