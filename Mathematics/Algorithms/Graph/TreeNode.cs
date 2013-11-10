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
        public TreeNode<NodeObjectType> Parent
        {
            get
            {
                return this.parent;
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

        /// <summary>
        /// Adiciona um novo valor filho ao nó corrente.
        /// </summary>
        /// <param name="child">O valor a ser adicionado.</param>
        /// <returns>O nó que contém o valor.</returns>
        public TreeNode<NodeObjectType> Add(NodeObjectType child)
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

        internal void Add(TreeNode<NodeObjectType> node)
        {
            this.childs.Add(node);
        }
    }
}
