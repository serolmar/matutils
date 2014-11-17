namespace Utilities.Cuda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Manuseador para o evento CUDA.
    /// </summary>
    public struct SCudaIpcEventHandle
    {
        /// <summary>
        /// O descritor do manuseador.
        /// </summary>
        char[] reserved;
    }

    /// <summary>
    /// Manuseador para a memória de IPC.
    /// </summary>
    public struct SCudaIpcMemHandle
    {
        /// <summary>
        /// O descritor do manuseador.
        /// </summary>
        char[] reserved;
    }

    /// <summary>
    /// Define um stream Cuda.
    /// </summary>
    struct CudaStream
    {
    }

    /// <summary>
    /// Define um evento CUDA.
    /// </summary>
    public struct SCudaEvent
    {
    }

    /// <summary>
    /// Define os recursos gráficos de CUDA.
    /// </summary>
    public struct SCudaGraphicsResource
    {
    }

    /// <summary>
    /// Define um UUID CUDA.
    /// </summary>
    public struct SCudaUuid
    {
        /// <summary>
        /// O UUID CUDA.
        /// </summary>
        char[] reserved;
    }

    /// <summary>
    /// Propriedades do dispositivo CUDA.
    /// </summary>
    public struct SCudaDeviceProp
    {
        /// <summary>
        /// O nome que identifica o dispositivo.
        /// </summary>
        char[] Name;

        /// <summary>
        /// A memória global disponível no dispositvo em bytes.
        /// </summary>
        long TotalGlobalMem;

        /// <summary>
        /// A memória partilhada disponível por bloco.
        /// </summary>
        long SharedMemPerBlock;

        /// <summary>
        /// Número de registos de 32 bit disponíveis por bloco.
        /// </summary>
        int RegsPerBlock;

        /// <summary>
        /// Tamanho do Warp em threads.
        /// </summary>
        int WarpSize;

        /// <summary>
        /// O passo em bytes permitido para cópias de memória.
        /// </summary>
        long MemPitch;

        /// <summary>
        /// Número máximo de threads por bloco.
        /// </summary>
        int MaxThreadsPerBlock;

        /// <summary>
        /// Tamanho máximo de cada dimensão de um bloco.
        /// </summary>
        int[] MaxThreadsDim;

        /// <summary>
        /// Tamanho máximo de cada dimensão de uma grelha.
        /// </summary>
        int[] MaxGridSize;

        /// <summary>
        /// Frequência do relógio.
        /// </summary>
        int ClockRate;

        /// <summary>
        /// Memória constante disponível no dispositivo em bytes.
        /// </summary>
        long TotalConstMem;

        /// <summary>
        /// Maior capacidade computacional.
        /// </summary>
        int Major;

        /// <summary>
        /// Menor capacidade computacional.
        /// </summary>
        int Minor;

        /// <summary>
        /// Alinhamento requerido para texturas.
        /// </summary>
        long TextureAlignment;

        /// <summary>
        /// Passo do alinhamento requerido para referências de texturas ligadas à memória de passo.
        /// </summary>
        long TexturePitchAlignment;

        /// <summary>
        /// O dispositivo pode copiar memória de forma concorrente e executar um kernel.
        /// </summary>
        [Obsolete("Use instead AsyncEngineCount")]
        int DeviceOverlap;              

        /// <summary>
        /// Número de multiprocessadores no dispositivo.
        /// </summary>
        int MultiProcessorCount;        

        /// <summary>
        /// Especifica se existe um limite de execução nos kernels.
        /// </summary>
        int KernelExecTimeoutEnabled;   

        /// <summary>
        /// O dispositivo é integrado em oposição a discreto.
        /// </summary>
        int Integrated;                 

        /// <summary>
        /// O dispositivo pode mapear memória de anfitrião com apontadores
        /// <see cref="CudaApi.CudaHostAlloc"/>/<see cref="CudaApi.CudaHostGetDevicePointer"/>.
        /// </summary>
        int CanMapHostMemory;           

        /// <summary>
        /// O nó de computação (ver <see cref="CudaComputeMode"/>.
        /// </summary>
        int ComputeMode;                

        /// <summary>
        /// Tamanho máximo de textura 1D.
        /// </summary>
        int MaxTexture1D;               

        /// <summary>
        /// Tamanho máximo de textura 1D mipmap.
        /// </summary>
        int MaxTexture1DMipmap;         

        /// <summary>
        /// Tamanho máximo para texturas 1D ligadas a memória linear.
        /// </summary>
        int MaxTexture1DLinear;         

        /// <summary>
        /// Dimensões máximas para texturas 2D.
        /// </summary>
        int[] MaxTexture2D ;            

        /// <summary>
        /// Dimensões máximas para texturas 2D mipmap.
        /// </summary>
        int[] MaxTexture2DMipmap;      

        /// <summary>
        /// Dimensões máximas (comprimento, largura, passo) para texturas 2D ligas à memória de passo.
        /// </summary>
        int[] MaxTexture2DLinear;      

        /// <summary>
        /// Dimensões máximas de textudas 2D se as operações de reunião de texturas têm de ser
        /// executadas.
        /// </summary>
        int[] MaxTexture2DGather;      

        /// <summary>
        /// Dimensões máximas de texturas 3D.
        /// </summary>
        int[] MaxTexture3D;            

        /// <summary>
        /// Dimensões máximas de texturas 3D alternadas.
        /// </summary>
        int[] MaxTexture3DAlt;       

        /// <summary>
        /// Dimenões máximas de texturas Cubemap.
        /// </summary>
        int MaxTextureCubemap;          

        /// <summary>
        /// Dimensões máximas de texturas 1D estratificáveis.
        /// </summary>
        int[] MaxTexture1DLayered ;    

        /// <summary>
        /// Dimensões máximas de texturas 2D estratificáveis.
        /// </summary>
        int[] MaxTexture2DLayered ;     

        /// <summary>
        /// Dimensões máximas de texturas Cubemax estatificáveis.
        /// </summary>
        int[] MaxTextureCubemapLayered;

        /// <summary>
        /// Tamanho máximo de surfaces 1D.
        /// </summary>
        int MaxSurface1D;              

        /// <summary>
        /// Dimensões máximas de surfaces 2D.
        /// </summary>
        int[] MaxSurface2D;            

        /// <summary>
        /// Dimensões máximas de surfaces 3D.
        /// </summary>
        int[] MaxSurface3D;           

        /// <summary>
        /// Dimensões máximas de suraces 1D estatificáveis.
        /// </summary>
        int[] MaxSurface1DLayered;     

        /// <summary>
        /// Dimensões máximas de surfaces 2D estatificáveis.
        /// </summary>
        int[] MaxSurface2DLayered;     

        /// <summary>
        /// Dimensões máximas de surfaces Cubemap.
        /// </summary>
        int MaxSurfaceCubemap;          

        /// <summary>
        /// Dimensões máixmas de surfaces Cubemap estatificáveis.
        /// </summary>
        int[] MaxSurfaceCubemapLayered;

        /// <summary>
        /// Requisitos de alinhamento para surfaces.
        /// </summary>
        long SurfaceAlignment;           

        /// <summary>
        /// O dispositivo pode possivelmente executar kernels de forma concorrente.
        /// </summary>
        int ConcurrentKernels;          

        /// <summary>
        /// O dispositivo tem suporte ECC activo.
        /// </summary>
        int EccEnabled;                

        /// <summary>
        /// Barramento de PCI do dispositivo.
        /// </summary>
        int PciBusID;                   

        /// <summary>
        /// Identificador do dispositivo de barramento para o dispositivo.
        /// </summary>
        int PciDeviceID;               

        /// <summary>
        /// O domínio do PCI do dispositivo.
        /// </summary>
        int PciDomainID;                

        /// <summary>
        /// Contém o valor 1 caso o dispositivo seja Tesla usando um condutor TCC e 0 caso contrário.
        /// </summary>
        int TccDriver;                  

        /// <summary>
        /// Número de motores assíncronos.
        /// </summary>
        int AsyncEngineCount;           

        /// <summary>
        /// Disopsitivo partilha um endereço de espaço unificado com o anfitrião.
        /// </summary>
        int UnifiedAddressing;          

        /// <summary>
        /// Frequência de relógio na obtenção de valor na memória.
        /// </summary>
        int MemoryClockRate;            

        /// <summary>
        /// Largura de banda do barramento da memória global em bits.
        /// </summary>
        int MemoryBusWidth;             

        /// <summary>
        /// Tamanho da provisão L2 em bytes.
        /// </summary>
        int L2CacheSize;                

        /// <summary>
        /// Número máximo de linhas de execução por multiprocessador.
        /// </summary>
        int MaxThreadsPerMultiProcessor;

        /// <summary>
        /// O dispositivo suporta prioridades de caudal.
        /// </summary>
        int StreamPrioritiesSupported;  

        /// <summary>
        /// Dispositivo suporta o aprovisionamento de globais em L1.
        /// </summary>
        int GlobalL1CacheSupported;     

        /// <summary>
        /// O dispositivo suporta aprovisionamento de locais em L1.
        /// </summary>
        int LocalL1CacheSupported;      

        /// <summary>
        /// Memória partilhada disponível por multiprocessador em bytes.
        /// </summary>
        long SharedMemPerMultiprocessor; 

        /// <summary>
        /// Registos 32-bit disponíveis por multiprocessador.
        /// </summary>
        int RegsPerMultiprocessor;      

        /// <summary>
        /// O dispositivo suporta a alocação de memória gerida neste sistema.
        /// </summary>
        int ManagedMemory;              

        /// <summary>
        /// O dispositivo encontra-se numa placa multi-GPU.
        /// </summary>
        int IsMultiGpuBoard;            

        /// <summary>
        /// Identificador único para um grupo de dispositivos na mesma placa multi-GPU.
        /// </summary>
        int MultiGpuBoardGroupID;
    }
}
