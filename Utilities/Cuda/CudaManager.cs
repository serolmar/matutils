namespace Utilities.Cuda
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Classe de instância única orientada para a gestão do ambiente CUDA da máquina anfitriã.
    /// </summary>
    public class CudaManager
    {
        /// <summary>
        /// O objeto responsável pela sincronização das linhas de fluxo do anfitião.
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// O gestor do ambiente CUDA.
        /// </summary>
        private static CudaManager manager;

        /// <summary>
        /// Mantém o conjunto dos dispositivos existentes no anfitrião.
        /// </summary>
        private CudaDeviceProxy[] devices;

        /// <summary>
        /// Instância uma nova instância de objectos do tipo <see cref="CudaManager"/>.
        /// </summary>
        /// <param name="devices">Os dispositivos encontrados.</param>
        private CudaManager(CudaDeviceProxy[] devices)
        {
            this.devices = devices;
        }

        /// <summary>
        /// Obtém o número de dispositivos carregados.
        /// </summary>
        public int DevicesCount
        {
            get
            {
                return this.devices.Length;
            }
        }

        /// <summary>
        /// Obtém a instância única do gestor do ambiente CUDA.
        /// </summary>
        /// <returns>O gestor do ambiente CUDA.</returns>
        public static CudaManager GetManager()
        {
            lock (lockObject)
            {
                if (manager == null)
                {
                    try
                    {
                        // Inicializa CUDA
                        var cudaResult = CudaApi.CudaInit(0);
                        if (cudaResult != ECudaResult.CudaSuccess)
                        {
                            throw CudaException.GetExceptionFromCudaResult(cudaResult);
                        }

                        // Obtém o número de dispositivos disponíveis
                        var deviceCount = default(int);
                        cudaResult = CudaApi.CudaDeviceGetCount(ref deviceCount);
                        if (cudaResult != ECudaResult.CudaSuccess)
                        {
                            throw CudaException.GetExceptionFromCudaResult(cudaResult);
                        }

                        var devices = new CudaDeviceProxy[deviceCount];
                        for (int i = 0; i < deviceCount; ++i)
                        {
                            var currentDevice = default(int);
                            cudaResult = CudaApi.CudaDeviceGet(ref currentDevice, i);
                            devices[i] = new CudaDeviceProxy(currentDevice);
                        }

                        manager = new CudaManager(devices);
                    }
                    catch (DllNotFoundException dllNotFoundException)
                    {
                        // O anfitrião não tem o condutor CUDA instalado.
                        throw new CudaException(
                            "The CUDA driver was not found in host. Please make sure that it was correctly installed.",
                            dllNotFoundException,
                            -2);
                    }
                    catch (CudaException cudaException)
                    {
                        // Uma excepção gerada internamente ocorreu.
                        throw cudaException;
                    }
                    catch (Exception exception)
                    {
                        // Uma excepção desconhecida ocorreu.
                        throw new CudaException(
                            "An unknown CUDA error has occurred. Please see the inner exception for further details.",
                            exception,
                            -1);
                    }
                }
            }

            return manager;
        }

        /// <summary>
        /// Obtém a versão do condutor CUDA instalado no anfitrião.
        /// </summary>
        /// <returns>A versão do condutor CUDA.</returns>
        public static int GetCudaDriverVersion()
        {
            try
            {
                var result = default(int);
                var cudaResult = CudaApi.CudaDriverGetVersion(ref result);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return result;
            }
            catch (CudaException cudaException)
            {
                throw cudaException;
            }
            catch (DllNotFoundException dllNotFoundException)
            {
                // O anfitrião não tem o condutor CUDA instalado.
                throw new CudaException(
                    "The CUDA driver was not found in host. Please make sure that it was correctly installed.",
                    dllNotFoundException,
                    -2);
            }
            catch (Exception exception)
            {
                // Uma excepção desconhecida ocorreu.
                throw new CudaException(
                    "An unknown CUDA error has occurred. Please see the inner exception for further details.",
                    exception,
                    -1);
            }
        }

        /// <summary>
        /// Obtém o dispositivo especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O dispositivo.</returns>
        public CudaDeviceProxy GetDevice(int index)
        {
            return this.devices[index];
        }

        /// <summary>
        /// Estabelece o contexto actual.
        /// </summary>
        /// <param name="context">O contexto a ser estabelecido.</param>
        public void SetCurrentContext(SCudaContext context)
        {
            var cudaResult = CudaApi.CudaCtxSetCurrent(context);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }
    }

    /// <summary>
    /// Implementa a representação de um dispositivo.
    /// </summary>
    public class CudaDeviceProxy
    {
        /// <summary>
        /// O número do dispositivo CUDA.
        /// </summary>
        private int cudaDevice;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CudaDeviceProxy"/>.
        /// </summary>
        /// <param name="cudaDevice"></param>
        internal CudaDeviceProxy(int cudaDevice)
        {
            this.cudaDevice = cudaDevice;
        }

        /// <summary>
        /// Obtém o número do dispostivo que pode ser usado na API CUDA.
        /// </summary>
        public int CudaDevice
        {
            get
            {
                return this.cudaDevice;
            }
        }

        /// <summary>
        /// Obtém o nome do dispositivo.
        /// </summary>
        public string Name
        {
            get
            {
                var result = new StringBuilder();
                var cudaResult = CudaApi.CudaDeviceGetName(result, 1024, this.cudaDevice);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Obtém a memória total do dispositivo.
        /// </summary>
        public long TotalMemory
        {
            get
            {
                var result = default(SizeT);
                var cudaResult = CudaApi.CudaDeviceTotalMem(ref result, this.cudaDevice);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return result;
            }
        }

        /// <summary>
        /// Cria um contexto associado ao dispositivo na linha de fluxo de chamada.
        /// </summary>
        /// <param name="flags">As marcas de criação do contexto.</param>
        /// <returns>O contexto.</returns>
        public CudaContextProxy CreateContext(ECudaContextFlags flags)
        {
            var result = default(SCudaContext);
            var cudaResult = CudaApi.CudaCtxCreate(ref result, flags, this.cudaDevice);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaContextProxy(result);
        }

        #region Atributos

        /// <summary>
        /// Obtém o número máximo de threads por bloco.
        /// </summary>
        public int MaxThreadsPerBlock
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxThreadsPerBlock);
            }
        }

        /// <summary>
        /// Obtém as dimensões de blocos máximas (X, Y, Z).
        /// </summary>
        public Tuple<int, int, int> MaxBlockDim
        {
            get
            {
                var x = this.GetAttributeValue(ECudaDeviceAttr.MaxBlockDimX);
                var y = this.GetAttributeValue(ECudaDeviceAttr.MaxBlockDimY);
                var z = this.GetAttributeValue(ECudaDeviceAttr.MaxBlockDimZ);
                return Tuple.Create(x, y, z);
            }
        }

        /// <summary>
        /// Obtém as dimensões de grelha máximas (X, Y, Z).
        /// </summary>
        public Tuple<int, int, int> MaxGridDim
        {
            get
            {
                var x = this.GetAttributeValue(ECudaDeviceAttr.MaxGridDimX);
                var y = this.GetAttributeValue(ECudaDeviceAttr.MaxBlockDimY);
                var z = this.GetAttributeValue(ECudaDeviceAttr.MaxBlockDimZ);
                return Tuple.Create(x, y, z);
            }
        }

        /// <summary>
        /// Obtém a memória partilhada disponível por bloco em bytes.
        /// </summary>
        public int MaxSharedMemoryPerBlock
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSharedMemoryPerBlock);
            }
        }

        /// <summary>
        /// Obtém a memória disponível no dispositivo para variáveis marcadas como constantes num kernel
        /// C de CUDA em bytes.
        /// </summary>
        public int TotalConstantMemory
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.TotalConstantMemory);
            }
        }

        /// <summary>
        /// Obtém o tamanho do warp em threads.
        /// </summary>
        public int WarpSize
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.WarpSize);
            }
        }

        /// <summary>
        /// Obtém o maior passo em bytes permitido em cópias de memória.
        /// </summary>
        public int MaxPitch
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxPitch);
            }
        }

        /// <summary>
        /// Obtém o número máximo de registos 32-bit disponíveis por bloco.
        /// </summary>
        public int MaxRegistersPerBlock
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxRegistersPerBlock);
            }
        }

        /// <summary>
        /// Obtém a frequência do relógio.
        /// </summary>
        public int ClockRate
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.ClockRate);
            }
        }

        /// <summary>
        /// Obtém o alinhamento requerido para texturas.
        /// </summary>
        public int TextureAlignment
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.TextureAlignment);
            }
        }

        /// <summary>
        /// Obtém o dispositivo pode possivelmente copiar memória e executar um kernel de forma concorrente.
        /// </summary>
        public int GpuOverlap
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.GpuOverlap);
            }
        }

        /// <summary>
        /// Obtém o número de multiprocessadores no dispositov.
        /// </summary>
        public int MultiProcessorCount
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MultiProcessorCount);
            }
        }

        /// <summary>
        /// Obtém um valor que specifica se existe um limite de execução dos kernels.
        /// </summary>
        /// <remarks>
        /// Verdadeiro se o limite existir e falso caso contrário.
        /// </remarks>
        public bool KernelExecTimeout
        {
            get
            {
                var resultValue = this.GetAttributeValue(ECudaDeviceAttr.KernelExecTimeout);
                return resultValue == 1;
            }
        }

        /// <summary>
        /// Obtém o dispositivo está integrado com a memória de anfitrião.
        /// </summary>
        public int Integrated
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.Integrated);
            }
        }

        /// <summary>
        /// Obtém o dispositivo pode mapear memória de anfitrião no espaço de endereçamento CUDA.
        /// </summary>
        /// <returns>
        /// Verdadeiro se o mapeamento for possível e falso caso contrário.
        /// </returns>
        public bool CanMapHostMemory
        {
            get
            {
                var resultValue = this.GetAttributeValue(ECudaDeviceAttr.CanMapHostMemory);
                return resultValue == 1;
            }
        }

        /// <summary>
        /// Obtém o nó de computação (ver <see cref="ECudaComputeMode"/>).
        /// </summary>
        public int ComputeMode
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.ComputeMode);
            }
        }

        /// <summary>
        /// Obtém a máxima largura da textura 1D.
        /// </summary>
        public int MaxTexture1DWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTexture1DWidth);
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas da textura 2D.
        /// </summary>
        public Tuple<int, int> MaxTexture2D
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DHeight);
                return Tuple.Create(width, height);
            }
        }

        /// <summary>
        /// Obtém o terno (largura, altura, profundidade) máximas da textura 3D.
        /// </summary>
        public Tuple<int, int, int> MaxTexture3D
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture3DWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture3DHeight);
                var depth = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture3DDepth);
                return Tuple.Create(width, height, depth);
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas da textura 2D estratificável.
        /// </summary>
        public Tuple<int, int> MaxTexture2DLayered
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DLayeredWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DLayeredHeight);
                return Tuple.Create(width, height);
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa textura 2D estratificável.
        /// </summary>
        public int MaxTexture2DLayeredLayers
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DLayeredLayers);
            }
        }

        /// <summary>
        /// Obtém o número de requisitos de alinhamento para surfaces.
        /// </summary>
        public int SurfaceAlignment
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.SurfaceAlignment);
            }
        }

        /// <summary>
        /// Obtém o dispositivo pode possivelmente executar múltiplos kernels concorrentemente.
        /// </summary>
        public int ConcurrentKernels
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.ConcurrentKernels);
            }
        }

        /// <summary>
        /// Obtém o dispositivo tem supoyrte Ecc activo.
        /// </summary>
        public int EccEnabled
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.EccEnabled);
            }
        }

        /// <summary>
        /// Obtém o identificador do barramento do dispositivo.
        /// </summary>
        public int PciBusId
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.PciBusId);
            }
        }

        /// <summary>
        /// Obtém o identificador do dispositivo.
        /// </summary>
        public int PciDeviceId
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.PciDeviceId);
            }
        }

        /// <summary>
        /// Obtém o dispositivo encontra-se a usar o modelo de condutor TCC.
        /// </summary>
        public int TccDriver
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.TccDriver);
            }
        }

        /// <summary>
        /// Obtém a frequência de relógio de memória.
        /// </summary>
        public int MemoryClockRate
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MemoryClockRate);
            }
        }

        /// <summary>
        /// Obtém a largura de banda do barramento de memória global em bits.
        /// </summary>
        public int GlobalMemoryBusWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.GlobalMemoryBusWidth);
            }
        }

        /// <summary>
        /// Obtém o tamanho da provisão L2 em bytes.
        /// </summary>
        public int L2CacheSize
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.L2CacheSize);
            }
        }

        /// <summary>
        /// Obtém o número máximo de threads residentes por multiprocessador.
        /// </summary>
        public int MaxThreadsPerMultiProcessor
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxThreadsPerMultiProcessor);
            }
        }

        /// <summary>
        /// Obtém o dispositivo partilha um espaço de endereçamento unificado com o anfitrião.
        /// </summary>
        public int UnifiedAddressing
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.UnifiedAddressing);
            }
        }

        /// <summary>
        /// Obtém a largura máxima da textura 1D estatificável.
        /// </summary>
        public int MaxTexture1DLayeredWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTexture1DLayeredWidth);
            }
        }

        /// <summary>
        /// Obtém o número máixmo de camadas numa textura 1D estratificável.
        /// </summary>
        public int MaxTexture1DLayeredLayers
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTexture1DLayeredLayers);
            }
        }

        /// <summary>
        /// Obtém o terno de máximos alternados (largura, altura, profundidade) da textura 3D.
        /// </summary>
        public Tuple<int, int, int> MaxTexture3DAlt
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture3DWidthAlt);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture3DHeightAlt);
                var depth = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture3DDepthAlt);
                return Tuple.Create(width, height, depth);
            }
        }

        /// <summary>
        /// Obtém o requisito de alinhamento de passo para texturas.
        /// </summary>
        public int TexturePitchAlignment
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.TexturePitchAlignment);
            }
        }

        /// <summary>
        /// Obtém a máxima largura/altura das texturas Cubemap.
        /// </summary>
        public int MaxTextureCubemapWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTextureCubemapWidth);
            }
        }

        /// <summary>
        /// Obtém a máxima largura/altura das texturas Cubemap estratificáveis.
        /// </summary>
        public int MaxTextureCubemapLayeredWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTextureCubemapLayeredWidth);
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas nas texturas Cubemap estratificáveis.
        /// </summary>
        public int MaxTextureCubemapLayeredLayers
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTextureCubemapLayeredLayers);
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface 1D.
        /// </summary>
        public int MaxSurface1DWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSurface1DWidth);
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas de uma surface 2D.
        /// </summary>
        public Tuple<int, int> MaxSurface2D
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxSurface2DWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxSurface2DHeight);
                return Tuple.Create(width, height);
            }
        }

        /// <summary>
        /// Obtém o terno (largura, altura, profundidade) máximas de uma surface 3D.
        /// </summary>
        public Tuple<int, int, int> MaxSurface3D
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxSurface3DWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxSurface3DHeight);
                var depth = this.GetAttributeValue(ECudaDeviceAttr.MaxSurface3DDepth);
                return Tuple.Create(width, height, depth);
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface 1D estratificável.
        /// </summary>
        public int MaxSurface1DLayeredWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSurface1DLayeredWidth);
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa surface 1D estratificável.
        /// </summary>
        public int MaxSurface1DLayeredLayers
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSurface1DLayeredLayers);
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas de uma surface 2D estratificável.
        /// </summary>
        public Tuple<int, int> MaxSurface2DLayered
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxSurface2DLayeredWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxSurface2DLayeredHeight);
                return Tuple.Create(width, height);
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa surface 2D estratificável.
        /// </summary>
        public int MaxSurface2DLayeredLayers
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSurface2DLayeredLayers);
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface Cubemap.
        /// </summary>
        public int MaxSurfaceCubemapWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSurfaceCubemapWidth);
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface Cubemap estratificável.
        /// </summary>
        public int MaxSurfaceCubemapLayeredWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSurfaceCubemapLayeredWidth);
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa surface estratificável.
        /// </summary>
        public int MaxSurfaceCubemapLayeredLayers
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSurfaceCubemapLayeredLayers);
            }
        }

        /// <summary>
        /// Obtém a largura máxima de texturas 1D lineares.
        /// </summary>
        public int MaxTexture1DLinearWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTexture1DLinearWidth);
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas de texturas 2D lineares.
        /// </summary>
        public Tuple<int, int> MaxTexture2DLinear
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DLinearWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DLinearHeight);
                return Tuple.Create(width, height);
            }
        }

        /// <summary>
        /// Obtém o passo máximo de texturas 2D lineares em bytes.
        /// </summary>
        public int MaxTexture2DLinearPitch
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DLinearPitch);
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máxima de texturas 2D mipmap.
        /// </summary>
        public Tuple<int, int> MaxTexture2DMipmappedDim
        {
            get
            {
                var width = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DMipmappedWidth);
                var height = this.GetAttributeValue(ECudaDeviceAttr.MaxTexture2DMipmappedHeight);
                return Tuple.Create(width, height);
            }
        }

        /// <summary>
        /// Obtém o número máximo de versão (Major, Minor) para capacidade computacional.
        /// </summary>
        public Tuple<int, int> ComputeCapability
        {
            get
            {
                var major = this.GetAttributeValue(ECudaDeviceAttr.ComputeCapabilityMajor);
                var minor = this.GetAttributeValue(ECudaDeviceAttr.ComputeCapabilityMinor);
                return Tuple.Create(major, minor);
            }
        }

        /// <summary>
        /// Obtém a largura máxima de texturas 1D mipmap.
        /// </summary>
        public int MaxTexture1DMipmappedWidth
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxTexture1DMipmappedWidth);
            }
        }

        /// <summary>
        /// Obtém o dispositivo suporta aprovisionamento de globais em L1.
        /// </summary>
        public int GlobalL1CacheSupported
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.GlobalL1CacheSupported);
            }
        }

        /// <summary>
        /// Obtém o dispositivo suporta aprovisionamento de locais em L1.
        /// </summary>
        public int LocalL1CacheSupported
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.LocalL1CacheSupported);
            }
        }

        /// <summary>
        /// Obtém a memória máxima disponível por multiprocessador em bytes.
        /// </summary>
        public int MaxSharedMemoryPerMultiprocessor
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxSharedMemoryPerMultiprocessor);
            }
        }

        /// <summary>
        /// Obtém o número máximo de registos 32-bit disponíveis por multiprocessador.
        /// </summary>
        public int MaxRegistersPerMultiprocessor
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MaxRegistersPerMultiprocessor);
            }
        }

        /// <summary>
        /// Obtém o dispositivo pode alocar memória gerida neste sistema.
        /// </summary>
        public int ManagedMemory
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.ManagedMemory);
            }
        }

        /// <summary>
        /// Obtém o dispositivo encontra-se numa placa multi-GPU.
        /// </summary>
        public int IsMultiGpuBoard
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.IsMultiGpuBoard);
            }
        }

        /// <summary>
        /// Obtém o identificador único para um grupo de dispositivos na mesma placa multi-GPU.
        /// </summary>
        public int MultiGpuBoardGroupID
        {
            get
            {
                return this.GetAttributeValue(ECudaDeviceAttr.MultiGpuBoardGroupID);
            }
        }

        #endregion Atributos

        /// <summary>
        /// Verifica a igualdade entre o objecto actual e o objecto especificado.
        /// </summary>
        /// <param name="obj">O objecto a ser comparado com o actual.</param>
        /// <returns>Verdadeiro caso os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as CudaDeviceProxy;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                return this.cudaDevice == innerObj.cudaDevice;
            }
        }

        /// <summary>
        /// Obtém o código confuso do objecto corrente.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            return this.cudaDevice.GetHashCode();
        }

        #region Funções privadas

        /// <summary>
        /// Obtém, caso possível, o valor do atributo CUDA.
        /// </summary>
        /// <param name="attribute">O atributo do qual se pretende obter o valor.</param>
        /// <returns>O valor do atributo.</returns>
        public int GetAttributeValue(ECudaDeviceAttr attribute)
        {
            var result = default(int);
            var cudaResult = CudaApi.CudaDeviceGetAttribute(ref result, attribute, this.cudaDevice);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return result;
        }

        #endregion Funções privadas
    }

    /// <summary>
    /// Implmenta a representação de um contexto.
    /// </summary>
    public class CudaContextProxy : IDisposable
    {
        /// <summary>
        /// O contexto CUDA.
        /// </summary>
        private SCudaContext cudaContext;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CudaContextProxy"/>.
        /// </summary>
        /// <param name="cudaContext"></param>
        internal CudaContextProxy(SCudaContext cudaContext)
        {
            this.cudaContext = cudaContext;
        }

        /// <summary>
        /// Obtém o contexto CUDA associado ao representante.
        /// </summary>
        public SCudaContext CudaContext
        {
            get
            {
                return this.cudaContext;
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração de provisão do contexto corrente.
        /// </summary>
        /// <returns>A configuração de provisão.</returns>
        public static ECudaFuncCache CurrentContextCacheConfig
        {
            get
            {
                var result = default(ECudaFuncCache);
                var cudaResult = CudaApi.CudaCtxGetCacheConfig(ref result);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return result;
            }
            set
            {
                var cudaResult = CudaApi.CudaCtxSetCacheConfig(value);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui a configuração da memória partilhada associada ao contexto actual.
        /// </summary>
        /// <returns>A configuração de memória partilhada.</returns>
        public static ECudaSharedConfig CurrentContexSharedMemConfig
        {
            get
            {
                var result = default(ECudaSharedConfig);
                var cudaResult = CudaApi.CudaCtxGetSharedMemConfig(ref result);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return result;
            }
            set
            {
                var cudaResult = CudaApi.CudaCtxSetSharedMemConfig(value);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }
            }
        }

        /// <summary>
        /// Dispõe o contexto e liberta todos os recursos associados com este.
        /// </summary>
        public void Dispose()
        {
            // Destrói o contexto corrente
            CudaApi.CudaCtxDestroy(this.cudaContext);
        }

        #region Membros

        /// <summary>
        /// Obtém a versão de API suportada pelo dispositivo associado ao contexto.
        /// </summary>
        /// <returns>A versão da API suportada.</returns>
        public uint GetContextApiVersion()
        {
            var result = default(uint);
            var cudaResult = CudaApi.CudaCtxGetApiVersion(this.cudaContext, ref result);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return result;
        }

        /// <summary>
        /// Estabelece o contexto CUDA actual.
        /// </summary>
        public void SetAsCurrent()
        {
            var cudaResult = CudaApi.CudaCtxSetCurrent(this.cudaContext);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Insere o contexto actual na pilha de contextos.
        /// </summary>
        public void PushAsCurrent()
        {
            var cudaResult = CudaApi.CudaCtxPushCurrent(this.cudaContext);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        #endregion Membros

        /// <summary>
        /// Obtém o contexto corrente na linha de fluxo.
        /// </summary>
        /// <returns>O contexto corrente.</returns>
        public static CudaContextProxy CudaContextGetCurrent()
        {
            var result = default(SCudaContext);
            var cudaResult = CudaApi.CudaCtxGetCurrent(ref result);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaContextProxy(result);
        }

        /// <summary>
        /// Remove o contexto CUDA da pilha de contextos.
        /// </summary>
        /// <returns>O contexto que se encontra no topo da pilha.</returns>
        public static CudaContextProxy PopCurrent()
        {
            var result = default(SCudaContext);
            var cudaResult = CudaApi.CudaCtxPopCurrent(ref result);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaContextProxy(result);
        }

        /// <summary>
        /// Obtém um representante do dispositivo associado ao contexto actual.
        /// </summary>
        /// <returns>O dispositivo.</returns>
        public static CudaDeviceProxy GetCurrentContextDevice()
        {
            var result = default(int);
            var cudaResult = CudaApi.CudaCtxGetDevice(ref result);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaDeviceProxy(result);

        }

        /// <summary>
        /// Obtém o limite associado ao contexto actual.
        /// </summary>
        /// <param name="limit">O tipo do limite.</param>
        /// <returns>O valor do limite.</returns>
        public static int GetCurrentContextLimit(ECudaLimit limit)
        {
            var result = default(SizeT);
            var cudaResult = CudaApi.CudaCtxGetLimit(ref result, limit);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return result;
        }

        /// <summary>
        /// Estabelece o valor do limite.
        /// </summary>
        /// <param name="limit">O tipo do lmite a ser estabelecido.</param>
        /// <param name="value">O valor do limite.</param>
        public static void SetCurrentContextLimit(ECudaLimit limit, int value)
        {
            var cudaResult = CudaApi.CudaCtxSetLimit(limit, value);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Obtém o par (prioridade de caudal menor, prioridade de caudal maior) associada ao caudal corrente.
        /// </summary>
        /// <returns>O par de valores que definem o intervalo de prioridades de caudal.</returns>
        public static Tuple<int, int> GetCurrentContextStreamPriorityRange()
        {
            var leastPriority = default(int);
            var greatestPriority = default(int);
            var cudaResult = CudaApi.CudaCtxGetStreamPriorityRange(ref leastPriority, ref greatestPriority);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return Tuple.Create(leastPriority, greatestPriority);
        }
    }
}
