namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma ligação de nós.
    /// </summary>
    /// <typeparam name="NodeType">O tipo de objectos que constituem os nós.</typeparam>
    /// <typeparam name="EdgeValueType">O tipo de objectos associados às arestas.</typeparam>
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
        /// <value>A aresta.</value>
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
        /// <value>O vértice final.</value>
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
