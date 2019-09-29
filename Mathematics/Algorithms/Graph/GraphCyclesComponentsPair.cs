namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Mantém um par de componentes conexas e respectivos ciclos classificados por componente.
    /// </summary>
    /// <typeparam name="VertexType">O tipo de objectos que constituem os vértices.</typeparam>
    /// <typeparam name="EdgeType">O tipo de objectos que constituem as arestas.</typeparam>
    /// <typeparam name="GraphType">O tipo dos objectos que constiuem os grafos.</typeparam>
    public class GraphCyclesComponentsPair<VertexType, EdgeType, GraphType>
        where EdgeType : IEdge<VertexType>
        where GraphType : IEdgeGraph<VertexType, EdgeType>
    {
        /// <summary>
        /// Mantém a lista de componentes.
        /// </summary>
        private List<GraphType> components;

        /// <summary>
        /// A lista de ciclos simples obtidos por componente.
        /// </summary>
        private List<List<GraphType>> cyclesPerComponent;

        /// <summary>
        /// Instancia um novo obejcto do tipo <see cref="GraphCyclesComponentsPair{VertexType, EdgeType, GraphType}"/>.
        /// </summary>
        /// <param name="components">A lista de componentes.</param>
        /// <param name="cyclesPerComponent">O conjunto de ciclos por componente.</param>
        public GraphCyclesComponentsPair(
            List<GraphType> components,
            List<List<GraphType>> cyclesPerComponent)
        {
            this.components = components;
            this.cyclesPerComponent = cyclesPerComponent;
        }

        /// <summary>
        /// Obtém a lista de componentes.
        /// </summary>
        /// <value>A lista de componentes.</value>
        public ReadOnlyCollection<GraphType> Components
        {
            get
            {
                return this.components.AsReadOnly();
            }
        }

        /// <summary>
        /// Obtém a lista de ciclos simples distribuídos por componente.
        /// </summary>
        /// <value>A lista de ciclos.</value>
        public ReadOnlyCollection<ReadOnlyCollection<GraphType>> CyclesPerComponent
        {
            get
            {
                var result = new List<ReadOnlyCollection<GraphType>>();
                foreach (var cyclePerComponent in this.cyclesPerComponent)
                {
                    result.Add(cyclePerComponent.AsReadOnly());
                }

                return result.AsReadOnly();
            }
        }
    }
}
