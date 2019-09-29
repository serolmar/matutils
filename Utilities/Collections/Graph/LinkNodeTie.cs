namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa uma ligação de nós.
    /// </summary>
    /// <typeparam name="NodeType">O tipo de objectos que constituem os nós.</typeparam>
    /// <typeparam name="EdgeType">O tipo de objectos que constituem as arestas.</typeparam>
    public class LinkNodeTie<NodeType, EdgeType>
        where EdgeType : IEdge<NodeType>
    {
        /// <summary>
        /// A aresta que liga ao vértice final.
        /// </summary>
        private EdgeType tieEdge;

        /// <summary>
        /// O vértice final.
        /// </summary>
        private LinkNode<NodeType, EdgeType> otherNode;

        /// <summary>
        /// Otbém a areasta que liga ao vértice final.
        /// </summary>
        /// <value>A aresta.</value>
        public EdgeType TieEdge
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
        public LinkNode<NodeType, EdgeType> OtherNode
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
