namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Define uma região de fusão de células em estruturas de dados tabulares.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que definem as coordenadas.</typeparam>
    public interface IMergingRegion<T>
    {
        /// <summary>
        /// Obtém o comparador associado às coordenadas.
        /// </summary>
        IComparer<T> Comparer { get; }

        /// <summary>
        /// Obtém ou atribui o valor da coordenada x do canto superior esquerdo.
        /// </summary>
        T TopLeftX { get; set; }

        /// <summary>
        /// Obtém ou atribui o valor da coordenda y do canto superior esquerdo.
        /// </summary>
        T TopLeftY { get; set; }

        /// <summary>
        /// Obtém ou atribui a coordenada x do canto inferior direito.
        /// </summary>
        T BottomRightX { get; set; }

        /// <summary>
        /// Obtém ou atribui a coordenada y do canto inferior direito.
        /// </summary>
        T BottomRightY { get; set; }
    }

    /// <summary>
    /// Define uma região rectangular.
    /// </summary>
    /// <typeparam name="T">O tipos dos objectos que constituem as regiões rectangulares.</typeparam>
    public interface IRectangularRegion<T> : IMergingRegion<T>
    {
        /// <summary>
        /// Verifica se existe sobreposição das regiões rectangulares.
        /// </summary>
        /// <param name="other">A outra região rectangular.</param>
        /// <returns>Verdadeiro caso se dê sobreposição e falso caso contrário.</returns>
        bool OverLaps(IMergingRegion<T> other);

        /// <summary>
        /// Obtém a intersecção da região rectangular actual com a região rectangular proporcionada.
        /// </summary>
        /// <remarks>
        /// A função retorna um rectângulo sem área sempre que exista uma fronteira
        /// em comum. Se não se verificar sobreposição, a função irá retornar um nulo.
        /// </remarks>
        /// <param name="other">A região rectangular a ser intersectada.</param>
        /// <returns>O resultado da intersecção.</returns>
        IRectangularRegion<T> Intersect(IMergingRegion<T> other);

        /// <summary>
        /// Obtém uma lista de regiões rectangulares que resultam da subtracção
        /// da região proporcionada à região actual.
        /// </summary>
        /// <param name="other">A região a subtrair.</param>
        /// <param name="increment">A função que permite determinar o incremento.</param>
        /// <param name="decrement">A função que permite determinar o decremento.</param>
        /// <returns>
        /// O conjunto de regiões que cobrem a região actual com excepção da região definida.
        /// </returns>
        List<IRectangularRegion<T>> Subtract(
            IMergingRegion<T> other,
            Func<T, T> increment,
            Func<T, T> decrement);

        /// <summary>
        /// Funde duas regiões rectangulares quando tal é possível.
        /// </summary>
        /// <param name="other">A região rectangular a ser fundida.</param>
        /// <param name="increment">A função que permite determinar o incremento.</param>
        /// <param name="decrement">A função que permite determinar o decremento.</param>
        /// <returns>O resultado da fusão de duas regiões rectangulares.</returns>
        IRectangularRegion<T> Merge(
            IMergingRegion<T> other,
            Func<T, T> increment,
            Func<T, T> decrement);
    }
}
