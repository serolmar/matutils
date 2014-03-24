namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite adicionar valores independentes às respectivas fracções.
    /// </summary>
    /// <typeparam name="Coefftype">O tipo dos coeficientes envolvidos.</typeparam>
    public class ElementFractionAddOper<Coefftype>
        : IAdditionOperation<Coefftype, Fraction<Coefftype>, Fraction<Coefftype>>
    {
        private IEuclidenDomain<Coefftype> domain;

        public ElementFractionAddOper(IEuclidenDomain<Coefftype> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                this.domain = domain;
            }
        }

        /// <summary>
        /// Permite adicionar um coeficiente independente a uma fracção.
        /// </summary>
        /// <param name="left">O coeficiente independente.</param>
        /// <param name="right">A fracção.</param>
        /// <returns>O resultado da soma.</returns>
        public Fraction<Coefftype> Add(Coefftype left, Fraction<Coefftype> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                return right.Add(left, this.domain);
            }
        }
    }
}
