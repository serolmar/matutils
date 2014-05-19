namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// O delegado responsável pela âncora do evento de uma adição ou remoção a uma função.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto a ser adicionado.</typeparam>
    /// <param name="sender">O objecto que envia o evento.</param>
    /// <param name="eventArgs">Os argumentos do evento.</param>
    public delegate void AddDeleteEventHandler<T>(object sender, AddDeleteEventArgs<T> eventArgs);

    /// <summary>
    /// Representa os argumentos de um evento que ocorre aquando da adição de um objecto.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto a ser adicionado.</typeparam>
    public class AddDeleteEventArgs<T> : EventArgs
    {
        /// <summary>
        /// O objecto adicionado ou removido.
        /// </summary>
        private T addedOrRemovedObject;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="AddDeleteEventArgs{T}"/>.
        /// </summary>
        /// <param name="addedOrRemovedObject">O objecto a ser adicionado ou removido.</param>
        public AddDeleteEventArgs(T addedOrRemovedObject)
        {
            this.addedOrRemovedObject = addedOrRemovedObject;
        }

        /// <summary>
        /// Obtém o objecto adicionado ou removido.
        /// </summary>
        /// <value>O objecto.</value>
        public T AddedOrRemovedObject
        {
            get
            {
                return this.addedOrRemovedObject;
            }
        }
    }
}
