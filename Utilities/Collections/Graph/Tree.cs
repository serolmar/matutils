namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma árovre.
    /// </summary>
    /// <typeparam name="NodeObjectType">O tipo de objectos associados aos nós.</typeparam>
    public class Tree<NodeObjectType> : ITree<NodeObjectType>
    {
        /// <summary>
        /// O nó de raiz.
        /// </summary>
        private TreeNode<NodeObjectType> rootNode;

        /// <summary>
        /// Instancia uma árvore proporcionando a raiz.
        /// </summary>
        /// <param name="rootNode">O nó raiz.</param>
        /// <exception cref="ArgumentNullException">Se o nós de raiz for nulo.</exception>
        public Tree(NodeObjectType rootNode)
        {
            if (rootNode == null)
            {
                throw new ArgumentNullException("rootNode");
            }
            else
            {
                this.rootNode = new TreeNode<NodeObjectType>(
                    rootNode,
                    this,
                    null);
            }
        }

        /// <summary>
        /// Permite instanciar uma árvore sem raiz para que possa ser usada rapidamente pelas livrarias internas.
        /// </summary>
        public Tree()
        {
        }

        /// <summary>
        /// Obtém a raiz da árvore.
        /// </summary>
        /// <value>A raiz.</value>
        public ITreeNode<NodeObjectType> RootNode
        {
            get
            {
                return this.rootNode;
            }
        }

        /// <summary>
        /// Obtém e atribui a raiz da árvore de acordo com o respectivo tipo.
        /// </summary>
        /// <value>A raiz</value>
        public TreeNode<NodeObjectType> InternalRootNode
        {
            get
            {
                return this.rootNode;
            }
            set
            {
                this.rootNode = value;
            }
        }

        /// <summary>
        /// Visita a árvore seguindo em primeiro lugar até às folhas.
        /// </summary>
        /// <param name="visitor">O visitador.</param>
        public void DephSearchVisit(IVisitor<NodeObjectType> visitor)
        {
            Stack<NodePointerPair> nodesStack = new Stack<NodePointerPair>();

            var topStack = new NodePointerPair() { Node = this.rootNode, Pointer = 0 };
            visitor.Visit(topStack.Node.NodeObject);
            nodesStack.Push(topStack);
            while (nodesStack.Count > 0)
            {
                topStack = nodesStack.Pop();
                if (topStack.Pointer < topStack.Node.ChildsList.Count)
                {
                    var newVisitNode = topStack.Node.ChildsList[topStack.Pointer];
                    visitor.Visit(newVisitNode.NodeObject);
                    nodesStack.Push(topStack);
                    nodesStack.Push(new NodePointerPair() { Node = newVisitNode, Pointer = 0 });
                    ++topStack.Pointer;
                }
            }
        }

        /// <summary>
        /// Visita a árvore segundo os níveis.
        /// </summary>
        /// <param name="visitor">O visitador.</param>
        public void BreadthFirstVisit(IVisitor<NodeObjectType> visitor)
        {
            Queue<TreeNode<NodeObjectType>> nodesQueue = new Queue<TreeNode<NodeObjectType>>();
            var firstNode = this.rootNode;
            visitor.Visit(firstNode.NodeObject);
            nodesQueue.Enqueue(firstNode);
            while (nodesQueue.Count > 0)
            {
                firstNode = nodesQueue.Dequeue();
                for (int i = 0; i < firstNode.ChildsList.Count; ++i)
                {
                    var currentChildNode = firstNode.ChildsList[i];
                    visitor.Visit(currentChildNode.NodeObject);
                    nodesQueue.Enqueue(currentChildNode);
                }
            }
        }

        /// <summary>
        /// Representa um par com nó e apontador.
        /// </summary>
        private class NodePointerPair
        {
            /// <summary>
            /// O nó.
            /// </summary>
            private TreeNode<NodeObjectType> node;

            /// <summary>
            /// O apontador.
            /// </summary>
            private int pointer;

            /// <summary>
            /// Obtém e atribui o nó.
            /// </summary>
            /// <value>O nó.</value>
            public TreeNode<NodeObjectType> Node
            {
                get
                {
                    return this.node;
                }
                set
                {
                    this.node = value;
                }
            }

            /// <summary>
            /// Obtém e atribui o apontador.
            /// </summary>
            /// <value>O apontador.</value>
            public int Pointer
            {
                get
                {
                    return this.pointer;
                }
                set
                {
                    this.pointer = value;
                }
            }
        }
    }
}
