namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um conjunto de restrições lineares em igualdades.
    /// </summary>
    /// <typeparam name="ConstraintsType">
    /// O tipo dos objectos que constituem as entradas das restrições.
    /// </typeparam>
    public class LinearConstraintsInput<ConstraintsType>
    {
        /// <summary>
        /// A matriz das restrições.
        /// </summary>
        private IMatrix<ConstraintsType> constraintsMatrix;

        /// <summary>
        /// O vector dos coeficientes independentes das restrições.
        /// </summary>
        private IVector<ConstraintsType> constraintsVector;

        /// <summary>
        /// Cria uma nova instância de um objecto do tipo <see cref="LinearConstraintsInput{ConstraintsType}"/>.
        /// </summary>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector dos coeficientes independentes das restrições.</param>
        public LinearConstraintsInput(
            IMatrix<ConstraintsType> constraintsMatrix,
            IVector<ConstraintsType> constraintsVector)
        {
            if (constraintsMatrix == null)
            {
                throw new ArgumentNullException("constraintsMatrix");
            }
            else if (constraintsVector == null)
            {
                throw new ArgumentNullException("constraintsVector");
            }
            else if (constraintsMatrix.GetLength(1) != constraintsVector.Length)
            {
                throw new ArgumentException("The vector length must match the constraints matrix second dimension.");
            }
            else
            {
                this.constraintsMatrix = constraintsMatrix;
                this.constraintsVector = constraintsVector;
            }
        }

        /// <summary>
        /// Obtém a matriz das restrições.
        /// </summary>
        public IMatrix<ConstraintsType> ConstraintsMatrix
        {
            get
            {
                return this.constraintsMatrix;
            }
        }

        /// <summary>
        /// Obtém o vector dos coeficientes independentes das restrições.
        /// </summary>
        private IVector<ConstraintsType> ConstraintsVector
        {
            get
            {
                return this.constraintsVector;
            }
        }
    }
}
