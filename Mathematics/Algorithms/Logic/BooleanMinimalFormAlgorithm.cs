namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities.Collections;

    /// <summary>
    /// Implementa o algoritmo habitual para simplificação de expressões lógicas colocadas na forma
    /// canónica. É útil em simplificação de esquemas electrónicos.
    /// </summary>
    public class BooleanMinimalFormAlgorithm : IAlgorithm<BooleanMinimalFormInOut, BooleanMinimalFormInOut>
    {
        /// <summary>
        /// Obtém a forma mínima para a lista de combinações lógicas fornecidas.
        /// </summary>
        /// <param name="data">A lista de combinações lógicas de entrada.</param>
        /// <returns>O resultado minimal.</returns>
        public BooleanMinimalFormInOut Run(BooleanMinimalFormInOut data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var usedDataImplicants = this.GetUsedDataImplicants(data);
                var sortedCombinations = this.GetSortedCombinations(usedDataImplicants);
                var implicantTableInfo = this.ProcessPrimeImplicantsList(sortedCombinations);
                var implicantMatrix = this.GetBitMatrixForResult(implicantTableInfo, usedDataImplicants);
                var essentialImplicants = this.Simplify(implicantTableInfo, usedDataImplicants, implicantMatrix);
                var result = new BooleanMinimalFormInOut();
                foreach (var essentialImplicant in essentialImplicants)
                {
                    result.Add(essentialImplicant, EBooleanMinimalFormOutStatus.ON);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém os implicantes usados na expressão lógica.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>O conjunto de implicantes.</returns>
        private ImplicantLine[] GetUsedDataImplicants(BooleanMinimalFormInOut data)
        {
            var result = new List<ImplicantLine>();
            for (int i = 0; i < data.Count; ++i)
            {
                var combination = data[i];
                if (combination.LogicOutput == EBooleanMinimalFormOutStatus.ON)
                {
                    var implicantLine = new ImplicantLine()
                    {
                        LineCombination = combination.LogicInput
                    };

                    implicantLine.TableTuples.Add(i);
                    result.Add(implicantLine);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Organiza as combinações de acordo com o número de bits ligados.
        /// </summary>
        /// <param name="implicants">O conjunto das combinações.</param>
        /// <returns>A lista de combinações organizadas de acordo com o número de bits ligados.</returns>
        private List<List<ImplicantLine>> GetSortedCombinations(ImplicantLine[] implicants)
        {
            var result = new List<List<ImplicantLine>>();
            for (int i = 0; i < result.Count; ++i)
            {
                result[i] = new List<ImplicantLine>();
            }

            for (int i = 0; i < implicants.Length; ++i)
            {
                var combination = implicants[i];
                var onBitsNumber = combination.LineCombination.CountElementsWithValue(
                        EBooleanMinimalFormOutStatus.ON);

                result[onBitsNumber].Add(combination);
            }

            return result;
        }

        /// <summary>
        /// Processa a lista de implicantes.
        /// </summary>
        /// <param name="implicantsList">A lista de implicantes a serem processados.</param>
        /// <returns>O resultado do processamento.</returns>
        private List<ImplicantLine> ProcessPrimeImplicantsList(List<List<ImplicantLine>> implicantsList)
        {
            var result = new List<ImplicantLine>();
            if (implicantsList.Count > 0)
            {
                var innerImplicantList = implicantsList;
                while (innerImplicantList.Count > 1)
                {
                    var groupedImplicantsList = new List<List<ImplicantLine>>();
                    var currentCombination = innerImplicantList[0];
                    for (int i = 1; i < innerImplicantList.Count; ++i)
                    {
                        var nextCombination = innerImplicantList[i];
                        var tempImplicantsList = new List<ImplicantLine>();
                        for (int j = 0; j < currentCombination.Count; ++j)
                        {
                            var upperPrimeImplicantCandidate = currentCombination[j];
                            var haveNotFoundReduction = true;
                            for (int k = j; k < nextCombination.Count; ++k)
                            {
                                var lowerPrimeImplicantCandidate = nextCombination[k];
                                var processedCombination = default(LogicCombinationBitArray);
                                if (upperPrimeImplicantCandidate.LineCombination.TryToGetReduced(
                                    lowerPrimeImplicantCandidate.LineCombination,
                                    out processedCombination))
                                {
                                    var implicantLineToAdd = new ImplicantLine()
                                    {
                                        LineCombination = processedCombination
                                    };

                                    foreach (var logicComb in upperPrimeImplicantCandidate.TableTuples)
                                    {
                                        implicantLineToAdd.TableTuples.Add(logicComb);
                                    }

                                    foreach (var logicComb in lowerPrimeImplicantCandidate.TableTuples)
                                    {
                                        implicantLineToAdd.TableTuples.Add(logicComb);
                                    }

                                    tempImplicantsList.Add(implicantLineToAdd);
                                    haveNotFoundReduction = false;
                                }

                                if (haveNotFoundReduction)
                                {
                                    result.Add(upperPrimeImplicantCandidate);
                                }
                            }

                            if (tempImplicantsList.Count > 0)
                            {
                                groupedImplicantsList.Add(tempImplicantsList);
                            }
                        }

                        currentCombination = nextCombination;
                    }

                    // O último grupo não é passível de ser simplificado
                    var last = innerImplicantList[innerImplicantList.Count - 1];
                    for (int i = 0; i < last.Count; ++i)
                    {
                        result.Add(last[i]);
                    }

                    innerImplicantList = groupedImplicantsList;
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a matriz de bits que permite simplificar a parte final dos implicantes primos.
        /// </summary>
        /// <param name="result">A tabela de implicantes primos.</param>
        /// <param name="usedDataImplicants">Os implicantes iniciais.</param>
        /// <returns>A matriz de bits representada como uma lista de vectores.</returns>
        private BitArray[] GetBitMatrixForResult(
            List<ImplicantLine> result,
            ImplicantLine[] usedDataImplicants)
        {
            var resultMatrix = new BitArray[usedDataImplicants.Length];
            for (int i = 0; i < usedDataImplicants.Length; ++i)
            {
                resultMatrix[i] = new BitArray(result.Count);
            }

            for (int i = 0; i < result.Count; ++i)
            {
                var currentPrimeImplicant = result[i];
                foreach (var tuple in currentPrimeImplicant.TableTuples)
                {
                    resultMatrix[tuple][i] = true;
                }
            }

            return resultMatrix;
        }

        /// <summary>
        /// Realiza o passo de simplificação final.
        /// </summary>
        /// <param name="implicantLines">Os implicantes primos.</param>
        /// <param name="usedDataImplicants">Os implicantes utilizados.</param>
        /// <param name="implicantMatrix">A matriz de cobertura.</param>
        /// <returns>Os implicantes primos essenciais.</returns>
        private List<LogicCombinationBitArray> Simplify(
            List<ImplicantLine> implicantLines,
            ImplicantLine[] usedDataImplicants,
            BitArray[] implicantMatrix)
        {
            var result = new List<LogicCombinationBitArray>();
            var markImplicantLines = new BitArray(implicantLines.Count);
            var markUsedDataImplicants = new BitArray(usedDataImplicants.Length);
            var state = default(bool);
            do
            {
                state = false; // Termina o ciclo caso não seja possível encontrar uma coluna única
                for (int i = 0; i < usedDataImplicants.Length; ++i)
                {
                    var implicantMatrixLine = implicantMatrix[i];
                    if (!markUsedDataImplicants[i] &&
                        MathFunctions.CountSettedBits(implicantMatrixLine) == 1)
                    {
                        // Procura pelo índice do item não nulo
                        var index = this.FindBitSettedIndex(implicantMatrixLine);

                        markUsedDataImplicants[i] = true;
                        markImplicantLines[index] = true;
                        for (int j = 0; j < usedDataImplicants.Length; ++j)
                        {
                            implicantMatrix[i][index] = false;
                        }

                        state = true;
                    }
                }

            } while (state);

            var coverMatrix = this.MountMatrix(
                result,
                implicantLines,
                implicantMatrix,
                markImplicantLines,
                markUsedDataImplicants);

            this.ProcessCoverMatrix(coverMatrix);
            for (int i = 0; i < coverMatrix.Count; ++i)
            {
                result.Add(coverMatrix.Keys[i].PrimeImplicant);
            }

            return result;
        }

        /// <summary>
        /// Constrói a matriz de cobertura numa lista ordenada.
        /// </summary>
        /// <param name="essentialPrimeImplicants">A lista dos implicantes primos resultantes.</param>
        /// <param name="primeImplicants">Os implicantes primos.</param>
        /// <param name="implicantMatrix">A matriz actual dos implicantes.</param>
        /// <param name="markImplicantLines">O vector que marca os implicantes que não vão ser considerados.</param>
        /// <param name="markUsedDataImplicants">
        /// O vector que marca os implicantes iniciais que não vão ser considerados.
        /// </param>
        /// <returns>A matriz requerida.</returns>
        private SortedList<CoverLine, bool> MountMatrix(
            List<LogicCombinationBitArray> essentialPrimeImplicants,
            List<ImplicantLine> primeImplicants,
            BitArray[] implicantMatrix,
            BitArray markImplicantLines,
            BitArray markUsedDataImplicants)
        {
            var result = new SortedList<CoverLine, bool>(new CoverLinesComparer());
            var remainingDataImplicantsNumber = markUsedDataImplicants.Length - MathFunctions.CountSettedBits(
                markUsedDataImplicants);

            for (int i = 0; i < markImplicantLines.Length; ++i)
            {
                if (markImplicantLines[i])
                {
                    essentialPrimeImplicants.Add(primeImplicants[i].LineCombination);
                }
                else
                {
                    var arrayLine = new BitArray(remainingDataImplicantsNumber);
                    var dataImplicantIndex = 0;
                    for (int j = 0; j < markUsedDataImplicants.Length; ++j)
                    {
                        if (!markUsedDataImplicants[j])
                        {
                            arrayLine[dataImplicantIndex] = implicantMatrix[j][i];
                            ++dataImplicantIndex;
                        }
                    }

                    result.Add(
                        new CoverLine() { Line = arrayLine, PrimeImplicant = primeImplicants[i].LineCombination },
                        false);
                }
            }

            return result;
        }

        /// <summary>
        /// Processa a lista ordenada.
        /// </summary>
        /// <param name="sortedList"></param>
        private void ProcessCoverMatrix(SortedList<CoverLine, bool> sortedList)
        {
            var count = sortedList.Count;
            var lastIndex = count - 1;
            for (int i = 0; i < lastIndex; ++i)
            {
                for (int j = i + 1; j < count; ++j)
                {
                    var coverKey = sortedList.Keys[i];
                    var coveredKey = sortedList.Keys[j];
                    if (this.CheckCover(coverKey.Line, coveredKey.Line))
                    {
                        sortedList.RemoveAt(j);
                        --count;
                        --lastIndex;
                    }
                }
            }
        }

        /// <summary>
        /// Tenta encontrar o índice do único bit ligado de um vector de bits.
        /// </summary>
        /// <param name="bitArray">O vector de bits.</param>
        /// <returns>O índice do bit ligado.</returns>
        private int FindBitSettedIndex(BitArray bitArray)
        {
            var index = 0;
            var integerValues = new int[(bitArray.Count >> 5) + 1];
            bitArray.CopyTo(integerValues, 0);
            integerValues[integerValues.Length - 1] &= ~(-1 << (bitArray.Count % 32));
            for (int i = 0; i < integerValues.Length; ++i)
            {
                var currentValue = integerValues[i];
                if (currentValue != 0)
                {
                    // A obteção do índice segue-se aqui
                    var innerIndex = 0;
                    var rank = 16;
                    while (currentValue != 1)
                    {
                        var tempValue = currentValue >> rank;
                        if (tempValue != 0)
                        {
                            currentValue = tempValue;
                            index += rank;
                        }

                        rank >>= 1;
                    }

                    index += innerIndex;

                    // Termina o ciclo
                    i = integerValues.Length;
                }
                else
                {
                    index += 32;
                }
            }

            return index;
        }

        /// <summary>
        /// Verifica se uma linha cobre a outra.
        /// </summary>
        /// <param name="cover">A linha de cobertura.</param>
        /// <param name="covered">A linha a cobrir.</param>
        /// <returns>Verdadeiro se a linha de cobertura cobrir a outra linha e falso caso contrário.</returns>
        private bool CheckCover(BitArray cover, BitArray covered)
        {
            var coverIntegerValues = new int[(cover.Count >> 5) + 1];
            cover.CopyTo(coverIntegerValues, 0);
            coverIntegerValues[coverIntegerValues.Length - 1] &= ~(-1 << (cover.Count % 32));

            var coveredIntegerValues = new int[(cover.Count >> 5) + 1];
            cover.CopyTo(coveredIntegerValues, 0);
            coveredIntegerValues[coveredIntegerValues.Length - 1] &= ~(-1 << (cover.Count % 32));

            for (int i = 0; i < coverIntegerValues.Length; ++i)
            {
                var coverValue = coverIntegerValues[i];
                var coveredValue = coveredIntegerValues[i];
                if ((coverValue & coveredValue) != coveredValue)
                {
                    return false;
                }
            }

            return true;
        }

        #region Classes Auxiliares

        /// <summary>
        /// Representa uma linha da tabela que auxilia a obtenção de implicantes primos.
        /// </summary>
        private class ImplicantLine
        {
            /// <summary>
            /// As colunas da tabela que foram combinadas.
            /// </summary>
            private List<int> tableTuples;

            /// <summary>
            /// A combinação final.
            /// </summary>
            private LogicCombinationBitArray lineCombination;

            public ImplicantLine()
            {
                this.tableTuples = new List<int>();
            }

            public List<int> TableTuples
            {
                get
                {
                    return this.tableTuples;
                }
            }

            public LogicCombinationBitArray LineCombination
            {
                get
                {
                    return this.lineCombination;
                }
                set
                {
                    this.lineCombination = value;
                }
            }
        }

        /// <summary>
        /// Reprsenta uma linha na matriz de cobertura utilizada no último passo do algoritmo.
        /// </summary>
        private class CoverLine
        {
            /// <summary>
            /// O implicante primo que se encontra em estudo.
            /// </summary>
            private LogicCombinationBitArray primeImplicant;

            /// <summary>
            /// A linha da matriz que corresopnde ao implicante primo.
            /// </summary>
            private BitArray line;

            public CoverLine()
            {
            }

            public LogicCombinationBitArray PrimeImplicant
            {
                get
                {
                    return this.primeImplicant;
                }
                set
                {
                    this.primeImplicant = value;
                }
            }

            public BitArray Line
            {
                get
                {
                    return this.line;
                }
                set
                {
                    this.line = value;
                }
            }
        }

        /// <summary>
        /// Permite comparar duas linhas de cobertura de modo a ser possível ordená-las
        /// da maior para a menor.
        /// </summary>
        private class CoverLinesComparer : Comparer<CoverLine>
        {
            /// <summary>
            /// Compara duas linhas de cobertura mediante o vector de bits.
            /// </summary>
            /// <param name="x">A primeira linha de cobertura a ser comparada.</param>
            /// <param name="y">A segunda linha de cobertura a ser comparada.</param>
            /// <returns>
            /// O valor 1 caso o primeiro seja posterior ao segundo, o valor zero caso sejam iguais
            /// e o valor -1 caso o primeiro seja anterior ao segundo.
            /// </returns>
            public override int Compare(CoverLine x, CoverLine y)
            {
                var length = x.Line.Length;
                var firstArray = new int[(length >> 5) + 1];
                var secondArray = new int[(length >> 5) + 1];
                x.Line.CopyTo(firstArray, 0);
                y.Line.CopyTo(secondArray, 0);
                firstArray[firstArray.Length - 1] &= ~(-1 << (length % 32));
                firstArray[secondArray.Length - 1] &= ~(-1 << (length % 32));

                for (int i = 0; i < length; ++i)
                {
                    var firstValue = firstArray[i];
                    var secondValue = secondArray[i];
                    if (firstValue < secondValue)
                    {
                        return 1;
                    }
                    else if (secondValue < firstValue)
                    {
                        return -1;
                    }
                }

                return 0;
            }
        }

        #endregion Classes Auxiliares
    }
}
