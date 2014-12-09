namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Define a excepção a ser lançada em caso de erros CUDA.
    /// </summary>
    public class CudaException : GenericException<int, string>
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CudaException"/>.
        /// </summary>
        /// <param name="code">O código do erro.</param>
        /// <param name="data">Os dados que serão mapeados às etiquetas.</param>
        public CudaException(int code = default(int), Dictionary<string, object> data = null)
            : base()
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CudaException"/>.
        /// </summary>
        /// <param name="message">A mensagem da excepção.</param>
        /// <param name="code">O código do erro.</param>
        /// <param name="data">Os dados que serão mapeados às etiquetas.</param>
        public CudaException(
            string message, 
            int code = default(int),
            Dictionary<string, object> data = null)
            : base(message)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CudaException"/>.
        /// </summary>
        /// <param name="message">A mensagem da excepção.</param>
        /// <param name="innerException">A excepção interna.</param>
        /// <param name="code">O código do erro.</param>
        /// <param name="data">Os dados que serão mapeados às etiquetas.</param>
        public CudaException(
            string message, 
            Exception innerException, 
            int code = default(int), 
            Dictionary<string, object> data = null)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="CudaException"/>.
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
        public CudaException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context,
            int code = default(int), 
            Dictionary<string, object> data = null)
            : base(info, context)
        {
        }

        /// <summary>
        /// Constrói uma excepção a partir do resultado CUDA <see cref="ECudaResult"/>.
        /// </summary>
        /// <param name="cudaResult">O resultado CUDA.</param>
        /// <returns>A excepção gerada.</returns>
        public static CudaException GetExceptionFromCudaResult(ECudaResult cudaResult)
        {
            var errorCode = (int)cudaResult;
            var errorPtr = IntPtr.Zero;
            var errorGetResult = CudaApi.CudaGetErrorString(cudaResult, ref errorPtr);
            var cudaErrorMessage = default(string);
            if (errorGetResult == ECudaResult.CudaSuccess)
            {
                cudaErrorMessage = Marshal.PtrToStringAnsi(errorPtr);
            }
            else
            {
                cudaErrorMessage = " can't get CUDA error description";
            }

            var exceptionMessage = string.Format(
                    "A CUDA error has occurred: {0}.",
                    cudaErrorMessage);
            return new CudaException(
                exceptionMessage,
                errorCode);
        }
    }
}
