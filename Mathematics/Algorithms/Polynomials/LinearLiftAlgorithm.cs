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
        :IAlgorithm<
        UnivariatePolynomialNormalForm<int>, 
        Tuple<UnivariatePolynomialNormalForm<int>, UnivariatePolynomialNormalForm<int>>,
        int,
        int,
        Tuple<UnivariatePolynomialNormalForm<int>, UnivariatePolynomialNormalForm<int>>>
    {
        /// <summary>
        /// Aplica o lema do levantamento para elevar a factorização módulo um número superior.
        /// </summary>
        /// <remarks>
        /// O valor de p terá de ser igual ao produto de ambos os factores introduzidos módulo m e estes
        /// terão de ser primos entre si. Caso contrário, o comportamento da função torna-se imprevisível,
        /// não fornecendo os resultados desejados.
        /// </remarks>
        /// <param name="polynom">O polinómio em consideração.</param>
        /// <param name="factors">Dois factores do polinómio.</param>
        /// <param name="module">O módulo sobre o qual o polinómio se factoriza.</param>
        /// <param name="k">O valor de k em m^k.</param>
        /// <returns>Os factores do polinómio módulo m^k.</returns>
        public Tuple<UnivariatePolynomialNormalForm<int>, UnivariatePolynomialNormalForm<int>> Run(
            UnivariatePolynomialNormalForm<int> polynom, 
            Tuple<UnivariatePolynomialNormalForm<int>, UnivariatePolynomialNormalForm<int>> factors, 
            int module, 
            int k)
        {
            if (polynom == null)
            {
                throw new ArgumentNullException("polynom");
            }
            if (factors == null)
            {
                throw new ArgumentNullException("factors");
            }
            if (k < 1)
            {
                throw new ArgumentException("The provided value of k can't be lower than one.");
            }
            if (module < 2)
            {
                throw new ArgumentException("The provided module can't be less than two.");
            }
            else
            {
                var integerDomain = new IntegerDomain();
                var lowerModuleField = new ModularIntegerField(module);
                var upperModuleField = new ModularIntegerField(module * module);
                var polynomialDomain = new UnivarPolynomEuclideanDomain<int>(polynom.VariableName, lowerModuleField);
                var polynomialRing = new UnivarPolynomRing<int>(polynom.VariableName, integerDomain);
                var gcdExtendedAlg = new LagrangeAlgorithm<UnivariatePolynomialNormalForm<int>>(polynomialDomain);

                var gcdOut = gcdExtendedAlg.Run(factors.Item1, factors.Item2);
                var first = factors.Item1;
                var second = factors.Item2;

                polynomialDomain.Field = upperModuleField;
                var d = polynomialRing.Multiply(first, second);
                d = polynomialRing.Add(polynom, polynomialRing.AdditiveInverse(d));
                var c = d.ApplyQuo(module, integerDomain);
                var currentModule = module;
                for (int i = 2; i <= k && !polynomialDomain.IsAdditiveUnity(d); ++i)
                {
                    polynomialDomain.Field = lowerModuleField;
                    var firstTemp = polynomialDomain.Rem(
                        polynomialDomain.Multiply(gcdOut.SecondFactor, c), 
                        first);
                    var secondTemp = polynomialDomain.Rem(
                        polynomialDomain.Multiply(gcdOut.FirstFactor, c), 
                        second);

                    polynomialDomain.Field = upperModuleField;
                    first = polynomialDomain.Add(first, firstTemp.Multiply(currentModule, upperModuleField));
                    second = polynomialDomain.Add(second, secondTemp.Multiply(currentModule, upperModuleField));

                    currentModule = upperModuleField.Module;
                    upperModuleField.Module *= module;

                    d = polynomialRing.Multiply(first, second);
                    d = polynomialRing.Add(polynom, polynomialRing.AdditiveInverse(d));
                    c = d.ApplyQuo(module, integerDomain);
                }

                return Tuple.Create(first, second);
            }
        }
    }
}
