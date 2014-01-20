namespace OdmpProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Mathematics;
    using Utilities;
    using Utilities.Collections;

    /// <summary>
    /// Permite obter uma correcção ao resultado proveniente da relaxalão linear. 
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes que corresponde à saída da relaxação linear.</typeparam>
    public class LinearRelRoundCorrectorAlg<CoeffType>
        : IAlgorithm<CoeffType[], IMatrix<CoeffType>, int, GreedyAlgSolution<CoeffType>>
    {
        /// <summary>
        /// O corpo responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IField<CoeffType> coeffsField;

        /// <summary>
        /// O object responsável pelos arredondamentos.
        /// </summary>
        private INearest<CoeffType, int> nearest;

        /// <summary>
        /// O objecto responsável pela conversão dos valores em inteiros.
        /// </summary>
        private IConversion<int, CoeffType> converter;

        private IComparer<CoeffType> comparer;

        /// <summary>
        /// Permite instanciar um objecto reponsável pela correcção da solução proveniente da relaxação linear.
        /// </summary>
        /// <param name="comparer">O comparador de custos.</param>
        /// <param name="converter">O conversor.</param>
        /// <param name="nearest">O objecto responsável pelos arredondamentos.</param>
        /// <param name="coeffsField">O corpo responsável pelas operações sobre os coeficientes.</param>
        public LinearRelRoundCorrectorAlg(
            IComparer<CoeffType> comparer,
            IConversion<int, CoeffType> converter,
            INearest<CoeffType, int> nearest,
            IField<CoeffType> coeffsField)
        {
            if (coeffsField == null)
            {
                throw new ArgumentNullException("coeffsField");
            }
            else if (nearest == null)
            {
                throw new ArgumentNullException("nearest");
            }
            else if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            else if(comparer == null){
            }
            else
            {
                this.coeffsField = coeffsField;
                this.converter = converter;
                this.nearest = nearest;
            }
        }

        /// <summary>
        /// Obtém uma solução a partir duma aproximação inicial.
        /// </summary>
        /// <param name="approximateMedians">As medianas.</param>
        /// <param name="costs">Os custos.</param>
        /// <param name="nopt">O número máximo melhoramentos a serem aplicados à solução encontrada.</param>
        /// <returns>A solução construída a partir da aproximação.</returns>
        public GreedyAlgSolution<CoeffType> Run(
            CoeffType[] approximateMedians, 
            IMatrix<CoeffType> costs,
            int nopt)
        {
            if (approximateMedians == null)
            {
                throw new ArgumentNullException("approximateMedians");
            }
            else if (costs == null)
            {
                throw new ArgumentNullException("costs");
            }
            else if (approximateMedians.Length != costs.GetLength(1))
            {
                throw new ArgumentException("The number of medians must match the number of columns in costs matrix.");
            }
            else
            {
                var settedSolutions = new IntegerSequence();
                var approximateSolutions = new List<int>();
                var sum = this.coeffsField.AdditiveUnity;
                for (int i = 0; i < approximateMedians.Length; ++i)
                {
                    var currentMedian = approximateMedians[i];
                    if (!this.coeffsField.IsAdditiveUnity(currentMedian))
                    {
                        sum = this.coeffsField.Add(sum, approximateMedians[i]);
                        if (this.converter.CanApplyDirectConversion(currentMedian))
                        {
                            var converted = this.converter.DirectConversion(currentMedian);
                            if (converted == 1)
                            {
                                settedSolutions.Add(i);
                            }
                            else
                            {
                                throw new OdmpProblemException(string.Format(
                                    "The median {0} at position {1} of medians array can't be converted to the unity.",
                                    currentMedian,
                                    i));
                            }
                        }
                        else
                        {
                            approximateSolutions.Add(i);
                        }
                    }
                }

                if (this.converter.CanApplyDirectConversion(sum))
                {
                }
                else
                {
                    throw new OdmpProblemException("The sum of medians can't be converted to an integer.");
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Utilizada para determinar uma ordenação da lista de posições que contêm valores aproximados
        /// de acordo com o valor correspondente.
        /// </summary>
        private class InnerComparer : IComparer<int>
        {
            private IComparer<CoeffType> coeffsComparer;

            private CoeffType[] coeffs;

            public InnerComparer(CoeffType[] coeffs, Comparer<CoeffType> coeffsComparer)
            {
                this.coeffsComparer = coeffsComparer;
                this.coeffs = coeffs;
            }

            /// <summary>
            /// Permite comparar dois interios conforme os coeficientes.
            /// </summary>
            /// <param name="x">O primeiro valor.</param>
            /// <param name="y">O segundo valor.</param>
            /// <returns>
            /// O valor 1 caso o primeiro seja maior do que o segundo, -1 caso o segundo seja maior do que
            /// o primeiro e 0 caso contrário.</returns>
            public int Compare(int x, int y)
            {
                var firstValue = this.coeffs[x];
                var secondValur = this.coeffs[y];

                // A comparação é invertidade de modo a proporcionar uma ordenação do maior para o menor.
                return this.coeffsComparer.Compare(secondValur, firstValue);
            }
        }
    }
}
