namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class LinkNodeTie<NodeType, EdgeValueType>
    {
        /// <summary>
        /// A aresta que liga ao vértice final.
        /// </summary>
        private Edge<NodeType, EdgeValueType> tieEdge;

        /// <summary>
        /// O vértice final.
        /// </summary>
        private LinkNode<NodeType, EdgeValueType> otherNode;

        /// <summary>
        /// Otbém a areasta que liga ao vértice final.
        /// </summary>
        public Edge<NodeType, EdgeValueType> TieEdge
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
        public LinkNode<NodeType, EdgeValueType> OtherNode
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
