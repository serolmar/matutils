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
        /// Estabelece o contexto CUDA actual.
        /// </summary>
        /// <param name="context">O contexto CUDA.</param>
        public void SetCurrentContext(CudaContextProxy context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("currentCtx");
            }
            else
            {
                var cudaResult = CudaApi.CudaCtxSetCurrent(context.CudaContext);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }
            }
        }

        /// <summary>
        /// Insere o contexto actual na pilha de contextos.
        /// </summary>
        /// <param name="context">O contexto a ser inserido.</param>
        public void PushCurrentContext(CudaContextProxy context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("currentCtx");
            }
            else
            {
                var cudaResult = CudaApi.CudaCtxPushCurrent(context.CudaContext);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }
            }
        }

        /// <summary>
        /// Remove o contexto CUDA da pilha de contextos.
        /// </summary>
        /// <returns>O contexto que se encontra no topo da pilha.</returns>
        public CudaContextProxy PopCurrentContext()
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
        public CudaDeviceProxy GetCurrentContextDevice()
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
        /// Carrega o módulo no contexto actual definido no ficheiro associado ao caminho especificado.
        /// </summary>
        /// <param name="modulePath">O caminho do ficheiro.</param>
        /// <returns>O módulo carregado.</returns>
        public CudaModuleProxy CudaModuleLoad(string modulePath)
        {
            var cudaModule = default(SCudaModule);
            var cudaResult = CudaApi.CudaModuleLoad(ref cudaModule, modulePath);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaModuleProxy(cudaModule);
        }

        /// <summary>
        /// Carrega um módulo no contexto actual a partir de conteúdo definido por texto.
        /// </summary>
        /// <param name="moduleData">O texto que define o módulo.</param>
        /// <returns>O módulo carregado.</returns>
        public CudaModuleProxy CudaModuleLoadData(string moduleData)
        {
            var cudaModule = default(SCudaModule);
            var cudaResult = CudaApi.CudaModuleLoadData(ref cudaModule, moduleData);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaModuleProxy(cudaModule);
        }

        /// <summary>
        /// Carrega um módulo no contexto actual proporcionando opções do compilador JIT.
        /// </summary>
        /// <param name="moduleData">Os dados que definem o módulo.</param>
        /// <param name="options">O conjunto de opções de compilação.</param>
        /// <param name="optionValues">O conjunto de valores a serem atribuídos consoante as opções.</param>
        /// <returns>O módulo carregado.</returns>
        public CudaModuleProxy CudaModuleLoadDataEx(
            string moduleData,
            ECudaJitOption[] options,
            string[] optionValues)
        {
            if (options == null && optionValues == null)
            {
                var cudaModule = default(SCudaModule);
                var cudaResult = CudaApi.CudaModuleLoadDataEx(
                    ref cudaModule,
                    moduleData,
                    0,
                    options,
                    optionValues);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return new CudaModuleProxy(cudaModule);
            }
            else if (options == null)
            {
                throw new ArgumentException("The number of options and option values must match.");
            }
            else if (optionValues == null)
            {
                throw new ArgumentException("The number of options and option values must match.");
            }
            else if (options.Length != optionValues.Length)
            {
                throw new ArgumentException("The number of options and option values must match.");
            }
            else
            {
                var cudaModule = default(SCudaModule);
                var cudaResult = CudaApi.CudaModuleLoadDataEx(
                    ref cudaModule,
                    moduleData,
                    (uint)options.Length,
                    options,
                    optionValues);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return new CudaModuleProxy(cudaModule);
            }
        }

        /// <summary>
        /// Carrega um módulo CUDA a partir dum ficheiro fatbinary no contexto actual.
        /// </summary>
        /// <param name="fatBinary">Os dados do ficheiro fatbinary.</param>
        /// <returns>O módulo carregado.</returns>
        public CudaModuleProxy CudaModuleLoadFatBinary(string fatBinary)
        {
            var cudaModule = default(SCudaModule);
            var cudaResult = CudaApi.CudaModuleLoadFatBinary(
                ref cudaModule,
                fatBinary);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaModuleProxy(cudaModule);
        }

        /// <summary>
        /// Descarrega o módulo do contexto actual.
        /// </summary>
        /// <param name="module">O módulo a ser descarregado.</param>
        public void UnloadModule(CudaModuleProxy module)
        {
            var cudaResult = CudaApi.CudaModuleUnload(module.CudaModule);
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
        /// Cria um contexto associado ao dispositivo.
        /// </summary>
        /// <param name="flags">As marcas de criação do contexto.</param>
        /// <returns>O contexto.</returns>
        public CudaContextProxy CrateContext(ECudaContextFlags flags = ECudaContextFlags.SchedAuto)
        {
            var result = default(SCudaContext);
            var cudaResult = CudaApi.CudaCtxCreate(ref result, flags, this.cudaDevice);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaContextProxy(result);
        }

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
        /// Obtém o contexto CUDA inerente.
        /// </summary>
        public SCudaContext CudaContext
        {
            get
            {
                return this.cudaContext;
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
    }

    /// <summary>
    /// Implementa a representação de um módulo.
    /// </summary>
    public class CudaModuleProxy
    {
        /// <summary>
        /// O módulo CUDA.
        /// </summary>
        private SCudaModule cudaModule;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CudaModuleProxy"/>.
        /// </summary>
        /// <param name="cudaModule">O módulo CUDA.</param>
        internal CudaModuleProxy(SCudaModule cudaModule)
        {
            this.cudaModule = cudaModule;
        }

        /// <summary>
        /// Obtém o módulo CUDA.
        /// </summary>
        public SCudaModule CudaModule
        {
            get
            {
                return this.cudaModule;
            }
        }

        /// <summary>
        /// Obtém a função especificada pelo nome a partir do módulo.
        /// </summary>
        /// <param name="functionName">O nome da função.</param>
        /// <returns>A função.</returns>
        public CudaFunctionProxy GetCudaFunction(string functionName)
        {
            var cudaFunction = default(SCudaFunction);
            var cudaResult = CudaApi.CudaModuleGetFunction(
                ref cudaFunction,
                this.cudaModule,
                functionName);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return new CudaFunctionProxy(cudaFunction);
        }

        /// <summary>
        /// Obtém um apontador para uma variável global, retornando o seu tamanho em bytes.
        /// </summary>
        /// <param name="name">O nome da variável global.</param>
        /// <returns>O par (apontador, tamanho).</returns>
        public Tuple<SCudaDevicePtr, ulong> GetGlobal(string name)
        {
            var resultDevicePtr = default(SCudaDevicePtr);
            var resultSize = default(SizeT);
            var cudaResult = CudaApi.CudaModuleGetGlobal(
                ref resultDevicePtr,
                ref resultSize,
                this.cudaModule,
                name);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return Tuple.Create(resultDevicePtr, (ulong)resultSize);
        }
    }

    /// <summary>
    /// Implementa a representação de uma função.
    /// </summary>
    public class CudaFunctionProxy
    {
        /// <summary>
        /// O manuseador para a função CUDA.
        /// </summary>
        private SCudaFunction cudaFunction;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CudaFunctionProxy"/>.
        /// </summary>
        /// <param name="cudaFunction">O manuseador para a função CUDA subjacente.</param>
        internal CudaFunctionProxy(SCudaFunction cudaFunction)
        {
            this.cudaFunction = cudaFunction;
        }

        /// <summary>
        /// Obtém o manuseador para a função CUDA subjacente.
        /// </summary>
        public SCudaFunction CudaFunction
        {
            get
            {
                return this.cudaFunction;
            }
        }

        /// <summary>
        /// Obtém o número máximo de linhas de fluxo por bloco.
        /// </summary>
        public int MaxThreadsPerBlock
        {
            get
            {
                return this.GetAttribute(ECudaFuncAttribute.MaxThreadsPerBlock);
            }
        }

        /// <summary>
        /// Obtém o tamanho em bytes da memória partilhada estática.
        /// </summary>
        public int SharedSizeBytes
        {
            get
            {
                return this.GetAttribute(ECudaFuncAttribute.SharedSizeBytes);
            }
        }

        /// <summary>
        /// Tamanho em bytes de toda a memória constante.
        /// </summary>
        public int ConstSizeBytes
        {
            get
            {
                return this.GetAttribute(ECudaFuncAttribute.ConstSizeBytes);
            }
        }

        /// <summary>
        /// Obtém o tamanho em bytes da memória local utilizada por cada linha de fluxo da função.
        /// </summary>
        public int LocalSizeBytes
        {
            get
            {
                return this.GetAttribute(ECudaFuncAttribute.LocalSizeBytes);
            }
        }

        /// <summary>
        /// Obtém o número de registos.
        /// </summary>
        public int NumRegs
        {
            get
            {
                return this.GetAttribute(ECudaFuncAttribute.NumRegs);
            }
        }

        /// <summary>
        /// Obtém a versão PTX.
        /// </summary>
        public int PtxVersion
        {
            get
            {
                return this.GetAttribute(ECudaFuncAttribute.PtxVersion);
            }
        }

        /// <summary>
        /// Obtém a versão binária.
        /// </summary>
        public int BinaryVersion
        {
            get
            {
                return this.GetAttribute(ECudaFuncAttribute.BinaryVersion);
            }
        }

        /// <summary>
        /// Executa a função.
        /// </summary>
        /// <param name="gridDimX">A dimensão X da grelha.</param>
        /// <param name="gridDimY">A dimensão Y da grelha.</param>
        /// <param name="gridDimZ">A dimensão Z da grelha.</param>
        /// <param name="blockDimX">A dimensão X do bloco.</param>
        /// <param name="blockDimY">A dimensão Y do bloco.</param>
        /// <param name="blockDimZ">A dimensão Z do bloco.</param>
        /// <param name="sharedMemBytes">O tamanho da memória partilhada em bytes.</param>
        /// <param name="hstream">O caudal de execução.</param>
        /// <param name="kernelParams">Os parâmetros do kernel (poderá ser nulo).</param>
        public void LaunchKernel(
            uint gridDimX,
            uint gridDimY,
            uint gridDimZ,
            uint blockDimX,
            uint blockDimY,
            uint blockDimZ,
            uint sharedMemBytes,
            CudaStream hstream,
            object[] kernelParams)
        {
            if (kernelParams == null)
            {
                var cudaResult = CudaApi.CudaLaunchKernel(
                    this.cudaFunction,
                    gridDimX,
                    gridDimY,
                    gridDimZ,
                    blockDimX,
                    blockDimY,
                    blockDimZ,
                    sharedMemBytes,
                    hstream.Stream,
                    IntPtr.Zero,
                    IntPtr.Zero);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }
            }
            else
            {
                var marshalParams = Utils.AllocUnmanagedPointersArray(kernelParams);
                var cudaResult = CudaApi.CudaLaunchKernel(
                    this.cudaFunction,
                    gridDimX,
                    gridDimY,
                    gridDimZ,
                    blockDimX,
                    blockDimY,
                    blockDimZ,
                    sharedMemBytes,
                    hstream.Stream,
                    marshalParams.Item1,
                    IntPtr.Zero);
                Utils.FreeUnmanagedArray(marshalParams);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }
            }
        }

        /// <summary>
        /// Estabelece a configuração da provisão.
        /// </summary>
        /// <param name="config">A configuração a ser estabelecida.</param>
        public void SetCacheConfig(ECudaFuncCache config)
        {
            var cudaResult = CudaApi.CudaFuncSetCacheConfig(
                this.cudaFunction,
                config);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Estabelece a configuração da memória partilhada.
        /// </summary>
        /// <param name="config">A configuração a ser estabelecida.</param>
        public void SetSharedMemConfig(ECudaSharedConfig config)
        {
            var cudaResult = CudaApi.CudaFuncSetSharedMemConfig(
                this.cudaFunction,
                config);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Obtém o valor do atributo especificado no argumento.
        /// </summary>
        /// <param name="cudaFunctionAttribute">O atributo a ser obtido.</param>
        /// <returns>O valor do atributo.</returns>
        private int GetAttribute(ECudaFuncAttribute cudaFunctionAttribute)
        {
            var result = default(int);
            var cudaResult = CudaApi.CudaFuncGetAttribute(
                ref result,
                cudaFunctionAttribute,
                this.cudaFunction);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }

            return result;
        }
    }

    /// <summary>
    /// Representa um caudal de execução CUDA.
    /// </summary>
    public class CudaStream : IDisposable
    {
        /// <summary>
        /// O caudal de execução CUDA.
        /// </summary>
        private SCudaStream stream;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CudaStream"/>.
        /// </summary>
        /// <param name="flags">As marcas do caudal (ver <see cref="ECudaStreamFlags"/>).</param>
        public CudaStream(uint flags)
        {
            var cudaResult = CudaApi.CudaStreamCreate(
                ref this.stream,
                flags);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="CudaStream"/>.
        /// </summary>
        /// <param name="flags">As marcas do caudal de execução (ver <see cref="ECudaStreamFlags"/>).</param>
        /// <param name="priority">A prioridade do caudal de execução.</param>
        public CudaStream(uint flags, int priority)
        {
            var cudaResult = CudaApi.CudaStreamCreateWithPriority(
                ref this.stream,
                flags,
                priority);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Obtém o caudal de execução CUDA.
        /// </summary>
        public SCudaStream Stream
        {
            get
            {
                return this.stream;
            }
        }

        /// <summary>
        /// Obtém as marcas do caudal de execução.
        /// </summary>
        public uint Flags
        {
            get
            {
                var result = default(uint);
                var cudaResult = CudaApi.CudaStreamGetFlags(this.stream, ref result);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return result;
            }
        }

        /// <summary>
        /// Obtém a prioridade do caudal de execução.
        /// </summary>
        public int Priority
        {
            get
            {
                var result = default(int);
                var cudaResult = CudaApi.CudaStreamGetPriority(this.stream, ref result);
                if (cudaResult != ECudaResult.CudaSuccess)
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }

                return result;
            }
        }

        /// <summary>
        /// Verifica se todas as tarefas foram terminadas no caudal de execução.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso todas as tarefas tenham sido terminadas e falso caso contrário.
        /// </returns>
        public bool Ready
        {
            get
            {
                var cudaResult = CudaApi.CudaStreamQuery(this.stream);
                if (cudaResult == ECudaResult.CudaSuccess)
                {
                    return true;
                }
                else if (cudaResult == ECudaResult.CudaErrorNotReady)
                {
                    return false;
                }
                else
                {
                    throw CudaException.GetExceptionFromCudaResult(cudaResult);
                }
            }
        }

        /// <summary>
        /// Espera até que todas as tarefas associadas ao caudal de execução terminem.
        /// </summary>
        public void Synchronize()
        {
            var cudaResult = CudaApi.CudaStreamSynchronize(this.stream);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Garante que todo o trabalho futuro enviado para o caudal de execução seja iniciado após o término
        /// despoletado pelo evento.
        /// </summary>
        /// <param name="cudaEvent">O evento.</param>
        public void WaitEvent(SCudaEvent cudaEvent)
        {
            var cudaResult = CudaApi.CudaStreamWaitEvent(
                this.stream,
                cudaEvent,
                0);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Adiciona uma função que é executada quando todas as tarefas na pilha do caudal são executadas.
        /// </summary>
        /// <remarks>
        /// A funcionalidade é apenas suportada em dispositivos com capacidade 2.1 ou superior.
        /// </remarks>
        /// <param name="callbackFunc">A função a ser executada.</param>
        /// <param name="userData">Os dados de utilizador passados para a função.</param>
        public void AddCallback(CudaStreamCallback callbackFunc, object userData)
        {
            var innerUserData = (IntPtr)userData;
            var cudaResult = CudaApi.CudaStreamAddCallback(
                this.stream,
                callbackFunc,
                innerUserData,
                0);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Associa memória ao caudal de execução de forma assíncrona.
        /// </summary>
        /// <param name="devicePtr">O apontador para a região de memória.</param>
        /// <param name="size">O tamanho da região de memória.</param>
        /// <param name="flags">As marcas de anexação.</param>
        public void AttachMemAsync(SCudaDevicePtr devicePtr, long size, ECudaMemAttachFlags flags)
        {
            var cudaResult = CudaApi.CudaStreamAttachMemAsync(
                this.stream,
                devicePtr,
                size,
                flags);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }

        /// <summary>
        /// Descarta o caudal de execução.
        /// </summary>
        public void Dispose()
        {
            var cudaResult = CudaApi.CudaStreamDestroy(this.stream);
            if (cudaResult != ECudaResult.CudaSuccess)
            {
                throw CudaException.GetExceptionFromCudaResult(cudaResult);
            }
        }
    }
}
