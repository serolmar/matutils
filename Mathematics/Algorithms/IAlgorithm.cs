using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics.Algorithms
{
    public interface IAlgorithm
    {
        /// <summary>
        /// Runs some algorithm.
        /// </summary>
        /// <remarks>
        /// Both the input and output are transacted in the object passed as an argument.
        /// </remarks>
        /// <param name="inputOutput">The input and output object.</param>
        void Run(IAlgorithmContract inputOutput);
    }
}
