namespace Mathematics
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
        protected int[] basicVariables;

        /// <summary>
        /// As variáveis não básicas.
        /// </summary>
        protected int[] nonBasicVariables;

        /// <summary>
        /// A função objectivo.
        /// </summary>
        protected IVector<ObjectiveCoeffType> objectiveFunction;

        /// <summary>
        /// O custo actual.
        /// </summary>
        protected ObjectiveCoeffType cost;

        /// <summary>
        /// A matriz das restrições.
        /// </summary>
        protected IMatrix<ConstraintsType> constraintsMatrix;

        /// <summary>
        /// O vector das restrições.
        /// </summary>
        protected IVector<ConstraintsType> constraintsVector;

        /// <summary>
        /// Permite criar uma instância de entrada para o algoritmo do simplex na forma normal de minimização. 
        /// Esta instância poderá corresponder a um estado intermédio deste algoritmo.
        /// </summary>
        /// <param name="basicVariables">O conjunto de variáveis básicas.</param>
        /// <param name="nonBasicVariables">O conjunto de variáveis não-básicas.</param>
        /// <param name="objectiveFunction">Os coeficientes da função objectivo.</param>
        /// <param name="cost">O custo.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        /// <exception cref="ArgumentException">
        /// Se as dimensões dos argumentos não definirem correctamente um problema.
        /// </exception>
        public SimplexInput(
            int[] basicVariables,
            int[] nonBasicVariables,
            IVector<ObjectiveCoeffType> objectiveFunction,
            ObjectiveCoeffType cost,
            IMatrix<ConstraintsType> constraintsMatrix,
            IVector<ConstraintsType> constraintsVector)
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
            else if (cost == null)
            {
                throw new ArgumentNullException("cost");
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
                if (constraintsMatrix.GetLength(0) != constraintsVector.Length)
                {
                    throw new ArgumentException(
                        "Constraints matrix must have the same number of lines as constraints vector.");
                }
                else if (nonBasicVariables.Length != constraintsMatrix.GetLength(1))
                {
                    throw new ArgumentException(
                        "The number of variables must be equal to the number of columns in constraints matrix.");
                }
                else if (nonBasicVariables.Length != objectiveFunction.Length)
                {
                    throw new ArgumentException(
                        "The number of non basic variables must be equal to the number of coefficients in objective funcion.");
                }
                else
                {
                    // Verifica a validade dos índices contidos nas variáveis básicas e não básicas.
                    this.CheckVariablesIndices(basicVariables, nonBasicVariables);
                    this.basicVariables = basicVariables;
                    this.nonBasicVariables = nonBasicVariables;
                    this.objectiveFunction = objectiveFunction;
                    this.cost = cost;
                    this.constraintsMatrix = constraintsMatrix;
                    this.constraintsVector = constraintsVector;
                }
            }
        }

        /// <summary>
        /// Obtém as variáveis básicas.
        /// </summary>
        /// <value>As variávei básicas.</value>
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
        /// <value>As variáveis não básicas.</value>
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
        /// <value>A função objectivo.</value>
        public IVector<ObjectiveCoeffType> ObjectiveFunction
        {
            get
            {
                return this.objectiveFunction;
            }
        }

        /// <summary>
        /// Obtém e atribui o custo actual.
        /// </summary>
        /// <value>O custo.</value>
        public ObjectiveCoeffType Cost
        {
            get
            {
                return this.cost;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Cost can't be a null value.");
                }
                else
                {
                    this.cost = value;
                }
            }
        }

        /// <summary>
        /// Obtém a matriz das restrições.
        /// </summary>
        /// <value>A martiz das restrições.</value>
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
        /// <value>O vector dos coeficientes independentes das restrições.</value>
        public IVector<ConstraintsType> ConstraintsVector
        {
            get
            {
                return this.constraintsVector;
            }
        }

        /// <summary>
        /// Averigua se os índices contidos nas variáveis básicas e não-básicas estão correctos.
        /// </summary>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <param name="nonBasicVariables">As variáveis não básicas.</param>
        /// <exception cref="MathematicsException">Se os índices não estiverem conformes.</exception>
        private void CheckVariablesIndices(int[] basicVariables, int[] nonBasicVariables)
        {
            var sortedIndices = new SortedSet<int>(Comparer<int>.Default);
            for (int i = 0; i < basicVariables.Length; ++i)
            {
                sortedIndices.Add(i);
            }

            for (int i = 0; i < nonBasicVariables.Length; ++i)
            {
                sortedIndices.Add(i);
            }

            var currentIndex = 0;
            foreach (var index in sortedIndices)
            {
                if (index == currentIndex)
                {
                    ++currentIndex;
                }
                else
                {
                    throw new MathematicsException("The indices in basic and non-basica variables are invalid.");
                }
            }
        }
    }
}
