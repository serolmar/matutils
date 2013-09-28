namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// O delegado responsável pela âncora de um evento de actualização a uma função.
    /// </summary>
    /// <typeparam name="T">O tipo de dados do valor.</typeparam>
    /// <param name="eventArgs">Os argumentos do evento.</param>
    internal delegate void UpdateEventHandler<OldValueType, NewValueType>(
    object sender,
    UpdateEventArgs<OldValueType, NewValueType> eventArgs);

    /// <summary>
    /// Representa os argumentos do evento que ocorre aquando da actualização de um valor.
    /// </summary>
    /// <typeparam name="OldValueType">O tipo do valor inicial.</typeparam>
    /// <typeparam name="NewValueType">O tipo do valor final.</typeparam>
    internal class UpdateEventArgs<OldValueType, NewValueType> : EventArgs
    {
        /// <summary>
        /// O valor anterior.
        /// </summary>
        private OldValueType oldValue;

        /// <summary>
        /// O novo valor.
        /// </summary>
        private NewValueType newValue;

        public UpdateEventArgs(OldValueType oldValue, NewValueType newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        /// <summary>
        /// Obtém o valor antigo.
        /// </summary>
        public OldValueType OldValue
        {
            get
            {
                return this.oldValue;
            }
        }

        /// <summary>
        /// Obtém o novo valor.
        /// </summary>
        public NewValueType NewValue
        {
            get
            {
                return this.newValue;
            }
        }
    }
}
