namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Enumeração de todos os erros susceptíveis de ocorrer durante a chamada de funções Cuda.
    /// </summary>
    public enum ECudaError
    {
        /// <summary>
        /// A chamada à API retornou sem erros a consulta foi bem-sucedida.
        /// </summary>
        CudaSuccess = 0,

        /// <summary>
        /// Falha no dispositivo causada normalmente pela chamada à função <see cref="CudaDriver.CudaLaunch"/>
        /// por este não ter sido configurado por intermédio da chamada à função 
        /// <see cref="CudaDriver.CudaConfigCall"/>.
        /// </summary>
        CudaErrorMissingConfiguration = 1,

        /// <summary>
        /// A chamada à API falhou porque não foi possível alocar memória suficiente para satisfazer
        /// a operação.
        /// </summary>
        CudaErrorMemoryAllocation = 2,

        /// <summary>
        /// A API falhou porque o condutor de CUDA ou o sistema não puderam ser inicializados.
        /// </summary>
        CudaErrorInitializationError = 3,

        /// <summary>
        /// Ocorreu uma excepçáo no dispositivo durante a execução do kernel. O rol de causas comuns inclui
        /// a má referência de um apontador para regiões no exterior do limite da memória partilhada. O dispositivo
        /// não pode ser utilizado até que seja chamada a função <see cref="CudaDriver.CudaThreadExit"/>. Todas
        /// as alocações de memória existentes no dispositivo são inválidas e estas terão de ser reconstituídas
        /// de modo a ser possível continuar a usufruir dos processos CUDA.
        /// </summary>
        CudaErrorLaunchFailure = 4,

        /// <summary>
        /// Indica que um processo kernel anterior falhou e era usado na emulação de lançamentos de kernel.
        /// </summary>
        [Obsolete("Deprecated as of CUDA 3.1 since device emulation was removed.")]
        CudaErrorPriorLaunchFailure = 5,

        /// <summary>
        /// Indica que o kernel do dispositivo demorou demasiado tempo a ser executado. Isto ocorre se os
        /// limites de execução se encontrarem activos - ver as propriedades do dispositivo com o auxílio da
        /// variável <see cref="CudaDriver.KernelExecTimeoutEnabled"/> para informação adicional. O dispositivo
        /// não pode ser utilizado até que seja chamada a função <see cref="CudaDriver.CudaThreadExit"/>. Todas
        /// as alocações de memória são inválidas e devem ser reconstruídas de modo a ser possível continuar a
        /// usufruir dos processos CUDA.
        /// </summary>
        CudaErrorLaunchTimeout = 6,

        /// <summary>
        /// Indica que o lançamento não ocorreu porque não existem recursos apropriados. Apesar deste erro ser
        /// semelhante ao erro <see cref="ECudaError.CudaErrorInvalidConfiguration"/>, indica geralmente que o
        /// utilizador tentou passar demasiados argumentos para o kernel do dispositivo ou o lançamento do kernel
        /// especifica demasiadas linhas de fluxo para o contador de registo do kernel.
        /// </summary>
        CudaErrorLaunchOutOfResources = 7,

        /// <summary>
        /// A função de dispositivo requerida não existe ou não se encontra adequadamente compilada para a
        /// arquitectura correcta.
        /// </summary>
        CudaErrorInvalidDeviceFunction = 8,

        /// <summary>
        /// O lançamento do kernel está a requerer recursos que nunca poderão ser satisfeitos pelo dispositivo
        /// actual. O pedido de mais memória partilhada por bloco do que aquele que o dispositivo suporta
        /// despoleta este erro, assim como o pedido de demasiadas linhas de fluxo ou blocos. Ver
        /// <see cref="CudaDriver.CudaDeviceProp"/> para mais limitações do dispositivo.
        /// </summary>
        CudaErrorInvalidConfiguration = 9,

        /// <summary>
        /// O ordinal de dispositivo fornecido pelo utilizador não corresponde a um dispositivo CUDA.
        /// </summary>
        CudaErrorInvalidDevice = 10,

        /// <summary>
        /// Um ou mais parâmetros passados para a chamada da API não está dentro do alcance aceitável para os
        /// valores.
        /// </summary>
        CudaErrorInvalidValue = 11,

        /// <summary>
        /// Um ou mais parâmetros relacionados com o passo passados para a chamada da API não se encontram
        /// dentro dos limites aceitáveis para esse tipo de parâmetros.
        /// </summary>
        CudaErrorInvalidPitchValue = 12,

        /// <summary>
        /// Indica que o símbolo name/identifier passado para a chamada da API não é um nome ou indentificador
        /// válido.
        /// </summary>
        CudaErrorInvalidSymbol = 13,

        /// <summary>
        /// O objecto amortecedor não pode ser mapeado.
        /// </summary>
        CudaErrorMapBufferObjectFailed = 14,

        /// <summary>
        /// Não é possível eliminar o mapeamento do objecto amortecedor.
        /// </summary>
        CudaErrorUnmapBufferObjectFailed = 15,

        /// <summary>
        /// Indica que pelo menos um apontador de anfitrião passado para a chamada da API não é um apontador
        /// de anfitrião válido.
        /// </summary>
        CudaErrorInvalidHostPointer = 16,

        /// <summary>
        /// Indica que pelo menos um apontador de dispositivo passado para a chamada da API não é um apontador
        /// de dispositivo válido.
        /// </summary>
        CudaErrorInvalidDevicePointer = 17,

        /// <summary>
        /// Indica que a textura passsada para a chamada da API não é uma textura válida.
        /// </summary>
        CudaErrorInvalidTexture = 18,

        /// <summary>
        /// A ligação da textura não é válida. Ocorre quando é chamada a função 
        /// <see cref="CudaDriver.CudaGetTextureAlignementOffset"/> com uma textura que não se encontra no
        /// intervalo das texturas.
        /// </summary>
        CudaErrorInvalidTextureBinding = 19,

        /// <summary>
        /// O descritor de canal passado para a chamada da API não é válido. Ocorre se o formato não for um
        /// dos formatos especificados por <see cref="CudaDriver.CudaChannelFormatKind"/> ou se uma das dimensões
        /// for inválida.
        /// </summary>
        CudaErrorInvalidChannelDescriptor = 20,

        /// <summary>
        /// Indica que a direcção da cópida de memória passada para a chamada da API não é um dos tipos
        /// especificados por <see cref="CudaDriver.CudaMemcpyKind"/>.
        /// </summary>
        CudaErrorInvalidMemcpyDirection = 21,

        /// <summary>
        /// Indica que o utilizador tomou o endereço de uma variável constante.
        /// </summary>
        [Obsolete("Used in versions prior to the CUDA 3.1 release.")]
        CudaErrorAddressOfConstant = 22,

        /// <summary>
        /// Indica que a obtenção da textura não pode ser efectuada, anteriormente usado na emulação de
        /// operações sobre texturas.
        /// </summary>
        [Obsolete("Used in versions prior to the CUDA 3.1 release.")]
        CudaErrorTextureFetchFailed = 23,

        /// <summary>
        /// Indica que a textura não foi conseguida para acesso, anteriormente usado na emulação de
        /// operações sobre texturas.
        /// </summary>
        [Obsolete("Used in versions prior to the CUDA 3.1 release.")]
        CudaErrorTextureNotBound = 24,

        /// <summary>
        /// Falha na operações de sincronização, anteriormente usado na emulação de operações sobre texturas.
        /// </summary>
        [Obsolete("Used in versions prior to the CUDA 3.1 release.")]
        CudaErrorSynchronizationError = 25,

        /// <summary>
        /// Indica que a textura non-float se encontrava a ser acedida por intermédio de linear filtering.
        /// Isto não é suportado por CUDA.
        /// </summary>
        CudaErrorInvalidFilterSetting = 26,

        /// <summary>
        /// Uma tentativa de leitura de uma textura non-float como sendo uma textura normalizada. Esta operação
        /// não é suportada por CUDA.
        /// </summary>
        CudaErrorInvalidNormSetting = 27,

        /// <summary>
        /// A mistura de código de dispositivo e código de emulação de dispositivo não é permitida.
        /// </summary>
        [Obsolete("Used in versions prior to the CUDA 3.1 release.")]
        CudaErrorMixedDeviceExecution = 28,

        /// <summary>
        /// Indica que uma chamada da API CUDA não pode ser executada porque está a ser chamada durante
        /// o encerramento do processo num ponto posterior ao descarregamento do condutor de CUDA.
        /// </summary>
        CudaErrorCudartUnloading = 29,

        /// <summary>
        /// Indicação de um erro interno desconhecido.
        /// </summary>
        CudaErrorUnknown = 30,

        /// <summary>
        /// Indica que a chamada da API ainda não se encontra implementada. Ambientes de produção de CUDA
        /// nunca deverão despoletar este erro.
        /// </summary>
        [Obsolete("Used in versions prior to the CUDA 4.1.")]
        CudaErrorNotYetImplemented = 31,

        /// <summary>
        /// Indica que um apontador de dispositivo emulado excedeu o limite de 32 bit.
        /// </summary>
        [Obsolete("Used in versions prior to the CUDA 3.1 release.")]
        CudaErrorMemoryValueTooLarge = 32,

        /// <summary>
        /// Indica que o manuseador do recurso passado para a chamada da API não é válido. Os manuseadores
        /// de recursos são tipos opacos tais como <see cref="CudaStream"/> e <see cref="CudaEvent"/>.
        /// </summary>
        CudaErrorInvalidResourceHandle = 33,

        /// <summary>
        /// Operações assíncronas anteriormente emitidas ainda não foram concluídas. Este resultado não constitui
        /// exactamente um erro mas deve ser distinto de <see cref="ECudaError.CudaSuccess"/> (que indica
        /// operação bem-sucedida). Chamadas que poderão retornar este valor são 
        /// <see cref="CudaDriver.CudaEventQuery"/> e <see cref="CudaDriver.CudaStreamQuery"/>.
        /// </summary>
        CudaErrorNotReady = 34,

        /// <summary>
        /// Indica que o condutor de CUDA instalado é mais antigo do que a livraria que se encontra activa.
        /// Trata-se de uma configuração que não é suportada. Os utilizadores deverão instalar uma actualização
        /// do condutor da NVIDIA de modo a permiter a execução da aplicação.
        /// </summary>
        CudaErrorInsufficientDriver = 35,

        /// <summary>
        /// Indica que o utilizador chamou <see cref="CudaDriver.CudaSetValidDevices"/>, 
        /// <see cref="CudaDriver.CudaSetDeviceFlats"/>, <see cref="CudaDriver.CudaD3D9Direct3DDevice"/>,
        /// <see cref="CudaDriver.CudaD3D10SetDirect3DDevice"/>, 
        /// <see cref="CudaDriver.CudaD3D11SetDirect3DDevice"/> ou <see cref="CudaDriver.CudaVDPAUSetVDPAUDevice"/>
        /// após inicializar o motor CUDA por intermédio da chamada de funções externas ao dispositivo de gestão
        /// (alocação de memória e lançamentos de kernel são exemplos de tais operações de gestão). Este erro
        /// também pode ser retornado durante a utilização da interoperabilidade de motor/condutor e existe um
        /// contexto <see cref="CudaContext"/> actvio na linha de fluxo do anfitrião.
        /// </summary>
        CudaErrorSetOnActiveProcess = 36,

        /// <summary>
        /// Indica que a surface passada para a chamada da API não é uma surface válida.
        /// </summary>
        CudaErrorInvalidSurface = 37,

        /// <summary>
        /// Indica que dispositivos sem suporte CUDA foram detectados pelo condutor de CUDA instalado.
        /// </summary>
        CudaErrorNoDevice = 38,

        /// <summary>
        /// Indica que um erro de ECC que não pode ser corrigido foi detectado.
        /// </summary>
        CudaErrorECCUncorrectable = 39,

        /// <summary>
        /// Indica que não foi possível resolver a ligação para um objecto partilhado.
        /// </summary>
        CudaErrorSharedObjectSymbolNotFound = 40,

        /// <summary>
        /// Falha na inicizaliação de um objecto partilhado.
        /// </summary>
        CudaErrorSharedObjectInitFailed = 41,

        /// <summary>
        /// Indica que o <see cref="CudaDriver.CudaLimit"/> passado para a chamada da API não é suportado
        /// pelo dispositivo corrente.
        /// </summary>
        CudaErrorUnsupportedLimit = 42,

        /// <summary>
        /// Indica que múltiplas variáveis globais ou constantes (ao longo de ficheiros fonte de CUDA separados
        /// na aplicação) partilham o mesmo nome de string.
        /// </summary>
        CudaErrorDuplicateVariableName = 43,

        /// <summary>
        /// Indica que múltiplas texturas (ao longo de ficheiros fonte de CUDA separados
        /// na aplicação) partilham o mesmo nome de string.
        /// </summary>
        CudaErrorDuplicateTextureName = 44,

        /// <summary>
        /// Indica que múltiplas surfaces (ao longo de ficheiros fonte de CUDA separados
        /// na aplicação) partilham o mesmo nome de string.
        /// </summary>
        CudaErrorDuplicateSurfaceName = 45,

        /**
         * This indicates that all Cuda devices are busy or unavailable at the current
         * time. Devices are often busy/unavailable due to use of
         * ::CudaComputeModeExclusive, ::CudaComputeModeProhibited or when long
         * running Cuda kernels have filled up the GPU and are blocking new work
         * from starting. They can also be unavailable due to memory constraints
         * on a device that already has active Cuda work being performed.
         */
        CudaErrorDevicesUnavailable = 46,

        /**
         * This indicates that the device kernel image is invalid.
         */
        CudaErrorInvalidKernelImage = 47,

        /**
         * This indicates that there is no kernel image available that is suitable
         * for the device. This can occur when a user specifies code generation
         * options for a particular Cuda source file that do not include the
         * corresponding device configuration.
         */
        CudaErrorNoKernelImageForDevice = 48,

        /**
         * This indicates that the current context is not compatible with this
         * the Cuda Runtime. This can only occur if you are using Cuda
         * Runtime/Driver interoperability and have created an existing Driver
         * context using the driver API. The Driver context may be incompatible
         * either because the Driver context was created using an older version 
         * of the API, because the Runtime API call expects a primary driver 
         * context and the Driver context is not primary, or because the Driver 
         * context has been destroyed. Please see \ref CudaRT_DRIVER "Interactions 
         * with the Cuda Driver API" for more information.
         */
        CudaErrorIncompatibleDriverContext = 49,

        /**
         * This error indicates that a call to ::CudaDeviceEnablePeerAccess() is
         * trying to re-enable peer addressing on from a context which has already
         * had peer addressing enabled.
         */
        CudaErrorPeerAccessAlreadyEnabled = 50,

        /**
         * This error indicates that ::CudaDeviceDisablePeerAccess() is trying to 
         * disable peer addressing which has not been enabled yet via 
         * ::CudaDeviceEnablePeerAccess().
         */
        CudaErrorPeerAccessNotEnabled = 51,

        /**
         * This indicates that a call tried to access an exclusive-thread device that 
         * is already in use by a different thread.
         */
        CudaErrorDeviceAlreadyInUse = 54,

        /**
         * This indicates profiler is not initialized for this run. This can
         * happen when the application is running with external profiling tools
         * like visual profiler.
         */
        CudaErrorProfilerDisabled = 55,

        /**
         * \deprecated
         * This error return is deprecated as of Cuda 5.0. It is no longer an error
         * to attempt to enable/disable the profiling via ::CudaProfilerStart or
         * ::CudaProfilerStop without initialization.
         */
        CudaErrorProfilerNotInitialized = 56,

        /**
         * \deprecated
         * This error return is deprecated as of Cuda 5.0. It is no longer an error
         * to call CudaProfilerStart() when profiling is already enabled.
         */
        CudaErrorProfilerAlreadyStarted = 57,

        /**
         * \deprecated
         * This error return is deprecated as of Cuda 5.0. It is no longer an error
         * to call CudaProfilerStop() when profiling is already disabled.
         */
        CudaErrorProfilerAlreadyStopped = 58,

        /**
         * An assert triggered in device code during kernel execution. The device
         * cannot be used again until ::CudaThreadExit() is called. All existing 
         * allocations are invalid and must be reconstructed if the program is to
         * continue using Cuda. 
         */
        CudaErrorAssert = 59,

        /**
         * This error indicates that the hardware resources required to enable
         * peer access have been exhausted for one or more of the devices 
         * passed to ::CudaEnablePeerAccess().
         */
        CudaErrorTooManyPeers = 60,

        /**
         * This error indicates that the memory range passed to ::CudaHostRegister()
         * has already been registered.
         */
        CudaErrorHostMemoryAlreadyRegistered = 61,

        /**
         * This error indicates that the pointer passed to ::CudaHostUnregister()
         * does not correspond to any currently registered memory region.
         */
        CudaErrorHostMemoryNotRegistered = 62,

        /**
         * This error indicates that an OS call failed.
         */
        CudaErrorOperatingSystem = 63,

        /**
         * This error indicates that P2P access is not supported across the given
         * devices.
         */
        CudaErrorPeerAccessUnsupported = 64,

        /**
         * This error indicates that a device runtime grid launch did not occur 
         * because the depth of the child grid would exceed the maximum supported
         * number of nested grid launches. 
         */
        CudaErrorLaunchMaxDepthExceeded = 65,

        /**
         * This error indicates that a grid launch did not occur because the kernel 
         * uses file-scoped textures which are unsupported by the device runtime. 
         * Kernels launched via the device runtime only support textures created with 
         * the Texture Object API's.
         */
        CudaErrorLaunchFileScopedTex = 66,

        /**
         * This error indicates that a grid launch did not occur because the kernel 
         * uses file-scoped surfaces which are unsupported by the device runtime.
         * Kernels launched via the device runtime only support surfaces created with
         * the Surface Object API's.
         */
        CudaErrorLaunchFileScopedSurf = 67,

        /**
         * This error indicates that a call to ::CudaDeviceSynchronize made from
         * the device runtime failed because the call was made at grid depth greater
         * than than either the default (2 levels of grids) or user specified device 
         * limit ::CudaLimitDevRuntimeSyncDepth. To be able to synchronize on 
         * launched grids at a greater depth successfully, the maximum nested 
         * depth at which ::CudaDeviceSynchronize will be called must be specified 
         * with the ::CudaLimitDevRuntimeSyncDepth limit to the ::CudaDeviceSetLimit
         * api before the host-side launch of a kernel using the device runtime. 
         * Keep in mind that additional levels of sync depth require the runtime 
         * to reserve large amounts of device memory that cannot be used for 
         * user allocations.
         */
        CudaErrorSyncDepthExceeded = 68,

        /**
         * This error indicates that a device runtime grid launch failed because
         * the launch would exceed the limit ::CudaLimitDevRuntimePendingLaunchCount.
         * For this launch to proceed successfully, ::CudaDeviceSetLimit must be
         * called to set the ::CudaLimitDevRuntimePendingLaunchCount to be higher 
         * than the upper bound of outstanding launches that can be issued to the
         * device runtime. Keep in mind that raising the limit of pending device
         * runtime launches will require the runtime to reserve device memory that
         * cannot be used for user allocations.
         */
        CudaErrorLaunchPendingCountExceeded = 69,

        /**
         * This error indicates the attempted operation is not permitted.
         */
        CudaErrorNotPermitted = 70,

        /**
         * This error indicates the attempted operation is not supported
         * on the current system or device.
         */
        CudaErrorNotSupported = 71,

        /**
         * Device encountered an error in the call stack during kernel execution,
         * possibly due to stack corruption or exceeding the stack size limit.
         * The context cannot be used, so it must be destroyed (and a new one should be created).
         * All existing device memory allocations from this context are invalid
         * and must be reconstructed if the program is to continue using Cuda.
         */
        CudaErrorHardwareStackError = 72,

        /**
         * The device encountered an illegal instruction during kernel execution
         * The context cannot be used, so it must be destroyed (and a new one should be created).
         * All existing device memory allocations from this context are invalid
         * and must be reconstructed if the program is to continue using Cuda.
         */
        CudaErrorIllegalInstruction = 73,

        /**
         * The device encountered a load or store instruction
         * on a memory address which is not aligned.
         * The context cannot be used, so it must be destroyed (and a new one should be created).
         * All existing device memory allocations from this context are invalid
         * and must be reconstructed if the program is to continue using Cuda.
         */
        CudaErrorMisalignedAddress = 74,

        /**
         * While executing a kernel, the device encountered an instruction
         * which can only operate on memory locations in certain address spaces
         * (global, shared, or local), but was supplied a memory address not
         * belonging to an allowed address space.
         * The context cannot be used, so it must be destroyed (and a new one should be created).
         * All existing device memory allocations from this context are invalid
         * and must be reconstructed if the program is to continue using Cuda.
         */
        CudaErrorInvalidAddressSpace = 75,

        /**
         * The device encountered an invalid program counter.
         * The context cannot be used, so it must be destroyed (and a new one should be created).
         * All existing device memory allocations from this context are invalid
         * and must be reconstructed if the program is to continue using Cuda.
         */
        CudaErrorInvalidPc = 76,

        /**
         * The device encountered a load or store instruction on an invalid memory address.
         * The context cannot be used, so it must be destroyed (and a new one should be created).
         * All existing device memory allocations from this context are invalid
         * and must be reconstructed if the program is to continue using Cuda.
         */
        CudaErrorIllegalAddress = 77,


        /**
         * This indicates an internal startup failure in the Cuda runtime.
         */
        CudaErrorStartupFailure = 0x7f,

        /**
         * Any unhandled Cuda driver error is added to this value and returned via
         * the runtime. Production releases of Cuda should not return such errors.
         * \deprecated
         * This error return is deprecated as of Cuda 4.1.
         */
        CudaErrorApiFailureBase = 10000
    }
}
