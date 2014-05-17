namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um nó de uma árvore.
    /// </summary>
    /// <typeparam name="NodeObjectType">O tipo de objectos associados aos nós.</typeparam>
    public class TreeNode<NodeObjectType> : ITreeNode<NodeObjectType>
    {
        /// <summary>
        /// A árvore da qual o nó faz parte.
        /// </summary>
        private Tree<NodeObjectType> owner;

        /// <summary>
        /// O objecto associado ao nó.
        /// </summary>
        private NodeObjectType nodeObject;

        /// <summary>
        /// O nó do qual descende.
        /// </summary>
        private TreeNode<NodeObjectType> parent;

        /// <summary>
        /// A lista de nós descendentes.
        /// </summary>
        private List<TreeNode<NodeObjectType>> childs = new List<TreeNode<NodeObjectType>>();

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TreeNode{NodeObjectType}"/>.
        /// </summary>
        /// <param name="nodeObject">O objecto associado ao nó.</param>
        /// <param name="owner">A árvore da qual faz parte.</param>
        /// <param name="parent">O nó do qual descende.</param>
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
        /// <value>O valor do nó.</value>
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
        /// Obtém o número de nós descendentes.
        /// </summary>
        /// <value>O número de nós descendentes.</value>
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
        /// <value>Os nós descendentes.</value>
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
        /// <value>O nó precedente.</value>
        public ITreeNode<NodeObjectType> Parent
        {
            get
            {
                return this.parent;
            }
        }

        /// <summary>
        /// Obtém e atribui o nó precedente.
        /// </summary>
        /// <value>O nó precedente.</value>
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

        /// <summary>
        /// Obtém e atribui a árvore da qual faz parte.
        /// </summary>
        /// <value>A árvore.</value>
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

        /// <summary>
        /// Obtém a lista de nós descendentes.
        /// </summary>
        /// <value>A lista de nós descendentes.</value>
        internal List<TreeNode<NodeObjectType>> ChildsList
        {
            get
            {
                return this.childs;
            }
        }

        /// <summary>
        /// Obtém o nó descendente especificado pelo índice.
        /// </summary>
        /// <param name="position">O índice.</param>
        /// <returns>O nó.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se o índice for negativo ou não for inferior ao número de nós descendentes.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">Se o argumento for nulo.</exception>
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
        /// <exception cref="ArgumentException">Se o nó interno for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a posição for negativa ou for superior ao número de descendentes.
        /// </exception>
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
        /// <exception cref="ArgumentException">Se o nó for nulo.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Se a posição for negativa ou for superior ao número de descendentes.
        /// </exception>
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

        /// <summary>
        /// Obtém uma representação textual do nó.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            if (this.nodeObject == null)
            {
                return "null";
            }
            else
            {
                return this.nodeObject.ToString();
            }
        }

        /// <summary>
        /// Adiciona um nó à lista de nós descendentes.
        /// </summary>
        /// <param name="node">O nó a adicionar.</param>
        internal void Add(TreeNode<NodeObjectType> node)
        {
            this.childs.Add(node);
        }
    }
}
