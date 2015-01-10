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
    /// <typeparam name="EdgeValueType">O tipo de objectos associados às arestas.</typeparam>
    public class GraphCyclesComponentsPair<VertexType, EdgeValueType>
    {
        /// <summary>
        /// Mantém a lista de componentes.
        /// </summary>
        private List<IGraph<VertexType, EdgeValueType>> components;

        /// <summary>
        /// A lista de ciclos simples obtidos por componente.
        /// </summary>
        private List<List<IGraph<VertexType, EdgeValueType>>> cyclesPerComponent;

        /// <summary>
        /// Instancia um novo obejcto do tipo <see cref="GraphCyclesComponentsPair{VertexType, EdgeValueType}"/>.
        /// </summary>
        /// <param name="components">A lista de componentes.</param>
        /// <param name="cyclesPerComponent">O conjunto de ciclos por componente.</param>
        public GraphCyclesComponentsPair(
            List<IGraph<VertexType, EdgeValueType>> components,
            List<List<IGraph<VertexType, EdgeValueType>>> cyclesPerComponent)
        {
            this.components = components;
            this.cyclesPerComponent = cyclesPerComponent;
        }

        /// <summary>
        /// Obtém a lista de componentes.
        /// </summary>
        /// <value>A lista de componentes.</value>
        public ReadOnlyCollection<IGraph<VertexType, EdgeValueType>> Components
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
        public ReadOnlyCollection<ReadOnlyCollection<IGraph<VertexType, EdgeValueType>>> CyclesPerComponent
        {
            get
            {
                var result = new List<ReadOnlyCollection<IGraph<VertexType, EdgeValueType>>>();
                foreach (var cyclePerComponent in this.cyclesPerComponent)
                {
                    result.Add(cyclePerComponent.AsReadOnly());
                }

                return result.AsReadOnly();
            }
        }
    }
}
