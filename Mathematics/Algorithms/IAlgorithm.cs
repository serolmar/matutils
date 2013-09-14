using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType">Recebe os dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType, out OutputType>
    {
        OutputType Run(InputType data);
    }

    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType">Recebe os dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType1, in InputType2, out OutputType>
    {
        OutputType Run(InputType1 first, InputType2 second);
    }

    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType">Recebe os dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType1, in InputType2, in InputType3, out OutputType>
    {
        OutputType Run(InputType1 first, InputType2 second, InputType3 third);
    }

    /// <summary>
    /// Esta interface representa um algoritmo arbitrário.
    /// </summary>
    /// <typeparam name="InputType">Recebe os dados de entrada.</typeparam>
    /// <typeparam name="OutputType">Recebe os dados de saída.</typeparam>
    public interface IAlgorithm<in InputType1, in InputType2, in InputType3, in InpuType4, out OutputType>
    {
        OutputType Run(InputType1 first, InputType2 second, InputType3 third, InpuType4 fourth);
    }
}
