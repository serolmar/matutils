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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Organiza as combinações de acordo com o número de bits ligados.
        /// </summary>
        /// <param name="data">O conjunto das combinações.</param>
        /// <returns>A lista de combinações organizadas de acordo com o número de bits ligados.</returns>
        private List<List<ImplicantLine>> GetSortedCombinations(BooleanMinimalFormInOut data)
        {
            var result = new List<List<ImplicantLine>>();
            for (int i = 0; i < result.Count; ++i)
            {
                result[i] = new List<ImplicantLine>();
            }

            foreach (var combination in data)
            {
                if (combination.LogicOutput == EBooleanMinimalFormOutStatus.ON)
                {
                    var onBitsNumber = combination.LogicInput.CountElementsWithValue(
                        EBooleanMinimalFormOutStatus.ON);
                    var implicantLine = new ImplicantLine()
                    {
                        IsPrimeImplicant = false
                    };

                    implicantLine.TableTuples.Add(combination.LogicInput);
                    result[onBitsNumber].Add(implicantLine);
                }
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
                    var tempImplicantList = new List<List<ImplicantLine>>();
                    var currentCombination = innerImplicantList[0];
                    for (int i = 1; i < innerImplicantList.Count; ++i)
                    {
                        var nextCombination = innerImplicantList[i];
                        for (int j = 0; j < currentCombination.Count; ++j)
                        {
                            for (int k = 0; k < nextCombination.Count; ++k)
                            {
                                // TODO: completar...
                            }
                        }

                        currentCombination = nextCombination;
                    }

                    innerImplicantList = tempImplicantList;
                }
            }

            return result;
        }

        /// <summary>
        /// Representa uma linha da tabela que auxilia a obtenção de implicantes primos.
        /// </summary>
        private class ImplicantLine
        {
            private List<LogicCombinationBitArray> tableTuples;

            private bool isPrimeImplicant;

            public ImplicantLine()
            {
                this.tableTuples = new List<LogicCombinationBitArray>();
            }

            public List<LogicCombinationBitArray> TableTuples
            {
                get
                {
                    return this.tableTuples;
                }
            }

            public bool IsPrimeImplicant
            {
                get
                {
                    return this.isPrimeImplicant;
                }
                set
                {
                    this.isPrimeImplicant = value;
                }
            }
        }
    }
}
