using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface ITree<NodeObjectType>
    {
        NodeObjectType RootNode { get; }

        /// <summary>
        /// Visita a árvore em pré-ordem.
        /// </summary>
        /// <param name="visitor">O visitador de objectos.</param>
        void PreOrderTreeTraveral(IVisitor<NodeObjectType> visitor);

        /// <summary>
        /// Vista a árvore em ordem.
        /// </summary>
        /// <param name="visitor">O visitador de objectos.</param>
        void InOrderTreeTraversal(IVisitor<NodeObjectType> visitor);

        /// <summary>
        /// Vista a árvore em pós ordem.
        /// </summary>
        /// <param name="visistor">O visitador de objectos.</param>
        void PostOrderTreeTraversal(IVisitor<NodeObjectType> visistor);
    }
}
