namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    public interface ITabularItem
        : IIndexed<int, ITabularRow>, 
        IEnumerable<ITabularRow>
    {
        void AddValidation(IDataValidation<int, object> validation);

        void RemoveValidation(IDataValidation<int, object> validation);

        void ClearValidations();

        /// <summary>
        /// Atribui o valor a uma célula.
        /// </summary>
        /// <typeparam name="ElementType">O tipo do valor.</typeparam>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="cellNumber">O número da coluna.</param>
        /// <param name="value">O valor.</param>
        void SetValue<ElementType>(int rowNumber, int cellNumber, ElementType value);

        /// <summary>
        /// Actualiza as linhas do item tabular que satisfazem uma determinada condição.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a atribuir à linha.</typeparam>
        /// <param name="values">O tipo de valores.</param>
        /// <param name="expression">A expressão de condição.</param>
        /// <returns>O número de linhas afectadas.</returns>
        int UpdateCellsWhere<ElementType>(
            IEnumerable<KeyValuePair<int, ElementType>> values,
            Func<ITabularRow, bool> expression);

        /// <summary>
        /// Adiciona uma linha ao final do item tabular.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de valores a adicionar.</typeparam>
        /// <param name="elements">Os valores a serem adicionados.</param>
        void Add<ElementType>(IEnumerable<ElementType> elements);

        /// <summary>
        /// Adiciona uma linha especificada por valores indexados por coluna ao final do item tabular.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de valores a adicionar.</typeparam>
        /// <param name="elements">Os valores a serem adicionados.</param>
        void Add<ElementType>(IEnumerable<KeyValuePair<int, ElementType>> elements);

        /// <summary>
        /// Adiciona várias linhas ao final do item tabular.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a adicionar.</typeparam>
        /// <param name="elements">Os valores dos elementos a serem adicionados.</param>
        void AddRange<ElementType>(IEnumerable<IEnumerable<ElementType>> elements);

        /// <summary>
        /// Adiciona várias linhas especificadas por valores indexados por coluna ao final do item tabular.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a adicionar.</typeparam>
        /// <param name="elements">Os valores dos elementos.</param>
        void AddRange<ElementType>(IEnumerable<IEnumerable<KeyValuePair<int, ElementType>>> elements);

        /// <summary>
        /// Insere uma linha com um conjunto de valores na posição especificada pelo índice.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a serem inseridos.</typeparam>
        /// <param name="index">O índice.</param>
        /// <param name="elements">Os valores a serem inseridos.</param>
        void Insert<ElementType>(int index, IEnumerable<ElementType> elements);

        /// <summary>
        /// Insere uma linha com um conjunto de valores na posição especificada pelo índice.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a serem inseridos.</typeparam>
        /// <param name="index">O índice.</param>
        /// <param name="elements">Os valores a serem inseridos.</param>
        void Insert<ElementType>(int index, IEnumerable< KeyValuePair<int, ElementType>> elements);

        /// <summary>
        /// Insere várias linhas com um conjunto de valores na posição especificada pelo índice.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a serem inseridos.</typeparam>
        /// <param name="index">O índice.</param>
        /// <param name="elements">Os valores a serem inseridos.</param>
        void InsertRange<ElementType>(int index, IEnumerable<IEnumerable<ElementType>> elements);

        /// <summary>
        /// Insere várias linhas com um conjunto de valores indexados por coluna na posição
        /// especificada pelo índice.
        /// </summary>
        /// <typeparam name="ElementType">O tipo de elementos a serem inseridos.</typeparam>
        /// <param name="index">O índice.</param>
        /// <param name="elements">Os valores a serem inseridos.</param>
        void InsertRange<ElementType>(int index, IEnumerable<IEnumerable<KeyValuePair<int, ElementType>>> elements);

        /// <summary>
        /// Remove uma linha especificada pela posição.
        /// </summary>
        /// <param name="rowIndex">O índice da posição.</param>
        void RemoveRow(int rowIndex);

        /// <summary>
        /// Remove todas as linhas que satisfaçam uma determinada condição.
        /// </summary>
        /// <param name="expression">A expressão.</param>
        /// <returns>O número de linhas removidas.</returns>
        int RemoveWhere(Func<ITabularRow, bool> expression);
    }
}
