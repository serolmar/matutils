﻿namespace Mathematics
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
            var polynomialDomain = status.ModularPolynomialDomain;
            var polynomialRing = status.MainPolynomialRing;
            var result = this.Initialize(status);

            var k = 0;
            if (!polynomialRing.IsAdditiveUnity(status.EPol) &&
                k < iterationsNumber)
            {
                do
                {
                    var c = status.EPol.ApplyQuo(status.LiftedFactorizationModule, status.MainDomain);
                    var sigmaTild = polynomialDomain.Multiply(status.SPol, c).ApplyFunction(
                        coeff=>this.ApplyModularHomomorphism(coeff, status.ModularField.Module), status.ModularField);
                    var tauTild = polynomialDomain.Multiply(status.TPol, c).ApplyFunction(
                        coeff=>this.ApplyModularHomomorphism(coeff, status.ModularField.Module), status.ModularField);

                    var sigmaQuoRemResult = polynomialDomain.GetQuotientAndRemainder(sigmaTild, status.InnerW1Factor);

                    var sigma = sigmaQuoRemResult.Remainder;
                    sigma = sigma.ApplyFunction(
                        coeff => this.ApplyModularHomomorphism(coeff, status.ModularField.Module),
                        status.ModularField);
                    var tau = polynomialDomain.Add(
                        tauTild,
                        polynomialDomain.Multiply(sigmaQuoRemResult.Quotient, status.InnerU1Factor));
                    tau = tau.ApplyFunction(
                        coeff => this.ApplyModularHomomorphism(coeff, status.ModularField.Module),
                        status.ModularField);

                    status.UFactor = polynomialRing.Add(
                        status.UFactor,
                        tau.Multiply(status.LiftedFactorizationModule, status.MainDomain));
                    status.WFactor = polynomialRing.Add(
                        status.WFactor,
                        sigma.Multiply(status.LiftedFactorizationModule, status.MainDomain));
                    status.EPol = polynomialRing.Add(
                        status.InnerPolynom,
                        polynomialRing.AdditiveInverse(polynomialRing.Multiply(status.UFactor, status.WFactor)));

                    status.LiftedFactorizationModule *= status.ModularField.Module;
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
        /// <returns>Verdadeiro caso se verifique alguma inicialização e falso caso contrário.</returns>
        private bool Initialize(LinearLiftingStatus<int> status)
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
                status.InnerU1Factor = status.InnerU1Factor.ApplyFunction(
                    coeff => this.ApplyModularHomomorphism(coeff, status.ModularField.Module), status.ModularField);

                leadingCoeff = status.W1Factor.GetLeadingCoefficient(status.MainDomain);
                normalized = status.ModularField.Multiply(
                    alpha,
                    status.ModularField.MultiplicativeInverse(leadingCoeff));
                status.InnerW1Factor = status.W1Factor.Multiply(normalized, status.ModularField);
                status.InnerW1Factor = status.InnerW1Factor.ApplyFunction(
                    coeff => this.ApplyModularHomomorphism(coeff, status.ModularField.Module), status.ModularField);

                var domainAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<int>>(status.ModularPolynomialDomain);
                var domainResult = domainAlg.Run(status.U1Factor, status.W1Factor);

                var invGcd = status.ModularField.MultiplicativeInverse(
                    domainResult.GreatestCommonDivisor.GetAsValue(status.ModularField));
                status.SPol = domainResult.FirstFactor.Multiply(
                    invGcd,
                    status.ModularField);
                status.TPol = domainResult.SecondFactor.Multiply(
                    invGcd,
                    status.ModularField);

                status.UFactor = status.InnerU1Factor.ReplaceLeadingCoeff(status.Gamma, status.MainDomain);
                status.WFactor = status.InnerW1Factor.ReplaceLeadingCoeff(status.Gamma, status.MainDomain);

                var ePol = status.MainPolynomialRing.Multiply(status.UFactor, status.WFactor);
                ePol = status.MainPolynomialRing.Add(
                    status.InnerPolynom,
                    status.MainPolynomialRing.AdditiveInverse(ePol));
                status.EPol = ePol;

                status.NotInitialized = false;
            }

            return result;
        }

        /// <summary>
        /// Determina a imagem do valor especificado relativamente ao homomorfismo modular associado
        /// a um módulo.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <param name="modulus">O módulo.</param>
        /// <returns>O valor da imagem do homomorfismo.</returns>
        private int ApplyModularHomomorphism(int value, int modulus)
        {
            var result = value % modulus;
            if (result < 0)
            {
                var positiveResult = result + modulus;
                if (positiveResult < -result)
                {
                    result = positiveResult;
                }
            }
            else if(result > 0)
            {
                var difference = modulus - result;
                if (result > modulus - result)
                {
                    result = -difference;
                }
            }

            return result;
        }
    }
}
