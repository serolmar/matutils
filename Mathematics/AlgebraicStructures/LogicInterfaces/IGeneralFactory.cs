using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Implementa uma fábrica que permite criar o objecto do tipo especificado sem passar argumentos.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objecto.</typeparam>
    public interface IGeneralFactory<out ObjectType>
    {
        /// <summary>
        /// Cria um objecto do tipo especificado.
        /// </summary>
        /// <returns>O objecto.</returns>
        ObjectType Create();
    }

    /// <summary>
    /// Implementa uma fábrica que permite criar um objecto do tipo especificado passando um argumento apenas.
    /// </summary>
    /// <typeparam name="ArgType">O tipo do argumento.</typeparam>
    /// <typeparam name="ObjectType">O tipo do objecto a ser criado.</typeparam>
    public interface IGeneralFactory<in ArgType, out ObjectType>
    {
        /// <summary>
        /// Cria um objecto do tipo especificado.
        /// </summary>
        /// <param name="argument">O argumento.</param>
        /// <returns>O objecto.</returns>
        ObjectType Create(ArgType argument);
    }

    /// <summary>
    /// Implementa uma fábrica que permite criar um objecto do tipo especificado passando dois agumentos.
    /// </summary>
    /// <typeparam name="FirstArgType">O primeiro argumento.</typeparam>
    /// <typeparam name="SecondArgType">O segundo argumento.</typeparam>
    /// <typeparam name="ObjectType">O tipo do objecto.</typeparam>
    public interface IGeneralFactory<in FirstArgType, in SecondArgType, out ObjectType>
    {
        /// <summary>
        /// Cria um objecto do tipo especificado.
        /// </summary>
        /// <param name="firstArgument">O primeiro argumento.</param>
        /// <param name="secondArgument">O segundo argumento.</param>
        /// <returns>O objecto.</returns>
        ObjectType Create(FirstArgType firstArgument, SecondArgType secondArgument);
    }

    /// <summary>
    /// Implementa uma fábrica que permite criar um objecto do tipo especificado passando três argumentos.
    /// </summary>
    /// <typeparam name="FirstArgType">O tipo do primeiro argumento.</typeparam>
    /// <typeparam name="SecondArgType">O tipo do segundo argumento.</typeparam>
    /// <typeparam name="ThirdArgType">O tipo do terceiro argumento.</typeparam>
    /// <typeparam name="ObjectType">O tipo do objecto a ser criado.</typeparam>
    public interface IGeneralFactory<in FirstArgType, in SecondArgType, in ThirdArgType, out ObjectType>
    {
        /// <summary>
        /// Cria um objecto do tipo especificado.
        /// </summary>
        /// <param name="firstArgument">O primeiro argumento.</param>
        /// <param name="secondArgument">O segundo argumento.</param>
        /// <param name="thridArgument">O terceiro argumento.</param>
        /// <returns>O objecto.</returns>
        ObjectType Create(FirstArgType firstArgument, SecondArgType secondArgument, ThirdArgType thridArgument);
    }
}
