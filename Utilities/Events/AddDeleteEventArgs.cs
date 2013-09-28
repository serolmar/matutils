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
    internal delegate void AddDeleteEventHandler<T>(object sender, AddDeleteEventArgs<T> eventArgs);

    /// <summary>
    /// Representa os argumentos de um evento que ocorre aquando da adição de um objecto.
    /// </summary>
    /// <typeparam name="T">O tipo de objecto a ser adicionado.</typeparam>
    internal class AddDeleteEventArgs<T> : EventArgs
    {
        /// <summary>
        /// O objecto adicionado ou removido.
        /// </summary>
        private T addedOrRemovedObject;

        public AddDeleteEventArgs(T addedOrRemovedObject)
        {
            this.addedOrRemovedObject = addedOrRemovedObject;
        }

        /// <summary>
        /// Obtém o objecto adicionado ou removido.
        /// </summary>
        public T AddedOrRemovedObject
        {
            get
            {
                return this.addedOrRemovedObject;
            }
        }
    }
}
