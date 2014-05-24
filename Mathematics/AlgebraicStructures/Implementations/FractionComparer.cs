namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite comprar duas fracções de acordo com a ordenação aritmética.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes na fracção.</typeparam>
    public class FractionComparer<CoeffType> : IComparer<Fraction<CoeffType>>
    {
        /// <summary>
        /// O objecto responsável pela multiplicação dos coeficientes.
        /// </summary>
        IMultiplication<CoeffType> multipliable;

        /// <summary>
        /// O comparador dos elementos da fracção.
        /// </summary>
        private IComparer<CoeffType> comparer;


        /// <summary>
        /// Cria um nova instância de um objecto do tipo <see cref="FractionComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de coeficientes das fracções.</param>
        /// <param name="multipliable">
        /// O objecto responsável pelas operações de multiplicação dos coeficientes
        /// da fracções.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Caso o objecto responsável pelas multiplicações seja nulo.
        /// </exception>
        public FractionComparer(IComparer<CoeffType> comparer, IMultiplication<CoeffType> multipliable)
        {
            if (multipliable == null)
            {
                throw new ArgumentNullException("multipliable");
            }
            else
            {
                if (comparer == null)
                {
                    this.comparer = Comparer<CoeffType>.Default;
                }
                else
                {
                    this.comparer = comparer;
                }

                this.multipliable = multipliable;
            }
        }

        /// <summary>
        /// Obtém o comparador dos elementos da fracção.
        /// </summary>
        public IComparer<CoeffType> Comparer
        {
            get
            {
                return this.comparer;
            }
        }

        /// <summary>
        /// Permite comparar o valor de duas fracções.
        /// </summary>
        /// <param name="x">A primeira fracção a ser comparada.</param>
        /// <param name="y">A segunda fracção a ser comparada.</param>
        /// <returns>
        /// O valor 1 caso a primeira fracção seja superior à segunda, 0 caso sejam iguais e -1 caso
        /// a primeira seja menor que a segunda.
        /// </returns>
        public int Compare(
            Fraction<CoeffType> x, 
            Fraction<CoeffType> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }
            else if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            else
            {
                var firstComparisionValue = this.multipliable.Multiply(x.Numerator, y.Denominator);
                var secondComparisionValue = this.multipliable.Multiply(x.Denominator, y.Numerator);
                return this.comparer.Compare(firstComparisionValue, secondComparisionValue);
            }
        }
    }
}
