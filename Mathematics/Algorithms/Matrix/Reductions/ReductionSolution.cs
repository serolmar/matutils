namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ReductionSolution<VectorType, FieldCoeffType, GroupCoeffType>
    {
        /// <summary>
        /// O conjunto dos vectores reduzidos.
        /// </summary>
        private List<VectorType> reducedVectors;

        /// <summary>
        /// Os coeficientes dos quais advém a redução encontrada.
        /// </summary>
        private IMatrix<GroupCoeffType> reductionCoefficients;

        /// <summary>
        /// Obtém e atribui os vectores reduzidos.
        /// </summary>
        /// <remarks>
        /// Note-se que, neste caso, não é necessária a introdução de um vector geométrico. Podemos ter,
        /// por exemplo, polinómios como elementos de um espaço vectorial.
        /// </remarks>
        public List<VectorType> ReducedVectors
        {
            get
            {
                return this.reducedVectors;
            }
            set
            {
                this.reducedVectors = value;
            }
        }

        /// <summary>
        /// Obtém e atribui os coeficientes dos quais advém a redução encontrada.
        /// </summary>
        private IMatrix<GroupCoeffType> ReductionCoefficients
        {
            get
            {
                return this.reductionCoefficients;
            }
            set
            {
                this.reductionCoefficients = value;
            }
        }
    }
}
