namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;

    /// <summary>
    /// Responsável pela aplicação do algoritmo dual.
    /// </summary>
    public class DualHeuristicAlgorithm<ElementType>
        : IAlgorithm<int, SparseDictionaryMatrix<ElementType>, DualHeuristicAlgInput<ElementType>, ElementType>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os elementos.
        /// </summary>
        private IRing<ElementType> ring;

        /// <summary>
        /// Comparador dos elementos.
        /// </summary>
        private IComparer<ElementType> comparer;

        public DualHeuristicAlgorithm(IComparer<ElementType> comparer, IRing<ElementType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
                this.ring = ring;
            }
        }

        /// <summary>
        /// Aplica o algoritmo dual aos dados de entrada para um determinado número de referências.
        /// </summary>
        /// <remarks>
        /// Os dados de entrada são alterados pelo algoritmo e podem ser reutilizados em fases posteriores.
        /// O valor de gama tem de ser previamente estimado.
        /// </remarks>
        /// <param name="refsNumber">O número de referências.</param>
        /// <param name="matrix">A matriz com os custos.</param>
        /// <param name="input">Os dados de entrada.</param>
        /// <returns>O custo aproximado pela heurística.</returns>
        public ElementType Run(
            int refsNumber,
            SparseDictionaryMatrix<ElementType> matrix,
            DualHeuristicAlgInput<ElementType> input)
        {
            if (refsNumber < 1)
            {
                throw new ArgumentException("The number of references must be at least one.");
            }
            else if (matrix == null)
            {
                throw new ArgumentNullException("matrix");
            }
            else if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            else
            {
                var cbar = this.Initialize(matrix, input);
                return this.Process(refsNumber, matrix, input, cbar);
            }
        }

        /// <summary>
        /// Inicializa as variáveis tau e os valores limite para o respectivo aumento.
        /// </summary>
        /// <param name="matrix">A matriz dos custos.</param>
        /// <param name="input">O resultado da inicialização do algoritmo dual.</param>
        /// <returns>Os valores limite.</returns>
        private Dictionary<int, ElementType> Initialize(
            SparseDictionaryMatrix<ElementType> matrix,
            DualHeuristicAlgInput<ElementType> input)
        {
            input.Taus.Clear();
            var result = new Dictionary<int, ElementType>();
            var cbarForComponent = new Dictionary<int, ElementType>();
            foreach (var line in matrix.GetLines())
            {
                // Setup the tau and cbar values
                var tempTau = input.Lambdas[line.Key];
                foreach (var coveredLine in line.Value.GetColumns())
                {
                    if (coveredLine.Key != line.Key)
                    {
                        var difference = this.ring.Add(
                            input.Lambdas[coveredLine.Key],
                            this.ring.AdditiveInverse(coveredLine.Value));
                        if (this.comparer.Compare(difference, this.ring.AdditiveUnity) < 0)
                        {
                            tempTau = this.ring.Add(tempTau, difference);
                        }
                    }
                }

                if (this.comparer.Compare(input.Gamma, tempTau) < 0)
                {
                    throw new OdmpProblemException("The gamma value must be greater than every computed tau value.");
                }

                input.Taus.Add(line.Key, tempTau);
            }

            foreach (var line in matrix.GetLines())
            {
                foreach (var coveredLine in line.Value.GetColumns())
                {
                    if (coveredLine.Key != line.Key)
                    {
                        var tempCbarValue = this.ring.Add(
                            coveredLine.Value,
                            this.ring.AdditiveInverse(input.Lambdas[coveredLine.Key]));
                        if (this.comparer.Compare(this.ring.AdditiveUnity, tempCbarValue) < 0)
                        {
                            var currentCbarInCoveredVertice = this.ring.AdditiveUnity;
                            if (cbarForComponent.TryGetValue(coveredLine.Key, out currentCbarInCoveredVertice))
                            {
                                if (this.comparer.Compare(tempCbarValue, currentCbarInCoveredVertice) < 0)
                                {
                                    cbarForComponent[coveredLine.Key] = coveredLine.Value;
                                }
                            }
                            else
                            {
                                cbarForComponent.Add(coveredLine.Key, coveredLine.Value);
                            }
                        }
                    }
                }
            }

            return cbarForComponent;
        }

        /// <summary>
        /// Realiza as iterações de melhoramento sobre os dados aproximados inicialmente.
        /// </summary>
        /// <param name="refsNumber">O número de referências.</param>
        /// <param name="matrix">A matriz dos custos.</param>
        /// <param name="input">Os dados de entrada.</param>
        /// <param name="cbar">O conjunto dos valores limite.</param>
        /// <returns>O valor do custo dual.</returns>
        private ElementType Process(
            int refsNumber,
            SparseDictionaryMatrix<ElementType> matrix,
            DualHeuristicAlgInput<ElementType> input,
            Dictionary<int, ElementType> cbar)
        {
            var multiplicativeSymmetric = this.ring.AdditiveInverse(this.ring.MultiplicativeUnity);
            var delta = this.ring.AdditiveUnity;
            while (!this.ring.IsMultiplicativeUnity(delta))
            {
                foreach (var line in matrix.GetLines())
                {
                    var bigDelta = this.GetBigDelta(
                        input,
                        line,
                        matrix);

                    var currentLambda = input.Lambdas[line.Key];
                    if (cbar.ContainsKey(line.Key))
                    {
                        var value = this.ring.Add(
                            cbar[line.Key],
                            this.ring.AdditiveInverse(currentLambda));
                        if (this.comparer.Compare(value, bigDelta) < 0)
                        {
                            bigDelta = value;
                            delta = this.ring.MultiplicativeUnity;
                            cbar.Remove(line.Key);

                            // TODO: cbar = min(u que cobre line tal que o custo é maior que lambda[line]
                            foreach (var coverLine in matrix.GetLines())
                            {
                                if (coverLine.Key != line.Key)
                                {
                                    if (coverLine.Value.ContainsColumn(line.Key))
                                    {
                                        var currentCost = coverLine.Value[line.Key];
                                        var compareCost = this.ring.Add(currentLambda, bigDelta);
                                        if (this.comparer.Compare(compareCost, currentCost) < 0)
                                        {
                                            var currentCbar = this.ring.AdditiveUnity;
                                            if (cbar.TryGetValue(line.Key, out currentCbar))
                                            {
                                                if (this.comparer.Compare(currentCost, currentCbar) < 0)
                                                {
                                                    cbar[line.Key] = currentCost;
                                                }
                                            }
                                            else
                                            {
                                                cbar.Add(line.Key, currentCost);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (this.comparer.Compare(this.ring.AdditiveUnity, bigDelta) < 0)
                    {
                        foreach (var coveringLine in matrix.GetLines())
                        {
                            if (coveringLine.Key != line.Key)
                            {
                                if (coveringLine.Value.ContainsColumn(line.Key))
                                {
                                    if (this.comparer.Compare(coveringLine.Value[line.Key], currentLambda) <= 0)
                                    {
                                        input.Taus[coveringLine.Key] = this.ring.Add(
                                            input.Taus[coveringLine.Key], 
                                            bigDelta);
                                    }
                                }
                            }
                        }

                        input.Lambdas[line.Key] = this.ring.Add(input.Lambdas[line.Key], bigDelta);
                        input.Taus[line.Key] = this.ring.Add(input.Taus[line.Key], bigDelta);
                    }
                }
            }

            var result = this.ring.AdditiveUnity;
            var prod = this.ring.AddRepeated(input.Gamma, refsNumber);
            foreach (var line in matrix.GetLines())
            {
                result = this.ring.Add(result, input.Lambdas[line.Key]);
            }

            result = this.ring.Add(result, this.ring.AdditiveInverse(prod));
            return result;
        }

        /// <summary>
        /// Obtém o valor da estimativa a ser adicionada a cada um dos nós durante o algoritmo.
        /// </summary>
        /// <param name="input">Os dados de entrada.</param>
        /// <param name="line">A linha correspondente ao nó de cobertura.</param>
        /// <param name="component">A matriz dos custos.</param>
        /// <returns>O valor da estimativa.</returns>
        private ElementType GetBigDelta(
            DualHeuristicAlgInput<ElementType> input,
            KeyValuePair<int, ISparseMatrixLine<ElementType>> line,
            SparseDictionaryMatrix<ElementType> component)
        {
            var result = this.ring.Add(input.Gamma, this.ring.AdditiveInverse(input.Taus[line.Key]));
            foreach (var otherLine in component.GetLines())
            {
                if (otherLine.Key != line.Key)
                {
                    if (otherLine.Value.ContainsColumn(line.Key))
                    {
                        var currentCost = otherLine.Value[line.Key];
                        var currentLambda = input.Lambdas[line.Key];
                        if (this.comparer.Compare(currentLambda, currentCost) >= 0)
                        {
                            var difference = this.ring.Add(
                                input.Gamma,
                                this.ring.AdditiveInverse(input.Taus[otherLine.Key]));
                            if (this.comparer.Compare(difference, result) < 0)
                            {
                                result = difference;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
