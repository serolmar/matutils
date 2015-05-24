namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;

    /// <summary>
    /// Representa uma árvore associativa.
    /// </summary>
    /// <typeparam name="LabelType">O tipo dos objectos que constituem as etiquetas.</typeparam>
    /// <typeparam name="ColType">O tipo de objectos associados ao conjunto de etiquetas.</typeparam>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem as etiquetas.</typeparam>
    public interface ITrie<LabelType, ColType, ObjectType>
        where ColType : IEnumerable<LabelType>
    {
        /// <summary>
        /// Otbém um iterador para a árvore associativa.
        /// </summary>
        /// <returns>O iterador.</returns>
        ITrieIterator<LabelType, ObjectType> GetIterator();
    }

    /// <summary>
    /// Representa um iterador que actua sobre árvores associativas.
    /// </summary>
    /// <typeparam name="LabelType">O tipo dos objectos que constituem as etiquetas.</typeparam>
    /// <typeparam name="ObjectType">O tipo de objectos associados ao conjunto de etiquetas.</typeparam>
    public interface ITrieIterator<LabelType, ObjectType> : IDisposable
    {
        /// <summary>
        /// Obtém o índice do prefixo actual.
        /// </summary>
        ObjectType Current { get; }

        /// <summary>
        /// Obtém um valor que indica se existe colecção associada ao nó apontado pelo iterador.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Otbém o conjunto de etiquetas.
        /// </summary>
        IEnumerable<LabelType> Labels { get; }

        /// <summary>
        /// Verifica se é possível avançar para a etiqueta proporcionada.
        /// </summary>
        /// <param name="label">A etiqueta.</param>
        /// <returns>Verdadeiro caso seja possível avançar e falso caso contrário.</returns>
        bool CanAdvance(LabelType label);

        /// <summary>
        /// Tenta obter o objecto actual.
        /// </summary>
        /// <param name="col">O parâmetro que recebe o índice do objecto caso este exitsta.</param>
        /// <returns>Verdadeiro se o objecto existir e falso caso contrário.</returns>
        bool TryGetCurrent(out ObjectType col);

        /// <summary>
        /// Avança na árvore para o próximo elemento especificado pela etiqueta.
        /// </summary>
        /// <param name="label">A etiqueta.</param>
        /// <returns>Verdadeiro se for possível avançar e falso caso contrário.</returns>
        bool GoForward(LabelType label);

        /// <summary>
        /// Retrocede uma posição.
        /// </summary>
        /// <returns>Verdadeiro caso seja possível retroceder e falso caso contrário.</returns>
        bool GoBack();

        /// <summary>
        /// Coloca o iterador na raíz da árvore.
        /// </summary>
        void Reset();
    }
}
