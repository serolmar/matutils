namespace Mathematics.Algorithms.Simplex
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SimplexAlgorithm<CoeffType> :
        IAlgorithm<SimplexInput<CoeffType, CoeffType>, SimplexOutput<CoeffType>>
    {
        /// <summary>
        /// O objecto responsável pelo bloqueio dos processos.
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// O objecto responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IField<CoeffType> coeffsField;

        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IComparer<CoeffType> coeffsComparer;

        public SimplexAlgorithm(IComparer<CoeffType> coeffsComparer, IField<CoeffType> coeffsField)
        {
            if (coeffsField == null)
            {
                throw new ArgumentNullException("coeffsField");
            }
            else if (coeffsComparer == null)
            {
                throw new ArgumentNullException("coeffsComparer");
            }
            else
            {
                this.coeffsComparer = coeffsComparer;
                this.coeffsField = coeffsField;
            }
        }

        /// <summary>
        /// Executa o algoritmo do simplex habitual sobre os dados de entrada.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>O resultado da execução.</returns>
        public SimplexOutput<CoeffType> Run(SimplexInput<CoeffType, CoeffType> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var hasSolution = true;
                var enteringVariable = this.GetNextEnteringVariable(data);
                var state = enteringVariable != -1;
                var cost = this.coeffsField.AdditiveUnity;
                while (state)
                {
                    var leavingVariable = this.GetNextLeavingVariable(enteringVariable, data);
                    if (leavingVariable == -1)
                    {
                        hasSolution = false;
                        state = false;
                    }
                    else
                    {
                        cost = this.ProcessReduction(enteringVariable, leavingVariable, data, cost);
                        leavingVariable = this.GetNextEnteringVariable(data);
                        state = enteringVariable != -1;
                    }
                }

                if (hasSolution)
                {
                    return this.BuildSolution(data, cost);
                }
                else
                {
                    return new SimplexOutput<CoeffType>(null, default(CoeffType), false);
                }
            }
        }

        /// <summary>
        /// Obtém o objecto responsável pelas operações sobre os coeficientes.
        /// </summary>
        public IField<CoeffType> CoeffsField
        {
            get
            {
                return this.coeffsField;
            }
        }

        /// <summary>
        /// Obtém o comparador de coeficientes.
        /// </summary>
        public IComparer<CoeffType> CoeffsComparer
        {
            get
            {
                return this.coeffsComparer;
            }
        }

        /// <summary>
        /// Obtém a próxima variável de entrada.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>A variável caso exista e -1 caso contrário.</returns>
        private int GetNextEnteringVariable(SimplexInput<CoeffType, CoeffType> data)
        {
            var result = -1;
            var value = this.coeffsField.AdditiveUnity;
            Parallel.For(0, data.ObjectiveFunction.GetLength(0), i =>
            {
                var current = data.ObjectiveFunction[i, 0];
                if (this.coeffsComparer.Compare(current, this.coeffsField.AdditiveUnity) < 0)
                {
                    if (this.coeffsComparer.Compare(current, value) < 0)
                    {
                        lock (this.lockObject)
                        {
                            result = i;
                            value = current;
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Obtém a próxima variável de saída.
        /// </summary>
        /// <param name="enteringVariable">A variável de entrada.</param>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>A variável caso exista e -1 caso contrário.</returns>
        private int GetNextLeavingVariable(int enteringVariable, SimplexInput<CoeffType, CoeffType> data)
        {
            var result = -1;
            var value = this.coeffsField.AdditiveUnity;

            // Estabelece o primeiro valor para futura comparação.
            var index = 0;
            while (index < enteringVariable && result == -1)
            {
                var currentValue = data.ConstraintsMatrix[index, enteringVariable];
                if (this.coeffsComparer.Compare(currentValue, this.coeffsField.AdditiveUnity) > 0)
                {
                    value = this.coeffsField.Multiply(
                        data.ConstraintsVector[index, 0],
                        this.coeffsField.MultiplicativeInverse(currentValue));
                    result = index;
                }

                ++index;
            }

            if (result == -1)
            {
                ++index;
                while (index < data.ConstraintsMatrix.GetLength(0) && result == -1)
                {
                    var currentValue = data.ConstraintsMatrix[index, enteringVariable];
                    if (this.coeffsComparer.Compare(currentValue, this.coeffsField.AdditiveUnity) > 0)
                    {
                        value = this.coeffsField.Multiply(
                            data.ConstraintsVector[index, 0],
                            this.coeffsField.MultiplicativeInverse(currentValue));
                        result = index;
                    }

                    ++index;
                }
            }

            if (result != -1)
            {
                Parallel.For(index, enteringVariable - 1, i =>
                {
                    var currentValue = data.ConstraintsMatrix[i, enteringVariable];
                    if (this.coeffsComparer.Compare(currentValue, this.coeffsField.AdditiveUnity) > 0)
                    {
                        currentValue = this.coeffsField.Multiply(
                            data.ConstraintsVector[i, 0],
                            this.coeffsField.MultiplicativeInverse(currentValue));
                        if (this.coeffsComparer.Compare(currentValue, value) < 0)
                        {
                            lock (this.lockObject)
                            {
                                result = i;
                                value = currentValue;
                            }
                        }
                    }
                });

                Parallel.For(enteringVariable + 1, data.ConstraintsMatrix.GetLength(0), i =>
                {
                    var currentValue = data.ConstraintsMatrix[i, enteringVariable];
                    if (this.coeffsComparer.Compare(currentValue, this.coeffsField.AdditiveUnity) > 0)
                    {
                        currentValue = this.coeffsField.Multiply(
                            data.ConstraintsVector[i, 0],
                            this.coeffsField.MultiplicativeInverse(currentValue));
                        if (this.coeffsComparer.Compare(currentValue, value) < 0)
                        {
                            lock (this.lockObject)
                            {
                                result = i;
                                value = currentValue;
                            }
                        }
                    }
                });
            }

            return result;
        }

        /// <summary>
        /// Aplica a condensação a todas as linhas da matriz das restrições.
        /// </summary>
        /// <param name="enteringVariable">A variável de entrada.</param>
        /// <param name="leavingVariable">A variável de saída.</param>
        /// <param name="data">Os dados de entrada.</param>
        /// <param name="currentCost">O custo anterior.</param>
        /// <returns>O custo após o processamento.</returns>
        private CoeffType ProcessReduction(
            int enteringVariable,
            int leavingVariable,
            SimplexInput<CoeffType, CoeffType> data,
            CoeffType currentCost)
        {
            var result = currentCost;

            // Actualiza a linha pivô
            var multiplicativeProduct = this.coeffsField.MultiplicativeInverse(
                data.ConstraintsMatrix[leavingVariable, enteringVariable]);
            Parallel.For(0, enteringVariable - 1, i =>
            {
                data.ConstraintsMatrix[leavingVariable, i] = this.coeffsField.Multiply(
                    data.ConstraintsMatrix[leavingVariable, i],
                    multiplicativeProduct);
            });

            Parallel.For(enteringVariable + 1, data.ConstraintsMatrix.GetLength(1), i =>
            {
                data.ConstraintsMatrix[leavingVariable, i] = this.coeffsField.Multiply(
                    data.ConstraintsMatrix[leavingVariable, i],
                    multiplicativeProduct);
            });

            data.ConstraintsVector[leavingVariable, 0] = this.coeffsField.Multiply(
                data.ConstraintsVector[leavingVariable, 0],
                multiplicativeProduct);

            data.ConstraintsMatrix[leavingVariable, enteringVariable] = multiplicativeProduct;

            // Actualiza as restantes linhas e o vector
            Parallel.For(0, leavingVariable - 1, line =>
            {
                multiplicativeProduct = data.ConstraintsMatrix[line, enteringVariable];
                if (!this.coeffsField.IsAdditiveUnity(multiplicativeProduct))
                {
                    multiplicativeProduct = this.coeffsField.AdditiveInverse(
                        data.ConstraintsMatrix[line, enteringVariable]);
                    for (int column = 0; column < enteringVariable; ++column)
                    {
                        data.ConstraintsMatrix[line, column] = this.coeffsField.Add(
                            data.ConstraintsMatrix[line, column],
                            this.coeffsField.Multiply(data.ConstraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    data.ConstraintsMatrix[line, enteringVariable] = multiplicativeProduct;

                    for (int column = enteringVariable + 1; column < data.ConstraintsMatrix.GetLength(1); ++column)
                    {
                        data.ConstraintsMatrix[line, column] = this.coeffsField.Add(
                            data.ConstraintsMatrix[line, column],
                            this.coeffsField.Multiply(data.ConstraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    data.ConstraintsVector[line, 0] = this.coeffsField.Add(
                            data.ConstraintsVector[line, 0],
                            this.coeffsField.Multiply(data.ConstraintsVector[leavingVariable, 0],
                            multiplicativeProduct));
                }
            });

            Parallel.For(0, leavingVariable - 1, line =>
            {
                multiplicativeProduct = data.ConstraintsMatrix[line, enteringVariable];
                if (!this.coeffsField.IsAdditiveUnity(multiplicativeProduct))
                {
                    multiplicativeProduct = this.coeffsField.AdditiveInverse(
                        data.ConstraintsMatrix[line, enteringVariable]);
                    for (int column = 0; column < enteringVariable; ++column)
                    {
                        data.ConstraintsMatrix[line, column] = this.coeffsField.Add(
                            data.ConstraintsMatrix[line, column],
                            this.coeffsField.Multiply(data.ConstraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    data.ConstraintsMatrix[line, enteringVariable] = multiplicativeProduct;

                    for (int column = enteringVariable + 1; column < data.ConstraintsMatrix.GetLength(1); ++column)
                    {
                        data.ConstraintsMatrix[line, column] = this.coeffsField.Add(
                            data.ConstraintsMatrix[line, column],
                            this.coeffsField.Multiply(data.ConstraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    data.ConstraintsVector[line, 0] = this.coeffsField.Add(
                            data.ConstraintsVector[line, 0],
                            this.coeffsField.Multiply(data.ConstraintsVector[leavingVariable, 0],
                            multiplicativeProduct));
                }
            });

            // Função objectivo
            multiplicativeProduct = data.ObjectiveFunction[0, enteringVariable];
            if (!this.coeffsField.IsAdditiveUnity(multiplicativeProduct))
            {
                Parallel.For(0, enteringVariable - 1, column =>
                {
                    data.ObjectiveFunction[0, column] = this.coeffsField.Add(
                                data.ObjectiveFunction[0, column],
                                this.coeffsField.Multiply(data.ConstraintsMatrix[leavingVariable, column],
                                multiplicativeProduct));
                });
                data.ObjectiveFunction[0, enteringVariable] = multiplicativeProduct;

                Parallel.For(enteringVariable + 1, data.ObjectiveFunction.GetLength(0), column =>
                {
                    data.ObjectiveFunction[0, column] = this.coeffsField.Add(
                                data.ObjectiveFunction[0, column],
                                this.coeffsField.Multiply(data.ConstraintsMatrix[leavingVariable, column],
                                multiplicativeProduct));
                });

                result = this.coeffsField.Add(
                            result,
                            this.coeffsField.Multiply(data.ConstraintsVector[leavingVariable, 0],
                            multiplicativeProduct));
            }

            // Troca as variáveis
            var swap = data.NonBasicVariables[enteringVariable];
            data.NonBasicVariables[enteringVariable] = data.BasicVariables[leavingVariable];
            data.BasicVariables[leavingVariable] = swap;

            return result;
        }

        /// <summary>
        /// Constrói a solução a partir da iteração actual.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <param name="cost">O custo calculado.</param>
        /// <returns>A solução.</returns>
        private SimplexOutput<CoeffType> BuildSolution(SimplexInput<CoeffType, CoeffType> data, CoeffType cost)
        {
            var solution = new CoeffType[data.NonBasicVariables.Length];
            for (int i = 0; i < solution.Length; ++i)
            {
                solution[i] = this.coeffsField.AdditiveUnity;
            }

            for (int i = 0; i < data.BasicVariables.Length; ++i)
            {
                var basicVariable = data.BasicVariables[i];
                if (basicVariable < solution.Length)
                {
                    solution[i] = data.ConstraintsVector[i, 0];
                }
            }

            return new SimplexOutput<CoeffType>(solution, cost, true);
        }
    }
}
