using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTests.SetExample
{
    enum ESymbolSetType
    {
        /// <summary>
        /// Qualquer carácter.
        /// </summary>
        ANY,

        /// <summary>
        /// O fim do ficheiro.
        /// </summary>
        EOF,

        /// <summary>
        /// A chaveta de abertura.
        /// </summary>
        LBRACE,

        /// <summary>
        /// A chaveta de fecho.
        /// </summary>
        RBRACE,

        /// <summary>
        /// O parêntesis recto de abertura.
        /// </summary>
        LBRACK,

        /// <summary>
        /// O parêntesis recto de fecho.
        /// </summary>
        RBRACK,

        /// <summary>
        /// Parêntesis de abertura.
        /// </summary>
        OPAR,

        /// <summary>
        /// Parêntesis de fecho.
        /// </summary>
        CPAR,

        /// <summary>
        /// O parêntesis angular de abertura.
        /// </summary>
        LANGLE,

        /// <summary>
        /// O parêntesis angular de fecho.
        /// </summary>
        RANGLE,

        /// <summary>
        /// A barra vertical.
        /// </summary>
        VBAR,

        /// <summary>
        /// A vírgula.
        /// </summary>
        COMMA,

        /// <summary>
        /// O espaço.
        /// </summary>
        SPACE,

        /// <summary>
        /// A mudança de linha.
        /// </summary>
        CHANGE_LINE,

        /// <summary>
        /// O símbolo de união.
        /// </summary>
        UNION,

        /// <summary>
        /// O símbolo de intersecção.
        /// </summary>
        INTERSECTION
    }
}
