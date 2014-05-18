namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa erros que possam ocorrer com as colecções.
    /// </summary>
    public class CollectionsException : UtilitiesException
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CollectionsException"/>.
        /// </summary>
        public CollectionsException() : base() { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CollectionsException"/>.
        /// </summary>
        /// <param name="message">A mensagem da excepção.</param>
        public CollectionsException(string message) : base(message) { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CollectionsException"/>.
        /// </summary>
        /// <param name="message">A mensagem da excepção.</param>
        /// <param name="innerException">A excepção interna.</param>
        public CollectionsException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CollectionsException"/>.
        /// </summary>
        /// <param name="info">
        /// O objecto do tipo <see cref="T:System.Runtime.Serialization.SerializationInfo" /> que contém os 
        /// dados serializados sobre a excepção.
        /// </param>
        /// <param name="context">
        /// O objecot do tipo <see cref="T:System.Runtime.Serialization.StreamingContext" /> que contém
        /// informação contexutal sobre a fonte ou o destino.
        /// </param>
        public CollectionsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context
            )
            : base(info, context) { }
    }
}
