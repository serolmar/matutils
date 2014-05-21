namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa um estado cujas transições são definidas por um delegado.
    /// </summary>
    /// <typeparam name="TSymbVal">O tipo de objectos que constituem os valores dos símbolos.</typeparam>
    /// <typeparam name="TSymbType">O tipo de objectos que constituem os tipos dos símbolos.</typeparam>
    public class DelegateStateImplementation<SymbolValue, SymbolType> : IState<SymbolValue, SymbolType>
    {
        /// <summary>
        /// The state id.
        /// </summary>
        protected int stateId;

        /// <summary>
        /// The state description.
        /// </summary>
        protected string description;

        /// <summary>
        /// The transition delegate.
        /// </summary>
        protected NextStateDelegate<SymbolValue, SymbolType> transitionDelegate;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="DelegateStateImplementation{SymbolValue, SymbolType}"/>.
        /// </summary>
        /// <param name="stateId">O identificador do estado.</param>
        /// <param name="description">A descrição do estado.</param>
        /// <param name="nextStateDelegate">O delegado responsável pela transição.</param>
        public DelegateStateImplementation(
            int stateId,
            string description,
            NextStateDelegate<SymbolValue, SymbolType> nextStateDelegate)
        {
            if (nextStateDelegate == null)
            {
                throw new ArgumentNullException("nextStateDelegate");
            }

            this.stateId = stateId;
            this.description = description;
            this.transitionDelegate = nextStateDelegate;
        }

        /// <summary>
        /// Obtém o próximo estado.
        /// </summary>
        /// <param name="reader">O leitor de símbolos.</param>
        /// <returns>Verdadeiro caso não tenha sido atingido o estado final e falso caso contrário.</returns>
        public IState<SymbolValue, SymbolType> NextState(ISymbolReader<SymbolValue, SymbolType> reader)
        {
            return this.transitionDelegate.Invoke(reader);
        }

        /// <summary>
        /// Determina um valor que indica se o objecto especificado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso o objecto seja igual e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as DelegateStateImplementation<SymbolValue, SymbolType>;
            if (innerObj == null)
            {
                return false;
            }

            return this.stateId == innerObj.stateId;
        }

        /// <summary>
        /// Retorna um código confuso da instância actual.
        /// </summary>
        /// <returns>O código confuso da instância actual utilizado em alguns algoritmos.</returns>
        public override int GetHashCode()
        {
            return this.stateId.GetHashCode();
        }

        /// <summary>
        /// Constrói uma representação textual da instância corrente.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.description))
            {
                return "No description.";
            }
            else
            {
                return this.description;
            }
        }
    }
}
