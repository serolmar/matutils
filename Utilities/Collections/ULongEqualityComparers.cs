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
        /// <returns></returns>
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
        /// <returns></returns>
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
