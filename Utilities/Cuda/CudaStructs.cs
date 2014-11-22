namespace Utilities.Cuda
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Referencia um vector CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaArray
    {
        /// <summary>
        /// O apontador para o vector.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Referencia um contexto CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaContext
    {
        /// <summary>
        /// Apontador para o contexto CUDA.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// O dispositivo CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaDevice
    {
        /// <summary>
        /// O valor do dispositivo.
        /// </summary>
        public int Value;
    }

    /// <summary>
    /// Referencia um apontador para o dispositivo.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaDevicePtr
    {
        /// <summary>
        /// O valor do apontador para o dispositivo.
        /// </summary>
        public uint Value;
    }

    /// <summary>
    /// Referencia um evento CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaEvent
    {
        /// <summary>
        /// Apontador para o evento CUDA.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Referencia uma função CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaFunction
    {
        /// <summary>
        /// Apontador para a função CUDA.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Referencia um recurso gráfico CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaGraphicsResource
    {
        /// <summary>
        /// O apontador para o recurso gráfico.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Referencia um vector mip mapped CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaMipmappedArray
    {
        /// <summary>
        /// Apontador para o vector mip mapped.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Referencia um módulo CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaModule
    {
        /// <summary>
        /// Apontador para o módulo CUDA.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Referencia um caudal CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaStream
    {
        /// <summary>
        /// Apontador para o caudal CUDA.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Representa um object surface CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaSufObj
    {
        /// <summary>
        /// Valor do apontador para o objecto surface CUDA.
        /// </summary>
        public Int64 Value;
    }

    /// <summary>
    /// Referencia uma surface CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaSurfRef
    {
        /// <summary>
        /// Apontador para o a surface CUDA.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Representa um objecto de textura CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaTexObj
    {
        /// <summary>
        /// Valor do objecto de textura CUDA.
        /// </summary>
        public Int64 Value;
    }

    /// <summary>
    /// Referencia um objecto de textura CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaTexRef
    {
        /// <summary>
        /// Apontador para o objecto de textura CUDA.
        /// </summary>
        public IntPtr Pointer;
    }

    /// <summary>
    /// Descritor de vector 3D CUDA.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaArray3DDescriptor
    {
        /// <summary>
        /// A profundidade.
        /// </summary>
        public SizeT Depth;

        /// <summary>
        /// As marcas.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// O formato do vector.
        /// </summary>
        public SCudaArray Format;

        /// <summary>
        /// A altura do vector.
        /// </summary>
        public SizeT Height;

        /// <summary>
        /// O número de canais.
        /// </summary>
        public uint NumChannels;

        /// <summary>
        /// A largura do vector.
        /// </summary>
        public SizeT Width;
    }

    /// <summary>
    /// Descritor de um vector.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaArrayDescriptor
    {
        /// <summary>
        /// O formato do vector.
        /// </summary>
        public ECudaArrayFormat Format;

        /// <summary>
        /// A altura do vector.
        /// </summary>
        public SizeT Height;

        /// <summary>
        /// O número de canais.
        /// </summary>
        public uint NumChannels;

        /// <summary>
        /// A largura do vector.
        /// </summary>
        public SizeT Width;
    }

    /// <summary>
    /// Parâmetros para cópia de memória 2D.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaMemCpy2D
    {
        /// <summary>
        /// A altura do vector.
        /// </summary>
        public SizeT Height;

        /// <summary>
        /// A largura do vector em bytes.
        /// </summary>
        public SizeT WidthInBytes;

        /// <summary>
        /// O vector de destino.
        /// </summary>
        public SCudaArray DstArray;

        /// <summary>
        /// O dispositivo de destino.
        /// </summary>
        public SCudaDevicePtr DstDevice;

        /// <summary>
        /// O anfitrião de destino.
        /// </summary>
        public IntPtr DstHost;

        /// <summary>
        /// O tipo de memória do destino.
        /// </summary>
        public ECudaMemoryType DstMemoryType;

        /// <summary>
        /// O passo do destino.
        /// </summary>
        public SizeT DstPitch;

        /// <summary>
        /// O valor de X no destino em bytes.
        /// </summary>
        public SizeT DstXInBytes;

        /// <summary>
        /// Valor de Y no destino.
        /// </summary>
        public SizeT DstY;

        /// <summary>
        /// O vector de origem.
        /// </summary>
        public SCudaArray SrcArray;

        /// <summary>
        /// O dispositivo de origem.
        /// </summary>
        public SCudaDevicePtr SrcDevice;

        /// <summary>
        /// O anfitrião de origem.
        /// </summary>
        public IntPtr SrcHost;

        /// <summary>
        /// O tipo de memória da origem.
        /// </summary>
        public ECudaMemoryType SrcMemoryType;

        /// <summary>
        /// O passo de origem.
        /// </summary>
        public SizeT SrcPitch;

        /// <summary>
        /// O valor de X da origem em bytes.
        /// </summary>
        public SizeT SrcXInBytes;

        /// <summary>
        /// O valor de Y na origem.
        /// </summary>
        public SizeT SrcY;
    }

    /// <summary>
    /// Parâmetros para cópia de memória 3D.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaMemCpy3D
    {
        /// <summary>
        /// A profunidade da cópia de memória.
        /// </summary>
        public SizeT Depth;

        /// <summary>
        /// A altura do vector.
        /// </summary>
        public SizeT Height;

        /// <summary>
        /// A largura do vector em bytes.
        /// </summary>
        public SizeT WidthInBytes;

        /// <summary>
        /// O vector de destino.
        /// </summary>
        public SCudaArray DstArray;

        /// <summary>
        /// O dispositivo de destino.
        /// </summary>
        public SCudaDevicePtr DstDevice;

        /// <summary>
        /// Altura do vector de destino.
        /// </summary>
        SizeT DstHeight;

        /// <summary>
        /// O anfitrião de destino.
        /// </summary>
        public IntPtr DstHost;

        /// <summary>
        /// O LOD de destino.
        /// </summary>
        public SizeT DstLod;

        /// <summary>
        /// O tipo de memória do destino.
        /// </summary>
        public ECudaMemoryType DstMemoryType;

        /// <summary>
        /// O passo do destino.
        /// </summary>
        public SizeT DstPitch;

        /// <summary>
        /// O valor de X no destino em bytes.
        /// </summary>
        public SizeT DstXInBytes;

        /// <summary>
        /// Valor de Y no destino.
        /// </summary>
        public SizeT DstY;

        /// <summary>
        /// Valor de Z no destino.
        /// </summary>
        public SizeT DstZ;

        /// <summary>
        /// Reservado.
        /// </summary>
        IntPtr Reserved0;

        /// <summary>
        /// Reservado.
        /// </summary>
        IntPtr Reserved1;

        /// <summary>
        /// O vector de origem.
        /// </summary>
        public SCudaArray SrcArray;

        /// <summary>
        /// O dispositivo de origem.
        /// </summary>
        public SCudaDevicePtr SrcDevice;

        /// <summary>
        /// A altura do vector de destino.
        /// </summary>
        public SizeT SourceHeight;

        /// <summary>
        /// O anfitrião de origem.
        /// </summary>
        public IntPtr SrcHost;

        /// <summary>
        /// O LOD de origem.
        /// </summary>
        public SizeT SrcLod;

        /// <summary>
        /// O tipo de memória da origem.
        /// </summary>
        public ECudaMemoryType SrcMemoryType;

        /// <summary>
        /// O passo de origem.
        /// </summary>
        public SizeT SrcPitch;

        /// <summary>
        /// O valor de X da origem em bytes.
        /// </summary>
        public SizeT SrcXInBytes;

        /// <summary>
        /// O valor de Y na origem.
        /// </summary>
        public SizeT SrcY;

        /// <summary>
        /// O valor de Z na origem.
        /// </summary>
        public SizeT SrcZ;
    }

    /// <summary>
    /// Parâmetros para cópia de memória 3D entre contextos.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaMemCpy3DPeer
    {
        /// <summary>
        /// A profunidade da cópia de memória.
        /// </summary>
        public SizeT Depth;

        /// <summary>
        /// A altura do vector.
        /// </summary>
        public SizeT Height;

        /// <summary>
        /// A largura do vector em bytes.
        /// </summary>
        public SizeT WidthInBytes;

        /// <summary>
        /// O vector de destino.
        /// </summary>
        public SCudaArray DstArray;

        /// <summary>
        /// O contexto de destino.
        /// </summary>
        SCudaContext DstContext;

        /// <summary>
        /// O dispositivo de destino.
        /// </summary>
        public SCudaDevicePtr DstDevice;

        /// <summary>
        /// Altura do vector de destino.
        /// </summary>
        SizeT DstHeight;

        /// <summary>
        /// O anfitrião de destino.
        /// </summary>
        public IntPtr DstHost;

        /// <summary>
        /// O LOD de destino.
        /// </summary>
        public SizeT DstLod;

        /// <summary>
        /// O tipo de memória do destino.
        /// </summary>
        public ECudaMemoryType DstMemoryType;

        /// <summary>
        /// O passo do destino.
        /// </summary>
        public SizeT DstPitch;

        /// <summary>
        /// O valor de X no destino em bytes.
        /// </summary>
        public SizeT DstXInBytes;

        /// <summary>
        /// Valor de Y no destino.
        /// </summary>
        public SizeT DstY;

        /// <summary>
        /// Valor de Z no destino.
        /// </summary>
        public SizeT DstZ;

        /// <summary>
        /// O vector de origem.
        /// </summary>
        public SCudaArray SrcArray;

        /// <summary>
        /// O contexto de origem.
        /// </summary>
        public SCudaContext SrcContext;

        /// <summary>
        /// O dispositivo de origem.
        /// </summary>
        public SCudaDevicePtr SrcDevice;

        /// <summary>
        /// A altura do vector de destino.
        /// </summary>
        public SizeT SourceHeight;

        /// <summary>
        /// O anfitrião de origem.
        /// </summary>
        public IntPtr SrcHost;

        /// <summary>
        /// O LOD de origem.
        /// </summary>
        public SizeT SrcLod;

        /// <summary>
        /// O tipo de memória da origem.
        /// </summary>
        public ECudaMemoryType SrcMemoryType;

        /// <summary>
        /// O passo de origem.
        /// </summary>
        public SizeT SrcPitch;

        /// <summary>
        /// O valor de X da origem em bytes.
        /// </summary>
        public SizeT SrcXInBytes;

        /// <summary>
        /// O valor de Y na origem.
        /// </summary>
        public SizeT SrcY;

        /// <summary>
        /// O valor de Z na origem.
        /// </summary>
        public SizeT SrcZ;
    }
}
