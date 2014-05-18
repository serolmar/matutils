namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite determinar o resultante entre dois polinómios recorrendo ao algoritmo do sub-resultante.
    /// </summary>
    /// <remarks>
    /// Convém notar que o algoritmo ainda não se encontra numa fase em que é possível obter o resultante
    /// entre dois quaisquer polinómios, apesar do consegui-lo na maior parte dos casos. Porém, premite
    /// calcular um múltiplo do máximo divisor comum entre dois polinómios operando directamente sobre os anéis.
    /// </remarks>
    public class UnivarPolSubResultantAlg<CoeffType> :
        IAlgorithm<
        UnivariatePolynomialNormalForm<CoeffType>,
        UnivariatePolynomialNormalForm<CoeffType>,
        IEuclidenDomain<CoeffType>,
        UnivariatePolynomialNormalForm<CoeffType>>
    {
        /// <summary>
        /// Obtém o último sub-resultante entre dois polinómios.
        /// </summary>
        /// <param name="first">O primeiro polinómio.</param>
        /// <param name="second">O segundo polinómio.</param>
        /// <param name="domain">O domínio responsável pelas operações sobre os coeficientes.</param>
        /// <returns>O polinómio que constitui o último sub-resultante.</returns>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public UnivariatePolynomialNormalForm<CoeffType> Run(
            UnivariatePolynomialNormalForm<CoeffType> first,
            UnivariatePolynomialNormalForm<CoeffType> second,
            IEuclidenDomain<CoeffType> domain)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var pseudoDomain = new UnivarPolynomPseudoDomain<CoeffType>(first.VariableName, domain);
                var innerFirstPol = first;
                var innerSecondPol = second;
                if (innerSecondPol.Degree > innerFirstPol.Degree)
                {
                    var swap = innerFirstPol;
                    innerFirstPol = innerSecondPol;
                    innerSecondPol = swap;
                }

                if (!pseudoDomain.IsAdditiveUnity(innerSecondPol))
                {
                    var firstLeadingCoeff = innerFirstPol.GetLeadingCoefficient(pseudoDomain.CoeffsDomain);
                    var tempDegree = innerFirstPol.Degree - innerSecondPol.Degree;
                    var temp = innerSecondPol;

                    innerSecondPol = pseudoDomain.Rem(innerFirstPol, innerSecondPol);
                    innerSecondPol = innerSecondPol.ApplyQuo(
                        firstLeadingCoeff,
                        pseudoDomain.CoeffsDomain);
                    if (tempDegree % 2 == 0)
                    {
                        innerSecondPol = innerSecondPol.GetSymmetric(pseudoDomain.CoeffsDomain);
                    }

                    innerFirstPol = temp;

                    // Ciclo relativo à determinação do primeiro valor de c
                    if (!pseudoDomain.IsAdditiveUnity(innerSecondPol))
                    {
                        firstLeadingCoeff = innerFirstPol.GetLeadingCoefficient(pseudoDomain.CoeffsDomain);
                        var c = MathFunctions.Power(firstLeadingCoeff, tempDegree, pseudoDomain.CoeffsDomain);

                        tempDegree = innerFirstPol.Degree - innerSecondPol.Degree;
                        temp = innerSecondPol;

                        innerSecondPol = pseudoDomain.Rem(innerFirstPol, innerSecondPol);
                        var den = MathFunctions.Power(c, tempDegree, pseudoDomain.CoeffsDomain);
                        innerSecondPol = innerSecondPol.ApplyQuo(
                            pseudoDomain.CoeffsDomain.Multiply(firstLeadingCoeff,den),
                            pseudoDomain.CoeffsDomain);
                        if (tempDegree % 2 == 0)
                        {
                            innerSecondPol = innerSecondPol.GetSymmetric(pseudoDomain.CoeffsDomain);
                        }

                        innerFirstPol = temp;

                        // Início do ciclo para obter a sequência de sub-resultantes
                        while (!pseudoDomain.IsAdditiveUnity(innerSecondPol))
                        {
                            firstLeadingCoeff = innerFirstPol.GetLeadingCoefficient(pseudoDomain.CoeffsDomain);
                            c = pseudoDomain.CoeffsDomain.Quo(
                                MathFunctions.Power(firstLeadingCoeff, tempDegree, pseudoDomain.CoeffsDomain),
                                MathFunctions.Power(c, tempDegree - 1, pseudoDomain.CoeffsDomain));

                            tempDegree = innerFirstPol.Degree - innerSecondPol.Degree;
                            temp = innerSecondPol;

                            innerSecondPol = pseudoDomain.Rem(innerFirstPol, innerSecondPol);
                            den = MathFunctions.Power(c, tempDegree, pseudoDomain.CoeffsDomain);
                            innerSecondPol = innerSecondPol.ApplyQuo(
                                pseudoDomain.CoeffsDomain.Multiply(firstLeadingCoeff, den),
                                pseudoDomain.CoeffsDomain);
                            if (tempDegree % 2 == 0)
                            {
                                innerSecondPol = innerSecondPol.GetSymmetric(pseudoDomain.CoeffsDomain);
                            }

                            innerFirstPol = temp;
                        }
                    }
                }

                return innerFirstPol;
            }
        }
    }
}
