namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Algoritmo que permite determinar os factores f' e g' de um polinómio p = f'g'
    /// módulo m^k dada a factorização p=fg módulo m.
    /// </summary>
    public class LinearLiftAlgorithm
        : IAlgorithm<
        LinearLiftingStatus<int>,
        int,
        bool>
    {
        /// <summary>
        /// Aplica o lema do levantamento para elevar a factorização módulo m um número superior m'.
        /// </summary>
        /// <remarks>
        /// Não é realizada qualquer verificação da integridade dos dados associados aos parâmetros de entrada.
        /// Caso estes não sejam iniciados convenientemente, os resultados obtidos poderão não estar correctos.
        /// </remarks>
        /// <param name="status">Contém os dados de entrada.</param>
        /// <param name="modulusLimit">O limite superior exculsivo para o módulo m'.</param>
        /// <returns>Verdadeiro caso seja executada alguma iteração e falso caso contrário.</returns>
        public bool Run(
            LinearLiftingStatus<int> status,
            int iterationsNumber)
        {
            var polynomialDomain = new UnivarPolynomEuclideanDomain<int>(
                status.Polynom.VariableName,
                status.ModularField);
            var polynomialRing = new UnivarPolynomRing<int>(status.Polynom.VariableName, status.MainDomain);
            var result = this.Initialize(status, polynomialDomain, polynomialRing);

            var k = 0;
            if (!polynomialRing.IsAdditiveUnity(status.EPol) &&
                k < iterationsNumber)
            {
                do
                {
                    var c = status.EPol.ApplyQuo(status.LiftedFactorizationModule, status.MainDomain);
                    var sigmaTild = polynomialDomain.Multiply(status.SPol, c);
                    var tauTild = polynomialDomain.Multiply(status.TPol, c);
                    var sigmaQuoRemResult = polynomialDomain.GetQuotientAndRemainder(sigmaTild, status.InnerW1Factor);

                    var sigma = sigmaQuoRemResult.Remainder;
                    var tau = polynomialDomain.Add(
                        tauTild,
                        polynomialDomain.Multiply(sigmaQuoRemResult.Quotient, status.InnerU1Factor));

                    status.UFactor = polynomialRing.Add(
                        status.UFactor,
                        tau.Multiply(status.ModularField.Module, status.MainDomain));
                    status.WFactor = polynomialRing.Add(
                        status.WFactor,
                        sigma.Multiply(status.ModularField.Module, status.MainDomain));
                    status.EPol = polynomialRing.Add(
                        status.InnerPolynom,
                        polynomialRing.Multiply(status.UFactor, status.WFactor));
                    ++k;
                } while (!polynomialRing.IsAdditiveUnity(status.EPol) &&
                k < iterationsNumber);
            }

            return result;
        }

        /// <summary>
        /// Inicialia o estado do algoritmo caso seja aplicável.
        /// </summary>
        /// <param name="status">O estado a ser tratado.</param>
        /// <param name="polynomialDomain">O domínio polinomial relativamente ao módulo.</param>
        /// <param name="polynomialRing">O anel polinomial habitual.</param>
        /// <returns>Verdadeiro caso se verifique alguma inicialização e falso caso contrário.</returns>
        private bool Initialize(
            LinearLiftingStatus<int> status,
            UnivarPolynomEuclideanDomain<int> polynomialDomain,
            UnivarPolynomRing<int> polynomialRing)
        {
            var result = false;
            if (status == null)
            {
                throw new ArgumentNullException("status");
            }
            else if (status.NotInitialized)
            {
                result = true;
                var alpha = status.Polynom.GetLeadingCoefficient(status.MainDomain);
                status.Gamma = alpha;
                status.InnerPolynom = status.Polynom.Multiply(alpha, status.MainDomain);

                var leadingCoeff = status.U1Factor.GetLeadingCoefficient(status.MainDomain);
                var normalized = status.ModularField.Multiply(
                    alpha,
                    status.ModularField.MultiplicativeInverse(leadingCoeff));
                status.InnerU1Factor = status.U1Factor.Multiply(normalized, status.ModularField);

                leadingCoeff = status.W1Factor.GetLeadingCoefficient(status.MainDomain);
                normalized = status.ModularField.Multiply(
                    alpha,
                    status.ModularField.MultiplicativeInverse(leadingCoeff));
                status.InnerW1Factor = status.W1Factor.Multiply(normalized, status.ModularField);

                var domainAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<int>>(polynomialDomain);
                var domainResult = domainAlg.Run(status.U1Factor, status.W1Factor);

                var invGcd = status.ModularField.MultiplicativeInverse(
                    domainResult.GreatestCommonDivisor.GetAsValue(polynomialDomain.Field));
                status.SPol = domainResult.FirstFactor.Multiply(
                    invGcd,
                    status.ModularField);
                status.TPol = domainResult.SecondFactor.Multiply(
                    invGcd,
                    status.ModularField);

                status.UFactor = status.InnerU1Factor.ReplaceLeadingCoeff(status.Gamma, status.MainDomain);
                status.WFactor = status.InnerW1Factor.ReplaceLeadingCoeff(status.Gamma, status.MainDomain);

                var ePol = polynomialRing.Multiply(status.UFactor, status.WFactor);
                ePol = polynomialRing.Add(
                    status.InnerPolynom,
                    polynomialRing.AdditiveInverse(ePol));
                status.EPol = ePol;

                status.NotInitialized = false;
            }

            return result;
        }
    }
}
