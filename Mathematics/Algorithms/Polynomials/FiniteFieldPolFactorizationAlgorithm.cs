namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PolType = UnivariatePolynomialNormalForm<int>;
    using MainPolType = UnivariatePolynomialNormalForm<Fraction<int, IntegerDomain>>;

    /// <summary>
    /// Implementa o algoritmo que permite factorizar polinómios cujos coeficientes são elementos
    /// de um corpo finito.
    /// </summary>
    public class FiniteFieldPolFactorizationAlgorithm
        : IAlgorithm<PolType, IModularField<int>, Dictionary<PolType, int>>
    {
        /// <summary>
        /// O algoritmo que permite obter a factorização em polinómios livres de quadrados.
        /// </summary>
        IAlgorithm<MainPolType, IField<Fraction<int, IntegerDomain>>, Dictionary<int, MainPolType>> 
        squareFreeFactorizationAlg;

        /// <summary>
        /// O algoritmo responsável pela resolução de sistemas de equações lineares.
        /// </summary>
        private IAlgorithm<IMatrix<int>, IMatrix<int>, LinearSystemSolution<int>> linearSystemSolver;

        public FiniteFieldPolFactorizationAlgorithm(
            IAlgorithm<MainPolType, IField<Fraction<int, IntegerDomain>>, Dictionary<int, MainPolType>> squareFreeFactorizationAlg,
            IAlgorithm<IMatrix<int>, IMatrix<int>, LinearSystemSolution<int>> linearSystemSolver)
        {
            if (squareFreeFactorizationAlg == null)
            {
                throw new ArgumentNullException("squareFreeFactorizationAlg");
            }
            else if (linearSystemSolver == null)
            {
                throw new ArgumentNullException("linearSystemSolver");
            }
            else
            {
                this.squareFreeFactorizationAlg = squareFreeFactorizationAlg;
                this.linearSystemSolver = linearSystemSolver;
            }
        }

        /// <summary>
        /// Executa o algoritmo sobre polinómios com coeficientes em corpos finitos.
        /// </summary>
        /// <param name="polynom">O polinómio a ser factorizado.</param>
        /// <param name="modularField">O corpo responsável pelas operações modulares.</param>
        /// <returns>
        /// O dicionário que contém cada um dos factores e o respectivo grau.
        /// </returns>
        public Dictionary<PolType, int> Run(PolType polynom, IModularField<int> modularField)
        {
            var result = new Dictionary<PolType, int>();

            var fractionField = new FractionField<int, IntegerDomain>(new IntegerDomain());
            var polynomDomain = new UnivarPolynomEuclideanDomain<int>(
                polynom.VariableName,
                modularField);
            var bachetBezourAlg = new LagrangeAlgorithm<PolType>(polynomDomain);

            var polynomialField = new UnivarPolynomEuclideanDomain<int>(
                polynom.VariableName,
                modularField);

            var forSquareFreeFact = this.ClonePol(polynom, fractionField);
            var squareFreeFactors = this.squareFreeFactorizationAlg.Run(forSquareFreeFact, fractionField);
            foreach (var factorsKvp in squareFreeFactors)
            {
                var clonedFactor = this.CloneInvPol(factorsKvp.Value, modularField);
                var factored = this.Factorize(clonedFactor, modularField, polynomialField, bachetBezourAlg);
                foreach (var factor in factored)
                {
                    result.Add(factor, factorsKvp.Key);
                }
            }

            return result;
        }

        /// <summary>
        /// Aplica o método da factorização em corpos finitos ao polinómio simples.
        /// </summary>
        /// <param name="polynom">O polinómio simples.</param>
        /// <param name="order">A ordem do corpo finito sob consideração.</param>
        /// <param name="integerModule">O corpo responsável pelas operações sobre os coeficientes.</param>
        /// <param name="polynomField">O corpo responsável pelo produto de polinómios.</param>
        /// <param name="bachetBezoutAlgorithm">O objecto responsável pelo algoritmo de máximo divisor comum.</param>
        /// <returns>A lista dos factores.</returns>
        private List<PolType> Factorize(
            PolType polynom,
            IModularField<int> integerModule,
            UnivarPolynomEuclideanDomain<int> polynomField,
            LagrangeAlgorithm<PolType> bachetBezoutAlgorithm)
        {
            var result = new List<PolType>();

            var polynomialStack = new Stack<PolType>();
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

            var factorsProduct = result[0].GetLeadingCoefficient(integerModule);
            for (int i = 1; i < result.Count; ++i)
            {
                var leadingMon = result[i].GetLeadingCoefficient(integerModule);
                factorsProduct = integerModule.Multiply(factorsProduct, leadingMon);
            }

            if (!integerModule.IsMultiplicativeUnity(factorsProduct))
            {
                result.Insert(0, new PolType(factorsProduct, 0, polynom.VariableName, integerModule));
            }

            return result;
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
        List<PolType> Process(
            PolType polynom,
            List<PolType> result,
            IModularField<int> integerModule,
            UnivarPolynomEuclideanDomain<int> polynomField,
            LagrangeAlgorithm<PolType> inverseAlgorithm)
        {
            var resultPol = new List<PolType>();
            if (polynom.Degree < 2)
            {
                result.Add(polynom);
            }
            else
            {

                var module = new ModularBachetBezoutField<PolType>(
                    polynom,
                    inverseAlgorithm);

                var degree = polynom.Degree;
                var arrayMatrix = new ArrayMatrix<int>(degree, degree, integerModule.AdditiveUnity);
                arrayMatrix[0, 0] = integerModule.AdditiveUnity;
                var pol = new PolType(
                    integerModule.MultiplicativeUnity,
                    1,
                    polynom.VariableName,
                    integerModule);
                pol = MathFunctions.Power(pol, integerModule.Module, module);
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

                var emtpyMatrix = new ZeroMatrix<int, IModularField<int>>(degree, 1, integerModule);
                var linearSystemSolution = this.linearSystemSolver.Run(arrayMatrix, emtpyMatrix);

                var numberOfFactors = linearSystemSolution.VectorSpaceBasis.Count;
                if (numberOfFactors < 2)
                {
                    result.Add(polynom);
                }
                else
                {
                    var hPol = default(PolType);
                    var linearSystemCount = linearSystemSolution.VectorSpaceBasis.Count;
                    for (int i = 0; i < linearSystemCount; ++i)
                    {
                        var currentBaseSolution = linearSystemSolution.VectorSpaceBasis[i];
                        var rowsLength = currentBaseSolution.GetLength(0);
                        for (int j = 1; j < rowsLength; ++j)
                        {
                            if (!integerModule.IsAdditiveUnity(currentBaseSolution[j, 0]))
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

                    for (int i = 0; i < numberOfFactors; ++i)
                    {
                        var currentPol = MathFunctions.GreatCommonDivisor(
                            polynom, 
                            hPol.Subtract(i, integerModule), 
                            polynomField);
                        var currentDegree = currentPol.Degree;
                        if (currentDegree == 1)
                        {
                            result.Add(currentPol);
                        }
                        else if (currentDegree > 1)
                        {
                            resultPol.Add(currentPol);
                        }
                    }
                }
            }

            return resultPol;
        }

        /// <summary>
        /// Obtém a representação polinomial a partir de um vector da base do espaço nulo.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <param name="module">O corpo responsável pelas operações sobre os ceoficientes do polinómio.</param>
        /// <param name="variableName">O nome da variável.</param>
        /// <returns>O polinómio.</returns>
        private PolType GetPolynomial(
            IMatrix<int> matrix,
            IModularField<int> module,
            string variableName)
        {
            var matrixDimension = matrix.GetLength(0);
            var temporaryDic = new Dictionary<int, int>();
            for (int i = 0; i < matrixDimension; ++i)
            {
                temporaryDic.Add(i, matrix[i, 0]);
            }

            return new PolType(
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
        private MainPolType ClonePol(PolType polynom, FractionField<int, IntegerDomain> fractionField)
        {
            var result = new MainPolType(
                polynom.VariableName);
            foreach (var termKvp in polynom)
            {
                result = result.Add(new Fraction<int, IntegerDomain>(
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
        private PolType CloneInvPol(
            MainPolType polynom,
            IModularField<int> modularIntegerField)
        {
            var result = new PolType(
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
    }
}
