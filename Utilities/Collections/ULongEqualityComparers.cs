// -----------------------------------------------------------------------
// <copyright file="ULongEqualityComparers.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define um comparador no qual o código confuso é dado como um valor longo.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos dos quais se pretende obter o código confuso.</typeparam>
    public interface IULongEqualityComparer<in T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso do objecto.</returns>
        ulong GetHashCode64(T obj);
    }

    /// <summary>
    /// Define um comparador no qual o código confuso resulta da aplicação da função
    /// </summary>
    /// <typeparam name="Obj"></typeparam>
    public class NativeObjectLongComparer<Obj> : EqualityComparer<Obj>, IULongEqualityComparer<Obj>
    {
        /// <summary>
        /// Determina se dois objectos longos sem sinal são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>Verdadeiro se os obejctos forem iguais e falso caso contrário.</returns>
        public override bool Equals(Obj x, Obj y)
        {
            return object.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso de um valor longo sem sinal.
        /// </summary>
        /// <param name="obj">O valor do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode(Obj obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.GetHashCode();
            }
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso do objecto.</returns>
        public ulong GetHashCode64(Obj obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var hash = obj.GetHashCode();
                return ((ulong)(long)hash | (((ulong)(long)hash) << 32));
            }
        }
    }

    /// <summary>
    /// Define um comparador no qual o código confuso resulta da aplicação da função
    /// <see cref="GetHashCode"/> habitual de um comparador existente.
    /// </summary>
    /// <typeparam name="Obj">O tipo dos obejctos sobre os quais actua o comparador.</typeparam>
    public class NativeOEqualbjectLongComparer<Obj> : EqualityComparer<Obj>, IULongEqualityComparer<Obj>
    {
        /// <summary>
        /// O comparador associado ao objecto.
        /// </summary>
        private IEqualityComparer<Obj> innerComparer;

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="NativeOEqualbjectLongComparer{Obj}"/>.
        /// </summary>
        public NativeOEqualbjectLongComparer()
        {
            this.innerComparer = Default;
        }

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="NativeOEqualbjectLongComparer{Obj}"/>.
        /// </summary>
        /// <param name="comparer">O comparador associado ao objecto.</param>
        public NativeOEqualbjectLongComparer(IEqualityComparer<Obj> comparer)
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
        /// Obtém o comparador associado ao objecto.
        /// </summary>
        public IEqualityComparer<Obj> Comparer
        {
            get
            {
                return this.innerComparer;
            }
        }

        /// <summary>
        /// Determina se dois objectos longos sem sinal são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>Verdadeiro se os obejctos forem iguais e falso caso contrário.</returns>
        public override bool Equals(Obj x, Obj y)
        {
            return this.innerComparer.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso de um valor longo sem sinal.
        /// </summary>
        /// <param name="obj">O valor do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode(Obj obj)
        {
            return this.innerComparer.GetHashCode(obj);
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso do objecto.</returns>
        public ulong GetHashCode64(Obj obj)
        {
            var hash = this.innerComparer.GetHashCode();
            return ((ulong)(long)hash | (((ulong)(long)hash) << 32));
        }
    }

    /// <summary>
    /// Comparador longo de inteiros.
    /// </summary>
    public class IntLongEqualityComparer : EqualityComparer<int>, IULongEqualityComparer<int>
    {
        /// <summary>
        /// Determina se dois objectos longos sem sinal são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>Verdadeiro se os objectos são iguais e falso caso contrário.</returns>
        public override bool Equals(int x, int y)
        {
            return EqualityComparer<int>.Default.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso de um valor longo sem sinal.
        /// </summary>
        /// <param name="obj">O valor do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode(int obj)
        {
            return obj;
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso do objecto.</returns>
        public ulong GetHashCode64(int obj)
        {
            return ((ulong)((uint)obj) | (((uint)obj) << 32));
        }
    }

    /// <summary>
    /// Define o comparador de longos.
    /// </summary>
    public class LongEqualityComparer
        : EqualityComparer<long>, IULongEqualityComparer<long>
    {
        /// <summary>
        /// Determina se dois objectos longos sem sinal são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>Verdadeiro se os objectos são iguais e falso caso contrário.</returns>
        public override bool Equals(long x, long y)
        {
            return Default.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso de um valor longo sem sinal.
        /// </summary>
        /// <param name="obj">O valor do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode(long obj)
        {
            return Default.GetHashCode(obj);
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso do objecto.</returns>
        public ulong GetHashCode64(long obj)
        {
            return Convert.ToUInt64(obj);
        }
    }

    /// <summary>
    /// Define o comparador de longos sem sinal.
    /// </summary>
    public class ULongEqualityComparer 
        : EqualityComparer<ulong>, IULongEqualityComparer<ulong>
    {
        /// <summary>
        /// Determina se dois objectos longos sem sinal são iguais.
        /// </summary>
        /// <param name="x">O primeiro objecto a ser comparado.</param>
        /// <param name="y">O segundo objecto a ser comparado.</param>
        /// <returns>Verdadeiro se os obejctos forem iguais e falso caso contrário.</returns>
        public override bool Equals(ulong x, ulong y)
        {
            return Default.Equals(x, y);
        }

        /// <summary>
        /// Obtém o código confuso de um valor longo sem sinal.
        /// </summary>
        /// <param name="obj">O valor do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode(ulong obj)
        {
            return Default.GetHashCode(obj);
        }

        /// <summary>
        /// Obtém o código confuso do objecto.
        /// </summary>
        /// <param name="obj">O objecto do qual se pretende obter o código confuso.</param>
        /// <returns>O código confuso do objecto.</returns>
        public ulong GetHashCode64(ulong obj)
        {
            return obj;
        }
    }
}
