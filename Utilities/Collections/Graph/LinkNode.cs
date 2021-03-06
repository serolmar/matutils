﻿namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um nó de ligação auxiliar.
    /// </summary>
    /// <typeparam name="NodeType">O tipo de objectos que constituem os nós.</typeparam>
    /// <typeparam name="EdgeType">O tipo de objectos que constituem as arestas.</typeparam>
    public class LinkNode<NodeType, EdgeType>
        where EdgeType : IEdge<NodeType>
    {
        /// <summary>
        /// O nó inicial.
        /// </summary>
        private NodeType node;

        /// <summary>
        /// Mantém a lista de areastas ligadas ao nó inicial.
        /// </summary>
        private List<EdgeType> connectedEdges = 
            new List<EdgeType>();

        /// <summary>
        /// O próximo nó e a aresta que lhe deu origem.
        /// </summary>
        private LinkNodeTie<NodeType, EdgeType> link;

        /// <summary>
        /// Obtém o nó.
        /// </summary>
        /// <value>O nó.</value>
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
        /// Obtém a lista de arestas conectadas ao vértice inicial.
        /// </summary>
        /// <value>A lista de arestas.</value>
        public List<EdgeType> ConnectedEdges
        {
            get
            {
                return this.connectedEdges;
            }
        }

        /// <summary>
        /// Obtém o próximo elemento e a aresta que lhe deu origem.
        /// </summary>
        /// <value>O próximo elemento e a aresta.</value>
        public LinkNodeTie<NodeType, EdgeType> Link
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

        /// <summary>
        /// Obtém a representação textual da ligação.
        /// </summary>
        /// <returns>A representação textual da ligação.</returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(this.node);
            var next = this.link;
            while (next != null) {
                result.Append("-->");
                result.Append(next.OtherNode.node);
                next = next.OtherNode.link;
            }

            return result.ToString();
        }
    }
}
