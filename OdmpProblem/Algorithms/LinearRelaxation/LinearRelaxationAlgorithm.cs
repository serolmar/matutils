namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Determina a solução da relaxação linear associada a uma matriz completa.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo dos coeficientes na matriz.</typeparam>
    public class LinearRelaxationAlgorithm<CoeffType>
        : IAlgorithm<SparseDictionaryMatrix<CoeffType>, int, CoeffType[]>
    {
        /// <summary>
        /// Mantém o corpo responsável pelas operações.
        /// </summary>
        private IField<CoeffType> coeffsField;

        /// <summary>
        /// Mantém o objecto responsável pelo algoritmo do simplex.
        /// </summary>
        private IAlgorithm<SimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>>, SimplexOutput<CoeffType>>
            simplexAlg;

        private IConversion<int, CoeffType> conversion;

        public LinearRelaxationAlgorithm(
            IAlgorithm<SimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>>, SimplexOutput<CoeffType>> simplexAlg,
            IConversion<int, CoeffType> conversion,
            IField<CoeffType> coeffsField)
        {
            if (simplexAlg == null)
            {
                throw new ArgumentNullException("simplexAlg");
            }
            else if (conversion == null)
            {
                throw new ArgumentNullException("conversion");
            }
            else if (coeffsField == null)
            {
                throw new ArgumentNullException("coeffsField");
            }
            else
            {
                this.coeffsField = coeffsField;
                this.conversion = conversion;
                this.simplexAlg = simplexAlg;
            }
        }

        /// <summary>
        /// Aplica o algoritmo sobre a matriz dos custos.
        /// </summary>
        /// <param name="costs">A matriz dos custos.</param>
        /// <param name="numberOfMedians">O número de medianas a serem escolhidas.</param>
        /// <returns>O resultado da relaxação.</returns>
        public CoeffType[] Run(SparseDictionaryMatrix<CoeffType> costs, int numberOfMedians)
        {
            if (costs == null)
            {
                throw new ArgumentNullException("costs");
            }
            else
            {
                var numberOfVertices = costs.GetLength(1);
                var objectiveFunction = new List<SimplexMaximumNumberField<CoeffType>>();

                // A mediana que cobre todas as outras é ligeiramente diferente
                objectiveFunction.Add(new SimplexMaximumNumberField<CoeffType>(
                    this.coeffsField.AdditiveUnity,
                    this.coeffsField.AdditiveInverse(this.coeffsField.MultiplicativeUnity)));

                for (int i = 1; i < numberOfVertices; ++i)
                {
                    objectiveFunction.Add(new SimplexMaximumNumberField<CoeffType>(
                        this.coeffsField.AdditiveUnity,
                        this.coeffsField.AddRepeated(
                        this.coeffsField.AdditiveInverse(this.coeffsField.MultiplicativeUnity),
                        2)));
                }

                // Adiciona as variáveis x à função objectivo.
                var numberXVars = 0;
                foreach (var line in costs.GetLines())
                {
                    foreach (var column in line.Value.GetColumns())
                    {
                        if (line.Key != column.Key)
                        {
                            ++numberXVars;
                            var valueToAdd = new SimplexMaximumNumberField<CoeffType>(
                                column.Value,
                                this.coeffsField.AdditiveInverse(this.coeffsField.MultiplicativeUnity));
                            objectiveFunction.Add(valueToAdd);
                        }
                    }
                }

                // Cria a matriz das restrições e preenche-a com os valores correctos
                var constraintsNumber = numberXVars + numberOfVertices;
                var nonBasicVariables = new int[objectiveFunction.Count];
                var basicVariables = new int[constraintsNumber];
                var linesLength = nonBasicVariables.Length + basicVariables.Length;

                // Preence os vectores que permitem seleccionar as variáveis.
                this.FillVariablesSelectors(nonBasicVariables, basicVariables, linesLength);

                // Preencimento da matriz das restrições
                var constraintsMatrix = new ArrayMatrix<CoeffType>(
                    constraintsNumber,
                    constraintsNumber,
                    this.coeffsField.AdditiveUnity);

                var constraintLineNumber = 0;
                var constraintXYLineNumber = 0;
                var unity = this.coeffsField.MultiplicativeUnity;
                var inverseAdditive = this.coeffsField.AdditiveInverse(unity);
                foreach (var line in costs.GetLines())
                {
                    foreach (var column in line.Value.GetColumns())
                    {
                        constraintsMatrix[constraintXYLineNumber, constraintLineNumber] = inverseAdditive;
                        constraintsMatrix[constraintXYLineNumber, constraintXYLineNumber + numberOfVertices] = unity;
                        ++constraintXYLineNumber;
                    }

                    ++constraintLineNumber;
                }

                var lastLine = constraintsNumber - 1;
                constraintsMatrix[lastLine, 0] = unity;
                constraintLineNumber = numberXVars;
                for (int i = 1; i < numberOfVertices; ++i)
                {
                    constraintsMatrix[constraintLineNumber, i] = unity;
                    constraintsMatrix[lastLine, i] = unity;
                    constraintXYLineNumber = numberOfVertices;
                    foreach (var lines in costs.GetLines())
                    {
                        foreach (var column in lines.Value.GetColumns())
                        {
                            if (column.Key == i)
                            {
                                constraintsMatrix[constraintLineNumber, constraintXYLineNumber] = unity;
                            }

                            ++constraintXYLineNumber;
                        }
                    }

                    ++constraintLineNumber;
                }

                // Preenchimento do vector independete das restrições
                var vector = new ArrayVector<CoeffType>(constraintsNumber, this.coeffsField.AdditiveUnity);
                lastLine = constraintsNumber - 1;
                for (int i = numberXVars; i < lastLine; ++i)
                {
                    vector[i] = unity;
                }

                vector[lastLine] = this.conversion.InverseConversion(numberOfMedians);
                var sumVector = this.coeffsField.AdditiveUnity;
                for (int i = 0; i < vector.Length; ++i) {
                    sumVector = this.coeffsField.Add(sumVector, vector[i]);
                }

                sumVector = this.coeffsField.AdditiveInverse(sumVector);
                var simplexInput = new SimplexInput<CoeffType, SimplexMaximumNumberField<CoeffType>>(
                    basicVariables,
                    nonBasicVariables,
                    new ArrayVector<SimplexMaximumNumberField<CoeffType>>(objectiveFunction.ToArray()),
                    new SimplexMaximumNumberField<CoeffType>(this.coeffsField.AdditiveUnity, sumVector),
                    constraintsMatrix,
                    vector);

                var resultSimplexSol = this.simplexAlg.Run(simplexInput);
                var result = new CoeffType[numberOfVertices];
                Array.Copy(resultSimplexSol.Solution, result, numberOfVertices);
                return result;
            }
        }

        /// <summary>
        /// Estabelece os valores iniciais para os selectores das variáveis.
        /// </summary>
        /// <param name="nonBasicVariables">O selector das variáveis não-básicas.</param>
        /// <param name="basicVariables">O selector das variáveis básicas.</param>
        /// <param name="linesLength">O tamanho de uma linha de restrições.</param>
        private void FillVariablesSelectors(
            int[] nonBasicVariables,
            int[] basicVariables,
            int linesLength)
        {
            var i = 0;
            for (; i < nonBasicVariables.Length; ++i)
            {
                nonBasicVariables[i] = i;
            }

            for (int count = 0; i < linesLength; ++i, ++count)
            {
                basicVariables[count] = i;
            }
        }
    }
}
