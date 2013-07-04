using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    internal class LinkNode<NodeType>
    {
        /// <summary>
        /// O nó inicial.
        /// </summary>
        private NodeType node;

        /// <summary>
        /// O próximo nó.
        /// </summary>
        private LinkNode<NodeType> link;

        /// <summary>
        /// Obtém o nó.
        /// </summary>
        public NodeType Node
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
        /// Obtém o próximo elemento.
        /// </summary>
        public LinkNode<NodeType> Link
        {
            get
            {
                return this.link;
            }
            set
            {
                this.link = value;
            }
        }
    }
}
