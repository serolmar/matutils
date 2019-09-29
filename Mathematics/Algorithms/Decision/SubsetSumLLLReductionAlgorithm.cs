namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo da soma dos subconjuntos com o auxílio do algoritmo LLL.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo do coeficiente.</typeparam>
    /// <typeparam name="NearestCoeffFieldType">O tipo do corpo a ser utilizado em conjunção com o algoritmo.</typeparam>
    public class SubsetSumLLLReductionAlgorithm<CoeffType, NearestCoeffFieldType>
        : IAlgorithm<CoeffType[], CoeffType, NearestCoeffFieldType, CoeffType[]>
    {
        /// <summary>
        /// O objecto responsável pela determinação do valor mais próximo.
        /// </summary>
        IField<NearestCoeffFieldType> nearestField;

        /// <summary>
        /// O conversor entre objectos.
        /// </summary>
        IConversion<CoeffType, NearestCoeffFieldType> converter;

        /// <summary>
        /// A fábrica responsável pela criação de vectores.
        /// </summary>
        IMathVectorFactory<NearestCoeffFieldType> vectorFactory;

        /// <summary>
        /// O objecto responsável pelo produto escalar.
        /// </summary>
        IScalarProductSpace<IMathVector<NearestCoeffFieldType>, NearestCoeffFieldType> scalarProd;

        /// <summary>
        /// O comparador de objectos que pertencem ao corpo.
        /// </summary>
        IComparer<NearestCoeffFieldType> fieldComparer;

        /// <summary>
        /// O objecto responsável pela determinação dos valores mais próximos.
        /// </summary>
        INearest<NearestCoeffFieldType, NearestCoeffFieldType> nearest;

        /// <summary>
        /// Instancia um novo objecto do tipo 
        /// <see cref="SubsetSumLLLReductionAlgorithm{CoeffType, NearestCoeffFieldType}"/>.
        /// </summary>
        /// <param name="vectorFactory">A fábrica responsável pela criação de vectores.</param>
        /// <param name="scalarProd">O objecto responsável pelos produtos escalares.</param>
        /// <param name="nearest">O objecto responsável pela determinação dos valores mais próximos.</param>
        /// <param name="fieldComparer">O comparador de objectos do corpo.</param>
        /// <param name="converter">O conversor de objectos.</param>
        /// <param name="nearestField">O objecto responsável pela determinação dos elementos mais próximos.</param>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public SubsetSumLLLReductionAlgorithm(
            IMathVectorFactory<NearestCoeffFieldType> vectorFactory,
            IScalarProductSpace<IMathVector<NearestCoeffFieldType>, NearestCoeffFieldType> scalarProd,
            INearest<NearestCoeffFieldType, NearestCoeffFieldType> nearest,
            IComparer<NearestCoeffFieldType> fieldComparer,
            IConversion<CoeffType, NearestCoeffFieldType> converter,
            IField<NearestCoeffFieldType> nearestField)
        {
            if (nearestField == null)
            {
                throw new ArgumentNullException("nearestField");
            }
            else if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }
            else if (fieldComparer == null)
            {
                throw new ArgumentNullException("fieldComparer");
            }
            else if (nearest == null)
            {
                throw new ArgumentNullException("nearest");
            }
            else if (scalarProd == null)
            {
                throw new ArgumentNullException("scalarProd");
            }
            else if (vectorFactory == null)
            {
                throw new ArgumentNullException("vectorFactory");
            }
            else
            {
                this.vectorFactory = vectorFactory;
                this.scalarProd = scalarProd;
                this.nearest = nearest;
                this.fieldComparer = fieldComparer;
                this.converter = converter;
                this.nearestField = nearestField;
            }
        }

        /// <summary>
        /// Permite encontrar uma solução do problema da soma dos subconjunto.
        /// </summary>
        /// <remarks>
        /// Dado um conjunto de valores A={a[1], a[2], a[3], ...}, encontrar um subconjunto B contido em A,
        /// B={a[b1], a[b2], ...} de tal forma que a sua soma a[b1]+a[b2]+... seja menor ou igual que o valor.
        /// O algoritmo implementado recorre à redução LLL.
        /// </remarks>
        /// <param name="coefficientValues">O conjunto de valores.</param>
        /// <param name="sum">A soma a ser encontrada.</param>
        /// <param name="reductionCoeff">O coeficiente de redução.</param>
        /// <returns>O subconjunto do conjunto inicial cuja soma mais se aproxima do valor fornecido.</returns>
        /// <exception cref="ArgumentNullException">Se algums dos argumentos for nulo.</exception>
        public CoeffType[] Run(
            CoeffType[] coefficientValues,
            CoeffType sum,
            NearestCoeffFieldType reductionCoeff)
        {
            if (coefficientValues == null)
            {
                throw new ArgumentNullException("coefficientValues");
            }
            else if (sum == null)
            {
                throw new ArgumentNullException("sum");
            }
            else
            {
                // Elabora o algoritmo
                if (coefficientValues.Length > 0)
                {
                    var vectorSpace = new VectorSpace<NearestCoeffFieldType>(
                        coefficientValues.Length + 1,
                        this.vectorFactory,
                        this.nearestField);

                    var lllReductionAlg = new LLLBasisReductionAlgorithm<IMathVector<NearestCoeffFieldType>, NearestCoeffFieldType, CoeffType>(
                        vectorSpace,
                        this.scalarProd,
                        this.nearest,
                        this.fieldComparer
                        );

                    // Constrói o conjunto de vectores a serem reduzidos.
                    var vectorSet = new IMathVector<NearestCoeffFieldType>[coefficientValues.Length + 1];
                    for (int i = 0; i < coefficientValues.Length; ++i)
                    {
                        var createdVector = this.vectorFactory.CreateVector(
                            coefficientValues.Length + 1,
                            this.nearestField.AdditiveUnity);
                        createdVector[i] = this.nearestField.MultiplicativeUnity;
                        createdVector[coefficientValues.Length] = this.nearestField.AdditiveInverse(
                            this.converter.InverseConversion(coefficientValues[i]));
                        vectorSet[i] = createdVector;
                    }

                    var lastVectorValue = this.vectorFactory.CreateVector(
                        coefficientValues.Length + 1,
                        this.nearestField.AdditiveUnity);
                    lastVectorValue[lastVectorValue.Length - 1] = this.converter.InverseConversion(sum);
                    vectorSet[coefficientValues.Length] = lastVectorValue;

                    var reduced = lllReductionAlg.Run(
                        vectorSet,
                        reductionCoeff);
                    var result = this.GetSolution(coefficientValues, reduced);
                    return result;
                }
                else
                {
                    // O tamanho do vector de coeficientes é zero.
                    return new CoeffType[0];
                }
            }
        }

        /// <summary>
        /// Obtém a solução.
        /// </summary>
        /// <param name="coeffs">Os coeficientes.</param>
        /// <param name="possibleSolutions">As soluções possíveis.</param>
        /// <returns>A solução.</returns>
        private CoeffType[] GetSolution(
            CoeffType[] coeffs,
            IMathVector<NearestCoeffFieldType>[] possibleSolutions)
        {
            var result = default(CoeffType[]);
            var lastPosition = coeffs.Length;
            var solutionItems = new List<CoeffType>();
            for (int i = 0; i < possibleSolutions.Length; ++i)
            {
                var currentCandidate = possibleSolutions[i];
                var lastPositionValue = currentCandidate[lastPosition];
                if (this.nearestField.IsAdditiveUnity(lastPositionValue))
                {
                    var foundSolution = true;
                    for (int j = 0; j < lastPosition; ++j)
                    {
                        var currentValue = currentCandidate[j];
                        if (this.nearestField.IsMultiplicativeUnity(currentValue))
                        {
                            solutionItems.Add(coeffs[j]);
                        }
                        else if (!this.nearestField.IsAdditiveUnity(currentValue))
                        {
                            solutionItems.Clear();
                            foundSolution = false;
                            j = lastPosition;
                        }
                    }

                    if (foundSolution)
                    {
                        result = solutionItems.ToArray();
                        i = possibleSolutions.Length;
                    }
                }
            }

            return result;
        }
    }
}
