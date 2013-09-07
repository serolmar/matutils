namespace Mathematics.Algorithms.Simplex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Representa a entrada para o algoritmo do simplex quando este se encontra escrito
    /// na sua forma normalizada, isto é, aquando da introdução das variáveis de folga.
    /// </summary>
    /// <typeparam name="ConstraintsType">O tipo associado aos coeficientes.</typeparam>
    /// <typeparam name="ObjectiveCoeffType">O tipo associado aos coeficientes da função objectivo.</typeparam>
    public class SimplexInput<ConstraintsType, ObjectiveCoeffType>
    {
        /// <summary>
        /// As variáveis básicas.
        /// </summary>
        private int[] basicVariables;

        /// <summary>
        /// As variáveis não básicas.
        /// </summary>
        private int[] nonBasicVariables;

        /// <summary>
        /// A função objectivo.
        /// </summary>
        private IMatrix<ObjectiveCoeffType> objectiveFunction;

        /// <summary>
        /// A matriz das restrições.
        /// </summary>
        private IMatrix<ConstraintsType> constraintsMatrix;

        /// <summary>
        /// O vector das restrições.
        /// </summary>
        private IMatrix<ConstraintsType> constraintsVector;

        public SimplexInput(
            int[] basicVariables,
            int[] nonBasicVariables,
            IMatrix<ObjectiveCoeffType> objectiveFunction,
            IMatrix<ConstraintsType> constraintsMatrix,
            IMatrix<ConstraintsType> constraintsVector)
        {
            if (basicVariables == null)
            {
                throw new ArgumentNullException("basicVariables");
            }
            else if (nonBasicVariables == null)
            {
                throw new ArgumentNullException("nonBasicVariables");
            }
            else if (objectiveFunction == null)
            {
                throw new ArgumentNullException("objectiveFunction");
            }
            else if (constraintsMatrix == null)
            {
                throw new ArgumentNullException("constraintsMatrix");
            }
            else if (constraintsVector == null)
            {
                throw new ArgumentNullException("constraintsVector");
            }
            else
            {
                if (objectiveFunction.GetLength(0) != 1)
                {
                    throw new ArgumentException("Objective function must be a column vector.");
                }
                else if (constraintsVector.GetLength(0) != 1)
                {
                    throw new ArgumentException("Constraints vector must be a column vector.");
                }
                else if (constraintsMatrix.GetLength(0) != constraintsVector.GetLength(0))
                {
                    throw new ArgumentException("Constraints matrix must have the same number of lines as constraints vector.");
                }
                else if (basicVariables.Length + nonBasicVariables.Length != constraintsMatrix.GetLength(1))
                {
                    throw new ArgumentException("The number of variables must be equal to the number of columns in constraints matrix.");
                }
                else if (nonBasicVariables.Length != objectiveFunction.GetLength(0))
                {
                    throw new ArgumentException("The number of non basic variables must be equal to the number of coefficients in objective funcion.");
                }

                this.basicVariables = basicVariables;
                this.nonBasicVariables = nonBasicVariables;
                this.objectiveFunction = objectiveFunction;
                this.constraintsMatrix = constraintsMatrix;
                this.constraintsVector = constraintsVector;
            }
        }

        /// <summary>
        /// Obtém as variáveis básicas.
        /// </summary>
        public int[] BasicVariables
        {
            get
            {
                return this.basicVariables;
            }
        }

        /// <summary>
        /// Obtém as variáveis não básicas.
        /// </summary>
        public int[] NonBasicVariables
        {
            get
            {
                return this.nonBasicVariables;
            }
        }

        /// <summary>
        /// Obtém a função objectivo.
        /// </summary>
        public IMatrix<ObjectiveCoeffType> ObjectiveFunction
        {
            get
            {
                return this.objectiveFunction;
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
        /// Obtém o vector das restrições.
        /// </summary>
        public IMatrix<ConstraintsType> ConstraintsVector
        {
            get
            {
                return this.constraintsVector;
            }
        }
    }
}
