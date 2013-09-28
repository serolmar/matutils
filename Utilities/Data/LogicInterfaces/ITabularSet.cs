namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

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
