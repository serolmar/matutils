namespace Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um tuplo mutável.
    /// </summary>
    public abstract class MutableTuple : IStructuralEquatable, IStructuralComparable, IComparable
    {
        /// <summary>
        /// Permite comparar o tuplo corrente ao objecto proporcioando.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// O valor 1 caso o tuplo seja superior, 0 se o tuplo for igual e -1 se o tuplo
        /// for inferior ao objecto.
        /// </returns>
        public virtual int CompareTo(object obj)
        {
            return this.InnerCompare(obj, Comparer<object>.Default);
        }

        /// <summary>
        /// Determina se o objecto proporcionado é estruturalmente igual ao tuplo
        /// mutável actual.
        /// </summary>
        /// <param name="other">O objecto.</param>
        /// <param name="comparer">O comparador dos objectos que constituem a estrutura.</param>
        /// <returns>Verdadeiro caso os objectos sejam estruturalmente iguais e falso caso contrário.</returns>
        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            return this.InnerEquals(other, comparer);
        }

        /// <summary>
        /// Determina o código confuso estrutural do tuplo mutável corrente.
        /// </summary>
        /// <param name="comparer">
        /// O objecto que permite o cálculo do código confuso dos objectos na estrutura.
        /// </param>
        /// <returns>O código confuso.</returns>
        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentException("comparer");
            }
            else
            {
                return this.InnerGetHashCode(comparer);
            }
        }

        /// <summary>
        /// Obtém a comparação estrutural entre o objecto proporcionado e o tuplo mutável corrente.
        /// </summary>
        /// <param name="other">O objecto.</param>
        /// <param name="comparer">O comparador dos objectos que constituem a estrutura.</param>
        /// <returns>
        /// O valor 1 caso o objecto seja inferior, 0 caso sejam iguais e -1 caso o objecto 
        /// seja superior ao tuplo mutável actual.
        /// </returns>
        int IStructuralComparable.CompareTo(object other, IComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                return this.InnerCompare(other, comparer);
            }
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            return this.InnerEquals(obj, EqualityComparer<object>.Default);
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            return this.InnerGetHashCode(EqualityComparer<object>.Default);
        }

        /// <summary>
        /// Obtém a representação textual do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("(");
            resultBuilder.Append(this.GetAsTextSequence());
            resultBuilder.Append(")");
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public abstract string GetAsTextSequence();

        /// <summary>
        /// Função de ajuda que permite facilitar a criação de tuplos mutáveis.
        /// </summary>
        /// <typeparam name="T1">O tipo do primeiro elemento do tuplo.</typeparam>
        /// <param name="item1">O primeiro elemento do tuplo.</param>
        /// <returns>O tuplo mutável.</returns>
        public static MutableTuple<T1> Create<T1>(T1 item1)
        {
            return new MutableTuple<T1>(item1);
        }

        /// <summary>
        /// Função de ajuda que permite facilitar a criação de tuplos mutáveis.
        /// </summary>
        /// <typeparam name="T1">O tipo do primeiro elemento do tuplo.</typeparam>
        /// <typeparam name="T2">O tipo do segundo elemento do tuplo.</typeparam>
        /// <param name="item2"></param>
        /// <param name="item1">O primeiro elemento do tuplo.</param>
        /// <returns>O tuplo mutável.</returns>
        public static MutableTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new MutableTuple<T1, T2>(item1, item2);
        }

        /// <summary>
        /// Função de ajuda que permite facilitar a criação de tuplos mutáveis.
        /// </summary>
        /// <typeparam name="T1">O tipo do primeiro elemento do tuplo.</typeparam>
        /// <typeparam name="T2">O tipo do segundo elemento do tuplo.</typeparam>
        /// <typeparam name="T3">O tipo do terceiro elemento do tuplo.</typeparam>
        /// <param name="item1">O primeiro elemento do tuplo.</param>
        /// <param name="item2">O segundo elemento do tuplo.</param>
        /// <param name="item3">O terceiro elemento do tuplo.</param>
        /// <returns>O tuplo mutável.</returns>
        public static MutableTuple<T1, T2, T3> Create<T1, T2, T3>(
            T1 item1, 
            T2 item2, 
            T3 item3)
        {
            return new MutableTuple<T1, T2, T3>(item1, item2, item3);
        }

        /// <summary>
        /// Função de ajuda que permite facilitar a criação de tuplos mutáveis.
        /// </summary>
        /// <typeparam name="T1">O tipo do primeiro elemento do tuplo.</typeparam>
        /// <typeparam name="T2">O tipo do segundo elemento do tuplo.</typeparam>
        /// <typeparam name="T3">O tipo do terceiro elemento do tuplo.</typeparam>
        /// <typeparam name="T4">O tipo do quarto elemento do tuplo.</typeparam>
        /// <param name="item1">O primeiro elemento do tuplo.</param>
        /// <param name="item2">O segundo elemento do tuplo.</param>
        /// <param name="item3">O terceiro elemento do tuplo.</param>
        /// <param name="item3">O quarto elemento do tuplo.</param>
        /// <returns>O tuplo mutável.</returns>
        public static MutableTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(
            T1 item1, 
            T2 item2, 
            T3 item3,
            T4 item4)
        {
            return new MutableTuple<T1, T2, T3, T4>(
                item1,
                item2,
                item3,
                item4);
        }

        /// <summary>
        /// Função de ajuda que permite facilitar a criação de tuplos mutáveis.
        /// </summary>
        /// <typeparam name="T1">O tipo do primeiro elemento do tuplo.</typeparam>
        /// <typeparam name="T2">O tipo do segundo elemento do tuplo.</typeparam>
        /// <typeparam name="T3">O tipo do terceiro elemento do tuplo.</typeparam>
        /// <typeparam name="T4">O tipo do quarto elemento do tuplo.</typeparam>
        /// <typeparam name="T5">O tipo do quinto elemento do tuplo.</typeparam>
        /// <param name="item1">O primeiro elemento do tuplo.</param>
        /// <param name="item2">O segundo elemento do tuplo.</param>
        /// <param name="item3">O terceiro elemento do tuplo.</param>
        /// <param name="item3">O quarto elemento do tuplo.</param>
        /// <param name="item5">O quinto elemento do tuplo.</param>
        /// <returns>O tuplo mutável.</returns>
        public static MutableTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(
            T1 item1,
            T2 item2,
            T3 item3,
            T4 item4,
            T5 item5)
        {
            return new MutableTuple<T1, T2, T3, T4, T5>(
                item1,
                item2,
                item3,
                item4,
                item5);
        }

        /// <summary>
        /// Função de ajuda que permite facilitar a criação de tuplos mutáveis.
        /// </summary>
        /// <typeparam name="T1">O tipo do primeiro elemento do tuplo.</typeparam>
        /// <typeparam name="T2">O tipo do segundo elemento do tuplo.</typeparam>
        /// <typeparam name="T3">O tipo do terceiro elemento do tuplo.</typeparam>
        /// <typeparam name="T4">O tipo do quarto elemento do tuplo.</typeparam>
        /// <typeparam name="T5">O tipo do quinto elemento do tuplo.</typeparam>
        /// <typeparam name="T6">O tipo do sexto elemento do tuplo.</typeparam>
        /// <param name="item1">O primeiro elemento do tuplo.</param>
        /// <param name="item2">O segundo elemento do tuplo.</param>
        /// <param name="item3">O terceiro elemento do tuplo.</param>
        /// <param name="item3">O quarto elemento do tuplo.</param>
        /// <param name="item5">O quinto elemento do tuplo.</param>
        /// <param name="item6">O sexto elemento do tuplo.</param>
        /// <returns>O tuplo mutável.</returns>
        public static MutableTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(
            T1 item1,
            T2 item2,
            T3 item3,
            T4 item4,
            T5 item5,
            T6 item6)
        {
            return new MutableTuple<T1, T2, T3, T4, T5, T6>(
                item1,
                item2,
                item3,
                item4,
                item5,
                item6);
        }

        /// <summary>
        /// Função de ajuda que permite facilitar a criação de tuplos mutáveis.
        /// </summary>
        ///<typeparam name="T1">O tipo do primeiro elemento do tuplo.</typeparam>
        /// <typeparam name="T2">O tipo do segundo elemento do tuplo.</typeparam>
        /// <typeparam name="T3">O tipo do terceiro elemento do tuplo.</typeparam>
        /// <typeparam name="T4">O tipo do quarto elemento do tuplo.</typeparam>
        /// <typeparam name="T5">O tipo do quinto elemento do tuplo.</typeparam>
        /// <typeparam name="T6">O tipo do sexto elemento do tuplo.</typeparam>
        /// <typeparam name="T7">O tipo do sétimo elemento do tuplo.</typeparam>
        /// <param name="item1">O primeiro elemento do tuplo.</param>
        /// <param name="item2">O segundo elemento do tuplo.</param>
        /// <param name="item3">O terceiro elemento do tuplo.</param>
        /// <param name="item3">O quarto elemento do tuplo.</param>
        /// <param name="item5">O quinto elemento do tuplo.</param>
        /// <param name="item6">O sexto elemento do tuplo.</param>
        /// <param name="item7">O sétimo elemento do tuplo.</param>
        /// <returns>O tuplo mutável.</returns>
        public static MutableTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(
            T1 item1,
            T2 item2,
            T3 item3,
            T4 item4,
            T5 item5,
            T6 item6,
            T7 item7)
        {
            return new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(
                item1,
                item2,
                item3,
                item4,
                item5,
                item6,
                item7);
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected abstract bool InnerEquals(object obj, IEqualityComparer comparer);

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected abstract int InnerGetHashCode(IEqualityComparer comparer);

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected abstract int InnerCompare(object other, IComparer comparer);
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    public class MutableTuple<T1> : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1}"/>.
        /// </summary>
        public MutableTuple()
        {
            this.item1 = default(T1);
        }

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1>(MutableTuple<T1> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return Tuple.Create<T1>(mutableTuple.item1);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1>(Tuple<T1> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1>(tuple.Item1);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1}"/>.
        /// </summary>
        /// <param name="item1">O primeiro item do tuplo.</param>
        public MutableTuple(T1 item1)
        {
            this.item1 = item1;
        }

        /// <summary>
        /// Obtém ou atribui o valor do item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    return comparer.Equals(
                        this.item1,
                        innerTuple.Item1);
                }
            }
            else
            {
                return comparer.Equals(
                    this.item1,
                    innerMutable.item1);
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(this.item1);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        return comparer.Compare(this.item1, innerTuple.Item1);
                    }
                }
                else
                {
                    return comparer.Compare(this.item1, innerMutableTuple.item1);
                }
            }
        }
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    /// <typeparam name="T2">O tipo do segundo elemento.</typeparam>
    public class MutableTuple<T1, T2> : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// O segundo elemento.
        /// </summary>
        private T2 item2;

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1, T2>(MutableTuple<T1, T2> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return Tuple.Create<T1, T2>(mutableTuple.item1, mutableTuple.item2);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1, T2>(Tuple<T1, T2> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1, T2>(tuple.Item1, tuple.Item2);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        public MutableTuple()
        {
            this.item1 = default(T1);
            this.item2 = default(T2);
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        /// <param name="item1">O priemiro item do tuplo.</param>
        /// <param name="item2">O segundo item do tuplo.</param>
        public MutableTuple(T1 item1, T2 item2)
        {
            this.item1 = item1;
            this.item2 = item2;
        }

        /// <summary>
        /// Obtém ou atribui o valor do primeiro item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do segundo item.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return this.item2;
            }
            set
            {
                this.item2 = value;
            }
        }
        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item2);
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1, T2>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1, T2>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    return this.Equals(
                        innerTuple.Item1,
                        innerTuple.Item2,
                        comparer);
                }
            }
            else
            {
                return this.Equals(
                    innerMutable.item1,
                    innerMutable.item2,
                    comparer);
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.item1);
            var h2 = comparer.GetHashCode(this.item2);
            return (((h1 << 5) + h1) ^ h2);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1, T2>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1, T2>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        var result = comparer.Compare(this.item1, innerTuple.Item1);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item2, innerTuple.Item2);
                        }

                        return result;
                    }
                }
                else
                {
                    var result = comparer.Compare(this.item1, innerMutableTuple.item1);
                    if (result == 0)
                    {
                        result = comparer.Compare(this.item2, innerMutableTuple.item2);
                    }

                    return result;
                }
            }
        }

        /// <summary>
        /// Compara dos itens proporcionados com os que estão associados ao objecto.
        /// </summary>
        /// <param name="item1">O primeiro item.</param>
        /// <param name="item2">O segundo item.</param>
        /// <param name="equalityComparer">O comparador.</param>
        /// <returns></returns>
        private bool Equals(
            T1 item1,
            T2 item2,
            IEqualityComparer equalityComparer)
        {
            return equalityComparer.Equals(this.item1, item1)
                && equalityComparer.Equals(this.item2, item2);
        }
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    /// <typeparam name="T2">O tipo do segundo elemento.</typeparam>
    /// <typeparam name="T3">O tipo do terceiro elemento.</typeparam>
    public class MutableTuple<T1, T2, T3> : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// O segundo elemento.
        /// </summary>
        private T2 item2;

        /// <summary>
        /// O terceiro elemento.
        /// </summary>
        private T3 item3;

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1, T2, T3>(MutableTuple<T1, T2, T3> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return Tuple.Create<T1, T2, T3>(
                    mutableTuple.item1,
                    mutableTuple.item2,
                    mutableTuple.item3);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1, T2, T3>(Tuple<T1, T2, T3> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1, T2, T3>(tuple.Item1, tuple.Item2, tuple.Item3);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        public MutableTuple()
        {
            this.item1 = default(T1);
            this.item2 = default(T2);
            this.item3 = default(T3);
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        /// <param name="item1">O priemiro item do tuplo.</param>
        /// <param name="item2">O segundo item do tuplo.</param>
        /// <param name="item3">O terceiro item do tuplo.</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
        }

        /// <summary>
        /// Obtém ou atribui o valor do primeiro item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do segundo item.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return this.item2;
            }
            set
            {
                this.item2 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do terceiro item.
        /// </summary>
        public T3 Item3
        {
            get
            {
                return this.item3;
            }
            set
            {
                this.item3 = value;
            }
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item2);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item3);
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1, T2, T3>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1, T2, T3>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    if (comparer.Equals(this.item1, innerTuple.Item1))
                    {
                        if (comparer.Equals(this.item2, innerTuple.Item2))
                        {
                            return comparer.Equals(this.item3, innerTuple.Item3);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (comparer.Equals(this.item1, innerMutable.item1))
                {
                    if (comparer.Equals(this.item2, innerMutable.item2))
                    {
                        return comparer.Equals(this.item3, innerMutable.item3);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.item1);
            var h2 = comparer.GetHashCode(this.item2);
            h1 = (((h1 << 5) + h1) ^ h2);
            var h3 = comparer.GetHashCode(this.item3);
            return (((h1 << 5) + h1) ^ h2);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1, T2, T3>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1, T2, T3>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        var result = comparer.Compare(this.item1, innerTuple.Item1);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item2, innerTuple.Item2);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item3, innerTuple.Item3);
                            }
                        }

                        return result;
                    }
                }
                else
                {
                    var result = comparer.Compare(this.item1, innerMutableTuple.item1);
                    if (result == 0)
                    {
                        result = comparer.Compare(this.item2, innerMutableTuple.item2);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item3, innerMutableTuple.item3);
                        }
                    }

                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    /// <typeparam name="T2">O tipo do segundo elemento.</typeparam>
    /// <typeparam name="T3">O tipo do terceiro elemento.</typeparam>
    /// <typeparam name="T4">O tipo do quarto elemento.</typeparam>
    public class MutableTuple<T1, T2, T3, T4> : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// O segundo elemento.
        /// </summary>
        private T2 item2;

        /// <summary>
        /// O terceiro elemento.
        /// </summary>
        private T3 item3;

        /// <summary>
        /// O quarto elemento.
        /// </summary>
        private T4 item4;

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1, T2, T3, T4>(MutableTuple<T1, T2, T3, T4> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return Tuple.Create<T1, T2, T3, T4>(
                    mutableTuple.item1,
                    mutableTuple.item2,
                    mutableTuple.item3,
                    mutableTuple.item4);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1, T2, T3, T4>(Tuple<T1, T2, T3, T4> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1, T2, T3, T4>(
                    tuple.Item1, 
                    tuple.Item2, 
                    tuple.Item3,
                    tuple.Item4);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        public MutableTuple()
        {
            this.item1 = default(T1);
            this.item2 = default(T2);
            this.item3 = default(T3);
            this.item4 = default(T4);
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        /// <param name="item1">O priemiro item do tuplo.</param>
        /// <param name="item2">O segundo item do tuplo.</param>
        /// <param name="item3">O terceiro item do tuplo.</param>
        /// <param name="item4">O quarto item do tuplo.</param>
        public MutableTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
        }

        /// <summary>
        /// Obtém ou atribui o valor do primeiro item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do segundo item.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return this.item2;
            }
            set
            {
                this.item2 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do terceiro item.
        /// </summary>
        public T3 Item3
        {
            get
            {
                return this.item3;
            }
            set
            {
                this.item3 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quarto item.
        /// </summary>
        public T4 Item4
        {
            get
            {
                return this.item4;
            }
            set
            {
                this.item4 = value;
            }
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item2);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item3);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item4);
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1, T2, T3, T4>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1, T2, T3, T4>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    if (comparer.Equals(this.item1, innerTuple.Item1))
                    {
                        if (comparer.Equals(this.item2, innerTuple.Item2))
                        {
                            if (comparer.Equals(this.item3, innerTuple.Item3))
                            {
                                return comparer.Equals(this.item4, innerTuple.Item4);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (comparer.Equals(this.item1, innerMutable.item1))
                {
                    if (comparer.Equals(this.item2, innerMutable.item2))
                    {
                        if (comparer.Equals(this.item3, innerMutable.item3))
                        {
                            return comparer.Equals(this.item4, innerMutable.item4);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.item1);
            var h2 = comparer.GetHashCode(this.item2);
            var hres1 = (((h1 << 5) + h1) ^ h2);
            var h3 = comparer.GetHashCode(this.item3);
            var h4 = comparer.GetHashCode(this.item4);
            var hres2 = (((h3 << 5) + h3) ^ h4);
            return (((hres1 << 5) + hres1) ^ hres2);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1, T2, T3, T4>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1, T2, T3, T4>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        var result = comparer.Compare(this.item1, innerTuple.Item1);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item2, innerTuple.Item2);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item3, innerTuple.Item3);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item4, innerTuple.Item4);
                                }
                            }
                        }

                        return result;
                    }
                }
                else
                {
                    var result = comparer.Compare(this.item1, innerMutableTuple.item1);
                    if (result == 0)
                    {
                        result = comparer.Compare(this.item2, innerMutableTuple.item2);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item3, innerMutableTuple.item3);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item4, innerMutableTuple.item4);
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    /// <typeparam name="T2">O tipo do segundo elemento.</typeparam>
    /// <typeparam name="T3">O tipo do terceiro elemento.</typeparam>
    /// <typeparam name="T4">O tipo do quarto elemento.</typeparam>
    /// <typeparam name="T5">O tipo do quinto elemento.</typeparam>
    public class MutableTuple<T1, T2, T3, T4, T5> : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// O segundo elemento.
        /// </summary>
        private T2 item2;

        /// <summary>
        /// O terceiro elemento.
        /// </summary>
        private T3 item3;

        /// <summary>
        /// O quarto elemento.
        /// </summary>
        private T4 item4;

        /// <summary>
        /// O quinto elemento.
        /// </summary>
        private T5 item5;

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1, T2, T3, T4, T5>(MutableTuple<T1, T2, T3, T4, T5> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return Tuple.Create<T1, T2, T3, T4, T5>(
                    mutableTuple.item1,
                    mutableTuple.item2,
                    mutableTuple.item3,
                    mutableTuple.item4,
                    mutableTuple.item5);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1, T2, T3, T4, T5>(Tuple<T1, T2, T3, T4, T5> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1, T2, T3, T4, T5>(
                    tuple.Item1,
                    tuple.Item2,
                    tuple.Item3,
                    tuple.Item4,
                    tuple.Item5);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        public MutableTuple()
        {
            this.item1 = default(T1);
            this.item2 = default(T2);
            this.item3 = default(T3);
            this.item4 = default(T4);
            this.item5 = default(T5);
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        /// <param name="item1">O priemiro item do tuplo.</param>
        /// <param name="item2">O segundo item do tuplo.</param>
        /// <param name="item3">O terceiro item do tuplo.</param>
        /// <param name="item4">O quarto item do tuplo.</param>
        /// <param name="item5">O quinto item do tuplo.</param>
        public MutableTuple(
            T1 item1, 
            T2 item2, 
            T3 item3, 
            T4 item4,
            T5 item5)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
            this.item5 = item5;
        }

        /// <summary>
        /// Obtém ou atribui o valor do primeiro item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do segundo item.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return this.item2;
            }
            set
            {
                this.item2 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do terceiro item.
        /// </summary>
        public T3 Item3
        {
            get
            {
                return this.item3;
            }
            set
            {
                this.item3 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quarto item.
        /// </summary>
        public T4 Item4
        {
            get
            {
                return this.item4;
            }
            set
            {
                this.item4 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quinto item.
        /// </summary>
        public T5 Item5
        {
            get
            {
                return this.item5;
            }
            set
            {
                this.item5 = value;
            }
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item2);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item3);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item4);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item5);
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1, T2, T3, T4, T5>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1, T2, T3, T4, T5>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    if (comparer.Equals(this.item1, innerTuple.Item1))
                    {
                        if (comparer.Equals(this.item2, innerTuple.Item2))
                        {
                            if (comparer.Equals(this.item3, innerTuple.Item3))
                            {
                                if (comparer.Equals(this.item4, innerTuple.Item4))
                                {
                                    return comparer.Equals(this.item5, innerTuple.Item5);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (comparer.Equals(this.item1, innerMutable.item1))
                {
                    if (comparer.Equals(this.item2, innerMutable.item2))
                    {
                        if (comparer.Equals(this.item3, innerMutable.item3))
                        {
                            if (comparer.Equals(this.item4, innerMutable.item4))
                            {
                                return comparer.Equals(this.item5, innerMutable.item5);
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.item1);
            var h2 = comparer.GetHashCode(this.item2);
            var hres1 = (((h1 << 5) + h1) ^ h2);
            var h3 = comparer.GetHashCode(this.item3);
            hres1 = (((hres1 << 5) + hres1) ^ h3);
            var h4 = comparer.GetHashCode(this.item4);
            var h5 = comparer.GetHashCode(this.item5);
            var hres2 = (((h4 << 5) + h4) ^ h5);
            return (((hres1 << 5) + hres1) ^ hres2);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1, T2, T3, T4, T5>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1, T2, T3, T4, T5>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        var result = comparer.Compare(this.item1, innerTuple.Item1);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item2, innerTuple.Item2);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item3, innerTuple.Item3);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item4, innerTuple.Item4);
                                    if (result == 0)
                                    {
                                        result = comparer.Compare(this.item5, innerTuple.Item5);
                                    }
                                }
                            }
                        }

                        return result;
                    }
                }
                else
                {
                    var result = comparer.Compare(this.item1, innerMutableTuple.item1);
                    if (result == 0)
                    {
                        result = comparer.Compare(this.item2, innerMutableTuple.item2);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item3, innerMutableTuple.item3);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item4, innerMutableTuple.item4);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item5, innerMutableTuple.item5);
                                }
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    /// <typeparam name="T2">O tipo do segundo elemento.</typeparam>
    /// <typeparam name="T3">O tipo do terceiro elemento.</typeparam>
    /// <typeparam name="T4">O tipo do quarto elemento.</typeparam>
    /// <typeparam name="T5">O tipo do quinto elemento.</typeparam>
    /// <typeparam name="T6">O tipo do sexto elemento.</typeparam>
    public class MutableTuple<T1, T2, T3, T4, T5, T6> : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// O segundo elemento.
        /// </summary>
        private T2 item2;

        /// <summary>
        /// O terceiro elemento.
        /// </summary>
        private T3 item3;

        /// <summary>
        /// O quarto elemento.
        /// </summary>
        private T4 item4;

        /// <summary>
        /// O quinto elemento.
        /// </summary>
        private T5 item5;

        /// <summary>
        /// O sexto elemento.
        /// </summary>
        private T6 item6;

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1, T2, T3, T4, T5, T6>(MutableTuple<T1, T2, T3, T4, T5, T6> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return Tuple.Create<T1, T2, T3, T4, T5, T6>(
                    mutableTuple.item1,
                    mutableTuple.item2,
                    mutableTuple.item3,
                    mutableTuple.item4,
                    mutableTuple.item5,
                    mutableTuple.item6);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1, T2, T3, T4, T5, T6>(Tuple<T1, T2, T3, T4, T5, T6> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1, T2, T3, T4, T5, T6>(
                    tuple.Item1,
                    tuple.Item2,
                    tuple.Item3,
                    tuple.Item4,
                    tuple.Item5,
                    tuple.Item6);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        public MutableTuple()
        {
            this.item1 = default(T1);
            this.item2 = default(T2);
            this.item3 = default(T3);
            this.item4 = default(T4);
            this.item5 = default(T5);
            this.item6 = default(T6);
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        /// <param name="item1">O priemiro item do tuplo.</param>
        /// <param name="item2">O segundo item do tuplo.</param>
        /// <param name="item3">O terceiro item do tuplo.</param>
        /// <param name="item4">O quarto item do tuplo.</param>
        /// <param name="item5">O quinto item do tuplo.</param>
        /// <param name="item6">O sexto item do tuplo.</param>
        public MutableTuple(
            T1 item1,
            T2 item2,
            T3 item3,
            T4 item4,
            T5 item5,
            T6 item6)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
            this.item5 = item5;
            this.item6 = item6;
        }

        /// <summary>
        /// Obtém ou atribui o valor do primeiro item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do segundo item.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return this.item2;
            }
            set
            {
                this.item2 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do terceiro item.
        /// </summary>
        public T3 Item3
        {
            get
            {
                return this.item3;
            }
            set
            {
                this.item3 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quarto item.
        /// </summary>
        public T4 Item4
        {
            get
            {
                return this.item4;
            }
            set
            {
                this.item4 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quinto item.
        /// </summary>
        public T5 Item5
        {
            get
            {
                return this.item5;
            }
            set
            {
                this.item5 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do sexto item.
        /// </summary>
        public T6 Item6
        {
            get
            {
                return this.item6;
            }
            set
            {
                this.item6 = value;
            }
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item2);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item3);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item4);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item5);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item6);
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1, T2, T3, T4, T5, T6>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1, T2, T3, T4, T5, T6>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    if (comparer.Equals(this.item1, innerTuple.Item1))
                    {
                        if (comparer.Equals(this.item2, innerTuple.Item2))
                        {
                            if (comparer.Equals(this.item3, innerTuple.Item3))
                            {
                                if (comparer.Equals(this.item4, innerTuple.Item4))
                                {
                                    if (comparer.Equals(this.item5, innerTuple.Item5))
                                    {
                                        return comparer.Equals(this.item6, innerTuple.Item6);
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (comparer.Equals(this.item1, innerMutable.item1))
                {
                    if (comparer.Equals(this.item2, innerMutable.item2))
                    {
                        if (comparer.Equals(this.item3, innerMutable.item3))
                        {
                            if (comparer.Equals(this.item4, innerMutable.item4))
                            {
                                if (comparer.Equals(this.item5, innerMutable.item5))
                                {
                                    return comparer.Equals(this.item6, innerMutable.item6);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.item1);
            var h2 = comparer.GetHashCode(this.item2);
            var hres1 = (((h1 << 5) + h1) ^ h2);
            var h3 = comparer.GetHashCode(this.item3);
            hres1 = (((hres1 << 5) + hres1) ^ h3);
            var h4 = comparer.GetHashCode(this.item4);
            var h5 = comparer.GetHashCode(this.item5);
            var hres2 = (((h4 << 5) + h4) ^ h5);
            var h6 = comparer.GetHashCode(this.item6);
            hres2 = (((hres2<< 5) + hres2) ^ h6);
            return (((hres1 << 5) + hres1) ^ hres2);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1, T2, T3, T4, T5, T6>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1, T2, T3, T4, T5, T6>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        var result = comparer.Compare(this.item1, innerTuple.Item1);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item2, innerTuple.Item2);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item3, innerTuple.Item3);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item4, innerTuple.Item4);
                                    if (result == 0)
                                    {
                                        result = comparer.Compare(this.item5, innerTuple.Item5);
                                        if (result == 0)
                                        {
                                            result = comparer.Compare(this.item6, innerTuple.Item6);
                                        }
                                    }
                                }
                            }
                        }

                        return result;
                    }
                }
                else
                {
                    var result = comparer.Compare(this.item1, innerMutableTuple.item1);
                    if (result == 0)
                    {
                        result = comparer.Compare(this.item2, innerMutableTuple.item2);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item3, innerMutableTuple.item3);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item4, innerMutableTuple.item4);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item5, innerMutableTuple.item5);
                                    if (result == 0)
                                    {
                                        result = comparer.Compare(this.item6, innerMutableTuple.item6);
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    /// <typeparam name="T2">O tipo do segundo elemento.</typeparam>
    /// <typeparam name="T3">O tipo do terceiro elemento.</typeparam>
    /// <typeparam name="T4">O tipo do quarto elemento.</typeparam>
    /// <typeparam name="T5">O tipo do quinto elemento.</typeparam>
    /// <typeparam name="T6">O tipo do sexto elemento.</typeparam>
    /// <typeparam name="T7">O tipo do sétimo elemento.</typeparam>
    public class MutableTuple<T1, T2, T3, T4, T5, T6, T7> : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// O segundo elemento.
        /// </summary>
        private T2 item2;

        /// <summary>
        /// O terceiro elemento.
        /// </summary>
        private T3 item3;

        /// <summary>
        /// O quarto elemento.
        /// </summary>
        private T4 item4;

        /// <summary>
        /// O quinto elemento.
        /// </summary>
        private T5 item5;

        /// <summary>
        /// O sexto elemento.
        /// </summary>
        private T6 item6;

        /// <summary>
        /// O sétimo elemento.
        /// </summary>
        private T7 item7;

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1, T2, T3, T4, T5, T6, T7>(MutableTuple<T1, T2, T3, T4, T5, T6, T7> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return Tuple.Create<T1, T2, T3, T4, T5, T6, T7>(
                    mutableTuple.item1,
                    mutableTuple.item2,
                    mutableTuple.item3,
                    mutableTuple.item4,
                    mutableTuple.item5,
                    mutableTuple.item6,
                    mutableTuple.item7);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1, T2, T3, T4, T5, T6, T7>(Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1, T2, T3, T4, T5, T6, T7>(
                    tuple.Item1,
                    tuple.Item2,
                    tuple.Item3,
                    tuple.Item4,
                    tuple.Item5,
                    tuple.Item6,
                    tuple.Item7);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1,T2}"/>.
        /// </summary>
        public MutableTuple()
        {
            this.item1 = default(T1);
            this.item2 = default(T2);
            this.item3 = default(T3);
            this.item4 = default(T4);
            this.item5 = default(T5);
            this.item6 = default(T6);
            this.item7 = default(T7);
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/>.
        /// </summary>
        /// <param name="item1">O priemiro item do tuplo.</param>
        /// <param name="item2">O segundo item do tuplo.</param>
        /// <param name="item3">O terceiro item do tuplo.</param>
        /// <param name="item4">O quarto item do tuplo.</param>
        /// <param name="item5">O quinto item do tuplo.</param>
        /// <param name="item6">O sexto item do tuplo.</param>
        /// <param name="item7">O sétimo item do tuplo.</param>
        public MutableTuple(
            T1 item1,
            T2 item2,
            T3 item3,
            T4 item4,
            T5 item5,
            T6 item6,
            T7 item7)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
            this.item5 = item5;
            this.item6 = item6;
            this.item7 = item7;
        }

        /// <summary>
        /// Obtém ou atribui o valor do primeiro item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do segundo item.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return this.item2;
            }
            set
            {
                this.item2 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do terceiro item.
        /// </summary>
        public T3 Item3
        {
            get
            {
                return this.item3;
            }
            set
            {
                this.item3 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quarto item.
        /// </summary>
        public T4 Item4
        {
            get
            {
                return this.item4;
            }
            set
            {
                this.item4 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quinto item.
        /// </summary>
        public T5 Item5
        {
            get
            {
                return this.item5;
            }
            set
            {
                this.item5 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do sexto item.
        /// </summary>
        public T6 Item6
        {
            get
            {
                return this.item6;
            }
            set
            {
                this.item6 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do sétimo item.
        /// </summary>
        public T7 Item7
        {
            get
            {
                return this.item7;
            }
            set
            {
                this.item7 = value;
            }
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item2);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item3);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item4);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item5);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item6);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item7);
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1, T2, T3, T4, T5, T6, T7>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1, T2, T3, T4, T5, T6, T7>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    if (comparer.Equals(this.item1, innerTuple.Item1))
                    {
                        if (comparer.Equals(this.item2, innerTuple.Item2))
                        {
                            if (comparer.Equals(this.item3, innerTuple.Item3))
                            {
                                if (comparer.Equals(this.item4, innerTuple.Item4))
                                {
                                    if (comparer.Equals(this.item5, innerTuple.Item5))
                                    {
                                        if (comparer.Equals(this.item6, innerTuple.Item6))
                                        {
                                            return comparer.Equals(this.item7, innerTuple.Item7);
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (comparer.Equals(this.item1, innerMutable.item1))
                {
                    if (comparer.Equals(this.item2, innerMutable.item2))
                    {
                        if (comparer.Equals(this.item3, innerMutable.item3))
                        {
                            if (comparer.Equals(this.item4, innerMutable.item4))
                            {
                                if (comparer.Equals(this.item5, innerMutable.item5))
                                {
                                    if (comparer.Equals(this.item6, innerMutable.item6))
                                    {
                                        return comparer.Equals(this.item7, innerMutable.item7);
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.item1);
            var h2 = comparer.GetHashCode(this.item2);
            var hres1 = (((h1 << 5) + h1) ^ h2);
            var h3 = comparer.GetHashCode(this.item3);
            var h4 = comparer.GetHashCode(this.item4);
            var hres2 = (((h3 << 5) + h3) ^ h4);
            hres1 = (((hres1 << 5) + hres1) ^ hres2);
            var h5 = comparer.GetHashCode(this.item5);
            var h6 = comparer.GetHashCode(this.item6);
            hres2 = (((h5 << 5) + h5) ^ h6);
            var h7 = comparer.GetHashCode(this.item7);
            hres2 = (((hres2 << 5) + hres2) ^ h7);
            return (((hres1 << 5) + hres1) ^ hres2);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1, T2, T3, T4, T5, T6, T7>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        var result = comparer.Compare(this.item1, innerTuple.Item1);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item2, innerTuple.Item2);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item3, innerTuple.Item3);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item4, innerTuple.Item4);
                                    if (result == 0)
                                    {
                                        result = comparer.Compare(this.item5, innerTuple.Item5);
                                        if (result == 0)
                                        {
                                            result = comparer.Compare(this.item6, innerTuple.Item6);
                                            if (result == 0)
                                            {
                                                result = comparer.Compare(this.item7, innerTuple.Item7);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        return result;
                    }
                }
                else
                {
                    var result = comparer.Compare(this.item1, innerMutableTuple.item1);
                    if (result == 0)
                    {
                        result = comparer.Compare(this.item2, innerMutableTuple.item2);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item3, innerMutableTuple.item3);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item4, innerMutableTuple.item4);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item5, innerMutableTuple.item5);
                                    if (result == 0)
                                    {
                                        result = comparer.Compare(this.item6, innerMutableTuple.item6);
                                        if (result == 0)
                                        {
                                            result = comparer.Compare(this.item7, innerMutableTuple.item7);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }

    /// <summary>
    /// Representa um tuplo cujos elementos podem ser alterados.
    /// </summary>
    /// <typeparam name="T1">O tipo do primeiro elemento.</typeparam>
    /// <typeparam name="T2">O tipo do segundo elemento.</typeparam>
    /// <typeparam name="T3">O tipo do terceiro elemento.</typeparam>
    /// <typeparam name="T4">O tipo do quarto elemento.</typeparam>
    /// <typeparam name="T5">O tipo do quinto elemento.</typeparam>
    /// <typeparam name="T6">O tipo do sexto elemento.</typeparam>
    /// <typeparam name="T7">O tipo do sétimo elemento.</typeparam>
    /// <typeparam name="TRest">O tipo do último elemento.</typeparam>
    public class MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : MutableTuple
        where TRest : MutableTuple
    {
        /// <summary>
        /// O primeiro elemento.
        /// </summary>
        private T1 item1;

        /// <summary>
        /// O segundo elemento.
        /// </summary>
        private T2 item2;

        /// <summary>
        /// O terceiro elemento.
        /// </summary>
        private T3 item3;

        /// <summary>
        /// O quarto elemento.
        /// </summary>
        private T4 item4;

        /// <summary>
        /// O quinto elemento.
        /// </summary>
        private T5 item5;

        /// <summary>
        /// O sexto elemento.
        /// </summary>
        private T6 item6;

        /// <summary>
        /// O sétimo elemento.
        /// </summary>
        private T7 item7;

        /// <summary>
        /// O último elemento.
        /// </summary>
        private TRest rest;

        /// <summary>
        /// Conversão implícita entre tuplo mutável e tuplo.
        /// </summary>
        /// <param name="mutableTuple">O tuplo mutável a ser convertido.</param>
        /// <returns>O tuplo que resulta da conversão.</returns>
        public static implicit operator Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest> mutableTuple)
        {
            if (mutableTuple == null)
            {
                return null;
            }
            else
            {
                return new Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
                    mutableTuple.item1,
                    mutableTuple.item2,
                    mutableTuple.item3,
                    mutableTuple.item4,
                    mutableTuple.item5,
                    mutableTuple.item6,
                    mutableTuple.item7, 
                    mutableTuple.rest);
            }
        }

        /// <summary>
        /// Conversão implícita entre tuplo e tuplo mutável.
        /// </summary>
        /// <param name="tuple">O tuplo a ser convertido.</param>
        /// <returns>O tuplo mutável. que resulta da conversão.</returns>
        public static implicit operator MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
        {
            if (tuple == null)
            {
                return null;
            }
            else
            {
                return new MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
                    tuple.Item1,
                    tuple.Item2,
                    tuple.Item3,
                    tuple.Item4,
                    tuple.Item5,
                    tuple.Item6,
                    tuple.Item7,
                    tuple.Rest);
            }
        }

        /// <summary>
        /// Instancia objectos do tipo <see cref="MutableTuple{T1, T2, T3, T4, T5, T6, T7}"/>.
        /// </summary>
        /// <param name="item1">O priemiro item do tuplo.</param>
        /// <param name="item2">O segundo item do tuplo.</param>
        /// <param name="item3">O terceiro item do tuplo.</param>
        /// <param name="item4">O quarto item do tuplo.</param>
        /// <param name="item5">O quinto item do tuplo.</param>
        /// <param name="item6">O sexto item do tuplo.</param>
        /// <param name="item7">O sétimo item do tuplo.</param>
        /// <param name="rest">O último item do tuplo.</param>
        public MutableTuple(
            T1 item1,
            T2 item2,
            T3 item3,
            T4 item4,
            T5 item5,
            T6 item6,
            T7 item7,
            TRest rest)
        {
            this.item1 = item1;
            this.item2 = item2;
            this.item3 = item3;
            this.item4 = item4;
            this.item5 = item5;
            this.item6 = item6;
            this.item7 = item7;
        }

        /// <summary>
        /// Obtém ou atribui o valor do primeiro item.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return this.item1;
            }
            set
            {
                this.item1 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do segundo item.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return this.item2;
            }
            set
            {
                this.item2 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do terceiro item.
        /// </summary>
        public T3 Item3
        {
            get
            {
                return this.item3;
            }
            set
            {
                this.item3 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quarto item.
        /// </summary>
        public T4 Item4
        {
            get
            {
                return this.item4;
            }
            set
            {
                this.item4 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do quinto item.
        /// </summary>
        public T5 Item5
        {
            get
            {
                return this.item5;
            }
            set
            {
                this.item5 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do sexto item.
        /// </summary>
        public T6 Item6
        {
            get
            {
                return this.item6;
            }
            set
            {
                this.item6 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do sétimo item.
        /// </summary>
        public T7 Item7
        {
            get
            {
                return this.item7;
            }
            set
            {
                this.item7 = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do último item.
        /// </summary>
        public TRest Rest
        {
            get
            {
                return this.rest;
            }
            set
            {
                this.rest = value;
            }
        }

        /// <summary>
        /// Obtém a representação textual interna do tuplo.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string GetAsTextSequence()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(this.item1);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item2);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item3);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item4);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item5);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item6);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.item7);
            resultBuilder.Append(", ");
            resultBuilder.Append(this.rest.GetAsTextSequence());
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Determina se o objecto actual é igual objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="comparer">O comparador estrutural para os elementos.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        protected override bool InnerEquals(object obj, IEqualityComparer comparer)
        {
            var innerMutable = obj as MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>;
            if (innerMutable == null)
            {
                var innerTuple = obj as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>;
                if (innerTuple == null)
                {
                    return false;
                }
                else
                {
                    if (comparer.Equals(this.item1, innerTuple.Item1))
                    {
                        if (comparer.Equals(this.item2, innerTuple.Item2))
                        {
                            if (comparer.Equals(this.item3, innerTuple.Item3))
                            {
                                if (comparer.Equals(this.item4, innerTuple.Item4))
                                {
                                    if (comparer.Equals(this.item5, innerTuple.Item5))
                                    {
                                        if (comparer.Equals(this.item6, innerTuple.Item6))
                                        {
                                            if (comparer.Equals(this.item7, innerTuple.Item7))
                                            {
                                                return ((IStructuralEquatable)this.rest).Equals(
                                                    innerTuple.Rest,
                                                    comparer);
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (comparer.Equals(this.item1, innerMutable.item1))
                {
                    if (comparer.Equals(this.item2, innerMutable.item2))
                    {
                        if (comparer.Equals(this.item3, innerMutable.item3))
                        {
                            if (comparer.Equals(this.item4, innerMutable.item4))
                            {
                                if (comparer.Equals(this.item5, innerMutable.item5))
                                {
                                    if (comparer.Equals(this.item6, innerMutable.item6))
                                    {
                                        if (comparer.Equals(this.item7, innerMutable.item7))
                                        {
                                            return ((IStructuralEquatable)this.rest).Equals(
                                                innerMutable.rest,
                                                comparer);
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do tuplo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        protected override int InnerGetHashCode(IEqualityComparer comparer)
        {
            var h1 = comparer.GetHashCode(this.item1);
            var h2 = comparer.GetHashCode(this.item2);
            var hres1 = (((h1 << 5) + h1) ^ h2);
            var h3 = comparer.GetHashCode(this.item3);
            var h4 = comparer.GetHashCode(this.item4);
            var hres2 = (((h3 << 5) + h3) ^ h4);
            hres1 = (((hres1 << 5) + hres1) ^ hres2);
            var h5 = comparer.GetHashCode(this.item5);
            var h6 = comparer.GetHashCode(this.item6);
            hres2 = (((h5 << 5) + h5) ^ h6);
            var h7 = comparer.GetHashCode(this.item7);
            var hres = ((IStructuralEquatable)this.rest).GetHashCode(comparer);
            var hres3 = (((h7 << 5) + h7) ^ hres);
            hres2 = (((hres2 << 5) + hres2) ^ hres);
            return (((hres1 << 5) + hres1) ^ hres2);
        }

        /// <summary>
        /// Permite comparar o obejcto corrente ao objecto proporcionado.
        /// </summary>
        /// <param name="other">O objecto a ser comparado.</param>
        /// <param name="comparer">O comparador estrtutural.</param>
        /// <returns>
        /// O valor 1 caso o obecto corrente seja superior, 0 caso sejam iguais e -1 se for
        /// inferior ao objecto proporcionado.
        /// </returns>
        protected override int InnerCompare(object other, IComparer comparer)
        {
            if (other == null)
            {
                return 1;
            }
            else
            {
                var innerMutableTuple = other as MutableTuple<T1, T2, T3, T4, T5, T6, T7, TRest>;
                if (innerMutableTuple == null)
                {
                    var innerTuple = other as Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>;

                    if (innerTuple == null)
                    {
                        throw new ArgumentException("Invalid object in multable tuple comparision.");
                    }
                    else
                    {
                        var result = comparer.Compare(this.item1, innerTuple.Item1);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item2, innerTuple.Item2);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item3, innerTuple.Item3);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item4, innerTuple.Item4);
                                    if (result == 0)
                                    {
                                        result = comparer.Compare(this.item5, innerTuple.Item5);
                                        if (result == 0)
                                        {
                                            result = comparer.Compare(this.item6, innerTuple.Item6);
                                            if (result == 0)
                                            {
                                                result = comparer.Compare(this.item7, innerTuple.Item7);
                                                if (result == 0)
                                                {
                                                    result = ((IStructuralComparable)this.rest).CompareTo(
                                                        innerTuple.Rest,
                                                        comparer);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        return result;
                    }
                }
                else
                {
                    var result = comparer.Compare(this.item1, innerMutableTuple.item1);
                    if (result == 0)
                    {
                        result = comparer.Compare(this.item2, innerMutableTuple.item2);
                        if (result == 0)
                        {
                            result = comparer.Compare(this.item3, innerMutableTuple.item3);
                            if (result == 0)
                            {
                                result = comparer.Compare(this.item4, innerMutableTuple.item4);
                                if (result == 0)
                                {
                                    result = comparer.Compare(this.item5, innerMutableTuple.item5);
                                    if (result == 0)
                                    {
                                        result = comparer.Compare(this.item6, innerMutableTuple.item6);
                                        if (result == 0)
                                        {
                                            result = comparer.Compare(this.item7, innerMutableTuple.item7);
                                            if (result == 0)
                                            {
                                                return ((IStructuralComparable)this.rest).CompareTo(
                                                    innerMutableTuple.rest,
                                                    comparer);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
