namespace Utilities.Cuda
{
    using System;

    /// <summary>
    /// Alguns dos valores que estão definidos como directivas ao pré-processador na ferramenta de trabalho
    /// CUDA.
    /// </summary>
    public static class CudaConstants
    {
        /// <summary>
        /// Ver <see cref="CudaConstants.CudaArray3DLayred"/> 
        /// </summary>
        [Obsolete("Use CudaArray3DLayred instead.")]
        public const int CudaArray3DWithArray2D = 0x01;

        /// <summary>
        /// Se esta marca estiver atribuído, o vector CUDA passa a ser uma colecção de seis vectores 2D que
        /// representam as faces de um cubo. A largura de tal vector CUDA deverá ser igual à sua altura e a profundidade deverá ser
        /// seis. Se a marca <see cref="CudaConstants.CudaArray3DLayred"/> também estiver atribuída, o vector
        /// CUDA representa um colecção de cubemaps e a profundidade deverá ser um múltiplo de seis.
        /// </summary>
        public const int CudaArray3DCubeMap = 0x04;

        /// <summary>
        /// Se esta marca estiver atribuída, indica que o vector CUDA corresponde a uma textura de profundidade.
        /// </summary>
        public const int CudaArray3DDepthTexture = 0x10;

        /// <summary>
        /// Se esta marca estiver atribuída, o vector CUDA consiste numa colecção de camadas onde cada camada
        /// consiste num vector 1D ou 2D e o membro de profundidade de <see cref="SCudaArray3DDescriptor"/>
        /// especifica o número de camadas e não a profundidade de um vector 3D.
        /// </summary>
        public const int CudaArray3DLayered = 0x01;

        /// <summary>
        /// Esta marca deverá ser atribuída de modo a ligar a referência à surface ao vector CUDA.
        /// </summary>
        public const int CudaArray3DSurfaceLdst = 0x02;

        /// <summary>
        /// Esta marca deve ser atribuída de modo a efectuar operações de conjugação de texturas num
        /// vector CUDA.
        /// </summary>
        public const int CudaArray3DTextureGather = 0x08;

        /// <summary>
        /// O tamanho do manuseador IPC.
        /// </summary>
        public const int CudaIPCHandleSize = 64;

        /// <summary>
        /// Indicador de que o pródimo valor nos parâmetros extra da função 
        /// <see cref="CudaApi.CudaLaunchKernel"/> será um apontador para um amortecedor que contém todos os
        /// parâmetros de kernel usados para lançar o kernel f. Este amortecedor tem de satisfazer todos os
        /// requisitos de alinhamento dos parâmetros individuais. Se a marca 
        /// <see cref="CudaConstants.CudaLaunchParamBufferSize"/> também não estiver especificada no vector extra
        /// então <see cref="CudaConstants.CudaLaunchParameterBufferPointer"/> não terá qualquer efeito.
        /// </summary>
        public static IntPtr CudaLaunchParameterBufferPointer = new IntPtr(0x01);

        /// <summary>
        /// Indicador de que o próximo valor nos parâmetros extra da função
        /// <see cref="CudaApi.CudaLaunchKernel"/> será um apontador para <see cref="SizeT"/> que contém o
        /// tamanho do amortecedor especificado por <see cref="CudaConstants.CudaLaunchParameterBufferPointer"/>.
        /// É necessário que <see cref="CudaConstants.CudaLaunchParameterBufferPointer"/> também esteja
        /// especificado no vector extra se o valor associado com
        /// <see cref="CudaConstants.CudaLaunchParamBufferSize"/> não for zero.
        /// </summary>
        public static IntPtr CudaLaunchParamBufferSize = new IntPtr(0x02);

        /// <summary>
        /// Terminador de final de vector para os parâmetros extra de <see cref="CudaApi.CudaLaunchKernel"/>.
        /// </summary>
        public static IntPtr CudaLaunchParamEnd = new IntPtr(0x00);

        /// <summary>
        /// Se esta marca for atribuída, a memória de anfitrião é mapeada no espaço de endereçamento CUDA
        /// e a função <see cref="CudaApi.CudaMemHostGetDevicePointer"/> pode ser chamada no apontador de
        /// anfitrião. Marca para <see cref="CudaApi.CudaMemHostAlloc"/>.
        /// </summary>
        public const int CudaMemHostAllocDeviceMap = 0x02;

        /// <summary>
        /// Se esta marca for atribuída, a memória de anfitrião é portável entre diferentes contextos CUDA.
        /// Marca para <see cref="CudaApi.CudaMemHostAlloc"/>.
        /// </summary>
        public const int CudaMemHostAllocPortable = 0x01;

        /// <summary>
        /// Se esta marca for atribuída, a memória é alocada com forma de escrita combinada - rápida a escrever
        /// mas lenta a ler excepto via a instrução de carregamento de caudal SSE4 (MONVNTDQA). Marca para
        /// <see cref="CudaApi.CudaMemHostAlloc"/>.
        /// </summary>
        public const int CudaMemHostAllocWritecombined = 0x04;

        /// <summary>
        /// Sse esta marca for atribuída, a memória de anfitrião é mapeada no espaço de endereçamento CUDA
        /// e <see cref="CudaApi.CudaMemHostGetDevicePointer"/> pode ser chamada no apontador de anfitrião.
        /// Marca para <see cref="CudaApi.CudaMemHostRegister"/>.
        /// </summary>
        public const int CudaMemHostDeviceMap = 0x02;

        /// <summary>
        /// Se esta marca for atribuída, a memória é portável entre contextos CUDA. Marca para
        /// <see cref="CudaApi.CudaMemHostRegister"/>.
        /// </summary>
        public const int CudaMemHostRegisterPortable = 0x01;

        /// <summary>
        /// Para referências de textudas carregadas no módulo, utilizar a unidade de textura por defeito
        /// a partir da referência da textura.
        /// </summary>
        public const int CudaParamTrDefault = -1;

        /// <summary>
        /// Subescreve o formato texref com um formato inferido do vector. Marca para
        /// <see cref="CudaApi.CudaTexRefSetArray"/>.
        /// </summary>
        public const int CudaTrsaOverrideFormat = 0x01;

        /// <summary>
        /// Usar coordenadas de textura normalizadas no intervalo [0,1) ao invés do intervalo [0,dim).
        /// Marca para <see cref="CudaApi.CudaTexRefSetFlags"/>.
        /// </summary>
        public const int CudaTrsfNormalizedCoordinates = 0x02;

        /// <summary>
        /// Lê a textura como inteiros ao invés de promover os valores a ponto flutuante no intervalo [0,1].
        /// Marca para <see cref="CudaApi.CudaTExRefSetFlags"/>.
        /// </summary>
        public const int CudaTrsfReadAsInteger = 0x01;

        /// <summary>
        /// Efectua conversões SRGB->linear durante a leitura da textura.
        /// Marca para <see cref="CudaApi.CudaTextRefSetFlags"/>.
        /// </summary>
        public const int CudaTrsfSrgb = 0x10;
    }
}
