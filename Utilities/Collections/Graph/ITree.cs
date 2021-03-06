﻿namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma árvore.
    /// </summary>
    /// <typeparam name="NodeObjectType">O tipo de objectos que constituem os nós.</typeparam>
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
