using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public class TreeNode<NodeObjectType> : ITreeNode<NodeObjectType>
    {
        private Tree<NodeObjectType> owner;

        private NodeObjectType nodeObject;

        private TreeNode<NodeObjectType> parent;

        private List<TreeNode<NodeObjectType>> childs = new List<TreeNode<NodeObjectType>>();

        internal TreeNode(
            NodeObjectType nodeObject, 
            Tree<NodeObjectType> owner, 
            TreeNode<NodeObjectType> parent)
        {
            this.nodeObject = nodeObject;
            this.owner = owner;
            this.parent = parent;
        }
        
        /// <summary>
        /// Obtém o valor do nó.
        /// </summary>
        public NodeObjectType NodeObject
        {
            get
            {
                return this.nodeObject;
            }
            set
            {
                this.nodeObject = value;
            }
        }

        public int Count
        {
            get
            {
                return this.childs.Count;
            }
        }

        /// <summary>
        /// Obtém os nós filho.
        /// </summary>
        public IEnumerable<ITreeNode<NodeObjectType>> Childs
        {
            get
            {
                return this.childs;
            }
        }

        /// <summary>
        /// Obtém o nó precedente e nulo caso o nó actual seja a raiz.
        /// </summary>
        public ITreeNode<NodeObjectType> Parent
        {
            get
            {
                return this.parent;
            }
        }

        internal TreeNode<NodeObjectType> InternalParent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = value;
            }
        }

        internal Tree<NodeObjectType> Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
            }
        }

        internal List<TreeNode<NodeObjectType>> ChildsList
        {
            get
            {
                return this.childs;
            }
        }

        public ITreeNode<NodeObjectType> ChildAt(int position)
        {
            if (position < 0 || position >= this.childs.Count)
            {
                throw new ArgumentOutOfRangeException("position");
            }
            else
            {
                return this.childs[position];
            }
        }

        /// <summary>
        /// Adiciona um novo valor filho ao nó corrente.
        /// </summary>
        /// <param name="child">O valor a ser adicionado.</param>
        /// <returns>O nó que contém o valor.</returns>
        public ITreeNode<NodeObjectType> Add(NodeObjectType child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }
            else
            {
                var node = new TreeNode<NodeObjectType>(
                    child,
                    this.owner,
                    this);
                this.childs.Add(node);
                return node;
            }
        }

        /// <summary>
        /// Remove o nó filho do nó actual.
        /// </summary>
        /// <param name="child">O nó a ser removido.</param>
        public void Remove(TreeNode<NodeObjectType> child)
        {
            this.childs.Remove(child);
        }

        /// <summary>
        /// Move o nó especificado estabelecendo-o como filho do nó actual na posição indicada
        /// como argumento.
        /// </summary>
        /// <param name="node">O nó.</param>
        /// <param name="insertPosition">A posição a inserir no conjunto dos filhos.</param>
        /// <returns>O nó movido.</returns>
        public ITreeNode<NodeObjectType> MoveNode(
            ITreeNode<NodeObjectType> node,
            int insertPosition)
        {
            var innerNode = node as TreeNode<NodeObjectType>;
            if (innerNode == null)
            {
                throw new ArgumentException("Tree node is invalid.");
            }
            else if (insertPosition < 0 || insertPosition > this.childs.Count)
            {
                throw new ArgumentOutOfRangeException("insertPosition");
            }
            else
            {
                var parentNode = innerNode.parent;
                parentNode.Remove(innerNode);
                innerNode.owner = this.owner;
                innerNode.parent = this;
                this.childs.Insert(insertPosition, innerNode);
                return innerNode;
            }
        }

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
        public ITreeNode<NodeObjectType> CopyNode(
            ITreeNode<NodeObjectType> node,
            int insertPosition)
        {
            var innerNode = node as TreeNode<NodeObjectType>;
            if (innerNode == null)
            {
                throw new ArgumentException("Tree node is invalid.");
            }
            else if (insertPosition < 0 || insertPosition > this.childs.Count)
            {
                throw new ArgumentOutOfRangeException("insertPosition");
            }
            else
            {
                var copiedNode = new TreeNode<NodeObjectType>(
                    innerNode.nodeObject,
                    this.owner,
                    this);
                var nodesQueue = new Queue<Tuple<TreeNode<NodeObjectType>,TreeNode<NodeObjectType>>>();
                nodesQueue.Enqueue(Tuple.Create(innerNode, copiedNode));
                while (nodesQueue.Count > 0)
                {
                    var topCopies = nodesQueue.Dequeue();
                    foreach (var child in topCopies.Item1.childs)
                    {
                        var copiedChild = new TreeNode<NodeObjectType>(
                            child.nodeObject,
                            this.owner,
                            topCopies.Item2);
                        topCopies.Item2.childs.Add(child);
                        nodesQueue.Enqueue(Tuple.Create(child, copiedChild));
                    }
                }

                this.childs.Insert(insertPosition, copiedNode);
                return copiedNode;
            }
        }

        internal void Add(TreeNode<NodeObjectType> node)
        {
            this.childs.Add(node);
        }
    }
}
