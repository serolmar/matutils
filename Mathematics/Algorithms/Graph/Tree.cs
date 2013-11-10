using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class Tree<NodeObjectType> : ITree<NodeObjectType>
    {
        private TreeNode<NodeObjectType> rootNode;

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
        /// Obtém a raíz da árvore.
        /// </summary>
        public ITreeNode<NodeObjectType> RootNode
        {
            get
            {
                return this.rootNode;
            }
        }

        /// <summary>
        /// Obtém a raiz da árvore de acordo com o respectivo tipo.
        /// </summary>
        internal TreeNode<NodeObjectType> InternalRootNode
        {
            get
            {
                return this.rootNode;
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

        private class NodePointerPair
        {
            private TreeNode<NodeObjectType> node;

            private int pointer;

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
