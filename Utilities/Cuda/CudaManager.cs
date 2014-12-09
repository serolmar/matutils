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
        /// Instância uma nova instância de objectos do tipo <see cref="CudaManager"/>.
        /// </summary>
        private CudaManager() { }

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
                        var cudaResult = CudaApi.CudaInit(0);
                        if (cudaResult != ECudaResult.CudaSuccess)
                        {
                            throw CudaException.GetExceptionFromCudaResult(cudaResult);
                        }

                        throw new NotImplementedException();
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
        /// Obtém o nome do dispositivo.
        /// </summary>
        public string DeviceName
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém as dimensões de grelha máximas (X, Y, Z).
        /// </summary>
        public Tuple<int, int, int> MaxGridDim
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de registos 32-bit disponíveis por bloco.
        /// </summary>
        public int MaxRegistersPerBlock
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a frequência do relógio.
        /// </summary>
        public int ClockRate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o alinhamento requerido para texturas.
        /// </summary>
        public int TextureAlignment
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo pode possivelmente copiar memória e executar um kernel de forma concorrente.
        /// </summary>
        public int GpuOverlap
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número de multiprocessadores no dispositov.
        /// </summary>
        public int MultiProcessorCount
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo está integrado com a memória de anfitrião.
        /// </summary>
        public int Integrated
        {
            get
            {
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o nó de computação (ver <see cref="ECudaComputeMode"/>).
        /// </summary>
        public int ComputeMode
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a máxima largura da textura 1D.
        /// </summary>
        public int MaxTexture1DWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) mãximas da textura 2D.
        /// </summary>
        public Tuple<int,int> MaxTexture2D
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o terno (largura, altura, profundidade) máximas da textura 3D.
        /// </summary>
        public Tuple<int,int,int> MaxTexture3D
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas da textura 2D estratificável.
        /// </summary>
        public Tuple<int,int> MaxTexture2DLayered
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa textura 2D estratificável.
        /// </summary>
        public int MaxTexture2DLayeredLayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número de requisitos de alinhamento para surfaces.
        /// </summary>
        public int SurfaceAlignment
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo pode possivelmente executar múltiplos kernels concorrentemente.
        /// </summary>
        public int ConcurrentKernels
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo tem supoyrte Ecc activo.
        /// </summary>
        public int EccEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o identificador do barramento do dispositivo.
        /// </summary>
        public int PciBusId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o identificador do dispositivo.
        /// </summary>
        public int PciDeviceId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo encontra-se a usar o modelo de condutor TCC.
        /// </summary>
        public int TccDriver
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a frequência de relógio de memória.
        /// </summary>
        public int MemoryClockRate
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura de banda do barramento de memória global em bits.
        /// </summary>
        public int GlobalMemoryBusWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o tamanho da provisão L2 em bytes.
        /// </summary>
        public int L2CacheSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de threads residentes por multiprocessador.
        /// </summary>
        public int MaxThreadsPerMultiProcessor
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo partilha um espaço de endereçamento unificado com o anfitrião.
        /// </summary>
        public int UnifiedAddressing
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura máxima da textura 1D estatificável.
        /// </summary>
        public int MaxTexture1DLayeredWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máixmo de camadas numa textura 1D estratificável.
        /// </summary>
        public int MaxTexture1DLayeredLayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o terno de máximos alternados (largura, altura, profundidade) da textura 3D.
        /// </summary>
        public Tuple<int, int, int> MaxTexture3DAlt
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o requisito de alinhamento de passo para texturas.
        /// </summary>
        public int TexturePitchAlignment
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a máxima largura/altura das texturas Cubemap.
        /// </summary>
        public int MaxTextureCubemapWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a máxima largura/altura das texturas Cubemap estratificáveis.
        /// </summary>
        public int MaxTextureCubemapLayeredWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas nas texturas Cubemap estratificáveis.
        /// </summary>
        public int MaxTextureCubemapLayeredLayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface 1D.
        /// </summary>
        public int MaxSurface1DWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas de uma surface 2D.
        /// </summary>
        public Tuple<int, int> MaxSurface2D
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o terno (largura, altura, profundidade) máximas de uma surface 3D.
        /// </summary>
        public Tuple<int, int, int> MaxSurface3D
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface 1D estratificável.
        /// </summary>
        public int MaxSurface1DLayeredWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa surface 1D estratificável.
        /// </summary>
        public int MaxSurface1DLayeredLayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas de uma surface 2D estratificável.
        /// </summary>
        public Tuple<int, int> MaxSurface2DLayered
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa surface 2D estratificável.
        /// </summary>
        public int MaxSurface2DLayeredLayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface Cubemap.
        /// </summary>
        public int MaxSurfaceCubemapWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura máxima de uma surface Cubemap estratificável.
        /// </summary>
        public int MaxSurfaceCubemapLayeredWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de camadas numa surface estratificável.
        /// </summary>
        public int MaxSurfaceCubemapLayeredLayers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura máxima de texturas 1D lineares.
        /// </summary>
        public int MaxTexture1DLinearWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máximas de texturas 2D lineares.
        /// </summary>
        public Tuple<int, int> MaxTexture2DLinear
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o passo máximo de texturas 2D lineares em bytes.
        /// </summary>
        public int MaxTexture2DLinearPitch
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o par (largura, altura) máxima de texturas 2D mipmap.
        /// </summary>
        public Tuple<int, int> MaxTexture2DMipmappedDim
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de versão (Major, Minor) para capacidade computacional.
        /// </summary>
        public Tuple<int, int> ComputeCapability
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a largura máxima de texturas 1D mipmap.
        /// </summary>
        public int MaxTexture1DMipmappedWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo suporta aprovisionamento de globais em L1.
        /// </summary>
        public int GlobalL1CacheSupported
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo suporta aprovisionamento de locais em L1.
        /// </summary>
        public int LocalL1CacheSupported
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém a memória máxima disponível por multiprocessador em bytes.
        /// </summary>
        public int MaxSharedMemoryPerMultiprocessor
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o número máximo de registos 32-bit disponíveis por multiprocessador.
        /// </summary>
        public int MaxRegistersPerMultiprocessor
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo pode alocar memória gerida neste sistema.
        /// </summary>
        public int ManagedMemory
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o dispositivo encontra-se numa placa multi-GPU.
        /// </summary>
        public int IsMultiGpuBoard
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obtém o identificador único para um grupo de dispositivos na mesma placa multi-GPU.
        /// </summary>
        public int MultiGpuBoardGroupID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion Atributos

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
}
