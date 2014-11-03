namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Contém os dados de entrada para o problema da decomposição.
    /// </summary>
    /// <typeparam name="ConstraintsType">
    /// O tipo dos obejctos que constituem as entradas das restrições.
    /// </typeparam>
    /// <typeparam name="ObjectiveCoeffType">
    /// O tipo de objectos que constituem as entradas da função objectivo.
    /// </typeparam>
    public class LinearDecompositionInput<ConstraintsType, ObjectiveCoeffType>
    {
        /// <summary>
        /// Define as arestas admmissíveis actuais.
        /// </summary>
        protected IVector<ObjectiveCoeffType>[] initialDecompositionPoints;

        /// <summary>
        /// A função objectivo.
        /// </summary>
        protected IVector<ObjectiveCoeffType> objectiveFunction;

        /// <summary>
        /// O custo actual.
        /// </summary>
        protected ObjectiveCoeffType cost;

        /// <summary>
        /// As restrições do problema principal.
        /// </summary>
        protected LinearConstraintsInput<ConstraintsType> masterConstraints;

        /// <summary>
        /// As restrições dos problemas decompostos.
        /// </summary>
        protected LinearConstraintsInput<ConstraintsType>[] decomposedConstraints;

        /// <summary>
        /// A matriz de base inversa que é utilizada durante o processo algorítmico.
        /// </summary>
        protected ISquareMatrix<ConstraintsType> inverseBasisMatrix;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="LinearDecompositionInput{ConstraintsType, ObjectiveCoeffType}"/>
        /// </summary>
        /// <param name="initialDecompositionPoints">As estimativas iniciais relativas ao problema da decomposição.</param>
        /// <param name="objectiveFunction">A função objectivo.</param>
        /// <param name="cost">O custo inicial.</param>
        /// <param name="masterConstraints">As restrições do problema principal.</param>
        /// <param name="decomposedConstraints">As restrições dos vários problemas decompostos.</param>
        /// <param name="inverseBasisMatrix">A matriz de base inversa.</param>
        public LinearDecompositionInput(
            IVector<ObjectiveCoeffType>[] initialDecompositionPoints,
            IVector<ObjectiveCoeffType> objectiveFunction,
            ObjectiveCoeffType cost,
            LinearConstraintsInput<ConstraintsType> masterConstraints,
            LinearConstraintsInput<ConstraintsType>[] decomposedConstraints,
            ISquareMatrix<ConstraintsType> inverseBasisMatrix)
        {
            this.ValidateArguments(
                initialDecompositionPoints,
                objectiveFunction,
                cost,
                masterConstraints,
                decomposedConstraints,
                inverseBasisMatrix);
            this.initialDecompositionPoints = initialDecompositionPoints;
            this.objectiveFunction = objectiveFunction;
            this.cost = cost;
            this.masterConstraints = masterConstraints;
            this.decomposedConstraints = decomposedConstraints;
            this.inverseBasisMatrix = inverseBasisMatrix;
        }

        /// <summary>
        /// Obtém ou atribui as arestas admmissíveis actuais.
        /// </summary>
        public IVector<ObjectiveCoeffType>[] InitialDecompositionPoints
        {
            get
            {
                return this.initialDecompositionPoints;
            }
        }

        /// <summary>
        /// Obtém ou atribui a função objectivo.
        /// </summary>
        public IVector<ObjectiveCoeffType> ObjectiveFunction
        {
            get
            {
                return this.objectiveFunction;
            }
        }

        /// <summary>
        /// Obtém ou atribui o custo actual.
        /// </summary>
        public ObjectiveCoeffType Cost
        {
            get
            {
                return this.cost;
            }
        }

        /// <summary>
        /// Obtém ou atribui as restrições do problema principal.
        /// </summary>
        public LinearConstraintsInput<ConstraintsType> MasterConstraints
        {
            get
            {
                return this.masterConstraints;
            }
        }

        /// <summary>
        /// Obtém ou atribui as restrições dos problemas decompostos.
        /// </summary>
        public LinearConstraintsInput<ConstraintsType>[] DecomposedConstraints
        {
            get
            {
                return this.decomposedConstraints;
            }
        }

        /// <summary>
        /// Obtém ou atribui a matriz de base inversa que é utilizada durante o processo algorítmico.
        /// </summary>
        public ISquareMatrix<ConstraintsType> InverseBasisMatrix
        {
            get
            {
                return this.inverseBasisMatrix;
            }
        }

        /// <summary>
        /// Valida a integridade dos dados nos argumentos.
        /// </summary>
        /// <param name="initialDecompositionPoints">As estimativas iniciais relativas ao problema da decomposição.</param>
        /// <param name="objectiveFunction">A função objectivo.</param>
        /// <param name="cost">O custo inicial.</param>
        /// <param name="masterConstraints">As restrições do problema principal.</param>
        /// <param name="decomposedConstraints">As restrições dos vários problemas decompostos.</param>
        /// <param name="inverseBasisMatrix">A matriz de base inversa.</param>
        private void ValidateArguments(
            IVector<ObjectiveCoeffType>[] initialDecompositionPoints,
            IVector<ObjectiveCoeffType> objectiveFunction,
            ObjectiveCoeffType cost,
            LinearConstraintsInput<ConstraintsType> masterConstraints,
            LinearConstraintsInput<ConstraintsType>[] decomposedConstraints,
            ISquareMatrix<ConstraintsType> inverseBasisMatrix)
        {
            if (initialDecompositionPoints == null)
            {
                throw new ArgumentNullException("initialDecompositionPoints");
            }
            else if (objectiveFunction == null)
            {
                throw new ArgumentNullException("objectiveFunction");
            }
            else if (cost == null)
            {
                throw new ArgumentNullException("cost");
            }
            else if (masterConstraints == null)
            {
                throw new ArgumentNullException("masterConstraints");
            }
            else if (decomposedConstraints == null)
            {
                throw new ArgumentNullException("decomposedConstraints");
            }
            else if (inverseBasisMatrix == null)
            {
                throw new ArgumentNullException("inverseBasisMatrix");
            }
            else if (initialDecompositionPoints.Length != decomposedConstraints.Length)
            {
                throw new ArgumentException(
                    "The number of initial problem points must match the number of decomposition problem constraints.");
            }
            else
            {
                var length = decomposedConstraints.Length;
                for (int i = 0; i < length; ++i)
                {
                    var currentDecomposedConstraint = decomposedConstraints[i];
                    var currentPoint = initialDecompositionPoints[i];
                    if (currentDecomposedConstraint == null)
                    {
                        throw new ArgumentException("Null decomposition constraints aren't allowed.");
                    }
                    else if (currentPoint == null)
                    {
                        throw new ArgumentException("Null initial points aren't allowed.");
                    }
                    else
                    {
                        var constraintColumnsLength = currentDecomposedConstraint.ConstraintsMatrix.GetLength(1);
                        if (initialDecompositionPoints.Length != constraintColumnsLength)
                        {
                            throw new ArgumentException(
                                "The number of initial points coordinates must match the number of columns of the decomposition problem constraints matrix.");
                        }
                    }
                }
            }
        }
    }
}
