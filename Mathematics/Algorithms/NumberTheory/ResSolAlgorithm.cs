namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo de Tonelli-Shanks que permite resolver, quando possível,
    /// a congruência x^2 = a (mod p) onde p é um número primo.
    /// </summary>
    public class ResSolAlgorithm<NumberType> : IAlgorithm<NumberType, NumberType, List<NumberType>>
    {
        /// <summary>
        /// O algoritmo responsaável pelo cálculo do símbolo de Legendre-Jacobi.
        /// </summary>
        private IAlgorithm<NumberType, NumberType, NumberType> legendreJacobiSymAlg;

        /// <summary>
        /// A fábrica responsável pela criação de objectos capazes de efectuarem operações modulares.
        /// </summary>
        private IModularFieldFactory<NumberType> modularFieldFactory;

        /// <summary>
        /// O objecto responsável pelas operações sobre números inteiros.
        /// </summary>
        private IIntegerNumber<NumberType> integerNumber;

        /// <summary>
        /// Instancia um objecto do tipo <see cref="ResSolAlgorithm{NumberType}"/>.
        /// </summary>
        /// <param name="modularFieldFactory">
        /// A fábrica responsável pela criação dos objectos responsáveis pelas operações modulares.
        /// </param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre inteiros.</param>
        /// <exception cref="ArgumentNullException">Se pelo menos um dos argumentos for nulo.</exception>
        public ResSolAlgorithm(
            IModularFieldFactory<NumberType> modularFieldFactory, 
            IIntegerNumber<NumberType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (modularFieldFactory == null)
            {
                throw new ArgumentNullException("modularFieldFactory");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.legendreJacobiSymAlg = new LegendreJacobiSymbolAlgorithm<NumberType>(integerNumber);
                this.modularFieldFactory = modularFieldFactory;
            }
        }

        /// <summary>
        /// Determina o resíduo quadrático de um número módulo o número primo ímpar especificado.
        /// </summary>
        /// <remarks>
        /// Se o módulo não for um número primo ímpar, os resultados estarão errados. Esta verificação
        /// não é realizada sobre esse módulo.
        /// </remarks>
        /// <param name="number">O número.</param>
        /// <param name="primeModule">O número primo que servirá de módulo.</param>
        /// <returns>A lista com os dois resíduos.</returns>
        /// <exception cref="ArgumentException">
        /// Se o módulo não for superior a dois, se o módulo for par ou se a solução não existir.
        /// </exception>
        public List<NumberType> Run(NumberType number, NumberType primeModule)
        {
            var two = this.integerNumber.MapFrom(2);
            if (this.integerNumber.Compare(primeModule, two) < 0)
            {
                throw new ArgumentException("The prime module must be a number greater than two.");
            }
            if (this.integerNumber.IsAdditiveUnity(this.integerNumber.Rem(primeModule, two)))
            {
                throw new ArgumentException("The prime module must be an even number.");
            }
            else if (!this.integerNumber.IsMultiplicativeUnity(this.legendreJacobiSymAlg.Run(number, primeModule)))
            {
                throw new ArgumentException("Solution doesn't exist.");
            }
            else
            {
                var innerNumber = this.integerNumber.Rem(number, primeModule);
                var firstStepModule = this.integerNumber.Predecessor(primeModule);
                var power = this.integerNumber.AdditiveUnity;
                var remQuoResult = this.integerNumber.GetQuotientAndRemainder(firstStepModule, two);
                while (this.integerNumber.IsAdditiveUnity(remQuoResult.Remainder))
                {
                    power = this.integerNumber.Successor(power);
                    firstStepModule = remQuoResult.Quotient;
                    remQuoResult = this.integerNumber.GetQuotientAndRemainder(firstStepModule, two);
                }

                var modularIntegerField = this.modularFieldFactory.CreateInstance(primeModule);
                if (this.integerNumber.IsMultiplicativeUnity(power))
                {
                    var tempPower = this.integerNumber.Successor(primeModule);
                    tempPower = this.integerNumber.Quo(tempPower, this.integerNumber.MapFrom(4));
                    var value = MathFunctions.Power(number, tempPower, modularIntegerField, this.integerNumber);
                    var result = new List<NumberType>() { 
                        value, 
                        this.integerNumber.Add(primeModule, this.integerNumber.AdditiveInverse(value))};
                    return result;
                }
                else
                {
                    var nonQuadraticResidue = this.FindNonQuadraticResidue(primeModule);
                    var poweredNonQuadraticResult = MathFunctions.Power(
                        nonQuadraticResidue,
                        firstStepModule,
                        modularIntegerField,
                        this.integerNumber);

                    var innerPower = this.integerNumber.Successor(firstStepModule);
                    innerPower = this.integerNumber.Quo(innerPower, two);
                    var result = MathFunctions.Power(innerNumber, innerPower, modularIntegerField, this.integerNumber);
                    var temp = MathFunctions.Power(innerNumber, firstStepModule, modularIntegerField, this.integerNumber);
                    while (!this.integerNumber.IsMultiplicativeUnity(temp))
                    {
                        var lowestIndex = this.FindLowestIndex(temp, power, modularIntegerField);
                        var aux = this.SquareValue(
                            poweredNonQuadraticResult,
                            this.integerNumber.Add(power, this.integerNumber.AdditiveInverse(this.integerNumber.Successor(lowestIndex))),
                            modularIntegerField);
                        result = modularIntegerField.Multiply(result, aux);
                        aux = modularIntegerField.Multiply(aux, aux);
                        temp = modularIntegerField.Multiply(temp, aux);
                        poweredNonQuadraticResult = aux;
                        power = lowestIndex;
                    }

                    return new List<NumberType>() { 
                        result, 
                        this.integerNumber.Add(primeModule, this.integerNumber.AdditiveInverse(result ))};
                }
            }
        }

        /// <summary>
        /// Encontra um não resíduo quadrático cuja existência é matematicamente garantida.
        /// </summary>
        /// <returns>O não resíduo quadrático.</returns>
        private NumberType FindNonQuadraticResidue(NumberType primeModule)
        {
            var result = this.integerNumber.AdditiveUnity;
            var minus = this.integerNumber.MapFrom(-1);
            var i = this.integerNumber.MapFrom(2);
            for (; this.integerNumber.Compare( i, primeModule)<0; i = this.integerNumber.Successor(i))
            {
                var legendreSymbol = this.legendreJacobiSymAlg.Run(i, primeModule);
                if (this.integerNumber.Equals(legendreSymbol, minus))
                {
                    result = i;
                    i = primeModule;
                }
            }

            return result;
        }

        /// <summary>
        /// Tenta encontrar o índice i tal que temp^(2^i) seja congruente com a unidade.
        /// </summary>
        /// <param name="temp">O valor de temp.</param>
        /// <param name="upperLimit">O valor do limite superior.</param>
        /// <param name="modularField">O objecto responsável pelas operações modulares.</param>
        /// <returns>O índice procurado.</returns>
        private NumberType FindLowestIndex(NumberType temp, NumberType upperLimit, IModularField<NumberType> modularField)
        {
            var result = this.integerNumber.MapFrom(-1);
            var innerTemp = temp;
            var i = this.integerNumber.AdditiveUnity;
            for (; this.integerNumber.Compare(i, upperLimit)<0; i = this.integerNumber.Successor(i))
            {
                if (this.integerNumber.IsMultiplicativeUnity(innerTemp ))
                {
                    result = i;
                    i = upperLimit;
                }
                else
                {
                    innerTemp = modularField.Multiply(innerTemp, innerTemp);
                }
            }

            return result;
        }

        /// <summary>
        /// Quadra um valor um número especificado de vezes.
        /// </summary>
        /// <param name="value">O valor.</param>
        /// <param name="times">O número de vezes.</param>
        /// <param name="modularField">O objecto responsável pelas operações modulares.</param>
        /// <returns>O resultado.</returns>
        private NumberType SquareValue(NumberType value, NumberType times, IModularField<NumberType> modularField)
        {
            var result = value;
            var i = this.integerNumber.AdditiveUnity;
            for (; this.integerNumber.Compare(i, times)<0; i = this.integerNumber.Successor(i))
            {
                result = modularField.Multiply(result, result);
            }

            return result;
        }
    }
}
