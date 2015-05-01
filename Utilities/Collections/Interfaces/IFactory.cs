namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite criar um objecto sem parâmetros de inicialização.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto a ser criado.</typeparam>
    public interface IFactory<out T>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <returns>O objecto criado.</returns>
        T Create();
    }

    /// <summary>
    /// Permite criar um objecto com um parâmetro de inicialização.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto a ser criado.</typeparam>
    /// <typeparam name="P1">O tipo do parâmetro.</typeparam>
    public interface IFactory<out T, in P1>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <param name="item1">O primeiro parâmetro de inicialização.</param>
        /// <returns>O objecto criado.</returns>
        T Create(P1 item1);
    }

    /// <summary>
    /// Permite criar um objecto com dois parâmetros de inicialização.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto a ser criado.</typeparam>
    /// <typeparam name="P1">O tipo do primeiro parâmetro.</typeparam>
    /// <typeparam name="P2">O tipo do segundo parâmetro.</typeparam>
    public interface IFactory<out T, in P1, in P2>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <param name="item1">O primeiro parâmetro de inicialização.</param>
        /// <param name="item2">O segundo parâmetro de inicialização.</param>
        /// <returns>O objecto criado.</returns>
        T Create(P1 item1, P2 item2);
    }

    /// <summary>
    /// Permite criar um objecto com três parâmetros de inicialização.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto a ser criado.</typeparam>
    /// <typeparam name="P1">O tipo do primeiro parâmetro.</typeparam>
    /// <typeparam name="P2">O tipo do segundo parâmetro.</typeparam>
    /// <typeparam name="P3">O tipo do terceiro parâmetro.</typeparam>
    public interface IFactory<out T, in P1, in P2, in P3>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <param name="item1">O primeiro parâmetro de inicialização.</param>
        /// <param name="item2">O segundo parâmetro de inicialização.</param>
        /// <param name="item3">O terceiro parâmetro de inicialização.</param>
        /// <returns>O objecto criado.</returns>
        T Create(P1 item1, P2 item2, P3 item3);
    }

    /// <summary>
    /// Permite criar um objecto com quatro parâmetros de inicialização.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto a ser criado.</typeparam>
    /// <typeparam name="P1">O tipo do primeiro parâmetro.</typeparam>
    /// <typeparam name="P2">O tipo do segundo parâmetro.</typeparam>
    /// <typeparam name="P3">O tipo do terceiro parâmetro.</typeparam>
    /// <typeparam name="P4">O tipo do quarto parâmetro.</typeparam>
    public interface IFactory<out T, in P1, in P2, in P3, in P4>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <param name="item1">O primeiro parâmetro de inicialização.</param>
        /// <param name="item2">O segundo parâmetro de inicialização.</param>
        /// <param name="item3">O terceiro parâmetro de inicialização.</param>
        /// <param name="item4">O quarto parâmetro de inicialização.</param>
        /// <returns>O objecto criado.</returns>
        T Create(P1 item1, P2 item2, P3 item3, P4 item4);
    }

    /// <summary>
    /// Permite criar um objecto com quatro parâmetros de inicialização.
    /// </summary>
    /// <typeparam name="T">O tipo do objecto a ser criado.</typeparam>
    /// <typeparam name="P1">O tipo do primeiro parâmetro.</typeparam>
    /// <typeparam name="P2">O tipo do segundo parâmetro.</typeparam>
    /// <typeparam name="P3">O tipo do terceiro parâmetro.</typeparam>
    /// <typeparam name="P4">O tipo do quarto parâmetro.</typeparam>
    /// <typeparam name="P5">O tipo do quinto parâmetro.</typeparam>
    public interface IFactory<out T, in P1, in P2, in P3, in P4, in P5>
    {
        /// <summary>
        /// Cria um objecto.
        /// </summary>
        /// <param name="item1">O primeiro parâmetro de inicialização.</param>
        /// <param name="item2">O segundo parâmetro de inicialização.</param>
        /// <param name="item3">O terceiro parâmetro de inicialização.</param>
        /// <param name="item4">O quarto parâmetro de inicialização.</param>
        /// <param name="item5">O quinto parâmetro de inicialização.</param>
        /// <returns>O objecto criado.</returns>
        T Create(P1 item1, P2 item2, P3 item3, P4 item4, P5 item5);
    }
}
