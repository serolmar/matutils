namespace Utilities.Lambda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// Permite construir uma expressão lambda com base num filtro escrito numa linguagem
    /// próxima do natural.
    /// </summary>
    public class SmartFilterLambdaBuilder
    {
        /// <summary>
        /// Costrói uma expressão que actua sobre o objecto.
        /// </summary>
        /// <typeparam name="ObjectType">O tipo de objecto.</typeparam>
        /// <param name="pattern">O filtro de pesquisa.</param>
        /// <returns>A expressão resultante.</returns>
        public Expression<Func<ObjectType, bool>> BuildExpressionTree<ObjectType>(string pattern)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constrói uma expressão que actua sobre uma propriedade pré-especificada do objecto.
        /// </summary>
        /// <typeparam name="ObjectType">O tipo de objecto.</typeparam>
        /// <typeparam name="PropertyType">O tipo de propriedade.</typeparam>
        /// <param name="selector">O selector da propriedade do objecto.</param>
        /// <param name="pattern">O filtro de pesquisa.</param>
        /// <returns>A expressão resultante.</returns>
        public Expression<Func<ObjectType, bool>> BuildExpressionTree<ObjectType, PropertyType>(
            Expression<Func<ObjectType, PropertyType>> selector, 
            string pattern)
        {
            throw new NotImplementedException();
        }
    }
}
