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
    public class LinearSystemSolution<ElementType>
    {
        /// <summary>
        /// O vector que corresponde à solução particular.
        /// </summary>
        private IMatrix<ElementType> vector;

        /// <summary>
        /// A base associada ao sistema homogéneo correspondente.
        /// </summary>
        private List<IMatrix<ElementType>> vectorSpaceBasis = new List<IMatrix<ElementType>>();

        /// <summary>
        /// Obtém e atribui o vector que contém a solução particular.
        /// </summary>
        public IMatrix<ElementType> Vector
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
        public List<IMatrix<ElementType>> VectorSpaceBasis
        {
            get
            {
                return this.vectorSpaceBasis;
            }
        }
    }
}
