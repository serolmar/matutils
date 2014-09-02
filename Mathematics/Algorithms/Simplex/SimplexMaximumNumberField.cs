namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa um número com parte finita e parte infinita.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem o custo.</typeparam>
    public class SimplexMaximumNumberField<ObjectType>
    {
        /// <summary>
        /// A parte finita do número.
        /// </summary>
        private ObjectType finitePart;

        /// <summary>
        /// A parte enorme do número.
        /// </summary>
        private ObjectType bigPart;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplexMaximumNumberField{ObjectType}"/>.
        /// </summary>
        public SimplexMaximumNumberField()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="SimplexMaximumNumberField{ObjectType}"/>.
        /// </summary>
        /// <param name="finitePart">A parte finita.</param>
        /// <param name="bigPart">A parte infinita.</param>
        public SimplexMaximumNumberField(ObjectType finitePart, ObjectType bigPart)
        {
            this.finitePart = finitePart;
            this.bigPart = bigPart;
        }

        /// <summary>
        /// Obtém e atribui a parte finita do número.
        /// </summary>
        /// <value>A parte finita.</value>
        public ObjectType FinitePart
        {
            get
            {
                return this.finitePart;
            }
            set
            {
                this.finitePart = value;
            }
        }

        /// <summary>
        /// Obtém e atribui a parte infinita do número.
        /// </summary>
        /// <value>A parte infinita.</value>
        public ObjectType BigPart
        {
            get
            {
                return this.bigPart;
            }
            set
            {
                this.bigPart = value;
            }
        }

        /// <summary>
        /// Permite comparar dois números grandes.
        /// </summary>
        /// <param name="first">O primeiro número a ser comparado.</param>
        /// <param name="second">O segundo número a ser comparado.</param>
        /// <param name="comparer">O comparador de coeficientes.</param>
        /// <returns>
        /// O valor 1 caso o primeiro número seja superior ao segundo, 0 se ambos forem iguais e -1 se o 
        /// primeiro número for inferior ao segundo.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Se algum dos dois primeiros argumentos for nulo.
        /// </exception>
        public static int Compare(
            SimplexMaximumNumberField<ObjectType> first,
            SimplexMaximumNumberField<ObjectType> second,
            IComparer<ObjectType> comparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else if (comparer == null)
            {
                return UncheckedCompare(first, second, Comparer<ObjectType>.Default);
            }
            else
            {
                return UncheckedCompare(first, second, comparer);
            }
        }

        /// <summary>
        /// Adicionar os valores especificados aos valores existentes.
        /// </summary>
        /// <param name="finitePart">Valor a ser adicionado à parte finita.</param>
        /// <param name="bigPart">Valor a ser adicionado à parte grande.</param>
        /// <param name="additionOperation">O objecto responsável pela adição dos coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se algum dos argumentos for nulo.</exception>
        public void Add(
            ObjectType finitePart, 
            ObjectType bigPart, 
            IAdditionOperation<ObjectType,ObjectType,ObjectType> additionOperation)
        {
            if (finitePart == null)
            {
                throw new ArgumentNullException("finitePart");
            }
            else if (bigPart == null)
            {
                throw new ArgumentNullException("bigPart");
            }
            else if (additionOperation == null)
            {
                throw new ArgumentNullException("additionOperation");
            }
            else
            {
                this.UnchekedAdd(finitePart, bigPart, additionOperation);
            }
        }

        /// <summary>
        /// Obtém uma representação textual de um objecto do tipo <see cref="SimplexMaximumNumberField{ObjectType}"/>
        /// </summary>
        /// <returns>A representação textual do objecto.</returns>
        public override string ToString()
        {
            return string.Format("{0} + Inf( {1} )", this.finitePart, this.bigPart);
        }

        /// <summary>
        /// Permite comparar dois números grandes sem efectuar qualquer verificação dos argumentos.
        /// </summary>
        /// <remarks>
        /// A utilização é destinada aos processo internos da livraria.
        /// </remarks>
        /// <param name="first">O primeiro número a ser comparado.</param>
        /// <param name="second">O segundo número a ser comparado.</param>
        /// <param name="comparer">O comparador de coeficientes.</param>
        /// <returns>
        /// O valor 1 caso o primeiro número seja superior ao segundo, 0 se ambos forem iguais e -1 se o 
        /// primeiro número for inferior ao segundo.
        /// </returns>
        internal static int UncheckedCompare(
            SimplexMaximumNumberField<ObjectType> first,
            SimplexMaximumNumberField<ObjectType> second,
            IComparer<ObjectType> comparer)
        {
            var compareValue = comparer.Compare(first.bigPart, second.bigPart);
            if (compareValue == 0)
            {
                return comparer.Compare(first.finitePart, second.finitePart);
            }
            else
            {
                return compareValue;
            }
        }

        /// <summary>
        /// Adicionar os valores especificados aos valores existentes.
        /// </summary>
        /// <remarks>
        /// A utilização é destinada aos processo internos da livraria.
        /// </remarks>
        /// <param name="finitePart">Valor a ser adicionado à parte finita.</param>
        /// <param name="bigPart">Valor a ser adicionado à parte grande.</param>
        /// <param name="additionOperation">O objecto responsável pela adição dos coeficientes.</param>
        public void UnchekedAdd(
            ObjectType finitePart,
            ObjectType bigPart,
            IAdditionOperation<ObjectType, ObjectType, ObjectType> additionOperation)
        {
            this.finitePart = additionOperation.Add(this.finitePart, finitePart);
            this.bigPart = additionOperation.Add(this.bigPart, bigPart);
        }
    }
}
