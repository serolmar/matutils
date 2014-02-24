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
    /// <typeparam name="T">O tipo de variável a utilizar.</typeparam>
    public class LinearLiftAlgorithm<T> : ILinearLiftAlgorithm<T>
    {
        /// <summary>
        /// Permite efectuar todas as operações sobre o conjunto dos inteiros.
        /// </summary>
        private IIntegerNumber<T> integerNumber;

        /// <summary>
        /// Permite criar instâncias de aneis polinomiais.
        /// </summary>
        private IUnivarPolDomainFactory<T> polynomialDomainFactory;

        /// <summary>
        /// Permite criar instâncias de corpos modulares sobre os coeficientes.
        /// </summary>
        private IModularFieldFactory<T> modularFieldFactory;

        public LinearLiftAlgorithm(
            IModularFieldFactory<T> modularFieldFactory,
            IUnivarPolDomainFactory<T> polynomialDomainFactory,
            IIntegerNumber<T> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (polynomialDomainFactory == null)
            {
                throw new ArgumentNullException("polynomialDomainFactory");
            }
            else if (modularFieldFactory == null)
            {
                throw new ArgumentNullException("modularFieldFactory");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.modularFieldFactory = modularFieldFactory;
                this.polynomialDomainFactory = polynomialDomainFactory;
            }
        }

        /// <summary>
        /// Obtém a classe responsável pelas operações sobre os números inteiros.
        /// </summary>
        public IIntegerNumber<T> IntegerNumber
        {
            get
            {
                return this.integerNumber;
            }
        }

        /// <summary>
        /// Obtém a classe responsável pela obtenção de instâncias de um domínio polinomial.
        /// </summary>
        public IUnivarPolDomainFactory<T> PolynomialDomainFactory
        {
            get
            {
                return this.polynomialDomainFactory;
            }
        }

        /// <summary>
        /// Obtém a classe responsável pela obtenção de instâncias de um corpo modular.
        /// </summary>
        public IModularFieldFactory<T> ModularFieldFactory
        {
            get
            {
                return this.modularFieldFactory;
            }
        }

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
            LinearLiftingStatus<T> status,
            int iterationsNumber)
        {
            if (status == null)
            {
                throw new ArgumentNullException("status");
            }
            else if (iterationsNumber < 1)
            {
                return false;
            }
            else
            {
                var modularField = this.modularFieldFactory.CreateInstance(status.InitializedFactorizationModulus);
                var polynomialDomain = this.polynomialDomainFactory.CreateInstance(
                    status.Polynom.VariableName,
                    modularField);
                var result = this.Initialize(status, polynomialDomain, modularField);
                var k = 0;
                if (!status.FoundSolution &&
                    k < iterationsNumber)
                {
                    result = true;
                    do
                    {
                        var sigmaProd = polynomialDomain.Multiply(
                            status.SPol,
                            status.EPol);

                        var monicDivisionResult = this.GetMonicDivision(
                            sigmaProd,
                            status.WFactor,
                            modularField);

                        // Cálculo dos factores
                        var firstMultTemp = polynomialDomain.Multiply(status.TPol, status.EPol);
                        var secondMultTemp = polynomialDomain.Multiply(
                            monicDivisionResult.Quotient,
                            status.UFactor);
                        status.UFactor = polynomialDomain.Add(status.UFactor, firstMultTemp);
                        status.UFactor = polynomialDomain.Add(status.UFactor, secondMultTemp);
                        status.WFactor = polynomialDomain.Add(status.WFactor, monicDivisionResult.Remainder);

                        // Cálculo dos restantes parâmetros
                        firstMultTemp = polynomialDomain.Multiply(status.SPol, status.UFactor);
                        secondMultTemp = polynomialDomain.Multiply(status.TPol, status.WFactor);
                        var b = polynomialDomain.Add(firstMultTemp, secondMultTemp);
                        b = b.Add(
                            modularField.AdditiveInverse(modularField.MultiplicativeUnity),
                            modularField);
                        firstMultTemp = polynomialDomain.Multiply(b, status.SPol);
                        monicDivisionResult = this.GetMonicDivision(
                            firstMultTemp,
                            status.WFactor,
                            modularField);

                        status.SPol = polynomialDomain.Add(
                            status.SPol,
                            polynomialDomain.AdditiveInverse(monicDivisionResult.Remainder));
                        firstMultTemp = polynomialDomain.Multiply(status.TPol, b);
                        secondMultTemp = polynomialDomain.Multiply(status.UFactor, monicDivisionResult.Quotient);
                        firstMultTemp = polynomialDomain.Add(firstMultTemp, secondMultTemp);
                        status.TPol = polynomialDomain.Add(
                            status.TPol,
                            polynomialDomain.AdditiveInverse(firstMultTemp));

                        status.LiftedFactorizationModule = modularField.Module;
                        modularField.Module = this.integerNumber.Multiply(
                            modularField.Module,
                            modularField.Module);
                        status.InitializedFactorizationModulus = modularField.Module;

                        status.EPol = polynomialDomain.Add(
                            status.Polynom,
                            polynomialDomain.AdditiveInverse(polynomialDomain.Multiply(
                            status.UFactor,
                            status.WFactor)));
                        status.FoundSolution = polynomialDomain.IsAdditiveUnity(status.EPol);
                        ++k;
                    } while (!status.FoundSolution &&
                    k < iterationsNumber);
                }

                return result;
            }
        }

        /// <summary>
        /// Inicialia o estado do algoritmo caso seja aplicável.
        /// </summary>
        /// <param name="status">O estado a ser tratado.</param>
        /// <param name="polynomialDomain">O domínio polinomial.</param>
        /// <param name="modularField">O corpo modular.</param>
        /// <returns>Verdadeiro caso se verifique alguma inicialização e falso caso contrário.</returns>
        private bool Initialize(
            LinearLiftingStatus<T> status,
            IEuclidenDomain<UnivariatePolynomialNormalForm<T>> polynomialDomain,
            IModularField<T> modularField)
        {
            var result = false;
            if (status == null)
            {
                throw new ArgumentNullException("status");
            }
            else if (status.NotInitialized)
            {
                var leadingCoeff = status.W1Factor.GetLeadingCoefficient(modularField);
                if (modularField.IsMultiplicativeUnity(leadingCoeff))
                {
                    result = true;

                    var domainAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<T>>(
                        polynomialDomain);
                    var domainResult = domainAlg.Run(status.U1Factor, status.W1Factor);

                    var invGcd = modularField.MultiplicativeInverse(
                        domainResult.GreatestCommonDivisor.GetAsValue(modularField));
                    status.SPol = domainResult.FirstFactor.Multiply(
                        invGcd,
                        modularField);
                    status.TPol = domainResult.SecondFactor.Multiply(
                        invGcd,
                        modularField);

                    status.UFactor = status.U1Factor;
                    status.WFactor = status.W1Factor;

                    modularField.Module = this.integerNumber.Multiply(modularField.Module, modularField.Module);
                    status.InitializedFactorizationModulus = modularField.Module;
                    var ePol = polynomialDomain.Multiply(status.UFactor, status.WFactor);
                    ePol = polynomialDomain.Add(
                        status.Polynom,
                        polynomialDomain.AdditiveInverse(ePol));
                    status.EPol = ePol;

                    status.NotInitialized = false;
                }
                else
                {
                    throw new MathematicsException(
                        "The W factor in lifting algorithm must be a monic polynomial.");
                }
            }

            return result;
        }

        /// <summary>
        /// Obtém a divisão de um polinómio geral por um polinómio mónico.
        /// </summary>
        /// <param name="dividend">O polinómio geral.</param>
        /// <param name="divisor">O polinómio mónico.</param>
        /// <param name="modularField">
        /// O domínio sobre os quais as operações sobre os coeficientes são realizadas.
        /// </param>
        /// <returns>O resultado da divisão.</returns>
        private DomainResult<UnivariatePolynomialNormalForm<T>> GetMonicDivision(
            UnivariatePolynomialNormalForm<T> dividend,
            UnivariatePolynomialNormalForm<T> divisor,
            IModularField<T> modularField)
        {
            if (dividend.IsZero)
            {
                return new DomainResult<UnivariatePolynomialNormalForm<T>>(
                    dividend,
                    dividend);
            }
            else if (divisor.Degree > dividend.Degree)
            {
                return new DomainResult<UnivariatePolynomialNormalForm<T>>(
                    new UnivariatePolynomialNormalForm<T>(dividend.VariableName),
                    dividend);
            }
            else
            {
                var remainderSortedCoeffs = dividend.GetOrderedCoefficients(Comparer<int>.Default);
                var divisorSorteCoeffs = divisor.GetOrderedCoefficients(Comparer<int>.Default);
                var quotientCoeffs = new UnivariatePolynomialNormalForm<T>(dividend.VariableName);
                var remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                var divisorLeadingDegree = divisorSorteCoeffs.Keys[divisorSorteCoeffs.Keys.Count - 1];
                while (remainderLeadingDegree >= divisorLeadingDegree && remainderSortedCoeffs.Count > 0)
                {
                    var remainderLeadingCoeff = remainderSortedCoeffs[remainderLeadingDegree];
                    var differenceDegree = remainderLeadingDegree - divisorLeadingDegree;
                    quotientCoeffs = quotientCoeffs.Add(remainderLeadingCoeff, differenceDegree, modularField);
                    remainderSortedCoeffs.Remove(remainderLeadingDegree);
                    for (int i = 0; i < divisorSorteCoeffs.Keys.Count - 1; ++i)
                    {
                        var currentDivisorDegree = divisorSorteCoeffs.Keys[i];
                        var currentCoeff = modularField.Multiply(
                            divisorSorteCoeffs[currentDivisorDegree],
                            remainderLeadingCoeff);
                        currentDivisorDegree += differenceDegree;
                        var addCoeff = default(T);
                        if (remainderSortedCoeffs.TryGetValue(currentDivisorDegree, out addCoeff))
                        {
                            addCoeff = modularField.Add(
                                addCoeff,
                                modularField.AdditiveInverse(currentCoeff));
                            if (modularField.IsAdditiveUnity(addCoeff))
                            {
                                remainderSortedCoeffs.Remove(currentDivisorDegree);
                            }
                            else
                            {
                                remainderSortedCoeffs[currentDivisorDegree] = addCoeff;
                            }
                        }
                        else
                        {
                            remainderSortedCoeffs.Add(
                                currentDivisorDegree,
                                modularField.AdditiveInverse(currentCoeff));
                        }
                    }

                    if (remainderSortedCoeffs.Count > 0)
                    {
                        remainderLeadingDegree = remainderSortedCoeffs.Keys[remainderSortedCoeffs.Keys.Count - 1];
                    }
                    else
                    {
                        remainderLeadingDegree = 0;
                    }
                }

                var remainder = new UnivariatePolynomialNormalForm<T>(
                    remainderSortedCoeffs,
                    dividend.VariableName,
                    modularField);
                return new DomainResult<UnivariatePolynomialNormalForm<T>>(
                    quotientCoeffs,
                    remainder);
            }
        }
    }
}
