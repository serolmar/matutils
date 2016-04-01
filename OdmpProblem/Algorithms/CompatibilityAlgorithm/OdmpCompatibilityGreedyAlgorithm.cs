namespace OdmpProblem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Mathematics;
    using Utilities;

    /// <summary>
    /// Implementa o algoritmo de compatibilidade sobre uma componente.
    /// </summary>
    public class OdmpCompatibilityGreedyAlgorithm<CostsType>
        : IAlgorithm<int, List<ILongSparseMathMatrix<CostsType>>, GreedyAlgSolution<CostsType>[]>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os custos.
        /// </summary>
        private IRing<CostsType> costsRing;

        /// <summary>
        /// O comparador responsável pela ordenação dos custos.
        /// </summary>
        private IComparer<CostsType> costsComparer;

        /// <summary>
        /// Cria uma nova instância de objectos do tipo <see cref="OdmpCompatibilityGreedyAlgorithm{CostsType}"/>.
        /// </summary>
        /// <param name="costsComparer">O comparador responsável pela ordenação dos custos.</param>
        /// <param name="costsRing">O anel responsável pelas operações sobre os custos.</param>
        public OdmpCompatibilityGreedyAlgorithm(
            IComparer<CostsType> costsComparer,
            IRing<CostsType> costsRing)
        {
            if (costsRing == null)
            {
                throw new ArgumentNullException("costsRing");
            }
            else if (costsComparer == null)
            {
                throw new ArgumentNullException("costsComparer");
            }
            else
            {
                this.costsComparer = costsComparer;
                this.costsRing = costsRing;
            }
        }

        /// <summary>
        /// Implementa o algoritmo de compatibilidade sobre um conjunto de componentes conexas.
        /// </summary>
        /// <param name="mediansNumber">O número de medianas a serem escolhidas.</param>
        /// <param name="costsMatrix">A matriz de custos.</param>
        /// <returns>O resultado da execução do algoritmo.</returns>
        public GreedyAlgSolution<CostsType>[] Run(int mediansNumber, List<ILongSparseMathMatrix<CostsType>> costsMatrix)
        {
            if (mediansNumber < costsMatrix.Count)
            {
                throw new ArgumentException(
                    "The number of medians to choose must be at least equal to the number of components.");
            }
            if (costsMatrix == null)
            {
                throw new ArgumentNullException("costsMatrix");
            }
            else if (costsMatrix.Count == 0)
            {
                throw new ArgumentException("No costs matrix was provided.");
            }
            else
            {
                var nodesNumber = this.CountNodes(costsMatrix);
                List<InsertionSortedCollection<CoordsElement>> sortedCosts =
                    new List<InsertionSortedCollection<CoordsElement>>();
                List<BitArraySymmetricMatrix> compatibilityMatrices = new List<BitArraySymmetricMatrix>();
                for (int i = 0; i < costsMatrix.Count; ++i)
                {
                    var costsComparer = new CoordsElementComparer(this.costsComparer);
                    var insertedSorted = new InsertionSortedCollection<CoordsElement>(costsComparer, false);
                    var compatibilityMatrix = this.GetCompatibilityMatrix(costsMatrix[i], insertedSorted);
                    compatibilityMatrices.Add(compatibilityMatrix);
                    sortedCosts.Add(insertedSorted);
                }

                var chosenNumber = nodesNumber - mediansNumber;
                var chosenIndices = new List<CoordsElement>[costsMatrix.Count];
                var currentBoards = new BitArray[costsMatrix.Count];
                var currentIndices = new int[costsMatrix.Count];

                // Obtém as primeiras variáveis
                for (int i = 0; i < compatibilityMatrices.Count; ++i)
                {
                    var currentCompatibilityMatrix = compatibilityMatrices[i];
                    var matrixLines = currentCompatibilityMatrix.GetLength(0);
                    var array = new BitArray(matrixLines);
                    chosenIndices[i] = new List<CoordsElement>();
                    currentBoards[i] = array;
                    currentIndices[i] = 0;
                    array[sortedCosts[i][0].CompatibilityIndex] = true;
                }

                var resultCosts = new CostsType[costsMatrix.Count];
                for (int i = 0; i < resultCosts.Length; ++i)
                {
                    resultCosts[i] = this.costsRing.AdditiveUnity;
                }

                for (int i = 0; i < chosenNumber; ++i)
                {
                    var j = 0;
                    var found = currentIndices[j++];
                    while (j < currentIndices.Length && found == -1)
                    {
                        found = currentIndices[j++];
                    }

                    if (found == -1)
                    {
                        return this.GetSolution(resultCosts, chosenIndices, costsMatrix);
                    }
                    else
                    {
                        var foundIndex = j - 1;
                        var currentSorteCosts = sortedCosts[foundIndex];
                        var minCost = currentSorteCosts[found].Value;
                        while (j < currentIndices.Length)
                        {
                            var currentIndex = currentIndices[j];
                            var currentCost = currentSorteCosts[currentIndex].Value;
                            if (this.costsComparer.Compare(currentCost, minCost) < 0)
                            {
                                foundIndex = j;
                                found = currentIndex;
                                minCost = currentCost;
                            }

                            ++j;
                        }

                        // Actualiza os valores.
                        chosenIndices[foundIndex].Add(currentSorteCosts[found]);
                        resultCosts[foundIndex] = this.costsRing.Add(resultCosts[foundIndex], minCost);
                        var currentCompatibility = compatibilityMatrices[foundIndex];
                        var currentBoard = currentBoards[foundIndex];
                        currentIndices[foundIndex] = this.GetNextVariableIndex(
                            found,
                            sortedCosts[foundIndex],
                            currentBoard,
                            currentCompatibility);
                    }
                }

                return this.GetSolution(resultCosts, chosenIndices, costsMatrix);
            }
        }

        /// <summary>
        /// Obtém a solução do problema a partir das variáveis de compatibilidade.
        /// </summary>
        /// <param name="resultCosts">Os custos por componente.</param>
        /// <param name="variables">A lista das variáveis escolhidas.</param>
        /// <param name="costsMatrices">As matrizes de custos.</param>
        /// <returns>A solução.</returns>
        private GreedyAlgSolution<CostsType>[] GetSolution(
            CostsType[] resultCosts,
            List<CoordsElement>[] variables,
            List<ILongSparseMathMatrix<CostsType>> costsMatrices)
        {
            var result = new GreedyAlgSolution<CostsType>[variables.Length];
            for (int i = 0; i < result.Length; ++i)
            {
                var integerSequence = new IntegerSequence();
                integerSequence.Add(0, costsMatrices[i].GetLength(0) - 1);
                var currentVariables = variables[i];
                for (int j = 0; j < currentVariables.Count; ++j)
                {
                    var column = currentVariables[j].Column;
                    integerSequence.Remove(column);
                }

                var algSol = new GreedyAlgSolution<CostsType>(integerSequence);
                algSol.Cost = resultCosts[i];

                result[i] = algSol;
            }

            return result;
        }

        /// <summary>
        /// Permite obter a próxima variável não compatível associada à matriz de compatibilidade.
        /// </summary>
        /// <param name="last">A última variável a ser escolhida.</param>
        /// <param name="board">A intersecção de todas as linhas de compatibilidade.</param>
        /// <param name="compatibility">A matriz de compatibilidade.</param>
        /// <returns>O índice da próxima variável e -1 caso não seja possível obtê-lo.</returns>
        private int GetNextVariableIndex(
            int last,
            InsertionSortedCollection<CoordsElement> orderedCosts,
            BitArray board,
            BitArraySymmetricMatrix compatibility)
        {
            var result = -1;
            var length = compatibility.GetLength(0);
            for (int i = last + 1; i < length; ++i)
            {
                var lineIndex = orderedCosts[i].CompatibilityIndex;
                var isNotCompatible = true;
                for (int j = 0; j < i; ++j)
                {
                    var columnIndex = orderedCosts[j].CompatibilityIndex;
                    var compatibilityItem = compatibility[lineIndex, columnIndex];
                    if (compatibilityItem && board[columnIndex])
                    {
                        isNotCompatible = false;
                        j = i;
                    }
                }

                if (isNotCompatible)
                {
                    for (int j = i + 1; j < length; ++j)
                    {
                        var columnIndex = orderedCosts[j].CompatibilityIndex;
                        var compatibilityItem = compatibility[lineIndex, columnIndex];
                        if (compatibilityItem && board[j])
                        {
                            isNotCompatible = false;
                            j = length;
                        }
                    }

                    if (isNotCompatible)
                    {
                        result = i;
                        board[orderedCosts[i].CompatibilityIndex] = true;
                        i = length;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a matriz de compatiblidade através dos custos.
        /// </summary>
        /// <param name="costsMatrix">A matriz dos custos.</param>
        /// <param name="sortedElements">
        /// O contentor para o conjunto de elementos ordenados por custo.
        /// </param>
        /// <returns>
        /// A matriz de compatibilidade ordenada de acordo como os elementos se encontram na matriz.
        /// </returns>
        private BitArraySymmetricMatrix GetCompatibilityMatrix(
            ILongSparseMathMatrix<CostsType> costsMatrix,
            InsertionSortedCollection<CoordsElement> sortedElements)
        {
            // Efectua a contagem de elementos atribuídos na matriz.
            var elementsNumber = 0;
            foreach (var line in costsMatrix.GetLines())
            {
                foreach (var column in line.Value)
                {
                    ++elementsNumber;
                }
            }

            var result = new BitArraySymmetricMatrix(elementsNumber, elementsNumber, false);
            var processedColumns = new List<CoordsElement>[costsMatrix.GetLength(0)];
            for (int i = 0; i < processedColumns.Length; ++i)
            {
                processedColumns[i] = new List<CoordsElement>();
            }

            var currentIndex = 0;
            foreach (var line in costsMatrix.GetLines())
            {
                foreach (var column in line.Value)
                {
                    if (column.Key != line.Key)
                    {
                        var element = new CoordsElement(line.Key, column.Key, currentIndex, column.Value);
                        sortedElements.Add(element);
                        var processedColumn = processedColumns[column.Key];
                        foreach (var processed in processedColumn)
                        {
                            result[processed.CompatibilityIndex, currentIndex] = true;
                        }

                        processedColumn.Add(element);

                        // Processa o pivor
                        processedColumn = processedColumns[line.Key];
                        foreach (var processed in processedColumn)
                        {
                            result[processed.CompatibilityIndex, currentIndex] = true;
                        }

                        ++currentIndex;
                    }
                }
            }
            

            return result;
        }

        /// <summary>
        /// Verifica se dois pares linha/coluna são compatíveis.
        /// </summary>
        /// <param name="first">O primeiro par.</param>
        /// <param name="second">O segundo par.</param>
        /// <returns>Verdadeiro caso os pares sejam compatíveis e falso caso contrário.</returns>
        private bool AreCompatible(CoordsElement first, CoordsElement second)
        {
            if (first.Row == second.Column)
            {
                if (second.Row < first.Row)
                {
                    return true;
                }
            }

            if (first.Column == second.Column)
            {
                if (second.Row < first.Row)
                {
                    return true;
                }
            }

            if (first.Column == second.Row)
            {
                if (second.Column > first.Column)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Obtém o número total de nós.
        /// </summary>
        /// <param name="costsMatrix">A lista dos custos por componente.</param>
        /// <returns>O número total de nós.</returns>
        private int CountNodes(List<ILongSparseMathMatrix<CostsType>> costsMatrix)
        {
            var result = 0;
            for (int i = 0; i < costsMatrix.Count; ++i)
            {
                result += costsMatrix[i].GetLength(0);
            }

            return result;
        }

        /// <summary>
        /// Representa o elemento de uma matriz de custos.
        /// </summary>
        private class CoordsElement
        {
            /// <summary>
            /// A linha associada ao elemento.
            /// </summary>
            private int row;

            /// <summary>
            /// A coluna associada ao elemento.
            /// </summary>
            private int column;

            /// <summary>
            /// O índice da linha ou coluna na matriz de compatibilidade.
            /// </summary>
            private int compatibilityIndex;

            /// <summary>
            /// O custo.
            /// </summary>
            private CostsType value;

            /// <summary>
            /// Cria uma nova instância de objectos do tipo <see cref="CoordsElement"/>.
            /// </summary>
            /// <param name="row">A linha onde se encontra o elemento.</param>
            /// <param name="column">A coluna onde se encontra o elemento.</param>
            /// <param name="compatibilityIndex">
            /// O índice da linha ou coluna na matriz de compatibilidade.
            /// </param>
            /// <param name="value">O valor associado ao elemento.</param>
            public CoordsElement(
                int row,
                int column,
                int compatibilityIndex,
                CostsType value)
            {
                this.row = row;
                this.column = column;
                this.compatibilityIndex = compatibilityIndex;
                this.value = value;
            }

            /// <summary>
            /// Obtém o número da linha onde se encontra o elemento.
            /// </summary>
            public int Row
            {
                get
                {
                    return row;
                }
            }

            /// <summary>
            /// Obtém o número da coluna onde se encontra o elemento.
            /// </summary>
            public int Column
            {
                get
                {
                    return this.column;
                }
            }

            /// <summary>
            /// Obtém ou atribui o índice da linha e coluna na matriz de compatibilidade.
            /// </summary>
            public int CompatibilityIndex
            {
                get
                {
                    return this.compatibilityIndex;
                }
            }

            /// <summary>
            /// Obtém o custo associado ao elemento.
            /// </summary>
            public CostsType Value
            {
                get
                {
                    return this.value;
                }
            }
        }

        /// <summary>
        /// Implementa um comparador de elementos.
        /// </summary>
        private class CoordsElementComparer : Comparer<CoordsElement>
        {
            /// <summary>
            /// O comparador de custos.
            /// </summary>
            private IComparer<CostsType> comparer;

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="CoordsElementComparer"/>.
            /// </summary>
            /// <param name="comparer">O compardor que permite comparar custos.</param>
            public CoordsElementComparer(IComparer<CostsType> comparer)
            {
                this.comparer = comparer;
            }

            /// <summary>
            /// Compara dois elementos.
            /// </summary>
            /// <param name="x">O primeiro elemento a ser comparado.</param>
            /// <param name="y">O segundo elemento a ser comparado.</param>
            /// <returns>
            /// O valor 1 caso o primeiro elemento seja superior ao segundo, 0 caso ambos sejam
            /// iguais e -1 caso o primeiro seja inferior ao segundo.
            /// </returns>
            public override int Compare(CoordsElement x, CoordsElement y)
            {
                return this.comparer.Compare(x.Value, y.Value);
            }
        }
    }
}
