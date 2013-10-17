namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo que permite factorizar polinómios cujos coeficientes são elementos
    /// de um corpo finito.
    /// </summary>
    public class FiniteFieldPolFactorizationAlgorithm
        : IAlgorithm<UnivariatePolynomialNormalForm<int, ModularIntegerField>,
        int,
        Dictionary<UnivariatePolynomialNormalForm<int, ModularIntegerField>, int>>
    {
        /// <summary>
        /// O algoritmo que permite obter a factorização em polinómios livres de quadrados.
        /// </summary>
        IAlgorithm<UnivariatePolynomialNormalForm<int, ModularIntegerField>,
        Dictionary<int, UnivariatePolynomialNormalForm<int, ModularIntegerField>>> squareFreeFactorizationAlg;

        private IAlgorithm<IMatrix<int>, IMatrix<int>, LinearSystemSolution<int>> linearSystemSolver;

        public FiniteFieldPolFactorizationAlgorithm(
            IAlgorithm<UnivariatePolynomialNormalForm<int, ModularIntegerField>,
            Dictionary<int, UnivariatePolynomialNormalForm<int, ModularIntegerField>>> squareFreeFactorizationAlg,
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
        /// <param name="order">A ordem do corpo finito a ser considerado.</param>
        /// <returns>
        /// O dicionário que contém cada um dos factores e o respectivo grau.
        /// </returns>
        public Dictionary<UnivariatePolynomialNormalForm<int, ModularIntegerField>, int> Run(
            UnivariatePolynomialNormalForm<int, ModularIntegerField> polynom,
            int order)
        {
            var result = new Dictionary<UnivariatePolynomialNormalForm<int, ModularIntegerField>, int>();
            var squareFreeFactors = this.squareFreeFactorizationAlg.Run(polynom);
            foreach (var factorsKvp in squareFreeFactors)
            {
                var factored = this.Factorize(factorsKvp.Value, order);
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
        /// <returns>A lista dos factores.</returns>
        private List<UnivariatePolynomialNormalForm<int, ModularIntegerField>> Factorize(
            UnivariatePolynomialNormalForm<int, ModularIntegerField> polynom,
            int order)
        {
            var result = new List<UnivariatePolynomialNormalForm<int, ModularIntegerField>>();
            if (polynom.Degree < 2)
            {
                result.Add(polynom);
            }
            else
            {
                var integerModule = new ModularIntegerField(order);
                var polynomField = new UnivarPolynomEuclideanDomain<int, ModularIntegerField>("x", integerModule);
                var bachetBezourAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<int, ModularIntegerField>,
                                UnivarPolynomEuclideanDomain<int, ModularIntegerField>>(polynomField);

                var module = new ModularBachetBezoutField<UnivariatePolynomialNormalForm<int, ModularIntegerField>>(
                    polynom,
                    bachetBezourAlg);

                var degree = polynom.Degree;
                var arrayMatrix = new ArrayMatrix<int>(degree, degree, polynom.Ring.AdditiveUnity);
                arrayMatrix[0, 0] = polynom.Ring.MultiplicativeUnity;
                var pol = new UnivariatePolynomialNormalForm<int, ModularIntegerField>(
                    polynom.Ring.MultiplicativeUnity,
                    1,
                    "x",
                    polynom.Ring);
                pol = MathFunctions.Power(pol, order, module);
                foreach (var term in pol)
                {
                    arrayMatrix[1, term.Key] = term.Value;
                }

                var auxPol = pol;
                for (int i = 2; i < degree; ++i)
                {
                    auxPol = module.Multiply(auxPol, pol);
                    foreach (var term in pol)
                    {
                        arrayMatrix[1, term.Key] = term.Value;
                    }
                }

                arrayMatrix = arrayMatrix.Subtract(ArrayMatrix<int>.GetIdentity<ModularIntegerField>(
                    degree,
                    integerModule), integerModule);
                var emtpyMatrix = new ZeroMatrix<int, ModularIntegerField>(degree, 1, integerModule);
                var linearSystemSolution = this.linearSystemSolver.Run(arrayMatrix, emtpyMatrix);

                var numberOfFactors = degree - linearSystemSolution.VectorSpaceBasis.Count;
                var polynomialStack = new Stack<UnivariatePolynomialNormalForm<int, ModularIntegerField>>();



                polynomialStack.Push(polynom);
                while (numberOfFactors > 0)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a representação polinomial a partir de um vector da base do espaço nulo.
        /// </summary>
        /// <param name="matrix">A matriz.</param>
        /// <param name="module">O corpo responsável pelas operações sobre os ceoficientes do polinómio.</param>
        /// <returns>O polinómio.</returns>
        private UnivariatePolynomialNormalForm<int, ModularIntegerField> GetPolynomial(
            IMatrix<int> matrix,
            ModularIntegerField module)
        {
            var matrixDimension = matrix.GetLength(0);
            var temporaryDic = new Dictionary<int, int>();
            for (int i = 0; i < matrixDimension; ++i)
            {
                temporaryDic.Add(i, matrix[i, 0]);
            }

            return new UnivariatePolynomialNormalForm<int, ModularIntegerField>(
                temporaryDic,
                "x",
                module);
        }
    }
}
