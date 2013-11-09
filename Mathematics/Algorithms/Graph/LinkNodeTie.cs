namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class LinkNodeTie<NodeType>
    {
        /// <summary>
        /// A aresta que liga ao vértice final.
        /// </summary>
        private Edge<NodeType> tieEdge;

        /// <summary>
        /// O vértice final.
        /// </summary>
        private LinkNode<NodeType> otherNode;

        /// <summary>
        /// Otbém a areasta que liga ao vértice final.
        /// </summary>
        public Edge<NodeType> TieEdge
        {
            get
            {
                return this.tieEdge;
            }
            set
            {
                this.tieEdge = value;
            }
        }

        /// <summary>
        /// Obtém o vértice final.
        /// </summary>
        public LinkNode<NodeType> OtherNode
        {
            get
            {
                return this.otherNode;
            }
            set
            {
                this.otherNode = value;
            }
        }
    }
}
