namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um nó de uma árvore.
    /// </summary>
    /// <typeparam name="NodeObjectType">O tipo de objectos associados aos nós.</typeparam>
    public interface ITreeNode<NodeObjectType>
    {
        /// <summary>
        /// Obtém e atribui o objecto associado ao nó.
        /// </summary>
        NodeObjectType NodeObject { get; set; }

        /// <summary>
        /// Obtém o número de filhos-
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Obtém todos os nós filho.
        /// </summary>
        IEnumerable<ITreeNode<NodeObjectType>> Childs { get; }

        /// <summary>
        /// Obtém o filho que se encontra na posição especificada.
        /// </summary>
        /// <param name="position">A posição.</param>
        /// <returns>O nó filho que se encontra na posição especificada.</returns>
        ITreeNode<NodeObjectType> ChildAt(int position);

        /// <summary>
        /// Adiciona um novo valor filho ao nó corrente.
        /// </summary>
        /// <param name="child">O valor a ser adicionado.</param>
        /// <returns>O nó que contém o valor.</returns>
        ITreeNode<NodeObjectType> Add(NodeObjectType child);

        /// <summary>
        /// Remove o nó filho do nó actual.
        /// </summary>
        /// <param name="child">O nó a ser removido.</param>
        void Remove(TreeNode<NodeObjectType> child);

        /// <summary>
        /// Move o nó especificado estabelecendo-o como filho do nó actual na posição indicada
        /// como argumento.
        /// </summary>
        /// <param name="node">O nó.</param>
        /// <param name="insertPosition">A posição a inserir no conjunto dos filhos.</param>
        /// <returns>O nó movido.</returns>
        ITreeNode<NodeObjectType> MoveNode(
            ITreeNode<NodeObjectType> node,
            int insertPosition);

        /// <summary>
        /// Copia o nó especificado inserindo-o como filho do nó actual na posição indicada
        /// como argumento.
        /// </summary>
        /// <remarks>
        /// O valor associado ao nó não será copiado mantendo-se, portanto, a sua referência.
        /// </remarks>
        /// <param name="node">O nó a ser copiado.</param>
        /// <param name="insertPosition">A posição a inserir.</param>
        /// <returns>O nó copiado.</returns>
        ITreeNode<NodeObjectType> CopyNode(
            ITreeNode<NodeObjectType> node,
            int insertPosition);
    }
}
