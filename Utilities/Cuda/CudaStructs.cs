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
    /// Referencia um apontador para o disopsitivo.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SCudaDevicePtr
    {
        /// <summary>
        /// O apontador para o vector.
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
}
