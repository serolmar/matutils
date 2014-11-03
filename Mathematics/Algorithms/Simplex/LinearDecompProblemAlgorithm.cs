namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo paralelo baseado na decomposição.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os coeficientes.</typeparam>
    public class LinearDecompProblemAlgorithm<CoeffType> :
        IAlgorithm<LinearDecompositionInput<CoeffType, CoeffType>, LinearDecompositionOutput<CoeffType>>,
        IAlgorithm<LinearDecompositionInput<CoeffType, SimplexMaximumNumberField<CoeffType>>, LinearDecompositionOutput<CoeffType>>
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
        /// Instancia um novo objecto do tipo <see cref="SimplexAlgorithm{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffsComparer">O comparador de coeficientes.</param>
        /// <param name="coeffsField">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public LinearDecompProblemAlgorithm(IComparer<CoeffType> coeffsComparer, IField<CoeffType> coeffsField)
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
        /// Executa o algoritmo da decomposição sobre os dados de entrada.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>O resultado da execução.</returns>
        public LinearDecompositionOutput<CoeffType> Run(
            LinearDecompositionInput<CoeffType, CoeffType> data)
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
        /// Executa o algoritmo da decomposição sobre os dados de entrada tendo em consideração
        /// a noção de inteiro grande.
        /// </summary>
        /// <param name="data">Os dados de entrada.</param>
        /// <returns>O resultado da execução.</returns>
        public LinearDecompositionOutput<CoeffType> Run(
            LinearDecompositionInput<CoeffType, 
            SimplexMaximumNumberField<CoeffType>> data)
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
    }
}
