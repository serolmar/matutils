﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo que permite factorizar polinómios cujos coeficientes são elementos
    /// de um corpo finito.
    /// </summary>
    public class FiniteFieldPolFactorizationAlgorithmOld<CoeffType>
        : IAlgorithm<UnivariatePolynomialNormalForm<CoeffType>, 
        IModularField<CoeffType>, 
        IEuclidenDomain<CoeffType>,
        Dictionary<int, FiniteFieldFactorizationResultOld<CoeffType>>>
    {
        /// <summary>
        /// O algoritmo que permite obter a factorização em polinómios livres de quadrados.
        /// </summary>
        IAlgorithm<UnivariatePolynomialNormalForm<Fraction<CoeffType>>,
            IField<Fraction<CoeffType>>,
            SquareFreeFactorizationResult<Fraction<CoeffType>, Fraction<CoeffType>>> 
        squareFreeFactorizationAlg;

        /// <summary>
        /// O algoritmo responsável pela resolução de sistemas de equações lineares.
        /// </summary>
        private IAlgorithm<IMatrix<CoeffType>, IMatrix<CoeffType>, LinearSystemSolution<CoeffType>> linearSystemSolver;

        private IConversion<int, CoeffType> integerConversion;

        public FiniteFieldPolFactorizationAlgorithmOld(
            IAlgorithm<UnivariatePolynomialNormalForm<Fraction<CoeffType>>, 
            IField<Fraction<CoeffType>>,
            SquareFreeFactorizationResult<Fraction<CoeffType>, Fraction<CoeffType>>> squareFreeFactorizationAlg,
            IConversion<int, CoeffType> integerConversion,
            IAlgorithm<IMatrix<CoeffType>, IMatrix<CoeffType>, LinearSystemSolution<CoeffType>> linearSystemSolver)
        {
            if (squareFreeFactorizationAlg == null)
            {
                throw new ArgumentNullException("squareFreeFactorizationAlg");
            }
            else if (linearSystemSolver == null)
            {
                throw new ArgumentNullException("linearSystemSolver");
            }
            else if (integerConversion == null)
            {
                throw new ArgumentNullException("integerConversion");
            }
            else
            {
                this.squareFreeFactorizationAlg = squareFreeFactorizationAlg;
                this.linearSystemSolver = linearSystemSolver;
                this.integerConversion = integerConversion;
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre polinómios com coeficientes em corpos finitos.
        /// </summary>
        /// <param name="polynom">O polinómio a ser factorizado.</param>
        /// <param name="modularField">O corpo responsável pelas operações modulares.</param>
        /// <param name="coeffsDomain">O domínio associado aos coeficientes.</param>
        /// <returns>
        /// O dicionário que contém cada um dos factores e o respectivo grau.
        /// </returns>
        public Dictionary<int, FiniteFieldFactorizationResultOld<CoeffType>> Run(
            UnivariatePolynomialNormalForm<CoeffType> polynom, 
            IModularField<CoeffType> modularField,
            IEuclidenDomain<CoeffType> coeffsDomain)
        {
            var result = new Dictionary<int, FiniteFieldFactorizationResultOld<CoeffType>>();

            var fractionField = new FractionField<CoeffType>(coeffsDomain);
            var polynomDomain = new UnivarPolynomEuclideanDomain<CoeffType>(
                polynom.VariableName,
                modularField);
            var bachetBezourAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<CoeffType>>(polynomDomain);
            var coeffLagAlg = new LagrangeAlgorithm<CoeffType>(coeffsDomain);

            var polynomialField = new UnivarPolynomEuclideanDomain<CoeffType>(
                polynom.VariableName,
                modularField);

            var forSquareFreeFact = this.ClonePol(polynom, fractionField);
            var squareFreeFactors = this.squareFreeFactorizationAlg.Run(forSquareFreeFact, fractionField);
            foreach (var factorsKvp in squareFreeFactors.Factors)
            {
                var divisionCoeff = this.GetDenominatorLcm(
                    factorsKvp.Value,
                    coeffLagAlg);
                var clonedFactor = this.GetIntegerPol(
                    factorsKvp.Value,
                    divisionCoeff,
                    coeffsDomain);
                var factored = this.Factorize(
                    clonedFactor, 
                    modularField, 
                    polynomialField, 
                    divisionCoeff,
                    bachetBezourAlg);
                result.Add(factorsKvp.Key, factored);
            }

            return result;
        }

        /// <summary>
        /// Aplica o método da factorização em corpos finitos ao polinómio simples.
        /// </summary>
        /// <param name="polynom">O polinómio simples.</param>
        /// <param name="integerModule">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <param name="polynomField">O corpo responsável pelo produto de polinómios.</param>
        /// <param name="divisionCoeff">O coeficiente pelo qual o polinómio terá de ser dividido no final.</param>
        /// <param name="bachetBezoutAlgorithm">O objecto responsável pelo algoritmo de máximo divisor comum.</param>
        /// <returns>A lista dos factores.</returns>
        private FiniteFieldFactorizationResultOld<CoeffType> Factorize(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            IModularField<CoeffType> integerModule,
            UnivarPolynomEuclideanDomain<CoeffType> polynomField,
            CoeffType divisionCoeff,
            LagrangeAlgorithm<UnivariatePolynomialNormalForm<CoeffType>> bachetBezoutAlgorithm)
        {
            var result = new List<UnivariatePolynomialNormalForm<CoeffType>>();

            var polynomialStack = new Stack<UnivariatePolynomialNormalForm<CoeffType>>();
            var processedPols = this.Process(
                polynom, 
                result, 
                integerModule, 
                polynomField,
                bachetBezoutAlgorithm);

            foreach (var processedPol in processedPols)
            {
                polynomialStack.Push(processedPol);
            }

            while (polynomialStack.Count > 0)
            {
                var topPolynomial = polynomialStack.Pop();
                processedPols = this.Process(
                topPolynomial,
                result,
                integerModule,
                polynomField,
                bachetBezoutAlgorithm);

                foreach (var processedPol in processedPols)
                {
                    polynomialStack.Push(processedPol);
                }
            }

            for (int i = 0; i < result.Count; ++i)
            {
                var leadingMon = result[i].GetLeadingCoefficient(integerModule);
                if (!integerModule.IsMultiplicativeUnity(leadingMon))
                {
                    var invLeadingMon = integerModule.MultiplicativeInverse(leadingMon);
                    result[i] = result[i].ApplyFunction(
                        c=>integerModule.Multiply(c, invLeadingMon), 
                        integerModule);
                }
            }

            var mainLeadingMon = polynom.GetLeadingCoefficient(integerModule);
            return new FiniteFieldFactorizationResultOld<CoeffType>(
                mainLeadingMon,
                divisionCoeff,
                result, 
                polynom);
        }

        /// <summary>
        /// Processa o polinómio determinando os respectivos factores.
        /// </summary>
        /// <remarks>
        /// Os factores constantes são ignorados, os factores lineares são anexados ao resultado e os factores
        /// cujos graus são superiores são retornados para futuro processamento. Se o polinómio a ser processado
        /// for irredutível, é adicionado ao resultado.
        /// </remarks>
        /// <param name="polynom">O polinómio a ser processado.</param>
        /// <param name="result">O contentor dos factores sucessivamente processados.</param>
        /// <param name="integerModule">O objecto responsável pelas operações sobre inteiros.</param>
        /// <param name="polynomField">O objecto responsável pelas operações sobre os polinómios.</param>
        /// <param name="module">O objecto responsável pelas operações sobre os polinómios módulo outro polinómio.</param>
        /// <param name="inverseAlgorithm">O algoritmo inverso.</param>
        /// <returns></returns>
        List<UnivariatePolynomialNormalForm<CoeffType>> Process(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            List<UnivariatePolynomialNormalForm<CoeffType>> result,
            IModularField<CoeffType> integerModule,
            UnivarPolynomEuclideanDomain<CoeffType> polynomField,
            LagrangeAlgorithm<UnivariatePolynomialNormalForm<CoeffType>> inverseAlgorithm)
        {
            var resultPol = new List<UnivariatePolynomialNormalForm<CoeffType>>();
            if (polynom.Degree < 2)
            {
                result.Add(polynom);
            }
            else
            {

                var module = new ModularBachetBezoutField<UnivariatePolynomialNormalForm<CoeffType>>(
                    polynom,
                    inverseAlgorithm);

                var degree = polynom.Degree;
                var arrayMatrix = new ArrayMatrix<CoeffType>(degree, degree, integerModule.AdditiveUnity);
                arrayMatrix[0, 0] = integerModule.AdditiveUnity;
                var pol = new UnivariatePolynomialNormalForm<CoeffType>(
                    integerModule.MultiplicativeUnity,
                    1,
                    polynom.VariableName,
                    integerModule);

                var integerModuleValue = this.integerConversion.DirectConversion(integerModule.Module);

                pol = MathFunctions.Power(pol, integerModuleValue, module);
                foreach (var term in pol)
                {
                    arrayMatrix[term.Key, 1] = term.Value;
                }

                var auxPol = pol;
                for (int i = 2; i < degree; ++i)
                {
                    auxPol = module.Multiply(auxPol, pol);
                    foreach (var term in auxPol)
                    {
                        arrayMatrix[term.Key, i] = term.Value;
                    }
                }

                for (int i = 1; i < degree; ++i)
                {
                    var value = arrayMatrix[i, i];
                    value = integerModule.Add(
                        value,
                        integerModule.AdditiveInverse(integerModule.MultiplicativeUnity));
                    arrayMatrix[i, i] = value;
                }

                var emtpyMatrix = new ZeroMatrix<CoeffType>(degree, 1, integerModule);
                var linearSystemSolution = this.linearSystemSolver.Run(arrayMatrix, emtpyMatrix);

                var numberOfFactors = linearSystemSolution.VectorSpaceBasis.Count;
                if (numberOfFactors < 2)
                {
                    result.Add(polynom);
                }
                else
                {
                    var hPol = default(UnivariatePolynomialNormalForm<CoeffType>);
                    var linearSystemCount = linearSystemSolution.VectorSpaceBasis.Count;
                    for (int i = 0; i < linearSystemCount; ++i)
                    {
                        var currentBaseSolution = linearSystemSolution.VectorSpaceBasis[i];
                        var rowsLength = currentBaseSolution.Length;
                        for (int j = 1; j < rowsLength; ++j)
                        {
                            if (!integerModule.IsAdditiveUnity(currentBaseSolution[j]))
                            {
                                hPol = this.GetPolynomial(currentBaseSolution, integerModule, polynom.VariableName);
                                j = rowsLength;
                            }

                            if (hPol != null)
                            {
                                j = rowsLength;
                            }
                        }

                        if (hPol != null)
                        {
                            i = linearSystemCount;
                        }
                    }

                    for (int i = 0, k = 0; k < numberOfFactors && i < integerModuleValue; ++i)
                    {
                        var converted = this.integerConversion.InverseConversion(i);
                        var currentPol = MathFunctions.GreatCommonDivisor(
                            polynom, 
                            hPol.Subtract(converted, integerModule), 
                            polynomField);
                        var currentDegree = currentPol.Degree;
                        if (currentDegree == 1)
                        {
                            result.Add(currentPol);
                            ++k;
                        }
                        else if (currentDegree > 1)
                        {
                            resultPol.Add(currentPol);
                            ++k;
                        }
                    }
                }
            }

            return resultPol;
        }

        /// <summary>
        /// Obtém a representação polinomial a partir de um vector da base do espaço nulo.
        /// </summary>
        /// <param name="vector">O vector.</param>
        /// <param name="module">O corpo responsável pelas operações sobre os ceoficientes do polinómio.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <returns>O polinómio.</returns>
        private UnivariatePolynomialNormalForm<CoeffType> GetPolynomial(
            IVector<CoeffType> vector,
            IModularField<CoeffType> module,
            string variableName)
        {
            var matrixDimension = vector.Length;
            var temporaryDic = new Dictionary<int, CoeffType>();
            for (int i = 0; i < matrixDimension; ++i)
            {
                temporaryDic.Add(i, vector[i]);
            }

            return new UnivariatePolynomialNormalForm<CoeffType>(
                temporaryDic,
                variableName,
                module);
        }

        /// <summary>
        /// Obtém uma cópia do polinómio especificado alterando-lhe o tipo de anel ou corpo.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="fractionField">O corpo.</param>
        /// <returns>A cópia.</returns>
        private UnivariatePolynomialNormalForm<Fraction<CoeffType>> ClonePol(
            UnivariatePolynomialNormalForm<CoeffType> polynom, 
            FractionField<CoeffType> fractionField)
        {
            var result = new UnivariatePolynomialNormalForm<Fraction<CoeffType>>(
                polynom.VariableName);
            foreach (var termKvp in polynom)
            {
                result = result.Add(new Fraction<CoeffType>(
                    termKvp.Value,
                    fractionField.EuclideanDomain.MultiplicativeUnity,
                    fractionField.EuclideanDomain),
                    termKvp.Key, fractionField);
            }

            return result;
        }

        /// <summary>
        /// Obém uma cópia do polinómio sobre o qual o corpo é alterado.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="modularIntegerField">O corpo.</param>
        /// <returns>A cópia.</returns>
        [Obsolete("Esta função é obsoleta.")]
        private UnivariatePolynomialNormalForm<CoeffType> CloneInvPol(
            UnivariatePolynomialNormalForm<Fraction<CoeffType>> polynom,
            IModularField<CoeffType> modularIntegerField)
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType>(
                polynom.VariableName);
            foreach (var termKvp in polynom)
            {
                result = result.Add(
                    modularIntegerField.Multiply(termKvp.Value.Numerator,
                    modularIntegerField.MultiplicativeInverse(termKvp.Value.Denominator)),
                    termKvp.Key,
                    modularIntegerField);
            }

            return result;
        }

        /// <summary>
        /// Obtém a parte inteira do polinómio aós ser reduzido ao mesmo denominador.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="denominatorLcm">O mínimo múltiplo comum do denominador.</param>
        /// <param name="coeffsDomain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O polinómio com os coeficientes inteiros.</returns>
        private UnivariatePolynomialNormalForm<CoeffType> GetIntegerPol(
            UnivariatePolynomialNormalForm<Fraction<CoeffType>> polynom,
            CoeffType denominatorLcm,
            IEuclidenDomain<CoeffType> coeffsDomain)
        {
            var result = new UnivariatePolynomialNormalForm<CoeffType>(polynom.VariableName);
            if (!polynom.IsZero)
            {
                if (coeffsDomain.IsMultiplicativeUnity(denominatorLcm))
                {
                    foreach (var term in polynom)
                    {
                        result.Terms.Add(term.Key, term.Value.Numerator);
                    }
                }
                else
                {
                    foreach (var term in polynom)
                    {
                        var integerPart = term.Value.Numerator;
                        var coFactor = coeffsDomain.Quo(denominatorLcm, term.Value.Denominator);
                        if (!coeffsDomain.IsMultiplicativeUnity(coFactor))
                        {
                            integerPart = coeffsDomain.Multiply(integerPart, coFactor);
                        }

                        result.Terms.Add(term.Key, integerPart);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém o mínimo múltiplo comum entre os denominadores do polinómio.
        /// </summary>
        /// <param name="polynom">O polinómio.</param>
        /// <param name="gcdCAlg">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O valor do mínimo múltiplo comum.</returns>
        private CoeffType GetDenominatorLcm(
            UnivariatePolynomialNormalForm<Fraction<CoeffType>> polynom,
            IBachetBezoutAlgorithm<CoeffType> gcdCAlg)
        {
            var termsEnumerator = polynom.GetEnumerator();
            var state = termsEnumerator.MoveNext();
            if (state)
            {
                var coeff = termsEnumerator.Current.Value.Denominator;
                state = termsEnumerator.MoveNext();
                while (state && gcdCAlg.Domain.IsMultiplicativeUnity(coeff))
                {
                    coeff = termsEnumerator.Current.Value.Denominator;
                    state = termsEnumerator.MoveNext();
                }

                while (state)
                {
                    var current = termsEnumerator.Current.Value.Denominator;
                    if (!gcdCAlg.Domain.IsMultiplicativeUnity(current))
                    {
                        var status = gcdCAlg.Run(coeff, current);
                        coeff = gcdCAlg.Domain.Multiply(status.FirstItem, status.SecondCofactor);
                    }

                    state = termsEnumerator.MoveNext();
                }

                return coeff;
            }
            else
            {
                return gcdCAlg.Domain.MultiplicativeUnity;
            }
        }
    }
}
