﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Mantém um par de componentes conexas e respectivos ciclos classificados por componente.
    /// </summary>
    public class GraphCyclesComponentsPair<VertexType>
    {
        /// <summary>
        /// Mantém a lista de componentes.
        /// </summary>
        private List<IGraph<VertexType>> components;

        /// <summary>
        /// A lista de ciclos simples obtidos por componente.
        /// </summary>
        private List<List<IGraph<VertexType>>> cyclesPerComponent;

        public GraphCyclesComponentsPair(
            List<IGraph<VertexType>> components,
            List<List<IGraph<VertexType>>> cyclesPerComponent)
        {
            this.components = components;
            this.cyclesPerComponent = cyclesPerComponent;
        }

        /// <summary>
        /// Obtém a lista de componentes.
        /// </summary>
        public ReadOnlyCollection<IGraph<VertexType>> Components
        {
            get
            {
                return this.components.AsReadOnly();
            }
        }

        /// <summary>
        /// Obtém a lista de ciclos simples distribuídos por componente.
        /// </summary>
        public ReadOnlyCollection<ReadOnlyCollection<IGraph<VertexType>>> CyclesPerComponent
        {
            get
            {
                var result = new List<ReadOnlyCollection<IGraph<VertexType>>>();
                foreach (var cyclePerComponent in this.cyclesPerComponent)
                {
                    result.Add(cyclePerComponent.AsReadOnly());
                }

                return result.AsReadOnly();
            }
        }
    }
}
