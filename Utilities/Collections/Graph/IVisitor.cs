namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define os objectos capazes de realizar uma visita a uma árvore.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos associados aos nós.</typeparam>
    public interface IVisitor<in ObjectType>
    {
        /// <summary>
        /// Visitador de um objecto para ser utilizado durante as vissitas
        /// a uma árvore ou a um grafo.
        /// </summary>
        /// <param name="objectToBeVisited">O objecto a ser visitado.</param>
        void Visit(ObjectType objectToBeVisited);
    }
}
