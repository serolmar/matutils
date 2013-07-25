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
}
