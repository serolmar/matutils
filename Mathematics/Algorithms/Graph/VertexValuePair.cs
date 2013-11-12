﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa cada nó da árvore de cobertura mínima de um grafo.
    /// </summary>
    /// <typeparam name="VertexType">O vértice do grafo.</typeparam>
    /// <typeparam name="ValueType">O valor associado ao vértice no contexto da árvore de cobertura mínima.</typeparam>
    public class VertexValuePair<VertexType, ValueType>
    {
        /// <summary>
        /// O vértice.
        /// </summary>
        VertexType vertex;

        /// <summary>
        /// O valor que lhe está associado.
        /// </summary>
        ValueType value;

        public VertexValuePair(VertexType vertex, ValueType value)
        {
            this.vertex = vertex;
            this.value = value;
        }

        /// <summary>
        /// Obtém o vértice da árvore de pesquisa mínima.
        /// </summary>
        public VertexType Vertex
        {
            get
            {
                return this.vertex;
            }
            internal set
            {
                this.vertex = value;
            }
        }

        /// <summary>
        /// Obtém o valor da árvore de pesquisa mínima.
        /// </summary>
        public ValueType Value
        {
            get
            {
                return this.value;
            }
            internal set
            {
                this.value = value;
            }
        }
    }
}