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
        LinearLiftingStatus<int>,
        Tuple<UnivariatePolynomialNormalForm<int>, UnivariatePolynomialNormalForm<int>>>
    {
        /// <summary>
        /// Aplica o lema do levantamento para elevar a factorização módulo um número superior.
        /// </summary>
        /// <remarks>
        /// Não é realizada qualquer verificação da integridade dos dados associados aos parâmetros de entrada.
        /// Caso estes não sejam iniciados convenientemente, os resultados obtidos poderão não estar correctos.
        /// </remarks>
        /// <param name="status">Contém os dados de entrada.</param>
        /// <returns>Os factores do polinómio módulo m^k.</returns>
        public Tuple<UnivariatePolynomialNormalForm<int>, UnivariatePolynomialNormalForm<int>> Run(
            LinearLiftingStatus<int> status)
        {
            if (status == null)
            {
                throw new ArgumentNullException("status");
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
