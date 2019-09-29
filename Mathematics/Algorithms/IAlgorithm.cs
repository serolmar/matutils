namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType">Recebe os dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType, out OutputType>
    {
        /// <summary>
        /// Executa o algoritmo.
        /// </summary>
        /// <param name="data">O argumento.</param>
        /// <returns>O resultado da execução.</returns>
        OutputType Run(InputType data);
    }

    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType1">Recebe os primeiros dados de entrada.</typeparam>
    /// <typeparam name="InputType2">Recebe os segundos dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType1, in InputType2, out OutputType>
    {
        /// <summary>
        /// Executa o algoritmo.
        /// </summary>
        /// <param name="first">O primeiro argumento.</param>
        /// <param name="second">O segundo argumento.</param>
        /// <returns>O resultado da execução.</returns>
        OutputType Run(InputType1 first, InputType2 second);
    }

    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType1">Recebe os primeiros dados de entrada.</typeparam>
    /// <typeparam name="InputType2">Recebe os segundos dados de entrada.</typeparam>
    /// <typeparam name="InputType3">Recebe os terceiros dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType1, in InputType2, in InputType3, out OutputType>
    {
        /// <summary>
        /// Executa o algoritmo.
        /// </summary>
        /// <param name="first">O primeiro argumento.</param>
        /// <param name="second">O segundo argumento.</param>
        /// <param name="third">O terceiro argumento.</param>
        /// <returns>O resultado da execução.</returns>
        OutputType Run(InputType1 first, InputType2 second, InputType3 third);
    }

    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType1">Recebe os primeiros dados de entrada.</typeparam>
    /// <typeparam name="InputType2">Recebe os segundos dados de entrada.</typeparam>
    /// <typeparam name="InputType3">Recebe os terceiros dados de entrada.</typeparam>
    /// <typeparam name="InputType4">Recebe os quartos dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType1, in InputType2, in InputType3, in InputType4, out OutputType>
    {
        /// <summary>
        /// Executa o algoritmo.
        /// </summary>
        /// <param name="first">O primeiro argumento.</param>
        /// <param name="second">O segundo argumento.</param>
        /// <param name="third">O terceiro argumento.</param>
        /// <param name="fourth">O quarto argumento.</param>
        /// <returns>O resultado da execução.</returns>
        OutputType Run(InputType1 first, InputType2 second, InputType3 third, InputType4 fourth);
    }
}
