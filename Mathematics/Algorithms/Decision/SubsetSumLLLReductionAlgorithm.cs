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
        IGroup<CoeffType> group;

        IField<NearestCoeffFieldType> nearestField;

        IConversion<CoeffType, NearestCoeffFieldType> converter;

        IVectorFactory<NearestCoeffFieldType> vectorFactory;

        IScalarProductSpace<IVector<NearestCoeffFieldType>, NearestCoeffFieldType> scalarProd;

        IComparer<NearestCoeffFieldType> fieldComparer;

        INearest<NearestCoeffFieldType, NearestCoeffFieldType> nearest;

        public SubsetSumLLLReductionAlgorithm(
            IVectorFactory<NearestCoeffFieldType> vectorFactory,
            IScalarProductSpace<IVector<NearestCoeffFieldType>, NearestCoeffFieldType> scalarProd,
            INearest<NearestCoeffFieldType, NearestCoeffFieldType> nearest,
            IComparer<NearestCoeffFieldType> fieldComparer,
            IConversion<CoeffType, NearestCoeffFieldType> converter,
            IField<NearestCoeffFieldType> nearestField,
            IGroup<CoeffType> group)
        {
            if (group == null)
            {
                throw new ArgumentNullException("group");
            }
            else if (nearestField == null)
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
                this.group = group;
            }
        }

        /// <summary>
        /// Permite encontrar uma solução do problema da soma dos subconjunto.
        /// </summary>
        /// <remarks>
        /// Dado um conjunto de valores A={a[1], a[2], a[3], ...}, encontrar um subconjunto B contido em A,
        /// B={a[b1], a[b2], ...} de tal forma que a sua soma a[b1]+a[b2]+... seja igual a <see cref="sum"/>.
        /// O algoritmo implementado recorre à redução LLL.
        /// </remarks>
        /// <param name="coefficientValues">O conjunto de valores.</param>
        /// <param name="sum">A soma a ser encontrada.</param>
        /// <param name="reductionCoeff">O coeficiente de redução.</param>
        /// <returns>O subconjunto do conjunto inicial cuja soma mais se aproxima do valor fornecido.</returns>
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

                    var lllReductionAlg = new LLLBasisReductionAlgorithm<IVector<NearestCoeffFieldType>, NearestCoeffFieldType, CoeffType>(
                        vectorSpace,
                        this.scalarProd,
                        this.nearest,
                        this.fieldComparer
                        );

                    // Constrói o conjunto de vectores a serem reduzidos.
                    var vectorSet = new IVector<NearestCoeffFieldType>[coefficientValues.Length + 1];
                    for (int i = 0; i < coefficientValues.Length; ++i)
                    {
                        var createdVector = this.vectorFactory.CreateVector(
                            coefficientValues.Length + 1,
                            this.nearestField.AdditiveUnity);
                        createdVector[i] = this.nearestField.MultiplicativeUnity;
                        createdVector[coefficientValues.Length] = this.converter.InverseConversion(
                            this.group.AdditiveInverse(coefficientValues[i]));
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

                }
                else
                {
                    // O tamanho do vector de coeficientes é zero.
                    return new CoeffType[0];
                }
            }

            throw new NotImplementedException();
        }
    }
}
