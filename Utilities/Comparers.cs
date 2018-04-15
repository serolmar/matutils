<<<<<<< HEAD
﻿// -----------------------------------------------------------------------
=======
// -----------------------------------------------------------------------
>>>>>>> 6c405f0b273be9ba1894ac97d9d861c844a787ec
// <copyright file="Comparers.cs" company="Sérgio O. Marques">
//  Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Comparador de igualdade compatível com códigos confusos
    /// de 64 bit.
    /// </summary>
    /// <typeparam name="T">
    /// O tipo de objectos a serem comparados.
    /// </typeparam>
    public interface IEqualityComparer64<T> :
        IHash64<T>
    {
        /// <summary>
        /// Verifica a igualdade de dois objectos.
        /// </summary>
        /// <param name="obj1">O primeiro objecto.</param>
        /// <param name="obj2">O segundo objecto.</param>
        /// <returns>
        /// Verdadeiro se os objectos forem iguais e falso caso contrário.
        /// </returns>
        bool Equals(T obj1, T obj2);
    }

    /// <summary>
    /// Ponto de partida para todos os comparadores de 64 bit.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos a serem comparados.</typeparam>
    public abstract class EqualityComparer64<T>
        : IEqualityComparer64<T>
    {
        /// <summary>
        /// Mantém uma instância do comparador por defeito.
        /// </summary>
        private static volatile EqualityComparer64<T> defaultComparer;

        /// <summary>
        /// Obtém o comparador definido por defeito.
        /// </summary>
        public static EqualityComparer64<T> Default
        {
            get
            {
                if (defaultComparer == null)
                {
                    defaultComparer = new EqualityComparer32To64<T>();
                }

                return defaultComparer;
            }
        }

        /// <summary>
        /// Verifica a igualdade de dois objectos.
        /// </summary>
        /// <param name="obj1">O primeiro objecto.</param>
        /// <param name="obj2">O segundo objecto.</param>
        /// <returns>
        /// Verdadeiro se os objectos forem iguais e falso caso contrário.
        /// </returns>
        public abstract bool Equals(T obj1, T obj2);

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public abstract ulong GetHash64(T obj);
    }

    /// <summary>
    /// Implementação de um comparador cujo código confuso de 64 bit
    /// é dado pelo código confuso de 32 bit proporcionado pela linguagem.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos a serem comparados.</typeparam>
    public class EqualityComparer32To64<T> 
        : EqualityComparer64<T>
    {
        /// <summary>
        /// Mantém o comparador de 32 bit.
        /// </summary>
        private IEqualityComparer<T> innerComparer;

        /// <summary>
        /// Insntancia uma nova instância de objectos do tipo <see cref="EqualityComparer32To64{T}"/>.
        /// </summary>
        public EqualityComparer32To64()
        {
            this.innerComparer = EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Insntancia uma nova instância de objectos do tipo <see cref="EqualityComparer32To64{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador de 32 bit.</param>
        public EqualityComparer32To64(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.innerComparer = comparer;
            }
        }

        /// <summary>
        /// Obtém o comparador de 32 bit.
        /// </summary>
        public IEqualityComparer<T> Comparer
        {
            get
            {
                return this.innerComparer;
            }
        }

        /// <summary>
        /// Verifica a igualdade de dois objectos.
        /// </summary>
        /// <param name="obj1">O primeiro objecto.</param>
        /// <param name="obj2">O segundo objecto.</param>
        /// <returns>
        /// Verdadeiro se os objectos forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(T obj1, T obj2)
        {
            return this.innerComparer.Equals(obj1, obj2);
        }

        /// <summary>
        /// Obtém o código confuso de 64 bits de um objecto.
        /// </summary>
        /// <param name="obj">O objeto.</param>
        /// <returns>O código confuso.</returns>
        public override ulong GetHash64(T obj)
        {
            return (ulong)this.innerComparer.GetHashCode(obj);
        }
    }

    /// <summary>
    /// Implementa um comparador de igualdade sobre colecções de elementos tendo em conta a ordem.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os elmentos das colecções.</typeparam>
    public class OrderedEqualityColComparer<CoeffType>
        : EqualityComparer<IEnumerable<CoeffType>>
    {
        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IEqualityComparer<CoeffType> coeffComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="OrderedEqualityColComparer{CoeffType}"/>.
        /// </summary>
        public OrderedEqualityColComparer()
        {
            this.coeffComparer = EqualityComparer<CoeffType>.Default;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="OrderedEqualityColComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffComparer">O comparador de coeficientes.</param>
        public OrderedEqualityColComparer(IEqualityComparer<CoeffType> coeffComparer)
        {
            if (coeffComparer == null)
            {
                this.coeffComparer = EqualityComparer<CoeffType>.Default;
            }
            else
            {
                this.coeffComparer = coeffComparer;
            }
        }

        /// <summary>
        /// Determina se dois objectos são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se os objetos forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(IEnumerable<CoeffType> x, IEnumerable<CoeffType> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                var xEnumerator = x.GetEnumerator();
                var yEnumerator = y.GetEnumerator();
                var xState = xEnumerator.MoveNext();
                var yState = yEnumerator.MoveNext();
                while (xState && yState)
                {
                    if (!this.coeffComparer.Equals(xEnumerator.Current, yEnumerator.Current))
                    {
                        return false;
                    }
                    else
                    {
                        xState = xEnumerator.MoveNext();
                        yState = yEnumerator.MoveNext();
                    }
                }

                if (xState || yState)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Retorna um código confuso para o objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Um código confuso para o objecto utilizado em alguns algoritmos.
        /// </returns>
        public override int GetHashCode(IEnumerable<CoeffType> obj)
        {
            if (obj == null)
            {
                return typeof(IEnumerable<CoeffType>).GetHashCode();
            }
            else
            {
                var res = 19;
                foreach (var item in obj)
                {
                    if (item == null)
                    {
                        res = res * 31;
                    }
                    else
                    {
                        res = res * 31 + item.GetHashCode();
                    }
                }

                return res;
            }
        }
    }

    /// <summary>
    /// Implementa um comparador de igualdade sobre colecções de elementos numa ordema arbitrária.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos que constituem os elmentos das colecções.</typeparam>
    public class UnorderedEqualityColComparer<CoeffType>
        : EqualityComparer<IEnumerable<CoeffType>>
    {
        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IEqualityComparer<CoeffType> coeffComparer;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnorderedEqualityColComparer{CoeffType}"/>.
        /// </summary>
        public UnorderedEqualityColComparer()
        {
            this.coeffComparer = EqualityComparer<CoeffType>.Default;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="UnorderedEqualityColComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffComparer">O comparador de coeficientes.</param>
        public UnorderedEqualityColComparer(IEqualityComparer<CoeffType> coeffComparer)
        {
            if (coeffComparer == null)
            {
                this.coeffComparer = EqualityComparer<CoeffType>.Default;
            }
            else
            {
                this.coeffComparer = coeffComparer;
            }
        }

        /// <summary>
        /// Determina se dois objectos são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>
        /// Verdadeiro se os objetos forem iguais e falso caso contrário.
        /// </returns>
        public override bool Equals(IEnumerable<CoeffType> x, IEnumerable<CoeffType> y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                var xCounts = this.CountItems(x);
                var yCounts = this.CountItems(y);
                if (xCounts.Count == yCounts.Count)
                {
                    foreach (var xCount in xCounts)
                    {
                        var existing = default(int);
                        if (yCounts.TryGetValue(xCount.Key, out existing))
                        {
                            if (existing != xCount.Value)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Retorna um código confuso para o objecto.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Um código confuso para o objecto utilizado em alguns algoritmos.
        /// </returns>
        public override int GetHashCode(IEnumerable<CoeffType> obj)
        {
            if (obj == null)
            {
                return typeof(IEnumerable<CoeffType>).GetHashCode();
            }
            else
            {
                var result = 17;
                var countItems = this.CountItems(obj);
                foreach (var countKvp in countItems)
                {
                    result ^= (countKvp.Value * countKvp.Key.GetHashCode());
                }

                return result;
            }
        }

        /// <summary>
        /// Conta os itens existentes na colecção.
        /// </summary>
        /// <param name="collection">A colecção.</param>
        /// <returns>A contagem dos itens.</returns>
        private Dictionary<CoeffType, int> CountItems(IEnumerable<CoeffType> collection)
        {
            var result = new Dictionary<CoeffType, int>(this.coeffComparer);
            foreach (var item in collection)
            {
                var existing = default(int);
                if (result.TryGetValue(item, out existing))
                {
                    ++existing;
                    result[item] = existing;
                }
                else
                {
                    result.Add(item, 1);
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Implementa um comparador lexicográfico sobre uma colecção.
    /// </summary>
    /// <typeparam name="CoeffType">O comparador dos coeficientes.</typeparam>
    public class CollectionLexicographicComparer<CoeffType>
        : Comparer<IEnumerable<CoeffType>>
    {
        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IComparer<CoeffType> coeffComparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CollectionLexicographicComparer{CoeffType}"/>.
        /// </summary>
        public CollectionLexicographicComparer()
        {
            this.coeffComparer = Comparer<CoeffType>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CollectionLexicographicComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffComparer">O comparador de coeficientes.</param>
        public CollectionLexicographicComparer(IComparer<CoeffType> coeffComparer)
        {
            if (coeffComparer == null)
            {
                throw new ArgumentNullException("coeffComparer");
            }
            else
            {
                this.coeffComparer = coeffComparer;
            }
        }

        /// <summary>
        /// Compara duas colecções.
        /// </summary>
        /// <param name="x">A primeira colecção na comparação.</param>
        /// <param name="y">A segunda colecção na comparação.</param>
        /// <returns>
        /// O valor 1 caso a primeira colecção seja superior à segunda, -1 caso a primeira colecção
        /// seja inferior à segunda e 0 caso sejam iguais.
        /// </returns>
        public override int Compare(
            IEnumerable<CoeffType> x,
            IEnumerable<CoeffType> y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (y == null)
            {
                return 1;
            }
            else
            {
                var xEnum = x.GetEnumerator();
                var yEnum = x.GetEnumerator();
                var xState = xEnum.MoveNext();
                var yState = yEnum.MoveNext();
                while (xState && yState)
                {
                    var comparision = this.coeffComparer.Compare(
                        xEnum.Current,
                        yEnum.Current);
                    if (comparision == 0)
                    {
                        xState = xEnum.MoveNext();
                        yState = yEnum.MoveNext();
                    }
                    else
                    {
                        return comparision;
                    }
                }

                if (xState)
                {
                    return 1;
                }

                if (yState)
                {
                    return -1;
                }

                return 0;
            }
        }
    }

    /// <summary>
    /// Implementa um comparador que permite ordenar itens de acordo
    /// com o seu comprimento e ordem lexicográfica.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de objectos a serem comparados.</typeparam>
    public class CollectionShortLexComparer<CoeffType>
        : Comparer<IEnumerable<CoeffType>>
    {
        /// <summary>
        /// O comparador de objectos.
        /// </summary>
        private IComparer<CoeffType> coeffsComparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CollectionShortLexComparer{CoeffType}"/>.
        /// </summary>
        public CollectionShortLexComparer()
        {
            this.coeffsComparer = Comparer<CoeffType>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CollectionShortLexComparer{CoeffType}"/>.
        /// </summary>
        /// <param name="coeffsComparer">O comparador de coeficientes.</param>
        public CollectionShortLexComparer(Comparer<CoeffType> coeffsComparer)
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
        /// Compara duas colecções.
        /// </summary>
        /// <param name="x">A primeira colecção na comparação.</param>
        /// <param name="y">A segunda colecção na comparação.</param>
        /// <returns>
        /// O valor 1 caso a primeira colecção seja superior à segunda, -1 caso a primeira colecção
        /// seja inferior à segunda e 0 caso sejam iguais.
        /// </returns>
        public override int Compare(
            IEnumerable<CoeffType> x,
            IEnumerable<CoeffType> y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (y == null)
            {
                return 1;
            }
            else
            {
                var xEnum = x.GetEnumerator();
                var yEnum = y.GetEnumerator();
                var xState = xEnum.MoveNext();
                var yState = yEnum.MoveNext();
                var state = xState && yState;
                var comparision = 0;
                while (state)
                {
                    comparision = this.coeffsComparer.Compare(
                        xEnum.Current,
                        yEnum.Current);
                    xState = xEnum.MoveNext();
                    yState = yEnum.MoveNext();
                    if (comparision != 0)
                    {
                        state = false;
                    }
                    else
                    {
                        state = xState && yState;
                    }
                }

                while (xState && yState)
                {
                    xState = xEnum.MoveNext();
                    yState = yEnum.MoveNext();
                }

                if (xState)
                {
                    return 1;
                }

                if (yState)
                {
                    return -1;
                }

                return comparision;
            }
        }
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
