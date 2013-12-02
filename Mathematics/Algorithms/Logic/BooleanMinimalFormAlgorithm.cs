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
                throw new NotImplementedException();
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
                    if (!markUsedDataImplicants[i] &&
                        MathFunctions.CountSettedBits(implicantMatrix[i]) == 1)
                    {
                        // Procura pelo índice do item não nulo


                        state = true;
                    }
                }

            } while (state);

            return result;
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
                    var innerIndex = 16;
                    var rank = 16;
                    var mask = 1 << innerIndex;
                    var coverMask = mask - 1;
                    while ((currentValue & mask) == 0)
                    {
                        if ((currentValue & coverMask) == 0)
                        {
                            innerIndex += rank;
                            currentValue >>= rank;
                        }
                        else
                        {
                            innerIndex -= rank;
                        }

                        rank >>= 1;
                        mask >>= rank;
                        coverMask >>= rank;
                    }

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
        /// Representa uma linha da tabela que auxilia a obtenção de implicantes primos.
        /// </summary>
        private class ImplicantLine
        {
            private List<int> tableTuples;

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
    }
}
