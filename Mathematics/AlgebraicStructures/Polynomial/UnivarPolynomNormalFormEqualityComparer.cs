namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um comparador de polinómios univariáveis na forma normal.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os coeficientes.</typeparam>
    public class UnivarPolynomNormalFormEqualityComparer<CoeffType> 
        : EqualityComparer<UnivariatePolynomialNormalForm<CoeffType>>
    {
        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IEqualityComparer<CoeffType> coeffsComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnivarPolynomNormalFormEqualityComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffsComparer">O comparador de coeficientes.</param>
        /// <exception cref="System.ArgumentNullException">Se o comparador de coeficientes for nulo.</exception>
        public UnivarPolynomNormalFormEqualityComparer(IEqualityComparer<CoeffType> coeffsComparer)
        {
            if (coeffsComparer == null)
            {
                throw new ArgumentNullException("coeffsComparer");
            }
            else
            {
                this.coeffsComparer = coeffsComparer;
            }
        }

        /// <summary>
        /// Deteremina se dois polinómios são iguais.
        /// </summary>
        /// <param name="x">O primeiro polinómio a ser comparado.</param>
        /// <param name="y">O segundo polinómio a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se ambos os polinómios forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(
            UnivariatePolynomialNormalForm<CoeffType> x, 
            UnivariatePolynomialNormalForm<CoeffType> y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else
            {
                return x.Equals(y, this.coeffsComparer);
            }
        }

        /// <summary>
        /// Retorna um código confuso para o polinómio proporcionado.
        /// </summary>
        /// <param name="obj">O polinómio.</param>
        /// <returns>
        /// Um código confuso para o polinómio que pode ser usado em alguns algoritmos.
        /// </returns>
        public override int GetHashCode(UnivariatePolynomialNormalForm<CoeffType> obj)
        {
            if (obj == null)
            {
                return typeof(UnivariatePolynomialNormalForm<CoeffType>).GetHashCode();
            }
            else
            {
                return obj.GetHashCode(this.coeffsComparer);
            }
        }
    }
}
