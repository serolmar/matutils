namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Implementa o algorimto do simplex revisto.
    /// </summary>
    /// <typeparam name="CoeffType">
    /// O tipo dos objectos que constituem os coeficientes.
    /// </typeparam>
    public class RevisedSimplexAlgorithm<CoeffType> :
        IAlgorithm<RevisedSimplexInput<CoeffType, CoeffType>, SimplexOutput<CoeffType>>,
        IAlgorithm<RevisedSimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>>, SimplexOutput<CoeffType>>
    {
        /// <summary>
        /// O objecto responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IField<CoeffType> coeffsField;

        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IComparer<CoeffType> coeffsComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="RevisedSimplexAlgorithm{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffsComparer">O comparador de coeficientes.</param>
        /// <param name="coeffsField">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public RevisedSimplexAlgorithm(IComparer<CoeffType> coeffsComparer, IField<CoeffType> coeffsField)
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
        /// Executa o algoritmo do simplex revisto sobre os dados de entrada.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>O resultado da execução.</returns>
        /// <exception cref="ArgumentNullException">
        /// Se os dados de entrada forem passados com um apontador nulo.
        /// </exception>
        /// <exception cref="MathematicsException">
        /// Se alguma das matrizes das restrições ou inversa for esparsa
        /// e o valor por defeito não for uma unidade aditiva.
        /// </exception>
        public SimplexOutput<CoeffType> Run(RevisedSimplexInput<CoeffType, CoeffType> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var sparseMatrix = data.ConstraintsMatrix as ISparseMatrix<CoeffType>;
                if (sparseMatrix != null)
                {
                    if (!this.coeffsField.IsAdditiveUnity(sparseMatrix.DefaultValue))
                    {
                        throw new MathematicsException(
                            "The default value in sparse constraints matrix must be the field's additive unity.");
                    }
                }

                sparseMatrix = data.InverseBasisMatrix as ISparseMatrix<CoeffType>;
                if (sparseMatrix != null)
                {
                    if (!this.coeffsField.IsAdditiveUnity(sparseMatrix.DefaultValue))
                    {
                        throw new MathematicsException(
                            "The default value in sparse inverse basis matrix must be the field's additive unity.");
                    }
                }

                var hasSolution = true;
                var initialBasisCosts = this.GetCosts(data.NonBasicVariables.Length, data.BasicVariables);
                var dualVector = this.ComputeDualVector(
                    initialBasisCosts,
                    data.InverseBasisMatrix,
                    data.ObjectiveFunction, data.BasicVariables);
                var enteringVariable = this.GetNextEnteringVariable(
                    dualVector,
                    data.ConstraintsMatrix,
                    data.ObjectiveFunction,
                    data.NonBasicVariables);
                var constraintsVector = new CoeffType[data.BasicVariables.Length];
                this.ComputeConstraintsVector(data.InverseBasisMatrix, data.ConstraintsVector, constraintsVector);
                if (enteringVariable != -1)
                {
                    var etaVector = new CoeffType[data.BasicVariables.Length];
                    var state = true;
                    while (state)
                    {
                        var leavingVariable = this.GetNextLeavingVariable(
                        data.NonBasicVariables[enteringVariable],
                        data.ConstraintsMatrix,
                        constraintsVector,
                        data.InverseBasisMatrix,
                        etaVector);
                        if (leavingVariable == -1)
                        {
                            hasSolution = false;
                            state = false;
                        }
                        else
                        {
                            this.UpdateInverseMatrix(leavingVariable, data.InverseBasisMatrix, etaVector);
                            this.UpdateVariables(
                                enteringVariable,
                                leavingVariable,
                                data.NonBasicVariables,
                                data.BasicVariables,
                                initialBasisCosts);
                            dualVector = this.ComputeDualVector(
                                initialBasisCosts,
                                data.InverseBasisMatrix,
                                data.ObjectiveFunction, data.BasicVariables);
                            enteringVariable = this.GetNextEnteringVariable(
                                dualVector,
                                data.ConstraintsMatrix,
                                data.ObjectiveFunction,
                                data.NonBasicVariables);
                            this.ComputeConstraintsVector(data.InverseBasisMatrix, data.ConstraintsVector, constraintsVector);
                            state = enteringVariable != -1;
                        }
                    }
                }

                if (hasSolution)
                {
                    return this.BuildSolution(
                        data.NonBasicVariables,
                        data.BasicVariables, 
                        data.Cost,
                        dualVector,
                        constraintsVector,
                        data.ConstraintsVector);
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
        public SimplexOutput<CoeffType> Run(RevisedSimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var sparseMatrix = data.ConstraintsMatrix as ISparseMatrix<CoeffType>;
                if (sparseMatrix != null)
                {
                    if (!this.coeffsField.IsAdditiveUnity(sparseMatrix.DefaultValue))
                    {
                        throw new MathematicsException(
                            "The default value in sparse constraints matrix must be the field's additive unity.");
                    }
                }

                sparseMatrix = data.InverseBasisMatrix as ISparseMatrix<CoeffType>;
                if (sparseMatrix != null)
                {
                    if (!this.coeffsField.IsAdditiveUnity(sparseMatrix.DefaultValue))
                    {
                        throw new MathematicsException(
                            "The default value in sparse inverse basis matrix must be the field's additive unity.");
                    }
                }

                var hasSolution = true;
                var initialBasisCosts = this.GetCosts(data.NonBasicVariables.Length, data.BasicVariables);
                var dualVector = this.ComputeDualVector(
                    initialBasisCosts,
                    data.InverseBasisMatrix,
                    data.ObjectiveFunction, data.BasicVariables);
                var enteringVariable = this.GetNextEnteringVariable(
                    dualVector,
                    data.ConstraintsMatrix,
                    data.ObjectiveFunction,
                    data.NonBasicVariables);
                var constraintsVector = new CoeffType[data.BasicVariables.Length];
                this.ComputeConstraintsVector(data.InverseBasisMatrix, data.ConstraintsVector, constraintsVector);
                if (enteringVariable != -1)
                {
                    var etaVector = new CoeffType[data.BasicVariables.Length];
                    var state = true;
                    while (state)
                    {
                        var leavingVariable = this.GetNextLeavingVariable(
                        data.NonBasicVariables[enteringVariable],
                        data.ConstraintsMatrix,
                        constraintsVector,
                        data.InverseBasisMatrix,
                        etaVector);
                        if (leavingVariable == -1)
                        {
                            hasSolution = false;
                            state = false;
                        }
                        else
                        {
                            this.UpdateInverseMatrix(leavingVariable, data.InverseBasisMatrix, etaVector);
                            this.UpdateVariables(
                                enteringVariable,
                                leavingVariable,
                                data.NonBasicVariables,
                                data.BasicVariables,
                                initialBasisCosts);
                            dualVector = this.ComputeDualVector(
                                initialBasisCosts,
                                data.InverseBasisMatrix,
                                data.ObjectiveFunction, data.BasicVariables);
                            enteringVariable = this.GetNextEnteringVariable(
                                dualVector,
                                data.ConstraintsMatrix,
                                data.ObjectiveFunction,
                                data.NonBasicVariables);
                            this.ComputeConstraintsVector(data.InverseBasisMatrix, data.ConstraintsVector, constraintsVector);
                            state = enteringVariable != -1;
                        }
                    }
                }

                if (hasSolution)
                {
                    return this.BuildSolution(
                        data.NonBasicVariables,
                        data.BasicVariables, 
                        data.Cost,
                        dualVector,
                        constraintsVector,
                        data.ConstraintsVector);
                }
                else
                {
                    return new SimplexOutput<CoeffType>(null, default(CoeffType), false);
                }
            }
        }

        /// <summary>
        /// Constrói uma representação da função objectivo correspondente ao conjunto de variáveis não básicas.
        /// </summary>
        /// <param name="nonBasicVariablesLength">O tamanho do vector das variáveis não básicas.</param>
        /// <param name="basicVariables">O conjunto das variáveis básicas.</param>
        /// <returns>A representação.</returns>
        private SortedSet<int> GetCosts(
            int nonBasicVariablesLength, 
            int[] basicVariables)
        {
            var result = new SortedSet<int>(Comparer<int>.Default);
            var length = basicVariables.Length;
            for (int i = 0; i < length; ++i)
            {
                var index = basicVariables[i];
                if (index < nonBasicVariablesLength)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o vector dual.
        /// </summary>
        /// <param name="nonBasicCostsIndices">Os índices dos valores com custos relativos às variáveis básicas.</param>
        /// <param name="inverseMatrix">A matriz da base inversa.</param>
        /// <param name="objectiveFunction">O vector que representa a função objectivo.</param>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <returns>O vector dual utilizado para determinar os valores dos custos.</returns>
        private CoeffType[] ComputeDualVector(
            SortedSet<int> nonBasicCostsIndices,
            ISquareMatrix<CoeffType> inverseMatrix,
            IVector<CoeffType> objectiveFunction,
            int[] basicVariables)
        {
            var result = new CoeffType[basicVariables.Length];
            var dimension = basicVariables.Length;
            Parallel.For(0, dimension, i =>
            {
                var sum = this.coeffsField.AdditiveUnity;
                foreach (var index in nonBasicCostsIndices)
                {
                    var objectiveValue = objectiveFunction[basicVariables[index]];
                    var valueToAdd = inverseMatrix[index, i];
                    valueToAdd = this.coeffsField.Multiply(
                        valueToAdd, 
                        this.coeffsField.AdditiveInverse(objectiveValue));
                    sum = this.coeffsField.Add(sum, valueToAdd);
                }

                result[i] = sum;
            });

            return result;
        }

        /// <summary>
        /// Obtém o vector dual.
        /// </summary>
        /// <param name="nonBasicCostsIndices">Os índices dos valores com custos relativos às variáveis básicas.</param>
        /// <param name="inverseMatrix">A matriz da base inversa.</param>
        /// <param name="objectiveFunction">O vector que representa a função objectivo.</param>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <returns>O vector dual utilizado para determinar os valores dos custos.</returns>
        private SimplexMaximumNumberField<CoeffType>[] ComputeDualVector(
            SortedSet<int> nonBasicCostsIndices,
            ISquareMatrix<CoeffType> inverseMatrix,
            IVector<SimplexMaximumNumberField<CoeffType>> objectiveFunction,
            int[] basicVariables)
        {
            var result = new SimplexMaximumNumberField<CoeffType>[basicVariables.Length];
            var dimension = basicVariables.Length;
            Parallel.For(0, dimension, i =>
            {
                var sum = new SimplexMaximumNumberField<CoeffType>(this.coeffsField.AdditiveUnity, this.coeffsField.AdditiveUnity);
                foreach (var index in nonBasicCostsIndices)
                {
                    var objectiveValue = objectiveFunction[basicVariables[index]];
                    var finitePart = objectiveValue.FinitePart;
                    var bigPart = objectiveValue.BigPart;
                    var inverseMatrixEntry = inverseMatrix[index, i];
                    finitePart = this.coeffsField.Multiply(
                        inverseMatrixEntry, 
                        this.coeffsField.AdditiveInverse(objectiveValue.FinitePart));
                    bigPart = this.coeffsField.Multiply(
                        inverseMatrixEntry,
                        this.coeffsField.AdditiveInverse(objectiveValue.BigPart));
                    sum.Add(finitePart, bigPart, this.coeffsField);
                }

                result[i] = sum;
            });

            return result;
        }

        /// <summary>
        /// Obtém a variável de entrada.
        /// </summary>
        /// <param name="dualVector">O vector dual que irá permitir calcular os coeficientes do custo.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="objectiveFunction">A função objectivo.</param>
        /// <param name="nonBasicVariables">As variáveis não básicas.</param>
        /// <returns>O índice da variável a entrar.</returns>
        private int GetNextEnteringVariable(
            CoeffType[] dualVector,
            IMatrix<CoeffType> constraintsMatrix,
            IVector<CoeffType> objectiveFunction,
            int[] nonBasicVariables)
        {
            var result = -1;
            var value = this.coeffsField.AdditiveUnity;
            var nonBasicLength = nonBasicVariables.Length;
            var constraintsLinesLength = dualVector.Length;

            var syncObject = new object(); // Utilizado para sincronizar as comparações.
            Parallel.For(0, nonBasicLength, i =>
            {
                var innerValue = this.coeffsField.AdditiveUnity;
                var nonBasicVariable = nonBasicVariables[i];
                if (nonBasicVariable < nonBasicLength)
                {
                    for (int j = 0; j < constraintsLinesLength; ++j)
                    {
                        var valueToAdd = this.coeffsField.Multiply(dualVector[j], constraintsMatrix[j, nonBasicVariable]);
                        valueToAdd = this.coeffsField.Add(
                            valueToAdd,
                            objectiveFunction[nonBasicVariable]);
                        innerValue = this.coeffsField.Add(innerValue, valueToAdd);
                    }

                    lock (syncObject)
                    {
                        if (this.coeffsComparer.Compare(innerValue, value) < 0)
                        {
                            result = i;
                            value = innerValue;
                        }
                    }
                }
                else
                {
                    var index = nonBasicVariable - nonBasicLength;
                    innerValue = dualVector[index];

                    lock (syncObject)
                    {
                        if (this.coeffsComparer.Compare(innerValue, value) < 0)
                        {
                            result = i;
                            value = innerValue;
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Obtém a variável de entrada.
        /// </summary>
        /// <param name="dualVector">O vector dual que irá permitir calcular os coeficientes do custo.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="objectiveFunction">A função objectivo.</param>
        /// <param name="nonBasicVariables">As variáveis não básicas.</param>
        /// <returns>O índice da variável a entrar.</returns>
        private int GetNextEnteringVariable(
            SimplexMaximumNumberField<CoeffType>[] dualVector,
            IMatrix<CoeffType> constraintsMatrix,
            IVector<SimplexMaximumNumberField<CoeffType>> objectiveFunction,
            int[] nonBasicVariables)
        {
            var result = -1;
            var value = new SimplexMaximumNumberField<CoeffType>(
                this.coeffsField.AdditiveUnity,
                this.coeffsField.AdditiveUnity);
            var nonBasicLength = nonBasicVariables.Length;
            var constraintsLinesLength = dualVector.Length;

            var syncObject = new object(); // Utilizado para sincronizar as comparações.
            Parallel.For(0, nonBasicLength, i =>
            {
                var finitePart = this.coeffsField.AdditiveUnity;
                var bigPart = this.coeffsField.AdditiveUnity;
                var nonBasicVariable = nonBasicVariables[i];
                if (nonBasicVariable < nonBasicLength)
                {
                    for (int j = 0; j < constraintsLinesLength; ++j)
                    {
                        var dualVectorEntry = dualVector[j];
                        var constraintsMatrixEntry = constraintsMatrix[j, nonBasicVariable];
                        var finiteValueToAdd = this.coeffsField.Multiply(dualVectorEntry.FinitePart, constraintsMatrixEntry);
                        var bigValueToAdd = this.coeffsField.Multiply(dualVectorEntry.BigPart, constraintsMatrixEntry);

                        var objectiveValue = objectiveFunction[nonBasicVariable];
                        finiteValueToAdd = this.coeffsField.Add(finiteValueToAdd, objectiveValue.FinitePart);
                        bigValueToAdd = this.coeffsField.Add(bigValueToAdd, objectiveValue.BigPart);

                        finitePart = this.coeffsField.Add(finitePart, finiteValueToAdd);
                        bigPart = this.coeffsField.Add(bigPart, bigValueToAdd);
                    }

                    lock (syncObject)
                    {
                        var comparisionValue = this.coeffsComparer.Compare(bigPart, value.BigPart);
                        if (comparisionValue < 0)
                        {
                            result = i;
                            value.FinitePart = finitePart;
                            value.BigPart = bigPart;
                        }
                        else if (comparisionValue == 0)
                        {
                            if (this.coeffsComparer.Compare(finitePart, value.FinitePart) < 0)
                            {
                                result = i;
                                value.FinitePart = finitePart;
                                value.BigPart = bigPart;
                            }
                        }
                    }
                }
                else
                {
                    var index = nonBasicVariable - nonBasicLength;
                    var dualValue = dualVector[index];
                    finitePart = dualValue.FinitePart;
                    bigPart = dualValue.BigPart;

                    lock (syncObject)
                    {
                        var comparisionValue = this.coeffsComparer.Compare(bigPart, value.BigPart);
                        if (comparisionValue < 0)
                        {
                            result = i;
                            value.FinitePart = finitePart;
                            value.BigPart = bigPart;
                        }
                        else if (comparisionValue == 0)
                        {
                            if (this.coeffsComparer.Compare(finitePart, value.FinitePart) < 0)
                            {
                                result = i;
                                value.FinitePart = finitePart;
                                value.BigPart = bigPart;
                            }
                        }
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Determina a próxima variável de saída.
        /// </summary>
        /// <remarks>
        /// Esta função permite fazer um pré-cálculo do vector eta, sendo o respectivo contentor de dados
        /// passado como argumento.
        /// </remarks>
        /// <param name="enteringVariable">A variável de entrada.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <param name="inverseBasisMatrix">A matriz das bases inversa.</param>
        /// <param name="etaVector">O contentor do vector eta.</param>
        /// <returns>A próxima variável de saída.</returns>
        private int GetNextLeavingVariable(
            int enteringVariable,
            IMatrix<CoeffType> constraintsMatrix,
            CoeffType[] constraintsVector,
            ISquareMatrix<CoeffType> inverseBasisMatrix,
            CoeffType[] etaVector)
        {
            // Calcula os coeficientes e coloca-os no vector eta.
            this.ComputeVariableCoefficients(
                enteringVariable,
                constraintsMatrix,
                inverseBasisMatrix,
                etaVector);

            var result = -1;
            var value = this.coeffsField.AdditiveUnity;

            // Estabelece o primeiro valor para futura comparação.
            var constraintsLength = constraintsMatrix.GetLength(0);
            var index = 0;
            while (index < constraintsLength && result == -1)
            {
                var currentValue = etaVector[index];
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
                for (; index < constraintsLength; ++index)
                {
                    var currentValue = etaVector[index];
                    if (this.coeffsComparer.Compare(currentValue, this.coeffsField.AdditiveUnity) > 0)
                    {
                        currentValue = this.coeffsField.Multiply(
                            constraintsVector[index],
                            this.coeffsField.MultiplicativeInverse(currentValue));
                        if (this.coeffsComparer.Compare(currentValue, value) < 0)
                        {
                            result = index;
                            value = currentValue;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Actualiza as entradas da matriz inversa.
        /// </summary>
        /// <param name="leavingVariableIndex">O índice da variável de saída.</param>
        /// <param name="inverseMatrix">A matriz inversa.</param>
        /// <param name="etaVector">O vector eta.</param>
        private void UpdateInverseMatrix(
            int leavingVariableIndex,
            ISquareMatrix<CoeffType> inverseMatrix,
            CoeffType[] etaVector)
        {
            // Actualiza o valor de eta
            var multiplicativeInverse = this.coeffsField.MultiplicativeInverse(etaVector[leavingVariableIndex]);
            Parallel.For(0, leavingVariableIndex, i =>
            {
                var value = this.coeffsField.Multiply(etaVector[i], multiplicativeInverse);
                etaVector[i] = this.coeffsField.AdditiveInverse(value);
            });

            etaVector[leavingVariableIndex] = multiplicativeInverse;

            var length = etaVector.Length;
            Parallel.For(leavingVariableIndex + 1, length, i =>
            {
                var value = this.coeffsField.Multiply(etaVector[i], multiplicativeInverse);
                etaVector[i] = this.coeffsField.AdditiveInverse(value);
            });

            // Actualiza a matriz inversa
            Parallel.For(0, leavingVariableIndex, i =>
            {
                inverseMatrix.CombineLines(
                    i,
                    leavingVariableIndex,
                    this.coeffsField.MultiplicativeUnity,
                    etaVector[i],
                    this.coeffsField);
            });

            Parallel.For(leavingVariableIndex + 1, length, i =>
            {
                inverseMatrix.CombineLines(
                    i,
                    leavingVariableIndex,
                    this.coeffsField.MultiplicativeUnity,
                    etaVector[i],
                    this.coeffsField);
            });

            inverseMatrix.ScalarLineMultiplication(leavingVariableIndex, etaVector[leavingVariableIndex], this.coeffsField);
        }

        /// <summary>
        /// Actualiza as variáveis.
        /// </summary>
        /// <param name="enteringVariableIndex">O índice da variável de entrada.</param>
        /// <param name="leavingVariableIndex">O índice da variável de saída.</param>
        /// <param name="nonBasicVariables">As variáveis não básicas.</param>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <param name="costsFunction">Os índices das funções de custos.</param>
        private void UpdateVariables(
            int enteringVariableIndex,
            int leavingVariableIndex,
            int[] nonBasicVariables,
            int[] basicVariables,
            SortedSet<int> costsFunction)
        {
            if (nonBasicVariables[enteringVariableIndex] < nonBasicVariables.Length)
            {
                if (!costsFunction.Contains(leavingVariableIndex))
                {
                    costsFunction.Add(leavingVariableIndex);
                }
            }
            else
            {
                costsFunction.Remove(leavingVariableIndex);
            }

            // Troca as variáveis
            var swap = nonBasicVariables[enteringVariableIndex];
            nonBasicVariables[enteringVariableIndex] = basicVariables[leavingVariableIndex];
            basicVariables[leavingVariableIndex] = swap;
        }

        /// <summary>
        /// Constrói a soução final a partir dos dados.
        /// </summary>
        /// <param name="nonBasicVariables">As variáveis não básicas.</param>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <param name="dataCost">O custo inicial.</param>
        /// <param name="dualVector">O vector dual.</param>
        /// <param name="computedConstraintsVector">O vector das restrições calculado.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <returns>A solução.</returns>
        private SimplexOutput<CoeffType> BuildSolution(
            int[] nonBasicVariables,
            int[] basicVariables,
            CoeffType dataCost,
            CoeffType[] dualVector,
            CoeffType[] computedConstraintsVector,
            IVector<CoeffType> constraintsVector)
        {
            var nonBasicLength = nonBasicVariables.Length;
            var basicLength = basicVariables.Length;

            // Constrói a solução.
            CoeffType[] solution = new CoeffType[nonBasicLength];

            Parallel.For(0, basicLength, i =>
            {
                var basicVariable = basicVariables[i];
                if (basicVariable < nonBasicLength)
                {
                    solution[basicVariable] = computedConstraintsVector[i];
                }
            });

            // Calcula o custo.
            var cost = this.coeffsField.AdditiveUnity;
            var length = dualVector.Length;
            for (int i = 0; i < length; ++i )
            {
                var valueToAdd = constraintsVector[i];
                valueToAdd = this.coeffsField.Multiply(
                    valueToAdd,
                    dualVector[i]);
                cost = this.coeffsField.Add(cost, valueToAdd);
            }

            cost = this.coeffsField.Add(cost, dataCost);
            return new SimplexOutput<CoeffType>(solution, cost, true);
        }

        /// <summary>
        /// Constrói a soução final a partir dos dados.
        /// </summary>
        /// <param name="nonBasicVariables">As variáveis não básicas.</param>
        /// <param name="basicVariables">As variáveis básicas.</param>
        /// <param name="dataCost">O custo inicial.</param>
        /// <param name="dualVector">O vector dual.</param>
        /// <param name="computedConstraintsVector">O vector das restrições calculado.</param>
        /// <param name="constraintsVector">O vector das restrições.</param>
        /// <returns>A solução.</returns>
        private SimplexOutput<CoeffType> BuildSolution(
            int[] nonBasicVariables,
            int[] basicVariables,
            SimplexMaximumNumberField<CoeffType> dataCost,
            SimplexMaximumNumberField<CoeffType>[] dualVector,
            CoeffType[] computedConstraintsVector,
            IVector<CoeffType> constraintsVector)
        {
            var nonBasicLength = nonBasicVariables.Length;
            var basicLength = basicVariables.Length;

            // Constrói a solução.
            CoeffType[] solution = new CoeffType[nonBasicLength];

            Parallel.For(0, basicLength, i =>
            {
                var basicVariable = basicVariables[i];
                if (basicVariable < nonBasicLength)
                {
                    solution[basicVariable] = computedConstraintsVector[i];
                }
            });

            // Calcula o custo.
            var finiteCost = this.coeffsField.AdditiveUnity;
            var bigCost = this.coeffsField.AdditiveUnity;
            var length = dualVector.Length;
            for (int i = 0; i < length; ++i)
            {
                var constraintsVectorValue = constraintsVector[i];
                var dualValue = dualVector[i];
                var finiteValueToAdd = dualValue.FinitePart;
                var bigValueToAdd = dualValue.BigPart;
                finiteValueToAdd = this.coeffsField.Multiply(finiteValueToAdd, constraintsVectorValue);
                bigValueToAdd = this.coeffsField.Multiply(bigValueToAdd, constraintsVectorValue);
                finiteCost = this.coeffsField.Add(finiteCost, finiteValueToAdd);
                bigCost = this.coeffsField.Add(bigCost, bigValueToAdd);
            }

            finiteCost = this.coeffsField.Add(finiteCost, dataCost.FinitePart);
            bigCost = this.coeffsField.Add(bigCost, dataCost.BigPart);
            if (this.coeffsField.IsAdditiveUnity(bigCost))
            {
                return new SimplexOutput<CoeffType>(solution, finiteCost, true);
            }
            else
            {
                // A solução corresponde a um valor inifinito para o custo.
                return new SimplexOutput<CoeffType>(solution, default(CoeffType), false);
            }
        }

        /// <summary>
        /// Calcula o vector dos termos independentes actual.
        /// </summary>
        /// <param name="inverseMatrix">A matriz inversa.</param>
        /// <param name="constraintsVector">O vector dos termos independentes original.</param>
        /// <param name="result">O contentor para os resultados.</param>
        private void ComputeConstraintsVector(
            ISquareMatrix<CoeffType> inverseMatrix,
            IVector<CoeffType> constraintsVector,
            CoeffType[] result)
        {
            var basicLength = inverseMatrix.GetLength(0);
            var inverseSparseMatrix = inverseMatrix as ISparseMatrix<CoeffType>;
            if (inverseSparseMatrix == null)
            {
                Parallel.For(0, basicLength, i =>
                {
                    var sum = this.coeffsField.AdditiveUnity;
                    for (int j = 0; j < basicLength; ++j)
                    {
                        var valueToAdd = constraintsVector[j];
                        valueToAdd = this.coeffsField.Multiply(valueToAdd, inverseMatrix[i, j]);
                        sum = this.coeffsField.Add(sum, valueToAdd);
                    }

                    result[i] = sum;
                });
            }
            else
            {
                Parallel.For(0, basicLength, i =>
                {
                    result[i] = this.coeffsField.AdditiveUnity;
                });

                Parallel.ForEach(inverseSparseMatrix.GetLines(), inverseLine =>
                {
                    var sum = this.coeffsField.AdditiveUnity;
                    var columns = inverseLine.Value.GetColumns();
                    foreach (var columnKvp in columns)
                    {
                        var valueToAdd = constraintsVector[columnKvp.Key];
                        valueToAdd = this.coeffsField.Multiply(valueToAdd, columnKvp.Value);
                        sum = this.coeffsField.Add(sum, valueToAdd);
                    }

                    result[inverseLine.Key] = sum;
                });
            }
        }

        /// <summary>
        /// Calcula os coeficientes da variável de entrada e coloca-os no vector eta.
        /// </summary>
        /// <param name="enteringVariable">A variável de entrada.</param>
        /// <param name="constraintsMatrix">A matriz das restrições.</param>
        /// <param name="inverseMatrix">A matriz inversa.</param>
        /// <param name="etaVector">O vector eta.</param>
        private void ComputeVariableCoefficients(
            int enteringVariable,
            IMatrix<CoeffType> constraintsMatrix,
            ISquareMatrix<CoeffType> inverseMatrix,
            CoeffType[] etaVector)
        {
            var length = constraintsMatrix.GetLength(1);
            if (enteringVariable >= length) // A matriz das restrições contém a identidade como entrada.
            {
                var entryIndex = enteringVariable - length;

                // O vector conterá a entrada respectiva na matriz inversa.
                var sparseInverseMatrix = inverseMatrix as ISparseMatrix<CoeffType>;
                length = etaVector.Length;
                if (sparseInverseMatrix == null)
                {
                    Parallel.For(0, length, i =>
                    {
                        etaVector[i] = inverseMatrix[i, entryIndex];
                    });
                }
                else
                {
                    Parallel.For(0, length, i =>
                    {
                        etaVector[i] = this.coeffsField.AdditiveUnity;
                    });

                    foreach (var line in sparseInverseMatrix.GetLines())
                    {
                        etaVector[line.Key] = line.Value[entryIndex];
                    }
                }
            }
            else
            {
                length = inverseMatrix.GetLength(0);
                var sparseContraintsMatrix = constraintsMatrix as ISparseMatrix<CoeffType>;
                if (sparseContraintsMatrix == null)
                {
                    var sparseInverseMatrix = inverseMatrix as ISparseMatrix<CoeffType>;
                    if (sparseInverseMatrix == null)
                    {
                        Parallel.For(0, length, i =>
                        {
                            var sum = this.coeffsField.AdditiveUnity;
                            for (int j = 0; j < length; ++j)
                            {
                                var valueToAdd = inverseMatrix[i, j];
                                valueToAdd = this.coeffsField.Multiply(
                                    valueToAdd,
                                    constraintsMatrix[j, enteringVariable]);
                                sum = this.coeffsField.Add(sum, valueToAdd);
                            }

                            etaVector[i] = sum;
                        });
                    }
                    else
                    {
                        Parallel.For(0, length, i =>
                        {
                            etaVector[i] = this.coeffsField.AdditiveUnity;
                        });

                        var inverseMatrixLines = sparseInverseMatrix.GetLines();
                        Parallel.ForEach(inverseMatrixLines, lineKvp =>
                        {
                            var sum = this.coeffsField.AdditiveUnity;
                            foreach (var columnKvp in lineKvp.Value.GetColumns())
                            {
                                var valueToAdd = constraintsMatrix[columnKvp.Key, enteringVariable];
                                valueToAdd = this.coeffsField.Multiply(valueToAdd, columnKvp.Value);
                                sum = this.coeffsField.Add(sum, valueToAdd);
                            }

                            etaVector[lineKvp.Key] = sum;
                        });
                    }
                }
                else
                {
                    var sparseInverseMatrix = inverseMatrix as ISparseMatrix<CoeffType>;
                    if (sparseInverseMatrix == null)
                    {
                        Parallel.For(0, length, i =>
                        {
                            var sum = this.coeffsField.AdditiveUnity;
                            var constraintsMatrixLines = sparseContraintsMatrix.GetLines();
                            foreach (var constraintsLine in constraintsMatrixLines)
                            {
                                var j = constraintsLine.Key;
                                var valueToAdd = default(CoeffType);
                                if (constraintsLine.Value.TryGetColumnValue(enteringVariable, out valueToAdd))
                                {
                                    valueToAdd = this.coeffsField.Multiply(valueToAdd, inverseMatrix[i, j]);
                                    sum = this.coeffsField.Add(sum, valueToAdd);
                                }
                            }

                            etaVector[i] = sum;
                        });
                    }
                    else
                    {
                        Parallel.For(0, length, i =>
                        {
                            etaVector[i] = this.coeffsField.AdditiveUnity;
                        });

                        Parallel.ForEach(sparseInverseMatrix.GetLines(), inverseMatrixLine =>
                        {
                            var sum = this.coeffsField.AdditiveUnity;
                            var constraintsMatrixLines = sparseContraintsMatrix.GetLines();
                            foreach (var constraintsLine in constraintsMatrixLines)
                            {
                                var j = constraintsLine.Key;
                                var inverseValue = default(CoeffType);
                                if (inverseMatrixLine.Value.TryGetColumnValue(j, out inverseValue))
                                {
                                    var constraintsValue = default(CoeffType);
                                    if (constraintsLine.Value.TryGetColumnValue(enteringVariable, out constraintsValue))
                                    {
                                        var valueToAdd = this.coeffsField.Multiply(inverseValue, constraintsValue);
                                        sum = this.coeffsField.Add(sum, valueToAdd);
                                    }
                                }
                            }

                            etaVector[inverseMatrixLine.Key] = sum;
                        });
                    }
                }
            }
        }
    }
}
