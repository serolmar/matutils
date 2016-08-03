// -----------------------------------------------------------------------
// <copyright file="FileTest.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;

    /// <summary>
    /// Define uma borda.
    /// </summary>
    public interface IBorderItem
    {
        /// <summary>
        /// The color of the border.
        /// </summary>
        SystemColour Colour { get; }

        /// <summary>
        /// The line style of the border.
        /// </summary>
        EBorderStyles Style { get; }
    }

    /// <summary>
    /// Define o estilo das bordas.
    /// </summary>
    public interface IBorder
    {
        /// <summary>
        /// O estilo da borda inferior.
        /// </summary>
        IBorderItem Bottom { get; }

        /// <summary>
        /// O estilo da borda diagonal.
        /// </summary>
        IBorderItem Diagonal { get; }

        /// <summary>
        /// O estilo da borda diagonal descendente.
        /// </summary>
        bool DiagonalDown { get; }

        /// <summary>
        /// O estilo da borda diagonal ascendente.
        /// </summary>
        bool DiagonalUp { get; }

        /// <summary>
        /// O estilo da borda esquerda.
        /// </summary>
        IBorderItem Left { get; }

        /// <summary>
        /// O estilo da borda direita.
        /// </summary>
        IBorderItem Right { get; }

        /// <summary>
        /// O estilo da borda superior.
        /// </summary>
        IBorderItem Top { get; }
    }

    /// <summary>
    /// Define o gradiente.
    /// </summary>
    public interface IGradientFill
    {
        /// <summary>
        /// Obtém a percentagem de fundo (se for 0 a cor 1 encontra-se no fundo e a cor 2 no topo)
        /// e se for 1 os papéis invertem-se.
        /// </summary>
        double Bottom { get; }

        /// <summary>
        /// Obtém a cor 1.
        /// </summary>
        SystemColour Color1 { get; }

        /// <summary>
        /// Obtém a cor 2.
        /// </summary>
        SystemColour Color2 { get; }

        /// <summary>
        /// Obtém o ângulo do gradiente linear.
        /// </summary>
        double Degree { get; }

        /// <summary>
        /// Obtém a percentagem de fundo.
        /// </summary>
        double Left { get; }

        /// <summary>
        /// Obtém a percentagem da direita.
        /// </summary>
        double Right { get; }

        /// <summary>
        /// Obtém a percentagem de topo.
        /// </summary>
        double Top { get; }

        /// <summary>
        /// Obtém o tipo de gradiente.
        /// </summary>
        EFillGradientType Type { get; }
    }

    /// <summary>
    /// Define o preencimento.
    /// </summary>
    public interface IFIll
    {
        /// <summary>
        /// A cor de fundo.
        /// </summary>
        SystemColour BackgroundColour { get; }

        /// <summary>
        /// O gradiente.
        /// </summary>
        IGradientFill Gradient { get; }

        /// <summary>
        /// A cor do padrão.
        /// </summary>
        SystemColour PatternColour { get; }

        /// <summary>
        /// O tipo de padrão.
        /// </summary>
        EFillStyle PatternType { get; }
    }

    /// <summary>
    /// Define um formato para os números.
    /// </summary>
    public interface IStyleNumberFormat
    {
        /// <summary>
        /// Obtém o descritor do formato.
        /// </summary>
        string Format { get; }
    }

    /// <summary>
    /// Define a fonte.
    /// </summary>
    public interface IFont
    {
        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra a negrito.
        /// </summary>
        bool Bold { get; }

        /// <summary>
        /// Obtém a cor da fonte.
        /// </summary>
        SystemColour Colour { get; }

        /// <summary>
        /// Obtém o número da família da fonte.
        /// </summary>
        int Family { get; }

        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra em itálico.
        /// </summary>
        bool Italic { get; }

        /// <summary>
        /// Obtém o nome da fonte.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Obtém o esquema.
        /// </summary>
        string Scheme { get; }

        /// <summary>
        /// Obtém o tamanho.
        /// </summary>
        float Size { get; }

        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra traçada.
        /// </summary>
        bool Strike { get; }

        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra sublinhada.
        /// </summary>
        bool UnderLine { get; }

        /// <summary>
        /// Obtém o tipo de sublinhado da fonte.
        /// </summary>
        EUnderlineType UnderLineType { get; }

        /// <summary>
        /// Obtém o alinhamento vertical da fonte.
        /// </summary>
        EVerticalAlignementFont VerticalAlign { get; }

        /// <summary>
        /// Estabelece os valores da fonte a partir da do sistema.
        /// </summary>
        /// <param name="font">A fonte do sistema.</param>
        void SetFromFont(Font font);
    }

    /// <summary>
    /// O estilo.
    /// </summary>
    public interface IStyle
    {
        /// <summary>
        /// Obtém as bordas.
        /// </summary>
        IBorder Border { get; }

        /// <summary>
        /// Obtém o preenchimento.
        /// </summary>
        IFIll Fill { get; }

        /// <summary>
        /// Obtém a fonte.
        /// </summary>
        IFont Font { get; }

        /// <summary>
        /// Obtém o alinhamento horizontal.
        /// </summary>
        EHorizontalAlignement HorizontalAlignment { get; }

        /// <summary>
        /// A margem entre as bordas e o texto.
        /// </summary>
        int Indent { get; }

        /// <summary>
        /// Obtém o formato numérico.
        /// </summary>
        IStyleNumberFormat Numberformat { get; }

        /// <summary>
        /// Obtém a ordem de leitura.
        /// </summary>
        EReadingOrder ReadingOrder { get; }

        /// <summary>
        /// Obtém um valor que indica se o texto terá de ser encolhido de modo a ajustar-se
        /// à célula.
        /// </summary>
        bool ShrinkToFit { get; }

        /// <summary>
        /// Obtém a orientação em graus, variando desde 0 a 180.
        /// </summary>
        int TextRotation { get; }

        /// <summary>
        /// Obém o alinhamento vertical.
        /// </summary>
        EVerticalAlignement VerticalAlignment { get; }

        /// <summary>
        /// Obtém um valor que indica se o texto será ajustado às margens.
        /// </summary>
        bool WrapText { get; }

        /// <summary>
        /// Obtém o nome do estilo.
        /// </summary>
        string Name { get; }
    }

    /// <summary>
    /// Define um segumento de texto formatado.
    /// </summary>
    public interface IRichText
    {
        /// <summary>
        /// Obtém a fonte associada ao texto.
        /// </summary>
        IFont TextFont { get; }

        /// <summary>
        /// Obtém o alinhamento vertical do texto.
        /// </summary>
        EVerticalAlignementFont VerticalAlign { get; }
    }

    /// <summary>
    /// Define uma colecção de segumentos de texto.
    /// </summary>
    public interface IRichTextCollection : IEnumerable<IRichText>
    {
        /// <summary>
        /// Obtém o número de segmentos de texto na colecção.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Obtém o texto representado pela colecção.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Obtém o segmento de texto especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice do segmento de texto.</param>
        /// <returns>O segmento de texto.</returns>
        IRichText this[int index] { get; }

        /// <summary>
        /// Adiciona um segmento de texto à colecção.
        /// </summary>
        /// <param name="text">O segmento a ser adicionado à colecção.</param>
        /// <param name="font">A fonte associada ao texto.</param>
        /// <param name="verticalAlign">O alinhamento vertical.</param>
        /// <returns>O segmento de texto.</returns>
        IRichText Add(
            string text, 
            IFont font,  
            EVerticalAlignementFont verticalAlign);

        /// <summary>
        /// Elimina todos os segmentos da colecção.
        /// </summary>
        void Clear();

        /// <summary>
        /// Remove um segmento de texto da colecção.
        /// </summary>
        /// <param name="item">O segmento de texto a ser removido.</param>
        void Remove(IRichText item);

        /// <summary>
        /// Remove o segmento de texto no índice especificado.
        /// </summary>
        /// <param name="index">O índice.</param>
        void RemoveAt(int index);
    }
}
