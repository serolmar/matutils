using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.Algorithms
{
    /// <summary>
    /// Revised simplex method using <see cref="double"/> as variable type. 
    /// The function run will alter all input variable status.
    /// </summary>
    public class RevisedSimplexWithDoubles
    {
        #region Fields
        private int[] nonBasicVariables;
        private int[] basicVariables;
        private double[] objectiveFunctionVector;
        private double[,] constraintsMatrix;
        private double[] constraintsVector;
        private double[,] inverseBasisMatrix;
        private double[] slackVariablesInObjectiveFunction;
        private bool hasRunned = false;
        private double bestCost;
        private double[] solution;
        #endregion

        public RevisedSimplexWithDoubles(
            double[] objectiveVector,
            double[] constraintsVector,
            double[,] constraintsMatrix
            )
        {
            this.ValidateArguments(objectiveVector, constraintsVector, constraintsMatrix);
            this.InitializeVariables(objectiveVector, constraintsVector, constraintsMatrix);
        }

        public double BestCost
        {
            get
            {
                if (!this.hasRunned)
                {
                    throw new Exception("Algorithm hasn't runned yet.");
                }

                return this.bestCost;
            }
        }

        public double this[int solutionIndex]
        {
            get
            {
                if (!this.hasRunned)
                {
                    throw new Exception("Algorithm hasn't runned yet.");
                }

                if (solutionIndex < 0 || solutionIndex >= this.nonBasicVariables.Length)
                {
                    throw new ArgumentOutOfRangeException("solutionIndex");
                }

                return this.solution[solutionIndex];
            }
        }

        public int SolutionVariablesLength
        {
            get
            {
                return this.nonBasicVariables.Length;
            }
        }

        public void Run()
        {
            bool state = true;
            int nonBasicCount = this.nonBasicVariables.Length;
            int basicCount = this.basicVariables.Length;
            int i, j, k;
            int enteringBasicVariable = 0;
            double[] etaVector = new double[basicCount];
            double temp;

            // Check entering basic variables and if solution is optimal
            double minimumCost = objectiveFunctionVector[0];
            for (i = 1; i < nonBasicCount; ++i)
            {
                if (objectiveFunctionVector[i] < minimumCost)
                {
                    enteringBasicVariable = i;
                    minimumCost = objectiveFunctionVector[i];
                }
            }

            if (minimumCost >= 0)
            {
                state = false;
            }
            else
            {
                for (i = 0; i < basicCount; ++i)
                {
                    temp = 0;
                    for (j = 0; j < basicCount; ++j)
                    {
                        temp += this.inverseBasisMatrix[i, j] * this.constraintsMatrix[j, enteringBasicVariable];
                    }

                    etaVector[i] = temp;
                }
            }

            double[] objectiveTest = new double[nonBasicCount];
            for (i = 0; i < nonBasicCount; ++i)
            {
                objectiveTest[i] = this.objectiveFunctionVector[i];
            }

            double[] checkConstraintVector = new double[basicCount];
            for (i = 0; i < basicCount; ++i)
            {
                checkConstraintVector[i] = this.constraintsVector[i];
            }

            while (state)
            {
                // Check leaving basic variables
                double aux = etaVector[0];
                temp = aux > 0 ? checkConstraintVector[0] / aux : double.MaxValue;
                int leavingBasicVariable = 0;
                for (i = 1; i < basicCount; ++i)
                {
                    aux = etaVector[i];
                    aux = aux > 0 ? checkConstraintVector[i] / aux : double.MaxValue;
                    if (aux < temp)
                    {
                        leavingBasicVariable = i;
                        temp = aux;
                    }
                }

                if (this.basicVariables[leavingBasicVariable] > nonBasicCount)
                {
                    this.slackVariablesInObjectiveFunction[leavingBasicVariable] = -this.objectiveFunctionVector[enteringBasicVariable];
                }
                else if (this.nonBasicVariables[enteringBasicVariable] < nonBasicCount)
                {
                    this.slackVariablesInObjectiveFunction[this.nonBasicVariables[enteringBasicVariable]] = 0;
                }

                // Computing the new inverse matrix
                double[,] newInverse = new double[basicCount, basicCount];
                for (i = 0; i < basicCount; ++i)
                {
                    newInverse[i, i] = 1;
                }

                temp = etaVector[leavingBasicVariable];
                for (i = 0; i < basicCount; ++i)
                {
                    if (i != leavingBasicVariable)
                    {
                        newInverse[i, leavingBasicVariable] = -etaVector[i] / temp;
                    }
                    else
                    {
                        newInverse[i, leavingBasicVariable] = 1 / temp;
                    }
                }

                double[,] inverse = new double[basicCount, basicCount];
                for (i = 0; i < basicCount; ++i)
                {
                    for (j = 0; j < basicCount; ++j)
                    {
                        aux = 0;
                        for (k = 0; k < basicCount; ++k)
                        {
                            aux += newInverse[i, k] * this.inverseBasisMatrix[k, j];
                        }

                        inverse[i, j] = aux;
                    }
                }

                // Exchange variables and updates variables
                i = this.nonBasicVariables[enteringBasicVariable];
                this.nonBasicVariables[enteringBasicVariable] = this.basicVariables[leavingBasicVariable];
                this.basicVariables[leavingBasicVariable] = i;
                this.inverseBasisMatrix = inverse;

                double[] multiplyTemp = new double[basicCount];
                for (i = 0; i < basicCount; ++i)
                {
                    temp = 0;
                    for (j = 0; j < basicCount; ++j)
                    {
                        temp += this.slackVariablesInObjectiveFunction[j] * inverse[j, i];
                    }

                    multiplyTemp[i] = temp;
                }

                // Compute new objective coefficients
                for (i = 0; i < nonBasicCount; ++i)
                {
                    if (this.nonBasicVariables[i] < nonBasicCount)
                    {
                        temp = 0;
                        for (j = 0; j < basicCount; ++j)
                        {
                            temp += multiplyTemp[j] * this.constraintsMatrix[j, this.nonBasicVariables[i]];
                        }

                        objectiveTest[i] = this.objectiveFunctionVector[this.nonBasicVariables[i]] + temp;

                    }
                    else
                    {
                        objectiveTest[i] = multiplyTemp[this.nonBasicVariables[i] - nonBasicCount];
                    }
                }

                // Check entering basic variables and if solution is optimal
                minimumCost = objectiveTest[0];
                enteringBasicVariable = 0;
                for (i = 1; i < nonBasicCount; ++i)
                {
                    if (objectiveTest[i] < minimumCost)
                    {
                        enteringBasicVariable = i;
                        minimumCost = objectiveTest[i];
                    }
                }

                if (minimumCost >= 0)
                {
                    state = false;
                }
                else
                {
                    if (this.nonBasicVariables[enteringBasicVariable] < nonBasicCount)
                    {
                        for (i = 0; i < basicCount; ++i)
                        {
                            temp = 0;
                            for (j = 0; j < basicCount; ++j)
                            {
                                temp += inverse[i, j] * this.constraintsMatrix[j, this.nonBasicVariables[enteringBasicVariable]];  //?
                            }

                            etaVector[i] = temp;
                        }
                    }
                    else
                    {
                        for (i = 0; i < basicCount; ++i)
                        {
                            for (j = 0; j < basicCount; ++j)
                            {
                                etaVector[i] = inverse[i, this.nonBasicVariables[enteringBasicVariable] - nonBasicCount];
                            }
                        }
                    }
                }

                for (i = 0; i < basicCount; ++i)
                {
                    temp = 0;
                    for (j = 0; j < basicCount; ++j)
                    {
                        temp += this.inverseBasisMatrix[i, j] * this.constraintsVector[j];
                    }

                    checkConstraintVector[i] = temp;
                }
            }  // While

            checkConstraintVector.CopyTo(this.constraintsVector, 0);
            for (i = 0; i < this.basicVariables.Length; ++i)
            {
                if (this.basicVariables[i] < this.nonBasicVariables.Length)
                {
                    this.bestCost += this.constraintsVector[i] * (-this.objectiveFunctionVector[basicVariables[i]]);
                }
            }

            this.solution = new double[nonBasicCount];
            for (i = 0; i < this.basicVariables.Length; ++i)
            {
                if (this.basicVariables[i] < this.nonBasicVariables.Length)
                {
                    this.solution[this.basicVariables[i]] = checkConstraintVector[this.basicVariables[i]];
                }
            }

            this.hasRunned = true;
        }

        private void ValidateArguments(
            double[] objectiveVector,
            double[] constraintsVector,
            double[,] constraintsMatrix)
        {
            if (objectiveVector == null)
            {
                throw new ArgumentNullException("objectiveVector");
            }

            if (constraintsVector == null)
            {
                throw new ArgumentNullException("constraintsVector");
            }

            if (constraintsMatrix == null)
            {
                throw new ArgumentNullException("constraintsMatrix");
            }

            if (constraintsMatrix.GetLength(0) != constraintsVector.Length)
            {
                throw new Exception("Constraints matriz must have as many lines as constraints vector has elements.");
            }

            if (constraintsMatrix.GetLength(1) != objectiveVector.Length)
            {
                throw new Exception("Constraints matrix must have as many columns as objective vector has elements.");
            }
        }

        private void InitializeVariables(double[] objectiveVector,
            double[] constraintsVector,
            double[,] constraintsMatrix)
        {
            this.constraintsVector = new double[constraintsVector.Length];
            Array.Copy(constraintsVector, this.constraintsVector, constraintsVector.Length);
            this.nonBasicVariables = new int[objectiveVector.Length];
            int i = 0;
            for (; i < objectiveVector.Length; ++i)
            {
                this.nonBasicVariables[i] = i;
            }

            this.basicVariables = new int[constraintsVector.Length];
            for (int j = 0; j < constraintsVector.Length; ++j)
            {
                this.basicVariables[j] = j + i;
            }

            this.objectiveFunctionVector = new double[objectiveVector.Length];
            for (i = 0; i < objectiveVector.Length; ++i)
            {
                this.objectiveFunctionVector[i] = -objectiveVector[i];
            }

            this.slackVariablesInObjectiveFunction = new double[constraintsVector.Length];

            this.constraintsMatrix = constraintsMatrix;

            this.inverseBasisMatrix = new double[constraintsVector.Length, constraintsVector.Length];
            for (i = 0; i < constraintsVector.Length; ++i)
            {
                this.inverseBasisMatrix[i, i] = 1;
            }
        }
    }
}
