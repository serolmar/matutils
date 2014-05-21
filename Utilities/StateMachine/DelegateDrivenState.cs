namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// O delegado responsável pela transição entre estados.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo dos objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    /// <param name="reader">O leitor de símbolos.</param>
    /// <returns>O próximo estado.</returns>
    public delegate IState<TSymbVal, TSymbType> NextStateDelegate<TSymbVal, TSymbType>(ISymbolReader<TSymbVal, TSymbType> reader);

    /// <summary>
    /// Implementa um estado cujas transições são definidas por um delegado.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo dos objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo dos objectos que constituem os tipos dos símbolos.</typeparam>
    public class DelegateDrivenState<TSymbVal, TSymbType> : IState<TSymbVal, TSymbType>
    {
        /// <summary>
        /// O identificador do estado.
        /// </summary>
        private int stateID;

        /// <summary>
        /// A descrição do estado.
        /// </summary>
        private string description = string.Empty;

        /// <summary>
        /// O delegado responsável pela próxima transição.
        /// </summary>
        private NextStateDelegate<TSymbVal, TSymbType> nextStateDelegate;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DelegateDrivenState{TSymbVal, TSymbType}"/>.
        /// </summary>
        /// <param name="stateID">O identificador do estado.</param>
        /// <param name="description">A descrição do estado.</param>
        /// <param name="nextStateDelegate">O delegado responsável pela transição.</param>
        /// <<exception cref="ArgumentNullException">Se o estado responsável pela próxima transição for nulo.</exception>
        public DelegateDrivenState(
            int stateID, 
            string description, 
            NextStateDelegate<TSymbVal, TSymbType> nextStateDelegate)
        {
            if (nextStateDelegate == null)
            {
                throw new ArgumentNullException("nextStateDelegate");
            }

            this.stateID = stateID;
            this.description = description;
            this.nextStateDelegate = nextStateDelegate;
        }

        /// <summary>
        /// Obtém o próximo estado com base no símbolo lido.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>O próximo estado.</returns>
        public IState<TSymbVal, TSymbType> NextState(ISymbolReader<TSymbVal, TSymbType> reader)
        {
            return this.nextStateDelegate.Invoke(reader);
        }

        /// <summary>
        /// Determina se o objecto é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso o objecto seja igual e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var left = obj as DelegateDrivenState<TSymbVal, TSymbType>;
            if (left == null)
            {
                return false;
            }

            return this.stateID.Equals(left.stateID);
        }

        /// <summary>
        /// Retorna um código confuso para a instância corrente.
        /// </summary>
        /// <returns>O código confuso da instância corrente utilizado em alguns algoritmos.</returns>
        public override int GetHashCode()
        {
            return this.stateID.GetHashCode();
        }
    }
}
