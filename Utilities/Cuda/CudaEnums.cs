namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define os possíveis resultados de uma chamada CUDA.
    /// </summary>
    public enum CudaResult
    {
        /// <summary>
        /// A chamada à API foi concluída sem erros. Indica também que uma consulta foi concluída.
        /// </summary>
        CudaSuccess = 0,

        /// <summary>
        /// Indica que uma ou mais parâmetros passados para a chamada à API não se encontram
        /// dentro dos limites aceitáveis de valores.
        /// </summary>
        CudaErrorInvalidValue = 1,

        /// <summary>
        /// A chamada à API falhou porque não foi possível alocar memória suficiente para
        /// executar a operação requerida.
        /// </summary>
        CudaErrorOutOfMemory = 2,

        /// <summary>
        /// Indica que o condutor CUDA não foi inicializado com a função <see cref="CudaApi.CudaInit"/>
        /// ou essa inicialização falhou.
        /// </summary>
        CudaErrorNotInitialized = 3,

        /// <summary>
        /// Indica que o condutor CUDA se encontra em processo de encerramento.
        /// </summary>
        CudaErrorDeinitialized = 4,

        /// <summary>
        /// O monitor de desempenho não se encontra inicializado para esta corrida. Isto pode acontecer quando
        /// a aplicação se enconta em execução monitorizada por ferramentas de desempenho externas tais como
        /// o Visual Profiler.
        /// </summary>
        CudaErrorProfilerDisabled = 5,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaProfileStart"/> e <see cref="CudaApi.CudaProfileStop"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        CudaErrorProfilerNotInitialized = 6,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaProfileStart"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        CudaErrorProfilerAlreadyStarted = 7,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaProfileStop"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 5.0.")]
        CudaErrorProfilerAlreadyStopped = 8,

        /// <summary>
        /// Indica que os dispositivos que suportam CUDA foram detectados pelo condutor
        /// instalado.
        /// </summary>
        CudaErrorNoDevice = 100,

        /// <summary>
        /// Indica que o ordinal de dispositivo proporcionado pelo utilizador não corresonde a um
        /// dispositivo CUDA válido.
        /// </summary>
        CudaErrorInvalidDevice = 101,

        /// <summary>
        /// Indica que a imagem de kernel do dispositivo é inválida. Pode também indicar a existência
        /// de um módulo CUDA inválido.
        /// </summary>
        CudaErrorInvalidImage = 200,

        /// <summary>
        /// Frequentemente indica que não existen nenhum contexto ligado à linha de fluxo corrente.
        /// Também pode ser retornado se o contexto passado para a chamada à API não foi um manuseador
        /// válido (tal como um contexto sobre o qual se invocou <see cref="CudaApi.CudaContexDestroy"/>).
        /// Também pode ser retornado se um utilizador misturar diferentes versões da API (isto é, um contexto
        /// 3010 com chamadas à API 3020). Ver <see cref="CudaApi.CudaContextGetApiVersion"/> para mais detalhes.
        /// </summary>
        CudaErrorInvalidContext = 201,

        /// <summary>
        /// Obsoleto: ver <see cref="CudaApi.CudaContextPushCurrent"/>.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 3.2.")]
        CudaErrorContextAlreadyCurrent = 202,

        /// <summary>
        /// Uma operação de mapeamento ou registo falhou.
        /// </summary>
        CudaErrorMapFailed = 205,

        /// <summary>
        /// Uma operação de eliminação de mapeamento ou registo falhou.
        /// </summary>
        CudaErrorUnmapFailed = 206,

        /// <summary>
        /// O vector corrente encontra-se mapeado e portanto não pode ser destruído.
        /// </summary>
        CudaErrorArrayIsMapped = 207,

        /// <summary>
        /// O recurso já se encontra mapeado.
        /// </summary>
        CudaErrorAlreadyMapped = 208,

        /// <summary>
        /// Não existe nenhuma imagem de kernel disponível que se adeque ao dispositivo. Isto pode ocorrer
        /// quando o utilizador especifica geração de código para um ficheiro de código fonte CUDA particular
        /// que não inclui a configuração de dispositivo correspondente.
        /// </summary>
        CudaErrorNoBinaryForGpu = 209,

        CudaErrorAlreadyAcquired = 210,

        CudaErrorNotMapped = 211,

        CudaErrorNotMappedAsArray = 212,

        CudaErrorNotMappedAsPointer = 213,

        CudaErrorEccUncorrectable = 214,

        CudaErrorUnsupportedLimit = 215,

        CudaErrorContextAlreadyInUse = 216,

        CudaErrorPeerAccessUnsupported = 217,

        CudaErrorInvalidPtx = 218,

        CudaErrorInvalidGraphicsContext = 219,

        CudaErrorInvalidSource = 300,

        CudaErrorFileNotFound = 301,

        CudaErrorSharedObjectSymbolNotFound = 302,

        CudaErrorSharedObjectInitFailed = 303,

        CudaErrorOperatingSystem = 304,

        CudaErrorNotFound = 500,

        CudaErrorNotReady = 600,

        CudaErrorIlegalAddress = 700,

        CudaErrorLaunchOutOfResources = 701,

        CudaErrorLaunchTimeout = 702,

        CudaErrorLaunchIncompatibleTexturing = 703,

        CudaErrorPeerAccessAlreadyEnabled = 704,

        CudaErrorPeerAccessNotEnabled = 705,

        CudaErrorPrimaryContextActive = 708,

        CudaErrorContextIsDestroyed = 709,

        CudaErrorAssert = 710,

        CudaErrorTooManyPeers = 711,

        CudaErrorHostMemoryAlreadyRegisteres = 712,

        CudaErrorHostMemoryNotRegistered = 713,

        CudaErrorHardwareStackError = 714,

        CudaErrorIllegalInstruction = 715,

        CudaErrorMissalignedAddress = 716,

        CudaErrorInvalidAddressSpace = 717,

        CudaErrorInvalidPc = 718,

        CudaErrorLaunchFailed = 719,

        CudaErrorNotPermited = 800,

        CudaErrorNotSupported = 801,

        CudaErrorUnknown = 999
    }
}
