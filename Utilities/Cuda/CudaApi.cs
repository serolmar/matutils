namespace Utilities.Cuda
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Permite fazer ligação estática às funções diponibilizadas pela API CUDA.
    /// </summary>
    public static class CudaApi
    {
        /// <summary>
        /// O nome da dll que exporta a API CUDA.
        /// </summary>
        internal const string DllName = "nvcuda";

        #region Gestão dos erros

        /// <summary>
        /// Obtém o nome do erro CUDA.
        /// </summary>
        /// <param name="error">O erro.</param>
        /// <param name="ptrStr">Apontador para um vector de mensagens de texto.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/> ou <see cref="ECudaResult.CuraErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGetErrorName")]
        public static extern ECudaResult CudaGetErrorName(ECudaResult error, ref string ptrStr);

        /// <summary>
        /// Obtém a descrição de um código de erro.
        /// </summary>
        /// <param name="error">O erro.</param>
        /// <param name="ptrStr">O apontador para a descrição.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/> ou <see cref="ECudaResult.CuraErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuGetErrorString")]
        public static extern ECudaResult CudaGetErrorString(ECudaResult error, ref string ptrStr);

        #endregion Gestão dos erros

        #region Inicialização

        /// <summary>
        /// Inicializa o condutor de CUDA e de ser chamda antes de qualquer outra função da API. Actualmente,
        /// o parâmetro das marcas tem de ser 0. Se <see cref="CudaApi.CudaInit"/> não for chamada qualquer
        /// função da API retornará <see cref="ECudaResult.CudaErrorNotInitialized"/>.
        /// </summary>
        /// <param name="flags">As marcas de inicialização.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSucess"/>,
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/> ou
        /// <see cref="ECudaResult.CudaErrorInvalidDevice"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuInit")]
        public static extern ECudaResult CudaInit(uint flags);

        #endregion Inicialização

        #region Gestão de versões

        /// <summary>
        /// Retorna o número da versão do condutor CUDA instalado. Esta função retorna automaticamente
        /// <see cref="ECudaResult.CudaErrorInvalidValue"/> se o argumento <see cref="driverVersion"/> for nulo.
        /// </summary>
        /// <param name="driverVersion">Irá conter o número da versão.</param>
        /// <returns>
        /// <see cref="ECudaResult.CudaSuccess"/> e <see cref="ECudaResult.CudaErrorInvalidValue"/>.
        /// </returns>
        [DllImport(DllName, EntryPoint = "cuDriverGetVersion")]
        public static extern ECudaResult CudaDriverGetVersion(ref int driverVersion);

        #endregion Gestão de versões
    }
}
