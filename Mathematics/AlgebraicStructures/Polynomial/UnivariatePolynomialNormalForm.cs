using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Representa um polinómio com apenas uma variável escrito na sua forma normal.
    /// </summary>
    /// <remarks>
    /// O modo de funcionamento deste tipo de polinómios é em tudo semelhante ao dos polinómios gerais diferindo
    /// apenas em questões de desempenho.
    /// </remarks>
    /// <typeparam name="ObjectType">O tipo de dados dos coeficientes.</typeparam>
    /// <typeparam name="RingType">O tipo de dados do anel responsável pelas respectivas operações.</typeparam>
    public class UnivariatePolynomialNormalForm<CoeffType, RingType>
        where RingType : IRing<CoeffType>
    {
        RingType ring;

        private Dictionary<int, CoeffType> terms;

        private string variableName;

        public UnivariatePolynomialNormalForm(string variable, RingType ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (string.IsNullOrWhiteSpace(variable))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                this.ring = ring;
                this.terms = new Dictionary<int, CoeffType>();
                this.variableName = variable;
            }
        }
    }
}
