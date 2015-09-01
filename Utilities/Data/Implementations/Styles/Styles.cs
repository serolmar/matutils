namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;

    /// <summary>
    /// Instancia uma nova instância de objectos do tipo <see cref="SystemColour"/>.
    /// </summary>
    public class SystemColour
    {
        /// <summary>
        /// A cor actual.
        /// </summary>
        protected Color colour;

        #region Cores especiais

        /// <summary>
        /// Nenhuma cor.
        /// </summary>
        private static SystemColour empty;

        /// <summary>
        /// Azul claro.
        /// </summary>
        private static SystemColour aliceBlue;

        /// <summary>
        /// Branco antigo.
        /// </summary>
        private static SystemColour antiqueWhite;

        /// <summary>
        /// Azul água.
        /// </summary>
        private static SystemColour aqua;

        /// <summary>
        /// Azul marinho.
        /// </summary>
        private static SystemColour aquamarine;

        /// <summary>
        /// Azulado.
        /// </summary>
        private static SystemColour azure;

        /// <summary>
        /// Bege.
        /// </summary>
        private static SystemColour beige;

        /// <summary>
        /// Vermelho amarelado.
        /// </summary>
        private static SystemColour bisque;

        /// <summary>
        /// Preto.
        /// </summary>
        private static SystemColour black;

        /// <summary>
        /// Amêndoa pálida.
        /// </summary>
        private static SystemColour blanchedAlmond;

        /// <summary>
        /// Azul.
        /// </summary>
        private static SystemColour blue;

        /// <summary>
        /// Azul violeta.
        /// </summary>
        private static SystemColour blueViolet;

        /// <summary>
        /// Castanho.
        /// </summary>
        private static SystemColour brown;

        /// <summary>
        /// Madeira forte.
        /// </summary>
        private static SystemColour burlyWood;

        /// <summary>
        /// Azul cadete.
        /// </summary>
        private static SystemColour cadetBlue;

        /// <summary>
        /// Cor-de-rosa.
        /// </summary>
        private static SystemColour chartreuse;

        /// <summary>
        /// Cor-de-chocolate.
        /// </summary>
        private static SystemColour chocolate;

        /// <summary>
        /// Coral.
        /// </summary>
        private static SystemColour coral;

        /// <summary>
        /// Azul flor-de-milho.
        /// </summary>
        private static SystemColour cornflowerBlue;

        /// <summary>
        /// Cor seda-de-milho.
        /// </summary>
        private static SystemColour cornsilk;

        /// <summary>
        /// Carmesim.
        /// </summary>
        private static SystemColour crimson;

        /// <summary>
        /// Ciano.
        /// </summary>
        private static SystemColour cyan;

        /// <summary>
        /// Azul escuro.
        /// </summary>
        private static SystemColour darkBlue;

        /// <summary>
        /// Ciano escuro.
        /// </summary>
        private static SystemColour darkCyan;

        /// <summary>
        /// Dourado escuro.
        /// </summary>
        private static SystemColour darkGoldenrod;

        /// <summary>
        /// Cinzento escuro.
        /// </summary>
        private static SystemColour darkGray;

        /// <summary>
        /// Verde escuro.
        /// </summary>
        private static SystemColour darkGreen;

        /// <summary>
        /// Caqui escuro.
        /// </summary>
        private static SystemColour darkKhaki;

        /// <summary>
        /// Magenta escuro.
        /// </summary>
        private static SystemColour darkMagenta;

        /// <summary>
        /// Verde azeitona escuro.
        /// </summary>
        private static SystemColour darkOliveGreen;

        /// <summary>
        /// Cor-de-laranja escuro.
        /// </summary>
        private static SystemColour darkOrange;

        /// <summary>
        /// Cor orquídea escuro.
        /// </summary>
        private static SystemColour darkOrchid;

        /// <summary>
        /// Vermelho escuro.
        /// </summary>
        private static SystemColour darkRed;

        /// <summary>
        /// Cor salmão escuro.
        /// </summary>
        private static SystemColour darkSalmon;

        /// <summary>
        /// Verde marinho escuro.
        /// </summary>
        private static SystemColour darkSeaGreen;

        /// <summary>
        /// Azul ardósia escuro.
        /// </summary>
        private static SystemColour darkSlateBlue;

        /// <summary>
        /// Cinzento ardósia escuro.
        /// </summary>
        private static SystemColour darkSlateGray;

        /// <summary>
        /// Turquesa escuro.
        /// </summary>
        private static SystemColour darkTurquoise;

        /// <summary>
        /// Violeta escuro.
        /// </summary>
        private static SystemColour darkViolet;

        /// <summary>
        /// Cor-de-rosa profundo.
        /// </summary>
        private static SystemColour deepPink;

        /// <summary>
        /// Azul céu profundo.
        /// </summary>
        private static SystemColour deepSkyBlue;

        /// <summary>
        /// Cinzento escurecido.
        /// </summary>
        private static SystemColour dimGray;

        /// <summary>
        /// Azul matreiro.
        /// </summary>
        private static SystemColour dodgerBlue;

        /// <summary>
        /// Cor-de-tijolo fogoso.
        /// </summary>
        private static SystemColour firebrick;

        /// <summary>
        /// Branco flural.
        /// </summary>
        private static SystemColour floralWhite;

        /// <summary>
        /// Verde floresta.
        /// </summary>
        private static SystemColour forestGreen;

        /// <summary>
        /// Fúcsia.
        /// </summary>
        private static SystemColour fuchsia;

        /// <summary>
        /// Acinzentado.
        /// </summary>
        private static SystemColour gainsboro;

        /// <summary>
        /// Branco fantasma.
        /// </summary>
        private static SystemColour ghostWhite;

        /// <summary>
        /// Dourado.
        /// </summary>
        private static SystemColour gold;

        /// <summary>
        /// Dourado torrado.
        /// </summary>
        private static SystemColour goldenrod;

        /// <summary>
        /// Cinzento.
        /// </summary>
        private static SystemColour gray;

        /// <summary>
        /// Verde.
        /// </summary>
        private static SystemColour green;

        /// <summary>
        /// Verde amarelado.
        /// </summary>
        private static SystemColour greenYellow;

        /// <summary>
        /// Melado.
        /// </summary>
        private static SystemColour honeydew;

        /// <summary>
        /// Cor-de-rosa quente.
        /// </summary>
        private static SystemColour hotPink;

        /// <summary>
        /// Vermelho índio.
        /// </summary>
        private static SystemColour indianRed;

        /// <summary>
        /// Indigo.
        /// </summary>
        private static SystemColour indigo;

        /// <summary>
        /// Cor-de-marfim.
        /// </summary>
        private static SystemColour ivory;

        /// <summary>
        /// Caqui.
        /// </summary>
        private static SystemColour khaki;

        /// <summary>
        /// Lavanda.
        /// </summary>
        private static SystemColour lavender;

        /// <summary>
        /// Lavanda alfazema.
        /// </summary>
        private static SystemColour lavenderBlush;

        /// <summary>
        /// Verde gramado.
        /// </summary>
        private static SystemColour lawnGreen;

        /// <summary>
        /// Limão sedoso.
        /// </summary>
        private static SystemColour lemonChiffon;

        /// <summary>
        /// Azul claro.
        /// </summary>
        private static SystemColour lightBlue;

        /// <summary>
        /// Coral claro.
        /// </summary>
        private static SystemColour lightCoral;

        /// <summary>
        /// Ciano claro.
        /// </summary>
        private static SystemColour lightCyan;

        /// <summary>
        /// Dourado torrado claro amarelado.
        /// </summary>
        private static SystemColour lightGoldenrodYellow;

        /// <summary>
        /// Cinzento claro.
        /// </summary>
        private static SystemColour lightGray;

        /// <summary>
        /// Verde claro.
        /// </summary>
        private static SystemColour lightGreen;

        /// <summary>
        /// Cor-de-rosa claro.
        /// </summary>
        private static SystemColour lightPink;

        /// <summary>
        /// Salmão claro.
        /// </summary>
        private static SystemColour lightSalmon;

        /// <summary>
        /// Verde marinho claro.
        /// </summary>
        private static SystemColour lightSeaGreen;

        /// <summary>
        /// Azul cerúleo claro.
        /// </summary>
        private static SystemColour lightSkyBlue;

        /// <summary>
        /// Cinzento ardósia claro.
        /// </summary>
        private static SystemColour lightSlateGray;

        /// <summary>
        /// Azul aço claro.
        /// </summary>
        private static SystemColour lightSteelBlue;

        /// <summary>
        /// Amarelo claro.
        /// </summary>
        private static SystemColour lightYellow;

        /// <summary>
        /// Lima.
        /// </summary>
        private static SystemColour lime;

        /// <summary>
        /// Verde lima.
        /// </summary>
        private static SystemColour limeGreen;

        /// <summary>
        /// Linho.
        /// </summary>
        private static SystemColour linen;

        /// <summary>
        /// Magenta.
        /// </summary>
        private static SystemColour magenta;

        /// <summary>
        /// Castanho avermelhado.
        /// </summary>
        private static SystemColour maroon;

        /// <summary>
        /// Água-marinho médio.
        /// </summary>
        private static SystemColour mediumAquamarine;

        /// <summary>
        /// Azul médio.
        /// </summary>
        private static SystemColour mediumBlue;

        /// <summary>
        /// Orquídea médio.
        /// </summary>
        private static SystemColour mediumOrchid;

        /// <summary>
        /// Púrpura médio.
        /// </summary>
        private static SystemColour mediumPurple;

        /// <summary>
        /// Verde marinho médio.
        /// </summary>
        private static SystemColour mediumSeaGreen;

        /// <summary>
        /// Azul ardósia médio.
        /// </summary>
        private static SystemColour mediumSlateBlue;

        /// <summary>
        /// Verde primavera médio.
        /// </summary>
        private static SystemColour mediumSpringGreen;

        /// <summary>
        /// Turquesa médio.
        /// </summary>
        private static SystemColour mediumTurquoise;

        /// <summary>
        /// Vermelho violeta médio.
        /// </summary>
        private static SystemColour mediumVioletRed;

        /// <summary>
        /// Azul meia-noite.
        /// </summary>
        private static SystemColour midnightBlue;

        /// <summary>
        /// Creme hortelã.
        /// </summary>
        private static SystemColour mintCream;

        /// <summary>
        /// Cor-de-rosa enevoado.
        /// </summary>
        private static SystemColour mistyRose;

        /// <summary>
        /// Mocassim.
        /// </summary>
        private static SystemColour moccasin;

        /// <summary>
        /// Branco navajo.
        /// </summary>
        private static SystemColour navajoWhite;

        /// <summary>
        /// Marinha.
        /// </summary>
        private static SystemColour navy;

        /// <summary>
        /// Renda antiga.
        /// </summary>
        private static SystemColour oldLace;

        /// <summary>
        /// Azeitona.
        /// </summary>
        private static SystemColour olive;

        /// <summary>
        /// Azeitona monótono.
        /// </summary>
        private static SystemColour oliveDrab;

        /// <summary>
        /// Cor-de-laranja.
        /// </summary>
        private static SystemColour orange;

        /// <summary>
        /// Cor-de-laranha avermelhado.
        /// </summary>
        private static SystemColour orangeRed;

        /// <summary>
        /// Orquídea.
        /// </summary>
        private static SystemColour orchid;

        /// <summary>
        /// Dourado torrado pálido.
        /// </summary>
        private static SystemColour paleGoldenrod;

        /// <summary>
        /// Verde pálido.
        /// </summary>
        private static SystemColour paleGreen;

        /// <summary>
        /// Turquesa pálido.
        /// </summary>
        private static SystemColour paleTurquoise;

        /// <summary>
        /// Vermelho violeta pálido.
        /// </summary>
        private static SystemColour paleVioletRed;

        /// <summary>
        /// Chicote de papaia.
        /// </summary>
        private static SystemColour papayaWhip;

        /// <summary>
        /// Pele de pêssego.
        /// </summary>
        private static SystemColour peachPuff;

        /// <summary>
        /// Perú.
        /// </summary>
        private static SystemColour peru;

        /// <summary>
        /// Cor-de-rosa.
        /// </summary>
        private static SystemColour pink;

        /// <summary>
        /// Ameixoa.
        /// </summary>
        private static SystemColour plum;

        /// <summary>
        /// Azul pó.
        /// </summary>
        private static SystemColour powderBlue;

        /// <summary>
        /// Púrpura.
        /// </summary>
        private static SystemColour purple;

        /// <summary>
        /// Vermelho.
        /// </summary>
        private static SystemColour red;

        /// <summary>
        /// Castanho rosado.
        /// </summary>
        private static SystemColour rosyBrown;

        /// <summary>
        /// Azul real.
        /// </summary>
        private static SystemColour royalBlue;

        /// <summary>
        /// Castanho cela.
        /// </summary>
        private static SystemColour saddleBrown;

        /// <summary>
        /// Salmão.
        /// </summary>
        private static SystemColour salmon;

        /// <summary>
        /// Castanho arenoso.
        /// </summary>
        private static SystemColour sandyBrown;

        /// <summary>
        /// Verde marinho.
        /// </summary>
        private static SystemColour seaGreen;

        /// <summary>
        /// Concha do mar.
        /// </summary>
        private static SystemColour seaShell;

        /// <summary>
        /// Terra ferrugem.
        /// </summary>
        private static SystemColour sienna;

        /// <summary>
        /// Prateado.
        /// </summary>
        private static SystemColour silver;

        /// <summary>
        /// Azul cerúleo.
        /// </summary>
        private static SystemColour skyBlue;

        /// <summary>
        /// Azul ardósia.
        /// </summary>
        private static SystemColour slateBlue;

        /// <summary>
        /// Cinzento ardósia.
        /// </summary>
        private static SystemColour slateGray;

        /// <summary>
        /// Neve.
        /// </summary>
        private static SystemColour snow;

        /// <summary>
        /// Verde primaveril.
        /// </summary>
        private static SystemColour springGreen;

        /// <summary>
        /// Azul aço.
        /// </summary>
        private static SystemColour steelBlue;

        /// <summary>
        /// Bronzeado.
        /// </summary>
        private static SystemColour tan;

        /// <summary>
        /// Cerceta.
        /// </summary>
        private static SystemColour teal;

        /// <summary>
        /// Cardo.
        /// </summary>
        private static SystemColour thistle;

        /// <summary>
        /// Tomate.
        /// </summary>
        private static SystemColour tomato;

        /// <summary>
        /// Transparente.
        /// </summary>
        private static SystemColour transparent;

        /// <summary>
        /// Turquesa.
        /// </summary>
        private static SystemColour turquoise;

        /// <summary>
        /// Violeta.
        /// </summary>
        private static SystemColour violet;

        /// <summary>
        /// Trigo.
        /// </summary>
        private static SystemColour wheat;

        /// <summary>
        /// Branco.
        /// </summary>
        private static SystemColour white;

        /// <summary>
        /// Branco fumo.
        /// </summary>
        private static SystemColour whiteSmoke;

        /// <summary>
        /// Amarelo.
        /// </summary>
        private static SystemColour yellow;

        /// <summary>
        /// Amarelo esverdeado.
        /// </summary>
        private static SystemColour yellowGreen;

        #endregion Cores especiais

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="SystemColour"/>.
        /// </summary>
        public SystemColour()
        {
            this.colour = new Color();
        }

        /// <summary>
        /// Obtém o byte A da cor.
        /// </summary>
        public byte A { get { return this.colour.A; } }

        /// <summary>
        /// Obtém o byte B da cor.
        /// </summary>
        public byte B { get { return this.colour.B; } }

        /// <summary>
        /// Obtém o byte G da cor.
        /// </summary>
        public byte G { get { return this.colour.G; } }

        /// <summary>
        /// Obtém o byte R da cor.
        /// </summary>
        public byte R { get { return this.colour.R; } }

        /// <summary>
        /// Obtém um valor que indica se a cor é vazia.
        /// </summary>
        public bool IsEmpty { get { return this.colour.IsEmpty; } }

        /// <summary>
        /// Obtém um valor que indica se a cor possui um nome.
        /// </summary>
        public bool IsNamedColour { get { return this.colour.IsNamedColor; } }

        /// <summary>
        /// Obtém um nome que indica se se trata de uma cor do sistema.
        /// </summary>
        public bool IsSystemColour { get { return this.colour.IsSystemColor; } }

        /// <summary>
        /// Obtém o nome da cor.
        /// </summary>
        public string Name { get { return this.colour.Name; } }

        #region Cores especiais

        /// <summary>
        /// Obtém a cor vazia.
        /// </summary>
        public static SystemColour Empty
        {
            get
            {
                if (empty == null)
                {
                    empty = new SystemColour();
                    empty.colour = Color.Empty;
                }

                return empty;
            }
        }

        /// <summary>
        /// Obtém a cor azul claro.
        /// </summary>
        public static SystemColour AliceBlue
        {
            get
            {
                if (aliceBlue == null)
                {
                    aliceBlue = new SystemColour();
                    aliceBlue.colour = Color.AliceBlue;
                }

                return aliceBlue;
            }
        }

        /// <summary>
        /// Obtém a cor barnco antigo.
        /// </summary>
        public static SystemColour AntiqueWhite
        {
            get
            {
                if (antiqueWhite == null)
                {
                    antiqueWhite = new SystemColour();
                    antiqueWhite.colour = Color.AntiqueWhite;
                }

                return antiqueWhite;
            }
        }

        /// <summary>
        /// Obtém a cor azul água.
        /// </summary>
        public static SystemColour Aqua
        {
            get
            {
                if (aqua == null)
                {
                    aqua = new SystemColour();
                    aqua.colour = Color.Aqua;
                }

                return aqua;
            }
        }

        /// <summary>
        /// Obtém a cor azul marinho água.
        /// </summary>
        public static SystemColour Aquamarine
        {
            get
            {
                if (aquamarine == null)
                {
                    aquamarine = new SystemColour();
                    aquamarine.colour = Color.Aquamarine;
                }

                return aquamarine;
            }
        }

        /// <summary>
        /// Obtém a cor azul.
        /// </summary>
        public static SystemColour Azure
        {
            get
            {
                if (azure == null)
                {
                    azure = new SystemColour();
                    azure.colour = Color.Azure;
                }

                return azure;
            }
        }

        /// <summary>
        /// Obtém a cor bege.
        /// </summary>
        public static SystemColour Beige
        {
            get
            {
                if (beige == null)
                {
                    beige = new SystemColour();
                    beige.colour = Color.Beige;
                }

                return beige;
            }
        }

        /// <summary>
        /// Obtém a cor vermelho amarelado.
        /// </summary>
        public static SystemColour Bisque
        {
            get
            {
                if (bisque == null)
                {
                    bisque = new SystemColour();
                    bisque.colour = Color.Bisque;
                }

                return bisque;
            }
        }

        /// <summary>
        /// Obtém a cor preto.
        /// </summary>
        public static SystemColour Black
        {
            get
            {
                if (black == null)
                {
                    black = new SystemColour();
                    black.colour = Color.Black;
                }

                return black;
            }
        }

        /// <summary>
        /// Obtém a cor amêndoa empalidecido.
        /// </summary>
        public static SystemColour BlanchedAlmond
        {
            get
            {
                if (blanchedAlmond == null)
                {
                    blanchedAlmond = new SystemColour();
                    blanchedAlmond.colour = Color.BlanchedAlmond;
                }

                return blanchedAlmond;
            }
        }

        /// <summary>
        /// Obtém a cor azul.
        /// </summary>
        public static SystemColour Blue
        {
            get
            {
                if (aliceBlue == null)
                {
                    blue = new SystemColour();
                    blue.colour = Color.Blue;
                }

                return blue;
            }
        }

        /// <summary>
        /// Obtém a cor azul violeta.
        /// </summary>
        public static SystemColour BlueViolet
        {
            get
            {
                if (blueViolet == null)
                {
                    blueViolet = new SystemColour();
                    blueViolet.colour = Color.BlueViolet;
                }

                return blueViolet;
            }
        }

        /// <summary>
        /// Obtém a cor castanho.
        /// </summary>
        public static SystemColour Brown
        {
            get
            {
                if (brown == null)
                {
                    brown = new SystemColour();
                    brown.colour = Color.Brown;
                }

                return brown;
            }
        }

        /// <summary>
        /// Obtém a cor madeira.
        /// </summary>
        public static SystemColour BurlyWood
        {
            get
            {
                if (burlyWood == null)
                {
                    burlyWood = new SystemColour();
                    burlyWood.colour = Color.BurlyWood;
                }

                return burlyWood;
            }
        }

        /// <summary>
        /// Obtém a cor azul cadete.
        /// </summary>
        public static SystemColour CadetBlue
        {
            get
            {
                if (cadetBlue == null)
                {
                    cadetBlue = new SystemColour();
                    cadetBlue.colour = Color.CadetBlue;
                }

                return cadetBlue;
            }
        }

        /// <summary>
        /// Obtém a cor verde amarelado.
        /// </summary>
        public static SystemColour Chartreuse
        {
            get
            {
                if (chartreuse == null)
                {
                    chartreuse = new SystemColour();
                    chartreuse.colour = Color.Chartreuse;
                }

                return chartreuse;
            }
        }

        /// <summary>
        /// Obtém a cor-de-cholocate.
        /// </summary>
        public static SystemColour Chocolate
        {
            get
            {
                if (chocolate == null)
                {
                    chocolate = new SystemColour();
                    chocolate.colour = Color.Chocolate;
                }

                return chocolate;
            }
        }

        /// <summary>
        /// Obtém a cor coral.
        /// </summary>
        public static SystemColour Coral
        {
            get
            {
                if (coral == null)
                {
                    coral = new SystemColour();
                    coral.colour = Color.Coral;
                }

                return coral;
            }
        }

        /// <summary>
        /// Obtém a cor azul flor de milho.
        /// </summary>
        public static SystemColour CornflowerBlue
        {
            get
            {
                if (cornflowerBlue == null)
                {
                    cornflowerBlue = new SystemColour();
                    cornflowerBlue.colour = Color.CornflowerBlue;
                }

                return cornflowerBlue;
            }
        }

        /// <summary>
        /// Obtém a cor seda de milho.
        /// </summary>
        public static SystemColour Cornsilk
        {
            get
            {
                if (cornsilk == null)
                {
                    cornsilk = new SystemColour();
                    cornsilk.colour = Color.Cornsilk;
                }

                return cornsilk;
            }
        }

        /// <summary>
        /// Obtém a cor carmesim.
        /// </summary>
        public static SystemColour Crimson
        {
            get
            {
                if (crimson == null)
                {
                    crimson = new SystemColour();
                    crimson.colour = Color.Crimson;
                }

                return crimson;
            }
        }

        /// <summary>
        /// Obtém a cor ciano.
        /// </summary>
        public static SystemColour Cyan
        {
            get
            {
                if (cyan == null)
                {
                    cyan = new SystemColour();
                    cyan.colour = Color.Cyan;
                }

                return cyan;
            }
        }

        /// <summary>
        /// Obtém a cor azul escuro.
        /// </summary>
        public static SystemColour DarkBlue
        {
            get
            {
                if (darkBlue == null)
                {
                    darkBlue = new SystemColour();
                    darkBlue.colour = Color.DarkBlue;
                }

                return darkBlue;
            }
        }

        /// <summary>
        /// Obtém a cor ciano escuro.
        /// </summary>
        public static SystemColour DarkCyan
        {
            get
            {
                if (darkCyan == null)
                {
                    darkCyan = new SystemColour();
                    darkCyan.colour = Color.DarkCyan;
                }

                return darkCyan;
            }
        }

        /// <summary>
        /// Obtém a cor dourado torrado escuro.
        /// </summary>
        public static SystemColour DarkGoldenrod
        {
            get
            {
                if (darkGoldenrod == null)
                {
                    darkGoldenrod = new SystemColour();
                    darkGoldenrod.colour = Color.DarkGoldenrod;
                }

                return darkGoldenrod;
            }
        }

        /// <summary>
        /// Obtém a cor cinzento escuro.
        /// </summary>
        public static SystemColour DarkGray
        {
            get
            {
                if (darkGray == null)
                {
                    darkGray = new SystemColour();
                    darkGray.colour = Color.DarkGray;
                }

                return darkGray;
            }
        }

        /// <summary>
        /// Obtém a cor verde escuro.
        /// </summary>
        public static SystemColour DarkGreen
        {
            get
            {
                if (darkGreen == null)
                {
                    darkGreen = new SystemColour();
                    darkGreen.colour = Color.DarkGreen;
                }

                return darkGreen;
            }
        }

        /// <summary>
        /// Obtém a cor caqui escuro.
        /// </summary>
        public static SystemColour DarkKhaki
        {
            get
            {
                if (darkKhaki == null)
                {
                    darkKhaki = new SystemColour();
                    darkKhaki.colour = Color.DarkKhaki;
                }

                return darkKhaki;
            }
        }

        /// <summary>
        /// Obtém a cor magenta escuro.
        /// </summary>
        public static SystemColour DarkMagenta
        {
            get
            {
                if (darkMagenta == null)
                {
                    darkMagenta = new SystemColour();
                    darkMagenta.colour = Color.DarkMagenta;
                }

                return darkMagenta;
            }
        }

        /// <summary>
        /// Obtém a cor verde azeitona escuro.
        /// </summary>
        public static SystemColour DarkOliveGreen
        {
            get
            {
                if (darkOliveGreen == null)
                {
                    darkOliveGreen = new SystemColour();
                    darkOliveGreen.colour = Color.DarkOliveGreen;
                }

                return darkOliveGreen;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-laranja escuro.
        /// </summary>
        public static SystemColour DarkOrange
        {
            get
            {
                if (darkOrange == null)
                {
                    darkOrange = new SystemColour();
                    darkOrange.colour = Color.DarkOrange;
                }

                return darkOrange;
            }
        }

        /// <summary>
        /// Obtém a cor orquídea escuro.
        /// </summary>
        public static SystemColour DarkOrchid
        {
            get
            {
                if (darkOrchid == null)
                {
                    darkOrchid = new SystemColour();
                    darkOrchid.colour = Color.DarkOrchid;
                }

                return darkOrchid;
            }
        }

        /// <summary>
        /// Obtém a cor vermelho escuro.
        /// </summary>
        public static SystemColour DarkRed
        {
            get
            {
                if (darkRed == null)
                {
                    darkRed = new SystemColour();
                    darkRed.colour = Color.DarkRed;
                }

                return darkRed;
            }
        }

        /// <summary>
        /// Obtém a cor salmão escuro.
        /// </summary>
        public static SystemColour DarkSalmon
        {
            get
            {
                if (darkSalmon == null)
                {
                    darkSalmon = new SystemColour();
                    darkSalmon.colour = Color.DarkSalmon;
                }

                return darkSalmon;
            }
        }

        /// <summary>
        /// Obtém a cor verde marinho escuro.
        /// </summary>
        public static SystemColour DarkSeaGreen
        {
            get
            {
                if (darkSeaGreen == null)
                {
                    darkSeaGreen = new SystemColour();
                    darkSeaGreen.colour = Color.DarkSeaGreen;
                }

                return darkSeaGreen;
            }
        }

        /// <summary>
        /// Obtém a cor azul ardósia escuro.
        /// </summary>
        public static SystemColour DarkSlateBlue
        {
            get
            {
                if (darkSlateBlue == null)
                {
                    darkSlateBlue = new SystemColour();
                    darkSlateBlue.colour = Color.DarkSlateBlue;
                }

                return darkSlateBlue;
            }
        }

        /// <summary>
        /// Obtém a cor cinzento ardósia escuro.
        /// </summary>
        public static SystemColour DarkSlateGray
        {
            get
            {
                if (darkSlateGray == null)
                {
                    darkSlateGray = new SystemColour();
                    darkSlateGray.colour = Color.DarkSlateGray;
                }

                return darkSlateGray;
            }
        }

        /// <summary>
        /// Obtém a cor turquesa escuro.
        /// </summary>
        public static SystemColour DarkTurquoise
        {
            get
            {
                if (darkTurquoise == null)
                {
                    darkTurquoise = new SystemColour();
                    darkTurquoise.colour = Color.DarkTurquoise;
                }

                return darkTurquoise;
            }
        }

        /// <summary>
        /// Obtém a cor violeta escuro.
        /// </summary>
        public static SystemColour DarkViolet
        {
            get
            {
                if (darkViolet == null)
                {
                    darkViolet = new SystemColour();
                    darkViolet.colour = Color.DarkViolet;
                }

                return darkViolet;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-rosa profundo.
        /// </summary>
        public static SystemColour DeepPink
        {
            get
            {
                if (deepPink == null)
                {
                    deepPink = new SystemColour();
                    deepPink.colour = Color.DeepPink;
                }

                return deepPink;
            }
        }

        /// <summary>
        /// Obtém a cor azul cerúleo escuro.
        /// </summary>
        public static SystemColour DeepSkyBlue
        {
            get
            {
                if (deepSkyBlue == null)
                {
                    deepSkyBlue = new SystemColour();
                    deepSkyBlue.colour = Color.DeepSkyBlue;
                }

                return deepSkyBlue;
            }
        }

        /// <summary>
        /// Obtém a cor cinzento escurecido.
        /// </summary>
        public static SystemColour DimGray
        {
            get
            {
                if (dimGray == null)
                {
                    dimGray = new SystemColour();
                    dimGray.colour = Color.DimGray;
                }

                return dimGray;
            }
        }

        /// <summary>
        /// Obtém a cor azul matreiro.
        /// </summary>
        public static SystemColour DodgerBlue
        {
            get
            {
                if (dodgerBlue == null)
                {
                    dodgerBlue = new SystemColour();
                    dodgerBlue.colour = Color.DodgerBlue;
                }

                return dodgerBlue;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-tijolo fogoso.
        /// </summary>
        public static SystemColour Firebrick
        {
            get
            {
                if (firebrick == null)
                {
                    firebrick = new SystemColour();
                    firebrick.colour = Color.Firebrick;
                }

                return firebrick;
            }
        }

        /// <summary>
        /// Obtém a cor branco floral.
        /// </summary>
        public static SystemColour FloralWhite
        {
            get
            {
                if (floralWhite == null)
                {
                    floralWhite = new SystemColour();
                    floralWhite.colour = Color.FloralWhite;
                }

                return floralWhite;
            }
        }

        /// <summary>
        /// Obtém a cor verde floresta.
        /// </summary>
        public static SystemColour ForestGreen
        {
            get
            {
                if (forestGreen == null)
                {
                    forestGreen = new SystemColour();
                    forestGreen.colour = Color.ForestGreen;
                }

                return forestGreen;
            }
        }

        /// <summary>
        /// Obtém a cor fúchia.
        /// </summary>
        public static SystemColour Fuchsia
        {
            get
            {
                if (fuchsia == null)
                {
                    fuchsia = new SystemColour();
                    fuchsia.colour = Color.AliceBlue;
                }

                return fuchsia;
            }
        }

        /// <summary>
        /// Obtém a cor acinzentado.
        /// </summary>
        public static SystemColour Gainsboro
        {
            get
            {
                if (gainsboro == null)
                {
                    gainsboro = new SystemColour();
                    gainsboro.colour = Color.Gainsboro;
                }

                return gainsboro;
            }
        }

        /// <summary>
        /// Obtém a cor branco fantasma.
        /// </summary>
        public static SystemColour GhostWhite
        {
            get
            {
                if (ghostWhite == null)
                {
                    ghostWhite = new SystemColour();
                    ghostWhite.colour = Color.GhostWhite;
                }

                return ghostWhite;
            }
        }

        /// <summary>
        /// Obtém a cor dourado.
        /// </summary>
        public static SystemColour Gold
        {
            get
            {
                if (gold == null)
                {
                    gold = new SystemColour();
                    gold.colour = Color.Gold;
                }

                return gold;
            }
        }

        /// <summary>
        /// Obtém a cor dourado torrado.
        /// </summary>
        public static SystemColour Goldenrod
        {
            get
            {
                if (goldenrod == null)
                {
                    goldenrod = new SystemColour();
                    goldenrod.colour = Color.Goldenrod;
                }

                return goldenrod;
            }
        }

        /// <summary>
        /// Obtém a cor cinzento.
        /// </summary>
        public static SystemColour Gray
        {
            get
            {
                if (gray == null)
                {
                    gray = new SystemColour();
                    gray.colour = Color.Gray;
                }

                return gray;
            }
        }

        /// <summary>
        /// Obtém a cor verde.
        /// </summary>
        public static SystemColour Green
        {
            get
            {
                if (green == null)
                {
                    green = new SystemColour();
                    green.colour = Color.Green;
                }

                return green;
            }
        }

        /// <summary>
        /// Obtém a cor verde amarelado.
        /// </summary>
        public static SystemColour GreenYellow
        {
            get
            {
                if (greenYellow == null)
                {
                    greenYellow = new SystemColour();
                    greenYellow.colour = Color.GreenYellow;
                }

                return greenYellow;
            }
        }

        /// <summary>
        /// Obtém a cor melaço.
        /// </summary>
        public static SystemColour Honeydew
        {
            get
            {
                if (honeydew == null)
                {
                    honeydew = new SystemColour();
                    honeydew.colour = Color.Honeydew;
                }

                return honeydew;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-rosa quente.
        /// </summary>
        public static SystemColour HotPink
        {
            get
            {
                if (hotPink == null)
                {
                    hotPink = new SystemColour();
                    hotPink.colour = Color.HotPink;
                }

                return aliceBlue;
            }
        }

        /// <summary>
        /// Obtém a cor vermelho índio.
        /// </summary>
        public static SystemColour IndianRed
        {
            get
            {
                if (indianRed == null)
                {
                    indianRed = new SystemColour();
                    indianRed.colour = Color.IndianRed;
                }

                return indianRed;
            }
        }

        /// <summary>
        /// Obtém a cor indigo.
        /// </summary>
        public static SystemColour Indigo
        {
            get
            {
                if (indigo == null)
                {
                    indigo = new SystemColour();
                    indigo.colour = Color.Indigo;
                }

                return indigo;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-marfim.
        /// </summary>
        public static SystemColour Ivory
        {
            get
            {
                if (ivory == null)
                {
                    ivory = new SystemColour();
                    ivory.colour = Color.Ivory;
                }

                return ivory;
            }
        }

        /// <summary>
        /// Obtém a cor caqui.
        /// </summary>
        public static SystemColour Khaki
        {
            get
            {
                if (khaki == null)
                {
                    khaki = new SystemColour();
                    khaki.colour = Color.Khaki;
                }

                return khaki;
            }
        }

        /// <summary>
        /// Obtém a cor lavanda.
        /// </summary>
        public static SystemColour Lavender
        {
            get
            {
                if (lavender == null)
                {
                    lavender = new SystemColour();
                    lavender.colour = Color.Lavender;
                }

                return lavender;
            }
        }

        /// <summary>
        /// Obtém a cor alfazema corada.
        /// </summary>
        public static SystemColour LavenderBlush
        {
            get
            {
                if (lavenderBlush == null)
                {
                    lavenderBlush = new SystemColour();
                    lavenderBlush.colour = Color.LavenderBlush;
                }

                return lavenderBlush;
            }
        }

        /// <summary>
        /// Obtém a cor verde gramado.
        /// </summary>
        public static SystemColour LawnGreen
        {
            get
            {
                if (lawnGreen == null)
                {
                    lawnGreen = new SystemColour();
                    lawnGreen.colour = Color.LawnGreen;
                }

                return lawnGreen;
            }
        }

        /// <summary>
        /// Obtém a cor limão seda.
        /// </summary>
        public static SystemColour LemonChiffon
        {
            get
            {
                if (lemonChiffon == null)
                {
                    lemonChiffon = new SystemColour();
                    lemonChiffon.colour = Color.LemonChiffon;
                }

                return lemonChiffon;
            }
        }

        /// <summary>
        /// Obtém a cor azul claro.
        /// </summary>
        public static SystemColour LightBlue
        {
            get
            {
                if (lightBlue == null)
                {
                    lightBlue = new SystemColour();
                    lightBlue.colour = Color.LightBlue;
                }

                return lightBlue;
            }
        }

        /// <summary>
        /// Obtém a cor coral claro.
        /// </summary>
        public static SystemColour LightCoral
        {
            get
            {
                if (lightCoral == null)
                {
                    lightCoral = new SystemColour();
                    lightCoral.colour = Color.LightCoral;
                }

                return lightCoral;
            }
        }

        /// <summary>
        /// Obtém a cor ciano claro.
        /// </summary>
        public static SystemColour LightCyan
        {
            get
            {
                if (lightCyan == null)
                {
                    lightCyan = new SystemColour();
                    lightCyan.colour = Color.LightCyan;
                }

                return lightCyan;
            }
        }

        /// <summary>
        /// Obtém a cor dourado torrado amarelado.
        /// </summary>
        public static SystemColour LightGoldenrodYellow
        {
            get
            {
                if (lightGoldenrodYellow == null)
                {
                    lightGoldenrodYellow = new SystemColour();
                    lightGoldenrodYellow.colour = Color.LightGoldenrodYellow;
                }

                return lightGoldenrodYellow;
            }
        }

        /// <summary>
        /// Obtém a cor cinzento claro.
        /// </summary>
        public static SystemColour LightGray
        {
            get
            {
                if (lightGray == null)
                {
                    lightGray = new SystemColour();
                    lightGray.colour = Color.LightGray;
                }

                return lightGray;
            }
        }

        /// <summary>
        /// Obtém a cor verde claro.
        /// </summary>
        public static SystemColour LightGreen
        {
            get
            {
                if (lightGreen == null)
                {
                    lightGreen = new SystemColour();
                    lightGreen.colour = Color.LightGreen;
                }

                return lightGreen;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-rosa claro.
        /// </summary>
        public static SystemColour LightPink
        {
            get
            {
                if (lightPink == null)
                {
                    lightPink = new SystemColour();
                    lightPink.colour = Color.LightPink;
                }

                return lightPink;
            }
        }

        /// <summary>
        /// Obtém a cor salmão claro.
        /// </summary>
        public static SystemColour LightSalmon
        {
            get
            {
                if (lightSalmon == null)
                {
                    lightSalmon = new SystemColour();
                    lightSalmon.colour = Color.LightSalmon;
                }

                return lightSalmon;
            }
        }

        /// <summary>
        /// Obtém a cor verde marinho claro.
        /// </summary>
        public static SystemColour LightSeaGreen
        {
            get
            {
                if (lightSeaGreen == null)
                {
                    lightSeaGreen = new SystemColour();
                    lightSeaGreen.colour = Color.LightSeaGreen;
                }

                return lightSeaGreen;
            }
        }

        /// <summary>
        /// Obtém a cor azul cerúleo claro.
        /// </summary>
        public static SystemColour LightSkyBlue
        {
            get
            {
                if (lightSkyBlue == null)
                {
                    lightSkyBlue = new SystemColour();
                    lightSkyBlue.colour = Color.LightSkyBlue;
                }

                return lightSkyBlue;
            }
        }

        /// <summary>
        /// Obtém a cor cinzento ardósia claro.
        /// </summary>
        public static SystemColour LightSlateGray
        {
            get
            {
                if (lightSlateGray == null)
                {
                    lightSlateGray = new SystemColour();
                    lightSlateGray.colour = Color.LightSlateGray;
                }

                return lightSlateGray;
            }
        }

        /// <summary>
        /// Obtém a cor azul aço claro.
        /// </summary>
        public static SystemColour LightSteelBlue
        {
            get
            {
                if (lightSteelBlue == null)
                {
                    lightSteelBlue = new SystemColour();
                    lightSteelBlue.colour = Color.LightSteelBlue;
                }

                return lightSteelBlue;
            }
        }

        /// <summary>
        /// Obtém a cor amarelo claro.
        /// </summary>
        public static SystemColour LightYellow
        {
            get
            {
                if (lightYellow == null)
                {
                    lightYellow = new SystemColour();
                    lightYellow.colour = Color.LightYellow;
                }

                return lightYellow;
            }
        }

        /// <summary>
        /// Obtém a cor lima.
        /// </summary>
        public static SystemColour Lime
        {
            get
            {
                if (lime == null)
                {
                    lime = new SystemColour();
                    lime.colour = Color.Lime;
                }

                return lime;
            }
        }

        /// <summary>
        /// Obtém a cor verde lima.
        /// </summary>
        public static SystemColour LimeGreen
        {
            get
            {
                if (limeGreen == null)
                {
                    limeGreen = new SystemColour();
                    limeGreen.colour = Color.LimeGreen;
                }

                return limeGreen;
            }
        }

        /// <summary>
        /// Obtém a cor linho.
        /// </summary>
        public static SystemColour Linen
        {
            get
            {
                if (linen == null)
                {
                    linen = new SystemColour();
                    linen.colour = Color.Linen;
                }

                return linen;
            }
        }

        /// <summary>
        /// Obtém a cor magenta.
        /// </summary>
        public static SystemColour Magenta
        {
            get
            {
                if (magenta == null)
                {
                    magenta = new SystemColour();
                    magenta.colour = Color.Magenta;
                }

                return magenta;
            }
        }

        /// <summary>
        /// Obtém a cor castanho avermelhado.
        /// </summary>
        public static SystemColour Maroon
        {
            get
            {
                if (maroon == null)
                {
                    maroon = new SystemColour();
                    maroon.colour = Color.Maroon;
                }

                return maroon;
            }
        }

        /// <summary>
        /// Obtém a cor água marinha médio.
        /// </summary>
        public static SystemColour MediumAquamarine
        {
            get
            {
                if (mediumAquamarine == null)
                {
                    mediumAquamarine = new SystemColour();
                    mediumAquamarine.colour = Color.MediumAquamarine;
                }

                return mediumAquamarine;
            }
        }

        /// <summary>
        /// Obtém a cor azul médio.
        /// </summary>
        public static SystemColour MediumBlue
        {
            get
            {
                if (mediumBlue == null)
                {
                    mediumBlue = new SystemColour();
                    mediumBlue.colour = Color.MediumBlue;
                }

                return mediumBlue;
            }
        }

        /// <summary>
        /// Obtém a cor orquídea médio.
        /// </summary>
        public static SystemColour MediumOrchid
        {
            get
            {
                if (mediumOrchid == null)
                {
                    mediumOrchid = new SystemColour();
                    mediumOrchid.colour = Color.MediumOrchid;
                }

                return mediumOrchid;
            }
        }

        /// <summary>
        /// Obtém a cor púrpura médio.
        /// </summary>
        public static SystemColour MediumPurple
        {
            get
            {
                if (mediumPurple == null)
                {
                    mediumPurple = new SystemColour();
                    mediumPurple.colour = Color.MediumPurple;
                }

                return mediumPurple;
            }
        }

        /// <summary>
        /// Obtém a cor verde marinho médio.
        /// </summary>
        public static SystemColour MediumSeaGreen
        {
            get
            {
                if (mediumSeaGreen == null)
                {
                    mediumSeaGreen = new SystemColour();
                    mediumSeaGreen.colour = Color.MediumSeaGreen;
                }

                return mediumSeaGreen;
            }
        }

        /// <summary>
        /// Obtém a cor azul ardósia médio.
        /// </summary>
        public static SystemColour MediumSlateBlue
        {
            get
            {
                if (mediumSlateBlue == null)
                {
                    mediumSlateBlue = new SystemColour();
                    mediumSlateBlue.colour = Color.MediumSlateBlue;
                }

                return mediumSlateBlue;
            }
        }

        /// <summary>
        /// Obtém a cor verde primaveril médio.
        /// </summary>
        public static SystemColour MediumSpringGreen
        {
            get
            {
                if (mediumSpringGreen == null)
                {
                    mediumSpringGreen = new SystemColour();
                    mediumSpringGreen.colour = Color.MediumSpringGreen;
                }

                return mediumSpringGreen;
            }
        }

        /// <summary>
        /// Obtém a cor turquesa médio.
        /// </summary>
        public static SystemColour MediumTurquoise
        {
            get
            {
                if (mediumTurquoise == null)
                {
                    mediumTurquoise = new SystemColour();
                    mediumTurquoise.colour = Color.MediumTurquoise;
                }

                return mediumTurquoise;
            }
        }

        /// <summary>
        /// Obtém a cor vermelho violeta médio.
        /// </summary>
        public static SystemColour MediumVioletRed
        {
            get
            {
                if (mediumVioletRed == null)
                {
                    mediumVioletRed = new SystemColour();
                    mediumVioletRed.colour = Color.MediumVioletRed;
                }

                return mediumVioletRed;
            }
        }

        /// <summary>
        /// Obtém a cor azul meia-noite.
        /// </summary>
        public static SystemColour MidnightBlue
        {
            get
            {
                if (midnightBlue == null)
                {
                    midnightBlue = new SystemColour();
                    midnightBlue.colour = Color.MidnightBlue;
                }

                return midnightBlue;
            }
        }

        /// <summary>
        /// Obtém a cor creme hortelã.
        /// </summary>
        public static SystemColour MintCream
        {
            get
            {
                if (mintCream == null)
                {
                    mintCream = new SystemColour();
                    mintCream.colour = Color.MintCream;
                }

                return mintCream;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-rosa enevoado.
        /// </summary>
        public static SystemColour MistyRose
        {
            get
            {
                if (mistyRose == null)
                {
                    mistyRose = new SystemColour();
                    mistyRose.colour = Color.MistyRose;
                }

                return mistyRose;
            }
        }

        /// <summary>
        /// Obtém a cor mocassim.
        /// </summary>
        public static SystemColour Moccasin
        {
            get
            {
                if (moccasin == null)
                {
                    moccasin = new SystemColour();
                    moccasin.colour = Color.Moccasin;
                }

                return moccasin;
            }
        }

        /// <summary>
        /// Obtém a cor branco navajo.
        /// </summary>
        public static SystemColour NavajoWhite
        {
            get
            {
                if (navajoWhite == null)
                {
                    navajoWhite = new SystemColour();
                    navajoWhite.colour = Color.NavajoWhite;
                }

                return navajoWhite;
            }
        }

        /// <summary>
        /// Obtém a cor marinha.
        /// </summary>
        public static SystemColour Navy
        {
            get
            {
                if (navy == null)
                {
                    navy = new SystemColour();
                    navy.colour = Color.Navy;
                }

                return navy;
            }
        }

        /// <summary>
        /// Obtém a cor renda antiga.
        /// </summary>
        public static SystemColour OldLace
        {
            get
            {
                if (oldLace == null)
                {
                    oldLace = new SystemColour();
                    oldLace.colour = Color.OldLace;
                }

                return oldLace;
            }
        }

        /// <summary>
        /// Obtém a cor azeitona.
        /// </summary>
        public static SystemColour Olive
        {
            get
            {
                if (olive == null)
                {
                    olive = new SystemColour();
                    olive.colour = Color.Olive;
                }

                return olive;
            }
        }

        /// <summary>
        /// Obtém a cor azeitona monótono.
        /// </summary>
        public static SystemColour OliveDrab
        {
            get
            {
                if (oliveDrab == null)
                {
                    oliveDrab = new SystemColour();
                    oliveDrab.colour = Color.OliveDrab;
                }

                return oliveDrab;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-laranja.
        /// </summary>
        public static SystemColour Orange
        {
            get
            {
                if (orange == null)
                {
                    orange = new SystemColour();
                    orange.colour = Color.Orange;
                }

                return orange;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-laranja avermelhado.
        /// </summary>
        public static SystemColour OrangeRed
        {
            get
            {
                if (orangeRed == null)
                {
                    orangeRed = new SystemColour();
                    orangeRed.colour = Color.OrangeRed;
                }

                return orangeRed;
            }
        }

        /// <summary>
        /// Obtém a cor orquídea.
        /// </summary>
        public static SystemColour Orchid
        {
            get
            {
                if (orchid == null)
                {
                    orchid = new SystemColour();
                    orchid.colour = Color.Orchid;
                }

                return orchid;
            }
        }

        /// <summary>
        /// Obtém a cor dourado torrado pálido.
        /// </summary>
        public static SystemColour PaleGoldenrod
        {
            get
            {
                if (paleGoldenrod == null)
                {
                    paleGoldenrod = new SystemColour();
                    paleGoldenrod.colour = Color.PaleGoldenrod;
                }

                return paleGoldenrod;
            }
        }

        /// <summary>
        /// Obtém a cor verde pálido.
        /// </summary>
        public static SystemColour PaleGreen
        {
            get
            {
                if (paleGreen == null)
                {
                    paleGreen = new SystemColour();
                    paleGreen.colour = Color.PaleGreen;
                }

                return paleGreen;
            }
        }

        /// <summary>
        /// Obtém a cor turquesa pálido.
        /// </summary>
        public static SystemColour PaleTurquoise
        {
            get
            {
                if (paleTurquoise == null)
                {
                    paleTurquoise = new SystemColour();
                    paleTurquoise.colour = Color.PaleTurquoise;
                }

                return paleTurquoise;
            }
        }

        /// <summary>
        /// Obtém a cor vermelho violeta pálido.
        /// </summary>
        public static SystemColour PaleVioletRed
        {
            get
            {
                if (paleVioletRed == null)
                {
                    paleVioletRed = new SystemColour();
                    paleVioletRed.colour = Color.PaleVioletRed;
                }

                return paleVioletRed;
            }
        }

        /// <summary>
        /// Obtém a cor chicote de papaia.
        /// </summary>
        public static SystemColour PapayaWhip
        {
            get
            {
                if (papayaWhip == null)
                {
                    papayaWhip = new SystemColour();
                    papayaWhip.colour = Color.PapayaWhip;
                }

                return papayaWhip;
            }
        }

        /// <summary>
        /// Obtém a cor pele de pêssego.
        /// </summary>
        public static SystemColour PeachPuff
        {
            get
            {
                if (peachPuff == null)
                {
                    peachPuff = new SystemColour();
                    peachPuff.colour = Color.PeachPuff;
                }

                return peachPuff;
            }
        }

        /// <summary>
        /// Obtém a cor perú.
        /// </summary>
        public static SystemColour Peru
        {
            get
            {
                if (peru == null)
                {
                    peru = new SystemColour();
                    peru.colour = Color.Peru;
                }

                return peru;
            }
        }

        /// <summary>
        /// Obtém a cor cor-de-rosa.
        /// </summary>
        public static SystemColour Pink
        {
            get
            {
                if (pink == null)
                {
                    pink = new SystemColour();
                    pink.colour = Color.Pink;
                }

                return pink;
            }
        }

        /// <summary>
        /// Obtém a cor ameixa.
        /// </summary>
        public static SystemColour Plum
        {
            get
            {
                if (plum == null)
                {
                    plum = new SystemColour();
                    plum.colour = Color.Plum;
                }

                return plum;
            }
        }

        /// <summary>
        /// Obtém a cor azul pó.
        /// </summary>
        public static SystemColour PowderBlue
        {
            get
            {
                if (powderBlue == null)
                {
                    powderBlue = new SystemColour();
                    powderBlue.colour = Color.PowderBlue;
                }

                return powderBlue;
            }
        }

        /// <summary>
        /// Obtém a cor púrpura.
        /// </summary>
        public static SystemColour Purple
        {
            get
            {
                if (purple == null)
                {
                    purple = new SystemColour();
                    purple.colour = Color.Purple;
                }

                return purple;
            }
        }

        /// <summary>
        /// Obtém a cor vermelho.
        /// </summary>
        public static SystemColour Red
        {
            get
            {
                if (red == null)
                {
                    red = new SystemColour();
                    red.colour = Color.Red;
                }

                return red;
            }
        }

        /// <summary>
        /// Obtém a cor castanho rosado.
        /// </summary>
        public static SystemColour RosyBrown
        {
            get
            {
                if (rosyBrown == null)
                {
                    rosyBrown = new SystemColour();
                    rosyBrown.colour = Color.RosyBrown;
                }

                return rosyBrown;
            }
        }

        /// <summary>
        /// Obtém a cor azul real.
        /// </summary>
        public static SystemColour RoyalBlue
        {
            get
            {
                if (royalBlue == null)
                {
                    royalBlue = new SystemColour();
                    royalBlue.colour = Color.RoyalBlue;
                }

                return royalBlue;
            }
        }

        /// <summary>
        /// Obtém a cor castanho cela.
        /// </summary>
        public static SystemColour SaddleBrown
        {
            get
            {
                if (saddleBrown == null)
                {
                    saddleBrown = new SystemColour();
                    saddleBrown.colour = Color.SaddleBrown;
                }

                return saddleBrown;
            }
        }

        /// <summary>
        /// Obtém a cor salmão.
        /// </summary>
        public static SystemColour Salmon
        {
            get
            {
                if (salmon == null)
                {
                    salmon = new SystemColour();
                    salmon.colour = Color.Salmon;
                }

                return salmon;
            }
        }

        /// <summary>
        /// Obtém a cor castanho arenoso.
        /// </summary>
        public static SystemColour SandyBrown
        {
            get
            {
                if (sandyBrown == null)
                {
                    sandyBrown = new SystemColour();
                    sandyBrown.colour = Color.SandyBrown;
                }

                return sandyBrown;
            }
        }

        /// <summary>
        /// Obtém a cor verde marinho.
        /// </summary>
        public static SystemColour SeaGreen
        {
            get
            {
                if (seaGreen == null)
                {
                    seaGreen = new SystemColour();
                    seaGreen.colour = Color.SeaGreen;
                }

                return seaGreen;
            }
        }

        /// <summary>
        /// Obtém a cor concha marinha.
        /// </summary>
        public static SystemColour SeaShell
        {
            get
            {
                if (seaShell == null)
                {
                    seaShell = new SystemColour();
                    seaShell.colour = Color.SeaShell;
                }

                return seaShell;
            }
        }

        /// <summary>
        /// Obtém a cor terra ferrugem.
        /// </summary>
        public static SystemColour Sienna
        {
            get
            {
                if (sienna == null)
                {
                    sienna = new SystemColour();
                    sienna.colour = Color.Sienna;
                }

                return sienna;
            }
        }

        /// <summary>
        /// Obtém a cor prateado.
        /// </summary>
        public static SystemColour Silver
        {
            get
            {
                if (silver == null)
                {
                    silver = new SystemColour();
                    silver.colour = Color.Silver;
                }

                return silver;
            }
        }

        /// <summary>
        /// Obtém a cor azul cerúleo.
        /// </summary>
        public static SystemColour SkyBlue
        {
            get
            {
                if (skyBlue == null)
                {
                    skyBlue = new SystemColour();
                    skyBlue.colour = Color.SkyBlue;
                }

                return skyBlue;
            }
        }

        /// <summary>
        /// Obtém a cor azul ardósia.
        /// </summary>
        public static SystemColour SlateBlue
        {
            get
            {
                if (slateBlue == null)
                {
                    slateBlue = new SystemColour();
                    slateBlue.colour = Color.SlateBlue;
                }

                return slateBlue;
            }
        }

        /// <summary>
        /// Obtém a cor cinzento ardósia.
        /// </summary>
        public static SystemColour SlateGray
        {
            get
            {
                if (slateGray == null)
                {
                    slateGray = new SystemColour();
                    slateGray.colour = Color.SlateGray;
                }

                return slateGray;
            }
        }

        /// <summary>
        /// Obtém a cor neve.
        /// </summary>
        public static SystemColour Snow
        {
            get
            {
                if (snow == null)
                {
                    snow = new SystemColour();
                    snow.colour = Color.Snow;
                }

                return snow;
            }
        }

        /// <summary>
        /// Obtém a cor verde primaveril.
        /// </summary>
        public static SystemColour SpringGreen
        {
            get
            {
                if (springGreen == null)
                {
                    springGreen = new SystemColour();
                    springGreen.colour = Color.SpringGreen;
                }

                return springGreen;
            }
        }

        /// <summary>
        /// Obtém a cor azul aço.
        /// </summary>
        public static SystemColour SteelBlue
        {
            get
            {
                if (steelBlue == null)
                {
                    steelBlue = new SystemColour();
                    steelBlue.colour = Color.SteelBlue;
                }

                return steelBlue;
            }
        }

        /// <summary>
        /// Obtém a cor bronzeado.
        /// </summary>
        public static SystemColour Tan
        {
            get
            {
                if (tan == null)
                {
                    tan = new SystemColour();
                    tan.colour = Color.Tan;
                }

                return tan;
            }
        }

        /// <summary>
        /// Obtém a cor cerceta.
        /// </summary>
        public static SystemColour Teal
        {
            get
            {
                if (teal == null)
                {
                    teal = new SystemColour();
                    teal.colour = Color.Teal;
                }

                return teal;
            }
        }

        /// <summary>
        /// Obtém a cor cardo.
        /// </summary>
        public static SystemColour Thistle
        {
            get
            {
                if (thistle == null)
                {
                    thistle = new SystemColour();
                    thistle.colour = Color.Thistle;
                }

                return thistle;
            }
        }

        /// <summary>
        /// Obtém a cor tomate.
        /// </summary>
        public static SystemColour Tomato
        {
            get
            {
                if (tomato == null)
                {
                    tomato = new SystemColour();
                    tomato.colour = Color.Tomato;
                }

                return tomato;
            }
        }

        /// <summary>
        /// Obtém a cor transparente.
        /// </summary>
        public static SystemColour Transparent
        {
            get
            {
                if (transparent == null)
                {
                    transparent = new SystemColour();
                    transparent.colour = Color.Transparent;
                }

                return transparent;
            }
        }

        /// <summary>
        /// Obtém a cor turquesa.
        /// </summary>
        public static SystemColour Turquoise
        {
            get
            {
                if (turquoise == null)
                {
                    turquoise = new SystemColour();
                    turquoise.colour = Color.Turquoise;
                }

                return turquoise;
            }
        }

        /// <summary>
        /// Obtém a cor violeta.
        /// </summary>
        public static SystemColour Violet
        {
            get
            {
                if (violet == null)
                {
                    violet = new SystemColour();
                    violet.colour = Color.Violet;
                }

                return violet;
            }
        }

        /// <summary>
        /// Obtém a cor trigo.
        /// </summary>
        public static SystemColour Wheat
        {
            get
            {
                if (wheat == null)
                {
                    wheat = new SystemColour();
                    wheat.colour = Color.Wheat;
                }

                return wheat;
            }
        }

        /// <summary>
        /// Obtém a cor branco.
        /// </summary>
        public static SystemColour White
        {
            get
            {
                if (white == null)
                {
                    white = new SystemColour();
                    white.colour = Color.White;
                }

                return white;
            }
        }

        /// <summary>
        /// Obtém a cor fumo branco.
        /// </summary>
        public static SystemColour WhiteSmoke
        {
            get
            {
                if (whiteSmoke == null)
                {
                    whiteSmoke = new SystemColour();
                    whiteSmoke.colour = Color.WhiteSmoke;
                }

                return whiteSmoke;
            }
        }

        /// <summary>
        /// Obtém a cor amarelo.
        /// </summary>
        public static SystemColour Yellow
        {
            get
            {
                if (yellow == null)
                {
                    yellow = new SystemColour();
                    yellow.colour = Color.Yellow;
                }

                return yellow;
            }
        }

        /// <summary>
        /// Obtém a cor amarelo esverdeado.
        /// </summary>
        public static SystemColour YellowGreen
        {
            get
            {
                if (yellowGreen == null)
                {
                    yellowGreen = new SystemColour();
                    yellowGreen.colour = Color.YellowGreen;
                }

                return yellowGreen;
            }
        }

        #endregion Cores especiais

        /// <summary>
        /// Obtém a cor <see cref="System.Drawing.Color"/>.
        /// </summary>
        internal Color Colour
        {
            get { return this.colour; }
        }

        /// <summary>
        /// Obtém o brilho associado à cor.
        /// </summary>
        /// <returns>O valor do brilho.</returns>
        public float GetBrightness()
        {
            return this.colour.GetBrightness();
        }

        /// <summary>
        /// Obtém a matiz associada à cor.
        /// </summary>
        /// <returns>O valor da matiz.</returns>
        public float GetHue()
        {
            return this.colour.GetHue();
        }

        /// <summary>
        /// Obtém a saturação associada à cor.
        /// </summary>
        /// <returns>A saturação.</returns>
        public float GetSaturation()
        {
            return this.colour.GetSaturation();
        }

        /// <summary>
        /// Obtém a cor como ARGB.
        /// </summary>
        /// <returns>A combinação dos bytes ARGB.</returns>
        public int ToArgb()
        {
            return this.colour.ToArgb();
        }

        /// <summary>
        /// Constrói uma cor com base nos bytes RGB.
        /// </summary>
        /// <param name="argb">A combinação dos bytes RGB.</param>
        /// <returns>A cor.</returns>
        public static SystemColour FromArgb(int argb)
        {
            var result = new SystemColour();
            result.colour = Color.FromArgb(argb);
            return result;
        }

        /// <summary>
        /// Obtém uma cor com base nos bytes ARGB, incluindo o canal alfa, tendo
        /// por base uma outra cor.
        /// </summary>
        /// <param name="alpha">O valor do canal alfa.</param>
        /// <param name="baseColour">A cor de base.</param>
        /// <returns>A cor construída.</returns>
        public static SystemColour FromArgb(int alpha, SystemColour baseColour)
        {
            var result = new SystemColour();
            if (baseColour == null)
            {
                result.colour = Color.FromArgb(alpha, Color.Empty);
            }
            else
            {
                result.colour = Color.FromArgb(alpha, baseColour.colour);
            }

            return result;
        }

        /// <summary>
        /// Obtém a cor, especificando cada um dos canais RGB.
        /// </summary>
        /// <param name="red">O valor do canal vermelho.</param>
        /// <param name="green">O valor do canal verde.</param>
        /// <param name="blue">O valor do canal azul.</param>
        /// <returns>A cor construída.</returns>
        public static SystemColour FromArgb(int red, int green, int blue)
        {
            var result = new SystemColour();
            result.colour = Color.FromArgb(red, green, blue);
            return result;
        }

        /// <summary>
        /// Obtém a cor, especificando cada um dos canais RGB, incluindo o canal alfa.
        /// </summary>
        /// <param name="alpha">O valor do canal alfa.</param>
        /// <param name="red">O valor do canal vermelho.</param>
        /// <param name="green">O valor do canal verde.</param>
        /// <param name="blue">O valor do canal azul.</param>
        /// <returns>A cor construída.</returns>
        public static SystemColour FromArgb(int alpha, int red, int green, int blue)
        {
            var result = new SystemColour();
            result.colour = Color.FromArgb(alpha, red, green, blue);
            return result;
        }

        /// <summary>
        /// Obtém a cor especificada pelo nome.
        /// </summary>
        /// <param name="name">O nome da cor.</param>
        /// <returns>A cor.</returns>
        public static SystemColour FromName(string name)
        {
            var result = new SystemColour();
            result.colour = Color.FromName(name);
            return result;
        }

        /// <summary>
        /// Determina se o objecto que representa a cor é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto a ser comparado.</param>
        /// <returns>Verdadeiro caso os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as SystemColour;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                return this.colour.Equals(innerObj.colour);
            }
        }

        /// <summary>
        /// Obtém o código confuso da cor.
        /// </summary>
        /// <returns>O código confuso da cor.</returns>
        public override int GetHashCode()
        {
            return this.colour.GetHashCode();
        }
    }

    /// <summary>
    /// O estilo de cada um dos elementos da borda.
    /// </summary>
    public class TableBorderItem : IBorderItem, ICloneable
    {
        #region Campos privados

        /// <summary>
        /// A cor da borda.
        /// </summary>
        private SystemColour colour;

        /// <summary>
        /// O estilo de linha da borda.
        /// </summary>
        private EBorderStyles style;

        #endregion Campos privados

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableBorderItem"/>.
        /// </summary>
        private TableBorderItem()
        {
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableBorderItem"/>.
        /// </summary>
        public TableBorderItem(SystemColour colour)
        {
            if (colour == null)
            {
                throw new ArgumentNullException("colour");
            }
            else
            {
                this.colour = colour;
                this.style = EBorderStyles.None;
            }
        }

        #region Propriedades

        /// <summary>
        /// Obtém a cor da borda.
        /// </summary>
        public SystemColour Colour
        {
            get
            {
                return this.colour;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.colour = value;
                }
            }
        }

        /// <summary>
        /// Obtém o estilo da linha da borda.
        /// </summary>
        public EBorderStyles Style
        {
            get
            {
                return this.style;
            }
            set
            {
                this.style = value;
            }
        }

        #endregion Propriedades

        /// <summary>
        /// Determina se o estilo actual é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto a ser comparado.</param>
        /// <returns>Verdadeiro caso os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TableBorderItem;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                if (this.style.Equals(innerObj.style))
                {
                    return this.colour.Equals(innerObj.colour);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Determina um código confuso para o estilo actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var result = this.style.GetHashCode();
            result ^= this.colour.GetHashCode();
            return result;
        }

        /// <summary>
        /// Cria um cópia da instância actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        public object Clone()
        {
            var result = new TableBorderItem();
            result.colour = this.colour;
            result.style = this.style;
            return result;
        }
    }

    /// <summary>
    /// O estilo da borda.
    /// </summary>
    public class TableBorder : IBorder, ICloneable
    {
        #region Campos privados

        /// <summary>
        /// O estilo da borda de fundo.
        /// </summary>
        private TableBorderItem bottom;

        /// <summary>
        /// O estilo da borda diagonal.
        /// </summary>
        private TableBorderItem diagonal;

        /// <summary>
        /// Valor que indica se contém diagonal decendente.
        /// </summary>
        private bool diagonalDown;

        /// <summary>
        /// Valor que indica se contém diagonal ascendente.
        /// </summary>
        private bool diagonalUp;

        /// <summary>
        /// O estilo da borda esquerda.
        /// </summary>
        private TableBorderItem left;

        /// <summary>
        /// O estilo da borda direita.
        /// </summary>
        private TableBorderItem right;

        /// <summary>
        /// O estilo da borda de topo.
        /// </summary>
        private TableBorderItem top;

        #endregion Campos privados

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableBorder"/>.
        /// </summary>
        private TableBorder()
        {
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableBorder"/>.
        /// </summary>
        public TableBorder(
            TableBorderItem bottom,
            TableBorderItem diagonal,
            TableBorderItem left,
            TableBorderItem right,
            TableBorderItem top)
        {
            if (bottom == null)
            {
                throw new ArgumentNullException("bottom");
            }
            else if (diagonal == null)
            {
                throw new ArgumentNullException("diagonal");
            }
            else if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else if (top == null)
            {
                throw new ArgumentNullException("top");
            }
            else
            {
                this.bottom = bottom;
                this.diagonal = diagonal;
                this.left = left;
                this.right = right;
                this.top = top;
            }
        }

        #region Propriedades

        /// <summary>
        /// Obtém o estilo da borda de fundo.
        /// </summary>
        public IBorderItem Bottom
        {
            get
            {
                return this.bottom;
            }
            set
            {
                var innerValue = value as TableBorderItem;
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value null or invalid.");
                }
                else
                {
                    this.bottom = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém o estilo da borda diagonal.
        /// </summary>
        public IBorderItem Diagonal
        {
            get
            {
                return this.diagonal;
            }
            set
            {
                var innerValue = value as TableBorderItem;
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value null or invalid.");
                }
                else
                {
                    this.diagonal = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém um valor que indica se contém diagonal descendente.
        /// </summary>
        public bool DiagonalDown
        {
            get
            {
                return this.diagonalDown;
            }
            set
            {
                this.diagonalDown = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se contém diagonal ascendente.
        /// </summary>
        public bool DiagonalUp
        {
            get
            {
                return this.diagonalUp;
            }
            set
            {
                this.diagonalUp = value;
            }
        }

        /// <summary>
        /// Obtém o estilo da borda esquerda.
        /// </summary>
        public IBorderItem Left
        {
            get
            {
                return this.left;
            }
            set
            {
                var innerValue = value as TableBorderItem;
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value null or invalid.");
                }
                else
                {
                    this.left = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém o estilo da borda direita.
        /// </summary>
        public IBorderItem Right
        {
            get
            {
                return this.right;
            }
            set
            {
                var innerValue = value as TableBorderItem;
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value null or invalid.");
                }
                else
                {
                    this.right = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém o estilo da borda de topo.
        /// </summary>
        public IBorderItem Top
        {
            get
            {
                return this.top;
            }
            set
            {
                var innerValue = value as TableBorderItem;
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value null or invalid.");
                }
                else
                {
                    this.top = innerValue;
                }
            }
        }

        #endregion Propriedades

        /// <summary>
        /// Determina se o estilo actual é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso os objectos sejam iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TableBorder;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                if (this.bottom.Equals(innerObj.bottom))
                {
                    if (this.diagonal.Equals(innerObj.diagonal))
                    {
                        if (this.diagonalDown == innerObj.diagonalDown)
                        {
                            if (this.diagonalUp == innerObj.diagonalUp)
                            {
                                if (this.left.Equals(innerObj.left))
                                {
                                    if (this.right.Equals(innerObj.right))
                                    {
                                        return this.top.Equals(innerObj.top);
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do estilo.
        /// </summary>
        /// <returns>O valor do código confuso.</returns>
        public override int GetHashCode()
        {
            var result = this.bottom.GetHashCode();
            result ^= this.diagonal.GetHashCode();
            result ^= this.diagonalDown.GetHashCode();
            result ^= this.diagonalUp.GetHashCode();
            result ^= this.left.GetHashCode();
            result ^= this.right.GetHashCode();
            result ^= this.top.GetHashCode();
            return result;
        }

        /// <summary>
        /// Cria um cópia do estilo actual.
        /// </summary>
        /// <returns>A cópia do estilo.</returns>
        public object Clone()
        {
            var tableBorder = new TableBorder();
            tableBorder.bottom = (TableBorderItem)this.bottom.Clone();
            tableBorder.diagonal = (TableBorderItem)this.diagonal.Clone();
            tableBorder.diagonalDown = this.diagonalDown;
            tableBorder.diagonalUp = this.diagonalUp;
            tableBorder.left = (TableBorderItem)this.left.Clone();
            tableBorder.right = (TableBorderItem)this.right.Clone();
            tableBorder.top = (TableBorderItem)this.top.Clone();
            return tableBorder;
        }
    }

    /// <summary>
    /// O preenchimento.
    /// </summary>
    public class TableGradientFill : IGradientFill, ICloneable
    {
        #region Campos

        /// <summary>
        /// O valor de fundo.
        /// </summary>
        private double bottom;

        /// <summary>
        /// A cor 1.
        /// </summary>
        private SystemColour colour1;

        /// <summary>
        /// A cor 2.
        /// </summary>
        private SystemColour colour2;

        /// <summary>
        /// O ângulo do gradiente linear.
        /// </summary>
        private double degree;

        /// <summary>
        /// O valor à esquerda.
        /// </summary>
        private double left;

        /// <summary>
        /// O valor à direita.
        /// </summary>
        private double right;

        /// <summary>
        /// O valor de topo.
        /// </summary>
        private double top;

        /// <summary>
        /// O tipo do gradiente.
        /// </summary>
        EFillGradientType type;

        #endregion Campos

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="TableGradientFill"/>.
        /// </summary>
        private TableGradientFill()
        {
        }

        /// <summary>
        /// Instancia um nova instância de objectos do tipo <see cref="TableGradientFill"/>.
        /// </summary>
        public TableGradientFill(SystemColour colour1, SystemColour colour2)
        {
            if (colour1 == null)
            {
                throw new ArgumentNullException("colour1");
            }
            else if (colour2 == null)
            {
                throw new ArgumentNullException("colour2");
            }
            else
            {

                this.colour1 = colour1;
                this.colour2 = colour2;
            }
        }

        #region Propriedades

        /// <summary>
        /// Obtém o valor de fundo.
        /// </summary>
        public double Bottom
        {
            get
            {
                return this.bottom;
            }
            set
            {
                this.bottom = value;
            }
        }

        /// <summary>
        /// Obtém a cor 1.
        /// </summary>
        public SystemColour Color1
        {
            get
            {
                return this.colour1;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.colour1 = value;
                }
            }
        }

        /// <summary>
        /// Obtém a cor 2.
        /// </summary>
        public SystemColour Color2
        {
            get
            {
                return this.colour2;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.colour2 = value;
                }
            }
        }

        /// <summary>
        /// Obtém o ângulo do gradiente linear.
        /// </summary>
        public double Degree
        {
            get
            {
                return this.degree;
            }
            set
            {
                this.degree = value;
            }
        }

        /// <summary>
        /// Obtém o valor à esquerda.
        /// </summary>
        public double Left
        {
            get
            {
                return this.left;
            }
            set
            {
                this.left = value;
            }
        }

        /// <summary>
        /// Obtém o valor à direita.
        /// </summary>
        public double Right
        {
            get
            {
                return this.right;
            }
            set
            {
                this.right = value;
            }
        }

        /// <summary>
        /// Obtém o valor de topo.
        /// </summary>
        public double Top
        {
            get
            {
                return this.top;
            }
            set
            {
                this.top = value;
            }
        }

        /// <summary>
        /// Obtém o tipo de gradiente.
        /// </summary>
        public EFillGradientType Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        #endregion Propriedades

        /// <summary>
        /// Determina se o gradiente actual é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto a ser comparado.</param>
        /// <returns>Verdadeiro se ambos os objectos forem iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TableGradientFill;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                if (this.bottom == innerObj.bottom)
                {
                    if (this.colour1.Equals(innerObj.colour1))
                    {
                        if (this.colour2.Equals(innerObj.colour2))
                        {
                            if (this.degree == innerObj.degree)
                            {
                                if (this.left == innerObj.left)
                                {
                                    if (this.right == innerObj.right)
                                    {
                                        if (this.top == innerObj.top)
                                        {
                                            return this.type == innerObj.type;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do objecto actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var result = this.colour1.GetHashCode();
            result ^= this.colour2.GetHashCode();
            result ^= this.bottom.GetHashCode();
            result ^= this.degree.GetHashCode();
            result ^= this.left.GetHashCode();
            result ^= this.right.GetHashCode();
            result ^= this.top.GetHashCode();
            result ^= this.type.GetHashCode();
            return result;
        }

        /// <summary>
        /// Obtém uma cópia do objecto actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        public object Clone()
        {
            var result = new TableGradientFill();
            result.bottom = this.bottom;
            result.colour1 = this.colour1;
            result.colour2 = this.colour2;
            result.degree = this.degree;
            result.left = this.left;
            result.right = this.right;
            result.top = this.top;
            result.type = this.type;
            return result;
        }
    }

    /// <summary>
    /// O preencimento.
    /// </summary>
    public class TableFill : IFIll, ICloneable
    {
        #region Campos

        /// <summary>
        /// A cor de fundo.
        /// </summary>
        private SystemColour backgroundColour;

        /// <summary>
        /// O gradiente de preenchimento.
        /// </summary>
        private TableGradientFill gradient;

        /// <summary>
        /// A cor do padrão.
        /// </summary>
        private SystemColour patternColour;

        /// <summary>
        /// O tipo do padrão.
        /// </summary>
        private EFillStyle patternType;

        #endregion Campos

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableFill"/>.
        /// </summary>
        private TableFill()
        {
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableFill"/>.
        /// </summary>
        public TableFill(
            SystemColour backgroundColour,
            TableGradientFill gradient,
            SystemColour patternColour
            )
        {
            if (backgroundColour == null)
            {
                throw new ArgumentNullException("backgroundColour");
            }
            else if (gradient == null)
            {
                throw new ArgumentNullException("gradient");
            }
            else if (patternColour == null)
            {
                throw new ArgumentNullException("patternColour");
            }
            else
            {
                this.backgroundColour = backgroundColour;
                this.gradient = gradient;
                this.patternColour = patternColour;
                this.patternType = EFillStyle.None;
            }
        }

        #region Propriedades

        /// <summary>
        /// Obtém a cor de fundo.
        /// </summary>
        public SystemColour BackgroundColour
        {
            get
            {
                return this.backgroundColour;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.backgroundColour = value;
                }
            }
        }

        /// <summary>
        /// Obtém o gradiente de preenchimento.
        /// </summary>
        public IGradientFill Gradient
        {
            get
            {
                return this.gradient;
            }
            set
            {
                var innerValue = value as TableGradientFill;
                if (innerValue == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.gradient = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém a cor do padrão.
        /// </summary>
        public SystemColour PatternColour
        {
            get
            {
                return this.patternColour;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.patternColour = value;
                }
            }
        }

        /// <summary>
        /// Obtém o tipo do padrão.
        /// </summary>
        public EFillStyle PatternType
        {
            get
            {
                return this.patternType;
            }
            set
            {
                this.patternType = value;
            }
        }

        #endregion Propriedades

        /// <summary>
        /// Determina se o objecto actual é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TableFill;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                if (this.backgroundColour.Equals(innerObj.backgroundColour))
                {
                    if (this.gradient.Equals(innerObj.gradient))
                    {
                        if (this.patternColour.Equals(innerObj.patternColour))
                        {
                            return this.patternType == innerObj.patternType;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso do objecto actual.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var result = this.backgroundColour.GetHashCode();
            result ^= this.gradient.GetHashCode();
            result ^= this.patternColour.GetHashCode();
            result ^= this.patternType.GetHashCode();
            return result;
        }

        /// <summary>
        /// Obtém uma cópia do objecto actual.
        /// </summary>
        /// <returns>A cópia.</returns>
        public object Clone()
        {
            var result = new TableFill();
            result.backgroundColour = this.backgroundColour;
            result.gradient = (TableGradientFill)this.gradient.Clone();
            result.patternColour = this.patternColour;
            result.patternType = this.patternType;
            return result;
        }
    }

    /// <summary>
    /// O formato numérico.
    /// </summary>
    public class TableNumberStyleFormat : IStyleNumberFormat, ICloneable
    {
        #region Campos

        /// <summary>
        /// O formato.
        /// </summary>
        private string format;

        #endregion Campos

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableNumberStyleFormat"/>.
        /// </summary>
        public TableNumberStyleFormat()
        {
            this.format = string.Empty;
        }

        #region Propriedades

        /// <summary>
        /// Obtém o formato.
        /// </summary>
        public string Format
        {
            get
            {
                return this.format;
            }
            set
            {
                if (value == null)
                {
                    this.format = string.Empty;
                }
                else
                {
                    this.format = value.Trim();
                }
            }
        }

        #endregion Propriedades

        /// <summary>
        /// Determina se o formato corrente é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso os objectos seja iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TableNumberStyleFormat;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                // TODO: escrever uma expressão que permita comparar dois formatos
                return this.format.Equals(innerObj.format);
            }
        }

        /// <summary>
        /// Determina o código confuso do objecto corrente.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            // TODO: escrever a função compatível com a de igualdade
            return this.format.GetHashCode();
        }

        /// <summary>
        /// Cria uma cópia do objecto corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public object Clone()
        {
            var result = new TableNumberStyleFormat();
            result.format = this.format;
            return result;
        }
    }

    /// <summary>
    /// A fonte.
    /// </summary>
    public class TableFont : IFont, ICloneable
    {
        #region Campos

        /// <summary>
        /// Valor que indica se a fonte está a negrito.
        /// </summary>
        private bool bold;

        /// <summary>
        /// A cor da fonte.
        /// </summary>
        private SystemColour colour;

        /// <summary>
        /// A família da fonte.
        /// </summary>
        private int family;

        /// <summary>
        /// Valor que indica se a fonte se encontra a itálico.
        /// </summary>
        private bool italic;

        /// <summary>
        /// O nome da fonte.
        /// </summary>
        private string name;

        /// <summary>
        /// O esquema da fonte.
        /// </summary>
        private string scheme;

        /// <summary>
        /// O tamanho da fonte.
        /// </summary>
        private float size;

        /// <summary>
        /// Valor que indica se a fonte se encontra traçada.
        /// </summary>
        private bool strike;

        /// <summary>
        /// Valor que indica se a fonte se encontra sublinhada.
        /// </summary>
        private bool underLine;

        /// <summary>
        /// O tipo de sublinhado da fonte.
        /// </summary>
        private EUnderlineType underLineType;

        /// <summary>
        /// O tipo de alinhamento vertical.
        /// </summary>
        private EVerticalAlignementFont verticalAlign;

        #endregion Campos

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableFont"/>.
        /// </summary>
        private TableFont()
        {
            this.name = "Calibri";
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableFont"/>.
        /// </summary>
        public TableFont(SystemColour colour)
            : this()
        {
            if (colour == null)
            {
                throw new ArgumentNullException("colour");
            }
            else
            {
                this.colour = colour;
                this.underLineType = EUnderlineType.None;
                this.verticalAlign = EVerticalAlignementFont.None;
                this.size = 11;
            }
        }

        #region Propriedaedes

        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra a negrito.
        /// </summary>
        public bool Bold
        {
            get
            {
                return this.bold;
            }
            set
            {
                this.bold = value;
            }
        }

        /// <summary>
        /// Obtém a cor da fonte.
        /// </summary>
        public SystemColour Colour
        {
            get
            {
                return this.colour;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.colour = value;
                }
            }
        }

        /// <summary>
        /// Obtém a família da fonte.
        /// </summary>
        public int Family
        {
            get
            {
                return this.family;
            }
            set
            {
                this.family = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra em itálico.
        /// </summary>
        public bool Italic
        {
            get
            {
                return this.italic;
            }
            set
            {
                this.italic = value;
            }
        }

        /// <summary>
        /// Obtém o nome da fonte.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("Property is null or invalid.");
                }
                else
                {
                    this.name = value.Trim();
                }
            }
        }

        /// <summary>
        /// Obtém o esquema da fonte.
        /// </summary>
        public string Scheme
        {
            get
            {
                return this.scheme;
            }
            set
            {
                if (scheme == null)
                {
                    this.scheme = string.Empty;
                }
                else
                {
                    this.scheme = value.Trim();
                }
            }
        }

        /// <summary>
        /// Obtém o tamanho da fonte.
        /// </summary>
        public float Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra traçada.
        /// </summary>
        public bool Strike
        {
            get
            {
                return this.strike;
            }
            set
            {
                this.strike = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se a fonte se encontra sublinhada.
        /// </summary>
        public bool UnderLine
        {
            get
            {
                return this.underLine;
            }
            set
            {
                this.underLine = value;
            }
        }

        /// <summary>
        /// Obtém o tipo de sublinhado.
        /// </summary>
        public EUnderlineType UnderLineType
        {
            get
            {
                return this.underLineType;
            }
            set
            {
                this.underLineType = value;
            }
        }

        /// <summary>
        /// Obtém o tipo de alinhamento vertical.
        /// </summary>
        public EVerticalAlignementFont VerticalAlign
        {
            get
            {
                return this.verticalAlign;
            }
            set
            {
                this.verticalAlign = value;
            }
        }

        #endregion Propriedades

        /// <summary>
        /// Estabelece o valor da fonte actual a partir da fonte do sistema.
        /// </summary>
        /// <param name="font">A fonte do sistema</param>
        public void SetFromFont(System.Drawing.Font font)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina se a fonte corrente é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TableFont;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                if (this.bold == innerObj.bold)
                {
                    if (this.colour.Equals(innerObj.colour))
                    {
                        if (this.family.Equals(innerObj.family))
                        {
                            if (this.italic == innerObj.italic)
                            {
                                if (this.name.Equals(innerObj.name))
                                {
                                    if (this.scheme.Equals(innerObj.scheme))
                                    {
                                        if (this.size == innerObj.size)
                                        {
                                            if (this.strike == innerObj.strike)
                                            {
                                                if (this.underLine == innerObj.underLine)
                                                {
                                                    if (this.underLineType == innerObj.underLineType)
                                                    {
                                                        return this.verticalAlign == innerObj.verticalAlign;
                                                    }
                                                    else
                                                    {
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Determina um código confuso para o objecto corrente.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var result = this.bold.GetHashCode();
            result ^= this.colour.GetHashCode();
            result ^= this.family.GetHashCode();
            result ^= this.italic.GetHashCode();
            result ^= this.name.GetHashCode();
            result ^= this.scheme.GetHashCode();
            result ^= this.size.GetHashCode();
            result ^= this.strike.GetHashCode();
            result ^= this.underLine.GetHashCode();
            result ^= this.underLineType.GetHashCode();
            result ^= this.verticalAlign.GetHashCode();
            return result;
        }

        /// <summary>
        /// Cria uma cópia do objecto corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public object Clone()
        {
            var result = new TableFont();
            result.bold = this.bold;
            result.colour = this.colour;
            result.family = this.family;
            result.italic = this.italic;
            result.name = this.name;
            result.scheme = this.scheme;
            result.size = this.size;
            result.strike = this.strike;
            result.underLine = this.underLine;
            result.underLineType = this.underLineType;
            result.verticalAlign = this.verticalAlign;
            return result;
        }
    }

    /// <summary>
    /// O estilo.
    /// </summary>
    public class TableStyle : IStyle, ICloneable
    {
        #region Campos

        /// <summary>
        /// A borda.
        /// </summary>
        private TableBorder border;

        /// <summary>
        /// O estilo de preenchimento.
        /// </summary>
        private TableFill fill;

        /// <summary>
        /// A fonte.
        /// </summary>
        private TableFont font;

        /// <summary>
        /// O alinhamento horizontal.
        /// </summary>
        private EHorizontalAlignement horizontalAllignement;

        /// <summary>
        /// A margem entre a borda e o texto.
        /// </summary>
        private int indent;

        /// <summary>
        /// O formato do número.
        /// </summary>
        private TableNumberStyleFormat numberFormat;

        /// <summary>
        /// A ordem de leitura.
        /// </summary>
        private EReadingOrder readingOrder;

        /// <summary>
        /// Valor que indica se se ajusta o texto.
        /// </summary>
        private bool shrinkToFit;

        /// <summary>
        /// A rotação do texto em graus.
        /// </summary>
        public int textRotation;

        /// <summary>
        /// O tipo do alinhamento vertical.
        /// </summary>
        private EVerticalAlignement verticalAlignement;

        /// <summary>
        /// Valor que indica se o texto será todo apresentado.
        /// </summary>
        private bool wrapText;

        /// <summary>
        /// O nome do estilo.
        /// </summary>
        private string name;

        #endregion Campos

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableStyle"/>.
        /// </summary>
        public TableStyle()
        {
            this.readingOrder = EReadingOrder.LeftToRight;
            this.verticalAlignement = EVerticalAlignement.Bottom;
            this.name = string.Empty;
        }

        #region Properties

        /// <summary>
        /// Obtém borda.
        /// </summary>
        public IBorder Border
        {
            get
            {
                return this.border;
            }
            set
            {
                var innerValue = value as TableBorder;
                if (innerValue == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.border = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém o preenchimento.
        /// </summary>
        public IFIll Fill
        {
            get
            {
                return this.fill;
            }
            set
            {
                var innerValue = value as TableFill;
                if (innerValue == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.fill = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém a fonte.
        /// </summary>
        public IFont Font
        {
            get
            {
                return this.font;
            }
            set
            {
                var innerValue = value as TableFont;
                if (innerValue == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.font = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém o alinhamento horizontal.
        /// </summary>
        public EHorizontalAlignement HorizontalAlignment
        {
            get
            {
                return this.horizontalAllignement;
            }
            set
            {
                this.horizontalAllignement = value;
            }
        }

        /// <summary>
        /// Obtém a margem entre a borda e o texto.
        /// </summary>
        public int Indent
        {
            get
            {
                return this.indent;
            }
            set
            {
                this.indent = value;
            }
        }

        /// <summary>
        /// Obtém o formato numérico.
        /// </summary>
        public IStyleNumberFormat Numberformat
        {
            get
            {
                return this.numberFormat;
            }
            set
            {
                var innerValue = value as TableNumberStyleFormat;
                if (innerValue == null)
                {
                    throw new UtilitiesDataException("Property value is null or invalid.");
                }
                else
                {
                    this.numberFormat = innerValue;
                }
            }
        }

        /// <summary>
        /// Obtém a ordem de leitura.
        /// </summary>
        public EReadingOrder ReadingOrder
        {
            get
            {
                return this.readingOrder;
            }
            set
            {
                this.readingOrder = value;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o texto será diminuído de modo a ser ajustado.
        /// </summary>
        public bool ShrinkToFit
        {
            get
            {
                return this.shrinkToFit;
            }
            set
            {
                this.shrinkToFit = value;
            }
        }

        /// <summary>
        /// Obtém a rotação do texto em graus.
        /// </summary>
        public int TextRotation
        {
            get
            {
                return this.textRotation;
            }
            set
            {
                this.textRotation = value;
            }
        }

        /// <summary>
        /// Obtém o tipo do alinhamento vertical.
        /// </summary>
        public EVerticalAlignement VerticalAlignment
        {
            get
            {
                return this.verticalAlignement;
            }
            set
            {
                this.verticalAlignement = value;
            }
        }

        /// <summary>
        /// Obtém uma valor que indica se o texto será todo apresentado.
        /// </summary>
        public bool WrapText
        {
            get
            {
                return this.wrapText;
            }
            set
            {
                this.wrapText = value;
            }
        }

        /// <summary>
        /// Obtém o nome do estilo.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        #endregion Propriedades

        /// <summary>
        /// Determina se o estilo corrente é igual ao objecto proporcionado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro se os objectos forem iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as TableStyle;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                if (this.border.Equals(innerObj.border))
                {
                    if (this.fill.Equals(innerObj.fill))
                    {
                        if (this.font.Equals(innerObj.font))
                        {
                            if (this.horizontalAllignement == innerObj.horizontalAllignement)
                            {
                                if (this.indent == innerObj.indent)
                                {
                                    if (this.name.Equals(innerObj.name))
                                    {
                                        if (this.numberFormat.Equals(innerObj.numberFormat))
                                        {
                                            if (this.readingOrder == innerObj.readingOrder)
                                            {
                                                if (this.shrinkToFit == innerObj.shrinkToFit)
                                                {
                                                    if (this.textRotation == innerObj.textRotation)
                                                    {
                                                        if (this.verticalAlignement == innerObj.verticalAlignement)
                                                        {
                                                            return this.wrapText.Equals(innerObj.wrapText);
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso para o objecto corrente.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var result = this.border.GetHashCode();
            result ^= this.fill.GetHashCode();
            result ^= this.font.GetHashCode();
            result ^= this.horizontalAllignement.GetHashCode();
            result ^= this.indent.GetHashCode();
            result ^= this.name.GetHashCode();
            result ^= this.numberFormat.GetHashCode();
            result ^= this.readingOrder.GetHashCode();
            result ^= this.shrinkToFit.GetHashCode();
            result ^= this.textRotation.GetHashCode();
            result ^= this.verticalAlignement.GetHashCode();
            result ^= this.wrapText.GetHashCode();
            return result;
        }

        /// <summary>
        /// Cria uma cópia do objecto corrente.
        /// </summary>
        /// <returns>A cópia.</returns>
        public object Clone()
        {
            var result = new TableStyle();
            result.border = (TableBorder)this.border.Clone();
            result.fill = (TableFill)this.fill.Clone();
            result.font = (TableFont)this.font.Clone();
            result.horizontalAllignement = this.horizontalAllignement;
            result.indent = this.indent;
            result.numberFormat = (TableNumberStyleFormat)this.numberFormat.Clone();
            result.readingOrder = this.readingOrder;
            result.shrinkToFit = this.shrinkToFit;
            result.textRotation = this.textRotation;
            result.verticalAlignement = this.verticalAlignement;
            result.wrapText = this.wrapText;
            return result;
        }
    }

    /// <summary>
    /// Implementação de uma colecção de segmentos de texto formatados.
    /// </summary>
    public class TableRichTextCollection : IRichTextCollection
    {
        /// <summary>
        /// A colecção dos elementos de texto formatados.
        /// </summary>
        private List<TableRichText> tableRichTextElements = new List<TableRichText>();

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableRichTextCollection"/>.
        /// </summary>
        public TableRichTextCollection()
        {
        }

        /// <summary>
        /// Obtém o número de segmentos de texto na colecção.
        /// </summary>
        public int Count
        {
            get
            {
                return this.tableRichTextElements.Count;
            }
        }

        /// <summary>
        /// Obtém o valor textual da colecção.
        /// </summary>
        /// <remarks>O texto irá assumir o formato do primeiro segmento.</remarks>
        public string Text
        {
            get
            {
                var text = string.Empty;
                foreach (var richText in this.tableRichTextElements)
                {
                    text += richText.Text;
                }

                return text;
            }
            set
            {
                var innerText = value;
                if (innerText == null)
                {
                    innerText = string.Empty;
                }

                if (this.tableRichTextElements.Count == 0)
                {
                    var richTextElement = new TableRichText(innerText, new TableFont(SystemColour.Black), EVerticalAlignementFont.None);
                    this.tableRichTextElements.Add(richTextElement);
                }
                else
                {
                    while (this.tableRichTextElements.Count > 1)
                    {
                        this.tableRichTextElements.RemoveAt(this.tableRichTextElements.Count - 1);
                    }

                    this.tableRichTextElements[0].Text = innerText;
                }
            }
        }

        /// <summary>
        /// Obtém o segmento de texto especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice do segmento de texto.</param>
        /// <returns>O segmento de texto.</returns>
        public IRichText this[int index]
        {
            get
            {
                if (index < 0 || index > this.tableRichTextElements.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                else
                {
                    return this.tableRichTextElements[index];
                }
            }
        }

        /// <summary>
        /// Add a rich text string.
        /// </summary>
        /// <param name="text">The text to add.</param>
        /// <param name="font">A fonte associada ao segmento de texto.</param>
        /// <param name="verticalAlignment">O alinhamento vertical do texto.</param>
        /// <returns>The added rich text item.</returns>
        public IRichText Add(string text, IFont font, EVerticalAlignementFont verticalAlignment)
        {
            if (font == null)
            {
                throw new ArgumentNullException("font");
            }
            else
            {
                var innerText = text;
                if (text == null)
                {
                    innerText = string.Empty;
                }

                var resultRichText = new TableRichText(innerText, font, verticalAlignment);
                this.tableRichTextElements.Add(resultRichText);
                return resultRichText;
            }
        }

        /// <summary>
        /// Limpa a colecção de segmentos de texto formatados.
        /// </summary>
        public void Clear()
        {
            this.tableRichTextElements.Clear();
        }

        /// <summary>
        /// Remove o segmento de texto especificado.
        /// </summary>
        /// <param name="item">O segmento de texto.</param>
        public void Remove(IRichText item)
        {
            var innerRichText = item as TableRichText;
            this.tableRichTextElements.Remove(innerRichText);
        }

        /// <summary>
        /// Removes an item at the specific index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index > this.tableRichTextElements.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            this.tableRichTextElements.RemoveAt(index);
        }

        /// <summary>
        /// Obtém um eumerador para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<IRichText> GetEnumerator()
        {
            for (int i = 0; i < this.tableRichTextElements.Count; ++i)
            {
                yield return this.tableRichTextElements[i];
            }
        }

        /// <summary>
        /// Obtém o enumerador não-genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Implementação de um 
    /// </summary>
    internal class TableRichText : IRichText
    {
        /// <summary>
        /// A fonte do texto.
        /// </summary>
        private IFont textFont;

        /// <summary>
        /// O valor do segmento de texto.
        /// </summary>
        private string text;

        /// <summary>
        /// O alinhamento vertical.
        /// </summary>
        private EVerticalAlignementFont verticalAlignement;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TableRichText"/>.
        /// </summary>
        /// <param name="text">O segmento de texto.</param>
        /// <param name="font">A fonte.</param>
        /// <param name="verticalAlign">O alinhamento vertical do texto.</param>
        public TableRichText(string text, IFont font, EVerticalAlignementFont verticalAlign)
        {
            this.textFont = font;
            this.verticalAlignement = verticalAlign;
            this.text = text;
        }

        /// <summary>
        /// Obtém ou atribui o texto.
        /// </summary>
        public string Text
        {
            get
            {
                if (this.text == null)
                {
                    return string.Empty;
                }
                else
                {
                    return this.text;
                }
            }
            set
            {
                this.text = value;
            }
        }

        /// <summary>
        /// Obtém ou atribui a fonte associada ao segmento de texto.
        /// </summary>
        public IFont TextFont
        {
            get
            {
                return this.textFont;
            }
            set
            {
                if (value == null)
                {
                    throw new UtilitiesDataException("The property is null or invalid.");
                }
                else
                {
                    this.textFont = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o alinhamento vertical.
        /// </summary>
        public EVerticalAlignementFont VerticalAlign
        {
            get
            {
                return this.verticalAlignement;
            }
            set
            {
                this.verticalAlignement = value;
            }
        }
    }
}
