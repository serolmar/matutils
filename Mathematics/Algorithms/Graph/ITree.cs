using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface ITree<NodeObjectType>
    {
        /// <summary>
        /// Obtém a raíz da árvore.
        /// </summary>
        ITreeNode<NodeObjectType> RootNode { get; }

        /// <summary>
        /// Visita a árvore seguindo em primeiro lugar até às folhas.
        /// </summary>
        /// <param name="visitor">O visitador.</param>
        void DephSearchVisit(IVisitor<NodeObjectType> visitor);

        /// <summary>
        /// Visita a árvore segundo os níveis.
        /// </summary>
        /// <param name="visitor">O visitador.</param>
        void BreadthFirstVisit(IVisitor<NodeObjectType> visitor);
    }
}
