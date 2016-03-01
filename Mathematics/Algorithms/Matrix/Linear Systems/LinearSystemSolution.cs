namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa a solução de um sistema de equações lineares.
    /// </summary>
    /// <remarks>
    /// A solução de um sistema de equações lineares resume-se ao vector particular da solução
    /// e uma base do espaço gerado pelas soluções da equação homogénea associada.
    /// </remarks>
    /// <typeparam name="ElementType">O tipo de objectos que constituem os coeficientes do sistema linear.</typeparam>
    public class LinearSystemSolution<ElementType>
    {
        /// <summary>
        /// O vector que corresponde à solução particular.
        /// </summary>
        private IMathVector<ElementType> vector;

        /// <summary>
        /// A base associada ao sistema homogéneo correspondente.
        /// </summary>
        private List<IMathVector<ElementType>> vectorSpaceBasis = new List<IMathVector<ElementType>>();

        /// <summary>
        /// Obtém e atribui o vector que contém a solução particular.
        /// </summary>
        /// <value>O vector que contém a solução particular.</value>
        public IMathVector<ElementType> Vector
        {
            get
            {
                return this.vector;
            }
            set
            {
                this.vector = value;
            }
        }

        /// <summary>
        /// Obtém uma base para o espaço vectorial definido pelas soluções do sistema homogéneo
        /// correspondente.
        /// </summary>
        /// <value>A base.</value>
        public List<IMathVector<ElementType>> VectorSpaceBasis
        {
            get
            {
                return this.vectorSpaceBasis;
            }
        }
    }
}
