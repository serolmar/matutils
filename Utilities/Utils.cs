namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class Utils
    {
        /// <summary>
        /// Define todos os tipos de símbolos definidos para o <see cref="StringSymbolReader"/>.
        /// </summary>
        private static string[] stringSymbolReaderTypes = {
            "integer","double","double_exponential","string","blancks","equal","double_equal","plus","double_plus","plus_equal",
            "minus","double_minus","minus_equal","times","double_times","times_equal","over","over_equal","exponential","colon",
            "double_colon","bitwise_and","double_and","and_equal","bitwise_or","double_or","or_equal","less_than","double_less","triple_less",
            "less_equal","great_than","double_great","triple_great","great_equal","left_parenthesis","right_parenthesis","left_bracket","right_bracket","double_quote",
            "left_bar","semi_colon","comma","tild","hat","question_mark","exclamation_mark","left_brace","right_brace","at",
            "cardinal","dollar","pound","chapter","euro","any"};

        /// <summary>
        /// Obtém o texto para o tipo de símbolo num leitor <see cref="StringSymbolReader"/> a partir de 
        /// um valor enumerado.
        /// </summary>
        /// <param name="type">O valor enumerado.</param>
        /// <returns>O tipo de símbolo.</returns>
        public static string GetStringSymbolType(EStringSymbolReaderType type)
        {
            return stringSymbolReaderTypes[(int)type];
        }
    }
}
