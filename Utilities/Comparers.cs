// -----------------------------------------------------------------------
// <copyright file="Comparers.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um comparador lexicográfico de colecções.
    /// </summary>
    /// <typeparam name="T">O tipo de elementos na colecção a comparar.</typeparam>
    public class LexicographicalComparer<T> : IComparer<ICollection<T>>
    {
        /// <summary>
        /// O comparador de elementos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância da classe <see cref="LexicographicalComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de elementos.</param>
        public LexicographicalComparer(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                this.comparer = Comparer<T>.Default;
            }
            else
            {
                this.comparer = comparer;
            }
        }

        #region IComparer<ICollection<T>> Members

        /// <summary>
        /// Compara dois objectos e retorna um valor que indica se um é menor, maior ou igual a outro.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// O valor 1 se o primeiro for maior do que o segundo, 0 se ambos forem iguais e -1 se o primeiro for
        /// menor do que o segundo.
        /// </returns>
        public int Compare(ICollection<T> x, ICollection<T> y)
        {
            IEnumerator<T> xEnum = x.GetEnumerator();
            IEnumerator<T> yEnum = y.GetEnumerator();
            bool xMoveNext = xEnum.MoveNext();
            bool yMoveNext = yEnum.MoveNext();
            while (xMoveNext && yMoveNext)
            {
                if (this.comparer.Compare(xEnum.Current, yEnum.Current) < 0)
                {
                    return -1;
                }
                else if (this.comparer.Compare(xEnum.Current, yEnum.Current) > 0)
                {
                    return 1;
                }

                xMoveNext = xEnum.MoveNext();
                yMoveNext = yEnum.MoveNext();
            }

            if (xMoveNext)
            {
                return 1;
            }

            if (yMoveNext)
            {
                return -1;
            }

            return 0;
        }

        #endregion
    }

    /// <summary>
    /// Comparador funcional de objectos, isto é, comparador que permite comparar
    /// determinados objectos com base num representante obtido após a aplicação de
    /// uma função.
    /// </summary>
    /// <typeparam name="P">O tipo dos objectos a serem comparados.</typeparam>
    /// <typeparam name="Q">O tipo dos representates.</typeparam>
    public class RepresentativeComparer<P, Q> : Comparer<P>
    {
        /// <summary>
        /// A função que permite construir um objecto a partir de outro.
        /// </summary>
        private Func<P, Q> getFunction;

        /// <summary>
        /// O comaparador dos objectos construídos.
        /// </summary>
        private Comparer<Q> comparer;

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="RepresentativeComparer{P, Q}"/>.
        /// </summary>
        /// <param name="getFunction">A função de transformação do objecto.</param>
        public RepresentativeComparer(Func<P, Q> getFunction)
        {
            if (getFunction == null)
            {
                throw new ArgumentNullException("getFunction");
            }
            else
            {
                this.getFunction = getFunction;
                this.comparer = Comparer<Q>.Default;
            }
        }

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="RepresentativeComparer{P, Q}"/>.
        /// </summary>
        /// <param name="getFunction">A função de transformação do objecto.</param>
        /// <param name="comparer">O comparador dos objectos construídos.</param>
        public RepresentativeComparer(
            Func<P, Q> getFunction,
            Comparer<Q> comparer)
            : this(getFunction)
        {
            if (comparer != null)
            {
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// A função que permite construir um objecto a partir de outro.
        /// </summary>
        private Func<P, Q> GetFunction
        {
            get
            {
                return this.getFunction;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesException("The get function value can't be null.");
                }
                else
                {
                    this.getFunction = value;
                }
            }
        }

        /// <summary>
        /// O comaparador dos objectos construídos.
        /// </summary>
        private Comparer<Q> Comparer
        {
            get
            {
                return this.comparer;
            }
            set
            {
                if (comparer == null)
                {
                    throw new UtilitiesException("The comparer value can't be null.");
                }
                else
                {
                    this.comparer = value;
                }
            }
        }

        /// <summary>
        /// Compara dois objectos e retorna um valor que indica se um é menor, maior ou igual a outro.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// O valor 1 se o primeiro for maior do que o segundo, 0 se ambos forem iguais e -1 se o primeiro for
        /// menor do que o segundo.
        /// </returns>
        public override int Compare(P x, P y)
        {
            var innerX = this.getFunction.Invoke(x);
            var innerY = this.getFunction.Invoke(y);
            return this.comparer.Compare(innerX, innerY);
        }
    }

    /// <summary>
    /// Implementa o comparador de inteiros invertido.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficiente a ser comparado.</typeparam>
    public class InverseComparer<CoeffType> : Comparer<CoeffType>
    {
        /// <summary>
        /// O coeficiente a ser comparado.
        /// </summary>
        private IComparer<CoeffType> coeffsComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="InverseComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffsComparer">O comparador de coeficientes.</param>
        public InverseComparer(IComparer<CoeffType> coeffsComparer)
        {
            if (coeffsComparer == null)
            {
                this.coeffsComparer = Comparer<CoeffType>.Default;
            }
            else
            {
                this.coeffsComparer = coeffsComparer;
            }
        }

        /// <summary>
        /// Compara dois inteiros de forma inversa.
        /// </summary>
        /// <param name="x">O primeiro elemento a ser comparado.</param>
        /// <param name="y">O segundo elemento a ser comparado.</param>
        /// <returns>Retorna -1 caso x seja superior a y, 0 caso sejam iguais e 1 se x for inferior a y.</returns>
        public override int Compare(CoeffType x, CoeffType y)
        {
            return -this.coeffsComparer.Compare(x, y);
        }
    }
}
