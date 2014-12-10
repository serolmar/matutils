namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma excepção à qual está associado um código de erro.
    /// </summary>
    /// <typeparam name="CodeType">O tipo de dados associado ao erro.</typeparam>
    /// <typeparam name="TagType">
    /// O tipo associado às etiquetas que poderão integrar as mensagens de erro.
    /// </typeparam>
    public class GenericException<CodeType, TagType> : Exception
    {
        /// <summary>
        /// O código de erro.
        /// </summary>
        private CodeType code;

        /// <summary>
        /// Os dados que serão mapeados às etiquetas.
        /// </summary>
        private Dictionary<TagType, object> tagData;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GenericException{CodeType, TagType}"/>.
        /// </summary>
        /// <param name="code">O código do erro.</param>
        /// <param name="data">Os dados que serão mapeados às etiquetas.</param>
        public GenericException(CodeType code = default(CodeType), Dictionary<TagType, object> data = null)
            : base()
        {
            this.SetArguments(code, data);
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GenericException{CodeType, TagType}"/>.
        /// </summary>
        /// <param name="message">A mensagem da excepção.</param>
        /// <param name="code">O código do erro.</param>
        /// <param name="data">Os dados que serão mapeados às etiquetas.</param>
        public GenericException(
            string message, 
            CodeType code = default(CodeType),
            Dictionary<TagType, object> data = null)
            : base(message)
        {
            this.SetArguments(code, data);
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GenericException{CodeType, TagType}"/>.
        /// </summary>
        /// <param name="message">A mensagem da excepção.</param>
        /// <param name="innerException">A excepção interna.</param>
        /// <param name="code">O código do erro.</param>
        /// <param name="data">Os dados que serão mapeados às etiquetas.</param>
        public GenericException(
            string message, 
            Exception innerException, 
            CodeType code = default(CodeType), 
            Dictionary<TagType, object> data = null)
            : base(message, innerException)
        {
            this.SetArguments(code, data);
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GenericException{CodeType, TagType}"/>.
        /// </summary>
        /// <param name="info">
        /// O objecto do tipo <see cref="T:System.Runtime.Serialization.SerializationInfo" /> que contém os 
        /// dados serializados sobre a excepção.
        /// </param>
        /// <param name="context">
        /// O objecot do tipo <see cref="T:System.Runtime.Serialization.StreamingContext" /> que contém
        /// informação contexutal sobre a fonte ou o destino.
        /// </param>
        /// <param name="code">O código do erro.</param>
        /// <param name="data">Os dados que serão mapeados às etiquetas.</param>
        public GenericException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context,
            CodeType code = default(CodeType), 
            Dictionary<TagType, object> data = null)
            : base(info, context)
        {
            this.SetArguments(code, data);
        }

        /// <summary>
        /// Obtém o código de erro.
        /// </summary>
        public CodeType Code
        {
            get
            {
                return this.code;
            }
        }

        /// <summary>
        /// Obtém os dados que serão mapeados às etiquetas.
        /// </summary>
        public Dictionary<TagType, object> TagData
        {
            get
            {
                return this.tagData;
            }
        }

        /// <summary>
        /// Efectua a atribuição dos campos internos a partir dos argumentos.
        /// </summary>
        /// <param name="code">O código de erro.</param>
        /// <param name="data">Os dados que poderão estar associados à mensagem e erro.</param>
        private void SetArguments(CodeType code, Dictionary<TagType, object> data)
        {
            this.code = code;
            if (data == null)
            {
                this.tagData = new Dictionary<TagType, object>();
            }
            else
            {
                this.tagData = data;
            }
        }
    }
}
