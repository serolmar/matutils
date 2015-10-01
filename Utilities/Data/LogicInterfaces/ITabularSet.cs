namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Define uma célula geral de uma tabela.
    /// </summary>
    public interface IGeneralTabularCell
    {
        /// <summary>
        /// Obtém o número da linha.
        /// </summary>
        /// <value>O número da linha.</value>
        int RowNumber { get; }

        /// <summary>
        /// Obtém o número da coluna.
        /// </summary>
        /// <value>O número da coluna.</value>
        int ColumnNumber { get; }

        /// <summary>
        /// Obtém o valor que indica se o conteúdo da célula é nulo ou um valor textual vazio.
        /// </summary>
        /// <value>Verdadeiro caso a célula seja vazia e falso caso contrário.</value>
        bool NullOrEmpty { get; }

        /// <summary>
        /// Obtém o tipo de valor do conteúdo da célula.
        /// </summary>
        /// <value>O tipo do valor.</value>
        Type ValueType { get; }

        /// <summary>
        /// Obtém o valor da célula.
        /// </summary>
        /// <typeparam name="T">O valor da célula a ser atribuído.</typeparam>
        /// <returns>O valor obtido.</returns>
        T GetCellValue<T>();

        /// <summary>
        /// Obtém o valor da célula como texto.
        /// </summary>
        /// <returns>A representação textual da célula.</returns>
        string GetAsText();
    }

    /// <summary>
    /// Define a célula de uma tabela só de leitura.
    /// </summary>
    public interface IReadonlyTabularCell : IGeneralTabularCell
    {
    }

    /// <summary>
    /// Define uma célula.
    /// </summary>
    public interface ITabularCell : IGeneralTabularCell
    {
        /// <summary>
        /// Atribui um valor à célula.
        /// </summary>
        /// <typeparam name="T">O tipo do valor a ser atribuído.</typeparam>
        /// <param name="value">O valor a ser atribuído.</param>
        void SetCellValue<T>(T value);
    }

    /// <summary>
    /// Define uma linha geral.
    /// </summary>
    /// <typeparam name="L">O tipo de objectos que constituem as células.</typeparam>
    public interface IGeneralTabularRow<out L>
        : IEnumerable<L>
    {
        /// <summary>
        /// Obtém o objecto especificado pelo índice.
        /// </summary>
        /// <value>
        /// O objecto.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns></returns>
        L this[int index] { get; }

        /// <summary>
        /// Obtém o número de elementos.
        /// </summary>
        /// <value>
        /// O número de elementos.
        /// </value>
        int Count { get; }

        /// <summary>
        /// O número da última coluna.
        /// </summary>
        int LastColumnNumber { get; }

        /// <summary>
        /// Obtém o número da linha.
        /// </summary>
        int RowNumber { get; }
    }

    /// <summary>
    /// Define a linha de uma tabela só de leitura.
    /// </summary>
    public interface IReadonlyTabularRow
        : IGeneralTabularRow<IReadonlyTabularCell>
    {
    }

    /// <summary>
    /// Define uma linha de tabela.
    /// </summary>
    public interface ITabularRow
        : IGeneralTabularRow<ITabularCell>
    {
    }

    /// <summary>
    /// Define uma tabela geral.
    /// </summary>
    /// <typeparam name="R">O tipo dos objectos que constituem as linhas.</typeparam>
    public interface IGeneralTabularItem<out R>
        : IEnumerable<R>
    {
        /// <summary>
        /// Obtém o objecto especificado pelo índice.
        /// </summary>
        /// <value>
        /// O objecto.
        /// </value>
        /// <param name="index">O índice.</param>
        /// <returns>O valor.</returns>
        R this[int index] { get; }

        /// <summary>
        /// Obtém o número da última linha.
        /// </summary>
        int LastRowNumber { get; }

        /// <summary>
        /// Obtém o número de elementos.
        /// </summary>
        /// <value>
        /// O número de elementos.
        /// </value>
        int Count { get; }

        /// <summary>
        /// Obtém o número de colunas para a linha especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número de colunas na linha.</returns>
        int ColumnsCount(int rowNumber);

        /// <summary>
        /// Obtém o número da última coluna na linha especificada.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <returns>O número da coluna.</returns>
        int GetLastColumnNumber(int rowNumber);

        /// <summary>
        /// Obtém o valor não genérico da célula.
        /// </summary>
        /// <param name="rowNumber">O número da linha.</param>
        /// <param name="columnNumber">O número da coluna.</param>
        /// <returns>O valor da célula.</returns>
        object GetCellValue(int rowNumber, int columnNumber);
    }

    /// <summary>
    /// Define uma tabela só de leitura.
    /// </summary>
    public interface IReadonlyTabularItem
        : IGeneralTabularItem<IReadonlyTabularRow>
    {
    }

    /// <summary>
    /// Implementa um item tabular.
    /// </summary>
    /// <remarks>
    /// Um item tabular pode ser uma tabela em memória, uma tabela oriunda de uma base-de-dados
    /// ou até algum objecto que cujo comportamento se assemelhe ao de uma tabela.
    /// </remarks>
    public interface ITabularItem
        : IGeneralTabularItem<ITabularRow>
    {
        /// <summary>
        /// Adiciona uma validação ao item tabular.
        /// </summary>
        /// <param name="validation">A validação.</param>
        /// <param name="validateExisting">
        /// Valor que indica se os itens existentes serão validados.
        /// </param>
        void AddValidation(IDataValidation<int, object> validation, bool validateExisting);

        /// <summary>
        /// Remove a validações especificada do item tabular.
        /// </summary>
        /// <param name="validation">A validação a ser removida.</param>
        void RemoveValidation(IDataValidation<int, object> validation);

        /// <summary>
        /// Limpa todas as validações do item tabular.
        /// </summary>
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
        void Insert<ElementType>(int index, IEnumerable<KeyValuePair<int, ElementType>> elements);

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

    /// <summary>
    /// Define um item tabular defindo com um nome e um índice.
    /// </summary>
    public interface INamedTabularItem : ITabularItem
    {
        /// <summary>
        /// Obtém o número do item tabular no respectivo conjunto de itens tabulares.
        /// </summary>
        /// <value>O número do item tabular.</value>
        int TabularItemNumber { get; }

        /// <summary>
        /// Obtém o nome do item tabular no respectivo conjunto de itens tabulares.
        /// </summary>
        /// <value>O nome do item tabular.</value>
        string Name { get; }
    }

    /// <summary>
    /// Define um conjunto de tabelas.
    /// </summary>
    public interface ITabularSet :
        IIndexed<int, ITabularRow>,
        IIndexed<string, ITabularRow>,
        IEnumerable<ITabularRow>
    {
        /// <summary>
        /// Verifica se determinado nome existe no conjunto de itens tabulares.
        /// </summary>
        /// <param name="name">O nome.</param>
        /// <returns>Verdadeiro caso um item com o nome especificado exista e falso caso contrário.</returns>
        bool ContainsName(string name);

        /// <summary>
        /// Remove a tabela na posição especificada pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        void Remove(int index);

        /// <summary>
        /// Remove a tabela especificada pelo respectivo nome.
        /// </summary>
        /// <param name="name">O nome da tabela.</param>
        void Remove(string name);

        /// <summary>
        /// Cria um item tabular com o nome especificado.
        /// </summary>
        /// <param name="name">O nome.</param>
        /// <returns>O item tabular.</returns>
        INamedTabularItem CreateTable(string name);

        /// <summary>
        /// Elimina todos os itens tabulares da tabela.
        /// </summary>
        void Clear();
    }
}
