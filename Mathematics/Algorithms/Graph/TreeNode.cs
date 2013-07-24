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

        private List<TreeNode<NodeObjectType>> childs = new List<TreeNode<NodeObjectType>>();
        
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

        public IEnumerable<ITreeNode<NodeObjectType>> Childs
        {
            get
            {
                return this.childs;
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

        public void Add(TreeNode<NodeObjectType> child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }
            else if (child.owner != null)
            {
                throw new ArgumentException("Root node was already added to some tree.");
            }
            else if (this.owner == null)
            {
                throw new MathematicsException("Node must belong to a tree in order to have child nodes.");
            }
            else
            {
                this.childs.Add(child);
                child.owner = this.owner;
            }
        }

        public void Remove(TreeNode<NodeObjectType> child)
        {
            this.childs.Remove(child);
            child.owner = null;
        }
    }
}
