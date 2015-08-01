namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Descreve os vários tipos de bordas.
    /// </summary>
    public enum EBorderStyles
    {
        /// <summary>
        /// Traço ponto.
        /// </summary>
        DashDot,

        /// <summary>
        /// Traço ponto ponto.
        /// </summary>
        DashDotDot,

        /// <summary>
        /// Tracejado.
        /// </summary>
        Dashed,

        /// <summary>
        /// Pontilhado.
        /// </summary>
        Dotted,

        /// <summary>
        /// Traço duplo.
        /// </summary>
        Double,

        /// <summary>
        /// Traço muito fino.
        /// </summary>
        Hair,

        /// <summary>
        /// Traço médio.
        /// </summary>
        Medium,

        /// <summary>
        /// Traço ponto médio.
        /// </summary>
        MediumDashDot,

        /// <summary>
        /// Traço ponto ponto médio.
        /// </summary>
        MediumDashDotDot,

        /// <summary>
        /// Tracejado médio.
        /// </summary>
        MediumDashed,

        /// <summary>
        /// Nenhum estilo.
        /// </summary>
        None,

        /// <summary>
        /// Espesso.
        /// </summary>
        Thick,

        /// <summary>
        /// Fino.
        /// </summary>
        Thin
    }

    /// <summary>
    /// Descreve o tipo de preenchimento do gradiente.
    /// </summary>
    public enum EFillGradientType
    {
        /// <summary>
        /// Linear.
        /// </summary>
        Linear,

        /// <summary>
        /// Nenhum.
        /// </summary>
        None,

        /// <summary>
        /// Caminho.
        /// </summary>
        Path
    }

    /// <summary>
    /// Descreve o tipo de preenchimento de cor.
    /// </summary>
    public enum EFillStyle
    {
        /// <summary>
        /// Nenhum.
        /// </summary>
        None = 0,

        /// <summary>
        /// Sólido.
        /// </summary>
        Solid = 1,

        /// <summary>
        /// Cinzento escuro.
        /// </summary>
        DarkGray = 2,

        /// <summary>
        /// Cinzento médio.
        /// </summary>
        MediumGray = 3,

        /// <summary>
        /// Cinzento claro.
        /// </summary>
        LightGray = 4,

        /// <summary>
        /// Cinzento 125.
        /// </summary>
        Gray125 = 5,

        /// <summary>
        /// Cinzento 0625.
        /// </summary>
        Gray0625 = 6,

        /// <summary>
        /// Vertical escuro.
        /// </summary>
        DarkVertical = 7,

        /// <summary>
        /// Horizontal escuro.
        /// </summary>
        DarkHorizontal = 8,

        /// <summary>
        /// Fundo escuro.
        /// </summary>
        DarkDown = 9,

        /// <summary>
        /// Cimo escuro.
        /// </summary>
        DarkUp = 10,

        /// <summary>
        /// Grelha escura.
        /// </summary>
        DarkGrid = 11,

        /// <summary>
        /// Gelosia escura.
        /// </summary>
        DarkTrellis = 12,

        /// <summary>
        /// Vertical claro.
        /// </summary>
        LightVertical = 13,

        /// <summary>
        /// Horizontal claro.
        /// </summary>
        LightHorizontal = 14,

        /// <summary>
        /// Fundo claro.
        /// </summary>
        LightDown = 15,

        /// <summary>
        /// Cimo claro.
        /// </summary>
        LightUp = 16,

        /// <summary>
        /// Grelha clara.
        /// </summary>
        LightGrid = 17,

        /// <summary>
        /// Gelosia clara.
        /// </summary>
        LightTrellis = 18,
    }

    /// <summary>
    /// Descreve o tipo de alinhamento horizontal.
    /// </summary>
    public enum EHorizontalAlignement
    {
        /// <summary>
        /// Geral.
        /// </summary>
        General = 0,

        /// <summary>
        /// Esquerdo.
        /// </summary>
        Left = 1,

        /// <summary>
        /// Centrado.
        /// </summary>
        Center = 2,

        /// <summary>
        /// Centro contínuo.
        /// </summary>
        CenterContinuous = 3,

        /// <summary>
        /// Direito.
        /// </summary>
        Right = 4,

        /// <summary>
        /// Preenchido.
        /// </summary>
        Fill = 5,

        /// <summary>
        /// Distribuído.
        /// </summary>
        Distributed = 6,

        /// <summary>
        /// Justificado.
        /// </summary>
        Justify = 7
    }

    /// <summary>
    /// Define o alinhamento vertical.
    /// </summary>
    public enum EVerticalAlignement
    {
        /// <summary>
        /// Topo.
        /// </summary>
        Top = 0,

        /// <summary>
        /// Centrado.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Fundo.
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// Distribuído.
        /// </summary>
        Distributed = 3,

        /// <summary>
        /// Justificado.
        /// </summary>
        Justify = 4
    }

    /// <summary>
    /// Enumera os vários tipos de ordem de leitura.
    /// </summary>
    public enum EReadingOrder
    {
        /// <summary>
        /// A ordem de leitura é determinada pelo primeiro espaço.
        /// </summary>
        ContextDependent = 0,

        /// <summary>
        /// Da esquerda para a direita.
        /// </summary>
        LeftToRight = 1,

        /// <summary>
        /// Da direita para a esquerda.
        /// </summary>
        RightToLeft = 2
    }

    /// <summary>
    /// Enumera os tipos de sublinhado.
    /// </summary>
    public enum EUnderlineType
    {
        /// <summary>
        /// Nenhum.
        /// </summary>
        None = 0,

        /// <summary>
        /// Único.
        /// </summary>
        Single = 1,

        /// <summary>
        /// Duplo.
        /// </summary>
        Double = 2,

        /// <summary>
        /// Conta única.
        /// </summary>
        SingleAccounting = 3,

        /// <summary>
        /// Conta dupla.
        /// </summary>
        DoubleAccounting = 4
    }

    /// <summary>
    /// Alinhamento vertical da fonte.
    /// </summary>
    public enum EVerticalAlignementFont
    {
        /// <summary>
        /// Nenhum.
        /// </summary>
        None = 0,

        /// <summary>
        /// Subescrito.
        /// </summary>
        Subscript = 1,

        /// <summary>
        /// Sobrescrito.
        /// </summary>
        Superscript = 2
    }
}
