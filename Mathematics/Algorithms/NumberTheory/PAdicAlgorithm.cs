using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Classe que representa uma expansão p-ádica.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os coeficientes.</typeparam>
    /// <typeparam name="OrderType">O tipo de objectos que constituem as potências.</typeparam>
    class PAdicExpansion<CoeffType, OrderType>
    {
        /// <summary>
        /// Mantém o valor do número primo.
        /// </summary>
        private CoeffType prime;

        /// <summary>
        /// Mantém os coeficientes.
        /// </summary>
        private SortedDictionary<OrderType, CoeffType> coefficients;

        /// <summary>
        /// Mantém o valor da ordem.
        /// </summary>
        private OrderType order;

        /// <summary>
        /// O resto da expansão a menos da potência do número primo.
        /// </summary>
        Fraction<CoeffType> remainder;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PAdicExpansion{CoeffType, OrderType}"/>
        /// </summary>
        /// <param name="prime">O número primo.</param>
        /// <param name="order">O valor da ordem.</param>
        /// <param name="coefficients">Os coeficientes.</param>
        /// <param name="remainder">
        /// O resto da expansão a menos da potência do número primo.
        /// </param>
        public PAdicExpansion(
            CoeffType prime,
            OrderType order,
            SortedDictionary<OrderType, CoeffType> coefficients,
            Fraction<CoeffType> remainder)
        {
            if (prime == null)
            {
                throw new ArgumentNullException("prime");
            }
            else if (order == null)
            {
                throw new ArgumentNullException("order");
            }
            else if (coefficients == null)
            {
                throw new ArgumentNullException("coefficients");
            }
            else if (remainder == null)
            {
                throw new ArgumentNullException("remainder");
            }
            else
            {
                this.prime = prime;
                this.order = order;
                this.coefficients = coefficients;
                this.remainder = remainder;
            }
        }

        /// <summary>
        /// Obtém o número primo associado à expansão.
        /// </summary>
        public CoeffType Prime
        {
            get
            {
                return this.prime;
            }
        }

        /// <summary>
        /// Obtém o valor da ordem.
        /// </summary>
        public OrderType Order
        {
            get
            {
                return this.order;
            }
        }

        /// <summary>
        /// Obtém os coeficientes da expansão.
        /// </summary>
        public SortedDictionary<OrderType, CoeffType> Coefficients
        {
            get
            {
                return this.coefficients;
            }
        }

        /// <summary>
        /// Obtém o valor do resto.
        /// </summary>
        public Fraction<CoeffType> Remainder
        {
            get
            {
                return this.remainder;
            }
        }

        /// <summary>
        /// Obtém o valor da expansão.
        /// </summary>
        /// <param name="domain">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre as potências.</param>
        /// <returns>O valor da expansão.</returns>
        public Fraction<CoeffType> GetExpansionValue(
            IEuclidenDomain<CoeffType> domain,
            IIntegerNumber<OrderType> integerNumber)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else
            {
                var result = default(Fraction<CoeffType>);
                var enumerator = this.coefficients.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    var prevPower = enumerator.Current.Key;
                    var coeff = enumerator.Current.Value;
                    var primePower = default(CoeffType);
                    var state = 0;
                    if (integerNumber.Compare(prevPower, integerNumber.AdditiveUnity) < 0)
                    {
                        primePower = MathFunctions.Power(
                            prime,
                            integerNumber.AdditiveInverse(prevPower),
                            domain,
                            integerNumber);

                        result = new Fraction<CoeffType>(
                            coeff,
                            primePower,
                            domain);

                        state = 1;
                    }
                    else if (integerNumber.IsAdditiveUnity(prevPower))
                    {
                        primePower = domain.MultiplicativeUnity;

                        result = new Fraction<CoeffType>(
                            coeff,
                            domain.MultiplicativeUnity,
                            domain);
                    }
                    else
                    {
                        primePower = MathFunctions.Power(
                            prime,
                            prevPower,
                            domain,
                            integerNumber);

                        result = new Fraction<CoeffType>(
                            domain.Multiply(coeff, primePower),
                            domain.MultiplicativeUnity,
                            domain);
                    }

                    while (state != -1)
                    {
                        if (enumerator.MoveNext())
                        {
                            var currPower = enumerator.Current.Key;
                            coeff = enumerator.Current.Value;

                            if (state == 0)
                            {
                                var diff = integerNumber.Add(
                                currPower,
                                integerNumber.AdditiveInverse(prevPower));

                                var aux = MathFunctions.Power(
                                    prime,
                                    diff,
                                    domain,
                                    integerNumber);

                                primePower = domain.Multiply(
                                    primePower,
                                    aux);

                                result = result.Add(
                                    domain.Multiply(coeff, primePower),
                                    domain);
                            }
                            else if (state == 1)
                            {
                                if (integerNumber.Compare(prevPower, integerNumber.AdditiveUnity) < 0)
                                {
                                    var aux = MathFunctions.Power(
                                        prime,
                                        integerNumber.AdditiveInverse(currPower),
                                        domain,
                                        integerNumber);
                                    primePower = domain.Quo(
                                        primePower,
                                        aux);

                                    var tmp = new Fraction<CoeffType>(
                                        coeff,
                                        primePower,
                                        domain);

                                    result = result.Add(tmp, domain);
                                }
                                else if (integerNumber.IsAdditiveUnity(prevPower))
                                {
                                    primePower = domain.MultiplicativeUnity;
                                    var tmp = new Fraction<CoeffType>(
                                        coeff,
                                        domain.MultiplicativeUnity,
                                        domain);

                                    result = result.Add(tmp, domain);
                                    state = 0;
                                }
                                else
                                {
                                    primePower = MathFunctions.Power(
                                        prime,
                                        prevPower,
                                        domain,
                                        integerNumber);

                                    result = new Fraction<CoeffType>(
                                        domain.Multiply(coeff, primePower),
                                        domain.MultiplicativeUnity,
                                        domain);
                                    state = 0;
                                }
                            }

                            prevPower = currPower;
                        }
                        else
                        {
                            state = -1;
                        }
                    }

                    // Adiciona ao resto
                    if (integerNumber.Compare(this.order, integerNumber.AdditiveUnity) < 0)
                    {
                        primePower = MathFunctions.Power(
                            prime,
                            integerNumber.AdditiveInverse(this.order),
                            domain,
                            integerNumber);

                        var aux = new Fraction<CoeffType>(
                            domain.MultiplicativeUnity,
                            primePower,
                            domain);

                        aux = aux.Multiply(this.remainder, domain);

                        result = result.Add(
                            aux,
                            domain);
                    }
                    else if (integerNumber.IsAdditiveUnity(this.order))
                    {
                        result = result.Add(
                            this.remainder,
                            domain);
                    }
                    else
                    {
                        primePower = MathFunctions.Power(
                            prime,
                            this.order,
                            domain,
                            integerNumber);

                        var aux = new Fraction<CoeffType>(
                            primePower,
                            domain.MultiplicativeUnity,
                            domain);

                        aux = aux.Multiply(this.remainder, domain);

                        result = result.Add(
                            aux,
                            domain);
                    }
                }
                else
                {
                    // Estabelece o resto
                    if(integerNumber.Compare(this.order, integerNumber.AdditiveUnity) < 0)
                    {
                        var primePower = MathFunctions.Power(
                            prime,
                            integerNumber.AdditiveInverse(this.order),
                            domain,
                            integerNumber);

                        result = new Fraction<CoeffType>(
                            domain.MultiplicativeUnity,
                            primePower,
                            domain);
                    }
                    else if (integerNumber.IsAdditiveUnity(this.order))
                    {
                        result = this.remainder;
                    }
                    else
                    {
                        var primePower = MathFunctions.Power(
                            prime,
                            this.order,
                            domain,
                            integerNumber);

                        result = new Fraction<CoeffType>(
                            primePower,
                            domain.MultiplicativeUnity,
                            domain);

                        result = result.Multiply(this.remainder, domain);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a expansão normalizada quando o tipo de coeficientes é comparável.
        /// </summary>
        /// <param name="coeffsDomain">O objecto responsável pelas operações sobre os coeficientes.</param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre as potências.</param>
        /// <param name="coeffsComparer">O comparador de coeficientes.</param>
        /// <returns>A expansão normalizada.</returns>
        public PAdicExpansion<CoeffType, OrderType> GetNormalized(
            IEuclidenDomain<CoeffType> coeffsDomain,
            IIntegerNumber<OrderType> integerNumber,
            IComparer<CoeffType> coeffsComparer)
        {
            if (coeffsDomain == null)
            {
                throw new ArgumentNullException("coeffsDomain");
            }
            else if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (coeffsComparer == null)
            {
                throw new ArgumentNullException("coeffsComparer");
            }
            else
            {
                var sortedDictionaryRes = new SortedDictionary<OrderType, CoeffType>(integerNumber);
                var rem = this.remainder;
                var enumerator = this.coefficients.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    var prevPower = enumerator.Current.Key;
                    var currentCoeff = enumerator.Current.Value;

                    var addNext = false;
                    if(coeffsComparer.Compare(currentCoeff, coeffsDomain.AdditiveUnity) < 0)
                    {
                        currentCoeff = coeffsDomain.Add(currentCoeff, this.prime);
                        if (!coeffsDomain.IsAdditiveUnity(currentCoeff))
                        {
                            sortedDictionaryRes.Add(
                                prevPower,
                                currentCoeff);
                        }

                        addNext = true;
                    }
                    else
                    {
                        sortedDictionaryRes.Add(
                            prevPower,
                            currentCoeff);
                    }

                    while (enumerator.MoveNext())
                    {
                        var currentPower = enumerator.Current.Key;
                        currentCoeff = enumerator.Current.Value;

                        if (addNext)
                        {
                            prevPower = integerNumber.Successor(prevPower);
                            while(integerNumber.Compare(prevPower, currentPower) < 0)
                            {
                                sortedDictionaryRes.Add(
                                    prevPower,
                                    coeffsDomain.Add(this.prime, coeffsDomain.AdditiveInverse(coeffsDomain.MultiplicativeUnity)));
                                prevPower = integerNumber.Successor(prevPower);
                            }

                            currentCoeff = coeffsDomain.Add(
                                    currentCoeff,
                                    coeffsDomain.AdditiveInverse(coeffsDomain.MultiplicativeUnity));
                        }

                        addNext = false;
                        if (coeffsComparer.Compare(currentCoeff, coeffsDomain.AdditiveUnity) < 0)
                        {
                            currentCoeff = coeffsDomain.Add(currentCoeff, this.prime);
                            if (!coeffsDomain.IsAdditiveUnity(currentCoeff))
                            {
                                sortedDictionaryRes.Add(
                                    prevPower,
                                    currentCoeff);
                            }

                            addNext = true;
                        }
                        else
                        {
                            sortedDictionaryRes.Add(
                                prevPower,
                                currentCoeff);
                        }
                    }

                    // Actualização
                    if (addNext)
                    {
                        prevPower = integerNumber.Successor(prevPower);
                        while (integerNumber.Compare(prevPower, this.order) < 0)
                        {
                            sortedDictionaryRes.Add(
                                prevPower,
                                coeffsDomain.Add(this.prime, coeffsDomain.AdditiveInverse(coeffsDomain.MultiplicativeUnity)));
                        }

                        // Estabelece o valor do resto
                        rem = rem.Add(coeffsDomain.AdditiveInverse(coeffsDomain.MultiplicativeUnity),
                            coeffsDomain);
                    }
                }

                return new PAdicExpansion<CoeffType, OrderType>(
                    this.prime,
                    this.order,
                    sortedDictionaryRes,
                    rem);
            }
        }

        /// <summary>
        /// Obtém uma representação textual da expansão.
        /// </summary>
        /// <returns>A representação textual da expansão.</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            var enumerator = this.coefficients.GetEnumerator();
            if (enumerator.MoveNext())
            {
                stringBuilder.AppendFormat(
                    "(({0})({1})^{2})",
                    enumerator.Current.Value,
                    this.prime,
                    enumerator.Current.Key);

                while (enumerator.MoveNext())
                {
                    stringBuilder.AppendFormat(
                    " + (({0})({1})^{2})",
                    enumerator.Current.Value,
                    this.prime,
                    enumerator.Current.Key);
                }

                stringBuilder.AppendFormat(
                    " + (({0})({1})^{2})",
                    this.remainder,
                    this.prime,
                    this.order);
            }
            else
            {
                stringBuilder.AppendFormat(
                    "({0}({1})^{2})",
                    this.remainder,
                    this.prime,
                    this.order);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Determinar a igualdade com outro objecto.
        /// </summary>
        /// <param name="obj">O objecto a comparar.</param>
        /// <returns>
        /// Verdadeiro se o objecto a comparar for igual e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj == null)
            {
                return false;
            }
            else
            {
                var other = obj as PAdicExpansion<CoeffType, OrderType>;
                if (other == null)
                {
                    return false;
                }
                else
                {
                    if(!this.order.Equals(other.order))
                    {
                        return false;
                    }else if (!this.prime.Equals(other.prime))
                    {
                        return false;
                    }
                    else
                    {
                        var thisEnumerator = this.coefficients.GetEnumerator();
                        var otherEnumerator = other.coefficients.GetEnumerator();
                        var state = true;
                        while (state)
                        {
                            if (thisEnumerator.MoveNext())
                            {
                                if (otherEnumerator.MoveNext())
                                {
                                    var thisPower = thisEnumerator.Current.Key;
                                    var otherPower = otherEnumerator.Current.Key;
                                    if (thisPower.Equals(otherPower))
                                    {
                                        var thisCoeff = thisEnumerator.Current.Value;
                                        var otherCoeff = otherEnumerator.Current.Value;
                                        if (!thisCoeff.Equals(otherCoeff))
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (otherEnumerator.MoveNext())
                                {
                                    return false;
                                }
                                else
                                {
                                    state = false;
                                }
                            }
                        }

                        return true;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do objecto actual.
        /// </summary>
        /// <returns>O código confuso do objecto.</returns>
        public override int GetHashCode()
        {
            var result = 19UL;
            var enumerator = this.coefficients.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currPow = (ulong)enumerator.Current.Key.GetHashCode();
                result ^= currPow * 17;
                var currCoeff = (ulong)enumerator.Current.Value.GetHashCode();
                result ^= currCoeff * 13;
            }

            result ^= (ulong)this.prime.GetHashCode() * 23;
            result ^= (ulong)this.order.GetHashCode() * 29;
            result ^= (ulong)this.remainder.GetHashCode() * 31;
            return result.GetHashCode();
        }
    }

    /// <summary>
    /// Permite obter a expansão p-ádica de uma fracção.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os coeficientes.</typeparam>
    /// <typeparam name="OrderType">O tipo de objectos que constituem as potências.</typeparam>
    class PAdicAlgorithm<CoeffType, OrderType>
        : IAlgorithm<Fraction<CoeffType>, CoeffType, OrderType, PAdicExpansion<CoeffType, OrderType>>
    {
        /// <summary>
        /// O objecto responsável pelas operações de domínio.
        /// </summary>
        private IEuclidenDomain<CoeffType> domain;

        /// <summary>
        /// Função que retorna os coeficientes (a,b) da identidade ap+bx=mdc.
        /// </summary>
        Func<CoeffType, CoeffType, IEuclidenDomain<CoeffType>, Tuple<CoeffType, CoeffType>> gcdExt;

        /// <summary>
        /// Mantém o objecto responsável pelas operações sobre as potências.
        /// </summary>
        private IIntegerNumber<OrderType> integerNumber;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PAdicAlgorithm{CoeffType, OrderType}"/>.
        /// </summary>
        /// <param name="domain">
        /// O algoritmo que permite determinar a decomposição ax+bp=mdc(x,p).
        /// </param>
        /// <param name="gcdExt">Os coeficientes (a,b) da expansão ap+bx=mdc</param>
        /// <param name="integerNumber">
        /// O objecto responsável pelas operações sobre as potências.
        /// </param>
        public PAdicAlgorithm(
            IEuclidenDomain<CoeffType> domain,
            Func<CoeffType, CoeffType, IEuclidenDomain<CoeffType>, Tuple<CoeffType, CoeffType>> gcdExt,
            IIntegerNumber<OrderType> integerNumber)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else if (gcdExt == null)
            {
                throw new ArgumentNullException("gcdExt");
            }
            else if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else
            {
                this.domain = domain;
                this.gcdExt = gcdExt;
                this.integerNumber = integerNumber;
            }
        }

        /// <summary>
        /// Executa o algoritmo que permite determinar a expansão p-ádica de uma fracção.
        /// </summary>
        /// <param name="value">O valor do qual se pretende obter a expansão p-ádica.</param>
        /// <param name="prime">O número primo cuja primalidade não será verificada.</param>
        /// <param name="order">O número de termos na expansão.</param>
        /// <returns>A expansão p-ádica da fracção até à ordem determinada.</returns>
        public PAdicExpansion<CoeffType, OrderType> Run(
            Fraction<CoeffType> value,
            CoeffType prime,
            OrderType order)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else if (prime == null)
            {
                throw new ArgumentNullException("prime");
            }
            else if (order == null)
            {
                throw new ArgumentNullException("order");
            }
            else
            {
                var domain = this.domain;
                var gcdExt = this.gcdExt;
                var intNumber = this.integerNumber;

                // Estabelece o resultado
                var coeffs = new SortedDictionary<OrderType, CoeffType>(intNumber);

                var numerator = value.Numerator;
                var denominator = value.Denominator;
                if (domain.IsMultiplicativeUnity(denominator))
                {
                    // Número inteiro
                    var start = intNumber.AdditiveUnity;
                    var quoAndRem = domain.GetQuotientAndRemainder(numerator, prime);
                    while (domain.IsAdditiveUnity(quoAndRem.Remainder))
                    {
                        numerator = quoAndRem.Quotient;
                        start = intNumber.Successor(start);
                    }

                    // O numerador contém agora um número que não é divisível pelo número primo em questão
                    var p1 = domain.Add(
                        prime,
                        domain.AdditiveInverse(domain.MultiplicativeUnity));
                    p1 = domain.AdditiveInverse(p1);
                    var n = numerator;
                    var ind = intNumber.AdditiveUnity;
                    while (intNumber.Compare(ind, order) < 0)
                    {
                        // Código aqui
                        var a = domain.Multiply(n, p1);
                        quoAndRem = domain.GetQuotientAndRemainder(a, prime);

                        var rem = quoAndRem.Remainder;
                        if (!domain.IsAdditiveUnity(rem))
                        {
                            // Não vale a pena adicionar os nulos
                            coeffs.Add(intNumber.Add(start, ind), rem);
                        }

                        n = domain.Add(n, quoAndRem.Quotient);
                        ind = intNumber.Successor(ind);
                    }

                    return new PAdicExpansion<CoeffType, OrderType>(
                        prime,
                        order,
                        coeffs,
                        new Fraction<CoeffType>(n, denominator, domain));
                }
                else
                {
                    var state = 0;
                    var start = intNumber.AdditiveUnity;
                    while (state != -1)
                    {
                        if (state == 0)
                        {
                            var denQuoAndRem = domain.GetQuotientAndRemainder(denominator, prime);
                            var numQuoAndRem = domain.GetQuotientAndRemainder(numerator, prime);
                            if (domain.IsAdditiveUnity(denQuoAndRem.Remainder))
                            {
                                state = -1;
                            }
                            else if (domain.IsAdditiveUnity(numQuoAndRem.Remainder))
                            {
                                state = 1;
                            }
                            else
                            {
                                state = -1;
                            }
                        }
                        else if (state == 1)
                        {
                            var denQuoAndRem = domain.GetQuotientAndRemainder(denominator, prime);
                            if (domain.IsAdditiveUnity(denQuoAndRem.Remainder))
                            {
                                state = -1;
                            }
                            else
                            {
                                start = intNumber.Successor(start);
                            }
                        }
                    }

                    // O número primo já foi extraído
                    // rem(n x a, p) + (quo(n x a, p) x d + n x b) x p / d = n / d
                    var algResult = gcdExt(denominator, prime, domain);
                    var n = numerator;
                    var a = algResult.Item1;
                    var b = algResult.Item2;
                    var ind = intNumber.AdditiveUnity;
                    while (intNumber.Compare(ind, order) < 0)
                    {
                        var aux = domain.Multiply(n, a);
                        var quoAndRem = domain.GetQuotientAndRemainder(aux, prime);

                        var rem = quoAndRem.Remainder;
                        if (!domain.IsAdditiveUnity(rem))
                        {
                            // Não vale a pena adicionar os nulos
                            coeffs.Add(intNumber.Add(start, ind), rem);
                        }

                        n = domain.Add(
                            domain.Multiply(n, b),
                            domain.Multiply(quoAndRem.Quotient, denominator));
                        ind = intNumber.Successor(ind);
                    }

                    return new PAdicExpansion<CoeffType, OrderType>(
                        prime,
                        intNumber.Add(start, order),
                        coeffs,
                        new Fraction<CoeffType>(n, denominator, domain));
                }
            }
        }
    }
}
