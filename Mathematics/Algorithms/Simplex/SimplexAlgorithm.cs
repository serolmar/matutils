namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementa o algoritmo paralelo do simplex habitual.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente a considerar no algoritmo.</typeparam>
    public class SimplexAlgorithm<CoeffType> :
        IAlgorithm<SimplexInput<CoeffType, CoeffType>, SimplexOutput<CoeffType>>,
        IAlgorithm<SimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>>, SimplexOutput<CoeffType>>
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

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplexAlgorithm{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffsComparer">O comparador de coeficientes.</param>
        /// <param name="coeffsField">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
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
        /// Obtém o objecto responsável pelas operações sobre os coeficientes.
        /// </summary>
        /// <value>O objecto responsável pelas operações sobres os coeficientes.</value>
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
        /// <value>O comparador de coeficientes.</value>
        public IComparer<CoeffType> CoeffsComparer
        {
            get
            {
                return this.coeffsComparer;
            }
        }

        /// <summary>
        /// Executa o algoritmo do simplex habitual sobre os dados de entrada.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>O resultado da execução.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se os dados de entrada forem passados com um apontador nulo.
        /// </exception>
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
                while (state)
                {
                    var leavingVariable = this.GetNextLeavingVariable(
                        enteringVariable,
                        data.ConstraintsMatrix,
                        data.ConstraintsVector);
                    if (leavingVariable == -1)
                    {
                        hasSolution = false;
                        state = false;
                    }
                    else
                    {
                        this.ProcessReduction(
                            enteringVariable, 
                            leavingVariable, 
                            data.ConstraintsMatrix,
                            data.ConstraintsVector);

                        data.Cost = this.ProcessObjectiveFunction(
                            enteringVariable,
                            leavingVariable,
                            data.BasicVariables,
                            data.NonBasicVariables,
                            data.Cost,
                            data.ObjectiveFunction,
                            data.ConstraintsMatrix,
                            data.ConstraintsVector);

                        enteringVariable = this.GetNextEnteringVariable(data);
                        state = enteringVariable != -1;
                    }
                }

                if (hasSolution)
                {
                    return this.BuildSolution(
                        data.BasicVariables,
                        data.NonBasicVariables,
                        data.ConstraintsVector,
                        data.Cost);
                }
                else
                {
                    return new SimplexOutput<CoeffType>(null, default(CoeffType), false);
                }
            }
        }

        /// <summary>
        /// Executa o algoritmo do simplex habitual tendo em consideração a notação de inteiro grande.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>O resultado da execução.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se os dados de entrada forem passados com um apontador nulo.
        /// </exception>
        public SimplexOutput<CoeffType> Run(SimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>> data)
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
                while (state)
                {
                    var leavingVariable = this.GetNextLeavingVariable(
                        enteringVariable,
                        data.ConstraintsMatrix,
                        data.ConstraintsVector);
                    if (leavingVariable == -1)
                    {
                        hasSolution = false;
                        state = false;
                    }
                    else
                    {
                        this.ProcessReduction(
                            enteringVariable,
                            leavingVariable,
                            data.ConstraintsMatrix,
                            data.ConstraintsVector);

                        data.Cost = this.ProcessObjectiveFunction(
                            enteringVariable,
                            leavingVariable,
                            data.BasicVariables,
                            data.NonBasicVariables,
                            data.Cost,
                            data.ObjectiveFunction,
                            data.ConstraintsMatrix,
                            data.ConstraintsVector);

                        enteringVariable = this.GetNextEnteringVariable(data);
                        state = enteringVariable != -1;
                    }
                }

                if (hasSolution)
                {
                    return this.BuildSolution(
                        data.BasicVariables,
                        data.NonBasicVariables,
                        data.ConstraintsVector,
                        data.Cost);
                }
                else
                {
                    return new SimplexOutput<CoeffType>(null, default(CoeffType), false);
                }
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
            Parallel.For(0, data.ObjectiveFunction.Length, i =>
            {
                var current = data.ObjectiveFunction[i];
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
        /// Obtém a próxima variável de entrada.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>A variável caso exista e -1 caso contrário.</returns>
        private int GetNextEnteringVariable(SimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>> data)
        {
            var result = -1;
            var value = new SimplexMaximumNumberField<CoeffType>(
                this.coeffsField.AdditiveUnity,
                this.coeffsField.AdditiveUnity);

            Parallel.For(0, data.ObjectiveFunction.Length, i =>
            {
                var current = data.ObjectiveFunction[i];
                if (this.coeffsComparer.Compare(current.BigPart, this.coeffsField.AdditiveUnity) < 0)
                {
                    if (this.ComapareBigNumbers(current, value) < 0)
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
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <returns>A variável caso exista e -1 caso contrário.</returns>
        private int GetNextLeavingVariable(int enteringVariable,
            IMatrix<CoeffType> constraintsMatrix,
            IVector<CoeffType> constraintsVector)
        {
            var result = -1;
            var value = this.coeffsField.AdditiveUnity;

            // Estabelece o primeiro valor para futura comparação.
            var index = 0;
            while (index < constraintsMatrix.GetLength(0) && result == -1)
            {
                var currentValue = constraintsMatrix[index, enteringVariable];
                if (this.coeffsComparer.Compare(currentValue, this.coeffsField.AdditiveUnity) > 0)
                {
                    value = this.coeffsField.Multiply(
                        constraintsVector[index],
                        this.coeffsField.MultiplicativeInverse(currentValue));
                    result = index;
                }

                ++index;
            }

            if (result != -1)
            {
                Parallel.For(index, constraintsMatrix.GetLength(0), i =>
                {
                    var currentValue = constraintsMatrix[i, enteringVariable];
                    if (this.coeffsComparer.Compare(currentValue, this.coeffsField.AdditiveUnity) > 0)
                    {
                        currentValue = this.coeffsField.Multiply(
                            constraintsVector[i],
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
        /// Compara dois números enormes.
        /// </summary>
        /// <param name="firstNumber">O primeiro número a ser comparado.</param>
        /// <param name="secondNumber">O segundo número a ser comparado.</param>
        /// <returns>
        /// Os valores -1, 0 e 1 caso o primeiro número seja respectivamente menor, igual ou maior
        /// do que o segundo número.
        /// </returns>
        private int ComapareBigNumbers(
            SimplexMaximumNumberField<CoeffType> firstNumber,
            SimplexMaximumNumberField<CoeffType> secondNumber)
        {
            var compareValue = this.coeffsComparer.Compare(firstNumber.BigPart, secondNumber.BigPart);
            if (compareValue == 0)
            {
                return this.coeffsComparer.Compare(firstNumber.FinitePart, secondNumber.FinitePart);
            }
            else
            {
                return compareValue;
            }
        }

        /// <summary>
        /// Aplica a condensação a todas as linhas da matriz das restrições.
        /// </summary>
        /// <param name="enteringVariable">A variável de entrada.</param>
        /// <param name="leavingVariable">A variável de saída.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        private void ProcessReduction(
            int enteringVariable,
            int leavingVariable,
            IMatrix<CoeffType> constraintsMatrix,
            IVector<CoeffType> constraintsVector)
        {
            // Actualiza a linha pivô
            var multiplicativeProduct = constraintsMatrix[leavingVariable, enteringVariable];
            if (!this.coeffsField.IsMultiplicativeUnity(multiplicativeProduct))
            {
                multiplicativeProduct = this.coeffsField.MultiplicativeInverse(multiplicativeProduct);
                Parallel.For(0, enteringVariable, i =>
                {
                    constraintsMatrix[leavingVariable, i] = this.coeffsField.Multiply(
                        constraintsMatrix[leavingVariable, i],
                        multiplicativeProduct);
                });

                Parallel.For(enteringVariable + 1, constraintsMatrix.GetLength(1), i =>
                {
                    constraintsMatrix[leavingVariable, i] = this.coeffsField.Multiply(
                        constraintsMatrix[leavingVariable, i],
                        multiplicativeProduct);
                });

                constraintsVector[leavingVariable] = this.coeffsField.Multiply(
                    constraintsVector[leavingVariable],
                    multiplicativeProduct);

                constraintsMatrix[leavingVariable, enteringVariable] = multiplicativeProduct;
            }

            // Actualiza as restantes linhas e o vector
            Parallel.For(0, leavingVariable, line =>
            {
                multiplicativeProduct = constraintsMatrix[line, enteringVariable];
                if (!this.coeffsField.IsAdditiveUnity(multiplicativeProduct))
                {
                    multiplicativeProduct = this.coeffsField.AdditiveInverse(
                        constraintsMatrix[line, enteringVariable]);
                    for (int column = 0; column < enteringVariable; ++column)
                    {
                        constraintsMatrix[line, column] = this.coeffsField.Add(
                            constraintsMatrix[line, column],
                            this.coeffsField.Multiply(constraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    constraintsMatrix[line, enteringVariable] = multiplicativeProduct;

                    for (int column = enteringVariable + 1; column < constraintsMatrix.GetLength(1); ++column)
                    {
                        constraintsMatrix[line, column] = this.coeffsField.Add(
                            constraintsMatrix[line, column],
                            this.coeffsField.Multiply(constraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    constraintsVector[line] = this.coeffsField.Add(
                            constraintsVector[line],
                            this.coeffsField.Multiply(constraintsVector[leavingVariable],
                            multiplicativeProduct));
                }
            });

            Parallel.For(leavingVariable + 1, constraintsMatrix.GetLength(0), line =>
            {
                multiplicativeProduct = constraintsMatrix[line, enteringVariable];
                if (!this.coeffsField.IsAdditiveUnity(multiplicativeProduct))
                {
                    multiplicativeProduct = this.coeffsField.AdditiveInverse(
                        constraintsMatrix[line, enteringVariable]);
                    for (int column = 0; column < enteringVariable; ++column)
                    {
                        constraintsMatrix[line, column] = this.coeffsField.Add(
                            constraintsMatrix[line, column],
                            this.coeffsField.Multiply(constraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    constraintsMatrix[line, enteringVariable] = multiplicativeProduct;

                    for (int column = enteringVariable + 1; column < constraintsMatrix.GetLength(1); ++column)
                    {
                        constraintsMatrix[line, column] = this.coeffsField.Add(
                            constraintsMatrix[line, column],
                            this.coeffsField.Multiply(constraintsMatrix[leavingVariable, column],
                            multiplicativeProduct));
                    }

                    constraintsVector[line] = this.coeffsField.Add(
                            constraintsVector[line],
                            this.coeffsField.Multiply(constraintsVector[leavingVariable],
                            multiplicativeProduct));
                }
            });
        }

        /// <summary>
        /// Processa a função objectivo.
        /// </summary>
        /// <param name="enteringVariable">A variável de entrada.</param>
        /// <param name="leavingVariable">a variável de saída.</param>
        /// <param name="nonBasicVariables">O conjunto de variáveis não-básicas.</param>
        /// <param name="currentCost">O custo actual.</param>
        /// <param name="objective">A função objectivo.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <returns>O resultado.</returns>
        private CoeffType ProcessObjectiveFunction(
            int enteringVariable,
            int leavingVariable,
            int[] basicVariables,
            int[] nonBasicVariables,
            CoeffType currentCost,
            IVector<CoeffType> objective,
            IMatrix<CoeffType> constraintsMatrix,
            IVector<CoeffType> constraintsVector)
        {
            var result = currentCost;
            var multiplicativeProduct = this.coeffsField.AdditiveInverse(objective[enteringVariable]);
            if (!this.coeffsField.IsAdditiveUnity(multiplicativeProduct))
            {
                Parallel.For(0, enteringVariable, column =>
                {
                    objective[column] = this.coeffsField.Add(
                                objective[column],
                                this.coeffsField.Multiply(constraintsMatrix[leavingVariable, column],
                                multiplicativeProduct));
                });

                objective[enteringVariable] = multiplicativeProduct;

                Parallel.For(enteringVariable + 1, objective.Length, column =>
                {
                    objective[column] = this.coeffsField.Add(
                                objective[column],
                                this.coeffsField.Multiply(constraintsMatrix[leavingVariable, column],
                                multiplicativeProduct));
                });

                result = this.coeffsField.Add(
                            result,
                            this.coeffsField.Multiply(constraintsVector[leavingVariable],
                            multiplicativeProduct));
            }

            // Troca as variáveis
            var swap = nonBasicVariables[enteringVariable];
            nonBasicVariables[enteringVariable] = basicVariables[leavingVariable];
            basicVariables[leavingVariable] = swap;

            return result;
        }

        /// <summary>
        /// Processa a função objectivo.
        /// </summary>
        /// <param name="enteringVariable">A variável de entrada.</param>
        /// <param name="leavingVariable">a variável de saída.</param>
        /// <param name="nonBasicVariables">O conjunto de variáveis não-básicas.</param>
        /// <param name="currentCost">O custo actual.</param>
        /// <param name="objective">A função objectivo.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <returns>O resultado.</returns>
        private SimplexMaximumNumberField<CoeffType> ProcessObjectiveFunction(
            int enteringVariable,
            int leavingVariable,
            int[] basicVariables,
            int[] nonBasicVariables,
            SimplexMaximumNumberField<CoeffType> currentCost,
            IVector<SimplexMaximumNumberField<CoeffType>> objective,
            IMatrix<CoeffType> constraintsMatrix,
            IVector<CoeffType> constraintsVector)
        {
            var result = currentCost;
            var multiplicativeProduct = this.GetAdditiveInverse(objective[enteringVariable]);
            if (!this.IsAdditiveUnity(multiplicativeProduct))
            {
                Parallel.For(0, enteringVariable, column =>
                {
                    objective[column] = this.Add(
                                objective[column],
                                this.Multiply(constraintsMatrix[leavingVariable, column],
                                multiplicativeProduct));
                });

                objective[enteringVariable] = multiplicativeProduct;

                Parallel.For(enteringVariable + 1, objective.Length, column =>
                {
                    objective[column] = this.Add(
                                objective[column],
                                this.Multiply(constraintsMatrix[leavingVariable, column],
                                multiplicativeProduct));
                });

                result = this.Add(
                            result,
                            this.Multiply(constraintsVector[leavingVariable],
                            multiplicativeProduct));
            }

            // Troca as variáveis
            var swap = nonBasicVariables[enteringVariable];
            nonBasicVariables[enteringVariable] = basicVariables[leavingVariable];
            basicVariables[leavingVariable] = swap;

            return result;
        }

        /// <summary>
        /// Otbém o simétrico aditivo de um número enorme.
        /// </summary>
        /// <param name="maxNumberField">O número.</param>
        /// <returns>O simétrico aditivo.</returns>
        private SimplexMaximumNumberField<CoeffType> GetAdditiveInverse(
            SimplexMaximumNumberField<CoeffType> maxNumberField)
        {
            return new SimplexMaximumNumberField<CoeffType>(
                this.coeffsField.AdditiveInverse(maxNumberField.FinitePart),
                this.coeffsField.AdditiveInverse(maxNumberField.BigPart));
        }

        /// <summary>
        /// Verifica se um número enorme constitui uma unidade aditiva.
        /// </summary>
        /// <param name="number">O número enorme.</param>
        /// <returns>Verdadeiro caso o número seja uma unidade aditiva e falso caso contrário.</returns>
        private bool IsAdditiveUnity(SimplexMaximumNumberField<CoeffType> number)
        {
            return this.coeffsField.IsAdditiveUnity(number.FinitePart) &&
                this.coeffsField.IsAdditiveUnity(number.BigPart);
        }

        /// <summary>
        /// Obtém o produto de um coeficiente por um número enorme.
        /// </summary>
        /// <param name="coeff">O coeficiente.</param>
        /// <param name="number">O número enorme.</param>
        /// <returns>O número enorme que resulta do produto.</returns>
        private SimplexMaximumNumberField<CoeffType> Multiply(
            CoeffType coeff,
            SimplexMaximumNumberField<CoeffType> number)
        {
            if (this.coeffsField.IsAdditiveUnity(coeff))
            {
                return new SimplexMaximumNumberField<CoeffType>(
                    this.coeffsField.AdditiveUnity,
                    this.coeffsField.AdditiveUnity);
            }
            else
            {
                return new SimplexMaximumNumberField<CoeffType>(
                    this.coeffsField.Multiply(coeff, number.FinitePart),
                    this.coeffsField.Multiply(coeff, number.BigPart));
            }
        }

        /// <summary>
        /// Adiciona dois números enormes.
        /// </summary>
        /// <param name="firstNumber">O primeiro número.</param>
        /// <param name="secondNumber">O segundo número.</param>
        /// <returns>O resultado da soma.</returns>
        private SimplexMaximumNumberField<CoeffType> Add(
            SimplexMaximumNumberField<CoeffType> firstNumber,
            SimplexMaximumNumberField<CoeffType> secondNumber)
        {
            return new SimplexMaximumNumberField<CoeffType>(
                this.coeffsField.Add(firstNumber.FinitePart, secondNumber.FinitePart),
                this.coeffsField.Add(firstNumber.BigPart, secondNumber.BigPart));
        }

        /// <summary>
        /// Constrói a solução a partir da iteração actual.
        /// </summary>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <param name="nonBasicVariables">As variáveis não-básicas.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <param name="cost">O custo actual.</param>
        /// <returns>A solução.</returns>
        private SimplexOutput<CoeffType> BuildSolution(
            int[] basicVariables,
            int[] nonBasicVariables,
            IVector<CoeffType> constraintsVector,
            CoeffType cost
            )
        {
            var solution = new CoeffType[nonBasicVariables.Length];
            for (int i = 0; i < solution.Length; ++i)
            {
                solution[i] = this.coeffsField.AdditiveUnity;
            }

            for (int i = 0; i < basicVariables.Length; ++i)
            {
                var basicVariable = basicVariables[i];
                if (basicVariable < solution.Length)
                {
                    solution[basicVariable] = constraintsVector[i];
                }
            }

            return new SimplexOutput<CoeffType>(solution, cost, true);
        }

        /// <summary>
        /// Constrói a solução a partir da iteração actual.
        /// </summary>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <param name="nonBasicVariables">As variáveis não-básicas.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <param name="cost">O custo actual.</param>
        /// <returns>A solução.</returns>
        private SimplexOutput<CoeffType> BuildSolution(
            int[] basicVariables,
            int[] nonBasicVariables,
            IVector<CoeffType> constraintsVector,
            SimplexMaximumNumberField<CoeffType> cost
            )
        {
            var solution = new CoeffType[nonBasicVariables.Length];
            for (int i = 0; i < solution.Length; ++i)
            {
                solution[i] = this.coeffsField.AdditiveUnity;
            }

            for (int i = 0; i < basicVariables.Length; ++i)
            {
                var basicVariable = basicVariables[i];
                if (basicVariable < solution.Length)
                {
                    solution[basicVariable] = constraintsVector[i];
                }
            }

            return new SimplexOutput<CoeffType>(solution, cost.FinitePart, true);
        }
    }
}
