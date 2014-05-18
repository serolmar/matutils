namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo da factorização em corpos finitos inteiros.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente.</typeparam>
    public class FiniteFieldPolFactorizationAlgorithm<CoeffType>
        : IAlgorithm<
        UnivariatePolynomialNormalForm<CoeffType>,
        IModularField<CoeffType>,
        FiniteFieldPolynomialFactorizationResult<CoeffType>>
    {
        /// <summary>
        /// O objecto responsável pelas operações sobre os números inteiros.
        /// </summary>
        private IIntegerNumber<CoeffType> integerNumber;

        /// <summary>
        /// O algoritmo responsável pela resolução de sistemas de equações lineares.
        /// </summary>
        private IAlgorithm<IMatrix<CoeffType>, IMatrix<CoeffType>, LinearSystemSolution<CoeffType>> linearSystemSolver;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="FiniteFieldPolFactorizationAlgorithm{CoeffType}"/>.
        /// </summary>
        /// <param name="linearSystemSolver">
        /// O objecto responsável pela resolução de um sistema de equações lineares.
        /// </param>
        /// <param name="integerNumber">O ojecto responsável pelas operações sobre os números inteiros.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Se algum dos argumentos for nulo.
        /// </exception>
        public FiniteFieldPolFactorizationAlgorithm(
            IAlgorithm<IMatrix<CoeffType>, IMatrix<CoeffType>, LinearSystemSolution<CoeffType>> linearSystemSolver,
            IIntegerNumber<CoeffType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (linearSystemSolver == null)
            {
                throw new ArgumentNullException("linearSystemSolver");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.linearSystemSolver = linearSystemSolver;
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre o polinómio com coeficientes inteiros.
        /// </summary>
        /// <remarks>
        /// O polinómio tem de ser livre de quadrados. Caso contrário, o resultado da aplicação do algoritmo
        /// é imprevisível.
        /// </remarks>
        /// <param name="polymomial">O polinómio.</param>
        /// <param name="modularField">O corpo modular.</param>
        /// <returns>A factorização do polinómio.</returns>
        public FiniteFieldPolynomialFactorizationResult<CoeffType> Run(
            UnivariatePolynomialNormalForm<CoeffType> polymomial,
            IModularField<CoeffType> modularField)
        {
            var fractionField = new FractionField<CoeffType>(this.integerNumber);
            var polynomDomain = new UnivarPolynomEuclideanDomain<CoeffType>(
                polymomial.VariableName,
                modularField);
            var bachetBezourAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<CoeffType>>(polynomDomain);

            var polynomialField = new UnivarPolynomEuclideanDomain<CoeffType>(
                polymomial.VariableName,
                modularField);

            var result = this.Factorize(
                    polymomial,
                    modularField,
                    polynomialField,
                    bachetBezourAlg);

            return result;
        }

        /// <summary>
        /// Aplica o método da factorização em corpos finitos ao polinómio simples.
        /// </summary>
        /// <param name="polynom">O polinómio simples.</param>
        /// <param name="integerModule">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <param name="polynomField">O corpo responsável pelo produto de polinómios.</param>
        /// <param name="bachetBezoutAlgorithm">O objecto responsável pelo algoritmo de máximo divisor comum.</param>
        /// <returns>A lista dos factores.</returns>
        private FiniteFieldPolynomialFactorizationResult<CoeffType> Factorize(
            UnivariatePolynomialNormalForm<CoeffType> polynom,
            IModularField<CoeffType> integerModule,
            UnivarPolynomEuclideanDomain<CoeffType> polynomField,
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
                        c => integerModule.Multiply(c, invLeadingMon),
                        integerModule);
                }
            }

            var mainLeadingMon = polynom.GetLeadingCoefficient(integerModule);
            return new FiniteFieldPolynomialFactorizationResult<CoeffType>(
                mainLeadingMon,
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

                var integerModuleValue = this.integerNumber.ConvertToInt(integerModule.Module);

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
                        var converted = this.integerNumber.MapFrom(i);
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
    }
}
