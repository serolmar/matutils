namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa a entrada para o algoritmo do simplex revisto quando este se encontra representado na sua forma
    /// normalizada.
    /// </summary>
    /// <typeparam name="ConstraintsType">O tipo dos objectos que contituem as entradas das restrições.</typeparam>
    /// <typeparam name="ObjectiveCoeffType">O tipo de objectos que constituem as entradas da função objectivo.</typeparam>
    public class RevisedSimplexInput<ConstraintsType, ObjectiveCoeffType>
        : SimplexInput<ConstraintsType, ObjectiveCoeffType>
    {
        /// <summary>
        /// A matriz de base inversa que é utilizada durante o processo algorítmico.
        /// </summary>
        protected ISquareMatrix<ConstraintsType> inverseBasisMatrix;

        /// <summary>
        /// Permite criar uma instância de entrada para o algoritmo do simplex na forma normal. 
        /// A base pode corresponder a uma instância que resultou da aplicação do algoritmo.
        /// </summary>
        /// <param name="basicVariables">O conjunto de variáveis básicas.</param>
        /// <param name="nonBasicVariables">O conjunto de variáveis não-básicas.</param>
        /// <param name="objectiveFunction">Os coeficientes da função objectivo.</param>
        /// <param name="cost">O custo.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <param name="inverseBasisMatrix">A matriz de base inversa que é utilizada durante o processo algorítmico.</param>
        /// /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        /// <exception cref="ArgumentException">
        /// Se as dimensões dos argumentos não definirem correctamente um problema.
        /// </exception>
        public RevisedSimplexInput(
            int[] basicVariables,
            int[] nonBasicVariables,
            IVector<ObjectiveCoeffType> objectiveFunction,
            ObjectiveCoeffType cost,
            IMatrix<ConstraintsType> constraintsMatrix,
            IVector<ConstraintsType> constraintsVector,
            ISquareMatrix<ConstraintsType> inverseBasisMatrix)
            : base(
                basicVariables,
                nonBasicVariables,
                objectiveFunction,
                cost,
                constraintsMatrix,
                constraintsVector)
        {
            if (inverseBasisMatrix == null)
            {
                throw new ArgumentNullException("The basis matrix must not be null.");
            }
            else if (inverseBasisMatrix.GetLength(0) != ConstraintsMatrix.GetLength(0))
            {
                throw new ArgumentException("The dimension of basis matrix must match the number of lines in constraints matrix.");
            }
            else
            {
                this.inverseBasisMatrix = inverseBasisMatrix;
            }
        }

        /// <summary>
        /// Obtém a matriz de base inversa que é utilizada no processo algorítmico.
        /// </summary>
        public ISquareMatrix<ConstraintsType> InverseBasisMatrix
        {
            get
            {
                return this.inverseBasisMatrix;
            }
        }
    }
}
