namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Todos os tipos de dados nativos da classe <see cref="StringSymbolReader"/>.
    /// </summary>
    public enum EStringSymbolReaderType
    {
        /// <summary>
        /// Inteiro.
        /// </summary>
        INTEGER = 0,

        /// <summary>
        /// Decimal.
        /// </summary>
        DOUBLE = 1,

        /// <summary>
        /// Decimal com notação exponencial.
        /// </summary>
        DOUBLE_EXPONENTIAL = 2,

        /// <summary>
        /// Texto.
        /// </summary>
        STRING = 3,

        /// <summary>
        /// Vazios.
        /// </summary>
        BLANCKS=4,

        /// <summary>
        /// =
        /// </summary>
        EQUAL=5,

        /// <summary>
        /// ==
        /// </summary>
        DOUBLE_EQUAL=6,

        /// <summary>
        /// +
        /// </summary>
        PLUS=7,

        /// <summary>
        /// ++
        /// </summary>
        DOUBLE_PLUS=8,

        /// <summary>
        /// +=
        /// </summary>
        PLUS_EQUAL=9,

        /// <summary>
        /// -
        /// </summary>
        MINUS=10,

        /// <summary>
        /// --
        /// </summary>
        DOUBLE_MINUS=11,

        /// <summary>
        /// -=
        /// </summary>
        MINUS_EQUAL=12,

        /// <summary>
        /// *
        /// </summary>
        TIMES=13,

        /// <summary>
        /// **
        /// </summary>
        DOUBLE_TIMES=14,

        /// <summary>
        /// *=
        /// </summary>
        TIMES_EQUAL=15,

        /// <summary>
        /// /
        /// </summary>
        OVER=16,

        /// <summary>
        /// /=
        /// </summary>
        OVER_EQUAL=17,

        /// <summary>
        /// ^
        /// </summary>
        EXPONENTIAL=18,

        /// <summary>
        /// :
        /// </summary>
        COLON=19,

        /// <summary>
        /// ::
        /// </summary>
        DOUBLE_COLON=20,

        /// <summary>
        /// &amp;
        /// </summary>
        BITWISE_AND=21,

        /// <summary>
        /// &amp;&amp;
        /// </summary>
        DOUBLE_AND=22,

        /// <summary>
        /// &amp;&amp;=
        /// </summary>
        AND_EQUAL=23,

        /// <summary>
        /// |
        /// </summary>
        BITWISE_OR=24,

        /// <summary>
        /// ||
        /// </summary>
        DOUBLE_OR=25,

        /// <summary>
        /// |=
        /// </summary>
        OR_EQUAL=26,

        /// <summary>
        /// &lt;
        /// </summary>
        LESS_THAN=27,

        /// <summary>
        /// &lt;&lt;
        /// </summary>
        DOUBLE_LESS=28,

        /// <summary>
        /// &lt;&lt;&lt;
        /// </summary>
        TRIPLE_LESS=29,

        /// <summary>
        /// &lt;=
        /// </summary>
        LESS_EQUAL=30,

        /// <summary>
        /// &gt;
        /// </summary>
        GREAT_THAN=31,

        /// <summary>
        /// &gt;&gt;
        /// </summary>
        DOUBLE_GREAT=32,

        /// <summary>
        /// &gt;&gt;&gt;
        /// </summary>
        TRIPLE_GREAT=33,

        /// <summary>
        /// &gt;=
        /// </summary>
        GREAT_EQUAL=34,

        /// <summary>
        /// (
        /// </summary>
        LEFT_PARENTHESIS=35,

        /// <summary>
        /// )
        /// </summary>
        RIGHT_PARENTHESIS=36,

        /// <summary>
        /// [
        /// </summary>
        LEFT_BRACKET=37,

        /// <summary>
        /// ]
        /// </summary>
        RIGHT_BRACKET=38,

        /// <summary>
        /// "
        /// </summary>
        DOUBLE_QUOTE=39,

        /// <summary>
        /// '
        /// </summary>
        QUOTE=40,

        /// <summary>
        /// \
        /// </summary>
        LEFT_BAR=41,

        /// <summary>
        /// ;
        /// </summary>
        SEMI_COLON=42,

        /// <summary>
        /// ,
        /// </summary>
        COMMA=43,

        /// <summary>
        /// ~
        /// </summary>
        TILD=44,

        /// <summary>
        /// ^
        /// </summary>
        HAT=45,

        /// <summary>
        /// ?
        /// </summary>
        QUESTION_MARK=46,

        /// <summary>
        /// !
        /// </summary>
        EXCLAMATION_MARK=47,

        /// <summary>
        /// {
        /// </summary>
        LEFT_BRACE=48,

        /// <summary>
        /// }
        /// </summary>
        RIGHT_BRACE=49,

        /// <summary>
        /// @
        /// </summary>
        AT=50,

        /// <summary>
        /// #
        /// </summary>
        CARDINAL=51,

        /// <summary>
        /// $
        /// </summary>
        DOLLAR=52,

        /// <summary>
        /// £
        /// </summary>
        POUND=53,

        /// <summary>
        /// §
        /// </summary>
        CHAPTER=54,

        /// <summary>
        /// €
        /// </summary>
        EURO=55,

        /// <summary>
        /// Qualquer.
        /// </summary>
        ANY=56
    }
}
